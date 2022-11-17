using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace Ventas.Class
{
    public class EnviarNIT
    {
        public static string ValidarNit(string numeronit)
        {
            string nitvalido = null;
            numeronit = numeronit.ToUpper();
            numeronit = numeronit.Trim();

            //expresion regular que valida si el el string cumple con el patron del nit
            string strPatron = @"^[0-9]+-([K]|[0-9])$";

            bool coincide = Regex.IsMatch(numeronit, strPatron);

            if (coincide == true & numeronit.Length > 3)
            {
                //obtiene la posicion del guion 
                int pos = numeronit.IndexOf("-");

                //obtiene los numeros antes del guion
                string strCorrelativo = numeronit.Substring(0, pos);
                //obtiene el Digito Verificador
                string strDigitoVerificador = numeronit.Substring(pos + 1);
                strDigitoVerificador = strDigitoVerificador.Trim();

                //variables para la validacion
                int Factor = strCorrelativo.Length + 1;
                int Suma = 0;
                int Valor = 0;

                //empieza el proceso de la validacion
                for (int x = 0; x <= pos - 1; x++)
                {
                    Valor = Int32.Parse(numeronit.Substring(x, 1));
                    Suma = Suma + (Valor * Factor);
                    Factor = Factor - 1;
                }

                //Se obtiene el residuo para validar con el Digito Verificador
                double xMod11 = 0;
                xMod11 = (11 - (Suma % 11)) % 11;
                string verificador = Math.Floor(xMod11).ToString();

                if ((verificador.Equals(strDigitoVerificador)) | (xMod11 == 10 && strDigitoVerificador.Equals("K")))
                {
                    nitvalido = numeronit;

                    nitvalido = nitvalido.Replace("-", "");

                    string ceros = "";

                    //se complementa con ceros el nit validado
                    for (int g = 0; g < 12 - nitvalido.Length; g++)
                    {
                        //ceros += "0";
                        ceros += "";
                    }

                    nitvalido = ceros + nitvalido;
                }
                else
                {
                    nitvalido = "CF";
                }

            }
            else
            {
                string nit = "";

                if (numeronit.Length >= 1)
                {
                    nit = numeronit.Substring(0, 1);

                    if (nit.Equals("C"))
                    {
                        nitvalido = "CF";
                    }
                    else
                    {
                        nitvalido = "CF";
                    }
                }
                else
                {
                    nitvalido = "CF";
                }
            }

            //retorna el nit ya validado y con el formato requerido para el XML
            return nitvalido;

        }

    }
}