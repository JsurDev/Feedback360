using System;
using System.Collections.Generic;
using CapaEntidad;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;

namespace CapaDatos
{
    public class CD_Room
    {
        DataSet ds = new DataSet();
        string Patron = ConfigurationManager.AppSettings["Patron"].ToString();

        public List<Room> ListarRoom()
        {
            List<Room> listaRoom = new List<Room>(); //esta lista la llenaremos
            try
            {
                SqlDataAdapter da = new SqlDataAdapter("SP_BuscarRoom", Conexion.cn);
                da.SelectCommand.CommandType = CommandType.StoredProcedure;
                da.Fill(ds, "Room");
                //llenamos la lista de Room devueltos por el SP
                if (ds != null)
                {
                    if (ds.Tables.Count > 0)
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                            {
                                listaRoom.Add(
                                    new Room()
                                    {
                                        IdRoom = Convert.ToInt32(ds.Tables[0].Rows[i]["IdRoom"]),
                                        Nombre = ds.Tables[0].Rows[i]["Nombre"].ToString(),
                                        Precio = Convert.ToDecimal(ds.Tables[0].Rows[i]["Precio"]),
                                        Cantidad = Convert.ToInt32(ds.Tables[0].Rows[i]["Cantidad"]),
                                        ImagenUrl = ds.Tables[0].Rows[i]["ImagenUrl"].ToString()
                                    }
                                );
                            }
                        }
                        else
                        {
                            listaRoom = new List<Room>();//si no hay datos regresa una lista vacia
                            listaRoom.Add(
                                new Room()
                                {
                                    IdRoom = -1,
                                    Error = "Sin registros"
                                }
                            );
                        }

                    }
                    else
                    {
                        listaRoom = new List<Room>();//si no hay datos regresa una lista vacia
                        listaRoom.Add(
                            new Room()
                            {
                                IdRoom = -1,
                                Error = "Tabla Vacia"
                            }
                        );
                    }
                }
                else
                {
                    listaRoom = new List<Room>();//si no hay datos regresa una lista vacia
                    listaRoom.Add(
                        new Room()
                        {
                            IdRoom = -1,
                            Error = "Datos Nulos"
                        }
                    );
                }
            }
            catch (Exception ex)
            {
                listaRoom = new List<Room>();
                listaRoom.Add(
                    new Room()
                    {
                        IdRoom = -1,
                        Error = ex.Message
                    }
                 );
            }
            return listaRoom;//aqui llenamos la lista
        }//termina metodo
    }
}
