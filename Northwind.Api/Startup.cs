using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Northwind.Application.Customers.Commands.CreateCustomer;
using Northwind.Application.Customers.Commands.DeleteCustomer;
using Northwind.Application.Customers.Commands.UpdateCustomer;
using Northwind.Application.Customers.Queries.GetCustomerDetail;
using Northwind.Application.Infrastructure;
using Northwind.Application.Infrastructure.AutoMapper;
using Northwind.Application.Interfaces;
using Northwind.Application.Products.Queries.GetProduct;
using Northwind.Common;
using Northwind.Infrastructure;
using Northwind.Persistence;
using System;
using System.Reflection;

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

            var concreteContext = (NorthwindDbContext)builder.Services.BuildServiceProvider().GetService<INorthwindDbContext>();
            concreteContext.Database.Migrate();
            NorthwindInitializer.Initialize(concreteContext);

            //add fluent validation
            //TODO: Add helper method when available e.g. .AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<CreateCustomerCommandValidator>());
            //see https://github.com/JeremySkinner/FluentValidation/issues/1205

            builder.Services.AddTransient<IValidator<CreateCustomerCommand>, CreateCustomerCommandValidator>();
            builder.Services.AddTransient<IValidator<DeleteCustomerCommand>, DeleteCustomerCommandValidator>();
            builder.Services.AddTransient<IValidator<UpdateCustomerCommand>, UpdateCustomerCommandValidator>();
            builder.Services.AddTransient<IValidator<GetCustomerDetailQuery>, GetCustomerDetailQueryValidator>();

            
        }
    }
}
