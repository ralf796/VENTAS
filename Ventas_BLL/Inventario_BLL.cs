using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ventas_BE;
using Ventas_DAL;

namespace Ventas_BLL
{
    public class Inventario_BLL:IDisposable
    {
        public void Dispose() { }
        public static List<Inventario_BE> GetSPInventario(Inventario_BE item)
        {
            List<Inventario_BE> data = null;
            using (var model = new Inventario_DAL())
            {
                data = model.GetSPInventario(item);
            }
            return data;
        }
    }
}
