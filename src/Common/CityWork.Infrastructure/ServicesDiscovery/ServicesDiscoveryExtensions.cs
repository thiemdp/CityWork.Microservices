using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Steeltoe.Discovery.Client;

namespace CityWork.Infrastructure
{
    public static class ServicesDiscoveryExtensions
    {
        public static IServiceCollection AddEurekaServicesDiscovery(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDiscoveryClient(configuration);
            services.AddSingleton<ICityWorkRestClient, CityWorkRestClient>();
            return services;
        }

        public static void UseDiscoveryClientEureka(this IApplicationBuilder app)
        {
            app.UseDiscoveryClient();
        }
    }
}
