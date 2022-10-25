using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ventas_BE
{
    public class Inventario_BE
    {
        public string NOMBRE { get; set; }
        public string DESCRIPCION { get; set; }
        public int? ESTANTERIA { get; set; }
        public int? NIVEL { get; set; }
        public int? ID_PRODUCTO { get; set; }
        public int? ID_CATEGORIA { get; set; }
        public int? ID_MODELO { get; set; }
        public int? ID_TIPO { get; set; }
        public int? ID_BODEGA { get; set; }
        public int MTIPO { get; set; }
        public int ESTADO { get; set; }
        public string CREADO_POR { get; set; }
        public string RESPUESTA { get; set; }
        public decimal PRECIO_COSTO { get; set; }
        public decimal PRECIO_VENTA { get; set; }
        public int STOCK { get; set; }
        public int ANIO_FABRICADO { get; set; }
        public string CODIGO { get; set; }
        public string NOMBRE_CATEGORIA { get; set; }
        public string NOMBRE_MODELO { get; set; }
        public string NOMBRE_TIPO { get; set; }
        public string NOMBRE_BODEGA { get; set; }
        public int ID_SUBCATEGORIA{ get; set; }
        public int ID_PROVEEDOR{ get; set; }
        public int ID_MARCA_REPUESTO{ get; set; }
        public int ID_MARCA_VEHICULO{ get; set; }
        public int ID_SERIE_VEHICULO{ get; set; }
        public int ANIO_INICIAL{ get; set; }
        public int ANIO_FINAL{ get; set; }
        public int ID_DELETE { get; set; }
    }
}
