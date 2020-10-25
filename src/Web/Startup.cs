using Common.Constants;

using Data;
using Data.Contracts.Repositories;
using Data.Models;
using Data.Repositories;

using Hangfire;
using Hangfire.Console;
using Hangfire.Dashboard;
using Hangfire.SqlServer;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

using Services.Contracts.Data;
using Services.Contracts.External;
using Services.CronJobs;
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
                    options.SignIn.RequireConfirmedAccount = true;
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

            services.AddHangfireServer();

            services.Configure<CookiePolicyOptions>(
                options =>
                {
                    options.CheckConsentNeeded = context => true;
                    options.MinimumSameSitePolicy = SameSiteMode.None;
                });

            services.AddControllersWithViews(
                options =>
                {
                    options.Filters.Add(new AutoValidateAntiforgeryTokenAttribute());
                });
            services.AddRazorPages();

            // Authentication
            services.AddAuthentication().
                AddFacebook(facebookOptions =>
                {
                    facebookOptions.AppId = Configuration["Authentication:Facebook:AppId"];
                    facebookOptions.AppSecret = Configuration["Authentication:Facebook:AppSecret"];
                }).
                AddGoogle(options =>
                {
                    var googleAuthNSection = Configuration.GetSection("Authentication:Google");

                    options.ClientId = googleAuthNSection["ClientId"];
                    options.ClientSecret = googleAuthNSection["ClientSecret"];
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
                return new SendGridEmailSender(config["APIKey"], config["Sender"], config["SenderName"]);
            });
            services.AddTransient<IEmailSender>(x =>
            {
                var config = Configuration.GetSection("SendGridEmailSender");
                return new SendGridEmailSender(config["APIKey"], config["Sender"], config["SenderName"]);
            });
            services.AddTransient<ICloudinary>(x =>
            {
                var config = Configuration.GetSection("Cloudinary");
                return new Cloudinary(config["CloudName"], config["APIKey"], config["APISecret"]);
            });

            services.AddTransient<IVideosService, VideosService>();
            services.AddTransient<IContactsService, ContactsService>();
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

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            //if (env.IsProduction())
            {
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

                using var serviceScope = app.ApplicationServices.CreateScope();
                var recurringJobManager = serviceScope.ServiceProvider.GetRequiredService<IRecurringJobManager>();
                JobManager.SeedHangfireJobs(recurringJobManager);
            }

            app.UseEndpoints(
                endpoints =>
                {
                    endpoints.MapControllerRoute("areaRoute", "{area:exists}/{controller=Home}/{action=Index}/{id?}");
                    endpoints.MapControllerRoute("default", "{controller=Home}/{action=Index}/{id?}");
                    endpoints.MapRazorPages();
                    endpoints.MapAreaControllerRoute("identity", "Identity", "{area:exists}/{controller=Home}/{action=Index}/{id?}");
                });
        }

        private class HangfireAuthorizationFilter : IDashboardAuthorizationFilter
        {
            public bool Authorize(DashboardContext context)
            {
                var httpContext = context.GetHttpContext();
                return httpContext.User.IsInRole(ApplicationRolesNames.AdminRole);
            }
        }
    }
}
