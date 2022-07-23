using ArtistSite.App.MessageHandlers;
using ArtistSite.App.Services;
using ArtistSite.Client.Services;
using ArtistSite.Shared.Authorization;
using ArtistSite.Shared.DataService;
using ArtistSite.Shared.DTO;
using Majorsoft.Blazor.Components.GdprConsent;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor.Services;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace ArtistSite.Client
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<ArtistSite.Client.App>("#app");
            builder.RootComponents.Add<HeadOutlet>("head::after");

            builder.Services
                .AddTransient<CGHApiAuthorizationMessageHandler>();

            builder.Services.AddOidcAuthentication(options =>
            {
                builder.Configuration.Bind("OidcConfiguration", options.ProviderOptions);
                builder.Configuration.Bind("UserOptions", options.UserOptions);
            });

            builder.Services.AddAuthorizationCore(authorizationOptions =>
            {
                authorizationOptions.AddPolicy(
                    Policies.CanManageEmployees,
                    Policies.CanManageEmployeesPolicy());
            });

            builder.Services.AddScoped<ILocalHttpClientService, LocalHttpClientService>();
            builder.Services.AddMudServices();
            builder.Services.AddScoped<INavigationService, NavigationService>();
            builder.Services.AddGdprConsent(); // Cookie consent.

            var httpClient = new HttpClient()
            {
                BaseAddress = new Uri(builder.HostEnvironment.BaseAddress)
            };

            using var response = await httpClient.GetAsync("appsettings.json");
            using var stream = await response.Content.ReadAsStreamAsync();
            builder.Configuration.AddJsonStream(stream);

            var CGHAppSettingsSection = builder.Configuration.GetSection("CGHAppSettings");
            builder.Services.Configure<CGHAppSettings>(CGHAppSettingsSection);

            //builder.Services.AddHttpClient<IEmployeeDataService, EmployeeDataService>(
            //    client => client.BaseAddress = new Uri("https://localhost:44323/"))
            //    .AddHttpMessageHandler<CGHApiAuthorizationMessageHandler>();

            builder.Services.AddSingleton<ISerializationHelper, SerializationHelper>();

            var apiBaseAddress = CGHAppSettingsSection["ApiBaseAddress"];
            Console.WriteLine($"apiBaseAddress: {apiBaseAddress}");

            builder.Services.AddHttpClient("CGHApi", c =>
            {
                c.BaseAddress = new Uri(apiBaseAddress);
                c.DefaultRequestHeaders.Add("api-version", "1");
            });

            builder.Services.AddScoped<ISerializationHelper, SerializationHelper>();
            builder.Services.AddScoped<ITestAuthDataService, TestAuthDataService>();
            builder.Services.AddScoped<IImageUploadDataService, ImageUploadDataService>();
            /// SETUP - Add your Dependency Injection for generated Data Service classes here
            builder.Services.AddScoped<IWebApiDataServiceAS, WebApiDataServiceAS>();

            builder.Services.AddScoped<ITokenProvider, TokenProviderWasm>();
            builder.Services.AddScoped<ITokenManager, TokenManagerWasm>();

            await builder.Build().RunAsync();
        }
    }
}