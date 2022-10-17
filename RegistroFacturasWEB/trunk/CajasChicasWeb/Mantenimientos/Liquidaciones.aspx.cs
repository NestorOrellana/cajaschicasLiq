using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Configuration;
using System.Web.UI.WebControls;
using DipCmiGT.LogicaCajasChicas;
using DipCmiGT.LogicaCajasChicas.Sesion;
using DipCmiGT.LogicaComun;

namespace RegistroFacturasWEB.Mantenimientos
{
    public partial class Liquidaciones : System.Web.UI.Page
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
                CargarGridLiquidaciones();
            }
        }

        protected void btnLimpiar_Click(object sender, EventArgs e)
        {
            LimpiarControles();
        }

        protected void btnGrabar_Click(object sender, EventArgs e)
        {
            GuardarLiquidacion();
        }

        protected void gvLiquidaciones_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            this.gvLiquidaciones.PageIndex = e.NewPageIndex;
            CargarGridLiquidaciones();
        }

        
        protected void CambioPais(object sender, EventArgs e)
        {
            string pais = DropDownListPais.SelectedValue;
            CargarDDLNiveles(pais);
            CargarDDLTipoGastos(pais);
        }

        protected void gvLiquidaciones_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName.Equals("Editar"))
            {
                CargarLiquidacionEdicion(e);
                OcultarAvisos();
            }
            if (e.CommandName.Equals("Baja"))
            {
                OcultarAvisos();
                DarBajaLiquidacion(e);
            }
            if (e.CommandName.Equals("Alta"))
            {
                OcultarAvisos();
                DarAltaLiquidacion(e);
            }
        }
        #endregion

        #region Metodos

        public void CargarGridLiquidaciones()
        {
            LiquidacionDTO _liquidacionDTO = new LiquidacionDTO();
            GestorLiquidacion gestorLiquidacion = null;

            int x = 1;
            try
            {
                gestorLiquidacion = GestorLiquidacion();
                gvLiquidaciones.DataSource = (from liquidacion in gestorLiquidacion.ListaLiquidaciones()
                                        select new
                                        {
                                            CODIGO_LIQUIDACION = liquidacion.CodigoLiquidacion,
                                            PAIS = liquidacion.Pais,
                                            ID_NIVEL = liquidacion.Nivel,
                                            NIVEL = liquidacion.DescripcionNivel,
                                            ID_GASTO = liquidacion.TipoGasto,
                                            TIPO_GASTO = liquidacion.DescripcionTipoGasto,
                                            MONTO_AUTORIZADO = liquidacion.MontoAutorizado,
                                            idAlta2 = liquidacion.Alta,
                                            ID_USUARIOALTA = liquidacion.USUARIO_MANTENIMIENTO.USUARIO_ALTA,
                                            FECHA_ALTA = liquidacion.USUARIO_MANTENIMIENTO.FECHA_ALTA,
                                            ID_USUARIOMODIFICACION = liquidacion.USUARIO_MANTENIMIENTO.USUARIO_MODIFICO,
                                            FECHA_MODIFICACION = liquidacion.USUARIO_MANTENIMIENTO.FECHA_MODIFICACION,
                                        }).ToArray();

                gvLiquidaciones.DataBind();


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                DesplegarError("Error al desplegar los registros");
            }
            finally
            {
                if (gestorLiquidacion != null) gestorLiquidacion.Dispose();
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

            hfCodigoLiquidacion.Value = "0";
            DropDownListPais.SelectedIndex = 0;
            DropDownListNiveles.SelectedIndex = 0;
            DropDownListTipoGasto.SelectedIndex = 0;
            txtMonto.Text = string.Empty;
            cbAlta.Checked = false;
            lblFechaModificacionDB.Text = string.Empty;
            lblFechaAltaBD.Text = string.Empty;
            lblUsuarioAltaBD.Text = string.Empty;
            lblUsuarioModificacionBD.Text = string.Empty;
            OcultarAvisos();
        }

        private LiquidacionDTO CargarObjetoLiquidacion(ref LiquidacionDTO _liquidacionDTO)
        {
            _liquidacionDTO.CodigoLiquidacion = int.Parse(hfCodigoLiquidacion.Value);
            _liquidacionDTO.Pais = DropDownListPais.SelectedValue;
            _liquidacionDTO.Nivel = int.Parse(DropDownListNiveles.SelectedValue);
            _liquidacionDTO.TipoGasto = int.Parse(DropDownListTipoGasto.SelectedValue);
            _liquidacionDTO.MontoAutorizado = decimal.Parse(txtMonto.Text);
            _liquidacionDTO.USUARIO_MANTENIMIENTO.USUARIO_ALTA = usuario;
            _liquidacionDTO.USUARIO_MANTENIMIENTO.USUARIO_MODIFICO = usuario;
            _liquidacionDTO.Alta = cbAlta.Checked;

            return _liquidacionDTO;
        }

        private bool ValidaCampos()
        {
            if (DropDownListPais.SelectedIndex != 0)
            {
                if (DropDownListNiveles.SelectedIndex  != 0)
                {
                    if (DropDownListTipoGasto.SelectedIndex != 0)
                    {
                        if (!string.IsNullOrEmpty(txtMonto.Text))
                        {
                            result = true;
                        }
                    }
                }
            }
            return result;
        }

        private void GuardarLiquidacion()
        {
            LiquidacionDTO _liquidacionDTO = new LiquidacionDTO();
            GestorLiquidacion gestorLiquidacion = null;
            OcultarAvisos();
            try
            {
                if (ValidaCampos())
                {
                    gestorLiquidacion = GestorLiquidacion();
                    int codigoNivel = int.Parse(DropDownListNiveles.SelectedValue);
                    int codigoGasto = int.Parse(DropDownListTipoGasto.SelectedValue);

                    if (!gestorLiquidacion.ValidaExisteLiquidacion(DropDownListPais.SelectedValue, codigoNivel, codigoGasto))
                    {
                        CargarObjetoLiquidacion(ref _liquidacionDTO);
                        gestorLiquidacion.AlmacenarLiquidacion(_liquidacionDTO);
                        hfCodigoLiquidacion.Value = _liquidacionDTO.CodigoLiquidacion.ToString();
                        LimpiarControles();
                        DesplegarAviso("La liquidacion fue almacenada correctamente");
                        CargarGridLiquidaciones();
                    }
                    else
                        DesplegarError("Ya existe un registro con los mismos datos que intenta guardar");
                }
                else
                    DesplegarError("Debe Completar los campos correctamente");
            }
            catch (ExcepcionesDIPCMI ex)
            {
                DesplegarError(ex.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                DesplegarError("Error al almacenar la liquidacion");
            }
            finally
            {
                if (gestorLiquidacion != null) gestorLiquidacion.Dispose();
            }
        }

        private void CargarLiquidacionEdicion(GridViewCommandEventArgs e)
        {
            int indice = Int32.Parse(e.CommandArgument.ToString());
            GridViewRow fila = gvLiquidaciones.Rows[indice];

            hfCodigoLiquidacion.Value = HttpUtility.HtmlDecode(fila.Cells[0].Text);
            string pais = HttpUtility.HtmlDecode(fila.Cells[1].Text);

            DropDownListPais.SelectedValue = pais;
            CargarDDLNiveles(pais);
            CargarDDLTipoGastos(pais);

            string iNivel = (((HiddenField)fila.FindControl("ID_NIVEL")).Value);
            string idTipoGasto = (((HiddenField)fila.FindControl("ID_GASTO")).Value);

            DropDownListNiveles.SelectedValue = iNivel;
            DropDownListTipoGasto.SelectedValue = idTipoGasto;
            txtMonto.Text = HttpUtility.HtmlDecode(fila.Cells[4].Text);

            cbAlta.Checked = (((CheckBox)fila.FindControl("idAlta2")).Checked);
            lblUsuarioAltaBD.Text = HttpUtility.HtmlDecode(fila.Cells[6].Text);
            lblFechaAltaBD.Text = HttpUtility.HtmlDecode(fila.Cells[7].Text);
            lblUsuarioModificacionBD.Text = HttpUtility.HtmlDecode(fila.Cells[8].Text);
            lblFechaModificacionDB.Text = HttpUtility.HtmlDecode(fila.Cells[9].Text);
        }

        private void DarBajaLiquidacion(GridViewCommandEventArgs e)
        {
            int indice = Int32.Parse(e.CommandArgument.ToString());
            GridViewRow fila = gvLiquidaciones.Rows[indice];
            GestorLiquidacion gestorLiquidacion = null;

            try
            {
                gestorLiquidacion = GestorLiquidacion();
                gestorLiquidacion.DarBajaLiquidacion(Convert.ToInt32(fila.Cells[0].Text), usuario);

                DesplegarAviso("La liquidacion fue dada de baja");
                CargarGridLiquidaciones();
            }
            catch
            {
                DesplegarError("Error al dar de baja la liquidacion");
            }
            finally
            {
                if (gestorLiquidacion != null) gestorLiquidacion.Dispose();
            }
        }

        private void DarAltaLiquidacion(GridViewCommandEventArgs e)
        {
            int indice = Int32.Parse(e.CommandArgument.ToString());
            GridViewRow fila = gvLiquidaciones.Rows[indice];
            GestorLiquidacion gestorLiquidacion = null;

            try
            {
                gestorLiquidacion = GestorLiquidacion();
                gestorLiquidacion.DarAltaLiquidacion(Convert.ToInt32(fila.Cells[0].Text), usuario);

                DesplegarAviso("La liquidacion se ha dado de alta con exito");
                CargarGridLiquidaciones();
            }
            catch
            {
                DesplegarError("Error al dar de alta la liquidacion");
            }
            finally
            {
                if (gestorLiquidacion != null) gestorLiquidacion.Dispose();
            }
        }

        private void CargarDDLPais()
        {
            GestorLiquidacion gestorLiquidacion = null;
            try
            {
                gestorLiquidacion = GestorLiquidacion();
                List<LlenarDDL_DTO> _listacentroDto = gestorLiquidacion.ListaPaisesDDL();

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
                if (gestorLiquidacion != null) gestorLiquidacion.Dispose();
            }
        }

        private void CargarDDLTipoGastos(string pais)
        {
            GestorLiquidacion gestorLiquidacion = null;
            try
            {
                gestorLiquidacion = GestorLiquidacion();
                List<LlenarDDL_DTO> _listacentroDto = gestorLiquidacion.ListaTiposGastos(pais);

                DropDownListTipoGasto.Items.Clear();
                DropDownListTipoGasto.Items.Add(new ListItem("::ELIJA UN TIPO DE GASTO::", "-1"));
                DropDownListTipoGasto.AppendDataBoundItems = true;
                DropDownListTipoGasto.DataSource = _listacentroDto;
                DropDownListTipoGasto.DataTextField = "DESCRIPCION";
                DropDownListTipoGasto.DataValueField = "IDENTIFICADOR";
                DropDownListTipoGasto.DataBind();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                DesplegarError("Error al recuperar Tipos de Gastos");
            }
            finally
            {
                if (gestorLiquidacion != null) gestorLiquidacion.Dispose();
            }
        }

        private void CargarDDLNiveles(string pais)
        {
            GestorLiquidacion gestorLiquidacion = null;
            try
            {
                gestorLiquidacion = GestorLiquidacion();
                List<LlenarDDL_DTO> _listacentroDto = gestorLiquidacion.ListaNivelesLiquidacion(pais);

                DropDownListNiveles.Items.Clear();
                DropDownListNiveles.Items.Add(new ListItem("::ELIJA UN NIVEL::", "-1"));
                DropDownListNiveles.AppendDataBoundItems = true;
                DropDownListNiveles.DataSource = _listacentroDto;
                DropDownListNiveles.DataTextField = "DESCRIPCION";
                DropDownListNiveles.DataValueField = "IDENTIFICADOR";
                DropDownListNiveles.DataBind();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                DesplegarError("Error al recuperar Tipos de Gastos");
            }
            finally
            {
                if (gestorLiquidacion != null) gestorLiquidacion.Dispose();
            }
        }
        #endregion

        #region Gestores

        protected GestorLiquidacion GestorLiquidacion()
        {
            GestorLiquidacion gestorLiquidacion = new GestorLiquidacion(cnnApl);
            return gestorLiquidacion;
        }
        #endregion
    }
}