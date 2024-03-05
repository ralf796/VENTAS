using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Ventas.Class;
using Ventas_BE;
using Ventas_BLL;
namespace Ventas.Controllers.Reportes
{
    public class REPProductosController : Controller
    {
        // GET: REPProductos
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
                item.MTIPO = 3;
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


        public ActionResult IndexHistorialProductos()
        {
            return View();
        }
        public JsonResult HistorialProductos(string fechaInicial = "", string fechaFinal = "")
        {
            try
            {
                var item = new Reportes_BE();
                item.MTIPO = 3;
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