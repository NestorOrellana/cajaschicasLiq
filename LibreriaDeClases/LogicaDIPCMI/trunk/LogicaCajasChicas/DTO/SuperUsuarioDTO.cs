using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DipCmiGT.LogicaComun;

namespace DipCmiGT.LogicaCajasChicas
{
    public class SuperUsuarioDTO
    {
        public string NoFactura { get; set; }
        public string NombreProveedor { get; set; }
        public decimal IdFactura { get; set; }
        public string EstadoActual { get; set; }
        public Int16 IdEstadoActual { get; set; }
        public double Total { get; set; }
        public bool CodDividida { get; set; }
        public string Dividida { get; set; }
        public string Fecha { get; set; }
        public decimal IDCCFactura { get; set; }
        public string CCFactura { get; set; }
        public Int16 IdNuevoEstado { get; set; }
        public Int16 IdEstadoCC { get; set; }
        public decimal IdCC { get; set; }
        public string NombreCC {get; set; }
        public string IdEstadoSicnro { get; set; }
        public string DocIdentificacion { get; set; }
        public long CodigoSincronizacion { get; set; }
        public string Serie { get; set; }
    }
}
