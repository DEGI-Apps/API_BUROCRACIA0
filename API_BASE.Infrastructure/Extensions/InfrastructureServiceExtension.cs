using API_BASE.Application.Interfaces;
using API_BASE.Infrastructure.Repositories;
using API_BASE.Infrastructure.Services;
using Microsoft.Extensions.DependencyInjection;

namespace API_BASE.Infrastructure.Extensions
{
    public static class InfrastructureServiceExtension
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {

            // Repositorio genérico
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

            // Servicios relacionados al contexto HTTP
            services.AddHttpContextAccessor();
            services.AddScoped<ICurrentUserService, CurrentUserService>();
            return services;
        }
    }
}
