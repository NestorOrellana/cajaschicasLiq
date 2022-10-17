using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.Reporting.WebForms;
using RegistroFacturasWEB.Util;
using System.Configuration;

namespace RegistroFacturasWEB.Reportes
{
    public partial class DocumentosIneldat : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                this.ReportViewer1.AsyncRendering = true;
                this.ReportViewer1.ServerReport.ReportServerCredentials = new SeguridadReporteria.MyReportServerCredentials();
                this.ReportViewer1.ProcessingMode = Microsoft.Reporting.WebForms.ProcessingMode.Remote;
                this.ReportViewer1.ServerReport.ReportServerUrl = new Uri(ConfigurationManager.AppSettings["MyReportServerURL"]);
                this.ReportViewer1.ServerReport.ReportPath = "/IP/Intermedia/RegistroFacturasWEB/DocumentosIneldat";
                this.ReportViewer1.ShowParameterPrompts = true;
                this.ReportViewer1.ShowToolBar = true;
            }
        }
    }
}