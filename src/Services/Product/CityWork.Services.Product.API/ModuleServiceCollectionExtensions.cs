using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using HealthChecks.UI.Client;

namespace CityWork.Services.Product.API
{
    public static class ModuleServiceCollectionExtensions
    {
        public static IServiceCollection AddModule(this IServiceCollection services,IConfiguration configuration, AppSettings appSettings)
        {

            services.AddDbContext<ProductDbContext>(options => options.UseSqlServer(appSettings.ConnectionStrings.CityWorkConnectionString));
            services.AddApplicationServices<ProductDbContext>(Assembly.GetExecutingAssembly(), configuration, appSettings);

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            return services;
        }

        public static void UseModuleMiddleware(this IApplicationBuilder app)
        {
            app.UseDiscoveryClientEureka();
            using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
                serviceScope.ServiceProvider.GetRequiredService<ProductDbContext>().Database.Migrate();
            }
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHealthChecks("/quickhealth");
                endpoints.MapHealthChecks("/health", new HealthCheckOptions()
                {
                    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
                });
            });
        }
        //internal static void InitDatabase(this IApplicationBuilder app)
        //{
        //    using (var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
        //    {
        //        var databaseInitializer = serviceScope.ServiceProvider.GetService<IDatabaseInitializer>();
        //        databaseInitializer.SeedAsync().Wait();
        //    }
        //}
    }
}
