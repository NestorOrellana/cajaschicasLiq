using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;

namespace LogicaCajasChicas.Entidad
{
    class IntermediaFacturaDetalle
    {
        #region Declaraciones

        private SqlConnection _sqlConn = null;
        private SqlTransaction _sqlTran = null;

        protected string sqlInsert = @" INSERT INTO rf.CajaChicaDetalle (BUKRS, DOCUMENT, BLDAT, TYPE, BUDAT, BUZEI, DUMMY, BSCHL, HKONT, WRBTR, WRIVA, MWSKZ, SGTXT, SGTXT2, KOSTL, AUFNR, ZUONR, GSBER, ZFBDT, UMSKZ, CO_AREA, S_WORKP, ACCT_TYPE, UNIT)
                                                                 VALUES (@BUKRS, @DOCUMENT, @BLDAT, @TYPE, @BUDAT, @BUZEI, @DUMMY, @BSCHL, @HKONT, @WRBTR, @WRIVA, REPLACE(REPLACE(@MWSKZ, 'NA', ''), 'CF',''), LEFT(@SGTXT,40), @SGTXT2, @KOSTL, @AUFNR, @ZUONR, @GSBER, @ZFBDT, @UMSKZ, @CO_AREA, @S_WORKP, @ACCT_TYPE, @UNIT) ";

        #endregion

        #region Constructores


        public IntermediaFacturaDetalle(SqlConnection sqlConn)
        {
            _sqlConn = sqlConn;
        }

        public IntermediaFacturaDetalle(SqlConnection sqlConn, SqlTransaction sqlTran)
        {
            _sqlConn = sqlConn;
            _sqlTran = sqlTran;
        }

        #endregion

        #region Metodos

        public IntermediaFacturaDetalleDTO EjecutarSentenciaSelect()
        {
            throw new NotImplementedException("");
        }

        public Int32 EjecutarSenteciaInsert(IntermediaFacturaDetalleDTO intermediaFacturaDetalleDTO)
        {
            SqlCommand _sqlComando = null;
            string sql = sqlInsert;

            _sqlComando = new SqlCommand(sql, _sqlConn);
            _sqlComando.CommandType = CommandType.Text;

            if (_sqlTran != null)
                _sqlComando.Transaction = _sqlTran;

            _sqlComando.Parameters.Add(new SqlParameter("@BUKRS", (object)intermediaFacturaDetalleDTO.BUKRS ?? DBNull.Value));
            _sqlComando.Parameters.Add(new SqlParameter("@DOCUMENT", (object)intermediaFacturaDetalleDTO.DOCUMENT ?? DBNull.Value));
            _sqlComando.Parameters.Add(new SqlParameter("@BLDAT", (object)intermediaFacturaDetalleDTO.BLDAT ?? DBNull.Value));
            _sqlComando.Parameters.Add(new SqlParameter("@TYPE", (object)intermediaFacturaDetalleDTO.TYPE ?? DBNull.Value));
            _sqlComando.Parameters.Add(new SqlParameter("@BUDAT", (object)intermediaFacturaDetalleDTO.BUDAT ?? DBNull.Value));
            _sqlComando.Parameters.Add(new SqlParameter("@BUZEI", (object)intermediaFacturaDetalleDTO.BUZEI ?? DBNull.Value));
            _sqlComando.Parameters.Add(new SqlParameter("@DUMMY", (object)intermediaFacturaDetalleDTO.DUMMY ?? DBNull.Value));
            _sqlComando.Parameters.Add(new SqlParameter("@BSCHL", (object)intermediaFacturaDetalleDTO.BSCHL ?? DBNull.Value));
            _sqlComando.Parameters.Add(new SqlParameter("@HKONT", (object)intermediaFacturaDetalleDTO.HKONT ?? DBNull.Value));
            _sqlComando.Parameters.Add(new SqlParameter("@WRBTR", (object)intermediaFacturaDetalleDTO.WRBTR ?? DBNull.Value));
            _sqlComando.Parameters.Add(new SqlParameter("@WRIVA", (object)intermediaFacturaDetalleDTO.WRIVA ?? DBNull.Value));
            _sqlComando.Parameters.Add(new SqlParameter("@MWSKZ", (object)intermediaFacturaDetalleDTO.MWSKZ ?? DBNull.Value));
            _sqlComando.Parameters.Add(new SqlParameter("@SGTXT", (object)intermediaFacturaDetalleDTO.SGTXT ?? DBNull.Value));
            _sqlComando.Parameters.Add(new SqlParameter("@SGTXT2", (object)intermediaFacturaDetalleDTO.SGTXT2 ?? DBNull.Value));
            _sqlComando.Parameters.Add(new SqlParameter("@KOSTL", (object)intermediaFacturaDetalleDTO.KOSTL ?? DBNull.Value));
            _sqlComando.Parameters.Add(new SqlParameter("@AUFNR", (object)intermediaFacturaDetalleDTO.AUFNR ?? DBNull.Value));
            _sqlComando.Parameters.Add(new SqlParameter("@ZUONR", (object)intermediaFacturaDetalleDTO.ZUONR ?? DBNull.Value));
            _sqlComando.Parameters.Add(new SqlParameter("@GSBER", (object)intermediaFacturaDetalleDTO.GSBER ?? DBNull.Value));
            _sqlComando.Parameters.Add(new SqlParameter("@ZFBDT", (object)intermediaFacturaDetalleDTO.ZFBDT ?? DBNull.Value));
            _sqlComando.Parameters.Add(new SqlParameter("@UMSKZ", (object)intermediaFacturaDetalleDTO.UMSKZ ?? DBNull.Value));
            _sqlComando.Parameters.Add(new SqlParameter("@CO_AREA", (object)intermediaFacturaDetalleDTO.CO_AREA ?? DBNull.Value));
            _sqlComando.Parameters.Add(new SqlParameter("@S_WORKP", (object)intermediaFacturaDetalleDTO.S_WORKP ?? DBNull.Value));
            _sqlComando.Parameters.Add(new SqlParameter("@ACCT_TYPE", (object)intermediaFacturaDetalleDTO.ACCT_TYPE ?? DBNull.Value));
            _sqlComando.Parameters.Add(new SqlParameter("@UNIT", (object)intermediaFacturaDetalleDTO.UNIT ?? DBNull.Value));

            return Convert.ToInt32(_sqlComando.ExecuteNonQuery());
        }

        public Int32 EjecutarSentenciaUpdate()
        {
            throw new NotImplementedException("");
        }

        protected List<IntermediaFacturaDetalleDTO> CargarReader(SqlDataReader sqlReader)
        {
            throw new NotImplementedException("");
        }

        #endregion
    }
}