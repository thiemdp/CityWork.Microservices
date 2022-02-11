using App.Metrics;
using App.Metrics.AspNetCore;
using App.Metrics.AspNetCore.Endpoints;
using App.Metrics.AspNetCore.Tracking;
using App.Metrics.Formatters.Prometheus;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;

namespace CityWork.Infrastructure
{
    public static class AppMetricsServiceCollectionExtensions
    {
        public static WebApplicationBuilder AddAppMetrics(this WebApplicationBuilder builder, AppMetricsOptions appMetricsOptions = default!)
        {
            if (appMetricsOptions?.IsEnabled ?? false)
            {
                builder.Host.UseMetricsWebTracking();
                builder.Services.Configure<KestrelServerOptions>(opts => { opts.AllowSynchronousIO = true; });
                var metrics =  AppMetrics.CreateDefaultBuilder()
                .Configuration.Configure(appMetricsOptions.MetricsOptions)
                .OutputMetrics.AsPrometheusPlainText()
                .Build();

                var options = new MetricsWebHostOptions
                {
                    EndpointOptions = endpointsOptions =>
                    {
                        endpointsOptions.MetricsEndpointEnabled = appMetricsOptions.MetricEndpointsOptions.MetricsEndpointEnabled;
                        endpointsOptions.MetricsTextEndpointEnabled = appMetricsOptions.MetricEndpointsOptions.MetricsTextEndpointEnabled;
                        endpointsOptions.EnvironmentInfoEndpointEnabled = appMetricsOptions.MetricEndpointsOptions.EnvironmentInfoEndpointEnabled;
                        endpointsOptions.MetricsTextEndpointOutputFormatter = metrics.OutputMetricsFormatters.OfType<MetricsPrometheusTextOutputFormatter>().First();
                        endpointsOptions.MetricsEndpointOutputFormatter = metrics.OutputMetricsFormatters.OfType<MetricsPrometheusTextOutputFormatter>().First();
                    },

                    TrackingMiddlewareOptions = trackingMiddlewareOptions =>
                    {
                        trackingMiddlewareOptions.ApdexTrackingEnabled = appMetricsOptions.MetricsWebTrackingOptions.ApdexTrackingEnabled;
                        trackingMiddlewareOptions.ApdexTSeconds = appMetricsOptions.MetricsWebTrackingOptions.ApdexTSeconds;
                        trackingMiddlewareOptions.IgnoredHttpStatusCodes = appMetricsOptions.MetricsWebTrackingOptions.IgnoredHttpStatusCodes;
                        trackingMiddlewareOptions.IgnoredRoutesRegexPatterns = appMetricsOptions.MetricsWebTrackingOptions.IgnoredRoutesRegexPatterns;
                        trackingMiddlewareOptions.OAuth2TrackingEnabled = appMetricsOptions.MetricsWebTrackingOptions.OAuth2TrackingEnabled;
                    },
                };

                builder.Services.AddMetrics(metrics);
                builder.Services.AddMetricsReportingHostedService(options.UnobservedTaskExceptionHandler);
                builder.Services.AddMetricsEndpoints(options.EndpointOptions);
                builder.Services.AddMetricsTrackingMiddleware(options.TrackingMiddlewareOptions);

                builder.Services.AddSingleton<IStartupFilter>(new DefaultMetricsEndpointsStartupFilter());
                builder.Services.AddSingleton<IStartupFilter>(new DefaultMetricsTrackingStartupFilter());
            }

            return builder;
        }
    }
}
