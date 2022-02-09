using MassTransit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityWork.Infrastructure
{
    public class PublishEvent : IPublishEvent
    {
        private IServiceProvider _serviceProvider;
        private AppSettings _appSettings;
        public PublishEvent(IServiceProvider serviceProvider, AppSettings appSettings)
        {
            _serviceProvider = serviceProvider;
            _appSettings = appSettings;
        }
        public async Task PublishAsync<T>(T message, CancellationToken cancellationToken) where T : class
        {
            if (_appSettings.MessageBroker.UsedMessageBroker())
            {
                var publishEndpoint = _serviceProvider.GetService(typeof(IPublishEndpoint)) as IPublishEndpoint;
                if (publishEndpoint != null)
                {
                       await publishEndpoint.Publish<T>(message, cancellationToken);
                }
            }
        }
    }
}
