using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using DipCmiGT.LogicaComun;

namespace DipCmiGT.LogicaCajasChicas.Entidad
{
    public class ImpuestoCH
    {
        #region Declaraciones
        private SqlConnection _sqlConn = null;
        private SqlTransaction _sqlTran = null;

        protected string sqlSelect = @"SELECT CodigoImpuesto, Pais, Tipo, Descripcion, Valor, Cuenta, OrdenVisualizacion, Alta, 
                                        UsuarioCreacion, FechaCreacion, UsuarioModificacion, FechaModificacion
                                        FROM ImpuestoCH";

        protected string sqlInsert = @"INSERT INTO ImpuestoCH (CodigoImpuesto, Pais, Tipo, Descripcion, Valor, Cuenta, OrdenVisualizacion, Alta, UsuarioCreacion, FechaCreacion)
                                    VALUES (@CodigoImpuesto, @Pais, @Tipo, @Descripcion, @Valor ,@Cuenta, @OrdenVisualizacion, @Alta, @UsuarioCreacion, getdate()) ";

        protected string sqlUpdate = @"UPDATE ImpuestoCH SET CodigoImpuesto = @CodigoImpuesto, Pais = @Pais, Tipo = @Tipo, Descripcion = @Descripcion, 
                                    Valor = @Valor,  Cuenta = @Cuenta, OrdenVisualizacion = @OrdenVisualizacion, Alta = @Alta,
                                    UsuarioModificacion = @UsuarioModificacion, FechaModificacion = getdate() 
                                    WHERE CodigoImpuesto = @CodigoImpuesto";
        #endregion

        #region Constructores
        public ImpuestoCH(SqlConnection sqlConn)
        {
            _sqlConn = sqlConn;
        }

        public ImpuestoCH(SqlConnection sqlConn, SqlTransaction sqlTran)
        {
            _sqlConn = sqlConn;
            _sqlTran = sqlTran;
        }
        #endregion

        #region Metodos
        public Int32 EjecutarSentenciaInsert(ImpuestoCHDTO impuestoDTO)
        {
            SqlCommand sqlComando;

            sqlComando = new SqlCommand(sqlInsert, _sqlConn);
            sqlComando.CommandType = CommandType.Text;

            if (_sqlTran != null)
                sqlComando.Transaction = _sqlTran;

            sqlComando.Parameters.Add(new SqlParameter("@CodigoImpuesto", (object)impuestoDTO.CodigoImpuesto ?? DBNull.Value));
            sqlComando.Parameters.Add(new SqlParameter("@Pais", (object)impuestoDTO.Pais.ToUpper() ?? DBNull.Value));
            sqlComando.Parameters.Add(new SqlParameter("@Tipo", (object)impuestoDTO.Tipo ?? DBNull.Value));
            sqlComando.Parameters.Add(new SqlParameter("@Descripcion", (object)impuestoDTO.Descripcion.ToUpper() ?? DBNull.Value));
            sqlComando.Parameters.Add(new SqlParameter("@Valor", (object)impuestoDTO.Valor ?? DBNull.Value));
            sqlComando.Parameters.Add(new SqlParameter("@Cuenta", (object)impuestoDTO.Cuenta ?? DBNull.Value));
            sqlComando.Parameters.Add(new SqlParameter("@OrdenVisualizacion", (object)impuestoDTO.OrdenVisualizacion ?? DBNull.Value));
            sqlComando.Parameters.Add(new SqlParameter("@UsuarioCreacion", (object)impuestoDTO.USUARIO_MANTENIMIENTO.USUARIO_ALTA.ToUpper() ?? DBNull.Value));

            return Convert.ToInt32(sqlComando.ExecuteScalar());

        }

        public Int32 EjectuarSentenciaUpdate(ImpuestoCHDTO impuestoDTO)
        {
            SqlCommand sqlComando;

            sqlComando = new SqlCommand(sqlUpdate, _sqlConn);
            sqlComando.CommandType = CommandType.Text;

            if (_sqlTran != null)
                sqlComando.Transaction = _sqlTran;

            sqlComando.Parameters.Add(new SqlParameter("@CodigoImpuesto", (object)impuestoDTO.CodigoImpuesto ?? DBNull.Value));
            sqlComando.Parameters.Add(new SqlParameter("@Pais", (object)impuestoDTO.Pais ?? DBNull.Value));
            sqlComando.Parameters.Add(new SqlParameter("@Tipo", (object)impuestoDTO.Tipo ?? DBNull.Value));
            sqlComando.Parameters.Add(new SqlParameter("@Descripcion", (object)impuestoDTO.Descripcion ?? DBNull.Value));
            sqlComando.Parameters.Add(new SqlParameter("@Valor", (object)impuestoDTO.Valor ?? DBNull.Value));
            sqlComando.Parameters.Add(new SqlParameter("@Cuenta", (object)impuestoDTO.Cuenta ?? DBNull.Value));
            sqlComando.Parameters.Add(new SqlParameter("@OrdenVisualizacion", (object)impuestoDTO.OrdenVisualizacion ?? DBNull.Value));
            sqlComando.Parameters.Add(new SqlParameter("@Alta", (object)impuestoDTO.Alta ?? DBNull.Value));
            sqlComando.Parameters.Add(new SqlParameter("@UsuarioModificacion", (object)impuestoDTO.USUARIO_MANTENIMIENTO.USUARIO_MODIFICO.ToUpper() ?? DBNull.Value));

            return Convert.ToInt32(sqlComando.ExecuteScalar());
        }

        protected List<ImpuestoCHDTO> CargarReader(SqlDataReader sqlReader)
        {
            ImpuestoCHDTO _impuestoDto = null;
            List<ImpuestoCHDTO> _listaImpuestos = new List<ImpuestoCHDTO>();

            try
            {
                while (sqlReader.Read())
                {
                    _impuestoDto = new ImpuestoCHDTO();


                    _impuestoDto.CodigoImpuesto = sqlReader.GetInt64(0);
                    _impuestoDto.Pais = sqlReader.GetString(1);
                    _impuestoDto.Tipo = sqlReader.GetString(2);
                    _impuestoDto.Descripcion = sqlReader.GetString(3);
                    _impuestoDto.Valor = sqlReader.GetDecimal(4);
                    _impuestoDto.Cuenta = sqlReader.GetString(5);
                    _impuestoDto.OrdenVisualizacion = sqlReader.GetInt32(6);
                    _impuestoDto.Alta = sqlReader.GetBoolean(7);
                    _impuestoDto.USUARIO_MANTENIMIENTO.USUARIO_ALTA = sqlReader.GetString(8);
                    _impuestoDto.USUARIO_MANTENIMIENTO.FECHA_ALTA = sqlReader.GetDateTime(9);
                    _impuestoDto.USUARIO_MANTENIMIENTO.USUARIO_MODIFICO = sqlReader.IsDBNull(10) ? null : sqlReader.GetString(10);
                    _impuestoDto.USUARIO_MANTENIMIENTO.FECHA_MODIFICACION = sqlReader.IsDBNull(11) ? (DateTime?)null : sqlReader.GetDateTime(11);

                    _listaImpuestos.Add(_impuestoDto);
                }
            }
            finally
            {
                if (sqlReader != null) sqlReader.Close();
            }
            return _listaImpuestos;
        }

        public List<ImpuestoCHDTO> ListaImpuestos()
        {
            SqlCommand sqlComando;
            string sql = sqlSelect;

            sqlComando = new SqlCommand(sql, _sqlConn);
            if (_sqlTran != null)
                sqlComando.Transaction = _sqlTran;

            return CargarReader(sqlComando.ExecuteReader());
        }

        public Int16 DarBajaImpuesto(Int32 CodigoImpuesto, string usuario)
        {
            SqlCommand sqlComando;
            string sql = @"Update ImpuestoCH set Alta = 0, UsuarioModificacion = @UsuarioModificacion, FechaModificacion = getdate()
                            where CodigoImpuesto = @CodigoImpuesto";

            sqlComando = new SqlCommand(sql, _sqlConn);
            sqlComando.CommandType = CommandType.Text;

            if (_sqlTran != null)
                sqlComando.Transaction = _sqlTran;

            sqlComando.Parameters.Add(new SqlParameter("@UsuarioModificacion", (object)usuario.ToUpper() ?? DBNull.Value));
            sqlComando.Parameters.Add(new SqlParameter("@CodigoImpuesto", (object)CodigoImpuesto ?? DBNull.Value));

            return Convert.ToInt16(sqlComando.ExecuteNonQuery());
        }

        public Int16 DarAltaImpuesto(Int32 codigoImpuesto, string usuario)
        {
            SqlCommand sqlComando;
            string sql = @"Update ImpuestoCH set Alta = 1, UsuarioModificacion = @UsuarioModificacion, FechaModificacion = getdate()
                            where CodigoImpuesto = @CodigoImpuesto";

            sqlComando = new SqlCommand(sql, _sqlConn);
            sqlComando.CommandType = CommandType.Text;

            if (_sqlTran != null)
                sqlComando.Transaction = _sqlTran;

            sqlComando.Parameters.Add(new SqlParameter("@UsuarioModificacion", (object)usuario.ToUpper() ?? DBNull.Value));
            sqlComando.Parameters.Add(new SqlParameter("@CodigoImpuesto", (object)codigoImpuesto ?? DBNull.Value));

            return Convert.ToInt16(sqlComando.ExecuteNonQuery());

        }

        public bool ExisteImpuesto(string pais, string descripcion)
        {
            SqlCommand sqlComando;
            string sql = @"SELECT COUNT(*)
                            FROM ImpuestoCH
                            WHERE Pais = @Pais AND Descripcion = @Descripcion";

            sqlComando = new SqlCommand(sql, _sqlConn);

            if (_sqlTran != null)
                sqlComando.Transaction = _sqlTran;
            sqlComando.Parameters.Add(new SqlParameter("@Pais", (object)pais ?? DBNull.Value));
            sqlComando.Parameters.Add(new SqlParameter("@Descripcion", (object)descripcion ?? DBNull.Value));
            return Convert.ToInt32(sqlComando.ExecuteScalar()) > 0 ? true : false;
        }
        #endregion
    }
}
