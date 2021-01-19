using AspNetCore.SEOHelper;
using Common.Constants;
using Common.Helpers;

using Data;
using Data.Contracts.Repositories;
using Data.Models;
using Data.Repositories;

using Hangfire;
using Hangfire.Console;
using Hangfire.SqlServer;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.CookiePolicy;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

using Services.Contracts.Data;
using Services.Contracts.External;
using Services.Data;
using Services.External;
using Services.Mapping;

using System;
using System.Reflection;

using Web.Models;

namespace Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(this.Configuration.GetConnectionString("DefaultConnection")));

            services.AddIdentity<ApplicationUser, ApplicationRole>(
                options =>
                {
                    options.Lockout.AllowedForNewUsers = true;
                    options.Lockout.MaxFailedAccessAttempts = 15;
                    options.User.RequireUniqueEmail = true;
                    //options.SignIn.RequireConfirmedAccount = true;
                })
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            services.AddHangfire(config =>
                    config.
                    SetDataCompatibilityLevel(CompatibilityLevel.Version_170).
                    UseSimpleAssemblyNameTypeSerializer().
                    UseRecommendedSerializerSettings().
                    UseSqlServerStorage(
                        this.Configuration.GetConnectionString("HangfireConnection"),
                        new SqlServerStorageOptions
                        {
                            CommandBatchMaxTimeout = TimeSpan.FromMinutes(5),
                            SlidingInvisibilityTimeout = TimeSpan.FromMinutes(5),
                            QueuePollInterval = TimeSpan.Zero,
                            UseRecommendedIsolationLevel = true,
                            UsePageLocksOnDequeue = true,
                            DisableGlobalLocks = true,
                        }
                    ).
                    UseConsole());

            services.AddHangfireServer(options => options.WorkerCount = 2);
            services.AddRouting(options =>
            {
                options.LowercaseQueryStrings = true;
                options.LowercaseUrls = true;
                options.AppendTrailingSlash = false;
            });

            services.Configure<CookiePolicyOptions>(
                options =>
                {
                    options.HttpOnly = HttpOnlyPolicy.Always;
                    options.Secure = CookieSecurePolicy.Always;
                    options.CheckConsentNeeded = context => true;
                    options.MinimumSameSitePolicy = SameSiteMode.None;
                });

            services.AddResponseCaching();
            services.AddResponseCompression(options =>
                {
                    options.EnableForHttps = true;
                });

            services.AddControllersWithViews(
                options =>
                {
                    options.Filters.Add(new AutoValidateAntiforgeryTokenAttribute());
                });
            services.AddRazorPages();

            services.AddAntiforgery(options =>
                {
                    options.HeaderName = DetailsConstants.CSRFCookieHeaderName;
                });

            // Authentication
            services.AddAuthentication().
                AddFacebook(facebookOptions =>
                {
                    facebookOptions.AppId = Configuration["Authentication:Facebook:AppId"];
                    facebookOptions.AppSecret = Configuration["Authentication:Facebook:AppSecret"];
                }).
                AddGoogle(options =>
                {
                    var googleAuthSection = Configuration.GetSection("Authentication:Google");
                    options.ClientId = googleAuthSection["ClientId"];
                    options.ClientSecret = googleAuthSection["ClientSecret"];
                });

            // Configuration
            services.AddSingleton(this.Configuration);

            // Data repositories
            services.AddScoped(typeof(IDeletableEntityRepository<>), typeof(DeletableEntityRepository<>));
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

            // Application services
            services.AddTransient<ISendGridEmailSender>(x =>
            {
                var config = Configuration.GetSection("SendGridEmailSender");
                return new SendGridEmailSender(config["APIKey"]);
            });
            services.AddTransient<IEmailSender>(x =>
            {
                var config = Configuration.GetSection("SendGridEmailSender");
                return new SendGridEmailSender(config["APIKey"]);
            });
            services.AddTransient<ICloudinary>(x =>
            {
                var config = Configuration.GetSection("Cloudinary");
                return new Cloudinary(config["CloudName"], config["APIKey"], config["APISecret"]);
            });

            services.AddTransient<IVideosService, VideosService>();
            services.AddTransient<IContactsService, ContactsService>();
            services.AddTransient<IUsersService, UsersService>();
            services.AddTransient<ITeamService, TeamService>();
            services.AddTransient<ITeamMemberService, TeamMemberService>();
            services.AddTransient<IImageService, ImageService>();
            services.AddTransient<IGalleryService, GalleryService>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            AutoMapperConfig.RegisterMappings(typeof(ErrorViewModel).GetTypeInfo().Assembly);

            // Seed data on application startup
            using (var serviceScope = app.ApplicationServices.CreateScope())
            {
                var dbContext = serviceScope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                dbContext.Database.Migrate();
                new ApplicationDbContextSeeder()
                    .SeedAsync(dbContext,
                               serviceScope
                               .ServiceProvider
                               .GetService<ILoggerFactory>()
                               .CreateLogger(typeof(ApplicationDbContextSeeder)))
                    .GetAwaiter()
                    .GetResult();
            }

            // Generate Sitemap
            using (var serviceScope = app.ApplicationServices.CreateScope())
            {
                var videosService = serviceScope.ServiceProvider.GetRequiredService<IVideosService>();
                var galleryService = serviceScope.ServiceProvider.GetRequiredService<IGalleryService>();
                SitemapFactory.Create(env);
                videosService.AddVideosToSitemapAsync().GetAwaiter().GetResult();
                galleryService.AddGaleriesToSitemapAsync().GetAwaiter().GetResult();
                SitemapFactory.UpdateSitemap();
            }

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseStatusCodePagesWithRedirects("/Home/StatusError/{0}");

            app.Use(async (context, next) =>
            {
                var url = context.Request.Path.Value;
                if (url.Contains("sitemap"))
                {
                    context.Request.Path = "/sitemap.xml";
                }

                await next();
            });

            app.UseHttpsRedirection();
            app.UseCookiePolicy();

            app.UseResponseCompression();
            app.UseResponseCaching();

            app.UseStaticFiles();
            app.UseXMLSitemap(env.ContentRootPath);
            app.UseRobotsTxt(env.ContentRootPath);

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseHangfireServer(new BackgroundJobServerOptions { WorkerCount = 2 });
            app.UseHangfireDashboard(
                "/hangfire",
                new DashboardOptions
                {
                    Authorization = new[]
                    {
                            new HangfireAuthorizationFilter()
                    }
                });

            app.UseRewriter(
                new RewriteOptions().
                Add(new RedirectLowerCaseRule()).
                Add(new RedirectEndingSlashCaseRule())
                );

            app.UseEndpoints(
                endpoints =>
                {
                    endpoints.MapControllerRoute(
                        name: "areaRoute",
                        pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");
                    endpoints.MapControllerRoute(
                        name: "default",
                        pattern: "{controller=Home}/{action=Index}/{id?}/{slug?}");
                    endpoints.MapRazorPages();
                });
        }
    }
}
