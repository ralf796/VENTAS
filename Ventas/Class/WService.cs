using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ventas.Class
{
    public class WService
    {
        public class login
        {
            public string USUARIO { get; set; }
            public string CONTRASENA { get; set; }
        }

        public class Reimpresion
        {
            public string SERIE { get; set; }
            public int NO_FACTURA { get; set; }
            public string NO_RESOLUCION { get; set; }
        }

        public class DatosUsuario : BaseRespuesta
        {
            public string nombre { get; set; }
            public string apellido { get; set; }
            public string token { get; set; }
            public string puntoventa { get; set; }
            public string canalventa { get; set; }
            public string plantausuario { get; set; }
            public string empresaplanta { get; set; }
            public long corte { get; set; }
            public long COD_CDV { get; set; }
            public String NO_AFILIACION { get; set; }
            public String LIQUIDA_CAMION { get; set; }
        }

        public class BaseRespuesta
        {
            public string MensajeRespuesta { get; set; }
            public string OPERACION { get; set; }
        }

        public class empresas
        {
            public int EMPRESA { get; set; }
            public string NOMBRE_EMPRESA { get; set; }
            public string NIT { get; set; }
            public string DIRECCION { get; set; }
            public string TELEFONO { get; set; }
            public string FAX { get; set; }
            public string CALL_CENTER { get; set; }
        }

        public class Cliente
        {
            public decimal PRECIO { get; set; }
            public int CLIENTE { get; set; }
            public string FORMA_PAGO { get; set; }
            public string NOMBRE_CLIENTE { get; set; }
            public string NIT { get; set; }
            public string DIRECCION { get; set; }
            public int PRODUCTO { get; set; }
            public string DESCRIPCION { get; set; }
            /// <summary>
            /// Token de validacion del usuario.
            /// </summary>
            public string TOKEN { get; set; }
            public string CREDITO_BLOQUEADO { get; set; }
            public string FECHA_ULTIMA_FACTURA { get; set; }
            public decimal PRECIO_ULTIMA_FACTURA { get; set; }
            public int ULTMA_EMPRESA_FACTURADA { get; set; }
            public string NOMBRE_ULTIMA_EMPRESA_FACTURADA { get; set; }
            public int CANAL_DISTRIBUCION { get; set; }
        }

        public class DatosDelCliente : BaseRespuesta
        {
            public List<empresas> ListaEmpresa { get; set; }
            public Cliente cliente { get; set; }
        }

        public class DatosTropiFactura : BaseRespuesta
        {
            public string RES { get; set; }
            public string FIRMA_ELECTRONICA { get; set; }
            public string FECHA_FACTURA { get; set; }
            public string NO_FACTURA { get; set; }
            public string NOMBRE_CLIENTE { get; set; }
            public string NIT { get; set; }
            public string DIRECCION { get; set; }
            public decimal IDP { get; set; }
            public decimal PRECIO_UNITARIO { get; set; }
            public decimal TOTAL_FACTURA { get; set; }
            public string TOTAL_LETRAS { get; set; }
            public string EMPRESA_AGENTE_RETENDOR { get; set; }
            public string NIT_GFACE { get; set; }
            public string NOMBRE_GFACE { get; set; }
            public string URL_GFACE { get; set; }
            public string NOMBRE_PILOTO { get; set; }
            public int CODIGO_REFERENCIA { get; set; }
            public string FORMA_PAGO { get; set; }
            public string SERIE_FEL { get; set; }
            public decimal NUMERO_FEL { get; set; }
            public string FECHA_CERTIFICACION { get; set; }
            public String PORCENTAJE_TANQUE { get; set; }
            public Decimal HOROMETRO { get; set; }
            public String SERIE { get; set; }
            public String FINCA { get; set; }
            public long NO_FICHA { get; set; }
            public String NOMBRE_RECIBE { get; set; }
            public String FRASE_1 { get; set; }
            public String FRASE_2 { get; set; }
            public String FRASE_3 { get; set; }
        }

        public class DatosFacturacion
        {
            public string TOKEN { get; set; }
            public string USUARIO { get; set; }
            public int EMPRESA { get; set; }
            public int CLIENTE { get; set; }
            public string NOMBRE_CLIENTE { get; set; }
            public string NIT { get; set; }
            public string DIRECCION { get; set; }
            public int PRODUCTO { get; set; }
            public decimal CANTIDAD { get; set; }
            public decimal PRECIO { get; set; }
            public string FORMA_PAGO { get; set; }
            public string DESCRIPCION { get; set; }
            public string LATITUD { get; set; }
            public string LONGITUD { get; set; }
            public String PORCENTAJE_TANQUE { get; set; }
            public Decimal HOROMETRO { get; set; }
            public String SERIE { get; set; }
            public String FINCA { get; set; }
            public long NO_FICHA { get; set; }
            public String NOMBRE_RECIBE { get; set; }
        }

        public class ReimpresionFactura
        {
            public string TIPO_ACTIVO { get; set; }
            public string SERIE { get; set; }
            public string NO_FACTURA { get; set; }
            public string NIT_VENDEDOR { get; set; }
            public int EMPRESA { get; set; }
            public string NOMBRE_EMPRESA { get; set; }
            public string DIRECCION_EMPRESA { get; set; }
            public string TELEFONO { get; set; }
            public string CALL_CENTER { get; set; }
            public string NOMBRE_ESTABLECIMIENTO { get; set; }
            public string DIRECCION_ESTABLECIMIENTO { get; set; }
            public int NO_ESTABLECIMIENTO { get; set; }
            public string NO_RESOLUCION { get; set; }
            public string FECHA_AUTORIZACION { get; set; }
            public int RANGO_INICIAL { get; set; }
            public int RANGO_FINAL { get; set; }
            public string FECHA_FACTURA { get; set; }
            public string NIT { get; set; }
            public string NOMBRE_CLIENTE { get; set; }
            public string DIRECCION { get; set; }
            public string FIRMA_ELECTRONICA { get; set; }
            public string DESCRIPCION { get; set; }
            public decimal CANTIDAD { get; set; }
            public decimal PRECIO_UNITARIO { get; set; }
            public decimal IDP { get; set; }
            public decimal SUBTOTAL { get; set; }
            public string EMPLEADO { get; set; }
            public string TOTAL_LETRAS { get; set; }
            public string AGENTE_RETENEDOR { get; set; }
            public string NOMBRE_PILOTO { get; set; }
            public string FORMA_PAGO { get; set; }
            public string NOMBRE_GFACE { get; set; }
            public string NIT_GFACE { get; set; }
            public string URL_GFACE { get; set; }
            public string MensajeRespuesta { get; set; }
            public string OPERACION { get; set; }
            public Int32 CODIGO_REFERENCIA { get; set; }
            public int SEGMENTACION { get; set; }
            public String UUID { get; set; }
            public String SERIE_FEL { get; set; }
            public long NUMERO_FEL { get; set; }
            public String FECHA_EMISION { get; set; }
            public String FECHA_CERTIFICACION { get; set; }
            public String FRASE_1 { get; set; }
            public String FRASE_2 { get; set; }
            public String FRASE_3 { get; set; }
            public Decimal PORCENTAJE_TANQUE { get; set; }
            public Decimal HOROMETRO { get; set; }
            public String SERIE_LECTURA { get; set; }
            public String FINCA { get; set; }
            public long NO_FICHA { get; set; }
            public String NOMBRE_RECIBE { get; set; }
        }

        public class UltimaVenta
        {
            public string FECHA { get; set; }
            public decimal PRECIO_UNITARIO { get; set; }
        }

        public class FacturasAnuladas
        {
            public string SERIE_ANULADA { get; set; }
            public int NO_FAC_ANULADA { get; set; }
            public string NO_RESOLUCION { get; set; }
        }


        public class DatosAnulaFEL
        {
            public int EMPRESA { get; set; }
            public string SERIE_DOC { get; set; }
            public int NO_DOC { get; set; }
            public string RESOLUCION_DOC { get; set; }
            public string FECHA_DOC { get; set; }
            public string NIT { get; set; }
            public string NIT_EMPRESA { get; set; }
            public string MOTIVO { get; set; }
            public string UUID { get; set; }
            public string CUI { get; set; }
        }

        public class DatosFacturaFEL
        {
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
        }

        public class DatosDetFacturaFEL
        {
            public int PRODUCTO { get; set; }
            public string DESCRIPCION_PRODUCTO { get; set; }
            public decimal PRECIO { get; set; }
            public decimal CANTIDAD { get; set; }
            public decimal DESCUENTO { get; set; }
            public string UNIDAD_MEDIDA { get; set; }
            public string IDP_LINEA { get; set; }
        }

        public class DatosNCFEL
        {
            public string SERIE { get; set; }
            public int NO_NC { get; set; }
            public string RESOLUCION { get; set; }
            public string FECHA_NC { get; set; }
            public string NIT { get; set; }
            public string NOMBRE_CLIENTE { get; set; }
            public string DIRECCION { get; set; }
            public string TOTAL_NC { get; set; }
            public string TOTAL_IVA_NC { get; set; }
            public string TOTAL_IDP_NC { get; set; }
            public int EMPRESA { get; set; }
            public string FECHA_FAC { get; set; }
            public string MOTIVO { get; set; }
            public string UUID_ANULA { get; set; }
            public decimal TOTAL_GALONES { get; set; }
            public int ESTADO { get; set; }
            public string SERIE_FAC { get; set; }
            public int NO_FAC { get; set; }
            public string NO_RESOLUCION { get; set; }
            public string NO_ELECTRONICO { get; set; }
            public string RESOLUCION_FAC { get; set; }
        }

        public class DatosNotaAbono
        {
            public string SERIE { get; set; }
            public string NO_DOCUMENTO { get; set; }
            public string SERIE_FAC { get; set; }
            public string RESOLUCION_FAC { get; set; }
            public string RESOLUCION { get; set; }
            public string TOTAL { get; set; }
            public int EMPRESA { get; set; }
            public string FECHA { get; set; }
            public string NIT { get; set; }
            public string NOMBRE_CLIENTE { get; set; }
            public string DIRECCION { get; set; }
            public int PLANTA_USUARIO { get; set; }
        }

        public class DatosNA
        {
            public string SERIE { get; set; }
            public int NO_DOCUMENTO { get; set; }
            public string RESOLUCION { get; set; }
            public string FECHA_DOCUMENTO { get; set; }
            public string TOTAL { get; set; }
        }

        public class DatosNotasCredito
        {
            public List<FacturasAnuladas> List_Fac_Anuladas { get; set; }
            //public List<Detalle_XML> ListaDetalles { get; set; }
            public string SERIE_NOTA { get; set; }
            public int NO_NOTA { get; set; }
            public string RESOLUCION { get; set; }
            public string FECHA { get; set; }
            public string NIT { get; set; }
            public string NOMBRE_CLIENTE { get; set; }
            public string DIRECCION { get; set; }
            public string DESCRIPCION { get; set; }
            public string CODIGO_PRODUCTO { get; set; }
            public decimal SUBTOTAL_SIN_IVA_SIN_IDP { get; set; }
            public decimal CANTIDAD_GALONES { get; set; }
            public decimal PRECIO { get; set; }
            public decimal IVA { get; set; }
            public decimal IDP { get; set; }
            public decimal TOTAL_IMPUESTOS { get; set; }
            /// <summary>
            /// Monto total de la nota de credito.
            /// </summary>
            public decimal TOTAL { get; set; }
            /// <summary>
            /// Forma de pago de la factura aplicada en la nota de credito.
            /// </summary>
            public string FORMA_PAGO { get; set; }
            /// <summary>
            /// Firma electronica de la nota de credito.
            /// </summary>
            public string FIRMA_ELECTRONICA { get; set; }
            /// <summary>
            /// Codigo interno del cliente.
            /// </summary>
            public int CLIENTE { get; set; }
            /// <summary>
            /// Codigo del empleado que genera la nota de credito.
            /// </summary>
            public string EMPLEADO { get; set; }
            /// <summary>
            /// Serie de la factura a la que se aplico la nota de credito.
            /// </summary>
            public string SERIE_FAC_ANULADA { get; set; }
            /// <summary>
            /// Numero de la factura a la que se le aplico la nota de credito.
            /// </summary>
            public int NO_FAC_ANULADA { get; set; }
            /// <summary>
            /// Total de libras de la nota de credito.
            /// </summary>
            public decimal TOTAL_LIBRAS { get; set; }
            /// <summary>
            /// Tipo de activo.
            /// </summary>
            public string TIPO_ACITVO { get; set; }
            /// <summary>
            /// Tipo de documento.
            /// </summary>
            public string TIPO_DOCUMENTO { get; set; }
            /// <summary>
            /// Usuario que genera la nota de credito.
            /// </summary>
            public string USUARIO { get; set; }
            /// <summary>
            /// Numero electronico de la nota de credito.
            /// </summary>
            public string NO_ELECTRONICO { get; set; }

            public string DESCRIPCION_NC { get; set; }
        }

        /// <summary>
        /// Clase property para cortes.
        /// </summary>
        public class ListaCortes
        {
            /// <summary>
            /// Codigo del empleado que realiza el corte.
            /// </summary>
            public string EMPLEADO { get; set; }
            /// <summary>
            /// Numero del corte.
            /// </summary>
            public int NO_CORTE { get; set; }
        }

        /// <summary>
        /// Datos para enviar las opciones de facturacion
        /// </summary>
        public class OpcionesFac
        {
            /// <summary>
            /// Codigo cliente interno
            /// </summary>
            public int CLIENTE { get; set; }
            /// <summary>
            /// Nombre del cliente
            /// </summary>
            public string NOMBRE_CLIENTE { get; set; }
            /// <summary>
            /// Nit del cliente
            /// </summary>
            public string NIT { get; set; }
            /// <summary>
            /// Direccion del cliente
            /// </summary>
            public string DIRECCION { get; set; }

        }

        /// <summary>
        /// Clase para enviar la lista de opciones a facturar
        /// </summary>
        public class listaOpciones : BaseRespuesta
        {
            /// <summary>
            /// Listado de las opciones que tiene el cliente para facturar
            /// </summary>
            public List<OpcionesFac> ListaFacturacion { get; set; }
        }

    }
}