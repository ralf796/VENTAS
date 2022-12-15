using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ventas_BE
{
    public class Cartera_BE
    {
        public int MTIPO { get; set; }
        public int ID_VENTA { get; set; }
        public string SERIE { get; set; }
        public decimal CORRELATIVO { get; set; }
        public string IDENTIFICADOR_UNICO { get; set; }
        public DateTime FECHA { get; set; }
        public DateTime FECHA_CERTIFICACION { get; set; }
        public string FECHA_STRING { get; set; }
        public string FECHA_CERTIFICACION_STRING { get; set; }
        public string ESTADO { get; set; }
        public string UUID { get; set; }
        public string SERIE_FEL { get; set; }
        public decimal NUMERO_FEL { get; set; }
        public string NIT { get; set; }
        public string NOMBRE { get; set; }
        public string DIRECCION { get; set; }
        public decimal TOTAL_VENTA { get; set; }
        public decimal SALDO { get; set; }
        public string FORMA_PAGO { get; set; }
    }
}
