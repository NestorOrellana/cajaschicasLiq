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
using LogicaCajasChicas;
using System.Text;

namespace RegistroFacturasWEB.Mantenimientos
{
    public partial class Usuarios : System.Web.UI.Page
    {

        #region declaraciones

        string cnnApl = ConfigurationManager.ConnectionStrings["CnnApl"].ToString();
        string usuario;
        bool result;

        #endregion

        #region Eventos

        protected void Page_Load(object sender, EventArgs e)
        {
            usuario = Context.User.Identity.Name.ToString();
            if (!IsPostBack)
            {
                CargarUsuariosGrid();
            }
        }

        protected void btnGrabar_Click(object sender, EventArgs e)
        {
            AlmacenarUsuarios();
        }

        protected void btnLimpiar_Click(object sender, EventArgs e)
        {
            LimpiarControles();
        }

        protected void gvUsuario_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            this.gvUsuario.PageIndex = e.NewPageIndex;
            CargarUsuariosGrid();
        }

        protected void gvUsuario_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName.Equals("Editar"))
            {
                OcultarAvisos();
                CargarUsuariosEdicion(e);
            }

            if (e.CommandName.Equals("Baja"))
            {
                OcultarAvisos();
                DarBajaUsuario(e);
            }

            if (e.CommandName.Equals("Alta"))
            {
                OcultarAvisos();
                DarAltaUsuario(e);
            }

            if (e.CommandName.Equals("AgregarCentroCosto"))
            {
                OcultarAvisos();
                AgregarCentroCosto(e);
            }

            if (e.CommandName.Equals("AgregarOrdenCompra"))
            {
                OcultarAvisos();
                AgregarOrdenCompra(e);
            }

            if (e.CommandName.Equals("AsignarRol"))
            {
                OcultarAvisos();
                AsignarRol(e);
            }


            if (e.CommandName.Equals("AsignarAprobador"))
            {
                OcultarAvisos();
                AgregarAprobador(e);
            }


            if (e.CommandName.Equals("MapeaSociedadCentro"))
            {
                OcultarAvisos();
                MapearCentrosCosto(e);
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

        private void LimpiarControles()
        {
            hfIdUsuario.Value = "0";
            txtNombreUsuario.Text = string.Empty;
            txtUsuario.Text = string.Empty;
            cbAlta.Checked = false;
            lblFechaModificacionDB.Text = string.Empty;
            lblUsuarioDB.Text = string.Empty;
            txtCorreo.Text = string.Empty;
            OcultarAvisos();
        }

        private UsuarioDTO CargarObjetoUsuarios(ref UsuarioDTO _usuarioDto)
        {
            _usuarioDto.ID_USUARIO = Convert.ToInt16(hfIdUsuario.Value);
            _usuarioDto.USUARIO = txtUsuario.Text;
            _usuarioDto.NOMBRE = txtNombreUsuario.Text;
            _usuarioDto.ALTA = _usuarioDto.ID_USUARIO.Equals(0) ? (bool?)null : cbAlta.Checked;
            _usuarioDto.USUARIO_MANTENIMIENTO.USUARIO_ALTA = usuario;
            _usuarioDto.USUARIO_MANTENIMIENTO.USUARIO_MODIFICO = usuario;
            _usuarioDto.CORREO = txtCorreo.Text;

            return _usuarioDto;
        }

        private bool ValidaCampos()
        {
            if ((txtNombreUsuario.Text != "") && (txtNombreUsuario.Text.Length <= 60)) { result = true; } else result = false;
            if ((txtUsuario.Text != "") && (txtUsuario.Text.Length <= 10)) { result = true; } else result = false;
            return result;
        }

        private void AlmacenarUsuarios()
        {
            UsuarioDTO _usuarioDto = new UsuarioDTO();
            GestorSeguridad gestorSeguridad = null;

            OcultarAvisos();
            try
            {
                if (ValidaCampos())
                {
                    gestorSeguridad = GestorSeguridad();
                    CargarObjetoUsuarios(ref _usuarioDto);
                    gestorSeguridad.AlmacenarUsuario(_usuarioDto);
                    hfIdUsuario.Value = _usuarioDto.ID_USUARIO.ToString();
                    DesplegarAviso("El usuario fue almacenado correctamente.");
                    CargarUsuariosGrid();
                }
                else
                    DesplegarError("Debe Completar todos los datos correctamente");
            }
            catch (ExcepcionesDIPCMI ex)
            {
                DesplegarError(ex.Message);
            }
            catch
            {
                DesplegarError("Error al almacenar el usuario.");
            }
            finally
            {
                if (gestorSeguridad != null) gestorSeguridad.Dispose();
            }
        }

        public void CargarUsuariosGrid()
        {
            UsuarioDTO _usuarioDto = new UsuarioDTO();
            GestorSeguridad gestorSeguridad = null;
            int x = 1;
            try
            {
                gestorSeguridad = GestorSeguridad();

                gvUsuario.DataSource = (from usuario in gestorSeguridad.ListaUsuario()
                                        select new
                                        {
                                            NUMERO = x++,
                                            ID_USUARIO = usuario.ID_USUARIO,
                                            USUARIO = usuario.USUARIO,
                                            NOMBRE = usuario.NOMBRE,
                                            idAltaUser = usuario.ALTA,
                                            USUARIO_ALTA = usuario.USUARIO_MANTENIMIENTO.USUARIO_ALTA,
                                            FECHA_ALTA = usuario.USUARIO_MANTENIMIENTO.FECHA_ALTA,
                                            USUARIO_MODIFICACION = usuario.USUARIO_MANTENIMIENTO.USUARIO_MODIFICO,
                                            FECHA_MODIFICACION = usuario.USUARIO_MANTENIMIENTO.FECHA_MODIFICACION,
                                            CORREO = usuario.CORREO
                                        }).ToArray();
                gvUsuario.DataBind();
            }
            catch
            {
                DesplegarError("Error al desplegar los usuario");
            }
            finally
            {
                if (gestorSeguridad != null) gestorSeguridad.Dispose();
            }
        }

        private void CargarUsuariosEdicion(GridViewCommandEventArgs e)
        {
            int indice = Int32.Parse(e.CommandArgument.ToString());
            GridViewRow fila = gvUsuario.Rows[indice];

            hfIdUsuario.Value = HttpUtility.HtmlDecode(fila.Cells[1].Text);
            txtUsuario.Text = HttpUtility.HtmlDecode(fila.Cells[2].Text);
            txtNombreUsuario.Text = HttpUtility.HtmlDecode(fila.Cells[3].Text);
            txtCorreo.Text = HttpUtility.HtmlDecode(fila.Cells[4].Text);
            cbAlta.Checked = (((CheckBox)fila.FindControl("idAlta")).Checked);
            lblFechaModificacionDB.Text = HttpUtility.HtmlDecode(fila.Cells[9].Text);
            lblUsuarioDB.Text = HttpUtility.HtmlDecode(fila.Cells[8].Text);
        }

        private void DarBajaUsuario(GridViewCommandEventArgs e)
        {
            int indice = Int32.Parse(e.CommandArgument.ToString());
            GridViewRow fila = gvUsuario.Rows[indice];
            GestorSeguridad gestorSeguridad = null;

            try
            {
                gestorSeguridad = GestorSeguridad();
                gestorSeguridad.DarBajaUsuario(Convert.ToInt16(fila.Cells[1].Text), usuario);
                DesplegarAviso("El usuario fue dado de baja.");
                CargarUsuariosGrid();
            }
            catch
            {
                DesplegarError("Error al dar baja al usuario");
            }
            finally
            {
                if (gestorSeguridad != null) gestorSeguridad.Dispose();
            }
        }

        private void DarAltaUsuario(GridViewCommandEventArgs e)
        {
            int indice = Int32.Parse(e.CommandArgument.ToString());
            GridViewRow fila = gvUsuario.Rows[indice];
            GestorSeguridad gestorSeguridad = null;

            try
            {
                gestorSeguridad = GestorSeguridad();
                gestorSeguridad.DarAltaUsuario(Convert.ToInt16(fila.Cells[1].Text), usuario);

                DesplegarAviso("El usuario fue dado de alta.");
                CargarUsuariosGrid();
            }
            catch
            {
                DesplegarError("Error al dar alta al usuario.");
            }
            finally
            {
                if (gestorSeguridad != null) gestorSeguridad.Dispose();
            }
        }

        private void AsignarRol(GridViewCommandEventArgs e)
        {
            int indice = Int32.Parse(e.CommandArgument.ToString());
            GridViewRow fila = gvUsuario.Rows[indice];

            string usuarioP = HttpUtility.HtmlDecode(fila.Cells[2].Text);
            Int32 IdUsuario = Convert.ToInt32(fila.Cells[1].Text);

            String b64 = Convert.ToBase64String(Encoding.UTF8.GetBytes(string.Format("Usuario={0}&IdUsuario={1}", usuarioP, IdUsuario)));

            string url = string.Concat("~/Mantenimientos/UsuarioRol.aspx?", b64);
            Response.Redirect(url);
        }

        private void AgregarCentroCosto(GridViewCommandEventArgs e)
        {
            int indice = Int32.Parse(e.CommandArgument.ToString());
            GridViewRow fila = gvUsuario.Rows[indice];

            string usuarioP = HttpUtility.HtmlDecode(fila.Cells[2].Text);
            String b64 = Convert.ToBase64String(Encoding.UTF8.GetBytes(string.Format("Usuario={0}", usuarioP)));

            string url = string.Concat("~/Mantenimientos/UsuarioCentroCosto.aspx?", b64);
            Response.Redirect(url);
        }

        private void MapearCentrosCosto(GridViewCommandEventArgs e)
        {
            int indice = Int32.Parse(e.CommandArgument.ToString());
            GridViewRow fila = gvUsuario.Rows[indice];

            string usuarioP = HttpUtility.HtmlDecode(fila.Cells[2].Text);
            String b64 = Convert.ToBase64String(Encoding.UTF8.GetBytes(string.Format("Usuario={0}", usuarioP)));

            string url = string.Concat("~/Mantenimientos/UsuarioSociedadCentro.aspx?", b64);
            Response.Redirect(url);
        }

        private void AgregarOrdenCompra(GridViewCommandEventArgs e)
        {
            int indice = Int32.Parse(e.CommandArgument.ToString());
            GridViewRow fila = gvUsuario.Rows[indice];

            string usuarioP = HttpUtility.HtmlDecode(fila.Cells[2].Text);
            String b64 = Convert.ToBase64String(Encoding.UTF8.GetBytes(string.Format("Usuario={0}", usuarioP)));

            string url = string.Concat("~/Mantenimientos/UsuarioOrdenCompra.aspx?", b64);
            Response.Redirect(url);
        }

        private void AgregarAprobador(GridViewCommandEventArgs e)
        {
            int indice = Int32.Parse(e.CommandArgument.ToString());
            GridViewRow fila = gvUsuario.Rows[indice];

            string usuarioP = HttpUtility.HtmlDecode(fila.Cells[2].Text);
            Int32 IdUsuario = Convert.ToInt32(fila.Cells[1].Text);

            String b64 = Convert.ToBase64String(Encoding.UTF8.GetBytes(string.Format("Usuario={0}&IdUsuario={1}", usuarioP, IdUsuario)));

           // string url = string.Concat("~/Aprobadores/ListadoAprobadores.aspx?", b64);
            string url = string.Concat("~/Aprobadores/AprobadorCentro.aspx?", b64);
            Response.Redirect(url);
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