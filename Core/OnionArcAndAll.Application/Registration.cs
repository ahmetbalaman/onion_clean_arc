using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using OnionArcAndAll.Application.Bases;
using OnionArcAndAll.Application.Behaviors;
using OnionArcAndAll.Application.Exceptions;
using OnionArcAndAll.Application.Features.Products.Rules;
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
            
            services.AddRulesFromAssemblyContaining(assembly, typeof(BaseRules));

            services.AddValidatorsFromAssembly(assembly);
            ValidatorOptions.Global.LanguageManager.Culture= new System.Globalization.CultureInfo("tr");

            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(FluentValidationBehavior<,>));

            // Register services from the assembly
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(assembly));
        }

        private static IServiceCollection AddRulesFromAssemblyContaining(this IServiceCollection services, Assembly assembly, Type type)
        {
            var types = assembly.GetTypes()
                .Where(t=> t.IsSubclassOf(type) && type != t).ToList();
           foreach(var t in types)
                services.AddTransient(t);

            return services;
        }

     
    }
}
