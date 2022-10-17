using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;

namespace LogicaCajasChicas.Entidad
{
    public class RegistroContable
    {
        #region Declaraciones

        private SqlConnection _sqlConn = null;
        private SqlTransaction _sqlTran = null;

        protected string sqlSelect = @" SELECT IdRegistroContable, Correlativo, CuentaContable,DefinicionCuentaContable,CargoAbono,Valor,IndicadorIVA,Alta,FechaAlta,FechaModificacion,IdFactura
                                        FROM RegistroContable";

        protected string sqlInsert = @" INSERT INTO RegistroContable (Correlativo, CuentaContable, DefinicionCuentaContable, CargoAbono, Valor, IndicadorIVA, IdFactura)
					                                          VALUES (@Correlativo, @CuentaContable, @DefinicionCuentaContable, @CargoAbono, @Valor, @IndicadorIVA, @IdFactura) ";
        #endregion

        #region Constructores

        public RegistroContable(SqlConnection sqlConn)
        {
            _sqlConn = sqlConn;
        }

        public RegistroContable(SqlConnection sqlConn, SqlTransaction sqlTran)
        {
            _sqlConn = sqlConn;
            _sqlTran = sqlTran;
        }

        #endregion

        #region Metodos

        public RegistroContableDTO EjecutarSentenciaSelect(decimal idRegistroContable)
        {
            List<RegistroContableDTO> listaRegistroContable = null;
            SqlCommand sqlComando;
            string sql = sqlSelect + "";

            sqlComando = new SqlCommand(sql, _sqlConn);
            sqlComando.CommandType = CommandType.Text;

            if (_sqlTran != null)
                sqlComando.Transaction = _sqlTran;

            sqlComando.Parameters.Add(new SqlParameter("@IdRegistroContable", (object)idRegistroContable ?? DBNull.Value));
            return listaRegistroContable.Count > 0 ? listaRegistroContable[0] : null;
        }

        public Int32 EjecutarSentenciaInsert(RegistroContableDTO registroContableDTO)
        {
            SqlCommand sqlCommando = new SqlCommand(sqlInsert, _sqlConn);
            sqlCommando.CommandType = CommandType.Text;

            if (_sqlTran != null)
                sqlCommando.Transaction = _sqlTran;

            sqlCommando.Parameters.Add(new SqlParameter("@Correlativo", (object)registroContableDTO.CORRELATIVO ?? DBNull.Value));
            sqlCommando.Parameters.Add(new SqlParameter("@CuentaContable", (object)registroContableDTO.CUENTA_CONTABLE ?? DBNull.Value));
            sqlCommando.Parameters.Add(new SqlParameter("@DefinicionCuentaContable", (object)registroContableDTO.DEFINICION_CUENTA_CONTABLE ?? DBNull.Value));
            sqlCommando.Parameters.Add(new SqlParameter("@CargoAbono", (object)registroContableDTO.CARGO_ABONO ?? DBNull.Value));
            sqlCommando.Parameters.Add(new SqlParameter("@Valor", (object)registroContableDTO.VALOR ?? DBNull.Value));
            sqlCommando.Parameters.Add(new SqlParameter("@IndicadorIVA", (object)registroContableDTO.INDICADOR_IVA ?? DBNull.Value));
            sqlCommando.Parameters.Add(new SqlParameter("IdFactura", (object)registroContableDTO.ID_FACTURA ?? DBNull.Value));

            return Convert.ToInt32(sqlCommando.ExecuteScalar());
        }

        protected List<RegistroContableDTO> CargarReader(SqlDataReader sqlReader)
        {
            RegistroContableDTO _registroContableDto = null;
            List<RegistroContableDTO> _listaRegistroContable = new List<RegistroContableDTO>();

            while (sqlReader.Read())
            {
                _registroContableDto = new RegistroContableDTO();
                _registroContableDto.ID_REGISTRO_CONTABLE = sqlReader.GetDecimal(0);
                _registroContableDto.CORRELATIVO = sqlReader.GetInt16(1);
                _registroContableDto.CUENTA_CONTABLE = sqlReader.GetString(2);
                _registroContableDto.DEFINICION_CUENTA_CONTABLE = sqlReader.GetString(3);
                _registroContableDto.CARGO_ABONO = sqlReader.GetInt16(4);
                _registroContableDto.VALOR = sqlReader.GetDouble(5);
                _registroContableDto.INDICADOR_IVA = sqlReader.GetString(6);
                _registroContableDto.ALTA = sqlReader.GetBoolean(7);
                _registroContableDto.FECHA_ALTA = sqlReader.GetDateTime(8);
                _registroContableDto.FECHA_MODIFICACION = sqlReader.IsDBNull(9) ? (DateTime?)null : sqlReader.GetDateTime(9);
                _registroContableDto.ID_FACTURA = sqlReader.GetDecimal(10);

                _listaRegistroContable.Add(_registroContableDto);
            }

            sqlReader.Close();
            return _listaRegistroContable;
        }


        protected List<RegistroContableSPDTO> CargarReaderSP(SqlDataReader sqlReader)
        {
            RegistroContableSPDTO _registroContableDto = null;
            List<RegistroContableSPDTO> _listaRegistroContable = new List<RegistroContableSPDTO>();

            while (sqlReader.Read())
            {
                _registroContableDto = new RegistroContableSPDTO();

                _registroContableDto.CUENTA_CONTABLE = sqlReader.GetString(0);
                _registroContableDto.INDICADOR_IVA = sqlReader.GetString(1);
                _registroContableDto.DEFINICION_CUENTA_CONTABLE = sqlReader.GetString(2);
                _registroContableDto.CARGO_ABONO = sqlReader.GetInt16(3);
                _registroContableDto.CARGO = sqlReader.GetDouble(4);
                _registroContableDto.ABONO = sqlReader.GetDouble(5);
                _registroContableDto.SERIE = sqlReader.GetString(6);
                _registroContableDto.NUMERO = sqlReader.GetString(7);//sqlReader.GetDecimal(7);
                _registroContableDto.DOCUMENTO_IDENTIFICACION = sqlReader.GetString(8);
                _registroContableDto.NUMERO_IDENTIFICACION = sqlReader.GetString(9);
                _registroContableDto.NOMBRE_PROVEEDOR = sqlReader.GetString(10);
                _registroContableDto.DIRECCION_PROVEEDOR = sqlReader.GetString(11);
                _registroContableDto.CODIGO_CC = sqlReader.GetString(12);
                _registroContableDto.FECHA_FACTURA = sqlReader.GetDateTime(13);
                _registroContableDto.SUMA_CARGO = sqlReader.GetDouble(14);
                _registroContableDto.SUMA_ABONO = sqlReader.GetDouble(15);
                _registroContableDto.ID_FACTURA = sqlReader.GetDecimal(16);
                _registroContableDto.DESCRIPCION = sqlReader.GetString(17);
                _registroContableDto.OBSERVACIONES = sqlReader.GetString(18);

                _listaRegistroContable.Add(_registroContableDto);
            }

            sqlReader.Close();
            return _listaRegistroContable;
        }

        public List<RegistroContableSPDTO> BuscarRegistroContableSP(decimal idFactura)
        {
            SqlCommand _sqlComando = null;

            string sql; 
                 sql = "RegistroContableFactura";

            _sqlComando = new SqlCommand(sql, _sqlConn);
            _sqlComando.CommandType = CommandType.StoredProcedure;

            if (_sqlTran != null)
                _sqlComando.Transaction = _sqlTran;

            _sqlComando.Parameters.Add(new SqlParameter("IdFactura", (object)idFactura ?? DBNull.Value));

            return CargarReaderSP(_sqlComando.ExecuteReader());
        }

        public List<RegistroContableDTO> BuscarRegistroContable(decimal idFactura)
        {
            SqlCommand _sqlComando = null;
            string sql = sqlSelect + " WHERE IdFactura = @IdFactura";

            _sqlComando = new SqlCommand(sql, _sqlConn);
            _sqlComando.CommandType = CommandType.Text;

            if (_sqlTran != null)
                _sqlComando.Transaction = _sqlTran;

            _sqlComando.Parameters.Add(new SqlParameter("IdFactura", (object)idFactura ?? DBNull.Value));

            return CargarReader(_sqlComando.ExecuteReader());
        }

        public Int32 AnularRegistroContable(decimal idFactura)
        {
            SqlCommand _sqlComando = null;
            string sql = "UPDATE RegistroContable SET Alta = 0, FechaModificacion = getdate() WHERE IdFactura = @IdFactura";

            _sqlComando = new SqlCommand(sql, _sqlConn);
            _sqlComando.CommandType = CommandType.Text;

            if (_sqlTran != null)
                _sqlComando.Transaction = _sqlTran;

            _sqlComando.Parameters.Add(new SqlParameter("@IdFactura", (object)idFactura ?? DBNull.Value));

            return _sqlComando.ExecuteNonQuery();
        }

        #endregion
    }
}
