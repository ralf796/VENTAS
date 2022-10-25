using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ventas_BE
{
    public class Clientes_BE
    {

        public int? ID_CLIENTE { get; set; }
        public string NOMBRE { get; set; }
        public string DIRECCION { get; set; }
        public string TELEFONO { get; set; }
        public string EMAIL { get; set; }
        public string NIT { get; set; }
        public string CREADO_POR { get; set; }
        public int? MTIPO { get; set; }
        public int? ESTADO { get; set; }
        public string RESPUESTA { get; set; }

    }
}
