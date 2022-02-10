using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace CityWork.Services.Product.API
{
    public static class ModuleServiceCollectionExtensions
    {
        public static IServiceCollection AddProductModule(this IServiceCollection services,IConfiguration configuration, AppSettings appSettings)
        {

            services.AddDbContext<ProductDbContext>(options => options.UseSqlServer(appSettings.ConnectionStrings.CityWorkConnectionString));
            services.AddApplicationServices<ProductDbContext>(Assembly.GetExecutingAssembly(), configuration, appSettings);

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            return services;
        }

        public static void MigrateDatabase(this IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
                serviceScope.ServiceProvider.GetRequiredService<ProductDbContext>().Database.Migrate();
            }
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
