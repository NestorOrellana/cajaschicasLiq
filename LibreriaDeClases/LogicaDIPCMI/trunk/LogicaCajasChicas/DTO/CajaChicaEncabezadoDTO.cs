using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DipCmiGT.LogicaCajasChicas
{
    public class CajaChicaEncabezadoDTO
    {
        public CajaChicaEncabezadoDTO()
        {
            USUARIO_MANTENIMIENTO = new UsuarioMantenimientoDTO();
        }

        public decimal ID_CAJA_CHICA { get; set; }
        public Int32 ID_SOCIEDAD_CENTRO { get; set; }
        public Int32 CORRELATIVO { get; set; }
        public string CAJA_CHICA_SAP { get; set; }
        public string DESCRIPCION { get; set; }
        public Int16? ESTADO { get; set; }
        public string ESTADO_DESC { get; set; }
        public string CODIGO_SOCIEDAD { get; set; }
        public string NOMBRE_EMPRESA { get; set; }
        public Int16 CODIGO_CENTRO { get; set; }
        public string NOMBRE_CENTRO { get; set; }
        public Int32 FACTURAS_CC { get; set; }
        public double MONTO_CC { get; set; }
        public string CODIGO_CC { get; set; }
        public string TIPO_OPERACION { get; set; }
        public string NOMBRE_CC { get; set; }
        public Int16? ID_SOCIEDAD_MONEDA { get; set; }
        public string MONEDA { get; set; }
        public string PAIS { get; set; }

        public UsuarioMantenimientoDTO USUARIO_MANTENIMIENTO { get; set; }
    }
}
