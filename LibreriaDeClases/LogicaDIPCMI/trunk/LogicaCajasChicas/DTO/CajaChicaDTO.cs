using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DipCmiGT.LogicaCajasChicas;

namespace DipCmiGT.LogicaCajasChicas
{
    public class CajaChicaDTO
    {
        public CajaChicaDTO()
        {
            USUARIO_MANTENIMIENTO = new UsuarioMantenimientoDTO();
            CAJA_ENCABEZADO = new CajaChicaEncabezadoDTO();
        }

        public decimal ID_CAJA_CHICA { get; set; }
        public string CODIGO_SOCIEDAD { get; set; }
        public Int16 ID_CENTRO { get; set; }
        public Int32 CORRELATIVO { get; set; }
        public string NUMERO_CAJA_CHICA { get; set; }
        public string DESCRIPCION { get; set; }
        public bool? ALTA { get; set; }
        public UsuarioMantenimientoDTO USUARIO_MANTENIMIENTO { get; set; }
        public CajaChicaEncabezadoDTO CAJA_ENCABEZADO { get; set; }

    }
}
