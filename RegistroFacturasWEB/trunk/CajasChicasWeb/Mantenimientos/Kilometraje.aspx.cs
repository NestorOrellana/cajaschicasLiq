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
    public partial class Kilometraje : System.Web.UI.Page
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
                CargarGridKilometrajes();
            }
        }

        protected void btnLimpiar_Click(object sender, EventArgs e)
        {
            LimpiarControles();
        }

        protected void btnGrabar_Click(object sender, EventArgs e)
        {
            GuardarKilometraje();
        }

        protected void gvKilometrajes_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            this.gvKilometrajes.PageIndex = e.NewPageIndex;
            CargarGridKilometrajes();
        }

        protected void gvKilometrajes_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName.Equals("Editar"))
            {
                CargarKilometrajeEdicion(e);
                OcultarAvisos();
            }
            if (e.CommandName.Equals("Baja"))
            {
                OcultarAvisos();
                DarBajaKilometraje(e);
            }
            if (e.CommandName.Equals("Alta"))
            {
                OcultarAvisos();
                DarAltaKilometraje(e);
            }
        }
        #endregion

        #region Metodos

        public void CargarGridKilometrajes()
        {
            KilometrajeDTO _kilometrajeDTO = new KilometrajeDTO();
            GestorKilometraje gestorKilometraje = null;

            int x = 1;
            try
            {
                gestorKilometraje = GestorKilometraje();
                gvKilometrajes.DataSource = (from kilometraje in gestorKilometraje.ListaKilometrajes()
                                       select new
                                       {
                                           CODIGO_KILOMETRAJE = kilometraje.CodigoKilometraje,
                                           PAIS = kilometraje.Pais,
                                           ORIGEN = kilometraje.Origen,
                                           DESTINO = kilometraje.Destino,
                                           KILOMETROS = kilometraje.Kilometros,
                                           idAlta2 = kilometraje.Alta,
                                           ID_USUARIOALTA = kilometraje.USUARIO_MANTENIMIENTO.USUARIO_ALTA,
                                           FECHA_ALTA = kilometraje.USUARIO_MANTENIMIENTO.FECHA_ALTA,
                                           ID_USUARIOMODIFICACION = kilometraje.USUARIO_MANTENIMIENTO.USUARIO_MODIFICO,
                                           FECHA_MODIFICACION = kilometraje.USUARIO_MANTENIMIENTO.FECHA_MODIFICACION,
                                       }).ToArray();

                gvKilometrajes.DataBind();


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                DesplegarError("Error al desplegar los registros");
            }
            finally
            {
                if (gestorKilometraje != null) gestorKilometraje.Dispose();
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

            hfCodigoKilometraje.Value = "0";
            DropDownListPais.SelectedIndex = 0;
            DropDownListPais.SelectedIndex = 0;
            txtOrigen.Value = string.Empty;
            txtDestino.Value = string.Empty;
            txtKilometros.Value = string.Empty;
            cbAlta.Checked = false;
            lblFechaModificacionDB.Text = string.Empty;
            lblFechaAltaBD.Text = string.Empty;
            lblUsuarioAltaBD.Text = string.Empty;
            lblUsuarioModificacionBD.Text = string.Empty;
            OcultarAvisos();
        }

        private KilometrajeDTO CargarObjetoKilometraje(ref KilometrajeDTO _kilometrajeDTO)
        {
            _kilometrajeDTO.CodigoKilometraje = long.Parse(hfCodigoKilometraje.Value);
            _kilometrajeDTO.Pais = DropDownListPais.SelectedValue;
            _kilometrajeDTO.Origen = txtOrigen.Value;
            _kilometrajeDTO.Destino = txtDestino.Value;
            _kilometrajeDTO.Kilometros = decimal.Parse(txtKilometros.Value);
            _kilometrajeDTO.USUARIO_MANTENIMIENTO.USUARIO_ALTA = usuario;
            _kilometrajeDTO.USUARIO_MANTENIMIENTO.USUARIO_MODIFICO = usuario;
            _kilometrajeDTO.Alta = cbAlta.Checked;

            return _kilometrajeDTO;
        }

        private bool ValidaCampos()
        {
            if (DropDownListPais.SelectedIndex != 0)
            {
                if (txtOrigen.Value != "" && txtOrigen.Value != null)
                {
                    if(txtDestino.Value != "" && txtDestino.Value != null)
                    {
                        if(txtKilometros.Value != "" && txtDestino.Value != "0")
                        {
                            result = true;
                        }
                    }
                }
            }
            return result;
        }

        private void GuardarKilometraje()
        {
            KilometrajeDTO _kilometrajeDTO = new KilometrajeDTO();
            GestorKilometraje gestorKilometraje = null;
            OcultarAvisos();
            try
            {
                if (ValidaCampos())
                {
                    gestorKilometraje = GestorKilometraje();
                    if (!gestorKilometraje.ValidaExisteKilometraje(DropDownListPais.SelectedValue, txtOrigen.Value, txtDestino.Value))
                    {
                        CargarObjetoKilometraje(ref _kilometrajeDTO);
                        gestorKilometraje.AlmacenarKilometraje(_kilometrajeDTO);
                        hfCodigoKilometraje.Value = _kilometrajeDTO.CodigoKilometraje.ToString();
                        LimpiarControles();
                        DesplegarAviso("El kilometraje fue almacenado Correctamente");
                        CargarGridKilometrajes();
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
                DesplegarError("Error al almacenar el kilometraje");
            }
            finally
            {
                if (gestorKilometraje != null) gestorKilometraje.Dispose();
            }
        }

        private void CargarKilometrajeEdicion(GridViewCommandEventArgs e)
        {
            int indice = Int32.Parse(e.CommandArgument.ToString());
            GridViewRow fila = gvKilometrajes.Rows[indice];

            hfCodigoKilometraje.Value = HttpUtility.HtmlDecode(fila.Cells[0].Text);

            DropDownListPais.SelectedValue = HttpUtility.HtmlDecode(fila.Cells[1].Text);
            txtOrigen.Value = HttpUtility.HtmlDecode(fila.Cells[2].Text);
            txtDestino.Value = HttpUtility.HtmlDecode(fila.Cells[3].Text);
            txtKilometros.Value = HttpUtility.HtmlDecode(fila.Cells[4].Text);

            cbAlta.Checked = (((CheckBox)fila.FindControl("idAlta2")).Checked);
            lblUsuarioAltaBD.Text = HttpUtility.HtmlDecode(fila.Cells[6].Text);
            lblFechaAltaBD.Text = HttpUtility.HtmlDecode(fila.Cells[7].Text);
            lblUsuarioModificacionBD.Text = HttpUtility.HtmlDecode(fila.Cells[8].Text);
            lblFechaModificacionDB.Text = HttpUtility.HtmlDecode(fila.Cells[9].Text);
        }

        private void DarBajaKilometraje(GridViewCommandEventArgs e)
        {
            int indice = Int32.Parse(e.CommandArgument.ToString());
            GridViewRow fila = gvKilometrajes.Rows[indice];
            GestorKilometraje gestorKilometraje = null;

            try
            {
                gestorKilometraje = GestorKilometraje();
                gestorKilometraje.DarBajaKilometraje(Convert.ToInt16(fila.Cells[0].Text), usuario);

                DesplegarAviso("El kilometraje fue dado de baja");
                CargarGridKilometrajes();
            }
            catch
            {
                DesplegarError("Error al dar de baja el registro");
            }
            finally
            {
                if (gestorKilometraje != null) gestorKilometraje.Dispose();
            }
        }

        private void DarAltaKilometraje(GridViewCommandEventArgs e)
        {
            int indice = Int32.Parse(e.CommandArgument.ToString());
            GridViewRow fila = gvKilometrajes.Rows[indice];
            GestorKilometraje gestorKilometraje = null;

            try
            {
                gestorKilometraje = GestorKilometraje();
                gestorKilometraje.DarAltaKilometraje(Convert.ToInt16(fila.Cells[0].Text), usuario);

                DesplegarAviso("El kilometraje se ha dado de alta con exito");
                CargarGridKilometrajes();
            }
            catch
            {
                DesplegarError("Error al dar de alta el kilometraje");
            }
            finally
            {
                if (gestorKilometraje != null) gestorKilometraje.Dispose();
            }
        }

        private void CargarDDLPais()
        {
            GestorKilometraje gestorKilometraje = null;
            try
            {
                gestorKilometraje = GestorKilometraje();
                List<LlenarDDL_DTO> _listacentroDto = gestorKilometraje.ListaPaisesDDL();

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
                if (gestorKilometraje != null) gestorKilometraje.Dispose();
            }
        }
        #endregion

        #region Gestores

        protected GestorKilometraje GestorKilometraje()
        {
            GestorKilometraje gestorKilometraje = new GestorKilometraje(cnnApl);
            return gestorKilometraje;
        }
        #endregion
    }
}