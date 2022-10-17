using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using DipCmiGT.LogicaComun;

namespace DipCmiGT.LogicaCajasChicas.Entidad
{
    public class UsuarioCentroCosto
    {
        #region Declaraciones

        private SqlConnection _sqlConn = null;
        private SqlTransaction _sqlTran = null;

        protected string sqlSelect = @" select a.IdUsuarioCentroCosto, a.Usuario, a.CentroCosto, a.IdSociedadCentro, a.Alta, a.UsuarioCreacion, a.FechaCreacion,
	                                        a.UsuarioModificacion, a.FechaModificacion, c.CodigoSociedad, c.Nombre, d.IdCentro, d.Nombre, VERAK, KTEXT, (a.CentroCosto + '::' + KTEXT + '/' + ISNULL(LTEXT,'')) CentroCostoDescripcion 
                                        from UsuarioCentroCosto a
                                        inner join SociedadCentro b on b.IdSociedadCentro = a.IdSociedadCentro and b.Alta = 1
                                        inner join Sociedad c on c.CodigoSociedad = b.CodigoSociedad and c.Alta = 1
                                        inner join Centro d on d.IdCentro = b.IdCentro and d.Alta = 1
                                        inner join sap.CentroCostoTMP e on e.KOSTL = a.CentroCosto ";
        /// VERAK, KTEXT, (a.CentroCosto + ' :: ' + KTEXT) CentroCostoDescripcion Modificado 04.03.2021

        protected string sqlInsert = @" INSERT INTO UsuarioCentroCosto (Usuario, CentroCosto, IdSociedadCentro, UsuarioCreacion)
                                                                VALUES (@Usuario, @CentroCosto, @IdSociedadCentro, @UsuarioCreacion)
                                        SELECT @@IDENTITY ";

        protected string sqlUpdate = @" UPDATE UsuarioCentroCosto SET Usuario = @Usuario, CentroCosto = @CentroCosto, IdSociedadCentro = @IdSociedadCentro, UsuarioModificacion = @UsuarioModificacion, 
                                        FechaModificacion = getdate()
                                       WHERE IdUsuarioCentroCosto = @IdUsuarioCentroCosto ";


        #endregion

        #region Constructores

        public UsuarioCentroCosto(SqlConnection sqlConn)
        {
            _sqlConn = sqlConn;
        }

        public UsuarioCentroCosto(SqlConnection sqlConn, SqlTransaction sqlTran)
        {
            _sqlConn = sqlConn;
            _sqlTran = sqlTran;
        }

        #endregion

        #region Metodos

        public UsuarioCentroCostoDTO EjecutarSentenciaSelect(Int32 idUsuarioCentroCosto)
        {
            SqlCommand _sqlComando = null;
            List<UsuarioCentroCostoDTO> _listaUsuarioCentroCostoDto = null; ;
            string sql = sqlSelect + " Where IdUsuarioCentroCosto = @IdUsuarioCentroCosto";

            _sqlComando = new SqlCommand(sql, _sqlConn);

            if (_sqlTran != null)
                _sqlComando.Transaction = _sqlTran;

            _sqlComando.Parameters.Add(new SqlParameter("@IdUsuarioCentroCosto", (object)idUsuarioCentroCosto ?? DBNull.Value));

            _listaUsuarioCentroCostoDto = CargarReader(_sqlComando.ExecuteReader());

            return (_listaUsuarioCentroCostoDto.Count > 0) ? _listaUsuarioCentroCostoDto[0] : null;
        }

        public List<LlenarDDL_DTO> ListaCentroCostoUsuarioDDL(string usuario, string idSociedadCentro)
        {
            List<LlenarDDL_DTO> _listaLlenarDDL = new List<LlenarDDL_DTO>();
            LlenarDDL_DTO _llenarDDL = null;
            SqlCommand _sqlComando = null;
            string sql = @" Select distinct(a.Centrocosto), a.CentroCosto + ' - ' +  c.KTEXT from  UsuarioCentroCosto a
                            inner join sap.CuentaPorCentroCostoTMP b on b.KOSTL = a.CentroCosto
                            inner join sap.CentroCostoTMP c on c.KOSTL = b.KOSTL and c.KOKRS = b.KOKRS and c.BUKRS = @BUKRS
                            where a.Usuario = @Usuario";

            _sqlComando = new SqlCommand(sql, _sqlConn);
            _sqlComando.CommandType = CommandType.Text;

            if (_sqlTran != null)
                _sqlComando.Transaction = _sqlTran;

            _sqlComando.Parameters.Add(new SqlParameter("@BUKRS", (object)idSociedadCentro ?? DBNull.Value));
            _sqlComando.Parameters.Add(new SqlParameter("@Usuario", (object)usuario ?? DBNull.Value));

            SqlDataReader _sqlDataR = _sqlComando.ExecuteReader();

            while (_sqlDataR.Read())
            {
                _llenarDDL = new LlenarDDL_DTO();

                _llenarDDL.IDENTIFICADOR = _sqlDataR.GetString(0);
                _llenarDDL.DESCRIPCION = _sqlDataR.GetString(1);

                _listaLlenarDDL.Add(_llenarDDL);
            }

            return _listaLlenarDDL;
        }

        public List<UsuarioCentroCostoDTO> ListaCentroCostoUsuario(string usuario, Int32 idSociedadCentro)
        {
            SqlCommand _sqlComando = null;
            string sql = sqlSelect + @" WHERE A.Usuario = @Usuario
                                        AND a.IdSociedadCentro = @IdSociedadCentro ";

            _sqlComando = new SqlCommand(sql, _sqlConn);

            if (_sqlTran != null)
                _sqlComando.Transaction = _sqlTran;

            _sqlComando.Parameters.Add(new SqlParameter("@Usuario", (object)usuario ?? DBNull.Value));
            _sqlComando.Parameters.Add(new SqlParameter("@IdSociedadCentro", (object)idSociedadCentro ?? DBNull.Value));

            return CargarReader(_sqlComando.ExecuteReader());
        }

        public List<UsuarioCentroCostoDTO> ListaUsuarioSociedadCentro(string usuario)
        {
            SqlCommand _sqlComando = null;
            string sql = sqlSelect + @" WHERE A.Usuario = @Usuario ";

            _sqlComando = new SqlCommand(sql, _sqlConn);

            if (_sqlTran != null)
                _sqlComando.Transaction = _sqlTran;

            _sqlComando.Parameters.Add(new SqlParameter("@Usuario", (object)usuario ?? DBNull.Value));

            return CargarReader(_sqlComando.ExecuteReader());
        }

        public List<UsuarioCentroCostoDTO> ListaUsuarioSociedadCentro(string usuario, Int32 idSociedadCentro)
        {
            SqlCommand _sqlComando = null;
            string sql = sqlSelect + @" WHERE A.Usuario = @Usuario and b.IdSociedadCentro = @IdSociedadCentro and a.alta = 1 ";

            _sqlComando = new SqlCommand(sql, _sqlConn);

            if (_sqlTran != null)
                _sqlComando.Transaction = _sqlTran;

            _sqlComando.Parameters.Add(new SqlParameter("@Usuario", (object)usuario ?? DBNull.Value));
            _sqlComando.Parameters.Add(new SqlParameter("@IdSociedadCentro", (object)idSociedadCentro ?? DBNull.Value));

            return CargarReader(_sqlComando.ExecuteReader());
        }

        public Int32 EjecutarSentenciaInsert(UsuarioCentroCostoDTO usuariocentrocostoDTO)
        {
            SqlCommand sqlComando = new SqlCommand(sqlInsert, _sqlConn);
            sqlComando.CommandType = CommandType.Text;

            if (_sqlTran != null)
                sqlComando.Transaction = _sqlTran;

            sqlComando.Parameters.Add(new SqlParameter("@Usuario", (object)usuariocentrocostoDTO.USUARIO.ToUpper() ?? DBNull.Value));
            sqlComando.Parameters.Add(new SqlParameter("@CentroCosto", (object)usuariocentrocostoDTO.CENTRO_COSTO ?? DBNull.Value));
            sqlComando.Parameters.Add(new SqlParameter("@IdSociedadCentro", (object)usuariocentrocostoDTO.ID_SOCIEDAD_CENTRO ?? DBNull.Value));
            sqlComando.Parameters.Add(new SqlParameter("@UsuarioCreacion", (object)usuariocentrocostoDTO.USUARIO_MANTENIMIENTO.USUARIO_ALTA.ToUpper() ?? DBNull.Value));

            return Convert.ToInt32(sqlComando.ExecuteScalar());
        }

        public Int32 EjecutarSentenciaUpdate(UsuarioCentroCostoDTO usuariocentrocostoDTO)
        {
            SqlCommand sqlComando;

            sqlComando = new SqlCommand(sqlUpdate, _sqlConn);
            sqlComando.CommandType = CommandType.Text;

            if (_sqlTran != null)
                sqlComando.Transaction = _sqlTran;

            sqlComando.Parameters.Add(new SqlParameter("@Usuario", (object)usuariocentrocostoDTO.USUARIO.ToUpper() ?? DBNull.Value));
            sqlComando.Parameters.Add(new SqlParameter("@CentroCosto", (object)usuariocentrocostoDTO.CENTRO_COSTO ?? DBNull.Value));
            sqlComando.Parameters.Add(new SqlParameter("@IdSociedadCentro", (object)usuariocentrocostoDTO.ID_SOCIEDAD_CENTRO ?? DBNull.Value));
            sqlComando.Parameters.Add(new SqlParameter("@UsuarioModificacion", (object)usuariocentrocostoDTO.USUARIO_MANTENIMIENTO.USUARIO_MODIFICO.ToUpper() ?? DBNull.Value));
            sqlComando.Parameters.Add(new SqlParameter("@IdUsuarioCentroCosto", (object)usuariocentrocostoDTO.ID_USUARIO_CENTRO_COSTO ?? DBNull.Value));

            return Convert.ToInt32(sqlComando.ExecuteScalar());
        }

        public UsuarioCentroCostoDTO BuscarCentroCosto(string usuario, string centroCosto, Int32 idSociedadCentro)
        {
            SqlCommand _sqlComando = null;
            List<UsuarioCentroCostoDTO> _listaUsuarioCentroCostoDto = null; ;
            string sql = sqlSelect + @"  where a.Usuario = @Usuario
                                         and a.CentroCosto = @CentroCosto
                                         and a.IdSociedadCentro = @IdSociedadCentro  ";

            _sqlComando = new SqlCommand(sql, _sqlConn);

            if (_sqlTran != null)
                _sqlComando.Transaction = _sqlTran;

            _sqlComando.Parameters.Add(new SqlParameter("@Usuario", (object)usuario ?? DBNull.Value));
            _sqlComando.Parameters.Add(new SqlParameter("@CentroCosto", (object)centroCosto ?? DBNull.Value));
            _sqlComando.Parameters.Add(new SqlParameter("@IdSociedadCentro", (object)idSociedadCentro ?? DBNull.Value));

            _listaUsuarioCentroCostoDto = CargarReader(_sqlComando.ExecuteReader());

            return (_listaUsuarioCentroCostoDto.Count > 0) ? _listaUsuarioCentroCostoDto[0] : null;
        }

        public Int16 DarBajaCentroCosto(Int32 idCentro, string usuario)
        {
            SqlCommand _sqlComando = null;
            string sql = @"Update UsuarioCentroCosto set Alta = 0, UsuarioModificacion = @UsuarioModificacion, FechaModificacion = getdate()
                            where IdUsuarioCentroCosto = @IdUsuarioCentroCosto";

            _sqlComando = new SqlCommand(sql, _sqlConn);
            _sqlComando.CommandType = CommandType.Text;

            if (_sqlTran != null)
                _sqlComando.Transaction = _sqlTran;

            _sqlComando.Parameters.Add(new SqlParameter("@UsuarioModificacion", (object)usuario.ToUpper() ?? DBNull.Value));
            _sqlComando.Parameters.Add(new SqlParameter("@IdUsuarioCentroCosto", (object)idCentro ?? DBNull.Value));

            return Convert.ToInt16(_sqlComando.ExecuteNonQuery());
        }

        public Int16 DarAltaCentroCosto(Int32 idCentro, string usuario)
        {
            SqlCommand _sqlComando = null;
            string sql = @"Update UsuarioCentroCosto set Alta = 1, UsuarioModificacion = @UsuarioModificacion, FechaModificacion = getdate()
                            where IdUsuarioCentroCosto = @IdUsuarioCentroCosto";

            _sqlComando = new SqlCommand(sql, _sqlConn);
            _sqlComando.CommandType = CommandType.Text;

            if (_sqlTran != null)
                _sqlComando.Transaction = _sqlTran;

            _sqlComando.Parameters.Add(new SqlParameter("@UsuarioModificacion", (object)usuario.ToUpper() ?? DBNull.Value));
            _sqlComando.Parameters.Add(new SqlParameter("@IdUsuarioCentroCosto", (object)idCentro ?? DBNull.Value));

            return Convert.ToInt16(_sqlComando.ExecuteNonQuery());
        }

        public bool ExisteUsuarioCentroCosto(string usuario, string centroCosto, Int32 idSociedadCentro)
        {
            SqlCommand _sqlComando = null;
            string sql = @" select COUNT(IdUsuarioCentroCosto)
                            from UsuarioCentroCosto a
                            inner join SociedadCentro b on b.IdSociedadCentro = a.IdSociedadCentro and b.Alta = 1
                            inner join Sociedad c on c.CodigoSociedad = b.CodigoSociedad and c.Alta = 1
                            inner join Centro d on d.IdCentro = b.IdCentro and d.Alta = 1 
                            where a.Usuario = @Usuario
                            and a.CentroCosto = @CentroCosto
                            and a.IdSociedadCentro = @IdSociedadCentro ";

            _sqlComando = new SqlCommand(sql, _sqlConn);
            _sqlComando.CommandType = CommandType.Text;

            if (_sqlTran != null)
                _sqlComando.Transaction = _sqlTran;

            _sqlComando.Parameters.Add(new SqlParameter("@Usuario", (object)usuario ?? DBNull.Value));
            _sqlComando.Parameters.Add(new SqlParameter("@CentroCosto", (object)centroCosto ?? DBNull.Value));
            _sqlComando.Parameters.Add(new SqlParameter("@IdSociedadCentro", (object)idSociedadCentro ?? DBNull.Value));

            return Convert.ToInt32(_sqlComando.ExecuteScalar()) == 0 ? false : true;
        }

        public List<UsuarioCentroCostoDTO> CargarReader(SqlDataReader sqlReader)
        {
            UsuarioCentroCostoDTO _usuarioCentroCostoDto = null;
            List<UsuarioCentroCostoDTO> _listaCentroCostoDto = new List<UsuarioCentroCostoDTO>();

            try
            {
                while (sqlReader.Read())
                {
                    _usuarioCentroCostoDto = new UsuarioCentroCostoDTO();

                    _usuarioCentroCostoDto.ID_USUARIO_CENTRO_COSTO = sqlReader.GetInt32(0);
                    _usuarioCentroCostoDto.USUARIO = sqlReader.GetString(1);
                    _usuarioCentroCostoDto.CENTRO_COSTO = sqlReader.GetString(2);
                    _usuarioCentroCostoDto.ID_SOCIEDAD_CENTRO = sqlReader.GetInt32(3);
                    _usuarioCentroCostoDto.ALTA = sqlReader.GetBoolean(4);
                    _usuarioCentroCostoDto.USUARIO_MANTENIMIENTO.USUARIO_ALTA = sqlReader.GetString(5);
                    _usuarioCentroCostoDto.USUARIO_MANTENIMIENTO.FECHA_ALTA = sqlReader.GetDateTime(6);
                    _usuarioCentroCostoDto.USUARIO_MANTENIMIENTO.USUARIO_MODIFICO = sqlReader.IsDBNull(7) ? null : sqlReader.GetString(7);
                    _usuarioCentroCostoDto.USUARIO_MANTENIMIENTO.FECHA_MODIFICACION = sqlReader.IsDBNull(8) ? (DateTime?)null : sqlReader.GetDateTime(8);
                    _usuarioCentroCostoDto.CODIGO_SOCIEDAD = sqlReader.GetString(9);
                    _usuarioCentroCostoDto.NOMBRE_SOCIEDAD = sqlReader.GetString(10);
                    _usuarioCentroCostoDto.ID_CENTRO = sqlReader.GetInt32(11);
                    _usuarioCentroCostoDto.NOMBRE_CENTRO = sqlReader.GetString(12);
                    _usuarioCentroCostoDto.VERAK = sqlReader.GetString(13);
                    _usuarioCentroCostoDto.KTEXT = sqlReader.GetString(14);
                    _usuarioCentroCostoDto.CENTRO_COSTO_DESCRIPCION = sqlReader.GetString(15);

                    _listaCentroCostoDto.Add(_usuarioCentroCostoDto);
                }
            }
            finally
            {
                if (sqlReader != null) sqlReader.Dispose();
            }
            return _listaCentroCostoDto;
        }

        public List<LlenarDDL_DTO> ListarSociedadCentroCosto(string usuario)
        {
            SqlCommand _sqlComando = null;
            string sql = @"select distinct c.CodigoSociedad, c.CodigoSociedad +' :: '+ c.Nombre
                            from UsuarioCentroCosto a
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

        public List<LlenarDDL_DTO> ListarCentroCentroCosto(string usuario, string codigoSociedad)
        {
            SqlCommand _sqlComando = null;
            string sql = @"select CAST(a.IdSociedadCentro as varchar(3)), cast(d.IdCentro as varchar(3)) +' :: '+ d.Nombre
                            from UsuarioCentroCosto a
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

        public List<UsuarioCentroCostoDTO> ListaCuentaCosto(string usuario, Int32 idSociedadCentro)
        {
            SqlCommand _sqlComando = null;
            string sql = sqlSelect + @" WHERE A.Usuario = @Usuario
                                        AND a.IdSociedadCentro = @IdSociedadCentro
                                        AND a.Alta = 1";

            _sqlComando = new SqlCommand(sql, _sqlConn);
            _sqlComando.CommandType = CommandType.Text;

            if (_sqlTran != null)
                _sqlComando.Transaction = _sqlTran;

            _sqlComando.Parameters.Add(new SqlParameter("@Usuario", (object)usuario ?? DBNull.Value));
            _sqlComando.Parameters.Add(new SqlParameter("@IdSociedadCentro", (object)idSociedadCentro ?? DBNull.Value));

            return CargarReader(_sqlComando.ExecuteReader());
        }

        #endregion

        #region MapeoRegistradorAprobador

        public List<LlenarDDL_DTO> BuscarAprobador(string ceco, string centro)
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
            _sqlcomando.Parameters.Add(new SqlParameter("CENTRO", (object)@centro ?? DBNull.Value));
            _sqlcomando.Parameters.Add(new SqlParameter("OPCION", 2));

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

        public List<LlenarDDL_DTO> BuscarRegistrador(string ceco, string centro)
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
            _sqlcomando.Parameters.Add(new SqlParameter("CENTRO", (object)@centro ?? DBNull.Value));
            _sqlcomando.Parameters.Add(new SqlParameter("OPCION", 3));

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

        public List<MapeoRegistradorAprobadorDTO> ListaMapeoRegistradorAprobador(string ceco, string centro, string aprobador, string registrador)
        {
            SqlCommand sqlComando = null;
            string sql = sqlSelect;

            sqlComando = new SqlCommand(@"SPMapeoRegistradorAprobador", _sqlConn);
            sqlComando.CommandType = CommandType.StoredProcedure;
            if (_sqlTran != null)
                sqlComando.Transaction = _sqlTran;

            sqlComando.Parameters.Add(new SqlParameter("CECO", (object)@ceco ?? DBNull.Value));
            sqlComando.Parameters.Add(new SqlParameter("CENTRO", (object)@centro ?? DBNull.Value));
            sqlComando.Parameters.Add(new SqlParameter("APROBADOR", (object)@aprobador ?? DBNull.Value));
            sqlComando.Parameters.Add(new SqlParameter("REGISTRADOR", (object)@registrador ?? DBNull.Value));
            sqlComando.Parameters.Add(new SqlParameter("OPCION", 4));

            return CargarReaderMapeoRegistradorAprobador(sqlComando.ExecuteReader());
        }

        protected List<MapeoRegistradorAprobadorDTO> CargarReaderMapeoRegistradorAprobador(SqlDataReader sqlReader)
        {
            MapeoRegistradorAprobadorDTO _mapeoDto = null;
            List<MapeoRegistradorAprobadorDTO> _listaMapeo = new List<MapeoRegistradorAprobadorDTO>();

            while (sqlReader.Read())
            {
                _mapeoDto = new MapeoRegistradorAprobadorDTO();

                _mapeoDto.CECO_ORDEN = sqlReader.GetString(0);
                _mapeoDto.ID_CENTRO = sqlReader.GetString(1);
                _mapeoDto.CENTRO = sqlReader.GetString(2);
                _mapeoDto.APROBADOR_US = sqlReader.GetString(3);
                _mapeoDto.APROBADOR = sqlReader.GetString(4);
                _mapeoDto.REGISTRADOR_US = sqlReader.GetString(5);
                _mapeoDto.REGISTRADOR = sqlReader.GetString(6);
                _mapeoDto.USUARIO_ALTA = sqlReader.GetString(7);

                _listaMapeo.Add(_mapeoDto);
            }
            sqlReader.Close();

            return _listaMapeo;
        }

        public List<MapeoRegistradorAprobadorDTO> InsertarMapeoRegistradorAprobador(string ceco, string centro, string aprobador, string registrador, string usuario)
        {
            SqlCommand sqlComando = null;
            string sql = sqlSelect;

            sqlComando = new SqlCommand(@"SPMapeoRegistradorAprobador", _sqlConn);
            sqlComando.CommandType = CommandType.StoredProcedure;
            if (_sqlTran != null)
                sqlComando.Transaction = _sqlTran;

            sqlComando.Parameters.Add(new SqlParameter("CECO", (object)@ceco ?? DBNull.Value));
            sqlComando.Parameters.Add(new SqlParameter("CENTRO", (object)@centro ?? DBNull.Value));
            sqlComando.Parameters.Add(new SqlParameter("APROBADOR", (object)@aprobador ?? DBNull.Value));
            sqlComando.Parameters.Add(new SqlParameter("REGISTRADOR", (object)@registrador ?? DBNull.Value));
            sqlComando.Parameters.Add(new SqlParameter("USUARIO", (object)@usuario ?? DBNull.Value));
            sqlComando.Parameters.Add(new SqlParameter("OPCION", 5));

            return CargarReaderMapeoRegistradorAprobador(sqlComando.ExecuteReader());
        }

        public List<MapeoRegistradorAprobadorDTO> EliminarMapeoRegistradorAprobador(string ceco, string centro, string aprobador, string registrador, string usuario)
        {
            SqlCommand sqlComando = null;
            string sql = sqlSelect;

            sqlComando = new SqlCommand(@"SPMapeoRegistradorAprobador", _sqlConn);
            sqlComando.CommandType = CommandType.StoredProcedure;
            if (_sqlTran != null)
                sqlComando.Transaction = _sqlTran;

            sqlComando.Parameters.Add(new SqlParameter("CECO", (object)@ceco ?? DBNull.Value));
            sqlComando.Parameters.Add(new SqlParameter("CENTRO", (object)@centro ?? DBNull.Value));
            sqlComando.Parameters.Add(new SqlParameter("APROBADOR", (object)@aprobador ?? DBNull.Value));
            sqlComando.Parameters.Add(new SqlParameter("REGISTRADOR", (object)@registrador ?? DBNull.Value));
            sqlComando.Parameters.Add(new SqlParameter("USUARIO", (object)@usuario ?? DBNull.Value));
            sqlComando.Parameters.Add(new SqlParameter("OPCION", 6));

            return CargarReaderMapeoRegistradorAprobador(sqlComando.ExecuteReader());
        }
        #endregion
    }
}
