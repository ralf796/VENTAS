using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services.Description;
using Ventas_BE;
using Ventas_BLL;
using conectorfelv2;


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


        public static string Firmar_Factura_FEL(FEL_BE VENTA)
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

    }
}