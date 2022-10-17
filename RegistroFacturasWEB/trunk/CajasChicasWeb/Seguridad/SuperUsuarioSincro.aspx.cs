using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using LogicaComun.Enum;
using DipCmiGT.LogicaCajasChicas;
using System.Configuration;
using DipCmiGT.LogicaCajasChicas.Sesion;
using DipCmiGT.LogicaComun;
using LogicaCajasChicas;


namespace RegistroFacturasWEB.Seguridad
{
    public partial class SuperUsuarioSincro : System.Web.UI.Page
    {
        #region Declaraciones

        string cnnApl = ConfigurationManager.ConnectionStrings["CnnInterfaces"].ToString();
        string cnnMm = ConfigurationManager.ConnectionStrings["CnnInterfacesMM"].ToString();
        string cnn = ConfigurationManager.ConnectionStrings["CnnApl"].ToString();
        string usuario;
        bool result;
        string dominio;

        #endregion

        #region Eventos

        protected void Page_Load(object sender, EventArgs e)
        {
            dominio = (string)Session["dominio"].ToString();
            usuario = Context.User.Identity.Name.ToString();
            if (!IsPostBack)
            {
                DatosSincro(false);
            }
        }


        #endregion

        #region Metodos

        private void Limpiar()
        {
            lblNFactura.Text = "Numero de Factura: ";
            lblEstadoSincro.Text = "Estado Actual: ";
            lblDescripcion.Text = "Descripcion: ";
            lblProveedor.Text = "Proveedor: ";
            lblDocId.Text = "Documento de Identificación: ";
            lblCodSincro.Text = "Codigo de Sincronizacion: ";
            hlblCodSincro.Text = "";
            hlblDocId.Text = "";
            hlblNFactura.Text = "";

            txtJustificacion.Text = "";
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

        private void DatosSincro(bool estado)
        {
            OcultarAvisos();
            lblNFactura.Visible = estado;
            lblEstadoSincro.Visible = estado;
            lblDescripcion.Visible = estado;
            lblProveedor.Visible = estado;
            lblDocId.Visible = estado;
            lblCodSincro.Visible = estado;
            lblJuestificacion.Visible = estado;
            txtJustificacion.Visible = estado;
            btbAnular.Visible = estado;

        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            OcultarAvisos();
            Limpiar();
            if ((ddlSociedad.SelectedItem.ToString() == "") || ((txtDocumento.Text == "") && (txtNoFactura.Text == "")))
            {
                DesplegarError("Debe llenar el campo sociedad y el campo de documento o numero de factura para realizar la busqueda");
            }
            else
            {
                string fechaFact = "";
                DateTime fch;
                string fechaForm = "";
                if (txtNoFactura.Text != "")
                {
                    if (string.IsNullOrEmpty(txtFechaFactura.Text) || string.IsNullOrEmpty(txtDocProveedor.Text))
                    {
                        DesplegarError("Al buscar por numero de factura debe indicar la fecha de la factura y el documento de proveedor");
                        return;
                    }
                    else
                    {
                        try
                        {
                            fch = Convert.ToDateTime(txtFechaFactura.Text);
                            fechaForm = fch.ToString("yyyy/MM/dd");

                            //string[] fecha = txtFechaFactura.Text.Split('/');
                            string[] fecha = fechaForm.Split('/');
                            fechaFact = fecha[0] + fecha[1] + fecha[2];
                        }
                        catch (Exception ex)
                        {
                            DesplegarError("Error en formato de fecha");
                        }
                    }
                }

                GestorSeguridad gestorSeguridad = null;
                try
                {
                    gestorSeguridad = GestorSeguridad(ddlSociedad.SelectedItem.ToString());
                    CargarDatosSincro(gestorSeguridad.BuscarDatosSincro(5, ddlSociedad.SelectedItem.ToString(), txtDocumento.Text, txtNoFactura.Text, fechaFact, txtDocProveedor.Text, txtSerie.Text));
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    DesplegarError("Error al retornar datos");
                }
                finally
                {
                    if (gestorSeguridad != null) gestorSeguridad.Dispose();
                }
            }
        }

        private void CargarDatosSincro(SuperUsuarioDTO superUsuarioDto)
        {
            if (superUsuarioDto != null)
            {
                lblNFactura.Text = string.Concat(lblNFactura.Text, superUsuarioDto.NoFactura);
                hlblNFactura.Text = superUsuarioDto.NoFactura.ToString();
                lblEstadoSincro.Text = string.Concat(lblEstadoSincro.Text, superUsuarioDto.EstadoActual);
                lblDescripcion.Text = string.Concat(lblDescripcion.Text, superUsuarioDto.NombreCC);
                lblProveedor.Text = string.Concat(lblProveedor.Text, superUsuarioDto.NombreProveedor);
                lblDocId.Text = string.Concat(lblDocId.Text, superUsuarioDto.DocIdentificacion);
                hlblDocId.Text = superUsuarioDto.DocIdentificacion.ToString();
                lblCodSincro.Text = string.Concat(lblCodSincro.Text, superUsuarioDto.CodigoSincronizacion);
                hlblCodSincro.Text = superUsuarioDto.CodigoSincronizacion.ToString();
                hfIdEstado.Value = superUsuarioDto.IdEstadoSicnro;
                DatosSincro(true);
            }
            else
            {
                DesplegarError("No se encontraron datos para mostra");
                DatosSincro(false);
            }
        }

        protected void btnAnular_Click(object sender, EventArgs e)
        {
            GestorSeguridad gestorSeguridad = null;
            GestorSeguridad gestorSeguridadL = null;
            string pais;
            try
            {
                if (dominio.ToString() != "")
                    pais = dominio.ToString().Substring(0, 2);
                else pais = "";

                if (txtJustificacion.Text == "")
                {
                    DesplegarError("Debe de ingresar justificación del cambio");
                }
                else
                {
                    gestorSeguridadL = GestorSeguridadLocal();
                    bool tienePermiso = gestorSeguridadL.ValidaPermisosAnulacion(usuario, ddlSociedad.SelectedItem.ToString());
                    if (tienePermiso)
                    {
                        string codSincronizacion = hlblCodSincro.Text;
                        long numFactura = long.Parse(hlblNFactura.Text);
                        string docIdentificacion = hlblDocId.Text;
                        string numFact = hlblNFactura.Text;
                        string serie = hlblNSerie.Text;
                        gestorSeguridad = GestorSeguridad();

                        gestorSeguridad.AnularSincronizacion(6, ddlSociedad.SelectedItem.ToString(), codSincronizacion.ToString());
                        gestorSeguridadL.AnularSincronizacionLog(6, ddlSociedad.SelectedItem.ToString(), codSincronizacion.ToString(), usuario, pais, txtJustificacion.Text);
                        List<decimal> idsFacturas = gestorSeguridadL.ModificarEstadosFacturas(numFact.Trim(), docIdentificacion.Trim(), usuario, serie.Trim());
                        foreach (decimal id in idsFacturas)
                        {
                            gestorSeguridadL.AnularSincronizacionLog(6, ddlSociedad.SelectedItem.ToString(), id.ToString(), usuario, pais, txtJustificacion.Text);
                        }
                        OcultarAvisos();
                        DesplegarAviso("Sincronización anulada correctamente");
                    }
                    else
                    {
                        DesplegarError("No posee permisos en la sociedad para anular esta factura ");
                    }
                }
            }
            catch (Exception ex)
            {
                DesplegarError("Error al ejecutar anulación");
            }
            finally
            {
                if (gestorSeguridad != null) gestorSeguridad.Dispose();
            }
        }


        #endregion

        #region Gestores

        protected GestorSeguridad GestorSeguridad()
        {
            GestorSeguridad gestorSeguridad = new GestorSeguridad(cnnApl);
            return gestorSeguridad;
        }

        protected GestorSeguridad GestorSeguridad(string codigoSociedad)
        {
            GestorSeguridad gestorResult;
            GestorSeguridad gestor = new GestorSeguridad(cnn);
            string mandante = gestor.ValidarMandante(codigoSociedad);
            if (mandante == "IP")
                gestorResult = new GestorSeguridad(cnnApl);
            else
                gestorResult = new GestorSeguridad(cnnMm);


            return gestorResult;
        }

        protected GestorSeguridad GestorSeguridadLocal()
        {
            GestorSeguridad gestorSeguridadL = new GestorSeguridad(cnn);
            return gestorSeguridadL;
        }
        #endregion
    }
}