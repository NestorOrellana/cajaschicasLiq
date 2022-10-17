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
    public class SincronizacionPO
    {
        #region Declaraciones
        private SqlConnection _sqlConn = null;
        private SqlTransaction _sqlTran = null;

        protected string sqlSelect = @"SELECT CodigoKilometraje, Pais, Origen, Destino, Kilometros, Alta, UsuarioAlta, FechaAlta, 
                                        UsuarioModificacion, FechaModificacion
                                        FROM Kilometraje";

        protected string sqlInsert = @"INSERT INTO Kilometraje (Pais, Origen, Destino, Kilometros, Alta, UsuarioAlta, FechaAlta)
                                    VALUES (@Pais, @Origen, @Destino, @Kilometros, @Alta, @UsuarioAlta ,getdate()) ";

        protected string sqlUpdate = @"UPDATE Kilometraje SET Pais = @Pais, Origen = @Origen, Destino = @Destino, Kilometros = @Kilometros, 
                                    Alta = @Alta, UsuarioModificacion = @UsuarioModificacion, FechaModificacion = getdate()
                                    WHERE CodigoKilometraje = @CodigoKilometraje";
        #endregion

        #region Constructores
        public SincronizacionPO(SqlConnection sqlConn)
        {
            _sqlConn = sqlConn;
        }

        public SincronizacionPO(SqlConnection sqlConn, SqlTransaction sqlTran)
        {
            _sqlConn = sqlConn;
            _sqlTran = sqlTran;
        }
        #endregion
        #region Metodos
        public Int32 EjecutarSentenciaInsert(KilometrajeDTO kilometrajeDTO)
        {
            SqlCommand sqlComando;

            sqlComando = new SqlCommand(sqlInsert, _sqlConn);
            sqlComando.CommandType = CommandType.Text;

            if (_sqlTran != null)
                sqlComando.Transaction = _sqlTran;

            sqlComando.Parameters.Add(new SqlParameter("@Pais", (object)kilometrajeDTO.Pais.ToUpper() ?? DBNull.Value));
            sqlComando.Parameters.Add(new SqlParameter("@Origen", (object)kilometrajeDTO.Origen ?? DBNull.Value));
            sqlComando.Parameters.Add(new SqlParameter("@Destino", (object)kilometrajeDTO.Destino ?? DBNull.Value));
            sqlComando.Parameters.Add(new SqlParameter("@Kilometros", (object)kilometrajeDTO.Kilometros ?? DBNull.Value));
            sqlComando.Parameters.Add(new SqlParameter("@Alta", (object)kilometrajeDTO.Alta ?? DBNull.Value));
            sqlComando.Parameters.Add(new SqlParameter("@UsuarioAlta", (object)kilometrajeDTO.USUARIO_MANTENIMIENTO.USUARIO_ALTA.ToUpper() ?? DBNull.Value));

            return Convert.ToInt32(sqlComando.ExecuteScalar());
        }

        public Int32 EjectuarSentenciaUpdate(KilometrajeDTO kilometrajeDTO)
        {
            SqlCommand sqlComando;

            sqlComando = new SqlCommand(sqlUpdate, _sqlConn);
            sqlComando.CommandType = CommandType.Text;

            if (_sqlTran != null)
                sqlComando.Transaction = _sqlTran;

            sqlComando.Parameters.Add(new SqlParameter("@CodigoKilometraje", (object)kilometrajeDTO.CodigoKilometraje ?? DBNull.Value));
            sqlComando.Parameters.Add(new SqlParameter("@Pais", (object)kilometrajeDTO.Pais ?? DBNull.Value));
            sqlComando.Parameters.Add(new SqlParameter("@Origen", (object)kilometrajeDTO.Origen ?? DBNull.Value));
            sqlComando.Parameters.Add(new SqlParameter("@Destino", (object)kilometrajeDTO.Destino ?? DBNull.Value));
            sqlComando.Parameters.Add(new SqlParameter("@Kilometros", (object)kilometrajeDTO.Kilometros ?? DBNull.Value));
            sqlComando.Parameters.Add(new SqlParameter("@Alta", (object)kilometrajeDTO.Alta ?? DBNull.Value));
            sqlComando.Parameters.Add(new SqlParameter("@UsuarioModificacion", (object)kilometrajeDTO.USUARIO_MANTENIMIENTO.USUARIO_MODIFICO.ToUpper() ?? DBNull.Value));

            return Convert.ToInt32(sqlComando.ExecuteScalar());
        }

        public Int16 DarBajaKilometraje(long codigoKilometraje, string usuario)
        {
            SqlCommand sqlComando;
            string sql = @"Update Kilometraje set Alta = 0, UsuarioModificacion = @UsuarioModificacion, FechaModificacion = getdate()
                            where CodigoKilometraje = @CodigoKilometraje";

            sqlComando = new SqlCommand(sql, _sqlConn);
            sqlComando.CommandType = CommandType.Text;

            if (_sqlTran != null)
                sqlComando.Transaction = _sqlTran;

            sqlComando.Parameters.Add(new SqlParameter("@UsuarioModificacion", (object)usuario.ToUpper() ?? DBNull.Value));
            sqlComando.Parameters.Add(new SqlParameter("@CodigoKilometraje", (object)codigoKilometraje ?? DBNull.Value));

            return Convert.ToInt16(sqlComando.ExecuteNonQuery());
        }

        public Int16 DarAltaKilometraje(long codigoKilometraje, string usuario)
        {
            SqlCommand sqlComando;
            string sql = @"Update Kilometraje set Alta = 1, UsuarioModificacion = @UsuarioModificacion, FechaModificacion = getdate()
                            where CodigoKilometraje = @CodigoKilometraje";

            sqlComando = new SqlCommand(sql, _sqlConn);
            sqlComando.CommandType = CommandType.Text;

            if (_sqlTran != null)
                sqlComando.Transaction = _sqlTran;

            sqlComando.Parameters.Add(new SqlParameter("@UsuarioModificacion", (object)usuario.ToUpper() ?? DBNull.Value));
            sqlComando.Parameters.Add(new SqlParameter("@CodigoKilometraje", (object)codigoKilometraje ?? DBNull.Value));

            return Convert.ToInt16(sqlComando.ExecuteNonQuery());
        }

        public List<KilometrajeDTO> ListaKilometrajes()
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

        protected List<KilometrajeDTO> CargarReader(SqlDataReader sqlReader)
        {
            KilometrajeDTO _kilometrajeDTO = null;
            List<KilometrajeDTO> _listaKilometraje = new List<KilometrajeDTO>();

            try
            {
                while (sqlReader.Read())
                {
                    _kilometrajeDTO = new KilometrajeDTO();

                    _kilometrajeDTO.CodigoKilometraje = sqlReader.GetInt64(0);
                    _kilometrajeDTO.Pais = sqlReader.GetString(1);
                    _kilometrajeDTO.Origen = sqlReader.GetString(2);
                    _kilometrajeDTO.Destino = sqlReader.GetString(3);
                    _kilometrajeDTO.Kilometros = sqlReader.GetDecimal(4);
                    _kilometrajeDTO.Alta = sqlReader.GetBoolean(5);
                    _kilometrajeDTO.USUARIO_MANTENIMIENTO.USUARIO_ALTA = sqlReader.GetString(6);
                    _kilometrajeDTO.USUARIO_MANTENIMIENTO.FECHA_ALTA = sqlReader.GetDateTime(7);
                    _kilometrajeDTO.USUARIO_MANTENIMIENTO.USUARIO_MODIFICO = sqlReader.IsDBNull(8) ? null : sqlReader.GetString(8);
                    _kilometrajeDTO.USUARIO_MANTENIMIENTO.FECHA_MODIFICACION = sqlReader.IsDBNull(9) ? (DateTime?)null : sqlReader.GetDateTime(9);

                    _listaKilometraje.Add(_kilometrajeDTO);
                }
            }
            finally
            {
                if (sqlReader != null) sqlReader.Close();
            }
            return _listaKilometraje;
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

        public bool ExisteKilometraje(string pais, string origen, string destino)
        {
            SqlCommand sqlComando;
            string sql = @"select COUNT(*)
                            from Kilometraje
                            where Pais = @Pais AND LOWER(Origen) = LOWER(@Origen) AND LOWER(Destino) = LOWER(@Destino)";

            sqlComando = new SqlCommand(sql, _sqlConn);

            if (_sqlTran != null)
                sqlComando.Transaction = _sqlTran;
            sqlComando.Parameters.Add(new SqlParameter("@Pais", (object)pais ?? DBNull.Value));
            sqlComando.Parameters.Add(new SqlParameter("@Origen", (object)origen ?? DBNull.Value));
            sqlComando.Parameters.Add(new SqlParameter("@Destino", (object)destino ?? DBNull.Value));

            return Convert.ToInt32(sqlComando.ExecuteScalar()) > 0 ? true : false;
        }
        #endregion
    }
}
