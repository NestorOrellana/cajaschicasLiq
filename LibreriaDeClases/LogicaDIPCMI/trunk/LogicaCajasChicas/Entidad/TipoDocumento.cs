using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using DipCmiGT.LogicaCajasChicas;
using System.Data;
using LogicaCajasChicas;

namespace DipCmiGT.LogicaCajasChicas.Entidad
{
    public class TipoDocumento
    {

        #region Declaraciones

        private SqlConnection _sqlConn = null;
        private SqlTransaction _sqlTran = null;
        protected string sqlSelect = @"SELECT a.IdTipoDocumento, a.TipoDocumento, a.Descripcion, a.Alta, a.UsuarioAlta, a.FechaCreacion, 
                                              a.UsuarioModificacion, a.FechaModificacion
                                        FROM TipoDocumento AS a
										INNER JOIN Sociedad AS b ON b.Pais = a.Pais
										INNER JOIN SociedadCentro AS c ON c.CodigoSociedad = b.CodigoSociedad";

        #endregion

        #region TipoDocumento

        public TipoDocumento(SqlConnection sqlConn)
        {
            _sqlConn = sqlConn;
        }

        public TipoDocumento(SqlConnection sqlConn, SqlTransaction sqlTran)
        {
            _sqlConn = sqlConn;
            _sqlTran = sqlTran;
        }

        #endregion

        #region Metodos

        /// <summary>
        /// Consulta registro tipo documento.
        /// </summary>
        /// <param name="idTipoProveedor">Identificador único</param>
        /// <returns>TipoDocumentoDTO</returns>
        public TipoDocumentoDTO EjecutarSentenciaSelect(Int32 idTipoProveedor)
        {
            List<TipoDocumentoDTO> _listaTipoDocumento = null;
            SqlCommand sqlComando;
            string sql = sqlSelect + " WHERE a.IdTipoDocumento = @IdTipoDocumento ";

            sqlComando = new SqlCommand(sql, _sqlConn);
            sqlComando.CommandType = CommandType.Text;

            if (_sqlTran != null)
                sqlComando.Transaction = _sqlTran;

            sqlComando.Parameters.Add(new SqlParameter("@IdTipoDocumento", (object)idTipoProveedor ?? DBNull.Value));

            _listaTipoDocumento = CargarReader(sqlComando.ExecuteReader());

            return _listaTipoDocumento.Count > 0 ? _listaTipoDocumento[0] : null;
        }

        /// <summary>
        /// Encapsula objeto tipo documento.
        /// </summary>
        /// <param name="sqlReader">SqlReader</param>
        /// <returns>Lista TipoDocumentoDTO</returns>
        protected List<TipoDocumentoDTO> CargarReader(SqlDataReader sqlReader)
        {
            TipoDocumentoDTO _tipoDocumento = null;
            List<TipoDocumentoDTO> _listaTipoDocumento = new List<TipoDocumentoDTO>();

            try
            {
                while (sqlReader.Read())
                {
                    _tipoDocumento = new TipoDocumentoDTO();

                    _tipoDocumento.ID_TIPO_DOCUMENTO = sqlReader.GetInt16(0);
                    _tipoDocumento.TIPO_DOCUMENTO = sqlReader.IsDBNull(1) ? null : sqlReader.GetString(1);
                    _tipoDocumento.DESCRIPCION = sqlReader.GetString(2);
                    _tipoDocumento.ALTA = sqlReader.GetBoolean(3);
                    _tipoDocumento.USUARIO_MANTENIMIENTO.USUARIO_ALTA = sqlReader.GetString(4);
                    _tipoDocumento.USUARIO_MANTENIMIENTO.FECHA_ALTA = sqlReader.GetDateTime(5);
                    _tipoDocumento.USUARIO_MANTENIMIENTO.USUARIO_MODIFICO = sqlReader.IsDBNull(6) ? null : sqlReader.GetString(6);
                    _tipoDocumento.USUARIO_MANTENIMIENTO.FECHA_MODIFICACION = sqlReader.IsDBNull(7) ? (DateTime?)null : sqlReader.GetDateTime(7);

                    _listaTipoDocumento.Add(_tipoDocumento);
                }
            }
            finally
            {
                if (sqlReader != null) sqlReader.Close();
            }

            return _listaTipoDocumento;
        }

        /// <summary>
        /// Lista tipo documentos activos.
        /// </summary>
        /// <returns>Lista TipoDocumentoDTO</returns>
        public List<TipoDocumentoDTO> ListaTipoDocumentoActivo(int IdSociedadCentro)
        {
            SqlCommand sqlComando;
            string sql = sqlSelect + " Where a.Alta = 1 and c.IdSociedadCentro = @IdSociedadCentro";

            sqlComando = new SqlCommand(sql, _sqlConn);
            sqlComando.CommandType = CommandType.Text;

            if (_sqlTran != null)
                sqlComando.Transaction = _sqlTran;
            sqlComando.Parameters.Add(new SqlParameter("@IdSociedadCentro", (object)IdSociedadCentro ?? DBNull.Value));

            return CargarReader(sqlComando.ExecuteReader());
        }


        public List<TipoDocumentoDTO> ListaTipoDocumentoPais(string dominio)
        {
            SqlCommand sqlComando;
            string sql = @"SELECT a.IdTipoDocumento, TipoDocumento, Descripcion, a.Alta, a.UsuarioAlta, 
                                  a.FechaCreacion, a.UsuarioModificacion, a.FechaModificacion
                                        FROM TipoDocumento AS a
										INNER JOIN Dominio AS b ON b.Identificador = a.pais
										WHERE a.Alta = 1 and b.Nombre = @dominio
                                        AND a.TipoDocumento <> 'Z6'";

            sqlComando = new SqlCommand(sql, _sqlConn);
            sqlComando.CommandType = CommandType.Text;

            if (_sqlTran != null)
                sqlComando.Transaction = _sqlTran;
            sqlComando.Parameters.Add(new SqlParameter("@dominio", (object)dominio ?? DBNull.Value));

            return CargarReader(sqlComando.ExecuteReader());
        }

        #endregion
    }
}
