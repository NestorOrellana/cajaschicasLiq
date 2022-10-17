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
    public class NivelLiquidacion
    {
        #region Declaraciones
        private SqlConnection _sqlConn = null;
        private SqlTransaction _sqlTran = null;

        protected string sqlSelect = @"SELECT CodigoNivel, Pais, Nivel, Alta, UsuarioAlta, FechaAlta, 
                                        UsuarioModificacion, FechaModificacion
                                        FROM NivelLiquidacion";

        protected string sqlInsert = @"INSERT INTO NivelLiquidacion (Pais, Nivel, Alta, UsuarioAlta, FechaAlta)
                                    VALUES (@Pais, @Nivel, @Alta, @UsuarioAlta ,getdate()) ";

        protected string sqlUpdate = @"UPDATE NivelLiquidacion SET Pais = @Pais, Nivel = @Nivel, 
                                    Alta = @Alta, UsuarioModificacion = @UsuarioModificacion, FechaModificacion = getdate()
                                    WHERE CodigoNivel = @CodigoNivel";
        #endregion

        #region Constructores
        public NivelLiquidacion(SqlConnection sqlConn)
        {
            _sqlConn = sqlConn;
        }

        public NivelLiquidacion(SqlConnection sqlConn, SqlTransaction sqlTran)
        {
            _sqlConn = sqlConn;
            _sqlTran = sqlTran;
        }
        #endregion
        #region Metodos
        public Int32 EjecutarSentenciaInsert(NivelLiquidacionDTO nivelDTO)
        {
            SqlCommand sqlComando;

            sqlComando = new SqlCommand(sqlInsert, _sqlConn);
            sqlComando.CommandType = CommandType.Text;

            if (_sqlTran != null)
                sqlComando.Transaction = _sqlTran;

            sqlComando.Parameters.Add(new SqlParameter("@Pais", (object)nivelDTO.Pais.ToUpper() ?? DBNull.Value));
            sqlComando.Parameters.Add(new SqlParameter("@Nivel", (object)nivelDTO.Nivel ?? DBNull.Value));
            sqlComando.Parameters.Add(new SqlParameter("@Alta", (object)nivelDTO.Alta ?? DBNull.Value));
            sqlComando.Parameters.Add(new SqlParameter("@UsuarioAlta", (object)nivelDTO.USUARIO_MANTENIMIENTO.USUARIO_ALTA.ToUpper() ?? DBNull.Value));

            return Convert.ToInt32(sqlComando.ExecuteScalar());
        }

        public Int32 EjectuarSentenciaUpdate(NivelLiquidacionDTO nivelDTO)
        {
            SqlCommand sqlComando;

            sqlComando = new SqlCommand(sqlUpdate, _sqlConn);
            sqlComando.CommandType = CommandType.Text;

            if (_sqlTran != null)
                sqlComando.Transaction = _sqlTran;

            sqlComando.Parameters.Add(new SqlParameter("@CodigoNivel", (object)nivelDTO.CodigoNivel ?? DBNull.Value));
            sqlComando.Parameters.Add(new SqlParameter("@Pais", (object)nivelDTO.Pais ?? DBNull.Value));
            sqlComando.Parameters.Add(new SqlParameter("@Nivel", (object)nivelDTO.Nivel ?? DBNull.Value));
            sqlComando.Parameters.Add(new SqlParameter("@Alta", (object)nivelDTO.Alta ?? DBNull.Value));
            sqlComando.Parameters.Add(new SqlParameter("@UsuarioModificacion", (object)nivelDTO.USUARIO_MANTENIMIENTO.USUARIO_MODIFICO.ToUpper() ?? DBNull.Value));

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

        public Int16 DarAltaNivel(int codigoNivel, string usuario)
        {
            SqlCommand sqlComando;
            string sql = @"Update NivelLiquidacion set Alta = 1, UsuarioModificacion = @UsuarioModificacion, FechaModificacion = getdate()
                            where CodigoNivel = @CodigoNivel";

            sqlComando = new SqlCommand(sql, _sqlConn);
            sqlComando.CommandType = CommandType.Text;

            if (_sqlTran != null)
                sqlComando.Transaction = _sqlTran;

            sqlComando.Parameters.Add(new SqlParameter("@UsuarioModificacion", (object)usuario.ToUpper() ?? DBNull.Value));
            sqlComando.Parameters.Add(new SqlParameter("@CodigoNivel", (object)codigoNivel ?? DBNull.Value));

            return Convert.ToInt16(sqlComando.ExecuteNonQuery());
        }

        public List<NivelLiquidacionDTO> ListaNiveles()
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

        protected List<NivelLiquidacionDTO> CargarReader(SqlDataReader sqlReader)
        {
            NivelLiquidacionDTO _nivelDTO = null;
            List<NivelLiquidacionDTO> _listaNiveles = new List<NivelLiquidacionDTO>();

            try
            {
                while (sqlReader.Read())
                {
                    _nivelDTO = new NivelLiquidacionDTO();

                    _nivelDTO.CodigoNivel = sqlReader.GetInt32(0);
                    _nivelDTO.Pais = sqlReader.GetString(1);
                    _nivelDTO.Nivel = sqlReader.GetString(2);
                    _nivelDTO.Alta = sqlReader.GetBoolean(3);
                    _nivelDTO.USUARIO_MANTENIMIENTO.USUARIO_ALTA = sqlReader.GetString(4);
                    _nivelDTO.USUARIO_MANTENIMIENTO.FECHA_ALTA = sqlReader.GetDateTime(5);
                    _nivelDTO.USUARIO_MANTENIMIENTO.USUARIO_MODIFICO = sqlReader.IsDBNull(6) ? null : sqlReader.GetString(6);
                    _nivelDTO.USUARIO_MANTENIMIENTO.FECHA_MODIFICACION = sqlReader.IsDBNull(7) ? (DateTime?)null : sqlReader.GetDateTime(7);

                    _listaNiveles.Add(_nivelDTO);
                }
            }
            finally
            {
                if (sqlReader != null) sqlReader.Close();
            }
            return _listaNiveles;
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

        public bool ExisteNivel(string pais, string nivel)
        {
            SqlCommand sqlComando;
            string sql = @"select COUNT(*)
                            from NivelLiquidacion
                            where Pais = @Pais AND LOWER(Nivel) = LOWER(@Nivel)";

            sqlComando = new SqlCommand(sql, _sqlConn);

            if (_sqlTran != null)
                sqlComando.Transaction = _sqlTran;
            sqlComando.Parameters.Add(new SqlParameter("@Pais", (object)pais ?? DBNull.Value));
            sqlComando.Parameters.Add(new SqlParameter("@Nivel", (object)nivel ?? DBNull.Value));

            return Convert.ToInt32(sqlComando.ExecuteScalar()) > 0 ? true : false;
        }
        #endregion
    }
}
