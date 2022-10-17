using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DipCmiGT.LogicaCajasChicas
{
    public class AprobacionFacturasDTO
    {
        public AprobacionFacturasDTO()
        {
            USUARIO_MANTENIMIENTO = new UsuarioMantenimientoDTO();
        }

        public Int32 ID_APROBACION_FACTURAS { get; set; }
        public Int32 ID_FACTURA { get; set; }
        public Int32 ID_APROBACION_CENTRO { get; set; }
        public Int16 ESTADO { get; set; }
        public string MONEDA { get; set; }

        public UsuarioMantenimientoDTO USUARIO_MANTENIMIENTO { get; set; }
    }
}
