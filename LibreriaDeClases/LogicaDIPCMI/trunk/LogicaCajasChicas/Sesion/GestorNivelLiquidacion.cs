using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using DipCmiGT.LogicaComun;
using LogicaCajasChicas.Entidad;

namespace DipCmiGT.LogicaCajasChicas.Sesion
{
    public class GestorNivelLiquidacion : IDisposable
    {
         #region Declaracion
        SqlConnection cnnSql = null;
        NivelLiquidacion _nivelLiquidacion;
        #endregion

        #region Constructor
        public GestorNivelLiquidacion(string conexion)
        {
            cnnSql = new SqlConnection(conexion);
        }
        # endregion

        #region Metodos
        public NivelLiquidacionDTO AlmacenarNivelLiquidacion(NivelLiquidacionDTO nivelDTO)
        {
            Int32 x = 0;
            if (_nivelLiquidacion == null) _nivelLiquidacion = new NivelLiquidacion(cnnSql);

            try
            {
                cnnSql.Open();

                if (nivelDTO.CodigoNivel.ToString() == "" || nivelDTO.CodigoNivel.ToString() == "0")
                {
                    x = _nivelLiquidacion.EjecutarSentenciaInsert(nivelDTO);
                }
                else
                    x = _nivelLiquidacion.EjectuarSentenciaUpdate(nivelDTO);
            }
            finally
            {
                if (cnnSql.State == ConnectionState.Open) cnnSql.Close();
            }

            return nivelDTO;
        }

        public Int16 DarBajaNivel(int codigoNivel, string usuario)
        {
            if (_nivelLiquidacion == null) _nivelLiquidacion = new NivelLiquidacion(cnnSql);

            try
            {
                cnnSql.Open();

                return _nivelLiquidacion.DarBajaNivel(codigoNivel, usuario);
            }
            finally
            {
                if (cnnSql.State != ConnectionState.Closed) cnnSql.Close();
            }
        }

        public Int16 DarAltaNivel(int codigoNivel, string usuario)
        {
            if (_nivelLiquidacion == null) _nivelLiquidacion = new NivelLiquidacion(cnnSql);

            try
            {
                cnnSql.Open();

                return _nivelLiquidacion.DarAltaNivel(codigoNivel, usuario);
            }
            finally
            {
                if (cnnSql.State != ConnectionState.Closed) cnnSql.Close();
            }
        }

        public List<NivelLiquidacionDTO> ListaNivelesLiquidacion()
        {
            if (_nivelLiquidacion == null) _nivelLiquidacion = new NivelLiquidacion(cnnSql);
            List<NivelLiquidacionDTO> _nivelDTO = new List<NivelLiquidacionDTO>();

            try
            {
                cnnSql.Open();
                return _nivelLiquidacion.ListaNiveles();
            }
            finally
            {
                if (cnnSql.State != ConnectionState.Closed) cnnSql.Close();
            }
        }

        public List<LlenarDDL_DTO> ListaPaisesDDL()
        {
            if (_nivelLiquidacion == null) _nivelLiquidacion = new NivelLiquidacion(cnnSql);

            try
            {
                cnnSql.Open();
                return _nivelLiquidacion.ListaPaisesDDL();
            }
            finally
            {
                if (cnnSql.State != ConnectionState.Closed) cnnSql.Close();
            }
        }

        public bool ValidaExisteNivel(string pais, string nivel)
        {
            if (_nivelLiquidacion == null) _nivelLiquidacion = new NivelLiquidacion(cnnSql);

            try
            {
                cnnSql.Open();
                return _nivelLiquidacion.ExisteNivel(pais, nivel);
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
