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
                model.Command.Parameters.AddWithValue("@ID_VENTA", item.ID_VENTA);
                model.Command.Parameters.AddWithValue("@SERIE", item.SERIE);
                model.Command.Parameters.AddWithValue("@CORRELATIVO", item.CORRELATIVO);
                model.Command.Parameters.AddWithValue("@ID_CLIENTE", item.ID_CLIENTE);
                model.Command.Parameters.AddWithValue("@TOTAL", item.TOTAL);
                model.Command.Parameters.AddWithValue("@SUBTOTAL", item.SUBTOTAL);
                model.Command.Parameters.AddWithValue("@TOTAL_DESCUENTO", item.TOTAL_DESCUENTO);
                model.Command.Parameters.AddWithValue("@PRECIO", item.PRECIO_VENTA);
                model.Command.Parameters.AddWithValue("@CANTIDAD", item.CANTIDAD);
                model.Command.Parameters.AddWithValue("@ID_PRODUCTO", item.ID_PRODUCTO);
                model.Command.Parameters.AddWithValue("@CREADO_POR", item.CREADO_POR);
                model.Command.Parameters.AddWithValue("@nombreModelo", item.NOMBRE_MODELO);
                model.Command.Parameters.AddWithValue("@nombreMarcaVehiculo", item.NOMBRE_MARCA_VEHICULO);
                model.Command.Parameters.AddWithValue("@nombreLineaVehiculo", item.NOMBRE_LINEA_VEHICULO);
                result = model.GetData<Ventas__BE>();
            }
            return result;
        }
    }
}
