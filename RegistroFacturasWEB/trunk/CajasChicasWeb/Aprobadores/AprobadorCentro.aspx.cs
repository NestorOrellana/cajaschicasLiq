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
using System.Text;

namespace RegistroFacturasWEB.Aprobaciones
{
    public partial class AprobadorCentro : System.Web.UI.Page
    {

        #region Declaraciones
        string cnnApl = ConfigurationManager.ConnectionStrings["CnnApl"].ToString();
        string usuario;
        
        #endregion

        #region Eventos

        protected void Page_Load(object sender, EventArgs e)
        {
            usuario = Context.User.Identity.Name.ToString();
            string strUrl = Request.RawUrl;
            string strParam = strUrl.Substring(strUrl.IndexOf('?') + 1);

            if (!IsPostBack)
            {
                txtUsuario.Enabled = false;
                if (!strUrl.Contains('?'))
                    Response.Redirect("~/principal.aspx");

                if (!string.IsNullOrEmpty(strParam))
                {
                    CapturarParametros(strParam);

                    txtUsuario.Enabled = false;
                    txtUsuario.Text = hfUsuario.Value;
                    CargarAprobadorCentroGrid();
                    CargarDDLSociedad();
                    //CargarDDLCentroCosto();
                    //CargarDDLOrdenCompra();
                    //CargarDDLCentro();
                    CargarDDLNivel();
                    OcultarAvisos();
                }
                else
                    Response.Redirect("~/principal.aspx");
            }//else
            //CargarAprobadorCentroGrid();
        }
        protected void ddlCentroCosto_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlOrdenCompra.SelectedValue != "-1")
                ddlOrdenCompra.SelectedValue = "-1";
        }

        protected void btnGrabar_Click(object sender, EventArgs e)
        {
            AlmacenaAprobadorCentro();
        }

        protected void btnLimpiar_Click(object sender, EventArgs e)
        {
            LimpiarControles();
        }


        protected void gvAprobadorCentro_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            this.gvAprobadorCentro.PageIndex = e.NewPageIndex;
            CargarAprobadorCentroGrid();
        }

        protected void gvAprobadorCentro_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            OcultarAvisos();
            if (e.CommandName.Equals("Editar"))
            {
                CargarAprobadorCentroEdicion(e);
            }

            if (e.CommandName.Equals("Baja"))
            {
                DarBajaAprobadorCentro(e);
            }
            if (e.CommandName.Equals("Alta"))
            {
                DarAltaAprobadorCentro(e);
            }
        }
        #endregion

        #region Metodos

        private void CargarDDLSociedad()
        {
            GestorSociedad gestorSociedad = null;

            try
            {
                gestorSociedad = GestorSociedad();

                List<LlenarDDL_DTO> _listasociedadDto = gestorSociedad.ListaSociedadesActivas();
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
                if (gestorSociedad != null) gestorSociedad.Dispose();
            }
        }

        private void CargarDDLCentro(string codSociedad)
        {
            GestorSociedad gestorSociedad = null;

            try
            {
                gestorSociedad = GestorSociedad();

                List<LlenarDDL_DTO> _listacentroDto = gestorSociedad.ListarCentroMapeado(codSociedad);
                ddlCentro.Items.Clear();
                ddlCentro.Items.Add(new ListItem("::ELIJA UNA OPCION::", "-1"));
                ddlCentro.AppendDataBoundItems = true;
                ddlCentro.DataSource = _listacentroDto;
                ddlCentro.DataTextField = "DESCRIPCION";
                ddlCentro.DataValueField = "IDENTIFICADOR";
                ddlCentro.DataBind();
            }
            catch
            {
                DesplegarError("Error al recuperar los Centros");
            }
            finally
            {
                if (gestorSociedad != null) gestorSociedad.Dispose();
            }
        }

        private void CargarDDLCentroCosto(string usuario, string codSociedad)
        {
            GestorSociedad gestorSociedad = null;

            try
            {
                gestorSociedad = GestorSociedad();

                List<LlenarDDL_DTO> _listacentrocostsoDto = gestorSociedad.ListarCentroCostoUsuariosDDL(usuario, codSociedad);
                ddlCentroCosto.Items.Clear();
                ddlCentroCosto.Items.Add(new ListItem("::ELIJA UNA OPCION::", "-1"));
                ddlCentroCosto.AppendDataBoundItems = true;
                ddlCentroCosto.DataSource = _listacentrocostsoDto;
                ddlCentroCosto.DataTextField = "DESCRIPCION";
                ddlCentroCosto.DataValueField = "IDENTIFICADOR";
                ddlCentroCosto.DataBind();
            }
            catch
            {
                DesplegarError("Error al recuperar los Centros de Costo");
            }
            finally
            {
                if (gestorSociedad != null) gestorSociedad.Dispose();
            }
        }

        private void CargarDDLOrdenCompra(string usuario, string codSociedad)
        {
            GestorSociedad gestorSociedad = null;

            try
            {
                gestorSociedad = GestorSociedad();

                List<LlenarDDL_DTO> _listaOrdenCompraDto = gestorSociedad.ListarOrdenCostoUsuarioDDL(usuario, codSociedad);
                ddlOrdenCompra.Items.Clear();
                ddlOrdenCompra.Items.Add(new ListItem("::ELIJA UNA OPCION::", "-1"));
                ddlOrdenCompra.AppendDataBoundItems = true;
                ddlOrdenCompra.DataSource = _listaOrdenCompraDto;
                ddlOrdenCompra.DataTextField = "DESCRIPCION";
                ddlOrdenCompra.DataValueField = "IDENTIFICADOR";
                ddlOrdenCompra.DataBind();
            }
            catch
            {
                DesplegarError("Error al recuperar Ordenes de Compra");
            }
            finally
            {
                if (gestorSociedad != null) gestorSociedad.Dispose();
            }
        }

        public void CargarAprobadorCentroGrid()
        {
            AprobadorCentroDTO _centroDto = new AprobadorCentroDTO();
            GestorAprobadores gestoraprobadores = null;

            int x = 1;
            try
            {
                gestoraprobadores = Gestoraprobadores();
                gvAprobadorCentro.DataSource = (from aprobadorcentro in gestoraprobadores.ListaAprobadorCentro(Convert.ToInt32(hfIdUsuario.Value))
                                                select new
                                        {
                                            Numero = x++,
                                            ID_APROBADORCENTRO = aprobadorcentro.ID_APROBADORCENTRO,
                                            ID_USUARIO = aprobadorcentro.ID_USUARIO,
                                            CENTRO_COSTO = aprobadorcentro.KOSTL,
                                            ORDEN_COMPRA = aprobadorcentro.AUFNR,
                                            ID_CENTRO = aprobadorcentro.ID_SOCIEDAD_CENTRO,
                                            CENTRO = aprobadorcentro.CENTRO,
                                            ID_NIVEL = aprobadorcentro.ID_NIVEL,
                                            NIVEL = aprobadorcentro.NIVEL,
                                            idAlta = aprobadorcentro.ALTA,
                                            USUARIO_CREACION = aprobadorcentro.USUARIO_MANTENIMIENTO.USUARIO_ALTA,
                                            FECHA_CREACION = aprobadorcentro.USUARIO_MANTENIMIENTO.FECHA_ALTA,
                                            USUARIO_MODIFICACION = aprobadorcentro.USUARIO_MANTENIMIENTO.USUARIO_MODIFICO,
                                            FECHA_MODIFICACION = aprobadorcentro.USUARIO_MANTENIMIENTO.FECHA_MODIFICACION,
                                            CODIGO_SOCIEDAD = aprobadorcentro.SOCIEDAD
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
            hfIdUsuario.Value = "0";
            hfUsuario.Value = string.Empty;
            ddlCentroCosto.DataValueField = "-1";
            ddlOrdenCompra.DataValueField = "-1";
            ddlCentro.DataValueField = "-1";
            ddlNivel.DataValueField = "-1";
            ddlSociedad.DataValueField = "-1";
            chAlta.Checked = false;
            lblUsuarioCreacionBD.Text = string.Empty;
            lblFechaCreacionBD.Text = string.Empty;
            lblUsuarioDB.Text = string.Empty;
            lblFechaModificacionDB.Text = string.Empty;
            OcultarAvisos();
        }

        private AprobadorCentroDTO CargarObjetoAprobadorCentro(ref AprobadorCentroDTO _aprobadorcentroDto)
        {
            string Kostl, aufnr;
            if (ddlCentroCosto.SelectedValue == "-1") Kostl = ""; else Kostl = ddlCentroCosto.SelectedValue;
            if (ddlOrdenCompra.SelectedValue == "-1") aufnr = ""; else aufnr = ddlOrdenCompra.SelectedValue;

            _aprobadorcentroDto.ID_APROBADORCENTRO = Convert.ToInt32(hfIdAprobadorCentro.Value);
            _aprobadorcentroDto.ID_USUARIO = Convert.ToInt32(hfIdUsuario.Value);
            _aprobadorcentroDto.KOSTL = Kostl;
            _aprobadorcentroDto.AUFNR = aufnr;
            _aprobadorcentroDto.ID_SOCIEDAD_CENTRO = Convert.ToInt32(ddlCentro.SelectedValue);
            _aprobadorcentroDto.ID_NIVEL = Convert.ToInt16(ddlNivel.SelectedValue);
            _aprobadorcentroDto.ALTA = chAlta.Checked;
            _aprobadorcentroDto.USUARIO_MANTENIMIENTO.USUARIO_ALTA = usuario;
            _aprobadorcentroDto.USUARIO_MANTENIMIENTO.USUARIO_MODIFICO = usuario;

            return _aprobadorcentroDto;
        }

        private void CargarAprobadorCentroEdicion(GridViewCommandEventArgs e)
        {

            int indice = Int32.Parse(e.CommandArgument.ToString());
            GridViewRow fila = gvAprobadorCentro.Rows[indice];

            try{

            if (fila.Cells[3].Text == "&nbsp;") hfIdCentroCosto.Value = "-1"; else hfIdCentroCosto.Value = HttpUtility.HtmlDecode(fila.Cells[3].Text);
            if (fila.Cells[4].Text == "&nbsp;") hfIdCentroCompra.Value = "-1"; else hfIdCentroCompra.Value = HttpUtility.HtmlDecode(fila.Cells[4].Text);

            hfIdAprobadorCentro.Value = HttpUtility.HtmlDecode(fila.Cells[1].Text);
            hfIdUsuario.Value = HttpUtility.HtmlDecode(fila.Cells[2].Text);
            hfIdSociedad.Value = HttpUtility.HtmlDecode(fila.Cells[14].Text);
            ddlSociedad.SelectedValue = hfIdSociedad.Value;

            CargarDDLCentro(hfIdSociedad.Value);
            ddlCentro.SelectedValue = HttpUtility.HtmlDecode(fila.Cells[5].Text);

            //CargarDDLCentroCosto(hfUsuario.Value, hfIdSociedad.Value);
            //ddlCentroCosto.SelectedValue = HttpUtility.HtmlDecode(centrocosto);

            //CargarDDLOrdenCompra(hfUsuario.Value, hfIdSociedad.Value);
            //ddlOrdenCompra.SelectedValue = HttpUtility.HtmlDecode(ordencompra);

            CargarDDLOrdenCompra(hfUsuario.Value, hfIdSociedad.Value);
            ddlOrdenCompra.SelectedValue = hfIdCentroCompra.Value;

            CargarDDLCentroCosto(hfUsuario.Value, hfIdSociedad.Value);
            ddlCentroCosto.SelectedValue = hfIdCentroCosto.Value;


            ddlNivel.SelectedValue = HttpUtility.HtmlDecode(fila.Cells[7].Text);
            chAlta.Checked = (((CheckBox)fila.FindControl("idAlta")).Checked);
            lblUsuarioCreacionBD.Text = HttpUtility.HtmlDecode(fila.Cells[10].Text);
            lblFechaCreacionBD.Text = HttpUtility.HtmlDecode(fila.Cells[11].Text);
            lblUsuarioDB.Text = HttpUtility.HtmlDecode(fila.Cells[12].Text);
            lblFechaModificacionDB.Text = HttpUtility.HtmlDecode(fila.Cells[13].Text);
            }
            catch
            {
                DesplegarError("Error al Cargar Datos");
            }
            
        }

        private void AlmacenaAprobadorCentro()
        {
            AprobadorCentroDTO _aprobadorcentroDto = new AprobadorCentroDTO();
            GestorAprobadores gestoraprobadores = null;
            string mensaje = string.Empty;

            OcultarAvisos();
            try
            {

                if (hfIdAprobadorCentro.Value == "0")
                {
                    CargarDDLCentro(hfIdSociedad.Value);
                    ddlCentro.SelectedValue = hfIdCentro.Value;

                    CargarDDLOrdenCompra(hfUsuario.Value, hfIdSociedad.Value);
                    ddlOrdenCompra.SelectedValue = hfIdCentroCompra.Value;

                    CargarDDLCentroCosto(hfUsuario.Value, hfIdSociedad.Value);
                    ddlCentroCosto.SelectedValue = hfIdCentroCosto.Value;


                    //if (hfIdCentroCosto.Value != "0")
                    //{
                    //    CargarDDLOrdenCompra(hfIdSociedad.Value);
                    //    ddlOrdenCompra.SelectedValue = hfIdCentroCompra.Value;
                    //    CargarDDLCentroCosto(hfIdSociedad.Value);
                    //}
                    //else
                    //{
                    //    CargarDDLCentroCosto(hfIdSociedad.Value);
                    //    ddlCentroCosto.SelectedValue = hfIdCentroCosto.Value;
                    //    CargarDDLOrdenCompra(hfIdSociedad.Value);
                    //}
                }

                mensaje = ValidarDatos();

                if (!mensaje.Equals(string.Empty))
                    throw new ExcepcionesDIPCMI(mensaje);

                gestoraprobadores = Gestoraprobadores();

                CargarObjetoAprobadorCentro(ref _aprobadorcentroDto);
                gestoraprobadores.AlmacenarAprobadorCentro(_aprobadorcentroDto);
                hfIdAprobadorCentro.Value = _aprobadorcentroDto.ID_APROBADORCENTRO.ToString();
                DesplegarAviso("Aprobador Centro Almacenada Correctamente");
                CargarAprobadorCentroGrid();

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

        private void CapturarParametros(string parametros)
        {
            string strParametros = Encoding.UTF8.GetString(Convert.FromBase64String(parametros));
            string[] argsParam = strParametros.Split('&');
            string[] param;
            string usuario = string.Empty;

            param = argsParam[0].Split('=');
            if (param[0] == "Usuario")
            {
                hfUsuario.Value = param[1];
                param = argsParam[1].Split('=');
                hfIdUsuario.Value = param[1];
                return;
            }
        }

        private string ValidarDatos()
        {
            string mensaje = string.Empty;

            if (txtUsuario.Text == string.Empty)
                mensaje += "Debe Indicar un Aprobador. <br>";

            if ((ddlCentro.SelectedValue == "0") || (ddlCentro.SelectedValue == string.Empty) || (ddlCentro.SelectedValue == "-1"))
                mensaje += "Debe seleccionar un centro. <br>";

            if (((ddlCentroCosto.SelectedValue == "0") || (ddlCentroCosto.SelectedValue == string.Empty) || (ddlCentroCosto.SelectedValue == "-1")) && ((ddlOrdenCompra.SelectedValue == "0") || (ddlOrdenCompra.SelectedValue == string.Empty) || (ddlOrdenCompra.SelectedValue == "-1")))
                mensaje += "Debe seleccionar un centro de costo o un centro de compra. <br>";

            //if ((ddlCentroCosto.SelectedValue == "0") || (ddlCentroCosto.SelectedValue == string.Empty) || (ddlCentroCosto.SelectedValue == "-1"))
            //    mensaje += "Debe seleccionar un centro de costo. <br>";

            //if ((ddlOrdenCompra.SelectedValue == "0") || (ddlOrdenCompra.SelectedValue == string.Empty) || (ddlOrdenCompra.SelectedValue == "-1"))
            //    mensaje += "Debe seleccionar un centro de compra. <br>";

            if ((ddlNivel.SelectedValue == "0") || (ddlNivel.SelectedValue == string.Empty) || (ddlNivel.SelectedValue == "-1"))
                mensaje += "Debe seleccionar un Nivel. <br>";

            return mensaje;
        }

        #endregion

        #region Gestores
        private GestorAprobadores Gestoraprobadores()
        {
            GestorAprobadores gestoraprobadores = new GestorAprobadores(cnnApl);
            return gestoraprobadores;
        }

        private GestorSociedad GestorSociedad()
        {
            GestorSociedad gestorsociedad = new GestorSociedad(cnnApl);
            return gestorsociedad;
        }
        #endregion
    }
}