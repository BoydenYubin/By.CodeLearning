using IdentityServer4.SSO.WebUI.Configuration;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Events;
using Serilog.Extensions.Logging;
using Serilog.Sinks.SystemConsole.Themes;
using System;
using System.Threading.Tasks;

namespace IdentityServer4.SSO.WebUI
{
    public class Program
    {
        static readonly LoggerProviderCollection Providers = new LoggerProviderCollection();
        public static async Task Main(string[] args)
        {
            Console.Title = "ByLearning-IdentityServer4-SSO-WebUI";
            //Step 1: config the serilog
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                .MinimumLevel.Override("System", LogEventLevel.Warning)
                .MinimumLevel.Override("Microsoft.AspNetCore.Authentication", LogEventLevel.Information)
                .Enrich.FromLogContext()
                .WriteTo.Providers(Providers)
                .WriteTo.File("ByLearning_SSO_log-.txt", rollingInterval: RollingInterval.Day, fileSizeLimitBytes: 1073741824 / 8)
                .WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss} {Level}] {SourceContext}{NewLine}{Message:lj}{NewLine}{Exception}{NewLine}", theme: AnsiConsoleTheme.Literate)
                //.WriteTo.Seq()  need to add,use the struscured data
                .CreateLogger();
            var host = CreateHostBuilder(args).Build();

            //Seed Data part
            Task.WaitAll(DbMigrationHelpers.EnsureSeedData(serviceScope: host.Services.CreateScope()));

            await host.RunAsync();
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder(args)
                       .UseSerilog((context, config) =>
                       {
                            #if DEBUG
                           config.MinimumLevel.Debug()
                            .MinimumLevel.Override("Microsoft.EntityFrameworkCore", LogEventLevel.Warning)
                            .MinimumLevel.Override("Microsoft.AspNetCore.Mvc",LogEventLevel.Warning)
                            .MinimumLevel.Override("Microsoft.AspNetCore.Routing", LogEventLevel.Warning)
                            .MinimumLevel.Override("Microsoft.AspNetCore.StaticFiles", LogEventLevel.Warning)
                            .MinimumLevel.Override("System", LogEventLevel.Warning)
                            .Enrich.FromLogContext()
                            .WriteTo.File("ByLearning_SSO_Core-.txt", outputTemplate: "[{Timestamp:HH:mm:ss} {Level}] {SourceContext}{NewLine}{Message:lj}{NewLine}",
                           rollingInterval: RollingInterval.Day, fileSizeLimitBytes: 1073741824 / 8);
                            #endif
                       }, preserveStaticLogger: true)
                       .ConfigureWebHostDefaults(webBuilder =>
                       {
                           webBuilder.UseStartup<Startup>();
                       });
        }
    }
}
