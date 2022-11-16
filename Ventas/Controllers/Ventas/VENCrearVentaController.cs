﻿using GenesysOracle.Clases;
using GenesysOracleSV.Clases;
using Newtonsoft.Json;
using SelectPdf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using Ventas_BE;
using Ventas_BLL;


namespace Ventas.Controllers.Ventas
{
    public class VENCrearVentaController : Controller
    {
        // GET: VENCrearVenta
        //[SessionExpireFilter]
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
        private bool SaveHeader(int idVenta = 0, string serie = "", decimal correlativo = 0, int idCliente = 0, decimal total = 0, decimal descuento = 0, decimal subtotal = 0, string usuario = "")
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
        public JsonResult SaveOrder(string encabezado = "", string detalles = "")
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

                if (SaveHeader(Convert.ToInt32(item.ID_VENTA), "SSS", 1, item.ID_CLIENTE, item.TOTAL, item.TOTAL_DESCUENTO, item.SUBTOTAL, usuario) == true)
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

                if (banderaDetail == true)
                    state = 2;
                else
                {
                    DescontProduct(Convert.ToInt32(item.ID_VENTA));
                }
                return Json(new { State = state, ORDEN_COMPRA = item.ID_VENTA }, JsonRequestBehavior.AllowGet);
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

        //        ID_PRODUCTO=22200&NOMBRE=SN,DESCRIPCION=CABLE CANDELA MAZDA PROTEGE 1.8L 95-98,PRECIO_COSTO=203.1,PRECIO_VENTA=365.41,usuarioModificaADMIN
        //https://localhost:44302/VENCrearVenta/UpdateProducto?ID_PRODUCTO=22200&NOMBRE=SN&DESCRIPCION=CABLE CANDELA MAZDA PROTEGE 1.8L 95-98&PRECIO_COSTO=203.1&PRECIO_VENTA=365.41&usuarioModificaADMIN
        public JsonResult UpdateProducto(int ID_PRODUCTO = 0, string NOMBRE = "", string DESCRIPCION = "", decimal PRECIO_COSTO = 0, decimal PRECIO_VENTA = 0, string usuarioModifica = "")
        {
            try
            {
                int estado = 0;
                var item = new Inventario_BE();
                item.ID_PRODUCTO = ID_PRODUCTO;
                item.NOMBRE = NOMBRE;
                item.DESCRIPCION = DESCRIPCION;
                item.STOCK = 0;
                item.PRECIO_COSTO = PRECIO_COSTO;
                item.PRECIO_VENTA = PRECIO_VENTA;
                item.CREADO_POR = "RALOPEZ";//Session["usuario"].ToString();
                item.MTIPO = 8;
                item.CODIGO = usuarioModifica;
                item = GetDatosInventario_(item).FirstOrDefault();
                if (item != null)
                    estado = 1;
                else
                    estado = 2;


                return Json(new { State = estado, data = item }, JsonRequestBehavior.AllowGet);
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
    }
}