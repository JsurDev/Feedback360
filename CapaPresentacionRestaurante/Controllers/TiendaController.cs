using CapaEntidad;// Referencia a la capa de entidad donde tengo datos de BD
using CapaNegocio;// Referencia a la capa de negocio donde tengo los metodos para acceder a la BD
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CapaPresentacionRestaurante.Controllers
{
    public class TiendaController : Controller
    {
        // GET: Tienda
        public ActionResult Index()
        {
            //return View();
            List<Room> listaProductos = objNegocio.ListarRoom(); // obtiene los productos
            return View(listaProductos); // se los pasa a la vista
        }

        [Authorize]
        public ActionResult MisCompras()
        {
            return View();
        }

        [Authorize]
        public ActionResult Notificaciones()
        {
            var cliente = Session["Cliente"] as Cliente;

            if (cliente == null)
            {
                return RedirectToAction("Login", "Acceso");
            }

            int idCliente = cliente.IdCliente;

            // Llamando al método correcto que obtiene TODAS las notificaciones
            List<Notificaciones> lista = new CN_Notificaciones().ListarTodasNotificaciones()
                .Where(n => n.IdCliente == idCliente)
                .OrderByDescending(n => n.Fecha)
                .ToList();

            return View(lista);
        }//En este caso como es una lista que paso a la vista no necesitaba pasarlo en formato Json como en otros casos.


        [Authorize]
        public ActionResult QuejaSugerencia()
        {
            return View();
        }


        private CN_Room objNegocio = new CN_Room();

        [HttpGet]
        public JsonResult MostrarMisRoom()
        {
            List<Room> listaProductos = objNegocio.ListarRoom(); // Obtiene los productos desde la base de datos
            return Json(new { data = listaProductos },JsonRequestBehavior.AllowGet); // Pasa la lista a la vista
        }

        [HttpPost]
        public JsonResult ListarRoom(Room obj)
        {
            List<Room> listaProductos = objNegocio.ListarRoom(); // este es suficiente
            var jsonResult = Json(new { data = listaProductos }, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        [HttpGet]
        public JsonResult ListarSolicitud()
        {
            // Verificamos si el cliente ha iniciado sesión
            var cliente = Session["Cliente"] as Cliente;

            if (cliente == null)
            {
                // Retorna lista vacía o un error si no hay sesión activa
                return Json(new { data = new List<Solicitud>() }, JsonRequestBehavior.AllowGet);
            }

            // Solo mostrar solicitudes del cliente autenticado
            int idCliente = cliente.IdCliente;

            List<Solicitud> oLista = new CN_Solicitud().ListarSolicitud()
                .Where(s => s.IdCliente == idCliente) // filtrar por cliente
                .ToList();

            return Json(new { data = oLista }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult ListarCategorias()
        {
            List<Categoria> oLista = new List<Categoria>();

            oLista = new CN_Categorias().ListarCategorias();

            return Json(new { data = oLista }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult ListarEstados()
        {
            List<Estado> oLista = new List<Estado>();

            oLista = new CN_Estados().ListarEstados();

            return Json(new { data = oLista }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult ListarUsuarios()
        {
            List<Usuario> oLista = new List<Usuario>();
            oLista = new CN_Usuarios().ListarUsuarios();
            return Json(new { data = oLista }, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public JsonResult ListarNotificaciones()
        {
            var cliente = Session["Cliente"] as Cliente;

            if (cliente == null)
            {
                return Json(new { data = new List<object>() }, JsonRequestBehavior.AllowGet);
            }

            int idCliente = cliente.IdCliente;

            List<Notificaciones> oLista = new CN_Notificaciones().ListarNotificaciones()
                .Where(n => n.IdCliente == idCliente && n.Fecha >= DateTime.Now.AddHours(-24))
                .ToList();

            // Convertimos cada notificación a un objeto anónimo con fecha en formato ISO
            var resultado = oLista.Select(n => new
            {
                n.IdNotificacion,
                n.IdCliente,
                n.Mensaje,
                Fecha = n.Fecha.ToString("yyyy-MM-ddTHH:mm:ss.fffZ"), // ISO 8601
                n.Leido
            });

            return Json(new { data = resultado }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult MarcarNotificacionLeida(int idNotificacion)
        {
            try
            {
                new CN_Notificaciones().MarcarComoLeida(idNotificacion);
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }//Termina Metodo


        //MANTENIMIENTO SOLICITUDE CLIENTE
        [HttpPost]
        public JsonResult GuardarSolicitud(Solicitud obj)
        {
            object resultado;
            string mensaje = string.Empty;
            string referenciaGenerada = string.Empty; // <- Declarada fuera

            if (obj.IdSolicitud == 0)
            {
                resultado = new CN_Solicitud().AgregarSolicitud(obj, out mensaje, out referenciaGenerada);
            }
            else
            {
                resultado = new CN_Solicitud().ModificarSolicitud(obj, out mensaje);
            }

            return Json(new { resultado = resultado, mensaje = mensaje, referencia = referenciaGenerada }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult ObtenerDatosCliente()
        {
            var cliente = Session["Cliente"] as Cliente;

            if (cliente == null)
            {
                return Json(new { success = false, message = "Sesión expirada" }, JsonRequestBehavior.AllowGet);
            }

            var datos = new
            {
                success = true,
                cliente.IdCliente,
                cliente.Nombre,
                cliente.Apellido,
                cliente.Telefono,
                cliente.Email
            };

            return Json(datos, JsonRequestBehavior.AllowGet);
        }






    }
}