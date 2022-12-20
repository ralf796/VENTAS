using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Ventas.Class;
using Ventas_BE;
using Ventas_BLL;

namespace Ventas.Controllers.Reportes
{
    public class REPBitacoraController : Controller
    {
        // GET: REPBitacora
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
        public JsonResult GetDatosSP(string fechaInicial = "", string fechaFinal = "")
        {
            try
            {
                var item = new Reportes_BE();
                item.MTIPO = 13;
                item.FECHA_INICIAL = Convert.ToDateTime(fechaInicial);
                item.FECHA_FINAL = DateTime.Now;
                var lista = GetSPReportes_(item);
                foreach (var row in lista)
                {
                    row.FECHA_CREACION_STRING = row.FECHA_CREACION.ToString("dd/MM/yyyy hh:mm tt");

                    if (row.TIPO_REPORTE != "BITACORA PRODUCTOS")
                        row.FECHA_VENTA_STRING = row.FECHA_VENTA.ToString("dd/MM/yyyy hh:mm tt");
                }
                return Json(new { State = 1, data = lista }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { State = -1, Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult GetDefault()
        {
            try
            {
                var item = new Reportes_BE();
                item.MTIPO = 22;
                item.FECHA_INICIAL = DateTime.Now;
                item.FECHA_FINAL = DateTime.Now;
                var lista= GetSPReportes_(item);
                item.MTIPO = 23;
                var listaVentas = GetSPReportes_(item);
                item.MTIPO = 24;
                var listaUsers = GetSPReportes_(item);
                return Json(new { State = 1, data = lista, ventasDia= listaVentas.Count(), usuarios= listaUsers.Count() }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { State = -1, Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}