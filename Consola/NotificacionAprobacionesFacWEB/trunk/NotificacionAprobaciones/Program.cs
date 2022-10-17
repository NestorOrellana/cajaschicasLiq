using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Net.Mail;
using System.Net;
using DipCmiGT.LogicaCajasChicas.Sesion;
using System.IO;
using LogicaCajasChicas;

namespace NotificacionAprobaciones
{
    class Program
    {
        static string cnn = ConfigurationManager.ConnectionStrings["cnnApl"].ToString();
        static string smptMail = ConfigurationManager.AppSettings["stmp"].ToString();
        static string CorreoAplicacion = ConfigurationManager.AppSettings["CorreoAplicacion"].ToString();
        static string CorreoNombre = ConfigurationManager.AppSettings["CorreoNombre"].ToString();
        static string AsuntoCorreo = ConfigurationManager.AppSettings["AsuntoCorreo"].ToString();
        static string LinkAprobacion = ConfigurationManager.AppSettings["LinkAprobacion"].ToString();
        static string CuentaNombre = ConfigurationManager.AppSettings["CuentaNombre"].ToString();
        static string CuentaPassword = ConfigurationManager.AppSettings["CuentaPassword"].ToString();

        static void Main(string[] args)
        {
            List<DatosCorreoDTO> _listaDatosCorreo = null;
            GestorAprobadores gAprobadores = null;

            try
            {

                gAprobadores = GestorAprobadores();

                _listaDatosCorreo = gAprobadores.BuscarAprobaciones();

                foreach (DatosCorreoDTO dcorreoDto in _listaDatosCorreo)
                {
                    EnviarCorreo(dcorreoDto.CORREO, dcorreoDto.NOMBRE, dcorreoDto.FACTURAS, dcorreoDto.FACT_DIV);
                }

            }
            finally
            {
                if (gAprobadores != null) gAprobadores.Dispose();
            }
        }

        private static string CuerpoCorreo(string nombreUsuario, Int32 numeroFacturas, Int32 numFactDiv)
        {

            string cuerpoCorreo = string.Empty;
            string facturas_divididas = "";

            if (numFactDiv > 0)
                facturas_divididas = "De las cuales " + numFactDiv + " estan divididas en diferentes Centros de Costo. ";

            cuerpoCorreo = string.Format("Estimado(a): {0}" + Environment.NewLine +
                                         "Se requiere de la aprobación de {1} facturas. " + Environment.NewLine +
                                         facturas_divididas + Environment.NewLine + 
                                         "El link de la pantalla de aprobaciones es: " + Environment.NewLine +
                                         "{2}" + Environment.NewLine + Environment.NewLine +
                                         "Saludos cordiales. ", nombreUsuario, numeroFacturas.ToString(), LinkAprobacion);

            return cuerpoCorreo;
        }

        public static void EnviarCorreo(string correoAprobador, string nombreUsuario, Int32 numeroFacturas, Int32 numFactDiv)
        {
            MailMessage correo = new MailMessage();
            SmtpClient smtp = new SmtpClient(smptMail);

            correo.From = new MailAddress(CorreoAplicacion, CorreoNombre);
            correo.To.Add(correoAprobador);
            correo.Subject = AsuntoCorreo;
            correo.Body = CuerpoCorreo(nombreUsuario, numeroFacturas, numFactDiv);
            correo.IsBodyHtml = false;
            correo.Priority = MailPriority.Normal;
            smtp.Credentials = new NetworkCredential(CuentaNombre, CuentaPassword);

            try
            {
                smtp.Send(correo);
                File.AppendAllText("Error_MailAprobaciones.log", string.Format("El correo se envió sin ningun problema a {0}." + DateTime.Now + Environment.NewLine, nombreUsuario));
            }
            catch (Exception ex)
            {
                File.AppendAllText("Error_MailAprobaciones.log", string.Format("Error al enviar correo a {0}. Mensaje: " + DateTime.Now + ex.Message + Environment.NewLine, nombreUsuario + ex.ToString()));
            }

        }

        private static GestorAprobadores GestorAprobadores()
        {
            GestorAprobadores ga = new GestorAprobadores(cnn);
            return ga;
        }
    }
}
