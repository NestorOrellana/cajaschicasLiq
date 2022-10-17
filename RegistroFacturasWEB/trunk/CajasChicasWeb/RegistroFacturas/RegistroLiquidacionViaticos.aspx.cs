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
using LogicaCajasChicas.Enum;
using LogicaCajasChicas;
using System.Web.Services;

namespace RegistroFacturasWEB.RegistroFacturas
{
    public partial class RegistroLiquidacionViaticos : System.Web.UI.Page
    {

        #region Declaraciones

        private string cnnApl = ConfigurationManager.ConnectionStrings["CnnApl"].ToString();
        private string ccGenerica = ConfigurationManager.AppSettings["CCGenerica"].ToString();
        private string usuario;
        private string tipoCuenta = ConfigurationManager.AppSettings["TIPO_CUENTA"].ToString();
        string MonedaCC;

        #endregion

        #region Eventos

        protected void Page_Load(object sender, EventArgs e)
        {
            usuario = Context.User.Identity.Name.ToString();
            string strUrl = Request.RawUrl;
            string strParam = strUrl.Substring(strUrl.IndexOf('?') + 1);

            if (!IsPostBack)
            {
                if (!strUrl.Contains('?'))
                    Response.Redirect("~/principal.aspx");

                if (!string.IsNullOrEmpty(strParam))
                {
                    CapturarParametros(strParam);
                    CargarTipoDoc();
                    CargarCentroCostoUsuario(usuario, Convert.ToInt32(hfSociedadCentro.Value));
                    CargarOrdenCostoUsuario(usuario, Convert.ToInt32(hfSociedadCentro.Value));
                    if (hfPais.Value.ToString() == "SV")
                    //((hfCodigoSociedad.Value.ToString() == "1240") || (hfCodigoSociedad.Value.ToString() == "1570") || (hfCodigoSociedad.Value.ToString() == "1250")
                    //|| (hfCodigoSociedad.Value.ToString() == "1550") || (hfCodigoSociedad.Value.ToString() == "1270"))
                    {
                        lblRetencion.Visible = true;
                        chRetencion.Visible = true;
                    }
                    if (hfPais.Value.ToString() == "CR")    //((hfCodigoSociedad.Value.ToString() == "1440") || (hfCodigoSociedad.Value.ToString() == "1630"))
                    {
                        this.gvDetalleFactura.Columns[6].HeaderText = "IMPUESTO 10%";
                        txtValReal.Visible = true;
                        lblValReal.Visible = true;
                        lblFacturaEspecial.Visible = false;
                        cbFacturaEspecial.Visible = false;

                        lblImpuestoServ.Visible = true;
                        chImpuestoServ.Visible = true;

                        btnAgregarFila.Visible = false;
                        this.gvDetalleFactura.Columns[15].Visible = false;

                        if (MonedaCC == "USD")
                        {
                            lblTipoCambio.Visible = true;
                            txtTCambio.Visible = true;
                        }
                    }
                    else
                        this.gvDetalleFactura.Columns[14].Visible = false;
                    if (Convert.ToDecimal(hfIdFactura.Value) > 0)
                    {
                        CargarDatosFactura();

                        foreach (GridViewRow row in gvDetalleFactura.Rows)
                        {
                            TextBox txtDescripcion = (TextBox)row.FindControl("txtDescripcion");
                            TextBox txtImpuestoServ = (TextBox)row.FindControl("txtImpuesto");
                            if (hfPais.Value.ToString() == "CR")
                            //((hfCodigoSociedad.Value.ToString() == "1440") || (hfCodigoSociedad.Value.ToString() == "1630"))
                            {
                                this.gvDetalleFactura.Columns[15].Visible = false;
                                this.gvDetalleFactura.Columns[6].HeaderText = "IMPUESTO 10%";
                                txtDescripcion.Enabled = false;

                                if (chImpuestoServ.Checked == true)
                                    txtImpuestoServ.Enabled = false;

                            }
                            else
                                this.gvDetalleFactura.Columns[14].Visible = false;
                        }
                    }
                    else
                        AgregarFila();
                    llenarColumnaImpuestos();
                }
                else
                    Response.Redirect("~/principal.aspx");
            }

        }

        protected void chDiferentesCO_CheckedChanged(object sender, EventArgs e)
        {
            if (chDiferentesCO.Checked == true)
            {
                txtTotalFactCO.Text = "";
                txtTotalFactCO.Enabled = true;
            }
            else
            {
                txtTotalFactCO.Text = "Total Factura 0.00";
                txtTotalFactCO.Enabled = false;
            }
        }

        protected void btnGrabar_Click(object sender, EventArgs e)
        {
            OcultarAvisos();
            AlmacenarFactura();
        }

        protected void btnNuevaFactura_Click(object sender, EventArgs e)
        {
            OcultarAvisos();
            LimpiarDatosPantalla();
        }
        #endregion

        #region Metodos

        private void llenarColumnaImpuestos()
        {
            GestorSociedad gestorSociedad = null; ;
            List<LlenarDDL_DTO> listaImpuestos = null;

            try
            {
                gestorSociedad = GestorSociedad();
                listaImpuestos = gestorSociedad.LlenarImpuetos(hfCodigoSociedad.Value);
            }
            finally
            {
                if (gestorSociedad != null) gestorSociedad.Dispose();
            }

            foreach (GridViewRow row in gvDetalleFactura.Rows)
            {
                DropDownList ddlImpuestos = (DropDownList)row.FindControl("ddlImpuestos");

                ddlImpuestos.Items.Clear();
                ddlImpuestos.Items.Add(new ListItem("::Impuesto::", "0"));
                ddlImpuestos.AppendDataBoundItems = true;
                ddlImpuestos.DataSource = listaImpuestos;
                ddlImpuestos.DataTextField = "DESCRIPCION";
                ddlImpuestos.DataValueField = "IDENTIFICADOR";
                ddlImpuestos.DataBind();
            }

            if (listaImpuestos.Count == 0)
            {
                gvDetalleFactura.Columns[7].Visible = false;
                // gvDetalleFactura.Columns[8].Visible = true;
            }
            else
            {
                gvDetalleFactura.Columns[7].Visible = true;
                //gvDetalleFactura.Columns[8].Visible = false;
            }
        }

        private void CapturarParametros(string parametros)
        {
            string strParametros = Encoding.UTF8.GetString(Convert.FromBase64String(parametros));
            string[] argsParam = strParametros.Split('&');
            string[] param;
            string nombreSociedad = string.Empty;
            string nombreCentro = string.Empty;
            string codigoCajaChica = string.Empty;
            string moneda = string.Empty;
            string codigoSociedad = string.Empty;
            string nombreCC = string.Empty;
            string pais = string.Empty;
            DateTime fechaInicioViaje = new DateTime();
            DateTime fechaFinViaje = new DateTime();
            string objetivo = string.Empty;
            string numeroDias = string.Empty;
            string viaticosRecibidos = string.Empty;
            string idNivel = string.Empty;

            param = argsParam[0].Split('=');
            if (param[0] == "idFactura")
            {
                hfIdFactura.Value = param[1];
                lblIdFactura.Text = hfIdFactura.Value;

                param = argsParam[1].Split('=');
                hfCajaChicaSAP.Value = param[1];

                param = argsParam[2].Split('=');
                hfSociedadCentro.Value = param[1];

                param = argsParam[3].Split('=');
                hfCodigoSociedad.Value = param[1];

                param = argsParam[4].Split('=');
                pais = param[1];
                hfPais.Value = pais;

                nombreCC = hfNombreCC.Value;

                return;
            }
            else
                hfIdCajaChica.Value = param[1];

            param = argsParam[1].Split('=');
            hfCodigoSociedad.Value = param[1];

            param = argsParam[2].Split('=');
            hfCodigoCentro.Value = param[1];

            param = argsParam[3].Split('=');
            nombreSociedad = param[1];

            param = argsParam[4].Split('=');
            nombreCentro = param[1];

            param = argsParam[5].Split('=');
            codigoCajaChica = param[1];

            param = argsParam[6].Split('=');
            hfSociedadCentro.Value = param[1];

            param = argsParam[7].Split('=');
            hfCajaChicaSAP.Value = param[1];

            param = argsParam[8].Split('=');
            moneda = param[1];
            MonedaCC = moneda;

            param = argsParam[9].Split('=');
            nombreCC = param[1];

            param = nombreCC.Split('-');
            nombreCC = param[1];
            nombreCC = nombreCC.Replace(" CC ", "");

            param = argsParam[10].Split('=');
            pais = param[1];

            param = argsParam[11].Split('=');
            fechaInicioViaje = Convert.ToDateTime(param[1]);

            param = argsParam[12].Split('=');
            fechaFinViaje = Convert.ToDateTime(param[1]);

            param = argsParam[13].Split('=');
            objetivo = param[1];

            param = argsParam[14].Split('=');
            numeroDias = param[1];

            param = argsParam[15].Split('=');
            viaticosRecibidos = param[1];

            param = argsParam[16].Split('=');
            idNivel = param[1];

            lblIdFactura.Text = hfIdFactura.Value;
            hfPais.Value = pais;
            lblInforCC.Text = string.Concat("Sociedad: ", nombreSociedad, " - Centro: ", nombreCentro, " - Codigo caja chica: ", codigoCajaChica, "- ", nombreCC, "- Moneda:", moneda);
        }

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

        private void CargarTipoDoc()
        {
            GestorProveedor gestorProveedor = null;
            List<TipoDocumentoDTO> listaDDLDto = new List<TipoDocumentoDTO>();

            try
            {
                gestorProveedor = GestorProveedor();
                listaDDLDto = gestorProveedor.ListaTipoDocumentoActivo(Convert.ToInt32(hfSociedadCentro.Value));
                ddlTipoDocumento.Items.Clear();
                ddlTipoDocumento.Items.Add(new ListItem("::Documento identificación::", "-1"));
                ddlTipoDocumento.AppendDataBoundItems = true;
                ddlTipoDocumento.DataSource = listaDDLDto;
                ddlTipoDocumento.DataTextField = "Descripcion";
                ddlTipoDocumento.DataValueField = "Id_Tipo_Documento";
                ddlTipoDocumento.DataBind();
            }
            catch
            {
                DesplegarError("Error al recuperar tipos de documentos");
            }
            finally
            {
                if (gestorProveedor != null) gestorProveedor.Dispose();
            }
        }

        private void CargarCentroCostoUsuario(string usuario, Int32 idUsuarioCentro)
        {
            GestorSociedad gestorSociedad = null;
            List<UsuarioCentroCostoDTO> listaDDLDto = new List<UsuarioCentroCostoDTO>();

            try
            {
                gestorSociedad = GestorSociedad();
                listaDDLDto = gestorSociedad.ListaCuentaCosto(usuario, idUsuarioCentro);

                ddlCentroCosto.Items.Clear();
                ddlCentroCosto.Items.Add(new ListItem("::Centro costo::", "-1"));
                ddlCentroCosto.AppendDataBoundItems = true;
                ddlCentroCosto.DataSource = listaDDLDto;
                ddlCentroCosto.DataTextField = "CENTRO_COSTO_DESCRIPCION";
                ddlCentroCosto.DataValueField = "CENTRO_COSTO";
                ddlCentroCosto.DataBind();
            }
            catch
            {
                DesplegarError("Error al recuperar centros de costo");
            }
            finally
            {
                if (gestorSociedad != null) gestorSociedad.Dispose();
            }
        }

        private void CargarOrdenCostoUsuario(string usuario, Int32 idUsuarioCentro)
        {
            GestorSociedad gestorSociedad = null;
            List<UsuarioOrdenCostoDTO> listaDDLDto = new List<UsuarioOrdenCostoDTO>();

            try
            {
                gestorSociedad = GestorSociedad();
                listaDDLDto = gestorSociedad.ListarUsuarioOrdenCosto(usuario, idUsuarioCentro);

                ddlOrdenCosto.Items.Clear();
                ddlOrdenCosto.Items.Add(new ListItem("::Centro costo::", "-1"));
                ddlOrdenCosto.AppendDataBoundItems = true;
                ddlOrdenCosto.DataSource = listaDDLDto;
                ddlOrdenCosto.DataTextField = "ORDEN_COSTO_DESCRIPCION";
                ddlOrdenCosto.DataValueField = "ORDEN_COSTO";
                ddlOrdenCosto.DataBind();
            }
            catch
            {
                DesplegarError("Error al recuperar orden de costo");
            }
            finally
            {
                if (gestorSociedad != null) gestorSociedad.Dispose();
            }
        }

        private void CopiarDatosGrid(ref List<FacturaDetalleDTO> _listaFacturaDetalleDto)
        {
            FacturaDetalleDTO _facturaDetalleDto = new FacturaDetalleDTO();
            Int16 numero = 1;

            foreach (GridViewRow row in gvDetalleFactura.Rows)
            {
                _facturaDetalleDto = new FacturaDetalleDTO();

                _facturaDetalleDto.NUMERO = numero++;
                _facturaDetalleDto.ID_FACTURA_DETALLE = 0;
                _facturaDetalleDto.DESCRIPCION = ((TextBox)row.FindControl("txtDescripcion")).Text;
                _facturaDetalleDto.CANTIDAD = string.IsNullOrEmpty(((TextBox)row.FindControl("txtCantidad")).Text) ? 0 : Convert.ToDouble(((TextBox)row.FindControl("txtCantidad")).Text);
                _facturaDetalleDto.IVA = Convert.ToDouble(((HiddenField)row.FindControl("hfIVAgd")).Value);
                _facturaDetalleDto.VALOR = Convert.ToDouble(((TextBox)row.FindControl("txtValorTotal")).Text) - Convert.ToDouble(((TextBox)row.FindControl("txtImpuesto")).Text);
                _facturaDetalleDto.IMPUESTO = Convert.ToDouble(((TextBox)row.FindControl("txtImpuesto")).Text);
                _facturaDetalleDto.CUENTA_CONTABLE = ((HiddenField)row.FindControl("hfCuentaContable")).Value;
                _facturaDetalleDto.DEFINICION_CC = ((HiddenField)row.FindControl("hfDefinicionCC")).Value;
                _facturaDetalleDto.IDENTIFICADOR_IVA = ((HiddenField)row.FindControl("hfTipoIva")).Value;
                _facturaDetalleDto.IMPORTE = Convert.ToInt16(((HiddenField)row.FindControl("hfImporte")).Value);
                _facturaDetalleDto.DEFINICION_TIPO_IVA = ((HiddenField)row.FindControl("hfTipoIVAD")).Value;


                _listaFacturaDetalleDto.Add(_facturaDetalleDto);
            }
        }

        private FacturaDetalleDTO CrearNuevaLineaGrid()
        {
            FacturaDetalleDTO _facturaDetalleDto = new FacturaDetalleDTO();

            _facturaDetalleDto.NUMERO = gvDetalleFactura.Rows.Count + 1;
            _facturaDetalleDto.CANTIDAD = 0;
            _facturaDetalleDto.VALOR = 0;
            _facturaDetalleDto.IVA = 0;
            _facturaDetalleDto.CUENTA_CONTABLE = "::Cuenta contable::";
            _facturaDetalleDto.CARGO_ABONO = 0;
            _facturaDetalleDto.IDENTIFICADOR_IVA = null;
            _facturaDetalleDto.IMPUESTO = 0;
            _facturaDetalleDto.DEFINICION_TIPO_IVA = "::Seleccione tipo IVA::";
            _facturaDetalleDto.ID_FACTURA = 0;

            return _facturaDetalleDto;
        }

        private void AgregarFila()
        {
            List<FacturaDetalleDTO> _listaFacturaDetalleDto = new List<FacturaDetalleDTO>();
            string mensaje = string.Empty;

            try
            {
                CopiarDatosGrid(ref _listaFacturaDetalleDto);

                mensaje = ValidarFilasCompletas(_listaFacturaDetalleDto);

                if (_listaFacturaDetalleDto.Count != 0)
                {
                    if (!string.IsNullOrEmpty(mensaje))
                        throw new ExcepcionesDIPCMI(mensaje);
                }

                _listaFacturaDetalleDto.Add(CrearNuevaLineaGrid());

                lblSumaCompra.Text = hfSumaCompra.Value.ToString();
                lblSumaIVA.Text = hfSumaIva.Value.ToString();

                CargarDatosGrid(_listaFacturaDetalleDto);
            }
            catch (ExcepcionesDIPCMI ex)
            {
                DesplegarError(ex.Message);
            }
        }

        private void CargarDatosGrid(List<FacturaDetalleDTO> _listaFacturaDetalleDto)
        {
            gvDetalleFactura.DataSource = (from detalle in _listaFacturaDetalleDto
                                           select new
                                           {
                                               ID_FACTURA_DETALLE = 0,
                                               NUMERO = detalle.NUMERO,
                                               descripcion = detalle.DESCRIPCION,
                                               cantidad = detalle.CANTIDAD.ToString(), //("N2"),
                                               iva = detalle.IVA.ToString(), //("N2"),
                                               hfiva = detalle.IVA.ToString(), //("N2"),
                                               valorTotal = detalle.VALOR.ToString(), //("N2"),
                                               impuesto = detalle.IMPUESTO.ToString(), //("N2"),

                                               CUENTACONTABLE = detalle.CUENTA_CONTABLE,
                                               hfCC = detalle.CUENTA_CONTABLE,
                                               definicionCC = detalle.DEFINICION_CC,
                                               hfDCC = detalle.DEFINICION_CC,
                                               tipoIVA = detalle.IDENTIFICADOR_IVA,
                                               hfTIVA = detalle.IDENTIFICADOR_IVA,
                                               hfTID = detalle.DEFINICION_TIPO_IVA,
                                               tipoIVADefinicion = detalle.DEFINICION_TIPO_IVA,
                                               hfImporteIVA = detalle.IMPORTE,
                                               DetalleCR = detalle.DETALLE
                                           }).ToArray();
            gvDetalleFactura.DataBind();
            LlenarColumnaDdlCuentaContable(hfCodigoSociedad.Value);
            //LlenarColumnaDdlTipoIva(hfCajaChicaSAP.Value, usuario);
            LlenarColumnaDdlTipoIva(hfCodigoSociedad.Value, usuario);
            llenarColumnaImpuestos();
        }

        private void LlenarColumnaDdlCuentaContable(string sociedad)
        {
            //foreach (GridViewRow row in gvDetalleFactura.Rows)
            //{
            //    DropDownList ddlCuentaContable = (DropDownList)row.FindControl("ddlCuentaContable");

            //    ddlCuentaContable.Items.Clear();
            //    ddlCuentaContable.Items.Add(new ListItem("::Cuenta contable::", "-1"));
            //    ddlCuentaContable.AppendDataBoundItems = true;
            //}

            GestorSociedad gestorSociedad = null; ;
            List<LlenarDDL_DTO> listaFacturaDetalleDto = null;

            try
            {
                gestorSociedad = GestorSociedad();

                if (ddlCentroCosto.SelectedValue != "-1")
                    //listaFacturaDetalleDto = gestorSociedad.BuscarCuentasContablesCentroCosto(ddlCentroCosto.SelectedValue, hfCodigoSociedad.Value);
                    listaFacturaDetalleDto = gestorSociedad.BuscarCuentasContablesCentroCosto(usuario, hfCodigoSociedad.Value, ddlCentroCosto.SelectedValue);

                if (ddlOrdenCosto.SelectedValue != "-1")
                    //listaFacturaDetalleDto = gestorSociedad.BuscarCuentasContablesCentroCosto(ddlCentroCosto.SelectedValue, hfCodigoSociedad.Value);
                    listaFacturaDetalleDto = gestorSociedad.BuscarCuentasContablesCentroCosto(usuario, hfCodigoSociedad.Value, ddlCentroCosto.SelectedValue);

            }
            finally
            {
                if (gestorSociedad != null) gestorSociedad.Dispose();
            }

            foreach (GridViewRow row in gvDetalleFactura.Rows)
            {
                DropDownList ddlCuentaContable = (DropDownList)row.FindControl("ddlCuentaContable");

                ddlCuentaContable.Items.Clear();
                ddlCuentaContable.Items.Add(new ListItem("::Cuenta contable::", "-1"));
                ddlCuentaContable.AppendDataBoundItems = true;
                ddlCuentaContable.DataSource = listaFacturaDetalleDto;
                ddlCuentaContable.DataTextField = "DESCRIPCION";
                ddlCuentaContable.DataValueField = "IDENTIFICADOR";
                ddlCuentaContable.DataBind();
            }

        }

        private void LlenarColumnaDdlTipoIva(string cajaChicaSAP, string usuario)
        {
            GestorCajaChica gestorCajaChica = null; ;
            List<IndicadoresIVADTO> listaIndicadoresIVADto = null;

            try
            {
                gestorCajaChica = GestorCajaChica();

                listaIndicadoresIVADto = gestorCajaChica.BuscarIndicadoresActivos(cajaChicaSAP, usuario);
            }
            finally
            {
                if (gestorCajaChica != null) gestorCajaChica.Dispose();
            }


            foreach (GridViewRow row in gvDetalleFactura.Rows)
            {
                DropDownList ddlTIva = (DropDownList)row.FindControl("ddlTipoIVA");


                ddlTIva.Items.Clear();
                ddlTIva.Items.Add(new ListItem("::Tipo IVA::", ""));
                ddlTIva.AppendDataBoundItems = true;

                ddlTIva.DataSource = listaIndicadoresIVADto;
                ddlTIva.DataTextField = "Descripcion";
                ddlTIva.DataValueField = "Indicador_IVA";
                ddlTIva.DataBind();
            }
        }

        protected void gvDetalleFactura_RowCommand(object sender, GridViewCommandEventArgs e)
        {

            if (e.CommandName.Equals("Eliminar"))
                EliminarDetalleFactura(e);
        }

        private void EliminarDetalleFactura(GridViewCommandEventArgs e)
        {
            int indice = Int32.Parse(e.CommandArgument.ToString());
            GridViewRow row = gvDetalleFactura.Rows[indice];
            List<FacturaDetalleDTO> listaFacturaDetalle = new List<FacturaDetalleDTO>();

            CopiarDatosGrid(ref listaFacturaDetalle);

            listaFacturaDetalle.RemoveAt(indice);

            CargarDatosGrid(listaFacturaDetalle);
        }

        private string ValidarFilasCompletas(List<FacturaDetalleDTO> _listaFacturaDetalleDto)
        {
            string mensaje = string.Empty;

            if (_listaFacturaDetalleDto.Count != 0)
            {

                if (_listaFacturaDetalleDto.Exists(x => x.CANTIDAD == 0))
                    mensaje = "No se permiten cantidades en 0. <br />";

                if (_listaFacturaDetalleDto.Exists(x => x.DEFINICION_CC == string.Empty))
                    mensaje += "Debe de seleccionar una cuenta contable. <br />";

                if (_listaFacturaDetalleDto.Exists(x => string.IsNullOrEmpty(x.IDENTIFICADOR_IVA.ToString())))
                    mensaje += "Debe de seleccionar el tipo de IVA. <br />";

                //if (_listaFacturaDetalleDto.Exists(x => x.TIPO_IVA != 0))
                //{
                //    if (_listaFacturaDetalleDto.Exists((x => x.TIPO_IVA > 0 && x.IVA == 0)))
                //        mensaje += "El IVA debe ser mayor a 0 <br />";
                //}

                if (_listaFacturaDetalleDto.Exists(x => x.DESCRIPCION == string.Empty))
                    mensaje += "Debe de indicar la descripción del gasto. <br />";

                if (_listaFacturaDetalleDto.Exists(x => x.VALOR == 0))
                    mensaje += "Debe de indicar el valor total.";

                if (chDiferentesCO.Checked == true && txtTotalFactCO.Text == "")
                    mensaje += "Debe completar el valor total de la Factura";
            }
            else
                mensaje += "No hay detalle de la factura. <br />";

            return mensaje;
        }

        private string ValidarDatosPantalla(ref List<FacturaDetalleDTO> _listaFacturaDetalle)
        {
            string mensaje = string.Empty;

            if (ddlCentroCosto.SelectedValue == "-1")
                mensaje += "Debe seleccionar un centro de costo. <br />";

            if (hfIdProveedor.Value.Equals(string.Empty))
                mensaje += "Debe seleccionar un proveedor. <br />";

            if (ddlTipoDocumento.SelectedValue == "-1")
                mensaje += "Debe seleccionar un tipo de documento. <br />";

            if (txtNumeroFactura.Text.Trim().Equals(string.Empty))
                mensaje += "Debe indicar el número de factura. <br />";

            if (!string.IsNullOrEmpty(hfIdProveedor.Value))
                txtNombreProveedor.Text = hfNombreProveedor.Value;

            if (string.IsNullOrEmpty(txtFechaFactura.Text))
                mensaje += "Debe de indicar la fecha de compra. <br />";

            CopiarDatosGrid(ref _listaFacturaDetalle);

            mensaje += ValidarFilasCompletas(_listaFacturaDetalle);

            return mensaje;
        }

        private FacturaEncabezadoDTO CargarDatosEncabezado()
        {
            FacturaEncabezadoDTO _facturaEncabezadoDto = new FacturaEncabezadoDTO();

            _facturaEncabezadoDto.ES_ESPECIAL = cbFacturaEspecial.Checked;
            _facturaEncabezadoDto.FECHA_FACTURA = Convert.ToDateTime(txtFechaFactura.Text);
            _facturaEncabezadoDto.ID_CAJA_CHICA = Convert.ToDecimal(hfIdCajaChica.Value);
            _facturaEncabezadoDto.ID_FACTURA = string.IsNullOrEmpty(hfIdFactura.Value) ? 0 : Convert.ToDecimal(hfIdFactura.Value);
            _facturaEncabezadoDto.ID_PROVEEDOR = Convert.ToInt32(hfIdProveedor.Value);
            _facturaEncabezadoDto.IVA = Convert.ToDouble(hfSumaIva.Value);
            _facturaEncabezadoDto.NUMERO = txtNumeroFactura.Text; //Convert.ToDecimal(txtNumeroFactura.Text);
            _facturaEncabezadoDto.TIPO_FACTURA = TipoOperacionEnum.CC.ToString();

            _facturaEncabezadoDto.SERIE = txtSerie.Text;
            _facturaEncabezadoDto.VALOR_TOTAL = Convert.ToDouble(hfSumaCompra.Value);
            _facturaEncabezadoDto.USUARIO_MANTENIMIENTO.USUARIO_ALTA = usuario;
            _facturaEncabezadoDto.CENTRO_COSTO = ddlCentroCosto.SelectedValue == "-1" ? string.Empty : ddlCentroCosto.SelectedItem.Text;
            _facturaEncabezadoDto.ORDEN_COSTO = ddlOrdenCosto.SelectedValue == "-1" ? string.Empty : ddlOrdenCosto.SelectedItem.Text;
            _facturaEncabezadoDto.RETENCION = chRetencion.Checked;
            //_facturaEncabezadoDto.FACTURA_DIVIDIDA = chDiferentesCO.Checked;
            //_facturaEncabezadoDto.TOTALFACTURADIVIDIDA = Convert.ToDouble(txtTotalFactCO.Text);
            //_facturaEncabezadoDto.OBSERVACIONES = txtObservaciones.Text; 
            _facturaEncabezadoDto.VALOR_REAL_FACT = Convert.ToDouble(txtValReal.Text);
            _facturaEncabezadoDto.TIPO_CAMBIO = string.IsNullOrEmpty(txtTCambio.Text) ? 1 : Convert.ToDouble(txtTCambio.Text);
            _facturaEncabezadoDto.RETSERVICIOS = chImpuestoServ.Checked;

            return _facturaEncabezadoDto;
        }

        private void AlmacenarFactura()
        {
            //string mensaje = string.Empty;
            //GestorCajaChica gestorCajaChica = null;
            //FacturaEncabezadoDTO _facturaEncabezadoDto = null;
            //List<FacturaDetalleDTO> _facturaDetalleDto = new List<FacturaDetalleDTO>();
            //try
            //{
            //    gestorCajaChica = GestorCajaChica();

            //    lblSumaCompra.Text = hfSumaCompra.Value;
            //    lblSumaIVA.Text = hfSumaIva.Value;
            //    txtNombreProveedor.Text = hfNombreProveedor.Value;

            //    mensaje = ValidarDatosPantalla(ref _facturaDetalleDto);

            //    if (!string.IsNullOrEmpty(mensaje))
            //        throw new ExcepcionesDIPCMI(mensaje);

            //    _facturaEncabezadoDto = CargarDatosEncabezado();

            //    _facturaEncabezadoDto.FACTURA_DETALLE.AddRange(_facturaDetalleDto);

            //    gestorCajaChica.AlmacenarFactura(_facturaEncabezadoDto, hfCodigoSociedad.Value);
            //    hfIdFactura.Value = _facturaEncabezadoDto.ID_FACTURA.ToString();

            //    CargarDatosGrid(_facturaEncabezadoDto.FACTURA_DETALLE);

            //    DesplegarAviso("Factura almacenada correctamente.");
            //}
            //catch (ExcepcionesDIPCMI ex)
            //{
            //    DesplegarError(ex.Message);
            //}

        }

        private void CargarDatosFactura()
        {
            GestorCajaChica gestorCajaChica = null;

            try
            {
                gestorCajaChica = GestorCajaChica();
                CargarDatosPantalla(gestorCajaChica.BuscarFactura(Convert.ToDecimal(hfIdFactura.Value)));

            }
            finally
            {
                if (gestorCajaChica != null) gestorCajaChica.Dispose();
            }
        }

        private void CargarDatosPantalla(FacturaEncabezadoDTO facturaEncabezadoDto)
        {
            if (facturaEncabezadoDto.FACTURA_DIVIDIDA == null) facturaEncabezadoDto.FACTURA_DIVIDIDA = false;
            hfIdCajaChica.Value = facturaEncabezadoDto.CAJA_CHICA.ID_CAJA_CHICA.ToString();
            hfIdFactura.Value = facturaEncabezadoDto.ID_FACTURA.ToString();
            hfIdProveedor.Value = facturaEncabezadoDto.ID_PROVEEDOR.ToString();
            hfNombreProveedor.Value = facturaEncabezadoDto.NOMBRE_PROVEEDOR;
            hfSumaCompra.Value = facturaEncabezadoDto.VALOR_TOTAL.ToString("F");
            hfSumaIva.Value = facturaEncabezadoDto.IVA.ToString("F");
            hfCodigoSociedad.Value = facturaEncabezadoDto.CAJA_CHICA.CODIGO_SOCIEDAD;
            hfNombreCC.Value = facturaEncabezadoDto.CAJA_CHICA.NOMBRE_CC.ToString();

            ddlCentroCosto.SelectedValue = facturaEncabezadoDto.CENTRO_COSTO.ToString();
            ddlOrdenCosto.SelectedValue = facturaEncabezadoDto.ORDEN_COSTO.ToString();
            ddlTipoDocumento.SelectedValue = facturaEncabezadoDto.ID_TIPO_DOCUMENTO.ToString();

            txtFechaFactura.Text = facturaEncabezadoDto.FECHA_FACTURA.ToString("yyyy/MM/dd");
            txtNombreProveedor.Text = facturaEncabezadoDto.NOMBRE_PROVEEDOR;
            txtSerie.Text = facturaEncabezadoDto.SERIE;
            txtNumeroFactura.Text = facturaEncabezadoDto.NUMERO.ToString();
            txtNumeroIdentificacion.Text = facturaEncabezadoDto.NUMERO_IDENTIFICACION;


            //txtRetencionISR.Text = facturaEncabezadoDto.RETENCION_ISR ? facturaEncabezadoDto.NUMERO_RETENCION_ISR.ToString() : string.Empty;
            //txtValorISR.Text = facturaEncabezadoDto.RETENCION_ISR ? facturaEncabezadoDto.VALOR_RETENCION_ISR.ToString() : string.Empty;
            //cbISR.Checked = facturaEncabezadoDto.RETENCION_ISR ? facturaEncabezadoDto.RETENCION_ISR : false;

            //txtRetencionIVA.Text = facturaEncabezadoDto.RETENCION_IVA ? facturaEncabezadoDto.NUMERO_RETENCION_IVA.ToString() : string.Empty;
            //txtValorIVA.Text = facturaEncabezadoDto.RETENCION_IVA ? facturaEncabezadoDto.VALOR_RETENCION_IVA.ToString() : string.Empty;
            //cbIVA.Checked = facturaEncabezadoDto.RETENCION_IVA ? facturaEncabezadoDto.RETENCION_IVA : false;

            lblSumaCompra.Text = facturaEncabezadoDto.VALOR_TOTAL.ToString("F");
            lblSumaIVA.Text = facturaEncabezadoDto.IVA.ToString("F");

            cbFacturaEspecial.Checked = facturaEncabezadoDto.ES_ESPECIAL;

            lblInforCC.Text = string.Concat("Sociedad: ", facturaEncabezadoDto.CAJA_CHICA.NOMBRE_EMPRESA, " - Centro: ", facturaEncabezadoDto.CAJA_CHICA.NOMBRE_CENTRO, "- Codigo caja chica: ", facturaEncabezadoDto.CAJA_CHICA.CODIGO_CC, "- Moneda:", facturaEncabezadoDto.CAJA_CHICA.MONEDA);

            lblIdFactura.Text = facturaEncabezadoDto.ID_FACTURA.ToString();

            //Datos de Factura Dividida en diferentes CO 
            txtTotalFactCO.Text = facturaEncabezadoDto.TOTALFACTURADIVIDIDA.ToString();
            chDiferentesCO.Checked = Convert.ToBoolean(facturaEncabezadoDto.FACTURA_DIVIDIDA.ToString());
            hfAcumulado.Value = facturaEncabezadoDto.ACUMULADO.ToString();
            txtObservaciones.Text = facturaEncabezadoDto.OBSERVACIONES.ToString();
            //lblAculumadoSuma.Text = facturaEncabezadoDto.ACUMULADO.ToString();
            txtValReal.Text = facturaEncabezadoDto.VALOR_REAL_FACT.ToString();
            if (facturaEncabezadoDto.CAJA_CHICA.MONEDA == "USD" && facturaEncabezadoDto.PAIS.ToString() == "CR")//(facturaEncabezadoDto.CAJA_CHICA.CODIGO_SOCIEDAD == "1440" || facturaEncabezadoDto.CAJA_CHICA.CODIGO_SOCIEDAD == "1630"))
            {
                lblTipoCambio.Visible = true;
                txtTCambio.Visible = true;
                txtTCambio.Text = facturaEncabezadoDto.TIPO_CAMBIO.ToString();
            }
            chImpuestoServ.Checked = Convert.ToBoolean(facturaEncabezadoDto.RETSERVICIOS.ToString());

            if (facturaEncabezadoDto.FACTURA_DIVIDIDA == true)
                chDiferentesCO.Enabled = false;
            else
                chDiferentesCO.Enabled = true;

            if (facturaEncabezadoDto.TIPO_DOCUMENTO.ToString() == "VALE")
            {
                btnAgregarFila.Enabled = false;
            }
            CargarDatosGrid(facturaEncabezadoDto.FACTURA_DETALLE);
        }

        private void LimpiarDatosPantalla()
        {
            List<FacturaDetalleDTO> facDetalleDto = new List<FacturaDetalleDTO>();

            hfIdFactura.Value = "0";
            hfImpuesto.Value = "0";
            hfIdProveedor.Value = string.Empty;
            hfNombreProveedor.Value = string.Empty;

            txtFechaFactura.Text = string.Empty;
            txtNombreProveedor.Text = string.Empty;
            txtNumeroFactura.Text = string.Empty;
            txtNumeroIdentificacion.Text = string.Empty;
            //txtRetencionISR.Text = string.Empty;
            //txtRetencionIVA.Text = string.Empty;
            txtSerie.Text = string.Empty;
            //txtValorISR.Text = string.Empty;
            //txtValorIVA.Text = string.Empty;

            cbFacturaEspecial.Checked = false;
            //cbISR.Checked = false;
            //cbIVA.Checked = false;

            lblIdFactura.Text = "0";
            txtValReal.Text = string.Empty;

            txtTCambio.Text = string.Empty;
            chImpuestoServ.Checked = false;

            //  TextBox txtDescripcion = (TextBox)row.FindControl("txtDescripcion");
            CargarDatosGrid(facDetalleDto);
        }
        #endregion

        #region Gestores

        private GestorProveedor GestorProveedor()
        {
            GestorProveedor gestorProveedor = new GestorProveedor(cnnApl);
            return gestorProveedor;
        }

        private GestorSociedad GestorSociedad()
        {
            GestorSociedad gestorSociedad = new GestorSociedad(cnnApl);
            return gestorSociedad;
        }

        private GestorCajaChica GestorCajaChica()
        {
            GestorCajaChica gestorCajaChica = new GestorCajaChica(cnnApl);
            return gestorCajaChica;
        }
        #endregion

    }
}