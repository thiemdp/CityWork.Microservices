using System;
using System.Collections.Generic;
using System.Text;
using CityWork.Domain.Entities;

namespace CityWork.Domain.Events
{
    public interface IDomainEvent<T> : IDomainEvent<T,int>
        where T : class, IEntity<int>
    {
         
    }

    public interface IDomainEvent<T,Tkey> where T: class,IEntity<Tkey>
    {
        public T Entity { get; }
        public DateTime EventDateTime { get; }
    }
}
