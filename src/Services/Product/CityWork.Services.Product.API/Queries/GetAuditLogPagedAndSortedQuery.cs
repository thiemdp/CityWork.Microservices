using CityWork.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;


namespace CityWork.Services.Product.API
{
    public class GetAuditLogPagedAndSortedQuery : PagedAndSortedRequest, IShouldNormalize, IRequest<PagedResult<AuditLogFullResponse>>
    {
        public string? KeyWords { get; set; } = default!;
        public string? Action { get; set; }
        public void Normalize()
        {
        }
    }
    public class GetAuditLogPagedAndSortedQueryHander : IRequestHandler<GetAuditLogPagedAndSortedQuery, PagedResult<AuditLogFullResponse>>
    {
        private readonly ICityWorkRestClient _restClient;
        
        public GetAuditLogPagedAndSortedQueryHander(ICityWorkRestClient restClient)
        {
            _restClient = restClient;
        }
        public async Task<PagedResult<AuditLogFullResponse>> Handle(GetAuditLogPagedAndSortedQuery request, CancellationToken cancellationToken)
        {
            string url = $"https://AuditLogApi/api/AuditLogs/Get?KeyWords={request.KeyWords}&Action={request.Action}&PageSize={request.PageSize}&PageNumber={request.PageNumber}";
            var result = await _restClient.GetAsync<PagedResult<AuditLogFullResponse>>(url,cancellationToken);
            return result;
        }
    }
}
