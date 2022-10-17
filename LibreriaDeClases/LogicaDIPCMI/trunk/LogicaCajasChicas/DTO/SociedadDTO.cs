using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DipCmiGT.LogicaCajasChicas
{
    public class SociedadDTO
    {
        public SociedadDTO()
        {
            USUARIO_MANTENIMIENTO = new UsuarioMantenimientoDTO();
        }

        public string CODIGO_SOCIEDAD { get; set; }
        public string NOMBRE { get; set; }
        public Int16 MESES_FACTURA { get; set; }
        public string PAIS { get; set; }
        public string MONEDA { get; set; }
        public double MONTO_COMPRA_CC { get; set; }
        public Int32 TOLERANCIA_COMPRA_CC { get; set; }
        public bool ALTA { get; set; }
        public string CUENTA_PROVEEDOR { get; set; }
        public string MANDANTE { get; set; }
        public UsuarioMantenimientoDTO USUARIO_MANTENIMIENTO { get; set; }
    }
}
