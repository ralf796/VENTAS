using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Ventas.Class;
using Ventas_BE;
using Ventas_BLL;

namespace Ventas.Controllers.Cartera
{
    public class CARCrearCreditoController : Controller
    {
        // GET: CARCrearCredito
        public ActionResult Index()
        {
            return View();
        }

        private static List<Cartera_BE> GetDatosSP_(Cartera_BE item)
        {
            List<Cartera_BE> lista = new List<Cartera_BE>();
            lista = Cartera_BLL.GetDatosSP(item);
            return lista;
        }

        public JsonResult GetVenta(int MTIPO = 0, int ID_VENTA = 0)
        {
            try
            {
                var item = new Cartera_BE();
                item.ID_VENTA = ID_VENTA;
                item.MTIPO = MTIPO;

                item = GetDatosSP_(item).FirstOrDefault();
                item.FECHA_STRING = item.FECHA.ToString("dd/MM/yyyy hh:mm tt");
                item.FECHA_CERTIFICACION_STRING = item.FECHA_CERTIFICACION.ToString("dd/MM/yyyy hh:mm tt");

                return Json(new { State = 1, data = item }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { State = -1, Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}


/*
 
--Crea un trigger llamado AddCines
CREATE TRIGGER AddCines 
--Se ejecutara en la tabla Cinemex_Ciudades
   ON  Cinemex_Ciudades
--Se ejecutara despues de un Insert o un Update a la tabla
   AFTER INSERT,UPDATE
AS 
BEGIN
	-- SET NOCOUNT ON impide que se generen mensajes de texto con cada instrucción 
	SET NOCOUNT ON;
    -- Se crea un Insert: cuando se inserten valores en la tabla Cinemex_Ciudades, el trigger insertara un registro en la tabla Cinemex_Cines
    INSERT INTO Cinemex_Cines 
    (ID, IDCiudad, Cine, Direccion)
    SELECT '500', ID, 'Cinemex ' + Ciudad, 'Prueba'
    FROM INSERTED
--Los valores que se insertaran, seran los que esten almacenados en la tabla virtual Inserted
END
GO
 
 */