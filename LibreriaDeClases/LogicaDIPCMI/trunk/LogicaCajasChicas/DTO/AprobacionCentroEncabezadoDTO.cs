using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DipCmiGT.LogicaCajasChicas
{
    public class AprobadorCentroEncabezadoDTO
    {

        public AprobadorCentroEncabezadoDTO()
        {
            USUARIO_MANTENIMIENTO = new UsuarioMantenimientoDTO();
        }

        public Int32 ID_APROBACION_ENCABEZADO { get; set; }
        public Int32 ID_USUARIO { get; set; }
        public string USUARIO { get; set; }
        public Int16 PORCENTAJE_COMPRA { get; set; }
        public Int32 TOLERANCIA { get; set; }
        public bool ALTA { get; set; }
        public UsuarioMantenimientoDTO USUARIO_MANTENIMIENTO { get; set; }

    }
}
