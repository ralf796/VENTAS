using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Ventas_BE;
using Ventas_BLL;

namespace Ventas.Controllers.Clientes
{
    public class CLIListarClientesController : Controller
    {
        // GET: CLIListarClientes
        public ActionResult Index()
        {
            return View();
        }
        private List<Clientes_BE> GetDatosCliente_(Clientes_BE item)
        {
            List<Clientes_BE> lista = new List<Clientes_BE>();
            lista = Clientes_BLL.GetSPCliente(item);
            return lista;
        }
        //FUNCION LISTAR CLIENTES
        public JsonResult GetClientes(int tipo = 0)
        {
            try
            {
                var item = new Clientes_BE();
                item.MTIPO = tipo;
                var lista = GetDatosCliente_(item);
                return Json(new { State = 1, data = lista }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { State = -1, Message = ex.Message }, JsonRequestBehavior.AllowGet);

            }
        }
        //FUNCION GUARDAR CLIENTE
        public JsonResult GuardarCliente(string nombre = "", string direccion = "", string telefono = "", string email = "", string nit = "")
        {
            try
            {
                string respuesta = "";
                var item = new Clientes_BE();
                item.NOMBRE = nombre;
                item.DIRECCION = direccion;
                item.TELEFONO = telefono;
                item.EMAIL = email;
                item.NIT = nit;
                item.CREADO_POR = "RALOPEZ";
                item.MTIPO = 1;
                var lista = GetDatosCliente_(item);

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

        //FUNCION ACTUALIZAR CLIENTE
        public JsonResult UpdateClientes(int id = 0, string nombre = "", string direccion = "", string telefono = "", string email = "", string nit = "", int tipo = 0)
        {
            try
            {
                string respuesta = "";
                var item = new Clientes_BE();
                item.MTIPO = tipo;
                item.ID_CLIENTE = id;
                item.NOMBRE = nombre;
                item.DIRECCION = direccion;
                item.TELEFONO = telefono;
                item.EMAIL = email;
                item.NIT = nit;
                var lista = GetDatosCliente_(item);
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

        //FUNCION DELETE
        public JsonResult Delete(int id = 0, int tipo = 0)
        {
            try
            {
                string respuesta = "";
                var item = new Clientes_BE();
                item.ID_CLIENTE = id;
                item.MTIPO = tipo;
                var lista = GetDatosCliente_(item);
                if (lista != null)
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
    }
}