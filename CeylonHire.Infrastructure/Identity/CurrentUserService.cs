using CeylonHire.Application.Interfaces.IServices;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace CeylonHire.Infrastructure.Identity
{
    public class CurrentUserService : ICurrentUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        public CurrentUserService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public int UserId
        {
            get
            {
                var userIdClaim = _httpContextAccessor.HttpContext?
                    .User?.FindFirst(ClaimTypes.NameIdentifier);

                if (userIdClaim == null)
                    throw new UnauthorizedAccessException("Unauthorized ...");

                return int.Parse(userIdClaim.Value);
            }
        }

        public string? Role
        {
            get
            {
                var userRoleClaim = _httpContextAccessor.HttpContext?
                    .User?.FindFirst(ClaimTypes.Role);

                if (userRoleClaim == null)
                    throw new UnauthorizedAccessException("Unauthorized ...");

                return userRoleClaim.Value;
            }
        }
    }
}
