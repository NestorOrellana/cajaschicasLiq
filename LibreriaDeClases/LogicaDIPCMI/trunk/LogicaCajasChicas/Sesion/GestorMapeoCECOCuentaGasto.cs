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
    public class GestorMapeoCECOCuentaGasto : IDisposable
    {
        #region Declaracion
        SqlConnection cnnSql = null;
        MapeoCECOCuentaGasto _mapeo;
        #endregion

        #region Constructor
        public GestorMapeoCECOCuentaGasto(string conexion)
        {
            cnnSql = new SqlConnection(conexion);
        }
        # endregion

        #region Metodos
        public MapeoCECOCuentaGastoDTO AlmacenarMapeo(MapeoCECOCuentaGastoDTO mapeoDTO)
        {
            Int32 x = 0;
            if (_mapeo == null) _mapeo = new MapeoCECOCuentaGasto(cnnSql);

            try
            {
                cnnSql.Open();

                if (mapeoDTO.CodigoMapeo.ToString() == "" || mapeoDTO.CodigoMapeo.ToString() == "0")
                {
                    x = _mapeo.EjecutarSentenciaInsert(mapeoDTO);
                }
                else
                    x = _mapeo.EjectuarSentenciaUpdate(mapeoDTO);
            }
            finally
            {
                if (cnnSql.State == ConnectionState.Open) cnnSql.Close();
            }

            return mapeoDTO;
        }

        public Int16 DarBajaMapeo(long codigoMapeo, string usuario)
        {
            if (_mapeo == null) _mapeo = new MapeoCECOCuentaGasto(cnnSql);

            try
            {
                cnnSql.Open();

                return _mapeo.DarBajaMapeo(codigoMapeo, usuario);
            }
            finally
            {
                if (cnnSql.State != ConnectionState.Closed) cnnSql.Close();
            }
        }

        public Int16 DarAltaMapeo(long codigoMapeo, string usuario)
        {
            if (_mapeo == null) _mapeo = new MapeoCECOCuentaGasto(cnnSql);

            try
            {
                cnnSql.Open();

                return _mapeo.DarAltaMapeo(codigoMapeo, usuario);
            }
            finally
            {
                if (cnnSql.State != ConnectionState.Closed) cnnSql.Close();
            }
        }

        public List<MapeoCECOCuentaGastoDTO> ListaMapeos()
        {
            if (_mapeo == null) _mapeo = new MapeoCECOCuentaGasto(cnnSql);

            try
            {
                cnnSql.Open();
                return _mapeo.ListaMapeos();
            }
            finally
            {
                if (cnnSql.State != ConnectionState.Closed) cnnSql.Close();
            }
        }

        public List<LlenarDDL_DTO> ListaPaisesDDL()
        {
            if (_mapeo == null) _mapeo = new MapeoCECOCuentaGasto(cnnSql);

            try
            {
                cnnSql.Open();
                return _mapeo.ListaPaisesDDL();
            }
            finally
            {
                if (cnnSql.State != ConnectionState.Closed) cnnSql.Close();
            }
        }

        public List<LlenarDDL_DTO> ListaCentrosCosto(string pais)
        {
            if (_mapeo == null) _mapeo = new MapeoCECOCuentaGasto(cnnSql);

            try
            {
                cnnSql.Open();
                return _mapeo.ListaCentrosCosto(pais);
            }
            finally
            {
                if (cnnSql.State != ConnectionState.Closed) cnnSql.Close();
            }
        }

        public List<LlenarDDL_DTO> ListaOrdenesCosto(string pais)
        {
            if (_mapeo == null) _mapeo = new MapeoCECOCuentaGasto(cnnSql);

            try
            {
                cnnSql.Open();
                return _mapeo.ListaOrdenCosto(pais);
            }
            finally
            {
                if (cnnSql.State != ConnectionState.Closed) cnnSql.Close();
            }
        }

        public List<LlenarDDL_DTO> ListaTiposGastos(string pais)
        {
            if (_mapeo == null) _mapeo = new MapeoCECOCuentaGasto(cnnSql);

            try
            {
                cnnSql.Open();
                return _mapeo.ListaGastos(pais);
            }
            finally
            {
                if (cnnSql.State != ConnectionState.Closed) cnnSql.Close();
            }
        }

        public List<LlenarDDL_DTO> ListaCuentasContables(string pais)
        {
            if (_mapeo == null) _mapeo = new MapeoCECOCuentaGasto(cnnSql);

            try
            {
                cnnSql.Open();
                return _mapeo.ListaCuentaContable(pais);
            }
            finally
            {
                if (cnnSql.State != ConnectionState.Closed) cnnSql.Close();
            }
        }

        public bool ValidaExisteMapeo(string pais, string centroCosto, string ordenCosto, int tipoGasto, string cuentaContable)
        {
            if (_mapeo == null) _mapeo = new MapeoCECOCuentaGasto(cnnSql);

            try
            {
                cnnSql.Open();
                return _mapeo.ExisteMapeo(pais, centroCosto, ordenCosto, tipoGasto, cuentaContable);
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
