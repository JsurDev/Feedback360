using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad
{
    public class Solicitud
    {
        //public Habitaciones oHabitaciones { get; set; }//public int IdHabitaciones { get; set; } //IdHabitaciones
        public int IdSolicitud { get; set; } 
        public int IdCliente { get; set; }
        public Categoria oCategoria { get; set; }
        public Usuario oUsuario { get; set; } 
        public Estado oEstado { get; set; }//Esto es de la tabla de Estado. El que mandare por correo
        public string Comentario { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Email { get; set; }
        public string Telefono { get; set; }
        public string Referencia { get; set; } //para manejar la fecha de la solicitud
        public bool Estatus { get; set; } //para manejar el estado de la solicitud : ACTIVO O INACTIVO
        public string Respuesta { get; set; } //para manejar la respuesta de la solicitud   
        public string Error { get; set; } //para manejar errores    
    }
}
