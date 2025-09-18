using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CapaDatos;
using CapaEntidad;

namespace CapaNegocio
{
    public class CN_Categorias
    {
        private CD_Categorias objCapaDato = new CD_Categorias();
        public List<Categoria> ListarCategorias()
        {
            return objCapaDato.ListarCategorias();
        }
        public int AgregarCategoria(Categoria obj, out string Mensaje)
        {
            Mensaje = string.Empty;
            if (string.IsNullOrEmpty(obj.Tipo))
            {
                Mensaje = "El nombre de la Categoria no puede quedar vacio!";
            }
            if (string.IsNullOrEmpty(Mensaje))
            {
                return objCapaDato.AgregarCategoria(obj, out Mensaje);
            }
            else
            {
                return 0;
            }
        }

        public bool ModificarCategoria(Categoria obj, out string Mensaje)
        {
            Mensaje = string.Empty;
            if (string.IsNullOrEmpty(obj.Tipo))
            {
                Mensaje = "El nombre de Categoria no puede quedar vacio!";
            }
            if (string.IsNullOrEmpty(Mensaje))
            {
                return objCapaDato.ModificarCategoria(obj, out Mensaje);
            }
            else
            {
                return false; //cambie de "0" a "false"
            }
        }/*termina el metodo*/

        public bool EliminarCategoria(int IdCat, out string Mensaje)
        {
            return objCapaDato.EliminarCategoria(IdCat, out Mensaje);
        }
    }
}
