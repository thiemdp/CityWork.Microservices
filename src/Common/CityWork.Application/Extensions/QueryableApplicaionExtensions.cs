using AutoMapper;
using CityWork.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CityWork.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace CityWork.Application
{
    public static class QueryableApplicaionExtensions
    {
        public static PagedResult<TResponse> ToPageResult<T,TResponse>(this IQueryable<T> query, PagedAndSortedRequest pagedResultRequest, IMapper mapper)
        {
            int totalCount = query.Count();
            var ListItems= query.PageBy(pagedResultRequest).Select(x=> mapper.Map<TResponse>(x)).ToList();
            return new PagedResult<TResponse>(totalCount,ListItems);
        }

        public static async Task<PagedResult<TResponse>> ToPageResultAsync<T, TResponse>(this IQueryable<T> query, PagedAndSortedRequest pagedResultRequest, IMapper mapper)
        {
            int totalCount = query.Count();
            var ListItems = await query.PageBy(pagedResultRequest).Select(x => mapper.Map<TResponse>(x)).ToListAsync();
            return new PagedResult<TResponse>(totalCount, ListItems);
        }
    }
}
