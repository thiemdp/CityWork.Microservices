using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace CityWork.Services.Product.API
{
    public class ProductDbContext : DbContextUnitOfWork<ProductDbContext>
    {
        public ProductDbContext(DbContextOptions<ProductDbContext> options,IDateTimeProvider dateTimeProvider)
            : base(options,dateTimeProvider)
        {
        }

        public ProductDbContext(DbContextOptions<ProductDbContext> options, IDateTimeProvider dateTimeProvider,ICurrentUser currentUser)
            : base(options, dateTimeProvider, currentUser)
        {
        }
        public DbSet<Product> Products { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            //builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}
