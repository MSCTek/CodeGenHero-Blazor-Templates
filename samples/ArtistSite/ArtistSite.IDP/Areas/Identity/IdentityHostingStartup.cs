using ArtistSite.IDP.Areas.Identity.Data;
using ArtistSite.IDP.Contexts;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

[assembly: HostingStartup(typeof(ArtistSite.IDP.Areas.Identity.IdentityHostingStartup))]

namespace ArtistSite.IDP.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) =>
            {
                services.AddDbContext<UserDbContext>(options =>
                    options.UseSqlServer(
                        context.Configuration.GetConnectionString("UserDbContextConnection")));

                // Do not register the default UI pages.
                //services.AddDefaultIdentity<ApplicationUser>(
                //    options => options.SignIn.RequireConfirmedAccount = true)
                //    .AddEntityFrameworkStores<UserDbContext>();

                services.AddIdentity<ApplicationUser, IdentityRole>(
                    options => options.SignIn.RequireConfirmedAccount = true)
                    .AddEntityFrameworkStores<UserDbContext>()
                    .AddDefaultTokenProviders();
            });
        }
    }
}