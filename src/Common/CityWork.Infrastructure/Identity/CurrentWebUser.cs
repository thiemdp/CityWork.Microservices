using Microsoft.AspNetCore.Http;
using System;
using System.Security.Claims;

namespace CityWork.Infrastructure
{
    public class CurrentWebUser : ICurrentUser
    {
        private readonly IHttpContextAccessor _context;

        public CurrentWebUser(IHttpContextAccessor context)
        {
            _context = context;
        }

        public bool IsAuthenticated
        {
            get
            {
                if (_context.HttpContext?.User != null)
                    return _context.HttpContext.User.Identity.IsAuthenticated;
                return false;
            }
        }

        public Guid? UserId
        {
            get
            {
                var userId = _context?.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value
                    ?? _context?.HttpContext?.User.FindFirst("sub")?.Value;
                if(!string.IsNullOrEmpty(userId))
                    return Guid.Parse(userId);
                return null;
            }
        }
    }
}
