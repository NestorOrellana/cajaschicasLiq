using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace DipCmiGT.LogicaCajasChicas.Entidad
{
    public class AprobadorCentro
    {

        #region Declaraciones

        private SqlConnection _sqlConn = null;
        private SqlTransaction _sqlTran = null;

        protected string sqlSelect = @" SELECT a.IdAprobadorCentro, a.IdUsuario, a.KOSTL,
                                        a.AUFNR, a.IdSociedadCentro, c.Nombre,  a.IdNivel, d.Nivel, a.Alta, a.UsuarioCreacion, 
                                        a.FechaCreacion, a.UsuarioModificacion, a.FechaModificacion, b.CodigoSociedad, e.Nombre
                                        FROM AprobadorCentro a
                                        inner join SociedadCentro b on b.IdSociedadCentro = a.IdSociedadCentro 
                                        inner join Centro c on c.IdCentro = b.IdCentro 
                                        inner join Nivel d on d.IdNivel = a.IdNivel
                                        inner join Usuario e on e.IdUsuario = a.IdUsuario  ";

        //        protected string sqlSelect = @"SELECT a.IdAprobadorCentro, a.IdUsuario, a.KOSTL,
        //                                        a.AUFNR, a.IdSociedadCentro, c.Nombre,  a.IdNivel, d.Nivel, a.Alta, a.UsuarioCreacion, 
        //                                        a.FechaCreacion, a.UsuarioModificacion, a.FechaModificacion, b.CodigoSociedad
        //                                        FROM AprobadorCentro a
        //                                        inner join SociedadCentro b on b.IdSociedadCentro = a.IdSociedadCentro
        //                                        inner join Centro c on c.IdCentro = b.IdCentro
        //                                        inner join Nivel d on d.IdNivel = a.IdNivel";

        //                                        @"SELECT a.IdAprobadorCentro, a.IdAprobacionEncabezado, a.KOSTL,
        //                                        a.AUFNR, a.IdSociedadCentro, c.Nombre,  a.IdNivel, d.Nivel, a.Alta, a.UsuarioCreacion, 
        //                                        a.FechaCreacion, a.UsuarioModificacion, a.FechaModificacion,
        //                                        (Select CodigoSociedad from SociedadCentro where IdSociedadCentro = a.IdSociedadCentro)
        //                                        FROM AprobadorCentro a
        //                                        inner join SociedadCentro b on b.IdSociedadCentro = a.IdSociedadCentro
        //                                        inner join Centro c on c.IdCentro = b.IdCentro
        //                                        inner join Nivel d on d.IdNivel = a.IdNivel";

        protected string sqlInsert = @"INSERT INTO AprobadorCentro (IdUsuario, KOSTL, AUFNR, IdSociedadCentro, IdNivel, UsuarioCreacion)
                                       VALUES (@IdUsuario, @KOSTL, @AUFNR, @IdSociedadCentro, @IdNivel, @UsuarioCreacion)";

        //INSERT INTO AprobadorCentro (IdAprobacionEncabezado, KOSTL, AUFNR, IdCentro, IdNivel, UsuarioCreacion)
        //                                       VALUES (@IdAprobacionEncabezado, @KOSTL, @AUFNR, @IdCentro, @IdNivel, @UsuarioCreacion)";

        protected string sqlUpdate = @"UPDATE AprobadorCentro SET KOSTL = @KOSTL, AUFNR = @AUFNR, IdSociedadCentro = @IdSociedadCentro,
                                       IdNivel = @IdNivel, Alta = @Alta,  UsuarioModificacion = @UsuarioModificacion, 
                                       FechaModificacion = getdate()
                                       WHERE IdAprobadorCentro = @IdAprobadorCentro ";

        #endregion

        #region Constructores
        public AprobadorCentro(SqlConnection sqlConn)
        {
            _sqlConn = sqlConn;
        }

        public AprobadorCentro(SqlConnection sqlConn, SqlTransaction sqlTran)
        {
            _sqlConn = sqlConn;
            _sqlTran = sqlTran;
        }

        #endregion

        #region Metodos


        public AprobadorCentroDTO EjecutarSentenciaSelect(Int32 IdAprobadorCentro)
        {
            List<AprobadorCentroDTO> listaAprobadorCentro = null;
            SqlCommand sqlComando;
            string sql = sqlSelect + "";

            sqlComando = new SqlCommand(sql, _sqlConn);
            sqlComando.CommandType = CommandType.Text;

            if (_sqlTran != null)
                sqlComando.Transaction = _sqlTran;

            sqlComando.Parameters.Add(new SqlParameter("@IdAprobadorCentro", (object)IdAprobadorCentro ?? DBNull.Value));
            return listaAprobadorCentro.Count > 0 ? listaAprobadorCentro[0] : null;
        }

        public Int32 EjecutarSentenciaInsert(AprobadorCentroDTO aprobadorcentroDto)
        {
            SqlCommand sqlCommando = new SqlCommand(sqlInsert, _sqlConn);
            sqlCommando.CommandType = CommandType.Text;

            if (_sqlTran != null)
                sqlCommando.Transaction = _sqlTran;

            sqlCommando.Parameters.Add(new SqlParameter("@IdSociedadCentro", (object)aprobadorcentroDto.ID_SOCIEDAD_CENTRO ?? DBNull.Value));
            sqlCommando.Parameters.Add(new SqlParameter("@IdUsuario", (object)aprobadorcentroDto.ID_USUARIO ?? DBNull.Value));
            sqlCommando.Parameters.Add(new SqlParameter("@KOSTL", (object)aprobadorcentroDto.KOSTL ?? DBNull.Value));
            sqlCommando.Parameters.Add(new SqlParameter("@AUFNR", (object)aprobadorcentroDto.AUFNR ?? DBNull.Value));
            sqlCommando.Parameters.Add(new SqlParameter("@IdNivel", (object)aprobadorcentroDto.ID_NIVEL ?? DBNull.Value));
            sqlCommando.Parameters.Add(new SqlParameter("@UsuarioCreacion", (object)aprobadorcentroDto.USUARIO_MANTENIMIENTO.USUARIO_ALTA ?? DBNull.Value));
            return Convert.ToInt32(sqlCommando.ExecuteScalar());
        }

        public Int32 EjecutarSentenciaUpdate(AprobadorCentroDTO aprobadorcentroDto)
        {
            SqlCommand sqlComando;

            sqlComando = new SqlCommand(sqlUpdate, _sqlConn);

            if (_sqlTran != null)
                sqlComando.Transaction = _sqlTran;

            sqlComando.Parameters.Add(new SqlParameter("@KOSTL", (object)aprobadorcentroDto.KOSTL ?? DBNull.Value));
            sqlComando.Parameters.Add(new SqlParameter("@AUFNR", (object)aprobadorcentroDto.AUFNR ?? DBNull.Value));
            sqlComando.Parameters.Add(new SqlParameter("@IdSociedadCentro", (object)aprobadorcentroDto.ID_SOCIEDAD_CENTRO ?? DBNull.Value));
            sqlComando.Parameters.Add(new SqlParameter("@IdNivel", (object)aprobadorcentroDto.ID_NIVEL ?? DBNull.Value));
            sqlComando.Parameters.Add(new SqlParameter("@Alta", (object)aprobadorcentroDto.ALTA ?? DBNull.Value));
            sqlComando.Parameters.Add(new SqlParameter("@UsuarioModificacion", (object)aprobadorcentroDto.USUARIO_MANTENIMIENTO.USUARIO_MODIFICO ?? DBNull.Value));
            sqlComando.Parameters.Add(new SqlParameter("@IdAprobadorCentro", (object)aprobadorcentroDto.ID_APROBADORCENTRO ?? DBNull.Value));

            return Convert.ToInt32(sqlComando.ExecuteScalar());
        }

        protected List<AprobadorCentroDTO> CargarReader(SqlDataReader sqlReader)
        {
            AprobadorCentroDTO _aprobadorcentroDto = null;
            List<AprobadorCentroDTO> _listaAprobadorCentro = new List<AprobadorCentroDTO>();

            while (sqlReader.Read())
            {
                _aprobadorcentroDto = new AprobadorCentroDTO();
                _aprobadorcentroDto.ID_APROBADORCENTRO = sqlReader.GetInt32(0);
                _aprobadorcentroDto.ID_USUARIO = sqlReader.GetInt32(1);
                _aprobadorcentroDto.KOSTL = sqlReader.GetString(2);
                _aprobadorcentroDto.AUFNR = sqlReader.GetString(3);
                _aprobadorcentroDto.ID_SOCIEDAD_CENTRO = sqlReader.GetInt32(4);
                _aprobadorcentroDto.CENTRO = sqlReader.GetString(5);
                _aprobadorcentroDto.ID_NIVEL = sqlReader.GetInt16(6);
                _aprobadorcentroDto.NIVEL = sqlReader.GetString(7);
                _aprobadorcentroDto.ALTA = sqlReader.GetBoolean(8);
                _aprobadorcentroDto.USUARIO_MANTENIMIENTO.USUARIO_ALTA = sqlReader.GetString(9);
                _aprobadorcentroDto.USUARIO_MANTENIMIENTO.FECHA_ALTA = sqlReader.GetDateTime(10);
                _aprobadorcentroDto.USUARIO_MANTENIMIENTO.USUARIO_MODIFICO = sqlReader.IsDBNull(11) ? null : sqlReader.GetString(11);
                _aprobadorcentroDto.USUARIO_MANTENIMIENTO.FECHA_MODIFICACION = sqlReader.IsDBNull(12) ? (DateTime?)null : sqlReader.GetDateTime(12);
                _aprobadorcentroDto.SOCIEDAD = sqlReader.GetString(13);
                _aprobadorcentroDto.NOMBRE = sqlReader.IsDBNull(14) ? string.Empty : sqlReader.GetString(14);

                _listaAprobadorCentro.Add(_aprobadorcentroDto);
            }

            sqlReader.Close();
            return _listaAprobadorCentro;
        }

        #endregion

        public List<AprobadorCentroDTO> BuscarAprobadores(Int32 idSociedadCentro, string centroCosto, string ordenCosto)
        {
            SqlCommand sqlComando;
            string sql = sqlSelect + @" where b.Alta = 1
                                        and c.Alta = 1
                                        and e.alta = 1
                                        and a.IdSociedadCentro = @IdSociedadCentro
                                        and ((a.KOSTL = @KOSTL and a.AUFNR = @AUFNR) 
	                                        or (a.KOSTL = @KOSTL and a.AUFNR = @AUFNR))
                                        and a.Alta = 1 ";

            sqlComando = new SqlCommand(sql, _sqlConn);

            if (_sqlTran != null)
                sqlComando.Transaction = _sqlTran;

            sqlComando.Parameters.Add(new SqlParameter("@IdSociedadCentro", (object)idSociedadCentro ?? DBNull.Value));
            sqlComando.Parameters.Add(new SqlParameter("@KOSTL", (object)centroCosto ?? DBNull.Value));
            sqlComando.Parameters.Add(new SqlParameter("@AUFNR", (object)ordenCosto ?? DBNull.Value));


            return CargarReader(sqlComando.ExecuteReader());
        }

        public List<AprobadorCentroDTO> BuscarAprobadoresCentroCosto(Int32 idSociedadCentro, Int32 idUsuario, Int16 idNivel)
        {
            SqlCommand sqlComando;
            string sql = sqlSelect + @" where a.Alta = 1
                                        and b.Alta = 1
                                        and c.Alta = 1
                                        and e.alta = 1
                                        and KOSTL is not null
                                        and a.IdSociedadCentro = @idSociedadCentro
										and e.IdUsuario = @idUsuario 
										and d.IdNivel = @idNivel 
										 ";

            sqlComando = new SqlCommand(sql, _sqlConn);

            if (_sqlTran != null)
                sqlComando.Transaction = _sqlTran;

            sqlComando.Parameters.Add(new SqlParameter("@IdSociedadCentro", (object)idSociedadCentro ?? DBNull.Value));
            sqlComando.Parameters.Add(new SqlParameter("@idUsuario", (object)idUsuario ?? DBNull.Value));
            sqlComando.Parameters.Add(new SqlParameter("@idNivel", (object)idNivel ?? DBNull.Value));


            return CargarReader(sqlComando.ExecuteReader());
        }

        public List<AprobadorCentroDTO> ListaAprobadorCentro(Int32 IdUsuario)
        {
            SqlCommand sqlComando;
            string sql = sqlSelect + @" where a.IdUsuario = @IdUsuario";

            sqlComando = new SqlCommand(sql, _sqlConn);

            if (_sqlTran != null)
                sqlComando.Transaction = _sqlTran;

            sqlComando.Parameters.Add(new SqlParameter("@IdUsuario", (object)IdUsuario ?? DBNull.Value));

            return CargarReader(sqlComando.ExecuteReader());
        }

        public Int32 DarBajaAprobadorCentro(Int32 idApobadorCentro, string usuario)
        {
            SqlCommand _sqlComando = null;
            string sql = @"Update AprobadorCentro set Alta = 0, UsuarioModificacion = @UsuarioModificacion, FechaModificacion = getdate()
                            where IdAprobadorCentro = @IdAprobadorCentro";

            _sqlComando = new SqlCommand(sql, _sqlConn);
            _sqlComando.CommandType = CommandType.Text;

            if (_sqlTran != null)
                _sqlComando.Transaction = _sqlTran;

            _sqlComando.Parameters.Add(new SqlParameter("@UsuarioModificacion", (object)usuario ?? DBNull.Value));
            _sqlComando.Parameters.Add(new SqlParameter("@IdAprobadorCentro", (object)idApobadorCentro ?? DBNull.Value));

            return Convert.ToInt16(_sqlComando.ExecuteNonQuery());
        }

        public Int32 DarAltaAprobadorCentro(Int32 idApobadorCentro, string usuario)
        {
            SqlCommand _sqlComando = null;
            string sql = @"Update AprobadorCentro set Alta = 1, UsuarioModificacion = @UsuarioModificacion, FechaModificacion = getdate()
                            where IdAprobadorCentro = @IdAprobadorCentro";

            _sqlComando = new SqlCommand(sql, _sqlConn);
            _sqlComando.CommandType = CommandType.Text;

            if (_sqlTran != null)
                _sqlComando.Transaction = _sqlTran;

            _sqlComando.Parameters.Add(new SqlParameter("@UsuarioModificacion", (object)usuario ?? DBNull.Value));
            _sqlComando.Parameters.Add(new SqlParameter("@IdAprobadorCentro", (object)idApobadorCentro ?? DBNull.Value));

            return Convert.ToInt16(_sqlComando.ExecuteNonQuery());
        }

        public Int32 ExisteRegistro(string CentroCosto, string OrdenCompra, Int32 IdSociedadCentro, Int16 IdNivel, Int32 IdUsuario)
        {
            SqlCommand _sqlComando = null;
            string sql = @"Select TOP 1 IdAprobadorCentro
                            from AprobadorCentro
                            where KOSTL = @KOSTL
                            and AUFNR = @AUFNR
                            and IdSociedadCentro = @IdSociedadCentro
                            and IdNivel = @IdNivel
                            and IdUsuario = @IdUsuario";

            _sqlComando = new SqlCommand(sql, _sqlConn);

            if (_sqlTran != null)
                _sqlComando.Transaction = _sqlTran;

            _sqlComando.Parameters.Add(new SqlParameter("@KOSTL", (object)CentroCosto ?? DBNull.Value));
            _sqlComando.Parameters.Add(new SqlParameter("@AUFNR", (object)OrdenCompra ?? DBNull.Value));
            _sqlComando.Parameters.Add(new SqlParameter("@IdSociedadCentro", (object)IdSociedadCentro ?? DBNull.Value));
            _sqlComando.Parameters.Add(new SqlParameter("@IdNivel", (object)IdNivel ?? DBNull.Value));
            _sqlComando.Parameters.Add(new SqlParameter("@IdUsuario", (object)IdUsuario ?? DBNull.Value));

            //return Convert.ToInt32(_sqlComando.ExecuteScalar()) > 0 ? false : true;
            return Convert.ToInt32(_sqlComando.ExecuteScalar());
        }

        public AprobadorCentroDTO BuscarCentroCosto(string CentroCosto, Int32 IdSociedadCentro, Int16 IdNivel, Int32 IdUsuario)
        {
            SqlCommand _sqlComando = null;
            List<AprobadorCentroDTO> _aprobadorCentroDto = null;

            string sql = sqlSelect + @" where KOSTL = @KOSTL
                            and a.IdSociedadCentro = @IdSociedadCentro
                            and a.IdNivel = @IdNivel
                            and a.IdUsuario = @IdUsuario";

            _sqlComando = new SqlCommand(sql, _sqlConn);

            if (_sqlTran != null)
                _sqlComando.Transaction = _sqlTran;

            _sqlComando.Parameters.Add(new SqlParameter("@KOSTL", (object)CentroCosto ?? DBNull.Value));
            _sqlComando.Parameters.Add(new SqlParameter("@IdSociedadCentro", (object)IdSociedadCentro ?? DBNull.Value));
            _sqlComando.Parameters.Add(new SqlParameter("@IdNivel", (object)IdNivel ?? DBNull.Value));
            _sqlComando.Parameters.Add(new SqlParameter("@IdUsuario", (object)IdUsuario ?? DBNull.Value));

            _aprobadorCentroDto = CargarReader(_sqlComando.ExecuteReader());

            return (_aprobadorCentroDto.Count > 0) ? _aprobadorCentroDto[0] : null;
        }

        public Int32 DarBajaAprobadorCentroEncabezado(Int32 idAprobadorEncabezado, string usuario)
        {
            SqlCommand _sqlComando = null;
            string sql = @"Update AprobadorCentro set Alta = 0, UsuarioModificacion = @UsuarioModificacion, FechaModificacion = getdate()
                            where IdAprobacionEncabezado = @IdAprobacionEncabezado";

            _sqlComando = new SqlCommand(sql, _sqlConn);
            _sqlComando.CommandType = CommandType.Text;

            if (_sqlTran != null)
                _sqlComando.Transaction = _sqlTran;

            _sqlComando.Parameters.Add(new SqlParameter("@UsuarioModificacion", (object)usuario ?? DBNull.Value));
            _sqlComando.Parameters.Add(new SqlParameter("@IdAprobacionEncabezado", (object)idAprobadorEncabezado ?? DBNull.Value));

            return Convert.ToInt16(_sqlComando.ExecuteNonQuery());
        }

        public string BuscarCecos(string ceco, string sociedad)
        {
            SqlCommand sqlComando;
            string sql = @"SELECT STUFF((
                            SELECT distinct CAST(',' AS VARCHAR(MAX)) + Item FROM dbo.Split(@cecos, ',')
                            WHERE Item NOT IN (
                                                SELECT a.KOSTL
                                                FROM sap.CuentaPorCentroCostoTMP a
                                                INNER JOIN sap.CentroCostoTMP b on b.KOSTL  = a.KOSTL
								                                                and b.KOKRS = a.KOKRS
                                                AND b.BUKRS = @BUKRS
							                    AND b.KOSTL  IN (SELECT Item FROM dbo.Split(@cecos, ',')))
                            FOR XML PATH('')), 1, 1, '')";

            sqlComando = new SqlCommand(sql, _sqlConn);
            sqlComando.CommandType = CommandType.Text;

            if (_sqlTran != null)
                sqlComando.Transaction = _sqlTran;

            sqlComando.Parameters.Add(new SqlParameter("@cecos", (object)ceco ?? DBNull.Value));
            sqlComando.Parameters.Add(new SqlParameter("@BUKRS", (object)sociedad ?? DBNull.Value));
            return Convert.ToString(sqlComando.ExecuteScalar());
        }

        public string BuscarOrden(string orden, string sociedad)
        {
            SqlCommand sqlComando;
            string sql = @"SELECT STUFF((
                            SELECT distinct CAST(',' AS VARCHAR(MAX)) + Item FROM dbo.Split(@Orden, ',')
                            WHERE Item NOT IN (
                                                SELECT b.AUFNR
                                                FROM [sap].[CuentaPorOrdenCOTMP] a
                                                INNER JOIN [sap].[OrdenCOTMP] b on b.AUART  = a.AUART
								                                                and b.BUKRS = a.BUKRS
                                                AND b.BUKRS = @BUKRS
							                    AND b.AUFNR  IN (SELECT Item FROM dbo.Split(@Orden, ',')))
                            FOR XML PATH('')), 1, 1, '')";

            sqlComando = new SqlCommand(sql, _sqlConn);
            sqlComando.CommandType = CommandType.Text;

            if (_sqlTran != null)
                sqlComando.Transaction = _sqlTran;

            sqlComando.Parameters.Add(new SqlParameter("@Orden", (object)orden ?? DBNull.Value));
            sqlComando.Parameters.Add(new SqlParameter("@BUKRS", (object)sociedad ?? DBNull.Value));
            return Convert.ToString(sqlComando.ExecuteScalar());
        }
    }
}
