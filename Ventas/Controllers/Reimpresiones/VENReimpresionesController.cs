using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Ventas_BE;
using Ventas_BLL;

namespace Ventas.Controllers.Ventas
{
    public class VENReimpresionesController : Controller
    {
        // GET: VENReimpresiones
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
        public JsonResult GetDatos(string fecha= "")
        {
            try
            {
                var item = new Reportes_BE();
                item.MTIPO = 17;
                item.FECHA_INICIAL = Convert.ToDateTime(fecha);
                item.FECHA_FINAL = Convert.ToDateTime(fecha);
                var lista = GetSPReportes_(item);

                foreach(var row in lista)
                {
                    row.FECHA_CREACION_STRING = row.FECHA_VENTA.ToString("dd/MM/yyyy hh:mm tt");
                }

                return Json(new { State = 1, data = lista }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { State = -1, Message = ex.Message }, JsonRequestBehavior.AllowGet);

            }
        }
    }
}