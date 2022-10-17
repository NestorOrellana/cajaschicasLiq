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
using LogicaCajasChicas;
using LogicaCajasChicas.Sesion;



namespace RegistroFacturasWEB.RegistroFacturas
{
    public partial class MapeoRegistradorAprobador : System.Web.UI.Page
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
        }

        protected void btnLimpiar_Click(object sender, EventArgs e)
        {
            LimpiarControles();
        }

        protected void gvMapeo_PageIndexChanging1(object sender, GridViewPageEventArgs e)
        {
            this.gvMapeo.PageIndex = e.NewPageIndex;
            CargarGrid();
        }

        protected void btbBuscar_Click(object sender, EventArgs e)
        {
            if (txtCeco.Text == "")
                DesplegarError("Debe Ingresar un Centro de Costo");
            else
            {
                CargarCentros(txtCeco.Text);
                txtCeco.Enabled = false;
                CargarGrid();
            }
        }

        public void ddlCentro_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlCentro.SelectedValue.ToString() == "0")
                DesplegarError("Debe seleccionar un centro");
            else
            {
                CargarAprobador(txtCeco.Text, ddlCentro.SelectedValue.ToString());
                CargarRegistrador(txtCeco.Text, ddlCentro.SelectedValue.ToString());
                CargarGrid();
            }
        }

        protected void ddlAprobador_SelectedIndexChanged(object sender, EventArgs e)
        {
            CargarGrid();
        }

        protected void ddlRegistrador_SelectedIndexChanged(object sender, EventArgs e)
        {
            CargarGrid();
        }

        protected void btbAsignar_Click(object sender, EventArgs e)
        {
            AlmacenarMapeo();
        }

        protected void btnLimpiar_Click1(object sender, EventArgs e)
        {
            LimpiarControles();
        }

        protected void gvMapeo_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName.Equals("Baja"))
            {
                OcultarAvisos();
                EliminarMapeo(e);

            }
        }

        #endregion

        #region Metodos

        private void EliminarMapeo(GridViewCommandEventArgs e)
        {
            int indice = Int32.Parse(e.CommandArgument.ToString());
            GridViewRow fila = gvMapeo.Rows[indice];
            GestorSeguridad gestorseguridad = null;

            try
            {
                gestorseguridad = GestorSeguridad();
                gestorseguridad.EliminarMpeoRegistradorAprobador(fila.Cells[1].Text, fila.Cells[2].Text, fila.Cells[4].Text, fila.Cells[6].Text, usuario);

                DesplegarAviso("El mapeo fue eliminado");
                CargarGrid();
            }
            catch
            {
                DesplegarError("Error en la baja del mapeo");
            }
            finally
            {
                if (gestorseguridad != null) gestorseguridad.Dispose();
            }
        }
        private void CargarCentros(string ceco)
        {
            List<LlenarDDL_DTO> listaDDLDto = new List<LlenarDDL_DTO>();
            GestorSeguridad gestorseguridad = null;
            try
            {
                gestorseguridad = GestorSeguridad();
                listaDDLDto = gestorseguridad.BuscarCentros(ceco);


                ddlCentro.DataSource = listaDDLDto;
                ddlCentro.DataTextField = "DESCRIPCION";
                ddlCentro.DataValueField = "IDENTIFICADOR";
                ddlCentro.DataBind();
                ddlCentro.Items.Insert(0, new ListItem("--   Seleccione centro   --", "-1"));
            }
            catch
            {
                DesplegarError("Error al recuperar los centros");
            }

            finally
            {
                if (gestorseguridad != null) gestorseguridad.Dispose();
            }
        }

        private void CargarAprobador(string ceco, string centro)
        {
            List<LlenarDDL_DTO> listaDDLDto = new List<LlenarDDL_DTO>();
            GestorSeguridad gestorseguridad = null;
            try
            {
                gestorseguridad = GestorSeguridad();
                listaDDLDto = gestorseguridad.BuscarAprobador(ceco, centro);


                ddlAprobador.DataSource = listaDDLDto;
                ddlAprobador.DataTextField = "DESCRIPCION";
                ddlAprobador.DataValueField = "IDENTIFICADOR";
                ddlAprobador.DataBind();
                ddlAprobador.Items.Insert(0, new ListItem("--   Seleccione Aprobador   --", "-1"));
            }
            catch
            {
                DesplegarError("Error al recuperar los Aprobadores");
            }

            finally
            {
                if (gestorseguridad != null) gestorseguridad.Dispose();
            }
        }

        private void CargarRegistrador(string ceco, string centro)
        {
            List<LlenarDDL_DTO> listaDDLDto = new List<LlenarDDL_DTO>();
            GestorSeguridad gestorseguridad = null;
            try
            {
                gestorseguridad = GestorSeguridad();
                listaDDLDto = gestorseguridad.BuscarRegistrador(ceco, centro);


                ddlRegistrador.DataSource = listaDDLDto;
                ddlRegistrador.DataTextField = "DESCRIPCION";
                ddlRegistrador.DataValueField = "IDENTIFICADOR";
                ddlRegistrador.DataBind();
                ddlRegistrador.Items.Insert(0, new ListItem("--   Seleccione Registrador   --", "-1"));
            }
            catch
            {
                DesplegarError("Error al recuperar los Registradores");
            }

            finally
            {
                if (gestorseguridad != null) gestorseguridad.Dispose();
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
            txtCeco.Text = string.Empty;
            txtCeco.Enabled = true;
            ddlCentro.Items.Clear();
            ddlAprobador.Items.Clear();
            ddlRegistrador.Items.Clear();
            OcultarAvisos();
            CargarGrid();
        }

        public void CargarGrid()
        {
            GestorSeguridad gestorSeguridad = null;
            int x = 1;
            string aprobador, registrador;

            if (ddlAprobador.SelectedValue.ToString() == "") aprobador = "-1"; else aprobador = ddlAprobador.SelectedValue.ToString();
            if (ddlRegistrador.SelectedValue.ToString() == "") registrador = "-1"; else registrador = ddlRegistrador.SelectedValue.ToString();

            try
            {
                gestorSeguridad = GestorSeguridad();
                gvMapeo.DataSource = (from mapeo in gestorSeguridad.ListaMapeosRegistradorAprobador(txtCeco.Text, ddlCentro.SelectedValue.ToString(), aprobador, registrador)
                                      select new
                                      {
                                          NUMERO = x++,
                                          CECO_ORDEN = mapeo.CECO_ORDEN,
                                          ID_CENTRO = mapeo.ID_CENTRO,
                                          CENTRO = mapeo.CENTRO,
                                          APROBADOR_US = mapeo.APROBADOR_US,
                                          APROBADOR = mapeo.APROBADOR,
                                          REGISTRADOR_US = mapeo.REGISTRADOR_US,
                                          REGISTRADOR = mapeo.REGISTRADOR,
                                          USUARIO_ALTA = mapeo.USUARIO_ALTA,
                                          ESTADO = 1

                                      }).ToArray();
                gvMapeo.DataBind();
            }
            catch
            {
                DesplegarError("Error al desplegar los mapeos");
            }
            finally
            {
                if (gestorSeguridad != null) gestorSeguridad.Dispose();
            }
        }

        private void AlmacenarMapeo()
        {
            MapeoRegistradorAprobadorDTO _mapeoDto = new MapeoRegistradorAprobadorDTO();
            GestorSeguridad gestorSegurida = null;

            OcultarAvisos();
            try
            {
                if (txtCeco.Text != "" || ddlCentro.SelectedValue.ToString() != "-1" ||
                    ddlAprobador.SelectedValue.ToString() != "-1" || ddlRegistrador.SelectedValue.ToString() != "-1" ||
                    ddlAprobador.SelectedValue.ToString() != "" || ddlRegistrador.SelectedValue.ToString() != "")
                {
                    gestorSegurida = GestorSeguridad();
                    gestorSegurida.InsertarMpeoRegistradorAprobador(txtCeco.Text, ddlCentro.SelectedValue.ToString(), ddlAprobador.SelectedValue.ToString(), ddlRegistrador.SelectedValue.ToString(), usuario);
                    DesplegarAviso("El Mapeo fue almacenado correctamente.");
                    CargarGrid();
                }
                else DesplegarError("Debe Completar todos los datos correctamente");
            }
            catch (ExcepcionesDIPCMI ex)
            {
                DesplegarError(ex.Message);
            }
            catch
            {
                DesplegarError("Error al almacenar Mapeo.");
            }
            finally
            {
                if (gestorSegurida != null) gestorSegurida.Dispose();
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