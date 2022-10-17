using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DipCmiGT.LogicaCajasChicas.Sesion;
using System.Configuration;
using DipCmiGT.LogicaCajasChicas;
using DipCmiGT.LogicaComun;
using DipCmiGT.LogicaComun.Util;
using System.Text;

namespace RegistroFacturasWEB.RegistroFacturas
{
    public partial class ListadoCajasChicas : System.Web.UI.Page
    {
        #region Declaraciones

        string cnn = ConfigurationManager.ConnectionStrings["CnnApl"].ToString();
        string usuario;
        string dominio;

        #endregion

        #region Eventos

        protected void Page_Load(object sender, EventArgs e)
        {
            usuario = Context.User.Identity.Name.ToString();
            dominio = (string)Session["dominio"].ToString();
            string codigoSociedad = hfSociedad.Value == "0" ? string.Empty : hfSociedad.Value;
            if (!IsPostBack)
            {
                CargarEstadoDDL();
                CargarSociedadDDL();
                CargarGridViewCajasChicas(Convert.ToInt16(hfCentro.Value), usuario, txtCodigoCC.Text);
            }
        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            string codigoSociedad = hfSociedad.Value == "0" ? "" : hfSociedad.Value;

            OcultarAvisos();
            CargarCentroDDL(usuario, codigoSociedad);
            ddlCentro.SelectedValue = hfCentro.Value;

            CargarGridViewCajasChicas(Convert.ToInt16(hfCentro.Value), usuario, txtCodigoCC.Text);
        }

        protected void gvListadoCajasChicas_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            OcultarAvisos();

            if (e.CommandName.Equals("AgregarFactura"))
                AgregarFactura(e);

            if (e.CommandName.Equals("ListarFacturas"))
                ListarFacturas(e);

            if (e.CommandName.Equals("ImprimirLiquidacion"))
                ImprimirLiquidacion(e);

            if (e.CommandName.Equals("CerrarCajaChica"))
                CerrarCajaChica(e);

            if (e.CommandName.Equals("BajaCajaChica"))
                DarBajaCajaChica(e);
        }

        protected void gvListadoCajasChicas_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            this.gvListadoCajasChicas.PageIndex = e.NewPageIndex;
            CargarGridViewCajasChicas(Convert.ToInt16(hfCentro.Value), usuario, txtCodigoCC.Text);
        }

        #endregion

        #region Metodos

        private void ImprimirLiquidacion(GridViewCommandEventArgs e)
        {
            int indice = Int32.Parse(e.CommandArgument.ToString());
            GridViewRow fila = gvListadoCajasChicas.Rows[indice];

            Int32 idCajaChica = Convert.ToInt32(fila.Cells[2].Text);

            //String b64 = Convert.ToBase64String(Encoding.UTF8.GetBytes(string.Format("idCajaChica={0}", idCajaChica)));
            String b64 = Convert.ToBase64String(Encoding.UTF8.GetBytes(string.Format("idCajaChica={0}&dominio={1}&opcion={2}", idCajaChica, dominio, 1)));
            string url = string.Concat("/Reportes/LiquidacionCC.aspx?", b64);

            string script = @"<script>function myFunction() { var myWindow = window.open('" + url + "', 'myWindow', 'width=950, height=600, resizable=no');myWindow.opener.document.write('<p>This is the source window!</p>');}</script>";
            Response.Write(script);

        }

        private List<CajaChicaEncabezadoDTO> BuscarCajasChicasUsuario(Int16 idCentro, string usuario, Int16 estado, string codigoCC)
        {
            GestorCajaChica gestorCajaChica = null;
            try
            {
                gestorCajaChica = GestorCajaChica();

                //CargarCentroDDL(usuario, hfSociedad.Value);

                ddlCentro.SelectedValue = hfCentro.Value;

                return gestorCajaChica.BuscarCajasChicasUsuario(idCentro, usuario, estado, codigoCC);
            }
            finally
            {
                if (gestorCajaChica != null) gestorCajaChica.Dispose();
            }
        }

        private void AgregarFactura(GridViewCommandEventArgs e)
        {
            int indice = Int32.Parse(e.CommandArgument.ToString());
            GridViewRow fila = gvListadoCajasChicas.Rows[indice];

            Int32 idCajaChica = Convert.ToInt32(fila.Cells[2].Text);
            string codigoSociedad = HttpUtility.HtmlDecode(fila.Cells[4].Text);
            Int16 idCentro = Convert.ToInt16(fila.Cells[6].Text);

            Int32 idCentroSociedad = Convert.ToInt32(fila.Cells[3].Text);
            string cajaChicaSAP = HttpUtility.HtmlDecode(fila.Cells[9].Text);
            string nombreSociedad = HttpUtility.HtmlDecode(fila.Cells[5].Text);
            string nombreCentro = HttpUtility.HtmlDecode(fila.Cells[7].Text);
            string codigoCajaChica = HttpUtility.HtmlDecode(fila.Cells[8].Text);
            string moneda = HttpUtility.HtmlDecode(fila.Cells[14].Text);
            string nombre_CC = HttpUtility.HtmlDecode(fila.Cells[21].Text);
            string pais = HttpUtility.HtmlDecode(fila.Cells[22].Text);

            String b64 = Convert.ToBase64String(Encoding.UTF8.GetBytes(string.Format("idCajaChica={0}&codigoSociedad={1}&idCentro={2}&nombreSociedad={3}&nombreCentro={4}&codigoCajaChica={5}&idCentroSociedad={6}&cajaChicaSAP={7}&moneda={8}&nombreCC={9}&pais={10}", idCajaChica, codigoSociedad, idCentro, nombreSociedad, nombreCentro, codigoCajaChica, idCentroSociedad, cajaChicaSAP, moneda, nombre_CC, pais)));

            string url = string.Concat("~/RegistroFacturas/RegistroFacturas.aspx?", b64);
            Response.Redirect(url);
        }

        private void ListarFacturas(GridViewCommandEventArgs e)
        {
            int indice = Int32.Parse(e.CommandArgument.ToString());
            GridViewRow fila = gvListadoCajasChicas.Rows[indice];

            Int32 idCajaChica = Convert.ToInt32(fila.Cells[2].Text);
            string nombreSociedad = HttpUtility.HtmlDecode(fila.Cells[5].Text);
            string nombreCentro = HttpUtility.HtmlDecode(fila.Cells[7].Text);
            string codigoCajaChica = HttpUtility.HtmlDecode(fila.Cells[8].Text);

            String b64 = Convert.ToBase64String(Encoding.UTF8.GetBytes(string.Format("idCajaChica={0}&nombreSociedad={1}&nombreCentro={2}&codigoCajaChica={3}", idCajaChica, nombreSociedad, nombreCentro, codigoCajaChica)));
            string url = string.Concat("~/RegistroFacturas/ListadoFacturas.aspx?", b64);
            Response.Redirect(url);
        }

        private void DarBajaCajaChica(GridViewCommandEventArgs e)
        {
            int indice = Int32.Parse(e.CommandArgument.ToString());
            GridViewRow fila = gvListadoCajasChicas.Rows[indice];
            GestorCajaChica gestorCajaChica = null;

            decimal idCajaChica = Convert.ToDecimal(fila.Cells[2].Text);
            string codigoSociedad = hfSociedad.Value == "0" ? "" : hfSociedad.Value;

            try
            {
                gestorCajaChica = GestorCajaChica();

                gestorCajaChica.DarBajaCajaChica(idCajaChica, usuario);
                CargarGridViewCajasChicas(Convert.ToInt16(hfCentro.Value), usuario, txtCodigoCC.Text);
                DesplegarAviso("Se dio baja a la caja chica.");

            }
            catch (ExcepcionesDIPCMI ex)
            {
                DesplegarError(ex.Message);
            }
            finally
            {
                if (gestorCajaChica != null) gestorCajaChica.Dispose();
            }

        }

        private void CerrarCajaChica(GridViewCommandEventArgs e)
        {
            int indice = Int32.Parse(e.CommandArgument.ToString());
            GridViewRow fila = gvListadoCajasChicas.Rows[indice];
            GestorCajaChica gestorCajaChica = null;
            string codigoSociedad = hfSociedad.Value == "0" ? "" : hfSociedad.Value;
            decimal idCajaChica = Convert.ToDecimal(fila.Cells[2].Text);

            try
            {
                gestorCajaChica = GestorCajaChica();

                gestorCajaChica.CerrarCajaChica(idCajaChica, usuario);
                CargarGridViewCajasChicas(Convert.ToInt16(hfCentro.Value), usuario, txtCodigoCC.Text);
                DesplegarAviso("Se cerro la caja chica.");
            }
            catch (ExcepcionesDIPCMI ex)
            {
                DesplegarError(ex.Message);
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

        //public void CargarCentroDDL(string codigoSociedad)
        //{
        //    List<LlenarDDL_DTO> listaDDLDto = new List<LlenarDDL_DTO>();
        //    GestorSociedad gestorSociedad = null;

        //    try
        //    {
        //        gestorSociedad = GestorSociedad();

        //        //listaDDLDto = gestorSociedad.ListarCentrosMapeados(usuario, codigoSociedad);

        //        ddlCentro.DataSource = listaDDLDto;
        //        ddlCentro.DataTextField = "DESCRIPCION";
        //        ddlCentro.DataValueField = "IDENTIFICADOR";
        //        ddlCentro.DataBind();
        //        ddlCentro.Items.Insert(0, new ListItem("--Seleccione centro--", "0"));
        //    }
        //    finally
        //    {

        //        if (gestorSociedad != null) gestorSociedad.Dispose();
        //    }
        //}

        public void CargarEstadoDDL()
        {
            List<LlenarDDL_DTO> listaDDLDto = new List<LlenarDDL_DTO>();

            listaDDLDto.Add(new LlenarDDL_DTO() { IDENTIFICADOR = "1", DESCRIPCION = "BUSCAR VIGENTES" });
            listaDDLDto.Add(new LlenarDDL_DTO() { IDENTIFICADOR = "2", DESCRIPCION = "BUSCAR CERRADAS" });
            listaDDLDto.Add(new LlenarDDL_DTO() { IDENTIFICADOR = "0", DESCRIPCION = "BUSCAR ANULADAS" });
            listaDDLDto.Add(new LlenarDDL_DTO() { IDENTIFICADOR = "-1", DESCRIPCION = "BUSCAR TODO" });

            ddlEstado.DataSource = listaDDLDto;
            ddlEstado.DataTextField = "DESCRIPCION";
            ddlEstado.DataValueField = "IDENTIFICADOR";
            ddlEstado.DataBind();

        }

        private void CargarSociedadDDL()
        {
            List<LlenarDDL_DTO> listaDDLDto = new List<LlenarDDL_DTO>();
            GestorSociedad gestorSociedad = null;

            try
            {
                gestorSociedad = GestorSociedad();
                listaDDLDto = gestorSociedad.ListarSociedadesUsuario(usuario,dominio);

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

        private void CargarCentroDDL(string usuario, string codigoSociedad)
        {
            List<LlenarDDL_DTO> listaDDLDto = new List<LlenarDDL_DTO>();
            GestorSociedad gestorSociedad = null;

            try
            {
                gestorSociedad = GestorSociedad();
                listaDDLDto = gestorSociedad.ListarCentrosUsuario(usuario, codigoSociedad);

                ddlCentro.DataSource = listaDDLDto;
                ddlCentro.DataTextField = "DESCRIPCION";
                ddlCentro.DataValueField = "IDENTIFICADOR";
                ddlCentro.DataBind();
                ddlCentro.Items.Insert(0, new ListItem("::Seleccione centro::", "0"));

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

        private void CargarGridViewCajasChicas(Int16 idCentro, string usuario, string codigoCC)
        {
            List<CajaChicaEncabezadoDTO> _listaCajaChicaDto = null;

            Int32 numero = 1;

            _listaCajaChicaDto = BuscarCajasChicasUsuario(Convert.ToInt16(hfCentro.Value), usuario, Convert.ToInt16(ddlEstado.SelectedValue), codigoCC);

            gvListadoCajasChicas.DataSource = (from cajaChica in _listaCajaChicaDto
                                               select new
                                               {
                                                   NUMERO = numero++,
                                                   EstadoCC = cajaChica.ESTADO,
                                                   ID_CAJA_CHICA = cajaChica.ID_CAJA_CHICA,
                                                   ID_SOCIEDAD_CENTRO = cajaChica.ID_SOCIEDAD_CENTRO,
                                                   CODIGO_SOCIEDAD = cajaChica.CODIGO_SOCIEDAD,
                                                   NOMBRE_SOCIEDAD = cajaChica.NOMBRE_EMPRESA,
                                                   ID_CENTRO = cajaChica.CODIGO_CENTRO,
                                                   NOMBRE_CENTRO = cajaChica.NOMBRE_CENTRO,
                                                   CODIGO_CAJA_CHICA = cajaChica.CODIGO_CC,
                                                   NUMERO_CAJA_CHICA = cajaChica.CAJA_CHICA_SAP,
                                                   ESTADO = cajaChica.ESTADO,
                                                   DESCRIPCION = cajaChica.DESCRIPCION,
                                                   FACTURAS_CC = cajaChica.FACTURAS_CC,
                                                   VALOR_CC = cajaChica.MONTO_CC.ToString("F"),
                                                   OPERACION = cajaChica.TIPO_OPERACION,
                                                   MONEDA = cajaChica.MONEDA,
                                                   NOMBRE_CC = cajaChica.NOMBRE_CC.ToString(),
                                                   PAIS = cajaChica.PAIS.ToString()

                                               }).ToArray();
            gvListadoCajasChicas.DataBind();
        }
        #endregion

        #region Gestores

        private GestorCajaChica GestorCajaChica()
        {
            GestorCajaChica gestorCajaChica = new GestorCajaChica(cnn);
            return gestorCajaChica;
        }

        private GestorSociedad GestorSociedad()
        {
            GestorSociedad gestorSociedad = new GestorSociedad(cnn);
            return gestorSociedad;
        }
        #endregion


    }
}