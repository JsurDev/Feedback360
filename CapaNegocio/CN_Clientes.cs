using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CapaDatos;
using CapaEntidad;

namespace CapaNegocio
{
    public class CN_Clientes
    {
        public CD_Clientes objCapaDato = new CD_Clientes();

        //VALIDANDO EL ACCESO DE CLIENTES
        public Cliente LogInClienteCN(string email, string contrasena)
        {
            return objCapaDato.LogInClienteCN(email, contrasena);
        }
        public List<Cliente> ListarClientes()
        {
            return objCapaDato.ListarClientes();
        }

        public int AgregarCliente(Cliente obj, out string Mensaje)
        {
            Mensaje = string.Empty;
            if (string.IsNullOrEmpty(obj.Nombre))
            {
                Mensaje = "El nombre de Cliente no puede quedar vacio!";
            }
            else if (string.IsNullOrEmpty(obj.Apellido))
            {
                Mensaje = "El apellido de Cliente no puede quedar vacio!";
            }
            else if (string.IsNullOrEmpty(obj.Email))
            {
                Mensaje = "El correo de Cliente no puede quedar vacio!";
            }
            if (string.IsNullOrEmpty(Mensaje))
            {
                return objCapaDato.AgregarCliente(obj, out Mensaje);
            }
            else //Pendinte de ver lo de Password
            {
                return 0;
            }
        } /*termina el metodo*/

        public bool ModificarCliente(Cliente obj, out string Mensaje)
        {
            Mensaje = string.Empty;
            if (string.IsNullOrEmpty(obj.Nombre))
            {
                Mensaje = "El nombre de Cliente no puede quedar vacio!";
            }
            else if (string.IsNullOrEmpty(obj.Apellido))
            {
                Mensaje = "El apellido de Cliente no puede quedar vacio!";
            }
            else if (string.IsNullOrEmpty(obj.Email))
            {
                Mensaje = "El correo de Cliente no puede quedar vacio!";
            }
            if (string.IsNullOrEmpty(Mensaje))
            {
                return objCapaDato.ModificarCliente(obj, out Mensaje);
            }
            else
            {
                return false; //cambie de "0" a "false"
            }
        }/*termina el metodo*/

        public bool EliminarCliente(int IdCliente, out string Mensaje)
        {
            return objCapaDato.EliminarCliente(IdCliente, out Mensaje);
        }

    }
}
