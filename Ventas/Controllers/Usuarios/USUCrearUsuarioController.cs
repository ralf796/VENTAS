using GenesysOracleSV.Clases;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using Ventas_BE;
using Ventas_BLL;

namespace Ventas.Controllers.Usuarios
{
    public class USUCrearUsuarioController : Controller
    {
        // GET: USUMantenimiento
        public ActionResult Index()
        {
            return View();
        }

        /*
         ********* CONEXION SQL
        DB: SQL8001.site4now.net
        USER: db_a8e200_dbsalesment_admin
        PASS: Xml1234#
         */
        /*
         * https://localhost:44302/USUMantenimiento/GuardarUsuario?primerNombre=&segundoNombre=&primerApellido=&segundoApellido=&telefono=&direccion=&idTipoEmpleado=1&email=&creadoPor=RALOPEZ&usuario=new&password=1234&idRol=1
         * */

        private List<Usuarios_BE> GetDatosUsuario_(Usuarios_BE item)
        {
            List<Usuarios_BE> lista = new List<Usuarios_BE>();
            lista = Usuarios_BLL.GetSPUsuario(item);
            return lista;
        }

        public JsonResult GuardarUsuario(string primerNombre = "", string segundoNombre = "", string primerApellido = "", string segundoApellido = "", string celular = "",
            string telefono = "", string direccion = "", int idTipoEmpleado = 0, string email = "", string usuario = "", string password = "", int idRol = 0, string urlFoto = "")
        {
            try
            {
                urlFoto = urlFoto.Replace("\\fakepath","");
                string respuesta = "";
                var item = new Usuarios_BE();
                item.PRIMER_NOMBRE = primerNombre;
                item.SEGUNDO_NOMBRE = segundoNombre;
                item.PRIMER_APELLIDO = primerApellido;
                item.SEGUNDO_APELLIDO = segundoApellido;
                item.DIRECCION = direccion;
                item.CELULAR = celular;
                item.TELEFONO = telefono;
                item.ID_TIPO_EMPLEADO = idTipoEmpleado;
                item.EMAIL = email;
                item.CREADO_POR = "RALOPEZ";
                item.USUARIO = usuario;
                item.PASSWORD = new Encryption().Encrypt(password.Trim());
                item.ID_ROL = idRol;
                item.URL_FOTOGRAFIA = urlFoto;
                item.MTIPO = 1;
                var lista = GetDatosUsuario_(item);

                if (lista.Count > 0)
                {
                    if (lista.FirstOrDefault().RESPUESTA != "")
                    {
                        string fileName = Server.MapPath(@"~\Content\Fotografias\imgtest.png");
                        Image image = Image.FromFile(item.URL_FOTOGRAFIA);
                        image.Save(fileName);

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

        public JsonResult GetLists(int tipo)
        {
            try
            {
                var item = new Usuarios_BE();
                item.MTIPO = tipo;

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