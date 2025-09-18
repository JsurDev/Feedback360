using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad
{
    public class Cliente
    {
        public int IdCliente { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Telefono { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        //public string ConfirmarPassword { get; set; }
        public bool Estatus { get; set; }
        public bool Restablecer { get; set; }
        public string Error { get; set; }
    }
}
