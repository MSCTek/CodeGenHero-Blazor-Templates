using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using $safeprojectname$;
using MudBlazor.Services;
using $ext_safeprojectname$.Shared.DTO;
using $ext_safeprojectname$.Shared.DataService;
using $safeprojectname$.Services;
using $ext_safeprojectname$.Shared.Constants;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddHttpClient("$ext_safeprojectname$.Blazor.ServerAPI", client => client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress))
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
builder.Services.AddScoped(sp => sp.GetRequiredService<IHttpClientFactory>().CreateClient("$ext_safeprojectname$.Blazor.ServerAPI"));

builder.Services.AddMsalAuthentication(options =>
{
    builder.Configuration.Bind("AzureAd", options.ProviderOptions.Authentication);
    /*
     WARNING: enabling the line below, as instructed in Microsoft documentation, will result in browser errors that include text such as:
        Failed to load resource: the server responded with a status of 400 (Bad Request)
        https://login.microsoftonline.com/$AadApiTenantId$/oauth2/v2.0/token
        authentication login-failed message AADSTS28000 Provided value for the input parameter scope is not valid because it contains more than one resource. Scope
        api://$AadHostClientId$/API.Access
        api://$AadApiClientId$/AllAccess
     */
    //options.ProviderOptions.DefaultAccessTokenScopes.Add("api://$AadHostClientId$/API.Access");
    options.ProviderOptions.LoginMode = "popup"; // redirect
})
    .AddAccountClaimsPrincipalFactory<RemoteAuthenticationState,
    RemoteUserAccount, CustomAccountFactory>();

builder.Services.AddOptions();

var aadAppSettingsSection = builder.Configuration.GetSection(Consts.AAD_GROUP_CONFIGURATION);
builder.Services.Configure<AADGroupConfiguration>(aadAppSettingsSection);

AADGroupConfiguration aadGroupConfiguration = new AADGroupConfiguration();
builder.Configuration.Bind(Consts.AAD_GROUP_CONFIGURATION, aadGroupConfiguration);

builder.Services.AddAuthorizationCore(configure => {
    configure.AddPolicy(Consts.ACCESS_ADMIN, policy => policy.RequireClaim("groups", aadGroupConfiguration.AdminGroupIdsArray));
    configure.AddPolicy(Consts.ACCESS_USER, policy => policy.RequireClaim("groups", aadGroupConfiguration.AuthorizedUserGroupIdsArray));
});

builder.Services.AddScoped<ILocalHttpClientService, LocalHttpClientService>();

builder.Services.AddMudServices();

builder.Services.AddScoped<ISerializationHelper, SerializationHelper>();
builder.Services.AddScoped<INavigationService, NavigationService>();

/// Setup - Add dependency injection for your generated WebApiDataService
//builder.Services.AddScoped<IWebApiDataService, WebApiDataService>();

await builder.Build().RunAsync();
