using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
/// <summary>
/// Document
/// https://github.com/xabaril/AspNetCore.Diagnostics.HealthChecks#HealthCheckUI
/// https://www.youtube.com/watch?v=Kbfto6Y2xdw
/// </summary>

namespace CityWork.Infrastructure
{
    public static class HealthChecksExtensions
    {
        public static IServiceCollection AddCityWorkHealthChecks(this IServiceCollection services,AppSettings appSettings )
        {
           var healthCheck = services.AddHealthChecks();
           healthCheck.AddCheck("self", () => HealthCheckResult.Healthy());
            if (appSettings.ConnectionStrings?.UseMSSQL == true)
            {
                healthCheck.AddSqlServer(appSettings.ConnectionStrings.CityWorkConnectionString, tags: new[] { "SQLHealhChecks" });
            }
            else if(appSettings.ConnectionStrings?.UseSQLite == true)
            {
                healthCheck.AddSqlite(appSettings.ConnectionStrings.CityWorkConnectionString, tags: new[] { "SQLiteHealhChecks" });
            }
            else if (appSettings.ConnectionStrings?.UseMySQL == true)
            {
                healthCheck.AddMySql(appSettings.ConnectionStrings.CityWorkConnectionString, tags: new[] { "MySQLHealhChecks" });
            }

            if(appSettings.Caching?.UseRedis==true)
            {
                healthCheck.AddRedis("redis",tags: new[] { appSettings.Caching.Redis.InstanceName });
            }    
            if(appSettings.MessageBroker?.UsedRabbitMQ == true)
            {
                healthCheck.AddRabbitMQ(
                appSettings.MessageBroker.RabbitMQ.ConnectionString,
                name: "catalog-rabbitmqbus-check",
                tags: new string[] { "rabbitmqbus" });
            }    
            return services;
        }
    }
}
