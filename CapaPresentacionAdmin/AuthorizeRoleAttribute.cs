using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Web;
using System.Web.Mvc;
using CapaEntidad; 
namespace CapaPresentacionAdmin
{
    public class AuthorizeRoleAttribute : AuthorizeAttribute
    {
        private readonly int[] _allowedRoles;
        //Este arreglo contiene los IDs de roles o puestos permitidos (Ej: 1 = Administrador, 2 = Gerente, etc.)

        //El params permite pasar varios roles como argumentos al atributo.
        public AuthorizeRoleAttribute(params int[] roles)
        {
            _allowedRoles = roles;
        }
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            var usuario = (Usuario)HttpContext.Current.Session["Usuario"];//Se ejecuta antes de la acción del controlador
            if (usuario == null || !_allowedRoles.Contains(usuario.oPuesto.IdPuesto))//obtenemos el Id de session
            {
                // Redirige a vista de acceso denegado
                //filterContext.Result = new RedirectResult(href = "@Url.Action("Index","Home")");
                // Redirige a una vista de acceso denegado o
                var urlHelper = new UrlHelper(filterContext.RequestContext);
                string url = urlHelper.Action("AccesoDenegado", "Acceso"); // Cree la vista para redirigir
                filterContext.Result = new RedirectResult(url);
            }
        }
    }
}