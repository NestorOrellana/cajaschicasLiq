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
    public partial class Gasto : System.Web.UI.Page
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
                CargarDDLPais();
                CargarGridGastos();
            }
        }

        protected void btnLimpiar_Click(object sender, EventArgs e)
        {
            LimpiarControles();
        }

        protected void btnGrabar_Click(object sender, EventArgs e)
        {
            GuardarGasto();
        }

        protected void gvGastos_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            this.gvGastos.PageIndex = e.NewPageIndex;
            CargarGridGastos();
        }

        protected void gvGastos_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName.Equals("Editar"))
            {
                CargarGastoEdicion(e);
                OcultarAvisos();
            }
            if (e.CommandName.Equals("Baja"))
            {
                OcultarAvisos();
                DarBajaGasto(e);
            }
            if (e.CommandName.Equals("Alta"))
            {
                OcultarAvisos();
                DarAltaGasto(e);
            }
        }
        #endregion

        #region Metodos

        public void CargarGridGastos()
        {
            GastosDTO _gastoDto = new GastosDTO();
            GestorGastos gestorGasto = null;

            int x = 1;
            try
            {
                gestorGasto = GestorGasto();
                gvGastos.DataSource = (from gasto in gestorGasto.ListaGastos()
                                          select new
                                          {
                                              CODIGO_GASTO = gasto.CodigoGasto,
                                              PAIS = gasto.Pais,
                                              GASTO = gasto.Gasto,
                                              idKilometraje = gasto.Kilometraje,
                                              idAlta2 = gasto.Alta,
                                              ID_USUARIOALTA = gasto.USUARIO_MANTENIMIENTO.USUARIO_ALTA,
                                              FECHA_ALTA = gasto.USUARIO_MANTENIMIENTO.FECHA_ALTA,
                                              ID_USUARIOMODIFICACION = gasto.USUARIO_MANTENIMIENTO.USUARIO_MODIFICO,
                                              FECHA_MODIFICACION = gasto.USUARIO_MANTENIMIENTO.FECHA_MODIFICACION,
                                          }).ToArray();

                gvGastos.DataBind();


            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                DesplegarError("Error al desplegar los registros");
            }
            finally
            {
                if (gestorGasto != null) gestorGasto.Dispose();
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

            hfCodigoGasto.Value = "0";
            DropDownListPais.SelectedIndex = 0;
            checkKilometraje.Checked = false;
            cbAlta.Checked = false;
            txtGasto.Value = string.Empty;
            lblFechaModificacionDB.Text = string.Empty;
            lblFechaAltaBD.Text = string.Empty;
            OcultarAvisos();
        }

        private GastosDTO CargarObjetoGasto(ref GastosDTO _gastoDto)
        {
            _gastoDto.CodigoGasto = int.Parse(hfCodigoGasto.Value);
            _gastoDto.Pais = DropDownListPais.SelectedValue;
            _gastoDto.Gasto = txtGasto.Value;
            _gastoDto.Kilometraje = checkKilometraje.Checked;
            _gastoDto.USUARIO_MANTENIMIENTO.USUARIO_ALTA = usuario;
            _gastoDto.USUARIO_MANTENIMIENTO.USUARIO_MODIFICO = usuario;
            _gastoDto.Alta = cbAlta.Checked;

            return _gastoDto;
        }

        private bool ValidaCampos()
        {
            if (DropDownListPais.SelectedIndex != 0)
            {
                if (txtGasto.Value != "" && txtGasto.Value != "0")
                {
                    result = true;
                }
            }
            return result;
        }

        private void GuardarGasto()
        {
            GastosDTO _gastoDto = new GastosDTO();
            GestorGastos gestorGasto = null;
            OcultarAvisos();
            try
            {
                if (ValidaCampos())
                {
                    gestorGasto = GestorGasto();
                    CargarObjetoGasto(ref _gastoDto);
                    gestorGasto.AlmacenarGasto(_gastoDto);
                    hfCodigoGasto.Value = _gastoDto.CodigoGasto.ToString();
                    DesplegarAviso("El gasto fue almacenado Correctamente");
                    CargarGridGastos();
                }
                else
                    DesplegarError("Debe Completar los campos correctamente");
            }
            catch (ExcepcionesDIPCMI ex)
            {
                DesplegarError(ex.Message);
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                DesplegarError("Error al almacenar el gasto");
            }
            finally
            {
                if (gestorGasto != null) gestorGasto.Dispose();
            }
        }

        private void CargarGastoEdicion(GridViewCommandEventArgs e)
        {
            int indice = Int32.Parse(e.CommandArgument.ToString());
            GridViewRow fila = gvGastos.Rows[indice];

            hfCodigoGasto.Value = HttpUtility.HtmlDecode(fila.Cells[0].Text);

            DropDownListPais.SelectedValue = HttpUtility.HtmlDecode(fila.Cells[1].Text);
            checkKilometraje.Checked = (((CheckBox)fila.FindControl("idKilometraje")).Checked);
            txtGasto.Value = HttpUtility.HtmlDecode(fila.Cells[2].Text);

            cbAlta.Checked = (((CheckBox)fila.FindControl("idAlta2")).Checked);
            lblUsuarioAltaBD.Text = HttpUtility.HtmlDecode(fila.Cells[7].Text);
            lblFechaAltaBD.Text = HttpUtility.HtmlDecode(fila.Cells[8].Text);
            lblUsuarioModificacionBD.Text = HttpUtility.HtmlDecode(fila.Cells[9].Text);
            lblFechaModificacionDB.Text = HttpUtility.HtmlDecode(fila.Cells[10].Text);
        }

        private void DarBajaGasto(GridViewCommandEventArgs e)
        {
            int indice = Int32.Parse(e.CommandArgument.ToString());
            GridViewRow fila = gvGastos.Rows[indice];
            GestorGastos gestorGasto = null;

            try
            {
                gestorGasto = GestorGasto();
                gestorGasto.DarBajaGasto(Convert.ToInt16(fila.Cells[0].Text), usuario);

                DesplegarAviso("El gasto fue dado de baja");
                CargarGridGastos();
            }
            catch
            {
                DesplegarError("Error al dar de baja el registro");
            }
            finally
            {
                if (gestorGasto != null) gestorGasto.Dispose();
            }
        }

        private void DarAltaGasto(GridViewCommandEventArgs e)
        {
            int indice = Int32.Parse(e.CommandArgument.ToString());
            GridViewRow fila = gvGastos.Rows[indice];
            GestorGastos gestorGasto = null;

            try
            {
                gestorGasto = GestorGasto();
                gestorGasto.DarAltaGasto(Convert.ToInt16(fila.Cells[0].Text), usuario);

                DesplegarAviso("El gasto se ha dado de alta con exito");
                CargarGridGastos();
            }
            catch
            {
                DesplegarError("Error al dar de alta el gasto");
            }
            finally
            {
                if (gestorGasto != null) gestorGasto.Dispose();
            }
        }

        private void CargarDDLPais()
        {
            GestorGastos gestorGasto = null;
            try
            {
                gestorGasto = GestorGasto();
                List<LlenarDDL_DTO> _listacentroDto = gestorGasto.ListaPaisesDDL();

                DropDownListPais.Items.Clear();
                DropDownListPais.Items.Add(new ListItem("::ELIJA UN PAIS::", "-1"));
                DropDownListPais.AppendDataBoundItems = true;
                DropDownListPais.DataSource = _listacentroDto;
                DropDownListPais.DataTextField = "DESCRIPCION";
                DropDownListPais.DataValueField = "IDENTIFICADOR";
                DropDownListPais.DataBind();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                DesplegarError("Error al recuperar Paises");
            }
            finally
            {
                if (gestorGasto != null) gestorGasto.Dispose();
            }
        }
        #endregion

        #region Gestores

        protected GestorGastos GestorGasto()
        {
            GestorGastos gestorSeguridad = new GestorGastos(cnnApl);
            return gestorSeguridad;
        }
        #endregion
    }
}