using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using System.Web.Security;
using System.Web.Configuration;
using System.Configuration;
using DipCmiGT.LogicaComun.Util;
using DipCmiGT.LogicaCajasChicas.Sesion;
using DipCmiGT.LogicaComun;
using DipCmiGT.LogicaCajasChicas;
using System.Net;
using System.Net.Sockets;
using System.DirectoryServices;

namespace RegistroFacturasWEB
{
    public partial class Login : System.Web.UI.Page
    {
        private string _path;
        private string _filterAttribute;


        string cnnSql = ConfigurationManager.ConnectionStrings["CnnApl"].ConnectionString;
       // public string dominio = "";
        string dominio = "";
        string clavedominio = "";
        string membership = "";
        bool newdomain = false;

        public string dominioEnviado = "";

        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {
                CargarDatos();
                CargarDominioDDL();
            }
        }

        //------------------------SATB-08.05.2017-Se agregaron dominios -------------------------------------
        public void CargarDominioDDL()
        {
            List<LlenarDDL_DTO> listaDDLDto = new List<LlenarDDL_DTO>();

            listaDDLDto.Add(new LlenarDDL_DTO() { IDENTIFICADOR = "PR", DESCRIPCION = "::Seleccione Dominio::" });
            listaDDLDto.Add(new LlenarDDL_DTO() { IDENTIFICADOR = "GTD", DESCRIPCION = "SOMOSCMI" });
            //listaDDLDto.Add(new LlenarDDL_DTO() { IDENTIFICADOR = "GT", DESCRIPCION = "GTDIPCMI" });
            //listaDDLDto.Add(new LlenarDDL_DTO() { IDENTIFICADOR = "SV", DESCRIPCION = "SVDIPCMI" });
            //listaDDLDto.Add(new LlenarDDL_DTO() { IDENTIFICADOR = "HN", DESCRIPCION = "HNDIPCMI" });
            //listaDDLDto.Add(new LlenarDDL_DTO() { IDENTIFICADOR = "CR", DESCRIPCION = "CRDIPCMI" });
            listaDDLDto.Add(new LlenarDDL_DTO() { IDENTIFICADOR = "CSI", DESCRIPCION = "CSI" }); //CSI
            listaDDLDto.Add(new LlenarDDL_DTO() { IDENTIFICADOR = "CMI", DESCRIPCION = "CMICORP" }); //CSI
            //listaDDLDto.Add(new LlenarDDL_DTO() { IDENTIFICADOR = "MM", DESCRIPCION = "MOLINOS" });

            ddlDominio.DataSource = listaDDLDto;
            ddlDominio.DataTextField = "DESCRIPCION";
            ddlDominio.DataValueField = "IDENTIFICADOR";
            ddlDominio.DataBind();
        }

        public void ddlDominio_SelectedIndexChanged(object sender, EventArgs e)
        {
            //GetGroups(ddlDominio.SelectedValue);
             SeleccionarDominio();
        }

        internal void SeleccionarDominio()
        {
            clavedominio = ddlDominio.SelectedValue.ToString();
            Session["dominio"] = ddlDominio.SelectedItem.Text;
            Session.Timeout = 500;
            switch (ddlDominio.SelectedValue)
            {
                case "GTD":
                    dominio = "@SOMOSCMI.COM";
                    membership = "LDAP";
                    _path = "LDAP://10.64.0.50";
                    newdomain = true;
                    break;
                case "GT":
                    dominio = "@GT.DIPCMI.CORP";
                    membership = "GTLDAP";
                    _path = "LDAP://DC=gt,DC=dipcmi,DC=corp";
                    break;
                case "CR":
                    dominio = "@CR.DIPCMI.CORP";
                    //membership = "CRLDAP";
                    _path = "LDAP://DC=cr,DC=dipcmi,DC=corp";
                    break;
                case "HN":
                    dominio = "@HN.DIPCMI.CORP";
                    //membership = "HNLDAP";
                    _path = "LDAP://DC=hn,DC=dipcmi,DC=corp";
                    break;
                case "SV":
                    dominio = "@SV.DIPCMI.CORP";
                    membership = "SVLDAP";
                    _path = "LDAP://DC=sv,DC=dipcmi,DC=corp";
                    break;
                case "CSI":
                    dominio = "@cmi.local";
                    membership = "CSILDAP";
                   _path =  "LDAP://172.18.1.243";//"LDAP://DC=cmi,DC=local";
                    break;
                case "MM":
                    dominio = "@cmi.local";
                    membership = "MMLDAP";
                    _path = "LDAP://srvdcsec.division.local:389/DC=division,DC=local"; //"LDAP://10.0.136.101:389";
                    break;

                case "CMI":
                    dominio = "@somoscmi";
                    membership = "CMILDAP";
                    _path = "LDAP://10.0.61.2";//"LDAP://DC=cmi,DC=local";
                    break;

                case "0":
                    break;
                default:
                    break;
            }
        }
        //-------------------FIN-SATB-08.05.2017-------------------------------------------------------------

        private void CargarDatos()
        {
            string app = Request.QueryString["app"];
            if (app == null)
                app = string.Empty;
            ViewState["app"] = app;

            string urlRetorno = Request.QueryString["ReturnUrl"];
            if (urlRetorno == null)
                urlRetorno = "";
            ViewState["ReturnUrl"] = urlRetorno;

            string usuario = Request.QueryString["Usuario"];
            if (usuario == null)
                usuario = "";

            //por regla general, todos los usuarios se crean con mayúsculas en la base de datos
            usuario = usuario.ToUpper();

            ViewState["Usuario"] = usuario;

            string clave = Request.QueryString["Clave"];
            if (clave == null)
                clave = "";
            ViewState["Clave"] = clave;


            ////

            //String b64 = Convert.ToBase64String(Encoding.UTF8.GetBytes(string.Format("dominio={0}", dominio)));
            //            urlRetorno = string.Concat(urlRetorno, "?", b64);
            //string url = string.Concat("~/RevisionFacturas/ListadoFacturas.aspx?", b64);
            /////


            if (!string.IsNullOrEmpty(usuario) && !string.IsNullOrEmpty(clave))
            {

                if (autenticar(usuario, clave, clavedominio))
                {
                    byte[] urlEnBytes = Convert.FromBase64String(urlRetorno);
                    UTF8Encoding encoder = new UTF8Encoding();

                    Response.Redirect(encoder.GetString(urlEnBytes));
                }
            }

            //HyperLink hlCambiarPass = (HyperLink)ControlLogin.FindControl("ahrefCambiarPass2");
            //hlCambiarPass.NavigateUrl = string.Format("ChangePassword.aspx?app={0}&ReturnUrl={1}", ViewState["app"], ViewState["ReturnUrl"]);
        }

        protected void lCMI_Authenticate(object sender, AuthenticateEventArgs e)
        {
            string app = ViewState["app"].ToString();
            string urlRetorno = ViewState["ReturnUrl"].ToString();
            string usuario = lCMI.UserName.ToUpper(); 
            string clave = lCMI.Password;
            GestorSeguridad gestorSeguridad = null;
            BitacoraDTO bitadoraDto = new BitacoraDTO();
            gestorSeguridad = GestorSeguridad();

            try
            {
               // e.Authenticated = Membership.Providers[membership].ValidateUser(string.Concat(lCMI.UserName, dominio), lCMI.Password); //"@CMI.CO"  //@cmi.local
               // e.Authenticated = Membership.Providers[membership].ValidateUser(string.Concat(lCMI.UserName), lCMI.Password);
               e.Authenticated = newdomain ? AuthenticateUser() : IsAuthenticated();  //Prueba usuarios
             //  e.Authenticated = true;
                if (e.Authenticated == false)
                    e.Authenticated = IsAuthenticated(); 
                lblAlert.Text = "estado " + e.Authenticated.ToString();
                if (e.Authenticated)
               // if (IsAuthenticated())
                {
                    if (!autenticar(usuario, clave, clavedominio))
                        throw new ExcepcionesDIPCMI("");
                    else
                    {
                        CargarObjetoBitacora(ref bitadoraDto);
                        gestorSeguridad.RegistrarBitacora(bitadoraDto);
                        CargarDatos();
                    }
                }else
                    throw new ExcepcionesDIPCMI("Error en la autenticación");

            }
            catch (Exception)
            {
                e.Authenticated = false;
            }
            finally
            {
                gestorSeguridad.Dispose();
            }
        }

        private bool autenticar(string usuario, string clave, string dominio)
        {
            bool autenticado = false;
            GestorSeguridad gestorSeguridad = null;

            
            try
            {
                gestorSeguridad = GestorSeguridad();

                if (gestorSeguridad.ValidarUsuario(usuario, clave, dominio))
                {
                    autenticado = true;

                    HttpCookie cookie = FormsAuthentication.GetAuthCookie(usuario, false);
                    FormsAuthenticationTicket ticket = FormsAuthentication.Decrypt(cookie.Value);

                    MachineKeySection machineKeySection = (MachineKeySection)WebConfigurationManager.OpenWebConfiguration(WebConfigurationManager.AppSettings["ConfigPath"]).GetSection("system.web/machineKey");
                    string vectorInicializacion = WebConfigurationManager.AppSettings["IV"];
                    string claveCifrada = Criptografo.Cifrar(usuario, machineKeySection.DecryptionKey, vectorInicializacion);
                    dominioEnviado = dominio;

                    FormsAuthenticationTicket newticket = new FormsAuthenticationTicket(
                                                                ticket.Version,
                                                                ticket.Name,
                                                                ticket.IssueDate,
                                                                ticket.Expiration,
                                                                ticket.IsPersistent,
                                                                claveCifrada,
                                                                ticket.CookiePath);
                    cookie.Value = FormsAuthentication.Encrypt(newticket);

                    /////
                    //Uri uri = new Uri(FormsAuthentication.GetRedirectUrl(ticket.Name, ticket.IsPersistent));
                    //cookie.Domain = "GTDIPCMI"; //uri.Authority;
                    //cookie.Expires = ticket.Expiration;
                    //////


                    Response.Cookies.Set(cookie);
                }
            }
            finally
            {
                gestorSeguridad.Dispose();
            }

            return autenticado;
        }

        private void CargarObjetoBitacora(ref BitacoraDTO bitacoraDto)
        {
            bitacoraDto.USUARIO = lCMI.UserName.ToString();
            bitacoraDto.ID_DOMINIO = ddlDominio.SelectedValue;
            bitacoraDto.IP = this.Request.UserHostAddress;
            bitacoraDto.FECHA_INGRESO = DateTime.Now.ToString("yyyy/MM/dd");
            bitacoraDto.HORA_INGRESO = DateTime.Now.ToString("HH:mm:ss");
            return;
        }


        private GestorSeguridad GestorSeguridad()
        {
            GestorSeguridad gs = new GestorSeguridad(cnnSql);
            return gs;
        }

        public bool AuthenticateUser()
        {
            try
            {
                if (dominio == "")
                    SeleccionarDominio();

                string domainUsername = lCMI.UserName + dominio;
                DirectoryEntry entry = new DirectoryEntry(_path);
                DirectorySearcher search = new DirectorySearcher(entry);

                string attributeName = "userprincipalname";
                search.Filter = string.Format("(&(objectcategory=User)(objectClass=person)({0}={1}))", attributeName, domainUsername);
                SearchResult result = search.FindOne();

                if (result != null)
                {
                    string userAccountName = result.Properties["samaccountname"][0].ToString();
                    string domainUserAccountName = ddlDominio.SelectedItem.Text + @"\" + userAccountName;
                    DirectoryEntry accountEntry = new DirectoryEntry(_path, domainUserAccountName, lCMI.Password);
                    Object obj = accountEntry.NativeObject;
                    DirectorySearcher searchAccount = new DirectorySearcher(accountEntry);

                    search.Filter = "(SAMAccountName=" + userAccountName + ")";
                    search.PropertiesToLoad.Add("cn");
                    SearchResult searchResult = search.FindOne();

                    if (searchResult == null)
                    {
                        return false;
                    }

                    _filterAttribute = (string)searchResult.Properties["cn"][0];
                }
                else
                    return false;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al Autenticarse " + ex.Message);
            }

            return true;
        }

        public bool IsAuthenticated()
        {

            try
            {
                if (dominio == "")
                    SeleccionarDominio();

                string domainAndUsername = ddlDominio.SelectedItem.Text + @"\" + lCMI.UserName;
                DirectoryEntry entry = new DirectoryEntry(_path, domainAndUsername, lCMI.Password);

                object obj = entry.NativeObject;

                DirectorySearcher search = new DirectorySearcher(entry);

                search.Filter = "(SAMAccountName=" + lCMI.UserName + ")";
                search.PropertiesToLoad.Add("cn");
                SearchResult result = search.FindOne();

                if (null == result)
                {
                    return false;
                }

                ////Update the new path to the user in the directory.
                //_path = result.Path;
                //string X = _path;
                
                //string toFind1 = "DC=";
                //int start = _path.IndexOf(toFind1) + toFind1.Length;
                //int final = start + 2; 
                //string texts = _path.Substring(start, 2);
                //switch (ddlDominio.SelectedValue)
                //{
                //    case "hn":
                //        _path = _path.Remove(start, Math.Min(start, 2)).Insert(start, "hn");
                //        break;
                //    case "gt":
                //        _path = _path.Remove(start, Math.Min(start, 2)).Insert(start, "gt");

                //        break;
                //}


                _filterAttribute = (string)result.Properties["cn"][0];
            }
            catch (Exception ex)
            {
                throw new Exception("Error al Autenticarse " + ex.Message);
            }

            return true;
        }
    
    
    }


}
