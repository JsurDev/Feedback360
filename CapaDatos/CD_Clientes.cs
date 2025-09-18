using System;
using System.Collections.Generic;
using CapaEntidad;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;

namespace CapaDatos
{
    public class CD_Clientes
    {
        DataSet ds = new DataSet();
        string Patron = ConfigurationManager.AppSettings["Patron"].ToString();

        public List<Cliente> ListarClientes()
        {
            List<Cliente> listaCliente = new List<Cliente>(); //esta lista la llenaremos
            try
            {
                SqlDataAdapter da = new SqlDataAdapter("SP_BuscarCliente", Conexion.cn);
                da.SelectCommand.CommandType = CommandType.StoredProcedure;
                da.Fill(ds, "Cliente");
                //llenamos la lista de usuario devueltos por el SP
                if (ds != null)
                {
                    if (ds.Tables.Count > 0)
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                            {
                                listaCliente.Add(
                                    new Cliente()
                                    {
                                        IdCliente = Convert.ToInt32(ds.Tables[0].Rows[i]["IdCliente"]),
                                        Nombre = ds.Tables[0].Rows[i]["Nombre"].ToString(),
                                        Apellido = ds.Tables[0].Rows[i]["Apellido"].ToString(),
                                        Telefono = ds.Tables[0].Rows[i]["Telefono"].ToString(),
                                        Email = ds.Tables[0].Rows[i]["Email"].ToString(),
                                        Password = ds.Tables[0].Rows[i]["Password"].ToString(),
                                        Restablecer = Convert.ToBoolean(ds.Tables[0].Rows[i]["Restablecer"]),
                                        Estatus = Convert.ToBoolean(ds.Tables[0].Rows[i]["Estatus"]),
                                    }
                                );
                            }
                        }
                        else
                        {
                            listaCliente = new List<Cliente>();//si no hay datos regresa una lista vacia
                            listaCliente.Add(
                                new Cliente()
                                {
                                    IdCliente = -1,
                                    Error = "Sin registros"
                                }
                            );
                        }

                    }
                    else
                    {
                        listaCliente = new List<Cliente>();//si no hay datos regresa una lista vacia
                        listaCliente.Add(
                            new Cliente()
                            {
                                IdCliente = -1,
                                Error = "Tabla Vacia"
                            }
                        );
                    }
                }
                else
                {
                    listaCliente = new List<Cliente>();//si no hay datos regresa una lista vacia
                    listaCliente.Add(
                        new Cliente()
                        {
                            IdCliente = -1,
                            Error = "Datos Nulos"
                        }
                    );
                }
            }
            catch (Exception ex)
            {
                listaCliente = new List<Cliente>();
                listaCliente.Add(
                    new Cliente()
                    {
                        IdCliente = -1,
                        Error = ex.Message
                    }
                 );
            }
            return listaCliente;//aqui llenamos la lista
        }//termina metodo


        public int AgregarCliente(Cliente obj, out string Mensaje)
        {
            int IdCliente = 0;
            Mensaje = string.Empty;//que este vacio y no null
            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("SP_AgregarCliente", oconexion);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Nombre", obj.Nombre);
                    cmd.Parameters.AddWithValue("@Apellido", obj.Apellido);
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
                    IdCliente = Convert.ToInt32(cmd.Parameters["Resultado"].Value);
                    Mensaje = cmd.Parameters["Mensaje"].Value.ToString();
                }
            }
            catch (Exception ex)
            {
                IdCliente = 0;
                Mensaje = ex.Message;
            }
            return IdCliente;
        }

        public bool ModificarCliente(Cliente obj, out string Mensaje)
        {
            bool resultado = false;
            Mensaje = string.Empty;
            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("SP_ModificarCliente", oconexion);
                    cmd.Parameters.AddWithValue("@IdCliente", obj.IdCliente);
                    cmd.Parameters.AddWithValue("@Nombre", obj.Nombre);
                    cmd.Parameters.AddWithValue("@Apellido", obj.Apellido);
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

        public bool EliminarCliente(int IdCliente, out string Mensaje)
        {
            bool Resp = false;
            Mensaje = string.Empty; // Asegúrate de que esté vacío inicialmente

            try
            {
                using (SqlConnection conex = new SqlConnection(Conexion.cn))
                {
                    using (SqlCommand cmd = new SqlCommand("SP_EliminarCliente", conex))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@IdCliente", IdCliente);
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

        public Cliente LogInClienteCN(string email, string contrasena)
        {
            Cliente cliente = null;
            try
            {
                using (SqlConnection conn = new SqlConnection(Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("SP_BuscarClientesLogIn", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Email", email);
                    cmd.Parameters.AddWithValue("@PasswordIngresada", contrasena);
                    cmd.Parameters.AddWithValue("@Patron", Patron);
                    conn.Open();
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        if (dr.Read())
                        {
                            cliente = new Cliente()
                            {
                                IdCliente = Convert.ToInt32(dr["IdCliente"]),
                                Nombre = dr["Nombre"].ToString(),
                                Apellido = dr["Apellido"].ToString(),
                                Telefono = dr["Telefono"].ToString(),
                                Email = dr["Email"].ToString(),
                                Password = dr["Password"].ToString(),
                                Restablecer = Convert.ToBoolean(dr["Restablecer"]),
                                Estatus = Convert.ToBoolean(dr["Estatus"])
                            };
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                cliente = new Cliente()
                {
                    IdCliente = -1,
                    Error = ex.Message
                };
            }
            return cliente;
        }//termina el metodo

    }
}
