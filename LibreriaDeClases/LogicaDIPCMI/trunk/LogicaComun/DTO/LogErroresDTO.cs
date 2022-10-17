using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DipCmiGT.LogicaComun
{
    public class LogErroresDTO
    {
        public int ID_LOG_ERRORES { get; set; }
        public string DESCRIPCION { get; set; }
        public string USUARIO { get; set; }
        public string FUNCION { get; set; }
        public DateTime FECHA_EVENTO { get; set; }
    }
}
