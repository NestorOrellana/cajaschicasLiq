using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using DipCmiGT.LogicaComun;
using System.Data;

namespace LogicaCajasChicas.Entidad
{
    /// <summary>
    /// Tabla de clases de costo que pueden imputar a la orden de gastos: Z_ZSTR_Z02_GR_ACC_AUFNR
    /// </summary>
    public class SAPCuentasOrdenesGastos
    {
        #region Declaraciones

        private SqlConnection _sqlConn = null;
        private SqlTransaction _sqlTran = null;

        protected string sqlSelect = @" SELECT KOKRS, BUKRS, AUART, KTOPL, KSTAR_LOW, KSTAR_HIGH
                                        FROM sap.CuentaPorOrdenCOTMP ";

        protected string sqlInsert = @" INSERT INTO [sap].[CuentaPorOrdenCOTMP] (KOKRS,BUKRS,AUART,KTOPL,KSTAR_LOW,KSTAR_HIGH) 
                                                                         values (@KOKRS,@BUKRS,@AUART,@KTOPL,@KSTAR_LOW,@KSTAR_HIGH) ";

        protected string sqlDelete = @" DELETE from sap.CuentaPorOrdenCOTMP ";

        #endregion

        #region Constructores

        public SAPCuentasOrdenesGastos(SqlConnection sqlConn)
        {
            _sqlConn = sqlConn;
        }

        public SAPCuentasOrdenesGastos(SqlConnection sqlConn, SqlTransaction sqlTran)
        {
            _sqlConn = sqlConn;
            _sqlTran = sqlTran;
        }

        #endregion

        #region Metodos

        public SAPCuentasOrdenesGastosDTO EjecutarSentenciaSelect()
        {
            throw new NotImplementedException("Método no implementado.");
        }

        public Int32 EjecutarSentenciaInsert(SAPCuentasOrdenesGastosDTO cuentasOrdenesGastosDto)
        {
            SqlCommand _sqlComando = null;

            _sqlComando = new SqlCommand(sqlInsert, _sqlConn);

            if (_sqlTran != null)
                _sqlComando.Transaction = _sqlTran;

            _sqlComando.Parameters.Add(new SqlParameter("KOKRS", (object)cuentasOrdenesGastosDto.KOKRS ?? DBNull.Value));
            _sqlComando.Parameters.Add(new SqlParameter("BUKRS", (object)cuentasOrdenesGastosDto.BUKRS ?? DBNull.Value));
            _sqlComando.Parameters.Add(new SqlParameter("AUART", (object)cuentasOrdenesGastosDto.AUART ?? DBNull.Value));
            _sqlComando.Parameters.Add(new SqlParameter("KTOPL", (object)cuentasOrdenesGastosDto.KTOPL ?? DBNull.Value));
            _sqlComando.Parameters.Add(new SqlParameter("KSTAR_LOW", (object)cuentasOrdenesGastosDto.KSTAR_LOW ?? DBNull.Value));
            _sqlComando.Parameters.Add(new SqlParameter("KSTAR_HIGH", (object)cuentasOrdenesGastosDto.KSTAR_HIGH ?? DBNull.Value));

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

            if (_sqlTran != null)
                _sqlComando.Transaction = _sqlTran;

            _sqlComando.Parameters.Add(new SqlParameter("@BUKRS", (object)codigoSociedad ?? DBNull.Value));

            return _sqlComando.ExecuteNonQuery();
        }

        protected List<SAPCuentasOrdenesGastosDTO> CargarReader(SqlDataReader sqlReader)
        {
            SAPCuentasOrdenesGastosDTO _cuentaOrdenesGastosDto = null;
            List<SAPCuentasOrdenesGastosDTO> _listaCuentaOrdenesGastosDto = new List<SAPCuentasOrdenesGastosDTO>();

            if (sqlReader.Read())
            {
                _cuentaOrdenesGastosDto = new SAPCuentasOrdenesGastosDTO();

                _cuentaOrdenesGastosDto.KOKRS = sqlReader.GetString(0);
                _cuentaOrdenesGastosDto.BUKRS = sqlReader.GetString(1);
                _cuentaOrdenesGastosDto.AUART = sqlReader.GetString(2);
                _cuentaOrdenesGastosDto.KTOPL = sqlReader.GetString(3);
                _cuentaOrdenesGastosDto.KSTAR_LOW = sqlReader.GetString(4);
                _cuentaOrdenesGastosDto.KSTAR_HIGH = sqlReader.GetString(5);

                _listaCuentaOrdenesGastosDto.Add(_cuentaOrdenesGastosDto);
            }

            return _listaCuentaOrdenesGastosDto;
        }

        public List<SAPCuentasOrdenesGastosDTO> ListarCuentaOrdenesGastos(string sociedad, string cuentaContable)
        {
            SqlCommand _sqlComando = null;
            string sql = sqlSelect + @" where BUKRS = @BUKRS
                                        and @cuentaContable between KSTAR_LOW and KSTAR_HIGH ";

            _sqlComando = new SqlCommand(sql, _sqlConn);

            if (_sqlTran != null)
                _sqlComando.Transaction = _sqlTran;

            _sqlComando.Parameters.Add(new SqlParameter("@BUKRS", (object)sociedad ?? DBNull.Value));
            _sqlComando.Parameters.Add(new SqlParameter("@cuentaContable", (object)cuentaContable ?? DBNull.Value));

            return CargarReader(_sqlComando.ExecuteReader());
        }

        public List<LlenarDDL_DTO> ListarOrdenCosto(string codigoSociedad)
        {
            List<LlenarDDL_DTO> _listaLlenarDDL = new List<LlenarDDL_DTO>();
            LlenarDDL_DTO _llenarDDL = null;
            SqlCommand _sqlComando = null;
            //INI-------------------SATB-08.06.2017------------------------------
            //-------Se agrego Distinct a consulta-------------------------------
            string sql = @" select distinct  AUFNR, AUFNR +' - '+ KTEXT
                            from sap.OrdenCOTMP a
                            inner join sap.CuentaPorOrdenCOTMP b on a.AUART = b.AUART
                            and b.BUKRS = @BUKRS ";
            //FIN------------------SATB-08.06.2017-------------------------------
            _sqlComando = new SqlCommand(sql, _sqlConn);
            _sqlComando.CommandType = CommandType.Text;

            if (_sqlTran != null)
                _sqlComando.Transaction = _sqlTran;

            _sqlComando.Parameters.Add(new SqlParameter("@BUKRS", (object)codigoSociedad ?? DBNull.Value));

            SqlDataReader _sqlDataR = _sqlComando.ExecuteReader();

            while (_sqlDataR.Read())
            {
                _llenarDDL = new LlenarDDL_DTO();

                _llenarDDL.IDENTIFICADOR = _sqlDataR.GetString(0);
                _llenarDDL.DESCRIPCION = _sqlDataR.GetString(1);

                _listaLlenarDDL.Add(_llenarDDL);
            }

            return _listaLlenarDDL;
        }

        public List<LlenarDDL_DTO> ListarOrdenCosto(string codigoSociedad, string ordenCompra)
        {
            List<LlenarDDL_DTO> _listaLlenarDDL = new List<LlenarDDL_DTO>();
            LlenarDDL_DTO _llenarDDL = null;
            SqlCommand _sqlComando = null;
            String buscaOC = "";
            //INI------------------SATB-08.06.2017------------------------------------
            //-----Se cambio SP por consulta, para obtener la Orden de compra---------
            if (ordenCompra != "")
                buscaOC = " and a.AUFNR =  (RIGHT('0000' + Ltrim(Rtrim(@AUFNR)),12))";
            else
                buscaOC = "";
            string sql = @" select DISTINCT AUFNR, AUFNR +' - '+ KTEXT
                                        from sap.OrdenCOTMP a
                                        inner join sap.CuentaPorOrdenCOTMP b on a.AUART = b.AUART
                                        and b.BUKRS = @BUKRS "
                           + buscaOC;
            //string sql = "ListarOrdenCosto";
            //FIN----------------SATB-08.06.2017-------------------------------------
            _sqlComando = new SqlCommand(sql, _sqlConn);
            _sqlComando.CommandType = CommandType.Text;
            //_sqlComando.CommandType = CommandType.StoredProcedure;

            if (_sqlTran != null)
                _sqlComando.Transaction = _sqlTran;

            _sqlComando.Parameters.Add(new SqlParameter("@BUKRS", (object)codigoSociedad ?? DBNull.Value));
            //_sqlComando.Parameters.Add(new SqlParameter("@OrCo", (object)ordenCompra ?? DBNull.Value));
            _sqlComando.Parameters.Add(new SqlParameter("@AUFNR", (object)ordenCompra ?? DBNull.Value));
            SqlDataReader _sqlDataR = _sqlComando.ExecuteReader();

            while (_sqlDataR.Read())
            {
                _llenarDDL = new LlenarDDL_DTO();

                _llenarDDL.IDENTIFICADOR = _sqlDataR.GetString(0);
                _llenarDDL.DESCRIPCION = _sqlDataR.GetString(1);

                _listaLlenarDDL.Add(_llenarDDL);
            }

            return _listaLlenarDDL;
        }


        #endregion
    }
}
