using System.Collections.Generic;
using System;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;

namespace DipCmiGT.LogicaCajasChicas.Entidad
{
    public class UsuarioRol
    {

        #region Declaraciones
        private SqlConnection _sqlConn = null;
        private SqlTransaction _sqlTran = null;

        protected string sqlSelect = @"Select a.IdUsuario, b.Usuario, a.IdRol, c.Rol, a.Alta, a.UsuarioAlta, a.FechaAlta, a.UsuarioModificacion, a.FechaModificacion
				                     from UsuarioRol as a 
				                     INNER JOIN Usuario as b on a.IdUsuario = b.IdUsuario  
				                     INNER JOIN Rol as c on a.IdRol = c.IdRol";


        //        protected string sqlSelect = @"SELECT IdUsuario, IdRol, Alta, UsuarioAlta, Fechaalta, UsuarioModificacion, FechaModificacion
        //                                        FROM UsuarioRol
        //                                        WHERE IdUsuario = @IdUsuario";

        protected string sqlInsert = @"INSERT INTO UsuarioRol (IdUsuario, IdRol, UsuarioAlta)
                                        VALUES (@IdUsuario, @IdRol, @UsuarioAlta) ";
        //                                        VALUES (@IdUsusario, @IdRol, @UsuarioAlta) " ;

        protected string sqlUpdate = @"UPDATE UsuarioRol SET IdRol = @IdRol, Alta = @Alta, UsuarioModificacion = @UsuarioModificacion, FechaModificacion = getdate()
                                        WHERE IdUsuario = @IdUsuario and IdRol = @IdRol";


        #endregion

        #region Constructores

        public UsuarioRol(SqlConnection sqlConn)
        {
            _sqlConn = sqlConn;
        }

        public UsuarioRol(SqlConnection sqlConn, SqlTransaction sqlTran)
        {
            _sqlConn = sqlConn;
            _sqlTran = sqlTran;
        }
        #endregion

        #region Metodos

        public UsuarioRolDTO EjecutarSentenciaSelect(Int16 IdUsuario, Int16 IdRol)
        {

            List<UsuarioRolDTO> listaUsuarioRol = null;
            SqlCommand sqlComando;
            string sql = sqlSelect + "";

            sqlComando = new SqlCommand(sql, _sqlConn);
            sqlComando.CommandType = CommandType.Text;

            if (_sqlTran != null)
                sqlComando.Transaction = _sqlTran;

            sqlComando.Parameters.Add(new SqlParameter("@IdUsuario", (object)IdUsuario ?? DBNull.Value));
            sqlComando.Parameters.Add(new SqlParameter("@IdRol", (object)IdRol ?? DBNull.Value));

            listaUsuarioRol = CargarReader(sqlComando.ExecuteReader());
            return listaUsuarioRol.Count > 0 ? listaUsuarioRol[0] : null;

        }

        public Int32 EjecutarSentenciaInsert(UsuarioRolDTO usuarioRolDTO)
        {
            SqlCommand sqlComando;

            sqlComando = new SqlCommand(sqlInsert, _sqlConn);
            sqlComando.CommandType = CommandType.Text;

            if (_sqlTran != null)
                sqlComando.Transaction = _sqlTran;

            sqlComando.Parameters.Add(new SqlParameter("@IdUsuario", (object)usuarioRolDTO.ID_USUARIO ?? DBNull.Value));
            sqlComando.Parameters.Add(new SqlParameter("@IdRol", (object)usuarioRolDTO.ID_ROL ?? DBNull.Value));
            // sqlComando.Parameters.Add(new SqlParameter("@Alta", (object)usuarioRolDTO.ALTA ?? DBNull.Value));
            sqlComando.Parameters.Add(new SqlParameter("@UsuarioAlta", (object)usuarioRolDTO.USUARIO_MANTENIMIENTO.USUARIO_ALTA.ToUpper() ?? DBNull.Value));

            return Convert.ToInt32(sqlComando.ExecuteScalar());

        }

        public Int32 EjecutarSentenciaUpdate(UsuarioRolDTO usuarioRolDTO)
        {
            SqlCommand sqlComando;

            sqlComando = new SqlCommand(sqlUpdate, _sqlConn);

            if (_sqlTran != null)
                sqlComando.Transaction = _sqlTran;

            sqlComando.Parameters.Add(new SqlParameter("@IdUsuario", (object)usuarioRolDTO.ID_USUARIO ?? DBNull.Value));
            sqlComando.Parameters.Add(new SqlParameter("@IdRol", (object)usuarioRolDTO.ID_ROL ?? DBNull.Value));
            sqlComando.Parameters.Add(new SqlParameter("@Alta", (object)usuarioRolDTO.ALTA ?? DBNull.Value));
            // sqlComando.Parameters.Add(new SqlParameter("@UsuarioAlta", (object)usuarioRolDTO.USUARIO_MANTENIMIENTO.USUARIO_ALTA ?? DBNull.Value));
            //sqlComando.Parameters.Add(new SqlParameter("@FechaAlta", (object)usuarioRolDTO.USUARIO_MANTENIMIENTO.FECHA_ALTA ?? DBNull.Value));
            sqlComando.Parameters.Add(new SqlParameter("@UsuarioModificacion", (object)usuarioRolDTO.USUARIO_MANTENIMIENTO.USUARIO_MODIFICO.ToUpper() ?? DBNull.Value));

            return Convert.ToInt32(sqlComando.ExecuteScalar());

        }

        public Int32 DarBajaRoles(string usuarioModificacion, Int32 idUsuario)
        {
            SqlCommand sqlComando;
            string sql = @" update usuarioRol set alta = 0, UsuarioModificacion = @UsuarioModificacion, FechaModificacion = getdate() where idusuario = @IdUsuario";

            sqlComando = new SqlCommand(sql, _sqlConn);

            if (_sqlTran != null)
                sqlComando.Transaction = _sqlTran;

            sqlComando.Parameters.Add(new SqlParameter("@UsuarioModificacion", (object)usuarioModificacion.ToUpper() ?? DBNull.Value));
            sqlComando.Parameters.Add(new SqlParameter("@IdUsuario", (object)idUsuario ?? DBNull.Value));

            return Convert.ToInt32(sqlComando.ExecuteScalar());
        }

        public List<UsuarioRolDTO> ListaUsuarioRol(Int32 IdUsuario)
        {
            SqlCommand sqlComando;
            string validacion = " WHERE a.IdUsuario = @IdUsuario and a.alta = 1 ";
            string sql = sqlSelect + validacion;

            sqlComando = new SqlCommand(sql, _sqlConn);

            if (_sqlTran != null)
                sqlComando.Transaction = _sqlTran;

            sqlComando.Parameters.Add(new SqlParameter("@IdUsuario", (object)IdUsuario ?? DBNull.Value));
            return CargarReader(sqlComando.ExecuteReader());
        }

        protected List<UsuarioRolDTO> CargarReader(SqlDataReader sqlReader)
        {
            UsuarioRolDTO _usuarioRolDTO = null;
            List<UsuarioRolDTO> _listaUsuarioRol = new List<UsuarioRolDTO>();

            while (sqlReader.Read())
            {
                _usuarioRolDTO = new UsuarioRolDTO();
                _usuarioRolDTO.ID_USUARIO = sqlReader.GetInt32(0);
                _usuarioRolDTO.NOMBRE_USUARIO = sqlReader.GetString(1);
                _usuarioRolDTO.ID_ROL = sqlReader.GetInt16(2);
                _usuarioRolDTO.NOMBRE_ROL = sqlReader.GetString(3);
                _usuarioRolDTO.ALTA = sqlReader.GetBoolean(4);
                _usuarioRolDTO.USUARIO_MANTENIMIENTO.USUARIO_ALTA = sqlReader.GetString(5);
                _usuarioRolDTO.USUARIO_MANTENIMIENTO.FECHA_ALTA = sqlReader.GetDateTime(6);
                _usuarioRolDTO.USUARIO_MANTENIMIENTO.USUARIO_MODIFICO = sqlReader.IsDBNull(7) ? null : sqlReader.GetString(7);
                _usuarioRolDTO.USUARIO_MANTENIMIENTO.FECHA_MODIFICACION = sqlReader.IsDBNull(8) ? (DateTime?)null : sqlReader.GetDateTime(8);

                _listaUsuarioRol.Add(_usuarioRolDTO);

            }

            sqlReader.Close();
            return _listaUsuarioRol;
        }

        public Int16 DarBajaUsuarioRol(Int32 IdUsuario, Int32 IdRol, string usuario)
        {
            SqlCommand sqlComando;
            string sql = @"Update UsuarioRol set Alta = 0, UsuarioModificacion = @UsuarioModificacion, FechaModificacion = getdate()
                            where IdUsuario = @IdUsuario and IdRol = @IdRol";

            sqlComando = new SqlCommand(sql, _sqlConn);

            if (_sqlTran != null)
                sqlComando.Transaction = _sqlTran;

            sqlComando.Parameters.Add(new SqlParameter("@IdUsuario", (object)IdUsuario ?? DBNull.Value));
            sqlComando.Parameters.Add(new SqlParameter("@IdRol", (object)IdRol ?? DBNull.Value));
            sqlComando.Parameters.Add(new SqlParameter("@UsuarioModificacion", (object)usuario ?? DBNull.Value));

            return Convert.ToInt16(sqlComando.ExecuteNonQuery());
        }

        public Int16 DarAltaUsuarioRol(Int32 IdUsuario, Int32 IdRol, string usuario)
        {
            SqlCommand sqlComando;
            string sql = @"Update UsuarioRol set Alta = 1, UsuarioModificacion = @UsuarioModificacion, FechaModificacion = getdate()
                            where IdUsuario = @IdUsuario and IdRol = @IdRol";

            sqlComando = new SqlCommand(sql, _sqlConn);

            if (_sqlTran != null)
                sqlComando.Transaction = _sqlTran;

            sqlComando.Parameters.Add(new SqlParameter("@IdUsuario", (object)IdUsuario ?? DBNull.Value));
            sqlComando.Parameters.Add(new SqlParameter("@IdRol", (object)IdRol ?? DBNull.Value));
            sqlComando.Parameters.Add(new SqlParameter("@UsuarioModificacion", (object)usuario ?? DBNull.Value));

            return Convert.ToInt16(sqlComando.ExecuteNonQuery());
        }

        public bool ExisteRolAsignado(Int32 idUsuario, Int16 idRol)
        {
            SqlCommand sqlComando;
            string sql = @"SELECT COUNT(idUsuario) 
                            FROM UsuarioRol 
                            WHERE idUsuario = @idUsuario 
                            AND idRol = @idRol";

            sqlComando = new SqlCommand(sql, _sqlConn);

            if (_sqlTran != null)
                sqlComando.Transaction = _sqlTran;

            sqlComando.Parameters.Add(new SqlParameter("@IdUsuario", (object)idUsuario ?? DBNull.Value));
            sqlComando.Parameters.Add(new SqlParameter("@IdRol", (object)idRol ?? DBNull.Value));

            return Convert.ToInt32(sqlComando.ExecuteScalar()) > 0 ? false : true;
        }

        public string[] BuscarRolesUsuarios(string usuario)
        {
            SqlCommand sqlComando;
            List<UsuarioRolDTO> _usuarioRolDto = new List<UsuarioRolDTO>();
            string[] usuarios;
            int pos = 0;

            string sql = sqlSelect + @" where a.Alta = 1
                                               and b.alta = 1
                                               and c.alta = 1
                                               and Usuario = @Usuario ";

            sqlComando = new SqlCommand(sql, _sqlConn);

            if (_sqlTran != null)
                sqlComando.Transaction = _sqlTran;

            sqlComando.Parameters.Add(new SqlParameter("@Usuario", (object)usuario ?? DBNull.Value));

            _usuarioRolDto = CargarReader(sqlComando.ExecuteReader());

            usuarios = new string[_usuarioRolDto.Count];

            foreach (UsuarioRolDTO usuRol in _usuarioRolDto)
            {
                usuarios[pos] = usuRol.NOMBRE_ROL;
                pos++;
            }

            return usuarios;
        }

        #endregion
    }
}
