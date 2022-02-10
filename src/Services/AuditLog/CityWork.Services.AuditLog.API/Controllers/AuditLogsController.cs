using Microsoft.AspNetCore.Mvc;

namespace CityWork.Services.AuditLog.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuditLogsController : ControllerBase
    {
        private readonly ILogger<AuditLogsController> _logger;
        private readonly Dispatcher _dispatcher;
        public AuditLogsController(ILogger<AuditLogsController> logger, Dispatcher dispatcher)
        {
            _logger = logger;
            _dispatcher = dispatcher;
        }

        [HttpGet( "[action]")]
        public async Task<ActionResult<PagedResult<AuditLogFullResponse>>> GetAsync([FromQuery] GetAuditLogPagedAndSortedQuery input)
        {
            var result = await _dispatcher.DispatchAsync(input);
            return result;
        }
    }
}