using API_BASE.Application.Interfaces;
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

        public string? NombreUsuario =>
            _httpContextAccessor.HttpContext?.User?.FindFirst("preferred_username")?.Value
            ?? _httpContextAccessor.HttpContext?.User?.Identity?.Name;
    }
}
