using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using LogicaComun.Enum;
using DipCmiGT.LogicaCajasChicas;
using System.Configuration;
using DipCmiGT.LogicaCajasChicas.Sesion;
using DipCmiGT.LogicaComun;



namespace RegistroFacturasWEB.Mantenimientos
{
    public partial class Sociedad : System.Web.UI.Page
    {

        #region Declaraciones

        string cnnApl = ConfigurationManager.ConnectionStrings["CnnApl"].ToString();
        string usuario;
        bool result;

        #endregion

        #region Eventos

        protected void Page_Load(object sender, EventArgs e)
        {
            usuario = Context.User.Identity.Name.ToString();
            if (!IsPostBack)
            {
                CargarSociedadGrid();
            }
        }

        protected void btnLimpiar_Click(object sender, EventArgs e)
        {
            LimpiarControles();
        }

        protected void btnGrabar_Click(object sender, EventArgs e)
        {
            AlmacenaSociedad();
        }

        protected void gvSociedad_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            this.gvSociedad.PageIndex = e.NewPageIndex;
            CargarSociedadGrid();
        }

        protected void gvSociedad_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName.Equals("Editar"))
            {
                CargarSociedadEdicion(e);
                OcultarAvisos();
            }
            if (e.CommandName.Equals("Baja"))
            {
                OcultarAvisos();
                DarBajaSociedad(e);
            }
            if (e.CommandName.Equals("Alta"))
            {
                OcultarAvisos();
                DarAltaSociedad(e);
            }
        }
        #endregion

        #region Metodos

        public void CargarSociedadGrid()
        {
            SociedadDTO _sociedadDto = new SociedadDTO();
            GestorSociedad gestorSociedad = null;

            int x = 1;
            try
            {
                gestorSociedad = Gestorsociedad();
                gvSociedad.DataSource = (from sociedad in gestorSociedad.ListaSociedad()
                                         select new
                                          {
                                              NUMERO = x++,
                                              CODIGO_SOCIEDAD = sociedad.CODIGO_SOCIEDAD,
                                              NOMBRE = sociedad.NOMBRE,
                                              MESESFACTURA = sociedad.MESES_FACTURA,
                                              PAIS = sociedad.PAIS,
                                              MONEDA = sociedad.MONEDA,
                                              idAlta2 = sociedad.ALTA,
                                              ID_USUARIOALTA = sociedad.USUARIO_MANTENIMIENTO.USUARIO_ALTA,
                                              FECHA_ALTA = sociedad.USUARIO_MANTENIMIENTO.FECHA_ALTA,
                                              ID_USUARIOMODIFICACION = sociedad.USUARIO_MANTENIMIENTO.USUARIO_MODIFICO,
                                              FECHA_MODIFICACION = sociedad.USUARIO_MANTENIMIENTO.FECHA_MODIFICACION,
                                              COMPRA = sociedad.MONTO_COMPRA_CC, 
                                              TOLERANCIA = sociedad.TOLERANCIA_COMPRA_CC,
                                              MANDANTE = sociedad.MANDANTE
                                          }).ToArray();

                gvSociedad.DataBind();


            }
            catch
            {
                DesplegarError("Error al desplegar las sociedades");
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

            hfIdSociedad.Value = "0";
            txtCodigo.Text = string.Empty;
            txtNombre.Text = string.Empty;
            cbAlta.Checked = false;
            txtPais.Value = string.Empty;
            txtMoneda.Value = string.Empty;
            txtValorCompraCC.Value = string.Empty;
            txtTolerancia.Value = string.Empty;
            lblFechaModificacionDB.Text = string.Empty;
            lblFechaAltaBD.Text = string.Empty;
            txtMesesFactura.Text = string.Empty;
            ddlMandante.SelectedItem.Value = "0";
            OcultarAvisos();
        }

        private SociedadDTO CargarObjetoSociedad(ref SociedadDTO _sociedadDto)
        {
            _sociedadDto.NOMBRE = txtNombre.Text;
            _sociedadDto.CODIGO_SOCIEDAD = txtCodigo.Text;
            _sociedadDto.MESES_FACTURA = Convert.ToInt16(txtMesesFactura.Text);
            _sociedadDto.PAIS = txtPais.Value;
            _sociedadDto.MONEDA = txtMoneda.Value;
            _sociedadDto.MONTO_COMPRA_CC = Convert.ToDouble(txtValorCompraCC.Value);
            _sociedadDto.TOLERANCIA_COMPRA_CC = Convert.ToInt32(txtTolerancia.Value);
            _sociedadDto.USUARIO_MANTENIMIENTO.USUARIO_ALTA = usuario;
            //_sociedadDto.USUARIO_MANTENIMIENTO.FECHA_ALTA = Convert.ToDateTime(lblFechaAltaBD.Text);
            _sociedadDto.USUARIO_MANTENIMIENTO.USUARIO_MODIFICO = usuario;
            _sociedadDto.ALTA = cbAlta.Checked;
            _sociedadDto.MANDANTE = ddlMandante.SelectedValue.ToString();
            // _sociedadDto.USUARIO_MANTENIMIENTO.FECHA_MODIFICACION = Convert.ToDateTime(lblFechaModificacionDB.Text);

            return _sociedadDto;
        }

        private bool ValidaCampos()
        {
            Int16 numer;
            if (txtValorCompraCC.Value != "")
            {
                if (txtTolerancia.Value != "")
                {
                    if (ddlMandante.SelectedValue != "0")
                    {
                        if ((txtCodigo.Text != "") && (txtCodigo.Text.Length <= 4))
                        {
                            result = true;
                            if ((txtNombre.Text != "") && (txtNombre.Text.Length <= 50))
                            {
                                result = true;
                                if ((txtMesesFactura.Text != "") && (txtMesesFactura.Text.Length <= 5))
                                {
                                    Boolean Numero = Int16.TryParse(txtMesesFactura.Text, out numer);

                                    if (Numero)
                                    {
                                        result = true;
                                    }
                                    else
                                        result = false;

                                }
                            }
                        }
                    }
                }
            }
            return result;
        }

        private void AlmacenaSociedad()
        {
            SociedadDTO _sociedadDto = new SociedadDTO();
            GestorSociedad gestorSociedad = null;
            OcultarAvisos();
            try
            {
                if (ValidaCampos())
                {
                    gestorSociedad = Gestorsociedad();
                    CargarObjetoSociedad(ref _sociedadDto);
                    gestorSociedad.AlmacenarSociedad(_sociedadDto);
                    hfIdSociedad.Value = _sociedadDto.CODIGO_SOCIEDAD.ToString();
                    DesplegarAviso("La Sociedad fue almacenada Correctamente");
                    CargarSociedadGrid();
                }
                else
                    DesplegarError("Debe Completar los campos correctamente");
            }
            catch (ExcepcionesDIPCMI ex)
            {
                DesplegarError(ex.Message);
            }
            catch
            {
                DesplegarError("Error al almacenar la Sociedad");
            }
            finally
            {
                if (gestorSociedad != null) gestorSociedad.Dispose();
            }
        }

        private void CargarSociedadEdicion(GridViewCommandEventArgs e)
        {
            int indice = Int32.Parse(e.CommandArgument.ToString());
            GridViewRow fila = gvSociedad.Rows[indice];

            hfIdSociedad.Value = HttpUtility.HtmlDecode(fila.Cells[1].Text);

            txtCodigo.Text = HttpUtility.HtmlDecode(fila.Cells[1].Text);
            txtNombre.Text = HttpUtility.HtmlDecode(fila.Cells[2].Text);
            txtMesesFactura.Text = HttpUtility.HtmlDecode(fila.Cells[3].Text);
            txtPais.Value = HttpUtility.HtmlDecode(fila.Cells[4].Text);
            txtMoneda.Value = HttpUtility.HtmlDecode(fila.Cells[5].Text);

            cbAlta.Checked = (((CheckBox)fila.FindControl("idAlta2")).Checked);
            lblUsuarioAltaBD.Text = HttpUtility.HtmlDecode(fila.Cells[7].Text);
            lblFechaAltaBD.Text = HttpUtility.HtmlDecode(fila.Cells[8].Text);
            lblUsuarioModificacionBD.Text = HttpUtility.HtmlDecode(fila.Cells[9].Text);
            lblFechaModificacionDB.Text = HttpUtility.HtmlDecode(fila.Cells[10].Text);

            txtValorCompraCC.Value = HttpUtility.HtmlDecode(fila.Cells[14].Text);
            txtTolerancia.Value = HttpUtility.HtmlDecode(fila.Cells[15].Text);
            ddlMandante.SelectedValue = HttpUtility.HtmlDecode(fila.Cells[16].Text); ;
        }

        private void DarBajaSociedad(GridViewCommandEventArgs e)
        {
            int indice = Int32.Parse(e.CommandArgument.ToString());
            GridViewRow fila = gvSociedad.Rows[indice];
            GestorSociedad gestorSociedad = null;

            try
            {
                gestorSociedad = Gestorsociedad();
                gestorSociedad.DarBajaSociedad(Convert.ToInt16(fila.Cells[1].Text), usuario);

                DesplegarAviso("La sociedad fue dada de baja");
                CargarSociedadGrid();
            }
            catch
            {
                DesplegarError("Error al dar de baja la sociedad");
            }
            finally
            {
                if (gestorSociedad != null) gestorSociedad.Dispose();
            }
        }

        private void DarAltaSociedad(GridViewCommandEventArgs e)
        {
            int indice = Int32.Parse(e.CommandArgument.ToString());
            GridViewRow fila = gvSociedad.Rows[indice];
            GestorSociedad gestorSociedad = null;

            try
            {
                gestorSociedad = Gestorsociedad();
                gestorSociedad.DarAltaSociedad(Convert.ToInt16(fila.Cells[1].Text), usuario);

                DesplegarAviso("La Sociedad fue dada de alta");
                CargarSociedadGrid();
            }
            catch
            {
                DesplegarError("Error al dar de alta la sociedad");
            }
            finally
            {
                if (gestorSociedad != null) gestorSociedad.Dispose();
            }
        }
        #endregion

        #region Gestores

        protected GestorSociedad Gestorsociedad()
        {
            GestorSociedad gestorSeguridad = new GestorSociedad(cnnApl);
            return gestorSeguridad;
        }
        #endregion
    }
}