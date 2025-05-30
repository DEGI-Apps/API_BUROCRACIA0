using API_BASE.Application.Interfaces;
using API_BASE.Application.Interfaces.UserExternal;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API_BASE.Infrastructure.Services
{
    public  class CurrentUserService : ICurrentUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CurrentUserService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public string? UserName =>
            _httpContextAccessor.HttpContext?.User?.Identity?.Name;
    }
}
