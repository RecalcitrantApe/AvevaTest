﻿using EmployeeApi.Application.Common.Interfaces;
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
                //Debug
                var httpContextAccessor = _httpContextAccessor;
                var context = httpContextAccessor.HttpContext;
                var user = context.User;
                var name = user.FindFirstValue(ClaimTypes.Name);
                return name;
            }
        }


       // public string UserId => _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
    }
}