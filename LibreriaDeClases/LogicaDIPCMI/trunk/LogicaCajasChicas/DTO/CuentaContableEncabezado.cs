using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DipCmiGT.LogicaCajasChicas
{
    public class CuentaContableEncabezado
    {
        public Int32 ID_CUENTA_CONTABLE { get; set; }
        public string BUKR { get; set; }
        public string KTOK { get; set; }
        public string TXT30 { get; set; }
        public UsuarioMantenimientoDTO USUARIO_MANTENIMIENTO { get; set; }


    }
}
