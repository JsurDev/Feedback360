using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad
{
    public class Estado
    {
       //[IdEstado]
       //[EstadoActual]
        public int IdEstado { get; set; }
        public string EstadoActual { get; set; } //Este es el objeto que cree en Solicitud
        public bool Estatus { get; set; } 
        public string Error { get; set; }  

    }
}
