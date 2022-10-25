using GenesysOracleSV.Clases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Ventas.Controllers
{
    public class DefaultController : Controller
    {
        // GET: Default
        //[SessionExpireFilterAttribute]
        public ActionResult Index()
        {
            return View();
        }
    }
}