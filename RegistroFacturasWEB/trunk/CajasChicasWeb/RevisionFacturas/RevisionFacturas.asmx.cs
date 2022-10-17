using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using LogicaCajasChicas;
using DipCmiGT.LogicaCajasChicas.Sesion;
using System.Configuration;
using DipCmiGT.LogicaComun;

namespace RegistroFacturasWEB.RevisionFacturas
{
    /// <summary>
    /// Summary description for RevisionFacturas
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    [System.Web.Script.Services.ScriptService]
    public class RevisionFacturas : System.Web.Services.WebService
    {

        private string _cadena = ConfigurationManager.ConnectionStrings["CnnApl"].ConnectionString;
        private string usuario;

        [WebMethod]
        public List<RegistroContableSPDTO> BuscarRegistroContableSP(decimal idFactura)
        {
            GestorCajaChica gestorCC = null;

            try
            {
                gestorCC = GestorCajaChica();
                return gestorCC.BuscarRegistroContableSP(idFactura);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                if (gestorCC != null) gestorCC.Dispose();
            }
        }

        [WebMethod]
        public List<LlenarDDL_DTO> ListarUsuarioSociedadCentro(string codigoSociedad)
        {
            GestorSociedad gestorSociedad = null;

            try
            {
                usuario = Context.User.Identity.Name.ToString();
                gestorSociedad = GestorSociedad();
                return gestorSociedad.ListarCentrosMapeados(codigoSociedad, usuario);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                if (gestorSociedad != null) gestorSociedad.Dispose();
            }
        }

        private GestorCajaChica GestorCajaChica()
        {
            GestorCajaChica gestorCajaChica = new GestorCajaChica(_cadena);
            return gestorCajaChica;
        }

        private GestorSociedad GestorSociedad()
        {
            GestorSociedad GestorSociedad = new GestorSociedad(_cadena);
            return GestorSociedad;
        }
    }
}
