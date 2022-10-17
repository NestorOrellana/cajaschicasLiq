using System.Collections.Generic;
using System;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;

namespace DipCmiGT.LogicaCajasChicas.Entidad
{
    public class UsuarioCajaChica
    {

        #region Declaraciones
        private SqlConnection _sqlConn = null;
        private SqlTransaction _sqlTran = null;

        #endregion

        #region Constructores

        public UsuarioCajaChica(SqlConnection sqlConn)
        {
            _sqlConn = sqlConn;
        }

        public UsuarioCajaChica(SqlConnection sqlConn, SqlTransaction sqlTran)
        {
            _sqlConn = sqlConn;
            _sqlTran = sqlTran;
        }
        #endregion

        #region Metodos

        public List<UsuarioCajaDTO> BuscarMapeoUsuarioCajaSP(string BUKRS, string Usuario, string busqueda, string dominio)
        {
            SqlCommand _sqlComando = null;

            string sql;
            sql = "CajaChicaporUsuario";

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


        public List<UsuarioCajaDTO> AsignacionCajaUsuario(string BUKRS, string Usuario, string caja)
        {
            SqlCommand sqlComando;

            string sqlInsert = "";
            sqlInsert = @"INSERT INTO MapeoUsuarioCajachica 
                          VALUES (@BUKRS, @LIFNR, @USUARIO)";
            sqlComando = new SqlCommand(sqlInsert, _sqlConn);
            sqlComando.CommandType = CommandType.Text;

            if (_sqlTran != null)
                sqlComando.Transaction = _sqlTran;

            sqlComando.Parameters.Add(new SqlParameter("BUKRS", (object)BUKRS ?? DBNull.Value));
            sqlComando.Parameters.Add(new SqlParameter("LIFNR", (object)caja ?? DBNull.Value));
            sqlComando.Parameters.Add(new SqlParameter("USUARIO", (object)Usuario ?? DBNull.Value));

            return CargarReader(sqlComando.ExecuteReader());
        }

        public List<UsuarioCajaDTO> QuitarCajaUsuario(string BUKRS, string Usuario, string caja)
        {
            SqlCommand sqlComando;

            string sqlInsert = "";
            sqlInsert = @"DELETE FROM MapeoUsuarioCajachica 
                          WHERE BUKR = @BUKRS
                          AND   LIFNR = @LIFNR
                          AND   USUARIO = @USUARIO";
            sqlComando = new SqlCommand(sqlInsert, _sqlConn);
            sqlComando.CommandType = CommandType.Text;

            if (_sqlTran != null)
                sqlComando.Transaction = _sqlTran;

            sqlComando.Parameters.Add(new SqlParameter("BUKRS", (object)BUKRS ?? DBNull.Value));
            sqlComando.Parameters.Add(new SqlParameter("LIFNR", (object)caja ?? DBNull.Value));
            sqlComando.Parameters.Add(new SqlParameter("USUARIO", (object)Usuario ?? DBNull.Value));

            return CargarReader(sqlComando.ExecuteReader());
        }


        protected List<UsuarioCajaDTO> CargarReader(SqlDataReader sqlReader)
        {
            UsuarioCajaDTO _usuarioCaja = null;
            List<UsuarioCajaDTO> _listaUsuarioCaja = new List<UsuarioCajaDTO>();

            while (sqlReader.Read())
            {
                _usuarioCaja = new UsuarioCajaDTO();
                _usuarioCaja.LIFNR = sqlReader.GetString(0);
                _usuarioCaja.NAME = sqlReader.GetString(1);
                _usuarioCaja.ESTADO = Convert.ToBoolean(sqlReader.GetInt32(2));
                _usuarioCaja.BUKRS = sqlReader.GetString(3);

                _listaUsuarioCaja.Add(_usuarioCaja);

            }
            sqlReader.Close();
            return _listaUsuarioCaja;
        }

        #endregion
    }
}
