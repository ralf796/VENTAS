using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ventas_BE
{
    public class Reportes_BE
    {
        public DateTime FECHA_INICIAL { get; set; }
        public DateTime FECHA_FINAL { get; set; }
        public DateTime FECHA_CREACION { get; set; }
        public DateTime FECHA_CORTE { get; set; }
        public int ID_VENTA { get; set; }
        public int ID_COBRO { get; set; }
        public int ID_CORTE { get; set; }
        public decimal TOTAL { get; set; }
        public string CREADO_POR { get; set; }
        public int ESTADO { get; set; }
        public int MTIPO { get; set; }
        public string FECHA_CREACION_STRING { get; set; }
        public decimal TOTAL_VENTA { get; set; }
        public string NOMBRE { get; set; }
        public int CANTIDAD { get; set; }
        public decimal MONTO { get; set; }
        public string SERIE { get; set; }
        public decimal SUBTOTAL { get; set; }
        public decimal TOTAL_DESCUENTO { get; set; }
        public string DESCRIPCION { get; set; }
        public decimal PRECIO_COSTO { get; set; }
        public decimal PRECIO_VENTA { get; set; }
        public int STOCK { get; set; }
        public string CODIGO { get; set; }
        public string CODIGO2 { get; set; }
        public string TIPO_COBRO { get; set; }
        public decimal TOTAL_EFECTIVO { get; set; }
        public decimal TOTAL_TARJETA { get; set; }
        public string CAJERO_CORTE { get; set; }
        public string CAJERO_COBRO { get; set; }
        public decimal CORRELATIVO { get; set; }
        public string NIT { get; set; }
        public string TELEFONO { get; set; }
        public string DIRECCION { get; set; }
        public string NOMBRE_PRODUCTO { get; set; }
        public decimal PRECIO_UNITARIO { get; set; }
        public decimal DESCUENTO { get; set; }
        public string RESPUESTA { get; set; }
        public string NOMBRE_COMPLETO { get; set; }
        public string DESCRIPCION_BITACORA { get; set; }
        public string ESTADO_STRING { get; set; }
        public string NOMBRE_MARCA_VEHICULO { get; set; }
        public string NOMBRE_SERIE_VEHICULO { get; set; }
        public decimal GANANCIA { get; set; }
        public int ANIO_INICIAL { get; set; }
        public int ANIO_FINAL { get; set; }
        public string NOMBRE_CLIENTE { get; set; }
        public string TIPO_REPORTE { get; set; }
        public string FECHA_VENTA_STRING { get; set; }
        public DateTime FECHA_VENTA { get; set; }
        public string CODIGO_INTERNO { get; set; }
    }
}
