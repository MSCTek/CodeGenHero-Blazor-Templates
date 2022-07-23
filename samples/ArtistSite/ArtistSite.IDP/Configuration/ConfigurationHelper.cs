using Microsoft.Extensions.Configuration;
using System;
using System.IO;

namespace ArtistSite.IDP.Configuration
{
    /// <summary>
    ///
    /// </summary>
    /// <see cref="https://arvehansen.net/codecave/2020/06/21/setup-and-deploy-identityserver4/#Logging"/>
    public static class ConfigurationHelper
    {
        public static IConfigurationRoot GetConfiguration(string secretsKey)
        {
            var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory());

            builder.AddJsonFile("appSettings.json", optional: false, reloadOnChange: true);

            if (!string.IsNullOrWhiteSpace(secretsKey))
                builder.AddUserSecrets(secretsKey);

            builder.AddEnvironmentVariables();

            return builder.Build();
        }
    }
}