using System;
using System.Collections.Generic;
using System.Text;

namespace CityWork.Domain.Entities
{
    public abstract class PagedAndSortedRequest {
        public int PageSize { get; set; } = 25;
        public int PageNumber { get; set; } = 1;
        public virtual string? Sorting { get; set; }
    }

    public interface IShouldNormalize
    {
        void Normalize();
    }
}
