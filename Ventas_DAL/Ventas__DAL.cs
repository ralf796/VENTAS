using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ventas_BE;

namespace Ventas_DAL
{
    public class Ventas__DAL : IDisposable
    {
        public void Dispose() { }
        public List<Ventas__BE> GetDatosSP(Ventas__BE item)
        {
            List<Ventas__BE> result = new List<Ventas__BE>();
            using (var model = new Base_SQL("sp_ventas"))
            {
                model.Command.Parameters.AddWithValue("@MTIPO", item.MTIPO);
                model.Command.Parameters.AddWithValue("@NIT", item.NIT);
                result = model.GetData<Ventas__BE>();
            }
            return result;
        }
    }
}
