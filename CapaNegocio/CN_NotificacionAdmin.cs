using CapaDatos;
using CapaEntidad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaNegocio
{
    public class CN_NotificacionAdmin
    {
        private CP_NotificacionAdmin objCapaDato = new CP_NotificacionAdmin();
        public List<NotificacionAdmin> ListarNotificacionAdmin()//todas las no leidas
        {
            return objCapaDato.ListarNotificacionAdmin();
        }

        public void MarcarComoLeida(int idNotificacion)
        {
            objCapaDato.MarcarComoLeida(idNotificacion);
        }

        public List<NotificacionAdmin> ListarTodasNotificacion()//Todas sin excepciones
        {
            return objCapaDato.ListarTodasNotificacion();
        }
    }
}
