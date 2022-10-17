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
using DipCmiGT.LogicaComun.Enum;

namespace RegistroFacturasWEB.Seguridad
{
    public partial class UsuarioRol : System.Web.UI.Page
    {
        #region Declaracion
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
                hfUrl.Value = Request.UrlReferrer.ToString();
                txtUsuario.Enabled = false;
                if (!strUrl.Contains('?'))
                    Response.Redirect("~/principal.aspx");

                if (!string.IsNullOrEmpty(strParam))
                {
                    CapturarParametros(strParam);

                    txtUsuario.Enabled = false;
                    CargarRolesUsuario();
                    //CargarUsuarioRolGrid();
                    OcultarAvisos();
                }
                else
                    Response.Redirect("~/principal.aspx");
            }

        }

        protected void btnLimpiar_Click(object sender, EventArgs e)
        {
            LimpiarControles();
        }

        protected void btnGrabar_Click(object sender, EventArgs e)
        {
            AlmacenaUsuarioRol();
        }

        //protected void gvUsuarioRol_RowCommand(object sender, GridViewCommandEventArgs e)
        //{
        //    if (e.CommandName.Equals("Editar"))
        //    {
        //        OcultarAvisos();
        //        CargarUsuarioRolEdicion(e);
        //    }
        //    if (e.CommandName.Equals("Baja"))
        //    {
        //        OcultarAvisos();
        //        DarBajaUsuarioRol(e);
        //    }
        //    if (e.CommandName.Equals("Alta"))
        //    {
        //        OcultarAvisos();
        //        DarAltaUsuarioRol(e);
        //    }

        //}

        //protected void gvRol_PageIndexChanging(object sender, GridViewPageEventArgs e)
        //{
        //    this.gvRol.PageIndex = e.NewPageIndex;
        //    CargarRolGrid();
        //}

        //protected void gvUsuarioRol_PageIndexChanging(object sender, GridViewPageEventArgs e)
        //{
        //    this.gvUsuarioRol.PageIndex = e.NewPageIndex;
        //    CargarUsuarioRolGrid();
        //}
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
                txtUsuario.Text = param[1];

                param = argsParam[1].Split('=');
                hfIdUsuario.Value = param[1];
                return;
            }
        }

        private void AlmacenaUsuarioRol()
        {
            decimal key = 0;
            List<UsuarioRolDTO> _listaUsuarioRolDto = new List<UsuarioRolDTO>();
            GestorSeguridad gestorseguridad = null;
            Dictionary<decimal, bool> usuarioRol = new Dictionary<decimal, bool>();

            try
            {
                gestorseguridad = GestorSeguridad();

                for (int i = 0; i < cblRoles.Items.Count; i++)
                {
                    if (cblRoles.Items[i].Selected == true)
                    {
                        key = Convert.ToDecimal(cblRoles.Items[i].Value);
                        usuarioRol[key] = cblRoles.Items[i].Selected;
                    }
                }


                _listaUsuarioRolDto = (from id in usuarioRol.Keys
                                       where usuarioRol[id]
                                       select new UsuarioRolDTO
                                       {
                                           ID_USUARIO = Convert.ToInt32(hfIdUsuario.Value),
                                           ID_ROL = (short)id,
                                           ALTA = Convert.ToBoolean(EstadoEnum.ALTA)
                                       }).ToList();

                gestorseguridad.AlmacenarUsuarioRol(_listaUsuarioRolDto, usuario, Convert.ToInt32(hfIdUsuario.Value));
                CargarRolesUsuario();

                DesplegarAviso("Los Roles por usuario fueron almacenados correctamente");

            }
            catch (ExcepcionesDIPCMI ex)
            {
                DesplegarError(ex.Message);
            }
            catch
            {
                DesplegarError("Error al almacenar rol por usuario");
            }
            finally
            {
                if (gestorseguridad != null) gestorseguridad.Dispose();
            }
        }

        public void CargarRolesUsuario()
        {
            UsuarioRolDTO _usuariorolDto = new UsuarioRolDTO();
            GestorSeguridad gestorseguridad = null;
            List<RolDTO> _listaRol = null;
            List<UsuarioRolDTO> _listaUsuarioRolDto = null;
            Dictionary<decimal, bool> usuarioRol = new Dictionary<decimal, bool>();
            decimal key = 0;


            try
            {
                gestorseguridad = GestorSeguridad();
                _listaRol = gestorseguridad.ListaRol();
                _listaUsuarioRolDto = gestorseguridad.ListaUsuarioRol(Convert.ToInt32(hfIdUsuario.Value));

                cblRoles.DataValueField = "ID_ROL";
                cblRoles.DataTextField = "NOMBRE_ROL";
                cblRoles.DataSource = _listaRol;
                cblRoles.DataBind();

                foreach (UsuarioRolDTO rolDto in _listaUsuarioRolDto)
                    usuarioRol[rolDto.ID_ROL] = _listaUsuarioRolDto.Find(u => u.ID_ROL == rolDto.ID_ROL) == null ? false : true;

                if (_listaUsuarioRolDto.Count > 0)
                {
                    for (int i = 0; i < cblRoles.Items.Count; i++)
                    {
                        foreach (KeyValuePair<decimal, bool> entry in usuarioRol)
                        {
                            if (Convert.ToDecimal(cblRoles.Items[i].Value) == entry.Key)
                            {
                                key = Convert.ToDecimal(cblRoles.Items[i].Value);
                                cblRoles.Items[i].Selected = usuarioRol[key];
                            }
                        }
                    }
                }
            }
            catch
            {
                DesplegarError("Error al Desplegar Roles");
            }
            finally
            {
                if (gestorseguridad != null) gestorseguridad.Dispose();
            }


        }

        //public void CargarUsuarioRolGrid()
        //{
        //    decimal key;
        //    UsuarioRolDTO _usuariorolDto = new UsuarioRolDTO();
        //    GestorSeguridad gestorseguridad = null;
        //    List<UsuarioRolDTO> _listaUsuarioRolDto = gestorseguridad.ListaUsuarioRol(Convert.ToInt32(hfIdUsuario.Value));

        //    int x = 1;
        //    try
        //    {
        //        gestorseguridad = GestorSeguridad();
        //        gvUsuarioRol.DataSource = (from usuariorol in _listaUsuarioRolDto
        //                                   select new
        //                                   {
        //                                       NUMERO = x++,
        //                                       USUARIO = usuariorol.ID_USUARIO,
        //                                       NOMBRE_USER = usuariorol.NOMBRE_USUARIO,
        //                                       ROL = usuariorol.ID_ROL,
        //                                       NOMBRE_ROL = usuariorol.NOMBRE_ROL,
        //                                       idAltaRol = usuariorol.ALTA,
        //                                       ID_USUARIOALTA = usuariorol.USUARIO_MANTENIMIENTO.USUARIO_ALTA,
        //                                       FECHA_ALTA = usuariorol.USUARIO_MANTENIMIENTO.FECHA_ALTA,
        //                                       ID_USUARIOMODIFICACION = usuariorol.USUARIO_MANTENIMIENTO.USUARIO_MODIFICO,
        //                                       FECHA_MODIFICACION = usuariorol.USUARIO_MANTENIMIENTO.FECHA_MODIFICACION
        //                                   }).ToArray();

        //        gvUsuarioRol.DataBind();


        //    }
        //    catch
        //    {
        //        DesplegarError("Error al Desplegar Roles por Usuario");
        //    }
        //    finally
        //    {
        //        if (gestorseguridad != null) gestorseguridad.Dispose();
        //    }
        //}

        private UsuarioRolDTO CargarObjetoUsuarioRol(ref UsuarioRolDTO _usuariorolDto, string idRol)
        {
            // _usuariorolDto.ID_USUARIO = Convert.ToInt16(ddlUsuario.SelectedValue);
            _usuariorolDto.ID_USUARIO = Convert.ToInt32(hfIdUsuario.Value);
            _usuariorolDto.ID_ROL = Convert.ToInt16(idRol);
            //_usuariorolDto.ALTA = cbAlta.Checked;
            _usuariorolDto.USUARIO_MANTENIMIENTO.USUARIO_ALTA = usuario;
            _usuariorolDto.USUARIO_MANTENIMIENTO.USUARIO_MODIFICO = usuario;
            return _usuariorolDto;
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
            CargarRolesUsuario();
            OcultarAvisos();
        }

        //private void DarBajaUsuarioRol(GridViewCommandEventArgs e)
        //{
        //    int indice = Int32.Parse(e.CommandArgument.ToString());
        //    GridViewRow fila = gvUsuarioRol.Rows[indice];
        //    GestorSeguridad gestorseguridad = null;

        //    try
        //    {
        //        gestorseguridad = GestorSeguridad();
        //        gestorseguridad.DarBajaUsuarioRol(Convert.ToInt16(fila.Cells[1].Text), Convert.ToInt16(fila.Cells[3].Text), usuario);

        //        DesplegarAviso("El rol del usuario fue dado de baja");
        //        CargarUsuarioRolGrid();
        //    }
        //    catch
        //    {
        //        DesplegarError("Error al dar de baja el rol para este usuario");
        //    }
        //    finally
        //    {
        //        if (gestorseguridad != null) gestorseguridad.Dispose();
        //    }
        //}

        //public void DarAltaUsuarioRol(GridViewCommandEventArgs e)
        //{
        //    int indice = Int32.Parse(e.CommandArgument.ToString());
        //    GridViewRow fila = gvUsuarioRol.Rows[indice];
        //    GestorSeguridad gestorseguridad = null;

        //    try
        //    {
        //        gestorseguridad = GestorSeguridad();
        //        gestorseguridad.DarAltaUsuarioRol(Convert.ToInt16(fila.Cells[1].Text), Convert.ToInt16(fila.Cells[3].Text), usuario);

        //        DesplegarAviso("El rol del usuario fue dado de alta");
        //        CargarUsuarioRolGrid();
        //    }
        //    catch
        //    {
        //        DesplegarError("Error al dar de alta el rol para este usuario");
        //    }
        //    finally
        //    {
        //        if (gestorseguridad != null) gestorseguridad.Dispose();
        //    }
        //}

        #endregion

        #region Gestores

        protected GestorSeguridad GestorSeguridad()
        {
            GestorSeguridad gesetorseguridad = new GestorSeguridad(cnnApl);
            return gesetorseguridad;
        }

        #endregion
    }
}