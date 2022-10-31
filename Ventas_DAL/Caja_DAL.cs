using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ventas_BE;


namespace Ventas_DAL
{
    public class Caja_DAL : IDisposable
    {
        public void Dispose() { }
        public List<Caja_BE> GetSPCaja(Caja_BE item)
        {
            List<Caja_BE> result = new List<Caja_BE>();
            using (var model = new Base_SQL("sp_caja"))
            {
                model.Command.Parameters.AddWithValue("@ID_VENTA", item.ID_VENTA);
                model.Command.Parameters.AddWithValue("@NOMBRE", item.NOMBRE);
                model.Command.Parameters.AddWithValue("@TOTAL", item.TOTAL);
                model.Command.Parameters.AddWithValue("@CREADO_POR", item.CREADO_POR);
                model.Command.Parameters.AddWithValue("@CANTIDAD", item.CANTIDAD);
                model.Command.Parameters.AddWithValue("@PRECIO_UNITARIO", item.PRECIO_UNITARIO);
                model.Command.Parameters.AddWithValue("@DESCUENTO", item.DESCUENTO);
                model.Command.Parameters.AddWithValue("@SUBTOTAL", item.SUBTOTAL);
                model.Command.Parameters.AddWithValue("@TIPO_COBRO", item.TIPO_COBRO);
                model.Command.Parameters.AddWithValue("@ID_DETALLE_VENTA", 0);
                model.Command.Parameters.AddWithValue("@MTIPO", item.MTIPO);
                result = model.GetData<Caja_BE>();
            }
            return result;
        }
    }
}
