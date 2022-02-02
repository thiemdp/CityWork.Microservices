namespace CityWork.Services.Product.API
{
    public class GetProductByIdQuery:Entity,IRequest<ProductFullResponse>
    {
    }
    public class GetProductByIdQueryHander : IRequestHandler<GetProductByIdQuery, ProductFullResponse>
    {
        private readonly ICRUDServices<Product> _crudServices;
        public GetProductByIdQueryHander(ICRUDServices<Product> crudServices)
        {
            _crudServices = crudServices;
        }
        public async Task<ProductFullResponse> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
        {
            var product = await _crudServices.GetByIdAsync(request.Id);
            if (product is null)
                throw new Exception("Product not found!");
            return _crudServices.Mapper.Map<ProductFullResponse>(product);
        }
    }
}
