using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using DipCmiGT.LogicaComun;

namespace LogicaCajasChicas.Entidad
{
    public class UsuarioSociedadCentro
    {
        #region Declaraciones

        private SqlConnection _sqlConn = null;
        private SqlTransaction _sqlTran = null;

        protected string sqlSelect = @" select distinct a.IdUsuarioSociedadCentro, a.Usuario, a.IdSociedadCentro, c.CodigoSociedad, c.Nombre, d.IdCentro, d.Nombre, a.UsuarioCreacion, 
                                        a.FechaCreacion, a.UsuarioModificacion, a.FechaModificacion, a.Alta
                                        from UsuarioSociedadCentro a
                                        inner join SociedadCentro b on b.IdSociedadCentro = a.IdSociedadCentro and b.Alta = 1
                                        inner join Sociedad c on c.CodigoSociedad = b.CodigoSociedad and c.Alta = 1
                                        inner join Centro d on d.IdCentro = b.IdCentro and d.Alta = 1  ";

        protected string sqlInsert = @" INSERT INTO UsuarioSociedadCentro (Usuario, IdSociedadCentro, UsuarioCreacion)
                                                                VALUES (@Usuario, @IdSociedadCentro, @UsuarioCreacion)
                                        SELECT @@IDENTITY ";

        protected string sqlUpdate = @" UPDATE UsuarioSociedadCentro SET Usuario = @Usuario, IdSociedadCentro = @IdSociedadCentro, UsuarioModificacion = @UsuarioModificacion, 
                                        FechaModificacion = getdate()
                                       WHERE IdUsuarioCentroCosto = @IdUsuarioCentroCosto ";


        #endregion

        #region Constructores

        public UsuarioSociedadCentro(SqlConnection sqlConn)
        {
            _sqlConn = sqlConn;
        }

        public UsuarioSociedadCentro(SqlConnection sqlConn, SqlTransaction sqlTran)
        {
            _sqlConn = sqlConn;
            _sqlTran = sqlTran;
        }

        #endregion

        #region Metodos

        public UsuarioSociedadCentroDTO EjecutarSentenciaSelect(Int32 idUsuarioCentroCosto)
        {
            SqlCommand _sqlComando = null;
            List<UsuarioSociedadCentroDTO> _listaUsuarioCentroCostoDto = null; ;
            string sql = sqlSelect + " Where IdUsuarioCentroCosto = @IdUsuarioCentroCosto";

            _sqlComando = new SqlCommand(sql, _sqlConn);

            if (_sqlTran != null)
                _sqlComando.Transaction = _sqlTran;

            _sqlComando.Parameters.Add(new SqlParameter("@IdUsuarioCentroCosto", (object)idUsuarioCentroCosto ?? DBNull.Value));

            _listaUsuarioCentroCostoDto = CargarReader(_sqlComando.ExecuteReader());

            return (_listaUsuarioCentroCostoDto.Count > 0) ? _listaUsuarioCentroCostoDto[0] : null;
        }

        public List<UsuarioSociedadCentroDTO> ListarUsuarioSociedadCentro(string usuario)
        {
            SqlCommand _sqlComando = null;
            string sql = sqlSelect + @" WHERE A.Usuario = @Usuario ";

            _sqlComando = new SqlCommand(sql, _sqlConn);

            if (_sqlTran != null)
                _sqlComando.Transaction = _sqlTran;

            _sqlComando.Parameters.Add(new SqlParameter("@Usuario", (object)usuario ?? DBNull.Value));

            return CargarReader(_sqlComando.ExecuteReader());
        }

        public List<UsuarioSociedadCentroDTO> ListarUsuarioSociedadCentro(string usuario, string codigoSociedad)
        {
            SqlCommand _sqlComando = null;
            string sql = sqlSelect + @" WHERE A.Usuario = @Usuario
                                        AND c.CodigoSociedad = @CodigoSociedad
                                        AND a.alta = 1 ";

            _sqlComando = new SqlCommand(sql, _sqlConn);

            if (_sqlTran != null)
                _sqlComando.Transaction = _sqlTran;

            _sqlComando.Parameters.Add(new SqlParameter("@Usuario", (object)usuario ?? DBNull.Value));
            _sqlComando.Parameters.Add(new SqlParameter("@CodigoSociedad", (object)codigoSociedad ?? DBNull.Value));

            return CargarReader(_sqlComando.ExecuteReader());
        }

        public UsuarioSociedadCentroDTO BuscarSociedadCentro(string usuario, Int32 idSociedadCentro)
        {
            SqlCommand _sqlComando = null;
            List<UsuarioSociedadCentroDTO> _listaUsuarioSociedadCentroDto = null; ;
            string sql = sqlSelect + @"  where a.Usuario = @Usuario
                                         and b.IdSociedadCentro = @IdSociedadCentro ";

            _sqlComando = new SqlCommand(sql, _sqlConn);

            if (_sqlTran != null)
                _sqlComando.Transaction = _sqlTran;

            _sqlComando.Parameters.Add(new SqlParameter("@Usuario", (object)usuario ?? DBNull.Value));
            _sqlComando.Parameters.Add(new SqlParameter("@IdSociedadCentro", (object)idSociedadCentro ?? DBNull.Value));

            _listaUsuarioSociedadCentroDto = CargarReader(_sqlComando.ExecuteReader());

            return (_listaUsuarioSociedadCentroDto.Count > 0) ? _listaUsuarioSociedadCentroDto[0] : null;
        }
        
        public Int32 EjecutarSentenciaInsert(UsuarioSociedadCentroDTO usuariocentrocostoDTO)
        {
            SqlCommand sqlComando = new SqlCommand(sqlInsert, _sqlConn);
            sqlComando.CommandType = CommandType.Text;

            if (_sqlTran != null)
                sqlComando.Transaction = _sqlTran;

            sqlComando.Parameters.Add(new SqlParameter("@Usuario", (object)usuariocentrocostoDTO.USUARIO.ToUpper() ?? DBNull.Value));
            sqlComando.Parameters.Add(new SqlParameter("@IdSociedadCentro", (object)usuariocentrocostoDTO.ID_SOCIEDAD_CENTRO ?? DBNull.Value));
            sqlComando.Parameters.Add(new SqlParameter("@UsuarioCreacion", (object)usuariocentrocostoDTO.USUARIO_MANTENIMIENTO.USUARIO_ALTA ?? DBNull.Value));

            return Convert.ToInt32(sqlComando.ExecuteScalar());
        }

        public Int32 EjecutarSentenciaUpdate(UsuarioSociedadCentroDTO usuariocentrocostoDTO)
        {
            SqlCommand sqlComando;

            sqlComando = new SqlCommand(sqlUpdate, _sqlConn);
            sqlComando.CommandType = CommandType.Text;

            if (_sqlTran != null)
                sqlComando.Transaction = _sqlTran;

            sqlComando.Parameters.Add(new SqlParameter("@Usuario", (object)usuariocentrocostoDTO.USUARIO.ToUpper() ?? DBNull.Value));
            sqlComando.Parameters.Add(new SqlParameter("@IdSociedadCentro", (object)usuariocentrocostoDTO.ID_SOCIEDAD_CENTRO ?? DBNull.Value));
            sqlComando.Parameters.Add(new SqlParameter("@UsuarioModificacion", (object)usuariocentrocostoDTO.USUARIO_MANTENIMIENTO.USUARIO_MODIFICO ?? DBNull.Value));
            sqlComando.Parameters.Add(new SqlParameter("@IdUsuarioCentroCosto", (object)usuariocentrocostoDTO.ID_USUARIO_SOCIEDAD_CENTRO ?? DBNull.Value));

            return Convert.ToInt32(sqlComando.ExecuteScalar());
        }

        public Int16 DarBajaUsrSociedadCentro(Int32 idSociedadCentro, string usuario)
        {
            SqlCommand _sqlComando = null;
            string sql = @"Update UsuarioSociedadCentro set Alta = 0, UsuarioModificacion = @UsuarioModificacion, FechaModificacion = getdate()
                            where IdUsuarioSociedadCentro = @IdUsuarioSociedadCentro";

            _sqlComando = new SqlCommand(sql, _sqlConn);
            _sqlComando.CommandType = CommandType.Text;

            if (_sqlTran != null)
                _sqlComando.Transaction = _sqlTran;

            _sqlComando.Parameters.Add(new SqlParameter("@UsuarioModificacion", (object)usuario ?? DBNull.Value));
            _sqlComando.Parameters.Add(new SqlParameter("@IdUsuarioSociedadCentro", (object)idSociedadCentro ?? DBNull.Value));

            return Convert.ToInt16(_sqlComando.ExecuteNonQuery());
        }

        public Int16 DarAltaUsrSociedadCentro(Int32 idSociedadCentro, string usuario)
        {
            SqlCommand _sqlComando = null;
            string sql = @"Update UsuarioSociedadCentro set Alta = 1, UsuarioModificacion = @UsuarioModificacion, FechaModificacion = getdate()
                            where IdUsuarioSociedadCentro = @IdUsuarioSociedadCentro";

            _sqlComando = new SqlCommand(sql, _sqlConn);
            _sqlComando.CommandType = CommandType.Text;

            if (_sqlTran != null)
                _sqlComando.Transaction = _sqlTran;

            _sqlComando.Parameters.Add(new SqlParameter("@UsuarioModificacion", (object)usuario ?? DBNull.Value));
            _sqlComando.Parameters.Add(new SqlParameter("@IdUsuarioSociedadCentro", (object)idSociedadCentro ?? DBNull.Value));

            return Convert.ToInt16(_sqlComando.ExecuteNonQuery());
        }

        public bool ExisteUsuarioSociedadCentro(Int32 idSociedadCentro, string usuario)
        {
            SqlCommand _sqlComando = null;
            string sql = @" SELECT COUNT(a.IdUsuarioSociedadCentro)
	                    FROM UsuarioSociedadCentro a
	                    where IdSociedadCentro = @IdSociedadCentro
	                    and Usuario = @Usuario ";

            _sqlComando = new SqlCommand(sql, _sqlConn);
            _sqlComando.CommandType = CommandType.Text;

            if (_sqlTran != null)
                _sqlComando.Transaction = _sqlTran;

            _sqlComando.Parameters.Add(new SqlParameter("@Usuario", (object)usuario ?? DBNull.Value));
            _sqlComando.Parameters.Add(new SqlParameter("@IdSociedadCentro", (object)idSociedadCentro ?? DBNull.Value));

            return Convert.ToInt32(_sqlComando.ExecuteScalar()) == 0 ? false : true;
        }

        public List<UsuarioSociedadCentroDTO> CargarReader(SqlDataReader sqlReader)
        {
            UsuarioSociedadCentroDTO _usuarioSociedadCentroDto = null;
            List<UsuarioSociedadCentroDTO> _listaSociedadCentroDto = new List<UsuarioSociedadCentroDTO>();

            try
            {

                while (sqlReader.Read())
                {
                    _usuarioSociedadCentroDto = new UsuarioSociedadCentroDTO();

                    _usuarioSociedadCentroDto.ID_USUARIO_SOCIEDAD_CENTRO = sqlReader.GetInt32(0);
                    _usuarioSociedadCentroDto.USUARIO = sqlReader.GetString(1);
                    _usuarioSociedadCentroDto.ID_SOCIEDAD_CENTRO = sqlReader.GetInt32(2);
                    _usuarioSociedadCentroDto.CODIGO_SOCIEDAD = sqlReader.GetString(3);
                    _usuarioSociedadCentroDto.NOMBRE_SOCIEDAD = sqlReader.GetString(4);
                    _usuarioSociedadCentroDto.ID_CENTRO = sqlReader.GetInt32(5);
                    _usuarioSociedadCentroDto.NOMBRE_CENTRO = sqlReader.GetString(6);
                    _usuarioSociedadCentroDto.USUARIO_MANTENIMIENTO.USUARIO_ALTA = sqlReader.GetString(7);
                    _usuarioSociedadCentroDto.USUARIO_MANTENIMIENTO.FECHA_ALTA = sqlReader.GetDateTime(8);
                    _usuarioSociedadCentroDto.USUARIO_MANTENIMIENTO.USUARIO_MODIFICO = sqlReader.IsDBNull(9) ? string.Empty : sqlReader.GetString(9);
                    _usuarioSociedadCentroDto.USUARIO_MANTENIMIENTO.FECHA_MODIFICACION = sqlReader.IsDBNull(10) ? (DateTime?)null : sqlReader.GetDateTime(10);
                    _usuarioSociedadCentroDto.ALTA = sqlReader.GetBoolean(11);
                    _listaSociedadCentroDto.Add(_usuarioSociedadCentroDto);
                }
                return _listaSociedadCentroDto;
            }
            finally
            {
                if (sqlReader != null) sqlReader.Close();
            }
        }

        public List<LlenarDDL_DTO> ListarUsuarioSociedad(string usuario)
        {
            SqlCommand _sqlComando = null;
            string sql = @"select distinct c.CodigoSociedad, c.CodigoSociedad +' :: '+ c.Nombre
                            from UsuarioSociedadCentro a
                            inner join SociedadCentro b on b.IdSociedadCentro = a.IdSociedadCentro and b.Alta = 1
                            inner join Sociedad c on c.CodigoSociedad = b.CodigoSociedad and c.Alta = 1
                            inner join Centro d on d.IdCentro = b.IdCentro and d.Alta = 1 
                            where a.Usuario = @Usuario";

            _sqlComando = new SqlCommand(sql, _sqlConn);
            _sqlComando.CommandType = CommandType.Text;

            if (_sqlTran != null)
                _sqlComando.Transaction = _sqlTran;

            _sqlComando.Parameters.Add(new SqlParameter("@usuario", (object)usuario));

            return CargarDDL(_sqlComando.ExecuteReader());
        }

        public List<LlenarDDL_DTO> ListarUsuarioCentro(string usuario, string codigoSociedad)
        {
            SqlCommand _sqlComando = null;
            string sql = @"select CAST(a.IdSociedadCentro as varchar(3)), cast(d.IdCentro as varchar(3)) +' :: '+ d.Nombre
                            from UsuarioSociedadCentro a
                            inner join SociedadCentro b on b.IdSociedadCentro = a.IdSociedadCentro and b.Alta = 1
                            inner join Sociedad c on c.CodigoSociedad = b.CodigoSociedad and c.Alta = 1
                            inner join Centro d on d.IdCentro = b.IdCentro and d.Alta = 1
                            where a.Usuario = @Usuario
                            and c.CodigoSociedad = @CodigoSociedad ";

            _sqlComando = new SqlCommand(sql, _sqlConn);
            _sqlComando.CommandType = CommandType.Text;

            if (_sqlTran != null)
                _sqlComando.Transaction = _sqlTran;

            _sqlComando.Parameters.Add(new SqlParameter("@usuario", (object)usuario));
            _sqlComando.Parameters.Add(new SqlParameter("@CodigoSociedad", (object)codigoSociedad));

            return CargarDDL(_sqlComando.ExecuteReader());
        }

        private List<LlenarDDL_DTO> CargarDDL(SqlDataReader sqlDataReader)
        {
            List<LlenarDDL_DTO> _listaDDL = new List<LlenarDDL_DTO>();
            LlenarDDL_DTO _DDL = new LlenarDDL_DTO();

            while (sqlDataReader.Read())
            {
                _DDL = new LlenarDDL_DTO();

                _DDL.IDENTIFICADOR = sqlDataReader.GetString(0);
                _DDL.DESCRIPCION = sqlDataReader.GetString(1);

                _listaDDL.Add(_DDL);
            }
            return _listaDDL;
        }

        public List<LlenarDDL_DTO> ListarUsuarioSociedadSuperUsuario(string usuario)
        {
            SqlCommand _sqlComando = null;
            string sql = @"select DISTINCT B.CodigoSociedad, concat(B.CodigoSociedad, '-', c.Pais) Pais from dbo.UsuarioSociedadCentro A
                            INNER JOIN dbo.sociedadcentro B 
                            ON a.idsociedadcentro = b.idsociedadcentro 
                            INNER JOIN dbo.sociedad C 
                            ON b.CodigoSociedad = c.CodigoSociedad
                            where a.usuario = @Usuario
                            Order by CodigoSociedad";

            _sqlComando = new SqlCommand(sql, _sqlConn);
            _sqlComando.CommandType = CommandType.Text;

            if (_sqlTran != null)
                _sqlComando.Transaction = _sqlTran;

            _sqlComando.Parameters.Add(new SqlParameter("@Usuario", (object)usuario));

            return CargarDDL(_sqlComando.ExecuteReader());
        }

        #endregion

    }
}
