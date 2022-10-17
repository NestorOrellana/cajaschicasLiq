using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using DipCmiGT.LogicaComun;

namespace DipCmiGT.LogicaCajasChicas.Entidad
{
    public class NumeroCajaChica
    {
        #region Declaraciones

        private SqlConnection _sqlConn = null;
        private SqlTransaction _sqlTran = null;

        protected string sqlSelect = @" SELECT NumeroCajaChica, Descripcion
                                        FROM NumeroCajaChica ";

        #endregion

        #region Constructores


        public NumeroCajaChica(SqlConnection sqlConn)
        {
            _sqlConn = sqlConn;
        }

        public NumeroCajaChica(SqlConnection sqlConn, SqlTransaction sqlTran)
        {
            _sqlConn = sqlConn;
            _sqlTran = sqlTran;
        }

        #endregion

        #region Metodos

        public List<LlenarDDL_DTO> ListarNumeroCajasChicas()
        {
            SqlCommand _sqlComando = null;
            string sql = sqlSelect;

            _sqlComando = new SqlCommand(sql, _sqlConn);

            if (_sqlTran != null)
                _sqlComando.Transaction = _sqlTran;

            return CargarReader(_sqlComando.ExecuteReader());
        }

        public List<LlenarDDL_DTO> CargarReader(SqlDataReader sqlDataReader)
        {
            List<LlenarDDL_DTO> _listaLlenarDDL = new List<LlenarDDL_DTO>();
            LlenarDDL_DTO _llenarDDL = null;

            while (sqlDataReader.Read())
            {
                _llenarDDL = new LlenarDDL_DTO();

                _llenarDDL.IDENTIFICADOR = sqlDataReader.GetString(0);
                _llenarDDL.DESCRIPCION = string.Concat(sqlDataReader.GetString(0), ' ', ':', ':', ' ', sqlDataReader.GetString(1));

                _listaLlenarDDL.Add(_llenarDDL);
            }

            return _listaLlenarDDL;
        }

        #endregion
    }
}
