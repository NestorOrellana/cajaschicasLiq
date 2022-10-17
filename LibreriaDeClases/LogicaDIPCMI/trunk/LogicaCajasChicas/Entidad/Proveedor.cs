using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;

namespace DipCmiGT.LogicaCajasChicas.Entidad
{
    public class Proveedor
    {
        #region Declaraciones

        private SqlConnection _sqlConn = null;
        private SqlTransaction _sqlTran = null;

        protected string sqlSelect = @"  SELECT IdProveedor, a.IdTipoDocumento, Descripcion, NumeroIdentificacion, Nombre, Direccion, Regimen, a.Alta, a.UsuarioAlta, a.FechaCreacion, a.UsuarioModificacion, a.FechaModificacion,
                                        ISNULL(a.IdTipoDocumento2,0) TipoDoc2, a.NumeroIdentificacion2, (Select c.Descripcion from dbo.TipoDocumento as c WHERE c.IdTipoDocumento = a.IdTipoDocumento2) as Descripcion2, ISNULL(a.Tipo,0) AS Tipo 
                                        FROM Proveedor a
                                        inner join TipoDocumento b on a.idtipodocumento = b.IdTipoDocumento and b.alta = 1 ";

        protected string sqlInsert = @"INSERT INTO Proveedor (IdTipoDocumento, NumeroIdentificacion, IdTipoDocumento2, NumeroIdentificacion2,Nombre, Direccion, Regimen, UsuarioAlta, Tipo)
                                                      VALUES (@IdTipoDocumento, @NumeroIdentificacion, @IdTipoDocumento2, @NumeroIdentificacion2, @Nombre, @Direccion, @Regimen, @UsuarioAlta, @Tipo) 
                                       SELECT @@IDENTITY";

        protected string sqlUpdate = @"UPDATE Proveedor SET IdTipoDocumento = @IdTipoDocumento, NumeroIdentificacion = @NumeroIdentificacion, 
                                                            IdTipoDocumento2 = @IdTipoDocumento2, NumeroIdentificacion2 = @NumeroIdentificacion2, 
                                                            Nombre = @Nombre, Direccion = @Direccion, Regimen = @Regimen, 
                                                            Alta = @Alta, UsuarioModificacion = @UsuarioModificacion, FechaModificacion = getdate(),
                                                            Tipo = @Tipo
                                                            WHERE IdProveedor = @IdProveedor ";

        #endregion

        #region Constructores


        public Proveedor(SqlConnection sqlConn)
        {
            _sqlConn = sqlConn;
        }

        public Proveedor(SqlConnection sqlConn, SqlTransaction sqlTran)
        {
            _sqlConn = sqlConn;
            _sqlTran = sqlTran;
        }

        #endregion

        #region Metodos

        public ProveedorDTO EjecutarSentenciaSelect(Int32 idProveedor)
        {
            List<ProveedorDTO> listaProveedores = null;
            SqlCommand sqlComando;
            string sql = sqlSelect + " Where IdProveedor = @IdProveedor ";

            sqlComando = new SqlCommand(sql, _sqlConn);
            sqlComando.CommandType = CommandType.Text;

            if (_sqlTran != null)
                sqlComando.Transaction = _sqlTran;

            sqlComando.Parameters.Add(new SqlParameter("@IdProveedor", (object)idProveedor ?? DBNull.Value));

            listaProveedores = CargarReader(sqlComando.ExecuteReader());

            return listaProveedores.Count > 0 ? listaProveedores[0] : null;
        }

        public ProveedorDTO BuscarProveedor(Int32 tipoDocumento, string numeroIdentificacion)
        {
            List<ProveedorDTO> listaProveedores = null;
            SqlCommand sqlComando;
            string sql = sqlSelect + @" Where (a.IdTipoDocumento = @IdTipoDocumento 
                                        and NumeroIdentificacion = @NumeroIdentificacion) 
                                        or (a.IdTipoDocumento2 = @IdTipoDocumento 
                                        and NumeroIdentificacion2 = @NumeroIdentificacion)";

                                    
            sqlComando = new SqlCommand(sql, _sqlConn);
            sqlComando.CommandType = CommandType.Text;

            if (_sqlTran != null)
                sqlComando.Transaction = _sqlTran;

            sqlComando.Parameters.Add(new SqlParameter("@IdTipoDocumento", (object)tipoDocumento ?? DBNull.Value));
            sqlComando.Parameters.Add(new SqlParameter("@NumeroIdentificacion", (object)numeroIdentificacion ?? DBNull.Value));

            listaProveedores = CargarReader(sqlComando.ExecuteReader());

            return listaProveedores.Count > 0 ? listaProveedores[0] : null;
        }

        public Int32 EjecutarSenteciaInsert(ProveedorDTO proveedorDTO)
        {
            SqlCommand sqlComando;

            sqlComando = new SqlCommand(sqlInsert, _sqlConn);
            sqlComando.CommandType = CommandType.Text;

            if (_sqlTran != null)
                sqlComando.Transaction = _sqlTran;

           // sqlComando.Parameters.Add(new SqlParameter("@TipoDocumento", (object)proveedorDTO.TIPO_DOCUMENTO ?? DBNull.Value));
            sqlComando.Parameters.Add(new SqlParameter("@IdTipoDocumento", (object)proveedorDTO.ID_TIPO_DOCUMENTO ?? DBNull.Value));
            sqlComando.Parameters.Add(new SqlParameter("@NumeroIdentificacion", (object)proveedorDTO.NUMERO_IDENTIFICACION.ToUpper() ?? DBNull.Value));
            sqlComando.Parameters.Add(new SqlParameter("@Nombre", (object)proveedorDTO.NOMBRE.ToUpper() ?? DBNull.Value));
            sqlComando.Parameters.Add(new SqlParameter("@Direccion", (object)proveedorDTO.DIRECCION.ToUpper() ?? DBNull.Value));
            sqlComando.Parameters.Add(new SqlParameter("@Regimen", (object)proveedorDTO.REGIMEN ?? DBNull.Value));
            sqlComando.Parameters.Add(new SqlParameter("@UsuarioAlta", (object)proveedorDTO.USUARIO_MANTENIMIENTO.USUARIO_ALTA.ToUpper()));
            sqlComando.Parameters.Add(new SqlParameter("@IdTipoDocumento2", (object)proveedorDTO.ID_TIPO_DOCUMENTO2 ?? DBNull.Value));
            sqlComando.Parameters.Add(new SqlParameter("@NumeroIdentificacion2", (object)proveedorDTO.NUMERO_IDENTIFICACION2.ToUpper() ?? DBNull.Value));
            sqlComando.Parameters.Add(new SqlParameter("@Tipo", (object)proveedorDTO.TIPO ?? DBNull.Value));

            return Convert.ToInt32(sqlComando.ExecuteScalar());
        }

        public Int32 EjecutarSentenciaUpdate(ProveedorDTO proveedorDTO)
        {
            SqlCommand sqlComando;

            sqlComando = new SqlCommand(sqlUpdate, _sqlConn);

            if (_sqlTran != null)
                sqlComando.Transaction = _sqlTran;

            sqlComando.Parameters.Add(new SqlParameter("@IdTipoDocumento", (object)proveedorDTO.ID_TIPO_DOCUMENTO ?? DBNull.Value));
            sqlComando.Parameters.Add(new SqlParameter("@NumeroIdentificacion", (object)proveedorDTO.NUMERO_IDENTIFICACION ?? DBNull.Value));
            sqlComando.Parameters.Add(new SqlParameter("@Nombre", (object)proveedorDTO.NOMBRE ?? DBNull.Value));
            sqlComando.Parameters.Add(new SqlParameter("@Direccion", (object)proveedorDTO.DIRECCION ?? DBNull.Value));
            sqlComando.Parameters.Add(new SqlParameter("@Regimen", (object)proveedorDTO.REGIMEN ?? DBNull.Value));
            sqlComando.Parameters.Add(new SqlParameter("Alta", (object)proveedorDTO.ALTA ?? DBNull.Value));
            sqlComando.Parameters.Add(new SqlParameter("@UsuarioModificacion", (object)proveedorDTO.USUARIO_MANTENIMIENTO.USUARIO_MODIFICO ?? DBNull.Value));
            sqlComando.Parameters.Add(new SqlParameter("@FechaModificacion", (object)proveedorDTO.USUARIO_MANTENIMIENTO.FECHA_MODIFICACION ?? DBNull.Value));
            sqlComando.Parameters.Add(new SqlParameter("@IdProveedor", (object)proveedorDTO.ID_PROVEEDOR ?? DBNull.Value));
            sqlComando.Parameters.Add(new SqlParameter("@IdTipoDocumento2", (object)proveedorDTO.ID_TIPO_DOCUMENTO2 ?? DBNull.Value));
            sqlComando.Parameters.Add(new SqlParameter("@NumeroIdentificacion2", (object)proveedorDTO.NUMERO_IDENTIFICACION2 ?? DBNull.Value));
            sqlComando.Parameters.Add(new SqlParameter("@Tipo", (object)proveedorDTO.TIPO ?? DBNull.Value));

            return Convert.ToInt32(sqlComando.ExecuteNonQuery());
        }

        protected List<ProveedorDTO> CargarReader(SqlDataReader sqlReader)
        {
            ProveedorDTO _proveedorDto = null;
            List<ProveedorDTO> _listaProveedor = new List<ProveedorDTO>();

            while (sqlReader.Read())
            {
                _proveedorDto = new ProveedorDTO();

                _proveedorDto.ID_PROVEEDOR = sqlReader.GetInt32(0);
                _proveedorDto.ID_TIPO_DOCUMENTO = sqlReader.GetInt16(1);
                _proveedorDto.TIPO_DOCUMENTO = sqlReader.GetString(2);
                _proveedorDto.NUMERO_IDENTIFICACION = sqlReader.GetString(3);
                _proveedorDto.NOMBRE = sqlReader.GetString(4);
                _proveedorDto.DIRECCION = sqlReader.GetString(5);
                _proveedorDto.REGIMEN = sqlReader.IsDBNull(6) ? null: sqlReader.GetString(6);
                _proveedorDto.ALTA = sqlReader.GetBoolean(7);
                _proveedorDto.USUARIO_MANTENIMIENTO.USUARIO_ALTA = sqlReader.GetString(8);
                _proveedorDto.USUARIO_MANTENIMIENTO.FECHA_ALTA = sqlReader.GetDateTime(9);
                _proveedorDto.USUARIO_MANTENIMIENTO.USUARIO_MODIFICO = sqlReader.IsDBNull(10) ? null : sqlReader.GetString(10);
                _proveedorDto.USUARIO_MANTENIMIENTO.FECHA_MODIFICACION = sqlReader.IsDBNull(11) ? (DateTime?)null : sqlReader.GetDateTime(11);
                _proveedorDto.ID_TIPO_DOCUMENTO2 = sqlReader.GetInt16(12);//sqlReader.GetInt16(12);
                _proveedorDto.NUMERO_IDENTIFICACION2 = sqlReader.IsDBNull(13) ? null : sqlReader.GetString(13);
                _proveedorDto.TIPO_DOCUMENTO2 = sqlReader.IsDBNull(14) ? null : sqlReader.GetString(14);
                _proveedorDto.TIPO = sqlReader.GetBoolean(15);

                _listaProveedor.Add(_proveedorDto);
            }
            sqlReader.Close();

            return _listaProveedor;
        }

        public bool ExisteDocumento(Int16 idTipoDocumento, string numeroIdentificación)
        {
            SqlCommand sqlComando;
            string sql = @"select COUNT(IdProveedor)
                            from Proveedor
                            where IdTipoDocumento = @IdTipoDocumento
                            and NumeroIdentificacion = @NumeroIdentificacion
                            or (IdTipoDocumento2 = @IdTipoDocumento
                            and NumeroIdentificacion2 = @NumeroIdentificacion)";

            sqlComando = new SqlCommand(sql, _sqlConn);

            if (_sqlTran != null)
                sqlComando.Transaction = _sqlTran;

            sqlComando.Parameters.Add(new SqlParameter("@IdTipoDocumento", (object)idTipoDocumento ?? DBNull.Value));
            sqlComando.Parameters.Add(new SqlParameter("@NumeroIdentificacion", (object)numeroIdentificación ?? DBNull.Value));

            return Convert.ToInt32(sqlComando.ExecuteScalar()) > 0 ? false : true;
        }

        public List<ProveedorDTO> ListaProveedores()
        {
            SqlCommand sqlComando = null;
            string sql = sqlSelect;

            sqlComando = new SqlCommand(sql, _sqlConn);
            if (_sqlTran != null)
                sqlComando.Transaction = _sqlTran;

            return CargarReader(sqlComando.ExecuteReader());
        }

        public List<ProveedorDTO> BusquedaProveedores(Int16 tipoDoc, string identificacion, string nombre)
        {
            SqlCommand sqlComando = null;
            string sql = sqlSelect + @" where ((a.IdTipoDocumento = case when @TipoDocumento = -1 then a.IdTipoDocumento else @TipoDocumento end)
                                        and (NumeroIdentificacion = case when @NumeroIdentificacion = '' then NumeroIdentificacion else @NumeroIdentificacion end))
                                        and ((Nombre = case when @Nombre = '' then Nombre else @Nombre end) 
                                        or Nombre like '%' + @Nombre + '%')";

            sqlComando = new SqlCommand(sql, _sqlConn);
            if (_sqlTran != null)
                sqlComando.Transaction = _sqlTran;

            sqlComando.Parameters.Add(new SqlParameter("@TipoDocumento", (object)tipoDoc ?? DBNull.Value));
            sqlComando.Parameters.Add(new SqlParameter("@NumeroIdentificacion", (object)identificacion ?? DBNull.Value));
            sqlComando.Parameters.Add(new SqlParameter("@Nombre", (object)nombre ?? DBNull.Value));

            return CargarReader(sqlComando.ExecuteReader());
        }

        public List<ProveedorDTO> ListaProveedoresDDL()
        {
            SqlCommand sqlComando = null;
            string sql = sqlSelect + @" where a.Alta = 1";

            sqlComando = new SqlCommand(sql, _sqlConn);
            if (_sqlTran != null)
                sqlComando.Transaction = _sqlTran;

            return CargarReader(sqlComando.ExecuteReader());
        }

        public Int16 DarBajaProveedor(Int32 idProveedor, string usuario)
        {
            SqlCommand sqlComando;
            string sql = @" Update Proveedor set Alta = 0, UsuarioModificacion = @UsuarioModificacion, FechaModificacion = getdate()
                            where IdProveedor = @IdProveedor";

            sqlComando = new SqlCommand(sql, _sqlConn);

            if (_sqlTran != null)
                sqlComando.Transaction = _sqlTran;

            sqlComando.Parameters.Add(new SqlParameter("@UsuarioModificacion", (object)usuario ?? DBNull.Value));
            sqlComando.Parameters.Add(new SqlParameter("@IdProveedor", (object)idProveedor ?? DBNull.Value));

            return Convert.ToInt16(sqlComando.ExecuteNonQuery());
        }

        public Int16 DarAltaProveedor(Int32 idProveedor, string usuario)
        {
            SqlCommand sqlComando;
            string sql = @" Update Proveedor set Alta = 1, UsuarioModificacion = @UsuarioModificacion, FechaModificacion = getdate()
                            where IdProveedor = @IdProveedor";

            sqlComando = new SqlCommand(sql, _sqlConn);

            if (_sqlTran != null)
                sqlComando.Transaction = _sqlTran;

            sqlComando.Parameters.Add(new SqlParameter("@UsuarioModificacion", (object)usuario ?? DBNull.Value));
            sqlComando.Parameters.Add(new SqlParameter("@IdProveedor", (object)idProveedor ?? DBNull.Value));

            return Convert.ToInt16(sqlComando.ExecuteNonQuery());
        }

        public string TipoDocumento(Int16 idTipoDocumento)
        {
            SqlCommand sqlComando;
            string sql = @"SELECT TipoDocumento FROM dbo.TipoDocumento
                            WHERE IdTipoDocumento = @IdTipoDocumento";

            sqlComando = new SqlCommand(sql, _sqlConn);

            if (_sqlTran != null)
                sqlComando.Transaction = _sqlTran;

            sqlComando.Parameters.Add(new SqlParameter("@IdTipoDocumento", (object)idTipoDocumento ?? DBNull.Value));

            return Convert.ToString(sqlComando.ExecuteScalar());
        }

        #endregion
    }
}
