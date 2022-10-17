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
    public partial class AprobadorCentro : System.Web.UI.Page
    {

        #region Declaraciones
        string cnnApl = ConfigurationManager.ConnectionStrings["CnnApl"].ToString();
        string usuario = "WPISQUIY";
        Int32 idUsuario = 1;
#endregion 

        #region Eventos

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CargarAprobadorCentroGrid();
                //CargarDDLCentroCosto();
                CargarDDLOrdenCompra();
                CargarDDLCentro();
                CargarDDLNivel();
            }
        }

        protected void btnGrabar_Click(object sender, EventArgs e)
        {
            AlmacenaAprobadorCentro();
        }

        protected void btnLimpiar_Click(object sender, EventArgs e)
        {
            LimpiarControles();
        }

        protected void gvAprobadorCentro_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName.Equals("Editar"))
            {
                OcultarAvisos();
                CargarAprobadorCentroEdicion(e);
            }
            if (e.CommandName.Equals("Baja"))
            {
                OcultarAvisos();
                DarBajaAprobadorCentro(e);
            }
            if (e.CommandName.Equals("Alta"))
            {
                OcultarAvisos();
                DarAltaAprobadorCentro(e);
            }
        }
        #endregion

        #region Metodos
        public void CargarAprobadorCentroGrid()
        {
            AprobadorCentroDTO _centroDto = new AprobadorCentroDTO();
            GestorAprobadores gestoraprobadores = null;

            int x = 1;
            try
            {
                gestoraprobadores = Gestoraprobadores();
                gvAprobadorCentro.DataSource = (from aprobadorcentro in gestoraprobadores.ListaAprobadorCentro()
                                                select new
                                        {
                                           Numero = x++,
                                           ID_APROBADORCENTRO = aprobadorcentro.ID_APROBADORCENTRO,
                                           CENTRO_COSTO = aprobadorcentro.KOSTL, 
                                           ORDEN_COMPRA = aprobadorcentro.AUFNR,
                                           ID_CENTRO = aprobadorcentro.ID_CENTRO,
                                           CENTRO = aprobadorcentro.CENTRO,
                                           ID_USUARIO = aprobadorcentro.ID_USUARIO,
                                           USUARIO = aprobadorcentro.USUARIO,
                                           ID_NIVEL = aprobadorcentro.ID_NIVEL,
                                           NIVEL = aprobadorcentro.NIVEL,
                                           PORCENTAJE_COMPRA = aprobadorcentro.PORCENTAJE_COMPRA,
                                           TOLERANCIA = aprobadorcentro.TOLERANCIA,
                                           idAlta = aprobadorcentro.ALTA,
                                           USUARIO_CREACION = aprobadorcentro.USUARIO_MANTENIMIENTO.USUARIO_ALTA,
                                           FECHA_CREACION = aprobadorcentro.USUARIO_MANTENIMIENTO.FECHA_ALTA,
                                           USUARIO_MODIFICACION = aprobadorcentro.USUARIO_MANTENIMIENTO.USUARIO_MODIFICO,
                                           FECHA_MODIFICACION = aprobadorcentro.USUARIO_MANTENIMIENTO.FECHA_MODIFICACION 
                                       }).ToArray();
                gvAprobadorCentro.DataBind();
            }
            catch
            {
                DesplegarError("Error al Desplegar Aprobador Centro");
            }
            finally
            {
                if (gestoraprobadores != null) gestoraprobadores.Dispose();
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
            hfIdAprobadorCentro.Value = "0";
            ddlCentroCosto.DataValueField = "-1";
            ddlOrdenCompra.DataValueField = "-1";
            ddlCentro.DataValueField = "-1";
            ddlNivel.DataValueField = "-1";
            txtPorcentaje.Text = string.Empty;
            txtTolerancia.Text = string.Empty;
            chAlta.Checked = false;
            lblUsuarioCreacionBD.Text = string.Empty;
            lblFechaCreacionBD.Text = string.Empty;
            lblUsuarioDB.Text = string.Empty;
            lblFechaModificacionDB.Text = string.Empty;
            OcultarAvisos();
        }

        private AprobadorCentroDTO CargarObjetoAprobadorCentro(ref AprobadorCentroDTO _aprobadorcentroDto)
        {
            _aprobadorcentroDto.ID_APROBADORCENTRO = Convert.ToInt32(hfIdAprobadorCentro.Value);
            _aprobadorcentroDto.KOSTL = ddlCentroCosto.SelectedValue;
            _aprobadorcentroDto.AUFNR = ddlOrdenCompra.SelectedValue;
            _aprobadorcentroDto.ID_CENTRO = Convert.ToInt32(ddlCentro.SelectedValue);
            _aprobadorcentroDto.ID_USUARIO = idUsuario;
            _aprobadorcentroDto.ID_NIVEL = Convert.ToInt16(ddlNivel.SelectedValue);
            _aprobadorcentroDto.PORCENTAJE_COMPRA = Convert.ToInt16(txtPorcentaje.Text);
            _aprobadorcentroDto.TOLERANCIA = Convert.ToInt16(txtTolerancia.Text);
            _aprobadorcentroDto.ALTA = chAlta.Checked;
            _aprobadorcentroDto.USUARIO_MANTENIMIENTO.USUARIO_ALTA = usuario;
            _aprobadorcentroDto.USUARIO_MANTENIMIENTO.USUARIO_MODIFICO = usuario;

            return _aprobadorcentroDto;
        }

        private void CargarAprobadorCentroEdicion(GridViewCommandEventArgs e)
        {
            int indice = Int32.Parse(e.CommandArgument.ToString());
            GridViewRow fila = gvAprobadorCentro.Rows[indice];

            hfIdAprobadorCentro.Value = HttpUtility.HtmlDecode(fila.Cells[1].Text);
            ddlCentroCosto.SelectedValue = HttpUtility.HtmlDecode(fila.Cells[2].Text);
            ddlOrdenCompra.SelectedValue = HttpUtility.HtmlDecode(fila.Cells[3].Text);
            ddlCentro.SelectedValue = HttpUtility.HtmlDecode(fila.Cells[4].Text);
            idUsuario = Convert.ToInt32(fila.Cells[6].Text);
            txtUsuario.Text = HttpUtility.HtmlDecode(fila.Cells[7].Text);
            ddlNivel.SelectedValue = HttpUtility.HtmlDecode(fila.Cells[8].Text);
            txtPorcentaje.Text = HttpUtility.HtmlDecode(fila.Cells[10].Text);
            txtTolerancia.Text = HttpUtility.HtmlDecode(fila.Cells[11].Text);
            chAlta.Checked = (((CheckBox)fila.FindControl("idAlta")).Checked);
            lblUsuarioCreacionBD.Text = HttpUtility.HtmlDecode(fila.Cells[13].Text);
            lblFechaCreacionBD.Text = HttpUtility.HtmlDecode(fila.Cells[14].Text);
            lblUsuarioDB.Text = HttpUtility.HtmlDecode(fila.Cells[15].Text);
            lblFechaModificacionDB.Text = HttpUtility.HtmlDecode(fila.Cells[16].Text);
        }

        private void AlmacenaAprobadorCentro()
        {
            AprobadorCentroDTO _aprobadorcentroDto = new AprobadorCentroDTO();
            GestorAprobadores gestoraprobadores = null;

            OcultarAvisos();
            try
            {
                //VALIDACIONES
                //if (txtNombre.Text != "" && txtNombre.Text.Length <= 50)
                //{
                gestoraprobadores = Gestoraprobadores();
                CargarObjetoAprobadorCentro(ref _aprobadorcentroDto);
                gestoraprobadores.AlmacenarAprobadorCentro(_aprobadorcentroDto);
                hfIdAprobadorCentro.Value = _aprobadorcentroDto.ID_APROBADORCENTRO.ToString();
                DesplegarAviso("Aprobador Centro Almacenada Correctamente");
                CargarAprobadorCentroGrid();
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
                DesplegarError("Error al almacenar Aprobador Centro");
            }
            finally
            {
                if (gestoraprobadores != null) gestoraprobadores.Dispose();
            }
        }

        private void DarBajaAprobadorCentro(GridViewCommandEventArgs e)
        {
            int indice = Int32.Parse(e.CommandArgument.ToString());
            GridViewRow fila = gvAprobadorCentro.Rows[indice];
            GestorAprobadores gestoraprobadores = null;

            try
            {
                gestoraprobadores = Gestoraprobadores();
                gestoraprobadores.DarBajaAprobadorCentro(Convert.ToInt16(fila.Cells[1].Text), usuario);

                DesplegarAviso("Aprobador Centro fue dado de baja");
                CargarAprobadorCentroGrid();
            }
            catch
            {
                DesplegarError("Error al dar de baja Aprobador Centro");
            }
            finally
            {
                if (gestoraprobadores != null) gestoraprobadores.Dispose();
            }
        }

        private void DarAltaAprobadorCentro(GridViewCommandEventArgs e)
        {
            int indice = Int32.Parse(e.CommandArgument.ToString());
            GridViewRow fila = gvAprobadorCentro.Rows[indice];
            GestorAprobadores gestoraprobadores = null;

            try
            {
                gestoraprobadores = Gestoraprobadores();
                gestoraprobadores.DarAltaAprobadorCentro(Convert.ToInt16(fila.Cells[1].Text), usuario);

                DesplegarAviso("Aprobador Centro fue dado de alta");
                CargarAprobadorCentroGrid();
            }
            catch
            {
                DesplegarError("Error al dar alta Aprobador Centro");
            }
            finally
            {
                if (gestoraprobadores != null) gestoraprobadores.Dispose();
            }
        }

        //private void CargarDDLCentroCosto()
        //{
        //    GestorAprobadorCentro gestoraprovadorcentro = null;

        //    try
        //    {
        //        gestoraprovadorcentro = GestorAprobadorcentro();

        //        List<SAPCentroCostoDTO> _listaCentroCosto = gestoraprovadorcentro.ListaCentroCosto();

        //        ddlCentroCosto.Items.Clear();
        //        ddlCentroCosto.Items.Add(new ListItem("::ELIJA UNA OPCION::", "-1"));
        //        ddlCentroCosto.AppendDataBoundItems = true;
        //        ddlCentroCosto.DataSource = _listaCentroCosto;
        //        ddlCentroCosto.DataTextField = "KOSTL";
        //        ddlCentroCosto.DataValueField = "KOSTL";
        //        ddlCentroCosto.DataBind();
        //    }
        //    catch
        //    {
        //        DesplegarError("Error al recuperar el Centro de Costo");
        //    }
        //    finally
        //    {
        //        if (gestoraprovadorcentro != null) gestoraprovadorcentro.Dispose();
        //    }

        //}

        private void CargarDDLOrdenCompra()
        {
            GestorAprobadores gestoraprobadores = null;

            try
            {
                gestoraprobadores = Gestoraprobadores();

                List<SAPOrdenesGastosDTO> _listaOrdenCompra = gestoraprobadores.ListaOrdenCompra();

                ddlOrdenCompra.Items.Clear();
                ddlOrdenCompra.Items.Add(new ListItem("::ELIJA UNA OPCION::", "-1"));
                ddlOrdenCompra.AppendDataBoundItems = true;
                ddlOrdenCompra.DataSource = _listaOrdenCompra;
                ddlOrdenCompra.DataTextField = "AUFNR";
                ddlOrdenCompra.DataValueField = "AUFNR";
                ddlOrdenCompra.DataBind();
            }
            catch
            {
                DesplegarError("Error al recuperar Orden de Compra");
            }
            finally
            {
                if (gestoraprobadores != null) gestoraprobadores.Dispose();
            }
        }

        private void CargarDDLCentro()
        {
            GestorAprobadores gestoraprobadores = null;

            try
            {
                gestoraprobadores = Gestoraprobadores();

                List<CentroDTO> _listaCentro = gestoraprobadores.ListaCentro();

                ddlCentro.Items.Clear();
                ddlCentro.Items.Add(new ListItem("::ELIJA UNA OPCION::", "-1"));
                ddlCentro.AppendDataBoundItems = true;
                ddlCentro.DataSource = _listaCentro;
                ddlCentro.DataTextField = "NOMBRE";
                ddlCentro.DataValueField = "ID_CENTRO";
                ddlCentro.DataBind();
            }
            catch
            {
                DesplegarError("Error al recuperar el Centro");
            }
            finally
            {
                if (gestoraprobadores != null) gestoraprobadores.Dispose();
            }

        }

        private void CargarDDLNivel()
        {
            GestorAprobadores gestoraprobadores = null;

            try
            {
                gestoraprobadores = Gestoraprobadores();

                List<NivelDTO> _listaNivel = gestoraprobadores.ListaNivel();

                ddlNivel.Items.Clear();
                ddlNivel.Items.Add(new ListItem("::ELIJA UNA OPCION::", "-1"));
                ddlNivel.AppendDataBoundItems = true;
                ddlNivel.DataSource = _listaNivel;
                ddlNivel.DataTextField = "NIVEL";
                ddlNivel.DataValueField = "ID_NIVEL";
                ddlNivel.DataBind();
            }
            catch
            {
                DesplegarError("Error al recuperar Niveles");
            }
            finally
            {
                if (gestoraprobadores != null) gestoraprobadores.Dispose();
            }

        }
        #endregion

        #region Gestores
        private GestorAprobadores Gestoraprobadores()
        {
            GestorAprobadores gestoraprobadores = new GestorAprobadores(cnnApl);
            return gestoraprobadores;
        }
        #endregion


    }
}