using ArtistSite.App.Services;
using ArtistSite.Server.Services;
using ArtistSite.Shared.Constants;
using ArtistSite.Shared.DataService;
using ArtistSite.Shared.DTO;
using IdentityModel;
using Majorsoft.Blazor.Components.GdprConsent;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MudBlazor.Services;
using System;

namespace ArtistSite.Server
{
    public class Startup
    {
        public Startup(IWebHostEnvironment env, IConfiguration configuration)
        {
            WebHostEnvironment = env;
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        public IWebHostEnvironment WebHostEnvironment { get; }
        // IConfigurationRoot

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseRouteDebugger(); // See: https://edi.wang/post/2020/4/29/my-aspnet-core-route-debugger-middleware
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapBlazorHub();
                //endpoints.MapRazorPages();
                endpoints.MapFallbackToPage("/_Host");
            });
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            // this.WebHostEnvironment.ApplicationName = Assembly.GetEntryAssembly().GetName().Name;
            services.AddRazorPages();
            //services.AddRazorPages(options =>
            //{
            //    options.Conventions.AddPageRoute(
            //        "/LoginIDP", "LoginIDP2/{redirectUri?}");
            //});
            services.AddServerSideBlazor();
            services.AddScoped<ILocalHttpClientService, LocalHttpClientService>();
            services.AddMudServices();
            services.AddScoped<INavigationService, NavigationService>();
            services.AddGdprConsent(); // Cookie consent.

            var cghAppSettingsSection = Configuration.GetSection("CGHAppSettings");
            services.Configure<CGHAppSettings>(cghAppSettingsSection);

            var apiBaseAddress = cghAppSettingsSection[nameof(CGHAppSettings.ApiBaseAddress)];
            Console.WriteLine($"apiBaseAddress: {apiBaseAddress}");

            services.AddHttpClient("CGHApi", c =>
            {
                c.BaseAddress = new Uri(apiBaseAddress);
                c.DefaultRequestHeaders.Add("api-version", "1");
            });

            services.AddScoped<ISerializationHelper, SerializationHelper>();
            services.AddScoped<ITestAuthDataService, TestAuthDataService>();
            services.AddScoped<IImageUploadDataService, ImageUploadDataService>();
            /// SETUP - Add your dependency injection for generated Data Service classes here
            services.AddScoped<IWebApiDataServiceAS, WebApiDataServiceAS>();


            services.AddAuthentication(options =>
            {
                options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
            })
                .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options =>
                {
                    options.AccessDeniedPath = "/AccessDenied";
                })
                .AddOpenIdConnect(OpenIdConnectDefaults.AuthenticationScheme,
                  options =>
                  {
                      options.SignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                      options.Authority = Configuration["OidcConfiguration:Authority"];
                      options.ClientId = Configuration["OidcConfiguration:ClientId"];
                      options.ClientSecret = Configuration["OidcConfiguration:ClientSecret"];
                      options.ResponseType = "code";
                      options.Scope.Add("openid");
                      options.Scope.Add("profile");
                      options.Scope.Add("email");
                      options.Scope.Add("offline_access");
                      options.Scope.Add("CGHApi");

                      options.Scope.Add("country");
                      options.Scope.Add(Consts.CLAIM_USERID);
                      options.Scope.Add("roles");
                      // options.ClaimActions.DeleteClaim("address"); // Extension method via using Microsoft.AspNetCore.Authentication;
                      options.ClaimActions.MapUniqueJsonKey("role", "role");
                      options.ClaimActions.MapUniqueJsonKey(Consts.CLAIM_USERID, Consts.CLAIM_USERID);

                      //options.CallbackPath = ... -- default is /signin-oidc
                      options.SaveTokens = true;
                      options.GetClaimsFromUserInfoEndpoint = true; // make the additional call to the UserInfo endpoint to pick up profile claims.
                      options.TokenValidationParameters.NameClaimType = JwtClaimTypes.Email; // "email"; // given_name
                      options.TokenValidationParameters.RoleClaimType = JwtClaimTypes.Role; //
                      options.SignedOutCallbackPath = "/_Host"; // PostLogoutRedirectUri
                      //options.SignedOutRedirectUri = "/_Host";
                  });

            /*
            builder.Services.AddOidcAuthentication(options =>
            {
            builder.Configuration.Bind("OidcConfiguration", options.ProviderOptions);
            builder.Configuration.Bind("UserOptions", options.UserOptions);
            });
            */

            services.AddScoped<ITokenProvider, TokenProviderSrvr>();
            services.AddScoped<ITokenManager, TokenManagerSrvr>();
        }
    }
}