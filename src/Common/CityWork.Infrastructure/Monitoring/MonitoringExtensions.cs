using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace CityWork.Infrastructure
{
    public static class MonitoringExtensions
    {
        public static WebApplicationBuilder AddMonitoringServices(this WebApplicationBuilder builder, MonitoringOptions monitoringOptions = null)
        {
            if (monitoringOptions?.AppMetrics?.IsEnabled ?? false)
            {
                builder.AddAppMetrics(monitoringOptions.AppMetrics);
            }

            return builder;
        }
    }
}
