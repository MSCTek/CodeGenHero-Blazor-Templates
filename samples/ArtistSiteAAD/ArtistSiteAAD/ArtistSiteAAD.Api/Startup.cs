using ArtistSiteAAD.Api.Authentication;
using ArtistSiteAAD.Api.Infrastructure;
using ArtistSiteAAD.Repository.Entities;
using ArtistSiteAAD.Repository.Mappers;
using ArtistSiteAAD.Repository.Repositories;
using ArtistSiteAAD.Shared.Authentication;
using ArtistSiteAAD.Shared.Constants;
using ArtistSiteAAD.Shared.DTO;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Web;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using Polly;
using Polly.Extensions.Http;

namespace ArtistSiteAAD.Api
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddHttpServices(this IServiceCollection services, string baseAddress)
        {
            return services;
        }

        private static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy()
        {
            return HttpPolicyExtensions
                .HandleTransientHttpError()
                .OrResult(msg => msg.StatusCode == System.Net.HttpStatusCode.NotFound)
                .WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));
        }
    }

    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            var requireAuthenticatedUserPolicy = new AuthorizationPolicyBuilder()
                .RequireAuthenticatedUser()
                .Build();

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddMicrosoftIdentityWebApi(Configuration.GetSection("AzureAd"));

            services.AddSingleton(Serilog.Log.Logger);
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddScoped<IUserSession, UserSession>();
            services.AddScoped<ICurrentUserService, CurrentUserService>();

            /// SETUP - Add your Data Context here after generating it from your Metadata
            services.AddDbContext<ArtistSiteDbContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"));

                if (System.Diagnostics.Debugger.IsAttached)
                {   // Write EF queries to the output console.
                    options.LogTo(Console.WriteLine);
                }
            });

            /// SETUP - Insert your generated Automapper profile here
            services.AddAutoMapper(typeof(ASAutoMapperProfile)); // Replace this with yours once it is generated. This is required for the API to run.

            /// SETUP - Add your generated Repository dependency-injection here
            services.AddScoped<IASRepository, ASRepository>();

            /// SETUP - Add your generated Generic Factory dependency-injection here
            services.AddScoped(typeof(IASGenericFactory<,>), typeof(ASGenericFactory<,>));

            services.AddCors(options =>
            {
                options.AddPolicy("Open", builder => builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
            });

            services.AddControllers(configure =>
            {
                configure.Filters.Add(new AuthorizeFilter(requireAuthenticatedUserPolicy)); // Add our AuthorizeFilter using the requireAuthenticatedUserPolicy defined above.
            }).AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            });

            services.Configure<RouteOptions>(options =>
            {
                options.ConstraintMap.Add("relatedEntitiesType", typeof(RelatedEntitiesTypeRouteConstraint));
            });

            services.AddAuthorization(authorizationOptions =>
            {
                AADGroupConfiguration aadGroupConfiguration = new AADGroupConfiguration();
                Configuration.Bind(Consts.AAD_GROUP_CONFIGURATION, aadGroupConfiguration);

                // Your Authorization Policies Here
                authorizationOptions.AddPolicy(Consts.ACCESS_ADMIN,
                    policy => policy.AddRequirements(new IsMemberOfGroupRequirement("CGHApp-Admin", aadGroupConfiguration.AdminGroupIdsArray)));

                authorizationOptions.AddPolicy(Consts.ACCESS_USER,
                    policy => policy.AddRequirements(new IsMemberOfGroupRequirement("CGHApp-User", aadGroupConfiguration.AuthorizedUserGroupIdsArray)));

            });

            services.AddSingleton<IAuthorizationHandler, IsMemberOfGroupHandler>();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "ArtistSiteAAD", Version = "v1" });
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Please insert JWT with Bearer into field",
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    new string[] { }
                } });
                c.OperationFilter<AddApiVersionToHeader>();
            });
        }
    }
}