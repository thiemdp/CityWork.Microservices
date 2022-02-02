using System;
using System.Collections.Generic;
using System.Text;

namespace CityWork.Domain.Entities
{
  public  interface IEntityAuditFull
    {
          DateTimeOffset CreatedOn { get; set; }
          string CreatedBy { get; set; }
          DateTimeOffset? ModifiedOn { get; set; }
          string ModifiedBy { get; set; }
    }
}
