using System;
using System.Collections.Generic;
using DipCmiGT.LogicaCajasChicas.Entidad;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;

namespace DipCmiGT.LogicaCajasChicas.Sesion
{
    public class GestorImpuestoCH : IDisposable
    {
        #region Declaracion
        ImpuestoCH _impuestoCH = null;
        SqlConnection cnnSql = null;
        #endregion

        #region Constructor
        public GestorImpuestoCH(string conexion)
        {
            cnnSql = new SqlConnection(conexion);
        }
        # endregion

        #region Metodos
        public List<ImpuestoCHDTO> ListaImpuestos()
        {
            if (_impuestoCH == null) _impuestoCH = new ImpuestoCH(cnnSql);
            List<ImpuestoCHDTO> _impuestoCHDTO = new List<ImpuestoCHDTO>();

            try
            {
                cnnSql.Open();
                return _impuestoCH.ListaImpuestos();
            }
            finally
            {
                if (cnnSql.State != ConnectionState.Closed) cnnSql.Close();
            }
        }

        public Int16 DarBajaImpuesto(Int32 CodigoImpuesto, string usuario)
        {
            if (_impuestoCH == null) _impuestoCH = new ImpuestoCH(cnnSql);

            try
            {
                cnnSql.Open();

                //return 0;
                return _impuestoCH.DarBajaImpuesto(CodigoImpuesto, usuario);
            }
            finally
            {
                if (cnnSql.State != ConnectionState.Closed) cnnSql.Close();
            }
        }

        public Int16 DarAltaImpuesto(Int32 CodigoImpuesto, string usuario)
        {
            if (_impuestoCH == null) _impuestoCH = new ImpuestoCH(cnnSql);

            try
            {
                cnnSql.Open();
                return _impuestoCH.DarAltaImpuesto(CodigoImpuesto, usuario);
            }
            finally
            {
                if (cnnSql.State != ConnectionState.Closed) cnnSql.Close();
            }
        }

        public ImpuestoCHDTO AlmacenarImpuesto(ImpuestoCHDTO impuestoDTO)
        {
            Int32 x = 0;
            if (_impuestoCH == null) _impuestoCH = new ImpuestoCH(cnnSql);

            try
            {
                cnnSql.Open();
                //Validacion si Existe Sociedad 
                if (impuestoDTO.CodigoImpuesto.ToString() != "" || impuestoDTO.CodigoImpuesto.ToString() != "0")
                {
                    if (!_impuestoCH.ExisteImpuesto(impuestoDTO.Pais, impuestoDTO.Descripcion))
                    {
                        x = _impuestoCH.EjecutarSentenciaInsert(impuestoDTO);
                    }
                    else
                        x = _impuestoCH.EjectuarSentenciaUpdate(impuestoDTO);
                }
            }
            finally
            {
                if (cnnSql.State == ConnectionState.Open) cnnSql.Close();
            }

            return impuestoDTO;
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
