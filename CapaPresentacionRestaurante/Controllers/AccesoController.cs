using System.Web.Mvc;
using CapaEntidad;// Referencia a la capa de entidad donde tengo datos de BD
using CapaNegocio;// Referencia a la capa de negocio donde tengo los metodos para acceder a la BD
using System.Web.Security;// Referencia a la capa de seguridad para el manejo de usuarios y roles
using CapaPresentacionRestaurante.Models; // Referencia a la capa de presentacion donde tengo los modelos de las vistas
using System.Data.SqlClient; // Referencia para manejar conexiones a la base de datos SQL Server
using System.Data;
using System; // Referencia para manejar operaciones de datos
namespace CapaPresentacionRestaurante.Controllers
{
    public class AccesoController : Controller
    {
        // GET: AccesoCliente
        static string cadena = "Data Source=LAPTOP-G48AD04G;Initial Catalog=Quejas;Integrated Security=True"; // Cadena de conexión a la base de datos

        public ActionResult Index()
        {
            return View();
        }//aqui tengo el LOGIN DE CLIENTE
        public ActionResult RestablecerPassword()
        {
            return View();
        }
        public ActionResult Crear()
        {
            return View();
        }

        [HttpPost]
        public ActionResult RegistrarCliente(CrearCliente oCliente)
        {
            bool registrado;
            string mensaje;
            
            if (oCliente.Password != oCliente.ConfirmarPassword)
            {
                ViewData["Mensaje"] = "Las contraseñas no coinciden. Por favor, intente de nuevo.";
                return View();
            }
            using (SqlConnection cn= new SqlConnection(cadena))
            {
                SqlCommand cmd= new SqlCommand("SP_CrearCuentaCliente", cn);
                cmd.Parameters.AddWithValue("@Nombre", oCliente.Nombre);
                cmd.Parameters.AddWithValue("@Apellido", oCliente.Apellido);
                cmd.Parameters.AddWithValue("@Telefono", oCliente.Telefono);
                cmd.Parameters.AddWithValue("@Email", oCliente.Email);
                cmd.Parameters.AddWithValue("@PasswordTextoPlano", oCliente.Password);
                //cmd.Parameters.AddWithValue("@Restablecer", 1);//Estoy forzando el envio del numero 1
                //cmd.Parameters.AddWithValue("@Estatus", 1);
                cmd.Parameters.Add("@Registrado",SqlDbType.Bit).Direction= ParameterDirection.Output;
                cmd.Parameters.Add("@Mensaje",SqlDbType.VarChar,100).Direction= ParameterDirection.Output;
                cmd.CommandType = CommandType.StoredProcedure;
                cn.Open();
                cmd.ExecuteNonQuery();
                registrado = Convert.ToBoolean(cmd.Parameters["@Registrado"].Value);
                mensaje = cmd.Parameters["@Mensaje"].Value.ToString();
            }
            ViewBag.Mensaje = mensaje;
            if (registrado)
            {
                return RedirectToAction("Index", "Acceso"); // Redirigimos al login si el registro fue exitoso
            }
            else
            {
                return View(oCliente);
            }
        }


        [HttpPost]
        public ActionResult Index(string email ,string contrasena)
        {
            Cliente oCliente = new Cliente();
            oCliente = new CN_Clientes().LogInClienteCN(email, contrasena);
            if (oCliente == null || oCliente.IdCliente <= 0)
            {
                ViewBag.Error = " Los datos no son correctos.  ";
                ViewBag.Email = email; // Guardamos el valor para que no se borre. El Cliente vera el mismo correo y solo debe introducir una contraseña
                return View();
            }
            else
            {
                FormsAuthentication.SetAuthCookie(oCliente.Email, false); // Guardamos el usuario en la cookie de autenticacion
                ViewBag.Error = null;
                Session["Cliente"] = oCliente;
                return RedirectToAction("Index", "Tienda");
            }
        }//TERMINA MI METODO

        //CERRAMOS SESION CLIENTE
        public ActionResult LogOut()
        {

            Session.Clear(); // Limpiamos la sesion
            Session.Abandon(); // Abandonamos la sesion
            FormsAuthentication.SignOut(); // Cerramos la sesion de autenticacion   
            // 👉 Guardamos el cliente en la sesión
            Session["Cliente"] = null;
            return RedirectToAction("Index", "Tienda"); // Redirigimos a la vista de log in
        }
    }
}