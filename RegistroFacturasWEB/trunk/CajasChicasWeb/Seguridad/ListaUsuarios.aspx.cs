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

namespace RegistroFacturasWEB.Seguridad
{
    public partial class ListaUsuarios : System.Web.UI.Page
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
                CargarGrid();
        }

        protected void gvListaUsuarios_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            this.gvListaUsuarios.PageIndex = e.NewPageIndex;
            CargarGrid();
        }

        protected void gvListaUsuarios_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName.Equals("AgregarCentroCosto"))
                AgregarCentroCosto(e);

            if (e.CommandName.Equals("AgregarOrdenCompra"))
                AgregarOrdenCompra(e);

            if (e.CommandName.Equals("AsignarRol"))
                AsignarRol(e);
        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            if (txtUsuario.Text != "")
            {
             //   BuscarUsuario(txtUsuario.Text);
            }
            else
                DesplegarAviso("Debe Ingresar Ususario para realizar la Busqueda");
        }
        #endregion

        #region Metodos
        //private void BuscarUsuario(string usuario)
        //{
        //    UsuarioDTO _usuarioDto = new UsuarioDTO();
        //    GestorSeguridad gestorSeguridad = null;

        //    int x = 1;
        //    try
        //    {
        //        gestorSeguridad = GestorSeguridad();
        //        gvListaUsuarios.DataSource = (from lista in gestorSeguridad.BuscarUsuario(usuario)
        //                                      select new
        //                                    {
        //                                   Numero = x++,
        //                                   ID_USUARIO = lista.ID_USUARIO,
        //                                   USUARIO = lista.USUARIO,
        //                                   NOMBRE = lista.NOMBRE
        //                               }).ToArray();
        //        gvListaUsuarios.DataBind();
        //    }
        //    catch
        //    {
        //        DesplegarError("Error al Desplegar los Usuarios");
        //    }
        //    finally
        //    {
        //        if (gestorSeguridad != null) gestorSeguridad.Dispose();
        //    }
        //}

        private void AsignarRol(GridViewCommandEventArgs e)
        {
            int indice = Int32.Parse(e.CommandArgument.ToString());
            GridViewRow fila = gvListaUsuarios.Rows[indice];

            string usuario = HttpUtility.HtmlDecode(fila.Cells[2].Text);
            Int32 IdUsuario = Convert.ToInt32(fila.Cells[1].Text);

            String b64 = Convert.ToBase64String(Encoding.UTF8.GetBytes(string.Format("Usuario={0}&IdUsuario={1}", usuario, IdUsuario)));

            string url = string.Concat("~/Seguridad/UsuarioRol.aspx?", b64);
            Response.Redirect(url);
        }
        private void AgregarCentroCosto(GridViewCommandEventArgs e)
        {
            int indice = Int32.Parse(e.CommandArgument.ToString());
            GridViewRow fila = gvListaUsuarios.Rows[indice];

            string usuario = HttpUtility.HtmlDecode(fila.Cells[2].Text);
            String b64 = Convert.ToBase64String(Encoding.UTF8.GetBytes(string.Format("Usuario={0}", usuario)));

            string url = string.Concat("~/Seguridad/UsuarioCentroCosto.aspx?", b64);
            Response.Redirect(url);
        }

        private void AgregarOrdenCompra(GridViewCommandEventArgs e)
        {
            int indice = Int32.Parse(e.CommandArgument.ToString());
            GridViewRow fila = gvListaUsuarios.Rows[indice];

            string usuario = HttpUtility.HtmlDecode(fila.Cells[2].Text);
            String b64 = Convert.ToBase64String(Encoding.UTF8.GetBytes(string.Format("Usuario={0}", usuario))); 

            string url = string.Concat("~/Seguridad/UsuarioOrdenCompra.aspx?", b64);
            Response.Redirect(url);
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

        public void CargarGrid()
        {
            UsuarioDTO _usuarioDto = new UsuarioDTO();
            GestorSeguridad gestorSeguridad = null;

            int x = 1;
            try
            {
                gestorSeguridad = GestorSeguridad();
                gvListaUsuarios.DataSource = (from lista in gestorSeguridad.ListaUsuario()
                                              select new
                                            {
                                           Numero = x++,
                                           ID_USUARIO = lista.ID_USUARIO,
                                           USUARIO = lista.USUARIO,
                                           NOMBRE = lista.NOMBRE
                                       }).ToArray();
                gvListaUsuarios.DataBind();
            }
            catch
            {
                DesplegarError("Error al Desplegar los Usuarios");
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