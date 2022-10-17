using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DipCmiGT.LogicaCajasChicas
{
    public class NivelLiquidacionDTO
    {
        public int CodigoNivel { get; set; }
        public string Pais { get; set; }
        public string Nivel { get; set; }
        public bool Alta { get; set; }
        public UsuarioMantenimientoDTO USUARIO_MANTENIMIENTO { get; set; }

         public NivelLiquidacionDTO()
        {
            USUARIO_MANTENIMIENTO = new UsuarioMantenimientoDTO();
        }
    }
}
