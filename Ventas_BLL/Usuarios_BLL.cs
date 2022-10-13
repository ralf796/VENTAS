using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ventas_BE;
using Ventas_DAL;

namespace Ventas_BLL
{
    public class Usuarios_BLL:IDisposable
    {
        public void Dispose() { }
        public static List<Usuarios_BE> GetSPUsuario(Usuarios_BE item)
        {
            List<Usuarios_BE> data = null;
            using (var model = new Usuarios_DAL())
            {
                data = model.GetSPUsuario(item);
            }
            return data;
        }
    }
}
