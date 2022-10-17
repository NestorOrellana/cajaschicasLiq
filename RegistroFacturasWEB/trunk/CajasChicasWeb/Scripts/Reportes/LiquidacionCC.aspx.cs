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
    public partial class LiquidacionCC : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string strUrl = Request.RawUrl;
            string strParam = strUrl.Substring(strUrl.IndexOf('?') + 1);
            string dominio = (string)Session["dominio"].ToString();

            if (!IsPostBack)
            {
                CapturarParametros(strParam);
                ReportParameter idCajachica = new ReportParameter("IdCajaChica", hfIdCajaChica.Value);
                ReportParameter Iddominio = new ReportParameter("dominio", dominio);
                ReportParameter opcion = new ReportParameter("opcion", hfOpcion.Value);

                ReportParameter[] Param = new ReportParameter[2];
                Param[0] = idCajachica;
                Param[1] = Iddominio;
                //Param[2] = opcion;

                this.ReportViewer1.AsyncRendering = true;
                this.ReportViewer1.ServerReport.ReportServerCredentials = new SeguridadReporteria.MyReportServerCredentials();
                this.ReportViewer1.ProcessingMode = Microsoft.Reporting.WebForms.ProcessingMode.Remote;
                this.ReportViewer1.ServerReport.ReportServerUrl = new Uri(ConfigurationManager.AppSettings["MyReportServerURL"]);
                this.ReportViewer1.ServerReport.ReportPath = "/IP/Intermedia/RegistroFacturasWEB/Liquidacion";
                this.ReportViewer1.ServerReport.SetParameters(Param);
                this.ReportViewer1.ShowParameterPrompts = false;
                this.ReportViewer1.ShowToolBar = true;
            }
        }


        private void CapturarParametros(string parametros)
        {
            string strParametros = Encoding.UTF8.GetString(Convert.FromBase64String(parametros));
            string[] argsParam = strParametros.Split('&');
            string[] param;
            string[] param2;

            param = argsParam[0].Split('=');
            hfIdCajaChica.Value = param[1];

            param2 = argsParam[2].Split('=');
            hfOpcion.Value = param2[1];
        }
    }
}