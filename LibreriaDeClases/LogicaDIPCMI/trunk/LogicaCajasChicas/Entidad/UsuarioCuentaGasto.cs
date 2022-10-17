using System.Collections.Generic;
using System;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;

namespace DipCmiGT.LogicaCajasChicas.Entidad
{
    public class UsuarioCuentaGasto
    {

        #region Declaraciones
        private SqlConnection _sqlConn = null;
        private SqlTransaction _sqlTran = null;

        #endregion

        #region Constructores

        public UsuarioCuentaGasto(SqlConnection sqlConn)
        {
            _sqlConn = sqlConn;
        }

        public UsuarioCuentaGasto(SqlConnection sqlConn, SqlTransaction sqlTran)
        {
            _sqlConn = sqlConn;
            _sqlTran = sqlTran;
        }
        #endregion

        #region Metodos

        public List<UsuarioCuentaDTO> BuscarMapeoUsuarioCuentaSP(string BUKRS, string Usuario, string busqueda, string dominio)
        {
            SqlCommand _sqlComando = null;

            string sql;
            sql = "CuentasGastoporUsuario";

            _sqlComando = new SqlCommand(sql, _sqlConn);
            _sqlComando.CommandType = CommandType.StoredProcedure;

            if (_sqlTran != null)
                _sqlComando.Transaction = _sqlTran;

            _sqlComando.Parameters.Add(new SqlParameter("BUKRS", (object)BUKRS ?? DBNull.Value));
            _sqlComando.Parameters.Add(new SqlParameter("Usuario", (object)Usuario ?? DBNull.Value));
            _sqlComando.Parameters.Add(new SqlParameter("busqueda", (object)busqueda ?? DBNull.Value));
            _sqlComando.Parameters.Add(new SqlParameter("dominio", (object)dominio ?? DBNull.Value));

            return CargarReader(_sqlComando.ExecuteReader());
        }


        public List<UsuarioCuentaDTO> AsignacionCuentaUsuario(string BUKRS, string Usuario, string cuenta)
        {
            SqlCommand sqlComando;

            string sqlInsert = "";
            sqlInsert = @"INSERT INTO MapeoUsuarioCECOCuenta 
                          VALUES (@BUKRS, @SAKNR, @USUARIO)";
            sqlComando = new SqlCommand(sqlInsert, _sqlConn);
            sqlComando.CommandType = CommandType.Text;

            if (_sqlTran != null)
                sqlComando.Transaction = _sqlTran;

            sqlComando.Parameters.Add(new SqlParameter("BUKRS", (object)BUKRS ?? DBNull.Value));
            sqlComando.Parameters.Add(new SqlParameter("SAKNR", (object)cuenta ?? DBNull.Value));
            sqlComando.Parameters.Add(new SqlParameter("USUARIO", (object)Usuario ?? DBNull.Value));

            return CargarReader(sqlComando.ExecuteReader());
        }

        public List<UsuarioCuentaDTO> QuitarCuentaUsuario(string BUKRS, string Usuario, string cuenta)
        {
            SqlCommand sqlComando;

            string sqlInsert = "";
            sqlInsert = @"DELETE FROM MapeoUsuarioCECOCuenta 
                          WHERE BUKRS = @BUKRS
                          AND   SAKNR = @SAKNR
                          AND   USUARIO = @USUARIO";
            sqlComando = new SqlCommand(sqlInsert, _sqlConn);
            sqlComando.CommandType = CommandType.Text;

            if (_sqlTran != null)
                sqlComando.Transaction = _sqlTran;

            sqlComando.Parameters.Add(new SqlParameter("BUKRS", (object)BUKRS ?? DBNull.Value));
            sqlComando.Parameters.Add(new SqlParameter("SAKNR", (object)cuenta ?? DBNull.Value));
            sqlComando.Parameters.Add(new SqlParameter("USUARIO", (object)Usuario ?? DBNull.Value));

            return CargarReader(sqlComando.ExecuteReader());
        }


        protected List<UsuarioCuentaDTO> CargarReader(SqlDataReader sqlReader)
        {
            UsuarioCuentaDTO _usuarioCuenta = null;
            List<UsuarioCuentaDTO> _listaUsuarioCuenta = new List<UsuarioCuentaDTO>();

            while (sqlReader.Read())
            {
                _usuarioCuenta = new UsuarioCuentaDTO();
                _usuarioCuenta.SAKNR = sqlReader.GetString(0);
                _usuarioCuenta.TXT50 = sqlReader.GetString(1);
                _usuarioCuenta.ESTADO = Convert.ToBoolean(sqlReader.GetInt32(2));
                _usuarioCuenta.BUKRS = sqlReader.GetString(3);


                _listaUsuarioCuenta.Add(_usuarioCuenta);

            }
            sqlReader.Close();
            return _listaUsuarioCuenta;
        }

        #endregion
    }
}
