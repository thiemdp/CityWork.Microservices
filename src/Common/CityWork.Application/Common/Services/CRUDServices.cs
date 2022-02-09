using AutoMapper;
using CityWork.Domain;
using CityWork.Domain.Entities;
using CityWork.Infrastructure;
using CityWork.Domain.Events;
using CityWork.Shared.Exceptions;
using MassTransit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;

namespace CityWork.Application
{
    public class CRUDServices<T> : CRUDServices<T, int>, ICRUDServices<T>
       where T : class, IEntity
    {
        public CRUDServices(IRepository<T> repository, IMapper mapper, IPublishEvent publishEvent, ICurrentUser currentUser)
            : base(repository, mapper, publishEvent,currentUser)
        {
        }
    }

    public class CRUDServices<T, TPrimaryKey> : ICRUDServices<T, TPrimaryKey>
         where T : class, IEntity<TPrimaryKey>
    {
        protected readonly IUnitOfWork _unitOfWork;
        private readonly IRepository<T, TPrimaryKey> _repository;
        private IMapper _mapper;
        private readonly IPublishEvent _publishEvent;
        private readonly ICurrentUser _currentUser;
        public CRUDServices(IRepository<T, TPrimaryKey> repository, IMapper mapper, IPublishEvent publishEvent, ICurrentUser currentUser)
        {
            _unitOfWork = repository.UnitOfWork;
            _repository = repository;
            _mapper = mapper;
            _publishEvent = publishEvent;
            _currentUser = currentUser;
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
            if (entity is ICheckEntityChange)
            {
                AuditLogEvent auditLogEvent = new AuditLogEvent()
                {
                    DataJson = JsonSerializer.Serialize(entity),
                    EventTime = DateTime.UtcNow,
                    ObjectId = entity.Id.ToString(),
                    TypeOf = typeof(T).Name,
                    UserId = _currentUser?.UserId.ToString()

                };
                if (adding)
                {
                    auditLogEvent.Description = $"Add new { auditLogEvent.TypeOf } have id= { auditLogEvent.ObjectId}";
                    auditLogEvent.Action = "Add";
                }
                else
                {
                    auditLogEvent.Description = $"Update { auditLogEvent.TypeOf } have id= { auditLogEvent.ObjectId}";
                    auditLogEvent.Action = "Update";
                }
                await _publishEvent.PublishAsync<AuditLogEvent>(auditLogEvent, cancellationToken);
            }

        }

        public async Task DeleteAsync(T entity, CancellationToken cancellationToken = default)
        {
            _repository.Delete(entity);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            if (entity is ICheckEntityChange)
            {
                AuditLogEvent auditLogEvent = new AuditLogEvent()
                {
                    DataJson = JsonSerializer.Serialize(entity),
                    EventTime = DateTime.UtcNow,
                    ObjectId = entity.Id.ToString(),
                    TypeOf = typeof(T).Name,
                    UserId = _currentUser?.UserId.ToString()

                };
                auditLogEvent.Description = $"Delete { auditLogEvent.TypeOf } have id= { auditLogEvent.ObjectId}";
                auditLogEvent.Action = "Delete";
                await _publishEvent.PublishAsync<AuditLogEvent>(auditLogEvent, cancellationToken);
            }
        }

        public async Task DeleteByIdAsync(TPrimaryKey id, CancellationToken cancellationToken = default)
        {
            T item = await GetByIdAsync(id);
            if (item == null)
                throw new Exception("Item is not found!");
            await DeleteAsync(item, cancellationToken);
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
