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
using System.Text;
using DipCmiGT.LogicaCajasChicas;

namespace RegistroFacturasWEB.Seguridad
{
    public partial class UsuarioSociedadCentro : System.Web.UI.Page
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
                hfUrl.Value = Request.UrlReferrer.ToString();
                txtUsuario.Enabled = false;
                if (!strUrl.Contains('?'))
                    Response.Redirect("~/principal.aspx");

                if (!string.IsNullOrEmpty(strParam))
                {
                    CapturarParametros(strParam);
                    CargarDDLSociedad();
                    //CargarGrid();
                }
                else
                    Response.Redirect("~/principal.aspx");
            }
        }

        protected void btnGrabar_Click(object sender, EventArgs e)
        {
            OcultarAvisos();
            Almacenar();
        }

        protected void btnLimpiar_Click(object sender, EventArgs e)
        {
            Limpiar();
        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            try
            {
                if (!Validar())
                    throw new ExcepcionesDIPCMI("Debe seleccionar una sociedad");

                OcultarAvisos();
                //CargarDDLSociedad();
                //ddlSociedad.SelectedValue = hfIdSociedad.Value;
                //CargarDDLCentro(hfIdSociedad.Value);
                //ddlCentro.SelectedValue = hfIdCentro.Value;
                CargarSociedadCentroUsr();
            }
            catch (ExcepcionesDIPCMI ex)
            {
                DesplegarError(ex.Message);
            }
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

        private void CargarSociedadCentroUsr()
        {
            decimal key = 0;
            GestorSociedad gestorSociedad = null;
            List<UsuarioSociedadCentroDTO> _listaUsuarioSociedadCentro = null;
            List<SociedadCentroDTO> _listaSociedadCentroDto = null;
            Dictionary<Int32, bool> usuarioSociedadCentro = new Dictionary<Int32, bool>();

            try
            {
                gestorSociedad = GestorSociedad();
                _listaUsuarioSociedadCentro = gestorSociedad.ListarUsuarioSociedadCentro(txtUsuario.Text, hfIdSociedad.Value);
                _listaSociedadCentroDto = gestorSociedad.ListaSociedadCentro(hfIdSociedad.Value);

                cblUsuSociedadCentro.DataValueField = "ID_SOCIEDAD_CENTRO";
                cblUsuSociedadCentro.DataTextField = "NOMBRE_CENTRO";
                cblUsuSociedadCentro.DataSource = _listaSociedadCentroDto;
                cblUsuSociedadCentro.DataBind();


                foreach (UsuarioSociedadCentroDTO usuarioSC in _listaUsuarioSociedadCentro)
                    usuarioSociedadCentro[usuarioSC.ID_SOCIEDAD_CENTRO] = _listaSociedadCentroDto.Find(u => u.ID_SOCIEDAD_CENTRO == usuarioSC.ID_SOCIEDAD_CENTRO) == null ? false : true;

                if (_listaUsuarioSociedadCentro.Count > 0)
                {
                    for (int i = 0; i < cblUsuSociedadCentro.Items.Count; i++)
                    {
                        foreach (KeyValuePair<Int32, bool> entry in usuarioSociedadCentro)
                        {
                            if (cblUsuSociedadCentro.Items[i].Value == entry.Key.ToString())
                            {
                                key = Convert.ToInt32(cblUsuSociedadCentro.Items[i].Value);
                                cblUsuSociedadCentro.Items[i].Selected = usuarioSociedadCentro[Convert.ToInt32(key)];
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

        protected GestorSociedad GestorSociedad()
        {
            GestorSociedad gestorSociedad = new GestorSociedad(cnn);
            return gestorSociedad;
        }

        protected GestorCentro GestorCentro()
        {
            GestorCentro gestorCentro = new GestorCentro(cnn);
            return gestorCentro;
        }

        private void Limpiar()
        {
            hfIdCentro.Value = "-1";
            hfIdSociedad.Value = "-1";
            hfIdUsuarioSociedadCentro.Value = "0";
        }

        private void Almacenar()
        {
            List<UsuarioSociedadCentroDTO> _listaUsuarioSociedadCentroDto = null;
            GestorSociedad gestorUsuarioCentroCosto = null;
            Dictionary<Int32, bool> usrSociedadCentro = new Dictionary<Int32, bool>();
            Int32 key = 0;

            try
            {
                gestorUsuarioCentroCosto = GestorSociedad();

                for (int i = 0; i < cblUsuSociedadCentro.Items.Count; i++)
                {
                    key = Convert.ToInt32(cblUsuSociedadCentro.Items[i].Value);
                    usrSociedadCentro[key] = cblUsuSociedadCentro.Items[i].Selected;
                }

                _listaUsuarioSociedadCentroDto = (from id in usrSociedadCentro
                                                  select new UsuarioSociedadCentroDTO
                                                  {
                                                      USUARIO = txtUsuario.Text,
                                                      ID_SOCIEDAD_CENTRO = id.Key,
                                                      CODIGO_SOCIEDAD = hfIdSociedad.Value,
                                                      ALTA = id.Value,
                                                  }).ToList();

                gestorUsuarioCentroCosto.AlmacenarUsuarioSociedadCentro(_listaUsuarioSociedadCentroDto, usuario);// AgregarUsuarioCentroCosto(_listaUsuarioCentroCostoDto, usuario);
                DesplegarAviso("El mapeo de sociedad centro fue operado correctamente al usuario");

                CargarSociedadCentroUsr();

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

        private void ValidarObjeto()
        {
            if ((hfIdCentro.Value == "") || hfIdCentro.Value == "0")
                throw new ExcepcionesDIPCMI("Debe seleccionar un centro");

            if ((hfIdSociedad.Value == "" || hfIdSociedad.Value == "0"))
                throw new ExcepcionesDIPCMI("Debe seleccionar una sociedad");

        }

        private bool Validar()
        {
            if ((ddlSociedad.SelectedValue == "0") || (ddlSociedad.SelectedValue == "-1"))
                return false;

            return true;
        }

        #endregion
    }
}