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
    public partial class MapeoUsuarioCaja_MM : System.Web.UI.Page
    {

        #region Declaraciones
        string cnnApl = ConfigurationManager.ConnectionStrings["CnnApl"].ToString();
        string usuario;
        string sociedad;
        #endregion

        #region Eventos
        protected void Page_Load(object sender, EventArgs e)
        { 
            usuario = Context.User.Identity.Name.ToString();
            hfSociedad.Value = ddlSociedad.SelectedValue;
            if (!IsPostBack)
            {
                OcultarAvisos();
                ddlSociedad.Items.Clear();
                CargarSociedadDDL(txtUsuario.Text, txtDominio.Text);
            }
            else
            {
                OcultarAvisos();
                if (txtUsuario.Text != "")
                {
                    sociedad = ddlSociedad.SelectedValue;
                    if (hfSociedad.Value == "0" || hfSociedad.Value == "")
                        CargarSociedadDDL(txtUsuario.Text, txtDominio.Text);
                }
                else
                {
                    gvMapeo.DataSource = null;
                    gvMapeo.DataBind();
                }
                
            }

        }

        protected void btnLimpiar_Click(object sender, EventArgs e)
        {
            LimpiarControles();
        }

        protected void btnVisualizar_Click(object sender, EventArgs e)
        {
            
            OcultarAvisos();
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
                gestorseguridad.AsignacionCajaUsuario(fila.Cells[1].Text, txtUsuario.Text, fila.Cells[2].Text);

                DesplegarAviso("La Caja fue asignada al usuario");
                CargarMapeoUsuarioCajaGrid();
            }
            catch
            {
                DesplegarError("Error en la asignación de Caja al usuario");
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
                gestorseguridad.QuitarCajaUsuario(fila.Cells[1].Text, txtUsuario.Text, fila.Cells[2].Text);

                DesplegarAviso("La Caja se dio de baja para el usuario");
                CargarMapeoUsuarioCajaGrid();
            }
            catch
            {
                DesplegarError("Error en la baja de la Caja al usuario");
            }
            finally
            {
                if (gestorseguridad != null) gestorseguridad.Dispose();
            }
        }

        private void CargarSociedadDDL(string usuarioB, string dominio)
        {
            List<LlenarDDL_DTO> listaDDLDto = new List<LlenarDDL_DTO>();
            GestorSociedad gestorSociedad = null;

            try
            {
                gestorSociedad = GestorSociedad();
                listaDDLDto = gestorSociedad.ListarSociedadesUsuario(usuarioB, dominio);

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
            UsuarioCuentaDTO _usuarioCuentaDto = new UsuarioCuentaDTO();
            GestorSeguridad gestorSeguridad = null;
            string usuarioB = txtUsuario.Text;
            string SocSelect = sociedad;
            string dominio = txtDominio.Text;
            string busqueda;
            if (txtBuscar.Text == "")
                busqueda = "";
            else
                busqueda = txtBuscar.Text;
            int x = 1;

            try
            {
                gestorSeguridad = GestorSeguridad();
                gvMapeo.DataSource = (from mapeo in gestorSeguridad.ListaUsuarioCaja(sociedad, usuarioB, busqueda, dominio)
                                      select new
                                      {
                                          Numero = x++,
                                          BUKRS = mapeo.BUKRS, //ddlSociedad.Text,
                                          LIFNR = mapeo.LIFNR,
                                          NAME = mapeo.NAME,
                                          Estado = mapeo.ESTADO
                                      }).ToArray();
                gvMapeo.DataBind();
            }
            catch
            {
                DesplegarError("Error al Desplegar el Mapeo de Cajas por Usuario");
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
            txtUsuario.Text = string.Empty;
            txtBuscar.Text = string.Empty;
            txtDominio.Text = string.Empty;
            ddlSociedad.Items.Clear();
            OcultarAvisos();
            gvMapeo.DataSource = null;
            gvMapeo.DataBind();
           // CargarMapeoUsuarioCuentaGrid();
        }

        //private UsuarioCajaDTO CargarObjetoUsuarioCaja(ref UsuarioCajaDTO _usuarioCajaDto)
        //{
        //    _usuarioCajaDto.BUKRS = ddlSociedad.Text; 
        //    _usuarioCajaDto.LIFNR 
        //}

        //private UsuarioCuentaDTO CargarObjetoUsuarioCuenta(ref UsuarioCuentaDTO _usuarioCuentaDto)
        //{
        //    _usuarioCuentaDto.BUKRS = ddlSociedad.Text;
        //    _usuarioCuentaDto.SAKNR = usuario;
        //    _usuarioCuentaDto.ESTADO = true;

        //    return _usuarioCuentaDto;
        //}
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