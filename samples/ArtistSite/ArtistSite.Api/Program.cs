using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Serilog;
using System;
using System.IO;
using System.Net;
using System.Reflection;

namespace ArtistSite.Api
{
    public class Program
    {
        public static readonly string AppName;
        public static readonly string Namespace;
        private static IConfiguration _configuration;

        static Program()
        {
            Namespace = typeof(Startup).Namespace;
            AppName = Namespace.Substring(Namespace.LastIndexOf('.', Namespace.LastIndexOf('.') - 1) + 1);
        }

        public static void Main(string[] args)
        {
            _configuration = GetConfiguration(args);
            Log.Logger = CreateSerilogLogger(_configuration);

            try
            {
                Log.Information("Configuring web host ({ApplicationContext})...", Program.AppName);
                //CreateHostBuilder(args).Build().Run();
                var host = BuildWebHost(_configuration, args);

                Log.Information("Starting web host ({ApplicationContext})...", Program.AppName);
                host.Run();

                //return 0;
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Program terminated unexpectedly ({ApplicationContext})!", Program.AppName);
                //return 1;
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        private static IWebHost BuildWebHost(IConfiguration configuration, string[] args)
        {
            IWebHost retVal = WebHost.CreateDefaultBuilder(args)
                .CaptureStartupErrors(false)
                .ConfigureKestrel(options =>
                {
                    var ports = GetDefinedPorts(configuration);
                    options.Listen(IPAddress.Any, ports.httpPort, listenOptions =>
                    {
                        listenOptions.Protocols = HttpProtocols.Http1AndHttp2;
                        listenOptions.UseHttps();
                    });

                    options.Listen(IPAddress.Any, ports.grpcPort, listenOptions =>
                    {
                        listenOptions.Protocols = HttpProtocols.Http2;
                        listenOptions.UseHttps();
                    });
                })
                .ConfigureAppConfiguration(x => x.AddConfiguration(configuration))
                //.UseFailing(options =>
                //{
                //    options.ConfigPath = "/Failing";
                //    options.NotFilteredPaths.AddRange(new[] { "/hc", "/liveness" });
                //})
                .UseStartup<Startup>()
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseSerilog()
                .Build();

            return retVal;
        }

        private static Serilog.ILogger CreateSerilogLogger(IConfiguration configuration)
        {
            //var seqServerUrl = configuration["Serilog:SeqServerUrl"];
            //var logstashUrl = configuration["Serilog:LogstashgUrl"];
            return new LoggerConfiguration()
                //.MinimumLevel.Verbose()
                //.Enrich.WithProperty("ApplicationContext", Program.AppName)
                //.Enrich.FromLogContext()
                //.WriteTo.Console()
                //.WriteTo.Seq(string.IsNullOrWhiteSpace(seqServerUrl) ? "http://seq" : seqServerUrl)
                //.WriteTo.Http(string.IsNullOrWhiteSpace(logstashUrl) ? "http://logstash:8080" : logstashUrl)
                .ReadFrom.Configuration(configuration)
                .CreateLogger();
        }

        private static IConfiguration GetConfiguration(string[] args)
        {
            IConfiguration retVal = null;

            // Mirrored from dotnet\runtime\src\libraries\Microsoft.Extensions.Hosting\src\Host.cs
            var hostBuilder = new HostBuilder();

            hostBuilder.UseContentRoot(Directory.GetCurrentDirectory());
            hostBuilder.ConfigureHostConfiguration(config =>
            {
                config.AddEnvironmentVariables(prefix: "DOTNET_");
                if (args != null)
                {
                    config.AddCommandLine(args);
                }
            });

            hostBuilder.ConfigureAppConfiguration((hostingContext, configBuilder) =>
            {
                IHostEnvironment env = hostingContext.HostingEnvironment;

                bool reloadOnChange = hostingContext.Configuration.GetValue("hostBuilder:reloadConfigOnChange", defaultValue: true);

                configBuilder.AddJsonFile("appsettings.json", optional: true, reloadOnChange: reloadOnChange)
                      .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true, reloadOnChange: reloadOnChange);

                if (env.IsDevelopment() && !string.IsNullOrEmpty(env.ApplicationName))
                {
                    var appAssembly = Assembly.Load(new AssemblyName(env.ApplicationName));
                    if (appAssembly != null)
                    {
                        configBuilder.AddUserSecrets(appAssembly, optional: true);
                    }
                }

                configBuilder.AddEnvironmentVariables();

                if (args != null)
                {
                    configBuilder.AddCommandLine(args);
                }

                var tempConfigValues = hostingContext.Configuration;
                // See if we should use the Azure Key Vault
                if (tempConfigValues.GetValue<bool>("UseVault", false))
                {
                    configBuilder.AddAzureKeyVault(
                    $"https://{tempConfigValues["Vault:Name"]}.vault.azure.net/",
                    tempConfigValues["Vault:ClientId"],
                    tempConfigValues["Vault:ClientSecret"]);
                }

                retVal = hostingContext.Configuration; // configBuilder.Build();
            });

            var tempHost = hostBuilder.Build();
            return retVal;

            /*** Original Code ***/
            //var builder = new ConfigurationBuilder()
            //    .SetBasePath(Directory.GetCurrentDirectory())
            //    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            //    .AddEnvironmentVariables();

            //var config = builder.Build();

            //if (config.GetValue<bool>("UseVault", false))
            //{
            //    builder.AddAzureKeyVault(
            //        $"https://{config["Vault:Name"]}.vault.azure.net/",
            //        config["Vault:ClientId"],
            //        config["Vault:ClientSecret"]);
            //}

            //return builder.Build();
        }

        private static (int httpPort, int grpcPort) GetDefinedPorts(IConfiguration config)
        {
            var port = config.GetValue("PORT", 5301);
            var grpcPort = config.GetValue("GRPC_PORT", 5302);
            Log.Information("GetDefinedPorts - port: {port}, grpcPort: {grpcPort}", port, grpcPort);

            return (port, grpcPort);
        }

        //public static IHostBuilder CreateHostBuilder(string[] args) =>
        //    Host.CreateDefaultBuilder(args)
        //        .ConfigureWebHostDefaults(webBuilder =>
        //        {
        //            webBuilder.UseStartup<Startup>();
        //        });
    }
}