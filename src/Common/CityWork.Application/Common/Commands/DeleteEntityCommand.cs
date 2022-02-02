using CityWork.Domain.Entities;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace CityWork.Application
{
    public class DeleteEntityCommand<TEntity, TKey> : IRequest
         where TEntity : AggregateRoot<TKey>
    {
        public TEntity Entity { get; set; }

        public DeleteEntityCommand(TEntity entity)
        {
            Entity = entity;
        }

        public class DeleteEntityCommandHandler : IRequestHandler<DeleteEntityCommand<TEntity, TKey>>
        {
            private readonly ICRUDServices<TEntity, TKey> _crudService;
            public DeleteEntityCommandHandler(ICRUDServices<TEntity, TKey> crudService)
            {
                this._crudService = crudService;
            }

            public async Task<Unit> Handle(DeleteEntityCommand<TEntity, TKey> request, CancellationToken cancellationToken)
            {
                await _crudService.DeleteAsync(request.Entity);
                return Unit.Value;
            }
        }
    }
}
