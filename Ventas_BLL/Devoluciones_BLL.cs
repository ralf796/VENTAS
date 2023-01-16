using System;
using System.Collections.Generic;
using Ventas_BE;
using Ventas_DAL;

namespace Ventas_BLL
{
    public class Devoluciones_BLL:IDisposable
    {
        public void Dispose() { }
        public static List<Devoluciones_BE> GetDatosSP(Devoluciones_BE item)
        {
            List<Devoluciones_BE> data = null;
            using (var model = new Devoluciones_DAL())
            {
                data = model.GetDatosSP(item);
            }
            return data;
        }
    }
}
