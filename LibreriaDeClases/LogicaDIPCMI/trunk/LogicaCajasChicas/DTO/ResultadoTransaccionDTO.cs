using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LogicaCajasChicas
{
    [Serializable]
    public class ResultadoTransaccionDTO
    {
        public decimal CODIGO { get; set; }
        public string MENSAJE { get; set; }
    }
}
