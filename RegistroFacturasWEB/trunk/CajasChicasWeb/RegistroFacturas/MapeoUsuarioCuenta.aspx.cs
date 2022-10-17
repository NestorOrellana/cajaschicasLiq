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
    public partial class MapeoUsuarioCuenta : System.Web.UI.Page
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
            CargarMapeoUsuarioCuentaGrid();
        }

        protected void gvMapeo_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            this.gvMapeo.PageIndex = e.NewPageIndex;
            CargarMapeoUsuarioCuentaGrid();
        }

        protected void gvMapeo_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName.Equals("Alta"))
            {
                OcultarAvisos();
                AltaCuentaUsuario(e);
              
            }
            if (e.CommandName.Equals("Baja"))
            {
                OcultarAvisos();
                BajaCuentaUsuario(e);

            }
        }

        #endregion

        #region Metodos

        private void AltaCuentaUsuario(GridViewCommandEventArgs e)
        {
            int indice = Int32.Parse(e.CommandArgument.ToString());
            GridViewRow fila = gvMapeo.Rows[indice];
            GestorSeguridad gestorseguridad = null;

            try
            {
                gestorseguridad = GestorSeguridad();
                gestorseguridad.AsignacionCuentaUsuario(fila.Cells[1].Text, usuario, fila.Cells[2].Text);

                DesplegarAviso("La cuenta fue asignada al usuario");
                CargarMapeoUsuarioCuentaGrid();
            }
            catch
            {
                DesplegarError("Error en la asignación de cuenta al usuario");
            }
            finally
            {
                if (gestorseguridad != null) gestorseguridad.Dispose();
            }
        }


        private void BajaCuentaUsuario(GridViewCommandEventArgs e)
        {
            int indice = Int32.Parse(e.CommandArgument.ToString());
            GridViewRow fila = gvMapeo.Rows[indice];
            GestorSeguridad gestorseguridad = null;

            try
            {
                gestorseguridad = GestorSeguridad();
                gestorseguridad.QuitarCuentaUsuario(fila.Cells[1].Text, usuario, fila.Cells[2].Text);

                DesplegarAviso("La cuenta se dio de baja para el usuario");
                CargarMapeoUsuarioCuentaGrid();
            }
            catch
            {
                DesplegarError("Error en la baja de la cuenta al usuario");
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


        public void CargarMapeoUsuarioCuentaGrid()
        {
            UsuarioCuentaDTO _usuarioCuentaDto = new UsuarioCuentaDTO();
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
                gvMapeo.DataSource = (from mapeo in gestorSeguridad.ListaUsuarioCuenta(ddlSociedad.Text, usuario, busqueda, dominio)
                                       select new
                                       {
                                           Numero = x++,
                                           BUKRS = ddlSociedad.Text,
                                           SAKNR = mapeo.SAKNR,
                                           TXT50 = mapeo.TXT50, 
                                           Estado = mapeo.ESTADO
                                       }).ToArray();
                gvMapeo.DataBind();
            }
            catch
            {
                DesplegarError("Error al Desplegar el Mapeo de Cuentas por Usuario");
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
            CargarMapeoUsuarioCuentaGrid();
        }

        private UsuarioCuentaDTO CargarObjetoUsuarioCuenta(ref UsuarioCuentaDTO _usuarioCuentaDto)
        {
            _usuarioCuentaDto.BUKRS = ddlSociedad.Text;
            _usuarioCuentaDto.SAKNR = usuario;
            _usuarioCuentaDto.ESTADO = true;

            return _usuarioCuentaDto;
        }

        private void AlmacenaCentro()
        {
            UsuarioCuentaDTO _usuarioCuentaDto = new UsuarioCuentaDTO();
            GestorSeguridad GestorSeguridad = null;
            OcultarAvisos();
            try
            {
                //if (txtNombre.Value != "" && txtNombre.Value.Length <= 50)
                //{
                //    gestorCentro = Gestorcentro();
                //    CargarObjetoCentro(ref _centroDto);
                //    gestorCentro.AlmacenarCentro(_centroDto);
                //    hfIdCentro.Value = _centroDto.ID_CENTRO.ToString();
                //    DesplegarAviso("El Centro fue almacenado correctamente");
                //    CargarCentroGrid();
                //}
                //else
                //    DesplegarError("Debe Completar los datos correctamente");
            }
            catch (ExcepcionesDIPCMI ex)
            {
                DesplegarError(ex.Message);
            }
            catch
            {
                DesplegarError("Error al almacenar Cuenta de Gasto para el Usuario");
            }
            finally
            {
                if (GestorSeguridad != null) GestorSeguridad.Dispose();
            }
        }


        //private void DarAltaCentro(GridViewCommandEventArgs e)
        //{
        //    int indice = Int32.Parse(e.CommandArgument.ToString());
        //    GridViewRow fila = gvMapeo.Rows[indice];
        //    GestorCentro gestorCentro = null;

        //    try
        //    {
        //        gestorCentro = Gestorcentro();
        //        gestorCentro.DarAltaCentro(Convert.ToInt16(fila.Cells[1].Text), usuario);

        //        DesplegarAviso("El centro fue dado de alta");
        //        CargarCentroGrid();
        //    }
        //    catch
        //    {
        //        DesplegarError("Error al dar alta al centro");
        //    }
        //    finally
        //    {
        //        if (gestorCentro != null) gestorCentro.Dispose();
        //    }
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