namespace CityWork.Services.Product.API
{
    public class ProductFullResponse:AggregateRoot
    {
        public string Code { get; set; } = default!;
        public string Name { get; set; } = default!;
        public string? Description { get; set; }
        public double Price { get; set; }
        public int Quantity { get; set; }
    }
}
