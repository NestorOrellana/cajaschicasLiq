using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using DipCmiGT.LogicaComun;

namespace DipCmiGT.LogicaCajasChicas.Entidad
{
    public class Sociedad
    {
        #region Declaraciones

        private SqlConnection _sqlConn = null;
        private SqlTransaction _sqlTran = null;

        protected string sqlSelect = @"SELECT CodigoSociedad, Nombre, MesesFactura, Pais, Moneda, MontoCompraCC, ToleranciaCompraCC, Alta, UsuarioAlta, FechaAlta, 
                                       UsuarioModificacion, FechaModificacion, Mandante, TiempoLiquidacion
                                       FROM Sociedad";

        protected string sqlInsert = @"INSERT INTO Sociedad (CodigoSociedad, Nombre, MesesFactura, Pais, Moneda, UsuarioAlta, MontoCompraCC, ToleranciaCompraCC, Mandante, TiempoLiquidacion)
                                                      VALUES (@CodigoSociedad, @Nombre, @MesesFactura, @Pais, @Moneda ,@UsuarioAlta, @MontoCompraCC, @ToleranciaCompraCC, @Mandante, @TiempoLiquidacion) ";

        protected string sqlUpdate = @"UPDATE Sociedad SET Nombre = @Nombre, MesesFactura = @MesesFactura, Pais = @Pais, Moneda = @Moneda, Alta = @Alta,  UsuarioModificacion = @UsuarioModificacion, FechaModificacion = getdate(),
                                                                    Mandante = @Mandante, MontoCompraCC = @MontoCompraCC, ToleranciaCompraCC = @ToleranciaCompraCC, TiempoLiquidacion = @TiempoLiquidacion
                                                            WHERE CodigoSociedad = @CodigoSociedad ";

        #endregion

        #region Constructores

        public Sociedad(SqlConnection sqlConn)
        {
            _sqlConn = sqlConn;
        }

        public Sociedad(SqlConnection sqlConn, SqlTransaction sqlTran)
        {
            _sqlConn = sqlConn;
            _sqlTran = sqlTran;
        }
        #endregion

        #region Metodos

        public SociedadDTO EjecutarSentenciaSelect(string CodSociedad)
        {
            List<SociedadDTO> listaSociedad = null;
            SqlCommand sqlComando;
            string sql = sqlSelect + " WHERE CodigoSociedad = @CodigoSociedad ";

            sqlComando = new SqlCommand(sql, _sqlConn);
            sqlComando.CommandType = CommandType.Text;

            if (_sqlTran != null)
                sqlComando.Transaction = _sqlTran;

            sqlComando.Parameters.Add(new SqlParameter("@CodigoSociedad", (object)CodSociedad ?? DBNull.Value));
            listaSociedad = CargarReader(sqlComando.ExecuteReader());
            return listaSociedad.Count > 0 ? listaSociedad[0] : null;
        }

        public Int32 EjecutarSentenciaInsert(SociedadDTO sociedadDTO)
        {
            SqlCommand sqlComando;

            sqlComando = new SqlCommand(sqlInsert, _sqlConn);
            sqlComando.CommandType = CommandType.Text;

            if (_sqlTran != null)
                sqlComando.Transaction = _sqlTran;

            sqlComando.Parameters.Add(new SqlParameter("@CodigoSociedad", (object)sociedadDTO.CODIGO_SOCIEDAD ?? DBNull.Value));
            sqlComando.Parameters.Add(new SqlParameter("@Nombre", (object)sociedadDTO.NOMBRE.ToUpper() ?? DBNull.Value));
            sqlComando.Parameters.Add(new SqlParameter("@MesesFactura", (object)sociedadDTO.MESES_FACTURA ?? DBNull.Value));
            sqlComando.Parameters.Add(new SqlParameter("@Pais", (object)sociedadDTO.PAIS.ToUpper() ?? DBNull.Value));
            sqlComando.Parameters.Add(new SqlParameter("@Moneda", (object)sociedadDTO.MONEDA.ToUpper() ?? DBNull.Value));
            sqlComando.Parameters.Add(new SqlParameter("@MontoCompraCC", (object)sociedadDTO.MONTO_COMPRA_CC ?? DBNull.Value));
            sqlComando.Parameters.Add(new SqlParameter("@ToleranciaCompraCC", (object)sociedadDTO.TOLERANCIA_COMPRA_CC ?? DBNull.Value));
            sqlComando.Parameters.Add(new SqlParameter("@UsuarioAlta", (object)sociedadDTO.USUARIO_MANTENIMIENTO.USUARIO_ALTA.ToUpper() ?? DBNull.Value));
            sqlComando.Parameters.Add(new SqlParameter("@Mandante", (object)sociedadDTO.MANDANTE.ToString() ?? DBNull.Value));
            sqlComando.Parameters.Add(new SqlParameter("@TiempoLiquidacion", (object)sociedadDTO.TiempoLiquidacion ?? DBNull.Value));

            return Convert.ToInt32(sqlComando.ExecuteScalar());

        }

        public Int32 EjectuarSentenciaUpdate(SociedadDTO sociedadDTO)
        {
            SqlCommand sqlComando;

            sqlComando = new SqlCommand(sqlUpdate, _sqlConn);
            sqlComando.CommandType = CommandType.Text;

            if (_sqlTran != null)
                sqlComando.Transaction = _sqlTran;

            sqlComando.Parameters.Add(new SqlParameter("@CodigoSociedad", (object)sociedadDTO.CODIGO_SOCIEDAD ?? DBNull.Value));
            sqlComando.Parameters.Add(new SqlParameter("@Nombre", (object)sociedadDTO.NOMBRE.ToUpper() ?? DBNull.Value));
            sqlComando.Parameters.Add(new SqlParameter("@MesesFactura", (object)sociedadDTO.MESES_FACTURA ?? DBNull.Value));
            sqlComando.Parameters.Add(new SqlParameter("@Pais", (object)sociedadDTO.PAIS.ToUpper() ?? DBNull.Value));
            sqlComando.Parameters.Add(new SqlParameter("@Moneda", (object)sociedadDTO.MONEDA ?? DBNull.Value));
            sqlComando.Parameters.Add(new SqlParameter("@MontoCompraCC", (object)sociedadDTO.MONTO_COMPRA_CC ?? DBNull.Value));
            sqlComando.Parameters.Add(new SqlParameter("@ToleranciaCompraCC", (object)sociedadDTO.TOLERANCIA_COMPRA_CC ?? DBNull.Value));
            sqlComando.Parameters.Add(new SqlParameter("@Alta", (object)sociedadDTO.ALTA ?? DBNull.Value));
            sqlComando.Parameters.Add(new SqlParameter("@UsuarioModificacion", (object)sociedadDTO.USUARIO_MANTENIMIENTO.USUARIO_MODIFICO.ToUpper() ?? DBNull.Value));
            sqlComando.Parameters.Add(new SqlParameter("@Mandante", (object)sociedadDTO.MANDANTE.ToUpper() ?? DBNull.Value));
            sqlComando.Parameters.Add(new SqlParameter("@TiempoLiquidacion", (object)sociedadDTO.TiempoLiquidacion ?? DBNull.Value));

            return Convert.ToInt32(sqlComando.ExecuteScalar());
        }

        protected List<SociedadDTO> CargarReader(SqlDataReader sqlReader)
        {
            SociedadDTO _sociedadDto = null;
            List<SociedadDTO> _listaSociedad = new List<SociedadDTO>();

            try
            {
                while (sqlReader.Read())
                {
                    _sociedadDto = new SociedadDTO();


                    _sociedadDto.CODIGO_SOCIEDAD = sqlReader.GetString(0);
                    _sociedadDto.NOMBRE = sqlReader.GetString(1);
                    _sociedadDto.MESES_FACTURA = sqlReader.GetInt16(2);
                    _sociedadDto.PAIS = sqlReader.GetString(3);
                    _sociedadDto.MONEDA = sqlReader.GetString(4);
                    _sociedadDto.MONTO_COMPRA_CC = sqlReader.GetDouble(5);
                    _sociedadDto.TOLERANCIA_COMPRA_CC = sqlReader.GetInt16(6);
                    _sociedadDto.TiempoLiquidacion = sqlReader.IsDBNull(13) ? 0 : sqlReader.GetInt32(13);
                    _sociedadDto.ALTA = sqlReader.GetBoolean(7);
                    _sociedadDto.USUARIO_MANTENIMIENTO.USUARIO_ALTA = sqlReader.GetString(8);
                    _sociedadDto.USUARIO_MANTENIMIENTO.FECHA_ALTA = sqlReader.GetDateTime(9);
                    _sociedadDto.USUARIO_MANTENIMIENTO.USUARIO_MODIFICO = sqlReader.IsDBNull(10) ? null : sqlReader.GetString(10);
                    _sociedadDto.USUARIO_MANTENIMIENTO.FECHA_MODIFICACION = sqlReader.IsDBNull(11) ? (DateTime?)null : sqlReader.GetDateTime(11);
                    _sociedadDto.MANDANTE = sqlReader.IsDBNull(12) ? null : sqlReader.GetString(12);

                    _listaSociedad.Add(_sociedadDto);
                }
            }
            finally
            {
                if (sqlReader != null) sqlReader.Close();
            }
            return _listaSociedad;
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

        public List<SociedadDTO> ListaSociedad()
        {
            SqlCommand sqlComando;
            string sql = sqlSelect;

            sqlComando = new SqlCommand(sql, _sqlConn);
            if (_sqlTran != null)
                sqlComando.Transaction = _sqlTran;

            return CargarReader(sqlComando.ExecuteReader());
        }

        public List<LlenarDDL_DTO> ListaSociedadesActivas()
        {
            List<LlenarDDL_DTO> _listaLlenarDDL = new List<LlenarDDL_DTO>();
            LlenarDDL_DTO _llenarDDL = null;
            SqlCommand sqlComando;
            string sql = @" SELECT CodigoSociedad, (CodigoSociedad + ' - ' + nombre) nombre
                            FROM Sociedad 
                            WHERE Alta = 1 ";

            sqlComando = new SqlCommand(sql, _sqlConn);
            sqlComando.CommandType = CommandType.Text;

            if (_sqlTran != null)
                sqlComando.Transaction = _sqlTran;

            SqlDataReader _sqlDataR = sqlComando.ExecuteReader();

            while (_sqlDataR.Read())
            {
                _llenarDDL = new LlenarDDL_DTO();

                _llenarDDL.IDENTIFICADOR = _sqlDataR.GetString(0);
                _llenarDDL.DESCRIPCION = _sqlDataR.GetString(1);

                _listaLlenarDDL.Add(_llenarDDL);
            }

            return _listaLlenarDDL;
        }

        public List<LlenarDDL_DTO> ListaRegistradores(string aprobador)
        {
            List<LlenarDDL_DTO> _listaLlenarDDL = new List<LlenarDDL_DTO>();
            LlenarDDL_DTO _llenarDDL = null;
            SqlCommand sqlComando;
            string sql = @" SELECT DISTINCT A.Registrador, B.Nombre 
	                            FROM dbo.MapeoRegistradorAprobador A 
		                            INNER JOIN dbo.Usuario B
			                            ON A.Registrador = B.Usuario
	                            WHERE	A.Aprobador = @aprobador
		                            AND A.Estado	= 1 
		                            AND B.Alta		= 1 ";

            sqlComando = new SqlCommand(sql, _sqlConn);
            sqlComando.CommandType = CommandType.Text;

            if (_sqlTran != null)
                sqlComando.Transaction = _sqlTran;

            sqlComando.Parameters.Add(new SqlParameter("@aprobador", (object)aprobador ?? DBNull.Value));

            SqlDataReader _sqlDataR = sqlComando.ExecuteReader();

            while (_sqlDataR.Read())
            {
                _llenarDDL = new LlenarDDL_DTO();

                _llenarDDL.IDENTIFICADOR = _sqlDataR.GetString(0);
                _llenarDDL.DESCRIPCION = _sqlDataR.GetString(1);

                _listaLlenarDDL.Add(_llenarDDL);
            }

            return _listaLlenarDDL;
        }

        public Int16 DarBajaSociedad(Int32 CodSociedad, string usuario)
        {
            SqlCommand sqlComando;
            string sql = @"Update Sociedad set Alta = 0, UsuarioModificacion = @UsuarioModificacion, FechaModificacion = getdate()
                            where CodigoSociedad = @CodigoSociedad";

            sqlComando = new SqlCommand(sql, _sqlConn);
            sqlComando.CommandType = CommandType.Text;

            if (_sqlTran != null)
                sqlComando.Transaction = _sqlTran;

            sqlComando.Parameters.Add(new SqlParameter("@UsuarioModificacion", (object)usuario.ToUpper() ?? DBNull.Value));
            sqlComando.Parameters.Add(new SqlParameter("@CodigoSociedad", (object)CodSociedad ?? DBNull.Value));

            return Convert.ToInt16(sqlComando.ExecuteNonQuery());
        }

        public Int16 DarAltaSociedad(Int32 codSociedad, string usuario)
        {
            SqlCommand sqlComando;
            string sql = @"Update Sociedad set Alta = 1, UsuarioModificacion = @UsuarioModificacion, FechaModificacion = getdate()
                            where CodigoSociedad = @CodigoSociedad";

            sqlComando = new SqlCommand(sql, _sqlConn);
            sqlComando.CommandType = CommandType.Text;

            if (_sqlTran != null)
                sqlComando.Transaction = _sqlTran;

            sqlComando.Parameters.Add(new SqlParameter("@UsuarioModificacion", (object)usuario.ToUpper() ?? DBNull.Value));
            sqlComando.Parameters.Add(new SqlParameter("@CodigoSociedad", (object)codSociedad ?? DBNull.Value));

            return Convert.ToInt16(sqlComando.ExecuteNonQuery());

        }

        public List<LlenarDDL_DTO> LlenarImpuestos(string codigoSociedad)
        {
            SqlCommand _sqlComando = null;
            string sql = @" SELECT Descripcion, Valor FROM dbo.Impuestos 
                                WHERE Pais = (SELECT Pais FROM dbo.Sociedad WHERE CodigoSociedad = @BUKRS)
                            ORDER BY orden";

            _sqlComando = new SqlCommand(sql, _sqlConn);
            _sqlComando.CommandType = CommandType.Text;

            if (_sqlTran != null)
                _sqlComando.Transaction = _sqlTran;

            _sqlComando.Parameters.Add(new SqlParameter("@BUKRS", (object)codigoSociedad));
            return CargarReaderDDL(_sqlComando.ExecuteReader());
        }

        protected List<LlenarDDL_DTO> CargarReaderDDL(SqlDataReader sqlDataReader)
        {
            List<LlenarDDL_DTO> listaLlenarDDL = new List<LlenarDDL_DTO>();
            LlenarDDL_DTO llenarDDL = null;

            while (sqlDataReader.Read())
            {
                llenarDDL = new LlenarDDL_DTO();

                llenarDDL.IDENTIFICADOR = sqlDataReader.GetDouble(1).ToString();
                llenarDDL.DESCRIPCION = sqlDataReader.GetString(0);

                listaLlenarDDL.Add(llenarDDL);
            }

            return listaLlenarDDL;
        }

        public string MandanteSociedad(string codSociedad)
        {
            SqlCommand sqlComando;
            string sql = @"SELECT TOP 1 Mandante FROM dbo.Sociedad WHERE CodigoSociedad = @CodigoSociedad";

            sqlComando = new SqlCommand(sql, _sqlConn);

            if (_sqlTran != null)
                sqlComando.Transaction = _sqlTran;

            sqlComando.Parameters.Add(new SqlParameter("@CodigoSociedad", (object)codSociedad ?? DBNull.Value));

            return Convert.ToString(sqlComando.ExecuteScalar());

        }
        #endregion
    }
}
