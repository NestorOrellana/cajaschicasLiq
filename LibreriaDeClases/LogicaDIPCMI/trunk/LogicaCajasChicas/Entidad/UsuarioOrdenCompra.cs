using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using DipCmiGT.LogicaComun;

namespace DipCmiGT.LogicaCajasChicas.Entidad
{
    public class UsuarioOrdenCompra
    {
        #region Declaraciones

        private SqlConnection _sqlConn = null;
        private SqlTransaction _sqlTran = null;

        protected string sqlSelect = @" select a.IdUsuarioOrdenCompra, a.Usuario, a.OrdenCompra, a.IdSociedadCentro, a.Alta, a.UsuarioCreacion, a.FechaCreacion,
	                                        a.UsuarioModificacion, a.FechaModificacion, c.CodigoSociedad, c.Nombre, d.IdCentro, d.Nombre, KTEXT, (a.OrdenCompra + ' - ' + KTEXT) OrdenCostoDescripcion
                                        from UsuarioOrdenCompra a
                                        inner join SociedadCentro b on b.IdSociedadCentro = a.IdSociedadCentro and b.Alta = 1
                                        inner join Sociedad c on c.CodigoSociedad = b.CodigoSociedad and c.Alta = 1
                                        inner join Centro d on d.IdCentro = b.IdCentro and d.Alta = 1
                                        inner join sap.OrdenCOTMP e on e.AUFNR = a.OrdenCompra ";

        protected string sqlInsert = @" INSERT INTO UsuarioOrdenCompra (Usuario, OrdenCompra, IdSociedadCentro, UsuarioCreacion)
					                                            VALUES (@Usuario, (RIGHT('0000' + Ltrim(Rtrim(@OrdenCompra)),12)), 
                                                                        @IdSociedadCentro, @UsuarioCreacion)

                                        SELECT @@IDENTITY ";

        #endregion

        #region Constructores

        public UsuarioOrdenCompra(SqlConnection sqlConn)
        {
            _sqlConn = sqlConn;
        }

        public UsuarioOrdenCompra(SqlConnection sqlConn, SqlTransaction sqlTran)
        {
            _sqlConn = sqlConn;
            _sqlTran = sqlTran;
        }

        #endregion

        #region Metodos

        public UsuarioOrdenCostoDTO EjecutarSentenciaSelect(Int32 idUsuarioOrdenCompra)
        {
            SqlCommand _sqlComando = null;
            List<UsuarioOrdenCostoDTO> _listausuarioordencompraDTO = null; ;
            string sql = sqlSelect + " Where idUsuarioOrdenCompra = @IdUsuarioOrdenCompra";

            _sqlComando = new SqlCommand(sql, _sqlConn);
            _sqlComando.CommandType = CommandType.Text;

            if (_sqlTran != null)
                _sqlComando.Transaction = _sqlTran;

            _sqlComando.Parameters.Add(new SqlParameter("@IdUsuarioOrdenCompra", (object)idUsuarioOrdenCompra ?? DBNull.Value));

            _listausuarioordencompraDTO = CargarReader(_sqlComando.ExecuteReader());

            return (_listausuarioordencompraDTO.Count > 0) ? _listausuarioordencompraDTO[0] : null;
        }

        public List<UsuarioOrdenCostoDTO> ListaUsuarioOrdenCompra(string usuario)
        {
            SqlCommand _sqlComando = null;
            string sql = sqlSelect + " WHERE a.Usuario = @Usuario ";

            _sqlComando = new SqlCommand(sql, _sqlConn);
            _sqlComando.CommandType = CommandType.Text;

            if (_sqlTran != null)
                _sqlComando.Transaction = _sqlTran;

            _sqlComando.Parameters.Add(new SqlParameter("@Usuario", (object)usuario ?? DBNull.Value));

            return CargarReader(_sqlComando.ExecuteReader());
        }

        public List<UsuarioOrdenCostoDTO> ListaOrdenCosto(string usuario, Int32 idSociedadCentro)
        {
            SqlCommand _sqlComando = null;
            string sql = sqlSelect + @" WHERE A.Usuario = @Usuario AND a.IdSociedadCentro = @IdSociedadCentro and a.alta = 1 ";

            _sqlComando = new SqlCommand(sql, _sqlConn);
            _sqlComando.CommandType = CommandType.Text;

            if (_sqlTran != null)
                _sqlComando.Transaction = _sqlTran;

            _sqlComando.Parameters.Add(new SqlParameter("@Usuario", (object)usuario ?? DBNull.Value));
            _sqlComando.Parameters.Add(new SqlParameter("@IdSociedadCentro", (object)idSociedadCentro ?? DBNull.Value));

            return CargarReader(_sqlComando.ExecuteReader());
        }

        public List<UsuarioOrdenCostoDTO> ListaOrdenCompra()
        {
            SqlCommand _sqlComando = null;
            string sql = sqlSelect + "";

            _sqlComando = new SqlCommand(sql, _sqlConn);
            _sqlComando.CommandType = CommandType.Text;

            if (_sqlTran != null)
                _sqlComando.Transaction = _sqlTran;

            return CargarReader(_sqlComando.ExecuteReader());
        }

        public Int32 EjecutarSentenciaInsert(UsuarioOrdenCostoDTO usuarioordencompraDTO)
        {
            SqlCommand sqlComando = new SqlCommand(sqlInsert, _sqlConn);
            sqlComando.CommandType = CommandType.Text;

            if (_sqlTran != null)
                sqlComando.Transaction = _sqlTran;

            sqlComando.Parameters.Add(new SqlParameter("@Usuario", (object)usuarioordencompraDTO.USUARIO.ToUpper() ?? DBNull.Value));
            sqlComando.Parameters.Add(new SqlParameter("@OrdenCompra", (object)usuarioordencompraDTO.ORDEN_COSTO ?? DBNull.Value));
            sqlComando.Parameters.Add(new SqlParameter("@IdSociedadCentro", (object)usuarioordencompraDTO.ID_SOCIEDAD_CENTRO));
            sqlComando.Parameters.Add(new SqlParameter("@UsuarioCreacion", (object)usuarioordencompraDTO.USUARIO_MANTENIMIENTO.USUARIO_ALTA.ToUpper() ?? DBNull.Value));

            return Convert.ToInt32(sqlComando.ExecuteScalar());
        }

        public Int16 DarBajaOrdenCompra(Int32 idUsuarioOrdenCompra, string usuario)
        {
            SqlCommand _sqlComando = null;
            string sql = @"Update UsuarioOrdenCompra set Alta = 0, UsuarioModificacion = @UsuarioModificacion, FechaModificacion = getdate()
                            where idUsuarioOrdenCompra = @idUsuarioOrdenCompra";

            _sqlComando = new SqlCommand(sql, _sqlConn);
            _sqlComando.CommandType = CommandType.Text;

            if (_sqlTran != null)
                _sqlComando.Transaction = _sqlTran;

            _sqlComando.Parameters.Add(new SqlParameter("@UsuarioModificacion", (object)usuario.ToUpper() ?? DBNull.Value));
            _sqlComando.Parameters.Add(new SqlParameter("@idUsuarioOrdenCompra", (object)idUsuarioOrdenCompra ?? DBNull.Value));

            return Convert.ToInt16(_sqlComando.ExecuteNonQuery());
        }

        public Int16 DarAltaOrdenCompra(Int32 idUsuarioOrdenCompra, string usuario)
        {
            SqlCommand _sqlComando = null;
            string sql = @"Update UsuarioOrdenCompra set Alta = 1, UsuarioModificacion = @UsuarioModificacion, FechaModificacion = getdate()
                            where IdUsuarioOrdenCompra = @IdUsuarioOrdenCompra";

            _sqlComando = new SqlCommand(sql, _sqlConn);
            _sqlComando.CommandType = CommandType.Text;

            if (_sqlTran != null)
                _sqlComando.Transaction = _sqlTran;

            _sqlComando.Parameters.Add(new SqlParameter("@UsuarioModificacion", (object)usuario.ToUpper() ?? DBNull.Value));
            _sqlComando.Parameters.Add(new SqlParameter("@idUsuarioOrdenCompra", (object)idUsuarioOrdenCompra ?? DBNull.Value));

            return Convert.ToInt16(_sqlComando.ExecuteNonQuery());
        }

        public List<UsuarioOrdenCostoDTO> CargarReader(SqlDataReader sqlReader)
        {
            UsuarioOrdenCostoDTO _usuarioOrdenCompraDto = null;
            List<UsuarioOrdenCostoDTO> _listaUsuarioOrdenCompraDto = new List<UsuarioOrdenCostoDTO>();

            try
            {

                while (sqlReader.Read())
                {
                    _usuarioOrdenCompraDto = new UsuarioOrdenCostoDTO();

                    _usuarioOrdenCompraDto.ID_USUARIO_ORDEN_COMPRA = sqlReader.GetInt32(0);
                    _usuarioOrdenCompraDto.USUARIO = sqlReader.GetString(1);
                    _usuarioOrdenCompraDto.ORDEN_COSTO = sqlReader.GetString(2);
                    _usuarioOrdenCompraDto.ID_SOCIEDAD_CENTRO = sqlReader.GetInt32(3);
                    _usuarioOrdenCompraDto.ALTA = sqlReader.GetBoolean(4);
                    _usuarioOrdenCompraDto.USUARIO_MANTENIMIENTO.USUARIO_ALTA = sqlReader.GetString(5);
                    _usuarioOrdenCompraDto.USUARIO_MANTENIMIENTO.FECHA_ALTA = sqlReader.GetDateTime(6);
                    _usuarioOrdenCompraDto.USUARIO_MANTENIMIENTO.USUARIO_MODIFICO = sqlReader.IsDBNull(7) ? null : sqlReader.GetString(7);
                    _usuarioOrdenCompraDto.USUARIO_MANTENIMIENTO.FECHA_MODIFICACION = sqlReader.IsDBNull(8) ? (DateTime?)null : sqlReader.GetDateTime(8);
                    _usuarioOrdenCompraDto.CODIGO_SOCIEDAD = sqlReader.GetString(9);
                    _usuarioOrdenCompraDto.NOMBRE_SOCIEDAD = sqlReader.GetString(10);
                    _usuarioOrdenCompraDto.ID_CENTRO = sqlReader.GetInt32(11);
                    _usuarioOrdenCompraDto.NOMBRE_CENTRO = sqlReader.GetString(12);
                    _usuarioOrdenCompraDto.KTEXT = sqlReader.GetString(13);
                    _usuarioOrdenCompraDto.ORDEN_COSTO_DESCRIPCION = sqlReader.GetString(14);

                    _listaUsuarioOrdenCompraDto.Add(_usuarioOrdenCompraDto);
                }
            }
            finally
            {
                if (sqlReader != null) sqlReader.Close();
            }
            return _listaUsuarioOrdenCompraDto;
        }

        public List<LlenarDDL_DTO> ListarSociedadOrdenCosto(string usuario)
        {
            SqlCommand _sqlComando = null;
            string sql = @"select distinct c.CodigoSociedad, c.CodigoSociedad +' :: '+ c.Nombre
                            from UsuarioOrdenCompra a
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

        public List<LlenarDDL_DTO> ListarCentroOrdenCosto(string usuario, string codigoSociedad)
        {
            SqlCommand _sqlComando = null;
            string sql = @"select CAST(a.IdSociedadCentro as varchar(3)), cast(d.IdCentro as varchar(3)) +' :: '+ d.Nombre
                            from UsuarioOrdenCompra  a
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

        public bool ExisteUsuarioOrdenCosto(string usuario, string ordenCosto, Int32 idSociedadCentro)
        {
            SqlCommand _sqlComando = null;
            string sql = @" select COUNT(a.IdUsuarioOrdenCompra)
                            from UsuarioOrdenCompra a
                            inner join SociedadCentro b on b.IdSociedadCentro = a.IdSociedadCentro and b.Alta = 1
                            inner join Sociedad c on c.CodigoSociedad = b.CodigoSociedad and c.Alta = 1
                            inner join Centro d on d.IdCentro = b.IdCentro and d.Alta = 1 
                            where a.Usuario = @Usuario
                            and a.OrdenCompra = @CentroCosto
                            and a.IdSociedadCentro = @IdSociedadCentro ";

            _sqlComando = new SqlCommand(sql, _sqlConn);
            _sqlComando.CommandType = CommandType.Text;

            if (_sqlTran != null)
                _sqlComando.Transaction = _sqlTran;

            _sqlComando.Parameters.Add(new SqlParameter("@Usuario", (object)usuario ?? DBNull.Value));
            _sqlComando.Parameters.Add(new SqlParameter("@CentroCosto", (object)ordenCosto ?? DBNull.Value));
            _sqlComando.Parameters.Add(new SqlParameter("@IdSociedadCentro", (object)idSociedadCentro ?? DBNull.Value));

            return Convert.ToInt32(_sqlComando.ExecuteScalar()) == 0 ? false : true;
        }

        public UsuarioOrdenCostoDTO BuscarUsuarioOrdenCosto(string usuario, string ordenCosto, Int32 idSociedadCentro)
        {
            SqlCommand _sqlComando = null;
            List<UsuarioOrdenCostoDTO> _listaUsuarioOrdenCostoDTO = null;

            string sql = sqlSelect + @" where a.Usuario = @Usuario
                            and a.OrdenCompra = RIGHT('0000' + Ltrim(Rtrim(@CentroCosto)),12)" +//@CentroCosto
                            "and a.IdSociedadCentro = @IdSociedadCentro ";

            _sqlComando = new SqlCommand(sql, _sqlConn);
            _sqlComando.CommandType = CommandType.Text;

            if (_sqlTran != null)
                _sqlComando.Transaction = _sqlTran;

            _sqlComando.Parameters.Add(new SqlParameter("@Usuario", (object)usuario ?? DBNull.Value));
            _sqlComando.Parameters.Add(new SqlParameter("@CentroCosto", (object)ordenCosto ?? DBNull.Value));
            _sqlComando.Parameters.Add(new SqlParameter("@IdSociedadCentro", (object)idSociedadCentro ?? DBNull.Value));

            _listaUsuarioOrdenCostoDTO = CargarReader(_sqlComando.ExecuteReader());

            return (_listaUsuarioOrdenCostoDTO.Count > 0) ? _listaUsuarioOrdenCostoDTO[0] : null;
        }

        public List<LlenarDDL_DTO> ListarOrdenCosto(string codigoSociedad)
        {
            List<LlenarDDL_DTO> _listaLlenarDDL = new List<LlenarDDL_DTO>();
            LlenarDDL_DTO _llenarDDL = null;
            SqlCommand _sqlComando = null;
            string sql = @" select AUFNR, AUFNR +' - '+ KTEXT
                            from sap.OrdenCOTMP a
                            inner join sap.CuentaPorOrdenCOTMP b on a.AUART = b.AUART
                            and b.BUKRS = @BUKRS ";



            _sqlComando = new SqlCommand(sql, _sqlConn);
            _sqlComando.CommandType = CommandType.Text;

            if (_sqlTran != null)
                _sqlComando.Transaction = _sqlTran;

            _sqlComando.Parameters.Add(new SqlParameter("@BUKRS", (object)codigoSociedad ?? DBNull.Value));

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


        public List<LlenarDDL_DTO> ListarOrdenCostoUsuario(string usuario, string codigoSociedad)
        {
            List<LlenarDDL_DTO> _listaLlenarDDL = new List<LlenarDDL_DTO>();
            LlenarDDL_DTO _llenarDDL = null;
            SqlCommand _sqlComando = null;
            string sql = @"select distinct(a.OrdenCompra), a.OrdenCompra + ' - ' + b.KTEXT
                            from UsuarioOrdenCompra a
                            inner join sap.OrdenCOTMP b on b.AUFNR = a.OrdenCompra and b.BUKRS = @BUKRS
                            inner join sap.CuentaPorOrdenCOTMP c on c.AUART = b.AUART
                            where a.Usuario = @Usuario";



            _sqlComando = new SqlCommand(sql, _sqlConn);
            _sqlComando.CommandType = CommandType.Text;

            if (_sqlTran != null)
                _sqlComando.Transaction = _sqlTran;

            _sqlComando.Parameters.Add(new SqlParameter("@Usuario", (object)usuario ?? DBNull.Value));
            _sqlComando.Parameters.Add(new SqlParameter("@BUKRS", (object)codigoSociedad ?? DBNull.Value));

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

        #endregion

    }
}
