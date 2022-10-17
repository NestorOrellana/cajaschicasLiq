using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;

namespace DipCmiGT.LogicaComun.Entidad
{
    public class LogErrores
    {

        #region Declaraciones
        private SqlConnection _sqlConn = null;
        private SqlTransaction _sqlTran = null;

        protected string sqlInsert = @"INSERT INTO LogErrores (Descripcion, Usuario, Funcion)
                                                       VALUES (@Descripcion, @Usuario, @Funcion)";
        #endregion

        #region Constructores

        public LogErrores(SqlConnection sqlConn)
        {
            _sqlConn = sqlConn;
        }

        public LogErrores(SqlConnection sqlConn, SqlTransaction sqlTran)
        {
            _sqlConn = sqlConn;
            _sqlTran = sqlTran;
        }

        #endregion

        #region Metodos

        protected List<LogErroresDTO> CargarReader(SqlDataReader sqlReader)
        {
            LogErroresDTO _logErrores = null;
            List<LogErroresDTO> _listaLogErrores = new List<LogErroresDTO>();

            while (sqlReader.Read())
            {
                _logErrores = new LogErroresDTO();

                _logErrores.ID_LOG_ERRORES = sqlReader.GetInt32(0);
                _logErrores.DESCRIPCION = sqlReader.GetString(1);
                _logErrores.USUARIO = sqlReader.GetString(2);
                _logErrores.FUNCION = sqlReader.GetString(3);
                _logErrores.FECHA_EVENTO = sqlReader.GetDateTime(4);

                _listaLogErrores.Add(_logErrores);
            }

            sqlReader.Close();
            return _listaLogErrores;

        }

        public Int32 SentenciaInsert(LogErroresDTO logErroresDto)
        {
            SqlCommand sqlCommando = new SqlCommand(sqlInsert, _sqlConn);
            sqlCommando.CommandType = CommandType.Text;

            if (_sqlTran != null)
                sqlCommando.Transaction = _sqlTran;

            sqlCommando.Parameters.Add(new SqlParameter("@Descripcion", (object)logErroresDto.DESCRIPCION ?? DBNull.Value));
            sqlCommando.Parameters.Add(new SqlParameter("@Usuario", (object)logErroresDto.USUARIO ?? DBNull.Value));
            sqlCommando.Parameters.Add(new SqlParameter("@Funcion", (object)logErroresDto.FUNCION ?? DBNull.Value));

            return Convert.ToInt32(sqlCommando.ExecuteScalar());
        }

        #endregion
    }
}
