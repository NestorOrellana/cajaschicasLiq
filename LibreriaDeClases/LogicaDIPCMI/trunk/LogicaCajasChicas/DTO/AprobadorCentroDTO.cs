using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DipCmiGT.LogicaCajasChicas
{
	public class AprobadorCentroDTO
	{

        public AprobadorCentroDTO()
        {
            USUARIO_MANTENIMIENTO = new UsuarioMantenimientoDTO();
        }

        public Int32 ID_APROBADORCENTRO { get; set; }
        public Int32 ID_USUARIO { get; set; }
        public string KOSTL { get; set; }
        public string AUFNR { get; set; }
        public Int32 ID_SOCIEDAD_CENTRO { get; set; }
        public string CENTRO { get; set; }
        public Int16 ID_NIVEL { get; set; }
        public string NIVEL { get; set; }
        public bool ALTA { get; set; }
        public string SOCIEDAD { get; set; }
        public string NOMBRE { get; set; }

        public UsuarioMantenimientoDTO USUARIO_MANTENIMIENTO { get; set; } 

	}
}
