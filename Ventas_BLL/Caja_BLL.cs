using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ventas_BE;
using Ventas_DAL;

namespace Ventas_BLL
{
    public class Caja_BLL : IDisposable
    {
        public void Dispose() { }
        public static List<Caja_BE> GetSPCaja(Caja_BE item)
        {
            List<Caja_BE> data = null;
            using (var model = new Caja_DAL())
            {
                data = model.GetSPCaja(item);
            }
            return data;
        }
    }
}
