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
    public partial class Centro : System.Web.UI.Page
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
                CargarCentroGrid();
        }

        protected void btnLimpiar_Click(object sender, EventArgs e)
        {
            LimpiarControles();
        }

        protected void btnGrabar_Click(object sender, EventArgs e)
        {
            AlmacenaCentro();
        }

        protected void gvCentro_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            this.gvCentro.PageIndex = e.NewPageIndex;
            CargarCentroGrid();
        }

        protected void gvCentro_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName.Equals("Editar"))
            {
                OcultarAvisos();
                CargarCentroEdicion(e);
            }
            if (e.CommandName.Equals("Baja"))
            {
                OcultarAvisos();
                DarBajaCentro(e);
            }
            if (e.CommandName.Equals("Alta"))
            {
                OcultarAvisos();
                DarAltaCentro(e);
            }
        }

        #endregion

        #region Metodos

        public void CargarCentroGrid()
        {
            CentroDTO _centroDto = new CentroDTO();
            GestorCentro gestorCentro = null;
          
            int x = 1;
            try
            {
                gestorCentro = Gestorcentro();
                gvCentro.DataSource = (from centro in gestorCentro.ListaCentro()
                                       select new
                                       {
                                           Numero = x++,
                                           CODIGO_CENTRO = centro.ID_CENTRO,
                                           NOMBRE = centro.NOMBRE,
                                           idAlta = centro.ALTA,
                                           ID_USUARIOALTA = centro.USUARIO_MANTENIMIENTO.USUARIO_ALTA,
                                           FECHA_ALTA = centro.USUARIO_MANTENIMIENTO.FECHA_ALTA,
                                           ID_USUARIOMODIFICACION = centro.USUARIO_MANTENIMIENTO.USUARIO_MODIFICO,
                                           FECHA_MODIFICACION = centro.USUARIO_MANTENIMIENTO.FECHA_MODIFICACION
                                       }).ToArray();
                gvCentro.DataBind();
            }
            catch
            {
                DesplegarError("Error al Desplegar los Centros");
            }
            finally
            {
                if (gestorCentro != null) gestorCentro.Dispose();
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
            hfIdCentro.Value = "0";
            txtNombre.Value = string.Empty;
            cbAlta.Checked = false;
            lblUsuarioAltaBD.Text = string.Empty;
            lblFechaAltaBD.Text = string.Empty;
            lblUsuarioModificacionBD.Text = string.Empty;
            lblFechaModificacionDB.Text = string.Empty;
            OcultarAvisos();
        }

        private CentroDTO CargarObjetoCentro(ref CentroDTO _centroDto)
        {
            _centroDto.ID_CENTRO = Convert.ToInt16(hfIdCentro.Value);
            _centroDto.NOMBRE = txtNombre.Value;
            _centroDto.ALTA = cbAlta.Checked;//_proveedorDto.ES_PEQUEÑO_CONTRIBUYENTE = cbPequeñoContribuyente.Checked;
            _centroDto.USUARIO_MANTENIMIENTO.USUARIO_ALTA = usuario;
            //_centroDto.USUARIO_MANTENIMIENTO.FECHA_ALTA = Convert.ToDateTime(lblFechaAltaBD.Text);
            _centroDto.USUARIO_MANTENIMIENTO.USUARIO_MODIFICO = usuario;
            //_centroDto.USUARIO_MANTENIMIENTO.FECHA_MODIFICACION = Convert.ToDateTime(lblFechaModificacionDB.Text);

            return _centroDto;
        }

        private void AlmacenaCentro()
        {
            CentroDTO _centroDto = new CentroDTO();
            GestorCentro gestorCentro = null;
            OcultarAvisos();
            try
            {
                if (txtNombre.Value != "" && txtNombre.Value.Length <= 50)
                {
                    gestorCentro = Gestorcentro();
                    CargarObjetoCentro(ref _centroDto);
                    gestorCentro.AlmacenarCentro(_centroDto);
                    hfIdCentro.Value = _centroDto.ID_CENTRO.ToString();
                    DesplegarAviso("El Centro fue almacenado correctamente");
                    CargarCentroGrid();
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
                DesplegarError("Error al almacenar Centro");
            }
            finally
            {
                if (gestorCentro != null) gestorCentro.Dispose();
            }
        }

        private void CargarCentroEdicion(GridViewCommandEventArgs e)
        {
            int indice = Int32.Parse(e.CommandArgument.ToString());
            GridViewRow fila = gvCentro.Rows[indice];

            hfIdCentro.Value = HttpUtility.HtmlDecode(fila.Cells[1].Text);

            txtNombre.Value = HttpUtility.HtmlDecode(fila.Cells[2].Text);
            cbAlta.Checked = (((CheckBox)fila.FindControl("idAlta")).Checked);
            lblUsuarioAltaBD.Text = HttpUtility.HtmlDecode(fila.Cells[4].Text);
            lblFechaAltaBD.Text = HttpUtility.HtmlDecode(fila.Cells[5].Text);
            lblUsuarioModificacionBD.Text = HttpUtility.HtmlDecode(fila.Cells[6].Text);
            lblFechaModificacionDB.Text = HttpUtility.HtmlDecode(fila.Cells[7].Text);
        }

        private void DarBajaCentro(GridViewCommandEventArgs e)
        {
            int indice = Int32.Parse(e.CommandArgument.ToString());
            GridViewRow fila = gvCentro.Rows[indice];
            GestorCentro gestorCentro = null;

            try
            {
                gestorCentro = Gestorcentro();
                gestorCentro.DarBajaCentro(Convert.ToInt16(fila.Cells[1].Text), usuario);

                DesplegarAviso("El centro fue dado de baja");
                CargarCentroGrid();
            }
            catch
            {
                DesplegarError("Error al dar de baja al centro");
            }
            finally
            {
                if (gestorCentro != null) gestorCentro.Dispose();
            }
        }

        private void DarAltaCentro(GridViewCommandEventArgs e)
        {
            int indice = Int32.Parse(e.CommandArgument.ToString());
            GridViewRow fila = gvCentro.Rows[indice];
            GestorCentro gestorCentro = null;

            try
            {
                gestorCentro = Gestorcentro();
                gestorCentro.DarAltaCentro(Convert.ToInt16(fila.Cells[1].Text), usuario);

                DesplegarAviso("El centro fue dado de alta");
                CargarCentroGrid();
            }
            catch
            {
                DesplegarError("Error al dar alta al centro");
            }
            finally
            {
                if (gestorCentro != null) gestorCentro.Dispose();
            }
        }
        #endregion

        #region Gestores

        protected GestorCentro Gestorcentro()
        {
            GestorCentro gestorSeguridad = new GestorCentro(cnnApl);
            return gestorSeguridad;
        }
        #endregion
    }
}