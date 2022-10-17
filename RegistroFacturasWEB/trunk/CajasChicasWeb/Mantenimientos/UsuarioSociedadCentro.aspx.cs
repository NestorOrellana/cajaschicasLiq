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

namespace RegistroFacturasWEB.Mantenimientos
{
    public partial class UsuarioSociedadCentro : System.Web.UI.Page
    {

        #region Declaraciones
        string cnn = ConfigurationManager.ConnectionStrings["CnnApl"].ToString();

        #endregion

        #region Eventos
        protected void Page_Load(object sender, EventArgs e)
        {
            string strUrl = Request.RawUrl;
            string strParam = strUrl.Substring(strUrl.IndexOf('?') + 1);

            if (!IsPostBack)
            {
                ///
                txtUsuario.Enabled = false;
                if (!strUrl.Contains('?'))
                    Response.Redirect("~/principal.aspx");

                if (!string.IsNullOrEmpty(strParam))
                {
                    CapturarParametros(strParam);
                    CargarDDLSociedad();
                    CargarGrid();
                }
                else
                    Response.Redirect("~/principal.aspx");
            }
        }

        protected void gvUsuarioOrdenCompra_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            this.gvUsuarioOrdenCompra.PageIndex = e.NewPageIndex;
            CargarGrid();
        }
        #endregion

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

        protected void btnGrabar_Click(object sender, EventArgs e)
        {

            Almacenar();
        }

        protected void btnLimpiar_Click(object sender, EventArgs e)
        {
            Limpiar();
        }

        private void Limpiar()
        {
            hfIdCentro.Value = "-1";
            hfIdSociedad.Value = "-1";
            hfIdUsuarioSociedadCentro.Value = "0";
        }

        private void CargarGrid()
        {
            UsuarioSociedadCentroDTO _usuarioordencompraDto = new UsuarioSociedadCentroDTO();
            GestorSociedad gestorSociedad = null;

            int x = 1;
            try
            {
                gestorSociedad = GestorSociedad();
                gvUsuarioOrdenCompra.DataSource = (from usuarioordencompra in gestorSociedad.ListarUsuarioSociedadCentro(txtUsuario.Text)
                                                   select new
                                                   {
                                                       NUMERO = x++,
                                                       ID_USUARIO_ORDEN_COMPRA = usuarioordencompra.ID_USUARIO_SOCIEDAD_CENTRO,
                                                       ID_SOCIEDAD_CENTRO = usuarioordencompra.ID_SOCIEDAD_CENTRO,
                                                       CODIGO_SOCIEDAD = usuarioordencompra.ID_SOCIEDAD_CENTRO,
                                                       NOMBRE_SOCIEDAD = usuarioordencompra.NOMBRE_SOCIEDAD,
                                                       CODIGO_CENTRO = usuarioordencompra.ID_CENTRO,
                                                       NOMBRE_CENTRO = usuarioordencompra.NOMBRE_CENTRO,
                                                       USUARIO_CREACION = usuarioordencompra.USUARIO_MANTENIMIENTO.USUARIO_ALTA,
                                                       FECHA_CREACION = usuarioordencompra.USUARIO_MANTENIMIENTO.FECHA_ALTA.ToShortDateString(),
                                                       USUARIO_MODIFICACION = usuarioordencompra.USUARIO_MANTENIMIENTO.USUARIO_MODIFICO,
                                                       FECHA_MODIFICACION = string.IsNullOrEmpty(usuarioordencompra.USUARIO_MANTENIMIENTO.FECHA_MODIFICACION.ToString()) ? string.Empty : Convert.ToDateTime(usuarioordencompra.USUARIO_MANTENIMIENTO.FECHA_MODIFICACION).ToShortDateString(),
                                                       idAltacc = usuarioordencompra.ALTA
                                                   }).ToArray();
                gvUsuarioOrdenCompra.DataBind();
            }
            catch
            {
                DesplegarError("Error al Desplegar Sociedad Centro por Usuarios");
            }
            finally
            {
                if (gestorSociedad != null) gestorSociedad.Dispose();
            }
        }

        private void Almacenar()
        {
            UsuarioSociedadCentroDTO _usuarioSociedadCentro = new UsuarioSociedadCentroDTO();
            GestorSociedad gestorSociedad = null; ;

            try
            {
                gestorSociedad = GestorSociedad();

                CargarObjetoPantalla(ref _usuarioSociedadCentro);

                gestorSociedad.AlmacenarUsuarioSociedadCentro(_usuarioSociedadCentro);

                Limpiar();

                CargarGrid();
            }
            catch (ExcepcionesDIPCMI ex)
            {
                DesplegarError(ex.Message);
            }

            finally
            {
                if (gestorSociedad != null) gestorSociedad.Dispose();
            }

        }

        private void CargarObjetoPantalla(ref UsuarioSociedadCentroDTO usuarioSociedadCentroDto)
        {
            usuarioSociedadCentroDto.ID_USUARIO_SOCIEDAD_CENTRO = Convert.ToInt32(hfIdUsuarioSociedadCentro.Value);
            usuarioSociedadCentroDto.ID_SOCIEDAD_CENTRO = Convert.ToInt32(hfIdCentro.Value);
            usuarioSociedadCentroDto.USUARIO = txtUsuario.Text;
            usuarioSociedadCentroDto.USUARIO_MANTENIMIENTO.USUARIO_ALTA = Context.User.Identity.Name.ToString();
            usuarioSociedadCentroDto.USUARIO_MANTENIMIENTO.USUARIO_MODIFICO = hfIdUsuarioSociedadCentro.Value == "0" ? null : Context.User.Identity.Name.ToString();
        }

        private void ValidarObjeto()
        {
            if ((hfIdCentro.Value == "") || hfIdCentro.Value == "0")
                throw new ExcepcionesDIPCMI("Debe seleccionar un centro");

            if ((hfIdSociedad.Value == "" || hfIdSociedad.Value == "0"))
                throw new ExcepcionesDIPCMI("Debe seleccionar una sociedad");

        }

    }
}