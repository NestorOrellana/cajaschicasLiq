using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DipCmiGT.LogicaCajasChicas
{
    public class CuentaContableDetalleDTO
    {
        public Int32 IdCuentaContableDetalle { get; set; }
        public string SAKNR { get;set;}
        public string TXT50 { get; set; }
        public Int32 ID_CUENTA_CONTABLE { get; set; }
        public UsuarioMantenimientoDTO USUARIO_MANTENIMIENTO { get; set; }

    }
}
