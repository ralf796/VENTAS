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
    public class INVMantenimientoController : Controller
    {
        #region VIEWS
        // GET: BODEGA

        public ActionResult IndexBodega()
        {
            return View();
        }
        // GET: CATEGORIA
        //

        public ActionResult IndexCategoria()
        {
            return View();
        }
        // GET: SUBCATEGORIA
        //

        public ActionResult IndexSubcategoria()
        {
            return View();
        }
        // GET: MODELO
        //

        public ActionResult IndexModelo()
        {
            return View();
        }
        // GET: PROVEEDOR
        //

        public ActionResult IndexProveedor()
        {
            return View();
        }
        // GET: MARCA REPUESTO
        //

        public ActionResult IndexMarcaRepuesto()
        {
            return View();
        }
        // GET: MARCA VEHICULO
        //

        public ActionResult IndexMarcaVehiculo()
        {
            return View();
        }
        // GET: SERIE VEHICULO
        //

        public ActionResult IndexSerieVehiculo()
        {
            return View();
        }
        // GET: CREAR PRODUCTO
        //

        [SessionExpireFilter]
        public ActionResult IndexCrearProducto()
        {
            return View();
        }
        // GET: LISTAR PRODUCTOS
        //

        [SessionExpireFilter]
        public ActionResult IndexListarProductos()
        {
            return View();
        }

        public ActionResult IndexEditarProducto()
        {
            return View();
        }
        #endregion

        #region BD
        private List<Inventario_BE> GetDatosInventario_(Inventario_BE item)
        {
            List<Inventario_BE> lista = new List<Inventario_BE>();
            lista = Inventario_BLL.GetSPInventario(item);
            return lista;
        }
        private List<Inventario_BE> GetInventario_select_(Inventario_BE item)
        {
            List<Inventario_BE> lista = new List<Inventario_BE>();
            lista = Inventario_BLL.GetInventario_select(item);
            return lista;
        }
        private List<Inventario_BE> GetInventario_delete_(Inventario_BE item)
        {
            List<Inventario_BE> lista = new List<Inventario_BE>();
            lista = Inventario_BLL.GetInventario_delete(item);
            return lista;
        }
        #endregion

        #region JSON_RESULTS

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
        public JsonResult GetProductosTable(string filtro = "", int anioI = 0, int anioF = 0)
        {
            try
            {
                var item = new Inventario_BE();
                item.MTIPO = 24;
                item.ID_UPDATE = 0;
                item.NOMBRE_MODELO = filtro;
                item.ANIO_INICIAL = anioI;
                item.ANIO_FINAL = anioF;
                var lista = GetInventario_select_(item);
                return Json(new { State = 1, data = lista }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { State = -1, Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult Delete(int id = 0, int tipo = 0)
        {
            try
            {
                string respuesta = "";
                var item = new Inventario_BE();
                item.ID_DELETE = id;
                item.MTIPO = tipo;
                var lista = GetInventario_delete_(item);
                if (lista != null)
                {
                    if (lista.FirstOrDefault().RESPUESTA != "")
                    {
                        respuesta = lista.FirstOrDefault().RESPUESTA;
                    }
                }
                return Json(new { State = 1 }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { State = -1, Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult OperacionesProducto(int ID_PRODUCTO = 0, string NOMBRE = "", int STOCK = 0, decimal PRECIO_COSTO = 0, decimal PRECIO_VENTA = 0, string PATH = "", int tipo = 0, string DESCRIPCION = "",
            string CODIGO = "", string CODIGO2 = "", string NOMBRE_MARCA_REPUESTO = "", string NOMBRE_MARCA_VEHICULO = "", string NOMBRE_SERIE_VEHICULO = "", string NOMBRE_DISTRIBUIDOR = "", int ANIO_INICIAL = 0, int ANIO_FINAL = 0)
        {
            try
            {
                PATH = PATH.Trim();
                string respuesta = "";
                var row = new Inventario_BE();
                row.ID_PRODUCTO = ID_PRODUCTO;
                row.NOMBRE = NOMBRE;
                row.DESCRIPCION = DESCRIPCION;
                row.STOCK = STOCK;
                row.PRECIO_COSTO = PRECIO_COSTO;
                row.PRECIO_VENTA = PRECIO_VENTA;
                row.CODIGO = CODIGO;
                row.CODIGO2 = CODIGO2;
                row.NOMBRE_MARCA_REPUESTO = NOMBRE_MARCA_REPUESTO;
                row.NOMBRE_MARCA_VEHICULO = NOMBRE_MARCA_VEHICULO;
                row.NOMBRE_SERIE_VEHICULO = NOMBRE_SERIE_VEHICULO;
                row.NOMBRE_DISTRIBUIDOR = NOMBRE_DISTRIBUIDOR;
                row.ANIO_INICIAL = ANIO_INICIAL;
                row.ANIO_FINAL= ANIO_FINAL;


                if (PATH != "" && PATH != null)
                    row.PATH_IMAGEN = PATH;

                row.CREADO_POR = Session["usuario"].ToString();//Session["usuario"].ToString();
                row.MTIPO = tipo;

                var lista = GetDatosInventario_(row);
                if (lista.Count > 0)
                {
                    if (lista.FirstOrDefault().RESPUESTA != "")
                    {
                        respuesta = lista.FirstOrDefault().RESPUESTA;


                        row.MTIPO = 4;
                        var resultDetalleProducto = Inventario_BLL.GetSPInventario(row);
                    }
                }
                return Json(new { State = 1 }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { State = -1, Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
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

                        for (int rowIterator = 3; rowIterator <= noOfRow; rowIterator++)
                        {
                            if (workSheet.Cells[rowIterator, 3].Value != null)
                            {
                                for (int i = 1; i <= 14; i++)
                                {
                                    if (workSheet.Cells[rowIterator, i].Value == null)
                                        workSheet.Cells[rowIterator, i].Value = "";
                                }



                                var row = new Inventario_BE();
                                row.NOMBRE = NullString(workSheet.Cells[rowIterator, 1].Value.ToString());
                                row.DESCRIPCION = NullString(workSheet.Cells[rowIterator, 2].Value.ToString());
                                row.CODIGO = NullString(workSheet.Cells[rowIterator, 3].Value.ToString());
                                row.CODIGO2 = NullString(workSheet.Cells[rowIterator, 4].Value.ToString());
                                row.STOCK = NullInt(workSheet.Cells[rowIterator, 5].Value.ToString());
                                row.PRECIO_COSTO = NullDecimal(workSheet.Cells[rowIterator, 6].Value.ToString());
                                row.PRECIO_VENTA = NullDecimal(workSheet.Cells[rowIterator, 7].Value.ToString());
                                row.ANIO_INICIAL = NullInt(workSheet.Cells[rowIterator, 8].Value.ToString());
                                row.ANIO_FINAL = NullInt(workSheet.Cells[rowIterator, 9].Value.ToString());
                                row.PATH_IMAGEN = NullString(workSheet.Cells[rowIterator, 10].Value.ToString());
                                row.NOMBRE_MARCA_REPUESTO = NullString(workSheet.Cells[rowIterator, 11].Value.ToString());
                                row.NOMBRE_MARCA_VEHICULO = NullString(workSheet.Cells[rowIterator, 12].Value.ToString());
                                row.NOMBRE_SERIE_VEHICULO = NullString(workSheet.Cells[rowIterator, 13].Value.ToString());
                                row.NOMBRE_DISTRIBUIDOR = NullString(workSheet.Cells[rowIterator, 14].Value.ToString());
                                row.CREADO_POR = Session["usuario"].ToString();         // Session["usuario"].ToString();
                                /*
                                row.MTIPO = 5;
                                int existeProducto = GetDatosInventario_(row).FirstOrDefault().STOCK;
                                if (existeProducto == 0)
                                */
                                list.Add(row);

                            }
                        }
                    }
                }

                //var groupCodigos1 = list.DistinctBy(i => i.CODIGO).ToList();
                /*
                var groupList = from r in list
                              group r by new { r.CODIGO, r.CODIGO2, r.NOMBRE_DISTRIBUIDOR, r.NOMBRE_MARCA_REPUESTO} into gp
                              select new
                              {
                                  CODIGO = gp.Key.CODIGO,
                                  CODIGO2 = gp.Key.CODIGO2,
                                  NOMBRE_DISTRIBUIDOR = gp.Key.NOMBRE_DISTRIBUIDOR,
                                  NOMBRE_MARCA_REPUESTO = gp.Key.NOMBRE_MARCA_REPUESTO
                              };
                */
                var groupCodigos1 = list.DistinctBy(p => new { p.CODIGO, p.CODIGO2, p.NOMBRE_DISTRIBUIDOR, p.NOMBRE_MARCA_REPUESTO });


                foreach (var row1 in groupCodigos1)
                {
                    string ss = "";
                    if (row1.CODIGO == "3P-3620")
                        ss = "";

                    row1.MTIPO = 1;
                    var resultHeader = GetDatosInventario_(row1);
                    /*
                    if (resultHeader != null)
                    {
                        if (resultHeader.FirstOrDefault().RESPUESTA == "02")
                            list.RemoveAll(x => x.CODIGO == row1.CODIGO && x.CODIGO2 == row1.CODIGO2 && x.NOMBRE_DISTRIBUIDOR == row1.NOMBRE_DISTRIBUIDOR && x.NOMBRE_MARCA_REPUESTO == row1.NOMBRE_MARCA_REPUESTO);
                    }
                    */
                }

                foreach (var dato in list)
                {
                    dato.MTIPO = 4;
                    var resultDetalleProducto = Inventario_BLL.GetSPInventario(dato);
                }
                return Json(new { State = 1, data = list, dataGroup = groupCodigos1.Count(), dataGroupDetail = list.Count() }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { State = -1, Message = ex.Message.ToString() }, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult GetProductoEditar(string codigo = "")
        {
            try
            {
                var item = new Inventario_BE();
                item.MTIPO = 25;
                item.ID_UPDATE = 0;
                item.NOMBRE_MODELO = codigo;
                var lista = GetInventario_select_(item).FirstOrDefault();
                return Json(new { State = 1, data = lista }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { State = -1, Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult GetDetallesProductoEditar(string codigo = "")
        {
            try
            {
                var item = new Inventario_BE();
                item.MTIPO = 26;
                item.ID_UPDATE = 0;
                item.NOMBRE_MODELO = codigo;
                var lista = GetInventario_select_(item);
                return Json(new { State = 1, data = lista }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { State = -1, Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult EliminarDetalleProducto(int ID_DETALLE = 0)
        {
            try
            {
                var item = new Inventario_BE();
                item.MTIPO = 27;
                item.ID_UPDATE = ID_DETALLE;
                item.NOMBRE_MODELO = "";
                var lista = GetInventario_select_(item);
                return Json(new { State = 1, data = lista }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { State = -1, Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult AgregarDetalleProducto(int anioI = 0, int anioF = 0, string marcaV = "", string linea = "", string codigo1 = "", string codigo2 = "")
        {
            try
            {
                var item = new Inventario_BE();
                item.MTIPO = 4;
                item.ANIO_INICIAL = anioI;
                item.ANIO_FINAL = anioF;
                item.NOMBRE_MARCA_VEHICULO = marcaV;
                item.NOMBRE_SERIE_VEHICULO = linea;
                item.CODIGO = codigo1;
                item.CODIGO2 = codigo2;
                var resultDetalleProducto = Inventario_BLL.GetSPInventario(item);
                return Json(new { State = 1 }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { State = -1, Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult CambiarURLImagenProducto(int id_producto = 0, string url = "")
        {
            try
            {
                var item = new Inventario_BE();
                item.MTIPO = 28;
                item.ID_UPDATE = id_producto;
                item.NOMBRE_MODELO = url;
                var lista = GetInventario_select_(item);
                return Json(new { State = 1 }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { State = -1, Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult CambiarImagenProducto(FormCollection formCollection)
        {
            try
            {
                int estado = 1;
                string path = "", url = "";
                int id_producto = Convert.ToInt32(Request.Form["ID_PRODUCTO"]);
                HttpPostedFileBase file = Request.Files["FileUpload"];
                var randomNumber = new Random().Next(0, 100);

                if ((file != null) && (file.ContentLength > 0) && !string.IsNullOrEmpty(file.FileName))
                {
                    string fileName = file.FileName;
                    string[] separarNombre = fileName.Split('.');
                    string extension = separarNombre[1];
                    Bitmap filee = new Bitmap(file.InputStream);
                    string directory = Server.MapPath(@"~\Varios\ProductosIMG");
                    if (!Directory.Exists(fileName))
                    {
                        Directory.CreateDirectory(directory);
                    }

                    path = Server.MapPath(@"~\Varios\ProductosIMG\" + separarNombre[0] + randomNumber + ".png");
                    filee.Save(path);

                    var urlBuilder = new System.UriBuilder(Request.Url.AbsoluteUri) { Path = Url.Content(@"~\Varios\ProductosIMG\" + separarNombre[0] + randomNumber + ".png"), Query = null, };
                    Uri uri = urlBuilder.Uri;
                    url = urlBuilder.ToString();
                }

                var item = new Inventario_BE();
                item.MTIPO = 28;
                item.ID_UPDATE = id_producto;
                item.NOMBRE_MODELO = url;
                var lista = GetInventario_select_(item);

                return Json(new { State = estado }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { State = -1, Message = ex.Message.ToString() }, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult ActualizarEncabezadoProducto(int ID_PRODUCTO = 0, string NOMBRE = "", string DESCRIPCION = "", string MARCA_REPUESTO = "", int STOCK = 0, decimal PRECIO_COSTO = 0, decimal PRECIO_VENTA = 0, string CODIGO = "", string CODIGO2 = "", string DISTRIBUIDOR= "")
        {
            try
            {
                var row = new Inventario_BE();
                row.ID_PRODUCTO = ID_PRODUCTO;
                row.CODIGO = CODIGO;
                row.CODIGO2 = CODIGO2;
                row.NOMBRE = NOMBRE;
                row.DESCRIPCION = DESCRIPCION;
                row.STOCK = STOCK;
                row.PRECIO_COSTO = PRECIO_COSTO;
                row.PRECIO_VENTA = PRECIO_VENTA;
                row.NOMBRE_MARCA_REPUESTO = MARCA_REPUESTO;
                row.NOMBRE_DISTRIBUIDOR = DISTRIBUIDOR;
                row.MTIPO = 9;

                var lista = GetDatosInventario_(row);
                return Json(new { State = 1 }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { State = -1, Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

        #region FUNCTIONS
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
        #endregion
    }
}