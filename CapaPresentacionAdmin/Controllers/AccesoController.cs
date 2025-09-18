using System.Web.Mvc;
using CapaEntidad;// Referencia a la capa de entidad donde tengo datos de BD
using CapaNegocio;// Referencia a la capa de negocio donde tengo los metodos para acceder a la BD
using System.Web.Security;// Referencia a la capa de seguridad para el manejo de usuarios y roles

namespace CapaPresentacionAdmin.Controllers
{
    public class AccesoController : Controller
    {
        // GET: Acceso
        public ActionResult LogIn()
        {
            return View();
        }
        public ActionResult RestablecerPassword()
        {
            return View();
        }
        public ActionResult AccesoDenegado()
        {
            return View();
        }
        public ActionResult HotelesServicios()
        {
            return View();
        }

        [HttpPost]
        public ActionResult LogIn(string email, string contrasena) 
        { 
            Usuario oUsuario = new Usuario();
            oUsuario = new CN_Usuarios().LoginUsuario(email, contrasena);
            if (oUsuario == null || oUsuario.IdUsuario <= 0)
            {
                ViewBag.Error = " Los datos no son correctos.  ";
                ViewBag.Email = email; // Guardamos el valor para que no se borre. El usuario vera el mismo correo y solo debe introducir una contraseña
                return View();
            }
            else
            {
                FormsAuthentication.SetAuthCookie(oUsuario.Email, false); // Guardamos el usuario en la cookie de autenticacion
                ViewBag.Error = null;
                Session["Usuario"] = oUsuario;
                return RedirectToAction("Index", "Home");
            }
        }//TERMINA EL METODO

        //Creando metodo para cerrar sesion
        public ActionResult LogOut()
        {
            Session["Usuario"] = null; // Limpiamos la sesion del cliente
            Session.Clear(); // Limpiamos la sesion
            Session.Abandon(); // Abandonamos la sesion
            FormsAuthentication.SignOut(); // Cerramos la sesion de autenticacion   
            return RedirectToAction("LogIn", "Acceso"); // Redirigimos a la vista de log in
        }

    }
}