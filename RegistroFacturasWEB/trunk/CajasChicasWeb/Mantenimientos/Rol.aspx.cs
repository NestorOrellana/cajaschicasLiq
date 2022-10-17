using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using LogicaComun.Enum;
using DipCmiGT.LogicaCajasChicas;
using DipCmiGT.LogicaComun;
using DipCmiGT.LogicaCajasChicas.Sesion;
using System.Configuration;

namespace RegistroFacturasWEB.Mantenimientos
{
    public partial class Rol : System.Web.UI.Page
    {

        #region Declaraciones

        string cnnApl = ConfigurationManager.ConnectionStrings["CnnApl"].ToString();
        string usuario;

        #endregion

        #region Eventos

        protected void Page_Load(object sender, EventArgs e)
        {
            usuario = Context.User.Identity.Name.ToString();
            if (!IsPostBack)
            {
                CargarRolGrid();
            }
        }

        protected void btnLimpiar_Click(object sender, EventArgs e)
        {
            LimpiarControles();
        }

        protected void btnGrabar_Click(object sender, EventArgs e)
        {
            btnGrabar.Enabled = false;
            AlmacenarRol();
            btnGrabar.Enabled = true;
        }

        protected void gvRol_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName.Equals("Editar"))
            {
                OcultarAvisos();
                CargarRolEdicion(e);
            }

            if (e.CommandName.Equals("Baja"))
            {
                OcultarAvisos();
                DarBajaRol(e);
            }

            if (e.CommandName.Equals("Alta"))
            {
                OcultarAvisos();
                DarAltaRol(e);
            }
        }

        protected void gvRol_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            this.gvRol.PageIndex = e.NewPageIndex;
            CargarRolGrid();
        }

        #endregion

        #region Metodos

        private void DesplegarAviso(string mensaje)
        {
            divMensaje.Attributes.CssStyle.Add("display", "table");
            divMensaje.InnerHtml = mensaje;
        }

        private void DesplegarError(string mensaje)
        {
            divMensajeError.Attributes.CssStyle.Add("display", "table");
            divMensajeError.InnerHtml = mensaje;
        }

        private void OcultarAvisos()
        {
            divMensaje.Attributes.CssStyle.Add("display", "none");
            divMensajeError.Attributes.CssStyle.Add("display", "none");
        }

        private void LimpiarControles()
        {
            hfIdRol.Value = "0";
            txtNombreRol.Text = string.Empty;
            cbAlta.Checked = false;
            lblFechaModificacionDB.Text = string.Empty;
            lblUsuarioDB.Text = string.Empty;
            OcultarAvisos();
        }

        private RolDTO CargarObjetoRol(ref RolDTO _rolDto)
        {
            _rolDto.ID_ROL = Convert.ToInt16(hfIdRol.Value);
            _rolDto.NOMBRE_ROL = txtNombreRol.Text;
            _rolDto.ALTA = _rolDto.ID_ROL.Equals(0) ? (bool?)null : cbAlta.Checked;
            _rolDto.USUARIO_MANTENIMIENTO.USUARIO_ALTA = usuario;
            _rolDto.USUARIO_MANTENIMIENTO.USUARIO_MODIFICO = usuario;

            return _rolDto;
        }

        private void AlmacenarRol()
        {
            RolDTO _rolDto = new RolDTO();
            GestorSeguridad gestorSeguridad = null;

            OcultarAvisos();
            try
            {
                if ((txtNombreRol.Text != "") && (txtNombreRol.Text.Length <= 25))
                {
                    gestorSeguridad = GestorSeguridad();
                    CargarObjetoRol(ref _rolDto);
                    gestorSeguridad.AlmacenarRol(_rolDto);
                    hfIdRol.Value = _rolDto.ID_ROL.ToString();
                    DesplegarAviso("El rol fue almacenado correctamente.");
                    CargarRolGrid();
                }
                else
                    DesplegarError("Completar correctamente los Datos");
            }
            catch (ExcepcionesDIPCMI ex)
            {
                DesplegarError(ex.Message);
            }
            catch
            {
                DesplegarError("Error al almacenar el Rol.");
            }
            finally
            {
                if (gestorSeguridad != null) gestorSeguridad.Dispose();
            }
        }

        public void CargarRolGrid()
        {
            RolDTO _rolDto = new RolDTO();
            GestorSeguridad gestorSeguridad = null;
            int x = 1;
            try
            {
                gestorSeguridad = GestorSeguridad();

                gvRol.DataSource = (from rol in gestorSeguridad.ListaRol()
                                    select new
                                    {
                                        NUMERO = x++,
                                        ID_ROL = rol.ID_ROL,
                                        NOMBRE_ROL = rol.NOMBRE_ROL,
                                        idAlta = rol.ALTA,
                                        USUARIO_ALTA = rol.USUARIO_MANTENIMIENTO.USUARIO_ALTA,
                                        FECHA_ALTA = rol.USUARIO_MANTENIMIENTO.FECHA_ALTA,
                                        USUARIO_MODIFICACION = rol.USUARIO_MANTENIMIENTO.USUARIO_MODIFICO,
                                        FECHA_MODIFICACION = rol.USUARIO_MANTENIMIENTO.FECHA_MODIFICACION
                                    }).ToArray();
                gvRol.DataBind();
            }
            catch
            {
                DesplegarError("Error al desplegar los roles");
            }
            finally
            {
                if (gestorSeguridad != null) gestorSeguridad.Dispose();
            }
        }

        private void CargarRolEdicion(GridViewCommandEventArgs e)
        {
            int indice = Int32.Parse(e.CommandArgument.ToString());
            GridViewRow fila = gvRol.Rows[indice];

            hfIdRol.Value = HttpUtility.HtmlDecode(fila.Cells[1].Text);
            txtNombreRol.Text = HttpUtility.HtmlDecode(fila.Cells[2].Text);
            cbAlta.Checked = (((CheckBox)fila.FindControl("idAlta")).Checked);
            lblFechaModificacionDB.Text = HttpUtility.HtmlDecode(fila.Cells[7].Text);
            lblUsuarioDB.Text = HttpUtility.HtmlDecode(fila.Cells[6].Text);
        }

        private void DarBajaRol(GridViewCommandEventArgs e)
        {
            int indice = Int32.Parse(e.CommandArgument.ToString());
            GridViewRow fila = gvRol.Rows[indice];
            GestorSeguridad gestorSeguridad = null;

            try
            {
                gestorSeguridad = GestorSeguridad();
                gestorSeguridad.DarBajaRol(Convert.ToInt16(fila.Cells[1].Text), usuario);

                DesplegarAviso("El rol fue dado de baja.");
                CargarRolGrid();
            }
            catch
            {
                DesplegarError("Error al dar baja al rol");
            }
            finally
            {
                if (gestorSeguridad != null) gestorSeguridad.Dispose();
            }
        }

        private void DarAltaRol(GridViewCommandEventArgs e)
        {
            int indice = Int32.Parse(e.CommandArgument.ToString());
            GridViewRow fila = gvRol.Rows[indice];
            GestorSeguridad gestorSeguridad = null;

            try
            {
                gestorSeguridad = GestorSeguridad();
                gestorSeguridad.DarAltaRol(Convert.ToInt16(fila.Cells[1].Text), usuario);

                DesplegarAviso("El rol fue dado de alta.");
                CargarRolGrid();
            }
            catch
            {
                DesplegarError("Error al dar alta al rol.");
            }
            finally
            {
                if (gestorSeguridad != null) gestorSeguridad.Dispose();
            }
        }

        #endregion

        #region Gestores

        protected GestorSeguridad GestorSeguridad()
        {
            GestorSeguridad gestorSeguridad = new GestorSeguridad(cnnApl);
            return gestorSeguridad;
        }

        #endregion
    }
}