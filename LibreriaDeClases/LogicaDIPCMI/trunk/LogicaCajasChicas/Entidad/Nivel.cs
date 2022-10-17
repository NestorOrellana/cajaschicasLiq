using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using DipCmiGT.LogicaCajasChicas;
using System.Data;
using DipCmiGT.LogicaComun;

namespace LogicaCajasChicas.Entidad
{
    public class Nivel
    {
        #region Declaraciones

        private SqlConnection _sqlConn = null;
        private SqlTransaction _sqlTran = null;
        protected string sqlSelect = @"SELECT IdNivel, Nivel, Alta
                                        FROM Nivel";
        #endregion

        #region Nivel

        public Nivel(SqlConnection sqlConn)
        {
            _sqlConn = sqlConn;
        }

        public Nivel(SqlConnection sqlConn, SqlTransaction sqlTran)
        {
            _sqlConn = sqlConn;
            _sqlTran = sqlTran;
        }

        #endregion

        #region Metodos

        public NivelDTO EjecutarSentenciaSelect()
        {
            List<NivelDTO> _listaNivel = null;
            SqlCommand sqlComando;
            string sql = sqlSelect + "";

            sqlComando = new SqlCommand(sql, _sqlConn);
            sqlComando.CommandType = CommandType.Text;

            if (_sqlTran != null)
                sqlComando.Transaction = _sqlTran;

            _listaNivel = CargarReader(sqlComando.ExecuteReader());
            return _listaNivel.Count > 0 ? _listaNivel[0] : null;
        }

        protected List<NivelDTO> CargarReader(SqlDataReader sqlReader)
        {
            NivelDTO _nivel = null;
            List<NivelDTO> _listaNivel = new List<NivelDTO>();

            while (sqlReader.Read())
            {
                _nivel = new NivelDTO();

                _nivel.ID_NIVEL = sqlReader.GetInt16(0);
                _nivel.NIVEL = sqlReader.GetString(1);
                _nivel.ALTA = sqlReader.GetBoolean(2);

                _listaNivel.Add(_nivel);
            }
            sqlReader.Close();

            return _listaNivel;
        }

        public List<NivelDTO> ListaNivel()
        {
            SqlCommand sqlComando;
            string sql = sqlSelect + " Where Alta = 1 ";

            sqlComando = new SqlCommand(sql, _sqlConn);

            if (_sqlTran != null)
                sqlComando.Transaction = _sqlTran;

            return CargarReader(sqlComando.ExecuteReader());
        }

        public List<LlenarDDL_DTO> ListaNivelDDL()
        {
            SqlCommand _sqlComando = null;
            string sql = @" select * from Nivel where alta = 1";

            _sqlComando = new SqlCommand(sql, _sqlConn);
            _sqlComando.CommandType = CommandType.Text;

            if (_sqlTran != null)
                _sqlComando.Transaction = _sqlTran;

            return CargarDDL(_sqlComando.ExecuteReader());
        }

        private List<LlenarDDL_DTO> CargarDDL(SqlDataReader sqlDataReader)
        {
            List<LlenarDDL_DTO> _listaDDL = new List<LlenarDDL_DTO>();
            LlenarDDL_DTO _DDL = new LlenarDDL_DTO();

            while (sqlDataReader.Read())
            {
                _DDL = new LlenarDDL_DTO();

                _DDL.IDENTIFICADOR = Convert.ToString(sqlDataReader.GetInt16(0));
                _DDL.DESCRIPCION = sqlDataReader.GetString(1);

                _listaDDL.Add(_DDL);
            }
            return _listaDDL;
        }
        #endregion
    }
}
