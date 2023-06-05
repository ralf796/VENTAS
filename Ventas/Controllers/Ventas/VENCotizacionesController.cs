using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Ventas.Class;
using Ventas_BE;
using Ventas_BLL;

namespace Ventas.Controllers.Ventas
{
    public class VENCotizacionesController : Controller
    {
        // GET: VENCotizaciones
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
        public JsonResult GetReporte(string fechaInicial = "", string fechaFinal = "")
        {
            try
            {
                var item = new Reportes_BE();
                item.MTIPO = 35;
                item.FECHA_INICIAL = Convert.ToDateTime(fechaInicial);
                item.FECHA_FINAL = DateTime.Now;
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

        private List<Ventas__BE> GetDatosSP_Ventas(Ventas__BE item)
        {
            List<Ventas__BE> lista = new List<Ventas__BE>();
            lista = Ventas__BLL.GetDatosSP(item);
            return lista;
        }

        private bool DescontProduct(int idVenta = 0)
        {
            bool respuesta = false;
            var item = new Ventas__BE();
            item.FECHA_FACTURA = DateTime.Now;
            item.MTIPO = 7;
            item.ID_VENTA = idVenta;
            var resultHeader = GetDatosSP_Ventas(item);

            if (resultHeader != null)
            {
                if (resultHeader.FirstOrDefault().CODIGO_RESPUESTA == "01")
                    respuesta = true;
            }
            else
                respuesta = false;

            return respuesta;
        }

        [SessionExpireFilter]
        public JsonResult SaveOrder(int id_venta = 0)
        {
            try
            {
                int state = 0;
                string usuario = Session["usuario"].ToString();
                var item = new Ventas__BE();
                item.ID_VENTA = id_venta;
                item.CREADO_POR = usuario;
                item.FECHA_FACTURA = DateTime.Now;
                bool response = DescontProduct(Convert.ToInt32(item.ID_VENTA));

                if (response)
                {
                    item.MTIPO = 15;
                    item.ID_VENTA = id_venta;
                    var resultHeader = GetDatosSP_Ventas(item);
                    state = 1;
                }
                else
                    state = 2;

                return Json(new { State = 1 }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { State = -1, Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}