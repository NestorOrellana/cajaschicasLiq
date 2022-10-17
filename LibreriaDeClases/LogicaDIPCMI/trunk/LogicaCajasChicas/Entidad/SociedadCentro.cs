using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using DipCmiGT.LogicaCajasChicas;
using System.Data;
using DipCmiGT.LogicaComun;

namespace LogicaCajasChicas.Entidad
{
    public class SociedadCentro
    {
        #region Declaraciones

        private SqlConnection _sqlConn = null;
        private SqlTransaction _sqlTran = null;

        protected string sqlSelect = @"SELECT distinct a.IdSociedadCentro, a.CodigoSociedad, c.Nombre, a.IdCentro, b.Nombre, 
                                        a.Alta, a.UsuarioAlta, a.FechaAlta,
                                        a.UsuarioModificacion, a.FechaModificacion 
                                        FROM SociedadCentro a
                                        inner join Centro b on b.IdCentro = a.IdCentro and b.Alta = 1
                                        inner join sociedad c on c.codigosociedad = a.codigosociedad and c.alta = 1";

        protected string sqlInsert = @"INSERT INTO SociedadCentro (CodigoSociedad, IdCentro, UsuarioAlta)
                                                      VALUES (@CodigoSociedad, @IdCentro, @UsuarioAlta) ";

        protected string sqlUpdate = @" ";

        #endregion

        #region Constructores

        public SociedadCentro(SqlConnection sqlConn)
        {
            _sqlConn = sqlConn;
        }

        public SociedadCentro(SqlConnection sqlConn, SqlTransaction sqlTran)
        {
            _sqlConn = sqlConn;
            _sqlTran = sqlTran;
        }

        #endregion

        #region Metodos

        public List<SociedadCentroDTO> ListaSociedadCentro()
        {
            SqlCommand sqlComando;
            string sql = sqlSelect;

            sqlComando = new SqlCommand(sql, _sqlConn);

            if (_sqlTran != null)
                sqlComando.Transaction = _sqlTran;

            return CargarReader(sqlComando.ExecuteReader());
        }

        public List<SociedadCentroDTO> EjecutarSentenciaSelect()
        {
            SqlCommand sqlComando;
            string sql = sqlSelect + "";

            sqlComando = new SqlCommand(sql, _sqlConn);
            sqlComando.CommandType = CommandType.Text;

            if (_sqlTran != null)
                sqlComando.Transaction = _sqlTran;

            return CargarReader(sqlComando.ExecuteReader());
        }

        public List<SociedadCentroDTO> EjecutarSentenciaSelect(string CodSociedad)
        {
            SqlCommand sqlComando;
            string sql = sqlSelect + @" WHERE a.alta = 1 and c.CodigoSociedad = @CodigoSociedad 
                                        order by b.nombre ";

            sqlComando = new SqlCommand(sql, _sqlConn);
            sqlComando.CommandType = CommandType.Text;

            if (_sqlTran != null)
                sqlComando.Transaction = _sqlTran;

            sqlComando.Parameters.Add(new SqlParameter("@CodigoSociedad", (object)CodSociedad ?? DBNull.Value));

            return CargarReader(sqlComando.ExecuteReader());
        }

        public Int32 EjecutarSentenciaInsert(SociedadCentroDTO sociedadCentroDTO)
        {
            SqlCommand sqlComando;

            sqlComando = new SqlCommand(sqlInsert, _sqlConn);
            sqlComando.CommandType = CommandType.Text;

            if (_sqlTran != null)
                sqlComando.Transaction = _sqlTran;

            sqlComando.Parameters.Add(new SqlParameter("@CodigoSociedad", (object)sociedadCentroDTO.CODIGO_SOCIEDAD ?? DBNull.Value));
            sqlComando.Parameters.Add(new SqlParameter("@IdCentro", (object)sociedadCentroDTO.ID_CENTRO ?? DBNull.Value));
            sqlComando.Parameters.Add(new SqlParameter("@UsuarioAlta", (object)sociedadCentroDTO.USUARIO_MANTENIMIENTO.USUARIO_ALTA.ToUpper() ?? DBNull.Value));

            return Convert.ToInt32(sqlComando.ExecuteScalar());

        }

        public Int32 EjectuarSentenciaUpdate(SociedadCentroDTO sociedadCentroDTO)
        {
            SqlCommand sqlComando;

            sqlComando = new SqlCommand(sqlUpdate, _sqlConn);

            if (_sqlTran != null)
                sqlComando.Transaction = _sqlTran;

            return Convert.ToInt32(sqlComando.ExecuteScalar());
        }

        protected List<SociedadCentroDTO> CargarReader(SqlDataReader sqlReader)
        {
            SociedadCentroDTO _sociedadCentroDto = null;
            List<SociedadCentroDTO> _listaSociedadCentro = new List<SociedadCentroDTO>();

            while (sqlReader.Read())
            {
                _sociedadCentroDto = new SociedadCentroDTO();

                _sociedadCentroDto.ID_SOCIEDAD_CENTRO = sqlReader.GetInt32(0);
                _sociedadCentroDto.CODIGO_SOCIEDAD = sqlReader.GetString(1);
                _sociedadCentroDto.NOMBRE_SOCIEDAD = sqlReader.GetString(2);
                _sociedadCentroDto.ID_CENTRO = sqlReader.GetInt32(3);
                _sociedadCentroDto.NOMBRE_CENTRO = sqlReader.GetString(4);
                _sociedadCentroDto.ALTA = sqlReader.GetBoolean(5);
                _sociedadCentroDto.USUARIO_MANTENIMIENTO.USUARIO_ALTA = sqlReader.GetString(6);
                _sociedadCentroDto.USUARIO_MANTENIMIENTO.FECHA_ALTA = sqlReader.GetDateTime(7);
                _sociedadCentroDto.USUARIO_MANTENIMIENTO.USUARIO_MODIFICO = sqlReader.IsDBNull(8) ? null : sqlReader.GetString(8);
                _sociedadCentroDto.USUARIO_MANTENIMIENTO.FECHA_MODIFICACION = sqlReader.IsDBNull(9) ? (DateTime?)null : sqlReader.GetDateTime(9);

                _listaSociedadCentro.Add(_sociedadCentroDto);
            }

            sqlReader.Close();
            return _listaSociedadCentro;
        }

        public bool ExisteSociedad(string codigoSociedad)
        {
            SqlCommand sqlComando;
            string sql = @"select COUNT(CodigoSociedad)
                            from Sociedad
                            where CodigoSociedad = @CodigoSociedad";

            sqlComando = new SqlCommand(sql, _sqlConn);

            if (_sqlTran != null)
                sqlComando.Transaction = _sqlTran;
            sqlComando.Parameters.Add(new SqlParameter("@CodigoSociedad", (object)codigoSociedad ?? DBNull.Value));
            return Convert.ToInt32(sqlComando.ExecuteScalar()) > 0 ? true : false;
        }

        public List<SociedadCentroDTO> BuscarMapeosUsuario(string usuario)
        {
            SqlCommand _sqlcomando = null;
            string sql = sqlSelect + @" where d.Usuario = @Usuario ";

            _sqlcomando = new SqlCommand(sql, _sqlConn);

            if (_sqlTran != null)
                _sqlcomando.Transaction = _sqlTran;

            _sqlcomando.Parameters.Add(new SqlParameter("@Usuario", (object)usuario ?? DBNull.Value));

            return CargarReader(_sqlcomando.ExecuteReader());
        }

        public List<LlenarDDL_DTO> ListarSociedades()
        {
            List<LlenarDDL_DTO> _listaLlenarDDL = new List<LlenarDDL_DTO>();
            SqlCommand _sqlComando = null;
            string sql = @" select distinct C.CodigoSociedad, C.CodigoSociedad + ' :: ' +C.Nombre
                            from UsuarioSociedadCentro a
                            inner join SociedadCentro b on b.IdSociedadCentro = a.IdSociedadCentro and b.Alta = 1
                            inner join Sociedad c on c.CodigoSociedad = b.CodigoSociedad and c.Alta = 1
                            inner join Centro d on d.IdCentro = b.IdCentro and d.Alta = 1 ";

            _sqlComando = new SqlCommand(sql, _sqlConn);
            _sqlComando.CommandType = CommandType.Text;

            if (_sqlTran != null)
                _sqlComando.Transaction = _sqlTran;


            return CargarDDL(_sqlComando.ExecuteReader());
        }

        public List<LlenarDDL_DTO> ListarSociedades(string usuario)
        {
            List<LlenarDDL_DTO> _listaLlenarDDL = new List<LlenarDDL_DTO>();
            SqlCommand _sqlComando = null;
            string sql = @" select distinct C.CodigoSociedad, C.CodigoSociedad + ' :: ' +C.Nombre
                            from UsuarioSociedadCentro a
                            inner join SociedadCentro b on b.IdSociedadCentro = a.IdSociedadCentro and b.Alta = 1
                            inner join Sociedad c on c.CodigoSociedad = b.CodigoSociedad and c.Alta = 1
                            inner join Centro d on d.IdCentro = b.IdCentro and d.Alta = 1 
                            inner join usuario e on e.usuario = a.usuario and e.alta = 1
                            where e.usuario = @usuario
                            and a.alta = 1 ";

            _sqlComando = new SqlCommand(sql, _sqlConn);
            _sqlComando.CommandType = CommandType.Text;

            _sqlComando.Parameters.Add(new SqlParameter("@usuario", (object)usuario ?? DBNull.Value));

            if (_sqlTran != null)
                _sqlComando.Transaction = _sqlTran;


            return CargarDDL(_sqlComando.ExecuteReader());
        }

        public List<LlenarDDL_DTO> ListarCentrosMapeados(string codigoSociedad, string usuario)
        {
            List<LlenarDDL_DTO> _listaLlenarDDL = new List<LlenarDDL_DTO>();
            LlenarDDL_DTO _llenarDDL = null;

            SqlCommand _sqlcomando = null;
            string sql = @" SELECT distinct a.IdSociedadCentro, b.Nombre NombreCentro
                            FROM SociedadCentro a
                            inner join Centro b on b.IdCentro = a.IdCentro and b.Alta = 1
                            inner join sociedad c on c.codigosociedad = a.codigosociedad and c.alta = 1
                            inner join usuario d on d.usuario = d.usuario and a.alta = 1
                            where c.CodigoSociedad = @CodigoSociedad
                            and d.usuario = @usuario ";

            _sqlcomando = new SqlCommand(sql, _sqlConn);

            if (_sqlTran != null)
                _sqlcomando.Transaction = _sqlTran;

            _sqlcomando.Parameters.Add(new SqlParameter("@CodigoSociedad", (object)codigoSociedad ?? DBNull.Value));
            _sqlcomando.Parameters.Add(new SqlParameter("@usuario", (object)usuario ?? DBNull.Value));

            SqlDataReader _sqlDataR = _sqlcomando.ExecuteReader();

            while (_sqlDataR.Read())
            {
                _llenarDDL = new LlenarDDL_DTO();

                _llenarDDL.IDENTIFICADOR = _sqlDataR.GetInt32(0).ToString();
                _llenarDDL.DESCRIPCION = _sqlDataR.GetString(1);

                _listaLlenarDDL.Add(_llenarDDL);
            }

            return _listaLlenarDDL;
        }

        public List<LlenarDDL_DTO> ListarSociedadCentrosMapeados(string codigoSociedad, string usuario)
        {
            List<LlenarDDL_DTO> _listaLlenarDDL = new List<LlenarDDL_DTO>();
            LlenarDDL_DTO _llenarDDL = null;

            SqlCommand _sqlcomando = null;
            string sql = @" select  a.IdSociedadCentro, d.Nombre NombreCentro
                            from UsuarioSociedadCentro a
                            inner join SociedadCentro b on b.IdSociedadCentro = a.IdSociedadCentro and b.Alta = 1
                            inner join Sociedad c on c.CodigoSociedad = b.CodigoSociedad and c.Alta = 1
                            inner join Centro d on d.IdCentro = b.IdCentro and d.Alta = 1
                            inner join Usuario e on e.Usuario = a.Usuario and e.Alta = 1
                            where c.CodigoSociedad = @CodigoSociedad
                            and e.Usuario = @usuario
                            and a.Alta = 1 ";

            _sqlcomando = new SqlCommand(sql, _sqlConn);

            if (_sqlTran != null)
                _sqlcomando.Transaction = _sqlTran;

            _sqlcomando.Parameters.Add(new SqlParameter("@CodigoSociedad", (object)codigoSociedad ?? DBNull.Value));
            _sqlcomando.Parameters.Add(new SqlParameter("@usuario", (object)usuario ?? DBNull.Value));

            SqlDataReader _sqlDataR = _sqlcomando.ExecuteReader();

            while (_sqlDataR.Read())
            {
                _llenarDDL = new LlenarDDL_DTO();

                _llenarDDL.IDENTIFICADOR = _sqlDataR.GetInt32(0).ToString();
                _llenarDDL.DESCRIPCION = _sqlDataR.GetString(1);

                _listaLlenarDDL.Add(_llenarDDL);
            }

            return _listaLlenarDDL;
        }

        public List<LlenarDDL_DTO> ListarCentrosMapeados(string codigoSociedad)
        {
            List<LlenarDDL_DTO> _listaLlenarDDL = new List<LlenarDDL_DTO>();
            LlenarDDL_DTO _llenarDDL = null;

            SqlCommand _sqlcomando = null;
            string sql = @" SELECT distinct a.IdSociedadCentro, b.Nombre NombreCentro
                            FROM SociedadCentro a
                            inner join Centro b on b.IdCentro = a.IdCentro and b.Alta = 1
                            inner join sociedad c on c.codigosociedad = a.codigosociedad and c.alta = 1
                            where c.CodigoSociedad = @CodigoSociedad";

            _sqlcomando = new SqlCommand(sql, _sqlConn);

            if (_sqlTran != null)
                _sqlcomando.Transaction = _sqlTran;

            _sqlcomando.Parameters.Add(new SqlParameter("@CodigoSociedad", (object)codigoSociedad ?? DBNull.Value));

            SqlDataReader _sqlDataR = _sqlcomando.ExecuteReader();

            while (_sqlDataR.Read())
            {
                _llenarDDL = new LlenarDDL_DTO();

                _llenarDDL.IDENTIFICADOR = _sqlDataR.GetInt32(0).ToString();
                _llenarDDL.DESCRIPCION = _sqlDataR.GetString(1);

                _listaLlenarDDL.Add(_llenarDDL);
            }

            return _listaLlenarDDL;
        }

        public Int16 DarBajaSociedadCentro(Int32 IdSociedadCentro, string usuario)
        {
            SqlCommand sqlComando;
            string sql = @"Update SociedadCentro set Alta = 0, UsuarioModificacion = @UsuarioModificacion, FechaModificacion = getdate()
                            where IdSociedadCentro = @IdSociedadCentro";

            sqlComando = new SqlCommand(sql, _sqlConn);
            sqlComando.CommandType = CommandType.Text;

            if (_sqlTran != null)
                sqlComando.Transaction = _sqlTran;

            sqlComando.Parameters.Add(new SqlParameter("@IdSociedadCentro", (object)IdSociedadCentro ?? DBNull.Value));
            sqlComando.Parameters.Add(new SqlParameter("@UsuarioModificacion", (object)usuario.ToUpper() ?? DBNull.Value));

            return Convert.ToInt16(sqlComando.ExecuteNonQuery());
        }

        public Int16 DarAltaSociedadCentro(Int32 IdSociedadCentro, string usuario)
        {
            SqlCommand sqlComando;
            string sql = @"Update SociedadCentro set Alta = 1, UsuarioModificacion = @UsuarioModificacion, FechaModificacion = getdate()
                            where IdSociedadCentro = @IdSociedadCentro";

            sqlComando = new SqlCommand(sql, _sqlConn);
            sqlComando.CommandType = CommandType.Text;

            if (_sqlTran != null)
                sqlComando.Transaction = _sqlTran;

            sqlComando.Parameters.Add(new SqlParameter("@IdSociedadCentro", (object)IdSociedadCentro ?? DBNull.Value));
            sqlComando.Parameters.Add(new SqlParameter("@UsuarioModificacion", (object)usuario.ToUpper() ?? DBNull.Value));

            return Convert.ToInt16(sqlComando.ExecuteNonQuery());

        }

        public bool ExisteSociedadCentro(string codigoSociedad, Int32 idCentro)
        {
            SqlCommand sqlComando;
            string sql = @"select COUNT(IdSociedadCentro) from SociedadCentro
                           WHERE CodigoSociedad = @CodigoSociedad and IdCentro = @IdCentro";

            sqlComando = new SqlCommand(sql, _sqlConn);

            if (_sqlTran != null)
                sqlComando.Transaction = _sqlTran;
            sqlComando.Parameters.Add(new SqlParameter("@CodigoSociedad", (object)codigoSociedad ?? DBNull.Value));
            sqlComando.Parameters.Add(new SqlParameter("@IdCentro", (object)idCentro ?? DBNull.Value));
            return Convert.ToInt32(sqlComando.ExecuteScalar()) > 0 ? true : false;
        }

        public List<LlenarDDL_DTO> ListarCentroMapeado(string codigoSociedad)
        {
            SqlCommand _sqlComando = null;
            string sql = @" select distinct CAST(b.IdSociedadCentro as varchar(10)), cast(d.IdCentro as varchar(10)) +' :: '+ d.Nombre
                            from SociedadCentro b 
                            inner join Sociedad c on c.CodigoSociedad = b.CodigoSociedad and c.Alta = 1
                            inner join Centro d on d.IdCentro = b.IdCentro and d.Alta = 1
                            where b.Alta = 1 
                            and c.CodigoSociedad = @CodigoSociedad  ";

            _sqlComando = new SqlCommand(sql, _sqlConn);
            _sqlComando.CommandType = CommandType.Text;

            if (_sqlTran != null)
                _sqlComando.Transaction = _sqlTran;

            _sqlComando.Parameters.Add(new SqlParameter("@CodigoSociedad", (object)codigoSociedad ?? DBNull.Value));

            return CargarDDL(_sqlComando.ExecuteReader());
        }

        public List<LlenarDDL_DTO> ListarSociedadMapeada()
        {
            SqlCommand _sqlComando = null;
            string sql = @" select distinct c.CodigoSociedad, c.CodigoSociedad +' :: '+ c.Nombre
                            from SociedadCentro b 
                            inner join Sociedad c on c.CodigoSociedad = b.CodigoSociedad and c.Alta = 1
                            inner join Centro d on d.IdCentro = b.IdCentro and d.Alta = 1
                            where b.Alta = 1  ";

            _sqlComando = new SqlCommand(sql, _sqlConn);
            _sqlComando.CommandType = CommandType.Text;

            if (_sqlTran != null)
                _sqlComando.Transaction = _sqlTran;

            return CargarDDL(_sqlComando.ExecuteReader());
        }

        protected List<LlenarDDL_DTO> CargarDDL(SqlDataReader sqlDataReader, bool niveles = false)
        {
            List<LlenarDDL_DTO> _listaDDL = new List<LlenarDDL_DTO>();
            LlenarDDL_DTO _DDL = new LlenarDDL_DTO();

            while (sqlDataReader.Read())
            {
                _DDL = new LlenarDDL_DTO();

                if (niveles) {
                    _DDL.IDENTIFICADOR = sqlDataReader.GetInt32(0).ToString();
                }
                else {
                    _DDL.IDENTIFICADOR = sqlDataReader.GetString(0);
                }

                _DDL.DESCRIPCION = sqlDataReader.GetString(1);

                _listaDDL.Add(_DDL);
            }
            return _listaDDL;
        }

        public List<LlenarDDL_DTO> ListarSociedadesUsuario(string usuario, string dominio)
        {
            SqlCommand _sqlComando = null;
            string sql = @" select distinct c.CodigoSociedad, c.CodigoSociedad +' :: '+ c.Nombre
                            from UsuarioOrdenCompra a
                            inner join SociedadCentro b on b.IdSociedadCentro = a.IdSociedadCentro and b.Alta = 1
                            inner join Sociedad c on c.CodigoSociedad = b.CodigoSociedad and c.Alta = 1
                            inner join Centro d on d.IdCentro = b.IdCentro and d.Alta = 1 
                            inner join Usuario e on e.Usuario = a.Usuario and e.Alta = 1
       					    inner join Dominio f on f.Identificador = e.dominio
                            where a.Usuario = @usuario
                            and   f.Nombre = @dominio
                            union 
                            select distinct c.CodigoSociedad, c.CodigoSociedad +' :: '+ c.Nombre
                            from UsuarioCentroCosto a
                            inner join SociedadCentro b on b.IdSociedadCentro = a.IdSociedadCentro and b.Alta = 1
                            inner join Sociedad c on c.CodigoSociedad = b.CodigoSociedad and c.Alta = 1
                            inner join Centro d on d.IdCentro = b.IdCentro and d.Alta = 1 
                            inner join Usuario e on e.Usuario = a.Usuario and e.Alta = 1
							inner join Dominio f on f.Identificador = e.dominio
                            where a.Usuario = @usuario
                            and   f.Nombre = @dominio ";

            _sqlComando = new SqlCommand(sql, _sqlConn);
            _sqlComando.CommandType = CommandType.Text;

            if (_sqlTran != null)
                _sqlComando.Transaction = _sqlTran;

            _sqlComando.Parameters.Add(new SqlParameter("@usuario", (object)usuario ?? DBNull.Value));
            _sqlComando.Parameters.Add(new SqlParameter("@dominio", (object)dominio ?? DBNull.Value));

            return CargarDDL(_sqlComando.ExecuteReader());
        }

        public List<LlenarDDL_DTO> ListarCentrosUsuario(string usuario, string codigoSociedad)
        {
            SqlCommand _sqlComando = null;
            string sql = @" select distinct CAST(a.IdSociedadCentro as varchar(10)), cast(d.IdCentro as varchar(10)) +' :: '+ d.Nombre
                            from UsuarioOrdenCompra  a
                            inner join SociedadCentro b on b.IdSociedadCentro = a.IdSociedadCentro and b.Alta = 1
                            inner join Sociedad c on c.CodigoSociedad = b.CodigoSociedad and c.Alta = 1
                            inner join Centro d on d.IdCentro = b.IdCentro and d.Alta = 1
                            where a.Usuario = @Usuario
                            and c.CodigoSociedad = @CodigoSociedad 
                            and a.alta = 1
                            union
                            select distinct CAST(a.IdSociedadCentro as varchar(10)), cast(d.IdCentro as varchar(10)) +' :: '+ d.Nombre
                            from UsuarioCentroCosto a
                            inner join SociedadCentro b on b.IdSociedadCentro = a.IdSociedadCentro and b.Alta = 1
                            inner join Sociedad c on c.CodigoSociedad = b.CodigoSociedad and c.Alta = 1
                            inner join Centro d on d.IdCentro = b.IdCentro and d.Alta = 1
                            where a.Usuario = @Usuario
                            and c.CodigoSociedad = @CodigoSociedad 
                            and a.alta = 1 ";

            _sqlComando = new SqlCommand(sql, _sqlConn);
            _sqlComando.CommandType = CommandType.Text;

            if (_sqlTran != null)
                _sqlComando.Transaction = _sqlTran;

            _sqlComando.Parameters.Add(new SqlParameter("@usuario", (object)usuario ?? DBNull.Value));
            _sqlComando.Parameters.Add(new SqlParameter("@CodigoSociedad", (object)codigoSociedad ?? DBNull.Value));

            return CargarDDL(_sqlComando.ExecuteReader());
        }

        public List<LlenarDDL_DTO> ListarNiveles(string codigoSociedad)
        {
            SqlCommand _sqlComando = null;
            string sql = @"SELECT n.CodigoNivel, n.Nivel 
                            FROM dbo.NivelLiquidacion n
                            INNER JOIN dbo.Sociedad s
                            ON s.Pais = n.Pais
                            WHERE s.CodigoSociedad = @CodigoSociedad";

            _sqlComando = new SqlCommand(sql, _sqlConn);
            _sqlComando.CommandType = CommandType.Text;

            if (_sqlTran != null)
                _sqlComando.Transaction = _sqlTran;

            _sqlComando.Parameters.Add(new SqlParameter("@CodigoSociedad", (object)codigoSociedad ?? DBNull.Value));

            return CargarDDL(_sqlComando.ExecuteReader(), true);
        }
        #endregion
    }
}
