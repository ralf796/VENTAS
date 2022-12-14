using System;
using System.Collections.Generic;
using System.Linq;
using Ventas_BE;
using Ventas_BLL;
using conectorfelv2;
using System.Text;
using Newtonsoft.Json;
using RestSharp;

namespace Ventas.Class
{
    public class Certificador_FEL
    {
        #region BD
        private static List<FEL_BE> GetDatosSP_(FEL_BE item)
        {
            List<FEL_BE> lista = new List<FEL_BE>();
            lista = FEL_BLL.GetDatosSP(item);
            return lista;
        }
        #endregion

        #region PROCESO DE FIRMAS
        public static string Certificador_Local_FAC_FEL(FEL_BE VENTA)
        {
            //CREDENCIALES FEL
            var item = new FEL_BE();
            item.MTIPO = 1;
            var CREDENCIALES_FEL = GetDatosSP_(item).FirstOrDefault();

            //DATOS EMPRESA LOCAL
            item = new FEL_BE();
            item.MTIPO = 2;
            var DATOS_EMPRESA = GetDatosSP_(item).FirstOrDefault();

            //DATOS ENCABEZADO VENTA
            item = new FEL_BE();
            item.MTIPO = 3;
            item.ID_VENTA = VENTA.ID_VENTA;
            var ENCABEZADO_VENTA = GetDatosSP_(item).FirstOrDefault();

            //DETALLES VENTA
            List<FEL_BE> DETALLES_VENTA = new List<FEL_BE>();
            item = new FEL_BE();
            item.MTIPO = 4;
            item.ID_VENTA = VENTA.ID_VENTA;
            DETALLES_VENTA = GetDatosSP_(item);

            conectorfelv2.RequestCertificacionFel request = new RequestCertificacionFel();
            string response;
            bool Datos_generales, Datos_emisor, Datos_receptor, Frases;
            bool Item_un_impuesto, Total_impuestos, Totales, Adenda, Agregar_adenda;

            Datos_generales = request.Datos_generales("GTQ", ENCABEZADO_VENTA.FECHA_FACTURA.ToString("yyyy-MM-dd hh:mm:ss"), "FACT", "", "", "");
            Datos_emisor = request.Datos_emisor("GEN", DATOS_EMPRESA.NO_ESTABLECIMIENTO, DATOS_EMPRESA.CODIGO_POSTAL, DATOS_EMPRESA.CORREO_EMISOR, DATOS_EMPRESA.PAIS, DATOS_EMPRESA.DEPARTAMENTO, DATOS_EMPRESA.MUNICIPIO, DATOS_EMPRESA.DIRECCION_ESTABLECIMIENTO, DATOS_EMPRESA.NIT_EMPRESA, DATOS_EMPRESA.NOMBRE_EMPRESA, DATOS_EMPRESA.NOMBRE_EMPRESA);
            Datos_receptor = request.Datos_receptor(ENCABEZADO_VENTA.NIT_CLIENTE.Replace("-", "").Replace("/", "").Trim(), ENCABEZADO_VENTA.NOMBRE_CLIENTE, "12002", "alejandrolopez445@gmail.com", DATOS_EMPRESA.PAIS, DATOS_EMPRESA.DEPARTAMENTO, DATOS_EMPRESA.MUNICIPIO, ENCABEZADO_VENTA.DIRECCION_CLIENTE, "");
            //Datos_receptor = request.Datos_receptor("89693515", "Alejandro López", "12002", "ingenieriaiosgt@gmail.com", "GT", "GUATEMALA", "GUATEMALA", "CUIDAD", "");

            Frases = request.Frases(1, 1, "", "");

            int rowDetalles = 1;
            foreach (var row in DETALLES_VENTA)
            {
                Item_un_impuesto = request.Item_un_impuesto("B", "UND", row.CANTIDAD.ToString(), row.DESCRIPCION_PRODUCTO, rowDetalles, row.PRECIO_UNITARIO.ToString("N2"), row.TOTAL_CON_IVA.ToString("N2"), "0", row.TOTAL_CON_IVA.ToString("N2"), "IVA", 1, "", row.TOTAL_SIN_IVA.ToString("N2"), row.TOTAL_IVA.ToString("N2"));
                rowDetalles++;
            }
            Total_impuestos = request.total_impuestos("IVA", ENCABEZADO_VENTA.TOTAL_IVA.ToString("N2"));
            Totales = request.Totales(ENCABEZADO_VENTA.TOTAL_CON_IVA.ToString("N2"));

            var randomNumber = new Random().Next(0, 100);
            Adenda = request.Adendas("Codigo_cliente", ENCABEZADO_VENTA.ID_CLIENTE.ToString());//Información Adicional
            Adenda = request.Adendas("Observaciones", $"SE APLICÓ UN DESCUENTO EN TIENDA POR Q {ENCABEZADO_VENTA.TOTAL_DESCUENTO.ToString("N2")}");
            Agregar_adenda = request.Agregar_adendas();
            response = request.enviar_peticion_fel(CREDENCIALES_FEL.USUARIO_FEL, CREDENCIALES_FEL.LLAVE_FEL, ENCABEZADO_VENTA.IDENTIFICADOR + randomNumber.ToString(), "ingenieriaiosgt@gmail.com", CREDENCIALES_FEL.USUARIO_PFX, CREDENCIALES_FEL.LLAVE_PFX, true);
            return response;
        }
        public static FEL_BE Certificador_XML_FAC_FEL(FEL_BE VENTA)
        {
            FEL_BE RESPUESTA_FEL = new FEL_BE();

            //CREDENCIALES FEL
            var item = new FEL_BE();
            item.MTIPO = 1;
            var CREDENCIALES_FEL = GetDatosSP_(item).FirstOrDefault();

            //DATOS EMPRESA LOCAL
            item = new FEL_BE();
            item.MTIPO = 2;
            var DATOS_EMPRESA = GetDatosSP_(item).FirstOrDefault();

            //DATOS ENCABEZADO VENTA
            item = new FEL_BE();
            item.MTIPO = 3;
            item.ID_VENTA = VENTA.ID_VENTA;
            var ENCABEZADO_VENTA = GetDatosSP_(item).FirstOrDefault();

            //DETALLES VENTA
            List<FEL_BE> DETALLES_VENTA = new List<FEL_BE>();
            item = new FEL_BE();
            item.MTIPO = 4;
            item.ID_VENTA = VENTA.ID_VENTA;
            DETALLES_VENTA = GetDatosSP_(item);

            StringBuilder xml = new StringBuilder();
            #region XML Documento
            xml.AppendLine($"<dte:GTDocumento xmlns:ds=\"http://www.w3.org/2000/09/xmldsig#\" xmlns:dte=\"http://www.sat.gob.gt/dte/fel/0.2.0\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" Version=\"0.1\" xsi:schemaLocation=\"http://www.sat.gob.gt/dte/fel/0.2.0\">");
            xml.AppendLine($"<dte:SAT ClaseDocumento=\"dte\">");
            xml.AppendLine($"<dte:DTE ID=\"DatosCertificados\">");
            xml.AppendLine($"<dte:DatosEmision ID=\"DatosEmision\">");
            xml.AppendLine($"<dte:DatosGenerales CodigoMoneda=\"GTQ\" FechaHoraEmision=\"{ENCABEZADO_VENTA.FECHA_FACTURA.ToString("yyyy-MM-ddThh:mm:ss-06:00")}\" Tipo=\"FACT\" ></dte:DatosGenerales>");
            xml.AppendLine($"<dte:Emisor AfiliacionIVA=\"GEN\" CodigoEstablecimiento=\"{1}\" CorreoEmisor=\"{DATOS_EMPRESA.CORREO_EMISOR}\" NITEmisor=\"{DATOS_EMPRESA.NIT_EMPRESA}\" NombreComercial=\"{DATOS_EMPRESA.NOMBRE_EMPRESA}\" NombreEmisor=\"{DATOS_EMPRESA.NOMBRE_EMPRESA}\">");
            xml.AppendLine($"<dte:DireccionEmisor>");
            xml.AppendLine($"<dte:Direccion>{DATOS_EMPRESA.DIRECCION_ESTABLECIMIENTO}</dte:Direccion>");
            xml.AppendLine($"<dte:CodigoPostal>" + DATOS_EMPRESA.CODIGO_POSTAL + "</dte:CodigoPostal>");
            xml.AppendLine($"<dte:Municipio>{DATOS_EMPRESA.MUNICIPIO}</dte:Municipio>");
            xml.AppendLine($"<dte:Departamento>{DATOS_EMPRESA.DEPARTAMENTO}</dte:Departamento>");
            xml.AppendLine($"<dte:Pais>GT</dte:Pais>");
            xml.AppendLine($"</dte:DireccionEmisor>");
            xml.AppendLine($"</dte:Emisor>");
            xml.AppendLine($"<dte:Receptor CorreoReceptor=\"{ENCABEZADO_VENTA.CORREO_CLIENTE ?? DATOS_EMPRESA.CORREO_EMISOR}\" IDReceptor=\"{ENCABEZADO_VENTA.NIT_CLIENTE}\" NombreReceptor=\"{ENCABEZADO_VENTA.NOMBRE_CLIENTE}\">");
            xml.AppendLine($"<dte:DireccionReceptor>");
            xml.AppendLine($"<dte:Direccion>{ENCABEZADO_VENTA.DIRECCION_CLIENTE}</dte:Direccion>");
            xml.AppendLine($"<dte:CodigoPostal>" + DATOS_EMPRESA.CODIGO_POSTAL + "</dte:CodigoPostal>");
            xml.AppendLine($"<dte:Municipio>" + DATOS_EMPRESA.MUNICIPIO + "</dte:Municipio>");
            xml.AppendLine($"<dte:Departamento>" + DATOS_EMPRESA.DEPARTAMENTO + "</dte:Departamento>");
            xml.AppendLine($"<dte:Pais>" + DATOS_EMPRESA.PAIS + "</dte:Pais>");
            xml.AppendLine($"</dte:DireccionReceptor>");
            xml.AppendLine($"</dte:Receptor>");
            xml.AppendLine($"<dte:Frases>");
            xml.AppendLine($"<dte:Frase CodigoEscenario=\"1\" TipoFrase=\"1\"></dte:Frase>");
            xml.AppendLine($"</dte:Frases>");
            xml.AppendLine($"<dte:Items>");

            int rowDetalles = 1;
            foreach (var row in DETALLES_VENTA)
            {
                decimal cantidad = row.CANTIDAD;
                decimal precioUnitario = row.PRECIO_UNITARIO;
                decimal subtotal = cantidad * precioUnitario;
                decimal descuento = row.DESCUENTO;
                decimal total = (subtotal - descuento);
                decimal montoGravable = total / (decimal)1.12;
                decimal iva = montoGravable * (decimal)0.12;
                decimal totalSinIva = montoGravable / (decimal)1.12;

                xml.AppendLine($"<dte:Item NumeroLinea=\"{rowDetalles}\" BienOServicio =\"B\">");
                xml.AppendLine($"<dte:Cantidad>{cantidad.ToString("0.00")}</dte:Cantidad>");
                xml.AppendLine($"<dte:UnidadMedida>UND</dte:UnidadMedida>");
                xml.AppendLine($"<dte:Descripcion>{row.DESCRIPCION_PRODUCTO}</dte:Descripcion>");
                xml.AppendLine($"<dte:PrecioUnitario>{precioUnitario.ToString("0.00")}</dte:PrecioUnitario>");
                xml.AppendLine($"<dte:Precio>{subtotal.ToString("0.00")}</dte:Precio>");
                xml.AppendLine($"<dte:Descuento>{descuento.ToString("0.00")}</dte:Descuento>");
                xml.AppendLine($"<dte:Impuestos>");
                xml.AppendLine($"<dte:Impuesto>");
                xml.AppendLine($"<dte:NombreCorto>IVA</dte:NombreCorto>");
                xml.AppendLine($"<dte:CodigoUnidadGravable>1</dte:CodigoUnidadGravable>");
                xml.AppendLine($"<dte:MontoGravable>{montoGravable.ToString("0.00")}</dte:MontoGravable>");
                xml.AppendLine($"<dte:MontoImpuesto>{iva.ToString("0.00")}</dte:MontoImpuesto>");
                xml.AppendLine($"</dte:Impuesto>");
                xml.AppendLine($"</dte:Impuestos>");
                xml.AppendLine($"<dte:Total>{total.ToString("0.00")}</dte:Total>");
                xml.AppendLine($"</dte:Item>");
                rowDetalles++;
            }
            xml.AppendLine($"</dte:Items>");
            xml.AppendLine($"<dte:Totales>");
            xml.AppendLine($"<dte:TotalImpuestos>");
            xml.AppendLine($"<dte:TotalImpuesto TotalMontoImpuesto=\"{ENCABEZADO_VENTA.TOTAL_IVA.ToString("0.00")}\" NombreCorto =\"IVA\" /> ");
            xml.AppendLine($"</dte:TotalImpuestos>");
            xml.AppendLine($"<dte:GranTotal>{ENCABEZADO_VENTA.TOTAL_CON_IVA.ToString("0.00")}</dte:GranTotal>");
            xml.AppendLine($"</dte:Totales>");

            xml.AppendLine($"</dte:DatosEmision>");
            xml.AppendLine($"</dte:DTE>");
            xml.AppendLine($"<dte:Adenda>");
            xml.AppendLine($"<Codigo_cliente>C01</Codigo_cliente>");
            xml.AppendLine($"<Observaciones></Observaciones>");
            xml.AppendLine($"</dte:Adenda>");
            xml.AppendLine($"</dte:SAT>");
            xml.AppendLine($"</dte:GTDocumento>");
            #endregion

            #region Firma XML
            byte[] xmlData = Encoding.UTF8.GetBytes(xml.ToString());
            string xmlBase64 = Convert.ToBase64String(xmlData);
            RestClient restClient = new RestClient("https://signer-emisores.feel.com.gt/sign_solicitud_firmas/firma_xml?");
            FEL_SIGNER_REQUEST signedRequest = new FEL_SIGNER_REQUEST
            {
                LLAVE = CREDENCIALES_FEL.LLAVE_PFX,
                ARCHIVO = xmlBase64,
                CODIGO = ENCABEZADO_VENTA.IDENTIFICADOR_UNICO + "-2",
                ALIAS = CREDENCIALES_FEL.USUARIO_FEL,
                ES_ANULACION = "N"
            };
            var restRequest = new RestRequest();
            restRequest.Method = Method.Post;
            restRequest.AddJsonBody(signedRequest);
            var response = restClient.Execute(restRequest);
            FEL_SIGNER_RESPONSE CONEXION_INFILE = JsonConvert.DeserializeObject<FEL_SIGNER_RESPONSE>(response.Content);

            if (CONEXION_INFILE.RESULTADO)
            {
                FEL_CERTIFICACION_REQUEST felRequest = new FEL_CERTIFICACION_REQUEST
                {
                    CORREO_COPIA = DATOS_EMPRESA.CORREO_EMISOR,
                    NIT_EMISOR = DATOS_EMPRESA.NIT_EMPRESA,
                    XML_DTE = CONEXION_INFILE.ARCHIVO
                };

                restClient = new RestClient("https://certificador.feel.com.gt/fel/certificacion/v2/dte/");
                restRequest = new RestRequest();
                restRequest.Method = Method.Post;
                restRequest.AddHeader("Usuario", CREDENCIALES_FEL.USUARIO_FEL);
                restRequest.AddHeader("Llave", CREDENCIALES_FEL.LLAVE_FEL);
                restRequest.AddHeader("Identificador", ENCABEZADO_VENTA.IDENTIFICADOR_UNICO + "-2");
                restRequest.AddJsonBody(felRequest);

                response = restClient.Execute(restRequest);
                FEL_CERTIFICACION_RESPONSE RESPUESTA_INFILE = JsonConvert.DeserializeObject<FEL_CERTIFICACION_RESPONSE>(response.Content);
                RESPUESTA_FEL.RESULTADO = RESPUESTA_INFILE.RESULTADO;

                if (RESPUESTA_FEL.RESULTADO)
                {
                    RESPUESTA_FEL.ID_VENTA = VENTA.ID_VENTA;
                    RESPUESTA_FEL.UUID = RESPUESTA_INFILE.UUID;
                    RESPUESTA_FEL.SERIE_FEL = RESPUESTA_INFILE.SERIE;
                    RESPUESTA_FEL.FECHA_CERTIFICACION = RESPUESTA_INFILE.FECHA;
                    RESPUESTA_FEL.NUMERO_FEL = Convert.ToDecimal(RESPUESTA_INFILE.NUMERO);
                }
                else
                {
                    RESPUESTA_FEL.MENSAJE_FEL = "Documento FEL No generado. " + response.Content.ToString();
                }
            }
            else
            {
                RESPUESTA_FEL.MENSAJE_FEL = $"Firma de XML no procesada {CONEXION_INFILE.DESCRIPCION}";
            }
            #endregion
            return RESPUESTA_FEL;
        }
        public static FEL_BE Anulador_XML_FEL(FEL_BE VENTA)
        {
            //CREDENCIALES FEL
            var item = new FEL_BE();
            item.MTIPO = 1;
            var CREDENCIALES_FEL = GetDatosSP_(item).FirstOrDefault();

            //DATOS VENTA
            item = new FEL_BE();
            item.MTIPO = 7;
            item.ID_VENTA = VENTA.ID_VENTA;
            var DATOS_VENTA = GetDatosSP_(item).FirstOrDefault();

            //DATOS EMPRESA LOCAL
            item = new FEL_BE();
            item.MTIPO = 2;
            var DATOS_EMPRESA = GetDatosSP_(item).FirstOrDefault();

            FEL_BE RESPUESTA_FEL = new FEL_BE();

            #region XML
            StringBuilder xml = new StringBuilder();
            xml.AppendLine($"<dte:GTAnulacionDocumento xmlns:ds=\"http://www.w3.org/2000/09/xmldsig#\" xmlns:dte=\"http://www.sat.gob.gt/dte/fel/0.1.0\" xmlns:n1=\"http://www.altova.com/samplexml/other-namespace\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" Version=\"0.1\" xsi:schemaLocation=\"http://www.sat.gob.gt/dte/fel/0.1.0 C:\\Users\\User\\Desktop\\FEL\\Esquemas\\GT_AnulacionDocumento-0.1.0.xsd\">");
            xml.AppendLine($"<dte:SAT>");
            xml.AppendLine($"<dte:AnulacionDTE ID=\"DatosCertificados\">");
            xml.AppendLine($"<dte:DatosGenerales FechaEmisionDocumentoAnular=\"{Convert.ToDateTime(DATOS_VENTA.FECHA_CERTIFICACION).ToString("yyyy-MM-ddThh:mm:ss-06:00")}\" FechaHoraAnulacion=\"{DateTime.Now.ToString("yyyy-MM-ddThh:mm:ss-06:00")}\" ID=\"DatosAnulacion\" IDReceptor=\"{DATOS_VENTA.NIT_CLIENTE}\" MotivoAnulacion=\"PRUEBA DE ANULACIÓN\" NITEmisor=\"{DATOS_EMPRESA.NIT_EMPRESA}\" NumeroDocumentoAAnular=\"{DATOS_VENTA.UUID}\"></dte:DatosGenerales>");
            xml.AppendLine($"</dte:AnulacionDTE>");
            xml.AppendLine($"</dte:SAT>");
            xml.AppendLine($"</dte:GTAnulacionDocumento>");
            #endregion

            #region Firma XML
            //Firma XML
            byte[] xmlData = Encoding.UTF8.GetBytes(xml.ToString());
            string xmlBase64 = Convert.ToBase64String(xmlData);
            RestClient restClient = new RestClient("https://signer-emisores.feel.com.gt/sign_solicitud_firmas/firma_xml?");
            FEL_SIGNER_REQUEST signedRequest = new FEL_SIGNER_REQUEST
            {
                LLAVE = CREDENCIALES_FEL.LLAVE_PFX,
                ARCHIVO = xmlBase64,
                CODIGO = VENTA.IDENTIFICADOR_UNICO,
                ALIAS = CREDENCIALES_FEL.USUARIO_FEL,
                ES_ANULACION = "S"
            };
            var restRequest = new RestRequest();
            restRequest.Method = Method.Post;
            restRequest.AddJsonBody(signedRequest);
            var response = restClient.Execute(restRequest);
            #endregion

            FEL_SIGNER_RESPONSE CONEXION_INFILE = JsonConvert.DeserializeObject<FEL_SIGNER_RESPONSE>(response.Content);

            if (CONEXION_INFILE.RESULTADO)
            {
                FEL_CERTIFICACION_REQUEST felRequest = new FEL_CERTIFICACION_REQUEST
                {
                    CORREO_COPIA = VENTA.CORREO_EMISOR,
                    NIT_EMISOR = VENTA.NIT_EMPRESA,
                    XML_DTE = CONEXION_INFILE.ARCHIVO
                };

                restClient = new RestClient("https://certificador.feel.com.gt/fel/anulacion/v2/dte/");
                restRequest = new RestRequest();
                restRequest.Method = Method.Post;
                restRequest.AddHeader("Usuario", CREDENCIALES_FEL.USUARIO_FEL);
                restRequest.AddHeader("Llave", CREDENCIALES_FEL.LLAVE_FEL);
                restRequest.AddHeader("Identificador", DATOS_VENTA.IDENTIFICADOR_UNICO + "-2");
                restRequest.AddJsonBody(felRequest);
                response = restClient.Execute(restRequest);

                FEL_CERTIFICACION_RESPONSE RESPUESTA_INFILE = JsonConvert.DeserializeObject<FEL_CERTIFICACION_RESPONSE>(response.Content);
                if (RESPUESTA_INFILE.RESULTADO)
                {
                    RESPUESTA_FEL.UUID = RESPUESTA_INFILE.UUID;
                    RESPUESTA_FEL.SERIE_FEL = RESPUESTA_INFILE.SERIE;
                    RESPUESTA_FEL.NUMERO_FEL = Convert.ToDecimal(RESPUESTA_INFILE.NUMERO);
                }
                else
                {
                    RESPUESTA_FEL.MENSAJE_FEL = $"Documento FEL No anulado. {response.Content}";
                }
            }
            else
            {
                RESPUESTA_FEL.MENSAJE_FEL = $"Firma de XML no procesada {CONEXION_INFILE.DESCRIPCION}";
            }
            return RESPUESTA_FEL;
        }
        #endregion

        #region CONSTRUCTORES FEL
        public class FEL_SIGNER_REQUEST
        {
            public string LLAVE { get; set; }
            public string ARCHIVO { get; set; }
            public string CODIGO { get; set; }
            public string ALIAS { get; set; }
            public string ES_ANULACION { get; set; }
        }
        public class FEL_SIGNER_RESPONSE
        {
            public bool RESULTADO { get; set; }
            public string DESCRIPCION { get; set; }
            public string ARCHIVO { get; set; }
        }
        public class FEL_CERTIFICACION_REQUEST
        {
            public string NIT_EMISOR { get; set; }
            public string CORREO_COPIA { get; set; }
            public string XML_DTE { get; set; }
        }
        public class FEL_CERTIFICACION_RESPONSE
        {
            public bool RESULTADO { get; set; }
            public DateTime FECHA { get; set; }
            public string UUID { get; set; }
            public string SERIE { get; set; }
            public string NUMERO { get; set; }
        }
        #endregion
    }
}