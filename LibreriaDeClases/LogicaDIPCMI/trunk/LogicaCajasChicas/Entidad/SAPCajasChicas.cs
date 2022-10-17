using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using DipCmiGT.LogicaComun;
using System.Data;
using SAP.Middleware.Connector;

namespace LogicaCajasChicas.Entidad
{
    public class SAPCajasChicas
    {
        #region Declaraciones

        private SqlConnection _sqlConn = null;
        private SqlTransaction _sqlTran = null;

        protected string sqlSelect = @" SELECT BUKR, LIFNR, NAME, WITHT, ERDAT
                                        FROM sap.CajaChicaTMP a
                                        INNER JOIN IdentificadoresIVA b on a.WITHT = b.IdentificadorIVA ";

        protected string sqlInsert = @" INSERT INTO sap.CajaChicaTMP  (BUKR, LIFNR, NAME, WITHT, ERDAT) 
                                                                VALUES(@BUKR, @LIFNR, @NAME, @WITHT, @ERDAT) ";

        protected string sqlDelete = @" delete from sap.CajaChicaTMP  ";

        #endregion

        #region Constructores

        public SAPCajasChicas(SqlConnection sqlConn)
        {
            _sqlConn = sqlConn;
        }

        public SAPCajasChicas(SqlConnection sqlConn, SqlTransaction sqlTran)
        {
            _sqlConn = sqlConn;
            _sqlTran = sqlTran;
        }

        #endregion

        #region Metodos

        public SAPCajasChicasDTO EjecutarSentenciaSelect()
        {
            throw new NotImplementedException("Método no implementado.");
        }

        public Int32 EjecutarSentenciaInsert(SAPCajasChicasDTO _sapCajasChicasDto)
        {
            SqlCommand _sqlComando = null;

            _sqlComando = new SqlCommand(sqlInsert, _sqlConn);

            if (_sqlTran != null)
                _sqlComando.Transaction = _sqlTran;

            _sqlComando.Parameters.Add(new SqlParameter("@BUKR", (object)_sapCajasChicasDto.BUKRS ?? DBNull.Value));
            _sqlComando.Parameters.Add(new SqlParameter("@LIFNR", (object)_sapCajasChicasDto.LIFNR ?? DBNull.Value));
            _sqlComando.Parameters.Add(new SqlParameter("@NAME", (object)_sapCajasChicasDto.NAME ?? DBNull.Value));
            _sqlComando.Parameters.Add(new SqlParameter("@WITHT", (object)_sapCajasChicasDto.WITHT ?? DBNull.Value));
            _sqlComando.Parameters.Add(new SqlParameter("@ERDAT", (object)_sapCajasChicasDto.ERDAT ?? DBNull.Value));

            return _sqlComando.ExecuteNonQuery();
        }

        public Int32 EjecutarSentenciaDelete(string codigoSociedad)
        {
            SqlCommand _sqlComando = null;
            string sql = sqlDelete + " WHERE BUKR = @BUKR ";

            _sqlComando = new SqlCommand(sql, _sqlConn);
            _sqlComando.CommandType = CommandType.Text;

            if (_sqlTran != null)
                _sqlComando.Transaction = _sqlTran;

            _sqlComando.Parameters.Add(new SqlParameter("@BUKR", (object)codigoSociedad ?? DBNull.Value));

            return _sqlComando.ExecuteNonQuery();
        }

        public Int32 EjecutarSentenciaUpdate()
        {
            throw new NotImplementedException("Método no implementado.");
        }

        protected List<SAPCajasChicasDTO> CargarReader(SqlDataReader sqlReader)
        {
            SAPCajasChicasDTO _centroCosto = null;
            List<SAPCajasChicasDTO> _listaCentroCosto = new List<SAPCajasChicasDTO>();

            while (sqlReader.Read())
            {
                _centroCosto = new SAPCajasChicasDTO();

                _centroCosto.BUKRS = sqlReader.GetString(0);
                _centroCosto.LIFNR = sqlReader.GetString(1);
                _centroCosto.NAME = sqlReader.GetString(2);
                _centroCosto.WITHT = sqlReader.GetString(3);
                _centroCosto.ERDAT = sqlReader.GetDateTime(4);

                _listaCentroCosto.Add(_centroCosto);
            }
            return _listaCentroCosto;
        }

        //public List<LlenarDDL_DTO> BuscarCajasChicas(string codigoSociedad)
        public List<LlenarDDL_DTO> BuscarCajasChicas(string codigoSociedad, string usuaraio)
        {
            List<LlenarDDL_DTO> listaLlenarDto = new List<LlenarDDL_DTO>();
            LlenarDDL_DTO llenarDto = null;
            SqlCommand _sqlcomando = null;
//            string sql = @" SELECT distinct LIFNR, LIFNR +' - '+NAME NOMBRE
//                            FROM sap.CajaChicaTMP a
//                            where NAME like 'CC%'
//                            and BUKR = @BUKR ";
//--------------INI---------SATB-03.08.2017----------------------------------------
//----------------Mostrar el mapeo de las cajas chicas por usuario-----------------
            string sql = @"SELECT DISTINCT Mapeo.LIFNR, (Mapeo.LIFNR + ' - ' + CC.NAME) AS Descripcion
                            FROM MapeoUsuarioCajachica AS Mapeo
                            INNER JOIN sap.CajaChicaTMP as CC
		                            ON CC.BUKR = Mapeo.BUKR
	                               AND CC.LIFNR = Mapeo.LIFNR
                            WHERE Mapeo.BUKR = @BUKR
                            AND   Mapeo.USUARIO = @Usuario";

//--------------FIN---------SATB-03.08.2017----------------------------------------

            _sqlcomando = new SqlCommand(sql, _sqlConn);
            _sqlcomando.CommandType = CommandType.Text;

            if (_sqlTran != null)
                _sqlcomando.Transaction = _sqlTran;

            _sqlcomando.Parameters.Add(new SqlParameter("@BUKR", (object)codigoSociedad));
            _sqlcomando.Parameters.Add(new SqlParameter("@Usuario", (object)usuaraio));

            SqlDataReader sqlDataReader = _sqlcomando.ExecuteReader();

            while (sqlDataReader.Read())
            {
                llenarDto = new LlenarDDL_DTO();

                llenarDto.IDENTIFICADOR = sqlDataReader.GetString(0);
                llenarDto.DESCRIPCION = sqlDataReader.GetString(1);

                listaLlenarDto.Add(llenarDto);
            }

            return listaLlenarDto;
        }

        #endregion
    }
}
