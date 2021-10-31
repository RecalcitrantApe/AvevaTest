using EmployeeApi.Application.Common.Interfaces;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace EmployeeApi.Web.Services
{
    public class ClaimsCurrentUserService : ICurrentUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ClaimsCurrentUserService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public string UserId 
        { 
            get
            {
                var httpContextAccessor = _httpContextAccessor;
                var context = httpContextAccessor.HttpContext;
                var name = context.User is not null ? context.User.FindFirstValue(ClaimTypes.Name) : "Seed";
                return name;
            }
        }

    }
}
