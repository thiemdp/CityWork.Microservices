using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace CityWork.Services.AuditLog.API
{
    public static class ModuleServiceCollectionExtensions
    {
        public static IServiceCollection AddModule(this IServiceCollection services,IConfiguration configuration, AppSettings appSettings)
        {

            services.AddDbContext<AuditLogDbContext>(options => options.UseSqlServer(appSettings.ConnectionStrings.CityWorkConnectionString));
            services.AddApplicationServices<AuditLogDbContext>(Assembly.GetExecutingAssembly(), configuration, appSettings);

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            return services;
        }

        public static void MigrateDatabase(this IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
                serviceScope.ServiceProvider.GetRequiredService<AuditLogDbContext>().Database.Migrate();
            }
        }
    }
}
