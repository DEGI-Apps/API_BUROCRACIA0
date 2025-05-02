
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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Filtro global para borrado logico
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                if (typeof(AuditableEntity).IsAssignableFrom(entityType.ClrType))
                {
                    var method = typeof(ApplicationDbContext)
                        .GetMethod(nameof(SetSoftDeleteFilter), System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static)!
                        .MakeGenericMethod(entityType.ClrType);

                    method.Invoke(null, new object[] { modelBuilder });
                }
            }
            modelBuilder.Entity<AuditableEntity>()
                        .Property(e => e.Borrado)
                        .HasDefaultValue(false);

        }

        private static void SetSoftDeleteFilter<T>(ModelBuilder builder) where T : AuditableEntity
        {
            builder.Entity<T>().HasQueryFilter(e => !e.Borrado);
        }


    }
}
