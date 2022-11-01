using GenesysOracleSV.Clases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Ventas_BE;
using Ventas_BLL;

namespace Ventas.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
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
        public JsonResult ValidarLogin(string usuario = "", string password = "")
        {
            try
            {
                string url = "";
                var item = new Usuarios_BE();
                item.USUARIO = usuario.ToUpper();
                item.PASSWORD = new Encryption().Encrypt(password.ToUpper().Trim());
                item.MTIPO = 1;
                var lista = new List<Usuarios_BE>();
                lista = GetSPLogin_(item);
                if (lista != null)
                {
                    item = lista.FirstOrDefault();

                    Session["id_usuario"] = item.ID_USUARIO.ToString().ToUpper();
                    Session["usuario"] = item.USUARIO.ToString().ToUpper();
                    Session["primer_nombre"] = item.PRIMER_NOMBRE.ToUpper();
                    Session["segundo_nombre"] = item.SEGUNDO_NOMBRE.ToUpper();
                    Session["primer_apellido"] = item.PRIMER_APELLIDO.ToUpper();
                    Session["segundo_apellido"] = item.SEGUNDO_APELLIDO.ToUpper();
                    Session["id_rol"] = Convert.ToInt16(item.ID_ROL);
                    Session["url_fotografia"] = item.PATH;

                    var urlBuilder = new System.UriBuilder(Request.Url.AbsoluteUri) { Path = Url.Content(@"~\"), Query = null, };
                    url = urlBuilder.ToString() + item.URL_PANTALLA;

                    item.URL_PANTALLA = url;
                    Session["url_pantalla"] = url;
                }
                else
                    item = null;

                return Json(new { State = 1, data = item }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { State = -1, Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        /*
            SUPER_USUARIO = 1,
            ADMINISTRADOR = 2,
            CAJERO = 3,
            BODEGUERO = 4,
            SECRETARIA = 5,
            VENDEDOR = 6
        */
    }
}