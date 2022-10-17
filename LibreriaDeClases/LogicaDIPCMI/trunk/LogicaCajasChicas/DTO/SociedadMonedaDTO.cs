using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DipCmiGT.LogicaCajasChicas;

namespace LogicaCajasChicas
{
    public class SociedadMonedaDTO
    {
        public SociedadMonedaDTO()
        {
            USUARIO_MANTENIMIENTO = new UsuarioMantenimientoDTO();
        }

        public Int16 ID_SOCIEDAD_MONEDA { get; set; }
        public string CODIGO_SOCIEDAD { get; set; }
        public string NOMBRE_SOCIEDAD { get; set; }
        public string MONEDA { get; set; }
        public bool ESTADO { get; set; }
        public UsuarioMantenimientoDTO USUARIO_MANTENIMIENTO { get; set; }
    }
}
