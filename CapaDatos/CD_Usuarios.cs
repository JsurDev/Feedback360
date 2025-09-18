using System;
using System.Collections.Generic;
using CapaEntidad;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;
namespace CapaDatos
{
    //SP_BuscarUsuario
    public class CD_Usuarios
    {
        DataSet ds = new DataSet();
        string Patron = ConfigurationManager.AppSettings["Patron"].ToString();

        public List<Usuario> ListarUsuarios()
        {
            List<Usuario> listaUsers = new List<Usuario>(); //esta lista la llenaremos
            try
            {
                SqlDataAdapter da = new SqlDataAdapter("SP_BuscarUsuario", Conexion.cn);
                da.SelectCommand.CommandType = CommandType.StoredProcedure;
                da.Fill(ds, "Usuario");
                //llenamos la lista de usuario devueltos por el SP
                if (ds != null)
                {
                    if (ds.Tables.Count > 0)
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                            {
                                listaUsers.Add(
                                    new Usuario()
                                    {
                                        IdUsuario = Convert.ToInt32(ds.Tables[0].Rows[i]["IdUsuario"]),
                                        Nombre = ds.Tables[0].Rows[i]["Nombre"].ToString(),
                                        Apellido = ds.Tables[0].Rows[i]["Apellido"].ToString(),
                                        Telefono = ds.Tables[0].Rows[i]["Telefono"].ToString(),
                                        Email = ds.Tables[0].Rows[i]["Email"].ToString(),
                                        Password = ds.Tables[0].Rows[i]["Password"].ToString(),
                                        Restablecer = Convert.ToBoolean(ds.Tables[0].Rows[i]["Restablecer"]),
                                        Estatus = Convert.ToBoolean(ds.Tables[0].Rows[i]["Estatus"]),
                                        oPuesto = new Puesto() { IdPuesto = int.Parse(ds.Tables[0].Rows[i]["IdPuesto"].ToString()), NombrePuesto = ds.Tables[0].Rows[i]["NombrePuesto"].ToString() }
                                    }
                                );
                            }
                        }
                        else
                        {
                            listaUsers = new List<Usuario>();//si no hay datos regresa una lista vacia
                            listaUsers.Add(
                                new Usuario()
                                {
                                    IdUsuario = -1,
                                    Error = "Sin registros"
                                }
                            );
                        }

                    }
                    else
                    {
                        listaUsers = new List<Usuario>();//si no hay datos regresa una lista vacia
                        listaUsers.Add(
                            new Usuario()
                            {
                                IdUsuario = -1,
                                Error = "Tabla Vacia"
                            }
                        );
                    }
                }
                else
                {
                    listaUsers = new List<Usuario>();//si no hay datos regresa una lista vacia
                    listaUsers.Add(
                        new Usuario()
                        {
                            IdUsuario = -1,
                            Error = "Datos Nulos"
                        }
                    );
                }
            }
            catch (Exception ex)
            {
                listaUsers = new List<Usuario>();
                listaUsers.Add(
                    new Usuario()
                    {
                        IdUsuario = -1,
                        Error = ex.Message
                    }
                 );
            }
            return listaUsers;//aqui llenamos la lista
        }//termina metodo



        public int AgregarUsuario(Usuario obj,out string Mensaje)
        {
            int IdUser = 0;
            Mensaje = string.Empty;//que este vacio y no null
            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("SP_AgregarUsuario", oconexion);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Nombre", obj.Nombre);
                    cmd.Parameters.AddWithValue("@Apellido", obj.Apellido);
                    cmd.Parameters.AddWithValue("@IdPuesto", obj.oPuesto.IdPuesto);
                    cmd.Parameters.AddWithValue("@Telefono", obj.Telefono);
                    cmd.Parameters.AddWithValue("@Email", obj.Email);
                    cmd.Parameters.AddWithValue("@Password", obj.Password);
                    cmd.Parameters.AddWithValue("@Restablecer", obj.Restablecer);
                    cmd.Parameters.AddWithValue("@Estatus", obj.Estatus);
                    cmd.Parameters.AddWithValue("@Patron", Patron);
                    cmd.Parameters.Add("Resultado", SqlDbType.Int).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("Mensaje", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;
                    oconexion.Open();
                    cmd.ExecuteNonQuery();
                    IdUser = Convert.ToInt32(cmd.Parameters["Resultado"].Value);
                    Mensaje = cmd.Parameters["Mensaje"].Value.ToString();
                }
            }
            catch (Exception ex) 
            {
                IdUser = 0;
                Mensaje=ex.Message;
            }
            return IdUser;
        }

        public bool ModificarUsuario(Usuario obj, out string Mensaje)
        {
            bool resultado = false;
            Mensaje= string.Empty;
            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("SP_ModificarUsuario", oconexion);
                    cmd.Parameters.AddWithValue("@IdUsuario", obj.IdUsuario);
                    cmd.Parameters.AddWithValue("@Nombre", obj.Nombre);
                    cmd.Parameters.AddWithValue("@Apellido", obj.Apellido);
                    cmd.Parameters.AddWithValue("@IdPuesto", obj.oPuesto.IdPuesto);
                    cmd.Parameters.AddWithValue("@Telefono", obj.Telefono);
                    cmd.Parameters.AddWithValue("@Email", obj.Email);
                    cmd.Parameters.AddWithValue("@Password", obj.Password);
                    cmd.Parameters.AddWithValue("@Restablecer", obj.Restablecer);
                    cmd.Parameters.AddWithValue("@Estatus", obj.Estatus);
                    cmd.Parameters.AddWithValue("@Patron", Patron);//el patron de la BD , lo declaramos abajo del "ds"
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

        //public bool EliminarUsuario(int IdUser, out string Mensaje)
        //{
        //    bool Resp = false;
        //    Mensaje = string.Empty; // Asegúrate de que esté vacío inicialmente

        //    try
        //    {
        //        using (SqlConnection conex = new SqlConnection(Conexion.cn))
        //        {
        //            using (SqlCommand cmd = new SqlCommand("SP_EliminarUsuario", conex))
        //            {
        //                cmd.CommandType = CommandType.StoredProcedure;
        //                cmd.Parameters.AddWithValue("@IdUsuario", IdUser);
        //                conex.Open();
        //                Resp = cmd.ExecuteNonQuery() > 0; // Verifica si se eliminó al menos un registro
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Resp = false;
        //        Mensaje = ex.Message; // Captura el mensaje de error
        //    }
        //    return Resp;
        //}
        public bool EliminarUsuario(int IdUser, out string Mensaje)
        {
            bool Resp = false;
            Mensaje = string.Empty;

            try
            {
                using (SqlConnection conex = new SqlConnection(Conexion.cn))
                {
                    using (SqlCommand cmd = new SqlCommand("SP_EliminarUsuario", conex))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@IdUsuario", IdUser);
                        conex.Open();
                        Resp = cmd.ExecuteNonQuery() > 0;
                    }
                }
            }
            catch (SqlException ex)
            {
                Resp = false;

                // Error personalizado lanzado desde RAISERROR (por defecto es 50000)
                if (ex.Number == 50000)
                {
                    Mensaje = ex.Message; // Este es tu mensaje personalizado
                }
                else
                {
                    Mensaje = "Error de base de datos: " + ex.Message;
                }
            }
            catch (Exception ex)
            {
                Resp = false;
                Mensaje = "Error inesperado: " + ex.Message;
            }

            return Resp;
        }


        //VALIDACION DE INICIO DE SESION USUARIO
        public Usuario LoginUsuario(string email, string password)
        {
            Usuario usuario = null;
            try
            {
                using (SqlConnection conn = new SqlConnection(Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("SP_BuscarUsuarioLogIn", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Email", email);
                    cmd.Parameters.AddWithValue("@PasswordIngresada", password);
                    cmd.Parameters.AddWithValue("@Patron", Patron);
                    conn.Open();
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        if (dr.Read())
                        {
                            usuario = new Usuario()
                            {
                                IdUsuario = Convert.ToInt32(dr["IdUsuario"]),
                                Nombre = dr["Nombre"].ToString(),
                                Apellido = dr["Apellido"].ToString(),
                                Telefono = dr["Telefono"].ToString(),
                                Email = dr["Email"].ToString(),
                                Password = dr["Password"].ToString(),
                                Restablecer = Convert.ToBoolean(dr["Restablecer"]),
                                Estatus = Convert.ToBoolean(dr["Estatus"]),
                                oPuesto = new Puesto()
                                {
                                    IdPuesto = Convert.ToInt32(dr["IdPuesto"]),
                                    NombrePuesto = dr["NombrePuesto"].ToString()
                                }
                            };
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                usuario = new Usuario()
                {
                    IdUsuario = -1,
                    Error = ex.Message
                };
            }
            return usuario;
        }//termina el metodo




    }
}
