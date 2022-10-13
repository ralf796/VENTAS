using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using static GenesysOracleSV.Clases.Utils;

namespace GenesysOracleSV.Clases
{
    public class PermissionFilterAttribute : AuthorizeAttribute
    {
        /// <summary>
        /// Acción para evaluar permiso
        /// </summary>
        public Acciones Accion { get; set; }
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            HttpContext ctx = HttpContext.Current;
            // Verificar si la sesión es compatible
            if (ctx.Session != null)
            {
                // Comprobar si el usuario tiene permiso de la acción solicitada
                if (Home.Instance.TienePermiso(Accion) == 0)
                {
                    //------------Verificar si es una soliciud AJAX ------
                    if (filterContext.HttpContext.Request.IsAjaxRequest())
                    {
                        filterContext.HttpContext.Response.ClearContent();
                        filterContext.HttpContext.Items["AjaxPermissionDenied"] = true;
                        filterContext.Result = new HttpUnauthorizedResult();
                    }
                    else
                        filterContext.Result = new RedirectResult(Home.Instance.ObtenerDefaultModulo());
                }
            }
        }
    }
}