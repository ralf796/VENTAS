using GenesysOracleSV.Models;
using Newtonsoft.Json;
using Oracle.ManagedDataAccess.Client;
using Oracle.ManagedDataAccess.Types;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Script.Serialization;

namespace GenesysOracleSV.Clases
{
    public class Utils
    {

        public int GetUsuarioRol()
        {
            int respuesta = 0;


            return respuesta;
        }


        private static string ObtenerConexion()
        {
            return ConfigurationManager.ConnectionStrings["genesysConnectionString"].ToString();
        }

        public static string ObtenerServidor()
        {
            var server = ConfigurationManager.ConnectionStrings["genesysEntities"].ConnectionString;
            if (server.Contains("192.168.136.23"))
                server = "DESARROLLO";
            else if (server.Contains("dbgenesys"))
                server = "ORACLE AUTONOMOUS";
            else
                server = "SERVER UNKNOWN";
            return server;
        }

        public static string GenerarToken(string cadena)
        {
            int aleatorio = new Random().Next(10000, 99999);
            cadena += aleatorio.ToString() + DateTime.Now.ToString("ddMMyyyy").Trim().ToUpper();
            return Encryption.Instance.Encrypt(cadena);
        }


        public enum Roles : int
        {
            SUPER_USUARIO = 1,
            ADMINISTRADOR = 2,
            CAJERO = 3,
            BODEGUERO = 4,
            SECRETARIA = 5,
            VENDEDOR = 6
        }

        public static class RandomGenerator
        {
            // Generate a random number between two numbers    
            public static int RandomNumber(int min, int max)
            {
                Random random = new Random();
                return random.Next(min, max);
            }

            // Generate a random string with a given size    
            public static string RandomString(int size, bool lowerCase)
            {
                StringBuilder builder = new StringBuilder();
                Random random = new Random();
                char ch;
                for (int i = 0; i < size; i++)
                {
                    ch = Convert.ToChar(Convert.ToInt32(Math.Floor(26 * random.NextDouble() + 65)));
                    builder.Append(ch);
                }
                if (lowerCase)
                    return builder.ToString().ToLower();
                return builder.ToString();
            }

            // Generate a random password    
            public static string RandomPassword()
            {
                StringBuilder builder = new StringBuilder();
                builder.Append(RandomString(4, true));
                builder.Append(RandomNumber(1000, 9999));
                builder.Append(RandomString(2, false));
                return builder.ToString();
            }
        }
    }
}