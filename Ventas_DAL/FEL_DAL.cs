using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ventas_BE;

namespace Ventas_DAL
{
    public class FEL_DAL:IDisposable
    {
        public void Dispose() { }
        public List<FEL_BE> GetDatosSP(FEL_BE item)
        {
            if (item.MTIPO != 5)
                item.FECHA_CERTIFICACION = null;

            List<FEL_BE> result = new List<FEL_BE>();
            using (var model = new Base_SQL("sp_fel"))
            {
                model.Command.Parameters.AddWithValue("@MTIPO", item.MTIPO);
                model.Command.Parameters.AddWithValue("@ID_VENTA", item.ID_VENTA);
                model.Command.Parameters.AddWithValue("@UUID", item.UUID);
                model.Command.Parameters.AddWithValue("@SERIE_FEL", item.SERIE_FEL);
                model.Command.Parameters.AddWithValue("@NUMERO_FEL", item.NUMERO_FEL);
                model.Command.Parameters.AddWithValue("@FECHA_CERTIFICACION", item.FECHA_CERTIFICACION);
                model.Command.Parameters.AddWithValue("@FEL", item.FEL);
                result = model.GetData<FEL_BE>();
            }
            return result;
        }
    }
}