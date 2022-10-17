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
using  System.Text;

namespace RegistroFacturasWEB.Aprobaciones
{
    public partial class ListadoAprobadores : System.Web.UI.Page
    {

        #region Declaraciones
        string cnnApl = ConfigurationManager.ConnectionStrings["CnnApl"].ToString();
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
                txtUsuario.Enabled = false;
                if (!strUrl.Contains('?'))
                    Response.Redirect("~/principal.aspx");

                if (!string.IsNullOrEmpty(strParam))
                {
                    CapturarParametros(strParam);

                    txtUsuario.Enabled = false;
                    CargarAprobadoresGrid(Convert.ToInt32(hfIdUsuario.Value));
                    OcultarAvisos();
                    txtUsuario.Text = hfUsuario.Value;
                }
                else
                    Response.Redirect("~/principal.aspx");
            }
            
        }

        protected void btnGrabar_Click(object sender, EventArgs e)
        {
            AlmacenaAprobadorCentroEncabezado();
        }

        protected void btnLimpiar_Click(object sender, EventArgs e)
        {
            LimpiarControles();
        }


        protected void gvAprobadores_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            this.gvAprobadores.PageIndex = e.NewPageIndex;
            CargarAprobadoresGrid(Convert.ToInt32(hfIdUsuario.Value));
        }

        protected void gvAprobadores_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName.Equals("Editar"))
            {
                OcultarAvisos();
                CargarAprobadorCentroEncabezadoEdicion(e);
            }
            if (e.CommandName.Equals("Baja"))
            {
                OcultarAvisos();
                DarBajaAprobadorCentroEncabezado(e);
            }
            if (e.CommandName.Equals("Alta"))
            {
                OcultarAvisos();
                DarAltaAprobadorCentroEncabezado(e);
            }
            if (e.CommandName.Equals("Detalle"))
                AgregarDetalle(e);
        }
        #endregion

        #region Metodos

        private void CapturarParametros(string parametros)
        {
            string strParametros = Encoding.UTF8.GetString(Convert.FromBase64String(parametros));
            string[] argsParam = strParametros.Split('&');
            string[] param;
            string usuario = string.Empty;

            param = argsParam[0].Split('=');
            if (param[0] == "Usuario")
            {
                hfUsuario.Value = param[1];
                param = argsParam[1].Split('=');
                hfIdUsuario.Value = param[1];

                return;
            }
        }

        public void CargarAprobadoresGrid(Int32 IdUsuario)
        {
            AprobadorCentroEncabezadoDTO _aprobadorCentroEncabezadoDto = new AprobadorCentroEncabezadoDTO();
            GestorAprobadores gestoraprobadores = null;

            int x = 1;
            try
            {
                gestoraprobadores = Gestoraprobadores();
                gvAprobadores.DataSource = (from aprobador in gestoraprobadores.ListaAprobadorCentroEncabezado(IdUsuario)
                                                select new
                                                {
                                                    Numero = x++,
                                                    ID_APROBACION_ENCABEZADO = aprobador.ID_APROBACION_ENCABEZADO,
                                                    ID_USUARIO = aprobador.ID_USUARIO,
                                                    USUARIO = aprobador.USUARIO,
                                                    PORCENTAJE = aprobador.PORCENTAJE_COMPRA,
                                                    TOLERANCIA = aprobador.TOLERANCIA,
                                                    idAlta = aprobador.ALTA,
                                                    USUARIO_CREACION = aprobador.USUARIO_MANTENIMIENTO.USUARIO_ALTA,
                                                    FECHA_CREACION = aprobador.USUARIO_MANTENIMIENTO.FECHA_ALTA,
                                                    USUARIO_MODIFICACION = aprobador.USUARIO_MANTENIMIENTO.USUARIO_MODIFICO,
                                                    FECHA_MODIFICACION = aprobador.USUARIO_MANTENIMIENTO.FECHA_MODIFICACION,
                                                    EstadoDetalle = aprobador.ALTA
                                                }).ToArray();
                gvAprobadores.DataBind();
            }
            catch
            {
                DesplegarError("Error al Desplegar Aprobador Centro");
            }
            finally
            {
                if (gestoraprobadores != null) gestoraprobadores.Dispose();
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
            hfIdAprobadorCentro.Value = "0";
            txtPorcentaje.Text = string.Empty;
            txtTolerancia.Text = string.Empty;
            chAlta.Checked = false;
            lblUsuarioCreacionBD.Text = string.Empty;
            lblFechaCreacionBD.Text = string.Empty;
            lblUsuarioDB.Text = string.Empty;
            lblFechaModificacionDB.Text = string.Empty;
            OcultarAvisos();
        }

        private AprobadorCentroEncabezadoDTO CargarObjetoAprobadorCentroEncabezado(ref AprobadorCentroEncabezadoDTO _aprobadorCentroEncabezadoDto)
        {
            _aprobadorCentroEncabezadoDto.ID_APROBACION_ENCABEZADO = Convert.ToInt32(hfIdAprobadorCentro.Value);
            _aprobadorCentroEncabezadoDto.ID_USUARIO = Convert.ToInt32(hfIdUsuario.Value);
            _aprobadorCentroEncabezadoDto.TOLERANCIA = Convert.ToInt16(txtTolerancia.Text);
            _aprobadorCentroEncabezadoDto.PORCENTAJE_COMPRA = Convert.ToInt16(txtPorcentaje.Text);
            _aprobadorCentroEncabezadoDto.ALTA = chAlta.Checked;
            _aprobadorCentroEncabezadoDto.USUARIO_MANTENIMIENTO.USUARIO_ALTA = usuario;
            _aprobadorCentroEncabezadoDto.USUARIO_MANTENIMIENTO.USUARIO_MODIFICO = usuario;

            return _aprobadorCentroEncabezadoDto;
        }

        private void CargarAprobadorCentroEncabezadoEdicion(GridViewCommandEventArgs e)
        {
            int indice = Int32.Parse(e.CommandArgument.ToString());
            GridViewRow fila = gvAprobadores.Rows[indice];


            hfIdAprobadorCentro.Value = HttpUtility.HtmlDecode(fila.Cells[1].Text);
            hfIdUsuario.Value = fila.Cells[2].Text;
            txtUsuario.Text = HttpUtility.HtmlDecode(fila.Cells[3].Text);
            txtPorcentaje.Text = HttpUtility.HtmlDecode(fila.Cells[4].Text);
            txtTolerancia.Text = HttpUtility.HtmlDecode(fila.Cells[5].Text);
            chAlta.Checked = (((CheckBox)fila.FindControl("idAlta")).Checked);
            lblUsuarioCreacionBD.Text = HttpUtility.HtmlDecode(fila.Cells[7].Text);
            lblFechaCreacionBD.Text = HttpUtility.HtmlDecode(fila.Cells[8].Text);
            lblUsuarioDB.Text = HttpUtility.HtmlDecode(fila.Cells[9].Text);
            lblFechaModificacionDB.Text = HttpUtility.HtmlDecode(fila.Cells[10].Text);
        }

        private string ValidarDatos()
        {
            string mensaje = string.Empty;

            if (txtUsuario.Text == string.Empty)
                mensaje += "Debe Indicar el Usuario . <br>";

            if (txtPorcentaje.Text == string.Empty) 
                mensaje += "Debe Indicar el Porcentaje. <br>";

            if (txtTolerancia.Text == string.Empty)
                mensaje += "Debe Indicar la Tolerancia. <br>";

            return mensaje;
        }

        private void AlmacenaAprobadorCentroEncabezado()
        {
            AprobadorCentroEncabezadoDTO _aprobadorCentroEncabezadoDto = new AprobadorCentroEncabezadoDTO();
            GestorAprobadores gestoraprobadores = null;
            string mensaje = string.Empty;

            OcultarAvisos();
            try
            {
                mensaje = ValidarDatos();

                if (!mensaje.Equals(string.Empty))
                    throw new ExcepcionesDIPCMI(mensaje);

                gestoraprobadores = Gestoraprobadores();
                CargarObjetoAprobadorCentroEncabezado(ref _aprobadorCentroEncabezadoDto);
                gestoraprobadores.AlmacenarAprobadorCentroEncabezado(_aprobadorCentroEncabezadoDto);
                hfIdAprobadorCentro.Value = _aprobadorCentroEncabezadoDto.ID_APROBACION_ENCABEZADO.ToString();
                DesplegarAviso("Aprobador Centro Almacenado Correctamente");
                CargarAprobadoresGrid(Convert.ToInt32(hfUsuario.Value));

            }
            catch (ExcepcionesDIPCMI ex)
            {
                DesplegarError(ex.Message);
            }
            catch
            {
                DesplegarError("Error al almacenar Aprobador Centro");
            }
            finally
            {
                if (gestoraprobadores != null) gestoraprobadores.Dispose();
            }
        }

        private void DarBajaAprobadorCentroEncabezado(GridViewCommandEventArgs e)
        {
            int indice = Int32.Parse(e.CommandArgument.ToString());
            GridViewRow fila = gvAprobadores.Rows[indice];
            GestorAprobadores gestoraprobadores = null;

            try
            {
                gestoraprobadores = Gestoraprobadores();
                gestoraprobadores.DarBajaAprobadorCentroEncabezado(Convert.ToInt16(fila.Cells[1].Text), usuario);

                DesplegarAviso("Aprobador Centro fue dado de baja");
                CargarAprobadoresGrid(Convert.ToInt32(hfIdUsuario.Value));
            }
            catch
            {
                DesplegarError("Error al dar de baja Aprobador Centro");
            }
            finally
            {
                if (gestoraprobadores != null) gestoraprobadores.Dispose();
            }
        }

        private void DarAltaAprobadorCentroEncabezado(GridViewCommandEventArgs e)
        {
            int indice = Int32.Parse(e.CommandArgument.ToString());
            GridViewRow fila = gvAprobadores.Rows[indice];
            GestorAprobadores gestoraprobadores = null;

            try
            {
                gestoraprobadores = Gestoraprobadores();
                gestoraprobadores.DarAltaAprobadorCentroEncabezado(Convert.ToInt16(fila.Cells[1].Text), usuario);

                DesplegarAviso("Aprobador Centro fue dado de alta");
                CargarAprobadoresGrid(Convert.ToInt32(hfIdUsuario.Value));
            }
            catch
            {
                DesplegarError("Error al dar alta Aprobador Centro");
            }
            finally
            {
                if (gestoraprobadores != null) gestoraprobadores.Dispose();
            }
        }

        private void AgregarDetalle(GridViewCommandEventArgs e)
        {
            int indice = Int32.Parse(e.CommandArgument.ToString());
            GridViewRow fila = gvAprobadores.Rows[indice]; 

            Int32 IdAprobacionEncabezado = Convert.ToInt32(fila.Cells[1].Text);
            Int32 idUsuario = Convert.ToInt32(fila.Cells[2].Text);
            string Usuario = fila.Cells[3].Text;

            String b64 = Convert.ToBase64String(Encoding.UTF8.GetBytes(string.Format("IdAprobacionEncabezado={0}&IdUsuario={1}&Usuario={2}", IdAprobacionEncabezado, idUsuario, Usuario)));

            string url = string.Concat("~/Aprobadores/AprobadorCentro.aspx?", b64);
            //string url = string.Concat("~/Aprobaciones/AprobadoresCentro.aspx?", b64);
            Response.Redirect(url);
        }
        #endregion

        #region Gestores
        private GestorAprobadores Gestoraprobadores()
        {
            GestorAprobadores gestoraprobadores = new GestorAprobadores(cnnApl);
            return gestoraprobadores;
        }
        #endregion


    }
}