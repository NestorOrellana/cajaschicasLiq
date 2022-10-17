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
    public class Gasto
    {
        #region Declaraciones
        private SqlConnection _sqlConn = null;
        private SqlTransaction _sqlTran = null;

        protected string sqlSelect = @"SELECT CodigoGasto, Pais, Gasto, Kilometraje, Alta, UsuarioAlta, FechaAlta, 
                                        UsuarioModificacion, FechaModificacion
                                        FROM Gasto";

        protected string sqlInsert = @"INSERT INTO Gasto (Pais, Gasto, Kilometraje, Alta, UsuarioAlta, FechaAlta)
                                    VALUES (@Pais, @Gasto, @Kilometraje, @Alta, @UsuarioAlta ,getdate()) ";

        protected string sqlUpdate = @"UPDATE Gasto SET Pais = @Pais, Gasto = @Gasto, Kilometraje = @Kilometraje, 
                                    Alta = @Alta, UsuarioModificacion = @UsuarioModificacion, FechaModificacion = getdate()
                                    WHERE CodigoGasto = @CodigoGasto";
        #endregion

        #region Constructores
        public Gasto(SqlConnection sqlConn)
        {
            _sqlConn = sqlConn;
        }

        public Gasto(SqlConnection sqlConn, SqlTransaction sqlTran)
        {
            _sqlConn = sqlConn;
            _sqlTran = sqlTran;
        }
        #endregion
        #region Metodos
        public Int32 EjecutarSentenciaInsert(GastosDTO gastoDTO)
        {
            SqlCommand sqlComando;

            sqlComando = new SqlCommand(sqlInsert, _sqlConn);
            sqlComando.CommandType = CommandType.Text;

            if (_sqlTran != null)
                sqlComando.Transaction = _sqlTran;

            sqlComando.Parameters.Add(new SqlParameter("@Pais", (object)gastoDTO.Pais.ToUpper() ?? DBNull.Value));
            sqlComando.Parameters.Add(new SqlParameter("@Gasto", (object)gastoDTO.Gasto ?? DBNull.Value));
            sqlComando.Parameters.Add(new SqlParameter("@Kilometraje", (object)gastoDTO.Kilometraje ?? DBNull.Value));
            sqlComando.Parameters.Add(new SqlParameter("@Alta", (object)gastoDTO.Alta ?? DBNull.Value));
            sqlComando.Parameters.Add(new SqlParameter("@UsuarioAlta", (object)gastoDTO.USUARIO_MANTENIMIENTO.USUARIO_ALTA.ToUpper() ?? DBNull.Value));

            return Convert.ToInt32(sqlComando.ExecuteScalar());
        }

        public Int32 EjectuarSentenciaUpdate(GastosDTO gastoDTO)
        {
            SqlCommand sqlComando;

            sqlComando = new SqlCommand(sqlUpdate, _sqlConn);
            sqlComando.CommandType = CommandType.Text;

            if (_sqlTran != null)
                sqlComando.Transaction = _sqlTran;

            sqlComando.Parameters.Add(new SqlParameter("@CodigoGasto", (object)gastoDTO.CodigoGasto ?? DBNull.Value));
            sqlComando.Parameters.Add(new SqlParameter("@Pais", (object)gastoDTO.Pais ?? DBNull.Value));
            sqlComando.Parameters.Add(new SqlParameter("@Gasto", (object)gastoDTO.Gasto ?? DBNull.Value));
            sqlComando.Parameters.Add(new SqlParameter("@Kilometraje", (object)gastoDTO.Kilometraje ?? DBNull.Value));
            sqlComando.Parameters.Add(new SqlParameter("@Alta", (object)gastoDTO.Alta ?? DBNull.Value));
            sqlComando.Parameters.Add(new SqlParameter("@UsuarioModificacion", (object)gastoDTO.USUARIO_MANTENIMIENTO.USUARIO_MODIFICO.ToUpper() ?? DBNull.Value));

            return Convert.ToInt32(sqlComando.ExecuteScalar());
        }

        public Int16 DarBajaGasto(Int32 codigoGasto, string usuario)
        {
            SqlCommand sqlComando;
            string sql = @"Update Gasto set Alta = 0, UsuarioModificacion = @UsuarioModificacion, FechaModificacion = getdate()
                            where CodigoGasto = @CodigoGasto";

            sqlComando = new SqlCommand(sql, _sqlConn);
            sqlComando.CommandType = CommandType.Text;

            if (_sqlTran != null)
                sqlComando.Transaction = _sqlTran;

            sqlComando.Parameters.Add(new SqlParameter("@UsuarioModificacion", (object)usuario.ToUpper() ?? DBNull.Value));
            sqlComando.Parameters.Add(new SqlParameter("@CodigoGasto", (object)codigoGasto ?? DBNull.Value));

            return Convert.ToInt16(sqlComando.ExecuteNonQuery());
        }

        public Int16 DarAltaGasto(Int32 codigoGasto, string usuario)
        {
            SqlCommand sqlComando;
            string sql = @"Update Gasto set Alta = 1, UsuarioModificacion = @UsuarioModificacion, FechaModificacion = getdate()
                            where CodigoGasto = @CodigoGasto";

            sqlComando = new SqlCommand(sql, _sqlConn);
            sqlComando.CommandType = CommandType.Text;

            if (_sqlTran != null)
                sqlComando.Transaction = _sqlTran;

            sqlComando.Parameters.Add(new SqlParameter("@UsuarioModificacion", (object)usuario.ToUpper() ?? DBNull.Value));
            sqlComando.Parameters.Add(new SqlParameter("@CodigoGasto", (object)codigoGasto ?? DBNull.Value));

            return Convert.ToInt16(sqlComando.ExecuteNonQuery());
        }

        public List<GastosDTO> ListaGastos()
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

        protected List<GastosDTO> CargarReader(SqlDataReader sqlReader)
        {
            GastosDTO _gastoDTO = null;
            List<GastosDTO> _listaGastos = new List<GastosDTO>();

            try
            {
                while (sqlReader.Read())
                {
                    _gastoDTO = new GastosDTO();

                    _gastoDTO.CodigoGasto = sqlReader.GetInt32(0);
                    _gastoDTO.Pais = sqlReader.GetString(1);
                    _gastoDTO.Gasto = sqlReader.GetString(2);
                    _gastoDTO.Kilometraje = sqlReader.GetBoolean(3);
                    _gastoDTO.Alta = sqlReader.GetBoolean(4);
                    _gastoDTO.USUARIO_MANTENIMIENTO.USUARIO_ALTA = sqlReader.GetString(5);
                    _gastoDTO.USUARIO_MANTENIMIENTO.FECHA_ALTA = sqlReader.GetDateTime(6);
                    _gastoDTO.USUARIO_MANTENIMIENTO.USUARIO_MODIFICO = sqlReader.IsDBNull(7) ? null : sqlReader.GetString(7);
                    _gastoDTO.USUARIO_MANTENIMIENTO.FECHA_MODIFICACION = sqlReader.IsDBNull(8) ? (DateTime?)null : sqlReader.GetDateTime(8);

                    _listaGastos.Add(_gastoDTO);
                }
            }
            finally
            {
                if (sqlReader != null) sqlReader.Close();
            }
            return _listaGastos;
        }

        protected List<LlenarDDL_DTO> CargarReaderPaisesDDL(SqlDataReader sqlDataReader)
        {
            List<LlenarDDL_DTO> listaLlenarDDL = new List<LlenarDDL_DTO>();
            LlenarDDL_DTO llenarDDL = null;

            while (sqlDataReader.Read())
            {
                llenarDDL = new LlenarDDL_DTO();

                llenarDDL.IDENTIFICADOR = sqlDataReader.GetString(0);
                llenarDDL.DESCRIPCION = sqlDataReader.GetString(0);

                listaLlenarDDL.Add(llenarDDL);
            }

            return listaLlenarDDL;
        }
        #endregion
    }
}
