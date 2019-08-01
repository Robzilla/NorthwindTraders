using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using Northwind.Persistence;
using Northwind.Application.Interfaces;
using Northwind.Common;
using Northwind.Infrastructure;

[assembly: FunctionsStartup(typeof(Northwind.Api.Startup))]

namespace Northwind.Api
{
    class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {

            // Add framework services.
            builder.Services.AddTransient<INotificationService, NotificationService>();
            builder.Services.AddTransient<IDateTime, MachineDateTime>();

            string SqlConnection = Environment.GetEnvironmentVariable("SqlConnectionString");
            builder.Services.AddDbContext<NorthwindDbContext>(
                options => options.UseSqlServer(SqlConnection));
        }
    }
}
