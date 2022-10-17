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

namespace RegistroFacturasWEB.Mantenimientos
{
    public partial class UsuarioCentroCosto : System.Web.UI.Page
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

        protected void btnGrabar_Click(object sender, EventArgs e)
        {
            Agregar();
        }

        protected void gvCentroCosto_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName.Equals("Agregar"))
                AltaCentroCosto(e);

            if (e.CommandName.Equals("Quitar"))
                BajaCentroCosto(e);
        }

        protected void gvCentroCosto_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            this.gvCentroCosto.PageIndex = e.NewPageIndex;
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

                CargarDDLCentro(hfIdCentro.Value);
                CargarDDLCentroCosto(hfIdSociedad.Value);
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

        private void CargarDDLCentroCosto(string codigoSociedad)
        {
            GestorSociedad gestorSociedad = null;

            try
            {
                gestorSociedad = GestorSociedad();

                List<LlenarDDL_DTO> _listacentrocostsoDto = gestorSociedad.ListarCentroCostoDDL(codigoSociedad);
                ddlCentroCosto.Items.Clear();
                ddlCentroCosto.Items.Add(new ListItem("::ELIJA UNA OPCION::", "-1"));
                ddlCentroCosto.AppendDataBoundItems = true;
                ddlCentroCosto.DataSource = _listacentrocostsoDto;
                ddlCentroCosto.DataTextField = "DESCRIPCION";
                ddlCentroCosto.DataValueField = "IDENTIFICADOR";
                ddlCentroCosto.DataBind();
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

        private void CargarGrid()
        {
            UsuarioCentroCostoDTO _usuariocentrocostoDto = new UsuarioCentroCostoDTO();
            GestorSociedad gestorusuariocentrocosto = null;

            int x = 1;
            try
            {
                gestorusuariocentrocosto = GestorSociedad();
                gvCentroCosto.DataSource = (from usuariocentrocosto in gestorusuariocentrocosto.ListarUsuarioCentroCosto(txtUsuario.Text)
                                            select new
                                            {
                                                NUMERO = x++,
                                                ID_USUARIO_CENTRO_COSTO = usuariocentrocosto.ID_USUARIO_CENTRO_COSTO,
                                                CODIGO_SOCIEDAD = usuariocentrocosto.ID_SOCIEDAD_CENTRO,
                                                NOMBRE_SOCIEDAD = usuariocentrocosto.NOMBRE_SOCIEDAD,
                                                CODIGO_CENTRO = usuariocentrocosto.ID_CENTRO,
                                                NOMBRE_CENTRO = usuariocentrocosto.NOMBRE_CENTRO,
                                                CENTRO_COSTO = usuariocentrocosto.CENTRO_COSTO,
                                                DESCRIPCION = usuariocentrocosto.KTEXT,
                                                idAltacc = usuariocentrocosto.ALTA,
                                                USUARIO_CREACION = usuariocentrocosto.USUARIO_MANTENIMIENTO.USUARIO_ALTA,
                                                FECHA_CREACION = usuariocentrocosto.USUARIO_MANTENIMIENTO.FECHA_ALTA.ToShortDateString(),
                                                USUARIO_MODIFICACION = usuariocentrocosto.USUARIO_MANTENIMIENTO.USUARIO_MODIFICO,
                                                FECHA_MODIFICACION = string.IsNullOrEmpty(usuariocentrocosto.USUARIO_MANTENIMIENTO.FECHA_MODIFICACION.ToString()) ? string.Empty : Convert.ToDateTime(usuariocentrocosto.USUARIO_MANTENIMIENTO.FECHA_MODIFICACION).ToShortDateString()
                                            }).ToArray();
                gvCentroCosto.DataBind();
            }
            catch
            {
                DesplegarError("Error al desplegar los mapeos del usuario");
            }
            finally
            {
                if (gestorusuariocentrocosto != null) gestorusuariocentrocosto.Dispose();
            }
        }

        private void AltaCentroCosto(GridViewCommandEventArgs e)
        {
            int indice = Int32.Parse(e.CommandArgument.ToString());
            GridViewRow fila = gvCentroCosto.Rows[indice];
            GestorSociedad gestorusuariocentrocosto = null;

            try
            {
                gestorusuariocentrocosto = GestorSociedad();
                gestorusuariocentrocosto.DarAltaCentroCosto(Convert.ToInt32(fila.Cells[1].Text), txtUsuario.Text.Trim());
                DesplegarAviso("El Centro de Costo fue dado de Alta para El Usuario");
                CargarGrid();
            }
            catch
            {
                DesplegarError("Error al Agregar Centro de Costo al Usuario");
            }
            finally
            {
                if (gestorusuariocentrocosto != null) gestorusuariocentrocosto.Dispose();
            }
        }

        private void BajaCentroCosto(GridViewCommandEventArgs e)
        {
            int indice = Int32.Parse(e.CommandArgument.ToString());
            GridViewRow fila = gvCentroCosto.Rows[indice];
            GestorSociedad gestorusariocentrocosto = null;

            try
            {
                gestorusariocentrocosto = GestorSociedad();
                gestorusariocentrocosto.DarBajaCentroCosto(Convert.ToInt32(fila.Cells[1].Text), txtUsuario.Text.Trim());
                DesplegarAviso("El Centro de Costo fue dado de Baja para el Usuario");
                CargarGrid();
            }
            catch
            {
                DesplegarError("Error al Actualizar el Centro de Costo al Usuario");
            }
            finally
            {
                if (gestorusariocentrocosto != null) gestorusariocentrocosto.Dispose();
            }
        }

        private void CargarObjetoUsuarioCentroCosto(ref UsuarioCentroCostoDTO _usuaricentrocostoDTO)
        {
            _usuaricentrocostoDTO.ID_USUARIO_CENTRO_COSTO = Convert.ToInt32(hfIdUsuarioCentroCosto.Value);
            _usuaricentrocostoDTO.USUARIO = txtUsuario.Text;
            _usuaricentrocostoDTO.CODIGO_SOCIEDAD = ddlSociedad.SelectedValue;
            _usuaricentrocostoDTO.CENTRO_COSTO = hfIdCentroCosto.Value;
            _usuaricentrocostoDTO.USUARIO_MANTENIMIENTO.USUARIO_ALTA = txtUsuario.Text.Trim();
            _usuaricentrocostoDTO.USUARIO_MANTENIMIENTO.USUARIO_MODIFICO = txtUsuario.Text.Trim();
            _usuaricentrocostoDTO.ID_SOCIEDAD_CENTRO = Convert.ToInt32(hfIdCentro.Value);
            _usuaricentrocostoDTO.USUARIO_MANTENIMIENTO.USUARIO_MODIFICO = hfIdUsuarioCentroCosto.Value == "0" ? null : txtUsuario.Text.Trim();
        }

        private void Agregar()
        {
            UsuarioCentroCostoDTO _usuariocentrocostoDto = new UsuarioCentroCostoDTO();
            GestorSociedad gestorusuariocentrocosto = null;
            OcultarAvisos();

            CargarDDLCentro(hfIdSociedad.Value);
            CargarDDLCentroCosto(hfIdSociedad.Value);

            ddlCentro.SelectedValue = hfIdCentro.Value;
            ddlCentroCosto.SelectedValue = hfIdCentroCosto.Value;

            try
            {
                if (txtUsuario.Text != "")
                {
                    gestorusuariocentrocosto = GestorSociedad();
                    CargarObjetoUsuarioCentroCosto(ref _usuariocentrocostoDto);
                    gestorusuariocentrocosto.AgregarUsuarioCentroCosto(_usuariocentrocostoDto);
                    //hfIdUsuarioCentroCosto.Value = _usuariocentrocostoDto.ID_USUARIO_CENTRO_COSTO.ToString();
                    DesplegarAviso("El centro costo fue asignado al usuario");

                    CargarGrid();
                }
                else
                    DesplegarError("Debe completar los datos correctamente");
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
                if (gestorusuariocentrocosto != null) gestorusuariocentrocosto.Dispose();
            }
        }
        #endregion

        #region Gestor

        protected GestorSociedad GestorSociedad()
        {
            GestorSociedad gestorSociedad = new GestorSociedad(cnn);
            return gestorSociedad;
        }
        #endregion

        protected void btnLimpiar_Click(object sender, EventArgs e)
        {
            Limpiar();
        }

        private void Limpiar()
        {
            OcultarAvisos();
            hfIdUsuarioCentroCosto.Value = "0";
            hfIdCentroCosto.Value = "0";
            CargarDDLSociedad();
        }
    }
}