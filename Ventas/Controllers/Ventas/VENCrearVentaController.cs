using GenesysOracleSV.Clases;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using Ventas_BE;
using Ventas_BLL;

namespace Ventas.Controllers.Ventas
{
    public class VENCrearVentaController : Controller
    {
        // GET: VENCrearVenta
        [SessionExpireFilter]
        public ActionResult Index()
        {
            return View();
        }

        #region BD
        private List<Ventas__BE> GetDatosSP_(Ventas__BE item)
        {
            List<Ventas__BE> lista = new List<Ventas__BE>();
            lista = Ventas__BLL.GetDatosSP(item);
            return lista;
        }
        #endregion

        #region FUNCTIONS
        private bool SaveHeader(string serie = "", decimal correlativo = 0, int idCliente = 0, decimal total = 0, decimal descuento = 0, decimal subtotal = 0, string usuario = "")
        {
            bool respuesta = false;
            var item = new Ventas__BE();
            item.MTIPO = 4;
            item.ID_VENTA = 2;
            item.SERIE = serie;
            item.CORRELATIVO = correlativo;
            item.ID_CLIENTE = idCliente;
            item.TOTAL = total;
            item.SUBTOTAL = subtotal;
            item.TOTAL_DESCUENTO = descuento;
            item.CREADO_POR = usuario;
            var resultHeader = GetDatosSP_(item);

            if (resultHeader != null)
            {
                if (resultHeader.FirstOrDefault().CODIGO_RESPUESTA == "01")
                    respuesta = true;
            }
            else
                respuesta = false;

            return respuesta;
        }

        private bool SaveDetail(int idVenta = 0, int idProducto = 0, int cantidad = 0, decimal precio = 0, decimal total = 0, decimal descuento = 0, decimal subtotal = 0)
        {
            bool respuesta = false;
            var item = new Ventas__BE();
            item.MTIPO = 5;
            item.ID_VENTA = idVenta;
            item.ID_PRODUCTO = idProducto;
            item.CANTIDAD = cantidad;
            item.PRECIO_VENTA = precio;
            item.TOTAL = total;
            item.DESCUENTO = descuento;
            item.SUBTOTAL = subtotal;
            var resultDetail = GetDatosSP_(item);

            if (resultDetail != null)
            {
                if (resultDetail.FirstOrDefault().CODIGO_RESPUESTA == "01")
                    respuesta = true;
            }
            else
                respuesta = false;

            return respuesta;
        }
        //DELETE TBL_VENTA_DETALLE WHERE ID_VENTA=idVenta; UPDATE TBL_VENTA SET ESTADO=0 WHERE ID_VENTA=idVenta;

        private bool DeleteOrder(int idVenta = 0)
        {
            bool respuesta = false;
            var item = new Ventas__BE();
            item.MTIPO = 6;
            item.ID_VENTA = idVenta;
            var resultDetail = GetDatosSP_(item);

            if (resultDetail != null)
            {
                if (resultDetail.FirstOrDefault().CODIGO_RESPUESTA == "01")
                    respuesta = true;
            }
            else
                respuesta = false;

            return respuesta;
        }
        #endregion

        #region JSON_RESULTS
        public JsonResult GetCliente(int tipo = 0, string nit = "")
        {
            try
            {
                var item = new Ventas__BE();
                item.MTIPO = tipo;
                item.NIT = nit.Trim();
                item = GetDatosSP_(item).FirstOrDefault();
                return Json(new { State = 1, data = item }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { State = -1, Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetList(int tipo = 0, int ID_MARCA_REPUESTO = 0, int ID_SUBCATEGORIA = 0, int ID_CATEGORIA = 0, int ID_SERIE_VEHICULO = 0, int ID_MARCA_VEHICULO = 0, int ID_MODELO = 0)
        {
            try
            {
                var item = new Ventas__BE();
                item.MTIPO = tipo;
                if (ID_MARCA_REPUESTO != 0)
                    item.ID_MARCA_REPUESTO = ID_MARCA_REPUESTO;
                if (ID_SUBCATEGORIA != 0)
                    item.ID_SUBCATEGORIA = ID_SUBCATEGORIA;
                if (ID_CATEGORIA != 0)
                    item.ID_CATEGORIA = ID_CATEGORIA;
                if (ID_SERIE_VEHICULO != 0)
                    item.ID_SERIE_VEHICULO = ID_SERIE_VEHICULO;
                if (ID_MARCA_VEHICULO != 0)
                    item.ID_MARCA_VEHICULO = ID_MARCA_VEHICULO;
                if (ID_MODELO != 0)
                    item.ID_MODELO = ID_MODELO;
                var lista = GetDatosSP_(item);
                return Json(new { State = 1, data = lista }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { State = -1, Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [SessionExpireFilter]
        public JsonResult SaveOrder(string encabezado = "", string detalles = "")
        {
            try
            {
                var item = JsonConvert.DeserializeObject<Ventas__BE>(encabezado);
                List<Ventas__BE> listaDetalles = JsonConvert.DeserializeObject<List<Ventas__BE>>(detalles);
                string usuario = Session["usuario"].ToString();
                int state = 1;
                bool banderaDetail = false;
                //var item = new Ventas__BE();
                //List<Ventas__BE> listaDetalles = new List<Ventas__BE>();

                if (SaveHeader("SSS", 1, item.ID_CLIENTE, item.TOTAL, item.TOTAL_DESCUENTO, item.SUBTOTAL, usuario) == true)
                {
                    foreach (var row in listaDetalles)
                    {
                        if (banderaDetail == false)
                        {
                            if (SaveDetail(Convert.ToInt32(item.ID_VENTA), row.ID_PRODUCTO, row.CANTIDAD, row.PRECIO_VENTA, row.TOTAL, row.TOTAL_DESCUENTO, row.SUBTOTAL) == false)
                            {
                                DeleteOrder(Convert.ToInt32(item.ID_VENTA));
                                banderaDetail = true;
                            }
                        }
                    }
                }

                if (banderaDetail == true)
                    state = 2;

                return Json(new { State = state }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { State = -1, Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

    }
}