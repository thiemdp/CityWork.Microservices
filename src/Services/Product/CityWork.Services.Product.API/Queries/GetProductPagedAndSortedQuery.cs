using CityWork.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;


namespace CityWork.Services.Product.API
{
    public class GetProductPagedAndSortedQuery : PagedAndSortedRequest, IShouldNormalize, IRequest<PagedResult<ProductFullResponse>>
    {
        public string? KeyWords { get; set; } = default!;
        public double? MinPrice { get; set; }  
        public double? MaxPrice { get; set; }
        public void Normalize()
        {
            Sorting = "Id";
        }
    }
    public class GetProductPagedAndSortedQueryHander : IRequestHandler<GetProductPagedAndSortedQuery, PagedResult <ProductFullResponse>>
    {
        private readonly ICRUDServices<Product> _crudServices;
        private readonly ICacheServices _cache;
        public GetProductPagedAndSortedQueryHander(ICRUDServices<Product> crudServices, ICacheServices cache)
        {
            _crudServices = crudServices;
            _cache = cache;
        }
        public async Task<PagedResult<ProductFullResponse>> Handle(GetProductPagedAndSortedQuery request, CancellationToken cancellationToken)
        {
            string key = $"GetProductPagedAndSortedQuery_{request.KeyWords}_{request.MinPrice}_{request.MaxPrice}_{request.PageNumber}_{request.PageSize}";
            var result = await _cache.GetOrSetAsync<PagedResult<ProductFullResponse>>(key, () => {
                IQueryable<Product> query = _crudServices.Repository.GetAll()
                      .WhereIf(request.KeyWords.HasValue(), x => x.Name.Contains(request.KeyWords) || x.Code.Contains(request.KeyWords) || x.Description.Contains(request.KeyWords))
                      .WhereIf(request.MinPrice.HasValue, x => x.Price >= request.MinPrice.Value)
                      .WhereIf(request.MaxPrice.HasValue, x => x.Price <= request.MaxPrice.Value)
                      ;

                return query.OrderBy(x => x.Id).ToPageResultAsync<Product, ProductFullResponse>(request, _crudServices.Mapper);
            });
            return result;

             
        }
    }
}
