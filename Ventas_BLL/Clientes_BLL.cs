using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ventas_DAL;
using Ventas_BE;

namespace Ventas_BLL
{
    public class Clientes_BLL : IDisposable
    {
        public void Dispose() { }
        public static List<Clientes_BE> GetSPCliente(Clientes_BE item)
        {
            List<Clientes_BE> data = null;
            using (var model = new Clientes_DAL())
            {
                data = model.GetSPCliente(item);
            }
            return data;
        }
    }
}
