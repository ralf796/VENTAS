﻿using System;
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
        public int? ID_CATEGORIA { get; set; }
        public int? ID_MODELO { get; set; }
        public int? ID_TIPO { get; set; }
        public int? ID_BODEGA { get; set; }
        public int MTIPO { get; set; }
        public int ESTADO { get; set; }
        public string CREADO_POR{ get; set; }
        public string RESPUESTA{ get; set; }
    }
}
