using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Configuration;
using DipCmiGT.LogicaCajasChicas.Sesion;
using DipCmiGT.LogicaCajasChicas;
using DipCmiGT.LogicaComun;
using LogicaCajasChicas;
using LogicaCajasChicas.Enum;
using System.Runtime.Remoting.Contexts;

namespace RegistroFacturasWEB.RegistroFacturas
{
    /// <summary>
    /// Summary description for MapeoUsuariosCentros
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    [System.Web.Script.Services.ScriptService]
    public class MapeoUsuariosCentros : System.Web.Services.WebService
    {

        #region Declaraciones

        private string _cadena = ConfigurationManager.ConnectionStrings["CnnApl"].ConnectionString;
        private Int16 _IVANormal = Convert.ToInt16(ConfigurationManager.AppSettings["IVANormal"]);
        private Int16 _retencionIVA = Convert.ToInt16(ConfigurationManager.AppSettings["RetencionIVA"]);
        private string usuario;

        #endregion

        [WebMethod]
        public List<LlenarDDL_DTO> ListarCajaChicaMapeadas(string sociedad, Int16 centro)
        {
            GestorCajaChica gestorcajachica = null;

            try
            {
                gestorcajachica = GestorCajaChica();

                return gestorcajachica.ListarCajaChica(sociedad, centro);
            }
            finally
            {

                if (gestorcajachica != null) gestorcajachica.Dispose();
            }

        }

        [WebMethod]
        public List<LlenarDDL_DTO> ListarCajaChicaMapeadasDDL(Int32 IdSociedadCentro)
        {
            GestorCajaChica gestorcajachica = null;

            try
            {
                gestorcajachica = GestorCajaChica();

                return gestorcajachica.ListarCajaChicaDDL(IdSociedadCentro);
            }
            finally
            {

                if (gestorcajachica != null) gestorcajachica.Dispose();
            }

        }

        [WebMethod]
        public List<LlenarDDL_DTO> ListarCentroMapeado(string codigoSociedad)
        {
            GestorSociedad gestorsociedad = null;

            try
            {
                gestorsociedad = GestorSociedad();

                return gestorsociedad.ListarCentroMapeado(codigoSociedad);
            }
            finally
            {

                if (gestorsociedad != null) gestorsociedad.Dispose();
            }
        }

        [WebMethod]
        public ProveedorDTO BuscarProveedor(Int16 idTipoDocumento, string numeroIdentificacion)
        {
            GestorProveedor gestorProveedor = null;
            try
            {
                gestorProveedor = GestorProveedor();

                return gestorProveedor.BuscarProveedor(idTipoDocumento, numeroIdentificacion);
            }
            finally
            {
                if (gestorProveedor != null) gestorProveedor.Dispose();
            }
        }


        [WebMethod]
        //public FacturaEncabezadoDTO BuscarTotalFactura(int IdProveedor, decimal NoFactura, string serie)
        public FacturaEncabezadoDTO BuscarTotalFactura(int IdProveedor, string NoFactura, string serie)
        {

                    GestorCajaChica gestorCajaChica = null;
                    try
                    {
                            gestorCajaChica = GestorCajaChica();

                            return gestorCajaChica.BuscarTotalFactura(IdProveedor, NoFactura, serie);
                    }
                    finally
                    {
                        if (gestorCajaChica != null) gestorCajaChica.Dispose();
                    }
        }


        [WebMethod]
        public IndicadoresIVADTO BuscarImporteIVA(string indicadorIVA, string Sociedad)
        {
            GestorCajaChica gestorCajaChica = null;
            try
            {
                gestorCajaChica = GestorCajaChica();

                return gestorCajaChica.BuscarIndicadorIVA(indicadorIVA, Sociedad);
            }
            finally
            {
                if (gestorCajaChica != null) gestorCajaChica.Dispose();
            }
        }

        [WebMethod]
        public double CalcularRetencionIVA(double valorCompra)
        {
            GestorCajaChica gestorCajaChica = null;
            try
            {
                gestorCajaChica = GestorCajaChica();
                return gestorCajaChica.CalcularRetencionIVA(valorCompra, _IVANormal, _retencionIVA);
            }
            finally
            {
                if (gestorCajaChica != null) gestorCajaChica.Dispose();
            }
        }

        [WebMethod]
        public double CalcularRetencionISR(double valorCompra)
        {
            GestorCajaChica gestorCajaChica = null;
            try
            {
                gestorCajaChica = GestorCajaChica();

                return gestorCajaChica.CalcularRetencionISR(valorCompra, TipoImpuestosEnum.ISR_SERVICIO);
            }
            finally
            {
                if (gestorCajaChica != null) gestorCajaChica.Dispose();
            }
        }

        [WebMethod]
        public double Listar(double valorCompra)
        {
            GestorCajaChica gestorCajaChica = null;
            try
            {
                gestorCajaChica = GestorCajaChica();

                return gestorCajaChica.CalcularRetencionISR(valorCompra, TipoImpuestosEnum.ISR_SERVICIO);
            }
            finally
            {
                if (gestorCajaChica != null) gestorCajaChica.Dispose();
            }
        }

        [WebMethod]
        public List<LlenarDDL_DTO> ListarCentroCostoDDL(string codigoSociedad)
        {
            GestorSociedad gestorUCC = null;
            try
            {
                gestorUCC = GestorSociedad();
                return gestorUCC.ListarCentroCostoDDL(codigoSociedad);
            }
            finally
            {
                if (gestorUCC != null) gestorUCC.Dispose();
            }
        }

        [WebMethod]
        public List<LlenarDDL_DTO> ListarCentroCostoUsuarioDDL(string usuario, string codigoSociedad)
        {
            GestorSociedad gestorUCC = null;
            try
            {
                gestorUCC = GestorSociedad();
                return gestorUCC.ListarCentroCostoUsuariosDDL(usuario, codigoSociedad);
            }
            finally
            {
                if (gestorUCC != null) gestorUCC.Dispose();
            }
        }

        [WebMethod]
        public List<LlenarDDL_DTO> ListarOrdenCostoDDL(string codigoSociedad)
        {
            GestorSociedad gestorUCC = null;
            try
            {
                gestorUCC = GestorSociedad();
                return gestorUCC.ListarOrdenCostoDDL(codigoSociedad);
            }
            finally
            {
                if (gestorUCC != null) gestorUCC.Dispose();
            }
        }

        [WebMethod]
        public List<LlenarDDL_DTO> ListarOrdenCostoUsuarioDDL(string usuario, string codigoSociedad)
        {
            GestorSociedad gestorUCC = null;
            try
            {
                gestorUCC = GestorSociedad();
                return gestorUCC.ListarOrdenCostoUsuarioDDL(usuario, codigoSociedad);
            }
            finally
            {
                if (gestorUCC != null) gestorUCC.Dispose();
            }
        }

        [WebMethod]
        public List<LlenarDDL_DTO> ListarCentroCentroCosto(string codigoSociedad)
        {
            usuario = Context.User.Identity.Name.ToString();
            GestorSociedad gestorUCC = null;
            try
            {
                gestorUCC = GestorSociedad();
                return gestorUCC.ListarCentroCentroCosto(usuario, codigoSociedad);
            }
            finally
            {
                if (gestorUCC != null) gestorUCC.Dispose();
            }
        }

        [WebMethod]
        //public List<LlenarDDL_DTO> BuscarCajasChicasSAP(string codigoSociedad)
        public List<LlenarDDL_DTO> BuscarCajasChicasSAP(string codigoSociedad)
        {
            usuario = Context.User.Identity.Name.ToString();
            GestorCajaChica gestorUCC = null;
            try
            {
                gestorUCC = GestorCajaChica();
                //return gestorUCC.BuscarCajasChicasSAP(codigoSociedad);
                return gestorUCC.BuscarCajasChicasSAP(codigoSociedad, usuario);
            }
            finally
            {
                if (gestorUCC != null) gestorUCC.Dispose();
            }
        }


        [WebMethod]
        //public List<LlenarDDL_DTO> BuscarCuentasContablesCentroCosto(string centroCosto, string codigoSociedad)
        public List<LlenarDDL_DTO> BuscarCuentasContablesCentroCosto(string codigoSociedad, string centroCosto)
        {
            usuario = Context.User.Identity.Name.ToString();
            GestorSociedad gestorSociedad = null;
            try
            {
                gestorSociedad = GestorSociedad();

                //return gestorSociedad.BuscarCuentasContablesCentroCosto(centroCosto, codigoSociedad);
                return gestorSociedad.BuscarCuentasContablesCentroCosto(usuario, codigoSociedad, centroCosto);
            }
            finally
            {
                if (gestorSociedad != null) gestorSociedad.Dispose();
            }
        }

        [WebMethod]
        public List<LlenarDDL_DTO> BuscarCuentasContablesOrdenCosto(string ordenCompra, string codigoSociedad)
        {

            GestorSociedad gestorSociedad = null;
            try
            {
                gestorSociedad = GestorSociedad();

                return gestorSociedad.BuscarCuentasContablesOrdenCosto(ordenCompra, codigoSociedad);
            }
            finally
            {
                if (gestorSociedad != null) gestorSociedad.Dispose();
            }
        }


        [WebMethod]
        public ResultadoTransaccionDTO AlmacenarFactura(FacturaEncabezadoDTO facturaEncabezadoDto)
        {
            GestorCajaChica gestorCajaChica = null;
            usuario = Context.User.Identity.Name.ToString();
            ResultadoTransaccionDTO retDto = new ResultadoTransaccionDTO();

            try
            {
                gestorCajaChica = GestorCajaChica();

                facturaEncabezadoDto.TIPO_FACTURA = "CC";
                facturaEncabezadoDto.USUARIO_MANTENIMIENTO.USUARIO_ALTA = usuario;
                facturaEncabezadoDto.USUARIO_MANTENIMIENTO.USUARIO_MODIFICO = facturaEncabezadoDto.ID_FACTURA == 0 ? null : usuario;

                gestorCajaChica.AlmacenarFactura(facturaEncabezadoDto);

                retDto.CODIGO = facturaEncabezadoDto.ID_FACTURA;
                retDto.MENSAJE = "Factura almacenada correctamente.";

                return retDto;
            }
            catch (ExcepcionesDIPCMI ex)
            {
                retDto.CODIGO = 0;
                retDto.MENSAJE = ex.Message;
                return retDto;
            }
            catch (Exception ex)
            {
                retDto.CODIGO = 0;
                retDto.MENSAJE = ex.Message;
                return retDto;
            }
            finally
            {
                if (gestorCajaChica != null) gestorCajaChica.Dispose();
            }


        }

        [WebMethod]
        public List<TipoDocumentoDTO> BuscarTipoDocumento(int IdSociedadCentro)
        {
            GestorProveedor gestorProveedor = null;
            List<TipoDocumentoDTO> listaDDLDto = new List<TipoDocumentoDTO>();

            try
            {
                gestorProveedor = GestorProveedor();
                return gestorProveedor.ListaTipoDocumentoActivo(IdSociedadCentro);
            }
            finally
            {
                if (gestorProveedor != null) gestorProveedor.Dispose();
            }
        }

        [WebMethod]
        public ResultadoTransaccionDTO AlmacenarProveedor(ProveedorDTO proveedorDto)
        {
            ResultadoTransaccionDTO ret = new ResultadoTransaccionDTO();
            GestorProveedor gestorProveedor = null;
            usuario = Context.User.Identity.Name.ToString();

            try
            {
                gestorProveedor = GestorProveedor();

                proveedorDto.USUARIO_MANTENIMIENTO.USUARIO_ALTA = usuario;
                proveedorDto.USUARIO_MANTENIMIENTO.USUARIO_MODIFICO = proveedorDto.ID_PROVEEDOR == 0 ? null : usuario;

                gestorProveedor = GestorProveedor();
                gestorProveedor.AlmacenarProveedor(proveedorDto);

                ret.CODIGO = proveedorDto.ID_PROVEEDOR;
                ret.MENSAJE = "El proveedor fue almacenado correctamente.";

                return ret;
            }
            catch (ExcepcionesDIPCMI ex)
            {
                ret.CODIGO = 0;
                ret.MENSAJE = ex.Message;

                return ret;
            }
            catch (Exception ex)
            {
                ret.CODIGO = 0;
                ret.MENSAJE = ex.Message;

                return ret;
            }
            finally
            {
                if (gestorProveedor != null) gestorProveedor.Dispose();
            }
        }

        [WebMethod]
        public List<LlenarDDL_DTO> ListarCentrosUsuario(string codigoSociedad)
        {
            GestorSociedad gestorUCC = null;
            usuario = Context.User.Identity.Name.ToString();

            try
            {
                gestorUCC = GestorSociedad();
                return gestorUCC.ListarCentrosUsuario(usuario, codigoSociedad);
            }
            finally
            {
                if (gestorUCC != null) gestorUCC.Dispose();
            }
        }

        [WebMethod]
        public List<LlenarDDL_DTO> ListarUsuarioCentro(string codigoSociedad)
        {
            GestorSociedad gestorUCC = null;
            usuario = Context.User.Identity.Name.ToString();

            try
            {
                gestorUCC = GestorSociedad();
                return gestorUCC.ListarUsuarioCentro(usuario, codigoSociedad);
            }
            finally
            {
                if (gestorUCC != null) gestorUCC.Dispose();
            }
        }

        [WebMethod]
        public List<RegistroContableSPDTO> BuscarRegistroContableSP(decimal idFactura)
        {
            GestorCajaChica gestorCC = null;

            try
            {
                gestorCC = GestorCajaChica();
                return gestorCC.BuscarRegistroContableSP(idFactura);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                if (gestorCC != null) gestorCC.Dispose();
            }
        }

        [WebMethod]
        public List<AprobadorCentroDTO> BuscarAprobadores(Int32 idSociedadCentro, string centroCosto, string ordenCosto)
        {
            GestorAprobadores gestorA = null;

            try
            {
                gestorA = GestorAprobadores();
                return gestorA.BuscarAprobadores(idSociedadCentro, centroCosto == "-1" ? "" : centroCosto, ordenCosto == "-1" ? "" : ordenCosto);

            }
            finally
            {
                if (gestorA != null) gestorA.Dispose();
            }

        }

        [WebMethod]
        public List<LlenarDDL_DTO> ListarSociedadMoneda(string codigoSociedad)
        {
            GestorSociedad gestorUCC = null;
            try
            {
                gestorUCC = GestorSociedad();
                return gestorUCC.ListarMonedaAsociada(codigoSociedad);
            }
            finally
            {
                if (gestorUCC != null) gestorUCC.Dispose();
            }
        }

        #region MapeoCuentas

        [WebMethod]
        public List<DominioDTO> BuscarDominio()
        {
            GestorSeguridad gestorSeguridad = null;
            List<DominioDTO> listaDDLDto = new List<DominioDTO>();

            try
            {
                gestorSeguridad = GestorSeguridad();
                return gestorSeguridad.BuscarDominio();
            }
            finally
            {
                if (gestorSeguridad != null) gestorSeguridad.Dispose();
            }
        }

        //[WebMethod]
        //public List<LlenarDDL_DTO> BuscarSociedad(string usuarioB)
        //{
        //    GestorSociedad gestorSociedad = null;
        //    List<LlenarDDL_DTO> listaDDLDto = new List<LlenarDDL_DTO>();

        //    try
        //    {
        //        gestorSociedad = GestorSociedad();
        //        return gestorSociedad.ListarSociedadesUsuario(usuarioB);
        //    }
        //    finally
        //    {
        //        if (gestorSociedad != null) gestorSociedad.Dispose();
        //    }
        //}

        [WebMethod]
        public List<UsuarioDTO> BusquedaUsuario(string usuario, string nombre, string dominio)
        {
            GestorSeguridad gestorSeguridad = null;

            try
            {
                gestorSeguridad = GestorSeguridad();
                return gestorSeguridad.BusquedaUsuario(usuario, nombre, dominio);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                if (gestorSeguridad != null) gestorSeguridad.Dispose();
            }
        }

        //[WebMethod]
        //public List<UsuarioCuentaDTO> BusquedaCuentaUsuario(string usuario, string sociedad, string busqueda)// string dominio)
        //{
        //    GestorSeguridad gestorSeguridad = null;

        //    try
        //    {
        //        gestorSeguridad = GestorSeguridad();
        //        return gestorSeguridad.ListaUsuarioCuenta(sociedad, usuario, busqueda);//, dominio);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception(ex.Message);
        //    }
        //    finally
        //    {
        //        if (gestorSeguridad != null) gestorSeguridad.Dispose();
        //    }
        //}


        #endregion

        #region Gestores

        private GestorSociedad GestorSociedad()
        {
            GestorSociedad gestorSociedad = new GestorSociedad(_cadena);
            return gestorSociedad;
        }

        private GestorProveedor GestorProveedor()
        {
            GestorProveedor gestorPropiedad = new GestorProveedor(_cadena);
            return gestorPropiedad;
        }

        private GestorCajaChica GestorCajaChica()
        {
            GestorCajaChica gestorCajaChica = new GestorCajaChica(_cadena);
            return gestorCajaChica;
        }

        private GestorAprobadores GestorAprobadores()
        {
            GestorAprobadores gestorA = new GestorAprobadores(_cadena);
            return gestorA;
        }


        private GestorSeguridad GestorSeguridad()
        {
            GestorSeguridad gestorSeguridad = new GestorSeguridad(_cadena);
            return gestorSeguridad;
        }

        #endregion
    }
}
