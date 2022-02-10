using CityWork.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;


namespace CityWork.Services.AuditLog.API
{
    public class GetAuditLogPagedAndSortedQuery : PagedAndSortedRequest, IShouldNormalize, IRequest<PagedResult<AuditLogFullResponse>>
    {
        public string? KeyWords { get; set; } = default!;
        public string? Action { get; set; }  
        public void Normalize()
        {
        }
    }
    public class GetAuditLogPagedAndSortedQueryHander : IRequestHandler<GetAuditLogPagedAndSortedQuery, PagedResult <AuditLogFullResponse>>
    {
        private readonly ICRUDServices<AuditLog,Guid> _crudServices;
       
        public GetAuditLogPagedAndSortedQueryHander(ICRUDServices<AuditLog, Guid> crudServices )
        {
            _crudServices = crudServices;
         
        }
        public async Task<PagedResult<AuditLogFullResponse>> Handle(GetAuditLogPagedAndSortedQuery request, CancellationToken cancellationToken)
        {
                IQueryable<AuditLog> query =   _crudServices.Repository.GetAll()
                      .WhereIf(request.KeyWords.HasValue(), x => x.Description.Contains(request.KeyWords))
                      .WhereIf(request.Action.HasValue(), x => x.Action == request.Action);
                return await query.OrderBy(x => x.EventTime).ToPageResultAsync<AuditLog, AuditLogFullResponse>(request, _crudServices.Mapper);
        }
    }
}
