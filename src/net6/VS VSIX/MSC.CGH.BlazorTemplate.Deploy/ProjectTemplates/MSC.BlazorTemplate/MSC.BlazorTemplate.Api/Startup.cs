namespace $safeprojectname$
{
    using AutoMapper;
    using IdentityServer4.AccessTokenValidation;
    using Microsoft.AspNetCore.Authentication.JwtBearer;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc.Authorization;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Identity.Web;
    using Microsoft.IdentityModel.Logging;
    using Microsoft.OpenApi.Models;
    using Newtonsoft.Json;
    using Polly;
    using Polly.Extensions.Http;
    using Serilog;
    using $safeprojectname$.Infrastructure;

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

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var requireAuthenticatedUserPolicy = new AuthorizationPolicyBuilder()
                .RequireAuthenticatedUser()
                .Build();

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddSingleton(Serilog.Log.Logger);

            /// SETUP - Add your Data Context here after generating it from your Metadata
            //services.AddDbContext<YourDataContext>(options =>
            //{
            //    options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"));

            //    if (System.Diagnostics.Debugger.IsAttached)
            //    {   // Write EF queries to the output console.
            //        options.LogTo(Console.WriteLine);
            //    }
            //});

            /// SETUP - Insert your generated Automapper profile here
            services.AddAutoMapper(typeof(Profile)); // Replace this with yours once it is generated. This is required for the API to run.

            /// SETUP - Add your generated Repository dependency-injection here
            // services.AddScoped<IYourEntityRepository, YourEntityRepository>(); - Example

            /// SETUP - Add your generated Generic Factory dependency-injection here
            // services.AddScoped(typeof(IGenericFactory<,>), typeof(GenericFactory<,>)); - Example

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
                // Your Authorization Policies Here
            });

            services.AddAuthentication(
                IdentityServerAuthenticationDefaults.AuthenticationScheme)

                 .AddIdentityServerAuthentication(options =>
                 {
                     options.Authority = Configuration["OidcConfiguration:Authority"];
                     options.ApiName = Configuration["OidcConfiguration:ApiName"];
                     options.ApiSecret = Configuration["OidcConfiguration:ApiSecret"];
                 });

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "$ext_safeprojectname$", Version = "v1" });
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