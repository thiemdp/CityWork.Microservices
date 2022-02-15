using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using HealthChecks.UI.Client;

namespace CityWork.Services.AuditLog.API
{
    public static class ModuleServiceCollectionExtensions
    {
        public static WebApplicationBuilder AddCityWorkModule(this WebApplicationBuilder builder )
        {
            var appSettings = new AppSettings();
            builder.Configuration.Bind(appSettings);
            builder.Services.AddDbContext<AuditLogDbContext>(options => options.UseSqlServer(appSettings.ConnectionStrings.CityWorkConnectionString));
            builder.AddApplicationServices<AuditLogDbContext>(Assembly.GetExecutingAssembly(), appSettings);
            builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            return builder;
        }

        public static void UseModuleMiddleware(this IApplicationBuilder app)
        {
            app.UseCityWorkServicesDiscoveryClient();

            using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>()?.CreateScope())
            {
                serviceScope?.ServiceProvider.GetRequiredService<AuditLogDbContext>().Database.Migrate();
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
    }
}
