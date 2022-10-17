using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DipCmiGT.LogicaCajasChicas
{
    public class FacturaEncabezadoDTO
    {
        public FacturaEncabezadoDTO()
        {
            FACTURA_DETALLE = new List<FacturaDetalleDTO>();
            USUARIO_MANTENIMIENTO = new UsuarioMantenimientoDTO();
            CAJA_CHICA = new CajaChicaEncabezadoDTO();
            DETALLE_APROBACION = new AprobadorFacturaDetalleDTO();
        }

        public decimal ID_FACTURA { get; set; }
        public Int16 ID_TIPO_DOCUMENTO { get; set; }
        public string CENTRO_COSTO { get; set; }
        public string ORDEN_COSTO { get; set; }
        public string TIPO_DOCUMENTO { get; set; }
        public string NUMERO_IDENTIFICACION { get; set; }
        public Int16 ID_TIPO_DOCUMENTO2 { get; set; }
        public string TIPO_DOCUMENTO2 { get; set; }
        public string NUMERO_IDENTIFICACION2 { get; set; }
        public string NOMBRE_PROVEEDOR { get; set; }
        public string SERIE { get; set; }
        //public decimal NUMERO { get; set; }
        public string NUMERO { get; set; }
        public DateTime FECHA_FACTURA { get; set; }
        public bool ES_ESPECIAL { get; set; }
        public bool RETENCION_IVA { get; set; }
        public bool RETENCION_ISR { get; set; }
        public double IMPUESTO { get; set; }
        public double IVA { get; set; }
        public double VALOR_TOTAL { get; set; }
        public Int16? ESTADO { get; set; }
        public UsuarioMantenimientoDTO USUARIO_MANTENIMIENTO { get; set; }
        public Int32 ID_PROVEEDOR { get; set; }
        public decimal ID_CAJA_CHICA { get; set; }
        public decimal? NUMERO_RETENCION_IVA { get; set; }
        public decimal? NUMERO_RETENCION_ISR { get; set; }
        public double? VALOR_RETENCION_IVA { get; set; }
        public double? VALOR_RETENCION_ISR { get; set; }
        public bool? APROBADA { get; set; }
        public string TIPO_FACTURA { get; set; }
        public Int16 NIVEL { get; set; }
        public string CODIGO_SOCIEDAD { get; set; }
        public string NOMBRE_CENTRO_COSTO { get; set; }
        public string NOMBRE_ORDEN_COSTO { get; set; }
        public string DESCRIPCION { get; set; }
        public string MONEDA { get; set; }
        public bool? FACTURA_DIVIDIDA { get; set; }
        public double? TOTALFACTURADIVIDIDA { get; set; }
        public decimal? TOTALFACTURADIVIDADISR { get; set; }
        public string OBSERVACIONES { get; set; }
        public decimal ACUMULADO { get; set; }
        public bool? RETENCION { get; set; }
        public bool? TIPO { get; set; }
        public double? VALOR_REAL_FACT { get; set; }
        public double? TIPO_CAMBIO { get; set; }
        public bool? RETSERVICIOS { get; set; }
        public string PAIS { get; set; }
        public string MANDANTE { get; set; }
        public string INDICADOR { get; set; }
        public string EstadoINELDAT { get; set; }

        public List<FacturaDetalleDTO> FACTURA_DETALLE { get; set; }
        public CajaChicaEncabezadoDTO CAJA_CHICA { get; set; }
        public AprobadorFacturaDetalleDTO DETALLE_APROBACION { get; set; }
    }
}