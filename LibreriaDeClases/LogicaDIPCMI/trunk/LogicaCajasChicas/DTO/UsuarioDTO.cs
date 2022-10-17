using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DipCmiGT.LogicaCajasChicas;

namespace DipCmiGT.LogicaCajasChicas
{
    public class UsuarioDTO
    {
        public UsuarioDTO()
        {
            LISTA_ROL = new List<RolDTO>();
            USUARIO_MANTENIMIENTO = new UsuarioMantenimientoDTO();
        }
        public Int16 ID_USUARIO { get; set; }
        public string USUARIO { get; set; }
        public string NOMBRE { get; set; }
        public bool? ALTA { get; set; }
        public string CORREO { get; set; }
        public UsuarioMantenimientoDTO USUARIO_MANTENIMIENTO { get; set; }
        public List<RolDTO> LISTA_ROL { get; set; }
        public string DOMINIO { get; set; }
        public string IDENTIFICADOR { get; set; }
    }
}
