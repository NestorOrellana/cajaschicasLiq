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
    public class GestorLiquidacion : IDisposable
    {
        #region Declaracion
        SqlConnection cnnSql = null;
        Liquidacion _liquidacion;
        #endregion

        #region Constructor
        public GestorLiquidacion(string conexion)
        {
            cnnSql = new SqlConnection(conexion);
        }
        # endregion

        #region Metodos
        public LiquidacionDTO AlmacenarLiquidacion(LiquidacionDTO liquidacionDTO)
        {
            Int32 x = 0;
            if (_liquidacion == null) _liquidacion = new Liquidacion(cnnSql);

            try
            {
                cnnSql.Open();

                if (liquidacionDTO.CodigoLiquidacion.ToString() == "" || liquidacionDTO.CodigoLiquidacion.ToString() == "0")
                {
                    x = _liquidacion.EjecutarSentenciaInsert(liquidacionDTO);
                }
                else
                    x = _liquidacion.EjectuarSentenciaUpdate(liquidacionDTO);
            }
            finally
            {
                if (cnnSql.State == ConnectionState.Open) cnnSql.Close();
            }

            return liquidacionDTO;
        }

        public Int16 DarBajaLiquidacion(int codigoLiquidacion, string usuario)
        {
            if (_liquidacion == null) _liquidacion = new Liquidacion(cnnSql);

            try
            {
                cnnSql.Open();

                return _liquidacion.DarBajaNivel(codigoLiquidacion, usuario);
            }
            finally
            {
                if (cnnSql.State != ConnectionState.Closed) cnnSql.Close();
            }
        }

        public Int16 DarAltaLiquidacion(int codigoLiquidacion, string usuario)
        {
            if (_liquidacion == null) _liquidacion = new Liquidacion(cnnSql);

            try
            {
                cnnSql.Open();

                return _liquidacion.DarAltaLiquidacion(codigoLiquidacion, usuario);
            }
            finally
            {
                if (cnnSql.State != ConnectionState.Closed) cnnSql.Close();
            }
        }

        public List<LiquidacionDTO> ListaLiquidaciones()
        {
            if (_liquidacion == null) _liquidacion = new Liquidacion(cnnSql);

            try
            {
                cnnSql.Open();
                return _liquidacion.ListaLiquidacion();
            }
            finally
            {
                if (cnnSql.State != ConnectionState.Closed) cnnSql.Close();
            }
        }

        public List<LlenarDDL_DTO> ListaPaisesDDL()
        {
            if (_liquidacion == null) _liquidacion = new Liquidacion(cnnSql);

            try
            {
                cnnSql.Open();
                return _liquidacion.ListaPaisesDDL();
            }
            finally
            {
                if (cnnSql.State != ConnectionState.Closed) cnnSql.Close();
            }
        }

        public List<LlenarDDL_DTO> ListaNivelesLiquidacion(string pais)
        {
            if (_liquidacion == null) _liquidacion = new Liquidacion(cnnSql);

            try
            {
                cnnSql.Open();
                return _liquidacion.ListaNiveles(pais);
            }
            finally
            {
                if (cnnSql.State != ConnectionState.Closed) cnnSql.Close();
            }
        }

        public List<LlenarDDL_DTO> ListaTiposGastos(string pais)
        {
            if (_liquidacion == null) _liquidacion = new Liquidacion(cnnSql);

            try
            {
                cnnSql.Open();
                return _liquidacion.ListaGastos(pais);
            }
            finally
            {
                if (cnnSql.State != ConnectionState.Closed) cnnSql.Close();
            }
        }

        public bool ValidaExisteLiquidacion(string pais, int codigoNivel, int codigoGasto)
        {
            if (_liquidacion == null) _liquidacion = new Liquidacion(cnnSql);

            try
            {
                cnnSql.Open();
                return _liquidacion.ExisteLiquidacion(pais, codigoNivel, codigoGasto);
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
