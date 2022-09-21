namespace $safeprojectname$
{
    using Microsoft.IdentityModel.Logging;
    using Serilog;
    using System;

    public class Program
    {
        public static readonly string AppName;
        public static readonly string Namespace;

        static Program()
        {
            Namespace = typeof(Startup).Namespace;
            AppName = Namespace.Substring(Namespace.LastIndexOf('.', Namespace.LastIndexOf('.') - 1) + 1);
        }

        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Host.UseSerilog((ctx, loggerConfiguration) => loggerConfiguration
                .ReadFrom.Configuration(builder.Configuration));

            try
            {
                builder.Services.AddControllers();
                // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
                builder.Services.AddEndpointsApiExplorer();
                builder.Services.AddSwaggerGen();

                Startup startup = new Startup(builder.Configuration);
                startup.ConfigureServices(builder.Services);

                var app = builder.Build();

                if (app.Environment.IsDevelopment())
                {
                    app.UseDeveloperExceptionPage();
                    //app.UseExceptionHandler("/error-development");

                    app.UseSwagger();
                    app.UseSwaggerUI();
                }
                else
                {
                    app.UseExceptionHandler("/error");
                }

                //mapper.ConfigurationProvider.AssertConfigurationIsValid(); // See: https://stackoverflow.com/questions/51547124/where-to-validate-automapper-configuration-in-asp-net-core-application

                app.UseSerilogRequestLogging();

                app.UseHttpsRedirection();

                app.UseRouting();

                app.UseCors("Open");

                app.UseAuthentication();

                app.UseAuthorization();

#if DEBUG
                IdentityModelEventSource.ShowPII = true;
#endif

                app.UseEndpoints(endpoints =>
                {
                    endpoints.MapControllers();
                });

                app.MapControllers();

                app.Run();
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Program terminated unexpectedly ({ApplicationContext})!", Program.AppName);
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

    }
}