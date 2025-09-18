using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Data;
using CapaPresentacionAdmin.Models;
using DocumentFormat.OpenXml.Bibliography;

namespace CapaPresentacionAdmin.Reportes
{
    public class C_Reportes
    {
        public List<ResporteSolicitudes> RetornarSolicitudes()
        {
            List<ResporteSolicitudes> objLista = new List<ResporteSolicitudes>();
            using (SqlConnection oconexion = new SqlConnection("Data Source =LAPTOP-G48AD04G; Initial Catalog = Quejas; User = 'Quejas'; Password = '123*'"))
            {
                string query = "SP_ReporteTotales";
                SqlCommand cmd = new SqlCommand(query, oconexion);
                cmd.CommandType = CommandType.StoredProcedure;
                oconexion.Open();

                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        objLista.Add(new Models.ResporteSolicitudes()
                        {
                            Mes = dr["Mes"].ToString(),
                            Cantidad = Convert.ToInt32(dr["Cantidad"].ToString()),
                        });
                    }
                }
            }
            return objLista;
        }

        public List<ReporteQSK> RetornarQSK()
        {
            List<ReporteQSK> objLista = new List<ReporteQSK>();
            using (SqlConnection oconexion = new SqlConnection("Data Source=LAPTOP-G48AD04G;Initial Catalog=Quejas;User ID=Quejas;Password=123*"))
            {
                string query = "SP_ReportePorServicio";

                SqlCommand cmd = new SqlCommand(query, oconexion);
                cmd.CommandType = CommandType.StoredProcedure;
                oconexion.Open();

                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        objLista.Add(new Models.ReporteQSK()
                        {
                            TipoSolicitud = dr["TipoSolicitud"].ToString(),
                            Total = Convert.ToInt32(dr["Total"].ToString()),
                        });
                    }
                }
            }
            return objLista;
        }
    }
}