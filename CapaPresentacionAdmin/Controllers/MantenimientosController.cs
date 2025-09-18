using CapaEntidad;
using CapaNegocio;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ClosedXML.Excel;
using System.IO;
using DocumentFormat.OpenXml.Spreadsheet;
using System;
using CapaPresentacionAdmin.Models;
using CapaPresentacionAdmin.Reportes;
using DocumentFormat.OpenXml.Office2010.Excel;
using Microsoft.Ajax.Utilities;
using CapaPresentacionAdmin.Utilidades;

namespace CapaPresentacionAdmin.Controllers
{
    //Esto lo agrego para que no se pueda acceder a las vistas sin Loguearse
    [Authorize]
    public class MantenimientosController : Controller
    {
        #region
        // Mantenimientos //

        [AuthorizeRole(1,2,3,4)]//Damos acceso al rol que preferimos
        public ActionResult Solicitudes()
        {
            return View();
        }

        [AuthorizeRole(1,3)]//Damos acceso al rol que preferimos
        public ActionResult NotificacionAdmin()
        {
            List<NotificacionAdmin> lista = new CN_NotificacionAdmin().ListarTodasNotificacion()
                .OrderByDescending(n => n.Fecha)
                .ToList();
            return View(lista);
        }//En este caso como es una lista que paso a la vista no necesitaba pasarlo en formato Json como en otros casos.

        [HttpGet]
        public JsonResult ListarNotificaciones()
        {
            List<NotificacionAdmin> oLista = new CN_NotificacionAdmin().ListarNotificacionAdmin()
                .Where(n => n.Fecha >= DateTime.Now.AddHours(-24))
                .ToList();

            // Convertimos cada notificación a un objeto anónimo con fecha en formato ISO
            var resultado = oLista.Select(n => new
            {
                n.IdNotificacionAdmin,
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
                new CN_NotificacionAdmin().MarcarComoLeida(idNotificacion);
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }//Termina Metodo

        [HttpGet]
        public JsonResult ListarSolicitud()
        {
            List<Solicitud> oLista = new List<Solicitud>();

            oLista = new CN_Solicitud().ListarSolicitud();

            return Json(new { data = oLista }, JsonRequestBehavior.AllowGet);
        }

        #endregion

        //BUSCANDO DATOS DEL CLIENTE POR "ID"

        private CN_Clientes servicioDatosCliente = new CN_Clientes();

        [HttpGet]
        public JsonResult ObtenerClientePorId(int idCliente)
        {
            var solicitudes = servicioDatosCliente.ListarClientes();
            var solicitud = solicitudes.FirstOrDefault(s => s.IdCliente == idCliente);

            if (solicitud != null)
            {
                return Json(new { success = true, data = solicitud }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { success = false, message = "No se encontró el cliente con el ID proporcionado."}, JsonRequestBehavior.AllowGet);
            }
        }
        #region
        // Mantenimientos CATEGORIAS//

        [AuthorizeRole(1,3)]
        public ActionResult Categorias()
        {
            return View();
        }

        public JsonResult ListarCategorias()
        {
            List<Categoria> oLista = new List<Categoria>();

            oLista = new CN_Categorias ().ListarCategorias();

            return Json(new { data = oLista }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GuardarCategoria(Categoria obj) /*aqui editamos el usuario*/
        {
            object resultado;
            string mensaje = string.Empty;
            if (obj.IdCategoria == 0)
            {
                resultado = new CN_Categorias().AgregarCategoria(obj, out mensaje);
            }
            else
            {
                resultado = new CN_Categorias().ModificarCategoria(obj, out mensaje);
            }

            return Json(new { resultado = resultado, mensaje = mensaje }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult EliminarCategoria(int IdCat)
        {
            bool respuesta = false;
            string mensaje = string.Empty;
            respuesta = new CN_Categorias().EliminarCategoria(IdCat, out mensaje);
            return Json(new { resultado = respuesta, mensaje = mensaje }, JsonRequestBehavior.AllowGet);
        }
        #endregion



        #region
        //////// CONTROLADORES DE ESTADO//////////

        [AuthorizeRole(1,3)]
        public ActionResult Estado()
        {
            return View();
        }
        public JsonResult ListarEstados()
        {
            List<Estado> oLista = new List<Estado>();

            oLista = new CN_Estados().ListarEstados();

            return Json(new { data = oLista }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GuardarEstado(Estado obj) /*aqui editamos el Estado*/
        {
            object resultado;
            string mensaje = string.Empty;
            if (obj.IdEstado == 0)
            {
                resultado = new CN_Estados().AgregarEstado(obj, out mensaje);
            }
            else
            {
                resultado = new CN_Estados().ModificarEstado(obj, out mensaje);
            }

            return Json(new { resultado = resultado, mensaje = mensaje }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult EliminarEstado(int id)
        {
            bool respuesta = false;
            string mensaje = string.Empty;
            respuesta = new CN_Estados().EliminarEstado(id, out mensaje);
            return Json(new { resultado = respuesta, mensaje = mensaje }, JsonRequestBehavior.AllowGet);
        }
        //////// TERMINAMOS CON LOS CONTROLADORES DE ESTADO//////////

        #endregion

        #region
        // Mantenimientos SOLICITUD
        //[HttpPost]
        //public JsonResult GuardarSolicitud(Solicitud obj)
        //{
        //    object resultado;
        //    string mensaje = string.Empty;
        //    string referenciaGenerada = string.Empty; // <- Declarada fuera

        //    if (obj.IdSolicitud == 0)
        //    {
        //        resultado = new CN_Solicitud().AgregarSolicitud(obj, out mensaje, out referenciaGenerada);
        //    }
        //    else
        //    {
        //        resultado = new CN_Solicitud().ModificarSolicitud(obj, out mensaje);
        //    }
        //    return Json(new { resultado = resultado, mensaje = mensaje, referencia = referenciaGenerada }, JsonRequestBehavior.AllowGet);
        //} //LA COMENTE PUES ACTUALICE EL ENVIO DE CORREOS CUANDO HAY MODIFICACION DE SOLICITUDES. CREE EL CODIGO EN LA LEGION DE CORREOS

        [HttpPost]
        public JsonResult EliminarSolicitud(int IdSol)
        {
            bool respuesta = false;
            string mensaje = string.Empty;
            respuesta = new CN_Solicitud().EliminarSolicitud(IdSol, out mensaje);
            return Json(new { resultado = respuesta, mensaje = mensaje }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        //Exportar EXCEL
        public ActionResult DescargarExcel()
        {
            // Llama directamente a la capa de negocio //Esta devuelve la LISTA Completa de Solicitud , tal cual esta en tabla SQL
            var solicitudes = new CN_Solicitud().ListarSolicitud();
            
            using (var workbook=new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Solicitudes");

                //Llamando a la lista de solicitudes
                worksheet.Cell(1,1).Value= "Id Solicitud";
                worksheet.Cell(1, 2).Value = "Id Cliente";
                worksheet.Cell(1, 3).Value = "Categoria";
                worksheet.Cell(1, 4).Value = "Asignado a";
                worksheet.Cell(1, 5).Value = "Estado";
                worksheet.Cell(1, 6).Value = "Comentario";
                worksheet.Cell(1, 7).Value = "Nombre";
                worksheet.Cell(1, 8).Value = "Apellido";
                worksheet.Cell(1, 9).Value = "Email";
                worksheet.Cell(1, 10).Value = "Telefono";
                worksheet.Cell(1, 11).Value = "Referencia";
                worksheet.Cell(1, 12).Value = "Estatus";

                int fila = 2;

                foreach(var s in solicitudes)
                {
                    worksheet.Cell(fila,1).Value=s.IdSolicitud;
                    worksheet.Cell(fila, 2).Value = s.IdCliente;
                    worksheet.Cell(fila, 3).Value = s.oCategoria.Tipo;
                    worksheet.Cell(fila, 4).Value = s.oUsuario.Nombre;
                    worksheet.Cell(fila, 5).Value = s.oEstado.EstadoActual;
                    worksheet.Cell(fila, 6).Value = s.Comentario;
                    worksheet.Cell(fila, 7).Value = s.Nombre;
                    worksheet.Cell(fila, 8).Value = s.Apellido;
                    worksheet.Cell(fila, 9).Value = s.Email;
                    worksheet.Cell(fila, 10).Value = s.Telefono;
                    worksheet.Cell(fila, 11).Value = s.Referencia;
                    worksheet.Cell(fila, 12).Value = s.Estatus ? "Activo":"Inactivo";
                    fila++;
                }
                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    stream.Position = 0;
                    string nombreArchivo = "Solicitudes.xlsx";
                    return File(stream.ToArray(),
                                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                                nombreArchivo);
                }
            }
        }//Termina Exportar EXCEL

        public ActionResult ExportarEstadoSolicitud()
        {
            var estados = new CN_Estados().ListarEstados();

            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("EstadosSolicitud");

                    //LLamamos a la Lista Solicitud
                    worksheet.Cell(1, 1).Value = "Id Estado";
                    worksheet.Cell(1, 2).Value = "Estad Actual Solicitud";
                    worksheet.Cell(1, 3).Value = "Estado";

                    int fila = 2;
                    foreach (var e in estados)
                    {
                        worksheet.Cell(fila, 1).Value = e.IdEstado;
                        worksheet.Cell(fila, 2).Value = e.EstadoActual;
                        worksheet.Cell(fila, 3).Value = e.Estatus ? "Activo" : "Inactivo";
                        fila++;
                    }

                    using (var stream = new MemoryStream())
                    {
                        workbook.SaveAs(stream);
                        stream.Position = 0;
                        string nombreArchivo = "EstadoSolicitud.xlsx";
                        return File(stream.ToArray(),
                          "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                          nombreArchivo);
                    }
            }
        }//termina el metodo ExportarEstadoSolicitud.


        #region
        //GESTIONAMOS EL ENVIO DE CORREOS

        [HttpPost]
        public JsonResult GuardarSolicitud(Solicitud obj)
        {
            object resultado;
            string mensaje = string.Empty;
            string referenciaGenerada = string.Empty;
            bool enviarCorreo = false;

            // Cargar el estado anterior si es una modificación
            Solicitud solicitudAnterior = null;

            if (obj.IdSolicitud != 0)
            {
                var listaSolicitudes = new CN_Solicitud().ListarSolicitud();
                solicitudAnterior = listaSolicitudes.FirstOrDefault(s => s.IdSolicitud == obj.IdSolicitud);
            }

            // Si es nueva solicitud
            if (obj.IdSolicitud == 0)
            {
                resultado = new CN_Solicitud().AgregarSolicitud(obj, out mensaje, out referenciaGenerada);
                enviarCorreo = true; // Siempre enviar correo al crear
            }
            else
            {
                resultado = new CN_Solicitud().ModificarSolicitud(obj, out mensaje);

                // Enviar correo solo si el estado cambió
                if (solicitudAnterior != null &&
                    solicitudAnterior.oEstado != null &&
                    obj.oEstado != null &&
                    solicitudAnterior.oEstado.IdEstado != obj.oEstado.IdEstado)
                {
                    enviarCorreo = true;
                }
            }
            // Envío del correo
            if (enviarCorreo)
            {
                string destino = obj.Email;
                string asunto = (obj.IdSolicitud == 0)
                ? "Confirmación de registro de su solicitud" 
                : "Actualización del estado de su solicitud";

                string cuerpo = $@"
                 <div style='font-family: Arial, sans-serif; color: #333;'>
                     <h3>Estimado/a {obj.Nombre} {obj.Apellido},</h3>
                     <p>Le informamos que el estado actual de su solicitud ha sido {(obj.IdSolicitud == 0 ? "registrado" : "actualizado")} exitosamente el día <strong>{DateTime.Now:dd/MM/yyyy}</strong> a las <strong>{DateTime.Now:HH:mm}</strong>.</p>
                     
                     <p>A continuación, encontrará los detalles de su solicitud:</p>
                     <ul>
                         <li><strong>Comentario:</strong> {obj.Comentario}</li>
                         <li><strong>Estado actual:</strong> {obj.oEstado?.EstadoActual}</li>
                         <li><strong>Referencia:</strong> {obj.Referencia}</li>
                         <li><strong>Respuesta del equipo:</strong> {obj.Respuesta}</li>
                     </ul>
                 
                     <p>Si tiene alguna consulta adicional, no dude en contactarnos al <strong>2275 8888</strong> o responder a este correo.</p>
                 
                     <br/>
                     <p>Atentamente,<br/>Equipo de Recursos Humanos</p>
                 </div>";


                Correo.EnviarCorreo(destino, asunto, cuerpo);
            }
            return Json(new
            {
                resultado = resultado,
                mensaje = mensaje,
                referencia = referenciaGenerada
            }, JsonRequestBehavior.AllowGet);
        }//Termina el metodo Guardar Solicitud con Envio de Correo incluido

        #endregion

    }
}