using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LogicaCajasChicas.Enum
{
    public enum TipoImpuestosEnum
    {
        ISR = 0,
        ISR_SERVICIO = 1,
        //ISR_COMPRA = 2,
        IVA_CREDITO = 50,
        IVA_FACTURA_ESPECIAL = 51,

        IMPUESTOS_VARIOS = 96,
        RETENCION_IVA = 97,
        RETENCION_ISR = 98,
        ISR_FACTURA_ESPECIAL = 99
    }
}
