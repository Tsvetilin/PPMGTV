using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.AzureKeyVault;

using System;

using static Common.Constants.DetailsConstants;

namespace Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration((context, config) =>
                {
                    var env = Environment.GetEnvironmentVariable(AspNetEnviramentVariableName) ?? ProductionEnvironmentName;
                    if (env == ProductionEnvironmentName)
                    {
                        var keyVaultEndpoint = new Uri(Environment.GetEnvironmentVariable(VaultUri));
                        config.AddAzureKeyVault(
                        keyVaultEndpoint.ToString(),
                        new DefaultKeyVaultSecretManager());
                    }
                })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
