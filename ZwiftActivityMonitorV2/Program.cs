using System;
using System.IO;
using System.Threading.Tasks;
using Dapplo.Microsoft.Extensions.Hosting.AppServices;
using Dapplo.Microsoft.Extensions.Hosting.WinForms;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;



namespace ZwiftActivityMonitorV2
{
    //static class Program
    //{
    //    /// <summary>
    //    /// The main entry point for the application.
    //    /// </summary>
    //    [STAThread]
    //    static void Main()
    //    {
    //        Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("NDUzNDQzQDMxMzkyZTMxMmUzMEF2Ymt3ckxSUkpCUU8yOHE4dldsYVVxRHBpVlNYNGhtcjdjVHI1MHd5WmM9");

    //        Application.EnableVisualStyles();
    //        Application.SetCompatibleTextRenderingDefault(false);
    //        Application.Run(new MainForm());
    //    }
    //}

    public static class Program
    {
        private const string AppSettingsFilePrefix = "appsettings";
        private const string HostSettingsFile = "hostsettings.json";
        private const string Prefix = "PREFIX_";

        public static Task Main(string[] args)
        {

            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("NDUzNDQzQDMxMzkyZTMxMmUzMEF2Ymt3ckxSUkpCUU8yOHE4dldsYVVxRHBpVlNYNGhtcjdjVHI1MHd5WmM9");

            System.Windows.Forms.Application.EnableVisualStyles();

            System.Windows.Forms.Application.SetCompatibleTextRenderingDefault(false);

            var executableLocation = Path.GetDirectoryName(typeof(Program).Assembly.Location);

            var host = new HostBuilder()
                .ConfigureWinForms<MainForm>()
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
                .ConfigureServices(serviceCollection =>
                {
                    // Add the ZwiftPacketMonitor extensions
                    ZwiftPacketMonitor.RegistrationExtensions.AddZwiftPacketMonitoring(serviceCollection);

                    // add our ZwiftPacketMonitor wrapper service
                    serviceCollection.AddSingleton<ZPMonitorService>();

                    //serviceCollection.AddTransient<AdvancedOptions>();
                    //serviceCollection.AddTransient<ConfigurationOptions>();
                    //serviceCollection.AddTransient<MonitorTimer>();
                })
                .UseWinFormsLifetime()
                .Build();


            ILoggerFactory lf = host.Services.GetRequiredService<ILoggerFactory>();
            ZPMonitorService zp = host.Services.GetRequiredService<ZPMonitorService>();
            ZAMsettings.Initialize(lf, zp);

            // Uncomment to test logging setup in json configuration
            //ILogger logger = lf.CreateLogger("Testing Logger");

            //logger.LogTrace("This is a trace message. Should be discarded.");
            //logger.LogDebug("This is a debug message. Should be seen in development.");
            //logger.LogInformation("This is an info message. Should be seen in development.");
            //logger.LogWarning("This is a warning message. Should be seen everywhere.");
            //logger.LogError("This is an error message. Should be seen everywhere..");
            //logger.LogCritical("This is a critical message. Should be seen everywhere.");

            //try
            //{
            //    throw new ApplicationException("Some Exception");
            //}
            //catch (Exception ex)
            //{
            //    logger.LogError(ex, $"This is a test of exception logging logic.");
            //}


            return host.RunAsync();
        }

        /// <summary>
        /// Configure the loggers
        /// </summary>
        /// <param name="hostBuilder">IHostBuilder</param>
        /// <returns>IHostBuilder</returns>
        private static IHostBuilder ConfigureLogging(this IHostBuilder hostBuilder)
        {
            string executableLocation = Path.GetDirectoryName(typeof(Program).Assembly.Location);
            // The location Path.GetTempPath() could be used but then the log file might be more difficult to find 

            return hostBuilder.ConfigureLogging((hostContext, configLogging) =>
            {
                configLogging
                    .AddConfiguration(hostContext.Configuration.GetSection("Logging"))
                    .AddConsole()
                    .AddFile(o => o.RootPath = executableLocation) // Utilize Karambolo.Extensions.Logging.File from https://github.com/adams85/filelogger
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
