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
    public class CD_Estados
    {
        DataSet ds = new DataSet();
        string Patron = ConfigurationManager.AppSettings["Patron"].ToString();

        public List<Estado> ListarEstados()
        {
            List<Estado> listaEstado = new List<Estado>(); //esta lista la llenaremos
            try
            {
                SqlDataAdapter da = new SqlDataAdapter("SP_BuscarEstado", Conexion.cn);
                da.SelectCommand.CommandType = CommandType.StoredProcedure;
                da.Fill(ds, "Estado");
                //llenamos la lista de usuario devueltos por el SP
                if (ds != null)
                {
                    if (ds.Tables.Count > 0)
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                            {
                                listaEstado.Add(
                                    new Estado()
                                    {   IdEstado = Convert.ToInt32(ds.Tables[0].Rows[i]["IdEstado"]),
                                        EstadoActual = ds.Tables[0].Rows[i]["EstadoActual"].ToString(),
                                        Estatus = Convert.ToBoolean(ds.Tables[0].Rows[i]["Estatus"]),
                                    }
                                );
                            }
                        }
                        else
                        {
                            listaEstado = new List<Estado>();//si no hay datos regresa una lista vacia
                            listaEstado.Add(
                                new Estado()
                                {
                                    IdEstado = -1,
                                    Error = "Sin registros"
                                }
                            );
                        }

                    }
                    else
                    {
                        listaEstado = new List<Estado>();//si no hay datos regresa una lista vacia
                        listaEstado.Add(
                            new Estado()
                            {
                                IdEstado = -1,
                                Error = "Tabla Vacia"
                            }
                        );
                    }
                }
                else
                {
                    listaEstado = new List<Estado>();//si no hay datos regresa una lista vacia
                    listaEstado.Add(
                        new Estado()
                        {
                            IdEstado = -1,
                            Error = "Datos Nulos"
                        }
                    );
                }
            }
            catch (Exception ex)
            {
                listaEstado = new List<Estado>();
                listaEstado.Add(
                    new Estado()
                    {
                        IdEstado = -1,
                        Error = ex.Message
                    }
                 );
            }
            return listaEstado;//aqui llenamos la lista
        }//termina metodo

        public int AgregarEstado(Estado obj, out string Mensaje)
        {
            int IdEstad = 0;
            Mensaje = string.Empty;//que este vacio y no null
            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("SP_AgregarEstado", oconexion);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@EstadoActual", obj.EstadoActual);
                    cmd.Parameters.AddWithValue("@Estatus", obj.Estatus);
                    cmd.Parameters.Add("Resultado", SqlDbType.Int).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("Mensaje", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;
                    oconexion.Open();
                    cmd.ExecuteNonQuery();
                    IdEstad = Convert.ToInt32(cmd.Parameters["Resultado"].Value);
                    Mensaje = cmd.Parameters["Mensaje"].Value.ToString();
                }
            }
            catch (Exception ex)
            {
                IdEstad = 0;
                Mensaje = ex.Message;
            }
            return IdEstad;
        }

        public bool ModificarEstado(Estado obj, out string Mensaje)
        {
            bool resultado = false;
            Mensaje = string.Empty;
            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("SP_ModificarEstado", oconexion);
                    cmd.Parameters.AddWithValue("@IdEstado", obj.IdEstado);
                    cmd.Parameters.AddWithValue("@EstadoActual", obj.EstadoActual);
                    cmd.Parameters.AddWithValue("@Estatus", obj.Estatus);
                    cmd.Parameters.Add("Resultado", SqlDbType.Int).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("Mensaje", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;
                    cmd.CommandType = CommandType.StoredProcedure;
                    oconexion.Open();
                    cmd.ExecuteNonQuery();
                    resultado = cmd.ExecuteNonQuery() > 0 ? true : false; /*Convert.ToInt32(cmd.Parameters["Resultado"].Value);*/
                    Mensaje = cmd.Parameters["Mensaje"].Value.ToString();
                }
            }
            catch (Exception ex)
            {
                resultado = false;
                Mensaje = ex.Message;
            }
            return resultado;
        }

        public bool EliminarEstado(int IdEstad, out string Mensaje)
        {
            bool Resp = false;
            Mensaje = string.Empty; // Asegúrate de que esté vacío inicialmente

            try
            {
                using (SqlConnection conex = new SqlConnection(Conexion.cn))
                {
                    using (SqlCommand cmd = new SqlCommand("SP_EliminarEstado", conex))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@IdEstado", IdEstad);
                        conex.Open();
                        Resp = cmd.ExecuteNonQuery() > 0; // Verifica si se eliminó al menos un registro
                    }
                }
            }
            catch (Exception ex)
            {
                Resp = false;
                Mensaje = ex.Message; // Captura el mensaje de error
            }
            return Resp;
        }


    }
}
