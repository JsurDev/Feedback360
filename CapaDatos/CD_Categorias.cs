using CapaEntidad;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;


namespace CapaDatos
{
    public class CD_Categorias
    {
        DataSet ds = new DataSet();
        string Patron = ConfigurationManager.AppSettings["Patron"].ToString();
        public List<Categoria> ListarCategorias()
        {
            List<Categoria> listaCategoria = new List<Categoria>(); //esta lista la llenaremos
            try
            {
                SqlDataAdapter da = new SqlDataAdapter("SP_BuscarCategoria", Conexion.cn);
                da.SelectCommand.CommandType = CommandType.StoredProcedure;
                da.Fill(ds, "Categoria");
                //llenamos la lista de Categoria devueltos por el SP
                if (ds != null)
                {
                    if (ds.Tables.Count > 0)
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                            {
                                listaCategoria.Add(
                                    new Categoria()
                                    {
                                        IdCategoria = Convert.ToInt32(ds.Tables[0].Rows[i]["IdCategoria"]),
                                        Tipo = ds.Tables[0].Rows[i]["Tipo"].ToString(),
                                        Estado = Convert.ToBoolean(ds.Tables[0].Rows[i]["Estado"]),
                                    }
                                );
                            }
                        }
                        else
                        {
                            listaCategoria = new List<Categoria>();//si no hay datos regresa una lista vacia
                            listaCategoria.Add(
                                new Categoria()
                                {
                                    IdCategoria = -1,
                                    Error = "Sin registros"
                                }
                            );
                        }

                    }
                    else
                    {
                        listaCategoria = new List<Categoria>();//si no hay datos regresa una lista vacia
                        listaCategoria.Add(
                            new Categoria()
                            {
                                IdCategoria = -1,
                                Error = "Tabla Vacia"
                            }
                        );
                    }
                }
                else
                {
                    listaCategoria = new List<Categoria>();//si no hay datos regresa una lista vacia
                    listaCategoria.Add(
                        new Categoria()
                        {
                            IdCategoria = -1,
                            Error = "Datos Nulos"
                        }
                    );
                }
            }
            catch (Exception ex)
            {
                listaCategoria = new List<Categoria>();
                listaCategoria.Add(
                    new Categoria()
                    {
                        IdCategoria = -1,
                        Error = ex.Message
                    }
                 );
            }
            return listaCategoria;//aqui llenamos la lista
        }//termina metodo


        public int AgregarCategoria(Categoria obj, out string Mensaje)
        {
            int IdCategoria = 0;
            Mensaje = string.Empty;//que este vacio y no null
            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("SP_AgregarCategoria", oconexion);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Tipo", obj.Tipo);
                    cmd.Parameters.AddWithValue("@Estado", obj.Estado);
                    //cmd.Parameters.AddWithValue("@Patron", Patron);//el patron de la BD , lo declaramos abajo del "ds"
                    cmd.Parameters.Add("Resultado", SqlDbType.Int).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("Mensaje", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;
                    oconexion.Open();
                    cmd.ExecuteNonQuery();
                    IdCategoria = Convert.ToInt32(cmd.Parameters["Resultado"].Value);
                    Mensaje = cmd.Parameters["Mensaje"].Value.ToString();
                }
            }
            catch (Exception ex)
            {
                IdCategoria = 0;
                Mensaje = ex.Message;
            }
            return IdCategoria;
        }

        public bool ModificarCategoria(Categoria obj, out string Mensaje)
        {
            bool resultado = false;
            Mensaje = string.Empty;
            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("SP_ModificarCategoria", oconexion);
                    cmd.Parameters.AddWithValue("@IdCategoria", obj.IdCategoria);
                    cmd.Parameters.AddWithValue("@Tipo", obj.Tipo);
                    cmd.Parameters.AddWithValue("@Estado", obj.Estado);
                    //cmd.Parameters.AddWithValue("@Patron", Patron);//el patron de la BD , lo declaramos abajo del "ds"
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

        public bool EliminarCategoria(int IdCat, out string Mensaje)
        {
            bool Resp = false;
            Mensaje = string.Empty; // Asegúrate de que esté vacío inicialmente

            try
            {
                using (SqlConnection conex = new SqlConnection(Conexion.cn))
                {
                    using (SqlCommand cmd = new SqlCommand("SP_EliminarCategoria", conex))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@IdCategoria", IdCat);
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
