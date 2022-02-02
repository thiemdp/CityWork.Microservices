using CityWork.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using CityWork.Domain;

namespace CityWork.Application
{
    public interface ICRUDServices<T>: ICRUDServices<T, int> where T :class, IEntity
    {
    }

    public interface ICRUDServices<T, TPrimaryKey> where T :class, IEntity<TPrimaryKey>
    {
        IMapper Mapper { get; }

        IRepository<T, TPrimaryKey> Repository { get; }

        Task<List<T>> GetAsync();

        Task<T> GetByIdAsync(TPrimaryKey id);

        Task AddOrUpdateAsync(T entity, CancellationToken cancellationToken=default);

        Task DeleteAsync(T entity, CancellationToken cancellationToken = default);

        Task DeleteByIdAsync(TPrimaryKey id, CancellationToken cancellationToken = default);
    }
}
