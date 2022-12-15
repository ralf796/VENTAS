using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ventas_BE;

namespace Ventas_DAL
{
    public class Cartera_DAL:IDisposable
    {
        public void Dispose() { }
        public List<Cartera_BE> GetDatosSP(Cartera_BE item)
        {
            List<Cartera_BE> result = new List<Cartera_BE>();
            using (var model = new Base_SQL("sp_cartera"))
            {
                model.Command.Parameters.AddWithValue("@ID_VENTA", item.ID_VENTA);
                model.Command.Parameters.AddWithValue("@MTIPO", item.MTIPO);
                result = model.GetData<Cartera_BE>();
            }
            return result;
        }
    }
}
