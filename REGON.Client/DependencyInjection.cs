using Microsoft.Extensions.DependencyInjection;
using REGON.Client.Services;
using REGON.Client.Settings;
using SOFTURE.Settings.Extensions;

namespace REGON.Client
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddRegonClient<TSettings>(this IServiceCollection services)
            where TSettings : IRegonSettings
        {
            var settings = services.GetSettings<TSettings, RegonSettings>(x => x.Regon);

            services.AddTransient<IRegonClient>(_ => new RegonClient(settings.ApiKey));

            return services;
        }
    }
}