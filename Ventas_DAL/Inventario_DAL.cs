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
                model.Command.Parameters.AddWithValue("@ESTANTE", item.ESTANTERIA);
                model.Command.Parameters.AddWithValue("@NIVEL", item.NIVEL);
                model.Command.Parameters.AddWithValue("@CREADO_POR", item.CREADO_POR);
                model.Command.Parameters.AddWithValue("@ID_PRODUCTO", item.ID_PRODUCTO);
                model.Command.Parameters.AddWithValue("@ID_CATEGORIA", item.ID_CATEGORIA);
                model.Command.Parameters.AddWithValue("@ID_MODELO", item.ID_MODELO);
                model.Command.Parameters.AddWithValue("@ID_BODEGA", item.ID_BODEGA);
                model.Command.Parameters.AddWithValue("@PRECIO_COSTO", item.PRECIO_COSTO);
                model.Command.Parameters.AddWithValue("@PRECIO_VENTA", item.PRECIO_VENTA);
                model.Command.Parameters.AddWithValue("@STOCK", item.STOCK);
                model.Command.Parameters.AddWithValue("@CODIGO", item.CODIGO);
                model.Command.Parameters.AddWithValue("@ID_SUBCATEGORIA", item.ID_SUBCATEGORIA);
                model.Command.Parameters.AddWithValue("@ID_PROVEEDOR", item.ID_PROVEEDOR);
                model.Command.Parameters.AddWithValue("@ID_MARCA_REPUESTO", item.ID_MARCA_REPUESTO);
                model.Command.Parameters.AddWithValue("@ID_SERIE_VEHICULO", item.ID_SERIE_VEHICULO);
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
                result = model.GetData<Inventario_BE>();
            }
            return result;
        }
        public List<Inventario_BE> GetInventario_create(Inventario_BE item)
        {
            List<Inventario_BE> result = new List<Inventario_BE>();
            using (var model = new Base_SQL("sp_inventario_create"))
            {
                model.Command.Parameters.AddWithValue("@NOMBRE", item.NOMBRE);
                model.Command.Parameters.AddWithValue("@DESCRIPCION", item.DESCRIPCION);
                model.Command.Parameters.AddWithValue("@ESTANTE", item.ESTANTERIA);
                model.Command.Parameters.AddWithValue("@NIVEL", item.NIVEL);
                model.Command.Parameters.AddWithValue("@ANIO_INICIAL", item.ANIO_INICIAL);
                model.Command.Parameters.AddWithValue("@ANIO_FINAL", item.ANIO_FINAL);
                model.Command.Parameters.AddWithValue("@CREADO_POR", item.CREADO_POR);
                model.Command.Parameters.AddWithValue("@ID_CATEGORIA", item.ID_CATEGORIA);
                model.Command.Parameters.AddWithValue("@ID_MARCA_VEHICULO", item.ID_MARCA_VEHICULO);
                model.Command.Parameters.AddWithValue("@MTIPO", item.MTIPO);
                result = model.GetData<Inventario_BE>();
            }
            return result;
        }
        public List<Inventario_BE> GetInventario_update(Inventario_BE item)
        {
            List<Inventario_BE> result = new List<Inventario_BE>();
            using (var model = new Base_SQL("sp_inventario_create"))
            {
                model.Command.Parameters.AddWithValue("@NOMBRE", item.NOMBRE);
                model.Command.Parameters.AddWithValue("@DESCRIPCION", item.DESCRIPCION);
                model.Command.Parameters.AddWithValue("@ESTANTE", item.ESTANTERIA);
                model.Command.Parameters.AddWithValue("@NIVEL", item.NIVEL);
                model.Command.Parameters.AddWithValue("@ANIO_INICIAL", item.ANIO_INICIAL);
                model.Command.Parameters.AddWithValue("@ANIO_FINAL", item.ANIO_FINAL);
                model.Command.Parameters.AddWithValue("@CREADO_POR", item.CREADO_POR);
                model.Command.Parameters.AddWithValue("@ID_CATEGORIA", item.ID_CATEGORIA);
                model.Command.Parameters.AddWithValue("@ID_MARCA_VEHICULO", item.ID_MARCA_VEHICULO);
                model.Command.Parameters.AddWithValue("@MTIPO", item.MTIPO);
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
    }
}
