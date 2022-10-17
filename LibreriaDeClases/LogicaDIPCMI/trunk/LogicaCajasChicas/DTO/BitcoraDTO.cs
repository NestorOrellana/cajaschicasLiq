using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DipCmiGT.LogicaCajasChicas;

namespace DipCmiGT.LogicaCajasChicas
{
    public class BitacoraDTO
    {
        public Int32 ID_BITACORA { get; set; }
        public string USUARIO { get; set; }
        public string ID_DOMINIO { get; set; }
        public string IP { get; set; }
        public string FECHA_INGRESO { get; set; }
        public string HORA_INGRESO { get; set; }
    }
}
