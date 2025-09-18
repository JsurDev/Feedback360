using CapaDatos;
using CapaEntidad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaNegocio
{
    public class CN_Notificaciones
    {
        private CD_Notificaciones objCapaDato = new CD_Notificaciones();
        public List<Notificaciones> ListarNotificaciones()
        {
            return objCapaDato.ListarNotificaciones();
        }

        public void MarcarComoLeida(int idNotificacion)
        {
            objCapaDato.MarcarComoLeida(idNotificacion);
        }

        public List<Notificaciones> ListarTodasNotificaciones()
        {
            return objCapaDato.ListarTodasNotificaciones();
        }
    }
}