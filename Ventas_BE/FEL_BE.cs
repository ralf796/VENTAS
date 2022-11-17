using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ventas_BE
{
    public class FEL_BE
    {
        public string USUARIO_FEL { get; set; }
        public string USUARIO_PFX { get; set; }
        public string LLAVE_FEL { get; set; }
        public string LLAVE_PFX { get; set; }

        #region DatosFacturaFEL
        public string SERIE { get; set; }
        public int NO_FACTURA { get; set; }
        public string RESOLUCION { get; set; }
        public string FECHA_FACTURA { get; set; }
        public string NIT { get; set; }
        public string NOMBRE_CLIENTE { get; set; }
        public string DIRECCION { get; set; }
        public string TOTAL_FACTURA { get; set; }
        public string TOTAL_IVA { get; set; }
        public string TOTAL_IDP { get; set; }
        public string TOTAL_ISR { get; set; }
        public int NO_FACTURA_ACTUAL { get; set; }
        public int CLIENTE { get; set; }
        public string CORREO { get; set; }
        public string TELEFONO { get; set; }
        #endregion

        #region DatosAnulaFEL
        public int EMPRESA { get; set; }
        public string SERIE_DOC { get; set; }
        public int NO_DOC { get; set; }
        public string RESOLUCION_DOC { get; set; }
        public string FECHA_DOC { get; set; }
        public string NIT_EMPRESA { get; set; }
        public string MOTIVO { get; set; }
        public string UUID { get; set; }
        public string CUI { get; set; }
        #endregion

        #region DatosDetFacturaFEL
        public int PRODUCTO { get; set; }
        public string DESCRIPCION_PRODUCTO { get; set; }
        public decimal PRECIO { get; set; }
        public decimal CANTIDAD { get; set; }
        public decimal DESCUENTO { get; set; }
        public string UNIDAD_MEDIDA { get; set; }
        public string IDP_LINEA { get; set; }

        #endregion

        #region EMPRESA
        public string NOMBRE_EMPRESA { get; set; }
        public string FAX { get; set; }
        public string CALL_CENTER { get; set; }
        #endregion

        #region RESPUESTA
        public string MensajeRespuesta { get; set; }
        public string OPERACION { get; set; }
        #endregion
    }
}
