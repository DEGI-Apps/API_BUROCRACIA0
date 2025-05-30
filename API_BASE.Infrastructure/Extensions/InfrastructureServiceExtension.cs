using API_BASE.Application.Interfaces;
using API_BASE.Application.Interfaces.UserExternal;
using API_BASE.Infrastructure.Persistence;
using API_BASE.Infrastructure.Persistence.Shared;
using API_BASE.Infrastructure.Repositories;
using API_BASE.Infrastructure.Services;
using AutoMapper.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace API_BASE.Infrastructure.Extensions
{
    public static class InfrastructureServiceExtension
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {

            // Repositorio genérico
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

            //agregar referecnias a dbconext de tabla compartidas
           // services.AddDbContext<SharedDbContext>(options =>
           // options.UseSqlServer(configuration.GetConnectionString("SharedConnection")));

            // Servicios relacionados al contexto HTTP
            services.AddHttpContextAccessor();
            services.AddScoped<ICurrentUserService, CurrentUserService>();
            return services;
        }
    }
}
