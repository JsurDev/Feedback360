using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad
{
    public class Usuario
    {
        public int IdUsuario { get; set; }
        public Puesto oPuesto { get; set; } // Esto es un objeto de la clase Puesto.
                                            // Lo agregue para llamar los valores INT de IdPuesto
                                            // y validar acceso en AuthorizedRoleAtribute
        public string Nombre { get; set; }  
        public string Apellido { get; set; }
        public string Telefono { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public bool Estatus { get; set; }
        public bool Restablecer { get; set; }
        public string Error { get; set; }

    }
}
