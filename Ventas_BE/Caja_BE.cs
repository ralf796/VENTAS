using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ventas_BE
{
    public class Caja_BE
    {
        public int ID_VENTA { get; set; }
        public int ID_CLIENTE { get; set; }
        public string NOMBRE { get; set; }
        public decimal TOTAL { get; set; }
        public string CREADO_POR { get; set; }
        public int MTIPO { get; set; }
        public int ESTADO { get; set; }
        public int CANTIDAD { get; set; }
        public decimal PRECIO_UNITARIO { get; set; }

        public decimal DESCUENTO { get; set; }
        public decimal SUBTOTAL { get; set; }
        public string TIPO_COBRO { get; set; }
        public DateTime FECHA_CREACION { get; set; }
        public string FECHA_CREACION_STRING { get; set; }
        public decimal TOTAL_VENTA2 { get; set; }
        public string SERIE { get; set; }
        public decimal CORRELATIVO { get; set; }
        public string NIT { get; set; }
        public string DIRECCION { get; set; }
        public decimal MONTO_TARJETA { get; set; }
        public decimal MONTO_EFECTIVO { get; set; }
        public string RESPUESTA { get; set; }

    }
}
