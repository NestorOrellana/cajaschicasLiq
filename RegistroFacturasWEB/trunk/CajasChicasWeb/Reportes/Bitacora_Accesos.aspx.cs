using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using RegistroFacturasWEB.Util;
using System.Configuration;
using Microsoft.Reporting.WebForms;

namespace RegistroFacturasWEB.Reportes
{
    public partial class Bitacora_Accesos : System.Web.UI.Page
    {
        private string usuario = string.Empty;

        protected void Page_Load(object sender, EventArgs e)
        {
            usuario = Context.User.Identity.Name.ToString();
            if (!IsPostBack)
            {
                ReportParameter usuarioPar = new ReportParameter("UsuarioCreacion", usuario);

                ReportParameter[] Param = new ReportParameter[1];
                Param[0] = usuarioPar;

                
                this.ReportViewer1.AsyncRendering = true;
                this.ReportViewer1.ServerReport.ReportServerCredentials = new SeguridadReporteria.MyReportServerCredentials();
                this.ReportViewer1.ProcessingMode = Microsoft.Reporting.WebForms.ProcessingMode.Remote;
                this.ReportViewer1.ServerReport.ReportServerUrl = new Uri(ConfigurationManager.AppSettings["MyReportServerURL"]);
                this.ReportViewer1.ServerReport.ReportPath = "/RegistroFacturasWEB/Bitacora_Accesos";
                this.ReportViewer1.ServerReport.SetParameters(Param);
                this.ReportViewer1.ShowParameterPrompts = true;
                this.ReportViewer1.ShowToolBar = true;
            }
        }
    
    }
}