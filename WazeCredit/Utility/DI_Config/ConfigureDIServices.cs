using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using WazeCredit.Models;
using WazeCredit.Service.LifeTimeExample;
using WazeCredit.Service;

namespace WazeCredit.Utility.DI_Config
{
    public static class ConfigureDIServices
    {
        public static IServiceCollection AddAllServices(this IServiceCollection services)
        {
            services.AddTransient<IMarketForecaster, MarketForecasterV2>();
            services.TryAddEnumerable(new[]
            {
                ServiceDescriptor.Scoped<IValidationChecker, AddressValidationChecker>(),
                ServiceDescriptor.Scoped<IValidationChecker, CreditValidationChecker>()
            });

            services.AddScoped<ICreditValidator, CreditValidator>();
            services.AddTransient<TransientService>();
            services.AddScoped<ScopedService>();
            services.AddSingleton<SingletonService>();

            services.AddScoped<CreditApprovedHigh>();
            services.AddScoped<CreditApprovedLow>();

            // Register service with conditional
            services.AddScoped<Func<CreditApprovedEnum, ICreditApproved>>(ServiceProvider => range =>
            {
                switch (range)
                {
                    case CreditApprovedEnum.Low: return ServiceProvider.GetService<CreditApprovedLow>();
                    case CreditApprovedEnum.High: return ServiceProvider.GetService<CreditApprovedHigh>();
                    default: return ServiceProvider.GetService<CreditApprovedLow>();
                }
            });

            // Change DI 
            //services.AddTransient<IMarketForecaster, MarketForecaster>();

            /* Different way to register service
            //services.AddSingleton<IMarketForecaster>(new MarketForecasterV2());
            //services.AddTransient<MarketForecasterV2>();
            //services.AddSingleton(new MarketForecasterV2());
            //services.AddTransient(typeof(MarketForecasterV2));
            services.AddTransient(typeof(IMarketForecaster), typeof(MarketForecasterV2));
            

            // use try add to check service be signed before or not if not signed then use MarketForecaster
            // else use MarketForecasterV2
            services.TryAddTransient<IMarketForecaster, MarketForecaster>();

            // replace service MarketForecaster on service MarketForecasterV2
            services.Replace(ServiceDescriptor.Transient<IMarketForecaster, MarketForecaster>());

            // remove all service be signed by IMarketForecaster
            services.RemoveAll<IMarketForecaster>();
            */
            /* Multiple service
            //services.AddScoped<IValidationChecker, AddressValidationChecker>();
            //services.AddScoped<IValidationChecker, CreditValidationChecker>();

            //Another way to register service multiple
            //services.TryAddEnumerable(ServiceDescriptor.Scoped<IValidationChecker, AddressValidationChecker>());
            //services.TryAddEnumerable(ServiceDescriptor.Scoped<IValidationChecker, CreditValidationChecker>());
            */

            return services;
        }


    }
}
