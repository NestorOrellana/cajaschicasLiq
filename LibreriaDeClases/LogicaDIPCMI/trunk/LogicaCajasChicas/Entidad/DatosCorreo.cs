using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;

namespace LogicaCajasChicas.Entidad
{
    class DatosCorreo
    {
        #region Declaraciones

        private SqlConnection _sqlConn = null;
        private SqlTransaction _sqlTran = null;

        #endregion

        #region Constructores

        public DatosCorreo(SqlConnection sqlConn)
        {
            _sqlConn = sqlConn;
        }

        public DatosCorreo(SqlConnection sqlConn, SqlTransaction sqlTran)
        {
            _sqlConn = sqlConn;
            _sqlTran = sqlTran;
        }

        #endregion

        #region Metodos

        public List<DatosCorreoDTO> BuscarAprobaciones()
        {
            SqlCommand _sqlCommand = null;
            string sql = "NotificacionAprobaciones";

            if (_sqlTran != null)
                _sqlCommand.Transaction = _sqlTran;

            _sqlCommand = new SqlCommand(sql, _sqlConn);
            _sqlCommand.CommandType = CommandType.StoredProcedure;

            return CargarReader(_sqlCommand.ExecuteReader());
        }

        protected List<DatosCorreoDTO> CargarReader(SqlDataReader sqlReader)
        {
            DatosCorreoDTO _datosCorreoDto = null;
            List<DatosCorreoDTO> _listaDatosCorreo = new List<DatosCorreoDTO>();

            while (sqlReader.Read())
            {
                _datosCorreoDto = new DatosCorreoDTO();

                _datosCorreoDto.USUARIO = sqlReader.GetString(0);
                _datosCorreoDto.NOMBRE = sqlReader.GetString(1);
                _datosCorreoDto.CORREO = sqlReader.GetString(2);
                _datosCorreoDto.FACTURAS = sqlReader.GetInt32(3);
                _datosCorreoDto.FACT_DIV = sqlReader.GetInt32(4);

                _listaDatosCorreo.Add(_datosCorreoDto);
            }

            sqlReader.Close();
            return _listaDatosCorreo;

        }

        #endregion
    }
}
