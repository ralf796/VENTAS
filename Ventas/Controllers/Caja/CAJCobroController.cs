using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Ventas_BE;
using Ventas_BLL;
namespace Ventas.Controllers.Caja
{
    public class CAJCobroController : Controller
    {
        // GET: CAJCobro
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
        //FUNCION LISTAR COBRO
        public JsonResult GetCobro()
        {
            try
            {
                var item = new Caja_BE();
                item.MTIPO = 1;
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
        //funcion LISTAR DETALLE VENTA
        public JsonResult GetDetalle(int id_venta = 0)
        {
            try
            {
                var item = new Caja_BE();
                item.ID_VENTA = id_venta;
                item.MTIPO = 2;
                var lista = GetDatosCaja_(item);
                return Json(new { State = 1, data = lista }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { State = -1, Message = ex.Message }, JsonRequestBehavior.AllowGet);

            }
        }
        // funcion cobro efectuado
        public JsonResult getCobroEfectuado(int id = 0, decimal cobro = 0, int formaPago = 0, string creado_por = "")
        {
            try
            {
                var item = new Caja_BE();
                item.ID_VENTA = id;
                item.TOTAL = cobro;
                if (formaPago == 1)
                {
                    item.TIPO_COBRO = "E";
                }
                else if (formaPago == 2)
                {
                    item.TIPO_COBRO = "T";
                }
                item.CREADO_POR = creado_por;
                item.MTIPO = 3;
                var lista = GetDatosCaja_(item);
                return Json(new { State = 1, data = lista }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { State = -1, Message = ex.Message }, JsonRequestBehavior.AllowGet);

            }

        }
        /*---------------------------------funcion Anular Venta*/
        public JsonResult getAularVenta(int id_venta = 0)
        {
            try
            {
                var item = new Caja_BE();
                item.ID_VENTA = id_venta;
                item.MTIPO = 5;
                var lista = GetDatosCaja_(item);
                return Json(new { State = 1, data = lista }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { State = -1, Message = ex.Message }, JsonRequestBehavior.AllowGet);

            }
        }
    }
}