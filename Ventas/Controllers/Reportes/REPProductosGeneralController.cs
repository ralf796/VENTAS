using GenesysOracleSV.Clases;
using OfficeOpenXml;
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
    public class REPProductosGeneralController : Controller
    {
        // GET: REPProductosGeneral
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
        public JsonResult getProducto(string fechaInicial = "", string fechaFinal = "")
        {
            try
            {
                var item = new Reportes_BE();
                item.MTIPO = 11;
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

        public ActionResult GenerarExcel(string fechaInicial = "", string fechaFinal = "")
        {
            MemoryStream fileExcel = new MemoryStream();
            ExcelWorksheet xlSheet;
            ExcelRange xlRange = null;

            using (ExcelPackage package = new ExcelPackage(fileExcel))
            {
                xlSheet = package.Workbook.Worksheets.Add("PRODUCTOS");

                var item = new Reportes_BE();
                item.MTIPO = 11;
                item.FECHA_INICIAL = Convert.ToDateTime(fechaInicial);
                item.FECHA_FINAL = Convert.ToDateTime(fechaFinal);
                var lista = GetSPReportes_(item);

                int nCell = 1;
                int nCol = 1;
                int rangoBorder = 0;
                string vRango = "";
                
                Utils.MergeCellsExcel(xlSheet, xlRange, nCell, nCol, nCol + 7, "Distribuidora de Auto Repuestos El Eden", 16, true, "L"); nCell++;
                Utils.MergeCellsExcel(xlSheet, xlRange, nCell, nCol, nCol + 7, $"Productos cargados de {Utils.Fecha_Larga_Letras(item.FECHA_INICIAL)} a {Utils.Fecha_Larga_Letras(item.FECHA_FINAL)}", 12, true, "L");

                nCell = 3; nCol = 1;
                Utils.AddTextCells(xlSheet, "C", "CÓDIGO INTERNO", nCell, nCol); nCol++;
                Utils.AddTextCells(xlSheet, "C", "CÓDIGO 1", nCell, nCol); nCol++;
                Utils.AddTextCells(xlSheet, "C", "CÓDIGO 2", nCell, nCol); nCol++;
                Utils.AddTextCells(xlSheet, "C", "NOMBRE", nCell, nCol); nCol++;
                Utils.AddTextCells(xlSheet, "C", "DESCRIPCION", nCell, nCol); nCol++;
                Utils.AddTextCells(xlSheet, "C", "PRECIO COSTO", nCell, nCol); nCol++;
                Utils.AddTextCells(xlSheet, "C", "PRECIO VENTA", nCell, nCol); nCol++;
                Utils.AddTextCells(xlSheet, "C", "GANANCIA", nCell, nCol); nCol++;
                Utils.AddTextCells(xlSheet, "C", "STOCK", nCell, nCol); nCol++;
                Utils.AddTextCells(xlSheet, "C", "ESTADO", nCell, nCol); nCol++;
                Utils.AddTextCells(xlSheet, "C", "MARCA VEHICULO", nCell, nCol); nCol++;
                Utils.AddTextCells(xlSheet, "C", "LINEA VEHICULO", nCell, nCol); nCol++;
                Utils.AddTextCells(xlSheet, "C", "AÑO INICIAL", nCell, nCol); nCol++;
                Utils.AddTextCells(xlSheet, "C", "AÑO FINAL", nCell, nCol);

                Utils.FillBackgroundRange(xlSheet, xlRange, nCell, 1, 14, "blue1");

                xlSheet.Row(3).Style.Locked = true;

                if (lista.Count > 0)
                {
                    nCell = 4;
                    nCol = 1;
                    foreach (var dato in lista)
                    {
                        Utils.AddTextCells(xlSheet, "C", dato.CODIGO_INTERNO, nCell, nCol); nCol++;
                        Utils.AddTextCells(xlSheet, "C", dato.CODIGO, nCell, nCol); nCol++;
                        Utils.AddTextCells(xlSheet, "C", dato.CODIGO2, nCell, nCol); nCol++;
                        Utils.AddTextCells(xlSheet, "L", dato.NOMBRE, nCell, nCol); nCol++;
                        Utils.AddTextCells(xlSheet, "L", dato.DESCRIPCION, nCell, nCol); nCol++;
                        Utils.AddText(xlSheet, nCell, nCol, "", dato.PRECIO_COSTO, 12, false, "R", 1); nCol++;
                        Utils.AddText(xlSheet, nCell, nCol, "", dato.PRECIO_VENTA, 12, false, "R", 1); nCol++;
                        Utils.AddText(xlSheet, nCell, nCol, "", dato.GANANCIA, 12, false, "R", 1); nCol++;
                        Utils.AddText(xlSheet, nCell, nCol, "", dato.STOCK, 12, false, "R", 1); nCol++;
                        Utils.AddTextCells(xlSheet, "L", dato.ESTADO_STRING, nCell, nCol); nCol++;
                        Utils.AddTextCells(xlSheet, "L", dato.NOMBRE_MARCA_VEHICULO, nCell, nCol); nCol++;
                        Utils.AddTextCells(xlSheet, "L", dato.NOMBRE_SERIE_VEHICULO, nCell, nCol); nCol++;
                        Utils.AddText(xlSheet, nCell, nCol, "", dato.ANIO_INICIAL, 12, false, "R", 0); nCol++;
                        Utils.AddText(xlSheet, nCell, nCol, "", dato.ANIO_FINAL, 12, false, "R", 0); 
                        nCell++;
                        rangoBorder = nCell - 1;
                        nCol = 1;
                    }
                    vRango = "A3:N" + rangoBorder;
                    Utils.FillBorderCellsAll(xlSheet, vRango);

                    nCell++;
                    nCol = 1;
                    Utils.MergeCellsExcel(xlSheet, xlRange, nCell, nCol, nCol + 7, "Número de productos seleccionados: " + lista.Count, 12, true, "L");
                }

                //Pages View                
                xlSheet.View.ShowGridLines = false;
                package.Save();
            }
            fileExcel.Seek(0, SeekOrigin.Current);
            string file64 = Convert.ToBase64String(fileExcel.ToArray());

            var file = new { File = file64, MimeType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", FileName = $"Productos.xlsx" };
            return Json(file);
        }
    }
}