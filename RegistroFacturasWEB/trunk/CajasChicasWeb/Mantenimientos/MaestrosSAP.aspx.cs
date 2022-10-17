using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using DipCmiGT.LogicaCajasChicas.Sesion;
using DipCmiGT.LogicaComun;
using SAP.Middleware.Connector;
using LogicaCajasChicas.Entidad;
using LogicaCajasChicas.Sesion;

namespace RegistroFacturasWEB.Mantenimientos
{
    public partial class MaestrosSAP : System.Web.UI.Page
    {

        #region Declaraciones

        string cnnApl = ConfigurationManager.ConnectionStrings["CnnApl"].ToString();
        RfcDestination SapRfcDestination;
        //RfcDestination SapRfcDestination = RfcDestinationManager.GetDestination("DEV");

        string usuario;

        #endregion

        #region Eventos

        protected void Page_Load(object sender, EventArgs e)
        {
            usuario = Context.User.Identity.Name.ToString();
            if (!IsPostBack)
            {
                CargarDDLSociedad();
            }
        }

        protected void btnCajaChica_Click(object sender, EventArgs e)
        {
            OcultarAvisos();
            MigrarDatosCC();
        }

        protected void btnCentroCosto_Click(object sender, EventArgs e)
        {
            OcultarAvisos();
            MigrarCentrosCosto();
        }

        protected void btnOrdenCosto_Click(object sender, EventArgs e)
        {
            OcultarAvisos();
            MigrarOrdenCosto();
        }

        protected void btnCuentaContable_Click(object sender, EventArgs e)
        {
            OcultarAvisos();
            MigrarCuentasContables();
        }

        #endregion

        #region Metodos

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

        private void MigrarDatosCC()
        {
            GestorDatosSAP gestorDatosSAP = null;
            GestorSociedad gestorsociedad = null;
            string mandante = "";

            try
            {
                if (!ValidarSociedad())
                    throw new ExcepcionesDIPCMI("Debe seleccionar una sociedad");


                gestorsociedad = GestorSociedad();
                mandante = gestorsociedad.MandanteSociedad(ddlSociedad.SelectedValue.ToString());
                SapRfcDestination = RfcDestinationManager.GetDestination(mandante);

                gestorDatosSAP = GestorDatosSAP();

                gestorDatosSAP.MigrarCajasChicas(SapRfcDestination, ddlSociedad.SelectedValue);

                DesplegarAviso("Importación de proveedores esporadicos fue satisfactorio.");
            }
            catch (ExcepcionesDIPCMI ex)
            {
                DesplegarError(ex.Message);
            }
            finally
            {
                if (gestorDatosSAP != null) gestorDatosSAP.Dispose();
            }
        }

        private void MigrarCentrosCosto()
        {
            GestorDatosSAP gestorDatosSAP = null;
            GestorSociedad gestorsociedad = null;
            string mandante = "";

            try
            {
                if (!ValidarSociedad())
                    throw new ExcepcionesDIPCMI("Debe seleccionar una sociedad");

                gestorsociedad = GestorSociedad();
                mandante = gestorsociedad.MandanteSociedad(ddlSociedad.SelectedValue.ToString());
                SapRfcDestination = RfcDestinationManager.GetDestination(mandante);

                gestorDatosSAP = GestorDatosSAP();

                gestorDatosSAP.MigrarCentroCosto(SapRfcDestination, ddlSociedad.SelectedValue);

                DesplegarAviso("Importación de centros de costo fue satisfactorio.");
            }
            catch (ExcepcionesDIPCMI ex)
            {
                DesplegarError(ex.Message);
            }
            finally
            {
                if (gestorDatosSAP != null) gestorDatosSAP.Dispose();
            }

        }

        private void MigrarOrdenCosto()
        {
            GestorDatosSAP gestorDatosSAP = null;
            GestorSociedad gestorsociedad = null;
            string mandante = "";

            try
            {
                if (!ValidarSociedad())
                    throw new ExcepcionesDIPCMI("Debe seleccionar una sociedad");

                gestorsociedad = GestorSociedad();
                mandante = gestorsociedad.MandanteSociedad(ddlSociedad.SelectedValue.ToString());
                SapRfcDestination = RfcDestinationManager.GetDestination(mandante);

                gestorDatosSAP = GestorDatosSAP();

                gestorDatosSAP.MigrarOrdenCosto(SapRfcDestination, ddlSociedad.SelectedValue);

                DesplegarAviso("Importación de ordenes de costo fue satisfactorio.");
            }
            catch (ExcepcionesDIPCMI ex)
            {
                DesplegarError(ex.Message);
            }
            finally
            {
                if (gestorDatosSAP != null) gestorDatosSAP.Dispose();
            }

        }

        private void MigrarCuentasContables()
        {
            GestorDatosSAP gestorDatosSAP = null;
            GestorSociedad gestorsociedad = null;
            string mandante = "";

            try
            {
                if (!ValidarSociedad())
                    throw new ExcepcionesDIPCMI("Debe seleccionar una sociedad");

                gestorsociedad = GestorSociedad();
                mandante = gestorsociedad.MandanteSociedad(ddlSociedad.SelectedValue.ToString());
                SapRfcDestination = RfcDestinationManager.GetDestination(mandante);

                gestorDatosSAP = GestorDatosSAP();

                gestorDatosSAP.MigrarCuentasContables(SapRfcDestination, ddlSociedad.SelectedValue);

                DesplegarAviso("Importación de cuentas contables fue satisfactorio.");
            }
            catch (ExcepcionesDIPCMI ex)
            {
                DesplegarError(ex.Message);
            }
            finally
            {
                if (gestorDatosSAP != null) gestorDatosSAP.Dispose();
            }

        }

        private bool ValidarSociedad()
        {
            if (ddlSociedad.SelectedValue == "-1")
                return false;

            return true;
        }

        #endregion

        #region Gestores

        protected GestorDatosSAP GestorDatosSAP()
        {
            GestorDatosSAP gestorsociead = new GestorDatosSAP(cnnApl);
            return gestorsociead;
        }

        protected GestorSociedad GestorSociedad()
        {
            GestorSociedad gestorsociead = new GestorSociedad(cnnApl);
            return gestorsociead;
        }

        #endregion

    }
}