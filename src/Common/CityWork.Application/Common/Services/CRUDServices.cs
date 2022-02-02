using AutoMapper;
using CityWork.Domain;
using CityWork.Domain.Entities;
using CityWork.Shared.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityWork.Application
{
    public class CRUDServices<T> : CRUDServices<T, int>, ICRUDServices<T>
       where T : class, IEntity
    {
        public CRUDServices(IRepository<T> repository, IMapper mapper) : base(repository, mapper)
        {
        }
    }

    public class CRUDServices<T, TPrimaryKey> : ICRUDServices<T, TPrimaryKey>
         where T : class, IEntity<TPrimaryKey>
    {
        protected readonly IUnitOfWork _unitOfWork;
        private readonly IRepository<T, TPrimaryKey> _repository;
        private IMapper _mapper;
        public CRUDServices(IRepository<T, TPrimaryKey> repository, IMapper mapper)
        {
            _unitOfWork = repository.UnitOfWork;
            _repository = repository;
            _mapper = mapper;
        }
         
        public IMapper Mapper => _mapper;

        public IRepository<T, TPrimaryKey> Repository => _repository;

        public async Task AddOrUpdateAsync(T entity, CancellationToken cancellationToken = default)
        {
            var adding = entity.Id is null || entity.Id.Equals(default(TPrimaryKey));
            if (adding)
                await _repository.InsertAsync(entity);
            else
                await _repository.UpdateAsync(entity);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            //if (adding)
            //{
            //    await _domainEvents.DispatchAsync(new EntityCreatedEvent<T>(entity, DateTime.UtcNow));
            //}
            //else
            //{
            //    await _domainEvents.DispatchAsync(new EntityUpdatedEvent<T>(entity, DateTime.UtcNow));
            //}
        }

        public async Task DeleteAsync(T entity, CancellationToken cancellationToken = default)
        {
            _repository.Delete(entity);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }

        public async Task DeleteByIdAsync(TPrimaryKey id, CancellationToken cancellationToken = default)
        {
            T item = await GetByIdAsync(id);
            if (item == null)
                throw new Exception("Item is not found!");
            await DeleteAsync(item,cancellationToken);
        }

        public async Task<List<T>> GetAsync()
        {
            return await _repository.GetAllListAsync();
        }

        public async Task<T> GetByIdAsync(TPrimaryKey id)
        {
            return await _repository.GetAsync(id);
        }
    }
}
