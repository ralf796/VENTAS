using GenesysOracleSV.Clases;
using MoreLinq;
using OfficeOpenXml;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Common;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using System.Web.Razor.Parser.SyntaxTree;
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

        public ActionResult IndexCrearProducto()
        {
            return View();
        }
        // GET: LISTAR PRODUCTOS
        //

        public ActionResult IndexListarProductos()
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
        private List<Inventario_BE> GetInventario_create_(Inventario_BE item)
        {
            List<Inventario_BE> lista = new List<Inventario_BE>();
            lista = Inventario_BLL.GetInventario_create(item);
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

        public JsonResult Guardar(string nombre = "", string descripcion = "", int tipo = 0, int estanteria = 0, int nivel = 0, int anioI = 0, int anioF = 0, int categoria = 0, string telefono = "", string direccion = "", string contacto = "", int marca = 0, int id = 0)
        {
            try
            {
                if (descripcion == "")
                    descripcion = nombre;

                string respuesta = "";
                var item = new Inventario_BE();
                item.ESTANTERIA = estanteria;
                item.NIVEL = nivel;
                item.NOMBRE = nombre;
                item.DESCRIPCION = descripcion;
                item.CREADO_POR = "RALOPEZ";    // Session["usuario"].ToString();
                item.ANIO_INICIAL = anioI;
                item.ANIO_FINAL = anioF;
                item.MTIPO = tipo;
                item.ID_CATEGORIA = categoria;
                item.TELEFONO = telefono;
                item.DIRECCION = direccion;
                item.CONTACTO = contacto;
                item.ID_MARCA_VEHICULO = marca;
                item.ID_UPDATE = id;

                var lista = GetInventario_create_(item);

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

        //public JsonResult OperacionesProducto(string NOMBRE = "", string DESCRIPCION = "", decimal PRECIO_COSTO = 0, decimal PRECIO_VENTA = 0, int STOCK = 0, string CODIGO = "", int ID_MARCA_REPUESTO = 0, int ID_SERIE_VEHICULO = 0, int ID_PRODUCTO = 0, int tipo = 0)
        public JsonResult OperacionesProducto(string NOMBRE = "", string DESCRIPCION = "", string CODIGO = "", string CODIGO2 = "", int STOCK = 0, decimal PRECIO_COSTO = 0, decimal PRECIO_VENTA = 0, int ANIO_INICIAL = 0, int ANIO_FINAL = 0, string PATH = "", string NOMBRE_MARCA_REPUESTO = "", string NOMBRE_MARCA_VEHICULO = "", string NOMBRE_SERIE_VEHICULO = "", string NOMBRE_DISTRIBUIDOR = "", int tipo = 0)
        {
            try
            {
                string respuesta = "";
                var row = new Inventario_BE();
                row.NOMBRE = NOMBRE;
                row.DESCRIPCION = DESCRIPCION;
                row.CODIGO = CODIGO;
                row.CODIGO2 = CODIGO2;
                row.STOCK = STOCK;
                row.PRECIO_COSTO = PRECIO_COSTO;
                row.PRECIO_VENTA = PRECIO_VENTA;
                row.ANIO_INICIAL = ANIO_INICIAL;
                row.ANIO_FINAL = ANIO_FINAL;
                row.PATH_IMAGEN = PATH;
                row.NOMBRE_MARCA_REPUESTO = NOMBRE_MARCA_REPUESTO;
                row.NOMBRE_MARCA_VEHICULO = NOMBRE_MARCA_VEHICULO;
                row.NOMBRE_SERIE_VEHICULO = NOMBRE_SERIE_VEHICULO;
                row.NOMBRE_DISTRIBUIDOR = NOMBRE_DISTRIBUIDOR;
                row.CREADO_POR = "RALOPEZ";//Session["usuario"].ToString();
                row.MTIPO = tipo;

                var lista = GetDatosInventario_(row);
                if (lista.Count > 0)
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

                                row.CREADO_POR = "RALOPEZ";         // Session["usuario"].ToString();

                                row.MTIPO = 5;
                                int existeProducto = GetDatosInventario_(row).FirstOrDefault().STOCK;

                                if (existeProducto == 0)
                                    list.Add(row);
                            }
                        }
                    }
                }

                var groupCodigos1 = list.DistinctBy(i => i.CODIGO).ToList();

                foreach (var row1 in groupCodigos1)
                {
                    row1.MTIPO = 1;
                    var resultHeader = GetDatosInventario_(row1);
                }

                foreach (var dato in list)
                {
                    dato.MTIPO = 4;
                    var resultDetalleProducto = Inventario_BLL.GetSPInventario(dato);
                }
                return Json(new { State = 1, data = list, dataGroup = groupCodigos1.Count() }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { State = -1, Message = ex.Message.ToString() }, JsonRequestBehavior.AllowGet);
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