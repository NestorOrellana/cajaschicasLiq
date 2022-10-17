using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Configuration;
using DipCmiGT.LogicaCajasChicas.Sesion;
using DipCmiGT.LogicaCajasChicas;
using DipCmiGT.LogicaComun;
using LogicaCajasChicas;
using LogicaCajasChicas.Enum;
using System.Runtime.Remoting.Contexts;
using DipCmiGT.LogicaCajasChicas;
using LogicaCajasChicas.DTO;

namespace RegistroFacturasWEB.Seguridad
{
    /// <summary>
    /// Summary description for SincronizacionPO1
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class SincronizacionPOWS : System.Web.Services.WebService
    {
        private string _cadena = ConfigurationManager.ConnectionStrings["CnnApl"].ConnectionString;

        [WebMethod]
        public RespuestaPoDTO GetRespuestaSync(RespuestaPoDTO resputaSincro)
        {
            return null;
        }
    }
}
