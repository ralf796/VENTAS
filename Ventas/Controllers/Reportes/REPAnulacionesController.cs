using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Ventas_BE;
using Ventas_BLL;

namespace Ventas.Controllers.Reportes
{
    public class REPAnulacionesController : Controller
    {
        // GET: REPAnulaciones
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

        public JsonResult GetDatos(string fechaInicial = "", string fechaFinal = "")
        {
            try
            {
                var item = new Reportes_BE();
                item.MTIPO = 26;
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
    }
}