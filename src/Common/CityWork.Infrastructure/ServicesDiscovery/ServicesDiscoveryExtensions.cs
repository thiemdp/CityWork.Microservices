using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Steeltoe.Discovery.Client;
using Steeltoe.Discovery.Consul;

namespace CityWork.Infrastructure
{
    public static class ServicesDiscoveryExtensions
    {
        public static IServiceCollection AddCityWorkServicesDiscovery(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDiscoveryClient(configuration);
            services.AddSingleton<ICityWorkRestClient, CityWorkRestClient>();
            return services;
        }

        public static void UseCityWorkServicesDiscoveryClient(this IApplicationBuilder app)
        {
            app.UseDiscoveryClient();
        }
    }
}
