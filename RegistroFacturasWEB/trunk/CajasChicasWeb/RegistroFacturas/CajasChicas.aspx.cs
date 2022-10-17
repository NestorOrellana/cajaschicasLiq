using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using LogicaComun.Enum;
using DipCmiGT.LogicaCajasChicas;
using DipCmiGT.LogicaComun;
using DipCmiGT.LogicaCajasChicas.Sesion;
using System.Configuration;
using System.Text;
using LogicaCajasChicas.Enum;

namespace RegistroFacturasWEB.RegistroFacturas
{
    public partial class CajasChicas : System.Web.UI.Page
    {
        #region Declaraciones

        string cnnApl = ConfigurationManager.ConnectionStrings["CnnApl"].ToString();
        string usuario;
        string dominio;

        #endregion

        #region Eventos

        protected void Page_Load(object sender, EventArgs e)
        {
            usuario = Context.User.Identity.Name.ToString();
            dominio = (string)Session["dominio"].ToString();
            if (!IsPostBack)
            {
                CargarSociedadDDL();
                CargarTipoOperacion();
                //BuscarCajasChicasSAP();
            }
        }

        protected void btnGrabar_Click(object sender, EventArgs e)
        {
            OcultarAvisos();
            AlmacenarCajaChica();
        }

        #endregion

        #region Metodos

        private void CargarTipoOperacion()
        {
            //List<System.Web.UI.WebControls.ListItem> colors = new List<System.Web.UI.WebControls.ListItem>();
            //colors.Add(new ListItem("::Seleccion una opción::", "0"));
            //colors.Add(new ListItem("CAJA CHICA", TipoOperacionEnum.CC.ToString()));
            //colors.Add(new ListItem("FONDO REVOLVENTE", TipoOperacionEnum.FR.ToString()));
            //colors.Add(new ListItem("KILOMETRAJE", TipoOperacionEnum.KM.ToString()));
            //ddlTipoOperacion.DataSource = colors;
            //ddlTipoOperacion.DataBind();
            ddlTipoOperacion.Items.Insert(0, new ListItem("--Seleccione operación--", "0"));
            ddlTipoOperacion.Items.Insert(1, new ListItem("CAJA CHICA", TipoOperacionEnum.CC.ToString()));
            ddlTipoOperacion.Items.Insert(2, new ListItem("FONDO REVOLVENTE", TipoOperacionEnum.FR.ToString()));
            ddlTipoOperacion.Items.Insert(3, new ListItem("KILOMETRAJE", TipoOperacionEnum.KM.ToString()));
            ddlTipoOperacion.Items.Insert(3, new ListItem("LIQUIDACION", TipoOperacionEnum.LQ.ToString()));
            ddlTipoOperacion.Items.Insert(4, new ListItem("VIÁTICOS LOCALES", TipoOperacionEnum.VL.ToString()));
        }

        private void MostrarCampos(bool mostrar = true) 
        {
            if (mostrar)
            {
                //camposLiquidacion.Style.Add("display", "block");
            }
            else
            {
                //camposLiquidacion.Style.Add("display", "none");
            }
        }

        private void DesplegarAviso(string mensaje)
        {
            divMensaje.Attributes.CssStyle.Add("display", "table");
            divMensaje.InnerHtml = mensaje;
        }

        private void DesplegarError(string mensaje)
        {
            divMensajeError.Attributes.CssStyle.Add("display", "table");
            divMensajeError.InnerHtml = mensaje;
        }

        private void OcultarAvisos()
        {
            divMensaje.Attributes.CssStyle.Add("display", "none");
            divMensajeError.Attributes.CssStyle.Add("display", "none");
        }

        //private void CargarEncargadoCC(string codigoSociedad)
        private void CargarEncargadoCC(string codigoSociedad, string usuario)
        {
            List<LlenarDDL_DTO> listaDDLDto = new List<LlenarDDL_DTO>();
            GestorCajaChica gestorCC = null;
            try
            {
                gestorCC = GestorCajaChica();
                //listaDDLDto = gestorCC.BuscarCajasChicasSAP(codigoSociedad);
                listaDDLDto = gestorCC.BuscarCajasChicasSAP(codigoSociedad, usuario);

                ddlCajaChica.DataSource = listaDDLDto;
                ddlCajaChica.DataTextField = "DESCRIPCION";
                ddlCajaChica.DataValueField = "IDENTIFICADOR";
                ddlCajaChica.DataBind();
                ddlCajaChica.Items.Insert(0, new ListItem("--Seleccione caja chica--", "0"));
            }
            catch
            {
                DesplegarError("Error al recuperar las Sociedades");
            }

            finally
            {
                if (gestorCC != null) gestorCC.Dispose();
            }
        }

        private void CargarMoneda(string codigoSociedad)
        {
            List<LlenarDDL_DTO> listaDDLDto = new List<LlenarDDL_DTO>();
            GestorSociedad gestorSociedad = null;
            try
            {
                gestorSociedad = GestorSociedad();
                listaDDLDto = gestorSociedad.ListarMonedaAsociada(codigoSociedad);

                ddlMoneda.DataSource = listaDDLDto;
                ddlMoneda.DataTextField = "DESCRIPCION";
                ddlMoneda.DataValueField = "IDENTIFICADOR";
                ddlMoneda.DataBind();
                ddlMoneda.Items.Insert(0, new ListItem("--Seleccione Moneda--", "0"));
            }
            catch
            {
                DesplegarError("Error al recuperar las Sociedades");
            }

            finally
            {
                if (gestorSociedad != null) gestorSociedad.Dispose();
            }
        }

        public void CargarCentroDDL(string codigoSociedad)
        {
            List<LlenarDDL_DTO> listaDDLDto = new List<LlenarDDL_DTO>();
            GestorSociedad gestorSociedad = null;

            try
            {
                gestorSociedad = GestorSociedad();

                //listaDDLDto = gestorSociedad.ListarCentrosMapeados(usuario, codigoSociedad);
                listaDDLDto = gestorSociedad.ListarCentrosUsuario(usuario, codigoSociedad);
                ddlCentro.DataSource = listaDDLDto;
                ddlCentro.DataTextField = "DESCRIPCION";
                ddlCentro.DataValueField = "IDENTIFICADOR";
                ddlCentro.DataBind();
                ddlCentro.Items.Insert(0, new ListItem("--Seleccione centro--", "0"));
            }
            catch
            {
                DesplegarError("Error al recuperar los Centros");
            }
            finally
            {

                if (gestorSociedad != null) gestorSociedad.Dispose();
            }
        }

        private void CargarSociedadDDL()
        {
            List<LlenarDDL_DTO> listaDDLDto = new List<LlenarDDL_DTO>();
            GestorSociedad gestorSociedad = null;

            try
            {
                gestorSociedad = GestorSociedad();
                listaDDLDto = gestorSociedad.ListarSociedadesUsuario(usuario, dominio);

                ddlSociedad.DataSource = listaDDLDto;
                ddlSociedad.DataTextField = "DESCRIPCION";
                ddlSociedad.DataValueField = "IDENTIFICADOR";
                ddlSociedad.DataBind();
                ddlSociedad.Items.Insert(0, new ListItem("--Seleccione sociedad--", "0"));
            }
            catch
            {
                DesplegarError("Error al recuperar las Sociedades");
            }

            finally
            {
                if (gestorSociedad != null) gestorSociedad.Dispose();
            }
        }

        private void CargarObjetoCajasChicas(ref  CajaChicaEncabezadoDTO cajaChicaDto, bool viaticosLocales)
        {
            cajaChicaDto.ID_CAJA_CHICA = Convert.ToInt32(hfIdCajaChica.Value);
            cajaChicaDto.ID_SOCIEDAD_CENTRO = Convert.ToInt32(hfCentro.Value);
            cajaChicaDto.CORRELATIVO = 0;
            cajaChicaDto.CAJA_CHICA_SAP = hfCajaChicaSAP.Value;
            cajaChicaDto.DESCRIPCION = txtDescripcion.Text;
            cajaChicaDto.ESTADO = cajaChicaDto.ID_CAJA_CHICA == 0 ? (Int16?)null : cbAlta.Checked ? Convert.ToInt16(1) : Convert.ToInt16(0);
            cajaChicaDto.TIPO_OPERACION = ddlTipoOperacion.SelectedValue;
            cajaChicaDto.USUARIO_MANTENIMIENTO.USUARIO_ALTA = usuario;
            cajaChicaDto.USUARIO_MANTENIMIENTO.USUARIO_MODIFICO = usuario;
            cajaChicaDto.NOMBRE_CC = ddlCajaChica.SelectedItem.Text;
            cajaChicaDto.ID_SOCIEDAD_MONEDA = Convert.ToInt16(hfMoneda.Value);

            if (viaticosLocales)
            {
                cajaChicaDto.FechaInicioViaje = Convert.ToDateTime(txtFechaInicioViaje.Text);
                cajaChicaDto.FechaFinViaje = Convert.ToDateTime(txtFechaFinViaje.Text);
                cajaChicaDto.Objetivo = txtObjetivo.Text;
                cajaChicaDto.NumeroDias = int.Parse(txtNumeroDias.Text);
                cajaChicaDto.ViaticosRecibidos = decimal.Parse(txtViaticosRecibidos.Text);
                cajaChicaDto.IdNivel = Convert.ToInt32(hfNivel.Value);
                cajaChicaDto.ViaticosLocales = true;
            }
            else {
                cajaChicaDto.ViaticosLocales = false;
            }

            return;
        }

        private string ValidarDatos()
        {
            string mensaje = string.Empty;

            if (ddlSociedad.SelectedValue == "0")
                mensaje += "Debe seleccionar una sociedad. <br>";

            if ((hfCentro.Value == "0") || (hfCentro.Value == string.Empty))
                mensaje += "Debe seleccionar un centro. <br>";

            if ((hfMoneda.Value == "0") || (hfMoneda.Value == string.Empty))
                mensaje += "Debe seleccionar una moneda. <br>";

            if (hfCajaChicaSAP.Value == "0")
                mensaje += "Debe seleccionar una caja chica. <br>";

            if (ddlTipoOperacion.SelectedValue == "0")
                mensaje += "Debe seleccionar un tipo de operación. <br>";

            if (txtDescripcion.Text.Equals(string.Empty))
                mensaje += "Debe de escribir la descripción de la caja chica. <br>";

            if (ddlTipoOperacion.SelectedValue == "VL") {

                if (txtFechaInicioViaje.Text.Equals(string.Empty))
                    mensaje += "Debe indicar la fecha de inicio del viaje. <br>";
                else if (txtFechaFinViaje.Text.Equals(string.Empty))
                    mensaje += "Debe indicar la fecha de fin del viaje. <br>";
                else {
                    DateTime fechaInicio = Convert.ToDateTime(txtFechaInicioViaje.Text);
                    DateTime fechaFin = Convert.ToDateTime(txtFechaFinViaje.Text);

                    if (fechaInicio > fechaFin)
                        mensaje += "La fecha de inicio de viaje no puede ser mayor a la fecha de fin. <br>";

                    if (fechaFin < fechaInicio)
                        mensaje += "La fecha de fin de viaje no puede ser menor a la fecha de inicio. <br>";
                }

                if (txtObjetivo.Text.Equals(string.Empty))
                    mensaje += "Debe indicar el objetivo. <br>";

                if (txtNumeroDias.Text.Equals(string.Empty))
                    mensaje += "Debe indicar el numero de dias. <br>";

                if (txtViaticosRecibidos.Text.Equals(string.Empty))
                    mensaje += "Debe indicar los viaticos recibidos. <br>";

                if (ddlNivel.SelectedValue == "0")
                    mensaje += "Debe selccionar el nivel de liquidacion. <br>";
            }

            return mensaje;
        }

        private void AlmacenarCajaChica()
        {
            CajaChicaEncabezadoDTO cajaChicaDto = new CajaChicaEncabezadoDTO();
            GestorCajaChica gestorCajaChica = null;
            string mensaje = string.Empty;

            try
            {
                gestorCajaChica = GestorCajaChica();

                CargarCentroDDL(hfSociedad.Value);
                //CargarEncargadoCC(hfSociedad.Value);
                CargarEncargadoCC(hfSociedad.Value, usuario);
                CargarMoneda(hfSociedad.Value);
                                
                ddlCentro.SelectedValue = hfCentro.Value;
                ddlCajaChica.SelectedValue = hfCajaChicaSAP.Value;
                ddlMoneda.SelectedValue = hfMoneda.Value;

                //ddlCentro.SelectedValue = hfCentro.Value;
                mensaje = ValidarDatos();

                if (!mensaje.Equals(string.Empty))
                    throw new ExcepcionesDIPCMI(mensaje);
                bool viaticosLocales = false;
                if (ddlTipoOperacion.SelectedValue == "VL")
                    viaticosLocales = true;

                CargarObjetoCajasChicas(ref cajaChicaDto, viaticosLocales);

                cajaChicaDto = gestorCajaChica.AlmacenarCajaChica(cajaChicaDto);

                String b64;
                string url;

                if (viaticosLocales)
                {
                    b64 = Convert.ToBase64String(Encoding.UTF8.GetBytes(string.Format(@"idCajaChica={0}
                        &codigoSociedad={1}&idCentro={2}&nombreSociedad={3}&nombreCentro={4}
                        &codigoCajaChica={5}&sociedadCentro={6}&cajaChicaSAP={7}&moneda={8}
                        &nombreCC={9}&nombreCC={10}&fechaInicio={11}&fechaFin{12}
                        &objetivo={13}&numeroDias{14}&viaticosRecibidos{15}&nivel{16}", 
                        cajaChicaDto.ID_CAJA_CHICA, cajaChicaDto.CODIGO_SOCIEDAD, 
                        cajaChicaDto.CODIGO_CENTRO, cajaChicaDto.NOMBRE_EMPRESA, 
                        cajaChicaDto.NOMBRE_CENTRO, cajaChicaDto.CODIGO_CC, hfCentro.Value, 
                        hfCajaChicaSAP.Value, cajaChicaDto.MONEDA, cajaChicaDto.NOMBRE_CC, cajaChicaDto.PAIS,
                        cajaChicaDto.FechaInicioViaje, cajaChicaDto.FechaFinViaje,
                        cajaChicaDto.Objetivo, cajaChicaDto.NumeroDias, cajaChicaDto.ViaticosRecibidos, 
                        cajaChicaDto.IdNivel)));
                    url = string.Concat("~/RegistroFacturas/RegistroLiquidacionViaticos.aspx?", b64);
                }
                else
                {
                    b64 = Convert.ToBase64String(Encoding.UTF8.GetBytes(string.Format("idCajaChica={0}&codigoSociedad={1}&idCentro={2}&nombreSociedad={3}&nombreCentro={4}&codigoCajaChica={5}&sociedadCentro={6}&cajaChicaSAP={7}&moneda={8}&nombreCC={9}&nombreCC={10}", cajaChicaDto.ID_CAJA_CHICA, cajaChicaDto.CODIGO_SOCIEDAD, cajaChicaDto.CODIGO_CENTRO, cajaChicaDto.NOMBRE_EMPRESA, cajaChicaDto.NOMBRE_CENTRO, cajaChicaDto.CODIGO_CC, hfCentro.Value, hfCajaChicaSAP.Value, cajaChicaDto.MONEDA, cajaChicaDto.NOMBRE_CC, cajaChicaDto.PAIS)));
                    url = string.Concat("~/RegistroFacturas/RegistroFacturas.aspx?", b64);
                }

                Response.Redirect(url);
            }
            catch (Exception ex)
            {
                DesplegarError(ex.Message);
            }
            finally
            {
                if (gestorCajaChica != null) gestorCajaChica.Dispose();
            }

        }

        #endregion

        #region Gestores

        private GestorCajaChica GestorCajaChica()
        {
            GestorCajaChica gestorCajaChica = new GestorCajaChica(cnnApl);
            return gestorCajaChica;
        }

        private GestorSociedad GestorSociedad()
        {
            GestorSociedad gestorSociedad = new GestorSociedad(cnnApl);
            return gestorSociedad;
        }

        #endregion
    }
}