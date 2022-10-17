using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;

namespace DipCmiGT.LogicaCajasChicas.Entidad
{
    class Rol
    {
        #region Declaraciones

        private SqlConnection _sqlConn = null;
        private SqlTransaction _sqlTran = null;

        protected string sqlSelect = @" SELECT IdRol, Rol, Alta, UsuarioAlta, FechaAlta, UsuarioModificacion, FechaModificacion
                                        FROM Rol";

        protected string sqlInsert = @" INSERT INTO Rol (Rol, UsuarioAlta)
		                                         VALUES (@Rol, @UsuarioAlta)
                                        SELECT @@identity ";

        protected string sqlUpdate = @" UPDATE Rol SET Rol = @Rol, UsuarioModificacion = @UsuarioModificacion, Alta = @Alta, FechaModificacion = GETDATE()
                                        WHERE IdRol = @IdRol ";

        #endregion

        #region Constructores


        public Rol(SqlConnection sqlConn)
        {
            _sqlConn = sqlConn;
        }

        public Rol(SqlConnection sqlConn, SqlTransaction sqlTran)
        {
            _sqlConn = sqlConn;
            _sqlTran = sqlTran;
        }

        #endregion

        #region Metodos

        /// <summary>
        /// Consulta registro rol.
        /// </summary>
        /// <param name="idProveedor">Identificador único</param>
        /// <returns>RolDTO</returns>
        public RolDTO EjecutarSentenciaSelect(Int32 idRol)
        {
            List<RolDTO> listaRol = null;
            SqlCommand sqlComando;
            string sql = sqlSelect + " Where IdRol = @IdRol ";

            sqlComando = new SqlCommand(sql, _sqlConn);
            sqlComando.CommandType = CommandType.Text;

            if (_sqlTran != null)
                sqlComando.Transaction = _sqlTran;

            sqlComando.Parameters.Add(new SqlParameter("@IdRol", (object)idRol ?? DBNull.Value));

            listaRol = CargarReader(sqlComando.ExecuteReader());

            return listaRol.Count > 0 ? listaRol[0] : null;
        }

        /// <summary>
        /// Inserta registro de rol.
        /// </summary>
        /// <param name="RolDTO">RolDTO</param>
        /// <returns>IdRol</returns>
        public Int16 EjecutarSenteciaInsert(RolDTO RolDTO)
        {
            SqlCommand sqlComando;

            sqlComando = new SqlCommand(sqlInsert, _sqlConn);
            sqlComando.CommandType = CommandType.Text;

            if (_sqlTran != null)
                sqlComando.Transaction = _sqlTran;

            sqlComando.Parameters.Add(new SqlParameter("@Rol", (object)RolDTO.NOMBRE_ROL ?? DBNull.Value));
            sqlComando.Parameters.Add(new SqlParameter("@UsuarioAlta", (object)RolDTO.USUARIO_MANTENIMIENTO.USUARIO_ALTA ?? DBNull.Value));

            return Convert.ToInt16(sqlComando.ExecuteScalar());
        }

        /// <summary>
        /// Actualiza registro de rol.
        /// </summary>
        /// <param name="rolDTO">RolDTO</param>
        /// <returns>Filas actualizadas</returns>
        public Int32 EjecutarSentenciaUpdate(RolDTO rolDTO)
        {
            SqlCommand sqlComando;

            sqlComando = new SqlCommand(sqlUpdate, _sqlConn);
            sqlComando.CommandType = CommandType.Text;

            if (_sqlTran != null)
                sqlComando.Transaction = _sqlTran;

            sqlComando.Parameters.Add(new SqlParameter("@IdRol", (object)rolDTO.ID_ROL ?? DBNull.Value));
            sqlComando.Parameters.Add(new SqlParameter("@Rol", (object)rolDTO.NOMBRE_ROL ?? DBNull.Value));
            sqlComando.Parameters.Add(new SqlParameter("@UsuarioModificacion", (object)rolDTO.USUARIO_MANTENIMIENTO.USUARIO_MODIFICO ?? DBNull.Value));
            sqlComando.Parameters.Add(new SqlParameter("@Alta", (object)rolDTO.ALTA ?? DBNull.Value));

            return Convert.ToInt32(sqlComando.ExecuteNonQuery());
        }

        /// <summary>
        /// Encapsula objeto rol.
        /// </summary>
        /// <param name="sqlReader">SqlReader</param>
        /// <returns>Lista RolDTO</returns>
        protected List<RolDTO> CargarReader(SqlDataReader sqlReader)
        {
            RolDTO _rolDTO = null;
            List<RolDTO> _listaRol = new List<RolDTO>();

            while (sqlReader.Read())
            {
                _rolDTO = new RolDTO();

                _rolDTO.ID_ROL = sqlReader.GetInt16(0);
                _rolDTO.NOMBRE_ROL = sqlReader.GetString(1);
                _rolDTO.ALTA = sqlReader.GetBoolean(2);
                _rolDTO.USUARIO_MANTENIMIENTO.USUARIO_ALTA = sqlReader.GetString(3);
                _rolDTO.USUARIO_MANTENIMIENTO.FECHA_ALTA = sqlReader.GetDateTime(4);
                _rolDTO.USUARIO_MANTENIMIENTO.USUARIO_MODIFICO = sqlReader.IsDBNull(5) ? null : sqlReader.GetString(5);
                _rolDTO.USUARIO_MANTENIMIENTO.FECHA_MODIFICACION = sqlReader.IsDBNull(6) ? (DateTime?)null : sqlReader.GetDateTime(6);

                _listaRol.Add(_rolDTO);
            }
            sqlReader.Close();

            return _listaRol;
        }


        /// <summary>
        /// Lista todos los proveedores
        /// </summary>
        /// <returns>Lista ProveedorDTO</returns>
        public List<RolDTO> ListaRoles()
        {
            SqlCommand sqlComando = null;
            string sql = sqlSelect;

            sqlComando = new SqlCommand(sql, _sqlConn);
            sqlComando.CommandType = CommandType.Text;

            if (_sqlTran != null)
                sqlComando.Transaction = _sqlTran;

            return CargarReader(sqlComando.ExecuteReader());
        }

        /// <summary>
        /// Método que da baja al proveedor.
        /// </summary>
        /// <param name="idProveedor">Número de identificación</param>
        /// <param name="usuario">Usuario da baja</param>
        /// <returns>Registro actualizado</returns>
        public Int16 DarBajaRol(Int32 idRol, string usuario)
        {
            SqlCommand sqlComando;
            string sql = @" Update Rol set Alta = 0, UsuarioModificacion = @UsuarioModificacion, FechaModificacion = getdate()
                            where IdRol = @IdRol";

            sqlComando = new SqlCommand(sql, _sqlConn);
            sqlComando.CommandType = CommandType.Text;

            if (_sqlTran != null)
                sqlComando.Transaction = _sqlTran;

            sqlComando.Parameters.Add(new SqlParameter("@UsuarioModificacion", (object)usuario ?? DBNull.Value));
            sqlComando.Parameters.Add(new SqlParameter("@IdRol", (object)idRol ?? DBNull.Value));

            return Convert.ToInt16(sqlComando.ExecuteNonQuery());
        }

        /// <summary>
        /// Método que da alta al proveedor.
        /// </summary>
        /// <param name="idProveedor">Número de identicicación</param>
        /// <param name="usuario">Usuario que da alta</param>
        /// <returns>Registro actualizado</returns>
        public Int16 DarAltaRol(Int32 idRol, string usuario)
        {
            SqlCommand sqlComando;
            string sql = @" Update Rol set Alta = 1, UsuarioModificacion = @UsuarioModificacion, FechaModificacion = getdate()
                            where IdRol = @IdRol";

            sqlComando = new SqlCommand(sql, _sqlConn);
            sqlComando.CommandType = CommandType.Text;

            if (_sqlTran != null)
                sqlComando.Transaction = _sqlTran;

            sqlComando.Parameters.Add(new SqlParameter("@UsuarioModificacion", (object)usuario ?? DBNull.Value));
            sqlComando.Parameters.Add(new SqlParameter("@IdRol", (object)idRol ?? DBNull.Value));

            return Convert.ToInt16(sqlComando.ExecuteNonQuery());
        }

        #endregion
    }
}
