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
    public class GestorGastos : IDisposable
    {
        #region Declaracion
        SqlConnection cnnSql = null;
        Gasto _gasto;
        #endregion

        #region Constructor
        public GestorGastos(string conexion)
        {
            cnnSql = new SqlConnection(conexion);
        }
        # endregion

        #region Metodos
        public GastosDTO AlmacenarGasto(GastosDTO gastoDTO)
        {
            Int32 x = 0;
            if (_gasto == null) _gasto = new Gasto(cnnSql);

            try
            {
                cnnSql.Open();
                
                if (gastoDTO.CodigoGasto.ToString() == "" || gastoDTO.CodigoGasto.ToString() == "0")
                {
                    x = _gasto.EjecutarSentenciaInsert(gastoDTO);
                }
                else
                    x = _gasto.EjectuarSentenciaUpdate(gastoDTO);
            }
            finally
            {
                if (cnnSql.State == ConnectionState.Open) cnnSql.Close();
            }

            return gastoDTO;
        }

        public Int16 DarBajaGasto(Int32 codigoGasto, string usuario)
        {
            if (_gasto == null) _gasto = new Gasto(cnnSql);

            try
            {
                cnnSql.Open();

                return _gasto.DarBajaGasto(codigoGasto, usuario);
            }
            finally
            {
                if (cnnSql.State != ConnectionState.Closed) cnnSql.Close();
            }
        }

        public Int16 DarAltaGasto(Int32 codigoGasto, string usuario)
        {
            if (_gasto == null) _gasto = new Gasto(cnnSql);

            try
            {
                cnnSql.Open();

                return _gasto.DarAltaGasto(codigoGasto, usuario);
            }
            finally
            {
                if (cnnSql.State != ConnectionState.Closed) cnnSql.Close();
            }
        }

        public List<GastosDTO> ListaGastos()
        {
            if (_gasto == null) _gasto = new Gasto(cnnSql);
            List<GastosDTO> _gastoDTO = new List<GastosDTO>();

            try
            {
                cnnSql.Open();
                return _gasto.ListaGastos();
            }
            finally
            {
                if (cnnSql.State != ConnectionState.Closed) cnnSql.Close();
            }
        }

        public List<LlenarDDL_DTO> ListaPaisesDDL()
        {
            if (_gasto == null) _gasto = new Gasto(cnnSql);

            try
            {
                cnnSql.Open();
                return _gasto.ListaPaisesDDL();
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
