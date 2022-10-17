using System.Collections.Generic;
using System;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;

namespace DipCmiGT.LogicaCajasChicas.Entidad
{
    public class SuperUsuario
    {

        #region Declaraciones
        private SqlConnection _sqlConn = null;
        private SqlTransaction _sqlTran = null;

        #endregion

        #region Constructores

        public SuperUsuario(SqlConnection sqlConn)
        {
            _sqlConn = sqlConn;
        }

        public SuperUsuario(SqlConnection sqlConn, SqlTransaction sqlTran)
        {
            _sqlConn = sqlConn;
            _sqlTran = sqlTran;
        }
        #endregion

        #region Metodos

        public SuperUsuarioDTO BuscarDatosFactura(int idTipoDocumento, string Identificacion, string serie, string numero, int opcion, string usuario)
        {
            List<SuperUsuarioDTO> _listasuperUsuarioDto = null;
            SqlCommand _sqlComando = null;

            string sql;
            sql = "SP_ModificarEstados";

            _sqlComando = new SqlCommand("SP_ModificarEstados", _sqlConn);
            _sqlComando.CommandType = CommandType.StoredProcedure;

            if (_sqlTran != null)
                _sqlComando.Transaction = _sqlTran;

            _sqlComando.Parameters.Add(new SqlParameter("usuario", (object)usuario ?? DBNull.Value));
            _sqlComando.Parameters.Add(new SqlParameter("tipoDocumento", (object)idTipoDocumento ?? DBNull.Value));
            _sqlComando.Parameters.Add(new SqlParameter("NumeroIdentificacion", (object)Identificacion ?? DBNull.Value));
            _sqlComando.Parameters.Add(new SqlParameter("serie", (object)serie ?? DBNull.Value));
            _sqlComando.Parameters.Add(new SqlParameter("numero", (object)numero ?? DBNull.Value));
            _sqlComando.Parameters.Add(new SqlParameter("opcion", (object)opcion ?? DBNull.Value));

            _listasuperUsuarioDto = CargarReader(_sqlComando.ExecuteReader());
            return _listasuperUsuarioDto.Count > 0 ? _listasuperUsuarioDto[0] : null;
        }

        public Int16 ModificarEstadoFactura(Int16 estado, decimal idfactura, Int32 estadoCC, decimal idCC, string usuario, string dominio, string estadoActual, int opcion, string justificacion)
        {

            SqlCommand _sqlComando = null;

            //_sqlComando = new SqlCommand("update dbo.FacturaEncabezado set estado = @estadoFactura where IdFactura = @idFactura", _sqlConn);
            _sqlComando = new SqlCommand(@"update dbo.FacturaEncabezado set estado = @estadoFactura where IdFactura in 
                                        (select distinct b.IdFactura from dbo.FacturaEncabezado A 
                                            INNER JOIN dbo.FacturaEncabezado B 
                                            ON a.Numero = b.Numero 
                                            AND a.Serie = b.Serie
                                            AND a.IdProveedor = b.IdProveedor
                                            Where a.IdFactura = @idFactura
                                            AND B.Estado <> 2
                                            AND A.estado <> 2)", _sqlConn);

            if (_sqlTran != null)
                _sqlComando.Transaction = _sqlTran;

            //SqlCommand _sqlComando = null;

            //string sql;
            //sql = "SP_ModificarEstados";

            //_sqlComando = new SqlCommand(sql, _sqlConn);
            //_sqlComando.CommandType = CommandType.StoredProcedure;

            //if (_sqlTran != null)
            //    _sqlComando.Transaction = _sqlTran;

            _sqlComando.Parameters.Add(new SqlParameter("usuario", (object)usuario ?? DBNull.Value));
            _sqlComando.Parameters.Add(new SqlParameter("estadoFactura", (object)estado ?? DBNull.Value));
            _sqlComando.Parameters.Add(new SqlParameter("idFactura", (object)idfactura ?? DBNull.Value));
            _sqlComando.Parameters.Add(new SqlParameter("estadoCC", (object)estadoCC ?? DBNull.Value));
            _sqlComando.Parameters.Add(new SqlParameter("idcc", (object)idCC ?? DBNull.Value));
            _sqlComando.Parameters.Add(new SqlParameter("dominio", (object)dominio ?? DBNull.Value));
            _sqlComando.Parameters.Add(new SqlParameter("estadoActual", (object)estadoActual ?? DBNull.Value));
            _sqlComando.Parameters.Add(new SqlParameter("opcion", (object)opcion ?? DBNull.Value));
            _sqlComando.Parameters.Add(new SqlParameter("justificacion", (object)justificacion ?? DBNull.Value));

            return Convert.ToInt16(_sqlComando.ExecuteScalar());

            //return Convert.ToInt16(_sqlComando.ExecuteNonQuery());
        }


        protected string CargarReaderMandante(SqlDataReader sqlDataReader)
        {
            string mandante = "";
            try
            {
                sqlDataReader.Read();
                mandante = sqlDataReader.GetString(0);
                return mandante;
            }
            finally
            {
                if (sqlDataReader != null) sqlDataReader.Close();
            }
        }

        protected List<SuperUsuarioDTO> CargarReader(SqlDataReader sqlDataReader)
        {
            List<SuperUsuarioDTO> _listaSuperUsuarioDto = new List<SuperUsuarioDTO>();
            SuperUsuarioDTO _superUsuarioDto = new SuperUsuarioDTO();

            try
            {
                while (sqlDataReader.Read())
                {
                    _superUsuarioDto = new SuperUsuarioDTO();

                    _superUsuarioDto.NombreProveedor = sqlDataReader.GetString(0);
                    _superUsuarioDto.IdFactura = sqlDataReader.GetDecimal(1);
                    _superUsuarioDto.IdEstadoActual = sqlDataReader.GetInt16(2);
                    _superUsuarioDto.EstadoActual = sqlDataReader.GetString(3);
                    _superUsuarioDto.Total = sqlDataReader.GetDouble(4);
                    _superUsuarioDto.CodDividida = sqlDataReader.GetBoolean(5);
                    _superUsuarioDto.Dividida = sqlDataReader.GetString(6);
                    _superUsuarioDto.Fecha = sqlDataReader.GetString(7);
                    _superUsuarioDto.IDCCFactura = sqlDataReader.GetDecimal(8);
                    _superUsuarioDto.CCFactura = sqlDataReader.GetString(9);


                    _listaSuperUsuarioDto.Add(_superUsuarioDto);
                }

                return _listaSuperUsuarioDto;
            }
            finally
            {
                if (sqlDataReader != null) sqlDataReader.Close();
            }
        }

        public SuperUsuarioDTO BuscarDatosCC(int opcion, string sociedad, string centro, string numero, string correlativo)
        {
            List<SuperUsuarioDTO> _listasuperUsuarioDto = null;
            SqlCommand _sqlComando = null;

            string sql;
            sql = "SP_ModificarEstados";

            _sqlComando = new SqlCommand(sql, _sqlConn);
            _sqlComando.CommandType = CommandType.StoredProcedure;

            if (_sqlTran != null)
                _sqlComando.Transaction = _sqlTran;

            _sqlComando.Parameters.Add(new SqlParameter("opcion", (object)opcion ?? DBNull.Value));
            _sqlComando.Parameters.Add(new SqlParameter("sociedad", (object)sociedad ?? DBNull.Value));
            _sqlComando.Parameters.Add(new SqlParameter("centro", (object)centro ?? DBNull.Value));
            _sqlComando.Parameters.Add(new SqlParameter("numeroCC", (object)numero ?? DBNull.Value));
            _sqlComando.Parameters.Add(new SqlParameter("correlativo", (object)correlativo ?? DBNull.Value));

            _listasuperUsuarioDto = CargarReaderCC(_sqlComando.ExecuteReader());
            return _listasuperUsuarioDto.Count > 0 ? _listasuperUsuarioDto[0] : null;
        }

        protected List<SuperUsuarioDTO> CargarReaderCC(SqlDataReader sqlDataReader)
        {
            List<SuperUsuarioDTO> _listaSuperUsuarioDto = new List<SuperUsuarioDTO>();
            SuperUsuarioDTO _superUsuarioDto = new SuperUsuarioDTO();

            try
            {
                while (sqlDataReader.Read())
                {
                    _superUsuarioDto = new SuperUsuarioDTO();

                    _superUsuarioDto.IdCC = sqlDataReader.GetDecimal(0);
                    _superUsuarioDto.NombreCC = sqlDataReader.GetString(1);
                    _superUsuarioDto.IdEstadoActual = sqlDataReader.GetInt16(2);
                    _superUsuarioDto.EstadoActual = sqlDataReader.GetString(3);

                    _listaSuperUsuarioDto.Add(_superUsuarioDto);
                }

                return _listaSuperUsuarioDto;
            }
            finally
            {
                if (sqlDataReader != null) sqlDataReader.Close();
            }
        }


        public Int16 ModificarEstadoCC(Int16 opcion, string usuario, string pais, decimal IdCC, Int16 estadoActual, Int16 estadoNuevo, string justificacion)
        {
            SqlCommand _sqlComando = null;

            string sql;
            sql = "SP_ModificarEstados";

            _sqlComando = new SqlCommand(sql, _sqlConn);
            _sqlComando.CommandType = CommandType.StoredProcedure;

            if (_sqlTran != null)
                _sqlComando.Transaction = _sqlTran;

            _sqlComando.Parameters.Add(new SqlParameter("usuario", (object)usuario ?? DBNull.Value));
            _sqlComando.Parameters.Add(new SqlParameter("dominio", (object)pais ?? DBNull.Value));
            _sqlComando.Parameters.Add(new SqlParameter("opcion", (object)opcion ?? DBNull.Value));
            _sqlComando.Parameters.Add(new SqlParameter("idCC", (object)IdCC ?? DBNull.Value));
            _sqlComando.Parameters.Add(new SqlParameter("estadoCC", (object)estadoActual ?? DBNull.Value));
            _sqlComando.Parameters.Add(new SqlParameter("justificacion", (object)justificacion ?? DBNull.Value));
            _sqlComando.Parameters.Add(new SqlParameter("EstadoNuevoCC", (object)estadoNuevo ?? DBNull.Value));

            return Convert.ToInt16(_sqlComando.ExecuteNonQuery());
        }

        public string ValidarMandante(string codigoSociedad)
        {
            SqlCommand sqlComando;
            string sql = "select Mandante from Sociedad where codigosociedad = @codigoSociedad";

            sqlComando = new SqlCommand(sql, _sqlConn);
            sqlComando.CommandType = CommandType.Text;

            if (_sqlTran != null)
                sqlComando.Transaction = _sqlTran;

            sqlComando.Parameters.Add(new SqlParameter("@codigoSociedad", (object)codigoSociedad ?? DBNull.Value));

            return CargarReaderMandante(sqlComando.ExecuteReader());
        }

        public SuperUsuarioDTO BuscarDatosSincro(int opcion, string sociedad, string documento, string noFactura, string fechaFactura, string docProveedor, string serie)
        {
            List<SuperUsuarioDTO> _listasuperUsuarioDto = null;
            SqlCommand _sqlComando = null;

            string sql;
            sql = "rf.SP_AnularSincronizacionCC";

            _sqlComando = new SqlCommand(sql, _sqlConn);
            _sqlComando.CommandType = CommandType.StoredProcedure;

            if (_sqlTran != null)
                _sqlComando.Transaction = _sqlTran;

            _sqlComando.Parameters.Add(new SqlParameter("opcion", (object)opcion ?? DBNull.Value));
            _sqlComando.Parameters.Add(new SqlParameter("documento", (object)documento ?? DBNull.Value));
            _sqlComando.Parameters.Add(new SqlParameter("sociedad", (object)sociedad ?? DBNull.Value));
            _sqlComando.Parameters.Add(new SqlParameter("noFactura", (object)noFactura ?? DBNull.Value));
            _sqlComando.Parameters.Add(new SqlParameter("fechaFact", (object)fechaFactura ?? DBNull.Value));
            _sqlComando.Parameters.Add(new SqlParameter("docProveedor", (object)docProveedor ?? DBNull.Value));
            _sqlComando.Parameters.Add(new SqlParameter("serie", (object)serie ?? DBNull.Value));

            _listasuperUsuarioDto = CargarReaderSincro(_sqlComando.ExecuteReader());
            return _listasuperUsuarioDto.Count > 0 ? _listasuperUsuarioDto[0] : null;
        }


        protected List<SuperUsuarioDTO> CargarReaderSincro(SqlDataReader sqlDataReader)
        {
            List<SuperUsuarioDTO> _listaSuperUsuarioDto = new List<SuperUsuarioDTO>();
            SuperUsuarioDTO _superUsuarioDto = new SuperUsuarioDTO();

            try
            {
                while (sqlDataReader.Read())
                {
                    _superUsuarioDto = new SuperUsuarioDTO();

                    _superUsuarioDto.CodigoSincronizacion = sqlDataReader.GetInt64(0);
                    _superUsuarioDto.NombreCC = sqlDataReader.GetString(1);
                    _superUsuarioDto.DocIdentificacion = sqlDataReader.GetString(2);
                    _superUsuarioDto.NombreProveedor = sqlDataReader.GetString(3);
                    _superUsuarioDto.NoFactura = sqlDataReader.GetString(4);
                    _superUsuarioDto.IdEstadoSicnro = sqlDataReader.GetString(5);
                    _superUsuarioDto.EstadoActual = sqlDataReader.GetString(6);
                    _superUsuarioDto.Serie = sqlDataReader.GetString(7);


                    _listaSuperUsuarioDto.Add(_superUsuarioDto);
                }

                return _listaSuperUsuarioDto;
            }
            finally
            {
                if (sqlDataReader != null) sqlDataReader.Close();
            }
        }

        public short ModificarEstadosFacturas(string listaIds, string usuario)
        {
            SqlCommand sqlComando;
            string sql = @"update dbo.facturaencabezado set estado = 0, UsuarioModifico = @Usuario, 
                           FechaModificacion = getdate() where idfactura in (" + listaIds + ")"; //@listaIds) ";

            sqlComando = new SqlCommand(sql, _sqlConn);
            sqlComando.CommandType = CommandType.Text;

            if (_sqlTran != null)
                sqlComando.Transaction = _sqlTran;

            sqlComando.Parameters.Add(new SqlParameter("@listaIds", (object)listaIds ?? DBNull.Value));
            sqlComando.Parameters.Add(new SqlParameter("@Usuario", (object)usuario ?? DBNull.Value));
            return Convert.ToInt16(sqlComando.ExecuteNonQuery());
        }

        public List<decimal> GetIdFacturasAnular(string noFactura, string documentoProveedor, string serie)
        {
            List<decimal> idsFacturas = new List<decimal>();
            SqlCommand sqlComando;
            string sql = @"select IdFactura from dbo.facturaencabezado a 
                          inner join dbo.proveedor b on a.idproveedor = b.idproveedor  
                          where numero = @noFactura and b.NumeroIdentificacion = @docProveedor
                            and serie = @Serie";

            sqlComando = new SqlCommand(sql, _sqlConn);
            sqlComando.CommandType = CommandType.Text;

            if (_sqlTran != null)
                sqlComando.Transaction = _sqlTran;

            sqlComando.Parameters.Add(new SqlParameter("@noFactura", (object)noFactura.ToString() ?? DBNull.Value));
            sqlComando.Parameters.Add(new SqlParameter("@docProveedor", (object)documentoProveedor ?? DBNull.Value));
            sqlComando.Parameters.Add(new SqlParameter("@Serie", (object)serie ?? DBNull.Value));
            SqlDataReader dataReader = sqlComando.ExecuteReader();

            try
            {
                while (dataReader.Read())
                {
                    idsFacturas.Add(dataReader.GetDecimal(0));
                }

                return idsFacturas;
            }
            finally
            {
                if (dataReader != null) dataReader.Close();
            }
        }


        public Int16 AnularSincronizacion(int opcion, string sociedad, string documento)
        {
            SqlCommand _sqlComando = null;

            string sql;
            sql = "rf.SP_AnularSincronizacionCC";

            _sqlComando = new SqlCommand(sql, _sqlConn);
            _sqlComando.CommandType = CommandType.StoredProcedure;

            if (_sqlTran != null)
                _sqlComando.Transaction = _sqlTran;

            _sqlComando.Parameters.Add(new SqlParameter("opcion", (object)opcion ?? DBNull.Value));
            _sqlComando.Parameters.Add(new SqlParameter("sociedad", (object)sociedad ?? DBNull.Value));
            _sqlComando.Parameters.Add(new SqlParameter("documento", (object)documento ?? DBNull.Value));

            return Convert.ToInt16(_sqlComando.ExecuteNonQuery());
        }

        public bool ValidaPermisosAnulacion(string usuario, string codigoSociedad)
        {
            SqlCommand sqlComando;
            string sql = @"select TOP 1 * from [dbo].[MapeoUsuarioCECOCuenta] WHERE BUKRS = @codigoSociedad 
                            AND USUARIO = @usuario ";

            sqlComando = new SqlCommand(sql, _sqlConn);
            sqlComando.CommandType = CommandType.Text;

            if (_sqlTran != null)
                sqlComando.Transaction = _sqlTran;

            sqlComando.Parameters.Add(new SqlParameter("@codigoSociedad", (object)codigoSociedad ?? DBNull.Value));
            sqlComando.Parameters.Add(new SqlParameter("@usuario", (object)usuario ?? DBNull.Value));
            SqlDataReader dataReader = sqlComando.ExecuteReader();
            return dataReader.HasRows;
        }

        public Int16 AnularSincronizacionLog(int opcion, string sociedad, string documento, string usuario, string pais, string justificacion)
        {
            SqlCommand _sqlComando = null;

            string sql;
            sql = "SP_ModificarEstados";

            _sqlComando = new SqlCommand(sql, _sqlConn);
            _sqlComando.CommandType = CommandType.StoredProcedure;

            if (_sqlTran != null)
                _sqlComando.Transaction = _sqlTran;

            _sqlComando.Parameters.Add(new SqlParameter("usuario", (object)usuario ?? DBNull.Value));
            _sqlComando.Parameters.Add(new SqlParameter("dominio", (object)pais ?? DBNull.Value));
            _sqlComando.Parameters.Add(new SqlParameter("opcion", (object)opcion ?? DBNull.Value));
            _sqlComando.Parameters.Add(new SqlParameter("sociedad", (object)sociedad ?? DBNull.Value));
            _sqlComando.Parameters.Add(new SqlParameter("documento", (object)documento ?? DBNull.Value));
            _sqlComando.Parameters.Add(new SqlParameter("justificacion", (object)justificacion ?? DBNull.Value));

            return Convert.ToInt16(_sqlComando.ExecuteNonQuery());
        }
        #endregion
    }
}
