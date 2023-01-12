using SelectPdf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Ventas_BE;
using Ventas_BLL;

namespace Ventas.Controllers.Reportes
{
    public class REPAntiguedadSaldosController : Controller
    {
        // GET: REPAntiguedadSaldos
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

        public JsonResult LoadReporte(string fechaInicial = "", string fechaFinal = "")
        {
            try
            {
                var item = new Reportes_BE();
                item.MTIPO = 27;
                item.FECHA_INICIAL = Convert.ToDateTime(fechaInicial);
                item.FECHA_FINAL = Convert.ToDateTime(fechaFinal);
                var lista = GetSPReportes_(item);
                foreach (var row in lista)
                    row.FECHA_CREACION_STRING = row.FECHA_VENCIMIENTO.ToString("dd/MM/yyyy hh:mm tt");
                return Json(new { State = 1, data = lista }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { State = -1, Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult PDFTest(string fecha1 = "", string fecha2 = "")
        {
            System.Text.StringBuilder html = new System.Text.StringBuilder();

            var item = new Reportes_BE();
            item.MTIPO = 27;
            item.FECHA_INICIAL = Convert.ToDateTime(fecha1);
            item.FECHA_FINAL = Convert.ToDateTime(fecha2);
            var Encabezado = GetSPReportes_(item);

            html.AppendLine($@"
            <!DOCTYPE html>
            <html>
            <head>
                <meta charset='utf-8' />
                <link rel='stylesheet' href='https://cdn.jsdelivr.net/npm/bootstrap@4.0.0/dist/css/bootstrap.min.css' integrity='sha384-Gn5384xqQ1aoWXA+058RXPxPg6fy4IWvTNh0E263XmFcJlSAwiGgFAW/dAiS6JXm' crossorigin='anonymous'>
                <script src='http://code.jquery.com/jquery-latest.min.js'></script>
            </head>
            <body>
                <div class='col-10 offset-1'>
                    <h2 class='text-center mt-1'><strong>ESTADOS DE CUENTA POR CLIENTE</strong></h2>
                    <BR>
                        <h4 class='text-center mt-1'> REPORTE DETALLADO DEL {item.FECHA_INICIAL.ToString("dd/MM/yyyy")} AL {item.FECHA_FINAL.ToString("dd/MM/yyyy")}</h4>
                </div>
                <div class='col-12'>
                    <table class='table-sm' style='width:100%' width='100 %'>
                        <thead style='font-size:16px'>
                            <tr style='background-color:gray; color:#FFFFFF'>
                                <th class='pl-2 pr-2 border text-center' style='vertical-align:middle;'>CLIENTE</th>
                                <th class='pl-2 pr-2 border text-center' style='vertical-align:middle;'>FECHA<br>VENCIMIENTO</th>
                                <th class='pl-2 pr-2 border text-center' style='vertical-align:middle;'>DIAS<br>ATRASO</th>
                                <th class='pl-2 pr-2 border text-center' style='vertical-align:middle;'>DOCUMENTO</th>
                                <th class='pl-2 pr-2 border text-center' style='vertical-align:middle;'>TOTAL<br>FACTURA</th>
                                <th class='pl-2 pr-2 border text-center' style='vertical-align:middle;'>SALDO</th>
                                <th class='pl-2 pr-2 border text-center' style='vertical-align:middle;'>A 30 DÍAS</th>
                                <th class='pl-2 pr-2 border text-center' style='vertical-align:middle;'>31 - 60<br>DÍAS</th>
                                <th class='pl-2 pr-2 border text-center' style='vertical-align:middle;'>61 - 90<br>DÍAS</th>
                                <th class='pl-2 pr-2 border text-center' style='vertical-align:middle;'>91 - 120<br>DÍAS</th>
                                <th class='pl-2 pr-2 border text-center' style='vertical-align:middle;'>MAYOR A 120<br>DÍAS</th>
                            </tr>
                        </thead>
                        <tbody style='font-size:15px'>
            ");
            foreach (var row in Encabezado)
            {
                var itemDetalle = new Reportes_BE();
                item.MTIPO = 28;
                item.FECHA_INICIAL = Convert.ToDateTime(fecha1);
                item.FECHA_FINAL = Convert.ToDateTime(fecha2);
                item.ID_VENTA = row.ID_CLIENTE;
                var Detalle = GetSPReportes_(item);

                bool banderaCliente = false;
                foreach (var row2 in Detalle)
                {
                    if (banderaCliente == true)
                        row.NOMBRE_CLIENTE = "";

                    html.AppendLine($@"
                            <tr>
                                <td class='text-left'>{row.NOMBRE_CLIENTE}</td>
                                <td class='text-center'>{row2.FECHA_VENCIMIENTO.ToString("dd/MM/yyyy")}</td>
                                <td class='text-center'>{row2.DIAS_ATRASO}</td>
                                <td class='text-center'>{row2.ID_VENTA}</td>
                                <td class='text-right pr-2'>{(row2.TOTAL_VENTA > 0 ? row2.TOTAL_VENTA.ToString("N2") : "-")}</td>
                                <td class='text-right pr-2'>{(row2.SALDO > 0 ? row2.SALDO.ToString("N2") : "-")}</td>
                                <td class='text-right pr-2'>{(row2.D_1_30 > 0 ? row2.D_1_30.ToString("N2") : "-")}</td>
                                <td class='text-right pr-2'>{(row2.D_31_60 > 0 ? row2.D_31_60.ToString("N2") : "-")}</td>
                                <td class='text-right pr-2'>{(row2.D_61_90 > 0 ? row2.D_61_90.ToString("N2") : "-")}</td>
                                <td class='text-right pr-2'>{(row2.D_91_120 > 0 ? row2.D_91_120.ToString("N2") : "-")}</td>
                                <td class='text-right pr-2'>{(row2.D_121_ > 0 ? row2.D_121_.ToString("N2") : "-")}</td>                                
                            </tr>
                    ");
                    banderaCliente = true;
                }

                if (Detalle != null)
                {
                    html.AppendLine($@"
                            <tr>
                                <td></td>
                                <td></td>
                                <td></td>
                                <td></td>
                                <td class='text-right pr-2' style='border-top: 1px solid;font-weight:bold'>{Detalle.Sum(x => x.TOTAL_VENTA).ToString("N2")}</td>
                                <td class='text-right pr-2' style='border-top: 1px solid;font-weight:bold'>{Detalle.Sum(x => x.SALDO).ToString("N2")}</td>
                                <td class='text-right pr-2' style='border-top: 1px solid;font-weight:bold'>{Detalle.Sum(x => x.D_1_30).ToString("N2")}</td>
                                <td class='text-right pr-2' style='border-top: 1px solid;font-weight:bold'>{Detalle.Sum(x => x.D_31_60).ToString("N2")}</td>
                                <td class='text-right pr-2' style='border-top: 1px solid;font-weight:bold'>{Detalle.Sum(x => x.D_61_90).ToString("N2")}</td>
                                <td class='text-right pr-2' style='border-top: 1px solid;font-weight:bold'>{Detalle.Sum(x => x.D_91_120).ToString("N2")}</td>
                                <td class='text-right pr-2' style='border-top: 1px solid;font-weight:bold'>{Detalle.Sum(x => x.D_121_).ToString("N2")}</td>
                            </tr>
                    ");
                }
            }
            html.AppendLine(@"
                        </tbody>
                    </table>
                </div>
            </body>
            </html>
        ");

            int webPageWidth = 1400;
            int webPageHeight = 600;

            // instantiate a html to pdf converter object
            HtmlToPdf converter = new HtmlToPdf();

            // set converter options
            converter.Options.PdfPageSize = PdfPageSize.Letter;
            converter.Options.PdfPageOrientation = PdfPageOrientation.Portrait;
            converter.Options.WebPageWidth = webPageWidth;
            converter.Options.WebPageHeight = webPageHeight;
            converter.Options.MarginLeft = 5;
            converter.Options.MarginRight = 5;
            converter.Options.MarginTop = 30;
            converter.Options.MarginBottom = 40;

            // create a new pdf document converting an url
            PdfDocument doc = converter.ConvertHtmlString(html.ToString(), "base");
            MemoryStream fileStream = new MemoryStream();
            doc.Save(fileStream);

            // close pdf document
            doc.Close();

            string file64 = Convert.ToBase64String(fileStream.ToArray());
            var file = new { File = file64, MimeType = "application/pdf", FileName = $"TEST PDF.pdf" };
            return Json(file);
        }
    }
}