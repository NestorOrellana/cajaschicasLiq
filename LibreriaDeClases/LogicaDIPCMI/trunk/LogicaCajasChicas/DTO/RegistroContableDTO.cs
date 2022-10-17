using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LogicaCajasChicas
{
    public class RegistroContableDTO
    {
        public decimal ID_REGISTRO_CONTABLE { get; set; }
        public Int16 CORRELATIVO { get; set; }
        public string CUENTA_CONTABLE { get; set; }
        public string DEFINICION_CUENTA_CONTABLE { get; set; }
        public Int16 CARGO_ABONO { get; set; }
        public double VALOR { get; set; }
        public string INDICADOR_IVA { get; set; }
        public bool ALTA { get; set; }
        public DateTime FECHA_ALTA { get; set; }
        public DateTime? FECHA_MODIFICACION { get; set; }
        public decimal ID_FACTURA { get; set; }
    }


    public class RegistroContableSPDTO
    {
        public string CUENTA_CONTABLE { get; set; }
        public string INDICADOR_IVA { get; set; }
        public string DEFINICION_CUENTA_CONTABLE { get; set; }
        public Int16 CARGO_ABONO { get; set; }
        public double CARGO { get; set; }
        public double ABONO { get; set; }
        public string SERIE { get; set; }
        //public decimal NUMERO { get; set; }
        public string NUMERO { get; set; }
        public string DOCUMENTO_IDENTIFICACION { get; set; }
        public string NUMERO_IDENTIFICACION { get; set; }
        public string NOMBRE_PROVEEDOR { get; set; }
        public string DIRECCION_PROVEEDOR { get; set; }
        public string CODIGO_CC { get; set; }
        public DateTime FECHA_FACTURA { get; set; }
        public double SUMA_CARGO { get; set; }
        public double SUMA_ABONO { get; set; }
        public decimal ID_FACTURA { get; set; }
        public string DESCRIPCION { get; set; }
        public string OBSERVACIONES { get; set; }
    }




}
