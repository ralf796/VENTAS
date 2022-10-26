using GenesysOracleSV.Clases;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Common;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Ventas_BE;
using Ventas_BLL;

namespace Ventas.Controllers.Inventario
{
    public class INVMantenimientoController : Controller
    {
        // GET: BODEGA
        //[SessionExpireFilterAttribute]
        public ActionResult IndexBodega()
        {
            return View();
        }
        // GET: CATEGORIA
        public ActionResult IndexCategoria()
        {
            return View();
        }
        // GET: SUBCATEGORIA
        public ActionResult IndexSubcategoria()
        {
            return View();
        }
        // GET: MODELO
        public ActionResult IndexModelo()
        {
            return View();
        }
        // GET: PROVEEDOR
        public ActionResult IndexProveedor()
        {
            return View();
        }
        // GET: MARCA REPUESTO
        public ActionResult IndexMarcaRepuesto()
        {
            return View();
        }
        // GET: MARCA VEHICULO
        public ActionResult IndexMarcaVehiculo()
        {
            return View();
        }
        // GET: SERIE VEHICULO
        public ActionResult IndexSerieVehiculo()
        {
            return View();
        }
        // GET: CREAR PRODUCTO
        public ActionResult IndexCrearProducto()
        {
            return View();
        }
        // GET: LISTAR PRODUCTOS
        public ActionResult IndexListarProductos()
        {
            return View();
        }

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
        public JsonResult Guardar(string nombre = "", string descripcion = "", int tipo = 0, int estanteria = 0, int nivel = 0, int anioI = 0, int anioF = 0, int categoria = 0, string telefono = "", string direccion = "", string contacto = "", int marca = 0)
        {
            try
            {
                string respuesta = "";
                var item = new Inventario_BE();
                item.NOMBRE = nombre;
                item.DESCRIPCION = descripcion;
                item.CREADO_POR = "RALOPEZ";
                item.ANIO_INICIAL = anioI;
                item.ANIO_FINAL = anioF;
                item.MTIPO = tipo;
                item.ID_CATEGORIA = categoria;
                item.TELEFONO = telefono;
                item.DIRECCION = telefono;
                item.CONTACTO = contacto;
                item.ID_MARCA_VEHICULO = marca;

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
        public JsonResult Update(int tipo = 0, int id = 0, string nombre = "", int estanteria = 0, int nivel = 0)
        {
            try
            {
                string respuesta = "";
                var item = new Inventario_BE();
                item.NOMBRE = nombre;
                item.CREADO_POR = "RALOPEZ";
                item.MTIPO = tipo;
                item.ESTANTERIA = estanteria;
                item.NIVEL = nivel;
                item.ID_UPDATE = id;

                var lista = GetInventario_update_(item);

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
            int ID_PROVEEDOR = 0, int ID_MARCA_REPUESTO = 0, int ID_SUBCATEGORIA = 0, int ID_SERIE_VEHICULO = 0, int ID_PRODUCTO=0, int tipo = 0)
        {
            try
            {
                string respuesta = "";
                var item = new Inventario_BE();
                item.NOMBRE = NOMBRE;
                item.DESCRIPCION = DESCRIPCION;
                item.CREADO_POR = "RALOPEZ";
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
    }
}