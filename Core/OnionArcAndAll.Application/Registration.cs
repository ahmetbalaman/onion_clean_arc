using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using OnionArcAndAll.Application.Behaviors;
using OnionArcAndAll.Application.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace OnionArcAndAll.Application
{
    public static class Registration
    {
        public static void AddApplication(this IServiceCollection services)
        {
            var assembly = Assembly.GetExecutingAssembly();

            // Register services for middlware exceptions
            services.AddTransient<ExceptionMiddleware>();

            services.AddValidatorsFromAssembly(assembly);
            ValidatorOptions.Global.LanguageManager.Culture= new System.Globalization.CultureInfo("tr");

            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(FluentValidationBehavior<,>));

            // Register services from the assembly
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(assembly));
        }

    }
}
