
using API_BASE.Application.Interfaces;
using API_BASE.Application.Interfaces.UserExternal;
using API_BASE.Domain.Base;
using API_BASE.Domain.Entities;
using API_BASE.Infrastructure.Persistence.Configurations;
using Microsoft.AspNetCore.Http;


using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace API_BASE.Infrastructure.Persistence
{
    public class ApplicationDbContext: DbContext
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ICurrentUserService _currentUserService; // Inyeccion de dependencias para obtener el usuario actual

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options,
                                   IHttpContextAccessor httpContextAccessor,
                                   ICurrentUserService currentUserService
            )
           : base(options)
        {
            _httpContextAccessor = httpContextAccessor;
            _currentUserService = currentUserService;
        }

        //Espacio para definir Tablas de la BD



        //Auditoria de tablas automatica
        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var usuario = _currentUserService.UserName ?? "Sistema";
            var ahora = DateTime.UtcNow;

            ActualizarAuditoriaBasica(usuario, ahora);

            var logs = GenerarBitacoraLogs(usuario, ahora);
            if (logs.Any())
                await BitacoraLogs.AddRangeAsync(logs, cancellationToken);

            return await base.SaveChangesAsync(cancellationToken);
        }

        private void ActualizarAuditoriaBasica(string usuario, DateTime ahora)
        {
            foreach (var entry in ChangeTracker.Entries<AuditableEntity>())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.FechaCreacion = ahora;
                        entry.Entity.UsuarioCreacion = usuario;
                        break;

                    case EntityState.Modified:
                        entry.Entity.FechaModificacion = ahora;
                        entry.Entity.UsuarioModificacion = usuario;
                        break;
                }
            }
        }

        private List<BitacoraLog> GenerarBitacoraLogs(string usuario, DateTime ahora)
        {
            var logs = new List<BitacoraLog>();

            var entradas = ChangeTracker.Entries()
                .Where(e =>
                    e.Entity is not BitacoraLog &&
                    e.State is EntityState.Added or EntityState.Modified or EntityState.Deleted);

            foreach (var entry in entradas)
            {
                var entidad = entry.Entity.GetType().Name;
                var accion = entry.State.ToString().ToUpper();
                string detalles = accion switch
                {
                    "ADDED" => SerializarValores(entry.CurrentValues),
                    "MODIFIED" => SerializarCambiosModificados(entry),
                    "DELETED" => SerializarValores(entry.OriginalValues),
                    _ => string.Empty
                };

                logs.Add(new BitacoraLog
                {
                    Usuario = usuario,
                    Fecha = ahora,
                    Accion = accion,
                    Entidad = entidad,
                    Detalles = detalles
                });
            }

            return logs;
        }
        private string SerializarValores(PropertyValues values)
        {
            var dict = values.Properties.ToDictionary(
                p => p.Name,
                p => values[p]?.ToString());

            return System.Text.Json.JsonSerializer.Serialize(dict);
        }

        private string SerializarCambiosModificados(EntityEntry entry)
        {
            var cambios = new Dictionary<string, object?>();

            foreach (var prop in entry.Properties)
            {
                if (prop.IsModified)
                {
                    cambios.Add(prop.Metadata.Name, new
                    {
                        Antes = prop.OriginalValue?.ToString(),
                        Despues = prop.CurrentValue?.ToString()
                    });
                }
            }

            return System.Text.Json.JsonSerializer.Serialize(cambios);
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

            // Configuraciones de AuditoriaLogs
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(BitacoraLogConfiguration).Assembly);

        }

        private static void SetSoftDeleteFilter<T>(ModelBuilder builder) where T : AuditableEntity
        {
            builder.Entity<T>().HasQueryFilter(e => !e.Borrado);
        }

        public DbSet<BitacoraLog> BitacoraLogs => Set<BitacoraLog>();

    }
}
