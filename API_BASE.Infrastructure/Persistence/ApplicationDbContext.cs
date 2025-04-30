
using API_BASE.Domain.Base;
using Microsoft.AspNetCore.Http;


using Microsoft.EntityFrameworkCore;

namespace API_BASE.Infrastructure.Persistence
{
    public class ApplicationDbContext: DbContext
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options,
                                   IHttpContextAccessor httpContextAccessor)
           : base(options)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        //Espacio para definir Tablas de la BD



        //Auditoria de tablas automatica
        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var username = _httpContextAccessor.HttpContext?.User?.Identity?.Name ?? "Sistema";

            foreach (var entry in ChangeTracker.Entries<AuditableEntity>())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.FechaCreacion = DateTime.UtcNow;
                        entry.Entity.UsuarioCreacion = username;
                        break;

                    case EntityState.Modified:
                        entry.Entity.FechaModificacion = DateTime.UtcNow;
                        entry.Entity.UsuarioModificacion = username;
                        break;
                }
            }

            return await base.SaveChangesAsync(cancellationToken);
        }
    }
}
