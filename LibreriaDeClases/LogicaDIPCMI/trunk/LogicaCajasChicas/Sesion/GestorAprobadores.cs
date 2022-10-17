using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using LogicaCajasChicas.Entidad;
using System.Data;
using DipCmiGT.LogicaCajasChicas;
using DipCmiGT.LogicaCajasChicas.Entidad;
using System.Transactions;
using DipCmiGT.LogicaComun;
using LogicaCajasChicas;
using DipCmiGT.LogicaComun.Enum;

namespace DipCmiGT.LogicaCajasChicas.Sesion
{
    public class GestorAprobadores : IDisposable
    {

        #region Declaraciones
        AprobadorCentroEncabezado _aprobadorCentroEncabezado = null;
        AprobadorCentro _aprobadorCentro = null;
        //SAPCentroCosto _centrocosto = null;
        SAPOrdenesGastos _ordencompra = null;
        Centro _centro = null;
        Nivel _nivel = null;
        DatosCorreo _datosCorreo = null;

        AprobacionFactura _Aprobacionfactura = null;
        FacturaEncabezado _facturaencabezado = null;
        //CajaChica _cajachica = null;

        UsuarioOrdenCompra _usuarioOrdenCompra = null;
        UsuarioCentroCosto _usuarioCentroCosto = null;

        SqlConnection cnnSql = null;
        #endregion

        #region Constructores
        public GestorAprobadores(string conexion)
        {
            cnnSql = new SqlConnection(conexion);
        }
        #endregion

        #region AprobadorCentro
        public AprobadorCentroDTO EjecutarSentenciaSelect(Int32 IdAprobadorCentro)
        {
            if (_aprobadorCentro == null) _aprobadorCentro = new AprobadorCentro(cnnSql);
            try
            {
                cnnSql.Open();
                return _aprobadorCentro.EjecutarSentenciaSelect(IdAprobadorCentro);
            }
            finally
            {
                if (cnnSql.State == ConnectionState.Open) cnnSql.Close();
            }
        }

        public AprobadorCentroDTO AlmacenarAprobadorCentro(AprobadorCentroDTO aprobadorcentroDto)
        {
            Int32 x = 0;
            if (_aprobadorCentro == null) _aprobadorCentro = new AprobadorCentro(cnnSql);

            try
            {
                cnnSql.Open();

                aprobadorcentroDto.ID_APROBADORCENTRO = _aprobadorCentro.ExisteRegistro(aprobadorcentroDto.KOSTL, aprobadorcentroDto.AUFNR, aprobadorcentroDto.ID_SOCIEDAD_CENTRO, aprobadorcentroDto.ID_NIVEL, aprobadorcentroDto.ID_USUARIO);
                    //throw new ExcepcionesDIPCMI("Detalle del Aprobador Centro ya se Encuentra Registrado.");

                    if (aprobadorcentroDto.ID_APROBADORCENTRO == 0) // || (!_aprobadorcentro.ExisteRegistro(aprobadorcentroDto.KOSTL, aprobadorcentroDto.AUFNR, aprobadorcentroDto.ID_CENTRO, aprobadorcentroDto.ID_USUARIO, aprobadorcentroDto.ID_NIVEL)))
                    {
                        x = _aprobadorCentro.EjecutarSentenciaInsert(aprobadorcentroDto);
                    }
                    else
                        x = _aprobadorCentro.EjecutarSentenciaUpdate(aprobadorcentroDto);
            }
            finally
            {
                if (cnnSql.State == ConnectionState.Open) cnnSql.Close();
            }

            return aprobadorcentroDto;
        }

        public List<AprobadorCentroDTO> ListaAprobadorCentro(Int32 IdUsuario)
        {
            if (_aprobadorCentro == null) _aprobadorCentro = new AprobadorCentro(cnnSql);
            List<AprobadorCentroDTO> _aprobadorcentroDto = new List<AprobadorCentroDTO>();

            try
            {
                cnnSql.Open();
                return _aprobadorCentro.ListaAprobadorCentro(IdUsuario);
            }
            finally
            {
                if (cnnSql.State != ConnectionState.Closed) cnnSql.Close();
            }
        }

        public List<AprobadorCentroDTO> BuscarAprobadoresCentroCosto(Int32 idSociedadCentro, Int32 idUsuario, Int16 idNivel)
        {
            if (_aprobadorCentro == null) _aprobadorCentro = new AprobadorCentro(cnnSql);
            List<AprobadorCentroDTO> _aprobadorcentroDto = new List<AprobadorCentroDTO>();

            try
            {
                cnnSql.Open();
                return _aprobadorCentro.BuscarAprobadoresCentroCosto(idSociedadCentro, idUsuario, idNivel);
            }
            finally
            {
                if (cnnSql.State != ConnectionState.Closed) cnnSql.Close();
            }
        }

        public Int32 DarBajaAprobadorCentro(Int32 idAprobadorCentro, string usuario)
        {
            if (_aprobadorCentro == null) _aprobadorCentro = new AprobadorCentro(cnnSql);
            try
            {
                cnnSql.Open();
                return _aprobadorCentro.DarBajaAprobadorCentro(idAprobadorCentro, usuario);
            }
            finally
            {
                if (cnnSql.State != ConnectionState.Closed) cnnSql.Close();
            }

        }

        public Int32 DarAltaAprobadorCentro(Int32 idAprobadorCentro, string usuario)
        {
            if (_aprobadorCentro == null) _aprobadorCentro = new AprobadorCentro(cnnSql);

            try
            {
                cnnSql.Open();
                return _aprobadorCentro.DarAltaAprobadorCentro(idAprobadorCentro, usuario);
            }
            finally
            {
                if (cnnSql.State != ConnectionState.Closed) cnnSql.Close();
            }
        }

        public string BuscarCeco(string ceco, string sociedad)
        {
            if (_aprobadorCentro == null) _aprobadorCentro = new AprobadorCentro(cnnSql);
            try
            {
                cnnSql.Open();
                return _aprobadorCentro.BuscarCecos(ceco, sociedad);
            }
            finally
            {
                if (cnnSql.State == ConnectionState.Open) cnnSql.Close();
            }
        }

        public string BuscarOrden(string orden, string sociedad)
        {
            if (_aprobadorCentro == null) _aprobadorCentro = new AprobadorCentro(cnnSql);
            try
            {
                cnnSql.Open();
                return _aprobadorCentro.BuscarOrden(orden, sociedad);
            }
            finally
            {
                if (cnnSql.State == ConnectionState.Open) cnnSql.Close();
            }
        }
        #endregion

        #region AprobadorCentroEncabezado

        public List<AprobadorCentroEncabezadoDTO> ListaAprobadorCentroEncabezado(Int32 IdUsuario)
        {
            if (_aprobadorCentroEncabezado == null) _aprobadorCentroEncabezado = new AprobadorCentroEncabezado(cnnSql);
            List<AprobadorCentroEncabezadoDTO> _aprobadorcentroencabezadoDTO = new List<AprobadorCentroEncabezadoDTO>();

            try
            {
                cnnSql.Open();
                return _aprobadorCentroEncabezado.ListaAprobadorCentroEncabezado(IdUsuario);
            }
            finally
            {
                if (cnnSql.State != ConnectionState.Closed) cnnSql.Close();
            }
        }

        public AprobadorCentroEncabezadoDTO AlmacenarAprobadorCentroEncabezado(AprobadorCentroEncabezadoDTO aprobadorcentroEncabezadoDto)
        {
            Int32 x = 0;
            TransactionScope scope = null;
            if (_aprobadorCentro == null) _aprobadorCentro = new AprobadorCentro(cnnSql);
            if (_aprobadorCentroEncabezado == null) _aprobadorCentroEncabezado = new AprobadorCentroEncabezado(cnnSql);

            try
            {
                cnnSql.Open();

                if (_aprobadorCentroEncabezado.ExisteAprobadorCentroEncabezado(aprobadorcentroEncabezadoDto.ID_USUARIO, aprobadorcentroEncabezadoDto.PORCENTAJE_COMPRA, aprobadorcentroEncabezadoDto.TOLERANCIA))
                    throw new ExcepcionesDIPCMI("El Aprobador Centro ya se Encuentra Registrado.");

                if (aprobadorcentroEncabezadoDto.ID_APROBACION_ENCABEZADO == 0)
                {
                    x = _aprobadorCentroEncabezado.EjecutarSentenciaInsert(aprobadorcentroEncabezadoDto);
                }
                else
                {
                    if (aprobadorcentroEncabezadoDto.ALTA == false)
                    {
                        scope = new TransactionScope();

                        x = _aprobadorCentroEncabezado.EjecutarSentenciaUpdate(aprobadorcentroEncabezadoDto);

                        x = _aprobadorCentro.DarBajaAprobadorCentroEncabezado(aprobadorcentroEncabezadoDto.ID_APROBACION_ENCABEZADO, aprobadorcentroEncabezadoDto.USUARIO_MANTENIMIENTO.USUARIO_MODIFICO);
                        scope.Complete();

                    }
                    else
                        x = _aprobadorCentroEncabezado.EjecutarSentenciaUpdate(aprobadorcentroEncabezadoDto);
                }
            }
            finally
            {
                if (cnnSql.State == ConnectionState.Open) cnnSql.Close();
                if (scope != null) scope.Dispose();
            }

            return aprobadorcentroEncabezadoDto;
        }

        public Int32 DarBajaAprobadorCentroEncabezado(Int32 idAprobadorCentro, string usuario)
        {
            if (_aprobadorCentro == null) _aprobadorCentro = new AprobadorCentro(cnnSql);
            if (_aprobadorCentroEncabezado == null) _aprobadorCentroEncabezado = new AprobadorCentroEncabezado(cnnSql);
            TransactionScope scope = null;
            Int32 x = 0;
            try
            {
                scope = new TransactionScope();
                cnnSql.Open();

                //Da baja al aprobador encabezado 
                x = _aprobadorCentroEncabezado.DarBajaAprobadorCentroEncabezado(idAprobadorCentro, usuario);


                //Da baja a todos los detalles del aprobador encabezado 
                x = _aprobadorCentro.DarBajaAprobadorCentroEncabezado(idAprobadorCentro, usuario);
                scope.Complete();
                return x;
            }
            finally
            {
                if (cnnSql.State != ConnectionState.Closed) cnnSql.Close();
                if (scope != null) scope.Dispose();
            }
        }

        public Int16 DarAltaAprobadorCentroEncabezado(Int32 idAprobadorCentro, string usuario)
        {
            if (_aprobadorCentroEncabezado == null) _aprobadorCentroEncabezado = new AprobadorCentroEncabezado(cnnSql);

            try
            {
                cnnSql.Open();
                return _aprobadorCentroEncabezado.DarAltaAprobadorCentroEncabezado(idAprobadorCentro, usuario);
            }
            finally
            {
                if (cnnSql.State != ConnectionState.Closed) cnnSql.Close();
            }
        }



        #endregion

        #region OrdenCompra

        public List<SAPOrdenesGastosDTO> ListaOrdenCompra()
        {
            if (_ordencompra == null) _ordencompra = new SAPOrdenesGastos(cnnSql);
            try
            {
                cnnSql.Open();
                return _ordencompra.ListaOrdenCompraDDL();

            }
            finally
            {
                if (cnnSql.State != ConnectionState.Closed) cnnSql.Close();
            }
        }

        public List<UsuarioOrdenCostoDTO> ListaOrdenCompraUsuario(string Usuario)
        {
            if (_usuarioOrdenCompra == null) _usuarioOrdenCompra = new UsuarioOrdenCompra(cnnSql);
            try
            {
                cnnSql.Open();
                return _usuarioOrdenCompra.ListaUsuarioOrdenCompra(Usuario);
            }
            finally
            {
                if (cnnSql.State != ConnectionState.Closed) cnnSql.Close();
            }
        }
        #endregion

        #region CentroCosto
        public List<UsuarioCentroCostoDTO> ListaCentroCostoUsuario(string Usuario)
        {
            if (_usuarioCentroCosto == null) _usuarioCentroCosto = new UsuarioCentroCosto(cnnSql);
            try
            {
                cnnSql.Open();
                return null;//_usuarioCentroCosto.ListaCentroCostoUsuario(Usuario);
            }
            finally
            {
                if (cnnSql.State != ConnectionState.Closed) cnnSql.Close();
            }
        }
        #endregion

        #region Centro

        public List<CentroDTO> ListaCentro()
        {
            if (_centro == null) _centro = new Centro(cnnSql);
            try
            {
                cnnSql.Open();
                return _centro.ListaCentro();
            }
            finally
            {
                if (cnnSql.State != ConnectionState.Closed) cnnSql.Close();
            }

        }
        #endregion

        #region Nivel

        public List<NivelDTO> ListaNivel()
        {
            if (_nivel == null) _nivel = new Nivel(cnnSql);
            try
            {
                cnnSql.Open();
                return _nivel.ListaNivel();
            }
            finally
            {
                if (cnnSql.State != ConnectionState.Closed) cnnSql.Close();
            }
        }

        public List<LlenarDDL_DTO> ListarNivelesDDL()
        {
            if (_nivel == null) _nivel = new Nivel(cnnSql);
            try
            {
                cnnSql.Open();
                return _nivel.ListaNivelDDL();
            }
            finally
            {
                if (cnnSql.State != ConnectionState.Closed) cnnSql.Close();
            }

        }
        #endregion

        #region AprobacionFactura
        public List<SAPOrdenesGastosDTO> ListarOrdenCompraFact()
        {
            if (_ordencompra == null) _ordencompra = new SAPOrdenesGastos(cnnSql);
            try
            {
                cnnSql.Open();
                return _ordencompra.ListaOrdenCompraDDL();
            }
            finally
            {
                if (cnnSql.State != ConnectionState.Closed) cnnSql.Close();
            }
        }
        public List<FacturaEncabezadoDTO> ListaFacturas(string Usuario, string dominio)
        {
            if (_Aprobacionfactura == null) _Aprobacionfactura = new AprobacionFactura(cnnSql);
            List<FacturaEncabezadoDTO> _aprobacionFacturaDto = new List<FacturaEncabezadoDTO>();

            try
            {
                cnnSql.Open();
                return _Aprobacionfactura.ListaEncabezadoFactura(Usuario, dominio);
            }
            finally
            {
                if (cnnSql.State != ConnectionState.Closed) cnnSql.Close();
            }
        }
        public List<FacturaEncabezadoDTO> BuscarFacturas(string usuario, string codSociedad, Int32 idSociedadCentro, Int16 Nivel, string CentroCosto, string OrdenCosto, string estado, string dominio, string registrador)
        {
            if (_Aprobacionfactura == null) _Aprobacionfactura = new AprobacionFactura(cnnSql);
            List<FacturaEncabezadoDTO> _aprobacionFacturaDto = new List<FacturaEncabezadoDTO>();

            try
            {
                cnnSql.Open();
                return _Aprobacionfactura.BuscarFacturas(usuario, codSociedad, idSociedadCentro, Nivel, CentroCosto, OrdenCosto, estado, dominio, registrador);
            }
            finally
            {
                if (cnnSql.State != ConnectionState.Closed) cnnSql.Close();
            }
        }
        public AprobacionFacturasDTO AlmacenarRegistro(AprobacionFacturasDTO _aprobacionfacturaDto)
        {
            if (_Aprobacionfactura == null) _Aprobacionfactura = new AprobacionFactura(cnnSql);
            if (_facturaencabezado == null) _facturaencabezado = new FacturaEncabezado(cnnSql);
            TransactionScope scope = null;
            Int32 x = 0;
            try
            {
                scope = new TransactionScope();
                cnnSql.Open();

                //Actualiza Estado en Encabezado Factura
                if (_aprobacionfacturaDto.ESTADO == 1)
                {
                    x = _facturaencabezado.AprobarFactura(_aprobacionfacturaDto.ID_FACTURA, _aprobacionfacturaDto.USUARIO_MANTENIMIENTO.USUARIO_ALTA);
                }
                else
                    x = _facturaencabezado.RechazarLaFactura(_aprobacionfacturaDto.ID_FACTURA, _aprobacionfacturaDto.USUARIO_MANTENIMIENTO.USUARIO_ALTA);

                //Inserta Registro en AprobacionFactura
                if (_Aprobacionfactura.ExisteFactura(_aprobacionfacturaDto.ID_FACTURA))
                {
                    x = _Aprobacionfactura.InsertaRegistroAprobacionFactura(_aprobacionfacturaDto);
                }
                else
                    x = _Aprobacionfactura.EjecutarSentenciaUpdateFactura(_aprobacionfacturaDto);

                scope.Complete();
            }
            finally
            {
                if (cnnSql.State != ConnectionState.Closed) cnnSql.Close();
                if (scope != null) scope.Dispose();
            }

            return _aprobacionfacturaDto;
        }
        public List<AprobadorCentroDTO> BuscarAprobadores(Int32 idSociedadCentro, string centroCosto, string ordenCosto)
        {
            if (_aprobadorCentro == null) _aprobadorCentro = new AprobadorCentro(cnnSql);

            try
            {
                cnnSql.Open();

                return _aprobadorCentro.BuscarAprobadores(idSociedadCentro, centroCosto, ordenCosto);
            }
            finally
            {
                if (cnnSql.State != ConnectionState.Closed) cnnSql.Close();
            }
        }

        public FacturaEncabezadoDTO BuscarFacturaAprobar(string usuario, Int64 idFactura, string dominio)
        {
            List<FacturaEncabezadoDTO> _listaFacEncabezadoDto = null;
            if (_Aprobacionfactura == null) _Aprobacionfactura = new AprobacionFactura(cnnSql);

            try
            {
                cnnSql.Open();

                _listaFacEncabezadoDto = _Aprobacionfactura.BuscarFacturaAprobar(usuario, idFactura, dominio);
            }
            finally
            {
                if (cnnSql.State != ConnectionState.Closed) cnnSql.Close();
            }

            return _listaFacEncabezadoDto.Count > 0 ? _listaFacEncabezadoDto[0] : null;
        }
        #endregion
        
        #region DatosCorreo

        public List<DatosCorreoDTO> BuscarAprobaciones()
        {
            if (_datosCorreo == null) _datosCorreo = new DatosCorreo(cnnSql);

            try
            {
                cnnSql.Open();

                return _datosCorreo.BuscarAprobaciones();
            }
            finally
            {
                if (cnnSql.State != ConnectionState.Closed) cnnSql.Close();
            }
        }

        #endregion

        #region IDisposbleMembers

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
