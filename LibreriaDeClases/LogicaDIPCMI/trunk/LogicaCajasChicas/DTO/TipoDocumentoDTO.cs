using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DipCmiGT.LogicaCajasChicas;

namespace LogicaCajasChicas
{
    public class TipoDocumentoDTO
    {
        public TipoDocumentoDTO()
        {
            USUARIO_MANTENIMIENTO = new UsuarioMantenimientoDTO();
        }

        public Int16 ID_TIPO_DOCUMENTO { get; set; }
        public string TIPO_DOCUMENTO { get; set; }
        public string DESCRIPCION { get; set; }
        public bool ALTA { get; set; }
        public UsuarioMantenimientoDTO USUARIO_MANTENIMIENTO { get; set; }
    }
}
