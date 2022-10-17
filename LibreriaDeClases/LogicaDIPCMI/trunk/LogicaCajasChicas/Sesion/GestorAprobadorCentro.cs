using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using LogicaCajasChicas.Entidad;
using System.Data;
using DipCmiGT.LogicaCajasChicas;
using DipCmiGT.LogicaCajasChicas.Entidad;
using System.Transactions;
using DipCmiGT.LogicaComun;

namespace DipCmiGT.LogicaCajasChicas.Sesion
{
    public class GestorAprobadorCentro : IDisposable
    {

        #region Declaraciones
        AprobadorCentro _aprobadorcentro = null;
        SAPCentroCosto _centrocosto = null;
        SAPOrdenesGastos _ordencompra = null;
        Centro _centro = null;
        Nivel _nivel = null;
        SqlConnection cnnSql = null;
        #endregion

        #region Constructores
        public GestorAprobadorCentro(string conexion)
        {
            cnnSql = new SqlConnection(conexion);
        }
        #endregion

        #region AprobadorCentro
        public AprobadorCentroDTO EjecutarSentenciaSelect(Int32 IdAprobadorCentro)
        {
            if (_aprobadorcentro == null) _aprobadorcentro = new AprobadorCentro(cnnSql);
            try
            {
                cnnSql.Open();
                return _aprobadorcentro.EjecutarSentenciaSelect(IdAprobadorCentro);
            }
            finally
            {
                if (cnnSql.State == ConnectionState.Open) cnnSql.Close();
            }
        }

        public AprobadorCentroDTO AlmacenarAprobadorCentro(AprobadorCentroDTO aprobadorcentroDto)
        {
            Int32 x = 0;
            if (_aprobadorcentro == null) _aprobadorcentro = new AprobadorCentro(cnnSql);

            try
            {
                cnnSql.Open();
                if (aprobadorcentroDto.ID_APROBADORCENTRO == 0) // || (!_aprobadorcentro.ExisteRegistro(aprobadorcentroDto.KOSTL, aprobadorcentroDto.AUFNR, aprobadorcentroDto.ID_CENTRO, aprobadorcentroDto.ID_USUARIO, aprobadorcentroDto.ID_NIVEL)))
                {
                    x = _aprobadorcentro.EjecutarSentenciaInsert(aprobadorcentroDto);
                }
                else
                    x = _aprobadorcentro.EjecutarSentenciaUpdate(aprobadorcentroDto);
            }
            finally
            {
                if (cnnSql.State == ConnectionState.Open) cnnSql.Close();
            }

            return aprobadorcentroDto;
        }

        public List<AprobadorCentroDTO> ListaAprobadorCentro()
        {
            if (_aprobadorcentro == null) _aprobadorcentro = new AprobadorCentro(cnnSql);
            List<AprobadorCentroDTO> _aprobadorcentroDto = new List<AprobadorCentroDTO>();

            try
            {
                cnnSql.Open();
                return _aprobadorcentro.ListaAprobadorCentro();
            }
            finally
            {
                if (cnnSql.State != ConnectionState.Closed) cnnSql.Close();
            }
        }

        public Int16 DarBajaAprobadorCentro(Int32 idAprobadorCentro, string usuario)
        {
            if (_aprobadorcentro == null) _aprobadorcentro = new AprobadorCentro(cnnSql);
            try
            {
                cnnSql.Open();
                return _aprobadorcentro.DarBajaAprobadorCentro(idAprobadorCentro, usuario);
            }
            finally
            {
                if (cnnSql.State != ConnectionState.Closed) cnnSql.Close();
            }

        }

        public Int16 DarAltaAprobadorCentro(Int32 idAprobadorCentro, string usuario)
        {
            if (_aprobadorcentro == null) _aprobadorcentro = new AprobadorCentro(cnnSql);

            try
            {
                cnnSql.Open();
                return _aprobadorcentro.DarAltaAprobadorCentro(idAprobadorCentro, usuario);
            }
            finally
            {
                if (cnnSql.State != ConnectionState.Closed) cnnSql.Close();
            }
        }

        #endregion

        #region CentroCosto

        //public List<SAPCentroCostoDTO> ListaCentroCosto()
        //{
        //    if (_centrocosto == null) _centrocosto = new SAPCentroCosto(cnnSql);

        //    try
        //    {
        //        cnnSql.Open();

        //        return _centrocosto.ListarCentrosCosto();
        //    }
        //    finally
        //    {
        //        if (cnnSql.State != ConnectionState.Closed) cnnSql.Close();
        //    }

        //}
        #endregion

        #region OrdenCompra

        public List<SAPOrdenesGastosDTO> ListaOrdenCompra()
        {
            if (_ordencompra == null) _ordencompra = new SAPOrdenesGastos(cnnSql);
            try
            {
                cnnSql.Open();
                return _ordencompra.ListaOrdenCompraDDL();

            }
            finally
            {
                if (cnnSql.State != ConnectionState.Closed) cnnSql.Close();
            }
        }
        #endregion

        #region Centro

        public List<CentroDTO> ListaCentro()
        {
            if (_centro == null) _centro = new Centro(cnnSql);
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
        #endregion

        #region Nivel

        public List<NivelDTO> ListaNivel()
        {
            if (_nivel == null) _nivel = new Nivel(cnnSql);
            try
            {
                cnnSql.Open();
                return _nivel.ListaNivel();
            }
            finally
            {
                if (cnnSql.State != ConnectionState.Closed) cnnSql.Close();
            }
        }
        #endregion

        #region IDisposbleMembers

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
