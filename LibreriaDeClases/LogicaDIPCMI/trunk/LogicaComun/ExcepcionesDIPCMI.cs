using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DipCmiGT.LogicaComun
{
    public class ExcepcionesDIPCMI : Exception
    {
        public string DETALLE { get; internal set; }

        public ExcepcionesDIPCMI(string mensaje, Exception ex)
            : base(mensaje, ex)
        {
            if (ex != null)
                this.DETALLE = ex.Message;
        }

        public ExcepcionesDIPCMI(string mensaje)
            : this(mensaje, null)
        {
        }
    }
}
