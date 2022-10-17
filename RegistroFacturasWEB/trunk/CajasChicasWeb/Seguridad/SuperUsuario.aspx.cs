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
    public partial class SuperUsuario : System.Web.UI.Page
    {
        #region Declaraciones

        string cnnApl = ConfigurationManager.ConnectionStrings["CnnApl"].ToString();
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
                DatosFactura(false);
                // CargarDDLTipoDocumento(dominio);
                CargarDDLSociedad(usuario);
                CargarNuevoEstado();
            }
        }

        protected void Limpiar()
        {
            lblEstado.Text = "Estado Actual: ";
            lblIdFact.Text = "Id Factura: ";
            lblMonto.Text = "Monto: ";
            lblDividida.Text = "Dividida: ";
            lblFecha.Text = "Fecha: ";
            lblIdCCF.Text = "ID CC: ";
            lblCCF.Text = "Caja Chica: ";
            ddlEstado.SelectedValue = "-1";
            txtJustificacion.Text = "";
        }

        private void CargarNuevoEstado()
        {
            List<LlenarDDL_DTO> listaDDLDto = new List<LlenarDDL_DTO>();

            listaDDLDto.Add(new LlenarDDL_DTO() { IDENTIFICADOR = "-1", DESCRIPCION = "::Nuevo Estado::" });
            listaDDLDto.Add(new LlenarDDL_DTO() { IDENTIFICADOR = "0", DESCRIPCION = "BAJA" });
            listaDDLDto.Add(new LlenarDDL_DTO() { IDENTIFICADOR = "1", DESCRIPCION = "VIGENTE" });

            ddlEstado.DataSource = listaDDLDto;
            ddlEstado.DataTextField = "DESCRIPCION";
            ddlEstado.DataValueField = "IDENTIFICADOR";
            ddlEstado.DataBind();
        }

        private void CargarDDLTipoDocumento(string dominio)
        {
            GestorProveedor gestorProveedor = null;

            try
            {
                gestorProveedor = GestorProveedor();

                List<TipoDocumentoDTO> _listaTipoDocumentoDto = gestorProveedor.ListaTipoDocumentoPais(dominio);

                ddlTipoDocumento.Items.Clear();
                ddlTipoDocumento.Items.Add(new ListItem("::DOCUMENTOS::", "-1"));
                ddlTipoDocumento.AppendDataBoundItems = true;
                ddlTipoDocumento.DataSource = _listaTipoDocumentoDto;
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

        private void CargarDDLSociedad(string usuario)
        {
            GestorSeguridad gestorSeguridad = null;

            try
            {
                gestorSeguridad = GestorSeguridad();

                List<LlenarDDL_DTO> _listaSociedadCentroDto = gestorSeguridad.ListaSociedadUsuario(usuario);

                ddlSociedad.Items.Clear();
                ddlSociedad.Items.Add(new ListItem("::SOCIEDAD::", "-1"));
                ddlSociedad.AppendDataBoundItems = true;
                ddlSociedad.DataSource = _listaSociedadCentroDto;
                ddlSociedad.DataTextField = "Identificador";
                ddlSociedad.DataValueField = "Descripcion";
                ddlSociedad.DataBind();
            }
            catch
            {
                DesplegarError("Error al recuperar tipos de documentos");
            }
            finally
            {
                if (gestorSeguridad != null) gestorSeguridad.Dispose();
            }
        }
        #endregion

        #region Metodos

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

        private void DatosFactura(bool estado)
        {
            lblProveedor.Visible = estado;
            lblEstado.Visible = estado;
            lblIdFact.Visible = estado;
            lblMonto.Visible = estado;
            lblDividida.Visible = estado;
            lblFecha.Visible = estado;
            lblCCF.Visible = estado;
            lblEstadoN.Visible = estado;
            ddlEstado.Visible = estado;
            lblJustificacion.Visible = estado;
            txtJustificacion.Visible = estado;
            btbGrabarFact.Visible = estado;
            lblIdCCF.Visible = estado;
            lblEstadoN.Visible = estado;
            ddlEstado.Visible = estado;
            lblJustificacion.Visible = estado;
            txtJustificacion.Visible = estado;
            btbGrabarFact.Visible = estado;
        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            OcultarAvisos();
            Limpiar();
            if ((ddlTipoDocumento.SelectedItem.Value.ToString() == "-1") || (txtNumDoc.Text == ""))
            {
                DesplegarError("Debe completar todos los datos");
            }
            else
            {
                GestorSeguridad gestorSeguridad = null;
                try
                {
                    gestorSeguridad = GestorSeguridad();
                    CargarDatosFactura(gestorSeguridad.BuscarDatosFactura(Convert.ToInt32(ddlTipoDocumento.SelectedItem.Value.ToString()), txtNumDoc.Text, txtSerie.Text, txtNumeroFact.Text, 1, usuario));
                }
                catch
                {
                    DesplegarError("Error al retornar datos");
                }
                finally
                {
                    if (gestorSeguridad != null) gestorSeguridad.Dispose();
                }
            }
        }

        private void CargarDatosFactura(SuperUsuarioDTO superUsuarioDto)
        {
            lblProveedor.Text = superUsuarioDto.NombreProveedor.ToString();
            lblIdFact.Text = string.Concat(lblIdFact.Text, superUsuarioDto.IdFactura.ToString());
            lblEstado.Text = string.Concat(lblEstado.Text, superUsuarioDto.EstadoActual);
            lblMonto.Text = string.Concat(lblMonto.Text, superUsuarioDto.Total.ToString());
            lblDividida.Text = string.Concat(lblDividida.Text, superUsuarioDto.Dividida);
            lblFecha.Text = string.Concat(lblFecha.Text, superUsuarioDto.Fecha);
            lblIdCCF.Text = string.Concat(lblIdCCF.Text, superUsuarioDto.IDCCFactura);
            lblCCF.Text = string.Concat(lblCCF.Text, superUsuarioDto.CCFactura);
            DatosFactura(true);
            hfEstado.Value = superUsuarioDto.IdEstadoActual.ToString();
            hfIdFactura.Value = superUsuarioDto.IdFactura.ToString();
            hfIdCC.Value = superUsuarioDto.IDCCFactura.ToString();

        }

        protected void btnGrabarFact_Click(object sender, EventArgs e)
        {
            GestorSeguridad gestorSeguridad = null;
            try
            {
                string pais = dominio.ToString().Substring(0, 2);

                //if (hfEstado.Value.ToString() == "2")
                //{
                //    DesplegarError("No se puede cambiar estado, porque ya ha sido sincronizado hacia SAP");
                //}
                //else 
                if (txtJustificacion.Text == "")
                {
                    DesplegarError("Debe de ingresar justificación del cambio");
                }
                else if (ddlEstado.SelectedItem.Value.ToString() == "-1")
                {
                    DesplegarError("Debe seleccionar el nuevo estado");
                }
                else
                {
                    gestorSeguridad = GestorSeguridad();

                    gestorSeguridad.ModificarEstadoFactura(Convert.ToInt16(ddlEstado.SelectedItem.Value.ToString()), Convert.ToDecimal(hfIdFactura.Value.ToString()), 1, Convert.ToDecimal(hfIdCC.Value.ToString()), usuario, pais, hfEstado.Value.ToString(), 2, txtJustificacion.Text);
                    OcultarAvisos();
                    DesplegarAviso("Datos Actualizados correctamente");
                }
            }
            catch
            {
                DesplegarError("Error al actualizar estado");
            }
            finally
            {
                if (gestorSeguridad != null) gestorSeguridad.Dispose();
            }
        }

        //public void txtPais_TextChanged(object sender, EventArgs e)
        //{
        //    CargarDDLTipoDocumento(txtPais.Text); //
        //}

        #endregion

        #region Gestores

        protected GestorSeguridad GestorSeguridad()
        {
            GestorSeguridad gestorSeguridad = new GestorSeguridad(cnnApl);
            return gestorSeguridad;
        }

        protected GestorProveedor GestorProveedor()
        {
            GestorProveedor gestorProveedor = new GestorProveedor(cnnApl);
            return gestorProveedor;
        }
        #endregion

        protected void ddlSociedad_SelectedIndexChanged(object sender, EventArgs e)
        {
            CargarDDLTipoDocumento(ddlSociedad.SelectedValue.ToString());
        }


    }
}