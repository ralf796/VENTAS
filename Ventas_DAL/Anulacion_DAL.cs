using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ventas_BE;

namespace Ventas_DAL
{
    public class Anulacion_DAL:IDisposable
    {
        public void Dispose() { }
        public List<Anulacion_BE> GetSPAnulacion(Anulacion_BE item)
        {
            List<Anulacion_BE> result = new List<Anulacion_BE>();
            using (var model = new Base_SQL("sp_anulaciones"))
            {
                model.Command.Parameters.AddWithValue("@ID_VENTA", item.ID_VENTA);
                model.Command.Parameters.AddWithValue("@CREADO_POR", item.CREADO_POR);
                model.Command.Parameters.AddWithValue("@FECHA", item.FECHA);
                model.Command.Parameters.AddWithValue("@MTIPO", item.MTIPO);
                result = model.GetData<Anulacion_BE>();
            }
            return result;
        }
    }
}
