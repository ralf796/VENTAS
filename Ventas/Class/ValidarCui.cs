using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace Ventas.Class
{
    public class ValidarCui
    {
        public static bool ValidarDPI(string dpi)
        {
            var regex = "^[0-9]{4}[0-9]{5}[0-9]{4}$";
            var test = Regex.IsMatch(dpi, regex);

            if (!test)
            {
                return false;
            }

            var cui = dpi.Replace("-", "");
            var cui2 = cui.Replace(" ", "");

            cui = "";
            cui = cui2;

            var numero = cui.Substring(0, 8);


            var depto = Convert.ToInt32(cui.Substring(9, 2));
            var muni = Convert.ToInt32(cui.Substring(11, 2));

            var validador = Convert.ToInt32(cui.Substring(8, 1));

            // Conteo de municipios por departamento  
            int[] munisPorDepto =
            {  
                /* 01 - Guatemala    */ 17,  
                /* 02 - El Progreso   */ 8,  
                /* 03 - Sacatepéquez  */ 16,  
                /* 04 - Chimaltenango  */ 16,  
                /* 05 - Escuintla    */ 14,  
                /* 06 - Santa Rosa   */ 14,  
                /* 07 - Sololá     */ 19,  
                /* 08 - Totonicapán   */ 8,  
                /* 09 - Quetzaltenango */ 24,  
                /* 10 - Suchitepéquez  */ 21,  
                /* 11 - Retalhuleu   */ 9,  
                /* 12 - San Marcos   */ 30,  
                /* 13 - Huehuetenango  */ 33,  
                /* 14 - Quiché     */ 21,  
                /* 15 - Baja Verapaz  */ 8,  
                /* 16 - Alta Verapaz  */ 17,  
                /* 17 - Petén      */ 14,  
                /* 18 - Izabal     */ 5,  
                /* 19 - Zacapa     */ 11,  
                /* 20 - Chiquimula   */ 11,  
                /* 21 - Jalapa     */ 7,  
                /* 22 - Jutiapa     */ 17
            };

            if (muni == 0 || depto == 0)
            {
                return false;
            }

            if (depto > munisPorDepto.Length)
            {
                return false;
            }

            if (muni > munisPorDepto[depto - 1])
            {
                return false;
            }

            int total = 0;
            for (int i = 0; i < numero.Length; i++)
            {
                total += (Convert.ToInt32(numero.Substring(i, 1))) * (i + 2);
            }

            int modulo = total % 11;

            return modulo == validador;
        }
    }
}