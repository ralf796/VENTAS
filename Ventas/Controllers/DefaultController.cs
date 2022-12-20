using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Ventas.Class;

namespace Ventas.Controllers
{
    public class DefaultController : Controller
    {
        // GET: Default
        [SessionExpireFilter]
        public ActionResult Index()
        {
            return View();
        }
    }
}