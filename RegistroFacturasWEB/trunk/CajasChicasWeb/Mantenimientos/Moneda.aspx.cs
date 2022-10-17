using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using DipCmiGT.LogicaCajasChicas.Sesion;
using LogicaCajasChicas;
using DipCmiGT.LogicaComun;
using DipCmiGT.LogicaComun.Enum;

namespace RegistroFacturasWEB.Mantenimientos
{
    public partial class Moneda : System.Web.UI.Page
    {
        #region Declaraciones
        string cnnApl = ConfigurationManager.ConnectionStrings["CnnApl"].ToString();
        string usuario;
        #endregion

        #region Eventos
        protected void Page_Load(object sender, EventArgs e)
        {
            usuario = Context.User.Identity.Name.ToString();
            if (!IsPostBack)
                CargarMonedaGrid();
        }

        protected void btnLimpiar_Click(object sender, EventArgs e)
        {
            LimpiarControles();
        }

        protected void btnGrabar_Click(object sender, EventArgs e)
        {
            OcultarAvisos();
            AlmacenaMoneda();
        }

        protected void gvMoneda_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            this.gvMoneda.PageIndex = e.NewPageIndex;
            CargarMonedaGrid();
        }

        protected void gvMoneda_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            OcultarAvisos();
            if (e.CommandName.Equals("Editar"))
                CargarMonedaEdicion(e);

            if (e.CommandName.Equals("Baja"))
                DarBajaMoneda(e);

            if (e.CommandName.Equals("Alta"))
                DarAltaMoneda(e);

        }

        #endregion

        #region Metodos

        public void CargarMonedaGrid()
        {
            MonedaDTO _monedaDto = new MonedaDTO();
            GestorSociedad gestorSociedad = null;

            int x = 1;
            try
            {
                gestorSociedad = GestorSociedad();
                gvMoneda.DataSource = (from moneda in gestorSociedad.ListarMonedas()
                                       select new
                                       {
                                           Numero = x++,
                                           MONEDA = moneda.MONEDA,
                                           DESCRIPCION = moneda.DESCRIPCION,
                                           idAlta = moneda.ESTADO,
                                           USUARIO_ALTA = moneda.USUARIO_MANTENIMIENTO.USUARIO_ALTA,
                                           FECHA_ALTA = moneda.USUARIO_MANTENIMIENTO.FECHA_ALTA,
                                           USUARIO_MODIFICACION = moneda.USUARIO_MANTENIMIENTO.USUARIO_MODIFICO,
                                           FECHA_MODIFICACION = moneda.USUARIO_MANTENIMIENTO.FECHA_MODIFICACION
                                       }).ToArray();
                gvMoneda.DataBind();
            }
            catch
            {
                DesplegarError("Error al Desplegar las monedas");
            }
            finally
            {
                if (gestorSociedad != null) gestorSociedad.Dispose();
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
            hfIdMoneda.Value = "0";
            txtMoneda.Text = string.Empty;
            txtDescripcion.Text = string.Empty;
            OcultarAvisos();
        }

        private MonedaDTO CargarObjetoMoneda(ref MonedaDTO _monedaDto)
        {
            _monedaDto.MONEDA = txtMoneda.Text;
            _monedaDto.DESCRIPCION = txtDescripcion.Text;
            _monedaDto.ESTADO = Convert.ToBoolean(EstadoEnum.ALTA);
            _monedaDto.USUARIO_MANTENIMIENTO.USUARIO_ALTA = usuario;


            return _monedaDto;
        }

        private void AlmacenaMoneda()
        {
            MonedaDTO _monedaDto = new MonedaDTO();
            GestorSociedad gestorSociedad = null;
            OcultarAvisos();
            try
            {
                gestorSociedad = GestorSociedad();
                CargarObjetoMoneda(ref _monedaDto);
                gestorSociedad.AlmacenarMoneda(_monedaDto);
                DesplegarAviso("La moneda fue almacenada correctamente");
                CargarMonedaGrid();

            }
            catch (ExcepcionesDIPCMI ex)
            {
                DesplegarError(ex.Message);
            }
            catch
            {
                DesplegarError("Error al almacenar la moneda");
            }
            finally
            {
                if (gestorSociedad != null) gestorSociedad.Dispose();
            }
        }

        private void CargarMonedaEdicion(GridViewCommandEventArgs e)
        {
            int indice = Int32.Parse(e.CommandArgument.ToString());
            GridViewRow fila = gvMoneda.Rows[indice];

            txtDescripcion.Text = HttpUtility.HtmlDecode(fila.Cells[2].Text);
            txtMoneda.Text = HttpUtility.HtmlDecode(fila.Cells[1].Text);
        }

        private void DarBajaMoneda(GridViewCommandEventArgs e)
        {
            int indice = Int32.Parse(e.CommandArgument.ToString());
            GridViewRow fila = gvMoneda.Rows[indice];
            GestorSociedad gestorSociedad = null;

            try
            {
                gestorSociedad = GestorSociedad();
                gestorSociedad.DarBajaMoneda(usuario, HttpUtility.HtmlDecode(fila.Cells[1].Text));

                DesplegarAviso("La moneda fue dada de baja");
                CargarMonedaGrid();
            }
            catch
            {
                DesplegarError("Error al dar de baja la moneda");
            }
            finally
            {
                if (gestorSociedad != null) gestorSociedad.Dispose();
            }
        }

        private void DarAltaMoneda(GridViewCommandEventArgs e)
        {
            int indice = Int32.Parse(e.CommandArgument.ToString());
            GridViewRow fila = gvMoneda.Rows[indice];
            GestorSociedad gestorSociedad = null;

            try
            {
                gestorSociedad = GestorSociedad();
                gestorSociedad.DarAltaMoneda(usuario, HttpUtility.HtmlDecode(fila.Cells[1].Text));

                DesplegarAviso("La moneda fue dada de alta");
                CargarMonedaGrid();
            }
            catch
            {
                DesplegarError("Error al dar alta a la moneda");
            }
            finally
            {
                if (gestorSociedad != null) gestorSociedad.Dispose();
            }
        }
        #endregion

        #region Gestores

        protected GestorSociedad GestorSociedad()
        {
            GestorSociedad gestorSociedad = new GestorSociedad(cnnApl);
            return gestorSociedad;
        }
        #endregion

        protected void btnBuscar_Click(object sender, EventArgs e)
        {

        }

    }
}