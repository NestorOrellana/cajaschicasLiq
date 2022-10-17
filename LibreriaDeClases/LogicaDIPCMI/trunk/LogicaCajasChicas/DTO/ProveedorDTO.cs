using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DipCmiGT.LogicaCajasChicas
{
    public class ProveedorDTO
    {
        public ProveedorDTO()
        {
            USUARIO_MANTENIMIENTO = new UsuarioMantenimientoDTO();
        }

        public Int32 ID_PROVEEDOR { get; set; }
        public Int16 ID_TIPO_DOCUMENTO { get; set; }
        public string TIPO_DOCUMENTO { get; set; }
        public string NUMERO_IDENTIFICACION { get; set; }
        public string TIPO_DOCUMENTO2 { get; set; }
        public Int16 ID_TIPO_DOCUMENTO2 { get; set; }
        public string NUMERO_IDENTIFICACION2 { get; set; }
        public string NOMBRE { get; set; }
        public string DIRECCION { get; set; }
        public string REGIMEN { get; set; }
        public bool ES_PEQUEÑO_CONTRIBUYENTE { get; set; }
        public bool? ALTA { get; set; }
        public bool? TIPO { get; set; }
        public UsuarioMantenimientoDTO USUARIO_MANTENIMIENTO { get; set; }
    }
}
