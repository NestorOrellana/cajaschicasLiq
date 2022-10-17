using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DipCmiGT.LogicaCajasChicas;

namespace DipCmiGT.LogicaCajasChicas
{
    public class UsuarioCentroCostoDTO
    {
        public UsuarioCentroCostoDTO()
        {
            USUARIO_MANTENIMIENTO = new UsuarioMantenimientoDTO();
        }

        public Int32 ID_USUARIO_CENTRO_COSTO { get; set; }
        public string USUARIO { get; set; }
        public string CENTRO_COSTO { get; set; }
        public Int32 ID_SOCIEDAD_CENTRO { get; set; }
        public bool ALTA { get; set; }
        public string CODIGO_SOCIEDAD { get; set; }
        public string NOMBRE_SOCIEDAD { get; set; }
        public Int32 ID_CENTRO { get; set; }
        public string NOMBRE_CENTRO { get; set; }
        public string VERAK { get; set; }
        public string KTEXT { get; set; }
        public string CENTRO_COSTO_DESCRIPCION { get; set; }

        public UsuarioMantenimientoDTO USUARIO_MANTENIMIENTO { get; set; }

    }
}
