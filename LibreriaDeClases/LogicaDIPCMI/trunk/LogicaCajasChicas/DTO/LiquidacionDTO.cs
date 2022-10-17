using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DipCmiGT.LogicaCajasChicas
{
    public class LiquidacionDTO
    {
        public int CodigoLiquidacion { get; set; }
        public string Pais { get; set; }
        public int Nivel { get; set; }
        public string DescripcionNivel { get; set; }
        public int TipoGasto { get; set; }
        public string DescripcionTipoGasto { get; set; }
        public decimal MontoAutorizado { get; set; }
        public bool Alta { get; set; }
        public UsuarioMantenimientoDTO USUARIO_MANTENIMIENTO { get; set; }

        public LiquidacionDTO()
        {
            USUARIO_MANTENIMIENTO = new UsuarioMantenimientoDTO();
        }
    }
}
