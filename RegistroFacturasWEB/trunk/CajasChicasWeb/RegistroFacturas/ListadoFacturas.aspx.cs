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


namespace RegistroFacturasWEB.RegistroFacturas
{
    public partial class ListadoFacturas : System.Web.UI.Page
    {
        #region Declaraciones

        string cnn = ConfigurationManager.ConnectionStrings["CnnApl"].ToString();
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
                if (!strUrl.Contains('?'))
                    Response.Redirect("~/principal.aspx");

                if (!string.IsNullOrEmpty(strParam))
                {
                    CapturarParametros(strParam);
                    CargarGrid(Convert.ToDecimal(hfIdCajaChica.Value));
                }
                else
                    Response.Redirect("~/principal.aspx");
            }
        }

        protected void gvListaFacturas_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            OcultarAvisos();

            if (e.CommandName.Equals("ModificarFactura"))
                ModificarFactura(e);

            if (e.CommandName.Equals("BajaFactura"))
                DarBajaFactura(e);

            if (e.CommandName.Equals("AltaFactura"))
                DarAltaFactura(e);
        }

        protected void gvListaFacturas_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            this.gvListaFacturas.PageIndex = e.NewPageIndex;
            CargarGrid(Convert.ToDecimal(hfIdCajaChica.Value));
        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            OcultarAvisos();
            CargarGrid(Convert.ToDecimal(hfIdCajaChica.Value));
        }

        #endregion

        #region Metodos

        private void CapturarParametros(string parametros)
        {
            string strParametros = Encoding.UTF8.GetString(Convert.FromBase64String(parametros));
            string[] argsParam = strParametros.Split('&');
            string[] param;
            string nombreSociedad = string.Empty;
            string nombreCentro = string.Empty;
            string codigoCajaChica = string.Empty;

            param = argsParam[0].Split('=');
            hfIdCajaChica.Value = param[1];

            param = argsParam[1].Split('=');
            nombreSociedad = param[1];

            param = argsParam[2].Split('=');
            nombreCentro = param[1];

            param = argsParam[3].Split('=');
            codigoCajaChica = param[1];

            lblInforCC.Text = string.Concat("Sociedad: ", nombreSociedad, " - Centro: ", nombreCentro, " - Codigo caja chica: ", codigoCajaChica);

        }

        private List<FacturaEncabezadoDTO> BuscarFacturas(decimal idCajaChica)
        {
            GestorCajaChica gestorCajaChica = null;
            string identificacion = string.IsNullOrEmpty(txtNumeroIdentificacion.Text) ? (string)string.Empty : txtNumeroIdentificacion.Text;
            string serie = string.IsNullOrEmpty(txtSerie.Text) ? (string)string.Empty : txtSerie.Text;
            //decimal numero = string.IsNullOrEmpty(txtNumero.Text) ? -1 : Convert.ToDecimal(txtNumero.Text);
            string numero = string.IsNullOrEmpty(txtNumero.Text) ? "-1" : txtNumero.Text;

            try
            {
                gestorCajaChica = GestorCajaChica();

                return gestorCajaChica.BuscarFacturasCajaChica(idCajaChica, -1, identificacion, serie, numero);
            }
            finally
            {
                if (gestorCajaChica != null) gestorCajaChica.Dispose();
            }
        }

        private void CargarGrid(decimal idCajaChica)
        {
            List<FacturaEncabezadoDTO> _listaEncabezadoDto = null;
            Int32 numero = 1;

            _listaEncabezadoDto = BuscarFacturas(idCajaChica);

            gvListaFacturas.DataSource = (from factura in _listaEncabezadoDto
                                          select new
                                          {
                                              EstadoFac = factura.ESTADO,
                                              NUMERO = numero++,
                                              ID_FACTURA = factura.ID_FACTURA,
                                              DOCUMENTO_IDENTIFICACION = factura.TIPO_DOCUMENTO,
                                              NUMERO_IDENTIFICACION = factura.NUMERO_IDENTIFICACION,
                                              NOMBRE_PROVEEDOR = factura.NOMBRE_PROVEEDOR,
                                              SERIE = factura.SERIE,
                                              NUMERO_DOCUMENTO = factura.NUMERO,
                                              FECHA_FACTURA = factura.FECHA_FACTURA.ToShortDateString(),
                                              ESPECIAL = factura.ES_ESPECIAL == true ? "SI" : "NO",
                                              IVA = factura.IVA.ToString("F"),
                                              TOTAL = factura.VALOR_TOTAL.ToString("F"),
                                              ESTADO = factura.ESTADO,
                                              //APROBACION = string.IsNullOrEmpty(factura.APROBADA.ToString()) ? false : factura.APROBADA,
                                              APROBACION = string.IsNullOrEmpty(factura.APROBADA.ToString()) ? (bool?)null : factura.APROBADA,
                                              APROBADA = string.IsNullOrEmpty(factura.APROBADA.ToString()) ? string.Empty : factura.APROBADA == true ? "APROBADA" : "RECHAZADA",
                                              ESTADO_CC = factura.CAJA_CHICA.ESTADO,
                                              estadoCC = factura.CAJA_CHICA.ESTADO,
                                              CAJA_CHICA_SAP = factura.CAJA_CHICA.CAJA_CHICA_SAP,
                                              ID_SOCIEDAD_CENTRO = factura.CAJA_CHICA.ID_SOCIEDAD_CENTRO,
                                              MONEDA = factura.MONEDA, 
                                              SOCIEDAD = factura.CODIGO_SOCIEDAD,
                                              PAIS = factura.PAIS,
                                              EstadoINELDAT = string.IsNullOrEmpty(factura.EstadoINELDAT) ? "N/A" : factura.EstadoINELDAT

                                          }).ToArray();
            gvListaFacturas.DataBind();


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

        private void ModificarFactura(GridViewCommandEventArgs e)
        {
            int indice = Int32.Parse(e.CommandArgument.ToString());
            GridViewRow fila = gvListaFacturas.Rows[indice];

            Int16 estado = Convert.ToInt16(fila.Cells[13].Text);
            bool? aprobada = string.IsNullOrEmpty(HttpUtility.HtmlDecode(fila.Cells[14].Text).Trim()) ? (bool?)null : Convert.ToBoolean(fila.Cells[14].Text);
            decimal idFactura = Convert.ToDecimal(fila.Cells[2].Text);
            string cajaChicaSAP = HttpUtility.HtmlDecode(fila.Cells[17].Text);
            string idSociedadCentro = HttpUtility.HtmlDecode(fila.Cells[18].Text);
            string CodigoSocieedad = HttpUtility.HtmlDecode(fila.Cells[19].Text);
            string pais = HttpUtility.HtmlDecode(fila.Cells[24].Text);
            //decimal monto = Convert.ToDecimal(fila.Cells[12].Text);
            try
            {

                if (aprobada != null)
                {
                    if ((bool)aprobada)
                        throw new ExcepcionesDIPCMI("No se pueden modificar facturas aprobadas.");

                    if (estado != 1)
                        throw new ExcepcionesDIPCMI("La factura no esta disponible para edición.");
                }

                String b64 = Convert.ToBase64String(Encoding.UTF8.GetBytes(string.Format(string.Format("idFactura={0}&cajaChicaSAP={1}&idSociedadCentro={2}&CodigoSociedad={3}&Pais={4}", idFactura, cajaChicaSAP, idSociedadCentro, CodigoSocieedad, pais))));
                string url = string.Concat("~/RegistroFacturas/RegistroFacturas.aspx?", b64);
                Response.Redirect(url);
            }
            catch (ExcepcionesDIPCMI ex)
            {
                DesplegarError(ex.Message);
            }
        }

        private void DarBajaFactura(GridViewCommandEventArgs e)
        {
            int indice = Int32.Parse(e.CommandArgument.ToString());
            GridViewRow fila = gvListaFacturas.Rows[indice];
            GestorCajaChica gestorCajaChica = null;

            Int16 estado = Convert.ToInt16(fila.Cells[13].Text);
            bool? aprobada = string.IsNullOrEmpty(HttpUtility.HtmlDecode(fila.Cells[14].Text).Trim()) ? (bool?)null : Convert.ToBoolean(HttpUtility.HtmlDecode(fila.Cells[14].Text));
            decimal idFactura = Convert.ToDecimal(fila.Cells[2].Text);

            try
            {
                gestorCajaChica = GestorCajaChica();

                //if ((bool)aprobada)
                //    throw new ExcepcionesDIPCMI("No se pueden modificar facturas aprobadas.");

                if (estado != 1)
                    throw new ExcepcionesDIPCMI("La factura no esta disponible para edición.");

                gestorCajaChica.AnularFactura(idFactura, usuario);

                CargarGrid(Convert.ToDecimal(hfIdCajaChica.Value));

                DesplegarAviso("La factura se anulo correcamente.");
            }
            catch (ExcepcionesDIPCMI ex)
            {
                DesplegarError(ex.Message);
            }
        }

        private void DarAltaFactura(GridViewCommandEventArgs e)
        {
            int indice = Int32.Parse(e.CommandArgument.ToString());
            GridViewRow fila = gvListaFacturas.Rows[indice];
            GestorCajaChica gestorCajaChica = null;

            Int16 estado = Convert.ToInt16(fila.Cells[13].Text);
            bool? aprobada = string.IsNullOrEmpty(HttpUtility.HtmlDecode(fila.Cells[14].Text).Trim()) ? (bool?)null : Convert.ToBoolean(fila.Cells[14].Text);
            decimal idFactura = Convert.ToDecimal(fila.Cells[2].Text);
            Int16 estadoCC = Convert.ToInt16(fila.Cells[16].Text);

            try
            {
                gestorCajaChica = GestorCajaChica();

                if (estado > 1)
                    throw new ExcepcionesDIPCMI("La factura no esta disponible para edición.");


                if (estadoCC != 1)
                    throw new ExcepcionesDIPCMI("La caja chica no esta vigente.");

                gestorCajaChica.DarVigenciaFactura(idFactura, usuario);

                CargarGrid(Convert.ToDecimal(hfIdCajaChica.Value));
                DesplegarAviso("Se dio de alta la factura.");
            }
            catch (ExcepcionesDIPCMI ex)
            {
                DesplegarError(ex.Message);
            }
        }

        #endregion

        #region Gestores

        protected GestorCajaChica GestorCajaChica()
        {
            GestorCajaChica gestorCajaChica = new GestorCajaChica(cnn);
            return gestorCajaChica;
        }

        #endregion


    }
}