using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ventas.Class
{
    public class Crystal
    {
        public string SERIE{ get; set; }
        public int CORRELATIVO { get; set; }
        public string FECHA_EMISION { get; set; }
        public string FECHA_CERTIFICACION { get; set; }
        public string NIT { get; set; }
        public string NOMBRE { get; set; }
        public string DIRECCION { get; set; }
        public string TELEFONO { get; set; }
        public decimal TOTAL { get; set; }
    }
}