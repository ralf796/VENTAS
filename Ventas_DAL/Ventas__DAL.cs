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
                model.Command.Parameters.AddWithValue("@ID_MARCA_REPUESTO", item.ID_MARCA_REPUESTO);
                model.Command.Parameters.AddWithValue("@ID_SUBCATEGORIA", item.ID_SUBCATEGORIA);
                model.Command.Parameters.AddWithValue("@ID_CATEGORIA", item.ID_CATEGORIA);
                model.Command.Parameters.AddWithValue("@ID_SERIE_VEHICULO", item.ID_SERIE_VEHICULO);
                model.Command.Parameters.AddWithValue("@ID_MARCA_VEHICULO", item.ID_MARCA_VEHICULO);
                model.Command.Parameters.AddWithValue("@ID_MODELO", item.ID_MODELO);
                result = model.GetData<Ventas__BE>();
            }
            return result;
        }
    }
}
