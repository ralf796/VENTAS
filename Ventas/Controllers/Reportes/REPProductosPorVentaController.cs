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
    public class REPProductosPorVentaController : Controller
    {
        // GET: REPProductosPorVenta
        [SessionExpireFilter]
        public ActionResult Index()
        {
            return View();
        }
        [SessionExpireFilter]
        public ActionResult ReporteCaja()
        {
            return View();
        }
        public ActionResult ReporteSemaforo()
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
                item.MTIPO = 34;
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
        public JsonResult GetReporteCaja(string fechaInicial = "", string fechaFinal = "")
        {
            try
            {
                var item = new Reportes_BE();
                item.MTIPO = 36;
                item.FECHA_INICIAL = Convert.ToDateTime(fechaInicial);
                item.FECHA_FINAL = Convert.ToDateTime(fechaFinal);
                var lista = GetSPReportes_(item);
                foreach (var row in lista)
                {
                    row.FECHA_CREACION_STRING = row.FECHA_CREACION.ToString("dd/MM/yyyy hh:mm tt");
                    row.FECHA_VENTA_STRING = row.FECHA_CORTE.ToString("dd/MM/yyyy");
                }
                return Json(new { State = 1, data = lista }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { State = -1, Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult GetReporteSemaforo(string fechaInicial = "", string fechaFinal = "")
        {
            try
            {
                var item = new Reportes_BE();
                item.MTIPO = 37;
                item.FECHA_INICIAL = Convert.ToDateTime(fechaInicial);
                item.FECHA_FINAL = Convert.ToDateTime(fechaFinal);
                List<Reportes_BE> lista = new List<Reportes_BE>();
                List<Reportes_BE> lista_result = new List<Reportes_BE>();
                lista = GetSPReportes_(item);

                decimal montoTotal = 0;
                int c1 = 0, c2 = 0, c3 = 0;

                c1 = Convert.ToInt32(lista.Count * 0.20m);
                c2 = Convert.ToInt32(lista.Count * 0.30m);
                c3 = Convert.ToInt32(lista.Count * 0.50m);

                int cont = 0;
                foreach (var row in lista)
                {
                    if (row.CANTIDAD <= c1)
                    {
                        row.ESTADO = 1;
                        row.DESCRIPCION = "BAJO";
                    }
                    else if (row.CANTIDAD > c1 && row.CANTIDAD <= (c1 + c2))
                    {
                        row.ESTADO = 2;
                        row.DESCRIPCION = "MEDIO";
                    }
                    else
                    {
                        row.ESTADO = 3;
                        row.DESCRIPCION = "FRECUENTE";
                    }
                    cont++;
                }


                if (lista.Count > 0)
                {
                    montoTotal = lista.Sum(x => x.MONTO);
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