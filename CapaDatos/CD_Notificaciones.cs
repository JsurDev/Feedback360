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
    public class CD_Notificaciones
    {
        DataSet ds = new DataSet();
        string Patron = ConfigurationManager.AppSettings["Patron"].ToString();


        public List<Notificaciones> ListarNotificaciones()
        {
            List<Notificaciones> lista = new List<Notificaciones>();

            using (SqlConnection oconexion = new SqlConnection(Conexion.cn))
            {
                SqlCommand cmd = new SqlCommand("SP_BuscarNotificaciones", oconexion);
                cmd.CommandType = CommandType.Text;

                oconexion.Open();
                SqlDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    lista.Add(new Notificaciones()
                    {
                        IdNotificacion = Convert.ToInt32(dr["IdNotificacion"]),
                        IdCliente = Convert.ToInt32(dr["IdCliente"]),
                        Mensaje = dr["Mensaje"].ToString(),
                        Fecha = Convert.ToDateTime(dr["Fecha"]),
                        Leido = Convert.ToBoolean(dr["Leido"])
                    });
                }
            }
            return lista;
        }

        public List<Notificaciones> ListarTodasNotificaciones() //Aqui estoy listando todas las notificaciones sin excepciones.
        {
            List<Notificaciones> lista = new List<Notificaciones>();

            using (SqlConnection oconexion = new SqlConnection(Conexion.cn))
            {
                SqlCommand cmd = new SqlCommand("SP_ListarTodasNotificaciones", oconexion);
                cmd.CommandType = CommandType.Text;

                oconexion.Open();
                SqlDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    lista.Add(new Notificaciones()
                    {
                        IdNotificacion = Convert.ToInt32(dr["IdNotificacion"]),
                        IdCliente = Convert.ToInt32(dr["IdCliente"]),
                        Mensaje = dr["Mensaje"].ToString(),
                        Fecha = Convert.ToDateTime(dr["Fecha"]),
                        Leido = Convert.ToBoolean(dr["Leido"])
                    });
                }
            }
            return lista;
        }

        public void MarcarComoLeida(int idNotificacion)
        {
            using (SqlConnection oconexion = new SqlConnection(Conexion.cn))
            {
                SqlCommand cmd = new SqlCommand("UPDATE Notificaciones SET Leido = 1 WHERE IdNotificacion = @Id", oconexion);
                cmd.Parameters.AddWithValue("@Id", idNotificacion);
                oconexion.Open();
                cmd.ExecuteNonQuery();
            }
        }


    }
}
