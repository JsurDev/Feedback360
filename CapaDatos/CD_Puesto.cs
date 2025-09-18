using System;
using System.Collections.Generic;
using CapaEntidad;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;

namespace CapaDatos
{
    public class CD_Puesto
    {
        DataSet ds = new DataSet();
        string Patron = ConfigurationManager.AppSettings["Patron"].ToString();

        public List<Puesto> ListarPuesto()
        {
            List<Puesto> listaPuesto = new List<Puesto>(); //esta lista la llenaremos
            try
            {
                SqlDataAdapter da = new SqlDataAdapter("SP_BuscarPuesto", Conexion.cn);
                da.SelectCommand.CommandType = CommandType.StoredProcedure;
                da.Fill(ds, "Puesto");
                //llenamos la lista de Puesto devueltos por el SP
                if (ds != null)
                {
                    if (ds.Tables.Count > 0)
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                            {
                                listaPuesto.Add(
                                    new Puesto()
                                    {
                                        IdPuesto = Convert.ToInt32(ds.Tables[0].Rows[i]["IdPuesto"]),
                                        NombrePuesto = ds.Tables[0].Rows[i]["NombrePuesto"].ToString(),
                                        Estado = Convert.ToBoolean(ds.Tables[0].Rows[i]["Estado"]),
                                    }
                                );
                            }
                        }
                        else
                        {
                            listaPuesto = new List<Puesto>();//si no hay datos regresa una lista vacia
                            listaPuesto.Add(
                                new Puesto()
                                {
                                    IdPuesto = -1,
                                    Error = "Sin registros"
                                }
                            );
                        }

                    }
                    else
                    {
                        listaPuesto = new List<Puesto>();//si no hay datos regresa una lista vacia
                        listaPuesto.Add(
                            new Puesto()
                            {
                                IdPuesto = -1,
                                Error = "Tabla Vacia"
                            }
                        );
                    }
                }
                else
                {
                    listaPuesto = new List<Puesto>();//si no hay datos regresa una lista vacia
                    listaPuesto.Add(
                        new Puesto()
                        {
                            IdPuesto = -1,
                            Error = "Datos Nulos"
                        }
                    );
                }
            }
            catch (Exception ex)
            {
                listaPuesto = new List<Puesto>();
                listaPuesto.Add(
                    new Puesto()
                    {
                        IdPuesto = -1,
                        Error = ex.Message
                    }
                 );
            }
            return listaPuesto;//aqui llenamos la lista
        }//termina el metodo

    }
}
