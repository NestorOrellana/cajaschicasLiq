using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using DipCmiGT.LogicaComun;
using DipCmiGT.LogicaCajasChicas;

namespace LogicaCajasChicas.Entidad
{
    public class Liquidacion
    {
        #region Declaraciones
        private SqlConnection _sqlConn = null;
        private SqlTransaction _sqlTran = null;

        protected string sqlSelect = @"SELECT cl.CodigoLiquidacion, cl.Pais, cl.Nivel, cl.TipoGasto, cl.MontoAutorizado, cl.Alta, cl.UsuarioAlta, 
                                        cl.FechaAlta, cl.UsuarioModificacion, cl.FechaModificacion, nl.Nivel AS DescripcionNivel, 
                                        g.Gasto AS DescripcionGasto
                                        FROM CatLiquidacion AS cl
                                        INNER JOIN NivelLiquidacion AS nl ON
                                        nl.CodigoNivel = cl.Nivel
                                        INNER JOIN Gasto AS g ON
                                        g.CodigoGasto = cl.TipoGasto";

        protected string sqlInsert = @"INSERT INTO CatLiquidacion (Pais, Nivel, TipoGasto, MontoAutorizado, Alta, UsuarioAlta, FechaAlta)
                                    VALUES (@Pais, @Nivel, @TipoGasto, @MontoAutorizado, @Alta, @UsuarioAlta ,getdate()) ";

        protected string sqlUpdate = @"UPDATE CatLiquidacion SET Pais = @Pais, Nivel = @Nivel, TipoGasto = @TipoGasto, MontoAutorizado = @MontoAutorizado, 
                                    Alta = @Alta, UsuarioModificacion = @UsuarioModificacion, FechaModificacion = getdate()
                                    WHERE CodigoLiquidacion = @CodigoLiquidacion";
        #endregion

        #region Constructores
        public Liquidacion(SqlConnection sqlConn)
        {
            _sqlConn = sqlConn;
        }

        public Liquidacion(SqlConnection sqlConn, SqlTransaction sqlTran)
        {
            _sqlConn = sqlConn;
            _sqlTran = sqlTran;
        }
        #endregion
        #region Metodos
        public Int32 EjecutarSentenciaInsert(LiquidacionDTO liquidacionDTO)
        {
            SqlCommand sqlComando;

            sqlComando = new SqlCommand(sqlInsert, _sqlConn);
            sqlComando.CommandType = CommandType.Text;

            if (_sqlTran != null)
                sqlComando.Transaction = _sqlTran;

            sqlComando.Parameters.Add(new SqlParameter("@Pais", (object)liquidacionDTO.Pais.ToUpper() ?? DBNull.Value));
            sqlComando.Parameters.Add(new SqlParameter("@Nivel", (object)liquidacionDTO.Nivel ?? DBNull.Value));
            sqlComando.Parameters.Add(new SqlParameter("@TipoGasto", (object)liquidacionDTO.TipoGasto ?? DBNull.Value));
            sqlComando.Parameters.Add(new SqlParameter("@MontoAutorizado", (object)liquidacionDTO.MontoAutorizado ?? DBNull.Value));
            sqlComando.Parameters.Add(new SqlParameter("@Alta", (object)liquidacionDTO.Alta ?? DBNull.Value));
            sqlComando.Parameters.Add(new SqlParameter("@UsuarioAlta", (object)liquidacionDTO.USUARIO_MANTENIMIENTO.USUARIO_ALTA.ToUpper() ?? DBNull.Value));

            return Convert.ToInt32(sqlComando.ExecuteScalar());
        }

        public Int32 EjectuarSentenciaUpdate(LiquidacionDTO liquidacionDTO)
        {
            SqlCommand sqlComando;

            sqlComando = new SqlCommand(sqlUpdate, _sqlConn);
            sqlComando.CommandType = CommandType.Text;

            if (_sqlTran != null)
                sqlComando.Transaction = _sqlTran;

            sqlComando.Parameters.Add(new SqlParameter("@CodigoLiquidacion", (object)liquidacionDTO.CodigoLiquidacion ?? DBNull.Value));
            sqlComando.Parameters.Add(new SqlParameter("@Pais", (object)liquidacionDTO.Pais ?? DBNull.Value));
            sqlComando.Parameters.Add(new SqlParameter("@Nivel", (object)liquidacionDTO.Nivel ?? DBNull.Value));
            sqlComando.Parameters.Add(new SqlParameter("@TipoGasto", (object)liquidacionDTO.TipoGasto ?? DBNull.Value));
            sqlComando.Parameters.Add(new SqlParameter("@MontoAutorizado", (object)liquidacionDTO.MontoAutorizado ?? DBNull.Value));
            sqlComando.Parameters.Add(new SqlParameter("@Alta", (object)liquidacionDTO.Alta ?? DBNull.Value));
            sqlComando.Parameters.Add(new SqlParameter("@UsuarioModificacion", (object)liquidacionDTO.USUARIO_MANTENIMIENTO.USUARIO_MODIFICO.ToUpper() ?? DBNull.Value));

            return Convert.ToInt32(sqlComando.ExecuteScalar());
        }

        public Int16 DarBajaNivel(int codigoNivel, string usuario)
        {
            SqlCommand sqlComando;
            string sql = @"Update NivelLiquidacion set Alta = 0, UsuarioModificacion = @UsuarioModificacion, FechaModificacion = getdate()
                            where CodigoNivel = @CodigoNivel";

            sqlComando = new SqlCommand(sql, _sqlConn);
            sqlComando.CommandType = CommandType.Text;

            if (_sqlTran != null)
                sqlComando.Transaction = _sqlTran;

            sqlComando.Parameters.Add(new SqlParameter("@UsuarioModificacion", (object)usuario.ToUpper() ?? DBNull.Value));
            sqlComando.Parameters.Add(new SqlParameter("@CodigoNivel", (object)codigoNivel ?? DBNull.Value));

            return Convert.ToInt16(sqlComando.ExecuteNonQuery());
        }

        public Int16 DarAltaLiquidacion(int codigoLiquidacion, string usuario)
        {
            SqlCommand sqlComando;
            string sql = @"Update CatLiquidacion set Alta = 1, UsuarioModificacion = @UsuarioModificacion, FechaModificacion = getdate()
                            where CodigoLiquidacion = @CodigoLiquidacion";

            sqlComando = new SqlCommand(sql, _sqlConn);
            sqlComando.CommandType = CommandType.Text;

            if (_sqlTran != null)
                sqlComando.Transaction = _sqlTran;

            sqlComando.Parameters.Add(new SqlParameter("@UsuarioModificacion", (object)usuario.ToUpper() ?? DBNull.Value));
            sqlComando.Parameters.Add(new SqlParameter("@CodigoLiquidacion", (object)codigoLiquidacion ?? DBNull.Value));

            return Convert.ToInt16(sqlComando.ExecuteNonQuery());
        }

        public List<LiquidacionDTO> ListaLiquidacion()
        {
            SqlCommand sqlComando;
            string sql = sqlSelect;

            sqlComando = new SqlCommand(sql, _sqlConn);
            if (_sqlTran != null)
                sqlComando.Transaction = _sqlTran;

            return CargarReader(sqlComando.ExecuteReader());
        }

        public List<LlenarDDL_DTO> ListaPaisesDDL()
        {
            SqlCommand sqlComando;
            string sql = @"select distinct Pais from [dbo].[Sociedad]";

            sqlComando = new SqlCommand(sql, _sqlConn);

            if (_sqlTran != null)
                sqlComando.Transaction = _sqlTran;

            return CargarReaderPaisesDDL(sqlComando.ExecuteReader());
        }

        public List<LlenarDDL_DTO> ListaNiveles(string pais)
        {
            SqlCommand sqlComando;
            string sql = @"select CodigoNivel, Nivel from [dbo].[NivelLiquidacion] WHERE Pais = @Pais AND Alta = 1";

            sqlComando = new SqlCommand(sql, _sqlConn);

            if (_sqlTran != null)
                sqlComando.Transaction = _sqlTran;

            sqlComando.Parameters.Add(new SqlParameter("@Pais", (object)pais ?? DBNull.Value));

            return CargarReaderPaisesDDL(sqlComando.ExecuteReader(), false);
        }

        public List<LlenarDDL_DTO> ListaGastos(string pais)
        {
            SqlCommand sqlComando;
            string sql = @"select CodigoGasto, Gasto from [dbo].[Gasto] WHERE Pais = @Pais AND Alta = 1";

            sqlComando = new SqlCommand(sql, _sqlConn);

            if (_sqlTran != null)
                sqlComando.Transaction = _sqlTran;

            sqlComando.Parameters.Add(new SqlParameter("@Pais", (object)pais ?? DBNull.Value));

            return CargarReaderPaisesDDL(sqlComando.ExecuteReader(), false);
        }

        protected List<LiquidacionDTO> CargarReader(SqlDataReader sqlReader)
        {
            LiquidacionDTO _liquidacionDTO = null;
            List<LiquidacionDTO> _listaLiquidaciones = new List<LiquidacionDTO>();

            try
            {
                while (sqlReader.Read())
                {
                    _liquidacionDTO = new LiquidacionDTO();

                    _liquidacionDTO.CodigoLiquidacion = sqlReader.GetInt32(0);
                    _liquidacionDTO.Pais = sqlReader.GetString(1);
                    _liquidacionDTO.Nivel = sqlReader.GetInt32(2);
                    _liquidacionDTO.DescripcionNivel = sqlReader.GetString(10);
                    _liquidacionDTO.TipoGasto = sqlReader.GetInt32(3);
                    _liquidacionDTO.DescripcionTipoGasto = sqlReader.GetString(11);
                    _liquidacionDTO.MontoAutorizado = sqlReader.GetDecimal(4);
                    _liquidacionDTO.Alta = sqlReader.GetBoolean(5);
                    _liquidacionDTO.USUARIO_MANTENIMIENTO.USUARIO_ALTA = sqlReader.GetString(6);
                    _liquidacionDTO.USUARIO_MANTENIMIENTO.FECHA_ALTA = sqlReader.GetDateTime(7);
                    _liquidacionDTO.USUARIO_MANTENIMIENTO.USUARIO_MODIFICO = sqlReader.IsDBNull(8) ? null : sqlReader.GetString(8);
                    _liquidacionDTO.USUARIO_MANTENIMIENTO.FECHA_MODIFICACION = sqlReader.IsDBNull(9) ? (DateTime?)null : sqlReader.GetDateTime(9);

                    _listaLiquidaciones.Add(_liquidacionDTO);
                }
            }
            finally
            {
                if (sqlReader != null) sqlReader.Close();
            }
            return _listaLiquidaciones;
        }

        protected List<LlenarDDL_DTO> CargarReaderPaisesDDL(SqlDataReader sqlDataReader, bool paises = true)
        {
            List<LlenarDDL_DTO> listaLlenarDDL = new List<LlenarDDL_DTO>();
            LlenarDDL_DTO llenarDDL = null;

            while (sqlDataReader.Read())
            {
                llenarDDL = new LlenarDDL_DTO();


                if (paises)
                {
                    llenarDDL.IDENTIFICADOR = sqlDataReader.GetString(0);
                    llenarDDL.DESCRIPCION = sqlDataReader.GetString(0);
                }
                else
                {
                    llenarDDL.IDENTIFICADOR = sqlDataReader.GetInt32(0).ToString();
                    llenarDDL.DESCRIPCION = sqlDataReader.GetString(1);
                }

                listaLlenarDDL.Add(llenarDDL);
            }

            return listaLlenarDDL;
        }

        public bool ExisteLiquidacion(string pais, int nivel, int tipoGasto)
        {
            SqlCommand sqlComando;
            string sql = @"select COUNT(*)
                            from CatLiquidacion
                            where Pais = @Pais AND Nivel = @Nivel AND TipoGasto = @TipoGasto";

            sqlComando = new SqlCommand(sql, _sqlConn);

            if (_sqlTran != null)
                sqlComando.Transaction = _sqlTran;
            sqlComando.Parameters.Add(new SqlParameter("@Pais", (object)pais ?? DBNull.Value));
            sqlComando.Parameters.Add(new SqlParameter("@Nivel", (object)nivel ?? DBNull.Value));
            sqlComando.Parameters.Add(new SqlParameter("@TipoGasto", (object)tipoGasto ?? DBNull.Value));

            return Convert.ToInt32(sqlComando.ExecuteScalar()) > 0 ? true : false;
        }
        #endregion
    }
}
