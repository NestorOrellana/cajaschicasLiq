using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DipCmiGT.LogicaCajasChicas
{
    public class UsuarioMantenimientoDTO
    {
        public string USUARIO_ALTA { get; set; }
        public DateTime FECHA_ALTA { get; set; }
        public string USUARIO_MODIFICO { get; set; }
        public DateTime? FECHA_MODIFICACION { get; set; }
    }
}
