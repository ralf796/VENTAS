using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Ventas_BE;
using Ventas_BLL;

namespace Ventas.Controllers.Compras
{
    public class COMListarComprasController : Controller
    {
        // GET: COMListarCompras
        public ActionResult Index()
        {
            return View();
        }

        private List<Inventario_BE> GetSPReportes_(Inventario_BE item)
        {
            List<Inventario_BE> lista = new List<Inventario_BE>();
            lista = Inventario_BLL.GetSPCompras(item);
            return lista;
        }

        public JsonResult GetDatos(string fechaInicial = "", string fechaFinal = "")
        {
            try
            {
                var item = new Inventario_BE();
                item.MTIPO = 2;
                item.FECHA_ENTREGA = DateTime.Now;
                item.FECHA_PEDIDO = DateTime.Now;
                item.FECHA_PAGO = DateTime.Now;
                var lista = GetSPReportes_(item);
                foreach (var row in lista)
                {
                    row.FECHA_ENTREGA_STRING = row.FECHA_ENTREGA.ToString("dd/MM/yyyy");
                    row.FECHA_PEDIDO_STRING = row.FECHA_PEDIDO.ToString("dd/MM/yyyy");
                    row.FECHA_PAGO_STRING = row.FECHA_PAGO.ToString("dd/MM/yyyy");
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