using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DipCmiGT.LogicaCajasChicas.Sesion;
using System.Text;
using DipCmiGT.LogicaCajasChicas;
using DipCmiGT.LogicaComun;
using System.Configuration;

namespace RegistroFacturasWEB.Seguridad
{
    public partial class AprobadorCentroCosto : System.Web.UI.Page
    {
        #region Declaraciones
        string cnnApl = ConfigurationManager.ConnectionStrings["CnnApl"].ToString();
        string usuario;

        #endregion

        #region Eventos

        protected void Page_Load(object sender, EventArgs e)
        {
            usuario = Context.User.Identity.Name.ToString();
            string strUrl = Request.RawUrl;
            string strParam = strUrl.Substring(strUrl.IndexOf('?') + 1);

            if (!IsPostBack)
            {
                txtUsuario.Enabled = false;
                if (!strUrl.Contains('?'))
                    Response.Redirect("~/principal.aspx");

                if (!string.IsNullOrEmpty(strParam))
                {
                    CapturarParametros(strParam);

                    txtUsuario.Enabled = false;
                    txtUsuario.Text = hfUsuario.Value;
                    CargarDDLSociedad();
                    CargarDDLNivel();
                    OcultarAvisos();
                }
                else
                    Response.Redirect("~/principal.aspx");
            }
        }

        protected void btnGrabar_Click(object sender, EventArgs e)
        {

            OcultarAvisos();
            CargarDDLCentro(hfIdSociedad.Value);
            ddlCentro.SelectedValue = hfIdCentro.Value;
            AlmacenaAprobadorCentro();
        }

        protected void btnLimpiar_Click(object sender, EventArgs e)
        {
            LimpiarControles();
        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            OcultarAvisos();
        }

        #endregion

        #region Metodos

        private void CargarDDLSociedad()
        {
            GestorSociedad gestorSociedad = null;

            try
            {
                gestorSociedad = GestorSociedad();

                List<LlenarDDL_DTO> _listasociedadDto = gestorSociedad.ListaSociedadesActivas();
                ddlSociedad.Items.Clear();
                ddlSociedad.Items.Add(new ListItem("::ELIJA UNA OPCION::", "-1"));
                ddlSociedad.AppendDataBoundItems = true;
                ddlSociedad.DataSource = _listasociedadDto;
                ddlSociedad.DataTextField = "DESCRIPCION";
                ddlSociedad.DataValueField = "IDENTIFICADOR";
                ddlSociedad.DataBind();
            }
            catch
            {
                DesplegarError("Error al recuperar Sociedades");
            }
            finally
            {
                if (gestorSociedad != null) gestorSociedad.Dispose();
            }
        }

        private void CargarDDLCentro(string codSociedad)
        {
            GestorSociedad gestorSociedad = null;

            try
            {
                gestorSociedad = GestorSociedad();

                List<LlenarDDL_DTO> _listacentroDto = gestorSociedad.ListarCentroMapeado(codSociedad);
                ddlCentro.Items.Clear();
                ddlCentro.Items.Add(new ListItem("::ELIJA UNA OPCION::", "-1"));
                ddlCentro.AppendDataBoundItems = true;
                ddlCentro.DataSource = _listacentroDto;
                ddlCentro.DataTextField = "DESCRIPCION";
                ddlCentro.DataValueField = "IDENTIFICADOR";
                ddlCentro.DataBind();
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

        private void DesplegarAviso(string Mensaje)
        {
            divMensaje.Attributes.CssStyle.Add("display", "table");
            divMensaje.InnerHtml = Mensaje;
        }

        private void DesplegarError(string Mensaje)
        {
            divMensajeError.Attributes.CssStyle.Add("display", "table");
            divMensajeError.InnerHtml = Mensaje;
        }

        private void OcultarAvisos()
        {
            divMensaje.Attributes.CssStyle.Add("display", "none");
            divMensajeError.Attributes.CssStyle.Add("display", "none");
        }

        private void LimpiarControles()
        {
            hfIdAprobadorCentro.Value = "0";
            hfIdUsuario.Value = "0";
            hfUsuario.Value = string.Empty;
            //ddlCentroCosto.DataValueField = "-1";
            //ddlOrdenCompra.DataValueField = "-1";
            ddlCentro.DataValueField = "-1";
            ddlNivel.DataValueField = "-1";
            ddlSociedad.DataValueField = "-1";
            //chAlta.Checked = false;
            //lblUsuarioCreacionBD.Text = string.Empty;
            //lblFechaCreacionBD.Text = string.Empty;
            //lblUsuarioDB.Text = string.Empty;
            //lblFechaModificacionDB.Text = string.Empty;
            OcultarAvisos();
        }

        private void AlmacenaAprobadorCentro()
        {
            UsuarioCentroCostoDTO _usuarioCentroCostoDto = new UsuarioCentroCostoDTO();
            List<AprobadorCentroDTO> _listaAprobadorCentro = null;
            GestorAprobadores gestorAprobadores = null;
            Dictionary<decimal, bool> uCentroCosto = new Dictionary<decimal, bool>();
            decimal key = 0;

            CargarDDLCentro(hfIdSociedad.Value);
            ddlCentro.SelectedValue = hfIdCentro.Value;

            try
            {
                gestorAprobadores = GestorAprobadores();

                for (int i = 0; i < cblCentroCosto.Items.Count; i++)
                {
                    key = Convert.ToDecimal(cblCentroCosto.Items[i].Value);
                    uCentroCosto[key] = cblCentroCosto.Items[i].Selected;
                }

                _listaAprobadorCentro = (from id in uCentroCosto
                                         select new AprobadorCentroDTO
                                         {
                                             ID_USUARIO = Convert.ToInt32(hfIdUsuario.Value),
                                             KOSTL = id.Key.ToString(),
                                             ID_SOCIEDAD_CENTRO = Convert.ToInt32(hfIdCentro.Value),
                                             AUFNR = string.Empty,
                                             ID_NIVEL = Convert.ToInt16(ddlNivel.SelectedValue),
                                             ALTA = id.Value
                                         }).ToList();

                //gestorAprobadores.AlmacenarAprobadorCentro(_listaAprobadorCentro, usuario);
                DesplegarAviso("El centro costo operado correctamente al usuario");

                CargarCentroCostoUsuario();

            }
            catch (ExcepcionesDIPCMI ex)
            {
                DesplegarError(ex.Message);
            }
            catch
            {
                DesplegarError("Error al almacenar Aprobador Centro");
            }
            finally
            {
                if (gestorAprobadores != null) gestorAprobadores.Dispose();
            }
        }

        private void CargarDDLNivel()
        {
            GestorAprobadores gestoraprobadores = null;

            try
            {
                gestoraprobadores = GestorAprobadores();

                List<NivelDTO> _listaNivel = gestoraprobadores.ListaNivel();

                ddlNivel.Items.Clear();
                ddlNivel.Items.Add(new ListItem("::ELIJA UNA OPCION::", "-1"));
                ddlNivel.AppendDataBoundItems = true;
                ddlNivel.DataSource = _listaNivel;
                ddlNivel.DataTextField = "NIVEL";
                ddlNivel.DataValueField = "ID_NIVEL";
                ddlNivel.DataBind();
            }
            catch
            {
                DesplegarError("Error al recuperar Niveles");
            }
            finally
            {
                if (gestoraprobadores != null) gestoraprobadores.Dispose();
            }

        }

        private void CapturarParametros(string parametros)
        {
            string strParametros = Encoding.UTF8.GetString(Convert.FromBase64String(parametros));
            string[] argsParam = strParametros.Split('&');
            string[] param;
            string usuario = string.Empty;

            param = argsParam[0].Split('=');
            if (param[0] == "Usuario")
            {
                hfUsuario.Value = param[1];
                param = argsParam[1].Split('=');
                hfIdUsuario.Value = param[1];
                return;
            }
        }

        private string ValidarDatos()
        {
            string mensaje = string.Empty;

            if (txtUsuario.Text == string.Empty)
                mensaje += "Debe Indicar un Aprobador. <br>";

            if ((ddlCentro.SelectedValue == "0") || (ddlCentro.SelectedValue == string.Empty) || (ddlCentro.SelectedValue == "-1"))
                mensaje += "Debe seleccionar un centro. <br>";

            //if (((ddlCentroCosto.SelectedValue == "0") || (ddlCentroCosto.SelectedValue == string.Empty) || (ddlCentroCosto.SelectedValue == "-1")) && ((ddlOrdenCompra.SelectedValue == "0") || (ddlOrdenCompra.SelectedValue == string.Empty) || (ddlOrdenCompra.SelectedValue == "-1")))
            //    mensaje += "Debe seleccionar un centro de costo o un centro de compra. <br>";

            //if ((ddlCentroCosto.SelectedValue == "0") || (ddlCentroCosto.SelectedValue == string.Empty) || (ddlCentroCosto.SelectedValue == "-1"))
            //    mensaje += "Debe seleccionar un centro de costo. <br>";

            //if ((ddlOrdenCompra.SelectedValue == "0") || (ddlOrdenCompra.SelectedValue == string.Empty) || (ddlOrdenCompra.SelectedValue == "-1"))
            //    mensaje += "Debe seleccionar un centro de compra. <br>";

            if ((ddlNivel.SelectedValue == "0") || (ddlNivel.SelectedValue == string.Empty) || (ddlNivel.SelectedValue == "-1"))
                mensaje += "Debe seleccionar un Nivel. <br>";

            return mensaje;
        }

        private void BuscarAprobadorCentroCosto()
        {
            decimal key = 0;
            GestorSociedad gestorSociedad = null;
            List<UsuarioCentroCostoDTO> _listaUsuarioCentroCostoDto = null;
            List<LlenarDDL_DTO> _listaCentroCostoDto = null;
            Dictionary<string, bool> usuarioCentroCosto = new Dictionary<string, bool>();

            try
            {
                gestorSociedad = GestorSociedad();
                _listaUsuarioCentroCostoDto = gestorSociedad.ListarUsuarioCentroCosto(txtUsuario.Text, Convert.ToInt32(hfIdCentro.Value));
                _listaCentroCostoDto = gestorSociedad.ListarCentroCostoDDL(hfIdSociedad.Value, txtCentroCosto.Text.Trim());

                cblCentroCosto.DataValueField = "IDENTIFICADOR";
                cblCentroCosto.DataTextField = "DESCRIPCION";
                cblCentroCosto.DataSource = _listaCentroCostoDto;
                cblCentroCosto.DataBind();


                foreach (UsuarioCentroCostoDTO usuarioCC in _listaUsuarioCentroCostoDto)
                    usuarioCentroCosto[usuarioCC.CENTRO_COSTO] = _listaCentroCostoDto.Find(u => u.IDENTIFICADOR == usuarioCC.CENTRO_COSTO) == null ? false : true;

                if (_listaUsuarioCentroCostoDto.Count > 0)
                {
                    for (int i = 0; i < cblCentroCosto.Items.Count; i++)
                    {
                        foreach (KeyValuePair<string, bool> entry in usuarioCentroCosto)
                        {
                            if (cblCentroCosto.Items[i].Value == entry.Key)
                            {
                                key = Convert.ToDecimal(cblCentroCosto.Items[i].Value);
                                cblCentroCosto.Items[i].Selected = usuarioCentroCosto[key.ToString()];
                            }
                        }
                    }
                }

            }
            catch
            {
                DesplegarError("Error al desplegar los mapeos del usuario");
            }
            finally
            {
                if (gestorSociedad != null) gestorSociedad.Dispose();
            }
        }

        protected void btnBuscar_Click1(object sender, EventArgs e)
        {
            OcultarAvisos();
            CargarDDLSociedad();
            ddlSociedad.SelectedValue = hfIdSociedad.Value;
            CargarDDLCentro(hfIdSociedad.Value);
            ddlCentro.SelectedValue = hfIdCentro.Value;
            CargarCentroCostoUsuario();

        }

        private void CargarCentroCostoUsuario()
        {
            decimal key = 0;
            GestorSociedad gestorSociedad = null;
            GestorAprobadores gestorAprobador = null;
            List<AprobadorCentroDTO> aprobadorCentroDto = null;
            List<LlenarDDL_DTO> _listaCentroCostoDto = null;
            Dictionary<string, bool> usuarioCentroCosto = new Dictionary<string, bool>();

            try
            {
                gestorSociedad = GestorSociedad();
                gestorAprobador = GestorAprobadores();
                aprobadorCentroDto = gestorAprobador.BuscarAprobadoresCentroCosto(Convert.ToInt32(hfIdCentro.Value), Convert.ToInt32(hfIdUsuario.Value), Convert.ToInt16(ddlNivel.SelectedValue));
                _listaCentroCostoDto = gestorSociedad.ListarCentroCostoDDL(hfIdSociedad.Value, txtCentroCosto.Text.Trim());

                cblCentroCosto.DataValueField = "IDENTIFICADOR";
                cblCentroCosto.DataTextField = "DESCRIPCION";
                cblCentroCosto.DataSource = _listaCentroCostoDto;
                cblCentroCosto.DataBind();


                foreach (AprobadorCentroDTO usuarioCC in aprobadorCentroDto)
                    usuarioCentroCosto[usuarioCC.KOSTL] = _listaCentroCostoDto.Find(u => u.IDENTIFICADOR == usuarioCC.KOSTL) == null ? false : true;

                if (aprobadorCentroDto.Count > 0)
                {
                    for (int i = 0; i < cblCentroCosto.Items.Count; i++)
                    {
                        foreach (KeyValuePair<string, bool> entry in usuarioCentroCosto)
                        {
                            if (cblCentroCosto.Items[i].Value == entry.Key)
                            {
                                key = Convert.ToDecimal(cblCentroCosto.Items[i].Value);
                                cblCentroCosto.Items[i].Selected = usuarioCentroCosto[key.ToString()];
                            }
                        }
                    }
                }

            }
            catch
            {
                DesplegarError("Error al desplegar los mapeos del usuario");
            }
            finally
            {
                if (gestorSociedad != null) gestorSociedad.Dispose();
                if (gestorAprobador != null) gestorAprobador.Dispose();
            }
        }

        #endregion

        #region Gestores
        private GestorAprobadores GestorAprobadores()
        {
            GestorAprobadores gestoraprobadores = new GestorAprobadores(cnnApl);
            return gestoraprobadores;
        }

        private GestorSociedad GestorSociedad()
        {
            GestorSociedad gestorsociedad = new GestorSociedad(cnnApl);
            return gestorsociedad;
        }
        #endregion
              
    }
}