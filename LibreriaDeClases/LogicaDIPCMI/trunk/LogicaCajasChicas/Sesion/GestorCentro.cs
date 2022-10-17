using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using LogicaComun.Enum;
using DipCmiGT.LogicaCajasChicas.Entidad;
using DipCmiGT.LogicaComun;
using DipCmiGT.LogicaComun.Util;

namespace DipCmiGT.LogicaCajasChicas.Sesion
{
    public class GestorCentro : IDisposable
    {

        #region Delcaraciones
        Centro _centro = null;
        SqlConnection cnnSql = null;
        #endregion

        #region Constructor

        public GestorCentro(string conexion)
        {
            cnnSql = new SqlConnection(conexion);
        }
        #endregion

        #region Centro

        public CentroDTO EjecutarSentenciaSelect(Int32 IdCentro)
        {
            if (_centro == null) _centro = new Centro(cnnSql);
            try
            {
                cnnSql.Open();
                return _centro.EjecutarSentenciaSelect(IdCentro);
            }
            finally
            {
                if (cnnSql.State == ConnectionState.Open) cnnSql.Close();
            }
        }

        public CentroDTO AlmacenarCentro(CentroDTO centroDTO)
        {
            Int32 x = 0;
            if (_centro == null) _centro = new Centro(cnnSql);

            try
            {
                cnnSql.Open();
                if (centroDTO.ID_CENTRO == 0)
                {
                    centroDTO.ID_CENTRO = _centro.EjecutarSentenciaInsert(centroDTO);
                }
                else
                    x = _centro.EjecutarSentenciaUpdate(centroDTO);
            }
            finally
            {
                if (cnnSql.State == ConnectionState.Open) cnnSql.Close();
            }

            return centroDTO;
        }

        public List<CentroDTO> ListaCentro()
        {
            if (_centro == null) _centro = new Centro(cnnSql);
            List<CentroDTO> _centroDto = new List<CentroDTO>();

            try
            {
                cnnSql.Open();
                return _centro.ListaCentro();
            }
            finally
            {
                if (cnnSql.State != ConnectionState.Closed) cnnSql.Close();
            }
        }

        public List<CentroDTO> ListaCentroDDL()
        {
            if (_centro == null) _centro = new Centro(cnnSql);
            List<CentroDTO> _centroDto = new List<CentroDTO>();

            try
            {
                cnnSql.Open();
                return _centro.ListaCentroDDL();
            }
            finally
            {
                if (cnnSql.State != ConnectionState.Closed) cnnSql.Close();
            }
        }

        public Int16 DarBajaCentro(Int32 idCentro, string usuario)
        {
            if (_centro == null) _centro = new Centro(cnnSql);
            try
            {
                cnnSql.Open();
                return _centro.DarBajaCentro(idCentro, usuario);
            }
            finally
            {
                if (cnnSql.State != ConnectionState.Closed) cnnSql.Close();
            }

        }

        public Int16 DarAltaCentro(Int32 idCentro, string usuario)
        {
            if (_centro == null) _centro = new Centro(cnnSql);

            try
            {
                cnnSql.Open();
                return _centro.DarAltaCentro(idCentro, usuario);
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
