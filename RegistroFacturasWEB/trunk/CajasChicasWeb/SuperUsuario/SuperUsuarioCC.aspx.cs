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
    public partial class SuperUsuarioCC : System.Web.UI.Page
    {
        #region Declaraciones

        string cnnApl = ConfigurationManager.ConnectionStrings["CnnApl"].ToString();
        string usuario;
        bool result;
        string dominio;
        string sociedad, centro, numero, correlativo;

        #endregion

        #region Eventos

        protected void Page_Load(object sender, EventArgs e)
        {
            dominio = (string)Session["dominio"].ToString();
            usuario = Context.User.Identity.Name.ToString();
            if (!IsPostBack)
            {
                DatosCC(false);
            }
        }


        #endregion

        #region Metodos

        private void Limpiar()
        {
            lblEstadoCC.Text = "Estado Actual: ";
            lblIdCC.Text = "Id CC: ";
            lblNombreCC.Text = "Nombre: ";
            ddlEstado.SelectedValue = "-1";
            txtJustificacionCC.Text = "";
        }

        private void DesplegarAviso(string mensaje)
        {
            OcultarAvisos();
            divMensaje.Attributes.CssStyle.Add("display", "table");
            divMensaje.InnerHtml = mensaje;
        }

        private void DesplegarError(string mensaje)
        {
            OcultarAvisos();
            divMensajeError.Attributes.CssStyle.Add("display", "table");
            divMensajeError.InnerHtml = mensaje;
        }

        private void OcultarAvisos()
        {
            divMensaje.Attributes.CssStyle.Add("display", "none");
            divMensajeError.Attributes.CssStyle.Add("display", "none");
        }

        private void DatosCC(bool estado)
        {
            lblEstadoCC.Visible = estado;
            lblIdCC.Visible = estado;
            lblNombreCC.Visible = estado;
            lblJustificacionCC.Visible = estado;
            txtJustificacionCC.Visible = estado;
            lblEstadoCCN.Visible = estado;
            ddlEstado.Visible = estado;
            btbGuardarCC.Visible = estado; 
        }

        private void CargarNuevoEstado()
        {
            List<LlenarDDL_DTO> listaDDLDto = new List<LlenarDDL_DTO>();

            listaDDLDto.Add(new LlenarDDL_DTO() { IDENTIFICADOR = "-1", DESCRIPCION = "::Nuevo Estado::" });
            listaDDLDto.Add(new LlenarDDL_DTO() { IDENTIFICADOR = "0", DESCRIPCION = "ANULADA" });
            listaDDLDto.Add(new LlenarDDL_DTO() { IDENTIFICADOR = "1", DESCRIPCION = "ABIERTA" });
            listaDDLDto.Add(new LlenarDDL_DTO() { IDENTIFICADOR = "2", DESCRIPCION = "CERRADA" });

            ddlEstado.DataSource = listaDDLDto;
            ddlEstado.DataTextField = "DESCRIPCION";
            ddlEstado.DataValueField = "IDENTIFICADOR";
            ddlEstado.DataBind();
        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            OcultarAvisos();
            Limpiar();
            if (txtNumeroCC.Text == "")
            {
                DesplegarError("Debe Ingresar el Numero de Caja Chica");
            }
            else
            {
                GestorSeguridad gestorSeguridad = null;
                try
                {
                    DatosCC(true);
                    int cont = 1;
                    string codigos = txtNumeroCC.Text;
                    string[] split = codigos.Split(new char[] { '-' });
                    foreach (string s in split)
                    {
                        switch (cont)
                        {
                            case 1: sociedad = s;
                                break;
                            case 2: centro = s;
                                break;
                            case 3: numero = s;
                                break;
                            case 4: correlativo = s;
                                break;
                            default: break;
                        }
                        cont++;
                    }
                    
                    
                    gestorSeguridad = GestorSeguridad();
                    CargarDatosCC(gestorSeguridad.BuscarDatosCC(3, sociedad, centro, numero, correlativo));
                    CargarNuevoEstado();
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

        private void CargarDatosCC(SuperUsuarioDTO superUsuarioDto)
        {
            lblEstadoCC.Text = string.Concat(lblEstadoCC.Text, superUsuarioDto.EstadoActual);
            lblIdCC.Text = string.Concat(lblIdCC.Text, superUsuarioDto.IdCC);
            lblNombreCC.Text = string.Concat(lblNombreCC.Text, superUsuarioDto.NombreCC);
            hfEstado.Value = superUsuarioDto.IdEstadoActual.ToString();
            hfIdCC.Value = superUsuarioDto.IdCC.ToString();
        }

        protected void btnGrabarFact_Click(object sender, EventArgs e)
        {
            GestorSeguridad gestorSeguridad = null;
            try
            {
                string pais = dominio.ToString().Substring(0, 2);

                if (txtJustificacionCC.Text == "")
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

                    gestorSeguridad.ModificarEstadoCC(4, usuario, pais, Convert.ToDecimal(hfIdCC.Value.ToString()), Convert.ToInt16(hfEstado.Value.ToString()), Convert.ToInt16(ddlEstado.SelectedItem.Value.ToString()), txtJustificacionCC.Text);

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

        #endregion

        #region Gestores

        protected GestorSeguridad GestorSeguridad()
        {
            GestorSeguridad gestorSeguridad = new GestorSeguridad(cnnApl);
            return gestorSeguridad;
        }
        #endregion

    }
}