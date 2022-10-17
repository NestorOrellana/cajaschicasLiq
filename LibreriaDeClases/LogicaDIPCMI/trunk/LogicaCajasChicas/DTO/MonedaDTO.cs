using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DipCmiGT.LogicaCajasChicas;

namespace LogicaCajasChicas
{
    public class MonedaDTO
    {
        public MonedaDTO()
        {
            USUARIO_MANTENIMIENTO = new UsuarioMantenimientoDTO();
        }

        public string MONEDA { get; set; }
        public string DESCRIPCION { get; set; }
        public bool ESTADO { get; set; }
        public UsuarioMantenimientoDTO USUARIO_MANTENIMIENTO { get; set; }
    }
}
