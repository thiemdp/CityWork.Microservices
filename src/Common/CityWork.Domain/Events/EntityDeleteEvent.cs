using CityWork.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityWork.Domain.Events
{
    public class EntityDeleteEvent<T> : IDomainEvent<T>
         where T : class, IEntity
    {
        public EntityDeleteEvent(T entity, DateTime eventDateTime)
        {
            Entity = entity;
            EventDateTime = eventDateTime;
        }
        public EntityDeleteEvent(T entity)
        {
            Entity = entity;
            EventDateTime = DateTime.UtcNow;
        }
        public T Entity { get; }

        public DateTime EventDateTime { get; }
    }

    public class EntityDeleteEvent<T,Tkey> : IDomainEvent<T,Tkey>
         where T :class, IEntity<Tkey>
    {
        public EntityDeleteEvent(T entity, DateTime eventDateTime)
        {
            Entity = entity;
            EventDateTime = eventDateTime;
        }
        public EntityDeleteEvent(T entity)
        {
            Entity = entity;
            EventDateTime = DateTime.UtcNow;
        }
        public T Entity { get; }

        public DateTime EventDateTime { get; }
    }
}
