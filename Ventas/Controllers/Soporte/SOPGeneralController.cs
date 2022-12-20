using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Ventas.Class;
using Ventas_BE;
using Ventas_BLL;

namespace Ventas.Controllers.Soporte
{
    public class SOPGeneralController : Controller
    {
        // GET: SOPGeneral
        public ActionResult Index()
        {
            return View();
        }

        private List<Usuarios_BE> GetSPLogin_(Usuarios_BE item)
        {
            List<Usuarios_BE> lista = new List<Usuarios_BE>();
            lista = Usuarios_BLL.GetSPLogin(item);
            return lista;
        }

        public JsonResult GetPassWord(string usuario = "")
        {
            try
            {
                var item = new Usuarios_BE();
                item.USUARIO = usuario.ToUpper();
                item.PASSWORD = "";
                item.MTIPO = 4;
                item = GetSPLogin_(item).FirstOrDefault();
                if (item != null)
                {
                    item.PASSWORD= new Encryption().Decrypt(item.PASSWORD);
                }

                return Json(new { State = 1, data = item }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { State = -1, Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}