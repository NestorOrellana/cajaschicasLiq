using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DipCmiGT.LogicaComun;

namespace DipCmiGT.LogicaCajasChicas
{
    public class UsuarioCajaDTO
    {
        public string BUKRS { get; set; }
        public string LIFNR { get; set; }
        public string NAME  { get; set; }
        public bool? ESTADO { get; set; }
    }
}
