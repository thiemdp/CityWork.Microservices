using System;
using System.ComponentModel.DataAnnotations;

namespace CityWork.Domain.Entities
{
    public class AggregateRoot : AggregateRoot<int>, IAggregateRoot
    {
        
    }

    public class AggregateRoot<TPrimaryKey> : Entity<TPrimaryKey>, IAggregateRoot<TPrimaryKey>, IEntityAuditFull
    {
        public virtual DateTimeOffset CreatedOn { get; set; }
        [MaxLength(50)]
        public virtual string? CreatedBy { get; set; }
        public virtual DateTimeOffset? ModifiedOn { get; set; }
        [MaxLength(50)]
        public virtual string? ModifiedBy { get; set; }  
    }
}