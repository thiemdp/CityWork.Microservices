using CityWork.Domain;
using CityWork.Domain.Entities;
using CityWork.Infrastructure;
using CityWork.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Data;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace CityWork.Infrastructure
{
    public class DbContextUnitOfWork<TDbContext> : DbContext, IUnitOfWork
        where TDbContext : DbContext
    {
        private IDbContextTransaction _dbContextTransaction;
        private readonly IDateTimeProvider _dateTimeProvider;
        private readonly ICurrentUser _currentUser;

        public DbContextUnitOfWork(DbContextOptions<TDbContext> options, IDateTimeProvider dateTimeProvider)
           : base(options)
        {
            _dateTimeProvider = dateTimeProvider;
        }
        public DbContextUnitOfWork(DbContextOptions<TDbContext> options, IDateTimeProvider dateTimeProvider, ICurrentUser currentUser)
            : base(options)
        {
            _dateTimeProvider = dateTimeProvider;
            _currentUser = currentUser;
        }

        public IDisposable BeginTransaction(IsolationLevel isolationLevel = IsolationLevel.ReadCommitted)
        {
            _dbContextTransaction = Database.BeginTransaction(isolationLevel);
            return _dbContextTransaction;
        }

        public async Task<IDisposable> BeginTransactionAsync(IsolationLevel isolationLevel = IsolationLevel.ReadCommitted, CancellationToken cancellationToken = default)
        {
            _dbContextTransaction = await Database.BeginTransactionAsync(isolationLevel, cancellationToken);
            return _dbContextTransaction;
        }

        public void CommitTransaction()
        {
            _dbContextTransaction.Commit();
        }

        public async Task CommitTransactionAsync(CancellationToken cancellationToken = default)
        {
            await _dbContextTransaction.CommitAsync(cancellationToken);
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
 
        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            AddAuditToEntity();
            return base.SaveChanges(acceptAllChangesOnSuccess);
        }

        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
        {
            AddAuditToEntity();
            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }
        
        private void AddAuditToEntity()
        {
            foreach (var entry in this.ChangeTracker.Entries<IEntityAuditFull>())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        if (_currentUser != null && _currentUser.UserId.HasValue)
                            entry.Entity.CreatedBy = _currentUser.UserId.Value.ToString();
                        if (_dateTimeProvider != null)
                            entry.Entity.CreatedOn = _dateTimeProvider.OffsetNow;
                        break;
                    case EntityState.Modified:
                        entry.Property(nameof(entry.Entity.CreatedBy)).IsModified = false;
                        entry.Property(nameof(entry.Entity.CreatedOn)).IsModified = false;
                        if (_currentUser != null && _currentUser.UserId.HasValue)
                            entry.Entity.ModifiedBy = _currentUser.UserId.Value.ToString();
                        if (_dateTimeProvider != null)
                            entry.Entity.ModifiedOn = _dateTimeProvider.OffsetNow;
                        break;
                }
            }
        }
    }
}