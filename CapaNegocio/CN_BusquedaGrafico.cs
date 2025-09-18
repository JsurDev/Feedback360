using CapaDatos;
using CapaEntidad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaNegocio
{
    public class CN_BusquedaGrafico
    {
        private CD_BusquedaGrafico objCapaDato = new CD_BusquedaGrafico();
        public List<Reporte> Solicitudes(string fechaInicio, string fechaFinal, string referenciaSolicitud)
        {
            return objCapaDato.Solicitudes(fechaInicio, fechaFinal, referenciaSolicitud);
        }
        public BusquedaGrafico VerBusqueda()
        {
            return objCapaDato.VerBusqueda();
        }


    }
}
