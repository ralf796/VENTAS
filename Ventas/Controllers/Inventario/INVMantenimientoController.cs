using GenesysOracleSV.Clases;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Common;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using Ventas_BE;
using Ventas_BLL;

namespace Ventas.Controllers.Inventario
{
    public class INVMantenimientoController : Controller
    {
        #region VIEWS
        // GET: BODEGA
        //[SessionExpireFilterAttribute]
        public ActionResult IndexBodega()
        {
            return View();
        }
        // GET: CATEGORIA
        //[SessionExpireFilterAttribute]
        public ActionResult IndexCategoria()
        {
            return View();
        }
        // GET: SUBCATEGORIA
        //[SessionExpireFilterAttribute]
        public ActionResult IndexSubcategoria()
        {
            return View();
        }
        // GET: MODELO
        //[SessionExpireFilterAttribute]
        public ActionResult IndexModelo()
        {
            return View();
        }
        // GET: PROVEEDOR
        //[SessionExpireFilterAttribute]
        public ActionResult IndexProveedor()
        {
            return View();
        }
        // GET: MARCA REPUESTO
        //[SessionExpireFilterAttribute]
        public ActionResult IndexMarcaRepuesto()
        {
            return View();
        }
        // GET: MARCA VEHICULO
        //[SessionExpireFilterAttribute]
        public ActionResult IndexMarcaVehiculo()
        {
            return View();
        }
        // GET: SERIE VEHICULO
        //[SessionExpireFilterAttribute]
        public ActionResult IndexSerieVehiculo()
        {
            return View();
        }
        // GET: CREAR PRODUCTO
        //[SessionExpireFilterAttribute]
        public ActionResult IndexCrearProducto()
        {
            return View();
        }
        // GET: LISTAR PRODUCTOS
        //[SessionExpireFilterAttribute]
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
        private List<Inventario_BE> GetInventario_update_(Inventario_BE item)
        {
            List<Inventario_BE> lista = new List<Inventario_BE>();
            lista = Inventario_BLL.GetInventario_update(item);
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
        public JsonResult GetDatosTable(int tipo = 0, int id = 0)
        {
            try
            {
                var item = new Inventario_BE();
                item.MTIPO = tipo;
                item.ID_UPDATE = id;
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
                string respuesta = "";
                var item = new Inventario_BE();
                item.ESTANTERIA = estanteria;
                item.NIVEL = nivel;
                item.NOMBRE = nombre;
                item.DESCRIPCION = descripcion;
                item.CREADO_POR = Session["usuario"].ToString();
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
        public JsonResult OperacionesProducto(string NOMBRE = "", string DESCRIPCION = "", decimal PRECIO_COSTO = 0, decimal PRECIO_VENTA = 0, int STOCK = 0, string CODIGO = "", int ID_BODEGA = 0, int ID_MODELO = 0,
            int ID_PROVEEDOR = 0, int ID_MARCA_REPUESTO = 0, int ID_SUBCATEGORIA = 0, int ID_SERIE_VEHICULO = 0, int ID_PRODUCTO = 0, int tipo = 0)
        {
            try
            {
                string respuesta = "";
                var item = new Inventario_BE();
                item.NOMBRE = NOMBRE;
                item.DESCRIPCION = DESCRIPCION;
                item.CREADO_POR = Session["usuario"].ToString();
                item.ID_PRODUCTO = ID_PRODUCTO;
                item.ID_MODELO = ID_MODELO;
                item.ID_BODEGA = ID_BODEGA;
                item.ID_SUBCATEGORIA = ID_SUBCATEGORIA;
                item.ID_PROVEEDOR = ID_PROVEEDOR;
                item.ID_MARCA_REPUESTO = ID_MARCA_REPUESTO;
                item.ID_SERIE_VEHICULO = ID_SERIE_VEHICULO;
                item.PRECIO_COSTO = PRECIO_COSTO;
                item.PRECIO_VENTA = PRECIO_VENTA;
                item.STOCK = STOCK;
                item.CODIGO = CODIGO;
                item.MTIPO = tipo;

                var lista = GetDatosInventario_(item);
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

                        for (int rowIterator = 2; rowIterator <= noOfRow; rowIterator++)
                        {
                            if (workSheet.Cells[rowIterator, 1].Value != null)
                            {
                                for(int i = 1; i <= 12; i++)
                                {
                                    if (workSheet.Cells[rowIterator, i].Value == null)
                                        workSheet.Cells[rowIterator, i].Value = "";
                                }

                                var row = new Inventario_BE();
                                row.NOMBRE = NullString(workSheet.Cells[rowIterator, 1].Value.ToString());
                                row.DESCRIPCION = NullString(workSheet.Cells[rowIterator, 2].Value.ToString());
                                row.CODIGO = NullString(workSheet.Cells[rowIterator, 3].Value.ToString());
                                row.STOCK = NullInt(workSheet.Cells[rowIterator, 4].Value.ToString());
                                row.PRECIO_COSTO = NullDecimal(workSheet.Cells[rowIterator, 5].Value.ToString());
                                row.PRECIO_VENTA = NullDecimal(workSheet.Cells[rowIterator, 6].Value.ToString());
                                row.ID_BODEGA = NullInt(workSheet.Cells[rowIterator, 7].Value.ToString());
                                row.ID_MODELO = NullInt(workSheet.Cells[rowIterator, 8].Value.ToString());
                                row.ID_PROVEEDOR = NullInt(workSheet.Cells[rowIterator, 9].Value.ToString());
                                row.ID_MARCA_REPUESTO = NullInt(workSheet.Cells[rowIterator, 10].Value.ToString());
                                row.ID_SUBCATEGORIA = NullInt(workSheet.Cells[rowIterator, 11].Value.ToString());
                                row.ID_SERIE_VEHICULO = NullInt(workSheet.Cells[rowIterator, 12].Value.ToString());
                                row.CREADO_POR = Session["usuario"].ToString();
                                row.MTIPO = 1;

                                //row.ID_PRODUCTO = workSheet.Cells[rowIterator, 1].Value.ToString();
                                var lista = GetDatosInventario_(row);
                                list.Add(row);
                            }
                        }
                    }
                }

                foreach (var dato in list)
                {
                    var lista = GetDatosInventario_(dato);
                }
                return Json(new { State = 1, data = list }, JsonRequestBehavior.AllowGet);
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