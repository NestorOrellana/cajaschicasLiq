using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DipCmiGT.LogicaCajasChicas
{
    public class ImpuestoCHDTO
    {
        public long CodigoImpuesto { get; set; }
        public string Pais { get; set; }
        public string Tipo { get; set; }
        public string Descripcion { get; set; }
        public decimal Valor { get; set; }
        public string Cuenta { get; set; }
        public int OrdenVisualizacion { get; set; }
        public bool Alta { get; set; }
        public UsuarioMantenimientoDTO USUARIO_MANTENIMIENTO { get; set; }

        public ImpuestoCHDTO()
        {
            USUARIO_MANTENIMIENTO = new UsuarioMantenimientoDTO();
        }
    }
}
