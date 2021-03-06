using Humanizer.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WazeCredit.Utility.AppSettingsClasses;

namespace WazeCredit.Utility.DI_Config
{
    public static class DI_AppsettingsConfig
    {
        /// <summary>
        /// Get section value to classes (TOptions)
        /// </summary>
        /// <param name="services">service collection</param>
        /// <param name="configuration">configuration</param>
        /// <returns></returns>
        public static IServiceCollection AddAppSettingsConfig(this IServiceCollection services
                    , IConfiguration configuration)
        {
            //Config AppSettings key to containter
            services.Configure<WazeForecastSettings>(configuration.GetSection("WazeForecast"));
            services.Configure<StripeSettings>(configuration.GetSection("Stripe"));
            services.Configure<SendGridSettings>(configuration.GetSection("SendGrid"));
            services.Configure<TwilloSettings>(configuration.GetSection("Twilio"));

            return services;
        } 
    }
}
