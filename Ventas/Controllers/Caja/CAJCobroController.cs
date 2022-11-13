using GenesysOracle.Clases;
using GenesysOracleSV.Clases;
using Newtonsoft.Json;
using SelectPdf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Ventas_BE;
using Ventas_BLL;
namespace Ventas.Controllers.Caja
{
    public class CAJCobroController : Controller
    {
        // GET: CAJCobro
        [SessionExpireFilterAttribute]
        public ActionResult Index()
        {
            return View();
        }
        private List<Caja_BE> GetDatosCaja_(Caja_BE item)
        {
            List<Caja_BE> lista = new List<Caja_BE>();
            lista = Caja_BLL.GetSPCaja(item);
            return lista;
        }
        //FUNCION LISTAR COBRO
        [SessionExpireFilter]
        public JsonResult GetCobro()
        {
            try
            {
                var item = new Caja_BE();
                item.MTIPO = 1;
                var lista = GetDatosCaja_(item);
                foreach (var row in lista)
                    row.FECHA_CREACION_STRING = row.FECHA_CREACION.ToString("dd/MM/yyyy");
                return Json(new { State = 1, data = lista }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { State = -1, Message = ex.Message }, JsonRequestBehavior.AllowGet);

            }
        }
        //funcion LISTAR DETALLE VENTA
        [SessionExpireFilter]
        public JsonResult GetDetalle(int id_venta = 0)
        {
            try
            {
                var item = new Caja_BE();
                item.ID_VENTA = id_venta;
                item.MTIPO = 2;
                var lista = GetDatosCaja_(item);
                return Json(new { State = 1, data = lista }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { State = -1, Message = ex.Message }, JsonRequestBehavior.AllowGet);

            }
        }
        public JsonResult GetDetalle2(int id_venta = 0)
        {
            try
            {
                var item = new Caja_BE();
                item.ID_VENTA = id_venta;
                item.MTIPO = 2;
                var lista = GetDatosCaja_(item);
                return Json(new { State = 1, data = lista }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { State = -1, Message = ex.Message }, JsonRequestBehavior.AllowGet);

            }
        }
        // funcion cobro efectuado
        [SessionExpireFilter]
        public JsonResult getCobroEfectuado(int id = 0, decimal cobro = 0, int formaPago = 0)
        {
            try
            {
                var item = new Caja_BE();
                item.ID_VENTA = id;
                item.TOTAL = cobro;
                if (formaPago == 1)
                {
                    item.TIPO_COBRO = "E";
                }
                else if (formaPago == 2)
                {
                    item.TIPO_COBRO = "T";
                }
                item.CREADO_POR = Session["usuario"].ToString();
                item.MTIPO = 3;
                var lista = GetDatosCaja_(item);
                return Json(new { State = 1, data = lista }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { State = -1, Message = ex.Message }, JsonRequestBehavior.AllowGet);

            }

        }
        /*---------------------------------funcion Anular Venta*/
        [SessionExpireFilter]
        public JsonResult getAularVenta(int id_venta = 0)
        {
            try
            {
                var item = new Caja_BE();
                item.ID_VENTA = id_venta;
                item.MTIPO = 5;
                var lista = GetDatosCaja_(item);
                return Json(new { State = 1, data = lista }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { State = -1, Message = ex.Message }, JsonRequestBehavior.AllowGet);

            }
        }
        //impresion
        public ActionResult GetComprobante(string encabezado = "", string detalles = "")
        {
            var item = JsonConvert.DeserializeObject<Caja_BE>(encabezado);
            List<Caja_BE> listaDetalles = JsonConvert.DeserializeObject<List<Caja_BE>>(detalles);

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
                    <b>SERIE:</b> {item.SERIE}<br>
                    <b>CORRELATIVO:</b> {item.CORRELATIVO}<br>
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
                            <th class='pl-2 pr-2 border text-center' style='vertical-align:middle; border: 1px solid'>DESCRIPCIÓN</th>
                            <th class='pl-2 pr-2 border text-center' style='vertical-align:middle; border: 1px solid'>PRECIO UNITARIO</th>
                            <th class='pl-2 pr-2 border text-center' style='vertical-align:middle; border: 1px solid'>DESCUENTO</th>
                            <th class='pl-2 pr-2 border text-center' style='vertical-align:middle; border: 1px solid'>TOTAL SIN DESCUENTO</th>
                            <th class='pl-2 pr-2 border text-center' style='vertical-align:middle; border: 1px solid'>TOTAL CON DESCUENTO</th>
                        </tr>
                    </thead>
                    <tbody id='tbodyListado'>");

            foreach (var dato in listaDetalles)
            {
                html.AppendLine("<tr>");
                html.AppendLine($"<td class='text-center' style='border-bottom: 1px solid'>{dato.CANTIDAD}</td>");
                html.AppendLine($"<td class='text-left' style='border-bottom: 1px solid'>{dato.NOMBRE}</td>");
                html.AppendLine($"<td class='text-right' style='border-bottom: 1px solid'>{dato.PRECIO_UNITARIO.ToString("N2")}</td>");
                html.AppendLine($"<td class='text-right' style='border-bottom: 1px solid'>{dato.DESCUENTO.ToString("N2")}</td>");
                html.AppendLine($"<td class='text-right' style='border-bottom: 1px solid'>{dato.TOTAL.ToString("N2")}</td>");
                html.AppendLine($"<td class='text-right' style='border-bottom: 1px solid'>{dato.SUBTOTAL.ToString("N2")}</td>");

                html.AppendLine("</tr>");

            }
            html.AppendLine($@"</tbody>
                </table>
            </div>
            <div class='row'>
                <div class='col-md-12 pl-5 text-center'>
                    <b>TOTAL SIN DESCUENTO :</b> <span class='font-weight-bold pl-2' style='font-size:22px'> Q{item.TOTAL.ToString("N2")}</span><br>
                    <b>DESCUENTO:</b> <span class='font-weight-bold pl-2' style='font-size:22px'> Q{item.DESCUENTO.ToString("N2")}</span><br>
                    <b>TOTAL A PAGAR:</b><span class='font-weight-bold pl-2'  style='font-size:22px'> Q{item.SUBTOTAL.ToString("N2")}</span><br>
                    <b>TOTAL EN LETRAS: </b> {new NumeroLetra().Convertir(item.SUBTOTAL.ToString(), true)}<br>
                </div>
            </div>
            <div class='row pt-5'>
                <div class='col-md-12 text-center border-dark border-top'>
                   
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
            var file = new { File = file64, MimeType = "application/pdf", FileName = $"Comprobante_{item.SERIE} {item.CORRELATIVO}_{DateTime.Now.ToString("ddMMyyyy")}.pdf" };
            return Json(file);
        }
    }
}