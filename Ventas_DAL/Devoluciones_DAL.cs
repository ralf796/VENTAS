using System;
using System.Collections.Generic;
using Ventas_BE;

namespace Ventas_DAL
{
    public class Devoluciones_DAL:IDisposable
    {
        public void Dispose() { }
        public List<Devoluciones_BE> GetDatosSP(Devoluciones_BE item)
        {
            List<Devoluciones_BE> result = new List<Devoluciones_BE>();
            using (var model = new Base_SQL("sp_devoluciones"))
            {
                model.Command.Parameters.AddWithValue("@ID_VENTA", item.ID_VENTA);
                model.Command.Parameters.AddWithValue("@CREADO_POR", item.CREADO_POR);
                model.Command.Parameters.AddWithValue("@OBSERVACIONES", item.OBSERVACIONES);
                model.Command.Parameters.AddWithValue("@MTIPO", item.MTIPO);
                result = model.GetData<Devoluciones_BE>();
            }
            return result;
        }
    }
}
