using CrystalDecisions.CrystalReports.Engine;
using Newtonsoft.Json;
using SelectPdf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using Ventas.Class;
using Ventas_BE;
using Ventas_BLL;


namespace Ventas.Controllers.Ventas
{
    public class VENCrearVentaController : Controller
    {
        // GET: VENCrearVenta
        [SessionExpireFilter]
        public ActionResult Index()
        {
            return View();
        }

        #region BD
        private List<Ventas__BE> GetDatosSP_(Ventas__BE item)
        {
            List<Ventas__BE> lista = new List<Ventas__BE>();
            lista = Ventas__BLL.GetDatosSP(item);
            return lista;
        }
        #endregion

        #region FUNCTIONS
        private bool SaveHeader(int idVenta = 0, string serie = "", decimal correlativo = 0, int idCliente = 0, decimal total = 0, decimal descuento = 0, decimal subtotal = 0, string usuario = "", int fel = 0)
        {
            bool respuesta = false;
            var item = new Ventas__BE();
            item.MTIPO = 4;
            item.ID_VENTA = idVenta;
            item.SERIE = serie;
            item.CORRELATIVO = correlativo;
            item.ID_CLIENTE = idCliente;
            item.TOTAL = total;
            item.SUBTOTAL = subtotal;
            item.TOTAL_DESCUENTO = descuento;
            item.CREADO_POR = usuario;
            item.FEL = fel;
            var resultHeader = GetDatosSP_(item);

            if (resultHeader != null)
            {
                if (resultHeader.FirstOrDefault().CODIGO_RESPUESTA == "01")
                    respuesta = true;
            }
            else
                respuesta = false;

            return respuesta;
        }
        private bool SaveDetail(int idVenta = 0, int idProducto = 0, int cantidad = 0, decimal precio = 0, decimal total = 0, decimal descuento = 0, decimal subtotal = 0)
        {
            bool respuesta = false;
            var item = new Ventas__BE();
            item.MTIPO = 5;
            item.ID_VENTA = idVenta;
            item.ID_PRODUCTO = idProducto;
            item.CANTIDAD = cantidad;
            item.PRECIO_VENTA = precio;
            item.TOTAL = total;
            item.TOTAL_DESCUENTO = descuento;
            item.SUBTOTAL = subtotal;
            var resultDetail = GetDatosSP_(item);

            if (resultDetail != null)
            {
                if (resultDetail.FirstOrDefault().CODIGO_RESPUESTA == "01")
                    respuesta = true;
            }
            else
                respuesta = false;

            return respuesta;
        }
        private bool DeleteOrder(int idVenta = 0)
        {
            bool respuesta = false;
            var item = new Ventas__BE();
            item.MTIPO = 6;
            item.ID_VENTA = idVenta;
            var resultDetail = GetDatosSP_(item);

            if (resultDetail != null)
            {
                if (resultDetail.FirstOrDefault().CODIGO_RESPUESTA == "01")
                    respuesta = true;
            }
            else
                respuesta = false;

            return respuesta;
        }
        private bool DescontProduct(int idVenta = 0)
        {
            bool respuesta = false;
            var item = new Ventas__BE();
            item.MTIPO = 7;
            item.ID_VENTA = idVenta;
            var resultHeader = GetDatosSP_(item);

            if (resultHeader != null)
            {
                if (resultHeader.FirstOrDefault().CODIGO_RESPUESTA == "01")
                    respuesta = true;
            }
            else
                respuesta = false;

            return respuesta;
        }
        private List<Inventario_BE> GetInventario_select_(Inventario_BE item)
        {
            List<Inventario_BE> lista = new List<Inventario_BE>();
            lista = Inventario_BLL.GetInventario_select(item);
            return lista;
        }
        private List<Clientes_BE> GetDatosCliente_(Clientes_BE item)
        {
            List<Clientes_BE> lista = new List<Clientes_BE>();
            lista = Clientes_BLL.GetSPCliente(item);
            return lista;
        }
        private List<Usuarios_BE> GetSPLogin_(Usuarios_BE item)
        {
            List<Usuarios_BE> lista = new List<Usuarios_BE>();
            lista = Usuarios_BLL.GetSPLogin(item);
            return lista;
        }
        private List<Inventario_BE> GetDatosInventario_(Inventario_BE item)
        {
            List<Inventario_BE> lista = new List<Inventario_BE>();
            lista = Inventario_BLL.GetSPInventario(item);
            return lista;
        }
        #endregion

        #region JSON_RESULTS
        public JsonResult GetCliente(int tipo = 0, string nit = "")
        {
            try
            {
                var item = new Ventas__BE();
                item.MTIPO = tipo;
                item.NIT = nit.Trim();
                item = GetDatosSP_(item).FirstOrDefault();
                return Json(new { State = 1, data = item }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { State = -1, Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult GetClienteID(int tipo = 0, int id = 0)
        {
            try
            {
                var item = new Ventas__BE();
                item.MTIPO = tipo;
                item.ID_CLIENTE = id;
                item = GetDatosSP_(item).FirstOrDefault();
                return Json(new { State = 1, data = item }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { State = -1, Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult GetList(int tipo = 0, int ID_MARCA_REPUESTO = 0, int ID_SUBCATEGORIA = 0, int ID_CATEGORIA = 0, int ID_SERIE_VEHICULO = 0, int ID_MARCA_VEHICULO = 0, int ID_MODELO = 0)
        {
            try
            {
                var item = new Ventas__BE();
                item.MTIPO = tipo;
                if (ID_MARCA_REPUESTO != 0)
                    item.ID_MARCA_REPUESTO = ID_MARCA_REPUESTO;
                if (ID_SUBCATEGORIA != 0)
                    item.ID_SUBCATEGORIA = ID_SUBCATEGORIA;
                if (ID_CATEGORIA != 0)
                    item.ID_CATEGORIA = ID_CATEGORIA;
                if (ID_SERIE_VEHICULO != 0)
                    item.ID_SERIE_VEHICULO = ID_SERIE_VEHICULO;
                if (ID_MARCA_VEHICULO != 0)
                    item.ID_MARCA_VEHICULO = ID_MARCA_VEHICULO;
                if (ID_MODELO != 0)
                    item.ID_MODELO = ID_MODELO;
                var lista = GetDatosSP_(item);

                return Json(new { State = 1, data = lista }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { State = -1, Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult SaveOrder(string encabezado = "", string detalles = "", int fel = 0)
        {
            try
            {
                var item = JsonConvert.DeserializeObject<Ventas__BE>(encabezado);
                List<Ventas__BE> listaDetalles = JsonConvert.DeserializeObject<List<Ventas__BE>>(detalles);
                string usuario = Session["usuario"].ToString();
                item.CREADO_POR = usuario;
                int state = 1;
                bool banderaDetail = false;

                var itemID = new Ventas__BE();
                itemID.MTIPO = 6;
                item.ID_VENTA = GetDatosSP_(itemID).FirstOrDefault().ID_VENTA;

                if (SaveHeader(Convert.ToInt32(item.ID_VENTA), "", 1, item.ID_CLIENTE, item.TOTAL, item.TOTAL_DESCUENTO, item.SUBTOTAL, usuario, fel) == true)
                {
                    foreach (var row in listaDetalles)
                    {
                        if (banderaDetail == false)
                        {
                            if (SaveDetail(Convert.ToInt32(item.ID_VENTA), row.ID_PRODUCTO, row.CANTIDAD, row.PRECIO_VENTA, row.TOTAL, row.TOTAL_DESCUENTO, row.SUBTOTAL) == false)
                            {
                                DeleteOrder(Convert.ToInt32(item.ID_VENTA));
                                banderaDetail = true;
                            }
                        }
                    }
                }

                string uuid = "";

                if (banderaDetail == true)
                    state = 2;
                else
                {
                    DescontProduct(Convert.ToInt32(item.ID_VENTA));
                    /*
                    if (fel == 1)
                    {
                        var respuestaFEL = Certificar_Factura_FEL(Convert.ToInt32(item.ID_VENTA));
                        if (respuestaFEL.RESULTADO)
                            uuid = respuestaFEL.UUID;
                    }
                    */

                }
                return Json(new { State = state, ORDEN_COMPRA = item.ID_VENTA, uuid = uuid }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { State = -1, Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult GetDatosProductos(int tipo = 0, int id = 0, string modelo = "", string marcaVehiculo = "", string nombreLinea = "", string nombreDescripcion = "")
        {
            try
            {
                var item = new Ventas__BE();


                if (modelo.Length == 4)
                {
                    item.ANIO_VEHICULO = Convert.ToInt32(modelo);
                }
                item.MTIPO = tipo;
                if (modelo != "" && modelo != "0")
                    item.NOMBRE_MODELO = modelo;
                if (marcaVehiculo != "" && marcaVehiculo != "0")
                    item.NOMBRE_MARCA_VEHICULO = marcaVehiculo;
                if (nombreLinea != "" && nombreLinea != "0")
                    item.NOMBRE_LINEA_VEHICULO = nombreLinea;
                var lista = GetDatosSP_(item);

                if (nombreLinea.Length > 2 && lista != null)
                    lista = lista.Where(x => x.NOMBRE_SERIE_VEHICULO.Contains(nombreLinea.ToUpper())).ToList();

                if (nombreDescripcion.Length > 2 && lista != null)
                    lista = lista.Where(x => x.NOMBRE_COMPLETO.Contains(nombreDescripcion.ToUpper())).ToList();

                return Json(new { State = 1, data = lista }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { State = -1, Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult GuardarCliente(string nombre = "", string direccion = "", string telefono = "", string email = "", string nit = "")
        {
            try
            {
                nit = nit.Trim().Replace("-", "").Replace("/", "");
                if (nit == "")
                    nit = "CF";

                if (direccion.Trim() == "")
                    direccion = "Ciudad";

                string respuesta = "";
                var item = new Clientes_BE();
                item.NOMBRE = nombre;
                item.DIRECCION = direccion;
                item.TELEFONO = telefono;
                item.EMAIL = email;
                item.NIT = nit;
                item.CREADO_POR = Session["usuario"].ToString();
                item.MTIPO = 1;
                var lista = GetDatosCliente_(item);

                int idCli = 0;
                if (lista.Count > 0)
                {
                    if (lista.FirstOrDefault().RESPUESTA != "")
                    {
                        respuesta = lista.FirstOrDefault().RESPUESTA;
                        idCli = Convert.ToInt32(lista.FirstOrDefault().ID_CLIENTE);
                    }
                }
                return Json(new { State = 1, ID_CLIENTE = idCli }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {

                return Json(new { State = -1, Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult GetDatosTable(int tipo = 0, int id = 0, string modelo = "", string marcaVehiculo = "", string nombreLinea = "")
        {
            try
            {
                var item = new Inventario_BE();
                item.MTIPO = tipo;
                item.ID_UPDATE = id;
                if (modelo != "" && modelo != "0")
                    item.NOMBRE_MODELO = modelo;
                if (marcaVehiculo != "" && marcaVehiculo != "0")
                    item.NOMBRE_MARCA_VEHICULO = marcaVehiculo;
                if (nombreLinea != "" && nombreLinea != "0")
                    item.NOMBRE_LINEA_VEHICULO = nombreLinea;
                var lista = GetInventario_select_(item);
                return Json(new { State = 1, data = lista }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { State = -1, Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult GetProductoPorCodigo(int tipo = 0, string codigo = "")
        {
            try
            {
                var item = new Ventas__BE();
                item.MTIPO = tipo;
                item.CODIGO = codigo.Trim();
                item.CODIGO2 = codigo.Trim();
                item = GetDatosSP_(item).FirstOrDefault();
                return Json(new { State = 1, data = item }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { State = -1, Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult ValidarLogin(string usuario = "", string password = "")
        {
            try
            {
                int estado = 0;
                var item = new Usuarios_BE();
                item.USUARIO = usuario.ToUpper();
                item.PASSWORD = new Encryption().Encrypt(password.ToUpper().Trim());
                item.MTIPO = 2;
                item = GetSPLogin_(item).FirstOrDefault();
                if (item != null)
                {
                    estado = 1;
                }
                else
                    estado = 2;

                return Json(new { State = estado }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { State = -1, Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult UpdateProducto(int ID_PRODUCTO = 0, string NOMBRE = "", string DESCRIPCION = "", decimal PRECIO_COSTO = 0, decimal PRECIO_VENTA = 0, string usuarioModifica = "")
        {
            try
            {
                int estado = 0;
                string codigo = "";
                var item = new Inventario_BE();
                item.ID_PRODUCTO = ID_PRODUCTO;

                if (NOMBRE != "")
                    item.NOMBRE = NOMBRE;

                if (DESCRIPCION != "")
                    item.DESCRIPCION = DESCRIPCION;
                item.STOCK = 0;
                item.PRECIO_COSTO = PRECIO_COSTO;
                item.PRECIO_VENTA = PRECIO_VENTA;
                item.CREADO_POR = Session["usuario"].ToString();
                item.MTIPO = 8;
                item.CODIGO = usuarioModifica;
                item = GetDatosInventario_(item).FirstOrDefault();
                if (item != null)
                {
                    estado = 1;
                    codigo = item.CODIGO;
                }
                else
                    estado = 2;


                return Json(new { State = estado, data = item, CODIGO = codigo }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { State = -1, Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult GetProductosTable(string filtro = "", int anioI = 0, int anioF = 0)
        {
            try
            {
                var item = new Ventas__BE();
                List<Ventas__BE> lista = new List<Ventas__BE>();
                item.MTIPO = 11;
                item.CODIGO = "";
                item.CODIGO2 = "";
                item.NOMBRE_MODELO = filtro;
                item.ANIO_INICIAL = anioI;
                item.ANIO_FINAL = anioF;
                lista = GetDatosSP_(item);
                return Json(new { State = 1, data = lista }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { State = -1, Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

        #region COTIZACION
        public ActionResult GetCotizacion(string encabezado = "", string detalles = "")
        {
            var item = JsonConvert.DeserializeObject<Ventas__BE>(encabezado);
            List<Ventas__BE> listaDetalles = JsonConvert.DeserializeObject<List<Ventas__BE>>(detalles);

            //Get html
            System.Text.StringBuilder html = new System.Text.StringBuilder();
            html.AppendLine("<!DOCTYPE html>");
            html.AppendLine("<html>");
            html.AppendLine("<head>");
            html.AppendLine("<meta charset='utf-8' />");
            html.AppendLine(@"<link rel='stylesheet' href='https://cdn.jsdelivr.net/npm/bootstrap@4.0.0/dist/css/bootstrap.min.css' integrity='sha384-Gn5384xqQ1aoWXA+058RXPxPg6fy4IWvTNh0E263XmFcJlSAwiGgFAW/dAiS6JXm' crossorigin='anonymous'>");
            html.AppendLine("<script src='http://code.jquery.com/jquery-latest.min.js'></script>");
            html.AppendLine("</head>");
            html.AppendLine("<body class='pt-4'>");
            html.AppendLine($@"
            <div class='row'>
                <div class='form-group col-md-3 text-center' style='margin-top:-35px'>
                    <img style='margin-top:10px' src='https://distribuidorarepuestoseleden.com/Varios/LogoElEden.jpeg' height='180' width='250' />
                </div>
                <div class='form-group col-md-7 text-center' style='margin-top:30px'>
                    <div class='row'>
                        <div class='form-group col-md-12'>
                            <h6 style='font-size:25px'>DISTRIBUIDORA DE REPUESTOS EL EDEN</h6>
                        </div>
                        <div class='form-group col-md-12'>
                            <h6 style='font-size:22px; margin-top:-20px'>CALZADA EL MOSQUITO 2-05 ZONA 3 SAN PEDRO SACATEPEQUEZ, SAN MARCOS</h6>
                        </div>
                        <div class='form-group col-md-12'>
                            <h6 style='font-size:22px; margin-top:-20px'>NIT: 74974324</h6>
                        </div>
                        <div class='form-group col-md-12'>
                            <h6 style='font-size: 22px; margin-top:-20px'>7760-8499</h6>
                        </div>
                    </div>
                </div>
            </div>
            <div class='row'>
                <div class='col-md-12 pl-5' style='margin-top:5px'>
                    <b>NIT:</b> {item.NIT}<br>
                    <b>CLIENTE:</b> {item.NOMBRE}<br>
                    <b>DIRECCIÓN:</b> {item.DIRECCION}<br>
                    <b>FECHA:</b> {DateTime.Now.ToString("dd/MM/yyyy")}<br>
                    <b>ATENDIDO POR:</b> {Session["usuario"].ToString()}<br>
                </div>
            </div>
            <div class='col-12 pt-4'>
                <table class='table table-sm' id='tdDatos'>
                    <thead>
                        <tr style=''>
                            <th class='pl-2 pr-2 border text-center' style='vertical-align:middle; border: 1px solid'>CANTIDAD</th>
                            <th class='pl-2 pr-2 border text-center d-none' style='vertical-align:middle; border: 1px solid'>CÓDIGO</th>
                            <th class='pl-2 pr-2 border text-center' style='vertical-align:middle; border: 1px solid'>DESCRIPCIÓN</th>
                            <th class='pl-2 pr-2 border text-center' style='vertical-align:middle; border: 1px solid'>MARCA</th>
                            <th class='pl-2 pr-2 border text-center' style='vertical-align:middle; border: 1px solid'>PRECIO</th>
                            <th class='pl-2 pr-2 border text-center' style='vertical-align:middle; border: 1px solid'>DESCUENTO</th>
                            <th class='pl-2 pr-2 border text-center' style='vertical-align:middle; border: 1px solid'>SUBTOTAL</th>
                        </tr>
                    </thead>
                    <tbody id='tbodyListado'>");
            decimal total = 0, descuento = 0;
            foreach (var dato in listaDetalles)
            {
                decimal tot = 0, sub = 0, desc = 0;
                tot = dato.CANTIDAD * dato.PRECIO_VENTA;
                desc = (dato.DESCUENTO);

                html.AppendLine("<tr>");
                html.AppendLine($"<td class='text-center' style='border-bottom: 1px solid;border-top: 1px solid'>{dato.CANTIDAD}</td>");
                html.AppendLine($"<td class='text-left d-none' style='border-bottom: 1px solid'>{dato.CODIGO} - {dato.CODIGO2}</td>");
                html.AppendLine($"<td class='text-left' style='border-bottom: 1px solid'>{dato.NOMBRE_PRODUCTO}</td>");
                html.AppendLine($"<td class='text-left' style='border-bottom: 1px solid'>{dato.NOMBRE_MARCA_REPUESTO}</td>");
                html.AppendLine($"<td class='text-right' style='border-bottom: 1px solid'>{dato.PRECIO_VENTA.ToString("N2")}</td>");
                html.AppendLine($"<td class='text-right' style='border-bottom: 1px solid'>{dato.TOTAL_DESCUENTO.ToString("N2")}</td>");
                html.AppendLine($"<td class='text-right' style='border-bottom: 1px solid'>{((dato.CANTIDAD * dato.PRECIO_VENTA) - dato.DESCUENTO).ToString("N2")}</td>");
                html.AppendLine("</tr>");
                total += (dato.CANTIDAD * dato.PRECIO_VENTA);
                descuento += dato.TOTAL_DESCUENTO;
            }
            html.AppendLine($@"</tbody>
                </table>
            </div>
            <div class='row'>
                <div class='col-md-12 pl-5'style='font-size:20px'>
                    <b style='font-size:20px'>TOTAL SIN DESCUENTO:</b> {total.ToString("N2")}<br>
                    <b style='font-size:20px'>DESCUENTO:</b> {descuento.ToString("N2")}<br>
                    <b style='font-size:20px'>TOTAL CON DESCUENTO:</b> {(total - descuento).ToString("N2")}<br>
                    <b style='font-size:23px'>TOTAL CON DESCUENTO EN LETRAS: </b> {new NumeroLetra().Convertir((total - descuento).ToString(), true)}<br>
                </div>
            </div>
            <div class='row pt-5'>
                <div class='col-md-12 text-center border-dark border-top'>
                    <b>-- NOTA: LA COTIZACIÓN VENCE EN 5 DÍAS --</b>
                </div>
            </div>");
            html.AppendLine("</body>");
            html.AppendLine("</html>");
            // instantiate a html to pdf converter object
            HtmlToPdf converter = new HtmlToPdf();
            converter.Options.PdfPageSize = PdfPageSize.Letter;
            converter.Options.PdfPageOrientation = PdfPageOrientation.Portrait;
            converter.Options.WebPageWidth = 1400;
            converter.Options.WebPageHeight = 600;

            converter.Options.MarginBottom = 30;
            converter.Options.MarginTop = 30;
            converter.Options.MarginLeft = 7;
            converter.Options.MarginRight = 7;

            // create a new pdf document converting an html
            PdfDocument doc = converter.ConvertHtmlString(html.ToString());

            //Save memory
            MemoryStream fileStream = new MemoryStream();
            doc.Save(fileStream);
            doc.Close();

            string file64 = Convert.ToBase64String(fileStream.ToArray());
            var file = new { File = file64, MimeType = "application/pdf", FileName = $"Cotizacion_{item.ID_CLIENTE}_{DateTime.Now.ToString("ddMMyyyy")}.pdf" };
            return Json(file);
        }
        #endregion

        #region CRYSTAL
        public void ImprimirFactura()
        {
            ReportDocument rd = new ReportDocument();
            //rd.Load(Path.Combine(Server.MapPath("~/Crystal"), "Factura_FEL.rpt"));
            //rd.Load(Path.Combine(Server.MapPath("~/Crystal/Factura_FEL.rpt")));
            //string path = Server.MapPath("~/Crystal/Factura_FEL1.rpt");

            string url = Server.MapPath(@"~\Content\Crystal\Factura_FEL.rpt");

            /*
            string path = Path.Combine(Server.MapPath(@"~\Crystal\Factura_FEL1.rpt"));

            var urlBuilder = new System.UriBuilder(Request.Url.AbsoluteUri) { Path = Url.Content(@"~\Crystal\Factura_FEL.rpt"), Query = null, };
            Uri uri = urlBuilder.Uri;
            string url = urlBuilder.ToString();
            */
            //rd.Load(url);
            rd.Load(url);

            //var listPedidos = BuscaDatosReporte(cobro);
            List<Ventas__BE> Lista = new List<Ventas__BE>();
            rd.SetDataSource(Lista);

            Response.Buffer = false;
            Response.ClearContent();
            Response.ClearHeaders();

            rd.ExportToHttpResponse(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, System.Web.HttpContext.Current.Response, false, "ReportePedidos");
            rd.Close();
            rd.Dispose();
        }
        #endregion

        public JsonResult GetHTML()
        {
            try
            {
                var item = new Ventas__BE();
                System.Text.StringBuilder html = new System.Text.StringBuilder();
                html.AppendLine(@"
                        <!DOCTYPE html>
                    <html>
                        <head>
                            <style>

                    * {
                        font-size: 12px;
                        font-family: 'Times New Roman';
                    }

                    td,
                    th,
                    tr,
                    table {
                        border-top: 1px solid black;
                        border-collapse: collapse;
                    }

                    td.producto,
                    th.producto {
                        width: 75px;
                        max-width: 75px;
                    }

                    td.cantidad,
                    th.cantidad {
                        width: 40px;
                        max-width: 40px;
                        word-break: break-all;
                    }

                    td.precio,
                    th.precio {
                        width: 40px;
                        max-width: 40px;
                        word-break: break-all;
                    }

                    .centrado {
                        text-align: center;
                        align-content: center;
                    }

                    .ticket {
                        width: 155px;
                        max-width: 155px;
                    }

                    img {
                        max-width: inherit;
                        width: inherit;
                    }
                    </style>
                        </head>
                        <body>
                            <div class='ticket'>
                                <img
                                    src='https://yt3.ggpht.com/-3BKTe8YFlbA/AAAAAAAAAAI/AAAAAAAAAAA/ad0jqQ4IkGE/s900-c-k-no-mo-rj-c0xffffff/photo.jpg'
                                    alt='Logotipo'>
                                <p class='centrado'>Parzibyte's blog
                                    <br>New New York
                                    <br>23/08/2017 08:22 a.m.</p>
                                <table>
                                    <thead>
                                        <tr>
                                            <th class='cantidad'>CANT</th>
                                            <th class='producto'>PRODUCTO</th>
                                            <th class='precio'>$$</th>
                                        </tr>
                                    </thead>
                                    <tbody>
                                        <tr>
                                            <td class='cantidad'>1.00</td>
                                            <td class='producto'>CHEETOS VERDES 80 G</td>
                                            <td class='precio'>$8.50</td>
                                        </tr>
                                        <tr>
                                            <td class='cantidad'>2.00</td>
                                            <td class='producto'>KINDER DELICE</td>
                                            <td class='precio'>$10.00</td>
                                        </tr>
                                        <tr>
                                            <td class='cantidad'>1.00</td>
                                            <td class='producto'>COCA COLA 600 ML</td>
                                            <td class='precio'>$10.00</td>
                                        </tr>
                                        <tr>
                                            <td class='cantidad'></td>
                                            <td class='producto'>TOTAL</td>
                                            <td class='precio'>$28.50</td>
                                        </tr>
                                    </tbody>
                                </table>
                                <p class='centrado'>¡GRACIAS POR SU COMPRA!
                                    <br>parzibyte.me</p>
                            </div>
                        </body>
                    </html>
                ");
                return Json(new { State = 1, data = html.ToString() }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { State = -1, Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult SaveFact()
        {
            try
            {
                string RESPUESTA = "";
                int sinFirma = 0;
                var item = new FEL_BE();
                //item.ID_VENTA = 151;

                for (int i = 322; i <= 322; i++)
                {
                    item = new FEL_BE();
                    item.ID_VENTA = i;

                    var respuestaFEL = Certificador_FEL.Certificador_XML_FAC_FEL(item);
                    if (respuestaFEL.RESULTADO)
                    {
                        var update = new FEL_BE();
                        update.MTIPO = 5;
                        //update.ID_VENTA = 151;
                        update.ID_VENTA = i;
                        update.UUID = respuestaFEL.UUID;
                        update.SERIE_FEL = respuestaFEL.SERIE_FEL;
                        update.NUMERO_FEL = respuestaFEL.NUMERO_FEL;
                        update.FECHA_CERTIFICACION = respuestaFEL.FECHA_CERTIFICACION;
                        var respuesta_update = FEL_BLL.GetDatosSP(update).FirstOrDefault();

                        RESPUESTA = "FACTURA FIRMADA Y GUARDADA";
                    }
                    else
                    {
                        //RESPUESTA = respuestaFEL.MENSAJE_FEL;
                        RESPUESTA = $"{RESPUESTA}, {i.ToString()}";
                    }
                }

                RESPUESTA = "Sin firma: " + RESPUESTA;

                return Json(new { RESPUESTA = RESPUESTA }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { State = -1, Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public FEL_BE Certificar_Factura_FEL(int idVenta = 0)
        {
            var respuesta = new FEL_BE();
            try
            {
                respuesta.ID_VENTA = idVenta;
                respuesta = Certificador_FEL.Certificador_XML_FAC_FEL(respuesta);
                if (respuesta.RESULTADO)
                {
                    var update = new FEL_BE();
                    update.MTIPO = 5;
                    update.ID_VENTA = respuesta.ID_VENTA;
                    update.UUID = respuesta.UUID;
                    update.SERIE_FEL = respuesta.SERIE_FEL;
                    update.NUMERO_FEL = respuesta.NUMERO_FEL;
                    update.FECHA_CERTIFICACION = respuesta.FECHA_CERTIFICACION;
                    var respuesta_update = FEL_BLL.GetDatosSP(update).FirstOrDefault();
                }
            }
            catch
            {
            }
            return respuesta;
        }
        public FEL_BE Anular_Factura_FEL(int idVenta = 0)
        {
            var respuesta = new FEL_BE();
            try
            {
                respuesta.ID_VENTA = idVenta;
                respuesta = Certificador_FEL.Anulador_XML_FEL(respuesta);
                if (respuesta.RESULTADO)
                {
                    var update = new FEL_BE();
                    update.MTIPO = 5;
                    update.ID_VENTA = respuesta.ID_VENTA;
                    update.UUID = respuesta.UUID;
                    update.SERIE_FEL = respuesta.SERIE_FEL;
                    update.NUMERO_FEL = respuesta.NUMERO_FEL;
                    update.FECHA_CERTIFICACION = respuesta.FECHA_CERTIFICACION;
                    var respuesta_update = FEL_BLL.GetDatosSP(update).FirstOrDefault();
                }
            }
            catch
            {
            }
            return respuesta;
        }
    }
}