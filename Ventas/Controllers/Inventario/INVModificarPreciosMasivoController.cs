using MoreLinq;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Ventas.Class;
using Ventas_BE;
using Ventas_BLL;

namespace Ventas.Controllers.Inventario
{
    public class INVModificarPreciosMasivoController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult CargarExcel(FormCollection formCollection)
        {
            try
            {
                HttpPostedFileBase file = Request.Files["FileUpload"];
                List<Inventario_BE> list = new List<Inventario_BE>();

                if ((file != null) && (file.ContentLength > 0) && !string.IsNullOrEmpty(file.FileName))
                {
                    string fileName = file.FileName;
                    string fileContentType = file.ContentType;
                    byte[] fileBytes = new byte[file.ContentLength];
                    var data = file.InputStream.Read(fileBytes, 0, Convert.ToInt32(file.ContentLength));
                    using (var package = new ExcelPackage(file.InputStream))
                    {
                        var currentSheet = package.Workbook.Worksheets;
                        var workSheet = currentSheet.First();
                        var noOfCol = workSheet.Dimension.End.Column;
                        var noOfRow = workSheet.Dimension.End.Row;

                        for (int rowIterator = 2; rowIterator <= noOfRow; rowIterator++)
                        {
                            if (workSheet.Cells[rowIterator, 1].Value != null)
                            {
                                var row = new Inventario_BE();
                                row.CODIGO_INTERNO= NullString(workSheet.Cells[rowIterator, 1].Value.ToString());
                                row.PRECIO_VENTA = NullDecimal(workSheet.Cells[rowIterator, 2].Value.ToString());
                                row.CREADO_POR = Session["usuario"].ToString();
                                list.Add(row);
                            }
                        }
                    }
                }

                foreach (var row in list)
                {
                    row.MTIPO = 10;
                    List<Inventario_BE> lista = new List<Inventario_BE>();
                    lista = Inventario_BLL.GetSPInventario(row);
                }

                return Json(new { State = 1, data = list, dataGroup = list.Count(), dataGroupDetail = list.Count() }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { State = -1, Message = ex.Message.ToString() }, JsonRequestBehavior.AllowGet);
            }
        }

        private string NullString(string cadena)
        {
            string respuesta = "";
            try
            {
                if (cadena == null)
                    respuesta = "";
                else if (cadena == "")
                    respuesta = "";
                else
                    respuesta = cadena;
            }
            catch
            {
                respuesta = "";
            }
            return respuesta;
        }
        private decimal NullDecimal(string cadena)
        {
            decimal respuesta = 0;
            try
            {
                if (cadena == null)
                    respuesta = 0;
                else if (cadena == "")
                    respuesta = 0;
                else
                    respuesta = Convert.ToDecimal(cadena);
            }
            catch
            {
                respuesta = 0;
            }
            return respuesta;
        }
    }
}