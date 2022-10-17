using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DipCmiGT.LogicaCajasChicas
{
    public class SociedadCentroDTO
    {

         public SociedadCentroDTO()
        {
            USUARIO_MANTENIMIENTO = new UsuarioMantenimientoDTO();
        }

        public Int32 ID_SOCIEDAD_CENTRO { get; set; }
        public string CODIGO_SOCIEDAD { get; set; }
        public Int32 ID_CENTRO { get; set; }
        public string NOMBRE_SOCIEDAD { get; set; }
        public string NOMBRE_CENTRO { get; set; }
        public bool ALTA { get; set; }
        public UsuarioMantenimientoDTO USUARIO_MANTENIMIENTO { get; set; }
    }
}
