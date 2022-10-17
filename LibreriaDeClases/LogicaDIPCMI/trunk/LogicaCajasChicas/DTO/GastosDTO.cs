using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DipCmiGT.LogicaCajasChicas
{
    public class GastosDTO
    {
        public int CodigoGasto { get; set; }
        public string Pais { get; set; }
        public string Gasto { get; set; }
        public bool Kilometraje { get; set; }
        public bool Alta { get; set; }
        public UsuarioMantenimientoDTO USUARIO_MANTENIMIENTO { get; set; }

         public GastosDTO()
        {
            USUARIO_MANTENIMIENTO = new UsuarioMantenimientoDTO();
        }
    }
}
