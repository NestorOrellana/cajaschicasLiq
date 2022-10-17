using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using DipCmiGT.LogicaComun;

namespace LogicaCajasChicas.Entidad
{
    public class SAPCuentaContable
    {
        #region Declaraciones

        private SqlConnection _sqlConn = null;
        private SqlTransaction _sqlTran = null;

        protected string sqlSelect = @" SELECT SAKNR, (SAKNR + ' :: ' + TXT50) SAKNR
                                        from sap.CuentasContables a";

        protected string sqlInsert = @" INSERT INTO sap.CuentasContables (BUKRS, KTOKS, TXT30, SAKNR, TXT50)
                                                                   VALUES(@BUKRS, @KTOKS, @TXT30, @SAKNR, @TXT50) ";

        protected string sqlDelete = @" DELETE FROM  sap.CuentasContables ";

        #endregion

        #region Constructores

        public SAPCuentaContable(SqlConnection sqlConn)
        {
            _sqlConn = sqlConn;
        }

        public SAPCuentaContable(SqlConnection sqlConn, SqlTransaction sqlTran)
        {
            _sqlConn = sqlConn;
            _sqlTran = sqlTran;
        }

        #endregion

        #region Metodos

        public SAPCuentaContableDTO EjecutarSentenciaSelect(string cuentaContable)
        {
            SqlCommand _sqlComando = null;
            List<SAPCuentaContableDTO> _listaCuentaContableDto = null;
            string sql = sqlSelect;

            _sqlComando = new SqlCommand(sql, _sqlConn);
            _sqlComando.CommandType = CommandType.Text;

            if (_sqlTran != null)
                _sqlComando.Transaction = _sqlTran;

            _sqlComando.Parameters.Add(new SqlParameter("@CuentaContable", (object)cuentaContable ?? DBNull.Value));

            _listaCuentaContableDto = CargarReader(_sqlComando.ExecuteReader());

            return _listaCuentaContableDto.Count > 0 ? _listaCuentaContableDto[0] : null;
        }

        public Int32 EjecutarSentenciaInsert(SAPCuentaContableDTO _cuentaContableDto)
        {
            SqlCommand _sqlComando = null;

            _sqlComando = new SqlCommand(sqlInsert, _sqlConn);
            _sqlComando.CommandType = CommandType.Text;

            if (_sqlTran != null)
                _sqlComando.Transaction = _sqlTran;

            _sqlComando.Parameters.Add(new SqlParameter("@BUKRS", (object)_cuentaContableDto.BUKRS ?? DBNull.Value));
            _sqlComando.Parameters.Add(new SqlParameter("@KTOKS", (object)_cuentaContableDto.KTOKS ?? DBNull.Value));
            _sqlComando.Parameters.Add(new SqlParameter("@TXT30", (object)_cuentaContableDto.TXT30 ?? DBNull.Value));
            _sqlComando.Parameters.Add(new SqlParameter("@SAKNR", (object)_cuentaContableDto.SAKNR ?? DBNull.Value));
            _sqlComando.Parameters.Add(new SqlParameter("@TXT50", (object)_cuentaContableDto.TXT50 ?? DBNull.Value));

            return _sqlComando.ExecuteNonQuery();
        }


        public Int32 EjecutarSentenciaUpdate()
        {
            throw new NotImplementedException("");
        }

        public Int32 EjecutarSentenciaDelete(string codigoSociedad)
        {
            SqlCommand _sqlComando = null;
            string sql = sqlDelete + " where BUKRS = @BUKRS ";

            _sqlComando = new SqlCommand(sql, _sqlConn);
            _sqlComando.CommandType = CommandType.Text;

            if (_sqlTran != null)
                _sqlComando.Transaction = _sqlTran;

            _sqlComando.Parameters.Add(new SqlParameter("@BUKRS", (object)codigoSociedad ?? DBNull.Value));

            return _sqlComando.ExecuteNonQuery();
        }

        protected List<SAPCuentaContableDTO> CargarReader(SqlDataReader sqlReader)
        {
            SAPCuentaContableDTO _cuentaContableDto = null;
            List<SAPCuentaContableDTO> _listaCuentaContableDto = new List<SAPCuentaContableDTO>();

            while (sqlReader.Read())
            {
                _cuentaContableDto = new SAPCuentaContableDTO();

                _cuentaContableDto.BUKRS = sqlReader.GetString(0);
                _cuentaContableDto.KTOKS = sqlReader.GetString(1);
                _cuentaContableDto.TXT30 = sqlReader.GetString(2);
                _cuentaContableDto.SAKNR = sqlReader.GetString(3);
                _cuentaContableDto.TXT50 = sqlReader.GetString(4);
                //_cuentaContableDto.KOSTL = sqlReader.GetString(5);
                //_cuentaContableDto.VERAK = sqlReader.GetString(6);
                //_cuentaContableDto.KTEXT = sqlReader.GetString(7);

                _listaCuentaContableDto.Add(_cuentaContableDto);
            }
            return _listaCuentaContableDto;
        }

        //public List<LlenarDDL_DTO> BuscarCuentasContablesCentroCosto(string centroCosto, string codigoSociedad)
        public List<LlenarDDL_DTO> BuscarCuentasContablesCentroCosto(string usuario, string codigoSociedad, string centroCosto)
        {
            SqlCommand _sqlComando = null;
//            string sql = sqlSelect + @" inner join sap.CentroCostoTMP b on b.BUKRS = a.BUKRS
//                                        inner join sap.CuentaPorCentroCostoTMP c on c.BUKRS = b.BUKRS 
//                                                                                and c.KOSTL = b.KOSTL
//                                        where ((a.SAKNR between HKONT_LOW and HKONT_HIGH) OR SAKNR = '2110601004')
//                                        and RTRIM(LTRIM(b.BKZKP)) = ''
//                                        and a.BUKRS = @BUKRS
//                                        and b.KOSTL = @KOSTL 
//                                        group by TXT50, SAKNR ";
//---------------INI-------------SATB-03.08.2017-----------------------------------------------
//---------------Consulta para obtener unicamente las cuentas mapeadas al usuario--------------
            string sql = @" SELECT Mapeo.SAKNR, (Cuentas.SAKNR + ' :: ' + Cuentas.TXT50) as Definicion
                                        FROM MapeoUsuarioCECOCuenta as Mapeo
                                        INNER JOIN sap.CuentasContables as Cuentas
                                                ON Cuentas.SAKNR = Mapeo.SAKNR
                                               AND Cuentas.BUKRS = Mapeo.BUKRS
                                        INNER JOIN sap.CuentaPorCentroCostoTMP as CuetnaCECO
												ON CuetnaCECO.BUKRS = Mapeo.BUKRS
                                        WHERE Mapeo.USUARIO = @Usuario
                                        AND   Mapeo.BUKRS = @BUKRS
                                        AND (Cuentas.SAKNR between HKONT_LOW and HKONT_HIGH)
                                        AND CuetnaCECO.KOSTL = @KOSTL
                                        ORDER BY TXT50";

//---------------INI-------------SATB-03.08.2017-----------------------------------------------
            _sqlComando = new SqlCommand(sql, _sqlConn);
            _sqlComando.CommandType = CommandType.Text;

            if (_sqlTran != null)
                _sqlComando.Transaction = _sqlTran;

            _sqlComando.Parameters.Add(new SqlParameter("@BUKRS", (object)codigoSociedad));
            _sqlComando.Parameters.Add(new SqlParameter("@USUARIO", (object)usuario));
            _sqlComando.Parameters.Add(new SqlParameter("@KOSTL", (object)centroCosto));


            return CargarReaderDDL(_sqlComando.ExecuteReader());
        }

        public List<LlenarDDL_DTO> BuscarCuentasContablesOrdenCosto(string ordenCompra, string codigoSociedad)
        {
            SqlCommand _sqlComando = null;
            string sql = sqlSelect + @" inner join sap.OrdenCOTMP b on b.BUKRS = a.BUKRS
                                        inner join sap.CuentaPorOrdenCOTMP c on c.BUKRS = b.BUKRS
										                                        and c.AUART = b.AUART
                                        where a.SAKNR between c.KSTAR_LOW and c.KSTAR_HIGH
                                        and (b.PHAS1 = 'X' or b.PHAS1 = 'X')
                                        and LTRIM(RTRIM(b.PHAS2)) = ''
                                        and a.BUKRS = @BUKRS
                                        and b.AUFNR = @AUFNR 
                                        and A.SAKNR = (CASE WHEN b.AUART = 'Z400' Or b.AUART = 'Z200' THEN 
                                                        '6901010201'
                                                        ELSE A.SAKNR
                                                        END )
                                        group by TXT50, SAKNR ";
            //-----------------------SATB-08.06.2017--------------------------------------------
            //--Se agrego linea 164 para asignar la cuenta de gasto para las ordenes de Inversion 

            _sqlComando = new SqlCommand(sql, _sqlConn);
            _sqlComando.CommandType = CommandType.Text;

            if (_sqlTran != null)
                _sqlComando.Transaction = _sqlTran;

            _sqlComando.Parameters.Add(new SqlParameter("@BUKRS", (object)codigoSociedad));
            _sqlComando.Parameters.Add(new SqlParameter("@AUFNR", (object)ordenCompra));

            return CargarReaderDDL(_sqlComando.ExecuteReader());
        }

        protected List<LlenarDDL_DTO> CargarReaderDDL(SqlDataReader sqlDataReader)
        {
            List<LlenarDDL_DTO> listaLlenarDDL = new List<LlenarDDL_DTO>();
            LlenarDDL_DTO llenarDDL = null;

            while (sqlDataReader.Read())
            {
                llenarDDL = new LlenarDDL_DTO();

                llenarDDL.IDENTIFICADOR = sqlDataReader.GetString(0);
                llenarDDL.DESCRIPCION = sqlDataReader.GetString(1);

                listaLlenarDDL.Add(llenarDDL);
            }

            return listaLlenarDDL;
        }

        #endregion
    }
}
