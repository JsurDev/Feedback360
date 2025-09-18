using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad
{
    public class NotificacionAdmin
    {
        public int IdNotificacionAdmin { get; set; }
        public int IdSolicitud { get; set; }
        public int IdUsuario { get; set; }
        public int IdCliente { get; set; }
        public DateTime Fecha { get; set; }
        public string Mensaje { get; set; }
        public bool Leido { get; set; }
        public int IdCategoria { get; set; }

    }
}
