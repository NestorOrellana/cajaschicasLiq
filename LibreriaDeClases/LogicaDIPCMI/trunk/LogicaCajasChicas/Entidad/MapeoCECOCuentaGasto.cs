using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using DipCmiGT.LogicaComun;
using DipCmiGT.LogicaCajasChicas;

namespace LogicaCajasChicas.Entidad
{
    public class MapeoCECOCuentaGasto
    {
        #region Declaraciones
        private SqlConnection _sqlConn = null;
        private SqlTransaction _sqlTran = null;

        protected string sqlSelect = @"SELECT m.CodigoMapeo, m.Pais, m.CentroCosto, c.KTEXT, m.OrdenCosto, oc.KTEXT, m.TipoGasto, g.Gasto, m.CuentaContable,
                                        (SELECT TXT50 SAKNR
                                            from sap.CuentasContables a
                                            WHERE SAKNR = m.CuentaContable
                                            group by TXT50
                                        ) CuentaContableStr, m.Alta, m.UsuarioAlta, m.FechaAlta, m.UsuarioModificacion, m.FechaModificacion 
                                        FROM dbo.MapeoCECOCuentaGasto m
                                        INNER JOIN sap.CentroCostoTMP c ON c.KOSTL = m.CentroCosto
                                        INNER JOIN sap.OrdenCOTMP oc ON oc.AUFNR = m.OrdenCosto
                                        INNER JOIN dbo.Gasto g ON g.CodigoGasto = m.TipoGasto";

        protected string sqlInsert = @"INSERT INTO MapeoCECOCuentaGasto (Pais, CentroCosto, OrdenCosto, TipoGasto, CuentaContable, Alta, UsuarioAlta, FechaAlta)
                                    VALUES (@Pais, @CentroCosto, @OrdenCosto, @TipoGasto, @CuentaContable, @Alta, @UsuarioAlta ,getdate()) ";

        protected string sqlUpdate = @"UPDATE MapeoCECOCuentaGasto SET Pais = @Pais, CentroCosto = @CentroCosto, OrdenCosto = @OrdenCosto, TipoGasto = @TipoGasto,
                                    CuentaContable = @CuentaContable, 
                                    Alta = @Alta, UsuarioModificacion = @UsuarioModificacion, FechaModificacion = getdate()
                                    WHERE CodigoMapeo = @CodigoMapeo";
        #endregion

        #region Constructores
        public MapeoCECOCuentaGasto(SqlConnection sqlConn)
        {
            _sqlConn = sqlConn;
        }

        public MapeoCECOCuentaGasto(SqlConnection sqlConn, SqlTransaction sqlTran)
        {
            _sqlConn = sqlConn;
            _sqlTran = sqlTran;
        }
        #endregion
        #region Metodos
        public Int32 EjecutarSentenciaInsert(MapeoCECOCuentaGastoDTO mapeoDTO)
        {
            SqlCommand sqlComando;

            sqlComando = new SqlCommand(sqlInsert, _sqlConn);
            sqlComando.CommandType = CommandType.Text;

            if (_sqlTran != null)
                sqlComando.Transaction = _sqlTran;

            sqlComando.Parameters.Add(new SqlParameter("@Pais", (object)mapeoDTO.Pais.ToUpper() ?? DBNull.Value));
            sqlComando.Parameters.Add(new SqlParameter("@CentroCosto", (object)mapeoDTO.CentroCosto ?? DBNull.Value));
            sqlComando.Parameters.Add(new SqlParameter("@OrdenCosto", (object)mapeoDTO.OrdenCosto ?? DBNull.Value));
            sqlComando.Parameters.Add(new SqlParameter("@TipoGasto", (object)mapeoDTO.TipoGasto ?? DBNull.Value));
            sqlComando.Parameters.Add(new SqlParameter("@CuentaContable", (object)mapeoDTO.CuentaContable ?? DBNull.Value));
            sqlComando.Parameters.Add(new SqlParameter("@Alta", (object)mapeoDTO.Alta ?? DBNull.Value));
            sqlComando.Parameters.Add(new SqlParameter("@UsuarioAlta", (object)mapeoDTO.USUARIO_MANTENIMIENTO.USUARIO_ALTA.ToUpper() ?? DBNull.Value));

            return Convert.ToInt32(sqlComando.ExecuteScalar());
        }

        public Int32 EjectuarSentenciaUpdate(MapeoCECOCuentaGastoDTO mapeoDTO)
        {
            SqlCommand sqlComando;

            sqlComando = new SqlCommand(sqlUpdate, _sqlConn);
            sqlComando.CommandType = CommandType.Text;

            if (_sqlTran != null)
                sqlComando.Transaction = _sqlTran;

            sqlComando.Parameters.Add(new SqlParameter("@CodigoMapeo", (object)mapeoDTO.CodigoMapeo ?? DBNull.Value));
            sqlComando.Parameters.Add(new SqlParameter("@Pais", (object)mapeoDTO.Pais ?? DBNull.Value));
            sqlComando.Parameters.Add(new SqlParameter("@CentroCosto", (object)mapeoDTO.CentroCosto ?? DBNull.Value));
            sqlComando.Parameters.Add(new SqlParameter("@OrdenCosto", (object)mapeoDTO.OrdenCosto ?? DBNull.Value));
            sqlComando.Parameters.Add(new SqlParameter("@TipoGasto", (object)mapeoDTO.TipoGasto ?? DBNull.Value));
            sqlComando.Parameters.Add(new SqlParameter("@CuentaContable", (object)mapeoDTO.CuentaContable ?? DBNull.Value));
            sqlComando.Parameters.Add(new SqlParameter("@Alta", (object)mapeoDTO.Alta ?? DBNull.Value));
            sqlComando.Parameters.Add(new SqlParameter("@UsuarioModificacion", (object)mapeoDTO.USUARIO_MANTENIMIENTO.USUARIO_MODIFICO.ToUpper() ?? DBNull.Value));

            return Convert.ToInt32(sqlComando.ExecuteScalar());
        }

        public Int16 DarBajaMapeo(long codigoMapeo, string usuario)
        {
            SqlCommand sqlComando;
            string sql = @"Update MapeoCECOCuentaGasto set Alta = 0, UsuarioModificacion = @UsuarioModificacion, FechaModificacion = getdate()
                            where CodigoMapeo = @CodigoMapeo";

            sqlComando = new SqlCommand(sql, _sqlConn);
            sqlComando.CommandType = CommandType.Text;

            if (_sqlTran != null)
                sqlComando.Transaction = _sqlTran;

            sqlComando.Parameters.Add(new SqlParameter("@UsuarioModificacion", (object)usuario.ToUpper() ?? DBNull.Value));
            sqlComando.Parameters.Add(new SqlParameter("@CodigoMapeo", (object)codigoMapeo ?? DBNull.Value));

            return Convert.ToInt16(sqlComando.ExecuteNonQuery());
        }

        public Int16 DarAltaMapeo(long codigoMapeo, string usuario)
        {
            SqlCommand sqlComando;
            string sql = @"Update MapeoCECOCuentaGasto set Alta = 1, UsuarioModificacion = @UsuarioModificacion, FechaModificacion = getdate()
                            where CodigoMapeo = @CodigoMapeo";

            sqlComando = new SqlCommand(sql, _sqlConn);
            sqlComando.CommandType = CommandType.Text;

            if (_sqlTran != null)
                sqlComando.Transaction = _sqlTran;

            sqlComando.Parameters.Add(new SqlParameter("@UsuarioModificacion", (object)usuario.ToUpper() ?? DBNull.Value));
            sqlComando.Parameters.Add(new SqlParameter("@CodigoMapeo", (object)codigoMapeo ?? DBNull.Value));

            return Convert.ToInt16(sqlComando.ExecuteNonQuery());
        }

        public List<MapeoCECOCuentaGastoDTO> ListaMapeos()
        {
            SqlCommand sqlComando;
            string sql = sqlSelect;

            sqlComando = new SqlCommand(sql, _sqlConn);
            if (_sqlTran != null)
                sqlComando.Transaction = _sqlTran;

            return CargarReader(sqlComando.ExecuteReader());
        }

        public List<LlenarDDL_DTO> ListaPaisesDDL()
        {
            SqlCommand sqlComando;
            string sql = @"select distinct Pais from [dbo].[Sociedad]";

            sqlComando = new SqlCommand(sql, _sqlConn);

            if (_sqlTran != null)
                sqlComando.Transaction = _sqlTran;

            return CargarReaderPaisesDDL(sqlComando.ExecuteReader());
        }

        public List<LlenarDDL_DTO> ListaCentrosCosto(string pais)
        {
            SqlCommand sqlComando;
            string sql = @"SELECT KOSTL, (KOSTL + '::' + KTEXT) FROM  sap.CentroCostoTMP WHERE BUKRS IN(select CodigoSociedad from dbo.sociedad where pais = @Pais)";

            sqlComando = new SqlCommand(sql, _sqlConn);

            if (_sqlTran != null)
                sqlComando.Transaction = _sqlTran;

            sqlComando.Parameters.Add(new SqlParameter("@Pais", (object)pais ?? DBNull.Value));

            return CargarReaderDDL(sqlComando.ExecuteReader());
        }

        public List<LlenarDDL_DTO> ListaOrdenCosto(string pais)
        {
            SqlCommand sqlComando;
            string sql = @"SELECT AUFNR, (AUFNR + '::' + KTEXT) FROM sap.OrdenCOTMP WHERE BUKRS IN(
	                        select CodigoSociedad from dbo.sociedad
	                        where pais = @Pais)";

            sqlComando = new SqlCommand(sql, _sqlConn);

            if (_sqlTran != null)
                sqlComando.Transaction = _sqlTran;

            sqlComando.Parameters.Add(new SqlParameter("@Pais", (object)pais ?? DBNull.Value));

            return CargarReaderDDL(sqlComando.ExecuteReader());
        }

        public List<LlenarDDL_DTO> ListaCuentaContable(string pais)
        {
            SqlCommand sqlComando;
            string sql = @"SELECT SAKNR, (SAKNR + ' :: ' + TXT50) SAKNR
                            from sap.CuentasContables a
                            inner join sap.CentroCostoTMP b on b.KOSTL = @Pais
                            group by TXT50, SAKNR";

            sqlComando = new SqlCommand(sql, _sqlConn);

            if (_sqlTran != null)
                sqlComando.Transaction = _sqlTran;

            sqlComando.Parameters.Add(new SqlParameter("@Pais", (object)pais ?? DBNull.Value));

            return CargarReaderDDL(sqlComando.ExecuteReader());
        }

        public List<LlenarDDL_DTO> ListaGastos(string pais)
        {
            SqlCommand sqlComando;
            /*string sql = @"SELECT a.TipoGasto, b.Gasto FROM dbo.CatLiquidacion a 
                            INNER JOIN dbo.Gasto b on b.CodigoGasto = a.TipoGasto
                            WHERE a.Alta = 1 AND a.Pais = @Pais";*/
            string sql = @"select CodigoGasto, Gasto from [dbo].[Gasto] WHERE Pais = @Pais AND Alta = 1";

            sqlComando = new SqlCommand(sql, _sqlConn);

            if (_sqlTran != null)
                sqlComando.Transaction = _sqlTran;

            sqlComando.Parameters.Add(new SqlParameter("@Pais", (object)pais ?? DBNull.Value));

            return CargarReaderPaisesDDL(sqlComando.ExecuteReader(), false);
        }

        protected List<MapeoCECOCuentaGastoDTO> CargarReader(SqlDataReader sqlReader)
        {
            MapeoCECOCuentaGastoDTO _mapeoDTO = null;
            List<MapeoCECOCuentaGastoDTO> _listaMapeos = new List<MapeoCECOCuentaGastoDTO>();

            try
            {
                while (sqlReader.Read())
                {
                    _mapeoDTO = new MapeoCECOCuentaGastoDTO();

                    _mapeoDTO.CodigoMapeo = sqlReader.GetInt64(0);
                    _mapeoDTO.Pais = sqlReader.GetString(1);
                    _mapeoDTO.CentroCosto = sqlReader.GetString(2);
                    _mapeoDTO.CentroCostoStr = sqlReader.GetString(3);
                    _mapeoDTO.OrdenCosto = sqlReader.GetString(4);
                    _mapeoDTO.OrdenCostoStr = sqlReader.GetString(5);
                    _mapeoDTO.TipoGasto = sqlReader.GetInt32(6);
                    _mapeoDTO.TipoGastoStr = sqlReader.GetString(7);
                    _mapeoDTO.CuentaContable = sqlReader.GetString(8);
                    _mapeoDTO.CuentaContableStr = sqlReader.GetString(9);
                    _mapeoDTO.Alta = sqlReader.GetBoolean(10);
                    _mapeoDTO.USUARIO_MANTENIMIENTO.USUARIO_ALTA = sqlReader.GetString(11);
                    _mapeoDTO.USUARIO_MANTENIMIENTO.FECHA_ALTA = sqlReader.GetDateTime(12);
                    _mapeoDTO.USUARIO_MANTENIMIENTO.USUARIO_MODIFICO = sqlReader.IsDBNull(13) ? null : sqlReader.GetString(13);
                    _mapeoDTO.USUARIO_MANTENIMIENTO.FECHA_MODIFICACION = sqlReader.IsDBNull(14) ? (DateTime?)null : sqlReader.GetDateTime(14);

                    _listaMapeos.Add(_mapeoDTO);
                }
            }
            finally
            {
                if (sqlReader != null) sqlReader.Close();
            }
            return _listaMapeos;
        }

        protected List<LlenarDDL_DTO> CargarReaderPaisesDDL(SqlDataReader sqlDataReader, bool paises = true)
        {
            List<LlenarDDL_DTO> listaLlenarDDL = new List<LlenarDDL_DTO>();
            LlenarDDL_DTO llenarDDL = null;

            while (sqlDataReader.Read())
            {
                llenarDDL = new LlenarDDL_DTO();


                if (paises)
                {
                    llenarDDL.IDENTIFICADOR = sqlDataReader.GetString(0);
                    llenarDDL.DESCRIPCION = sqlDataReader.GetString(0);
                }
                else
                {
                    llenarDDL.IDENTIFICADOR = sqlDataReader.GetInt32(0).ToString();
                    llenarDDL.DESCRIPCION = sqlDataReader.GetString(1);
                }

                listaLlenarDDL.Add(llenarDDL);
            }

            return listaLlenarDDL;
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

        public bool ExisteMapeo(string pais, string centroCosto, string ordenCosto, int tipoGasto, string cuentaContable)
        {
            SqlCommand sqlComando;
            string sql = @"select COUNT(*)
                            from MapeoCECOCuentaGasto
                            where Pais = @Pais AND 
                            CentroCosto = @CentroCosto AND 
                            OrdenCosto = @OrdenCosto AND
                            TipoGasto = @TipoGasto AND 
                            CuentaContable = @CuentaContable";

            sqlComando = new SqlCommand(sql, _sqlConn);

            if (_sqlTran != null)
                sqlComando.Transaction = _sqlTran;
            sqlComando.Parameters.Add(new SqlParameter("@Pais", (object)pais ?? DBNull.Value));
            sqlComando.Parameters.Add(new SqlParameter("@CentroCosto", (object)centroCosto ?? DBNull.Value));
            sqlComando.Parameters.Add(new SqlParameter("@OrdenCosto", (object)ordenCosto ?? DBNull.Value));
            sqlComando.Parameters.Add(new SqlParameter("@TipoGasto", (object)tipoGasto ?? DBNull.Value));
            sqlComando.Parameters.Add(new SqlParameter("@CuentaContable", (object)cuentaContable ?? DBNull.Value));

            return Convert.ToInt32(sqlComando.ExecuteScalar()) > 0 ? true : false;
        }
        #endregion
    }
}
