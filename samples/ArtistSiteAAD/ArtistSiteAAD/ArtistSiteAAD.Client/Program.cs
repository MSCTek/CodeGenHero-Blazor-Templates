using ArtistSiteAAD.Client;
using ArtistSiteAAD.Client.Services;
using ArtistSiteAAD.Shared.Constants;
using ArtistSiteAAD.Shared.DataService;
using ArtistSiteAAD.Shared.DTO;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddHttpClient("ArtistSiteAAD.Blazor.ServerAPI", client => client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress))
    .AddHttpMessageHandler<BaseAddressAuthorizationMessageHandler>();

var cghAppSettingsSection = builder.Configuration.GetSection("CGHAppSettings");
builder.Services.Configure<CGHAppSettings>(cghAppSettingsSection);

var apiBaseAddress = cghAppSettingsSection["ApiBaseAddress"];
Console.WriteLine($"apiBaseAddress: {apiBaseAddress}");

builder.Services.AddScoped<CustomAuthorizationMessageHandler>();

builder.Services.AddHttpClient(Consts.HTTPCLIENTNAME_AUTHORIZED,
    client =>
    {
        client.BaseAddress = new Uri(apiBaseAddress);
        client.DefaultRequestHeaders.Add("api-version", "1");
    })
    .AddHttpMessageHandler<CustomAuthorizationMessageHandler>();

builder.Services.AddHttpClient(Consts.HTTPCLIENTNAME_ANONYMOUS,
    client =>
    {
        client.BaseAddress = new Uri(apiBaseAddress);
        client.DefaultRequestHeaders.Add("api-version", "1");
    });

// Supply HttpClient instances that include access tokens when making requests to the server project
builder.Services.AddScoped(sp => sp.GetRequiredService<IHttpClientFactory>().CreateClient("ArtistSiteAAD.Blazor.ServerAPI"));

builder.Services.AddMsalAuthentication(options =>
{
    builder.Configuration.Bind("AzureAd", options.ProviderOptions.Authentication);
    /*
     WARNING: enabling the line below, as instructed in Microsoft documentation, will result in browser errors that include text such as:
        Failed to load resource: the server responded with a status of 400 (Bad Request)
        https://login.microsoftonline.com/c94e4b8d-ba95-4de2-9da8-1947f842b07f/oauth2/v2.0/token
        authentication login-failed message AADSTS28000 Provided value for the input parameter scope is not valid because it contains more than one resource. Scope
        api://5a48213f-571a-492f-ba42-35501f0eb07a/API.Access
        api://115e7c72-4ab0-4195-b563-667a805c58d7/AllAccess
     */
    //options.ProviderOptions.DefaultAccessTokenScopes.Add("api://5a48213f-571a-492f-ba42-35501f0eb07a/API.Access");
    options.ProviderOptions.LoginMode = "popup"; // redirect
})
    .AddAccountClaimsPrincipalFactory<RemoteAuthenticationState,
    RemoteUserAccount, CustomAccountFactory>();

builder.Services.AddOptions();

var aadAppSettingsSection = builder.Configuration.GetSection(Consts.AAD_GROUP_CONFIGURATION);
builder.Services.Configure<AADGroupConfiguration>(aadAppSettingsSection);

AADGroupConfiguration aadGroupConfiguration = new AADGroupConfiguration();
builder.Configuration.Bind(Consts.AAD_GROUP_CONFIGURATION, aadGroupConfiguration);

builder.Services.AddAuthorizationCore(configure =>
{
    configure.AddPolicy(Consts.ACCESS_ADMIN, policy => policy.RequireClaim("groups", aadGroupConfiguration.AdminGroupIdsArray));
    configure.AddPolicy(Consts.ACCESS_USER, policy => policy.RequireClaim("groups", aadGroupConfiguration.AuthorizedUserGroupIdsArray));
});

builder.Services.AddScoped<ILocalHttpClientService, LocalHttpClientService>();

builder.Services.AddMudServices();

builder.Services.AddScoped<ISerializationHelper, SerializationHelper>();

builder.Services.AddScoped<INavigationService, NavigationService>();

/// Setup - Add dependency injection for your generated WebApiDataService
builder.Services.AddScoped<IWebApiDataServiceAS, WebApiDataServiceAS>();

await builder.Build().RunAsync();
