using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ventas_BE
{
    public class FEL_BE
    {
        public int MTIPO { get; set; }
        public int ID_VENTA { get; set; }
        public string USUARIO_FEL { get; set; }
        public string USUARIO_PFX { get; set; }
        public string LLAVE_FEL { get; set; }
        public string LLAVE_PFX { get; set; }
        public string SERIE { get; set; }
        public int CORRELATIVO { get; set; }
        public int NO_ESTABLECIMIENTO { get; set; }
        public string RESOLUCION { get; set; }
        public DateTime FECHA_FACTURA { get; set; }
        public string NIT_CLIENTE { get; set; }
        public string NOMBRE_CLIENTE { get; set; }
        public string DIRECCION_CLIENTE { get; set; }
        public string DIRECCION_ESTABLECIMIENTO { get; set; }
        public string TOTAL { get; set; }
        public string NIT_EMPRESA { get; set; }
        public string MOTIVO { get; set; }
        public string SERIE_FEL { get; set; }
        public decimal NUMERO_FEL { get; set; }
        public string UUID { get; set; }
        public int PRODUCTO { get; set; }
        public string DESCRIPCION_PRODUCTO { get; set; }
        public decimal PRECIO { get; set; }
        public int CANTIDAD { get; set; }
        public decimal DESCUENTO { get; set; }
        public decimal DESCUENTO_UNITARIO { get; set; }
        public string NOMBRE_EMPRESA { get; set; }
        public string MensajeRespuesta { get; set; }
        public string OPERACION { get; set; }
        public string SERIE_NC { get; set; }
        public string TOTAL_NOTA { get; set; }
        public string NO_NC { get; set; }
        public string FECHA_NOTA { get; set; }
        public string UUID_ANULA { get; set; }
        public int ESTADO { get; set; }
        public string NO_ELECTRONICO { get; set; }
        public string RESOLUCION_FAC { get; set; }
        public string TOTAL_IVA_NC { get; set; }
        public string CORREO_EMISOR { get; set; }
        public string CODIGO_POSTAL { get; set; }
        public string PAIS { get; set; }
        public string DEPARTAMENTO { get; set; }
        public string MUNICIPIO { get; set; }
        public string IDENTIFICADOR { get; set; }
        public string IDENTIFICADOR_UNICO { get; set; }
        public decimal TOTAL_IVA { get; set; }
        public decimal TOTAL_SIN_IVA { get; set; }
        public decimal TOTAL_DESCUENTO { get; set; }
        public decimal TOTAL_CON_IVA { get; set; }
        public decimal TOTAL_VENTA { get; set; }
        public decimal PRECIO_UNITARIO { get; set; }
        public decimal SUBTOTAL_CON_IVA { get; set; }
        public int ID_CLIENTE { get; set; }
        public DateTime? FECHA_CERTIFICACION { get; set; }
        public bool RESULTADO  { get; set; }
        public string MENSAJE_FEL { get; set; }
        public int ID_PRODUCTO{ get; set; }
        public string CODIGO_INTERNO{ get; set; }
        public decimal IVA_UNITARIO { get; set; }
    }
}
