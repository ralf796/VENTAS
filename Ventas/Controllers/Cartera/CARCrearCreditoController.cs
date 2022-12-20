using SelectPdf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Ventas.Class;
using Ventas_BE;
using Ventas_BLL;

namespace Ventas.Controllers.Cartera
{
    public class CARCrearCreditoController : Controller
    {
        // GET: CARCrearCredito
        [SessionExpireFilter]
        public ActionResult Index()
        {
            return View();
        }

        private static List<Cartera_BE> GetDatosSP_(Cartera_BE item)
        {
            List<Cartera_BE> lista = new List<Cartera_BE>();
            lista = Cartera_BLL.GetDatosSP(item);
            return lista;
        }

        public JsonResult GetVenta(int MTIPO = 0, int ID_VENTA = 0)
        {
            try
            {
                var item = new Cartera_BE();
                item.ID_VENTA = ID_VENTA;
                item.MTIPO = MTIPO;

                item = GetDatosSP_(item).FirstOrDefault();

                if (item != null)
                {

                    item.FECHA_STRING = item.FECHA.ToString("dd/MM/yyyy hh:mm tt");
                    item.FECHA_CERTIFICACION_STRING = item.FECHA_CERTIFICACION.ToString("dd/MM/yyyy hh:mm tt");
                }

                return Json(new { State = 1, data = item }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { State = -1, Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult CrearRecibo(int ID_VENTA = 0, decimal ABONO = 0, string OBSERVACIONES = "")
        {
            try
            {
                var item = new Cartera_BE();
                item.ID_VENTA = ID_VENTA;
                item.MTIPO = 4;
                item.ABONO = ABONO;
                item.OBSERVACIONES = OBSERVACIONES;
                item.CREADO_POR = Session["usuario"].ToString();
                item = GetDatosSP_(item).FirstOrDefault();

                return Json(new { State = 1, data = item }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { State = -1, Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult GenerarReciboPDF(int id_venta = 0)
        {
            var itemHeader = new Cartera_BE();
            var item = new Cartera_BE();
            List<Cartera_BE> listaDetalles = new List<Cartera_BE>();
            item.ID_VENTA = id_venta;
            item.MTIPO = 6;
            listaDetalles = GetDatosSP_(item);
            itemHeader = listaDetalles.FirstOrDefault();


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
                    <b>NO. CRÉDITO:</b> {itemHeader.ID_ESTADO_CUENTA}<br>
                    <b>FECHA DE PAGO:</b> {Convert.ToDateTime(itemHeader.FECHA_PAGO).ToString("dd/MM/yyyy")}<br>
                    <b>ÓRDEN DE COMPRA:</b> {itemHeader.ID_VENTA}<br>
                    <b>FORMA DE PAGO:</b> CRÉDITO<br>
                    <b>CLIENTE:</b> {itemHeader.NOMBRE}<br>
                    <b>DIRECCIÓN:</b> {itemHeader.DIRECCION}<br>
                    <b>FECHA:</b> {DateTime.Now.ToString("dd/MM/yyyy")}<br>
                    <b>FECHA Y HORA DE VENTA:</b> {itemHeader.FECHA_VENTA.ToString("dd/MM/yyyy hh:mm tt")}<br>
                    <b>VENTA EMITIDA POR:</b> {itemHeader.CREADO_POR}<br><br>
                    </div>
            </div>
            <div class='row justify-content-center'>
                <div class='col-7 pt-4'>
                    <table class='table table-sm' id='tdDatos'>
                        <thead>
                            <tr style=''>
                                <th class='pl-2 pr-2 border text-center' style='vertical-align:middle; border: 1px solid'>ABONO</th>
                                <th class='pl-2 pr-2 border text-center' style='vertical-align:middle; border: 1px solid'>NO. RECIBO</th>
                                <th class='pl-2 pr-2 border text-center' style='vertical-align:middle; border: 1px solid'>FECHA ABONO</th>
                                <th class='pl-2 pr-2 border text-center' style='vertical-align:middle; border: 1px solid'>OBSERVACIONES</th>
                                <th class='pl-2 pr-2 border text-center' style='vertical-align:middle; border: 1px solid'>MONTO ABONO</th>
                                <th class='pl-2 pr-2 border text-center' style='vertical-align:middle; border: 1px solid'>CAJERO</th>
                            </tr>
                        </thead>
                        <tbody id='tbodyListado'>");
            int abonoCont = 0;
            if (itemHeader.FECHA_CREACION_RECIBO.Year > 2020)
            {
                foreach (var dato in listaDetalles)
                {
                    abonoCont++;
                    html.AppendLine("<tr>");
                    html.AppendLine($"<td class='text-center' style='border-bottom: 1px solid'>{abonoCont}</td>");
                    html.AppendLine($"<td class='text-center' style='border-bottom: 1px solid'>{dato.NO_RECIBO}</td>");
                    html.AppendLine($"<td class='text-left' style='border-bottom: 1px solid'>{dato.FECHA_CREACION_RECIBO}</td>");
                    html.AppendLine($"<td class='text-left' style='border-bottom: 1px solid'>{dato.OBSERVACIONES}</td>");
                    html.AppendLine($"<td class='text-right' style='border-bottom: 1px solid'>{dato.MONTO_RECIBO}</td>");
                    html.AppendLine($"<td class='text-right' style='border-bottom: 1px solid'>{dato.RECIBO_CREADO_POR}</td>");
                    html.AppendLine("</tr>");                    
                }
            }
            html.AppendLine($@"</tbody>
                    </table>
                </div>
            </div>
            <div class='row'>
                <div class='col-md-12 pl-5 text-center'>
                    <b>CANTIDAD DE ABONOS :</b> <span class='font-weight-bold pl-2' style='font-size:22px'> {abonoCont}</span><br/><br/>
                    <b>MONTO ABONADO:</b> <span class='font-weight-bold pl-2' style='font-size:22px'> Q{itemHeader.TOTAL_VENTA.ToString("N2")}</span><br>
                    <b>MONTO ABONADO EN LETRAS: </b> {new NumeroLetra().Convertir(itemHeader.TOTAL_VENTA.ToString(), true)}<br><br/>
                    <b>MONTO ABONADO:</b> <span class='font-weight-bold pl-2' style='font-size:22px'> Q{itemHeader.ABONO.ToString("N2")}</span><br>
                    <b>MONTO ABONADO EN LETRAS: </b> {new NumeroLetra().Convertir(itemHeader.ABONO.ToString(), true)}<br><br/>
                    <b>SALDO PENDIENTE:</b><span class='font-weight-bold pl-2'  style='font-size:22px'> Q{itemHeader.SALDO.ToString("N2")}</span><br>
                    <b>SALDO PENDIENTE EN LETRAS: </b> {new NumeroLetra().Convertir(itemHeader.SALDO.ToString(), true)}<br><br/>
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
            var file = new { File = file64, MimeType = "application/pdf", FileName = $"CREDITO {itemHeader.ID_ESTADO_CUENTA} - {itemHeader.NOMBRE}.pdf" };
            return Json(file);
        }
    }
}