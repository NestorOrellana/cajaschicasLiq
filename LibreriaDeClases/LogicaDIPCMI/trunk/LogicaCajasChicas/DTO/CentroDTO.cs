using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DipCmiGT.LogicaComun;

namespace DipCmiGT.LogicaCajasChicas
{
    public class CentroDTO
    {
        public CentroDTO()
        {
            USUARIO_MANTENIMIENTO = new UsuarioMantenimientoDTO();
        }

        public Int32 ID_CENTRO { get; set; }
        public string NOMBRE { get; set; }
        public bool ALTA { get; set; }
        public UsuarioMantenimientoDTO USUARIO_MANTENIMIENTO { get; set; }
    }
}
