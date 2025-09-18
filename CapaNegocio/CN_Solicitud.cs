using System.Collections.Generic;
using CapaDatos;
using CapaEntidad;

namespace CapaNegocio
{
    public class CN_Solicitud
    {
        private CD_Solicitud objCapaDato = new CD_Solicitud();

        public List<Solicitud> ListarSolicitud()
        {
            return objCapaDato.ListarSolicitud();
        }

        public int AgregarSolicitud(Solicitud obj, out string Mensaje, out string ReferenciaGenerada)
        {
            Mensaje = string.Empty;
            ReferenciaGenerada = string.Empty;

            if (string.IsNullOrEmpty(obj.Nombre))
            {
                Mensaje = "El nombre de usuario no puede quedar vacio!";
            }
            else if (string.IsNullOrEmpty(obj.Apellido))
            {
                Mensaje = "El apellido de usuario no puede quedar vacio!";
            }
            else if (string.IsNullOrEmpty(obj.Email))
            {
                Mensaje = "El correo de usuario no puede quedar vacio!";
            }

            if (string.IsNullOrEmpty(Mensaje))
            {
                return objCapaDato.AgregarSolicitud(obj, out Mensaje, out ReferenciaGenerada);
            }
            else
            {
                return 0;
            }
        }//Termina el metodo


        public bool ModificarSolicitud(Solicitud obj, out string Mensaje) //cambie de "int" a "bool"
        {
            Mensaje = string.Empty;
            if (string.IsNullOrEmpty(obj.Nombre))
            {
                Mensaje = "El nombre de usuario no puede quedar vacio!";
            }
            else if (string.IsNullOrEmpty(obj.Apellido))
            {
                Mensaje = "El apellido de usuario no puede quedar vacio!";
            }
            else if (string.IsNullOrEmpty(obj.Email))
            {
                Mensaje = "El correo de usuario no puede quedar vacio!";
            }
            if (string.IsNullOrEmpty(Mensaje))
            {
                return objCapaDato.ModificarSolicitud(obj, out Mensaje);
            }
            else
            {
                return false;
            }
        }

        public bool EliminarSolicitud(int IdSol, out string Mensaje)
        {
            return objCapaDato.EliminarSolicitud(IdSol, out Mensaje);
        }
    }
}
