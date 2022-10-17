using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using DipCmiGT.LogicaComun;

namespace LogicaCajasChicas.Entidad
{
    public class SociedadMoneda
    {
        #region Declaraciones

        private SqlConnection _sqlConn = null;
        private SqlTransaction _sqlTran = null;

        protected string sqlSelect = @"SELECT     IdSociedadMoneda, a.CodigoSociedad, b.Nombre, a.Moneda, a.Estado, UsuarioCreacion, a.FechaCreacion, a.UsuarioModificacion, a.FechaModificacion
                                       FROM         SociedadMoneda a
                                       inner join Sociedad b on b.CodigoSociedad = a.CodigoSociedad and b.Alta = 1
                                       inner join Moneda c on c.Moneda = a.Moneda and c.Estado = 1 ";

        protected string sqlInsert = @" INSERT INTO SociedadMoneda (CodigoSociedad, Moneda, UsuarioCreacion)
                                                            VALUES (@CodigoSociedad, @Moneda, @UsuarioCreacion) ";

        protected string sqlUpdate = @" ";

        #endregion

        #region Constructores

        public SociedadMoneda(SqlConnection sqlConn)
        {
            _sqlConn = sqlConn;
        }

        public SociedadMoneda(SqlConnection sqlConn, SqlTransaction sqlTran)
        {
            _sqlConn = sqlConn;
            _sqlTran = sqlTran;
        }

        #endregion

        #region Metodos

        public SociedadMonedaDTO EjecutarSentenciaSelect(Int32 idSociedadMoneda)
        {
            SqlCommand _sqlComando = null;
            List<SociedadMonedaDTO> _listaUsuarioCentroCostoDto = null; ;
            string sql = sqlSelect + " Where IdSociedadMoneda = @IdSociedadMoneda";

            _sqlComando = new SqlCommand(sql, _sqlConn);

            if (_sqlTran != null)
                _sqlComando.Transaction = _sqlTran;

            _sqlComando.Parameters.Add(new SqlParameter("@IdUsuarioCentroCosto", (object)idSociedadMoneda ?? DBNull.Value));

            _listaUsuarioCentroCostoDto = CargarReader(_sqlComando.ExecuteReader());

            return (_listaUsuarioCentroCostoDto.Count > 0) ? _listaUsuarioCentroCostoDto[0] : null;
        }

        public Int16 EjecutarSentenciaInsert(SociedadMonedaDTO sociedadMonedaDto)
        {
            SqlCommand sqlComando = new SqlCommand(sqlInsert, _sqlConn);
            sqlComando.CommandType = CommandType.Text;

            if (_sqlTran != null)
                sqlComando.Transaction = _sqlTran;

            sqlComando.Parameters.Add(new SqlParameter("@CodigoSociedad", (object)sociedadMonedaDto.CODIGO_SOCIEDAD ?? DBNull.Value));
            sqlComando.Parameters.Add(new SqlParameter("@Moneda", (object)sociedadMonedaDto.MONEDA.ToUpper() ?? DBNull.Value));
            sqlComando.Parameters.Add(new SqlParameter("@UsuarioCreacion", (object)sociedadMonedaDto.USUARIO_MANTENIMIENTO.USUARIO_ALTA.ToUpper() ?? DBNull.Value));

            return Convert.ToInt16(sqlComando.ExecuteScalar());
        }

        public List<SociedadMonedaDTO> ListarSociedadMoneda(string CodigoSociedad)
        {
            SqlCommand _sqlComando = null;
            string sql = sqlSelect + " where b.CodigoSociedad = @CodigoSociedad and a.Estado = 1 and c.Estado = 1 ";

            _sqlComando = new SqlCommand(sql, _sqlConn);
            _sqlComando.CommandType = CommandType.Text;

            if (_sqlTran != null)
                _sqlComando.Transaction = _sqlTran;

            _sqlComando.Parameters.Add(new SqlParameter("@CodigoSociedad", (object)CodigoSociedad ?? DBNull.Value));

            return CargarReader(_sqlComando.ExecuteReader());
        }

        public SociedadMonedaDTO BuscarSociedadMoneda(string codigoSociedad, string moneda)
        {
            SqlCommand _sqlComando = null;
            List<SociedadMonedaDTO> _listaUsuarioCentroCostoDto = null; ;
            string sql = sqlSelect + @"  where a.CodigoSociedad = @CodigoSociedad
                                         and a.Moneda = @Moneda ";

            _sqlComando = new SqlCommand(sql, _sqlConn);

            if (_sqlTran != null)
                _sqlComando.Transaction = _sqlTran;

            _sqlComando.Parameters.Add(new SqlParameter("@CodigoSociedad", (object)codigoSociedad ?? DBNull.Value));
            _sqlComando.Parameters.Add(new SqlParameter("@Moneda", (object)moneda ?? DBNull.Value));


            _listaUsuarioCentroCostoDto = CargarReader(_sqlComando.ExecuteReader());

            return (_listaUsuarioCentroCostoDto.Count > 0) ? _listaUsuarioCentroCostoDto[0] : null;
        }

        public Int16 DarBajaSociedadMoneda(Int16 idSociedadMoneda, string usuario)
        {
            SqlCommand _sqlComando = null;
            string sql = @"Update SociedadMoneda set Estado = 0, UsuarioModificacion = @UsuarioModificacion, FechaModificacion = getdate()
                            where IdSociedadMoneda = @IdSociedadMoneda";

            _sqlComando = new SqlCommand(sql, _sqlConn);
            _sqlComando.CommandType = CommandType.Text;

            if (_sqlTran != null)
                _sqlComando.Transaction = _sqlTran;

            _sqlComando.Parameters.Add(new SqlParameter("@UsuarioModificacion", (object)usuario.ToUpper() ?? DBNull.Value));
            _sqlComando.Parameters.Add(new SqlParameter("@IdSociedadMoneda", (object)idSociedadMoneda ?? DBNull.Value));

            return Convert.ToInt16(_sqlComando.ExecuteNonQuery());
        }

        public Int16 DarAltaSociedadMoneda(Int32 idSociedadMoneda, string usuario)
        {
            SqlCommand _sqlComando = null;
            string sql = @"Update SociedadMoneda set Estado = 1, UsuarioModificacion = @UsuarioModificacion, FechaModificacion = getdate()
                            where IdSociedadMoneda = @IdSociedadMoneda";

            _sqlComando = new SqlCommand(sql, _sqlConn);
            _sqlComando.CommandType = CommandType.Text;

            if (_sqlTran != null)
                _sqlComando.Transaction = _sqlTran;

            _sqlComando.Parameters.Add(new SqlParameter("@UsuarioModificacion", (object)usuario.ToUpper() ?? DBNull.Value));
            _sqlComando.Parameters.Add(new SqlParameter("@IdSociedadMoneda", (object)idSociedadMoneda ?? DBNull.Value));

            return Convert.ToInt16(_sqlComando.ExecuteNonQuery());
        }

        public bool ExisteUsuarioCentroCosto(string codigoSociedad, string moneda)
        {
            SqlCommand _sqlComando = null;
            string sql = @" select COUNT(IdSociedadMoneda)
                            FROM         SociedadMoneda a
                            inner join Sociedad b on b.CodigoSociedad = a.CodigoSociedad and b.Alta = 1
                            inner join Moneda c on c.Moneda = a.Moneda and c.Estado = 1 
                            where a.CodigoSociedad = @CodigoSociedad
                            and a.Moneda = @Moneda ";

            _sqlComando = new SqlCommand(sql, _sqlConn);
            _sqlComando.CommandType = CommandType.Text;

            if (_sqlTran != null)
                _sqlComando.Transaction = _sqlTran;

            _sqlComando.Parameters.Add(new SqlParameter("@CodigoSociedad", (object)codigoSociedad ?? DBNull.Value));
            _sqlComando.Parameters.Add(new SqlParameter("@Moneda", (object)moneda ?? DBNull.Value));

            return Convert.ToInt32(_sqlComando.ExecuteScalar()) == 0 ? false : true;
        }

        public List<SociedadMonedaDTO> CargarReader(SqlDataReader sqlReader)
        {
            SociedadMonedaDTO _sociedadMonedaDto = null;
            List<SociedadMonedaDTO> _listaSociedadMonedaDto = new List<SociedadMonedaDTO>();

            try
            {
                while (sqlReader.Read())
                {
                    _sociedadMonedaDto = new SociedadMonedaDTO();

                    _sociedadMonedaDto.ID_SOCIEDAD_MONEDA = sqlReader.GetInt16(0);
                    _sociedadMonedaDto.CODIGO_SOCIEDAD = sqlReader.GetString(1);
                    _sociedadMonedaDto.NOMBRE_SOCIEDAD = sqlReader.GetString(2);
                    _sociedadMonedaDto.MONEDA = sqlReader.GetString(3);
                    _sociedadMonedaDto.ESTADO = sqlReader.GetBoolean(4);
                    _sociedadMonedaDto.USUARIO_MANTENIMIENTO.USUARIO_ALTA = sqlReader.GetString(5);
                    _sociedadMonedaDto.USUARIO_MANTENIMIENTO.FECHA_ALTA = sqlReader.GetDateTime(6);
                    _sociedadMonedaDto.USUARIO_MANTENIMIENTO.USUARIO_MODIFICO = sqlReader.IsDBNull(7) ? null : sqlReader.GetString(7);
                    _sociedadMonedaDto.USUARIO_MANTENIMIENTO.FECHA_MODIFICACION = sqlReader.IsDBNull(8) ? (DateTime?)null : sqlReader.GetDateTime(8);

                    _listaSociedadMonedaDto.Add(_sociedadMonedaDto);
                }
            }
            finally
            {
                if (sqlReader != null) sqlReader.Dispose();
            }
            return _listaSociedadMonedaDto;
        }

        public List<LlenarDDL_DTO> ListarMonedaAsociada(string codigoSociedad)
        {
            SqlCommand _sqlComando = null;
            string sql = @" Select cast(a.IdSociedadMoneda as varchar(4)), a.Moneda + '-' + b.Descripcion
                            from SociedadMoneda a
                            inner join Moneda b on b.Moneda = a.Moneda and b.Estado = 1
                            where CodigoSociedad = @CodigoSociedad 
                            and a.Estado = 1 ";

            _sqlComando = new SqlCommand(sql, _sqlConn);
            _sqlComando.CommandType = CommandType.Text;

            if (_sqlTran != null)
                _sqlComando.Transaction = _sqlTran;

            _sqlComando.Parameters.Add(new SqlParameter("@CodigoSociedad", (object)codigoSociedad ?? DBNull.Value));

            return CargarReaderDDL(_sqlComando.ExecuteReader());
        }

        public List<LlenarDDL_DTO> CargarReaderDDL(SqlDataReader sqlDataReader)
        {
            List<LlenarDDL_DTO> _listaLlenarDDL = new List<LlenarDDL_DTO>();
            LlenarDDL_DTO _llenarDDL = null;

            while (sqlDataReader.Read())
            {
                _llenarDDL = new LlenarDDL_DTO();
                _llenarDDL.IDENTIFICADOR = sqlDataReader.GetString(0);
                _llenarDDL.DESCRIPCION = sqlDataReader.GetString(1);
                _listaLlenarDDL.Add(_llenarDDL);
            }

            return _listaLlenarDDL;
        }

        #endregion
    }
}
