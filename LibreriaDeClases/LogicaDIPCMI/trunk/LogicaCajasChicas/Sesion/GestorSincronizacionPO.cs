using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using DipCmiGT.LogicaComun;
using LogicaCajasChicas.Entidad;
using LogicaCajasChicas.DTO;

namespace DipCmiGT.LogicaCajasChicas.Sesion
{
    public class GestorSincronizacionPO : IDisposable
    {
         #region Declaracion
        SqlConnection cnnSql = null;
        Kilometraje _kilometraje;
        #endregion

        #region Constructor
        public GestorSincronizacionPO(string conexion)
        {
            cnnSql = new SqlConnection(conexion);
        }
        # endregion

        #region Metodos
        public RespuestaPoDTO ActualizarLog(RespuestaPoDTO respuestaSincro)
        {
            Int32 x = 0;
            if (_kilometraje == null) _kilometraje = new Kilometraje(cnnSql);

            try
            {
                cnnSql.Open();

                /*if (respuestaSincro.CodigoKilometraje.ToString() == "" || respuestaSincro.CodigoKilometraje.ToString() == "0")
                {
                    x = _kilometraje.EjecutarSentenciaInsert(respuestaSincro);
                }
                else
                    x = _kilometraje.EjectuarSentenciaUpdate(respuestaSincro);*/
            }
            finally
            {
                if (cnnSql.State == ConnectionState.Open) cnnSql.Close();
            }

            return respuestaSincro;
        }

        public Int16 DarBajaKilometraje(long codigoKilometraje, string usuario)
        {
            if (_kilometraje == null) _kilometraje = new Kilometraje(cnnSql);

            try
            {
                cnnSql.Open();

                return _kilometraje.DarBajaKilometraje(codigoKilometraje, usuario);
            }
            finally
            {
                if (cnnSql.State != ConnectionState.Closed) cnnSql.Close();
            }
        }
        #endregion

        #region IDisposable Members

        private void DisposeConnSql()
        {
            if (cnnSql != null)
            {
                if (cnnSql.State != ConnectionState.Closed)
                    cnnSql.Close();
            }
        }

        public void Dispose()
        {
            try
            {
                DisposeConnSql();
            }
            catch
            {
                GC.SuppressFinalize(this);
            }
        }
        #endregion
    }
}
