using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using DipCmiGT.LogicaComun;

namespace DipCmiGT.LogicaCajasChicas.Entidad
{
    public class Centro
    {
        #region Declaraciones

        private SqlConnection _sqlConn = null;
        private SqlTransaction _sqlTran = null;

        protected string sqlSelect = @"SELECT IdCentro, Nombre, Alta, UsuarioAlta, FechaAlta,
                                       UsuarioModificacion, FechaModificacion
                                       FROM Centro";

        protected string sqlInsert = @"INSERT INTO Centro (Nombre, UsuarioAlta)
                                                      VALUES (@Nombre, @UsuarioAlta) 
                                        SELECT @@Identity ";

        protected string sqlUpdate = @"UPDATE Centro SET Nombre = @Nombre, Alta = @Alta,  UsuarioModificacion = @UsuarioModificacion, 
                                                        FechaModificacion = getdate()
                                                            WHERE IdCentro = @IdCentro ";

        #endregion

        #region Constructores

        public Centro(SqlConnection sqlConn)
        {
            _sqlConn = sqlConn;
        }

        public Centro(SqlConnection sqlConn, SqlTransaction sqlTran)
        {
            _sqlConn = sqlConn;
            _sqlTran = sqlTran;
        }

        #endregion

        #region Metodos

        public CentroDTO EjecutarSentenciaSelect(Int32 IdCentro)
        {
            List<CentroDTO> listaCentro = null;
            SqlCommand sqlComando;
            string sql = sqlSelect + "";

            sqlComando = new SqlCommand(sql, _sqlConn);
            sqlComando.CommandType = CommandType.Text;

            if (_sqlTran != null)
                sqlComando.Transaction = _sqlTran;

            sqlComando.Parameters.Add(new SqlParameter("@IdCentro", (object)IdCentro ?? DBNull.Value));
            return listaCentro.Count > 0 ? listaCentro[0] : null;
        }

        public Int32 EjecutarSentenciaInsert(CentroDTO centroDTO)
        {
            SqlCommand sqlCommando = new SqlCommand(sqlInsert, _sqlConn);
            sqlCommando.CommandType = CommandType.Text;

            if (_sqlTran != null)
                sqlCommando.Transaction = _sqlTran;

            sqlCommando.Parameters.Add(new SqlParameter("@Nombre", (object)centroDTO.NOMBRE ?? DBNull.Value));
            sqlCommando.Parameters.Add(new SqlParameter("@UsuarioAlta", (object)centroDTO.USUARIO_MANTENIMIENTO.USUARIO_ALTA ?? DBNull.Value));

            return Convert.ToInt32(sqlCommando.ExecuteScalar());
        }

        public Int32 EjecutarSentenciaUpdate(CentroDTO centroDTO)
        {
            SqlCommand sqlComando;

            sqlComando = new SqlCommand(sqlUpdate, _sqlConn);

            if (_sqlTran != null)
                sqlComando.Transaction = _sqlTran;

            sqlComando.Parameters.Add(new SqlParameter("@Nombre", (object)centroDTO.NOMBRE ?? DBNull.Value));
            sqlComando.Parameters.Add(new SqlParameter("@Alta", (object)centroDTO.ALTA ?? DBNull.Value));
            sqlComando.Parameters.Add(new SqlParameter("@UsuarioModificacion", (object)centroDTO.USUARIO_MANTENIMIENTO.USUARIO_MODIFICO ?? DBNull.Value));
            sqlComando.Parameters.Add(new SqlParameter("IdCentro", (object)centroDTO.ID_CENTRO ?? DBNull.Value));

            return Convert.ToInt32(sqlComando.ExecuteScalar());
        }

        protected List<CentroDTO> CargarReader(SqlDataReader sqlReader)
        {
            CentroDTO _centroDto = null;
            List<CentroDTO> _listaCentro = new List<CentroDTO>();

            while (sqlReader.Read())
            {
                _centroDto = new CentroDTO();
                _centroDto.ID_CENTRO = sqlReader.GetInt32(0);
                _centroDto.NOMBRE = sqlReader.GetString(1);
                _centroDto.ALTA = sqlReader.GetBoolean(2);
                _centroDto.USUARIO_MANTENIMIENTO.USUARIO_ALTA = sqlReader.GetString(3);
                _centroDto.USUARIO_MANTENIMIENTO.FECHA_ALTA = sqlReader.GetDateTime(4);
                _centroDto.USUARIO_MANTENIMIENTO.USUARIO_MODIFICO = sqlReader.IsDBNull(5) ? null : sqlReader.GetString(5);
                _centroDto.USUARIO_MANTENIMIENTO.FECHA_MODIFICACION = sqlReader.IsDBNull(6) ? (DateTime?)null : sqlReader.GetDateTime(6);

                _listaCentro.Add(_centroDto);
            }

            sqlReader.Close();
            return _listaCentro;

        }

        public List<CentroDTO> ListaCentro()
        {
            SqlCommand sqlComando;
            string sql = sqlSelect;

            sqlComando = new SqlCommand(sql, _sqlConn);

            if (_sqlTran != null)
                sqlComando.Transaction = _sqlTran;

            return CargarReader(sqlComando.ExecuteReader());
        }

        public List<CentroDTO> ListaCentroDDL()
        {
            SqlCommand sqlComando;
            string sql = sqlSelect + @" WHERE Alta = 1";

            sqlComando = new SqlCommand(sql, _sqlConn);

            if (_sqlTran != null)
                sqlComando.Transaction = _sqlTran;

            return CargarReader(sqlComando.ExecuteReader());
        }

        public Int16 DarBajaCentro(Int32 idCentro, string usuario)
        {
            SqlCommand _sqlComando = null;
            string sql = @"Update Centro set Alta = 0, UsuarioModificacion = @UsuarioModificacion, FechaModificacion = getdate()
                            where IdCentro = @IdCentro";

            _sqlComando = new SqlCommand(sql, _sqlConn);
            _sqlComando.CommandType = CommandType.Text;

            if (_sqlTran != null)
                _sqlComando.Transaction = _sqlTran;

            _sqlComando.Parameters.Add(new SqlParameter("@UsuarioModificacion", (object)usuario ?? DBNull.Value));
            _sqlComando.Parameters.Add(new SqlParameter("@IdCentro", (object)idCentro ?? DBNull.Value));

            return Convert.ToInt16(_sqlComando.ExecuteNonQuery());
        }

        public Int16 DarAltaCentro(Int32 idCentro, string usuario)
        {
            SqlCommand _sqlComando = null;
            string sql = @"Update Centro set Alta = 1, UsuarioModificacion = @UsuarioModificacion, FechaModificacion = getdate()
                            where IdCentro = @IdCentro";

            _sqlComando = new SqlCommand(sql, _sqlConn);
            _sqlComando.CommandType = CommandType.Text;

            if (_sqlTran != null)
                _sqlComando.Transaction = _sqlTran;

            _sqlComando.Parameters.Add(new SqlParameter("@UsuarioModificacion", (object)usuario ?? DBNull.Value));
            _sqlComando.Parameters.Add(new SqlParameter("@IdCentro", (object)idCentro ?? DBNull.Value));

            return Convert.ToInt16(_sqlComando.ExecuteNonQuery());
        }

        
        public List<LlenarDDL_DTO> BuscarCentros(string ceco)
        {
            List<LlenarDDL_DTO> listaLlenarDto = new List<LlenarDDL_DTO>();
            LlenarDDL_DTO llenarDto = null;
            SqlCommand _sqlcomando = null;

            string sql = @"SPMapeoRegistradorAprobador";

            _sqlcomando = new SqlCommand(sql, _sqlConn);
            _sqlcomando.CommandType = CommandType.StoredProcedure;

            if (_sqlTran != null)
                _sqlcomando.Transaction = _sqlTran;

            _sqlcomando.Parameters.Add(new SqlParameter("CECO", (object)@ceco ?? DBNull.Value));
            _sqlcomando.Parameters.Add(new SqlParameter("OPCION", 1));

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
