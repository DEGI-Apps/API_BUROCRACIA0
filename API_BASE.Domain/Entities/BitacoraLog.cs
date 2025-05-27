using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API_BASE.Domain.Entities
{
    public class BitacoraLog
    {

        public int Id { get; set; }

        public string Usuario { get; set; } = string.Empty;

        public string Accion { get; set; } = string.Empty;

        public string Entidad { get; set; } = string.Empty;

        public string Detalles { get; set; } = string.Empty;

        public DateTime Fecha { get; set; } = DateTime.UtcNow;

    }
}
