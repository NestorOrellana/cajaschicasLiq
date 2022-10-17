using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using DipCmiGT.LogicaCajasChicas;


namespace DipCmiGT.LogicaCajasChicas.Entidad
{
    /// <summary>
    /// Tabla de Ordenes de Gastos : Z_ZSTR_AUFK
    /// </summary>
    public class SAPOrdenesGastos
    {
        #region Declaraciones

        private SqlConnection _sqlConn = null;
        private SqlTransaction _sqlTran = null;

        protected string sqlSelect = @" SELECT AUFNR, AUART, KTEXT, BUKRS, PHAS1, PHAS2, PHAS3
                                        FROM sap.OrdenCOTMP ";

        protected string sqlInsert = @" INSERT INTO [sap].[OrdenCOTMP] (AUFNR,AUART,KTEXT,BUKRS,PHAS1,PHAS2,PHAS3) 
                                                                values (@AUFNR,@AUART,@KTEXT,@BUKRS,@PHAS1,@PHAS2,@PHAS3) ";

        protected string sqlDelete = @" DELETE FROM sap.OrdenCOTMP ";

        #endregion

        #region Constructores

        public SAPOrdenesGastos(SqlConnection sqlConn)
        {
            _sqlConn = sqlConn;
        }

        public SAPOrdenesGastos(SqlConnection sqlConn, SqlTransaction sqlTran)
        {
            _sqlConn = sqlConn;
            _sqlTran = sqlTran;
        }

        #endregion

        #region Metodos

        public List<SAPOrdenesGastosDTO> ListaOrdenCompraDDLFacturas()
        {
            SqlCommand _sqlComando = null;
            string sql = @"SELECT AUFNR FROM sap.OrdenCOTMP";

            _sqlComando = new SqlCommand(sql, _sqlConn);

            if (_sqlTran != null)
                _sqlComando.Transaction = _sqlTran;

            return CargarReaderDDL(_sqlComando.ExecuteReader());
        }

        public List<SAPOrdenesGastosDTO> CargarReaderDDL(SqlDataReader sqlReader)
        {
            SAPOrdenesGastosDTO _sapordengastosDto = null;
            List<SAPOrdenesGastosDTO> _listaordencompra = new List<SAPOrdenesGastosDTO>();

            while (sqlReader.Read())
            {
                _sapordengastosDto = new SAPOrdenesGastosDTO();

                _sapordengastosDto.AUFNR = sqlReader.GetString(0);
                _listaordencompra.Add(_sapordengastosDto);

            }
            return _listaordencompra;
        }
        public SAPOrdenesGastosDTO EjecutarSentenciaSelect()
        {
            throw new NotImplementedException("Método no implementado.");
        }

        public Int32 EjecutarSentenciaInsert(SAPOrdenesGastosDTO ordenesGastosDto)
        {
            SqlCommand _sqlComando = null;

            _sqlComando = new SqlCommand(sqlInsert, _sqlConn);

            if (_sqlTran != null)
                _sqlComando.Transaction = _sqlTran;

            _sqlComando.Parameters.Add(new SqlParameter("@AUFNR", (object)ordenesGastosDto.AUFNR ?? DBNull.Value));
            _sqlComando.Parameters.Add(new SqlParameter("@AUART", (object)ordenesGastosDto.AUART ?? DBNull.Value));
            _sqlComando.Parameters.Add(new SqlParameter("@KTEXT", (object)ordenesGastosDto.KTEXT ?? DBNull.Value));
            _sqlComando.Parameters.Add(new SqlParameter("@BUKRS", (object)ordenesGastosDto.BUKRS ?? DBNull.Value));
            _sqlComando.Parameters.Add(new SqlParameter("@PHAS1", (object)ordenesGastosDto.PHAS1 ?? DBNull.Value));
            _sqlComando.Parameters.Add(new SqlParameter("@PHAS2", (object)ordenesGastosDto.PHAS2 ?? DBNull.Value));
            _sqlComando.Parameters.Add(new SqlParameter("@PHAS3", (object)ordenesGastosDto.PHAS3 ?? DBNull.Value));

            return _sqlComando.ExecuteNonQuery();
        }


        public Int32 EjecutarSentenciaUpdate()
        {
            throw new NotImplementedException("Metodo no implementado");
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

        protected List<SAPOrdenesGastosDTO> CargarReader(SqlDataReader sqlReader)
        {
            SAPOrdenesGastosDTO _ordenesGastosDto = null;
            List<SAPOrdenesGastosDTO> _listaOrdenesGastos = new List<SAPOrdenesGastosDTO>();

            while (sqlReader.Read())
            {
                _ordenesGastosDto = new SAPOrdenesGastosDTO();

                _ordenesGastosDto.AUFNR = sqlReader.GetString(0);
                _ordenesGastosDto.AUART = sqlReader.GetString(1);
                _ordenesGastosDto.KTEXT = sqlReader.GetString(2);
                _ordenesGastosDto.BUKRS = sqlReader.GetString(3);
                _ordenesGastosDto.PHAS1 = sqlReader.GetString(4);
                _ordenesGastosDto.PHAS2 = sqlReader.GetString(5);
                _ordenesGastosDto.PHAS3 = sqlReader.GetString(6);

                _listaOrdenesGastos.Add(_ordenesGastosDto);
            }

            return _listaOrdenesGastos;
        }

        public List<SAPOrdenesGastosDTO> ListarOrdenesGastos(string sociedad)
        {
            SqlCommand _sqlComando = null;
            string sql = sqlSelect + @" Where BUKRS = @BUKRS and PHAS1 = 'X' 
                                        and RTRIM(LTRIM(PHAS2)) = '' 
                                        and RTRIM(LTRIM(PHAS3)) = '' ";

            _sqlComando = new SqlCommand(sqlSelect, _sqlConn);

            if (_sqlTran != null)
                _sqlComando.Transaction = _sqlTran;

            _sqlComando.Parameters.Add(new SqlParameter("@BUKRS", (object)sociedad ?? DBNull.Value));

            return CargarReader(_sqlComando.ExecuteReader());
        }

        public List<SAPOrdenesGastosDTO> ListaOrdenCompraDDL()
        {
            SqlCommand _sqlComando = null;
            string sql = sqlSelect + "";

            _sqlComando = new SqlCommand(sql, _sqlConn);

            if (_sqlTran != null)
                _sqlComando.Transaction = _sqlTran;

            return CargarReader(_sqlComando.ExecuteReader());

        }
        #endregion
    }
}
