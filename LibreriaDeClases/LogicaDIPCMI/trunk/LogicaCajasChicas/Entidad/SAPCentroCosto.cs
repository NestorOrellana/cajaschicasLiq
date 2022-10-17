using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using DipCmiGT.LogicaCajasChicas;

namespace DipCmiGT.LogicaCajasChicas.Entidad
{
    /// <summary>
    /// Tabla Centros de costo : ZITEMDATA
    /// </summary>
    public class SAPCentroCosto
    {
        #region Declaraciones

        private SqlConnection _sqlConn = null;
        private SqlTransaction _sqlTran = null;

        protected string sqlSelect = @" SELECT KOKRS, KOSTL, BUKRS, GSBER, KOSAR, VERAK, KTEXT, KHINR, BKZKP, LTEXT
                                        FROM sap.CentroCostoTMP ";

        protected string sqlInsert = @" INSERT INTO sap.CentroCostoTMP (KOKRS,KOSTL,BUKRS,GSBER,KOSAR,VERAK,KTEXT,KHINR,BKZKP, LTEXT) 
                                                                VALUES (@KOKRS,@KOSTL,@BUKRS,@GSBER,@KOSAR,@VERAK,@KTEXT,@KHINR,@BKZKP, @LTEXT) ";

        protected string sqlDelete = @" delete from sap.CentroCostoTMP ";

        #endregion

        #region Constructores

        public SAPCentroCosto(SqlConnection sqlConn)
        {
            _sqlConn = sqlConn;
        }

        public SAPCentroCosto(SqlConnection sqlConn, SqlTransaction sqlTran)
        {
            _sqlConn = sqlConn;
            _sqlTran = sqlTran;
        }

        #endregion

        #region Metodos

        public SAPCentroCostoDTO EjecutarSentenciaSelect()
        {
            throw new NotImplementedException("Método no implementado.");
        }

        public Int32 EjecutarSentenciaInsert(SAPCentroCostoDTO _sapCentroCostoDto)
        {
            SqlCommand _sqlComando = null;

            _sqlComando = new SqlCommand(sqlInsert, _sqlConn);

            if (_sqlTran != null)
                _sqlComando.Transaction = _sqlTran;

            _sqlComando.Parameters.Add(new SqlParameter("@KOKRS", (object)_sapCentroCostoDto.KOKRS ?? DBNull.Value));
            _sqlComando.Parameters.Add(new SqlParameter("@KOSTL", (object)_sapCentroCostoDto.KOSTL ?? DBNull.Value));
            _sqlComando.Parameters.Add(new SqlParameter("@BUKRS", (object)_sapCentroCostoDto.BUKRS ?? DBNull.Value));
            _sqlComando.Parameters.Add(new SqlParameter("@GSBER", (object)_sapCentroCostoDto.GSBER ?? DBNull.Value));
            _sqlComando.Parameters.Add(new SqlParameter("@KOSAR", (object)_sapCentroCostoDto.KOSAR ?? DBNull.Value));
            _sqlComando.Parameters.Add(new SqlParameter("@VERAK", (object)_sapCentroCostoDto.VERAK ?? DBNull.Value));
            _sqlComando.Parameters.Add(new SqlParameter("@KTEXT", (object)_sapCentroCostoDto.KTEXT ?? DBNull.Value));
            _sqlComando.Parameters.Add(new SqlParameter("@KHINR", (object)_sapCentroCostoDto.KHINR ?? DBNull.Value));
            _sqlComando.Parameters.Add(new SqlParameter("@BKZKP", (object)_sapCentroCostoDto.BKZKP ?? DBNull.Value));
            _sqlComando.Parameters.Add(new SqlParameter("@LTEXT", (object)_sapCentroCostoDto.LTEXT ?? DBNull.Value));

            return _sqlComando.ExecuteNonQuery();
        }


        public Int32 EjecutarSentenciaUpdate()
        {
            throw new NotImplementedException("Método no implementado.");
        }

        public Int32 EjecutarSentenciaDelete(string codigoSociedad)
        {
            SqlCommand _sqlComando = null;
            string sql = sqlDelete + " WHERE BUKRS = @BUKRS ";

            _sqlComando = new SqlCommand(sql, _sqlConn);
            _sqlComando.CommandType = System.Data.CommandType.Text;

            if (_sqlTran != null)
                _sqlComando.Transaction = _sqlTran;

            _sqlComando.Parameters.Add(new SqlParameter("@BUKRS", (object)codigoSociedad ?? DBNull.Value));

            return _sqlComando.ExecuteNonQuery();
        }

        protected List<SAPCentroCostoDTO> CargarReader(SqlDataReader sqlReader)
        {
            SAPCentroCostoDTO _centroCosto = null;
            List<SAPCentroCostoDTO> _listaCentroCosto = new List<SAPCentroCostoDTO>();

            while (sqlReader.Read())
            {
                _centroCosto = new SAPCentroCostoDTO();

                _centroCosto.KOKRS = sqlReader.GetString(0);
                _centroCosto.KOSTL = sqlReader.GetString(1);
                _centroCosto.BUKRS = sqlReader.GetString(2);
                _centroCosto.GSBER = sqlReader.GetString(3);
                _centroCosto.KOSAR = sqlReader.GetString(4);
                _centroCosto.VERAK = sqlReader.GetString(5);
                _centroCosto.KTEXT = sqlReader.GetString(6);
                _centroCosto.KHINR = sqlReader.GetString(7);
                _centroCosto.BKZKP = sqlReader.GetString(8);
                _centroCosto.LTEXT = sqlReader.GetString(9);

                _listaCentroCosto.Add(_centroCosto);
            }
            return _listaCentroCosto;
        }

        protected List<SAPCentroCostoDTO> CargarReaderDDL(SqlDataReader sqlReader)
        {
            SAPCentroCostoDTO _centroCosto = null;
            List<SAPCentroCostoDTO> _listaCentroCosto = new List<SAPCentroCostoDTO>();

            while (sqlReader.Read())
            {
                _centroCosto = new SAPCentroCostoDTO();

                _centroCosto.KOSTL = sqlReader.GetString(0);

                _listaCentroCosto.Add(_centroCosto);
            }
            return _listaCentroCosto;
        }

        public List<SAPCentroCostoDTO> ListarGastosDesbolqueados()
        {
            SqlCommand _sqlComando = null;
            string sql = sqlSelect + " Where RTRIM(LTRIM(BKZKP)) = '' ";

            _sqlComando = new SqlCommand(sql, _sqlConn);

            if (_sqlTran != null)
                _sqlComando.Transaction = _sqlTran;

            return CargarReader(_sqlComando.ExecuteReader());
        }

        #endregion
    }
}
