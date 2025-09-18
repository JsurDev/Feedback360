using System;
using System.Collections.Generic;
using CapaEntidad;
using System.Data.SqlClient;
using System.Data;
namespace CapaDatos
{
    public class CD_BusquedaGrafico
    {
        public List<Reporte> Solicitudes(string fechaInicio, string fechaFinal, string referenciaSolicitud)//estoy Usando la Capa Entidad de Reportes donde estoy llamando un SP InnerJoin
        {
            List<Reporte> listaSolicitudes = new List<Reporte>(); //esta lista la llenaremos
            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("SP_ReporteSolicitud", oconexion);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@fechaInicio", fechaInicio);
                    cmd.Parameters.AddWithValue("@fechaFin", fechaFinal);
                    cmd.Parameters.AddWithValue("@referencia", referenciaSolicitud);
                    cmd.CommandType = CommandType.StoredProcedure;
                    oconexion.Open();
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            listaSolicitudes.Add(new Reporte()
                            {
                                Clientes = dr["Clientes"].ToString(),
                                Fecha = dr["Fecha"].ToString(),
                                IdCategoria = dr["IdCategoria"].ToString(),
                                //IdCategoria = Convert.ToInt32(dr["IdCategoria"]),
                                Referencia = dr["Referencia"].ToString()
                            });
                        }
                    }
                }
            } catch {
                listaSolicitudes = new List<Reporte>();
            }

            return listaSolicitudes;//aqui llenamos la lista
        }//termina metodo

        public BusquedaGrafico VerBusqueda()
        {
            BusquedaGrafico objeto = new BusquedaGrafico();
            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("SP_ReporteBusqueda", oconexion);
                    cmd.CommandType = CommandType.StoredProcedure;
                    oconexion.Open();
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            objeto = new BusquedaGrafico()//aqui llenamos el objeto
                            {
                                TotalCliente = Convert.ToInt32(dr["TotalCliente"]),
                                TotalSolicitud = Convert.ToInt32(dr["TotalSolicitud"]),
                                TotalQueja = Convert.ToInt32(dr["TotalQueja"]),
                                TotalSugerencia = Convert.ToInt32(dr["TotalSugerencia"]),
                                TotalFelicitaciones = Convert.ToInt32(dr["TotalFelicitaciones"]),
                            };
                        }
                    }
                }
            }
            catch
            {
                objeto= new BusquedaGrafico();  
            }
                return objeto;//Retorna el objeto con los totales
        }
    }//termina el metodo
}
