using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CapaEntidad;
using CapaNegocio;
using CapaPresentacionAdmin.Models;
using CapaPresentacionAdmin.Reportes;
using ClosedXML.Excel;
using DocumentFormat.OpenXml.Office2010.Excel;


namespace CapaPresentacionAdmin.Controllers
{
    //Esto lo agrego para que no se pueda acceder a las vistas sin Loguearse
    [Authorize]
    public class HomeController : Controller
    {
        [AuthorizeRole(1,2,3,4)]
        public ActionResult Index()
        {
            return View();
        }
        #region

        [AuthorizeRole(1,3)]
        public ActionResult Usuarios()
        {
            return View();
        }

        [HttpGet]
        public JsonResult ListarPuesto()
        {
            List<Puesto> oLista = new List<Puesto>();
            oLista = new CN_Puesto().ListarPuesto();
            return Json(new { data = oLista }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet] 
        public JsonResult ListarUsuarios()
        {
            List<Usuario> oLista= new List<Usuario>();
            oLista = new CN_Usuarios().ListarUsuarios();
            return Json(new { data=oLista},JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GuardarUsuario(Usuario obj) /*aqui editamos el usuario*/
        {
            object resultado;
            string mensaje = string.Empty;
            if (obj.IdUsuario == 0)
            {
                resultado = new CN_Usuarios().AgregarUsuario(obj, out mensaje);
            }
            else
            {
                resultado = new CN_Usuarios().ModificarUsuario(obj, out mensaje);
            }

            return Json(new { resultado = resultado, mensaje = mensaje }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult EliminarUsuario(int IdUser)
        {
            bool respuesta = false;
            string mensaje = string.Empty;
            respuesta = new CN_Usuarios().EliminarUsuario(IdUser, out mensaje);
            return Json(new { resultado = respuesta, mensaje = mensaje }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region

        [AuthorizeRole(1,3)]
        public ActionResult Clientes()
        {
            return View();
        }

        [HttpGet]
        public JsonResult ListarClientes()
        {
            List<Cliente> oLista = new List<Cliente>();

            oLista = new CN_Clientes().ListarClientes();

            return Json(new { data = oLista }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GuardarCliente(Cliente obj) /*aqui editamos el usuario*/
        {
            object resultado;
            string mensaje = string.Empty;
            if (obj.IdCliente == 0)
            {
                resultado = new CN_Clientes().AgregarCliente(obj, out mensaje);
            }
            else
            {
                resultado = new CN_Clientes().ModificarCliente(obj, out mensaje);
            }

            return Json(new { resultado = resultado, mensaje = mensaje }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult EliminarCliente(int id)
        {
            bool respuesta = false;
            string mensaje = string.Empty;
            respuesta = new CN_Clientes().EliminarCliente(id, out mensaje);
            return Json(new { resultado = respuesta, mensaje = mensaje }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region
        [AuthorizeRole(1,2,3,4)]
        public ActionResult BusquedaGrafico()
        {
            return View();
        }

        [HttpGet]
        public JsonResult listaReporte(string fechaInicio, string fechaFinal, string referenciaSolicitud)
        {
            List<Reporte> oLista = new List<Reporte>();
            oLista = new CN_BusquedaGrafico().Solicitudes(fechaInicio, fechaFinal, referenciaSolicitud);
            return Json(new { data = oLista }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult VistaBusquedaGrafico()//SOLO SON TOTALES
        {
            BusquedaGrafico objeto = new CN_BusquedaGrafico().VerBusqueda();
            return Json(new { resultado = objeto }, JsonRequestBehavior.AllowGet);
        }
        #endregion




        #region 

        //EXPORTANDO A EXCEL //Esta parte de REPORTES EXCEL tiene Diseño de tabla con formato para filtrar. 
        [HttpPost]
        public FileResult ExportarSolicitud(string fechaInicio, string fechaFinal, string referenciaSolicitud, XLWorkbook xLWorkbook)
        {
            List<Reporte> oLista = new List<Reporte>();
            oLista = new CN_BusquedaGrafico().Solicitudes(fechaInicio, fechaFinal, referenciaSolicitud);
            DataTable dataTable = new DataTable();
            dataTable.Locale = new System.Globalization.CultureInfo("es-SV");
            dataTable.Columns.Add("Fecha", typeof(string));
            dataTable.Columns.Add("Clientes", typeof(string));
            dataTable.Columns.Add("Tipo Solicitud", typeof(string));
            dataTable.Columns.Add("Referencia", typeof(string));

            foreach (Reporte reporte in oLista)
            {
                dataTable.Rows.Add(new object[]
                {
                    reporte.Fecha,
                    reporte.Clientes,
                    reporte.IdCategoria,
                    reporte.Referencia
                });
            }
            dataTable.TableName = "ReportesCompletos";
            using (XLWorkbook wb = new XLWorkbook())
            {
                var hoja = wb.Worksheets.Add(dataTable);

                // Obtener el rango con datos
                var rangoConDatos = hoja.RangeUsed();

                // Encabezados: negrita, fondo azul claro, texto centrado
                var encabezados = rangoConDatos.FirstRow();
                encabezados.Style.Font.Bold = true;
                encabezados.Style.Fill.BackgroundColor = XLColor.TealBlue;
                encabezados.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

                // Bordes a todo el rango de datos
                rangoConDatos.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                rangoConDatos.Style.Border.InsideBorder = XLBorderStyleValues.Thin;

                // Autoajustar el ancho de las columnas
                hoja.Columns().AdjustToContents();

                using (System.IO.MemoryStream stream = new System.IO.MemoryStream())
                {
                    wb.SaveAs(stream);
                    string nombreArchivo = "ReporteSolicitudes_" + DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".xlsx";
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", nombreArchivo);
                }
            }
        }
        //TERMINA EXPORTANDO A EXCEL

        //Exportar Clientes a EXCEL.
        public ActionResult ExportarUsuariosExcel() //Declaramos el metodo
        {
            //llamamos a la capa CN_Usuarios para obtener una
            //lista completa de usuarios desde la baase de datos
            var usuarios = new CN_Usuarios().ListarUsuarios();

            //Creamos un nuevo libro de trabajo sin EXCEL instalado
            using (var workbook = new XLWorkbook())
            {
                //agregamos una hoja Excel de nombre "Usuarios_Feedback_GardenHotelSV"
                var ws = workbook.Worksheets.Add("Usuarios_Feedback_GardenHotelSV");

                //Aqui estoy declarando Excabezados
                ws.Cell(1, 1).Value = "Id Usuario";
                ws.Cell(1, 2).Value = "Nombre";
                ws.Cell(1, 3).Value = "Apellido";
                ws.Cell(1, 4).Value = "Puesto";
                ws.Cell(1, 5).Value = "Telefono";
                ws.Cell(1, 6).Value = "Email";
                ws.Cell(1, 7).Value = "Contraseña";
                ws.Cell(1, 8).Value = "Estatus";
                ws.Cell(1, 9).Value = "Restablecer";

                //Aqui estoy llenando el excel con los datos de la lista. FILA 2
                int fila = 2;
                foreach(var u in usuarios)
                {
                    ws.Cell(fila, 1).Value = u.IdUsuario;
                    ws.Cell(fila, 2).Value = u.Nombre;
                    ws.Cell(fila, 3).Value = u.Apellido;
                    ws.Cell(fila, 4).Value=u.oPuesto.NombrePuesto ?? ""; //?? "" significa que si es NULL se pondra vacio
                    ws.Cell(fila, 5).Value = u.Telefono;
                    ws.Cell(fila, 6).Value = u.Email;
                    ws.Cell(fila, 7).Value = u.Password;
                    ws.Cell(fila, 8).Value = u.Estatus ? "Activo" : "Inactivo";
                    ws.Cell(fila, 9).Value = u.Restablecer ? "Si" : "No";
                    fila++; //se incrementa fila por fila para pasar a la siguiente
                }

                //Formato a la tabla
                var rangoTabla = ws.Range(1, 1, fila - 1, 9);
                //fila 1 y columna 1 : A1 celda
                //Luego fila-1 debido a fila++. Caso contrario me mostrara una fila vacia.


                var tabla = rangoTabla.CreateTable();//CreateTable lo convierte en tabla EXCEL con formato dinamico
                tabla.Theme = XLTableTheme.TableStyleMedium9;// aqui aplique el prediseño azul 

                //Aqui doy estilo a las columnas
                ws.Columns().AdjustToContents();//ajustamos el ancho de la columna segun su contenido

                //Descargando el archivo
                using (var stream = new MemoryStream())//creamos un espacio en memoria para Guardarlo temporalmente
                {
                    workbook.SaveAs(stream);//aqui guardamos Excel en stream
                    stream.Position = 0;//Reinicia el puntero del archivo al inicio para leerle desde ahi,sino quedara vacio
                    return File(stream.ToArray(),//Responde con HTTP y provoca descarga automatica//stream.ToArray(), convierte el archivo a bytes
                    "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                    "Usuarios_Feedback_GardenHotelSV.xlsx");
                }
            }
        }//Termina la Descarga de Excel Usuarios

        #endregion

        #region

        //AQUI SI MOSTRARE GRAFICOS

        [AuthorizeRole(1,3, 4)]
        public ActionResult Graficos()
        {
            return View();
        }

        [HttpGet]
        public JsonResult ResporteSolicitudes() //Aqui se llena el reporte de solicitud por mes y Cant
        {
            C_Reportes objetoReporte = new C_Reportes();

            List<ResporteSolicitudes> objLista = objetoReporte.RetornarSolicitudes();

            return Json(objLista, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult ReporteQSK() //Aqui se llena el reporte de los 3 tipos de solicitud Individual
        {
            C_Reportes objetoReporte = new C_Reportes();

            List<ReporteQSK> objLista = objetoReporte.RetornarQSK();

            return Json(objLista, JsonRequestBehavior.AllowGet);
        }
        #endregion

    }
} 