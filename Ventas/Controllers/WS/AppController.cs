using Microsoft.Owin;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Ventas_BE;
using Ventas_BLL;

namespace Ventas.Controllers.WS
{
    public class AppController : Controller
    {
        // GET: App
        public ActionResult Index()
        {
            return View();
        }

        private List<Inventario_BE> GetInventario_select_(Inventario_BE item)
        {
            List<Inventario_BE> lista = new List<Inventario_BE>();
            lista = Inventario_BLL.GetInventario_select(item);
            return lista;
        }

        /*
        //[TokenSession]
        [HttpGet]
        [Route("~/Api/GetProductos")]
        [Authorize]
        public JsonResult<List<Inventario_BE>> AppGetProductos()
        {
            try
            {
                UserProvider userProvider = new UserProvider(((OwinContext)Request.GetOwinContext()).Authentication.User);

                var item = new Inventario_BE();
                item.MTIPO = 20;                
                var lista = GetInventario_select_(item);

                return this.Json<List<Inventario_BE>>(lista, new JsonSerializerSettings()
                {
                    NullValueHandling = (NullValueHandling)1
                });
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        */
    }
}