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

namespace RegistroFacturasWEB.RegistroFacturas.Reportes
{
    public partial class LiquidacionCC : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string strUrl = Request.RawUrl;
            string strParam = strUrl.Substring(strUrl.IndexOf('?') + 1);

            if (!IsPostBack)
            {
                CapturarParametros(strParam);
                ReportParameter idCajachica = new ReportParameter("IdCajaChica", hfIdCajaChica.Value);

                ReportParameter[] Param = new ReportParameter[1];
                Param[0] = idCajachica;

                this.ReportViewer1.AsyncRendering = false;
                this.ReportViewer1.ServerReport.ReportServerCredentials = new SeguridadReporteria.MyReportServerCredentials();
                this.ReportViewer1.ProcessingMode = Microsoft.Reporting.WebForms.ProcessingMode.Remote;
                this.ReportViewer1.ServerReport.ReportServerUrl = new Uri(ConfigurationManager.AppSettings["MyReportServerURL"]);
                this.ReportViewer1.ServerReport.ReportPath = "/RegistroFacturasWEB/Liquidacion";
                this.ReportViewer1.ServerReport.SetParameters(Param);

                this.ReportViewer1.ShowParameterPrompts = false;
            }
        }


        private void CapturarParametros(string parametros)
        {
            string strParametros = Encoding.UTF8.GetString(Convert.FromBase64String(parametros));
            string[] argsParam = strParametros.Split('&');
            string[] param;

            param = argsParam[0].Split('=');
            hfIdCajaChica.Value = param[1];
        }
    }
}