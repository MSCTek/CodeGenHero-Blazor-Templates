using ArtistSite.App.Shared;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections;
using System.Linq;
using System.Text;

namespace ArtistSite.Client.Pages
{
    public class TestConfigViewModel : CGHComponentBase
    {
        public string ConfigurationDebugView
        {
            get
            {
                var root = (IConfigurationRoot)Configuration;
                var debugView = root.GetDebugView();

                return debugView;
            }
        }

        public string ConfigurationProviders
        {
            get
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendLine("ConfigurationProviders: ");

                var key = "CGHAppSettings:ApiBaseAddress";
                var configRoot = (IConfigurationRoot)Configuration;
                var configProviders = configRoot.Providers.ToList();
                for (var i = configProviders.Count - 1; i >= 0; i--)
                {
                    var configProvider = configProviders[i];
                    sb.AppendLine($"configProvider: {configProvider.ToString()}");

                    if (configProvider.TryGet(key, out var value))
                    {
                        sb.AppendLine($"{key}: {value}");
                    }
                }

                return sb.ToString();
            }
        }

        public string EnvironmentVariablesString
        {
            get
            {
                StringBuilder sb = new StringBuilder();

                sb.AppendLine("GetEnvironmentVariables: ");
                string aspnetcoreEnvironment = HostEnvironment.Environment; // Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
                sb.AppendLine($"ASPNETCORE_ENVIRONMENT: {aspnetcoreEnvironment}");
                sb.AppendLine($"HostEnvironment.IsDevelopment: {HostEnvironment.IsDevelopment()}");
                sb.AppendLine($"HostEnvironment.IsProduction: {HostEnvironment.IsProduction()}");

                foreach (DictionaryEntry de in Environment.GetEnvironmentVariables())
                {
                    sb.AppendLine(string.Format("  {0} = {1}", de.Key, de.Value));
                }

                return sb.ToString();
            }
        }

        [Inject]
        public IWebAssemblyHostEnvironment HostEnvironment { get; set; }

        public string OidcConfigurationString
        {
            get
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendLine("OidcConfiguration: ");

                var oidcConfigSection = Configuration.GetSection("OidcConfiguration");
                // services.Configure<OidcConfigSection>(oidcConfigSection);
                var itemArray = oidcConfigSection.AsEnumerable();
                foreach (var item in itemArray)
                {
                    sb.AppendLine($"{item.Key}: {item.Value}");
                }

                return sb.ToString();
            }
        }

        public string CGHAppSettingsString
        {
            get
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendLine("CGHAppSettings: ");

                var cghAppSettingsConfigSection = Configuration.GetSection("CGHAppSettings");
                var itemArray = cghAppSettingsConfigSection.AsEnumerable();
                foreach (var item in itemArray)
                {
                    sb.AppendLine($"{item.Key}: {item.Value}");
                }

                return sb.ToString();
            }
        }
    }
}