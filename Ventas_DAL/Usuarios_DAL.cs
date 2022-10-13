using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ventas_BE;

namespace Ventas_DAL
{
    public class Usuarios_DAL:IDisposable
    {
        public void Dispose() { }
        public List<Usuarios_BE> GuardarUsuario(Usuarios_BE item)
        {
            List<Usuarios_BE> result = new List<Usuarios_BE>();
            using (var model = new Base_SQL("sp_guardar_usuario"))
            {
                model.Command.Parameters.AddWithValue("@PRIMER_NOMBRE", item.PRIMER_NOMBRE);
                model.Command.Parameters.AddWithValue("@SEGUNDO_NOMBRE", item.SEGUNDO_NOMBRE);
                model.Command.Parameters.AddWithValue("@PRIMER_APELLIDO", item.PRIMER_APELLIDO);
                model.Command.Parameters.AddWithValue("@SEGUNDO_APELLIDO", item.SEGUNDO_APELLIDO);
                model.Command.Parameters.AddWithValue("@DIRECCION", item.DIRECCION);
                model.Command.Parameters.AddWithValue("@TELEFONO", item.TELEFONO);
                model.Command.Parameters.AddWithValue("@ID_TIPO_EMPLEADO", item.ID_TIPO_EMPLEADO);
                model.Command.Parameters.AddWithValue("@EMAIL", item.EMAIL);
                model.Command.Parameters.AddWithValue("@CREADO_POR", item.CREADO_POR);
                result = model.GetData<Usuarios_BE>();
            }
            return result;
        }
    }
}
