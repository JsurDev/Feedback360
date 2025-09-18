using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CapaPresentacionRestaurante.Models
{
    public class CrearCliente
    {
        public int IdCliente { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Telefono { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public bool Restablecer { get; set; }
        public bool Estatus { get; set; }
        public string ConfirmarPassword { get; set; }

    }
}