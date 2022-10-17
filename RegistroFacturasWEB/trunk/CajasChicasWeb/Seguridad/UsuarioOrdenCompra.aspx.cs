using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
//using LogicaCajasChicas.Sesion;
using System.Configuration;
using DipCmiGT.LogicaCajasChicas;
using DipCmiGT.LogicaComun;
using DipCmiGT.LogicaCajasChicas.Sesion;
using DipCmiGT.LogicaComun.Util;
using System.Text;

namespace RegistroFacturasWEB.Seguridad
{
    public partial class UsuarioOrdenCompra : System.Web.UI.Page
    {
        #region Declaraciones
        string cnn = ConfigurationManager.ConnectionStrings["CnnApl"].ToString();
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
                hfUrl.Value = Request.UrlReferrer.ToString();
                if (!strUrl.Contains('?'))
                    Response.Redirect("~/principal.aspx");

                if (!string.IsNullOrEmpty(strParam))
                {
                    CapturarParametros(strParam);
                    CargarDDLSociedad();
                    //CargarOrdenCostoUsuario();
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

        protected void btnLimpiar_Click(object sender, EventArgs e)
        {
            OcultarAvisos();
            txtOrdenCosto.Text = string.Empty;
            Limpiar();
            hfIdSociedad.Value = "-1";
            hfIdCentro.Value = "-1";
            ddlSociedad.SelectedValue = "-1";
            ddlCentro.SelectedValue = "-1";

            List<LlenarDDL_DTO> ddl = new List<LlenarDDL_DTO>();

            cblCentroCosto.DataTextField = "DESCRIPCION";
            cblCentroCosto.DataValueField = "IDENTIFICADOR";
            cblCentroCosto.DataSource = ddl;
            cblCentroCosto.DataBind();
        }

        private void Limpiar()
        {
            OcultarAvisos();
            hfIdUsuarioOrdenCompra.Value = "0";
            hfIdOrdenCosto.Value = "0";
            CargarDDLSociedad();
        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            try
            {
                OcultarAvisos();

                if (!Validar())
                    throw new ExcepcionesDIPCMI("Debe seleccionar una sociedad o un centro");

                CargarOrdenCostoUsuario();
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

                CargarDDLCentro(hfIdSociedad.Value);
                CargarDDLOrdenCosto(hfIdSociedad.Value);
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

        private void CargarOrdenCostoUsuario()
        {
            decimal key = 0;
            GestorSociedad gestorSociedad = null;
            List<UsuarioOrdenCostoDTO> _listaUsuarioOrdenCostoDto = null;
            List<LlenarDDL_DTO> _listaOrdenCostoDto = null;
            Dictionary<string, bool> usuarioOrdenCosto = new Dictionary<string, bool>();

            try
            {
                gestorSociedad = GestorSociedad();
                _listaUsuarioOrdenCostoDto = gestorSociedad.ListarUsuarioOrdenCosto(txtUsuario.Text, Convert.ToInt32(hfIdCentro.Value));
                _listaOrdenCostoDto = gestorSociedad.ListarOrdenCostoDDL(hfIdSociedad.Value, txtOrdenCosto.Text.Trim());

                cblCentroCosto.DataValueField = "IDENTIFICADOR";
                cblCentroCosto.DataTextField = "DESCRIPCION";
                cblCentroCosto.DataSource = _listaOrdenCostoDto;
                cblCentroCosto.DataBind();

                foreach (UsuarioOrdenCostoDTO usuarioCC in _listaUsuarioOrdenCostoDto)
                {
                    if (!_listaOrdenCostoDto.Exists(x => x.IDENTIFICADOR == usuarioCC.ORDEN_COSTO)) continue;

                    usuarioOrdenCosto[usuarioCC.ORDEN_COSTO] = _listaOrdenCostoDto.Find(u => u.IDENTIFICADOR == usuarioCC.ORDEN_COSTO) == null ? false : true;
                }

                if (_listaUsuarioOrdenCostoDto.Count > 0)
                {
                    for (int i = 0; i < cblCentroCosto.Items.Count; i++)
                    {
                        foreach (KeyValuePair<string, bool> entry in usuarioOrdenCosto)
                        {
                            if (cblCentroCosto.Items[i].Value == entry.Key)
                            {
                                key = Convert.ToDecimal(cblCentroCosto.Items[i].Value);
                                cblCentroCosto.Items[i].Selected = usuarioOrdenCosto[key.ToString().PadLeft(12,'0')];
                            }
                        }
                    }
                }
            }
            catch
            {
                DesplegarError("Error al Desplegar la Orden de Compra por Usuarios");
            }
            finally
            {
                if (gestorSociedad != null) gestorSociedad.Dispose();
            }
        }

        private void Agregar()
        {
            UsuarioOrdenCostoDTO _usuarioOrdenCostoDto = new UsuarioOrdenCostoDTO();
            List<UsuarioOrdenCostoDTO> _listaUsuarioOrdenCostoDto = null;
            GestorSociedad gestorSociedad = null;
            Dictionary<decimal, bool> uOrdenCosto = new Dictionary<decimal, bool>();
            decimal key = 0;

            CargarDDLCentro(hfIdSociedad.Value);
            ddlCentro.SelectedValue = hfIdCentro.Value;

            try
            {
                gestorSociedad = GestorSociedad();

                for (int i = 0; i < cblCentroCosto.Items.Count; i++)
                {
                    key = Convert.ToDecimal(cblCentroCosto.Items[i].Value);
                    uOrdenCosto[key] = cblCentroCosto.Items[i].Selected;
                }

                _listaUsuarioOrdenCostoDto = (from id in uOrdenCosto
                                              //where uCentroCosto[id]
                                              select new UsuarioOrdenCostoDTO
                                              {
                                                  USUARIO = txtUsuario.Text,
                                                  ORDEN_COSTO = id.Key.ToString(),
                                                  ID_SOCIEDAD_CENTRO = Convert.ToInt32(hfIdCentro.Value),
                                                  ALTA = id.Value,
                                              }).ToList();

                gestorSociedad.AgregarUsuarioOrdenCosto(_listaUsuarioOrdenCostoDto, usuario);
                DesplegarAviso("El centro costo operado correctamente al usuario");
                CargarOrdenCostoUsuario();
            }
            catch (ExcepcionesDIPCMI ex)
            {
                DesplegarError(ex.Message);
            }
            catch
            {
                DesplegarError("Error al Asignar Orden de Compra al Usuario");
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

        private void CargarDDLOrdenCosto(string codigoSociedad)
        {
            GestorSociedad gestorSociedad = null;

            try
            {
                gestorSociedad = GestorSociedad();

                List<LlenarDDL_DTO> _listacentrocostsoDto = gestorSociedad.ListarOrdenCostoDDL(codigoSociedad);

            }
            catch
            {
                DesplegarError("Error al recuperar los Centros de Costo");
            }
            finally
            {
                if (gestorSociedad != null) gestorSociedad.Dispose();
            }
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
            GestorSociedad gestorusuarioordencomrpa = new GestorSociedad(cnn);
            return gestorusuarioordencomrpa;
        }
        #endregion

    }
}