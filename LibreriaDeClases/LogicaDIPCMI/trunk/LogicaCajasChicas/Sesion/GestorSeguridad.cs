using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using LogicaComun.Enum;
using DipCmiGT.LogicaCajasChicas.Entidad;
using DipCmiGT.LogicaComun;
using DipCmiGT.LogicaComun.Util;
using System.Transactions;
using LogicaCajasChicas;
using LogicaCajasChicas.Sesion;
using LogicaCajasChicas.Entidad;


namespace DipCmiGT.LogicaCajasChicas.Sesion
{
    public class GestorSeguridad : IDisposable
    {
        #region Declaracion

        Usuario _usuario = null;
        Rol _rol = null;
        UsuarioRol _usuarioRol = null;
        UsuarioCuentaGasto _usuarioCuenta = null;
        UsuarioCajaChica _usuarioCaja = null;
        SuperUsuario _superUsuario = null;
        Centro _centro = null;
        UsuarioCentroCosto _usuarioCentroCosto = null;
        UsuarioSociedadCentro _usuarioSociedadCentro = null;
        

        SqlConnection cnnSql = null;

        #endregion

        #region Constructor

        public GestorSeguridad(string conexion)
        {
            cnnSql = new SqlConnection(conexion);
        }

        #endregion

        #region Rol

        public RolDTO EjecutarSentenciaSelect(Int32 idRol)
        {
            if (_rol == null) _rol = new Rol(cnnSql);

            try
            {
                cnnSql.Open();

                return _rol.EjecutarSentenciaSelect(idRol);
            }
            finally
            {
                if (cnnSql.State == ConnectionState.Open) cnnSql.Close();
            }
        }

        /// <summary>
        /// Método que almacena o actualiza un proveedor.
        /// </summary>
        /// <param name="rolDTO">ProveedorDTO</param>
        /// <returns>ProveedorDTO</returns>
        public RolDTO AlmacenarRol(RolDTO rolDTO)
        {
            Int32 x = 0;
            if (_rol == null) _rol = new Rol(cnnSql);

            try
            {
                cnnSql.Open();

                if (rolDTO.ID_ROL == 0)
                {
                    rolDTO.ID_ROL = _rol.EjecutarSenteciaInsert(rolDTO);
                }
                else
                    x = _rol.EjecutarSentenciaUpdate(rolDTO);
            }
            finally
            {
                if (cnnSql.State == ConnectionState.Open) cnnSql.Close();
            }

            return rolDTO;
        }

        /// <summary>
        /// Función que lista todos los proveedores registrados.
        /// </summary>
        /// <returns>Lista ProveedorDTO</returns>
        public List<RolDTO> ListaRol()
        {
            if (_rol == null) _rol = new Rol(cnnSql);
            List<RolDTO> _rolDto = new List<RolDTO>();

            try
            {
                cnnSql.Open();

                return _rol.ListaRoles();
            }
            finally
            {
                if (cnnSql.State != ConnectionState.Closed) cnnSql.Close();
            }
        }

        /// <summary>
        /// Método que da baja al proveedor.
        /// </summary>
        /// <param name="idProveedor">Número de identificación</param>
        /// <param name="usuario">Usuario da baja</param>
        /// <returns>Registro actualizado</returns>
        public Int16 DarBajaRol(Int32 idRol, string usuario)
        {
            if (_rol == null) _rol = new Rol(cnnSql);

            try
            {
                cnnSql.Open();

                return _rol.DarBajaRol(idRol, usuario);
            }
            finally
            {
                if (cnnSql.State != ConnectionState.Closed) cnnSql.Close();
            }
        }

        /// <summary>
        /// Método que da alta al proveedor.
        /// </summary>
        /// <param name="idProveedor">Número de identicicación</param>
        /// <param name="usuario">Usuario que da alta</param>
        /// <returns>Registro actualizado</returns>
        public Int16 DarAltaRol(Int32 idRol, string usuario)
        {
            if (_rol == null) _rol = new Rol(cnnSql);

            try
            {
                cnnSql.Open();

                return _rol.DarAltaRol(idRol, usuario);
            }
            finally
            {
                if (cnnSql.State != ConnectionState.Closed) cnnSql.Close();
            }
        }

        #endregion

        #region Usuario

        public UsuarioDTO SeleccionarUsuario(Int32 idUsuario)
        {
            if (_usuario == null) _usuario = new Usuario(cnnSql);

            try
            {
                cnnSql.Open();

                return _usuario.EjecutarSentenciaSelect(idUsuario);
            }
            finally
            {
                if (cnnSql.State == ConnectionState.Open) cnnSql.Close();
            }
        }

        /// <summary>
        /// Método que almacena o actualiza un proveedor.
        /// </summary>
        /// <param name="rolDTO">ProveedorDTO</param>
        /// <returns>ProveedorDTO</returns>
        public UsuarioDTO AlmacenarRol(UsuarioDTO usuarioDTO)
        {
            if (_usuario == null) _usuario = new Usuario(cnnSql);

            try
            {
                cnnSql.Open();

                if (usuarioDTO.ID_USUARIO == 0)
                {
                    usuarioDTO.ID_USUARIO = _usuario.EjecutarSenteciaInsert(usuarioDTO);
                }

            }
            finally
            {
                if (cnnSql.State == ConnectionState.Open) cnnSql.Close();
            }

            return usuarioDTO;
        }
        public UsuarioDTO AlmacenarUsuario(UsuarioDTO usuarioDTO)
        {
            Int32 x = 0;
            if (_usuario == null) _usuario = new Usuario(cnnSql);
            try
            {
                cnnSql.Open();

                if (usuarioDTO.ID_USUARIO == 0)
                {
                    if (!_usuario.ExisteUsuario(usuarioDTO.USUARIO, usuarioDTO.IDENTIFICADOR))
                        throw new ExcepcionesDIPCMI("El Usuario ya se encuentra registrado");
                    else
                        usuarioDTO.ID_USUARIO = _usuario.EjecutarSenteciaInsert(usuarioDTO);
                }
                else
                    x = _usuario.EjecutarSentenciaUpdate(usuarioDTO);
            }
            finally
            {
                if (cnnSql.State == ConnectionState.Open) cnnSql.Close();
            }

            return usuarioDTO;
        }

        /// <summary>
        /// Función que lista todos los proveedores registrados.
        /// </summary>
        /// <returns>Lista ProveedorDTO</returns>
        public List<UsuarioDTO> ListaUsuario()
        {
            if (_usuario == null) _usuario = new Usuario(cnnSql);
            List<UsuarioDTO> _usuarioDto = new List<UsuarioDTO>();

            try
            {
                cnnSql.Open();

                return _usuario.ListaUsuarios();
            }
            finally
            {
                if (cnnSql.State != ConnectionState.Closed) cnnSql.Close();
            }
        }

        public List<UsuarioDTO> BuscarUsuario(string usuario, string nombre, string dominio)
        {
            if (_usuario == null) _usuario = new Usuario(cnnSql);
            List<UsuarioDTO> _usuarioDto = new List<UsuarioDTO>();

            try
            {
                cnnSql.Open();

                return _usuario.BuscarUsuario(usuario, nombre, dominio);
            }
            finally
            {
                if (cnnSql.State != ConnectionState.Closed) cnnSql.Close();
            }
        }

        public string[] BuscarRolUsuario(string usuario)
        {
            if (_usuarioRol == null) _usuarioRol = new UsuarioRol(cnnSql);

            try
            {
                cnnSql.Open();

                return _usuarioRol.BuscarRolesUsuarios(usuario);
            }
            finally
            {
                if (cnnSql.State != ConnectionState.Closed) cnnSql.Close();
            }
        }

        /// <summary>
        /// Método que da baja al proveedor.
        /// </summary>
        /// <param name="idProveedor">Número de identificación</param>
        /// <param name="usuario">Usuario da baja</param>
        /// <returns>Registro actualizado</returns>
        public Int16 DarBajaUsuario(Int32 idUsuario, string usuario)
        {
            if (_usuario == null) _usuario = new Usuario(cnnSql);

            try
            {
                cnnSql.Open();

                return _usuario.DarBajaUsuario(idUsuario, usuario);
            }
            finally
            {
                if (cnnSql.State != ConnectionState.Closed) cnnSql.Close();
            }
        }

        /// <summary>
        /// Método que da alta al proveedor.
        /// </summary>
        /// <param name="idProveedor">Número de identicicación</param>
        /// <param name="usuario">Usuario que da alta</param>
        /// <returns>Registro actualizado</returns>
        public Int16 DarAltaUsuario(Int32 idUsuario, string usuario)
        {
            if (_usuario == null) _usuario = new Usuario(cnnSql);

            try
            {
                cnnSql.Open();

                return _usuario.DarAltaUsuario(idUsuario, usuario);
            }
            finally
            {
                if (cnnSql.State != ConnectionState.Closed) cnnSql.Close();
            }
        }

        public bool ValidarUsuario(string usuario, string clave, string dominio)
        {
            if (_usuario == null) _usuario = new Usuario(cnnSql);
            UsuarioDTO usuarioDto = null;

            try
            {
                cnnSql.Open();

                usuarioDto = _usuario.BuscarUsuarioValidar(usuario, dominio);
            }
            finally
            {
                if (cnnSql.State != ConnectionState.Closed) cnnSql.Close();
            }

            return usuarioDto == null ? false : true;
        }

        //INI--------------------SATB-09.06.2017-------------------------
        //---Cargar DDL de Dominio para asginarselo a Usuario 
        public List<LlenarDDL_DTO> ListaDominio()
        {

            if (_usuario == null) _usuario = new Usuario(cnnSql);

            try
            {
                cnnSql.Open();
                return _usuario.ListaDominio();
            }
            finally
            {
                if (cnnSql.State != ConnectionState.Closed) cnnSql.Close();
            }
        }

        //FIN--------------------SATB-09.06.2017-------------------------
        //INI--------------------SATB-26.10.2017-------------------------
        public List<DominioDTO> BuscarDominio()
        {
            if (_usuario == null) _usuario = new Usuario(cnnSql);

            try
            {
                cnnSql.Open();

                return _usuario.SeleccionarDominio();
            }
            finally
            {
                if (cnnSql.State != ConnectionState.Closed) cnnSql.Close();
            }

        }

        public List<UsuarioDTO> BusquedaUsuario(string usuario, string nombre, string dominio)
        {
            if (_usuario == null) _usuario = new Usuario(cnnSql);

            try
            {
                cnnSql.Open();
                return _usuario.BusquedadUsuario(usuario, nombre, dominio);
            }
            finally
            {
                if (cnnSql.State != ConnectionState.Closed) cnnSql.Close();
            }
        }

        public BitacoraDTO RegistrarBitacora(BitacoraDTO bitacoraDTO)
        {
            if (_usuario == null) _usuario = new Usuario(cnnSql);
            try
            {
                cnnSql.Open();
                bitacoraDTO.ID_BITACORA = _usuario.RegistrarBitacora(bitacoraDTO);
            }
            finally
            {
                if (cnnSql.State == ConnectionState.Open) cnnSql.Close();
            }

            return bitacoraDTO;
        }


        #endregion

        #region UsuarioRol

        public UsuarioRolDTO EjecutarSentenciaSelect(Int16 IdUsuario, Int16 IdRol)
        {
            if (_usuarioRol == null) _usuarioRol = new UsuarioRol(cnnSql);
            try
            {
                cnnSql.Open();
                return _usuarioRol.EjecutarSentenciaSelect(IdUsuario, IdRol);
            }
            finally
            {
                if (cnnSql.State == ConnectionState.Open) cnnSql.Close();
            }
        }

        public Int32 AlmacenarUsuarioRol(List<UsuarioRolDTO> _listaUsuarioRolDTO, string usuarioModificacion, Int32 idUsuario)
        {
            Int32 x = 0;
            TransactionScope ts = null;

            if (_usuarioRol == null) _usuarioRol = new UsuarioRol(cnnSql);

            try
            {
                ts = new TransactionScope();
                cnnSql.Open();

                _usuarioRol.DarBajaRoles(usuarioModificacion, idUsuario);

                foreach (UsuarioRolDTO usuarioRolDTO in _listaUsuarioRolDTO)
                {
                    usuarioRolDTO.USUARIO_MANTENIMIENTO.USUARIO_ALTA = usuarioModificacion;
                    usuarioRolDTO.USUARIO_MANTENIMIENTO.USUARIO_MODIFICO = usuarioModificacion;

                    if (_usuarioRol.ExisteRolAsignado(usuarioRolDTO.ID_USUARIO, usuarioRolDTO.ID_ROL))
                    {
                        x = _usuarioRol.EjecutarSentenciaInsert(usuarioRolDTO);
                    }
                    else
                    {
                        x = _usuarioRol.EjecutarSentenciaUpdate(usuarioRolDTO);
                    }
                }

                ts.Complete();
            }
            finally
            {
                if (cnnSql.State == ConnectionState.Open) cnnSql.Close();
                if (ts != null) ts.Dispose();
            }

            return x;
        }

        public List<UsuarioRolDTO> ListaUsuarioRol(Int32 IdUsuario)
        {
            if (_usuarioRol == null) _usuarioRol = new UsuarioRol(cnnSql);
            List<UsuarioRolDTO> _usuarioRolDto = new List<UsuarioRolDTO>();

            try
            {
                cnnSql.Open();
                return _usuarioRol.ListaUsuarioRol(IdUsuario);
            }
            finally
            {
                if (cnnSql.State != ConnectionState.Closed) cnnSql.Close();
            }
        }

        public Int16 DarBajaUsuarioRol(Int16 IdUsuario, Int16 IdRol, string usuario)
        {
            if (_usuarioRol == null) _usuarioRol = new UsuarioRol(cnnSql);
            try
            {
                cnnSql.Open();
                return _usuarioRol.DarBajaUsuarioRol(IdUsuario, IdRol, usuario);
            }
            finally
            {
                if (cnnSql.State != ConnectionState.Closed) cnnSql.Close();
            }
        }

        public Int16 DarAltaUsuarioRol(Int16 IdUsuario, Int16 IdRol, string usuario)
        {
            if (_usuarioRol == null) _usuarioRol = new UsuarioRol(cnnSql);
            try
            {
                cnnSql.Open();
                return _usuarioRol.DarAltaUsuarioRol(IdUsuario, IdRol, usuario);
            }
            finally
            {
                if (cnnSql.State != ConnectionState.Closed) cnnSql.Close();
            }
        }
        #endregion

        #region IDisposable Members

        //public void Dispose()
        //{
        //    throw new NotImplementedException();
        //}
        private void DisposeConnSql()
        {
            if (cnnSql != null)
            {
                if (cnnSql.State != ConnectionState.Closed)
                    cnnSql.Close();
            }
        }

        public void Dispose()
        {
            try
            {
                DisposeConnSql();
            }
            catch
            {
                GC.SuppressFinalize(this);
            }
        }
        #endregion

        #region UsuarioCuentaGasto
        //---------------------------Mapeo Usuario Cuenta Gasto SATB 21.03.17--------

        public List<UsuarioCuentaDTO> ListaUsuarioCuenta(string BUKRS, string Usuario, string busqueda, string dominio)
        {
            if (_usuarioCuenta == null) _usuarioCuenta = new UsuarioCuentaGasto(cnnSql);
            List<UsuarioCuentaDTO> _rolDto = new List<UsuarioCuentaDTO>();

            try
            {
                cnnSql.Open();

                return _usuarioCuenta.BuscarMapeoUsuarioCuentaSP(BUKRS, Usuario, busqueda, dominio);
            }
            finally
            {
                if (cnnSql.State != ConnectionState.Closed) cnnSql.Close();
            }
        }

        //FIN-------------------------Mapeo Usuario Cuenta Gasto SATB 21.03.17--------

        //---------------------------Mapeo Usuario Cuenta Gasto SATB 02.08.17--------

        public List<UsuarioCuentaDTO> AsignacionCuentaUsuario(string BUKRS, string Usuario, string cuenta)
        {
            if (_usuarioCuenta == null) _usuarioCuenta = new UsuarioCuentaGasto(cnnSql);
            List<UsuarioCuentaDTO> _rolDto = new List<UsuarioCuentaDTO>();

            try
            {
                cnnSql.Open();

                return _usuarioCuenta.AsignacionCuentaUsuario(BUKRS, Usuario, cuenta);
            }
            finally
            {
                if (cnnSql.State != ConnectionState.Closed) cnnSql.Close();
            }
        }

        public List<UsuarioCuentaDTO> QuitarCuentaUsuario(string BUKRS, string Usuario, string cuenta)
        {
            if (_usuarioCuenta == null) _usuarioCuenta = new UsuarioCuentaGasto(cnnSql);
            List<UsuarioCuentaDTO> _rolDto = new List<UsuarioCuentaDTO>();

            try
            {
                cnnSql.Open();

                return _usuarioCuenta.QuitarCuentaUsuario(BUKRS, Usuario, cuenta);
            }
            finally
            {
                if (cnnSql.State != ConnectionState.Closed) cnnSql.Close();
            }
        }

        //FIN-------------------------Mapeo Usuario Cuenta Gasto SATB 02.08.17--------
        #endregion

        #region UsuarioCajaChica
        //---------------------------Mapeo Usuario Caja Chica SATB 03.08.17--------

        public List<UsuarioCajaDTO> ListaUsuarioCaja(string BUKRS, string Usuario, string busqueda, string dominio)
        {
            if (_usuarioCaja == null) _usuarioCaja = new UsuarioCajaChica(cnnSql);
            List<UsuarioCajaDTO> _rolDto = new List<UsuarioCajaDTO>();

            try
            {
                cnnSql.Open();

                return _usuarioCaja.BuscarMapeoUsuarioCajaSP(BUKRS, Usuario, busqueda, dominio);
            }
            finally
            {
                if (cnnSql.State != ConnectionState.Closed) cnnSql.Close();
            }
        }

        //FIN-------------------------Mapeo Usuario Caja Chica SATB 03.08.17--------

        //---------------------------Mapeo Usuario Caja Chica SATB 03.08.17--------

        public List<UsuarioCajaDTO> AsignacionCajaUsuario(string BUKRS, string Usuario, string caja)
        {
            if (_usuarioCaja == null) _usuarioCaja = new UsuarioCajaChica(cnnSql);
            List<UsuarioCajaDTO> _rolDto = new List<UsuarioCajaDTO>();

            try
            {
                cnnSql.Open();

                return _usuarioCaja.AsignacionCajaUsuario(BUKRS, Usuario, caja);
            }
            finally
            {
                if (cnnSql.State != ConnectionState.Closed) cnnSql.Close();
            }
        }

        public List<UsuarioCajaDTO> QuitarCajaUsuario(string BUKRS, string Usuario, string caja)
        {
            if (_usuarioCaja == null) _usuarioCaja = new UsuarioCajaChica(cnnSql);
            List<UsuarioCajaDTO> _rolDto = new List<UsuarioCajaDTO>();

            try
            {
                cnnSql.Open();

                return _usuarioCaja.QuitarCajaUsuario(BUKRS, Usuario, caja);
            }
            finally
            {
                if (cnnSql.State != ConnectionState.Closed) cnnSql.Close();
            }
        }

        //FIN-------------------------Mapeo Usuario Caja Chica SATB 03.08.17--------

        public SuperUsuarioDTO BuscarDatosFactura(int idTipoDocumento, string Identificacion, string serie, string numero, int opcion, string usuario)
        {
            SuperUsuarioDTO _superUsuarioDto = null;
            if (_superUsuario == null) _superUsuario = new SuperUsuario(cnnSql);

            try
            {
                cnnSql.Open();
                _superUsuarioDto = _superUsuario.BuscarDatosFactura(idTipoDocumento, Identificacion, serie, numero, opcion, usuario);

            }
            finally
            {
                if (cnnSql.State != ConnectionState.Closed) cnnSql.Close();
            }

            return _superUsuarioDto;
        }

        public string ValidarMandante(string codigoSociedad)
        {
            string mandante = "";
            if (_superUsuario == null) _superUsuario = new SuperUsuario(cnnSql);

            try
            {
                cnnSql.Open();
                mandante = _superUsuario.ValidarMandante(codigoSociedad);

            }
            finally
            {
                if (cnnSql.State != ConnectionState.Closed) cnnSql.Close();
            }

            return mandante;
        }

        public Int16 ModificarEstadoFactura(Int16 estado, decimal idfactura, Int32 estadoCC, decimal idCC, string usuario, string dominio, string estadoActual, int opcion, string justificacion)
        {
            if (_superUsuario == null) _superUsuario = new SuperUsuario(cnnSql);

            try
            {
                cnnSql.Open();
                return _superUsuario.ModificarEstadoFactura(estado, idfactura, estadoCC, idCC, usuario, dominio, estadoActual, opcion, justificacion);

            }
            finally
            {
                if (cnnSql.State != ConnectionState.Closed) cnnSql.Close();
            }
        }

        public SuperUsuarioDTO BuscarDatosCC(int opcion, string sociedad, string centro, string numero, string correlativo)
        {
            SuperUsuarioDTO _superUsuarioDto = null;
            if (_superUsuario == null) _superUsuario = new SuperUsuario(cnnSql);

            try
            {
                cnnSql.Open();
                _superUsuarioDto = _superUsuario.BuscarDatosCC(opcion, sociedad, centro, numero, correlativo);

            }
            finally
            {
                if (cnnSql.State != ConnectionState.Closed) cnnSql.Close();
            }

            return _superUsuarioDto;
        }

        public Int16 ModificarEstadoCC(Int16 opcion, string usuario, string pais, decimal IdCC, Int16 estadoActual, Int16 estadoNuevo, string justificacion)
        {
            if (_superUsuario == null) _superUsuario = new SuperUsuario(cnnSql);

            try
            {
                cnnSql.Open();
                return _superUsuario.ModificarEstadoCC(opcion, usuario, pais, IdCC, estadoActual, estadoNuevo, justificacion);

            }
            finally
            {
                if (cnnSql.State != ConnectionState.Closed) cnnSql.Close();
            }
        }

        public SuperUsuarioDTO BuscarDatosSincro(int opcion, string sociedad, string documento, string noFactura, string fechaFactura, string docProveedor, string serie)
        {
            SuperUsuarioDTO _superUsuarioDto = null;
            if (_superUsuario == null) _superUsuario = new SuperUsuario(cnnSql);

            try
            {
                cnnSql.Open();
                _superUsuarioDto = _superUsuario.BuscarDatosSincro(opcion, sociedad, documento, noFactura, fechaFactura, docProveedor, serie);

            }
            finally
            {
                if (cnnSql.State != ConnectionState.Closed) cnnSql.Close();
            }

            return _superUsuarioDto;
        }

        public List<decimal> ModificarEstadosFacturas(string noFactura, string docProveedor, string usuario, string serie)
        {
            if (_superUsuario == null) _superUsuario = new SuperUsuario(cnnSql);

            try
            {
                cnnSql.Open();
                List<decimal> idsFacturas = _superUsuario.GetIdFacturasAnular(noFactura, docProveedor, serie);
                string idsFacturasStr = string.Join(", ", idsFacturas.ToArray());
                short result = _superUsuario.ModificarEstadosFacturas(idsFacturasStr, usuario);
                return idsFacturas;
            }
            finally
            {
                if (cnnSql.State != ConnectionState.Closed) cnnSql.Close();
            }
        }

        public Int16 AnularSincronizacion(int opcion, string sociedad, string documento)
        {
            if (_superUsuario == null) _superUsuario = new SuperUsuario(cnnSql);

            try
            {
                cnnSql.Open();
                return _superUsuario.AnularSincronizacion(opcion, sociedad, documento);

            }
            finally
            {
                if (cnnSql.State != ConnectionState.Closed) cnnSql.Close();
            }
        }

        public bool ValidaPermisosAnulacion(string usuario, string codigoSociedad)
        {
            if (_superUsuario == null) _superUsuario = new SuperUsuario(cnnSql);

            try
            {
                cnnSql.Open();
                return _superUsuario.ValidaPermisosAnulacion(usuario, codigoSociedad);

            }
            finally
            {
                if (cnnSql.State != ConnectionState.Closed) cnnSql.Close();
            }
        }

        public Int16 AnularSincronizacionLog(int opcion, string sociedad, string documento, string usuario, string pais, string justificacion)
        {
            if (_superUsuario == null) _superUsuario = new SuperUsuario(cnnSql);

            try
            {
                cnnSql.Open();
                return _superUsuario.AnularSincronizacionLog(opcion, sociedad, documento, usuario, pais, justificacion);

            }
            finally
            {
                if (cnnSql.State != ConnectionState.Closed) cnnSql.Close();
            }
        }

        public List<LlenarDDL_DTO> ListaSociedadUsuario(string usuario)
        {
            if (_usuarioSociedadCentro == null) _usuarioSociedadCentro = new UsuarioSociedadCentro(cnnSql);

            try
            {
                cnnSql.Open();
                return _usuarioSociedadCentro.ListarUsuarioSociedadSuperUsuario(usuario);
            }
            finally
            {
                if (cnnSql.State != ConnectionState.Closed) cnnSql.Close();
            }

        }

        #endregion

        #region MapeoRegistradorAprobador

        public List<LlenarDDL_DTO> BuscarCentros(string ceco)
        {
            if (_centro == null) _centro = new Centro(cnnSql);

            try
            {
                cnnSql.Open();
                return _centro.BuscarCentros(ceco);
            }
            finally
            {
                if (cnnSql.State != ConnectionState.Closed) cnnSql.Close();
            }
        }

        public List<LlenarDDL_DTO> BuscarAprobador(string ceco, string centro)
        {
            if (_usuarioCentroCosto == null) _usuarioCentroCosto = new UsuarioCentroCosto(cnnSql);

            try
            {
                cnnSql.Open();
                return _usuarioCentroCosto.BuscarAprobador(ceco, centro);
            }
            finally
            {
                if (cnnSql.State != ConnectionState.Closed) cnnSql.Close();
            }
        }

        public List<LlenarDDL_DTO> BuscarRegistrador(string ceco, string centro)
        {
            if (_usuarioCentroCosto == null) _usuarioCentroCosto = new UsuarioCentroCosto(cnnSql);

            try
            {
                cnnSql.Open();
                return _usuarioCentroCosto.BuscarRegistrador(ceco, centro);
            }
            finally
            {
                if (cnnSql.State != ConnectionState.Closed) cnnSql.Close();
            }
        }

        public List<MapeoRegistradorAprobadorDTO> ListaMapeosRegistradorAprobador(string ceco, string centro, string aprobador, string registrador)
        {
            if (_usuarioCentroCosto == null) _usuarioCentroCosto = new UsuarioCentroCosto(cnnSql);
            List<MapeoRegistradorAprobadorDTO> _mapeoDto = new List<MapeoRegistradorAprobadorDTO>();

            try
            {
                cnnSql.Open();

                return _usuarioCentroCosto.ListaMapeoRegistradorAprobador(ceco, centro, aprobador, registrador);
            }
            finally
            {
                if (cnnSql.State != ConnectionState.Closed) cnnSql.Close();
            }
        }

        public List<MapeoRegistradorAprobadorDTO> InsertarMpeoRegistradorAprobador(string ceco, string centro, string aprobador, string registrador, string usuario)
        {
            if (_usuarioCentroCosto == null) _usuarioCentroCosto = new UsuarioCentroCosto(cnnSql);
            List<MapeoRegistradorAprobadorDTO> _mapeoDto = new List<MapeoRegistradorAprobadorDTO>();

            try
            {
                cnnSql.Open();

                return _usuarioCentroCosto.InsertarMapeoRegistradorAprobador(ceco, centro, aprobador, registrador, usuario);
            }
            finally
            {
                if (cnnSql.State != ConnectionState.Closed) cnnSql.Close();
            }
        }

        public List<MapeoRegistradorAprobadorDTO> EliminarMpeoRegistradorAprobador(string ceco, string centro, string aprobador, string registrador, string usuario)
        {
            if (_usuarioCentroCosto == null) _usuarioCentroCosto = new UsuarioCentroCosto(cnnSql);
            List<MapeoRegistradorAprobadorDTO> _mapeoDto = new List<MapeoRegistradorAprobadorDTO>();

            try
            {
                cnnSql.Open();

                return _usuarioCentroCosto.EliminarMapeoRegistradorAprobador(ceco, centro, aprobador, registrador, usuario);
            }
            finally
            {
                if (cnnSql.State != ConnectionState.Closed) cnnSql.Close();
            }
        }

        #endregion
    }
}
