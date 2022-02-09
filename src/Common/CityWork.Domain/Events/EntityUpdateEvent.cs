using CityWork.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityWork.Domain.Events
{
    public class EntityUpdateEvent<T> : IDomainEvent<T>
         where T : class, IEntity
    {
        public EntityUpdateEvent(T entity, DateTime eventDateTime)
        {
            Entity = entity;
            EventDateTime = eventDateTime;
        }
        public EntityUpdateEvent(T entity)
        {
            Entity = entity;
            EventDateTime = DateTime.UtcNow;
        }
        public T Entity { get; }

        public DateTime EventDateTime { get; }
    }

    public class EntityUpdateEvent<T,Tkey> : IDomainEvent<T,Tkey>
         where T : class, IEntity<Tkey>
    {
        public EntityUpdateEvent(T entity, DateTime eventDateTime)
        {
            Entity = entity;
            EventDateTime = eventDateTime;
        }
        public EntityUpdateEvent(T entity)
        {
            Entity = entity;
            EventDateTime = DateTime.UtcNow;
        }
        public T Entity { get; }

        public DateTime EventDateTime { get; }
    }
}
