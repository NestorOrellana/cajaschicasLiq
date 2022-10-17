using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;
using System.Web.Configuration;
using System.Configuration;
using DipCmiGT.LogicaComun.Util;
using System.Runtime.Remoting.Contexts;
using System.Text;

namespace RegistroFacturasWEB
{
    public partial class Base : System.Web.UI.MasterPage
    {

        public string Password
        {
            get
            {
                HttpCookie cookie = Request.Cookies[".DIPCMIFORMSAUTH"];
                FormsAuthenticationTicket ticket = FormsAuthentication.Decrypt(cookie.Value);
                MachineKeySection machineKeySection = (MachineKeySection)WebConfigurationManager.OpenWebConfiguration(ConfigurationManager.AppSettings["ConfigPath"]).GetSection("system.web/machineKey");
                string vectorInicializacion = WebConfigurationManager.AppSettings["IV"];

                return Criptografo.Decifrar(ticket.UserData, machineKeySection.DecryptionKey, vectorInicializacion);
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            
            lblUsuarioSistema.Text = "Usuario: "  + Context.User.Identity.Name.ToString();
           
        }

    }
}