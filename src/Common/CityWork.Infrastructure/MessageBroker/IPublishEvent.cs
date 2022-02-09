using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityWork.Infrastructure
{
    public interface IPublishEvent
    {
        Task PublishAsync<T>(T message,CancellationToken cancellationToken) where T:class;
    }
}
