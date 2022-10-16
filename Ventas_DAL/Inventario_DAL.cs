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
                //model.Command.Parameters.AddWithValue("@ESTANTE", item.ESTANTERIA);
                model.Command.Parameters.AddWithValue("@NIVEL", item.NIVEL);
                model.Command.Parameters.AddWithValue("@CREADO_POR", item.CREADO_POR);
                model.Command.Parameters.AddWithValue("@ID_CATEGORIA", item.ID_CATEGORIA);
                model.Command.Parameters.AddWithValue("@ID_MODELO", item.ID_MODELO);
                model.Command.Parameters.AddWithValue("@ID_TIPO", item.ID_TIPO);
                model.Command.Parameters.AddWithValue("@ID_BODEGA", item.ID_BODEGA);
                model.Command.Parameters.AddWithValue("@MTIPO", item.MTIPO);
                result = model.GetData<Inventario_BE>();
            }
            return result;
        }
    }
}
