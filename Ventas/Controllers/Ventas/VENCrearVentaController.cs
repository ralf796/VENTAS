using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using Ventas_BE;
using Ventas_BLL;

namespace Ventas.Controllers.Ventas
{
    public class VENCrearVentaController : Controller
    {
        // GET: VENCrearVenta
        public ActionResult Index()
        {
            return View();
        }

        private List<Ventas__BE> GetDatosSP_(Ventas__BE item)
        {
            List<Ventas__BE> lista = new List<Ventas__BE>();
            lista = Ventas__BLL.GetDatosSP(item);
            return lista;
        }

        public JsonResult GetCliente(int tipo = 0, string nit = "")
        {
            try
            {
                var item = new Ventas__BE();
                item.MTIPO = tipo;
                item.NIT = nit.Trim();
                item = GetDatosSP_(item).FirstOrDefault();
                return Json(new { State = 1, data = item }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { State = -1, Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult GetList(int tipo = 0, int ID_MARCA_REPUESTO = 0, int ID_SUBCATEGORIA = 0, int ID_CATEGORIA = 0, int ID_SERIE_VEHICULO = 0, int ID_MARCA_VEHICULO = 0, int ID_MODELO = 0)
        {
            try
            {
                var item = new Ventas__BE();
                item.MTIPO = tipo;
                if (ID_MARCA_REPUESTO != 0)
                    item.ID_MARCA_REPUESTO = ID_MARCA_REPUESTO;
                if (ID_SUBCATEGORIA != 0)
                    item.ID_SUBCATEGORIA = ID_SUBCATEGORIA;
                if (ID_CATEGORIA != 0)
                    item.ID_CATEGORIA = ID_CATEGORIA;
                if (ID_SERIE_VEHICULO != 0)
                    item.ID_SERIE_VEHICULO = ID_SERIE_VEHICULO;
                if (ID_MARCA_VEHICULO != 0)
                    item.ID_MARCA_VEHICULO = ID_MARCA_VEHICULO;
                if (ID_MODELO != 0)
                    item.ID_MODELO = ID_MODELO;
                var lista = GetDatosSP_(item);
                return Json(new { State = 1, data = lista }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { State = -1, Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        /*         
            insert into TBL_VENTA(ID_VENTA, serie, CORRELATIVO, FECHA, ID_CLIENTE, TOTAL, SUBTOTAL, TOTAL_IVA, TOTAL_DESCUENTO, CREADO_POR, ESTADO, FECHA_CREACION, ID_ESTADO_VENTA)
            values (@ID_VENTA, @SERIE, @CORRELATIVO, GETDATE, @ID_CLIENTE, @TOTAL, @SUBTOTAL, 0, @TOTAL_DESCUENTO, @CREADO_POR, 1, GETDATE(), 1)
         */


        public JsonResult SaveOrder(int tipo = 0, int ID_CLIENTE = 0, decimal TOTAL = 0, decimal SUBTOTAL = 0, decimal TOTAL_DESCUENTO = 0)
        {
            try
            {
                var item = new Ventas__BE();
                item.MTIPO = tipo;
                item.ID_VENTA = 2;
                item.SERIE = "SSS";
                item.CORRELATIVO = 1;
                item.ID_CLIENTE = ID_CLIENTE;
                item.TOTAL = TOTAL;
                item.SUBTOTAL = SUBTOTAL;
                item.TOTAL_DESCUENTO = TOTAL_DESCUENTO;
                item.CREADO_POR = "RALOPEZ";
                var lista = GetDatosSP_(item);
                return Json(new { State = 1, data = lista }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { State = -1, Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}