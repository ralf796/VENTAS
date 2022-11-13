using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ventas_BE;
using Ventas_DAL;

namespace Ventas_BLL
{
    public class Anulacion_BLL:IDisposable
    {
        public void Dispose() { }
        public static List<Anulacion_BE> GetSPAnulacion(Anulacion_BE item)
        {
            List<Anulacion_BE> data = null;
            using (var model = new Anulacion_DAL())
            {
                data = model.GetSPAnulacion(item);
            }
            return data;
        }
    }
}
