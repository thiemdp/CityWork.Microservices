using System.ComponentModel.DataAnnotations;

namespace CityWork.Services.Product.API
{
    public class AddUpdateProductCommand : Entity, IRequest<ProductFullResponse>
    {
        [MaxLength(100)]
        [Required]
        public string Code { get; set; } = default!;
        [MaxLength(256)]
        [Required]
        public string Name { get; set; } = default!;
        public string? Description { get; set; }
        public double Price { get; set; }
        public int Quantity { get; set; }
    }

    public class AddUpdateProductCommandHandler : IRequestHandler<AddUpdateProductCommand, ProductFullResponse>
    {
        private readonly ICRUDServices<Product> _crudservices;
        public AddUpdateProductCommandHandler(ICRUDServices<Product> crudservices)
        {
            _crudservices = crudservices;
        }
        public async Task<ProductFullResponse> Handle(AddUpdateProductCommand request, CancellationToken cancellationToken)
        {
            Product product = _crudservices.Mapper.Map<Product>(request);
            await _crudservices.AddOrUpdateAsync(product,cancellationToken);
            return _crudservices.Mapper.Map<ProductFullResponse>(product);
        }
    }
}
