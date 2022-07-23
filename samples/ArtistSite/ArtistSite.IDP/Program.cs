// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using ArtistSite.IDP.Areas.Identity.Data;
using ArtistSite.IDP.Configuration;
using ArtistSite.IDP.Contexts;
using IdentityModel;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.SystemConsole.Themes;
using System;
using System.Linq;
using System.Security.Claims;

namespace ArtistSite.IDP
{
    public class Program
    {
        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseSerilog()
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });

        public static int Main(string[] args)
        {
            SetupLogger();

            try
            {
                Log.Information("Starting host...");
                var host = CreateHostBuilder(args).Build();
                SeedTheDatabase(host); // Best practice = in Main, using service scope

                host.Run();
                return 0;
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Host terminated unexpectedly.");
                return 1;
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        private static void ConfigureAppInsightsSink(
            IConfigurationRoot config,
            LoggerConfiguration loggerConfiguration)
        {
            var aiKey = config[AppSettingPathConstants.AppInsightsKey];
            if (!string.IsNullOrWhiteSpace(aiKey))
            {
                var telemetryConfiguration = TelemetryConfiguration.CreateDefault();
                telemetryConfiguration.InstrumentationKey = aiKey;
                loggerConfiguration.WriteTo.ApplicationInsights(
                    telemetryConfiguration, TelemetryConverter.Traces);
            }
        }

        private static void ConfigureLogStreamSink(
            IConfigurationRoot config,
            LoggerConfiguration loggerConfiguration)
        {
            loggerConfiguration.WriteTo.File(
                @"C:\home\LogFiles\Application\ArtistSite.IDP.log", // @"D:\home\LogFiles\Application\ArtistSite.IDP.log",
                fileSizeLimitBytes: 5_000_000,
                rollOnFileSizeLimit: true,
                shared: true,
                flushToDiskInterval: TimeSpan.FromSeconds(1),
                outputTemplate: "[{Timestamp:dd HH:mm:ss} {Level:u3}] {SourceContext}" +
                                "{NewLine}{Message:lj}" +
                                "{NewLine}{Exception}{NewLine}");
        }

        private static void SeedTheDatabase(IHost host)
        {
            using (var scope = host.Services.CreateScope())
            {
                try
                {
                    var context = scope.ServiceProvider.GetService<UserDbContext>();

                    // Ensure the DB is migrated before seeding
                    context.Database.Migrate();

                    // Use the user manager to create test users
                    var userManager = scope.ServiceProvider
                        .GetRequiredService<UserManager<ApplicationUser>>();

                    var adminUser = userManager.FindByNameAsync("myAdmin").Result;
                    if (adminUser == null)
                    {
                        adminUser = new ApplicationUser
                        {
                            UserName = "myAdmin",
                            EmailConfirmed = true
                        };

                        var result = userManager.CreateAsync(adminUser, "Bl@zor1").Result;
                        if (!result.Succeeded)
                        {
                            throw new Exception(result.Errors.First().Description);
                        }

                        result = userManager.AddClaimsAsync(adminUser, new Claim[]{
                                new Claim(JwtClaimTypes.Name, "Administrator"),
                                new Claim(JwtClaimTypes.GivenName, "Admin"),
                                new Claim(JwtClaimTypes.FamilyName, "User"),
                                new Claim(JwtClaimTypes.Email, "myAdmin@example.com"),
                                new Claim("country", "USA"),
                                new Claim("role", "ADMIN")
                            }).Result;

                        if (!result.Succeeded)
                        {
                            throw new Exception(result.Errors.First().Description);
                        }
                    }

                    var basicUser = userManager.FindByNameAsync("myUser").Result;
                    if (basicUser == null)
                    {
                        basicUser = new ApplicationUser
                        {
                            UserName = "myUser",
                            EmailConfirmed = true
                        };

                        var result = userManager.CreateAsync(basicUser, "Bl@zor2").Result;
                        if (!result.Succeeded)
                        {
                            throw new Exception(result.Errors.First().Description);
                        }

                        result = userManager.AddClaimsAsync(basicUser, new Claim[]{
                                new Claim(JwtClaimTypes.Name, "Basic User"),
                                new Claim(JwtClaimTypes.GivenName, "Basic"),
                                new Claim(JwtClaimTypes.FamilyName, "User"),
                                new Claim(JwtClaimTypes.Email, "myUser@example.com"),
                                new Claim("country", "USA"),
                                new Claim("role", "USER")
                            }).Result;

                        if (!result.Succeeded)
                        {
                            throw new Exception(result.Errors.First().Description);
                        }
                    }
                }
                catch (Exception ex)
                {
                    var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
                    logger.LogError(ex, "An error occurred while seeding the database.");
                }
            }
        }

        private static void SetupLogger()
        {
            var config = ConfigurationHelper
                .GetConfiguration("0F1064CC-A830-41AA-9216-FD57931D83B1");
            var loggerConfiguration = new LoggerConfiguration()
                .ReadFrom.Configuration(config)
                .MinimumLevel.Debug()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                .MinimumLevel.Override("Microsoft.Hosting.Lifetime", LogEventLevel.Information)
                .MinimumLevel.Override("System", LogEventLevel.Warning)
                .MinimumLevel.Override("Microsoft.AspNetCore.Authentication", LogEventLevel.Information);

            ConfigureAppInsightsSink(config, loggerConfiguration);
            ConfigureLogStreamSink(config, loggerConfiguration);
            Log.Logger = loggerConfiguration
                .Enrich.WithMachineName()
                .Enrich.FromLogContext()
                .WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss} {Level}] {SourceContext}{NewLine}{Message:lj}{NewLine}{Exception}{NewLine}", theme: AnsiConsoleTheme.Code)
                .CreateLogger();
        }
    }
}