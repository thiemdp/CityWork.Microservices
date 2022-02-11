using App.Metrics;
using App.Metrics.AspNetCore.Endpoints;
using App.Metrics.AspNetCore.Tracking;
/// <summary>
/// Document
/// https://www.youtube.com/watch?v=sM7D8biBf4k&t=291s
/// https://www.app-metrics.io/web-monitoring/aspnet-core/quick-start/
/// AppMetrics:{
/// "IsEnabled":true,
/// "MetricsOptions": {
///"DefaultContextLabel": "MyMvcApplication",
///    "Enabled": true
///  },
///"MetricsWebTrackingOptions": {
///"ApdexTrackingEnabled": true,
///"ApdexTSeconds": 0.1,
///"IgnoredHttpStatusCodes": [ 404 ],
///"IgnoredRoutesRegexPatterns": [],
///"OAuth2TrackingEnabled": true
///  },
///"MetricEndpointsOptions": {
///"MetricsEndpointEnabled": true,
///"MetricsTextEndpointEnabled": true,    
///"EnvironmentInfoEndpointEnabled": true
///  }
/// }
/// </summary>
namespace CityWork.Infrastructure
{
    public class AppMetricsOptions
    {
        public bool IsEnabled { get; set; }

        public MetricsOptions MetricsOptions { get; set; } = default!;

        public MetricsWebTrackingOptions MetricsWebTrackingOptions { get; set; }=default!;

        public MetricEndpointsOptions MetricEndpointsOptions { get; set; } = default!;
    }
}
