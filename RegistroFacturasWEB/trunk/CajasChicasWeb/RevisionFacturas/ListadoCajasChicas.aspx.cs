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

namespace RegistroFacturasWEB.RevisionFacturas
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
                CargarGridViewCajasChicas(codigoSociedad, Convert.ToInt16(hfCentro.Value), usuario, 0, 0);
            }
        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            string[] numeroCC = null;
            string codigoSociedad = hfSociedad.Value == "0" ? "0" : hfSociedad.Value;

            CargarSociedadCentroDDL(usuario, codigoSociedad);
            ddlCentro.SelectedValue = hfCentro.Value;

            if (!string.IsNullOrEmpty(txtCogidoCC.Text.Trim()))
                numeroCC = txtCogidoCC.Text.Split('-');

            Int32 numeroCajaChica = string.IsNullOrEmpty(txtCogidoCC.Text) ? 0 : Convert.ToInt32(numeroCC[2]);
            Int32 correlativo = string.IsNullOrEmpty(txtCogidoCC.Text) ? 0 : Convert.ToInt32(numeroCC[3]);

            CargarGridViewCajasChicas(codigoSociedad, Convert.ToInt16(hfCentro.Value), usuario, numeroCajaChica, correlativo);
        }

        protected void gvListadoCajasChicas_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName.Equals("ListarFacturas"))
                ListarFacturas(e);

            if (e.CommandName.Equals("ImprimirLiquidacion"))
                ImprimirLiquidacion(e);
        }

        protected void gvListadoCajasChicas_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            string[] numeroCC = null;
            string codigoSociedad = hfSociedad.Value == "0" ? string.Empty : hfSociedad.Value;
            this.gvListadoCajasChicas.PageIndex = e.NewPageIndex;

            if (!string.IsNullOrEmpty(txtCogidoCC.Text.Trim()))
                numeroCC = txtCogidoCC.Text.Split('-');

            Int32 numeroCajaChica = string.IsNullOrEmpty(txtCogidoCC.Text) ? 0 : Convert.ToInt32(numeroCC[0]);
            Int32 correlativo = string.IsNullOrEmpty(txtCogidoCC.Text) ? 0 : Convert.ToInt32(numeroCC[1]);

            CargarGridViewCajasChicas(codigoSociedad, Convert.ToInt16(hfCentro.Value), usuario, numeroCajaChica, correlativo);
        }

        #endregion

        #region Metodos

        public void CargarEstadoDDL()
        {
            List<LlenarDDL_DTO> listaDDLDto = new List<LlenarDDL_DTO>();

            listaDDLDto.Add(new LlenarDDL_DTO() { IDENTIFICADOR = "1", DESCRIPCION = "NO REVISADAS" });
            listaDDLDto.Add(new LlenarDDL_DTO() { IDENTIFICADOR = "2", DESCRIPCION = "REVISADAS" });

            ddlEstado.DataSource = listaDDLDto;
            ddlEstado.DataTextField = "DESCRIPCION";
            ddlEstado.DataValueField = "IDENTIFICADOR";
            ddlEstado.DataBind();

        }

        private void ImprimirLiquidacion(GridViewCommandEventArgs e)
        {
            int indice = Int32.Parse(e.CommandArgument.ToString());
            GridViewRow fila = gvListadoCajasChicas.Rows[indice];

            Int32 idCajaChica = Convert.ToInt32(fila.Cells[1].Text);

            String b64 = Convert.ToBase64String(Encoding.UTF8.GetBytes(string.Format("idCajaChica={0}&dominio={1}&opcion={2}", idCajaChica, dominio,0)));
            string url = string.Concat("../Reportes/LiquidacionCC.aspx?", b64);

            string script = @"<script>function myFunction() { var myWindow = window.open('" + url + "', 'myWindow', 'width=950, height=600, resizable=no');myWindow.opener.document.write('<p>This is the source window!</p>');}</script>";
            Response.Write(script);

        }

        private List<CajaChicaEncabezadoDTO> BuscarCajasChicasUsuario(string codigoSociedad, Int16 idCentro, Int32 numeroCajachica, Int32 correlativo)
        {
            GestorCajaChica gestorCajaChica = null;
            List<CajaChicaEncabezadoDTO> listaCajaChicaDto = null;

            try
            {
                gestorCajaChica = GestorCajaChica();

                if (ddlEstado.SelectedValue == "1")
                    listaCajaChicaDto = gestorCajaChica.BuscarCajasRevision(usuario, Convert.ToInt32(hfCentro.Value), Convert.ToInt32(ddlSociedad.SelectedValue), numeroCajachica, correlativo);

                if (ddlEstado.SelectedValue == "2")
                    listaCajaChicaDto = gestorCajaChica.BuscarCajasChicasRevisadas(usuario, Convert.ToInt32(hfCentro.Value), Convert.ToInt32(ddlSociedad.SelectedValue), numeroCajachica, correlativo);
            }
            finally
            {
                if (gestorCajaChica != null) gestorCajaChica.Dispose();
            }
            return listaCajaChicaDto;
        }

        private void ListarFacturas(GridViewCommandEventArgs e)
        {
            int indice = Int32.Parse(e.CommandArgument.ToString());
            GridViewRow fila = gvListadoCajasChicas.Rows[indice];

            Int32 idCajaChica = Convert.ToInt32(fila.Cells[1].Text);
            string codigoSociedad = HttpUtility.HtmlDecode(fila.Cells[2].Text);
            string nombreSociedad = HttpUtility.HtmlDecode(fila.Cells[3].Text);
            string nombreCentro = HttpUtility.HtmlDecode(fila.Cells[5].Text);
            string codigoCajaChica = HttpUtility.HtmlDecode(fila.Cells[7].Text);

            String b64 = Convert.ToBase64String(Encoding.UTF8.GetBytes(string.Format("idCajaChica={0}&CodigoSociedad={1}&nombreSociedad={2}&nombreCentro={3}&codigoCajaChica={4}", idCajaChica, codigoSociedad, nombreSociedad, nombreCentro, codigoCajaChica)));
            string url = string.Concat("~/RevisionFacturas/ListadoFacturas.aspx?", b64);
            Response.Redirect(url);
        }

        private void CargarSociedadDDL()
        {
            List<LlenarDDL_DTO> listaDDLDto = new List<LlenarDDL_DTO>();
            GestorSociedad gestorSociedad = null;

            try
            {
                gestorSociedad = GestorSociedad();
                listaDDLDto = gestorSociedad.ListarSociedades(usuario);

                ddlSociedad.DataSource = listaDDLDto;
                ddlSociedad.DataTextField = "DESCRIPCION";
                ddlSociedad.DataValueField = "IDENTIFICADOR";
                ddlSociedad.DataBind();
                ddlSociedad.Items.Insert(0, new ListItem("::Seleccione sociedad::", "0"));
            }
            finally
            {
                if (gestorSociedad != null) gestorSociedad.Dispose();
            }
            CargarCentroDDL();
        }

        private void CargarCentroDDL()
        {
            List<LlenarDDL_DTO> listaDDLDto = new List<LlenarDDL_DTO>();
            GestorSociedad gestorSociedad = null;

            try
            {

                //ddlCentro.DataSource = listaDDLDto;
                //ddlCentro.DataTextField = "DESCRIPCION";
                //ddlCentro.DataValueField = "IDENTIFICADOR";
                //ddlCentro.DataBind();
                ddlCentro.Items.Insert(0, new ListItem("::Seleccione centro::", "0"));
            }
            finally
            {
                if (gestorSociedad != null) gestorSociedad.Dispose();
            }
        }

        private void CargarSociedadCentroDDL(string usuario, string idSociedad)
        {
            List<LlenarDDL_DTO> listaDDLDto = new List<LlenarDDL_DTO>();
            GestorSociedad gestorSociedad = null;

            try
            {
                gestorSociedad = GestorSociedad();
                listaDDLDto = gestorSociedad.ListarUsuarioCentro(usuario, hfSociedad.Value);

                ddlCentro.DataSource = listaDDLDto;
                ddlCentro.DataTextField = "DESCRIPCION";
                ddlCentro.DataValueField = "IDENTIFICADOR";
                ddlCentro.DataBind();
                ddlCentro.Items.Insert(0, new ListItem("::Seleccione centro::", "0"));

            }
            finally
            {
                if (gestorSociedad != null) gestorSociedad.Dispose();
            }
        }

        private void CargarGridViewCajasChicas(string codigoSociedad, Int16 idCentro, string usuario, Int32 numeroCajaChica, Int32 correlativo)
        {
            List<CajaChicaEncabezadoDTO> _listaCajaChicaDto = null;

            Int32 numero = 1;

            _listaCajaChicaDto = BuscarCajasChicasUsuario(codigoSociedad, Convert.ToInt16(hfCentro.Value), numeroCajaChica, correlativo);

            gvListadoCajasChicas.DataSource = (from cajaChica in _listaCajaChicaDto
                                               select new
                                               {
                                                   NUMERO = numero++,
                                                   ID_CAJA_CHICA = cajaChica.ID_CAJA_CHICA,
                                                   CODIGO_SOCIEDAD = cajaChica.CODIGO_SOCIEDAD,
                                                   NOMBRE_SOCIEDAD = cajaChica.NOMBRE_EMPRESA,
                                                   ID_CENTRO = cajaChica.CODIGO_CENTRO,
                                                   NOMBRE_CENTRO = cajaChica.NOMBRE_CENTRO,
                                                   CODIGO_CAJA_CHICA = cajaChica.CODIGO_CC,
                                                   NUMERO_CAJA_CHICA = cajaChica.CAJA_CHICA_SAP,
                                                   CORRELATIVO = cajaChica.CORRELATIVO,
                                                   DESCRIPCION = cajaChica.DESCRIPCION,
                                                   MONTO = cajaChica.MONTO_CC.ToString("F"),
                                                   USUARIO = cajaChica.USUARIO_MANTENIMIENTO.USUARIO_ALTA,
                                                   FECHA_CREACION = cajaChica.USUARIO_MANTENIMIENTO.FECHA_ALTA.ToString("dd/MM/yyyy"),
                                                   MONEDA = cajaChica.MONEDA
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