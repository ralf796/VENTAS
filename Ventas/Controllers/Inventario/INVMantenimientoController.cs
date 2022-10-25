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
        private List<Inventario_BE> GetInventario_delete_(Inventario_BE item)
        {
            List<Inventario_BE> lista = new List<Inventario_BE>();
            lista = Inventario_BLL.GetInventario_delete(item);
            return lista;
        }
        private List<Inventario_BE> GetInventario_create_(Inventario_BE item)
        {
            List<Inventario_BE> lista = new List<Inventario_BE>();
            lista = Inventario_BLL.GetInventario_create(item);
            return lista;
        }

        
        public JsonResult GuardarProducto(int ID_CATEGORIA = 0, int ID_MODELO = 0, int ID_TIPO = 0, int ID_BODEGA = 0, string NOMBRE = "", string DESCRIPCION = "",
            decimal PRECIO_COSTO = 0, decimal PRECIO_VENTA = 0, int STOCK = 0, int ANIO_FABRICADO = 0, string CODIGO = "")
        {
            try
            {
                string respuesta = "";
                var item = new Inventario_BE();
                item.MTIPO = 17;
                item.ID_CATEGORIA = ID_CATEGORIA;
                item.ID_MODELO = ID_MODELO;
                item.ID_TIPO = ID_TIPO;
                item.ID_BODEGA = ID_BODEGA;
                item.NOMBRE = NOMBRE;
                item.DESCRIPCION = DESCRIPCION;
                item.PRECIO_COSTO = PRECIO_COSTO;
                item.PRECIO_VENTA = PRECIO_VENTA;
                item.STOCK = STOCK;
                item.ANIO_FABRICADO = ANIO_FABRICADO;
                item.CODIGO = CODIGO;
                item.CREADO_POR = "RALOPEZ";

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
        public JsonResult Update_Delete(string nombre = "", string descripcion = "", int tipo = 0, int id = 0, int estanteria = 0, int nivel = 0)
        {
            try
            {
                string respuesta = "";
                var item = new Inventario_BE();
                item.NOMBRE = nombre;
                item.DESCRIPCION = descripcion;
                item.MTIPO = tipo;
                item.ESTANTERIA = estanteria;
                item.NIVEL = nivel;

                if (tipo >= 1 && tipo <= 4)
                    item.ID_CATEGORIA = id;
                else if (tipo >= 5 && tipo <= 8)
                    item.ID_MODELO = id;
                else if (tipo >= 9 && tipo <= 12)
                    item.ID_TIPO = id;
                else if (tipo >= 13 && tipo <= 16)
                    item.ID_BODEGA = id;

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
        public JsonResult Update_Delete_Producto(int id = 0, int tipo = 0, int ID_CATEGORIA = 0, int ID_MODELO = 0, int ID_TIPO = 0, int ID_BODEGA = 0, string NOMBRE = "", string DESCRIPCION = "", decimal PRECIO_COSTO = 0, decimal PRECIO_VENTA = 0, int STOCK = 0, int ANIO_FABRICADO = 0, string CODIGO = "")
        {
            try
            {
                string respuesta = "";
                var item = new Inventario_BE();
                item.ID_PRODUCTO = id;
                item.MTIPO = tipo;
                item.ID_CATEGORIA = ID_CATEGORIA;
                item.ID_MODELO = ID_MODELO;
                item.ID_TIPO = ID_TIPO;
                item.ID_BODEGA = ID_BODEGA; ;
                item.NOMBRE = NOMBRE;
                item.DESCRIPCION = DESCRIPCION;
                item.PRECIO_COSTO = PRECIO_COSTO;
                item.PRECIO_VENTA = PRECIO_VENTA;
                item.STOCK = STOCK;
                item.ANIO_FABRICADO = ANIO_FABRICADO;
                item.CODIGO = CODIGO;
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
        public JsonResult GetDatosTable(int tipo = 0)
        {
            try
            {
                var item = new Inventario_BE();
                item.MTIPO = tipo;
                var lista = GetInventario_select_(item);
                return Json(new { State = 1, data = lista }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { State = -1, Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }


        public JsonResult Delete(int id = 0, int tipo=0)
        {
            try
            {
                string respuesta = "";
                var item = new Inventario_BE();
                item.ID_DELETE = id;
                item.MTIPO = tipo;
                var lista = GetInventario_delete_(item);
                if (lista !=null)
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

        public JsonResult Guardar(string nombre = "", string descripcion = "", int tipo = 0, int estanteria = 0, int nivel = 0)
        {
            try
            {
                string respuesta = "";
                var item = new Inventario_BE();
                item.NOMBRE = nombre;
                item.DESCRIPCION = descripcion;
                item.CREADO_POR = "RALOPEZ";
                item.MTIPO = tipo;
                item.ESTANTERIA = estanteria;
                item.NIVEL = nivel;

                var lista = GetInventario_create_(item);

                if (lista!=null)
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