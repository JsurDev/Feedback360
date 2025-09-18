using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad
{
    public class Notificaciones
    {
        public int IdNotificacion { get; set; }
        public int IdCliente { get; set; }
        public string Mensaje { get; set; }
        public DateTime Fecha { get; set; }
        public bool Leido { get; set; }
        public string Error { get; set; }
    }
}
