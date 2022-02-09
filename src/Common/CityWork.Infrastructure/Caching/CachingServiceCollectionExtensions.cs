using Microsoft.Extensions.DependencyInjection;

namespace CityWork.Infrastructure
{
    public static class CachingServiceCollectionExtensions
    {
        public static IServiceCollection AddCaches(this IServiceCollection services, CachingOptions options = null)
        {
            //services.AddMemoryCache(opt =>
            //{
            //    opt.SizeLimit = options?.InMemory?.SizeLimit;
            //});

            var distributedProvider = options?.Provider;

            if (distributedProvider == "InMemory")
            {
                services.AddDistributedMemoryCache(opt =>
                {
                    opt.SizeLimit = options?.InMemory?.SizeLimit;
                });
                services.AddSingleton<ICacheServices,CacheServices>();
            }
            else if (distributedProvider == "Redis")
            {
                services.AddDistributedRedisCache(opt =>
                {
                    opt.Configuration = options?.Redis.Configuration;
                    opt.InstanceName = options?.Redis.InstanceName;
                });
                services.AddSingleton<ICacheServices, CacheServices>();
            }
            else if (distributedProvider == "SqlServer")
            {
                services.AddDistributedSqlServerCache(opt =>
                {
                    opt.ConnectionString = options?.SqlServer.ConnectionString;
                    opt.SchemaName = options?.SqlServer.SchemaName;
                    opt.TableName = options?.SqlServer.TableName;
                });
                services.AddSingleton<ICacheServices, CacheServices>();
            }

            return services;
        }
    }
}
