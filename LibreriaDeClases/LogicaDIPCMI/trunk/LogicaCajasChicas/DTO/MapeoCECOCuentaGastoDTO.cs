using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DipCmiGT.LogicaCajasChicas
{
    public class MapeoCECOCuentaGastoDTO
    {
        public long CodigoMapeo { get; set; }
        public string Pais { get; set; }
        public string CentroCosto { get; set; }
        public string CentroCostoStr { get; set; }
        public string OrdenCosto { get; set; }
        public string OrdenCostoStr { get; set; }
        public int TipoGasto { get; set; }
        public string TipoGastoStr { get; set; }
        public string CuentaContable { get; set; }
        public string CuentaContableStr { get; set; }
        public bool Alta { get; set; }
        public UsuarioMantenimientoDTO USUARIO_MANTENIMIENTO { get; set; }

        public MapeoCECOCuentaGastoDTO()
        {
            USUARIO_MANTENIMIENTO = new UsuarioMantenimientoDTO();
        }
    }
}
