using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityWork.Application
{
    public class Dispatcher
    {
        private readonly IMediator _mediator;
        public Dispatcher(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<T> DispatchAsync<T>(IRequest<T> request, CancellationToken cancellationToken = default)
        {
            Task<T> result = _mediator.Send<T>(request, cancellationToken);
            return await result;
        }
    }
}
