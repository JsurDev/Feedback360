using CapaDatos;
using CapaEntidad;
using System.Collections.Generic;

namespace CapaNegocio
{
    public class CN_Puesto
    {
        private CD_Puesto objCapaDato = new CD_Puesto();

        public List<Puesto> ListarPuesto()
        {
            return objCapaDato.ListarPuesto();
        }


    }
}
