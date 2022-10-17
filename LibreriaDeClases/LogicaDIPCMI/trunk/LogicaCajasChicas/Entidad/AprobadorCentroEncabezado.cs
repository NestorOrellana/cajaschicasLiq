using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace DipCmiGT.LogicaCajasChicas.Entidad
{
    public class AprobadorCentroEncabezado
    {

        #region Declaraciones

        private SqlConnection _sqlConn = null;
        private SqlTransaction _sqlTran = null;

        protected string sqlSelect = @" SELECT a.IdAprobacionEncabezado, a.IdUsuario, b.Usuario, a.PorcentajeCompra,
                                        a.Tolerancia, a.Alta, a.UsuarioCreacion, a.FechaCreacion, a.UsuarioModificacion,
                                        a.FechaModificacion
                                        FROM AprobacionCentroEncabezado a
                                        inner join Usuario b on b.IdUsuario = a.IdUsuario";


        protected string sqlInsert = @"INSERT INTO AprobacionCentroEncabezado  (IdUsuario, PorcentajeCompra, Tolerancia, UsuarioCreacion)
                                       VALUES (@IdUsuario, @PorcentajeCompra, @Tolerancia, @UsuarioCreacion)";

        protected string sqlUpdate = @"UPDATE AprobacionCentroEncabezado SET PorcentajeCompra = @PorcentajeCompra, Tolerancia = @Tolerancia,
                                       Alta = @Alta,  UsuarioModificacion = @UsuarioModificacion, FechaModificacion = getdate()
                                       WHERE IdAprobacionEncabezado = @IdAprobacionEncabezado";

        #endregion

        #region Constructores
        public AprobadorCentroEncabezado(SqlConnection sqlConn)
        {
            _sqlConn = sqlConn;
        }

        public AprobadorCentroEncabezado(SqlConnection sqlConn, SqlTransaction sqlTran)
        {
            _sqlConn = sqlConn;
            _sqlTran = sqlTran;
        }

        #endregion

        #region Metodos

        public AprobadorCentroEncabezadoDTO EjecutarSentenciaSelect(Int32 IdAprobadorCentro)
        {
            List<AprobadorCentroEncabezadoDTO> listaAprobadorCentro = null;
            SqlCommand sqlComando;
            string sql = sqlSelect + "";

            sqlComando = new SqlCommand(sql, _sqlConn);
            sqlComando.CommandType = CommandType.Text;

            if (_sqlTran != null)
                sqlComando.Transaction = _sqlTran;

            sqlComando.Parameters.Add(new SqlParameter("@IdAprobadorCentro", (object)IdAprobadorCentro ?? DBNull.Value));
            return listaAprobadorCentro.Count > 0 ? listaAprobadorCentro[0] : null;
        }

        public Int32 EjecutarSentenciaInsert(AprobadorCentroEncabezadoDTO aprobadorcentroEncabezadoDto)
        {
            SqlCommand sqlCommando = new SqlCommand(sqlInsert, _sqlConn);
            sqlCommando.CommandType = CommandType.Text;

            if (_sqlTran != null)
                sqlCommando.Transaction = _sqlTran;

            sqlCommando.Parameters.Add(new SqlParameter("@IdUsuario", (object)aprobadorcentroEncabezadoDto.ID_USUARIO ?? DBNull.Value));
            sqlCommando.Parameters.Add(new SqlParameter("@PorcentajeCompra", (object)aprobadorcentroEncabezadoDto.PORCENTAJE_COMPRA ?? DBNull.Value));
            sqlCommando.Parameters.Add(new SqlParameter("@Tolerancia", (object)aprobadorcentroEncabezadoDto.TOLERANCIA ?? DBNull.Value));
            sqlCommando.Parameters.Add(new SqlParameter("@UsuarioCreacion", (object)aprobadorcentroEncabezadoDto.USUARIO_MANTENIMIENTO.USUARIO_ALTA ?? DBNull.Value));

            return Convert.ToInt32(sqlCommando.ExecuteScalar());
        }

        public Int32 EjecutarSentenciaUpdate(AprobadorCentroEncabezadoDTO aprobadorcentroEcnabezadoDto)
        {
            SqlCommand sqlComando;

            sqlComando = new SqlCommand(sqlUpdate, _sqlConn);

            if (_sqlTran != null)
                sqlComando.Transaction = _sqlTran;

            sqlComando.Parameters.Add(new SqlParameter("@IdAprobacionEncabezado", (object)aprobadorcentroEcnabezadoDto.ID_APROBACION_ENCABEZADO ?? DBNull.Value));
            sqlComando.Parameters.Add(new SqlParameter("@PorcentajeCompra", (object)aprobadorcentroEcnabezadoDto.PORCENTAJE_COMPRA ?? DBNull.Value));
            sqlComando.Parameters.Add(new SqlParameter("@Tolerancia", (object)aprobadorcentroEcnabezadoDto.TOLERANCIA ?? DBNull.Value));
            sqlComando.Parameters.Add(new SqlParameter("@UsuarioModificacion", (object)aprobadorcentroEcnabezadoDto.USUARIO_MANTENIMIENTO.USUARIO_MODIFICO ?? DBNull.Value));
            sqlComando.Parameters.Add(new SqlParameter("@Alta", (object)aprobadorcentroEcnabezadoDto.ALTA ?? DBNull.Value));

            return Convert.ToInt32(sqlComando.ExecuteScalar());
        }

        protected List<AprobadorCentroEncabezadoDTO> CargarReader(SqlDataReader sqlReader)
        {
            AprobadorCentroEncabezadoDTO _aprobadorcentroEncabezadoDto = null;
            List<AprobadorCentroEncabezadoDTO> _listaAprobadorCentroEncabezado = new List<AprobadorCentroEncabezadoDTO>();

            while (sqlReader.Read())
            {
                _aprobadorcentroEncabezadoDto = new AprobadorCentroEncabezadoDTO();
                _aprobadorcentroEncabezadoDto.ID_APROBACION_ENCABEZADO = sqlReader.GetInt32(0);
                _aprobadorcentroEncabezadoDto.ID_USUARIO = sqlReader.GetInt32(1);
                _aprobadorcentroEncabezadoDto.USUARIO = sqlReader.GetString(2);
                _aprobadorcentroEncabezadoDto.PORCENTAJE_COMPRA = sqlReader.GetInt16(3);
                _aprobadorcentroEncabezadoDto.TOLERANCIA = sqlReader.GetInt32(4);
                _aprobadorcentroEncabezadoDto.ALTA = sqlReader.GetBoolean(5);
                _aprobadorcentroEncabezadoDto.USUARIO_MANTENIMIENTO.USUARIO_ALTA = sqlReader.GetString(6);
                _aprobadorcentroEncabezadoDto.USUARIO_MANTENIMIENTO.FECHA_ALTA = sqlReader.GetDateTime(7);
                _aprobadorcentroEncabezadoDto.USUARIO_MANTENIMIENTO.USUARIO_MODIFICO = sqlReader.IsDBNull(8) ? null : sqlReader.GetString(8);
                _aprobadorcentroEncabezadoDto.USUARIO_MANTENIMIENTO.FECHA_MODIFICACION = sqlReader.IsDBNull(9) ? (DateTime?)null : sqlReader.GetDateTime(9);

                _listaAprobadorCentroEncabezado.Add(_aprobadorcentroEncabezadoDto);
            }

            sqlReader.Close();
            return _listaAprobadorCentroEncabezado;


        #endregion
        }

        public List<AprobadorCentroEncabezadoDTO> ListaAprobadorCentroEncabezado(Int32 Idusuario)
        {
            SqlCommand sqlComando;
            string sql = sqlSelect + @" where a.IdUsuario = @IdUsuario";

            sqlComando = new SqlCommand(sql, _sqlConn);

            if (_sqlTran != null)
                sqlComando.Transaction = _sqlTran;

            sqlComando.Parameters.Add(new SqlParameter("@IdUsuario", (object)Idusuario ?? DBNull.Value));

            return CargarReader(sqlComando.ExecuteReader());
        }

        public Int16 DarBajaAprobadorCentroEncabezado(Int32 idApobadorCentro, string usuario)
        {
            SqlCommand _sqlComando = null;
            string sql = @"Update AprobacionCentroEncabezado set Alta = 0, UsuarioModificacion = @UsuarioModificacion, FechaModificacion = getdate()
                            where IdAprobacionEncabezado = @IdAprobacionEncabezado";

            _sqlComando = new SqlCommand(sql, _sqlConn);
            _sqlComando.CommandType = CommandType.Text;

            if (_sqlTran != null)
                _sqlComando.Transaction = _sqlTran;

            _sqlComando.Parameters.Add(new SqlParameter("@UsuarioModificacion", (object)usuario ?? DBNull.Value));
            _sqlComando.Parameters.Add(new SqlParameter("@IdAprobacionEncabezado", (object)idApobadorCentro ?? DBNull.Value));

            return Convert.ToInt16(_sqlComando.ExecuteNonQuery());
        }

        public Int16 DarAltaAprobadorCentroEncabezado(Int32 idApobadorCentro, string usuario)
        {
            SqlCommand _sqlComando = null;
            string sql = @"Update AprobacionCentroEncabezado set Alta = 1, UsuarioModificacion = @UsuarioModificacion, FechaModificacion = getdate()
                            where IdAprobacionEncabezado = @IdAprobacionEncabezado";

            _sqlComando = new SqlCommand(sql, _sqlConn);
            _sqlComando.CommandType = CommandType.Text;

            if (_sqlTran != null)
                _sqlComando.Transaction = _sqlTran;

            _sqlComando.Parameters.Add(new SqlParameter("@UsuarioModificacion", (object)usuario ?? DBNull.Value));
            _sqlComando.Parameters.Add(new SqlParameter("@IdAprobacionEncabezado", (object)idApobadorCentro ?? DBNull.Value));

            return Convert.ToInt16(_sqlComando.ExecuteNonQuery());
        }

        public bool ExisteAprobadorCentroEncabezado(Int32 IdUsuario, Int16 Porcentaje, Int32 Tolerancia)
        {
            SqlCommand sqlComando;
            string sql = @"select COUNT(IdAprobacionEncabezado) 
                            from AprobacionCentroEncabezado
                            where IdUsuario = @IdUsuario
                            and PorcentajeCompra = @PorcentajeCompra
                            and Tolerancia = @Tolerancia";

            sqlComando = new SqlCommand(sql, _sqlConn);

            if (_sqlTran != null)
                sqlComando.Transaction = _sqlTran;
            sqlComando.Parameters.Add(new SqlParameter("@IdUsuario", (object)IdUsuario ?? DBNull.Value));
            sqlComando.Parameters.Add(new SqlParameter("@PorcentajeCompra", (object)Porcentaje ?? DBNull.Value));
            sqlComando.Parameters.Add(new SqlParameter("@Tolerancia", (object)Tolerancia ?? DBNull.Value));
            return Convert.ToInt32(sqlComando.ExecuteScalar()) > 0 ? true : false;
        }

    }
}
