using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Ventas_BE;
using Ventas_BLL;

namespace Ventas.Controllers.Usuarios
{
    public class USUListarUsuarioController : Controller
    {
        // GET: USUListarUsuario
        public ActionResult Index()
        {
            return View();
        }

        private List<Usuarios_BE> GetDatosUsuario_(Usuarios_BE item)
        {
            List<Usuarios_BE> lista = new List<Usuarios_BE>();
            lista = Usuarios_BLL.GetSPUsuario(item);
            return lista;
        }

        public JsonResult GetUsuarios()
        {
            try
            {
                var item = new Usuarios_BE();
                item.MTIPO = 4;
                var lista = GetDatosUsuario_(item);
                return Json(new { State = 1, data = lista }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { State = -1, Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}