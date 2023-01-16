using System;

namespace Ventas_BE
{
    public class Devoluciones_BE
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
        public string CREADO_POR { get; set; }
        public string OBSERVACIONES { get; set; }
        public DateTime FECHA_VENTA { get; set; }
        public string FECHA_VENTA_STRING { get; set; }
        public int ID_CLIENTE { get; set; }
        public string VENTA_CREADO_POR { get; set; }
        public int ID_VENTA_DETALLE { get; set; }
        public int ID_PRODUCTO { get; set; }
        public decimal PRECIO_UNITARIO{ get; set; }
        public decimal TOTAL{ get; set; }
        public decimal DESCUENTO{ get; set; }
        public decimal SUBTOTAL{ get; set; }
        public string CODIGO_INTERNO{ get; set; }
        public string CODIGO{ get; set; }
        public string CODIGO2{ get; set; }
        public string NOMBRE_PRODUCTO{ get; set; }
        public string NOMBRE_MARCA_VEHICULO{ get; set; }
        public int CANTIDAD{ get; set; }
    }
}
