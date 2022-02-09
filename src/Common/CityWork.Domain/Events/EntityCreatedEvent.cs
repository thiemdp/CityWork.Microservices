using CityWork.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityWork.Domain.Events
{
    public class EntityCreatedEvent<T> : IDomainEvent<T>
         where T : class, IEntity
    {
        public EntityCreatedEvent(T entity, DateTime eventDateTime)
        {
            Entity = entity;
            EventDateTime = eventDateTime;
        }

        public EntityCreatedEvent(T entity )
        {
            Entity = entity;
            EventDateTime = DateTime.UtcNow;
        }

        public T Entity { get; }

        public DateTime EventDateTime { get; }
    }

    public class EntityCreatedEvent<T,Tkey> : IDomainEvent<T,Tkey>
         where T : class, IEntity<Tkey>
    {
        public EntityCreatedEvent(T entity, DateTime eventDateTime)
        {
            Entity = entity;
            EventDateTime = eventDateTime;
        }
        public EntityCreatedEvent(T entity)
        {
            Entity = entity;
            EventDateTime = DateTime.UtcNow;
        }
        public T Entity { get; }

        public DateTime EventDateTime { get; }
    }
}
