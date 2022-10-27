﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ventas_BE
{
    public class Ventas__BE
    {
        public int ID_CLIENTE { get; set; }
        public string NOMBRE{ get; set; }
        public string DIRECCION { get; set; }
        public string TELEFONO { get; set; }
        public string EMAIL { get; set; }
        public string NIT { get; set; }
        public string CREADO_POR { get; set; }
        public int ESTADO { get; set; }
        public int MTIPO { get; set; }
        public string MENSAJE_RESPUESTA{ get; set; }
        public string CODIGO_RESPUESTA{ get; set; }
        public string CODIGO{ get; set; }
        public string DESCRIPCION { get; set; }
        public decimal PRECIO_VENTA { get; set; }
        public int ID_PRODUCTO { get; set; }
        public int STOCK{ get; set; }
        public string NOMBRE_MARCA_REPUESTO { get; set; }
        public string NOMBRE_CATEGORIA { get; set; }
        public string NOMBRE_SUBCATEGORIA { get; set; }
        public string NOMBRE_MARCA_VEHICULO { get; set; }
        public string NOMBRE_SERIE_VEHICULO { get; set; }
        public int? ID_MARCA_REPUESTO { get; set; }
        public int? ID_SUBCATEGORIA { get; set; }
        public int? ID_CATEGORIA { get; set; }
        public int? ID_SERIE_VEHICULO { get; set; }
        public int? ID_MARCA_VEHICULO { get; set; }
        public int? ID_MODELO { get; set; }

    }
}
