using CapaEntidad;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaDatos
{
    public class CP_NotificacionAdmin
    {
        DataSet ds = new DataSet();
        string Patron = ConfigurationManager.AppSettings["Patron"].ToString();

        public List<NotificacionAdmin> ListarNotificacionAdmin()//aqui estoy listando las notificaciones no leidas =!1
        {
            List<NotificacionAdmin> lista = new List<NotificacionAdmin>();

            using (SqlConnection oconexion = new SqlConnection(Conexion.cn))
            {
                SqlCommand cmd = new SqlCommand("SP_BuscarNotNoLeidaAdmin", oconexion);
                cmd.CommandType = CommandType.Text;

                oconexion.Open();
                SqlDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    lista.Add(new NotificacionAdmin()
                    {   
                        IdNotificacionAdmin = Convert.ToInt32(dr["IdNotificacionAdmin"]),
                        IdSolicitud = Convert.ToInt32(dr["IdSolicitud"]),
                        IdCliente = Convert.ToInt32(dr["IdCliente"]),
                        IdUsuario = Convert.ToInt32(dr["IdUsuario"]),
                        Mensaje = dr["Mensaje"].ToString(),
                        Fecha = Convert.ToDateTime(dr["Fecha"]),
                        Leido = Convert.ToBoolean(dr["Leido"]),
                        IdCategoria = Convert.ToInt32(dr["IdCategoria"])
                    });
                }
            }
            return lista;
        }

        public List<NotificacionAdmin> ListarTodasNotificacion() //Aqui estoy listando todas las Notificacion sin excepciones.
        {
            List<NotificacionAdmin> lista = new List<NotificacionAdmin>();

            using (SqlConnection oconexion = new SqlConnection(Conexion.cn))
            {
                SqlCommand cmd = new SqlCommand("SP_ListarTodaNotificacionAdmin", oconexion);
                cmd.CommandType = CommandType.Text;

                oconexion.Open();
                SqlDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    lista.Add(new NotificacionAdmin()
                    {
                        IdNotificacionAdmin = Convert.ToInt32(dr["IdNotificacionAdmin"]),
                        IdSolicitud = Convert.ToInt32(dr["IdSolicitud"]),
                        IdCliente = Convert.ToInt32(dr["IdCliente"]),
                        IdUsuario = Convert.ToInt32(dr["IdUsuario"]),
                        Mensaje = dr["Mensaje"].ToString(),
                        Fecha = Convert.ToDateTime(dr["Fecha"]),
                        Leido = Convert.ToBoolean(dr["Leido"]),
                        IdCategoria = Convert.ToInt32(dr["IdCategoria"])
                    });
                }
            }
            return lista;
        }

        public void MarcarComoLeida(int idNotificacion)
        {
            using (SqlConnection oconexion = new SqlConnection(Conexion.cn))
            {
                SqlCommand cmd = new SqlCommand("UPDATE NotificacionAdmin SET Leido = 1 WHERE IdNotificacionAdmin = @Id", oconexion);
                cmd.Parameters.AddWithValue("@Id", idNotificacion);
                oconexion.Open();
                cmd.ExecuteNonQuery();
            }
        }
    }
}
