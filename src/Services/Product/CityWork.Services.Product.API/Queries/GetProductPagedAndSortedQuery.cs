using CityWork.Infrastructure;
using Microsoft.EntityFrameworkCore;


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
        public GetProductPagedAndSortedQueryHander(ICRUDServices<Product> crudServices)
        {
            _crudServices = crudServices;
        }
        public async Task<PagedResult<ProductFullResponse>> Handle(GetProductPagedAndSortedQuery request, CancellationToken cancellationToken)
        {
            IQueryable<Product> query = _crudServices.Repository.GetAll()
                .WhereIf(request.KeyWords.HasValue(),x=>x.Name.Contains(request.KeyWords)|| x.Code.Contains(request.KeyWords)||x.Description.Contains(request.KeyWords))
                .WhereIf(request.MinPrice.HasValue,x=>x.Price>= request.MinPrice.Value)
                .WhereIf(request.MaxPrice.HasValue,x=>x.Price<= request.MaxPrice.Value)
                ;

            return   query.OrderBy(x=>x.Id).ToPageResult<Product, ProductFullResponse>(request, _crudServices.Mapper);
             
        }
    }
}
