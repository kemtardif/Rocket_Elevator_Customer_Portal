using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Rocker_Elevator_Customer_Portal.Areas.Identity.Data;

[assembly: HostingStartup(typeof(Rocker_Elevator_Customer_Portal.Areas.Identity.IdentityHostingStartup))]
namespace Rocker_Elevator_Customer_Portal.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) => {
                services.AddDbContext<ClientDbContext>(options =>
                    options.UseSqlServer(
                        context.Configuration.GetConnectionString("SQLS")));

                services.AddDefaultIdentity<Client>(options => options.SignIn.RequireConfirmedAccount = false)
                    .AddEntityFrameworkStores<ClientDbContext>();
            });
        }
    }
}