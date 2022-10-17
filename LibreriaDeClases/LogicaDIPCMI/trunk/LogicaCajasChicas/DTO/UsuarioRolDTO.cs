using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DipCmiGT.LogicaCajasChicas
{
    public class UsuarioRolDTO
    {

        public UsuarioRolDTO()
        {
            USUARIO_MANTENIMIENTO = new UsuarioMantenimientoDTO();
        }
        public Int32 ID_USUARIO { get; set; }
        public Int16 ID_ROL { get; set; }
        public bool ALTA { get; set; }

        public string NOMBRE_USUARIO { get; set; }
        public string NOMBRE_ROL { get; set; }
        public UsuarioMantenimientoDTO USUARIO_MANTENIMIENTO { get; set; }
    

    }
}
