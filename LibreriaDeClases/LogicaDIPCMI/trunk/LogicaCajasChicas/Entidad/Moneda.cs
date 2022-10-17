using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;

namespace LogicaCajasChicas.Entidad
{
    public class Moneda
    {
        #region Declaraciones

        private SqlConnection _sqlConn = null;
        private SqlTransaction _sqlTran = null;
        protected string sqlSelect = @" SELECT Moneda, Descripcion, Estado, UsuarioAlta, FechaCreacion, UsuarioModificacion, FechaModificacion
                                        FROM Moneda ";
        protected string sqlInset = @" INSERT INTO Moneda (Moneda, Descripcion, UsuarioAlta)
                                       VALUES             (@Moneda, @Descripcion, @UsuarioAlta) ";

        protected string sqlUpdate = @" UPDATE Moneda SET Moneda = @Moneda, Descripcion = @Descripcion, UsuarioModificacion = @UsuarioModificacion, 
                                                          FechaModificacion = getdate()
                                        WHERE Moneda = @Moneda";
        #endregion

        #region Moneda

        public Moneda(SqlConnection sqlConn)
        {
            _sqlConn = sqlConn;
        }

        public Moneda(SqlConnection sqlConn, SqlTransaction sqlTran)
        {
            _sqlConn = sqlConn;
            _sqlTran = sqlTran;
        }

        #endregion

        #region Metodos

        public MonedaDTO EjecutarSentenciaSelect()
        {
            List<MonedaDTO> _listaMoneda = null;
            SqlCommand sqlComando;
            string sql = sqlSelect + "";

            sqlComando = new SqlCommand(sql, _sqlConn);
            sqlComando.CommandType = CommandType.Text;

            if (_sqlTran != null)
                sqlComando.Transaction = _sqlTran;

            _listaMoneda = CargarReader(sqlComando.ExecuteReader());
            return _listaMoneda.Count > 0 ? _listaMoneda[0] : null;
        }

        public List<MonedaDTO> ListarMonedas()
        {
            SqlCommand sqlComando;
            string sql = sqlSelect;

            sqlComando = new SqlCommand(sql, _sqlConn);
            sqlComando.CommandType = CommandType.Text;

            if (_sqlTran != null)
                sqlComando.Transaction = _sqlTran;

            return CargarReader(sqlComando.ExecuteReader());

        }

        public List<MonedaDTO> ListarMonedasActivas()
        {
            SqlCommand sqlComando;
            string sql = sqlSelect + " WHERE estado = 1 ";

            sqlComando = new SqlCommand(sql, _sqlConn);
            sqlComando.CommandType = CommandType.Text;

            if (_sqlTran != null)
                sqlComando.Transaction = _sqlTran;

            return CargarReader(sqlComando.ExecuteReader());

        }

        public Int32 EjecutarSentenciaInsert(MonedaDTO monedaDto)
        {
            SqlCommand sqlComando;
            string sql = sqlInset;

            sqlComando = new SqlCommand(sql, _sqlConn);
            sqlComando.CommandType = CommandType.Text;

            if (_sqlTran != null)
                sqlComando.Transaction = _sqlTran;

            sqlComando.Parameters.Add(new SqlParameter("Moneda", (object)monedaDto.MONEDA.ToUpper() ?? DBNull.Value));
            sqlComando.Parameters.Add(new SqlParameter("Descripcion", (object)monedaDto.DESCRIPCION.ToUpper() ?? DBNull.Value));
            sqlComando.Parameters.Add(new SqlParameter("UsuarioAlta", (object)monedaDto.USUARIO_MANTENIMIENTO.USUARIO_ALTA ?? DBNull.Value));

            return sqlComando.ExecuteNonQuery();
        }

        public Int32 EjecutarSenteciaUpdate(MonedaDTO monedaDto)
        {
            SqlCommand sqlComando;
            string sql = sqlUpdate;

            sqlComando = new SqlCommand(sql, _sqlConn);
            sqlComando.CommandType = CommandType.Text;

            if (_sqlTran != null)
                sqlComando.Transaction = _sqlTran;

            sqlComando.Parameters.Add(new SqlParameter("@Descripcion", (object)monedaDto.DESCRIPCION ?? DBNull.Value));
            sqlComando.Parameters.Add(new SqlParameter("@UsuarioModificacion", (object)monedaDto.USUARIO_MANTENIMIENTO.USUARIO_MODIFICO));
            sqlComando.Parameters.Add(new SqlParameter("@Moneda", (object)monedaDto.MONEDA ?? DBNull.Value));

            return sqlComando.ExecuteNonQuery();
        }

        protected List<MonedaDTO> CargarReader(SqlDataReader sqlReader)
        {
            MonedaDTO _moneda = null;
            List<MonedaDTO> _listaMoneda = new List<MonedaDTO>();

            while (sqlReader.Read())
            {
                _moneda = new MonedaDTO();

                _moneda.MONEDA = sqlReader.GetString(0);
                _moneda.DESCRIPCION = sqlReader.GetString(1);
                _moneda.ESTADO = sqlReader.GetBoolean(2);
                _moneda.USUARIO_MANTENIMIENTO.USUARIO_ALTA = sqlReader.GetString(3);
                _moneda.USUARIO_MANTENIMIENTO.FECHA_ALTA = sqlReader.GetDateTime(4);
                _moneda.USUARIO_MANTENIMIENTO.USUARIO_MODIFICO = sqlReader.IsDBNull(5) ? null : sqlReader.GetString(5);
                _moneda.USUARIO_MANTENIMIENTO.FECHA_MODIFICACION = sqlReader.IsDBNull(6) ? (DateTime?)null : sqlReader.GetDateTime(6);

                _listaMoneda.Add(_moneda);
            }
            sqlReader.Close();

            return _listaMoneda;
        }

        public void DarBajaMoneda(string usuarioModificacion, string moneda)
        {
            SqlCommand sqlComando;
            string sql = @"update Moneda set Estado = 0, UsuarioModificacion = @UsuarioModificacion, FechaModificacion = getdate()
                           where Moneda = @Moneda ";

            sqlComando = new SqlCommand(sql, _sqlConn);
            sqlComando.CommandType = CommandType.Text;

            if (_sqlTran != null)
                sqlComando.Transaction = _sqlTran;

            sqlComando.Parameters.Add(new SqlParameter("@UsuarioModificacion", (object)usuarioModificacion));
            sqlComando.Parameters.Add(new SqlParameter("@Moneda", (object)moneda));

            sqlComando.ExecuteNonQuery();
        }

        public void DarAltaMoneda(string usuarioModificacion, string moneda)
        {
            SqlCommand sqlComando;
            string sql = @"update Moneda set Estado = 1, UsuarioModificacion = @UsuarioModificacion, FechaModificacion = getdate()
                           where Moneda = @Moneda ";

            sqlComando = new SqlCommand(sql, _sqlConn);
            sqlComando.CommandType = CommandType.Text;

            if (_sqlTran != null)
                sqlComando.Transaction = _sqlTran;

            sqlComando.Parameters.Add(new SqlParameter("@UsuarioModificacion", (object)usuarioModificacion));
            sqlComando.Parameters.Add(new SqlParameter("@Moneda", (object)moneda));

            sqlComando.ExecuteNonQuery();
        }

        public bool ExisteMoneda(string moneda)
        {
            SqlCommand sqlComando;
            Int32 existe = 0;
            string sql = @" select count(Moneda)
                        from Moneda
                        where Moneda = @Moneda";

            sqlComando = new SqlCommand(sql, _sqlConn);
            sqlComando.CommandType = CommandType.Text;

            if (_sqlTran != null)
                sqlComando.Transaction = _sqlTran;

            sqlComando.Parameters.Add(new SqlParameter("@Moneda", (object)moneda ?? DBNull.Value));

            existe = Convert.ToInt32(sqlComando.ExecuteScalar());

            return existe == 0 ? false : true;
        }

        #endregion
    }
}

