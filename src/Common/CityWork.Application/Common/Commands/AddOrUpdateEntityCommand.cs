using CityWork.Domain.Entities;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace CityWork.Application
{
    public class AddOrUpdateEntityCommand<TEntity, TKey> : IRequest<TEntity>
        where TEntity : AggregateRoot<TKey>
    {
        public AddOrUpdateEntityCommand(TEntity entity)
        {
            Entity = entity;
        }

        public TEntity Entity { get; set; }

        public class AddOrUpdateEntityCommandHandler : IRequestHandler<AddOrUpdateEntityCommand<TEntity, TKey>, TEntity>
        {
            private readonly ICRUDServices<TEntity, TKey> _crudService;
            public AddOrUpdateEntityCommandHandler(ICRUDServices<TEntity, TKey> crudService)
            {
                this._crudService = crudService;
            }

            public async Task<TEntity> Handle(AddOrUpdateEntityCommand<TEntity, TKey> request, CancellationToken cancellationToken)
            {
                await _crudService.AddOrUpdateAsync(request.Entity);
                return request.Entity;
            }
        }
    }
}
