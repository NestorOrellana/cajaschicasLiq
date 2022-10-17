using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DipCmiGT.LogicaCajasChicas
{
    public class ArticuloDTO
    {

        public string ID_ARTICULO { get; set; }
        public string NOMBRE { get; set; }
        public bool ALTA { get; set; }
        public UsuarioMantenimientoDTO USUARIO_MANTENIMIENTO { get; set; }
    }
}
