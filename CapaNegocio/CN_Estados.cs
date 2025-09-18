using CapaDatos;
using CapaEntidad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaNegocio
{
    public class CN_Estados
    {
        private CD_Estados objCapaDato = new CD_Estados();

        public List<Estado> ListarEstados()
        {
            return objCapaDato.ListarEstados();
        }

        public int AgregarEstado(Estado obj, out string Mensaje)
        {
            Mensaje = string.Empty;
            if (string.IsNullOrEmpty(obj.EstadoActual))
            {
                Mensaje = "El Estado no puede quedar vacio!";
            }
            if (string.IsNullOrEmpty(Mensaje))
            {
                return objCapaDato.AgregarEstado(obj, out Mensaje);
            }
            else
            {
                return 0;
            }
        } /*termina el metodo*/

        public bool ModificarEstado(Estado obj, out string Mensaje)
        {
            Mensaje = string.Empty;
            if (string.IsNullOrEmpty(obj.EstadoActual))
            {
                Mensaje = "El nombre de usuario no puede quedar vacio!";
            }
            if (string.IsNullOrEmpty(Mensaje))
            {
                return objCapaDato.ModificarEstado(obj, out Mensaje);
            }
            else
            {
                return false; //cambie de "0" a "false"
            }
        }/*termina el metodo*/

        public bool EliminarEstado(int IdEstado, out string Mensaje)
        {
            return objCapaDato.EliminarEstado(IdEstado, out Mensaje);
        }
    }
}
