using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DipCmiGT.LogicaCajasChicas
{
    public class FacturaDetalleDTO
    {
        public decimal ID_FACTURA_DETALLE { get; set; }
        public Int32 NUMERO { get; set; }
        public double CANTIDAD { get; set; }
        public double VALOR { get; set; }
        public double IVA { get; set; }
        public string CUENTA_CONTABLE { get; set; }
        public string DEFINICION_CC { get; set; }
        public Int16 CARGO_ABONO { get; set; }
        public string IDENTIFICADOR_IVA { get; set; }
        public string DEFINICION_TIPO_IVA { get; set; }
        public string DESCRIPCION { get; set; }
        public double IMPUESTO { get; set; }
        public double IMPORTE { get; set; }
        public DateTime? FECHA_MODIFICACION { get; set; }
        public bool ALTA { get; set; }
        public string DETALLE { get; set; }

        public decimal ID_FACTURA { get; set; }
    }
}
