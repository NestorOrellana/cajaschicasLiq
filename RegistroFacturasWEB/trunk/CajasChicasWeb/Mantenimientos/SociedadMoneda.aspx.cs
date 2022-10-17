using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using DipCmiGT.LogicaCajasChicas.Sesion;
using DipCmiGT.LogicaComun;
using LogicaCajasChicas;

namespace RegistroFacturasWEB.Mantenimientos
{
    public partial class SociedadMoneda : System.Web.UI.Page
    {
        #region Declaraciones
        string cnnApl = ConfigurationManager.ConnectionStrings["CnnApl"].ToString();
        string usuario;

        #endregion

        #region Eventos

        protected void Page_Load(object sender, EventArgs e)
        {
            usuario = Context.User.Identity.Name.ToString();

            if (!IsPostBack)
                CargarDDLSociedad();

        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            OcultarAvisos();
            CargarMoneda();
        }

        protected void btnGrabar_Click(object sender, EventArgs e)
        {
            OcultarAvisos();
            AlmacenarSociedadMoneda();
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

        private void CargarMoneda()
        {
            string key = string.Empty;
            GestorSociedad gestorSociedad = null;
            List<SociedadMonedaDTO> _listarSociedadMoneda = null;
            List<MonedaDTO> _listarMonedaDto = null;
            Dictionary<string, bool> usuarioCentroCosto = new Dictionary<string, bool>();

            try
            {
                gestorSociedad = GestorSociedad();
                _listarSociedadMoneda = gestorSociedad.ListarSociedadMoneda(ddlSociedad.SelectedValue);
                _listarMonedaDto = gestorSociedad.ListarMonedasActivas();

                cblMonedas.DataValueField = "MONEDA";
                cblMonedas.DataTextField = "DESCRIPCION";
                cblMonedas.DataSource = _listarMonedaDto;
                cblMonedas.DataBind();


                foreach (SociedadMonedaDTO usuarioCC in _listarSociedadMoneda)
                    usuarioCentroCosto[usuarioCC.MONEDA] = _listarMonedaDto.Find(u => u.MONEDA == usuarioCC.MONEDA) == null ? false : true;

                if (_listarSociedadMoneda.Count > 0)
                {
                    for (int i = 0; i < cblMonedas.Items.Count; i++)
                    {
                        foreach (KeyValuePair<string, bool> entry in usuarioCentroCosto)
                        {
                            if (cblMonedas.Items[i].Value == entry.Key)
                            {
                                key = cblMonedas.Items[i].Value;
                                cblMonedas.Items[i].Selected = usuarioCentroCosto[key.ToString()];
                            }
                        }
                    }
                }

            }
            catch
            {
                DesplegarError("Error al desplegar los mapeos del monedas");
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
        }

        private void AlmacenarSociedadMoneda()
        {
            //SociedadMonedaDTO _sociedadMonedaDto = new SociedadMonedaDTO();
            List<SociedadMonedaDTO> _listaSociedadMoneda = null;
            GestorSociedad gestorSociedad = null;
            Dictionary<string, bool> uCentroCosto = new Dictionary<string, bool>();
            string key = string.Empty;

            try
            {
                gestorSociedad = GestorSociedad();

                for (int i = 0; i < cblMonedas.Items.Count; i++)
                {
                    key = cblMonedas.Items[i].Value;
                    uCentroCosto[key.ToString()] = cblMonedas.Items[i].Selected;
                }

                _listaSociedadMoneda = (from id in uCentroCosto
                                        //where uCentroCosto[id]
                                        select new SociedadMonedaDTO
                                        {
                                            ID_SOCIEDAD_MONEDA = 0,
                                            MONEDA = id.Key,
                                            CODIGO_SOCIEDAD = ddlSociedad.SelectedValue,
                                            ESTADO = id.Value
                                        }).ToList();

                gestorSociedad.AgregarSociedadMoneda(_listaSociedadMoneda, usuario);
                DesplegarAviso("El mapeo entre sociedad moneda fue realizado correctamente.");

                CargarMoneda();
            }
            catch (ExcepcionesDIPCMI ex)
            {
                DesplegarError(ex.Message);
            }
            catch
            {
                DesplegarError("Error al mapear sociedad moneda.");
            }
            finally
            {
                if (gestorSociedad != null) gestorSociedad.Dispose();
            }
        }

        #endregion

        #region Gestores

        private GestorSociedad GestorSociedad()
        {
            GestorSociedad gestorsociedad = new GestorSociedad(cnnApl);
            return gestorsociedad;
        }

        #endregion

        protected void btnLimpiar_Click(object sender, EventArgs e)
        {
            LimpiarDatos();
        }


        private void LimpiarDatos()
        {
            OcultarAvisos();
            ddlSociedad.SelectedValue = "-1";

            List<LlenarDDL_DTO> ddl = new List<LlenarDDL_DTO>();

            cblMonedas.DataTextField = "DESCRIPCION";
            cblMonedas.DataValueField = "IDENTIFICADOR";
            cblMonedas.DataSource = ddl;
            cblMonedas.DataBind();
        }



    }
}