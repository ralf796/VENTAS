using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ventas_BE;

namespace Ventas_DAL
{
    public class Reportes_DAL : IDisposable
    {
        public void Dispose() { }
        public List<Reportes_BE> GetSPReportes(Reportes_BE item)
        {
            List<Reportes_BE> result = new List<Reportes_BE>();
            using (var model = new Base_SQL("sp_reportes"))
            {
                model.Command.Parameters.AddWithValue("@FECHA_INICIAL", item.FECHA_INICIAL);
                model.Command.Parameters.AddWithValue("@FECHA_FINAL", item.FECHA_FINAL);
                model.Command.Parameters.AddWithValue("@ID_VENTA", item.ID_VENTA);
                model.Command.Parameters.AddWithValue("@CODIGO", item.CODIGO);
                model.Command.Parameters.AddWithValue("@MTIPO", item.MTIPO);
                result = model.GetData<Reportes_BE>();
            }
            return result;
        }
    }
}
