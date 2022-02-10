using Microsoft.Extensions.DependencyInjection;
using CityWork.Infrastructure;
using Microsoft.EntityFrameworkCore;
using MediatR;
using CityWork.Domain;
using System.Reflection;
using Microsoft.Extensions.Configuration;

namespace CityWork.Application
{
    public static class ApplicationServicesExtensions
    {
        public static IServiceCollection AddApplicationServices<TDbContext>(this IServiceCollection services, Assembly assembly,IConfiguration configuration, AppSettings appSettings)
            where TDbContext : DbContext
        {
            services.AddEurekaServicesDiscovery(configuration);
            services.AddSingleton(appSettings);
            services.AddMediatR(assembly);
            services.AddAutoMapper(assembly);
            services.AddScoped<Dispatcher>();
            services.AddScoped<ICurrentUser, CurrentWebUser>();
            services.AddDateTimeProvider();
            services.AddCaches(appSettings.Caching);
            //.net 6 Error????
            //services.AddTransient(typeof(IRepository<>), typeof(DbContextRepository<,>));
            //services.AddTransient(typeof(IRepository<,>), typeof(DbContextRepository<,,>));

            //services.AddTransient(typeof(ICRUDServices<>), typeof(CRUDServices<>));
            //services.AddTransient(typeof(ICRUDServices<,>), typeof(CRUDServices<,>));
            services.AddMessageBrokerWithMassTransit(appSettings);
            services.AddServicesFromDbContext<TDbContext>();
            return services;
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
