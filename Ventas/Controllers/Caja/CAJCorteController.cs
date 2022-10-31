using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Ventas_BE;
using Ventas_BLL;
namespace Ventas.Controllers.Caja
{
    public class CAJCorteController : Controller
    {
        // GET: CAJCorte
        public ActionResult Index()
        {
            return View();
        }
        private List<Caja_BE> GetDatosCaja_(Caja_BE item)
        {
            List<Caja_BE> lista = new List<Caja_BE>();
            lista = Caja_BLL.GetSPCaja(item);
            return lista;
        }
        public JsonResult GetCorte()
        {
            try
            {
                var item = new Caja_BE();
                item.MTIPO = 4;
                var lista = GetDatosCaja_(item);

                foreach (var row in lista)
                    row.FECHA_CREACION_STRING = row.FECHA_CREACION.ToString("dd/MM/yyyy");

                return Json(new { State = 1, data = lista }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { State = -1, Message = ex.Message }, JsonRequestBehavior.AllowGet);

            }
        }
    }
}