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



namespace RegistroFacturasWEB.RegistroFacturas
{
    public partial class MapeoUsuarioCaja : System.Web.UI.Page
    {

        #region Declaraciones
        string cnnApl = ConfigurationManager.ConnectionStrings["CnnApl"].ToString();
        string usuario;
        string dominio;
        #endregion

        #region Eventos
        protected void Page_Load(object sender, EventArgs e)
        {
            usuario = Context.User.Identity.Name.ToString();
            dominio = (string)Session["dominio"].ToString();
            if (!IsPostBack)
                CargarSociedadDDL();
            //    CargarMapeoUsuarioCuentaGrid();
        }

        protected void btnLimpiar_Click(object sender, EventArgs e)
        {
            LimpiarControles();
        }

        protected void btnVisualizar_Click(object sender, EventArgs e)
        {
            CargarMapeoUsuarioCajaGrid();
        }

        protected void gvMapeo_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            this.gvMapeo.PageIndex = e.NewPageIndex;
            CargarMapeoUsuarioCajaGrid();
        }

        protected void gvMapeo_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName.Equals("Alta"))
            {
                OcultarAvisos();
                AltaCajaUsuario(e);
              
            }
            if (e.CommandName.Equals("Baja"))
            {
                OcultarAvisos();
                BajaCajaUsuario(e);

            }
        }

        #endregion

        #region Metodos

        private void AltaCajaUsuario(GridViewCommandEventArgs e)
        {
            int indice = Int32.Parse(e.CommandArgument.ToString());
            GridViewRow fila = gvMapeo.Rows[indice];
            GestorSeguridad gestorseguridad = null;

            try
            {
                gestorseguridad = GestorSeguridad();
                gestorseguridad.AsignacionCajaUsuario(fila.Cells[1].Text, usuario, fila.Cells[2].Text);

                DesplegarAviso("La caja chica fue asignada al usuario");
                CargarMapeoUsuarioCajaGrid();
            }
            catch
            {
                DesplegarError("Error en la asignación de caja chica al usuario");
            }
            finally
            {
                if (gestorseguridad != null) gestorseguridad.Dispose();
            }
        }


        private void BajaCajaUsuario(GridViewCommandEventArgs e)
        {
            int indice = Int32.Parse(e.CommandArgument.ToString());
            GridViewRow fila = gvMapeo.Rows[indice];
            GestorSeguridad gestorseguridad = null;

            try
            {
                gestorseguridad = GestorSeguridad();
                gestorseguridad.QuitarCajaUsuario(fila.Cells[1].Text, usuario, fila.Cells[2].Text);

                DesplegarAviso("La caja chica se dio de baja para el usuario");
                CargarMapeoUsuarioCajaGrid();
            }
            catch
            {
                DesplegarError("Error en la baja de la caja chica al usuario");
            }
            finally
            {
                if (gestorseguridad != null) gestorseguridad.Dispose();
            }
        }

        private void CargarSociedadDDL()
        {
            List<LlenarDDL_DTO> listaDDLDto = new List<LlenarDDL_DTO>();
            GestorSociedad gestorSociedad = null;

            try
            {
                gestorSociedad = GestorSociedad();
                listaDDLDto = gestorSociedad.ListarSociedadesUsuario(usuario, dominio);

                ddlSociedad.DataSource = listaDDLDto;
                ddlSociedad.DataTextField = "DESCRIPCION";
                ddlSociedad.DataValueField = "IDENTIFICADOR";
                ddlSociedad.DataBind();
                ddlSociedad.Items.Insert(0, new ListItem("::Seleccione sociedad::", "0"));

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


        public void CargarMapeoUsuarioCajaGrid()
        {
            UsuarioCajaDTO _usuarioCajaDto = new UsuarioCajaDTO();
            GestorSeguridad gestorSeguridad = null;
            string busqueda;
            if (txtBuscar.Text == "")
                busqueda = "";
            else
                busqueda = txtBuscar.Text;
            int x = 1;
            try
            {
                gestorSeguridad = GestorSeguridad();
                gvMapeo.DataSource = (from mapeo in gestorSeguridad.ListaUsuarioCaja(ddlSociedad.Text, usuario, busqueda, dominio)
                                       select new
                                       {
                                           Numero = x++,
                                           BUKRS = ddlSociedad.Text,
                                           LIFNR = mapeo.LIFNR,
                                           NAME  = mapeo.NAME, 
                                           Estado = mapeo.ESTADO
                                       }).ToArray();
                gvMapeo.DataBind();
            }
            catch
            {
                DesplegarError("Error al Desplegar el Mapeo de Caja Chica por Usuario");
            }
            finally
            {
                if (gestorSeguridad != null) gestorSeguridad.Dispose();
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
            txtBuscar.Text = string.Empty;
            OcultarAvisos();
            CargarMapeoUsuarioCajaGrid();
        }

        private UsuarioCajaDTO CargarObjetoUsuarioCaja(ref UsuarioCajaDTO _usuarioCajaDto)
        {
            _usuarioCajaDto.BUKRS = ddlSociedad.Text;
            _usuarioCajaDto.LIFNR = usuario;
            _usuarioCajaDto.ESTADO = true;

            return _usuarioCajaDto;
        }

        #endregion

        #region Gestores

        protected GestorSeguridad GestorSeguridad()
        {
            GestorSeguridad gestorSeguridad = new GestorSeguridad(cnnApl);
            return gestorSeguridad;
        }

        protected GestorSociedad GestorSociedad()
        {
            GestorSociedad gestorSociedad = new GestorSociedad(cnnApl);
            return gestorSociedad;
        }
#endregion 
    }
}