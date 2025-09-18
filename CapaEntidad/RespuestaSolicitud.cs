using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad
{
    public class RespuestaSolicitud
    {
        public int IdRespuestasSolicitud { get; set; }
        public string IdSolicitud { get; set; }
        public string IdUsuario { get; set; }
        public string Respuesta { get; set; }

    }
}
