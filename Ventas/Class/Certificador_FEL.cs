using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services.Description;
using Ventas_BE;
using Ventas_BLL;
using conectorfelv2;
using static Ventas.Class.WService;
using System.Web.Razor.Tokenizer;
using System.Text;
using Newtonsoft.Json;
using System.Web.Http.Results;
using RestSharp;
using System.Drawing;
using System.Threading.Tasks;
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

        public static string Firmar_Factura(FEL_BE VENTA, string moneda = "GTQ")
        {
            //apifel4.RequestCertificacionFel request = new RequestCertificacionFel();
            bool Item_un_impuesto, Total_impuestos, Totales, Adenda, Agregar_adenda;
            int NoEstablecimiento;
            string FechaEmision, NombreEmpresa, NITEmpresa, DireccionEstablecimiento, NIT_Receptor, Nombre_Receptor, Direccion_Receptor, DescripcionProducto;
            string UnidadMedida, TotalFactura, TotalIVA, UsuarioFEL, UsuarioPFX, LlaveFEL, LlavePFX, ID_DOC;
            decimal Cantidad, PrecioUnitario, TotalLinea, TotalSinIVA, IVA, DescuentoLinea;

            //CREDENCIALES FEL
            var item = new FEL_BE();
            item.MTIPO = 1;
            var CREDENCIALES_FEL = GetDatosSP_(item).FirstOrDefault();
            UsuarioFEL = CREDENCIALES_FEL.USUARIO_FEL;
            UsuarioPFX = CREDENCIALES_FEL.USUARIO_PFX;
            LlaveFEL = CREDENCIALES_FEL.LLAVE_FEL;
            LlavePFX = CREDENCIALES_FEL.LLAVE_PFX;

            //DATOS EMPRESA LOCAL
            item = new FEL_BE();
            item.MTIPO = 2;
            var EMPRESA_EDEN = GetDatosSP_(item).FirstOrDefault();
            NoEstablecimiento = EMPRESA_EDEN.NO_ESTABLECIMIENTO;
            NombreEmpresa = EMPRESA_EDEN.NOMBRE_EMPRESA;
            DireccionEstablecimiento = EMPRESA_EDEN.DIRECCION_ESTABLECIMIENTO;
            NITEmpresa = EMPRESA_EDEN.NIT_EMPRESA;

            //DATOS ENCABEZADO VENTA
            item = new FEL_BE();
            item.MTIPO = 3;
            item.ID_VENTA = VENTA.ID_VENTA;
            var ENCABEZADO_VENTA = GetDatosSP_(item).FirstOrDefault();
            ID_DOC = ENCABEZADO_VENTA.SERIE + "-" + ENCABEZADO_VENTA.CORRELATIVO.ToString();
            //FechaEmision = ENCABEZADO_VENTA.FECHA.ToString();
            NIT_Receptor = ENCABEZADO_VENTA.NIT_CLIENTE.Replace("-", "").Replace("/", "").Trim();
            Nombre_Receptor = ENCABEZADO_VENTA.NOMBRE_CLIENTE.Replace("\"", ""); ;
            Direccion_Receptor = ENCABEZADO_VENTA.DIRECCION_CLIENTE;
            TotalFactura = ENCABEZADO_VENTA.TOTAL;
            //TotalIVA = ENCABEZADO_VENTA.TOTAL_IVA;

            /*
            //// Datos_generales = request.Datos_generales("GTQ", FechaEmision, "FACT", "", "");   
            Datos_generales = request.Datos_generales(moneda, FechaEmision, "FACT", "", "");
            Datos_emisor = request.Datos_emisor("GEN", NoEstablecimiento, "01001", "", "GT", "GUATEMALA", "GUATEMALA", DireccionEstablecimiento, NITEmpresa, NombreEmpresa, NombreEmpresa);
            Datos_receptor = request.Datos_receptor(NIT_Receptor, Nombre_Receptor, "01001", "alejandrolopez445@gmail.com", "GT", "GUATEMALA", "GUATEMALA", Direccion_Receptor, "");
            */

            //FRASES FEL
            item = new FEL_BE();
            item.MTIPO = 4;
            var FRASES_REQUEST = GetDatosSP_(item).FirstOrDefault();

            //DETALLES VENTA
            List<FEL_BE> DETALLES_VENTA = new List<FEL_BE>();
            item = new FEL_BE();
            item.MTIPO = 4;
            DETALLES_VENTA = GetDatosSP_(item);

            int rowDetalles = 1;
            foreach (var row in DETALLES_VENTA)
            {
                Cantidad = row.CANTIDAD;
                DescripcionProducto = row.DESCRIPCION_PRODUCTO;
                PrecioUnitario = row.PRECIO;
                TotalLinea = row.PRECIO * row.CANTIDAD;
                TotalSinIVA = Math.Round((TotalLinea / (decimal)1.12), 2);
                DescuentoLinea = (decimal)row.DESCUENTO;
                IVA = Math.Round(((TotalLinea / (decimal)1.12) * (decimal)0.12), 2);
                //Item_un_impuesto = request.Item_un_impuesto("B", "UNIDAD", Cantidad.ToString(), DescripcionProducto, rowDetalles, PrecioUnitario.ToString(), TotalLinea.ToString(), DescuentoLinea.ToString(), TotalLinea.ToString(), "IVA", 1, "", TotalSinIVA.ToString(), IVA.ToString());
                rowDetalles++;
            }

            /*  PROCESOS FEL
            Total_impuestos = request.total_impuestos("IVA", TotalIVA);
            Totales = request.Totales(TotalFactura);

            Adenda = request.Adendas("Codigo_cliente", "C01");//Información Adicional
            Adenda = request.Adendas("Observaciones", "");

            Agregar_adenda = request.Agregar_adendas();
            response = request.enviar_peticion_fel(UsuarioFEL, LlaveFEL, ID_DOC, "", UsuarioPFX, LlavePFX, true);
            return response;
            */
            return "";
        }
        public static string Firma_NC(FEL_BE ENCABEZADO)
        {
            //apifel4.RequestCertificacionFel requestNC = new RequestCertificacionFel();
            string UsuarioFEL, UsuarioPFX, LlaveFEL, LlavePFX, ID_DOC, response = "", FechaEmision, NombreEmpresa, NITEmpresa, DireccionEstablecimiento, NIT_Receptor, Nombre_Receptor, Direccion_Receptor, TotalNC, TotalIVA_NC, TOTAL_IDP_NC, FechaFac, Motivo, UUID_Anula;
            int NoEstablecimiento, ESTADO;
            decimal TotalSinIVA;

            string NO_ELECTRONICO;
            string RESOLUCION_FAC;

            //CREDENCIALES FEL
            var item = new FEL_BE();
            item.MTIPO = 1;
            var CREDENCIALES_FEL = GetDatosSP_(item).FirstOrDefault();
            UsuarioFEL = CREDENCIALES_FEL.USUARIO_FEL;
            UsuarioPFX = CREDENCIALES_FEL.USUARIO_PFX;
            LlaveFEL = CREDENCIALES_FEL.LLAVE_FEL;
            LlavePFX = CREDENCIALES_FEL.LLAVE_PFX;

            //DATOS EMPRESA LOCAL
            item = new FEL_BE();
            item.MTIPO = 2;
            var EMPRESA_EDEN = GetDatosSP_(item).FirstOrDefault();
            NoEstablecimiento = EMPRESA_EDEN.NO_ESTABLECIMIENTO;
            NombreEmpresa = EMPRESA_EDEN.NOMBRE_EMPRESA;
            DireccionEstablecimiento = EMPRESA_EDEN.DIRECCION_ESTABLECIMIENTO;
            NITEmpresa = EMPRESA_EDEN.NIT_EMPRESA;

            ID_DOC = ENCABEZADO.SERIE_NC + "-" + ENCABEZADO.NO_NC.ToString();
            FechaEmision = ENCABEZADO.FECHA_NOTA;
            NIT_Receptor = ENCABEZADO.NIT_CLIENTE.Replace("-", "").Replace("/", "").Trim();
            Nombre_Receptor = ENCABEZADO.NOMBRE_CLIENTE;
            Direccion_Receptor = ENCABEZADO.DIRECCION_CLIENTE;
            TotalNC = ENCABEZADO.TOTAL_NOTA;

            TOTAL_IDP_NC = "0";
            //TotalSinIVA = decimal.Parse(TotalNC) - decimal.Parse(TOTAL_IDP_NC) - decimal.Parse(TotalIVA_NC);
            //FechaFac = ENCABEZADO.FECHA_FACTURA;
            Motivo = ENCABEZADO.MOTIVO;
            UUID_Anula = ENCABEZADO.UUID_ANULA;
            //TOTAL_GALONES = 0;
            ESTADO = ENCABEZADO.ESTADO;

            NO_ELECTRONICO = ENCABEZADO.NO_ELECTRONICO;
            RESOLUCION_FAC = ENCABEZADO.RESOLUCION_FAC;

            TotalIVA_NC = ENCABEZADO.TOTAL_IVA_NC;
            TotalSinIVA = decimal.Parse(TotalNC) - decimal.Parse(TOTAL_IDP_NC) - decimal.Parse(TotalIVA_NC);

            /*PROCESO FEL
            Datos_generales = requestNC.Datos_generales("GTQ", FechaEmision, "NCRE", "", "");
            Datos_emisor = requestNC.Datos_emisor("GEN", NoEstablecimiento, "01001", "", "GT", "GUATEMALA", "GUATEMALA", DireccionEstablecimiento, NITEmpresa, NombreEmpresa, NombreEmpresa);
            Datos_receptor = requestNC.Datos_receptor(NIT_Receptor, Nombre_Receptor, "01001", "", "GT", "GUATEMALA", "GUATEMALA", Direccion_Receptor, "");

            Item_un_impuesto = requestNC.Item_un_impuesto("S", "UND", "1", "ANULACION DE FACTURA", 1, TotalNC, TotalNC, "0", TotalNC, "IVA", 1, "", TotalSinIVA.ToString(), TotalIVA_NC);
            Total_impuestos = requestNC.total_impuestos("IVA", TotalIVA_NC);

            Totales = requestNC.Totales(TotalNC);
            Complemento_notas = requestNC.Complemento_notas("Notas", "Notas", "http://www.sat.gob.gt/fel/notas.xsd", FechaFac, Motivo, UUID_Anula, "", "", "");
            response = requestNC.enviar_peticion_fel(UsuarioFEL, LlaveFEL, ID_DOC, "", UsuarioPFX, LlavePFX, true);
            */
            return response;
        }
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
            //Objeto Respuesta
            //conectorfelv2.Respuesta result = new conectorfelv2.Respuesta();
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
            //xml.AppendLine($"<dte:Receptor CorreoReceptor=\"{Cliente.Email ?? Empresa.Email}\" IDReceptor=\"{Cliente.Nit}\" NombreReceptor=\"{Cliente.Nombre}\">");
            xml.AppendLine($"<dte:Receptor CorreoReceptor=\"alejandrolopez445@gmail.com\" IDReceptor=\"{ENCABEZADO_VENTA.NIT_CLIENTE}\" NombreReceptor=\"{ENCABEZADO_VENTA.NOMBRE_CLIENTE}\">");
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
                /*
                int Cantidad = row.CANTIDAD;
                string DescripcionProducto = row.DESCRIPCION_PRODUCTO;
                decimal PrecioUnitario = row.PRECIO;
                decimal TotalLinea = row.PRECIO * row.CANTIDAD - (decimal)row.DESCUENTO;
                decimal precio = row.PRECIO * row.CANTIDAD;
                TotalSinIVA = Math.Round((TotalLinea / (decimal)1.12), 2);
                DescuentoLinea = (decimal)Detalles.DESCUENTO;
                UnidadMedida = Detalles.UNIDAD_MEDIDA;
                IVA = Math.Round(((TotalLinea / (decimal)1.12) * (decimal)0.12), 5);
                */

                //xml.AppendLine($"<dte:Item BienOServicio=\"{(row.Tipo == 1 ? "B" : "S")}\" NumeroLinea=\"{row.Linea}\">");
                xml.AppendLine($"<dte:Item BienOServicio=\"B\" NumeroLinea=\"{rowDetalles}\">");
                xml.AppendLine($"<dte:Cantidad>{row.CANTIDAD.ToString("N2")}</dte:Cantidad>");
                xml.AppendLine($"<dte:UnidadMedida>UND</dte:UnidadMedida>");
                xml.AppendLine($"<dte:Descripcion>{row.DESCRIPCION_PRODUCTO}</dte:Descripcion>");
                xml.AppendLine($"<dte:PrecioUnitario>{row.PRECIO_UNITARIO.ToString("0.00")}</dte:PrecioUnitario>");
                xml.AppendLine($"<dte:Precio>{row.TOTAL_CON_IVA.ToString("0.00")}</dte:Precio>");
                xml.AppendLine($"<dte:Descuento>{row.DESCUENTO_UNITARIO.ToString("0.00")}</dte:Descuento>");
                xml.AppendLine($"<dte:Impuestos>");
                xml.AppendLine($"<dte:Impuesto>");
                xml.AppendLine($"<dte:NombreCorto>IVA</dte:NombreCorto>");
                xml.AppendLine($"<dte:CodigoUnidadGravable>1</dte:CodigoUnidadGravable>");
                xml.AppendLine($"<dte:MontoGravable>{row.TOTAL_SIN_IVA.ToString("0.00")}</dte:MontoGravable>");
                xml.AppendLine($"<dte:MontoImpuesto>{row.TOTAL_IVA.ToString("0.00")}</dte:MontoImpuesto>");
                xml.AppendLine($"</dte:Impuesto>");
                xml.AppendLine($"</dte:Impuestos>");
                xml.AppendLine($"<dte:Total>{row.TOTAL_CON_IVA.ToString("0.00")}</dte:Total>");
                xml.AppendLine($"</dte:Item>");

                /*
                conectorfelv2.RequestCertificacionFel request = new RequestCertificacionFel();
                var tem_un_impuesto = request.Item_un_impuesto("B", "UND", row.CANTIDAD.ToString(), row.DESCRIPCION_PRODUCTO, rowDetalles, row.PRECIO_UNITARIO.ToString("N2"),
                    row.TOTAL_CON_IVA.ToString("N2"), "0", row.TOTAL_CON_IVA.ToString("N2"), "IVA", 1, "", row.TOTAL_SIN_IVA.ToString("N2"), row.TOTAL_IVA.ToString("N2"));
                */
                rowDetalles++;
            }
            xml.AppendLine($"</dte:Items>");
            xml.AppendLine($"<dte:Totales>");
            xml.AppendLine($"<dte:TotalImpuestos>");
            xml.AppendLine($"<dte:TotalImpuesto NombreCorto=\"IVA\" TotalMontoImpuesto=\"{ENCABEZADO_VENTA.TOTAL_IVA.ToString("0.00")}\"></dte:TotalImpuesto>");
            xml.AppendLine($"</dte:TotalImpuestos>");
            xml.AppendLine($"<dte:GranTotal>{ENCABEZADO_VENTA.TOTAL_CON_IVA.ToString("0.00")}</dte:GranTotal>");
            xml.AppendLine($"</dte:Totales>");
            xml.AppendLine($"</dte:DatosEmision>");
            xml.AppendLine($"</dte:DTE>");
            xml.AppendLine($"<dte:Adenda>");
            xml.AppendLine($"<Codigo_cliente>C01</Codigo_cliente>");
            xml.AppendLine($"<Observaciones>ESTA ES UNA ADENDA</Observaciones>");
            xml.AppendLine($"</dte:Adenda>");
            xml.AppendLine($"</dte:SAT>");
            xml.AppendLine($"</dte:GTDocumento>");
            #endregion

            #region Firma XML
            //Firma XML
            byte[] xmlData = Encoding.UTF8.GetBytes(xml.ToString());
            string xmlBase64 = Convert.ToBase64String(xmlData);
            RestClient restClient = new RestClient("https://signer-emisores.feel.com.gt/sign_solicitud_firmas/firma_xml?");
            FELSignerRequest signedRequest = new FELSignerRequest
            {
                llave = CREDENCIALES_FEL.LLAVE_PFX,
                archivo = xmlBase64,
                codigo = ENCABEZADO_VENTA.IDENTIFICADOR_UNICO,
                alias = CREDENCIALES_FEL.USUARIO_FEL,
                es_anulacion = "N"
            };
            var restRequest = new RestRequest();
            restRequest.Method = Method.Post;
            restRequest.AddJsonBody(signedRequest);
            var response = restClient.Execute(restRequest);
            FELSignerResponse signedResponse = JsonConvert.DeserializeObject<FELSignerResponse>(response.Content);

            if (signedResponse.resultado)
            {
                //Item solicitud
                FELCertificacionRequest felRequest = new FELCertificacionRequest
                {
                    correo_copia = DATOS_EMPRESA.CORREO_EMISOR,
                    nit_emisor = DATOS_EMPRESA.NIT_EMPRESA,
                    xml_dte = signedResponse.archivo
                };

                //Certifica
                restClient = new RestClient("https://certificador.feel.com.gt/fel/certificacion/v2/dte/");
                restRequest = new RestRequest();
                restRequest.Method = Method.Post;
                restRequest.AddHeader("Usuario", CREDENCIALES_FEL.USUARIO_FEL);
                restRequest.AddHeader("Llave", CREDENCIALES_FEL.LLAVE_FEL);
                restRequest.AddHeader("Identificador", ENCABEZADO_VENTA.IDENTIFICADOR_UNICO);
                //restRequest.AddHeader("Observaciones", $"SE APLICÓ UN DESCUENTO EN TIENDA POR Q {ENCABEZADO_VENTA.TOTAL_DESCUENTO.ToString("N2")}");
                restRequest.AddJsonBody(felRequest);

                //Adenda = request.Adendas("Observaciones", $"SE APLICÓ UN DESCUENTO EN TIENDA POR Q {ENCABEZADO_VENTA.TOTAL_DESCUENTO.ToString("N2")}");
                response = restClient.Execute(restRequest);

                FELCertificacionResponse felResponse = JsonConvert.DeserializeObject<FELCertificacionResponse>(response.Content);

                RESPUESTA_FEL.RESULTADO = felResponse.resultado;

                if (RESPUESTA_FEL.RESULTADO)
                {
                    RESPUESTA_FEL.ID_VENTA = VENTA.ID_VENTA;
                    RESPUESTA_FEL.UUID = felResponse.uuid;
                    RESPUESTA_FEL.SERIE_FEL = felResponse.serie;
                    RESPUESTA_FEL.FECHA_CERTIFICACION = felResponse.fecha;
                    RESPUESTA_FEL.NUMERO_FEL = Convert.ToDecimal(felResponse.numero);
                }
                else
                {
                    RESPUESTA_FEL.MENSAJE_FEL = "Documento FEL No generado.";
                }
            }
            else
            {
                RESPUESTA_FEL.MENSAJE_FEL = $"Firma de XML no procesada {signedResponse.descripcion}";
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

            //Objeto Respuesta
            //conectorfelv2.Respuesta result = new conectorfelv2.Respuesta();
            FEL_BE RESPUESTA_FEL = new FEL_BE();

            #region XML
            StringBuilder xml = new StringBuilder();
            xml.AppendLine($"<dte:GTAnulacionDocumento xmlns:ds=\"http://www.w3.org/2000/09/xmldsig#\" xmlns:dte=\"http://www.sat.gob.gt/dte/fel/0.1.0\" xmlns:n1=\"http://www.altova.com/samplexml/other-namespace\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" Version=\"0.1\" xsi:schemaLocation=\"http://www.sat.gob.gt/dte/fel/0.1.0 C:\\Users\\User\\Desktop\\FEL\\Esquemas\\GT_AnulacionDocumento-0.1.0.xsd\">");
            xml.AppendLine($"<dte:SAT>");
            xml.AppendLine($"<dte:AnulacionDTE ID=\"DatosCertificados\">");
            xml.AppendLine($"<dte:DatosGenerales FechaEmisionDocumentoAnular=\"{VENTA.FECHA_FACTURA.ToString("yyyy-MM-ddThh:mm:ss-06:00")}\" FechaHoraAnulacion=\"{DateTime.Now.ToString("yyyy-MM-ddThh:mm:ss-06:00")}\" ID=\"DatosAnulacion\" IDReceptor=\"{VENTA.NIT_CLIENTE}\" MotivoAnulacion=\"Anulación de documento electrónico\" NITEmisor=\"{VENTA.NIT_EMPRESA}\" NumeroDocumentoAAnular=\"{VENTA.UUID}\"></dte:DatosGenerales>");
            xml.AppendLine($"</dte:AnulacionDTE>");
            xml.AppendLine($"</dte:SAT>");
            xml.AppendLine($"</dte:GTAnulacionDocumento>");
            #endregion
            #region Firma XML
            //Firma XML
            byte[] xmlData = Encoding.UTF8.GetBytes(xml.ToString());
            string xmlBase64 = Convert.ToBase64String(xmlData);
            RestClient restClient = new RestClient("https://signer-emisores.feel.com.gt/sign_solicitud_firmas/firma_xml?");
            FELSignerRequest signedRequest = new FELSignerRequest
            {
                llave = CREDENCIALES_FEL.LLAVE_PFX,
                archivo = xmlBase64,
                codigo = VENTA.IDENTIFICADOR_UNICO,
                alias = CREDENCIALES_FEL.USUARIO_FEL,
                es_anulacion = "S"
            };
            //restRequest = new RestRequest();
            var restRequest = new RestRequest();
            restRequest.Method = Method.Post;
            restRequest.AddJsonBody(signedRequest);
            var response = restClient.Execute(restRequest);

            #endregion
            FELSignerResponse signedResponse = JsonConvert.DeserializeObject<FELSignerResponse>(response.Content);

            if (signedResponse.resultado)
            {
                //Item solicitud
                FELCertificacionRequest felRequest = new FELCertificacionRequest
                {
                    correo_copia = VENTA.CORREO_EMISOR,
                    nit_emisor = VENTA.NIT_EMPRESA,
                    xml_dte = signedResponse.archivo
                };

                //Certifica
                restClient = new RestClient("https://certificador.feel.com.gt/fel/anulacion/v2/dte/");
                restRequest = new RestRequest();
                restRequest.Method = Method.Post;
                restRequest.AddHeader("Usuario", CREDENCIALES_FEL.USUARIO_FEL);
                restRequest.AddHeader("Llave", CREDENCIALES_FEL.LLAVE_PFX);
                restRequest.AddHeader("Identificador", $"{VENTA.IDENTIFICADOR_UNICO}");
                restRequest.AddJsonBody(felRequest);
                response = restClient.Execute(restRequest);

                FELCertificacionResponse felResponse = JsonConvert.DeserializeObject<FELCertificacionResponse>(response.Content);

                RESPUESTA_FEL.RESULTADO = felResponse.resultado;

                if (RESPUESTA_FEL.RESULTADO)
                {
                    RESPUESTA_FEL.UUID = felResponse.uuid;
                    RESPUESTA_FEL.SERIE_FEL = felResponse.serie;
                    RESPUESTA_FEL.NUMERO_FEL = Convert.ToDecimal(felResponse.numero);
                }
                else
                {
                    RESPUESTA_FEL.MENSAJE_FEL = "Documento FEL No anulado.";
                }
            }
            else
            {
                RESPUESTA_FEL.MENSAJE_FEL = $"Firma de XML no procesada {signedResponse.descripcion}";
            }
            return RESPUESTA_FEL;
        }

        /*
        public static string Firma_Facturas(List<WService.DatosFacturaFEL> facturas, List<WService.DatosDetFacturaFEL> Lista_detalles_fac, string moneda = "GTQ", string tasaCambio = "")
        {
            RequestCertificacionFel request = new RequestCertificacionFel();
            genesysEntities db = new genesysEntities();

            string xml_generado;
            string response = "";
            bool Datos_generales;
            bool Datos_emisor;
            bool Datos_receptor;
            bool Frases;
            bool Item_un_impuesto;
            bool Item_dos_impuesto;
            bool Total_impuestos;
            bool Totales;
            bool Adenda;
            bool Agregar_adenda;
            string FechaEmision;
            int NoEstablecimiento;
            string NombreEmpresa;
            string NITEmpresa;
            string DireccionEstablecimiento;
            string NIT_Receptor;
            string Nombre_Receptor;
            string Direccion_Receptor;
            decimal Cantidad;
            string DescripcionProducto;
            decimal PrecioUnitario;
            decimal TotalLinea;
            decimal TotalSinIVA;
            decimal IVA;
            decimal DescuentoLinea;
            string UnidadMedida;
            string TotalFactura;
            string TotalIVA;
            string TOTAL_IDP;
            string IDP_LINEA;
            int Empresa;
            string UsuarioFEL;
            string UsuarioPFX;
            string LlaveFEL;
            string LlavePFX;
            string ID_DOC;
            decimal valor2 = 0;
            string nombreEstablecimiento;
            WService.DatosFacturaFEL EncabezadoFac;

            Int32 CorrelativoEncabezado = facturas.Count();

            EncabezadoFac = new WService.DatosFacturaFEL();
            EncabezadoFac = facturas[i];


            // VERIFICO SI EL CLIENTE ES DE ZONA FRANCA - aramirez
            int no_factura_new = EncabezadoFac.NO_FACTURA_ACTUAL;
            string consultaZF = " select count(*) as total " +
                    "    from cat_cliente a " +
                    "    where CLIENTE=" + EncabezadoFac.CLIENTE + "  and a.segmentacion = 3 ";

            int clienteZF = db.Database.SqlQuery<int>(consultaZF).FirstOrDefault();
            //int clienteZF = 1;

            var uno = clienteZF;


            // CONSULTA PARA DEVOLVER LOS DATOS DE CADA SERIE Y RESOLUCION
            string consulta = "SELECT A.SERIE AS SERIE, A.NO_RESOLUCION AS RESOLUCION, " +
                                              "        A.RANGO_INICIAL AS R_INICIAL, A.RANGO_FINAL AS R_FINAL, C.NIT AS NIT, " +
                                              "        C.NOMBRE_EMPRESA AS EMPRESA, D.NOMBRE_ESTABLECIMIENTO, " +
                                              "        D.NO_ESTABLECIMINETO AS NO_ESTABLECIMIENTO, D.DIR_ESTABLECIMIENTO AS DIRECCION_ESTABLECIMIENTO, C.EMPRESA AS CODEMPRESA, A.TIPO_ACTIVO " +
                                              "  FROM fac_serie a " +
                                              "  inner join cat_planta b on a.planta = b.planta " +
                                              "  inner join cat_establecimiento d on d.establecimiento = a.establecimiento " +
                                              "  inner join cat_empresa c on d.empresa = c.empresa " +
                                              "  where serie='" + EncabezadoFac.SERIE + "'  AND NO_RESOLUCION = '" + EncabezadoFac.RESOLUCION + "' ";


            Datos_Resolucion_factura resolucion = db.Database.SqlQuery<Datos_Resolucion_factura>(consulta).SingleOrDefault();

            Empresa = Convert.ToInt32(resolucion.CODEMPRESA);
            NITEmpresa = resolucion.NIT;
            //    ID_DOC = EncabezadoFac.SERIE + "-2-" + EncabezadoFac.NO_FACTURA.ToString();

            //ID_DOC = EncabezadoFac.SERIE + "-2-" + EncabezadoFac.NO_FACTURA.ToString();

            ID_DOC = EncabezadoFac.SERIE + "-" + EncabezadoFac.RESOLUCION + "-" + EncabezadoFac.NO_FACTURA.ToString();

            // if (Empresa == 6)
            nombreEstablecimiento = resolucion.NOMBRE_ESTABLECIMIENTO;
            //  else
            //  nombreEstablecimiento = resolucion.EMPRESA;

            //CREDENCIALES FEL
            string consultaFEL = "SELECT USUARIO_FEL AS USUARIO_FEL, USUARIO_PFX AS USUARIO_PFX, LLAVE_FEL AS LLAVE_FEL, LLAVE_PFX AS LLAVE_PFX " +
                                    " FROM CFG_FEL WHERE EMPRESA = " + Empresa;

            CredencialesFEL credenciales = db.Database.SqlQuery<CredencialesFEL>(consultaFEL).SingleOrDefault();

            UsuarioFEL = credenciales.USUARIO_FEL;
            UsuarioPFX = credenciales.USUARIO_PFX;
            LlaveFEL = credenciales.LLAVE_FEL;
            LlavePFX = credenciales.LLAVE_PFX;
            FechaEmision = EncabezadoFac.FECHA_FACTURA.ToString();
            NoEstablecimiento = resolucion.NO_ESTABLECIMIENTO;
            NombreEmpresa = resolucion.EMPRESA;
            // nombreEstablecimiento = resolucion.NOMBRE_ESTABLECIMIENTO;
            DireccionEstablecimiento = resolucion.DIRECCION_ESTABLECIMIENTO;
            NIT_Receptor = EncabezadoFac.NIT.Replace("-", "").Replace("/", "").Trim();
            //Nombre_Receptor = EncabezadoFac.NOMBRE_CLIENTE.Replace("\"", "");
            Nombre_Receptor = HttpUtility.HtmlEncode(EncabezadoFac.NOMBRE_CLIENTE);
            Direccion_Receptor = EncabezadoFac.DIRECCION;
            TotalFactura = EncabezadoFac.TOTAL_FACTURA;

            TOTAL_IDP = EncabezadoFac.TOTAL_IDP;

            // Datos_generales = request.Datos_generales("GTQ", FechaEmision, "FACT", "", "");   
            Datos_generales = request.Datos_generales(moneda, FechaEmision, "FACT", "", "", "");
            Datos_emisor = request.Datos_emisor("GEN", NoEstablecimiento, "01001", "", "GT", "GUATEMALA", "GUATEMALA", DireccionEstablecimiento, NITEmpresa, NombreEmpresa, nombreEstablecimiento);
            Datos_receptor = request.Datos_receptor(NIT_Receptor, Nombre_Receptor, "01001", EncabezadoFac.CORREO, "GT", "GUATEMALA", "GUATEMALA", Direccion_Receptor, "");

            WService.DatosDetFacturaFEL Detalles;
            Int32 CorrelativoDetalles = Lista_detalles_fac.Count();

            for (int d = 0; d < CorrelativoDetalles; d++)
            {
                Detalles = new WService.DatosDetFacturaFEL();
                Detalles = Lista_detalles_fac[d];
                Cantidad = Detalles.CANTIDAD;
                DescripcionProducto = Detalles.DESCRIPCION_PRODUCTO;
                PrecioUnitario = Detalles.PRECIO;
                TotalLinea = Detalles.PRECIO * Detalles.CANTIDAD - (decimal)Detalles.DESCUENTO;
                decimal precio = Detalles.PRECIO * Detalles.CANTIDAD;
                TotalSinIVA = Math.Round((TotalLinea / (decimal)1.12), 2);
                DescuentoLinea = (decimal)Detalles.DESCUENTO;
                UnidadMedida = Detalles.UNIDAD_MEDIDA;
                IVA = Math.Round(((TotalLinea / (decimal)1.12) * (decimal)0.12), 5);
                Item_un_impuesto = request.Item_un_impuesto("B", UnidadMedida, Cantidad.ToString(), DescripcionProducto, d + 1, PrecioUnitario.ToString(), precio.ToString(), DescuentoLinea.ToString(), TotalLinea.ToString(), "IVA", 1, "", TotalSinIVA.ToString(), IVA.ToString());
            }
            Total_impuestos = request.total_impuestos("IVA", TotalIVA);
            Totales = request.Totales(TotalFactura);
            Agregar_adenda = request.Agregar_adendas();
            response = request.enviar_peticion_fel(UsuarioFEL, LlaveFEL, ID_DOC, "", UsuarioPFX, LlavePFX, true);
            return response;
        }
        */

        public class FELSignerRequest
        {
            public string llave { get; set; }
            public string archivo { get; set; }
            public string codigo { get; set; }
            public string alias { get; set; }
            public string es_anulacion { get; set; }
        }
        public class FELSignerResponse
        {
            public bool resultado { get; set; }
            public string descripcion { get; set; }
            public string archivo { get; set; }
        }
        public class FELCertificacionRequest
        {
            public string nit_emisor { get; set; }
            public string correo_copia { get; set; }
            public string xml_dte { get; set; }
        }
        public class FELCertificacionResponse
        {
            public bool resultado { get; set; }
            public DateTime fecha { get; set; }
            public string uuid { get; set; }
            public string serie { get; set; }
            public string numero { get; set; }
        }
    }
}
















/*
            xml.AppendLine($"<dte:GTDocumento xmlns:ds=\"http://www.w3.org/2000/09/xmldsig#\" xmlns: dte =\"http://www.sat.gob.gt/dte/fel/0.2.0\" xmlns: xsi =\"http://www.w3.org/2001/XMLSchema-instance\" Version =\"0.1\" xsi: schemaLocation =\"http://www.sat.gob.gt/dte/fel/0.2.0\">");
            xml.AppendLine($"<dte:SAT ClaseDocumento=\"dte\">");
            xml.AppendLine($"<dte:DTE ID=\"DatosCertificados\">");
            xml.AppendLine($"<dte:DatosEmision ID=\"DatosEmision\">");
            xml.AppendLine($"<dte:DatosGenerales CodigoMoneda=\"GTQ\" FechaHoraEmision =\"2021-02-26T09:58:00-06:00\" Tipo =\"FACT\"></dte:DatosGenerales>");
            xml.AppendLine($"<dte:Emisor AfiliacionIVA=\"GEN\" CodigoEstablecimiento =\"1\" CorreoEmisor =\"demo@demo.com.gt\" NITEmisor =\"11111111111K\" NombreComercial =\"DEMO\" NombreEmisor =\"DEMO, SOCIEDAD ANONIMA\">");
            xml.AppendLine($"<dte:DireccionEmisor>");
            xml.AppendLine($"<dte:Direccion>CUIDAD</dte:Direccion>");
            xml.AppendLine($"<dte:CodigoPostal>01001</dte:CodigoPostal>");
            xml.AppendLine($"<dte:Municipio>GUATEMALA</dte:Municipio>");
            xml.AppendLine($"<dte:Departamento>GUATEMALA</dte:Departamento>");
            xml.AppendLine($"<dte:Pais>GT</dte:Pais>");
            xml.AppendLine($"</dte:DireccionEmisor>");
            xml.AppendLine($"</dte:Emisor>");
            xml.AppendLine($"<dte:Receptor CorreoReceptor=\"demo@demo.com\" IDReceptor =\"CF\" NombreReceptor =\"Consumidor Final\">");
            xml.AppendLine($"<dte:DireccionReceptor>");
            xml.AppendLine($"<dte:Direccion>CUIDAD</dte:Direccion>");
            xml.AppendLine($"<dte:CodigoPostal>01001</dte:CodigoPostal>");
            xml.AppendLine($"<dte:Municipio>GUATEMALA</dte:Municipio>");
            xml.AppendLine($"<dte:Departamento>GUATEMALA</dte:Departamento>");
            xml.AppendLine($"<dte:Pais>GT</dte:Pais>");
            xml.AppendLine($"</dte:DireccionReceptor>");
            xml.AppendLine($"</dte:Receptor>");
            xml.AppendLine($"<dte:Frases>");
            xml.AppendLine($"<dte:Frase CodigoEscenario=\"1\" TipoFrase =\"1\"></dte:Frase>");
            xml.AppendLine($"</dte:Frases>");
            xml.AppendLine($"<dte:Items>");
            xml.AppendLine($"<dte:Item BienOServicio=\"B\" NumeroLinea =\"1\">");
            xml.AppendLine($"<dte:Cantidad>1.00</dte:Cantidad>");
            xml.AppendLine($"<dte:UnidadMedida>UND</dte:UnidadMedida>");
            xml.AppendLine($"<dte:Descripcion>PRODUCTO1 | CODIGO | CAJA</dte:Descripcion>");
            xml.AppendLine($"<dte:PrecioUnitario>120.00</dte:PrecioUnitario>");
            xml.AppendLine($"<dte:Precio>120.00</dte:Precio>");
            xml.AppendLine($"<dte:Descuento>0.00</dte:Descuento>");
            xml.AppendLine($"<dte:Impuestos>");
            xml.AppendLine($"<dte:Impuesto>");
            xml.AppendLine($"<dte:NombreCorto>IVA</dte:NombreCorto>");
            xml.AppendLine($"<dte:CodigoUnidadGravable>1</dte:CodigoUnidadGravable>");
            xml.AppendLine($"<dte:MontoGravable>107.14</dte:MontoGravable>");
            xml.AppendLine($"<dte:MontoImpuesto>12.86</dte:MontoImpuesto>");
            xml.AppendLine($"</dte:Impuesto>");
            xml.AppendLine($"</dte:Impuestos>");
            xml.AppendLine($"<dte:Total>120.00</dte:Total>");
            xml.AppendLine($"</dte:Item>");
            xml.AppendLine($"</dte:Items>");
            xml.AppendLine($"<dte:Totales>");
            xml.AppendLine($"<dte:TotalImpuestos>");
            xml.AppendLine($"<dte:TotalImpuesto NombreCorto=\"IVA\" TotalMontoImpuesto =\"12.86\"></dte:TotalImpuesto>");
            xml.AppendLine($"</dte:TotalImpuestos>");
            xml.AppendLine($"<dte:GranTotal>120.00</dte:GranTotal>");
            xml.AppendLine($"</dte:Totales>");
            xml.AppendLine($"</dte:DatosEmision>");
            xml.AppendLine($"</dte:DTE>");
            xml.AppendLine($"<dte:Adenda>");
            xml.AppendLine($"<Codigo_cliente>C01</Codigo_cliente>");
            xml.AppendLine($"<Observaciones>ESTA ES UNA ADENDA</Observaciones>");
            xml.AppendLine($"</dte:Adenda>");
            xml.AppendLine($"</dte:SAT>");
            xml.AppendLine($"</dte:GTDocumento>");
            */