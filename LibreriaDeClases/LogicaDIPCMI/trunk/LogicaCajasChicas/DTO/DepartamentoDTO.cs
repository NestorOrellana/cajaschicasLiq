using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DipCmiGT.LogicaCajasChicas
{
    public class DepartamentoDTO
    {
        public Int16 ID_DEPARTAMENTO { get; set; }
        public string NOMBRE { get; set; }
        public UsuarioMantenimientoDTO USUARIO_MANTENIMIENTO { get; set; }
    }
}
