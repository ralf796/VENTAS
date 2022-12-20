using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ventas_BE;
using Ventas_DAL;

namespace Ventas_BLL
{
    public class FEL_BLL : IDisposable
    {
        public void Dispose() { }
        public static List<FEL_BE> GetDatosSP(FEL_BE item)
        {
            List<FEL_BE> data = null;
            using (var model = new FEL_DAL())
            {
                data = model.GetDatosSP(item);
            }
            return data;
        }
        public static List<FEL_BE> LOG(FEL_BE item)
        {
            List<FEL_BE> data = null;
            using (var model = new FEL_DAL())
            {
                data = model.LOG(item);
            }
            return data;
        }
    }
}
