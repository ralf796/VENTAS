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
        private static readonly genesysEntities db = new genesysEntities();
        private static string ObtenerConexion()
        {
            return ConfigurationManager.ConnectionStrings["genesysConnectionString"].ToString();
        }

        /// <summary>
        /// Determina el servidor en el que se encuentra un usuario
        /// </summary>
        /// <returns></returns>
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

        public static DateTime ObtenerFecha()
        {
            string queryPuntoVenta = "SELECT util_pkg.obtener_fecha FROM DUAL";
            return db.Database.SqlQuery<DateTime>(queryPuntoVenta).FirstOrDefault();
        }

        public static List<MOTIVO_ANULACION> GetMotivoAnulacion()
        {
            genesysEntities db = new genesysEntities();
            string query = "SELECT util_pkg.get_motivo_anulacion_fn() FROM DUAL";
            var json = db.Database.SqlQuery<string>(query).FirstOrDefault();
            var data = new JavaScriptSerializer().Deserialize<List<MOTIVO_ANULACION>>(json);
            return data;
        }

        public static string GenerarToken(string cadena)
        {
            int aleatorio = new Random().Next(10000, 99999);
            cadena += aleatorio.ToString() + ObtenerFecha().ToString().Trim().ToUpper();
            return Encryption.Instance.Encrypt(cadena);
        }

        public static int ValidarContraseña(string usuario, string contraseña)
        {
            contraseña = Encryption.Instance.Encrypt(contraseña.Trim().ToUpper());
            string query = "SELECT COUNT(*) FROM SVCAT_USUARIO WHERE USUARIO='" + usuario + "' AND contrasena='" + contraseña + "'";
            int cont = db.Database.SqlQuery<int>(query).FirstOrDefault();
            return cont;
        }


        public static GenericResponse CrearBitacora(SVCFG_BITACORA datosJson)
        {
            var conn = new OracleConnection(ObtenerConexion());
            conn.Open();
            var cmd = new OracleCommand("util_pkg.crear_bitacora_sp", conn) { CommandType = CommandType.StoredProcedure, CommandTimeout = 0 };

            cmd.Parameters.Add("pjson", JsonConvert.SerializeObject(datosJson));
            cmd.Parameters.Add("respuesta", OracleDbType.Clob).Direction = ParameterDirection.Output;

            var respCrearFctura = new GenericResponse();
            try
            {
                cmd.ExecuteNonQuery();

                var clobData = (OracleClob)cmd.Parameters["respuesta"].Value;
                string json = Convert.ToString(clobData.Value);
                respCrearFctura = JsonConvert.DeserializeObject<GenericResponse>(json);
                return new GenericResponse { ESTADO = respCrearFctura.ESTADO, MENSAJE = respCrearFctura.MENSAJE };
            }
            catch (Exception ex)
            {
                return new GenericResponse { ESTADO = respCrearFctura.ESTADO, MENSAJE = respCrearFctura.MENSAJE };
            }
            finally
            {
                if (conn.State == ConnectionState.Open)
                    conn.Close();
            }
        }

        public static List<TIPO_DOCUMENTO_> GetTipoDocumentosFactura()
        {
            genesysEntities db = new genesysEntities();
            string query = "SELECT util_pkg.fn_get_tipo_documento_facturas() FROM DUAL";
            var json = db.Database.SqlQuery<string>(query).FirstOrDefault();
            var list = new JavaScriptSerializer().Deserialize<List<TIPO_DOCUMENTO_>>(json);
            return list;
        }

        public enum Acciones : int
        {
            LISTAR = 1,
            INSERTAR = 2,
            ACTUALIZAR = 3,
            ELIMINAR = 4,
            LIQUIDAR = 5,
            CORTE = 6,
            EXPORTAR_EXCEL = 7,
            EXPORTAR_PDF = 8,
            AUTORIZAR_CREDITO = 9,
            VER_PASSWORD = 10,
            GENERAR_REPORTE = 11,
            DESASOCIAR_TARJETA_VISANET = 12,
            AUTORIZAR_CUPON = 13,
            AJUSTRE_LIBRAS = 14,
            REPORTE_FRECUENCIA_CLIENTE = 15,
            REPORTE_CLIENTE_DEJO_CONSUMIR = 16
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



        public class MOTIVO_ANULACION
        {
            public int ID_MOTIVO_ANULACION { get; set; }
            public string DESCRIPCION { get; set; }
        }
        public class CLIENTE
        {

            public decimal? CODIGO_REFERENCIA { get; set; }
            public string NIT { get; set; }
            public string NRC { get; set; }
            public string NOMBRE_CLIENTE { get; set; }
            public string DIRECCION { get; set; }
            public string FORMA_PAGO { get; set; }
            public long? ID_CLIENTE { get; set; }
            public long? CORPORACION_CLIENTE { get; set; }
            public string CREDITO_BLOQUEADO { get; set; }
            public long? ID_CANAL_DISTRIBUCION { get; set; }
            public string AUTORIZADO_POR { get; set; }
            public long? APLICA_PERCEPCION { get; set; }
            public long? ID_SEGMENTACION { get; set; }
        }
        public class GenericResponse
        {
            public int ESTADO { get; set; }
            public string MENSAJE { get; set; }
            public int? ID_VALUE { get; set; }
        }
        public class SVBITACORA
        {

            public List<SVCFG_CLIENTE_DESCUENTO> DESCUENTOS { get; set; }
            public List<SVCAT_PRECIO> PRECIOS { get; set; }
        }
        public class TIPO_DOCUMENTO_
        {
            public long ID_TIPO_DOCUMENTO { get; set; }
            public string DESCRIPCION { get; set; }
        }

    }
}