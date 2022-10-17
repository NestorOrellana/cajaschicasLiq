using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DipCmiGT.LogicaCajasChicas
{
    public class MapeoRegistradorAprobadorDTO
    {
        public string CECO_ORDEN { get; set; }
        public string ID_CENTRO { get; set; }
        public string CENTRO { get; set; }
        public string APROBADOR_US { get; set; }
        public string APROBADOR { get; set; }
        public string REGISTRADOR_US { get; set; }
        public string REGISTRADOR { get; set; }
        public string USUARIO_ALTA { get; set; }
        public bool ESTADO { get; set; }
    }
}
