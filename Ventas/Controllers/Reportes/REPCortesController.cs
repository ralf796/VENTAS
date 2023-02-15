using SelectPdf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using Ventas.Class;
using Ventas_BE;
using Ventas_BLL;
namespace Ventas.Controllers.Reportes
{
    public class REPCortesController : Controller
    {
        // GET: REPCortes
        [SessionExpireFilter]
        public ActionResult Index()
        {
            return View();
        }
        private List<Reportes_BE> GetSPReportes_(Reportes_BE item)
        {
            List<Reportes_BE> lista = new List<Reportes_BE>();
            lista = Reportes_BLL.GetSPReportes(item);
            return lista;
        }

        public JsonResult getCortes(string fechaInicial = "", string fechaFinal = "")
        {
            try
            {
                var item = new Reportes_BE();
                item.MTIPO = 6;
                item.FECHA_INICIAL = Convert.ToDateTime(fechaInicial);
                item.FECHA_FINAL = Convert.ToDateTime(fechaFinal);
                var lista = GetSPReportes_(item);
                foreach (var row in lista)
                    row.FECHA_CREACION_STRING = row.FECHA_CREACION.ToString("dd/MM/yyyy");
                return Json(new { State = 1, data = lista }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { State = -1, Message = ex.Message }, JsonRequestBehavior.AllowGet);

            }
        }

        // funcion que devuelve el total de venta segun rango de fechas
        public JsonResult getTotalCorte(string fechaInicial = "", string fechaFinal = "")
        {
            try
            {
                var item = new Reportes_BE();
                item.MTIPO = 7;
                item.FECHA_INICIAL = Convert.ToDateTime(fechaInicial);
                item.FECHA_FINAL = Convert.ToDateTime(fechaFinal);
                var lista = GetSPReportes_(item);
                return Json(new { State = 1, data = lista }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { State = -1, Message = ex.Message }, JsonRequestBehavior.AllowGet);

            }
        }

        public JsonResult GetDetalle(int id_venta = 0)
        {
            try
            {
                var item = new Reportes_BE();
                item.MTIPO = 31;
                item.FECHA_INICIAL = DateTime.Now;
                item.FECHA_FINAL = DateTime.Now;
                item.ID_VENTA = id_venta;
                var lista = GetSPReportes_(item);
                foreach (var row in lista)
                    row.FECHA_CREACION_STRING = row.FECHA_CREACION.ToString("dd/MM/yyyy");

                return Json(new { State = 1, data = lista }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { State = -1, Message = ex.Message }, JsonRequestBehavior.AllowGet);

            }
        }

        [SessionExpireFilter]
        public ActionResult PDF_Corte(int corte = 0)
        {
            var item = new Reportes_BE();
            item.MTIPO = 32;
            item.FECHA_INICIAL = DateTime.Now;
            item.FECHA_FINAL = DateTime.Now;
            item.ID_VENTA = corte;
            item = Reportes_BLL.GetSPReportes(item).FirstOrDefault();

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
                <div class='col-md-12 pl-5' style='margin-top:5px; font-size: 22px;'>
                    <b style='font-size: 22px;'>CORTE:</b> {corte}<br>
                    <b style='font-size: 22px;'>IMPRESO POR:</b> {Session["usuario"].ToString()}<br>
                    <b style='font-size: 22px;'>FECHA IMPRESIÓN:</b> {DateTime.Now.ToString("dd/MM/yyyy hh:mm")}<br>                    
                    <b style='font-size: 22px;'>CAJERO:</b> {item.CREADO_POR}<br>
                    <b style='font-size: 22px;'>FECHA CORTE:</b> {item.FECHA_CORTE.ToString("dd/MM/yyyy hh:mm")}<br>
                    <b style='font-size: 22px;'>MONTO CORTE:</b> {item.TOTAL.ToString("N2")}<br>                    
                    <b style='font-size: 22px;'>MONTO CORTE EN LETRAS:</b> {new NumeroLetra().Convertir(item.TOTAL.ToString(), true)}<br>                    
                </div>
            </div>
            ");
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
            var file = new { File = file64, MimeType = "application/pdf", FileName = $"Corte_{corte}.pdf" };
            return Json(file);
        }
    }
}