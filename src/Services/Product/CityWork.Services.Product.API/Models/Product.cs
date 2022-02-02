using System.ComponentModel.DataAnnotations;

namespace CityWork.Services.Product.API
{
    public class Product : AggregateRoot
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
}
