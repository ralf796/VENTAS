using GenesysOracleSV.Clases;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
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
        [SessionExpireFilter]
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

        public JsonResult GuardarUsuarioFoto(FormCollection formCollection)
        {
            try
            {
                string primerNombre = Request.Form["primerNombre"].ToString();
                string segundoNombre = Request.Form["segundoNombre"].ToString();
                string primerApellido = Request.Form["primerApellido"].ToString();
                string segundoApellido = Request.Form["segundoApellido"].ToString();
                string celular = Request.Form["celular"].ToString();
                string telefono = Request.Form["telefono"].ToString();
                string direccion = Request.Form["direccion"].ToString();
                //int idTipoEmpleado = Convert.ToInt16(Request.Form["idTipoEmpleado"]);
                string email = Request.Form["email"].ToString();
                string usuario = Request.Form["usuario"].ToString();
                string password = Request.Form["password"].ToString();
                int idRol = Convert.ToInt16(Request.Form["idRol"]);
                string urlFoto = Request.Form["urlFoto"].ToString();

                int estado = 1;
                var randomNumber = new Random().Next(0, 100);
                string path = "", url = "";

                //int tipo = Convert.ToInt32(Request.Form["tipo"]);
                HttpPostedFileBase file = Request.Files["foto"];
                if ((file != null) && (file.ContentLength > 0) && !string.IsNullOrEmpty(file.FileName))
                {
                    string fileName = file.FileName;
                    string[] separarNombre = fileName.Split('.');
                    string extension = separarNombre[1];
                    Bitmap filee = new Bitmap(file.InputStream);
                    //string rut = @"C:\Users\raul.lopez\Documents\REPOSSV\" + file.FileName;
                    //string rut = @"C:\Users\raul.lopez\source\repos\VENTAS\Ventas\Content\Fotografias\" + file.FileName;

                    string directory = Server.MapPath(@"~\Content\Fotografias");
                    if (!Directory.Exists(fileName))
                    {
                        Directory.CreateDirectory(directory);
                    }

                    path = Server.MapPath(@"~\Content\Fotografias\" + separarNombre[0] + randomNumber + ".png");
                    filee.Save(path);

                    var urlBuilder = new System.UriBuilder(Request.Url.AbsoluteUri) { Path = Url.Content(@"~\Content\Fotografias\" + separarNombre[0] + randomNumber + ".png"), Query = null, };
                    Uri uri = urlBuilder.Uri;
                    url = urlBuilder.ToString();
                }

                string respuesta = "";
                var item = new Usuarios_BE();
                item.PRIMER_NOMBRE = primerNombre;
                item.SEGUNDO_NOMBRE = segundoNombre;
                item.PRIMER_APELLIDO = primerApellido;
                item.SEGUNDO_APELLIDO = segundoApellido;
                item.DIRECCION = direccion;
                item.CELULAR = celular;
                item.TELEFONO = telefono;
                //item.ID_TIPO_EMPLEADO = idTipoEmpleado;
                item.EMAIL = email;
                item.CREADO_POR = Session["usuario"].ToString();
                item.USUARIO = usuario;
                item.PASSWORD = new Encryption().Encrypt(password.Trim());
                item.ID_ROL = idRol;
                item.PATH = url;
                item.MTIPO = 1;
                var lista = GetDatosUsuario_(item);

                if (lista != null)
                {
                    if (lista.FirstOrDefault().RESPUESTA != "")
                    {
                        respuesta = lista.FirstOrDefault().RESPUESTA;
                    }
                    else
                        estado = 2;
                }
                else
                    estado = 2;
                return Json(new { State = estado, path_foto = url, respuesta = respuesta }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { State = -1, Message = ex.Message.ToString() }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult EditarUsuarioFoto(FormCollection formCollection)
        {
            try
            {
                string primerNombre = Request.Form["primerNombre"].ToString();
                string segundoNombre = Request.Form["segundoNombre"].ToString();
                string primerApellido = Request.Form["primerApellido"].ToString();
                string segundoApellido = Request.Form["segundoApellido"].ToString();
                //string celular = Request.Form["celular"].ToString();
                string telefono = Request.Form["telefono"].ToString();
                string direccion = Request.Form["direccion"].ToString();
                string email = Request.Form["email"].ToString();
                //int idTipoEmpleado = Convert.ToInt16(Request.Form["idTipoEmpleado"]);
                string urlFoto = Request.Form["urlFoto"].ToString();
                string idRol = Request.Form["idRol"].ToString();
                int id = Convert.ToInt16(Request.Form["id"]);


                int estado = 1;
                var randomNumber = new Random().Next(0, 100);
                string path = "", url = "";

                HttpPostedFileBase file = Request.Files["foto"];
                if ((file != null) && (file.ContentLength > 0) && !string.IsNullOrEmpty(file.FileName))
                {
                    string fileName = file.FileName;
                    string[] separarNombre = fileName.Split('.');
                    string extension = separarNombre[1];
                    Bitmap filee = new Bitmap(file.InputStream);

                    string directory = Server.MapPath(@"~\Content\Fotografias");
                    if (!Directory.Exists(fileName))
                        Directory.CreateDirectory(directory);

                    path = Server.MapPath(@"~\Content\Fotografias\" + separarNombre[0] + randomNumber + ".png");
                    filee.Save(path);

                    var urlBuilder = new System.UriBuilder(Request.Url.AbsoluteUri) { Path = Url.Content(@"~\Content\Fotografias\" + separarNombre[0] + randomNumber + ".png"), Query = null, };
                    Uri uri = urlBuilder.Uri;
                    url = urlBuilder.ToString();
                }

                string respuesta = "";
                var item = new Usuarios_BE();
                item.ID_USUARIO = id;
                item.PRIMER_NOMBRE = primerNombre;
                item.SEGUNDO_NOMBRE = segundoNombre;
                item.PRIMER_APELLIDO = primerApellido;
                item.SEGUNDO_APELLIDO = segundoApellido;
                item.DIRECCION = direccion;
                //item.CELULAR = celular;
                item.TELEFONO = telefono;
                //item.ID_TIPO_EMPLEADO = idTipoEmpleado;
                item.EMAIL = email;
                item.CREADO_POR = Session["usuario"].ToString();

                if (idRol != "" && idRol != null && idRol != "null")
                    item.ID_ROL = Convert.ToInt32(idRol);
                if (url != "" && url != null)
                    item.PATH = url;
                item.MTIPO = 5;
                item.ID_EMPLEADO = id;
                var lista = GetDatosUsuario_(item);

                if (lista != null)
                {
                    if (lista.FirstOrDefault().RESPUESTA != "")
                    {
                        respuesta = lista.FirstOrDefault().RESPUESTA;
                    }
                    else
                        estado = 2;
                }
                else
                    estado = 2;
                return Json(new { State = estado, path_foto = url, respuesta = respuesta }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { State = -1, Message = ex.Message.ToString() }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult Delete(int id = 0)
        {
            try
            {
                string respuesta = "";
                var item = new Usuarios_BE();
                item.ID_TIPO_EMPLEADO = id;
                item.MTIPO = 6;
                var lista = GetDatosUsuario_(item);
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
    }
}