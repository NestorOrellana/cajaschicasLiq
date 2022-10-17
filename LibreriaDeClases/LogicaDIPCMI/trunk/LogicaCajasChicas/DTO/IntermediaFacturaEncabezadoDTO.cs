using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LogicaCajasChicas
{
    public class IntermediaFacturaEncabezadoDTO
    {
        public IntermediaFacturaEncabezadoDTO()
        {
            INTERMEDIA_FACTURA_DETALLE = new List<IntermediaFacturaDetalleDTO>();
        }

        public string BUKRS { get; set; }
        public Int64 DOCUMENT { get; set; }
        public string BLDAT { get; set; }
        public string TYPE { get; set; }
        public string BUDAT { get; set; }
        public string XBLNR { get; set; }
        public string BKTXT { get; set; }
        public string BLART { get; set; }
        public string CURRENCY { get; set; }
        public decimal KURSF { get; set; }
        public string RECORDMODE { get; set; }
        public string NAME { get; set; }
        public string NAME2 { get; set; }
        public string NAME3 { get; set; }
        public string NAME4 { get; set; }

        public string ORT01 { get; set; }
        public string STCD1 { get; set; }
        public string STCD2 { get; set; }
        public string DUMMY { get; set; }
        public string ZSTCDT { get; set; }

        public List<IntermediaFacturaDetalleDTO> INTERMEDIA_FACTURA_DETALLE { get; set; }
    }
}
