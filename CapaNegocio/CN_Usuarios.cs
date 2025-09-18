using System.Collections.Generic;
using CapaDatos;
using CapaEntidad;

namespace CapaNegocio
{
    public class CN_Usuarios
    {
        private CD_Usuarios objCapaDato = new CD_Usuarios();

        //VALIDANDO EL ACCESO DE USUARIOS
        public Usuario LoginUsuario(string email, string password)
        {
            return objCapaDato.LoginUsuario(email, password);
        }

        public List<Usuario> ListarUsuarios()
        {
            return objCapaDato.ListarUsuarios();
        }
        
        public int AgregarUsuario(Usuario obj, out string Mensaje)
        {
            Mensaje = string.Empty;

            if (string.IsNullOrEmpty(obj.Nombre))
            {
                Mensaje = "El nombre de usuario no puede quedar vacío!";
            }
            else if (string.IsNullOrEmpty(obj.Apellido))
            {
                Mensaje = "El apellido de usuario no puede quedar vacío!";
            }
            else if (string.IsNullOrEmpty(obj.Email))
            {
                Mensaje = "El correo de usuario no puede quedar vacío!";
            }// Si hay un mensaje de error, no continuar
            if (!string.IsNullOrEmpty(Mensaje))
            {
                return 0;
            }// Llamar al método real si todo está bien
            return objCapaDato.AgregarUsuario(obj, out Mensaje);
        }


        public bool ModificarUsuario(Usuario obj, out string Mensaje)
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
                return objCapaDato.ModificarUsuario(obj, out Mensaje);
            }
            else
            {
                return false; //cambie de "0" a "false"
            }
        }/*termina el metodo*/

        public bool EliminarUsuario(int IdUser, out string Mensaje)
        {
            return objCapaDato.EliminarUsuario(IdUser, out Mensaje);
        }


    }
}