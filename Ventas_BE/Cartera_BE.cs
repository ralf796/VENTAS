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
        public DateTime? FECHA_PAGO { get; set; }
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
        public string CREADO_POR { get; set; }
        public decimal ABONO { get; set; }
        public string OBSERVACIONES{ get; set; }
        public int NO_RECIBO { get; set; }
        public DateTime FECHA_CREACION_RECIBO { get; set; }
        public DateTime FECHA_CREACION_CREDITO { get; set; }
        public string FECHA_CREACION_CREDITO_STRING { get; set; }
        public decimal MONTO_RECIBO { get; set; }
        public string RECIBO_CREADO_POR { get; set; }
        public DateTime FECHA_VENTA { get; set; }
        public string FECHA_VENTA_STRING { get; set; }
        public int ID_ESTADO_CUENTA { get; set; }
        public int ID_CLIENTE { get; set; }
        public string TELEFONO{ get; set; }
        public string CREDITO_CREADO_POR{ get; set; }
        public string VENTA_CREADO_POR{ get; set; }
        public string FECHA_PAGO_STRING { get; set; }
        public int DIAS_ATRASO{ get; set; }
    }
}
