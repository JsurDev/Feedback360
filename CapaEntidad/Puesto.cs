using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad
{
    public class Puesto
    {
        public int IdPuesto { get; set; }
        public string NombrePuesto { get; set; }
        public bool Estado { get; set; }
        public string Error { get; set; }
    }
}
