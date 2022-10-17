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
    public partial class NivelLiquidacion : System.Web.UI.Page
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
                CargarGridNiveles();
            }
        }

        protected void btnLimpiar_Click(object sender, EventArgs e)
        {
            LimpiarControles();
        }

        protected void btnGrabar_Click(object sender, EventArgs e)
        {
            GuardarNivelLiquidacion();
        }

        protected void gvNiveles_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            this.gvNiveles.PageIndex = e.NewPageIndex;
            CargarGridNiveles();
        }

        protected void gvNiveles_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName.Equals("Editar"))
            {
                CargarNivelEdicion(e);
                OcultarAvisos();
            }
            if (e.CommandName.Equals("Baja"))
            {
                OcultarAvisos();
                DarBajaNivel(e);
            }
            if (e.CommandName.Equals("Alta"))
            {
                OcultarAvisos();
                DarAltaNivel(e);
            }
        }
        #endregion

        #region Metodos

        public void CargarGridNiveles()
        {
            NivelLiquidacionDTO _nivelDTO = new NivelLiquidacionDTO();
            GestorNivelLiquidacion gestorNivel = null;

            int x = 1;
            try
            {
                gestorNivel = GestorNivelLiquidacion();
                gvNiveles.DataSource = (from nivel in gestorNivel.ListaNivelesLiquidacion()
                                             select new
                                             {
                                                 CODIGO_NIVEL = nivel.CodigoNivel,
                                                 PAIS = nivel.Pais,
                                                 NIVEL = nivel.Nivel,
                                                 idAlta2 = nivel.Alta,
                                                 ID_USUARIOALTA = nivel.USUARIO_MANTENIMIENTO.USUARIO_ALTA,
                                                 FECHA_ALTA = nivel.USUARIO_MANTENIMIENTO.FECHA_ALTA,
                                                 ID_USUARIOMODIFICACION = nivel.USUARIO_MANTENIMIENTO.USUARIO_MODIFICO,
                                                 FECHA_MODIFICACION = nivel.USUARIO_MANTENIMIENTO.FECHA_MODIFICACION,
                                             }).ToArray();

                gvNiveles.DataBind();


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                DesplegarError("Error al desplegar los registros");
            }
            finally
            {
                if (gestorNivel != null) gestorNivel.Dispose();
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

            hfCodigoNivel.Value = "0";
            DropDownListPais.SelectedIndex = 0;
            txtNivel.Value = string.Empty;
            cbAlta.Checked = false;
            lblFechaModificacionDB.Text = string.Empty;
            lblFechaAltaBD.Text = string.Empty;
            lblUsuarioAltaBD.Text = string.Empty;
            lblUsuarioModificacionBD.Text = string.Empty;
            OcultarAvisos();
        }

        private NivelLiquidacionDTO CargarObjetoNivel(ref NivelLiquidacionDTO _nivelDTO)
        {
            _nivelDTO.CodigoNivel = int.Parse(hfCodigoNivel.Value);
            _nivelDTO.Pais = DropDownListPais.SelectedValue;
            _nivelDTO.Nivel = txtNivel.Value;
            _nivelDTO.USUARIO_MANTENIMIENTO.USUARIO_ALTA = usuario;
            _nivelDTO.USUARIO_MANTENIMIENTO.USUARIO_MODIFICO = usuario;
            _nivelDTO.Alta = cbAlta.Checked;

            return _nivelDTO;
        }

        private bool ValidaCampos()
        {
            if (DropDownListPais.SelectedIndex != 0)
            {
                if (txtNivel.Value != "" && txtNivel.Value != null)
                {
                    result = true;
                }
            }
            return result;
        }

        private void GuardarNivelLiquidacion()
        {
            NivelLiquidacionDTO _nivelDTO = new NivelLiquidacionDTO();
            GestorNivelLiquidacion gestorNivel = null;
            OcultarAvisos();
            try
            {
                if (ValidaCampos())
                {
                    gestorNivel = GestorNivelLiquidacion();
                    if (!gestorNivel.ValidaExisteNivel(DropDownListPais.SelectedValue, txtNivel.Value))
                    {
                        CargarObjetoNivel(ref _nivelDTO);
                        gestorNivel.AlmacenarNivelLiquidacion(_nivelDTO);
                        hfCodigoNivel.Value = _nivelDTO.CodigoNivel.ToString();
                        LimpiarControles();
                        DesplegarAviso("El nivel de liquidacion fue almacenado correctamente");
                        CargarGridNiveles();
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
                DesplegarError("Error al almacenar el nivel de liquidacion");
            }
            finally
            {
                if (gestorNivel != null) gestorNivel.Dispose();
            }
        }

        private void CargarNivelEdicion(GridViewCommandEventArgs e)
        {
            int indice = Int32.Parse(e.CommandArgument.ToString());
            GridViewRow fila = gvNiveles.Rows[indice];

            hfCodigoNivel.Value = HttpUtility.HtmlDecode(fila.Cells[0].Text);

            DropDownListPais.SelectedValue = HttpUtility.HtmlDecode(fila.Cells[1].Text);
            txtNivel.Value = HttpUtility.HtmlDecode(fila.Cells[2].Text);

            cbAlta.Checked = (((CheckBox)fila.FindControl("idAlta2")).Checked);
            lblUsuarioAltaBD.Text = HttpUtility.HtmlDecode(fila.Cells[4].Text);
            lblFechaAltaBD.Text = HttpUtility.HtmlDecode(fila.Cells[5].Text);
            lblUsuarioModificacionBD.Text = HttpUtility.HtmlDecode(fila.Cells[6].Text);
            lblFechaModificacionDB.Text = HttpUtility.HtmlDecode(fila.Cells[7].Text);
        }

        private void DarBajaNivel(GridViewCommandEventArgs e)
        {
            int indice = Int32.Parse(e.CommandArgument.ToString());
            GridViewRow fila = gvNiveles.Rows[indice];
            GestorNivelLiquidacion gestorNivel = null;

            try
            {
                gestorNivel = GestorNivelLiquidacion();
                gestorNivel.DarBajaNivel(Convert.ToInt32(fila.Cells[0].Text), usuario);

                DesplegarAviso("El nivel de liquidacion fue dado de baja");
                CargarGridNiveles();
            }
            catch
            {
                DesplegarError("Error al dar de baja el registro");
            }
            finally
            {
                if (gestorNivel != null) gestorNivel.Dispose();
            }
        }

        private void DarAltaNivel(GridViewCommandEventArgs e)
        {
            int indice = Int32.Parse(e.CommandArgument.ToString());
            GridViewRow fila = gvNiveles.Rows[indice];
            GestorNivelLiquidacion gestorNivel = null;

            try
            {
                gestorNivel = GestorNivelLiquidacion();
                gestorNivel.DarAltaNivel(Convert.ToInt32(fila.Cells[0].Text), usuario);

                DesplegarAviso("El nivel de liquidacion se ha dado de alta con exito");
                CargarGridNiveles();
            }
            catch
            {
                DesplegarError("Error al dar de alta el nivel de liquidacion");
            }
            finally
            {
                if (gestorNivel != null) gestorNivel.Dispose();
            }
        }

        private void CargarDDLPais()
        {
            GestorNivelLiquidacion gestorNivel = null;
            try
            {
                gestorNivel = GestorNivelLiquidacion();
                List<LlenarDDL_DTO> _listacentroDto = gestorNivel.ListaPaisesDDL();

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
                if (gestorNivel != null) gestorNivel.Dispose();
            }
        }
        #endregion

        #region Gestores

        protected GestorNivelLiquidacion GestorNivelLiquidacion()
        {
            GestorNivelLiquidacion gestorNivelLiquidacion = new GestorNivelLiquidacion(cnnApl);
            return gestorNivelLiquidacion;
        }
        #endregion
    }
}