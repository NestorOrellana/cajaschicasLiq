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
    public partial class SociedadCentro : System.Web.UI.Page
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
                CargarDDLCentro();
                CargarDDLSociedad();
                CargarSociedadCentroGrid();
            }
        }

        protected void btnGrabar_Click(object sender, EventArgs e)
        {
            AlmacenarSociedadCentro();
        }

        protected void btnLimpiar_Click(object sender, EventArgs e)
        {
            LimpiarControles();
        }

        protected void gvSociedadCentro_RowCommand(object sender, GridViewCommandEventArgs e)
        {

            if (e.CommandName.Equals("Baja"))
            {
                OcultarAvisos();
                DarBajaSociedadCentro(e);
            }

            if (e.CommandName.Equals("Alta"))
            {
                OcultarAvisos();
                DarAltaSociedadCentro(e);
            }
        }

        protected void gvSociedadCentro_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            this.gvSociedadCentro.PageIndex = e.NewPageIndex;
            CargarSociedadCentroGrid();
        }
        #endregion

        #region Metodos
        private string ValidarDatos()
        {
            string mensaje = string.Empty;

            if ((ddlSociedad.SelectedValue == "0") || (ddlSociedad.SelectedValue == "-1") || (ddlSociedad.SelectedValue == string.Empty))
                mensaje += "Debe seleccionar una sociedad. <br>";

            if ((ddlCentro.SelectedValue == "0") || (ddlCentro.SelectedValue == string.Empty) || (ddlCentro.SelectedValue == "-1"))
                mensaje += "Debe seleccionar un centro. <br>";

            return mensaje;
        }
        
        private void DarBajaSociedadCentro(GridViewCommandEventArgs e)
        {
            int indice = Int32.Parse(e.CommandArgument.ToString());
            GridViewRow fila = gvSociedadCentro.Rows[indice];
            GestorSociedad gestorsociedad = null;

            try
            {
                gestorsociedad = GestorSociedad();
                gestorsociedad.DarBajaSociedadCentro(Convert.ToInt32(fila.Cells[1].Text), usuario);

                DesplegarAviso("La Sociedad Centro fue dada de baja.");
                CargarSociedadCentroGrid();
            }
            catch
            {
                DesplegarError("Error al dar baja Sociedad Centro");
            }
            finally
            {
                if (gestorsociedad != null) gestorsociedad.Dispose();
            }
        }
        private void DarAltaSociedadCentro(GridViewCommandEventArgs e)
        {
            int indice = Int32.Parse(e.CommandArgument.ToString());
            GridViewRow fila = gvSociedadCentro.Rows[indice];
            GestorSociedad gestorsociedad = null;

            try
            {
                gestorsociedad = GestorSociedad();
                gestorsociedad.DarAltaSociedadCentro(Convert.ToInt32(fila.Cells[1].Text), usuario);

                DesplegarAviso("La Sociedad Centro fue dada de alta.");
                CargarSociedadCentroGrid();
            }
            catch
            {
                DesplegarError("Error al dar alta la Sociedad Centro");
            }
            finally
            {
                if (gestorsociedad != null) gestorsociedad.Dispose();
            }
        }
        public void CargarSociedadCentroGrid()
        {
            SociedadCentroDTO _socieadCentroDto = new SociedadCentroDTO();
            GestorSociedad gestorsociedad = null;

            int x = 1;
            try
            {
                gestorsociedad = GestorSociedad();
                gvSociedadCentro.DataSource = (from sociedadcentro in gestorsociedad.ListaSociedadCentro()
                                       select new
                                       {
                                           Numero = x++,
                                           ID_SOCIEDAD_CENTRO = sociedadcentro.ID_SOCIEDAD_CENTRO,
                                           ID_SOCIEDAD = sociedadcentro.CODIGO_SOCIEDAD,
                                           SOCIEDAD = sociedadcentro.NOMBRE_SOCIEDAD,
                                           ID_CENTRO = sociedadcentro.ID_CENTRO,
                                           CENTRO = sociedadcentro.NOMBRE_CENTRO,
                                           idAlta = sociedadcentro.ALTA,
                                           USUARIO_CREACION = sociedadcentro.USUARIO_MANTENIMIENTO.USUARIO_ALTA,
                                           FECHA_CREACION = sociedadcentro.USUARIO_MANTENIMIENTO.FECHA_ALTA,
                                           USUARIO_MODIFICACION = sociedadcentro.USUARIO_MANTENIMIENTO.USUARIO_MODIFICO,
                                           FECHA_MODIFICACION = sociedadcentro.USUARIO_MANTENIMIENTO.FECHA_MODIFICACION
                                       }).ToArray();
                gvSociedadCentro.DataBind();
            }
            catch
            {
                DesplegarError("Error al Desplegar los Datos");
            }
            finally
            {
                if (gestorsociedad != null) gestorsociedad.Dispose();
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
            OcultarAvisos();
            ddlCentro.SelectedValue = "-1";
            ddlSociedad.SelectedValue = "-1";

        }

        private void CargarDDLCentro()
        {
            GestorCentro gestorcentro = null;
            try
            {
                gestorcentro = GestorCentro();
                List<CentroDTO> _listacentroDto = gestorcentro.ListaCentroDDL();

                ddlCentro.Items.Clear();
                ddlCentro.Items.Add(new ListItem("::ELIJA UNA OPCION::", "-1"));
                ddlCentro.AppendDataBoundItems = true;
                ddlCentro.DataSource = _listacentroDto;
                ddlCentro.DataTextField = "NOMBRE";
                ddlCentro.DataValueField = "ID_CENTRO";
                ddlCentro.DataBind();
            }
            catch
            {
                DesplegarError("Error al recuperar Centros");
            }
            finally
            {
                if (gestorcentro != null) gestorcentro.Dispose();
            }
        }

        private void CargarDDLSociedad()
        {
            GestorSociedad gestorsociedad = null;
            try
            {
                gestorsociedad = GestorSociedad();
                List<LlenarDDL_DTO> _listasociedadDto = gestorsociedad.ListaSociedadesActivas();


                ddlSociedad.Items.Clear();
                ddlSociedad.Items.Add(new ListItem("::ELIJA UNA OPCION::", "-1"));
                ddlSociedad.AppendDataBoundItems = true;
                ddlSociedad.DataSource = _listasociedadDto;
                ddlSociedad.DataTextField = "DESCRIPCION";
                ddlSociedad.DataValueField = "IDENTIFICADOR";
                ddlSociedad.DataBind();

            }
            catch
            {
                DesplegarError("Error al recuperar Sociedades");
            }
            finally
            {
                if (gestorsociedad != null) gestorsociedad.Dispose();
            }
        }

        private SociedadCentroDTO CargarObjetoSociedadCentro(ref SociedadCentroDTO _sociedadCentroDto)
        {
            _sociedadCentroDto.ID_SOCIEDAD_CENTRO = Convert.ToInt32(hfIdSociedadCentro.Value);
            _sociedadCentroDto.CODIGO_SOCIEDAD = ddlSociedad.SelectedValue;
            _sociedadCentroDto.ID_CENTRO = Convert.ToInt32(ddlCentro.SelectedValue);
            _sociedadCentroDto.USUARIO_MANTENIMIENTO.USUARIO_ALTA = usuario;
            _sociedadCentroDto.USUARIO_MANTENIMIENTO.USUARIO_MODIFICO = usuario;

            return _sociedadCentroDto;
        }

        private void AlmacenarSociedadCentro()
        {
            SociedadCentroDTO _sociedadcentroDto = new SociedadCentroDTO();
            GestorSociedad gestorsociedad = null;
            string mensaje = string.Empty;

            OcultarAvisos();
            try
            {
                    //if (ValidaCampos())
                    //{
                      mensaje = ValidarDatos();

                     if (!mensaje.Equals(string.Empty))
                        throw new ExcepcionesDIPCMI(mensaje);

                        gestorsociedad = GestorSociedad();
                        CargarObjetoSociedadCentro(ref _sociedadcentroDto);

                        gestorsociedad.AlmacenarSociedadCentro(_sociedadcentroDto);
                        hfIdSociedadCentro.Value = _sociedadcentroDto.ID_SOCIEDAD_CENTRO.ToString();
                        DesplegarAviso("La Sociedad Centro fue almacenada correctamente");
                        CargarSociedadCentroGrid();
                    //}
                    //else DesplegarError("Debe Completar todos los datos correctamente");
            }
            catch (ExcepcionesDIPCMI ex)
            {
                DesplegarError(ex.Message);
            }
            catch
            {
                DesplegarError("Error al almacenar la Sociedad Centro.");
            }
            finally
            {
                if (gestorsociedad != null) gestorsociedad.Dispose();
            }
        }

        #endregion

        #region Gestores
        protected GestorSociedad GestorSociedad()
        {
            GestorSociedad gestorsociead = new GestorSociedad(cnnApl);
            return gestorsociead;
        }

        protected GestorCentro GestorCentro()
        {
            GestorCentro gestorcentro = new GestorCentro(cnnApl);
            return gestorcentro;
        }
        #endregion
    }
}