using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Ventas_BE;
using Ventas_BLL;

namespace Ventas.Controllers.Devoluciones
{
    public class DEVCrearDevolucionController : Controller
    {
        // GET: DEVCrearDevolucion
        public ActionResult Index()
        {
            return View();
        }

        private static List<Devoluciones_BE> GetDatosSP_(Devoluciones_BE item)
        {
            List<Devoluciones_BE> lista = new List<Devoluciones_BE>();
            lista = Devoluciones_BLL.GetDatosSP(item);
            return lista;
        }

        public JsonResult GetVenta(int ID_VENTA = 0)
        {
            try
            {
                var item = new Devoluciones_BE();
                List<Devoluciones_BE> lista = new List<Devoluciones_BE>();
                item.ID_VENTA = ID_VENTA;
                item.MTIPO = 1;
                item.OBSERVACIONES = "";
                item.CREADO_POR = "";

                Devoluciones_BE encabezado = GetDatosSP_(item).FirstOrDefault();

                if (encabezado != null)
                {
                    encabezado.FECHA_STRING = encabezado.FECHA.ToString("dd/MM/yyyy hh:mm tt");
                    encabezado.FECHA_CERTIFICACION_STRING = encabezado.FECHA_CERTIFICACION.ToString("dd/MM/yyyy hh:mm tt");

                    item.MTIPO = 2;
                    lista = GetDatosSP_(item);
                }

                return Json(new { State = 1, data = encabezado, data_lista = lista }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { State = -1, Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}