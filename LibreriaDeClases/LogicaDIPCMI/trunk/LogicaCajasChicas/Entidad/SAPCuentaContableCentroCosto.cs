using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using DipCmiGT.LogicaComun;


namespace LogicaCajasChicas.Entidad
{
    /// <summary>
    /// Tabla de Cuentas que  pueden imputar en el centro de costo : Z_STR_Z02_GR
    /// </summary>
    public class SAPCuentaContableCentroCosto
    {
        #region Declaraciones

        private SqlConnection _sqlConn = null;
        private SqlTransaction _sqlTran = null;

        protected string sqlSelect = @" select a.KOKRS, a.KOSTL, KTOPL, HKONT_LOW, HKONT_HIGH, b.*
                                        from sap.CuentaPorCentroCostoTMP a
                                        inner join sap.CentroCostoTMP b on b.KOSTL = a.KOSTL
								                                        and b.KOKRS = a.KOKRS 
                                                                        and a.BUKRS = B.BUKRS ";

        protected string sqlInsert = @" INSERT INTO [sap].[CuentaPorCentroCostoTMP] (KOKRS,KOSTL,KTOPL,HKONT_LOW,HKONT_HIGH, BUKRS) 
                                                                             VALUES (@KOKRS,@KOSTL,@KTOPL,@HKONT_LOW,@HKONT_HIGH, @BUKRS) ";

        protected string sqlDelete = @" delete from sap.CuentaPorCentroCostoTMP ";

        #endregion

        #region Constructores

        public SAPCuentaContableCentroCosto(SqlConnection sqlConn)
        {
            _sqlConn = sqlConn;
        }

        public SAPCuentaContableCentroCosto(SqlConnection sqlConn, SqlTransaction sqlTran)
        {
            _sqlConn = sqlConn;
            _sqlTran = sqlTran;
        }

        #endregion

        #region Metodos

        public SAPCuentaContableCentroCostoDTO EjecutarSentenciaSelect()
        {
            throw new NotImplementedException("");
        }

        public Int32 EjecutarSentenciaInsert(SAPCuentaContableCentroCostoDTO _cuentaContableCentroCostoDto)
        {
            SqlCommand _sqlComando = null;

            _sqlComando = new SqlCommand(sqlInsert, _sqlConn);
            _sqlComando.CommandType = CommandType.Text;

            if (_sqlTran != null)
                _sqlComando.Transaction = _sqlTran;

            _sqlComando.Parameters.Add(new SqlParameter("@KOKRS", (object)_cuentaContableCentroCostoDto.KOKRS ?? DBNull.Value));
            _sqlComando.Parameters.Add(new SqlParameter("@KOSTL", (object)_cuentaContableCentroCostoDto.KOSTL ?? DBNull.Value));
            _sqlComando.Parameters.Add(new SqlParameter("@KTOPL", (object)_cuentaContableCentroCostoDto.KTOPL ?? DBNull.Value));
            _sqlComando.Parameters.Add(new SqlParameter("@HKONT_LOW", (object)_cuentaContableCentroCostoDto.HKONT_LOW ?? DBNull.Value));
            _sqlComando.Parameters.Add(new SqlParameter("@HKONT_HIGH", (object)_cuentaContableCentroCostoDto.HKONT_HIGH ?? DBNull.Value));
            _sqlComando.Parameters.Add(new SqlParameter("@BUKRS", (object)_cuentaContableCentroCostoDto.BUKRS ?? DBNull.Value));

            return _sqlComando.ExecuteNonQuery();
        }


        public Int32 EjecutarSentenciaUpdate()
        {
            throw new NotImplementedException("");
        }

        public Int32 EjecutarSentenciaDelete(string codigoSociedad)
        {
            SqlCommand _sqlCommando = null;
            string sql = sqlDelete + "WHERE BUKRS = @BUKRS AND HKONT_LOW not like  '21%'";
            //<> '2110601004'";

            _sqlCommando = new SqlCommand(sql, _sqlConn);
            _sqlCommando.CommandType = CommandType.Text;

            if (_sqlTran != null)
                _sqlCommando.Transaction = _sqlTran;

            _sqlCommando.Parameters.Add(new SqlParameter("@BUKRS", (object)codigoSociedad ?? DBNull.Value));

            return _sqlCommando.ExecuteNonQuery();
        }

        protected List<SAPCuentaContableCentroCostoDTO> CargarReader(SqlDataReader sqlReader)
        {
            SAPCuentaContableCentroCostoDTO _cuentaContableCentroCostoDto = null;
            List<SAPCuentaContableCentroCostoDTO> _listaCuentaContableCentroCostoDto = new List<SAPCuentaContableCentroCostoDTO>();

            while (sqlReader.Read())
            {
                _cuentaContableCentroCostoDto = new SAPCuentaContableCentroCostoDTO();
                _cuentaContableCentroCostoDto.KOKRS = sqlReader.GetString(0);
                _cuentaContableCentroCostoDto.KOSTL = sqlReader.GetString(1);
                _cuentaContableCentroCostoDto.KTOPL = sqlReader.GetString(2);
                _cuentaContableCentroCostoDto.HKONT_LOW = sqlReader.GetString(3);
                _cuentaContableCentroCostoDto.HKONT_HIGH = sqlReader.GetString(4);

                _listaCuentaContableCentroCostoDto.Add(_cuentaContableCentroCostoDto);
            }
            return _listaCuentaContableCentroCostoDto;
        }

        public List<SAPCuentaContableCentroCostoDTO> ListaRCuentaContableCentroCosto(string centroCosto, string cuentaContable)
        {
            SqlCommand _sqlComando = null;
            string sql = sqlSelect + @" where KOSTL = @KOSTL
                                        and @CuentaContable between HKONT_LOW and HKONT_HIGH ";

            _sqlComando = new SqlCommand(sql, _sqlConn);
            _sqlComando.CommandType = CommandType.Text;

            if (_sqlTran != null)
                _sqlComando.Transaction = _sqlTran;

            _sqlComando.Parameters.Add(new SqlParameter("@KOSTL", (object)centroCosto ?? DBNull.Value));
            _sqlComando.Parameters.Add(new SqlParameter("@CuentaContable", (object)cuentaContable ?? DBNull.Value));

            return CargarReader(_sqlComando.ExecuteReader());
        }

        public List<LlenarDDL_DTO> ListarCentrosCosto(string codigoSociedad)
        {
            List<LlenarDDL_DTO> _listaLlenarDDL = new List<LlenarDDL_DTO>();
            LlenarDDL_DTO _llenarDDL = null;
            SqlCommand _sqlComando = null;
            string sql = @" select DISTINCT a.KOSTL, a.KOSTL + ' - ' + KTEXT
                            from sap.CuentaPorCentroCostoTMP a
                            inner join sap.CentroCostoTMP b on b.KOSTL = a.KOSTL
								                            and b.KOKRS = a.KOKRS
                            and b.BUKRS = @BUKRS ";

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

        public List<LlenarDDL_DTO> ListarCentrosCosto(string codigoSociedad, string centroCosto)
        {
            List<LlenarDDL_DTO> _listaLlenarDDL = new List<LlenarDDL_DTO>();
            LlenarDDL_DTO _llenarDDL = null;
            SqlCommand _sqlComando = null;
            //            string sql = @" select DISTINCT a.KOSTL, a.KOSTL + ' - ' + KTEXT
            //                            from sap.CuentaPorCentroCostoTMP a
            //                            inner join sap.CentroCostoTMP b on b.KOSTL = a.KOSTL
            //								                            and b.KOKRS = a.KOKRS
            //                            and b.BUKRS = @BUKRS 
            //                            and b.kostl in (@CeCos) ";

            string sql = "ListarCentrosCosto";

            _sqlComando = new SqlCommand(sql, _sqlConn);
            _sqlComando.CommandType = CommandType.StoredProcedure;

            if (_sqlTran != null)
                _sqlComando.Transaction = _sqlTran;

            _sqlComando.Parameters.Add(new SqlParameter("@BUKRS", (object)codigoSociedad ?? DBNull.Value));
            _sqlComando.Parameters.Add(new SqlParameter("@CeCos", (string)centroCosto ?? (object)DBNull.Value));

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
