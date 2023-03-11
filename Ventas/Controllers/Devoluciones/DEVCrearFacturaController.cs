using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Ventas.Class;
using Ventas_BE;
using Ventas_BLL;

namespace Ventas.Controllers.Devoluciones
{
    public class DEVCrearFacturaController : Controller
    {
        // GET: DEVCrearFactura
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
        private static List<Devoluciones_BE> GetDatosSPdevolucion_(Devoluciones_BE item)
        {
            List<Devoluciones_BE> lista = new List<Devoluciones_BE>();
            lista = Devoluciones_BLL.GetDatosSP(item);
            return lista;
        }

        private bool SaveHeader(int idVenta = 0, string serie = "", decimal correlativo = 0, int idCliente = 0, decimal total = 0, decimal descuento = 0, decimal subtotal = 0, string usuario = "", int fel = 0, int esCredito = 0, string fecha_factura = "")
        {
            bool respuesta = false;
            var item = new Ventas__BE();
            item.MTIPO = 12;
            item.ID_VENTA = idVenta;
            item.SERIE = serie;
            item.CORRELATIVO = correlativo;
            item.ID_CLIENTE = idCliente;
            item.TOTAL = total;
            item.SUBTOTAL = subtotal;
            item.TOTAL_DESCUENTO = descuento;
            item.CREADO_POR = usuario;
            item.FEL = fel;
            item.ID_PRODUCTO = esCredito;
            item.FECHA_FACTURA = Convert.ToDateTime(fecha_factura);
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
            item.FECHA_FACTURA = DateTime.Now;
            item.MTIPO = 5;
            item.ID_VENTA = idVenta;
            item.ID_PRODUCTO = idProducto;
            item.CANTIDAD = cantidad;
            item.PRECIO_VENTA = precio;
            item.TOTAL = total;
            item.TOTAL_DESCUENTO = descuento;
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
        private bool DeleteOrder(int idVenta = 0)
        {
            bool respuesta = false;
            var item = new Ventas__BE();
            item.FECHA_FACTURA = DateTime.Now;
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
        private bool DescontProduct(int idVenta = 0)
        {
            bool respuesta = false;
            var item = new Ventas__BE();
            item.FECHA_FACTURA = DateTime.Now;
            item.MTIPO = 7;
            item.ID_VENTA = idVenta;
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
        public void AnularVenta(int id_venta = 0, string usuario = "")
        {
            var itemV = new Devoluciones_BE();
            itemV.ID_VENTA = id_venta;
            itemV.MTIPO = 4;
            itemV.OBSERVACIONES = "";
            itemV.CREADO_POR = "";
            itemV = GetDatosSPdevolucion_(itemV).FirstOrDefault();

            int fel = 0;

            if (itemV != null)
                fel = itemV.FEL;

            if (fel == 1)
            {
                try
                {
                    var itemAnula = new FEL_BE();
                    itemAnula.ID_VENTA = id_venta;
                    var respuestaFEL = Certificador_FEL.Anulador_XML_FEL(itemAnula);
                }
                catch
                {

                }
            }

            var item = new Caja_BE();
            item.ID_VENTA = id_venta;
            item.MTIPO = 5;
            item.CREADO_POR = usuario;
            var lista = GetDatosCaja_(item);
        }
        public void SaveDevolucion(int id_venta_anterior = 0, int id_venta_actual = 0, string usuario = "")
        {
            var itemV = new Devoluciones_BE();
            itemV.ID_VENTA_ANTERIOR = id_venta_anterior;
            itemV.ID_VENTA_NUEVA = id_venta_actual;
            itemV.MTIPO = 3;
            itemV.OBSERVACIONES = "";
            itemV.CREADO_POR = usuario;
            itemV = GetDatosSPdevolucion_(itemV).FirstOrDefault();
        }

        private List<Ventas__BE> GetDatosSP_(Ventas__BE item)
        {
            List<Ventas__BE> lista = new List<Ventas__BE>();
            lista = Ventas__BLL.GetDatosSP(item);
            return lista;
        }
        public JsonResult SaveOrder(string encabezado = "", string detalles = "", int fel = 0, int esCredito = 0, string fecha_factura = "", int id_venta_anterior = 0)
        {
            try
            {
                if (fecha_factura == "")
                    fecha_factura = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss");
                else
                    fecha_factura = $"{fecha_factura} 08:00:00";

                var item = JsonConvert.DeserializeObject<Ventas__BE>(encabezado);
                List<Ventas__BE> listaDetalles = JsonConvert.DeserializeObject<List<Ventas__BE>>(detalles);
                string usuario = Session["usuario"].ToString();
                item.CREADO_POR = usuario;
                int state = 1;
                bool banderaDetail = false;

                //return Json(new { State = 2 }, JsonRequestBehavior.AllowGet);

                var itemID = new Ventas__BE();
                itemID.MTIPO = 6;
                itemID.FECHA_FACTURA = DateTime.Now;
                item.ID_VENTA = GetDatosSP_(itemID).FirstOrDefault().ID_VENTA;

                if (SaveHeader(Convert.ToInt32(item.ID_VENTA), "", 1, item.ID_CLIENTE, item.TOTAL, item.TOTAL_DESCUENTO, item.SUBTOTAL, usuario, fel, esCredito, fecha_factura) == true)
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

                string uuid = "";

                if (banderaDetail == true)
                    state = 2;
                else
                {
                    var firma = new FEL_BE();
                    firma.ID_VENTA = Convert.ToInt32(item.ID_VENTA);
                    var respuestaFEL = Certificador_FEL.Certificador_XML_FAC_FEL(firma);
                    if (!respuestaFEL.RESULTADO)
                    {
                        return Json(new { State = -2, Message = "Error de firma FEL: " + respuestaFEL.MENSAJE_FEL }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        AnularVenta(id_venta_anterior, usuario);
                        DescontProduct(Convert.ToInt32(item.ID_VENTA));
                        SaveDevolucion(id_venta_anterior, Convert.ToInt32(item.ID_VENTA), usuario);

                        DescontProduct(Convert.ToInt32(item.ID_VENTA));

                        var update = new FEL_BE();
                        update.MTIPO = 5;
                        update.ID_VENTA = Convert.ToInt32(item.ID_VENTA);
                        update.UUID = respuestaFEL.UUID;
                        update.SERIE_FEL = respuestaFEL.SERIE_FEL;
                        update.NUMERO_FEL = respuestaFEL.NUMERO_FEL;
                        update.FECHA_CERTIFICACION = respuestaFEL.FECHA_CERTIFICACION;
                        update.FEL = 1;
                        var respuesta_update = FEL_BLL.GetDatosSP(update).FirstOrDefault();

                        uuid = respuestaFEL.UUID;
                    }

                }
                return Json(new { State = state, ORDEN_COMPRA = item.ID_VENTA, uuid = uuid }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { State = -1, Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}