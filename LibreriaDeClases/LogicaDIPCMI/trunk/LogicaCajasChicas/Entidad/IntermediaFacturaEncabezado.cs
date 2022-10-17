using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using DipCmiGT.LogicaComun;

namespace LogicaCajasChicas.Entidad
{
    class IntermediaFacturaEncabezado
    {
        #region Declaraciones

        private SqlConnection _sqlConn = null;
        private SqlTransaction _sqlTran = null;


        protected string sqlInsert = @" INSERT INTO rf.CajaChicaEncabezado (BUKRS, DOCUMENT, BLDAT, TYPE, BUDAT, XBLNR, BKTXT, BLART, CURRENCY, KURSF, RECORDMODE, NAME, NAME2, NAME3, NAME4, ORT01, STCD1, STCD2, DUMMY, ZSTCDT)
                                                                    VALUES (@BUKRS, @DOCUMENT, @BLDAT, @TYPE, @BUDAT, @XBLNR, @BKTXT, @BLART, @CURRENCY, @KURSF, @RECORDMODE, @NAME, @NAME2, @NAME3, @NAME4, @ORT01, @STCD1, @STCD2, @DUMMY, @ZSTCDT) ";

        #endregion

        #region Constructores


        public IntermediaFacturaEncabezado(SqlConnection sqlConn)
        {
            _sqlConn = sqlConn;
        }

        public IntermediaFacturaEncabezado(SqlConnection sqlConn, SqlTransaction sqlTran)
        {
            _sqlConn = sqlConn;
            _sqlTran = sqlTran;
        }

        #endregion

        #region Metodos

        public IntermediaFacturaEncabezadoDTO EjecutarSentenciaSelect()
        {
            throw new NotImplementedException("");
        }


        public Int32 EjecutarSenteciaInsert(IntermediaFacturaEncabezadoDTO intermediaFacturaEncabezadoDTO)
        {
            SqlCommand _sqlComando = null;
            string sql = sqlInsert;

            _sqlComando = new SqlCommand(sql, _sqlConn);
            _sqlComando.CommandType = CommandType.Text;

            if (_sqlTran != null)
                _sqlComando.Transaction = _sqlTran;

            _sqlComando.Parameters.Add(new SqlParameter("@BUKRS", (object)intermediaFacturaEncabezadoDTO.BUKRS ?? DBNull.Value));
            _sqlComando.Parameters.Add(new SqlParameter("@DOCUMENT", (object)intermediaFacturaEncabezadoDTO.DOCUMENT ?? DBNull.Value));
            _sqlComando.Parameters.Add(new SqlParameter("@BLDAT", (object)intermediaFacturaEncabezadoDTO.BLDAT ?? DBNull.Value));
            _sqlComando.Parameters.Add(new SqlParameter("@TYPE", (object)intermediaFacturaEncabezadoDTO.TYPE ?? DBNull.Value));
            _sqlComando.Parameters.Add(new SqlParameter("@BUDAT", (object)intermediaFacturaEncabezadoDTO.BUDAT ?? DBNull.Value));
            _sqlComando.Parameters.Add(new SqlParameter("@XBLNR", (object)intermediaFacturaEncabezadoDTO.XBLNR ?? DBNull.Value));
            _sqlComando.Parameters.Add(new SqlParameter("@BKTXT", (object)intermediaFacturaEncabezadoDTO.BKTXT ?? DBNull.Value));
            _sqlComando.Parameters.Add(new SqlParameter("@BLART", (object)intermediaFacturaEncabezadoDTO.BLART ?? DBNull.Value));
            _sqlComando.Parameters.Add(new SqlParameter("@CURRENCY", (object)intermediaFacturaEncabezadoDTO.CURRENCY ?? DBNull.Value));
            _sqlComando.Parameters.Add(new SqlParameter("@KURSF", (object)intermediaFacturaEncabezadoDTO.KURSF ?? DBNull.Value));
            _sqlComando.Parameters.Add(new SqlParameter("@RECORDMODE", (object)intermediaFacturaEncabezadoDTO.RECORDMODE ?? DBNull.Value));
            _sqlComando.Parameters.Add(new SqlParameter("@NAME", (object)intermediaFacturaEncabezadoDTO.NAME ?? DBNull.Value));
            _sqlComando.Parameters.Add(new SqlParameter("@NAME2", (object)intermediaFacturaEncabezadoDTO.NAME2 ?? DBNull.Value));
            _sqlComando.Parameters.Add(new SqlParameter("@NAME3", (object)intermediaFacturaEncabezadoDTO.NAME3 ?? DBNull.Value));
            _sqlComando.Parameters.Add(new SqlParameter("@NAME4", (object)intermediaFacturaEncabezadoDTO.NAME4 ?? DBNull.Value));
            _sqlComando.Parameters.Add(new SqlParameter("@ORT01", (object)intermediaFacturaEncabezadoDTO.ORT01 ?? DBNull.Value));
            _sqlComando.Parameters.Add(new SqlParameter("@STCD1", (object)intermediaFacturaEncabezadoDTO.STCD1 ?? DBNull.Value));
            _sqlComando.Parameters.Add(new SqlParameter("@STCD2", (object)intermediaFacturaEncabezadoDTO.STCD2 ?? DBNull.Value));
            _sqlComando.Parameters.Add(new SqlParameter("@DUMMY", (object)intermediaFacturaEncabezadoDTO.DUMMY ?? DBNull.Value));
            _sqlComando.Parameters.Add(new SqlParameter("@ZSTCDT", (object)intermediaFacturaEncabezadoDTO.ZSTCDT ?? DBNull.Value));

            return _sqlComando.ExecuteNonQuery();
        }

        public Int32 Prueba()
        {
            SqlCommand _sqlComando = null;
            string sql = sqlInsert;

            _sqlComando = new SqlCommand(sql, _sqlConn);
            _sqlComando.CommandType = CommandType.Text;

            if (_sqlTran != null)
                _sqlComando.Transaction = _sqlTran;

            return _sqlComando.ExecuteNonQuery();
        }
        public Int32 EjecutarSentenciaUpdate()
        {
            throw new NotImplementedException("");
        }

        protected List<IntermediaFacturaEncabezadoDTO> CargarReader(SqlDataReader sqlReader)
        {
            throw new NotImplementedException("");
        }

        public Int64 BuscarCorrelativo(string codigoSociedad)
        {
            SqlCommand _sqlComando = null;
            string sql = @" select ISNULL(max(document),0) correlativo
                            from rf.CajaChicaEncabezado
                            where bukrs = @CodigoSociedad ";

            _sqlComando = new SqlCommand(sql, _sqlConn);
            _sqlComando.CommandType = CommandType.Text;

            if (_sqlTran != null)
                _sqlComando.Transaction = _sqlTran;

            _sqlComando.Parameters.Add(new SqlParameter("@CodigoSociedad", (object)codigoSociedad ?? DBNull.Value));

            return Convert.ToInt64(_sqlComando.ExecuteScalar());
        }

        public bool ExisteRegistro(string burks, Int64 document, string bldat, string type)
        {
            SqlCommand _sqlComando = null;
            string sql = @" select count(document) 
                            from rf.CajaChicaEncabezado
                            where bukrs = @bukrs 
                            and document = @Document
                            and bldat = @bldat
                            and type = @type";

            _sqlComando = new SqlCommand(sql, _sqlConn);
            _sqlComando.CommandType = CommandType.Text;

            if (_sqlTran != null)
                _sqlComando.Transaction = _sqlTran;

            _sqlComando.Parameters.Add(new SqlParameter("@bukrs", (object)burks ?? DBNull.Value));
            _sqlComando.Parameters.Add(new SqlParameter("@Document", (object)document ?? DBNull.Value));
            _sqlComando.Parameters.Add(new SqlParameter("@bldat", (object)bldat ?? DBNull.Value));
            _sqlComando.Parameters.Add(new SqlParameter("@type", (object)type ?? DBNull.Value));

            return Convert.ToInt32(_sqlComando.ExecuteScalar()) == 0 ? true : false;
        }

        //Cuando es factura con marca de dividida, buscar en la tabla intermedia si ya existe un registro para no insertar 2 veces el encabezado 
        //o la posicion del detalle 
        //public Int32 BuscarFacturaDividida(string codigoSociedad, decimal NumeroFact, string nitProveedor, string serie)
        public Int32 BuscarFacturaDividida(string codigoSociedad, string NumeroFact, string nitProveedor, string serie)
        {
            SqlCommand _sqlComando = null;
//            string sql = @"SELECT ISNULL(DOCUMENT,0) FROM rf.CajaChicaEncabezado
//                           WHERE BUKRS = @BUKRS
//                           AND XBLNR = @XBLNR
//                           AND STCD1 = @STCD1
//                           AND ZUONR = @ZUONR";

            string sql = @"SELECT ISNULL(Enc.DOCUMENT,0) FROM rf.CajaChicaEncabezado as Enc
						  INNER JOIN rf.CajaChicaDetalle as Det
						  ON	Enc.BUKRS = Det.BUKRS
						  AND	Enc.DOCUMENT = det.DOCUMENT
						  AND	Enc.BLDAT = det.BLDAT
                          WHERE Enc.BUKRS = @BUKRS
                          AND Enc.XBLNR = @XBLNR
                          AND Enc.STCD1 = @STCD1
                          AND Det.ZUONR = @ZUONR";

            _sqlComando = new SqlCommand(sql, _sqlConn);
            _sqlComando.CommandType = CommandType.Text;

            if (_sqlTran != null)
                _sqlComando.Transaction = _sqlTran;

            _sqlComando.Parameters.Add(new SqlParameter("@BUKRS", (object)codigoSociedad ?? DBNull.Value));
            _sqlComando.Parameters.Add(new SqlParameter("@XBLNR", (object)Convert.ToString(NumeroFact) ?? DBNull.Value));
            _sqlComando.Parameters.Add(new SqlParameter("@STCD1", (object)nitProveedor ?? DBNull.Value));
            _sqlComando.Parameters.Add(new SqlParameter("@ZUONR", (object)serie ?? DBNull.Value));

            return Convert.ToInt32(_sqlComando.ExecuteScalar());
        }

        public Int32 ObtenerBUZEIFactDiv(string codigoSociedad, Int32 Documento)
        {
            SqlCommand _sqlComando = null;
            string sql = @"SELECT ISNULL(MAX(BUZEI),0) 
                           FROM rf.CajaChicaDetalle
                           WHERE BUKRS	= @BUKRS
                           AND   DOCUMENT = @DOCUMENT";

            _sqlComando = new SqlCommand(sql, _sqlConn);
            _sqlComando.CommandType = CommandType.Text;

            if (_sqlTran != null)
                _sqlComando.Transaction = _sqlTran;

            _sqlComando.Parameters.Add(new SqlParameter("@BUKRS", (object)codigoSociedad ?? DBNull.Value));
            _sqlComando.Parameters.Add(new SqlParameter("@DOCUMENT", (object)Documento?? DBNull.Value));

            return Convert.ToInt32(_sqlComando.ExecuteScalar());
        }
        #endregion
    }
}
