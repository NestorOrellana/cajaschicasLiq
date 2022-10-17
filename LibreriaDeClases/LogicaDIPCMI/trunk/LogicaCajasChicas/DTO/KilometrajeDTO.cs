using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DipCmiGT.LogicaCajasChicas
{
    public class KilometrajeDTO
    {
        public long CodigoKilometraje { get; set; }
        public string Pais { get; set; }
        public string Origen { get; set; }
        public string Destino { get; set; }
        public decimal Kilometros { get; set; }
        public bool Alta { get; set; }
        public UsuarioMantenimientoDTO USUARIO_MANTENIMIENTO { get; set; }

         public KilometrajeDTO()
        {
            USUARIO_MANTENIMIENTO = new UsuarioMantenimientoDTO();
        }
    }
}
