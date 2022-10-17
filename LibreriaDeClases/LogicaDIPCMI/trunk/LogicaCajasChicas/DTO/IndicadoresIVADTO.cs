using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LogicaCajasChicas
{
    [Serializable]
    public class IndicadoresIVADTO
    {
        public string INDICADOR_IVA { get; set; }
        public string DESCRIPCION { get; set; }
        public double IMPORTE { get; set; }
        public bool ACTIVO { get; set; }
    }
}
