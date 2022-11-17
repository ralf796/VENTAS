using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ventas.Class
{
    public class FEL
    {

        public static string Firma_Facturas(List<WService.DatosFacturaFEL> facturas, List<WService.DatosDetFacturaFEL> Lista_detalles_fac, string moneda = "GTQ", string tasaCambio = "")
        {
            //apifel4.RequestCertificacionFel request = new RequestCertificacionFel();
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
            /*bool Complemento_notas;*/
            /*bool Complemento_cambiaria;*/
            /*bool Complemento_especial;*/
            /*bool Complemento_exportacion;*/
            WService.DatosFacturaFEL EncabezadoFac;

            Int32 CorrelativoEncabezado = facturas.Count();

            for (int i = 0; i < CorrelativoEncabezado; i++)
            {
                EncabezadoFac = new WService.DatosFacturaFEL();
                EncabezadoFac = facturas[i];
                ID_DOC = EncabezadoFac.SERIE + "-" + EncabezadoFac.NO_FACTURA.ToString();

                //CREDENCIALES FEL
                string consultaFEL = @"SELECT USUARIO_FEL AS USUARIO_FEL, USUARIO_PFX AS USUARIO_PFX, LLAVE_FEL AS LLAVE_FEL, LLAVE_PFX AS LLAVE_PFX
                                       FROM CFG_FEL WHERE EMPRESA =";

                CredencialesFEL credenciales = db.Database.SqlQuery<CredencialesFEL>(consultaFEL).SingleOrDefault();

                UsuarioFEL = credenciales.USUARIO_FEL;
                UsuarioPFX = credenciales.USUARIO_PFX;
                LlaveFEL = credenciales.LLAVE_FEL;
                LlavePFX = credenciales.LLAVE_PFX;
                FechaEmision = EncabezadoFac.FECHA_FACTURA.ToString();
                NoEstablecimiento = 1;
                NombreEmpresa = "NOMBRE EMPRESA";
                DireccionEstablecimiento = "DIRECCION ESTABLECIMIENTO";
                NIT_Receptor = EncabezadoFac.NIT.Replace("-", "").Replace("/", "").Trim();
                Nombre_Receptor = EncabezadoFac.NOMBRE_CLIENTE.Replace("\"", ""); ;
                Direccion_Receptor = EncabezadoFac.DIRECCION;
                TotalFactura = EncabezadoFac.TOTAL_FACTURA;
                TOTAL_IDP = EncabezadoFac.TOTAL_IDP;

                // Datos_generales = request.Datos_generales("GTQ", FechaEmision, "FACT", "", "");   
                Datos_generales = request.Datos_generales(moneda, FechaEmision, "FACT", "", "");
                Datos_emisor = request.Datos_emisor("GEN", NoEstablecimiento, "01001", "", "GT", "GUATEMALA", "GUATEMALA", DireccionEstablecimiento, NITEmpresa, NombreEmpresa, NombreEmpresa);
                Datos_receptor = request.Datos_receptor(NIT_Receptor, Nombre_Receptor, "01001", EncabezadoFac.CORREO, "GT", "GUATEMALA", "GUATEMALA", Direccion_Receptor, "");


                // bool coso = request.Adendas("TasaCambio", "7.78934");


                // verifico si el cliente es zona franca o cliente normal
                if (clienteZF == 0)
                {
                    // cliente normal
                    Frases = request.Frases(1, 1);
                    Frases = request.Frases(2, 1);
                    TotalIVA = EncabezadoFac.TOTAL_IVA;
                }
                else
                {

                    String consultaMaquila = "Select count(*) from cat_cliente_maquila where cliente=" + EncabezadoFac.CLIENTE;
                    int x = db.Database.SqlQuery<int>(consultaMaquila).FirstOrDefault();
                    // cliente de zona franca
                    Frases = request.Frases(1, 1);
                    Frases = request.Frases(2, 1);

                    if (x > 0)
                    {
                        Frases = request.Frases(4, 11);
                    }
                    else
                    {
                        Frases = request.Frases(4, 12);
                    }


                    TotalIVA = Convert.ToString(0);
                }

                WService.DatosDetFacturaFEL Detalles;

                Int32 CorrelativoDetalles = Lista_detalles_fac.Count();

                //SOLO LLEVA IVA SIN IDP
                if (decimal.Parse(TOTAL_IDP) == 0)
                {
                    for (int d = 0; d < CorrelativoDetalles; d++)
                    {

                        ////foreach (var DetallesList in Lista_detalles_fac)
                        ////{
                        Detalles = new WService.DatosDetFacturaFEL();
                        Detalles = Lista_detalles_fac[d];

                        Cantidad = Detalles.CANTIDAD;

                        DescripcionProducto = Detalles.DESCRIPCION_PRODUCTO;

                        PrecioUnitario = Detalles.PRECIO;
                        TotalLinea = Detalles.PRECIO * Detalles.CANTIDAD;
                        TotalSinIVA = Math.Round((TotalLinea / (decimal)1.12), 2);

                        DescuentoLinea = (decimal)Detalles.DESCUENTO;
                        UnidadMedida = Detalles.UNIDAD_MEDIDA;

                        IVA = Math.Round(((TotalLinea / (decimal)1.12) * (decimal)0.12), 2);

                        //FACTURA SOLO CON IVA
                        Item_un_impuesto = request.Item_un_impuesto("B", UnidadMedida, Cantidad.ToString(), DescripcionProducto, d + 1, PrecioUnitario.ToString(), TotalLinea.ToString(), DescuentoLinea.ToString(), TotalLinea.ToString(), "IVA", 1, "", TotalSinIVA.ToString(), IVA.ToString());
                    }

                    Total_impuestos = request.total_impuestos("IVA", TotalIVA);
                    Totales = request.Totales(TotalFactura);
                }

                //Adenda = request.Adendas("TasaCambio", tasaCambio);
                //Complemento_notas = request.Complemento_notas("","","","","","","","","");
                //Complemento_cambiaria = request.Complemento_cambiaria("","","");
                //Complemento_especial = request.Complemento_especial("","","","","","");
                //Complemento_exportacion = request.Complemento_exportacion("","","","","","","","","","","","","");

                Adenda = request.Adendas("Codigo_cliente", "C01");//Información Adicional
                Adenda = request.Adendas("Observaciones", "");

                if (moneda != "GTQ")
                    Adenda = request.Adendas("TasaCambio", tasaCambio.ToString());

                Agregar_adenda = request.Agregar_adendas();
                response = request.enviar_peticion_fel(UsuarioFEL, LlaveFEL, ID_DOC, "", UsuarioPFX, LlavePFX, true);
            }

            return response;
        }

        public static string Firma_NC(List<WService.DatosNCFEL> notas, bool flagAntiguo)
        {
            apifel4.RequestCertificacionFel requestNC = new RequestCertificacionFel();
            genesysEntities db = new genesysEntities();

            int Empresa;
            string UsuarioFEL;
            string UsuarioPFX;
            string LlaveFEL;
            string LlavePFX;
            string ID_DOC;
            string response = "";
            bool Datos_generales;
            bool Datos_emisor;
            bool Datos_receptor;
            bool Item_un_impuesto;
            bool Item_dos_impuesto;
            bool Total_impuestos;
            bool Totales;
            bool Complemento_notas;
            string FechaEmision;
            int NoEstablecimiento;
            string NombreEmpresa;
            string NITEmpresa;
            string DireccionEstablecimiento;
            string NIT_Receptor;
            string Nombre_Receptor;
            string Direccion_Receptor;
            string TotalNC;
            string TotalIVA_NC;
            string TOTAL_IDP_NC;
            decimal TotalSinIVA;
            decimal TotalSinIDP;
            string FechaFac;
            string Motivo;
            string UUID_Anula;
            decimal TOTAL_GALONES;
            decimal TOTALtemp;
            int ESTADO;

            string NO_ELECTRONICO;
            string RESOLUCION_FAC;

            WService.DatosNCFEL EncabezadoNC;

            Int32 CorrelativoNC = notas.Count();

            for (int i = 0; i < CorrelativoNC; i++)
            {
                EncabezadoNC = new WService.DatosNCFEL();
                EncabezadoNC = notas[i];

                // VERIFICO SI EL CLIENTE ES DE ZONA FRANCA - aramirez
                string consultaZF = " select count(*) as total " +
                        "    from cat_cliente a " +
                        "    inner join fac_factura_encabezado b on a.cliente = b.cliente " +
                        "    where b.serie = '" + EncabezadoNC.SERIE_FAC + "' and b.no_factura = " + EncabezadoNC.NO_FAC + "  and a.segmentacion = 3 ";

                int clienteZF = db.Database.SqlQuery<int>(consultaZF).FirstOrDefault();

                var uno = clienteZF;

                string consulta_resolucion = "";
                Datos_resolucion resolucion = null;

                consulta_resolucion = " SELECT A.SERIE_NC, A.RESOLUCION_NC, " +
                             " A.RANGO_INICIAL, A.RANGO_FINAL, A.TIPO_ACTIVO, " +
                             " C.NIT, UPPER(C.NOMBRE_EMPRESA) AS NOMBRE_EMPRESA, B.NOMBRE_ESTABLECIMIENTO, B.NO_ESTABLECIMINETO AS NO_ESTABLECIMIENTO, " +
                             " A.DISPOSITIVO_ELECTRONICO, B.DIR_ESTABLECIMIENTO, C.EMPRESA, C.TELEFONO" +
                             " FROM cyc_nota_credito_serie a " +
                             " inner join cat_establecimiento b on a.establecimiento = b.establecimiento " +
                             " inner join cat_empresa c on b.empresa = c.empresa " +
                             " where a.serie_NC='" + EncabezadoNC.SERIE + "' AND a.RESOLUCION_NC = '" + EncabezadoNC.RESOLUCION + "'";

                resolucion = db.Database.SqlQuery<Datos_resolucion>(consulta_resolucion).SingleOrDefault();

                Empresa = resolucion.EMPRESA;
                ID_DOC = EncabezadoNC.SERIE + "-" + EncabezadoNC.NO_NC.ToString();
                FechaEmision = EncabezadoNC.FECHA_NC;
                NoEstablecimiento = resolucion.NO_ESTABLECIMIENTO;
                NITEmpresa = resolucion.NIT;
                DireccionEstablecimiento = resolucion.DIR_ESTABLECIMIENTO;
                NombreEmpresa = resolucion.NOMBRE_EMPRESA;
                NIT_Receptor = EncabezadoNC.NIT.Replace("-", "").Replace("/", "").Trim();
                Nombre_Receptor = EncabezadoNC.NOMBRE_CLIENTE;
                Direccion_Receptor = EncabezadoNC.DIRECCION;
                TotalNC = EncabezadoNC.TOTAL_NC;

                TOTAL_IDP_NC = EncabezadoNC.TOTAL_IDP_NC;
                //    TotalSinIVA = decimal.Parse(TotalNC) - decimal.Parse(TOTAL_IDP_NC) - decimal.Parse(TotalIVA_NC);
                FechaFac = EncabezadoNC.FECHA_FAC;
                Motivo = EncabezadoNC.MOTIVO;
                UUID_Anula = EncabezadoNC.UUID_ANULA;
                TOTAL_GALONES = EncabezadoNC.TOTAL_GALONES;
                ESTADO = EncabezadoNC.ESTADO;

                NO_ELECTRONICO = EncabezadoNC.NO_ELECTRONICO;
                RESOLUCION_FAC = EncabezadoNC.RESOLUCION_FAC;

                // verifico si el cliente es zona franca o cliente normal
                if (clienteZF == 0)
                {
                    // cliente normal
                    TotalIVA_NC = EncabezadoNC.TOTAL_IVA_NC;
                    TotalSinIVA = decimal.Parse(TotalNC) - decimal.Parse(TOTAL_IDP_NC) - decimal.Parse(TotalIVA_NC);
                }
                else
                {
                    // cliente de zona franca
                    TotalIVA_NC = Convert.ToString(0);
                    //TotalSinIVA = decimal.Parse(TotalNC);
                    TotalSinIVA = decimal.Parse(TotalNC) - decimal.Parse(TOTAL_IDP_NC) - decimal.Parse(TotalIVA_NC);
                }

                //CREDENCIALES FEL
                string consultaFEL = "SELECT USUARIO_FEL AS USUARIO_FEL, USUARIO_PFX AS USUARIO_PFX, LLAVE_FEL AS LLAVE_FEL, LLAVE_PFX AS LLAVE_PFX " +
                                        " FROM CFG_FEL WHERE EMPRESA = " + Empresa;

                CredencialesFEL credenciales = db.Database.SqlQuery<CredencialesFEL>(consultaFEL).SingleOrDefault();

                UsuarioFEL = credenciales.USUARIO_FEL;
                UsuarioPFX = credenciales.USUARIO_PFX;
                LlaveFEL = credenciales.LLAVE_FEL;
                LlavePFX = credenciales.LLAVE_PFX;

                Datos_generales = requestNC.Datos_generales("GTQ", FechaEmision, "NCRE", "", "");
                Datos_emisor = requestNC.Datos_emisor("GEN", NoEstablecimiento, "01001", "", "GT", "GUATEMALA", "GUATEMALA", DireccionEstablecimiento, NITEmpresa, NombreEmpresa, NombreEmpresa);
                Datos_receptor = requestNC.Datos_receptor(NIT_Receptor, Nombre_Receptor, "01001", "", "GT", "GUATEMALA", "GUATEMALA", Direccion_Receptor, "");
                //Frases = request.Frases(1, 1);

                //IVA
                if (Convert.ToDecimal(TOTAL_IDP_NC) == 0)
                {
                    Item_un_impuesto = requestNC.Item_un_impuesto("S", "UND", "1", "ANULACION DE FACTURA", 1, TotalNC, TotalNC, "0", TotalNC, "IVA", 1, "", TotalSinIVA.ToString(), TotalIVA_NC);
                    Total_impuestos = requestNC.total_impuestos("IVA", TotalIVA_NC);
                }

                //IVA e IDP
                if (Convert.ToDecimal(TOTAL_IDP_NC) > 0)
                {

                    TotalSinIDP = Math.Round((decimal.Parse(TotalNC) - decimal.Parse(TOTAL_IDP_NC)) / TOTAL_GALONES, 2);


                    TOTALtemp = TotalSinIDP * TOTAL_GALONES;
                    if (clienteZF == 0)
                    {
                        Item_dos_impuesto = requestNC.Item_dos_impuestos("S", "UND", TOTAL_GALONES.ToString(), "ANULACION DE FACTURA", 1, TotalSinIDP.ToString(), TOTALtemp.ToString(), "0", TotalNC, "IVA", 1, "", TotalSinIVA.ToString(), TotalIVA_NC, "PETROLEO", 9, TOTAL_GALONES.ToString(), "", TOTAL_IDP_NC);
                    }
                    else
                    {
                        // double totalSinIvaJz = TotalSinIVA - TOTAL_IDP_NC;
                        Item_dos_impuesto = requestNC.Item_dos_impuestos("S", "UND", TOTAL_GALONES.ToString(), "ANULACION DE FACTURA", 1, TotalSinIDP.ToString(), TOTALtemp.ToString(), "0", TotalNC, "IVA", 2, "", TotalSinIVA.ToString(), TotalIVA_NC, "PETROLEO", 9, TOTAL_GALONES.ToString(), "", TOTAL_IDP_NC);
                    }
                    Total_impuestos = requestNC.total_impuestos("IVA", TotalIVA_NC);
                    Total_impuestos = requestNC.total_impuestos("PETROLEO", TOTAL_IDP_NC);
                }

                Totales = requestNC.Totales(TotalNC);
                Complemento_notas = requestNC.Complemento_notas("Notas", "Notas", "http://www.sat.gob.gt/fel/notas.xsd", FechaFac, Motivo, UUID_Anula, "", "", "");
                response = requestNC.enviar_peticion_fel(UsuarioFEL, LlaveFEL, ID_DOC, "", UsuarioPFX, LlavePFX, true);
            }
            return response;
        }
    }
}