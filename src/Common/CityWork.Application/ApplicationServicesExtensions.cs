using Microsoft.Extensions.DependencyInjection;
using CityWork.Infrastructure;
using Microsoft.EntityFrameworkCore;
using MediatR;
using CityWork.Domain;
using System.Reflection;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Builder;

namespace CityWork.Application
{
    public static class ApplicationServicesExtensions
    {
        public static WebApplicationBuilder AddApplicationServices<TDbContext>(this WebApplicationBuilder builder, Assembly assembly, AppSettings appSettings)
            where TDbContext : DbContext
        {
            builder.Host.UseCityWorkLogger(configuration =>
            {
                return new LoggingOptions();
            });
            builder.Services.AddCityWorkServicesDiscovery(builder.Configuration);
            builder.Services.AddSingleton(appSettings);
            builder.Services.AddMediatR(assembly);
            builder.Services.AddAutoMapper(assembly);
            builder.Services.AddScoped<Dispatcher>();
            builder.Services.AddScoped<ICurrentUser, CurrentWebUser>();
            builder.Services.AddDateTimeProvider();
            builder.Services.AddCaches(appSettings.Caching);
            //.net 6 Error????
            //services.AddTransient(typeof(IRepository<>), typeof(DbContextRepository<,>));
            //services.AddTransient(typeof(IRepository<,>), typeof(DbContextRepository<,,>));

            //services.AddTransient(typeof(ICRUDServices<>), typeof(CRUDServices<>));
            //services.AddTransient(typeof(ICRUDServices<,>), typeof(CRUDServices<,>));
            builder.Services.AddMessageBrokerWithMassTransit(appSettings);
            builder.Services.AddServicesFromDbContext<TDbContext>();
            builder.Services.AddCityWorkHealthChecks(appSettings);
            builder.AddMonitoringServices(appSettings.Monitoring);
            return builder;
        }
        public static IServiceCollection AddServicesFromDbContext<TDbContext>(this IServiceCollection services)
           where TDbContext : DbContext
        {
            Type t = typeof(TDbContext);
            var properties = t.GetProperties().Where(x => x.PropertyType.Name.ToLower().StartsWith("dbset")).ToList();
            foreach (var property in properties)
            {
                Type pty = property.PropertyType;

                Type entity = pty.GenericTypeArguments[0];
                if (entity != null)
                {
                    var IdFieldProperty = entity.GetProperty("Id");
                    if (IdFieldProperty != null)
                    {
                        if (IdFieldProperty.PropertyType == typeof(int))
                        {
                            // add repository
                            Type tempInterface = typeof(IRepository<>);
                            Type i = tempInterface.MakeGenericType(entity);
                            Type tempImplement = typeof(DbContextRepository<,>);
                            Type imp = tempImplement.MakeGenericType(t, entity);
                            services.AddScoped(i, imp);
                            // add CRUDServices
                            tempInterface = typeof(ICRUDServices<>);
                            i = tempInterface.MakeGenericType(entity);
                            tempImplement = typeof(CRUDServices<>);
                            imp = tempImplement.MakeGenericType(entity);
                            services.AddScoped(i, imp);
                        }
                        else
                        {
                            // add repository
                            Type tempInterface = typeof(IRepository<,>);
                            Type i = tempInterface.MakeGenericType(entity, IdFieldProperty.PropertyType);
                            Type tempImplement = typeof(DbContextRepository<,,>);
                            Type imp = tempImplement.MakeGenericType(t, entity, IdFieldProperty.PropertyType);
                            services.AddScoped(i, imp);
                            // add CRUDServices
                            tempInterface = typeof(ICRUDServices<,>);
                            i = tempInterface.MakeGenericType(entity, IdFieldProperty.PropertyType);
                            tempImplement = typeof(CRUDServices<,>);
                            imp = tempImplement.MakeGenericType(entity, IdFieldProperty.PropertyType);
                            services.AddScoped(i, imp);
                        }
                    }
                }
            }
            return services;
        }

    }
}
