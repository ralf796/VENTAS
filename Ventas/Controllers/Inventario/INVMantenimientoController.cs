using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Ventas_BE;
using Ventas_BLL;

namespace Ventas.Controllers.Inventario
{
    public class INVMantenimientoController : Controller
    {
        // GET: CATEGORIA
        public ActionResult IndexCategoria()
        {
            return View();
        }
        // GET: MODELO
        public ActionResult IndexModelo()
        {
            return View();
        }
        // GET: TIPO
        public ActionResult IndexMantenimiento()
        {
            return View();
        }
        // GET: BODEGA
        public ActionResult IndexBodega()
        {
            return View();
        }



        private List<Inventario_BE> GetDatosInventario_(Inventario_BE item)
        {
            List<Inventario_BE> lista = new List<Inventario_BE>();
            lista = Inventario_BLL.GetSPInventario(item);
            return lista;
        }

        public JsonResult Guardar(string nombre = "", string descripcion = "", int tipo=0)
        {
            try
            {
                string respuesta = "";
                var item = new Inventario_BE();
                item.NOMBRE = nombre;
                item.DESCRIPCION = descripcion;
                item.CREADO_POR = "RALOPEZ";
                item.MTIPO = tipo;

                var lista = GetDatosInventario_(item);

                if (lista.Count > 0)
                {
                    if (lista.FirstOrDefault().RESPUESTA != "")
                    {
                        respuesta = lista.FirstOrDefault().RESPUESTA;
                    }
                }
                return Json(new { State = 1 }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { State = -1, Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetDatosTable()
        {
            try
            {
                var item = new Inventario_BE();
                item.MTIPO = 4;
                var lista = GetDatosInventario_(item);
                return Json(new { State = 1, data = lista }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { State = -1, Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

    }
}