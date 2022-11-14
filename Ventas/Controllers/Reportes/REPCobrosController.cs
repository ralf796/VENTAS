using GenesysOracleSV.Clases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Ventas_BE;
using Ventas_BLL;
namespace Ventas.Controllers.Reportes
{
    public class REPCobrosController : Controller
    {
        // GET: REPCobros
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

        public JsonResult getCobro(string fechaInicial = "", string fechaFinal = "")
        {
            try
            {
                var item = new Reportes_BE();
                item.MTIPO = 4;
                item.FECHA_INICIAL = Convert.ToDateTime(fechaInicial);
                item.FECHA_FINAL = Convert.ToDateTime(fechaFinal);
                var lista = GetSPReportes_(item);
                foreach (var row in lista)
                    row.FECHA_CREACION_STRING = row.FECHA_CREACION.ToString("dd/MM/yyyy hh:mm tt");
                return Json(new { State = 1, data = lista }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { State = -1, Message = ex.Message }, JsonRequestBehavior.AllowGet);

            }
        }

        // funcion que devuelve el total de venta segun rango de fechas
        public JsonResult getTotalMonto(string fechaInicial = "", string fechaFinal = "")
        {
            try
            {
                var item = new Reportes_BE();
                item.MTIPO = 5;
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
        // funcion total tarjeta-----------------------------------------------------
        public JsonResult getTotalEfectivo(string fechaInicial = "", string fechaFinal = "")
        {
            try
            {
                var item = new Reportes_BE();
                item.MTIPO = 8;
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
        // -------------------------------total tarjeta---------------------------------
        public JsonResult getTotalTarjeta(string fechaInicial = "", string fechaFinal = "")
        {
            try
            {
                var item = new Reportes_BE();
                item.MTIPO = 9;
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
    }
}