using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ventas_BE;

namespace Ventas_DAL
{
    public class Inventario_DAL : IDisposable
    {
        public void Dispose() { }
        public List<Inventario_BE> GetSPInventario(Inventario_BE item)
        {
            List<Inventario_BE> result = new List<Inventario_BE>();
            using (var model = new Base_SQL("sp_inventario"))
            {
                model.Command.Parameters.AddWithValue("@NOMBRE", item.NOMBRE);
                model.Command.Parameters.AddWithValue("@DESCRIPCION", item.DESCRIPCION);
                model.Command.Parameters.AddWithValue("@CREADO_POR", item.CREADO_POR);
                model.Command.Parameters.AddWithValue("@ID_PRODUCTO", item.ID_PRODUCTO);
                model.Command.Parameters.AddWithValue("@ID_MODELO", item.ID_MODELO);
                model.Command.Parameters.AddWithValue("@ID_BODEGA", item.ID_BODEGA);
                model.Command.Parameters.AddWithValue("@ID_SUBCATEGORIA", item.ID_SUBCATEGORIA);
                model.Command.Parameters.AddWithValue("@ID_PROVEEDOR", item.ID_PROVEEDOR);
                model.Command.Parameters.AddWithValue("@ID_MARCA_REPUESTO", item.ID_MARCA_REPUESTO);
                model.Command.Parameters.AddWithValue("@ID_SERIE_VEHICULO", item.ID_SERIE_VEHICULO);
                model.Command.Parameters.AddWithValue("@PRECIO_COSTO", item.PRECIO_COSTO);
                model.Command.Parameters.AddWithValue("@PRECIO_VENTA", item.PRECIO_VENTA);
                model.Command.Parameters.AddWithValue("@STOCK", item.STOCK);
                model.Command.Parameters.AddWithValue("@CODIGO", item.CODIGO);
                model.Command.Parameters.AddWithValue("@CODIGO2", item.CODIGO2);
                model.Command.Parameters.AddWithValue("@NOMBRE_MARCA_REPUESTO", item.NOMBRE_MARCA_REPUESTO);
                model.Command.Parameters.AddWithValue("@NOMBRE_MARCA_VEHICULO", item.NOMBRE_MARCA_VEHICULO);
                model.Command.Parameters.AddWithValue("@NOMBRE_LINEA_VEHICULO", item.NOMBRE_SERIE_VEHICULO);
                model.Command.Parameters.AddWithValue("@NOMBRE_DISTRIBUIDOR", item.NOMBRE_DISTRIBUIDOR);
                model.Command.Parameters.AddWithValue("@PATH", item.PATH_IMAGEN);
                model.Command.Parameters.AddWithValue("@ANIO_INICIAL", item.ANIO_INICIAL);
                model.Command.Parameters.AddWithValue("@ANIO_FINAL", item.ANIO_FINAL);
                model.Command.Parameters.AddWithValue("@MTIPO", item.MTIPO);
                result = model.GetData<Inventario_BE>();
            }
            return result;
        }
        public List<Inventario_BE> GetInventario_select(Inventario_BE item)
        {
            List<Inventario_BE> result = new List<Inventario_BE>();
            using (var model = new Base_SQL("sp_inventario_select"))
            {
                model.Command.Parameters.AddWithValue("@MTIPO", item.MTIPO);
                model.Command.Parameters.AddWithValue("@ID", item.ID_UPDATE);
                model.Command.Parameters.AddWithValue("@nombreModelo", item.NOMBRE_MODELO);
                model.Command.Parameters.AddWithValue("@nombreMarcaVehiculo", item.NOMBRE_MARCA_VEHICULO);
                model.Command.Parameters.AddWithValue("@nombreLineaVehiculo", item.NOMBRE_LINEA_VEHICULO);
                model.Command.Parameters.AddWithValue("@ANIO_INICIAL", item.ANIO_INICIAL);
                model.Command.Parameters.AddWithValue("@ANIO_FINAL", item.ANIO_FINAL);
                result = model.GetData<Inventario_BE>();
            }
            return result;
        }
        public List<Inventario_BE> GetInventario_delete(Inventario_BE item)
        {
            List<Inventario_BE> result = new List<Inventario_BE>();
            using (var model = new Base_SQL("sp_inventario_delete"))
            {
                model.Command.Parameters.AddWithValue("@ID", item.ID_DELETE);
                model.Command.Parameters.AddWithValue("@MTIPO", item.MTIPO);
                result = model.GetData<Inventario_BE>();
            }
            return result;
        }
        public List<Inventario_BE> GetSPCompras(Inventario_BE item)
        {
            List<Inventario_BE> result = new List<Inventario_BE>();
            using (var model = new Base_SQL("sp_compras"))
            {
                model.Command.Parameters.AddWithValue("@MTIPO", item.MTIPO);
                model.Command.Parameters.AddWithValue("@ID_COMPRA", item.ID_PRODUCTO);
                model.Command.Parameters.AddWithValue("@NOMBRE_PROVEEDOR", item.NOMBRE_PROVEEDOR);
                model.Command.Parameters.AddWithValue("@CONTACTO_PROVEEDOR", item.CONTACTO_PROVEEDOR);
                model.Command.Parameters.AddWithValue("@FECHA_PEDIDO", item.FECHA_PEDIDO);
                model.Command.Parameters.AddWithValue("@FECHA_PAGO", item.FECHA_PAGO);
                model.Command.Parameters.AddWithValue("@FECHA_ENTREGA", item.FECHA_ENTREGA);
                model.Command.Parameters.AddWithValue("@TELEFONO_PROVEEDOR", item.TELEFONO);
                model.Command.Parameters.AddWithValue("@NO_FACTURA", item.NO_FACTURA);
                model.Command.Parameters.AddWithValue("@MONTO_FACTURA", item.MONTO_FACTURA);
                model.Command.Parameters.AddWithValue("@SERIE_FACTURA", item.SERIE_FACTURA);
                model.Command.Parameters.AddWithValue("@CREADO_POR", item.CREADO_POR);
                model.Command.Parameters.AddWithValue("@FILE1", item.FILE1);
                model.Command.Parameters.AddWithValue("@FILE2", item.FILE2);
                result = model.GetData<Inventario_BE>();
            }
            return result;
        }
    }
}
