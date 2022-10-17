using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using LogicaCajasChicas;

namespace DipCmiGT.LogicaCajasChicas.Entidad
{
    public class Valores_ISR
    {
        private SqlConnection _sqlConn = null;
        private SqlTransaction _sqlTran = null;

        public Valores_ISR(SqlTransaction sqlTran)
        {
            _sqlTran = sqlTran;
        }

        public Valores_ISR(SqlConnection sqlConn)
        {
            _sqlConn = sqlConn;
        }

        public Valores_ISR(SqlConnection sqlConn, SqlTransaction sqlTran)
        {
            _sqlConn = sqlConn;
            _sqlTran = sqlTran;
        }

        private string sqlSelect = @" SELECT     ID_ISR, TIPO_ISR, RANGO_INICIAL, RANGO_FINAL, IMPORTE_FIJO, TIPO_IMPOSITIVO
                                    FROM VALORES_ISR     ";

        public Valores_ISRDTO BuscarIsrRango(Int16 tipoIsr, double monto)
        {
            List<Valores_ISRDTO> _valoresISRDto = null;
            string sql = sqlSelect + @" where TIPO_ISR = @tipoIsr
                                        and @moto between RANGO_INICIAL and RANGO_FINAL ";

            SqlCommand sqlComando = null;

            sqlComando = new SqlCommand(sql, _sqlConn);
            sqlComando.CommandType = CommandType.Text;

            if (_sqlTran != null)
                sqlComando.Transaction = _sqlTran;

            sqlComando.Parameters.Add(new SqlParameter("@tipoIsr", (object)tipoIsr ?? DBNull.Value));
            sqlComando.Parameters.Add(new SqlParameter("@moto", (object)monto ?? DBNull.Value));

            _valoresISRDto = CargarReader(sqlComando.ExecuteReader());

            return _valoresISRDto.Count > 0 ? _valoresISRDto[0] : null;
        }

        private List<Valores_ISRDTO> CargarReader(SqlDataReader sqlReader)
        {
            Valores_ISRDTO _valoresISR = null;
            List<Valores_ISRDTO> _listaValoresISR = new List<Valores_ISRDTO>();

            while (sqlReader.Read())
            {
                _valoresISR = new Valores_ISRDTO();

                _valoresISR.ID_ISR = sqlReader.GetInt32(0);
                _valoresISR.TIPOISR = sqlReader.GetInt16(1);
                _valoresISR.RANGO_INICIAL = sqlReader.GetDouble(2);
                _valoresISR.RANGO_FINAL = sqlReader.GetDouble(3);
                _valoresISR.IMPORTE_FIJO = sqlReader.GetDouble(4);
                _valoresISR.TIPO_IMPOSITIVO = sqlReader.GetInt16(5);

                _listaValoresISR.Add(_valoresISR);

            }

            sqlReader.Close();

            return _listaValoresISR;
        }

    }
}
