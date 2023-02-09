using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Ventas_BE;
using Ventas_BLL;

namespace Ventas.Controllers.Compras
{
    public class COMRegistrarCompraController : Controller
    {
        // GET: CONRegistrarCompra
        public ActionResult Index()
        {
            return View();
        }

        private List<Inventario_BE> GetSPCompras_(Inventario_BE item)
        {
            List<Inventario_BE> lista = new List<Inventario_BE>();
            lista = Inventario_BLL.GetSPCompras(item);
            return lista;
        }
        public JsonResult Guardar(FormCollection formCollection)
        {
            var respuesta = new Respuesta();
            try
            {
                string nombreProveedor = Request.Form["nombreProveedor"].ToString();
                string contactoProveedor = Request.Form["contactoProveedor"].ToString();
                string fechaPedido = Request.Form["fechaPedido"].ToString();
                string fechaPago = Request.Form["fechaPago"].ToString();
                string fechaEntrega = Request.Form["fechaEntrega"].ToString();
                string telefono = Request.Form["telefono"].ToString();                
                decimal monto = Convert.ToDecimal(Request.Form["monto"]);
                string serie= Request.Form["serie"].ToString();
                string numero= Request.Form["numero"].ToString();
                string pathExcel = "";
                string pathAdjunto = "";

                var randomNumber = new Random().Next(0, 10);
                HttpPostedFileBase file = Request.Files["excel"];
                if ((file != null) && (file.ContentLength > 0) && !string.IsNullOrEmpty(file.FileName))
                {
                    string fileName = file.FileName;
                    string[] separarNombre = fileName.Split('.');
                    string extension = separarNombre[1];
                    //Bitmap filee = new Bitmap(file.InputStream);

                    MemoryStream target = new MemoryStream();
                    file.InputStream.CopyTo(target);
                    byte[] data = target.ToArray();

                    string directory = Server.MapPath(@"~\Content\Compras\Adjuntos");
                    if (!Directory.Exists(fileName))
                        Directory.CreateDirectory(directory);

                    pathExcel = Server.MapPath(@"~\Content\Compras\Adjuntos\" + randomNumber + fileName);
                    file.SaveAs(pathExcel);


                    var urlBuilder1 = new System.UriBuilder(Request.Url.AbsoluteUri) { Path = Url.Content(@"~\Content\Compras\Adjuntos\" + randomNumber + fileName), Query = null, };
                    Uri uri1 = urlBuilder1.Uri;
                    pathExcel = urlBuilder1.ToString();
                }
                HttpPostedFileBase fileAdjunto = Request.Files["adjunto"];
                if ((fileAdjunto != null) && (fileAdjunto.ContentLength > 0) && !string.IsNullOrEmpty(fileAdjunto.FileName))
                {
                    string fileName = fileAdjunto.FileName;
                    string[] separarNombre = fileName.Split('.');
                    string extension = separarNombre[1];                   

                    string directory = Server.MapPath(@"~\Content\Compras\Adjuntos");
                    if (!Directory.Exists(fileName))
                        Directory.CreateDirectory(directory);

                    pathAdjunto = Server.MapPath(@"~\Content\Compras\Adjuntos\" + randomNumber + fileName);
                    fileAdjunto.SaveAs(pathAdjunto);

                    var urlBuilder1 = new System.UriBuilder(Request.Url.AbsoluteUri) { Path = Url.Content(@"~\Content\Compras\Adjuntos\" + randomNumber + fileName), Query = null, };
                    Uri uri1 = urlBuilder1.Uri;
                    pathAdjunto = urlBuilder1.ToString();
                }

                
                var item = new Inventario_BE();
                item.NOMBRE_PROVEEDOR = nombreProveedor;
                item.CONTACTO_PROVEEDOR= contactoProveedor;
                item.FECHA_PEDIDO= Convert.ToDateTime(fechaPedido);
                item.FECHA_PAGO= Convert.ToDateTime(fechaPago);
                item.FECHA_ENTREGA= Convert.ToDateTime(fechaEntrega);
                item.TELEFONO= telefono;
                item.NO_FACTURA = numero;
                item.MONTO_FACTURA= monto;
                item.SERIE_FACTURA= serie;
                item.CREADO_POR = Session["usuario"].ToString();                
                item.FILE1= pathExcel;
                item.FILE2= pathAdjunto;
                item.MTIPO = 1;
                var lista = GetSPCompras_(item);

                return Json(new { State = 1 }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                return Json(new { State = -1, Message = ex.Message }, JsonRequestBehavior.AllowGet);
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
        private int NullInt(string cadena)
        {
            int respuesta = 0;
            try
            {
                if (cadena == null)
                    respuesta = 0;
                else if (cadena == "")
                    respuesta = 0;
                else
                    respuesta = Convert.ToInt16(cadena);
            }
            catch
            {
                respuesta = 0;
            }
            return respuesta;
        }
    }
}