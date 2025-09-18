using System;
using System.Collections.Generic;
using CapaEntidad;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;
using System.Runtime.InteropServices.ComTypes;

namespace CapaDatos
{
    public class CD_Solicitud
    {
        DataSet ds = new DataSet();
        string Patron = ConfigurationManager.AppSettings["Patron"].ToString();

        public List<Solicitud> ListarSolicitud()
        {
            List<Solicitud> listaSolicitud = new List<Solicitud>(); //esta lista la llenaremos
            try
            {
                SqlDataAdapter da = new SqlDataAdapter("SP_BuscarSolicitud", Conexion.cn);
                da.SelectCommand.CommandType = CommandType.StoredProcedure;
                da.Fill(ds, "Solicitud");
                //llenamos la lista de usuario devueltos por el SP
                if (ds != null)
                {
                    if (ds.Tables.Count > 0)
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                            {
                                listaSolicitud.Add(
                                    new Solicitud()
                                    {
                                        IdSolicitud = Convert.ToInt32(ds.Tables[0].Rows[i]["IdSolicitud"]),
                                        IdCliente = Convert.ToInt32(ds.Tables[0].Rows[i]["IdCliente"]),
                                        oCategoria =new Categoria() { IdCategoria = int.Parse(ds.Tables[0].Rows[i]["IdCategoria"].ToString()), Tipo = ds.Tables[0].Rows[i]["Tipo"].ToString() },
                                        oUsuario = new Usuario() { IdUsuario=int.Parse(ds.Tables[0].Rows[i]["IdUsuario"].ToString()), Nombre = ds.Tables[0].Rows[i]["Nombre"].ToString() },
                                        oEstado = new Estado() { IdEstado = int.Parse(ds.Tables[0].Rows[i]["IdEstado"].ToString()), EstadoActual = ds.Tables[0].Rows[i]["EstadoActual"].ToString() },
                                        Comentario = ds.Tables[0].Rows[i]["Comentario"].ToString(),
                                        Nombre = ds.Tables[0].Rows[i]["Nombre"].ToString(),
                                        Apellido = ds.Tables[0].Rows[i]["Apellido"].ToString(),
                                        Email = ds.Tables[0].Rows[i]["Email"].ToString(),
                                        Telefono = ds.Tables[0].Rows[i]["Telefono"].ToString(),
                                        Referencia = ds.Tables[0].Rows[i]["Referencia"].ToString(),
                                        Respuesta = ds.Tables[0].Rows[i]["Respuesta"].ToString(),
                                        Estatus = Convert.ToBoolean(ds.Tables[0].Rows[i]["Estatus"]),
                                    }
                                );
                            }
                        }
                        else
                        {
                            listaSolicitud = new List<Solicitud>();//si no hay datos regresa una lista vacia
                            listaSolicitud.Add(
                                new Solicitud()
                                {
                                    IdSolicitud = -1,
                                    Error = "Sin registros"
                                }
                            );
                        }

                    }
                    else
                    {
                        listaSolicitud = new List<Solicitud>();//si no hay datos regresa una lista vacia
                        listaSolicitud.Add(
                            new Solicitud()
                            {
                                IdSolicitud = -1,
                                Error = "Tabla Vacia"
                            }
                        );
                    }
                }
                else
                {
                    listaSolicitud = new List<Solicitud>();//si no hay datos regresa una lista vacia
                    listaSolicitud.Add(
                        new Solicitud()
                        {
                            IdSolicitud = -1,
                            Error = "Datos Nulos"
                        }
                    );
                }
            }
            catch (Exception ex)
            {
                listaSolicitud = new List<Solicitud>();
                listaSolicitud.Add(
                    new Solicitud()
                    {
                        IdSolicitud = -1,
                        Error = ex.Message
                    }
                 );
            }
            return listaSolicitud;//aqui llenamos la lista
        }//termina metodo




        public int AgregarSolicitud(Solicitud obj, out string Mensaje, out string ReferenciaGenerada)
        {
            int IdUser = 0;
            Mensaje = string.Empty; // que esté vacío y no null
            ReferenciaGenerada = string.Empty; // Inicializa la referencia generada
            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("SP_AgregarSolicitud", oconexion);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@IdCliente", obj.IdCliente);
                    cmd.Parameters.AddWithValue("@IdCategoria", obj.oCategoria.IdCategoria);
                    //cmd.Parameters.AddWithValue("@IdUsuario", obj.oUsuario.IdUsuario);
                    //cmd.Parameters.AddWithValue("@IdEstado", obj.oEstado.IdEstado);
                    cmd.Parameters.AddWithValue("@IdUsuario", obj.oUsuario?.IdUsuario > 0 ? obj.oUsuario.IdUsuario : (object)DBNull.Value);//Validacion para que admita valores vacios
                    cmd.Parameters.AddWithValue("@IdEstado", obj.oEstado?.IdEstado > 0 ? obj.oEstado.IdEstado : (object)DBNull.Value);//Validacion para que admita valores vacios
                    cmd.Parameters.AddWithValue("@Comentario", obj.Comentario);
                    cmd.Parameters.AddWithValue("@Nombre", obj.Nombre);
                    cmd.Parameters.AddWithValue("@Apellido", obj.Apellido);
                    cmd.Parameters.AddWithValue("@Email", obj.Email);
                    cmd.Parameters.AddWithValue("@Telefono", obj.Telefono);
                    cmd.Parameters.AddWithValue("@Estatus", obj.Estatus);
                    cmd.Parameters.AddWithValue("@Referencia", obj.Referencia ?? (object)DBNull.Value);
                    //Error//cmd.Parameters.AddWithValue("@Respuesta", obj.Respuesta? > null ?obj.Respuesta:(objeto)DBNull.Value);
                    //Correcto//cmd.Parameters.AddWithValue("@Respuesta",string.IsNullOrWhiteSpace(obj.Respuesta) ? (object)DBNull.Value : obj.Respuesta);
                    cmd.Parameters.AddWithValue("@Respuesta",string.IsNullOrWhiteSpace(obj.Respuesta) ? (object)DBNull.Value : obj.Respuesta);
                    cmd.Parameters.Add("Resultado", SqlDbType.Int).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("Mensaje", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("ReferenciaGenerada", SqlDbType.NVarChar, 20).Direction = ParameterDirection.Output; // Parámetro para la referencia generada
                    oconexion.Open();
                    cmd.ExecuteNonQuery();
                    IdUser = Convert.ToInt32(cmd.Parameters["Resultado"].Value);
                    Mensaje = cmd.Parameters["Mensaje"].Value.ToString();
                    ReferenciaGenerada = cmd.Parameters["ReferenciaGenerada"].Value.ToString(); // Recuperamos la referencia generada
                }
            }
            catch (Exception ex)
            {
                IdUser = 0;
                Mensaje = ex.Message;
                ReferenciaGenerada = string.Empty; // En caso de error
            }
            return IdUser;
        }//termina el metodo

        public bool ModificarSolicitud(Solicitud obj, out string Mensaje)
        {
            bool Resultado = false;
            Mensaje = string.Empty;//que este vacio y no null
            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("SP_ModificarSolicitud", oconexion);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@IdSolicitud", obj.IdSolicitud);
                    cmd.Parameters.AddWithValue("@IdCliente", obj.IdCliente);
                    //cmd.Parameters.AddWithValue("@IdCategoria", obj.oCategoria.IdCategoria);
                    cmd.Parameters.AddWithValue("@IdCategoria", obj.oCategoria.IdCategoria);
                    //cmd.Parameters.AddWithValue("@IdUsuario", obj.IdUsuario);
                    cmd.Parameters.AddWithValue("@IdUsuario", obj.oUsuario.IdUsuario);
                    //cmd.Parameters.AddWithValue("@IdEstado", obj.IdEstado);
                    cmd.Parameters.AddWithValue("@IdEstado", obj.oEstado.IdEstado);
                    cmd.Parameters.AddWithValue("@Comentario", obj.Comentario);
                    cmd.Parameters.AddWithValue("@Nombre", obj.Nombre);
                    cmd.Parameters.AddWithValue("@Apellido", obj.Apellido);
                    cmd.Parameters.AddWithValue("@Email", obj.Email);
                    cmd.Parameters.AddWithValue("@Telefono", obj.Telefono);
                    cmd.Parameters.AddWithValue("@Estatus", obj.Estatus);
                    cmd.Parameters.AddWithValue("@Patron", Patron);//el patron de la BD , lo declaramos abajo del "ds"
                    cmd.Parameters.AddWithValue("@Referencia", obj.Referencia);
                    cmd.Parameters.AddWithValue("Respuesta",obj.Respuesta);
                    cmd.Parameters.Add("Resultado", SqlDbType.Int).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("Mensaje", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;
                    oconexion.Open();
                    cmd.ExecuteNonQuery();//ejecuta el SP en la BD
                    //Resultado = Convert.ToBoolean(cmd.Parameters["Resultado"].Value);
                    Resultado = cmd.ExecuteNonQuery() > 0 ? true : false;
                    Mensaje = cmd.Parameters["Mensaje"].Value.ToString();
                }
            }
            catch (Exception ex)
            {
                Resultado = false;
                Mensaje = ex.Message;
            }
            return Resultado;
        }//termina el metodo

        public bool EliminarSolicitud(int IdSol, out string Mensaje)
        {
            bool Resp = false;
            Mensaje = string.Empty; // Asegúrate de que esté vacío inicialmente

            try
            {
                using (SqlConnection conex = new SqlConnection(Conexion.cn))
                {
                    using (SqlCommand cmd = new SqlCommand("SP_EliminarSolicitud", conex))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@IdSolicitud", IdSol);
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
        }//Termina el metodo de Eliminar
    }
}
