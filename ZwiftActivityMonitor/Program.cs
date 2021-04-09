using System;
using System.IO;
using System.Threading.Tasks;
using Dapplo.Microsoft.Extensions.Hosting.AppServices;
using Dapplo.Microsoft.Extensions.Hosting.Plugins;
using Dapplo.Microsoft.Extensions.Hosting.WinForms;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace ZwiftActivityMonitor
{
    public static class Program
    {
        private const string AppSettingsFilePrefix = "appsettings";
        private const string HostSettingsFile = "hostsettings.json";
        private const string Prefix = "PREFIX_";

        public static Task Main(string[] args)
        {
            var executableLocation = Path.GetDirectoryName(typeof(Program).Assembly.Location);

            var host = new HostBuilder()
                .ConfigureWinForms<MainForm>()
                //.ConfigureWinForms<MonitorStatistics>()
                .ConfigureConfiguration(args)
                .ConfigureLogging()
                .ConfigureSingleInstance(builder =>
                {
                    builder.MutexId = "{80B16FA8-ECAE-4DD8-9F8A-FE7E6780A825}";
                    builder.WhenNotFirstInstance = (hostingEnvironment, logger) =>
                    {
                        // This is called when an instance was already started, this is in the second instance
                        logger.LogWarning("Application {0} already running.", hostingEnvironment.ApplicationName);
                    };
                })
                //.ConfigurePlugins(pluginBuilder =>
                //{
                //    if (executableLocation == null)
                //    {
                //        return;
                //    }

                //    var runtime = Path.GetFileName(executableLocation);
                //    var parentDirectory = Directory.GetParent(executableLocation).FullName;
                //    var configuration = Path.GetFileName(parentDirectory);
                //    var basePath = Path.Combine(executableLocation, @"..\..\..\..\");
                //    // Specify the location from where the Dll's are "globbed"
                //    pluginBuilder.AddScanDirectories(basePath);
                //    // Add the framework libraries which can be found with the specified globs
                //    pluginBuilder.IncludeFrameworks(@$"**\bin\{configuration}\netstandard2.0\*.FrameworkLib.dll");
                //    // Add the plugins which can be found with the specified globs
                //    pluginBuilder.IncludePlugins(@$"**\bin\{configuration}\{runtime}\*.Sample.Plugin*.dll");
                //})
                .ConfigureServices(serviceCollection =>
                {
                    // Add the ZwiftPacketMonitor extensions
                    ZwiftPacketMonitor.RegistrationExtensions.AddZwiftPacketMonitoring(serviceCollection);

                    // add our ZwiftPacketMonitor wrapper service
                    serviceCollection.AddSingleton<ZPMonitorService>();

                    serviceCollection.AddTransient<AdvancedOptions>();
                    serviceCollection.AddTransient<ConfigurationOptions>();
                    serviceCollection.AddSingleton<MonitorTimer>();
                })
                .UseWinFormsLifetime()
                .Build();

            ILoggerFactory lf = host.Services.GetRequiredService<ILoggerFactory>();
            ZPMonitorService zp = host.Services.GetRequiredService<ZPMonitorService>();
            ZAMsettings.Initialize(lf, zp);


            return host.RunAsync();
        }

        /// <summary>
        /// Configure the loggers
        /// </summary>
        /// <param name="hostBuilder">IHostBuilder</param>
        /// <returns>IHostBuilder</returns>
        private static IHostBuilder ConfigureLogging(this IHostBuilder hostBuilder)
        {
            return hostBuilder.ConfigureLogging((hostContext, configLogging) =>
            {
                configLogging
                    .AddConfiguration(hostContext.Configuration.GetSection("Logging"))
                    .AddConsole()
                    .AddDebug();
            });
        }

        /// <summary>
        /// Configure the configuration
        /// </summary>
        /// <param name="hostBuilder"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        private static IHostBuilder ConfigureConfiguration(this IHostBuilder hostBuilder, string[] args)
        {
            return hostBuilder.ConfigureHostConfiguration(configHost =>
            {
                configHost
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile(HostSettingsFile, optional: true)
                    .AddEnvironmentVariables(prefix: Prefix)
                    .AddCommandLine(args);
                    
            })
                .ConfigureAppConfiguration((hostContext, configApp) =>
                {
                    configApp
                        .AddJsonFile(AppSettingsFilePrefix + ".json", optional: true);
                        //.AddEnvironmentVariables(prefix: Prefix)
                        //.AddCommandLine(args);
                    if (!string.IsNullOrEmpty(hostContext.HostingEnvironment.EnvironmentName))
                    {
                        configApp.AddJsonFile(AppSettingsFilePrefix + $".{hostContext.HostingEnvironment.EnvironmentName}.json", optional: true);
                    }
                    configApp
                        .AddEnvironmentVariables(prefix: Prefix)
                        .AddCommandLine(args);
                        
                });
        }
    }
}
