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
        public static WebApplicationBuilder AddCityWorkModule(this WebApplicationBuilder builder)
        {
            var appSettings = new AppSettings();
            builder.Configuration.Bind(appSettings);

            builder.Services.AddDbContext<ProductDbContext>(options => options.UseSqlServer(appSettings.ConnectionStrings.CityWorkConnectionString));
            builder.AddApplicationServices<ProductDbContext>(Assembly.GetExecutingAssembly(), appSettings);
            builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            return builder;
        }

        public static void UseModuleMiddleware(this IApplicationBuilder app)
        {
            app.UseCityWorkServicesDiscoveryClient();
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
