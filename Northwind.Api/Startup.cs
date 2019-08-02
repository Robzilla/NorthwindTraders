using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using Northwind.Persistence;
using Northwind.Application.Interfaces;
using Northwind.Common;
using Northwind.Infrastructure;
using AutoMapper;
using System.Reflection;
using Northwind.Application.Infrastructure.AutoMapper;
using MediatR;
using Northwind.Application.Products.Queries.GetProduct;
using Northwind.Application.Infrastructure;

[assembly: FunctionsStartup(typeof(Northwind.Api.Startup))]

namespace Northwind.Api
{
    class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            // Add AutoMapper
            builder.Services.AddAutoMapper(new Assembly[] { typeof(AutoMapperProfile).GetTypeInfo().Assembly });

            // Add framework services.
            builder.Services.AddTransient<INotificationService, NotificationService>();
            builder.Services.AddTransient<IDateTime, MachineDateTime>();

            // Add MediatR
            builder.Services.AddMediatR(typeof(GetProductQueryHandler).GetTypeInfo().Assembly);
            builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(RequestPerformanceBehaviour<,>));
            builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(RequestValidationBehavior<,>));

            // Add DbContext using SQL Server Provider
            string SqlConnection = Environment.GetEnvironmentVariable("SqlConnectionString");
            builder.Services.AddDbContext<INorthwindDbContext, NorthwindDbContext>(options =>
                options.UseSqlServer(SqlConnection));
        }
    }
}
