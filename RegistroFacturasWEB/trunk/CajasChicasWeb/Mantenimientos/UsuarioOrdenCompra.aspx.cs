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

namespace RegistroFacturasWEB.Mantenimientos
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

        protected void btnGrabar_Click(object sender, EventArgs e)
        {
            Agregar();
        }

        protected void gvUsuarioOrdenCompra_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName.Equals("Agregar"))
                AltaOrdenCompra(e);

            if (e.CommandName.Equals("Quitar"))
                BajaOrdenCompra(e);
        }

        protected void gvUsuarioOrdenCompra_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            this.gvUsuarioOrdenCompra.PageIndex = e.NewPageIndex;
            CargarGrid();
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

        private void CargarGrid()
        {
            UsuarioOrdenCostoDTO _usuarioordencompraDto = new UsuarioOrdenCostoDTO();
            GestorSociedad gestorusuarioordencompra = null;

            int x = 1;
            try
            {
                gestorusuarioordencompra = GestorSociedad();
                gvUsuarioOrdenCompra.DataSource = (from usuarioordencompra in gestorusuarioordencompra.ListarUsuarioOrdenCompra(txtUsuario.Text)
                                                   select new
                                                   {
                                                       NUMERO = x++,
                                                       ID_USUARIO_ORDEN_COMPRA = usuarioordencompra.ID_USUARIO_ORDEN_COMPRA,
                                                       CODIGO_SOCIEDAD = usuarioordencompra.ID_SOCIEDAD_CENTRO,
                                                       NOMBRE_SOCIEDAD = usuarioordencompra.NOMBRE_SOCIEDAD,
                                                       CODIGO_CENTRO = usuarioordencompra.ID_CENTRO,
                                                       NOMBRE_CENTRO = usuarioordencompra.NOMBRE_CENTRO,
                                                       ORDEN_COSTO = usuarioordencompra.ORDEN_COSTO,
                                                       DESCRIPCION = usuarioordencompra.KTEXT,
                                                       idAltacc = usuarioordencompra.ALTA,
                                                       USUARIO_CREACION = usuarioordencompra.USUARIO_MANTENIMIENTO.USUARIO_ALTA,
                                                       FECHA_CREACION = usuarioordencompra.USUARIO_MANTENIMIENTO.FECHA_ALTA.ToShortDateString(),
                                                       USUARIO_MODIFICACION = usuarioordencompra.USUARIO_MANTENIMIENTO.USUARIO_MODIFICO,
                                                       FECHA_MODIFICACION = string.IsNullOrEmpty(usuarioordencompra.USUARIO_MANTENIMIENTO.FECHA_MODIFICACION.ToString()) ? string.Empty : Convert.ToDateTime(usuarioordencompra.USUARIO_MANTENIMIENTO.FECHA_MODIFICACION).ToShortDateString()
                                                   }).ToArray();
                gvUsuarioOrdenCompra.DataBind();
            }
            catch
            {
                DesplegarError("Error al Desplegar la Orden de Compra por Usuarios");
            }
            finally
            {
                if (gestorusuarioordencompra != null) gestorusuarioordencompra.Dispose();
            }
        }

        private void AltaOrdenCompra(GridViewCommandEventArgs e)
        {
            int indice = Int32.Parse(e.CommandArgument.ToString());
            GridViewRow fila = gvUsuarioOrdenCompra.Rows[indice];
            GestorSociedad gestorusuarioordencompra = null;

            try
            {
                gestorusuarioordencompra = GestorSociedad();
                gestorusuarioordencompra.DarAltaOrdenCompra(Convert.ToInt32(fila.Cells[1].Text), usuario);
                DesplegarAviso("La Orden de Compra fue dada de Alta para El Usuario");
                CargarGrid();
            }
            catch
            {
                DesplegarError("Error al dar de Alta Orden de Compra al Usuario");
            }
            finally
            {
                if (gestorusuarioordencompra != null) gestorusuarioordencompra.Dispose();
            }
        }

        private void BajaOrdenCompra(GridViewCommandEventArgs e)
        {
            int indice = Int32.Parse(e.CommandArgument.ToString());
            GridViewRow fila = gvUsuarioOrdenCompra.Rows[indice];
            GestorSociedad gestorusarioordencompra = null;

            try
            {
                gestorusarioordencompra = GestorSociedad();
                gestorusarioordencompra.DarBajaOrdenCompra(Convert.ToInt32(fila.Cells[1].Text), usuario);
                DesplegarAviso("La Orden de Compra fue dada de baja para el Usuario");
                CargarGrid();
            }
            catch
            {
                DesplegarError("Error al dar de Baja la Orden de Compra para el Usuario");
            }
            finally
            {
                if (gestorusarioordencompra != null) gestorusarioordencompra.Dispose();
            }
        }

        private void CargarObjetoUsuarioOrdenCompra(ref UsuarioOrdenCostoDTO _usuarioordencompraDTO)
        {
            _usuarioordencompraDTO.ID_USUARIO_ORDEN_COMPRA = Convert.ToInt32(hfIdUsuarioOrdenCompra.Value);
            _usuarioordencompraDTO.USUARIO = txtUsuario.Text;
            _usuarioordencompraDTO.ORDEN_COSTO = hfIdOrdenCosto.Value;
            _usuarioordencompraDTO.USUARIO_MANTENIMIENTO.USUARIO_ALTA = usuario;
            _usuarioordencompraDTO.USUARIO_MANTENIMIENTO.USUARIO_MODIFICO = usuario;
            _usuarioordencompraDTO.ID_SOCIEDAD_CENTRO = Convert.ToInt32(hfIdCentro.Value);
            _usuarioordencompraDTO.USUARIO_MANTENIMIENTO.USUARIO_MODIFICO = hfIdUsuarioOrdenCompra.Value == "0" ? null : txtUsuario.Text.Trim();
        }

        private void Agregar()
        {
            UsuarioOrdenCostoDTO _usuarioordencompraDto = new UsuarioOrdenCostoDTO();
            GestorSociedad gestorusuarioordencompra = null;
            OcultarAvisos();

            CargarDDLCentro(hfIdSociedad.Value);
            CargarDDLOrdenCosto(hfIdSociedad.Value);

            ddlCentro.SelectedValue = hfIdCentro.Value;
            ddlOrdenCosto.SelectedValue = hfIdOrdenCosto.Value;

            try
            {
                if ((txtUsuario.Text != "") & (ddlOrdenCosto.SelectedValue != "-1"))
                {
                    gestorusuarioordencompra = GestorSociedad();
                    CargarObjetoUsuarioOrdenCompra(ref _usuarioordencompraDto);
                    gestorusuarioordencompra.AgregarUsuarioOrdenCompra(_usuarioordencompraDto);
                    //hfIdUsuarioOrdenCompra.Value = _usuarioordencompraDto.ID_USUARIO_ORDEN_COMPRA.ToString();
                    DesplegarAviso("La Orden de Compra fue asignada al usuario");
                    CargarGrid();
                }
                else
                    DesplegarError("Debe Completar los datos correctamente");
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
                if (gestorusuarioordencompra != null) gestorusuarioordencompra.Dispose();
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
                ddlOrdenCosto.Items.Clear();
                ddlOrdenCosto.Items.Add(new ListItem("::ELIJA UNA OPCION::", "-1"));
                ddlOrdenCosto.AppendDataBoundItems = true;
                ddlOrdenCosto.DataSource = _listacentrocostsoDto;
                ddlOrdenCosto.DataTextField = "DESCRIPCION";
                ddlOrdenCosto.DataValueField = "IDENTIFICADOR";
                ddlOrdenCosto.DataBind();
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
        #endregion

        #region Gestor
        protected GestorSociedad GestorSociedad()
        {
            GestorSociedad gestorusuarioordencomrpa = new GestorSociedad(cnn);
            return gestorusuarioordencomrpa;
        }
        #endregion

        protected void btnLimpiar_Click(object sender, EventArgs e)
        {
            Limpiar();
        }

        private void Limpiar()
        {
            OcultarAvisos();
            hfIdUsuarioOrdenCompra.Value = "0";
            hfIdOrdenCosto.Value = "0";
            CargarDDLSociedad();
        }
    }
}