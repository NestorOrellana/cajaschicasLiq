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
    public partial class Impuestos : System.Web.UI.Page
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
                CargarGridImpuestos();
            }
        }

        protected void btnLimpiar_Click(object sender, EventArgs e)
        {
            LimpiarControles();
        }

        protected void btnGrabar_Click(object sender, EventArgs e)
        {
            GuardarImpuesto();
        }

        protected void gvImpuestos_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            this.gvImpuestos.PageIndex = e.NewPageIndex;
            CargarGridImpuestos();
        }

        protected void gvImpuestos_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName.Equals("Editar"))
            {
                CargarImpuestoEdicion(e);
                OcultarAvisos();
            }
            if (e.CommandName.Equals("Baja"))
            {
                OcultarAvisos();
                DarBajaImpuesto(e);
            }
            if (e.CommandName.Equals("Alta"))
            {
                OcultarAvisos();
                DarAltaImpuesto(e);
            }
        }
        #endregion

        #region Metodos

        public void CargarGridImpuestos()
        {
            ImpuestoCHDTO _sociedadDto = new ImpuestoCHDTO();
            GestorImpuestoCH gestorImpuesto = null;

            int x = 1;
            try
            {
                gestorImpuesto = GestorImpuestoCH();
                gvImpuestos.DataSource = (from impuesto in gestorImpuesto.ListaImpuestos()
                                         select new
                                          {
                                              CODIGO_SOCIEDAD = impuesto.CodigoImpuesto,
                                              DESCRIPCION = impuesto.Descripcion,
                                              PAIS = impuesto.Pais,
                                              TIPO = impuesto.Tipo,
                                              VALOR = impuesto.Valor,
                                              CUENTA = impuesto.Cuenta,
                                              idAlta2 = impuesto.Alta,
                                              ID_USUARIOALTA = impuesto.USUARIO_MANTENIMIENTO.USUARIO_ALTA,
                                              FECHA_ALTA = impuesto.USUARIO_MANTENIMIENTO.FECHA_ALTA,
                                              ID_USUARIOMODIFICACION = impuesto.USUARIO_MANTENIMIENTO.USUARIO_MODIFICO,
                                              FECHA_MODIFICACION = impuesto.USUARIO_MANTENIMIENTO.FECHA_MODIFICACION,
                                          }).ToArray();

                gvImpuestos.DataBind();


            }
            catch
            {
                DesplegarError("Error al desplegar los registros");
            }
            finally
            {
                if (gestorImpuesto != null) gestorImpuesto.Dispose();
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

            hfCodigoImpuesto.Value = "0";
            DropDownListPais.SelectedIndex = 0;
            DropDownListTipo.SelectedIndex = 0;
            cbAlta.Checked = false;
            txtDescripcion.Value = string.Empty;
            txtValor.Value = string.Empty;
            txtCuenta.Value = string.Empty;
            lblFechaModificacionDB.Text = string.Empty;
            lblFechaAltaBD.Text = string.Empty;
            OcultarAvisos();
        }

        private ImpuestoCHDTO CargarObjetoSociedad(ref ImpuestoCHDTO _impuestoDto)
        {
            _impuestoDto.Pais = DropDownListPais.SelectedValue;
            _impuestoDto.Tipo = DropDownListTipo.SelectedValue;
            _impuestoDto.Descripcion = txtDescripcion.Value;
            _impuestoDto.Valor = Convert.ToDecimal(txtValor.Value);
            _impuestoDto.Cuenta = txtCuenta.Value;
            _impuestoDto.OrdenVisualizacion = Convert.ToInt32(txtOrden.Value);
            _impuestoDto.USUARIO_MANTENIMIENTO.USUARIO_ALTA = usuario;
            _impuestoDto.USUARIO_MANTENIMIENTO.USUARIO_MODIFICO = usuario;
            _impuestoDto.Alta = cbAlta.Checked;

            return _impuestoDto;
        }

        private bool ValidaCampos()
        {
            if (DropDownListPais.SelectedIndex != 0)
            {
                if (DropDownListTipo.SelectedIndex != 0)
                {
                    if (txtDescripcion.Value != "")
                    {
                        if (txtValor.Value != "")
                        {
                            result = true;
                            if (txtCuenta.Value != "")
                            {
                                result = true;
                                if (txtOrden.Value != "")
                                {
                                    result = true;
                                }
                            }
                        }
                    }
                }
            }
            return result;
        }

        private void GuardarImpuesto()
        {
            ImpuestoCHDTO _impuestoDto = new ImpuestoCHDTO();
            GestorImpuestoCH gestorImpuesto = null;
            OcultarAvisos();
            try
            {
                if (ValidaCampos())
                {
                    gestorImpuesto = GestorImpuestoCH();
                    CargarObjetoSociedad(ref _impuestoDto);
                    gestorImpuesto.AlmacenarImpuesto(_impuestoDto);
                    hfCodigoImpuesto.Value = _impuestoDto.CodigoImpuesto.ToString();
                    DesplegarAviso("El impuesto fue almacenado Correctamente");
                    CargarGridImpuestos();
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
                if (gestorImpuesto != null) gestorImpuesto.Dispose();
            }
        }

        private void CargarImpuestoEdicion(GridViewCommandEventArgs e)
        {
            int indice = Int32.Parse(e.CommandArgument.ToString());
            GridViewRow fila = gvImpuestos.Rows[indice];

            hfCodigoImpuesto.Value = HttpUtility.HtmlDecode(fila.Cells[1].Text);

            DropDownListPais.SelectedValue = HttpUtility.HtmlDecode(fila.Cells[2].Text);
            DropDownListTipo.SelectedValue = HttpUtility.HtmlDecode(fila.Cells[3].Text);
            txtDescripcion.Value = HttpUtility.HtmlDecode(fila.Cells[4].Text);
            txtValor.Value = HttpUtility.HtmlDecode(fila.Cells[5].Text);
            txtCuenta.Value = HttpUtility.HtmlDecode(fila.Cells[6].Text);

            cbAlta.Checked = (((CheckBox)fila.FindControl("idAlta2")).Checked);
            lblUsuarioAltaBD.Text = HttpUtility.HtmlDecode(fila.Cells[7].Text);
            lblFechaAltaBD.Text = HttpUtility.HtmlDecode(fila.Cells[8].Text);
            lblUsuarioModificacionBD.Text = HttpUtility.HtmlDecode(fila.Cells[9].Text);
            lblFechaModificacionDB.Text = HttpUtility.HtmlDecode(fila.Cells[10].Text);
        }

        private void DarBajaImpuesto(GridViewCommandEventArgs e)
        {
            int indice = Int32.Parse(e.CommandArgument.ToString());
            GridViewRow fila = gvImpuestos.Rows[indice];
            GestorImpuestoCH gestorImpuesto = null;

            try
            {
                gestorImpuesto = GestorImpuestoCH();
                gestorImpuesto.DarBajaImpuesto(Convert.ToInt16(fila.Cells[1].Text), usuario);

                DesplegarAviso("El registro fue dado de baja");
                CargarGridImpuestos();
            }
            catch
            {
                DesplegarError("Error al dar de baja el registro");
            }
            finally
            {
                if (gestorImpuesto != null) gestorImpuesto.Dispose();
            }
        }

        private void DarAltaImpuesto(GridViewCommandEventArgs e)
        {
            int indice = Int32.Parse(e.CommandArgument.ToString());
            GridViewRow fila = gvImpuestos.Rows[indice];
            GestorImpuestoCH gestorImpuesto = null;

            try
            {
                gestorImpuesto = GestorImpuestoCH();
                gestorImpuesto.DarAltaImpuesto(Convert.ToInt16(fila.Cells[1].Text), usuario);

                DesplegarAviso("El impuesto ");
                CargarGridImpuestos();
            }
            catch
            {
                DesplegarError("Error al dar de alta la sociedad");
            }
            finally
            {
                if (gestorImpuesto != null) gestorImpuesto.Dispose();
            }
        }
        #endregion

        #region Gestores

        protected GestorImpuestoCH GestorImpuestoCH()
        {
            GestorImpuestoCH gestorSeguridad = new GestorImpuestoCH(cnnApl);
            return gestorSeguridad;
        }
        #endregion
    }
}