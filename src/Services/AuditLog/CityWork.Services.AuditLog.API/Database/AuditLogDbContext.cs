using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace CityWork.Services.AuditLog.API
{
    public class AuditLogDbContext : DbContextUnitOfWork<AuditLogDbContext>
    {
        public AuditLogDbContext(DbContextOptions<AuditLogDbContext> options,IDateTimeProvider dateTimeProvider)
            : base(options,dateTimeProvider)
        {
        }

        public AuditLogDbContext(DbContextOptions<AuditLogDbContext> options, IDateTimeProvider dateTimeProvider,ICurrentUser currentUser)
            : base(options, dateTimeProvider, currentUser)
        {
        }
        public DbSet<AuditLog> AuditLogs { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            //builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            builder.Entity<AuditLog>().HasIndex(x => x.EventTime);
        }
    }
}
