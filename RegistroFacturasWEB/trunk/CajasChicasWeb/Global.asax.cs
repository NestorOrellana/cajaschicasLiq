using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;
using DipCmiGT.LogicaCajasChicas.Sesion;
using System.Configuration;

namespace RegistroFacturasWEB
{
    public class Global : System.Web.HttpApplication
    {
        private string cnn = ConfigurationManager.ConnectionStrings["CnnApl"].ToString();
        string dominio = "";

        protected void Application_Start(object sender, EventArgs e)
        {

        }

        protected void Session_Start(object sender, EventArgs e)
        {
            Session["dominio"] = dominio;
        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {

        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {
            if (!(HttpContext.Current.User == null))
            {
                if (HttpContext.Current.User.Identity.AuthenticationType == "Forms")
                {
                    FormsAuthenticationTicket tkt;

                    tkt = FormsAuthentication.Decrypt(Request.Cookies[FormsAuthentication.FormsCookieName].Value);

                    if (string.IsNullOrEmpty(tkt.Name))
                        return;

                    String[] usuario = { "" };

                    using (GestorSeguridad gs = new GestorSeguridad(cnn))
                    {
                        usuario = gs.BuscarRolUsuario(tkt.Name);
                    }

                    //En este momento asignamos el rol que le ha correspondido
                    System.Web.Security.FormsIdentity id;
                    id = (System.Web.Security.FormsIdentity)HttpContext.Current.User.Identity;
                    HttpContext.Current.User = new System.Security.Principal.GenericPrincipal(id, usuario);
                }
            }
        }

        protected void Application_Error(object sender, EventArgs e)
        {

        }

        protected void Session_End(object sender, EventArgs e)
        {

        }

        protected void Application_End(object sender, EventArgs e)
        {

        }
    }
}