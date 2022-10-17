using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using DipCmiGT.LogicaCajasChicas.Entidad;
using System.Data;
using DipCmiGT.LogicaComun.Util;
using DipCmiGT.LogicaComun;
using LogicaComun.Enum;
using LogicaCajasChicas.Entidad;
using LogicaCajasChicas;
using System.Transactions;
using DipCmiGT.LogicaComun.Enum;

namespace DipCmiGT.LogicaCajasChicas.Sesion
{
    /// <summary>
    /// Clase para gestionar los procesos relacionados con la sociedad.
    /// </summary>
    public class GestorSociedad : IDisposable
    {

        #region Declaracion

        SAPCuentaContable _sapCuentaContable = null;
        UsuarioCentroCosto _usuarioCentroCosto = null;
        SociedadCentro _sociedadCentro = null;
        Sociedad _sociedad = null;
        SAPCajasChicas _sapCajaChica = null;
        SAPCuentaContableCentroCosto _sapCuentaContableCentroCosto = null;
        SAPCuentasOrdenesGastos _sapCuentaOrdenesGastos = null;
        UsuarioOrdenCompra _usuarioOrdenCosto = null;
        SAPOrdenesGastos _sapOrdenCompra = null;
        UsuarioSociedadCentro _usuarioSociedadCentro = null;
        Moneda _moneda = null;
        SociedadMoneda _sociedadMoneda = null;

        SqlConnection cnnSql = null;

        #endregion

        #region Constructor
        public GestorSociedad(string conexion)
        {
            cnnSql = new SqlConnection(conexion);
        }
        # endregion

        # region Sociedad

        public SociedadDTO EjeccutarSentenciaSelect(string CodSociedad)
        {
            if (_sociedad == null) _sociedad = new Sociedad(cnnSql);

            try
            {
                cnnSql.Open();

                return _sociedad.EjecutarSentenciaSelect(CodSociedad);
            }
            finally
            {
                if (cnnSql.State != ConnectionState.Closed) cnnSql.Close();
            }
        }

        public SociedadDTO AlmacenarSociedad(SociedadDTO sociedadDTO)
        {
            Int32 x = 0;
            if (_sociedad == null) _sociedad = new Sociedad(cnnSql);

            try
            {
                cnnSql.Open();
                //Validacion si Existe Sociedad 
                if (sociedadDTO.CODIGO_SOCIEDAD != "")
                {
                    if (!_sociedad.ExisteSociedad(sociedadDTO.CODIGO_SOCIEDAD))
                    {
                        x = _sociedad.EjecutarSentenciaInsert(sociedadDTO);
                    }
                    else
                        x = _sociedad.EjectuarSentenciaUpdate(sociedadDTO);
                }
            }
            finally
            {
                if (cnnSql.State == ConnectionState.Open) cnnSql.Close();
            }

            return sociedadDTO;
        }

        public List<SociedadDTO> ListaSociedad()
        {
            if (_sociedad == null) _sociedad = new Sociedad(cnnSql);
            List<SociedadDTO> _socieadDto = new List<SociedadDTO>();

            try
            {
                cnnSql.Open();
                return _sociedad.ListaSociedad();
            }
            finally
            {
                if (cnnSql.State != ConnectionState.Closed) cnnSql.Close();
            }
        }

        public List<LlenarDDL_DTO> ListaSociedadesActivas()
        {
            if (_sociedad == null) _sociedad = new Sociedad(cnnSql);

            try
            {
                cnnSql.Open();
                return _sociedad.ListaSociedadesActivas();
            }
            finally
            {
                if (cnnSql.State != ConnectionState.Closed) cnnSql.Close();
            }
        }

        public List<LlenarDDL_DTO> ListaRegistradores(string aprobador)
        {
            if (_sociedad == null) _sociedad = new Sociedad(cnnSql);

            try
            {
                cnnSql.Open();
                return _sociedad.ListaRegistradores(aprobador);
            }
            finally
            {
                if (cnnSql.State != ConnectionState.Closed) cnnSql.Close();
            }
        }

        public Int16 DarBajaSociedad(Int32 CodSociedad, string usuario)
        {
            if (_sociedad == null) _sociedad = new Sociedad(cnnSql);

            try
            {
                cnnSql.Open();

                //return 0;
                return _sociedad.DarBajaSociedad(CodSociedad, usuario);
            }
            finally
            {
                if (cnnSql.State != ConnectionState.Closed) cnnSql.Close();
            }
        }

        public Int16 DarAltaSociedad(Int32 CodSociedad, string usuario)
        {
            if (_sociedad == null) _sociedad = new Sociedad(cnnSql);

            try
            {
                cnnSql.Open();

                //return 0;
                return _sociedad.DarAltaSociedad(CodSociedad, usuario);
            }
            finally
            {
                if (cnnSql.State != ConnectionState.Closed) cnnSql.Close();
            }
        }

        public string MandanteSociedad (string CodSociedad)
        {
            if (_sociedad == null) _sociedad = new Sociedad(cnnSql);

            try
            {
                cnnSql.Open();
                return _sociedad.MandanteSociedad(CodSociedad);
            }
            finally
            {
                if (cnnSql.State != ConnectionState.Closed) cnnSql.Close();
            }
        }
        # endregion

        #region CuentasContablesSociedades

        public List<LlenarDDL_DTO> BuscarCuentasContablesOrdenCosto(string ordenCompra, string codigoSociedad)
        {
            if (_sapCuentaContable == null) _sapCuentaContable = new SAPCuentaContable(cnnSql);

            try
            {
                cnnSql.Open();

                return _sapCuentaContable.BuscarCuentasContablesOrdenCosto(ordenCompra, codigoSociedad);
            }
            finally
            {
                if (cnnSql.State != ConnectionState.Closed) cnnSql.Close();
            }
        }

        public List<LlenarDDL_DTO> BuscarCuentasContablesCentroCosto(string usuario, string codigoSociedad, string centroCosto)
        {
            if (_sapCuentaContable == null) _sapCuentaContable = new SAPCuentaContable(cnnSql);

            try
            {
                cnnSql.Open();

               // return _sapCuentaContable.BuscarCuentasContablesCentroCosto(centroCosto, codigoSociedad);
                return _sapCuentaContable.BuscarCuentasContablesCentroCosto(usuario, codigoSociedad, centroCosto);
            }
            finally
            {
                if (cnnSql.State != ConnectionState.Closed) cnnSql.Close();
            }
        }

        public List<LlenarDDL_DTO> LlenarImpuetos(string codigoSociedad)
        {
            if (_sociedad == null) _sociedad = new Sociedad(cnnSql);
            

            try
            {
                cnnSql.Open();
                return _sociedad.LlenarImpuestos(codigoSociedad);
            }
            finally
            {
                if (cnnSql.State != ConnectionState.Closed) cnnSql.Close();
            }
        }

        #endregion

        #region CajaChica

      //public List<LlenarDDL_DTO> LlenarDDL(string codigoSociedad)
      public List<LlenarDDL_DTO> LlenarDDL(string codigoSociedad, string usuario)
        {
            if (_sapCajaChica == null) _sapCajaChica = new SAPCajasChicas(cnnSql);

            try
            {
                cnnSql.Open();
                //return _sapCajaChica.BuscarCajasChicas(codigoSociedad);
                return _sapCajaChica.BuscarCajasChicas(codigoSociedad, usuario);
            }
            finally
            {
                if (cnnSql.State != ConnectionState.Closed) cnnSql.Close();
            }
        }

        #endregion

        #region SociedadCentros

        public List<LlenarDDL_DTO> ListarSociedadMapeada()
        {
            if (_sociedadCentro == null) _sociedadCentro = new SociedadCentro(cnnSql);

            try
            {
                cnnSql.Open();
                return _sociedadCentro.ListarSociedadMapeada();
            }
            finally
            {
                if (cnnSql.State != ConnectionState.Closed) cnnSql.Close();
            }
        }


        public List<LlenarDDL_DTO> ListarCentroMapeado(string codigoSociedad)
        {
            if (_sociedadCentro == null) _sociedadCentro = new SociedadCentro(cnnSql);

            try
            {
                cnnSql.Open();
                return _sociedadCentro.ListarCentroMapeado(codigoSociedad);
            }
            finally
            {
                if (cnnSql.State != ConnectionState.Closed) cnnSql.Close();
            }
        }

        public List<UsuarioCentroCostoDTO> ListarUsuarioCentroCosto(string usuario)
        {
            if (_usuarioCentroCosto == null) _usuarioCentroCosto = new UsuarioCentroCosto(cnnSql);
            try
            {
                cnnSql.Open();
                return _usuarioCentroCosto.ListaUsuarioSociedadCentro(usuario);
            }
            finally
            {
                if (cnnSql.State != ConnectionState.Closed) cnnSql.Close();
            }
        }

        public List<UsuarioCentroCostoDTO> ListarUsuarioCentroCosto(string usuario, Int32 idSociedadCentro)
        {
            if (_usuarioCentroCosto == null) _usuarioCentroCosto = new UsuarioCentroCosto(cnnSql);
            try
            {
                cnnSql.Open();
                return _usuarioCentroCosto.ListaUsuarioSociedadCentro(usuario, idSociedadCentro);
            }
            finally
            {
                if (cnnSql.State != ConnectionState.Closed) cnnSql.Close();
            }
        }

        public List<SociedadCentroDTO> ListaSociedadCentro()
        {
            if (_sociedadCentro == null) _sociedadCentro = new SociedadCentro(cnnSql);
            List<SociedadCentroDTO> _centroDto = new List<SociedadCentroDTO>();

            try
            {
                cnnSql.Open();
                return _sociedadCentro.ListaSociedadCentro();
            }
            finally
            {
                if (cnnSql.State != ConnectionState.Closed) cnnSql.Close();
            }
        }

        public List<SociedadCentroDTO> ListaSociedadCentro(string codigoSociedad)
        {
            if (_sociedadCentro == null) _sociedadCentro = new SociedadCentro(cnnSql);
            List<SociedadCentroDTO> _centroDto = new List<SociedadCentroDTO>();

            try
            {
                cnnSql.Open();
                return _sociedadCentro.EjecutarSentenciaSelect(codigoSociedad);
            }
            finally
            {
                if (cnnSql.State != ConnectionState.Closed) cnnSql.Close();
            }
        }

        public SociedadCentroDTO AlmacenarSociedadCentro(SociedadCentroDTO sociedadcentroDTO)
        {
            Int32 x = 0;
            if (_sociedadCentro == null) _sociedadCentro = new SociedadCentro(cnnSql);

            try
            {
                cnnSql.Open();
                ////Validacion si Existe Sociedad 
                if (!_sociedadCentro.ExisteSociedadCentro(sociedadcentroDTO.CODIGO_SOCIEDAD, sociedadcentroDTO.ID_CENTRO))
                {
                    x = _sociedadCentro.EjecutarSentenciaInsert(sociedadcentroDTO);
                }
            }
            finally
            {
                if (cnnSql.State == ConnectionState.Open) cnnSql.Close();
            }

            return sociedadcentroDTO;
        }



        #endregion

        #region UsuariosCentrosCosto

        public List<UsuarioCentroCostoDTO> ListaCuentaCosto(string usuario, Int32 idSociedadCentro)
        {
            if (_usuarioCentroCosto == null) _usuarioCentroCosto = new UsuarioCentroCosto(cnnSql);

            try
            {
                cnnSql.Open();

                return _usuarioCentroCosto.ListaCuentaCosto(usuario, idSociedadCentro);
            }
            finally
            {
                if (cnnSql.State != ConnectionState.Closed) cnnSql.Close();
            }
        }

        public List<LlenarDDL_DTO> ListarSociedadCentroCosto(string usuario)
        {
            if (_usuarioCentroCosto == null) _usuarioCentroCosto = new UsuarioCentroCosto(cnnSql);

            try
            {
                cnnSql.Open();

                return _usuarioCentroCosto.ListarSociedadCentroCosto(usuario);
            }
            finally
            {
                if (cnnSql.State != ConnectionState.Closed) cnnSql.Close();
            }
        }

        public List<LlenarDDL_DTO> ListarCentroCostoDDL(string codigoSociedad)
        {
            if (_sapCuentaContableCentroCosto == null) _sapCuentaContableCentroCosto = new SAPCuentaContableCentroCosto(cnnSql);
            try
            {
                cnnSql.Open();
                return _sapCuentaContableCentroCosto.ListarCentrosCosto(codigoSociedad);
            }
            finally
            {
                if (cnnSql.State != ConnectionState.Closed) cnnSql.Close();
            }
        }

        public List<LlenarDDL_DTO> ListarCentroCostoDDL(string codigoSociedad, string centroCosto)
        {
            if (_sapCuentaContableCentroCosto == null) _sapCuentaContableCentroCosto = new SAPCuentaContableCentroCosto(cnnSql);
            try
            {
                cnnSql.Open();

                if (!string.IsNullOrEmpty(centroCosto))
                    return _sapCuentaContableCentroCosto.ListarCentrosCosto(codigoSociedad, centroCosto);
                else
                    return _sapCuentaContableCentroCosto.ListarCentrosCosto(codigoSociedad);
            }
            finally
            {
                if (cnnSql.State != ConnectionState.Closed) cnnSql.Close();
            }
        }

        public List<LlenarDDL_DTO> ListarCentroCentroCosto(string usuario, string codigoSociedad)
        {
            if (_usuarioCentroCosto == null) _usuarioCentroCosto = new UsuarioCentroCosto(cnnSql);

            try
            {
                cnnSql.Open();

                return _usuarioCentroCosto.ListarCentroCentroCosto(usuario, codigoSociedad);
            }
            finally
            {
                if (cnnSql.State != ConnectionState.Closed) cnnSql.Close();
            }
        }

        //public List<LlenarDDL_DTO> ListarOrdenCostoUsuarioDDL(string usuario, string codigoSociedad)
        //{
        //    if (_usuarioOrdenCompra == null) _usuarioOrdenCompra = new UsuarioOrdenCompra(cnnSql);
        //    try
        //    {
        //        cnnSql.Open();
        //        return  _usuarioOrdenCompra
        //    }
        //    finally
        //    {
        //        if (cnnSql.State != ConnectionState.Closed) cnnSql.Close();
        //    }
        //}

        //public UsuarioCentroCostoDTO AgregarUsuarioCentroCosto(UsuarioCentroCostoDTO usuariocentroCostoDTO)
        //        {
        //    if (_sapCuentaContableCentroCosto == null) _sapCuentaContableCentroCosto = new SAPCuentaContableCentroCosto(cnnSql);
        //    try
        //    {
        //        cnnSql.Open();
        //        return _sapCuentaContableCentroCosto.ListarCentrosCosto(codigoSociedad);
        //    }
        //    finally
        //    {
        //        if (cnnSql.State != ConnectionState.Closed) cnnSql.Close();
        //    }
        //}

        public Int16 DarAltaCentroCosto(Int32 idCentro, string usuario)
        {
            if (_usuarioCentroCosto == null) _usuarioCentroCosto = new UsuarioCentroCosto(cnnSql);

            try
            {
                cnnSql.Open();
                return _usuarioCentroCosto.DarAltaCentroCosto(idCentro, usuario);
            }
            finally
            {
                if (cnnSql.State != ConnectionState.Closed) cnnSql.Close();
            }
        }

        public Int16 DarBajaCentroCosto(Int32 idCentro, string usuario)
        {
            if (_usuarioCentroCosto == null) _usuarioCentroCosto = new UsuarioCentroCosto(cnnSql);

            try
            {
                cnnSql.Open();
                return _usuarioCentroCosto.DarBajaCentroCosto(idCentro, usuario);
            }
            finally
            {
                if (cnnSql.State != ConnectionState.Closed) cnnSql.Close();
            }
        }

        public Int32 AgregarUsuarioCentroCosto(List<UsuarioCentroCostoDTO> _listaUsuariocentroCostoDTO, string usuarioMantenimiento)
        {
            Int32 x = 0;
            if (_usuarioCentroCosto == null) _usuarioCentroCosto = new UsuarioCentroCosto(cnnSql);
            UsuarioCentroCostoDTO _uCCDto = null;
            TransactionScope ts = null;

            try
            {
                ts = new TransactionScope();
                cnnSql.Open();

                foreach (UsuarioCentroCostoDTO usuariocentroCostoDTO in _listaUsuariocentroCostoDTO)
                {
                    _uCCDto = _usuarioCentroCosto.BuscarCentroCosto(usuariocentroCostoDTO.USUARIO, usuariocentroCostoDTO.CENTRO_COSTO, usuariocentroCostoDTO.ID_SOCIEDAD_CENTRO);

                    if (_uCCDto == null && usuariocentroCostoDTO.ALTA == Convert.ToBoolean(EstadoEnum.ALTA))
                    {
                        usuariocentroCostoDTO.USUARIO_MANTENIMIENTO.USUARIO_ALTA = usuarioMantenimiento;
                        usuariocentroCostoDTO.ID_USUARIO_CENTRO_COSTO = _usuarioCentroCosto.EjecutarSentenciaInsert(usuariocentroCostoDTO);
                    }

                    if (_uCCDto == null) continue;

                    if (_uCCDto.ALTA != usuariocentroCostoDTO.ALTA)
                    {
                        usuariocentroCostoDTO.USUARIO_MANTENIMIENTO.USUARIO_MODIFICO = usuarioMantenimiento;
                        usuariocentroCostoDTO.ID_USUARIO_CENTRO_COSTO = _uCCDto.ID_USUARIO_CENTRO_COSTO;

                        if (usuariocentroCostoDTO.ALTA == Convert.ToBoolean(EstadoEnum.BAJA))
                            _usuarioCentroCosto.DarBajaCentroCosto(usuariocentroCostoDTO.ID_USUARIO_CENTRO_COSTO, usuarioMantenimiento);
                        else
                            _usuarioCentroCosto.DarAltaCentroCosto(usuariocentroCostoDTO.ID_USUARIO_CENTRO_COSTO, usuarioMantenimiento);
                    }
                }
                ts.Complete();
            }
            finally
            {
                if (ts != null) ts.Dispose();
                if (cnnSql.State == ConnectionState.Open) cnnSql.Close();
            }
            return x;
        }

        #endregion

        #region UsuarioOrdenCosto

        public Int16 DarAltaOrdenCompra(Int32 idUsuarioOrdenCompra, string usuario)
        {
            if (_usuarioOrdenCosto == null) _usuarioOrdenCosto = new UsuarioOrdenCompra(cnnSql);

            try
            {
                cnnSql.Open();
                return _usuarioOrdenCosto.DarAltaOrdenCompra(idUsuarioOrdenCompra, usuario);
            }
            finally
            {
                if (cnnSql.State != ConnectionState.Closed) cnnSql.Close();
            }
        }

        public Int16 DarBajaOrdenCompra(Int32 idUsuarioOrdenCompra, string usuario)
        {
            if (_usuarioOrdenCosto == null) _usuarioOrdenCosto = new UsuarioOrdenCompra(cnnSql);

            try
            {
                cnnSql.Open();
                return _usuarioOrdenCosto.DarBajaOrdenCompra(idUsuarioOrdenCompra, usuario);
            }
            finally
            {
                if (cnnSql.State != ConnectionState.Closed) cnnSql.Close();
            }
        }

        public List<UsuarioOrdenCostoDTO> ListarUsuarioOrdenCosto(string usuario, Int32 idSociedadCentro)
        {
            if (_usuarioOrdenCosto == null) _usuarioOrdenCosto = new UsuarioOrdenCompra(cnnSql);
            try
            {
                cnnSql.Open();
                return _usuarioOrdenCosto.ListaOrdenCosto(usuario, idSociedadCentro);
            }
            finally
            {
                if (cnnSql.State != ConnectionState.Closed) cnnSql.Close();
            }
        }

        public List<LlenarDDL_DTO> ListarUsuariosCentroCostoDDL(string codigoSociedad)
        {
            if (_sapCuentaContableCentroCosto == null) _sapCuentaContableCentroCosto = new SAPCuentaContableCentroCosto(cnnSql);
            try
            {
                cnnSql.Open();
                return _sapCuentaContableCentroCosto.ListarCentrosCosto(codigoSociedad);
            }
            finally
            {
                if (cnnSql.State != ConnectionState.Closed) cnnSql.Close();
            }
        }

        public List<LlenarDDL_DTO> ListarCentroCostoUsuariosDDL(string usuario, string codigoSociedad)
        {
            if (_usuarioCentroCosto == null) _usuarioCentroCosto = new UsuarioCentroCosto(cnnSql);
            try
            {
                cnnSql.Open();
                return _usuarioCentroCosto.ListaCentroCostoUsuarioDDL(usuario, codigoSociedad);
            }
            finally
            {
                if (cnnSql.State != ConnectionState.Closed) cnnSql.Close();
            }
        }

        #endregion

        #region UsuarioOrdeneCompra

        public List<UsuarioOrdenCostoDTO> ListarUsuarioOrdenCompra(string usuario)
        {
            if (_usuarioOrdenCosto == null) _usuarioOrdenCosto = new UsuarioOrdenCompra(cnnSql);
            try
            {
                cnnSql.Open();
                return _usuarioOrdenCosto.ListaUsuarioOrdenCompra(usuario);
            }
            finally
            {
                if (cnnSql.State != ConnectionState.Closed) cnnSql.Close();
            }
        }

        public List<SAPOrdenesGastosDTO> ListaOrdenCompraFacturas()
        {
            if (_sapOrdenCompra == null) _sapOrdenCompra = new SAPOrdenesGastos(cnnSql);
            try
            {
                cnnSql.Open();
                return _sapOrdenCompra.ListaOrdenCompraDDLFacturas();
            }
            finally
            {
                if (cnnSql.State != ConnectionState.Closed) cnnSql.Close();
            }
        }

        public Int32 AgregarUsuarioOrdenCosto(List<UsuarioOrdenCostoDTO> listaUsuarioOrdenCosto, string usuarioMantenimiento)
        {
            if (_usuarioOrdenCosto == null) _usuarioOrdenCosto = new UsuarioOrdenCompra(cnnSql);
            TransactionScope ts = null;
            UsuarioOrdenCostoDTO uOCDto = null;
            Int32 x = 0;

            try
            {
                ts = new TransactionScope();
                cnnSql.Open();

                foreach (UsuarioOrdenCostoDTO usuarioOrdenCompraDTO in listaUsuarioOrdenCosto)
                {
                    uOCDto = _usuarioOrdenCosto.BuscarUsuarioOrdenCosto(usuarioOrdenCompraDTO.USUARIO, usuarioOrdenCompraDTO.ORDEN_COSTO, usuarioOrdenCompraDTO.ID_SOCIEDAD_CENTRO);
                    if (uOCDto == null && usuarioOrdenCompraDTO.ALTA == Convert.ToBoolean(EstadoEnum.ALTA))
                    {
                        usuarioOrdenCompraDTO.USUARIO_MANTENIMIENTO.USUARIO_ALTA = usuarioMantenimiento;
                        usuarioOrdenCompraDTO.ID_USUARIO_ORDEN_COMPRA = _usuarioOrdenCosto.EjecutarSentenciaInsert(usuarioOrdenCompraDTO);
                    }

                    if (uOCDto == null) continue;

                    if (uOCDto.ALTA != usuarioOrdenCompraDTO.ALTA)
                    {
                        usuarioOrdenCompraDTO.USUARIO_MANTENIMIENTO.USUARIO_MODIFICO = usuarioMantenimiento;
                        usuarioOrdenCompraDTO.ID_USUARIO_ORDEN_COMPRA = uOCDto.ID_USUARIO_ORDEN_COMPRA;

                        if (usuarioOrdenCompraDTO.ALTA == Convert.ToBoolean(EstadoEnum.BAJA))
                            _usuarioOrdenCosto.DarBajaOrdenCompra(usuarioOrdenCompraDTO.ID_USUARIO_ORDEN_COMPRA, usuarioMantenimiento);
                        else
                            _usuarioOrdenCosto.DarAltaOrdenCompra(usuarioOrdenCompraDTO.ID_USUARIO_ORDEN_COMPRA, usuarioMantenimiento);
                    }

                }

                ts.Complete();
            }
            finally
            {
                if (ts != null) ts.Dispose();
                if (cnnSql.State == ConnectionState.Open) cnnSql.Close();
            }
            return x;
        }


        public Int16 DarBajaSociedadCentro(Int32 idSociedadCentro, string usuario)
        {
            if (_sociedadCentro == null) _sociedadCentro = new SociedadCentro(cnnSql);
            try
            {
                cnnSql.Open();
                return _sociedadCentro.DarBajaSociedadCentro(idSociedadCentro, usuario);
            }
            finally
            {
                if (cnnSql.State != ConnectionState.Closed) cnnSql.Close();
            }

        }

        public Int16 DarAltaSociedadCentro(Int32 idSociedadCentro, string usuario)
        {
            if (_sociedadCentro == null) _sociedadCentro = new SociedadCentro(cnnSql);

            try
            {
                cnnSql.Open();
                return _sociedadCentro.DarAltaSociedadCentro(idSociedadCentro, usuario);
            }
            finally
            {
                if (cnnSql.State != ConnectionState.Closed) cnnSql.Close();
            }
        }


        public List<LlenarDDL_DTO> ListarOrdenCostoDDL(string codigoSociedad)
        {
            if (_sapCuentaOrdenesGastos == null) _sapCuentaOrdenesGastos = new SAPCuentasOrdenesGastos(cnnSql);
            try
            {
                cnnSql.Open();
                return _sapCuentaOrdenesGastos.ListarOrdenCosto(codigoSociedad);
            }
            finally
            {
                if (cnnSql.State != ConnectionState.Closed) cnnSql.Close();
            }
        }

        public List<LlenarDDL_DTO> ListarOrdenCostoDDL(string codigoSociedad, string ordenCosto)
        {
            if (_sapCuentaOrdenesGastos == null) _sapCuentaOrdenesGastos = new SAPCuentasOrdenesGastos(cnnSql);
            try
            {
                cnnSql.Open();

                if (!string.IsNullOrEmpty(ordenCosto))
                    return _sapCuentaOrdenesGastos.ListarOrdenCosto(codigoSociedad, ordenCosto);
                else
                    return _sapCuentaOrdenesGastos.ListarOrdenCosto(codigoSociedad);
            }
            finally
            {
                if (cnnSql.State != ConnectionState.Closed) cnnSql.Close();
            }
        }


        public List<LlenarDDL_DTO> ListarOrdenCostoUsuarioDDL(string usuario, string codigoSociedad)
        {
            if (_usuarioOrdenCosto == null) _usuarioOrdenCosto = new UsuarioOrdenCompra(cnnSql);
            try
            {
                cnnSql.Open();
                return _usuarioOrdenCosto.ListarOrdenCostoUsuario(usuario, codigoSociedad);
            }
            finally
            {
                if (cnnSql.State != ConnectionState.Closed) cnnSql.Close();
            }
        }
        #endregion

        #region UsuarioSociedadCentro

        public List<LlenarDDL_DTO> ListarSociedadesUsuario(string usuario, string dominio)
        {
            if (_sociedadCentro == null) _sociedadCentro = new SociedadCentro(cnnSql);

            try
            {
                cnnSql.Open();
                return _sociedadCentro.ListarSociedadesUsuario(usuario, dominio);
            }
            finally
            {
                if (cnnSql.State != ConnectionState.Closed) cnnSql.Close();
            }
        }

        public List<LlenarDDL_DTO> ListarSociedades(string usuario)
        {
            if (_sociedadCentro == null) _sociedadCentro = new SociedadCentro(cnnSql);

            try
            {
                cnnSql.Open();
                return _sociedadCentro.ListarSociedades(usuario);
            }
            finally
            {
                if (cnnSql.State != ConnectionState.Closed) cnnSql.Close();
            }
        }

        public List<LlenarDDL_DTO> ListarCentrosMapeados(string codigoSociedad, string usuario)
        {
            if (_sociedadCentro == null) _sociedadCentro = new SociedadCentro(cnnSql);

            try
            {
                cnnSql.Open();

                return _sociedadCentro.ListarSociedadCentrosMapeados(codigoSociedad, usuario);
            }
            finally
            {
                if (cnnSql.State != ConnectionState.Closed) cnnSql.Close();
            }
        }

        public List<LlenarDDL_DTO> ListarSociedadCentrosMapeados(string codigoSociedad, string usuario)
        {
            if (_sociedadCentro == null) _sociedadCentro = new SociedadCentro(cnnSql);

            try
            {
                cnnSql.Open();

                return _sociedadCentro.ListarSociedadCentrosMapeados(codigoSociedad, usuario);
            }
            finally
            {
                if (cnnSql.State != ConnectionState.Closed) cnnSql.Close();
            }
        }

        public List<UsuarioSociedadCentroDTO> ListarUsuarioSociedadCentro(string usuario)
        {
            if (_usuarioSociedadCentro == null) _usuarioSociedadCentro = new UsuarioSociedadCentro(cnnSql);

            try
            {
                cnnSql.Open();

                return _usuarioSociedadCentro.ListarUsuarioSociedadCentro(usuario);
            }
            finally
            {
                if (cnnSql.State != ConnectionState.Closed) cnnSql.Close();
            }
        }

        public List<UsuarioSociedadCentroDTO> ListarUsuarioSociedadCentro(string usuario, string codigoSociedad)
        {
            if (_usuarioSociedadCentro == null) _usuarioSociedadCentro = new UsuarioSociedadCentro(cnnSql);

            try
            {
                cnnSql.Open();

                return _usuarioSociedadCentro.ListarUsuarioSociedadCentro(usuario, codigoSociedad);
            }
            finally
            {
                if (cnnSql.State != ConnectionState.Closed) cnnSql.Close();
            }
        }

        public void AlmacenarUsuarioSociedadCentro(UsuarioSociedadCentroDTO usuarioSociedadCentro)
        {
            if (_usuarioSociedadCentro == null) _usuarioSociedadCentro = new UsuarioSociedadCentro(cnnSql);

            try
            {
                cnnSql.Open();

                if (_usuarioSociedadCentro.ExisteUsuarioSociedadCentro(usuarioSociedadCentro.ID_SOCIEDAD_CENTRO, usuarioSociedadCentro.USUARIO))
                    throw new ExcepcionesDIPCMI("El mapeo sociedad centro ya existe con el usuario");

                usuarioSociedadCentro.ID_USUARIO_SOCIEDAD_CENTRO = _usuarioSociedadCentro.EjecutarSentenciaInsert(usuarioSociedadCentro);
            }
            finally
            {
                if (cnnSql.State != ConnectionState.Closed) cnnSql.Close();
            }
        }

        public Int32 AlmacenarUsuarioSociedadCentro(List<UsuarioSociedadCentroDTO> _listaUsuariocentroCostoDTO, string usuarioMantenimiento)
        {
            Int32 x = 0;
            
            UsuarioSociedadCentroDTO _uSCDto = null;
            TransactionScope ts = null;
            if (_usuarioSociedadCentro == null) _usuarioSociedadCentro = new UsuarioSociedadCentro(cnnSql);

            try
            {
                ts = new TransactionScope();
                cnnSql.Open();

                foreach (UsuarioSociedadCentroDTO usuarioSociedadCentroDto in _listaUsuariocentroCostoDTO)
                {
                    _uSCDto = _usuarioSociedadCentro.BuscarSociedadCentro(usuarioSociedadCentroDto.USUARIO, usuarioSociedadCentroDto.ID_SOCIEDAD_CENTRO );

                    if (_uSCDto == null && usuarioSociedadCentroDto.ALTA == Convert.ToBoolean(EstadoEnum.ALTA))
                    {
                        usuarioSociedadCentroDto.USUARIO_MANTENIMIENTO.USUARIO_ALTA = usuarioMantenimiento;
                        usuarioSociedadCentroDto.ID_USUARIO_SOCIEDAD_CENTRO = _usuarioSociedadCentro.EjecutarSentenciaInsert(usuarioSociedadCentroDto);
                    }

                    if (_uSCDto == null) continue;

                    if (_uSCDto.ALTA != usuarioSociedadCentroDto.ALTA)
                    {
                        usuarioSociedadCentroDto.USUARIO_MANTENIMIENTO.USUARIO_MODIFICO = usuarioMantenimiento;
                        usuarioSociedadCentroDto.ID_USUARIO_SOCIEDAD_CENTRO = _uSCDto.ID_USUARIO_SOCIEDAD_CENTRO;

                        if (usuarioSociedadCentroDto.ALTA == Convert.ToBoolean(EstadoEnum.BAJA))
                            _usuarioSociedadCentro.DarBajaUsrSociedadCentro(usuarioSociedadCentroDto.ID_USUARIO_SOCIEDAD_CENTRO, usuarioMantenimiento);
                        else
                            _usuarioSociedadCentro.DarAltaUsrSociedadCentro(usuarioSociedadCentroDto.ID_USUARIO_SOCIEDAD_CENTRO, usuarioMantenimiento);
                    }
                }
                ts.Complete();
            }
            finally
            {
                if (ts != null) ts.Dispose();
                if (cnnSql.State == ConnectionState.Open) cnnSql.Close();
            }
            return x;

        }

        public List<LlenarDDL_DTO> ListarCentrosUsuario(string usuario, string codigoSociedad)
        {
            if (_sociedadCentro == null) _sociedadCentro = new SociedadCentro(cnnSql);

            try
            {
                cnnSql.Open();

                return _sociedadCentro.ListarCentrosUsuario(usuario, codigoSociedad);

            }
            finally
            {
                if (cnnSql.State != ConnectionState.Closed) cnnSql.Close();
            }
        }

        public List<LlenarDDL_DTO> ListarUsuarioSociedad(string usuario)
        {
            if (_usuarioSociedadCentro == null) _usuarioSociedadCentro = new UsuarioSociedadCentro(cnnSql);

            try
            {
                cnnSql.Open();
                return _usuarioSociedadCentro.ListarUsuarioSociedad(usuario);
            }
            finally
            {
                if (cnnSql.State != ConnectionState.Closed) cnnSql.Close();
            }
        }

        public List<LlenarDDL_DTO> ListarUsuarioCentro(string usuario, string codigoSociedad)
        {
            if (_usuarioSociedadCentro == null) _usuarioSociedadCentro = new UsuarioSociedadCentro(cnnSql);

            try
            {
                cnnSql.Open();
                return _usuarioSociedadCentro.ListarUsuarioCentro(usuario, codigoSociedad);
            }
            finally
            {
                if (cnnSql.State != ConnectionState.Closed) cnnSql.Close();
            }
        }

        #endregion

        #region Moneda

        public Int32 AlmacenarMoneda(MonedaDTO monedaDTO)
        {
            Int32 x = 0;
            if (_moneda == null) _moneda = new Moneda(cnnSql);

            try
            {
                cnnSql.Open();

                if (!_moneda.ExisteMoneda(monedaDTO.MONEDA))
                {
                    x = _moneda.EjecutarSentenciaInsert(monedaDTO);
                }
                else
                    x = _moneda.EjecutarSenteciaUpdate(monedaDTO);

            }
            finally
            {
                if (cnnSql.State == ConnectionState.Open) cnnSql.Close();
            }

            return x;
        }

        public void DarAltaMoneda(string usuarioModificacion, string moneda)
        {
            if (_moneda == null) _moneda = new Moneda(cnnSql);

            try
            {
                cnnSql.Open();
                _moneda.DarAltaMoneda(usuarioModificacion, moneda);
            }
            finally
            {
                if (cnnSql.State == ConnectionState.Open) cnnSql.Close();
            }
        }

        public void DarBajaMoneda(string usuarioModificacion, string moneda)
        {
            if (_moneda == null) _moneda = new Moneda(cnnSql);

            try
            {
                cnnSql.Open();
                _moneda.DarBajaMoneda(usuarioModificacion, moneda);
            }
            finally
            {
                if (cnnSql.State == ConnectionState.Open) cnnSql.Close();
            }
        }

        public List<MonedaDTO> ListarMonedas()
        {
            if (_moneda == null) _moneda = new Moneda(cnnSql);

            try
            {
                cnnSql.Open();
                return _moneda.ListarMonedas();
            }
            finally
            {
                if (cnnSql.State == ConnectionState.Open) cnnSql.Close();
            }
        }

        public List<MonedaDTO> ListarMonedasActivas()
        {
            if (_moneda == null) _moneda = new Moneda(cnnSql);

            try
            {
                cnnSql.Open();
                return _moneda.ListarMonedasActivas();
            }
            finally
            {
                if (cnnSql.State == ConnectionState.Open) cnnSql.Close();
            }
        }

        #endregion

        #region SociedadMoneda

        public Int32 AgregarSociedadMoneda(List<SociedadMonedaDTO> _listaSociedadMoneda, string usuarioMantenimiento)
        {
            Int32 x = 0;
            if (_sociedadMoneda == null) _sociedadMoneda = new SociedadMoneda(cnnSql);
            SociedadMonedaDTO _sociedadMonedaDto = null;
            TransactionScope ts = null;

            try
            {
                ts = new TransactionScope();
                cnnSql.Open();

                foreach (SociedadMonedaDTO sociedadMonedaDTO in _listaSociedadMoneda)
                {
                    _sociedadMonedaDto = _sociedadMoneda.BuscarSociedadMoneda(sociedadMonedaDTO.CODIGO_SOCIEDAD, sociedadMonedaDTO.MONEDA);

                    if (_sociedadMonedaDto == null && sociedadMonedaDTO.ESTADO == Convert.ToBoolean(EstadoEnum.ALTA))
                    {
                        sociedadMonedaDTO.USUARIO_MANTENIMIENTO.USUARIO_ALTA = usuarioMantenimiento;
                        sociedadMonedaDTO.ID_SOCIEDAD_MONEDA = _sociedadMoneda.EjecutarSentenciaInsert(sociedadMonedaDTO);
                    }

                    if (_sociedadMonedaDto == null) continue;

                    if (_sociedadMonedaDto.ESTADO != sociedadMonedaDTO.ESTADO)
                    {
                        sociedadMonedaDTO.USUARIO_MANTENIMIENTO.USUARIO_MODIFICO = usuarioMantenimiento;
                        sociedadMonedaDTO.ID_SOCIEDAD_MONEDA = _sociedadMonedaDto.ID_SOCIEDAD_MONEDA;

                        if (sociedadMonedaDTO.ESTADO == Convert.ToBoolean(EstadoEnum.BAJA))
                            _sociedadMoneda.DarBajaSociedadMoneda(sociedadMonedaDTO.ID_SOCIEDAD_MONEDA, usuarioMantenimiento);
                        else
                            _sociedadMoneda.DarAltaSociedadMoneda(sociedadMonedaDTO.ID_SOCIEDAD_MONEDA, usuarioMantenimiento);
                    }
                }
                ts.Complete();
            }
            finally
            {
                if (ts != null) ts.Dispose();
                if (cnnSql.State == ConnectionState.Open) cnnSql.Close();
            }
            return x;
        }

        public List<SociedadMonedaDTO> ListarSociedadMoneda(string codigoSociedad)
        {
            if (_sociedadMoneda == null) _sociedadMoneda = new SociedadMoneda(cnnSql);
            try
            {
                cnnSql.Open();
                return _sociedadMoneda.ListarSociedadMoneda(codigoSociedad);
            }
            finally
            {
                if (cnnSql.State != ConnectionState.Closed) cnnSql.Close();
            }
        }

        public List<LlenarDDL_DTO> ListarMonedaAsociada(string codigoSociedad)
        {
            if (_sociedadMoneda == null) _sociedadMoneda = new SociedadMoneda(cnnSql);

            try
            {
                cnnSql.Open();
                return _sociedadMoneda.ListarMonedaAsociada(codigoSociedad);
            }
            finally
            {
                if (cnnSql.State != ConnectionState.Closed) cnnSql.Close();
            }
        }

        #endregion

        #region IDisposable Members

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
    }
}