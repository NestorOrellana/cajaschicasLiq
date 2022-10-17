using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using System.Configuration;
using DipCmiGT.LogicaCajasChicas;
using DipCmiGT.LogicaComun;
using DipCmiGT.LogicaCajasChicas.Sesion;
using DipCmiGT.LogicaComun.Util;
using System.Text;
using System.Runtime.Remoting.Contexts;

namespace RegistroFacturasWEB.Seguridad
{
    public partial class UsuarioCentroCosto : System.Web.UI.Page
    {
        #region Declaraciones
        string cnn = ConfigurationManager.ConnectionStrings["CnnApl"].ToString();
        string usuario;

        #endregion

        #region Eventos
        protected void Page_Load(object sender, EventArgs e)
        {
            string strUrl = Request.RawUrl;
            string strParam = strUrl.Substring(strUrl.IndexOf('?') + 1);
            usuario = Context.User.Identity.Name.ToString();

            if (!IsPostBack)
            {
                txtUsuario.Enabled = false;
                if (!strUrl.Contains('?'))
                    Response.Redirect("~/principal.aspx");

                if (!string.IsNullOrEmpty(strParam))
                {
                    hfUrl.Value = Request.UrlReferrer.ToString();
                    CapturarParametros(strParam);
                    CargarDDLSociedad();
                }
                else
                    Response.Redirect("~/principal.aspx");
            }
        }

        protected void btnGrabar_Click(object sender, EventArgs e)
        {
            try
            {
                OcultarAvisos();

                if (cblCentroCosto.Items.Count == 0)
                    throw new ExcepcionesDIPCMI("No hay centros de costo desplegados");

                Agregar();
            }
            catch (ExcepcionesDIPCMI ex)
            {
                DesplegarError(ex.Message);
            }
        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            try
            {
                OcultarAvisos();

                if (!Validar())
                    throw new ExcepcionesDIPCMI("Debe seleccionar una sociedad o un centro");

                CargarCentroCostoUsuario();
            }
            catch (ExcepcionesDIPCMI ex)
            {
                DesplegarError(ex.Message);
            }

        }

        protected void btnLimpiar_Click(object sender, EventArgs e)
        {
            OcultarAvisos();
            txtCentroCosto.Text = string.Empty;
            Limpiar();
            CargarCentroCostoUsuario();
        }

        #endregion

        #region Metodos

        private void CapturarParametros(string parametros)
        {
            string strParametros = Encoding.UTF8.GetString(Convert.FromBase64String(parametros));
            string[] argsParam = strParametros.Split('&');
            string[] param;
            string usuario = string.Empty;

            param = argsParam[0].Split('=');
            if (param[0] == "Usuario")
            {
                txtUsuario.Text = param[1];
                return;
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

        private void CargarDDLSociedad()
        {
            GestorSociedad gestorSociedad = null;

            try
            {
                gestorSociedad = GestorSociedad();

                List<LlenarDDL_DTO> _listacentrocostsoDto = gestorSociedad.ListarSociedadMapeada();
                ddlSociedad.Items.Clear();
                ddlSociedad.Items.Add(new ListItem("::ELIJA UNA OPCION::", "-1"));
                ddlSociedad.AppendDataBoundItems = true;
                ddlSociedad.DataSource = _listacentrocostsoDto;
                ddlSociedad.DataTextField = "DESCRIPCION";
                ddlSociedad.DataValueField = "IDENTIFICADOR";
                ddlSociedad.DataBind();

                CargarDDLCentro(hfIdCentro.Value);
                //CargarDDLCentroCosto(hfIdSociedad.Value);
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

        private void CargarDDLCentro(string codigoSociedad)
        {
            GestorSociedad gestorSociedad = null;

            try
            {
                gestorSociedad = GestorSociedad();

                List<LlenarDDL_DTO> _listacentrocostsoDto = gestorSociedad.ListarCentroMapeado(codigoSociedad);
                ddlCentro.Items.Clear();
                ddlCentro.Items.Add(new ListItem("::ELIJA UNA OPCION::", "-1"));
                ddlCentro.AppendDataBoundItems = true;
                ddlCentro.DataSource = _listacentrocostsoDto;
                ddlCentro.DataTextField = "DESCRIPCION";
                ddlCentro.DataValueField = "IDENTIFICADOR";
                ddlCentro.DataBind();
            }
            catch
            {
                DesplegarError("Error al recuperar Centros");
            }
            finally
            {
                if (gestorSociedad != null) gestorSociedad.Dispose();
            }
        }

        private void CargarCentroCostoUsuario()
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

        private void Agregar()
        {
            List<UsuarioCentroCostoDTO> _listaUsuarioCentroCostoDto = null;
            GestorSociedad gestorUsuarioCentroCosto = null;
            Dictionary<decimal, bool> uCentroCosto = new Dictionary<decimal, bool>();
            decimal key = 0;

            CargarDDLCentro(hfIdSociedad.Value);
            ddlCentro.SelectedValue = hfIdCentro.Value;

            try
            {
                gestorUsuarioCentroCosto = GestorSociedad();

                for (int i = 0; i < cblCentroCosto.Items.Count; i++)
                {
                    key = Convert.ToDecimal(cblCentroCosto.Items[i].Value);
                    uCentroCosto[key] = cblCentroCosto.Items[i].Selected;
                }

                _listaUsuarioCentroCostoDto = (from id in uCentroCosto
                                               //where uCentroCosto[id]
                                               select new UsuarioCentroCostoDTO
                                               {
                                                   USUARIO = txtUsuario.Text,
                                                   CENTRO_COSTO = id.Key.ToString(),
                                                   ID_SOCIEDAD_CENTRO = Convert.ToInt32(hfIdCentro.Value),
                                                   ALTA = id.Value,
                                               }).ToList();

                gestorUsuarioCentroCosto.AgregarUsuarioCentroCosto(_listaUsuarioCentroCostoDto, usuario);
                DesplegarAviso("El centro costo operado correctamente al usuario");

                CargarCentroCostoUsuario();

            }
            catch (ExcepcionesDIPCMI ex)
            {
                DesplegarError(ex.Message);
            }
            catch
            {
                DesplegarError("Error al asignar centro de costo al usuario");
            }
            finally
            {
                if (gestorUsuarioCentroCosto != null) gestorUsuarioCentroCosto.Dispose();
            }
        }

        private void Limpiar()
        {
            OcultarAvisos();
            hfIdUsuarioCentroCosto.Value = "0";
            hfIdCentroCosto.Value = "0";
            CargarDDLSociedad();
        }

        private bool Validar()
        {
            if ((hfIdSociedad.Value == "0") || (hfIdSociedad.Value == "-1"))
                return false;
            else
            {
                CargarDDLSociedad();
                ddlSociedad.SelectedValue = hfIdSociedad.Value;
                CargarDDLCentro(hfIdSociedad.Value);
            }

            if ((hfIdCentro.Value == "0") || (hfIdCentro.Value == "-1"))
                return false;
            else
                ddlCentro.SelectedValue = hfIdCentro.Value;

            return true;
        }


        #endregion

        #region Gestor

        protected GestorSociedad GestorSociedad()
        {
            GestorSociedad gestorSociedad = new GestorSociedad(cnn);
            return gestorSociedad;
        }
        #endregion
    }
}