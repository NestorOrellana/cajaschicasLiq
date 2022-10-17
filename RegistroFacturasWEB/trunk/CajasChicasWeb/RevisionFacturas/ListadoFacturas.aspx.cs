using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
//using LogicaCajasChicas.Sesion;
using System.Configuration;
using DipCmiGT.LogicaCajasChicas;
using DipCmiGT.LogicaComun;
using DipCmiGT.LogicaCajasChicas.Sesion;
using DipCmiGT.LogicaComun.Util;
using System.Text;


namespace RegistroFacturasWEB.RevisionFacturas
{
    public partial class ListadoFacturas : System.Web.UI.Page
    {
        #region Declaraciones

        string cnn = ConfigurationManager.ConnectionStrings["CnnApl"].ToString();
        string cnnInterfaces = ConfigurationManager.ConnectionStrings["CnnInterfaces"].ToString();
        string cnnInterfacesMM = ConfigurationManager.ConnectionStrings["CnnInterfacesMM"].ToString();

        string mandante;
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
            if (e.CommandName.Equals("AceptarFactura"))
                AceptarFactura(e);

            if (e.CommandName.Equals("RechazarFactura"))
                RechazarFactura(e);
        }

        protected void gvListaFacturas_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            this.gvListaFacturas.PageIndex = e.NewPageIndex;
            CargarGrid(Convert.ToDecimal(hfIdCajaChica.Value));
        }

        protected void btnGrabar_Click(object sender, EventArgs e)
        {
            RevisionMasiva();
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
            hfCodigoSociedad.Value = param[1];

            param = argsParam[2].Split('=');
            nombreSociedad = param[1];

            param = argsParam[3].Split('=');
            nombreCentro = param[1];

            param = argsParam[4].Split('=');
            codigoCajaChica = param[1];

            lblInforCC.Text = string.Concat("Sociedad: ", nombreSociedad, " - Centro: ", nombreCentro, " - Codigo caja chica: ", codigoCajaChica);
        }

        private List<FacturaEncabezadoDTO> BuscarFacturas(decimal idCajaChica, Int16 estadoCajaChica, Int16 estadoFactura)
        {
            GestorCajaChica gestorCajaChica = null;
            string identificacion = string.IsNullOrEmpty(txtNumeroIdentificacion.Text) ? (string)string.Empty : txtNumeroIdentificacion.Text;
            string serie = string.IsNullOrEmpty(txtSerie.Text) ? (string)string.Empty : txtSerie.Text;
            //decimal numero = string.IsNullOrEmpty(txtNumero.Text) ? -1 : Convert.ToDecimal(txtNumero.Text);
            string numero = string.IsNullOrEmpty(txtNumero.Text) ? "-1" : txtNumero.Text;

            try
            {
                gestorCajaChica = GestorCajaChica();

                return gestorCajaChica.BuscarFacturasCajaChica(idCajaChica, estadoCajaChica, estadoFactura, identificacion, serie, numero);
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
            const Int16 estadoCajaChica = 2;
            const Int16 estadoFactura = 1;


            _listaEncabezadoDto = BuscarFacturas(idCajaChica, estadoCajaChica, estadoFactura);

            gvListaFacturas.DataSource = (from factura in _listaEncabezadoDto
                                          select new
                                          {
                                              idAprobada = false,
                                              idRechazada = false,
                                              FACTURA_DIVIDIDA = factura.FACTURA_DIVIDIDA,
                                              NUMERO = numero++,
                                              ID_FACTURA = factura.ID_FACTURA,
                                              TIPO_DOCUMENTO = factura.TIPO_DOCUMENTO,
                                              NIT = factura.NUMERO_IDENTIFICACION,
                                              NOMBRE_PROVEEDOR = factura.NOMBRE_PROVEEDOR,
                                              SERIE = factura.SERIE,
                                              NUMERO_DOCUMENTO = factura.NUMERO,
                                              FECHA_FACTURA = factura.FECHA_FACTURA.ToShortDateString(),
                                              ESPECIAL = factura.ES_ESPECIAL == true ? "SI" : "NO",
                                              IVA = factura.IVA.ToString("F"),
                                              TOTAL = factura.VALOR_TOTAL.ToString("F"),
                                              VIGENTE = factura.ESTADO,
                                              MONEDA = factura.MONEDA,
                                              IMPUESTO = factura.IMPUESTO, 
                                              MANDANTE = factura.MANDANTE,
                                              INDICADOR = factura.INDICADOR
                                          }).ToArray();
            gvListaFacturas.DataBind();

        }

        //private void CargarGridFacturasRevision(decimal idCajaChica, string proveedor, string serie, decimal numero)
        private void CargarGridFacturasRevision(decimal idCajaChica, string proveedor, string serie, string numero)
        {
            List<FacturaEncabezadoDTO> _listaEncabezadoDto = null;
            GestorCajaChica gestorCajaChica = null;
            //int indice = Int32.Parse(e.CommandArgument.ToString());
            //GridViewRow fila = gvListaFacturas.Rows[indice];
            //decimal idFactura = Convert.ToDecimal(fila.Cells[3].Text);
            //decimal idcajachica = Convert.ToDecimal(hfIdCajaChica.Value);
            //string proveedor = (fila.Cells[5].Text);
            //string serie = (fila.Cells[7].Text);
            //decimal numero = Convert.ToDecimal(fila.Cells[8].Text);

            _listaEncabezadoDto = BuscarTodasFacturasDivididas(idCajaChica, proveedor, serie, numero);

            foreach (FacturaEncabezadoDTO factEnc in _listaEncabezadoDto)
            {
                gestorCajaChica = GestorIntermediaCC();
                gestorCajaChica.AceptarFactura(factEnc.ID_FACTURA, usuario, hfCodigoSociedad.Value);
            }

            //gvListaFacturas.DataSource = (from factura in _listaEncabezadoDto
            //                              select new
            //                              {
            //                                  idAprobada = false,
            //                                  idRechazada = false,
            //                              }).ToArray();
            //gvListaFacturas.DataBind();

        }

        ///private void CargarGridFacturasRevisionRezacho(decimal idCajaChica, string proveedor, string serie, decimal numero)
        private void CargarGridFacturasRevisionRezacho(decimal idCajaChica, string proveedor, string serie, string numero)
        {
            List<FacturaEncabezadoDTO> _listaEncabezadoDto = null;
            GestorCajaChica gestorCajaChica = null;

            _listaEncabezadoDto = BuscarTodasFacturasDivididas(idCajaChica, proveedor, serie, numero);

            foreach (FacturaEncabezadoDTO factEnc in _listaEncabezadoDto)
            {
                gestorCajaChica = GestorIntermediaCC();
                gestorCajaChica.RechazarFactura(factEnc.ID_FACTURA, usuario);
            }

        }

        private List<FacturaEncabezadoDTO> BuscarTodasFacturasDivididas(decimal idCajaChica, string proveedor, string serie, string numero)
        {
            GestorCajaChica gestorCajaChica = null;

            try
            {
                gestorCajaChica = GestorCajaChica();

                return gestorCajaChica.BuscarTodasFacturasDivididas(idCajaChica, proveedor, serie, numero);
            }
            finally
            {
                if (gestorCajaChica != null) gestorCajaChica.Dispose();
            }
        }
        private void AceptarFactura(GridViewCommandEventArgs e)
        {
            int indice = Int32.Parse(e.CommandArgument.ToString());
            GridViewRow fila = gvListaFacturas.Rows[indice];
            GestorCajaChica gestorCajaChica = null;

            string serie = "";
            decimal idFactura = Convert.ToDecimal(fila.Cells[3].Text);
            decimal idcajachica = Convert.ToDecimal(hfIdCajaChica.Value);
            string proveedor = HttpUtility.HtmlDecode(fila.Cells[5].Text).Trim(); // (fila.Cells[5].Text);
            serie = HttpUtility.HtmlDecode(fila.Cells[7].Text).Trim();
            if (fila.Cells[7].Text == "&nbsp;")
                serie = "";
            else
            serie = (fila.Cells[7].Text);
            //decimal numero = Convert.ToDecimal(fila.Cells[8].Text);
            string numero = fila.Cells[8].Text;
            mandante = fila.Cells[21].Text;

            OcultarAvisos();
            try
            {
                
                gestorCajaChica = GestorIntermediaCC();

                CargarGridFacturasRevision(idcajachica, proveedor, serie, numero);
               // /////gestorCajaChica.BuscarTodasFacturasDivididas(idcajachica, proveedor, serie, numero);
                
                //gestorCajaChica.AceptarFactura(idFactura, usuario, hfCodigoSociedad.Value);

                CargarGrid(Convert.ToDecimal(hfIdCajaChica.Value));

                DesplegarAviso("El registro fue aceptado exitosamente.");
            }
            catch(Exception ex)
            {
                DesplegarError("Error al aceptar el registro." + ex.Message);
            }
            finally
            {
                if (gestorCajaChica != null) gestorCajaChica.Dispose();
            }
        }

        private void RechazarFactura(GridViewCommandEventArgs e)
        {
            OcultarAvisos();
            int indice = Int32.Parse(e.CommandArgument.ToString());
            GridViewRow fila = gvListaFacturas.Rows[indice];
            GestorCajaChica gestorCajaChica = null;

            decimal idFactura = Convert.ToDecimal(fila.Cells[3].Text);
            decimal idcajachica = Convert.ToDecimal(hfIdCajaChica.Value);
            string proveedor = HttpUtility.HtmlDecode(fila.Cells[5].Text).Trim();
            string serie = HttpUtility.HtmlDecode(fila.Cells[7].Text).Trim();
            //decimal numero = Convert.ToDecimal(fila.Cells[8].Text);
            string numero = fila.Cells[8].Text;

            OcultarAvisos();
            try
            {
                gestorCajaChica = GestorCajaChica();

                CargarGridFacturasRevisionRezacho(idcajachica, proveedor, serie, numero);
                //gestorCajaChica.RechazarFactura(idFactura, usuario);
                CargarGrid(Convert.ToDecimal(hfIdCajaChica.Value));
                DesplegarAviso("El registro fue rechazado correctamente.");
            }
            catch
            {
                DesplegarError("El registro no puedo ser rechazado.");
            }
            finally
            {
                if (gestorCajaChica != null) gestorCajaChica.Dispose();
            }
        }

        private void RevisionMasiva()
        {
            GestorCajaChica gestorCajaChica = null;
            int aprob = 0;
            int rechaz = 0;
            try
            {
                OcultarAvisos();
                foreach (GridViewRow row in gvListaFacturas.Rows)
                {

                    decimal idcajachica = Convert.ToDecimal(hfIdCajaChica.Value);
                    string proveedor = HttpUtility.HtmlDecode(row.Cells[5].Text).Trim();
                    string serie = HttpUtility.HtmlDecode(row.Cells[7].Text).Trim();
                    //decimal numero = Convert.ToDecimal(row.Cells[8].Text);
                    string numero = row.Cells[8].Text;
                    mandante = row.Cells[21].Text;

                    aprob = Convert.ToInt32(((CheckBox)row.FindControl("idAprobada")).Checked);
                    rechaz = Convert.ToInt32(((CheckBox)row.FindControl("idRechazada")).Checked);
                    decimal idFactura = Convert.ToDecimal(row.Cells[3].Text);
                    if (aprob == 1)
                    {
                        gestorCajaChica = GestorIntermediaCC();
                        CargarGridFacturasRevision(idcajachica, proveedor, serie, numero);
                        //gestorCajaChica.AceptarFactura(idFactura, usuario, hfCodigoSociedad.Value);

                    }
                    else if (rechaz == 1)
                    {
                        gestorCajaChica = GestorCajaChica();
                        CargarGridFacturasRevisionRezacho(idcajachica, proveedor, serie, numero);
                        //gestorCajaChica.RechazarFactura(idFactura, usuario);
                    }
                    DesplegarAviso("Facturas Revisadas Correctamente.");
                }
                CargarGrid(Convert.ToDecimal(hfIdCajaChica.Value));
            }
            catch
            {
                DesplegarError("Error en la Revisión de Facturas");
            }
            finally
            {
                if (gestorCajaChica != null) gestorCajaChica.Dispose();
            }
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

        public string EncriptarQueryString(string strQueryString)
        {
            return Criptografo.Encrypt(strQueryString, "!#$%&/()");
        }

        #endregion

        #region Gestores

        protected GestorCajaChica GestorCajaChica()
        {
            GestorCajaChica gestorCajaChica = new GestorCajaChica(cnn);
            return gestorCajaChica;
        }

        protected GestorCajaChica GestorIntermediaCC()
        {
            string con;
            //if (mandante == "IP") con = cnnInterfaces; else con = cnnInterfacesMM;
            if (mandante == "MM") con = cnnInterfacesMM; else con = cnnInterfaces;
            string[] conexiones = new string[] { cnn, con };
            //string[] conexiones = new string[] { cnn, cnnInterfaces };

            GestorCajaChica gestorCajaChica = new GestorCajaChica(conexiones);
            return gestorCajaChica;
        }

        #endregion

    }
}