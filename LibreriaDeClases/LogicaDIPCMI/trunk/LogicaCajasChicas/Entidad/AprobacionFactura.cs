using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace DipCmiGT.LogicaCajasChicas.Entidad
{
    public class AprobacionFactura
    {
        //preuba
        #region Declaraciones
        private SqlConnection _sqlConn = null;
        private SqlTransaction _sqlTran = null;

        protected string sqlSelect = @"Select distinct(a.IdFactura), a.Numero, a.Serie, a.CentroCosto, a.OrdenesCosto,
                                        a.Aprobada, a.EsEspecial, a.Estado, a.FechaFactura, a.IVA, a.IdCajaChica, b.Descripcion, a.IdProveedor,
                                        a.NumeroRetencionISR, a.NumeroRetencionIVA, a.RetencionISR, a.RetencionIVA, 
                                        a.TipoFactura, a.ValorRetencionISR, a.ValorRetencionIVA, a.ValorTotal, c.IdSociedadCentro, d.IdAprobadorCentro,
                                        a.Nivel, e.Usuario Aprobador, '', '', '', ''
                                        from  FacturaEncabezado a
                                        inner join CajaChicaEncabezado b on a.IdCajaChica = b.IdCajaChica and b.Estado = 1
                                        inner join SociedadCentro c on c.IdSociedadCentro = b.IdSociedadCentro and c.Alta = 1
                                        inner join AprobadorCentro d on d.IdSociedadCentro = c.IdSociedadCentro           
                                                              and ((d.KOSTL = a.CentroCosto) and (d.AUFNR = a.OrdenesCosto))
                                                              and d.Alta = 1 and d.IdNivel = a.Nivel                                        
                                        inner join Usuario e on e.IdUsuario = d.IdUsuario and e.Alta = 1 and e.Usuario = @Usuario
                                        where (a.Aprobada is null or a.Aprobada = 'false')";





        //                                        @"Select distinct(a.IdFactura), a.Numero, a.Serie, a.CentroCosto, a.OrdenesCosto,
        //                                        a.Aprobada, a.EsEspecial, a.Estado, a.FechaFactura, a.IVA, a.IdCajaChica, b.Descripcion, a.IdProveedor,
        //                                        a.NumeroRetencionISR, a.NumeroRetencionIVA, a.RetencionISR, a.RetencionIVA, 
        //                                        a.TipoFactura, a.ValorRetencionISR, a.ValorRetencionIVA, a.ValorTotal, c.IdSociedadCentro, d.IdAprobacionEncabezado,
        //                                        a.Nivel, f.Usuario Aprobador, d.IdAprobadorCentro
        //                                        from  FacturaEncabezado a
        //                                        inner join CajaChicaEncabezado b on a.IdCajaChica = b.IdCajaChica and b.Estado = 1
        //                                        inner join SociedadCentro c on c.IdSociedadCentro = b.IdSociedadCentro and c.Alta = 1
        //                                        inner join AprobadorCentro d on d.IdSociedadCentro = c.IdSociedadCentro           
        //                                                                                 and ((d.KOSTL = a.CentroCosto) and (d.AUFNR = a.OrdenesCosto))
        //                                                                                 and d.Alta = 1 and d.IdNivel = a.Nivel                                        
        //                                        inner join AprobacionCentroEncabezado e on e.IdAprobacionEncabezado = d.IdAprobacionEncabezado and e.Alta = 1
        //                                        inner join Usuario f on f.IdUsuario = e.IdUsuario and f.Alta = 1 and f.Usuario = @Usuario
        //                                        where (a.Aprobada is null or a.Aprobada = 'false')";

        protected string sqlInsert = @"INSERT INTO AprobacionFacturas (IdFactura, IdAprobadorCentro, Estado, UsuarioAlta)
                                        VALUES (@IdFactura, @IdAprobadorCentro, @Estado, @UsuarioAlta)";

        protected string sqlUpdate = @"UPDATE AprobacionFacturas SET IdAprobadorCentro = @IdAprobadorCentro, Estado = @Estado, 
                                       UsuarioModificacion = @UsuarioModificacion, FechaModificacion = getdate()
                                       WHERE IdFactura = @IdFactura";
        #endregion

        #region Constructores
        public AprobacionFactura(SqlConnection sqlConn)
        {
            _sqlConn = sqlConn;
        }

        public AprobacionFactura(SqlConnection sqlConn, SqlTransaction sqlTran)
        {
            _sqlConn = sqlConn;
            _sqlTran = sqlTran;
        }


        #endregion

        #region Metodos


        public List<FacturaEncabezadoDTO> BuscarFacturas(string usuario, string codSociedad, Int32 idSociedadCentro, Int16 Nivel, string CentroCosto, string OrdenCosto, string estado, string dominio, string registrador)
        {
            SqlCommand sqlComando;
            string sql = sqlSelect;

            
            if (estado == "3")
            {
                sqlComando = new SqlCommand("BuscarFacturasAprobar", _sqlConn);
                sqlComando.Parameters.Add(new SqlParameter("@dominio", (object)dominio ?? DBNull.Value));
            }
            else
            {
                sqlComando = new SqlCommand("BuscarFacturasRevisadasAprobador", _sqlConn);
                sqlComando.Parameters.Add(new SqlParameter("@Aprobada", (object)estado ?? DBNull.Value));
            }
            sqlComando.CommandType = CommandType.StoredProcedure;


            if (_sqlTran != null)
                sqlComando.Transaction = _sqlTran;

            sqlComando.Parameters.Add(new SqlParameter("@usuario", (object)usuario ?? DBNull.Value));
            sqlComando.Parameters.Add(new SqlParameter("@CodigoSociedad", (object)codSociedad ?? DBNull.Value));
            sqlComando.Parameters.Add(new SqlParameter("@IdSociedadCentro", (object)idSociedadCentro ?? DBNull.Value));
            sqlComando.Parameters.Add(new SqlParameter("@nivel", (object)Nivel ?? DBNull.Value));
            sqlComando.Parameters.Add(new SqlParameter("@centroCosto", (object)CentroCosto ?? DBNull.Value));
            sqlComando.Parameters.Add(new SqlParameter("@ordenCosto", (object)OrdenCosto ?? DBNull.Value));
           // sqlComando.Parameters.Add(new SqlParameter("@registrador", (object)registrador ?? DBNull.Value));
            //sqlComando.Parameters.Add(new SqlParameter("@dominio", (object)dominio ?? DBNull.Value));


            return CargarReader(sqlComando.ExecuteReader());
        }

        public List<FacturaEncabezadoDTO> BuscarFacturaAprobar(string usuario, Int64 idFactura, string dominio)
        {
            SqlCommand sqlComando;
            string sql = sqlSelect;


            sqlComando = new SqlCommand("BuscarFacturaAprobar", _sqlConn);
            sqlComando.CommandType = CommandType.StoredProcedure;


            if (_sqlTran != null)
                sqlComando.Transaction = _sqlTran;

            sqlComando.Parameters.Add(new SqlParameter("@usuario", (object)usuario ?? DBNull.Value));
            sqlComando.Parameters.Add(new SqlParameter("@idFactura", (object)idFactura ?? DBNull.Value));
           // sqlComando.Parameters.Add(new SqlParameter("@dominio", (object)dominio ?? DBNull.Value));

            return CargarReaderAprobacionFactura(sqlComando.ExecuteReader());
        }

        protected List<FacturaEncabezadoDTO> CargarReaderAprobacionFactura(SqlDataReader sqlReader)
        {
            FacturaEncabezadoDTO _facturaencabezadoDto = null;
            List<FacturaEncabezadoDTO> _listaFacturaEncabezado = new List<FacturaEncabezadoDTO>();

            while (sqlReader.Read())
            {
                _facturaencabezadoDto = new FacturaEncabezadoDTO();

                _facturaencabezadoDto.ID_FACTURA = sqlReader.GetDecimal(0);
                _facturaencabezadoDto.NUMERO = sqlReader.GetString(1); //sqlReader.GetDecimal(1);
                _facturaencabezadoDto.SERIE = sqlReader.GetString(2);
                _facturaencabezadoDto.CENTRO_COSTO = sqlReader.GetString(3);
                _facturaencabezadoDto.ORDEN_COSTO = sqlReader.GetString(4);
                _facturaencabezadoDto.APROBADA = sqlReader.IsDBNull(5) ? (bool?)false : sqlReader.GetBoolean(5);
                _facturaencabezadoDto.FECHA_FACTURA = sqlReader.GetDateTime(6);
                _facturaencabezadoDto.IVA = sqlReader.GetDouble(7);
                _facturaencabezadoDto.ID_CAJA_CHICA = sqlReader.GetDecimal(8);
                _facturaencabezadoDto.CAJA_CHICA.DESCRIPCION = sqlReader.GetString(9);
                _facturaencabezadoDto.ID_PROVEEDOR = sqlReader.GetInt32(10);
                _facturaencabezadoDto.TIPO_FACTURA = sqlReader.GetString(11);
                _facturaencabezadoDto.VALOR_TOTAL = sqlReader.GetDouble(12);
                _facturaencabezadoDto.DETALLE_APROBACION.ID_SOCIEDAD_CENTRO = sqlReader.GetInt32(13);
                _facturaencabezadoDto.DETALLE_APROBACION.ID_APROBDOR_CENTRO = sqlReader.GetInt32(14);
                _facturaencabezadoDto.NIVEL = sqlReader.GetInt16(15);
                _facturaencabezadoDto.DETALLE_APROBACION.APROBADOR = sqlReader.GetString(16);

                _facturaencabezadoDto.NOMBRE_PROVEEDOR = sqlReader.GetString(17);
                _facturaencabezadoDto.NOMBRE_CENTRO_COSTO = sqlReader.IsDBNull(18) ? string.Empty : sqlReader.GetString(18);
                _facturaencabezadoDto.NOMBRE_ORDEN_COSTO = sqlReader.IsDBNull(19) ? string.Empty : sqlReader.GetString(19);
                _facturaencabezadoDto.USUARIO_MANTENIMIENTO.USUARIO_ALTA = sqlReader.IsDBNull(20) ? string.Empty : sqlReader.GetString(20);
                _facturaencabezadoDto.USUARIO_MANTENIMIENTO.FECHA_ALTA = sqlReader.GetDateTime(21);
                _facturaencabezadoDto.DESCRIPCION = sqlReader.IsDBNull(22) ? string.Empty : sqlReader.GetString(22);
                _facturaencabezadoDto.MONEDA = sqlReader.GetString(23);
               // _facturaencabezadoDto.IMPUESTO = sqlReader.GetDouble(24);

                _listaFacturaEncabezado.Add(_facturaencabezadoDto);
            }

            sqlReader.Close();
            return _listaFacturaEncabezado;

        }

        protected List<FacturaEncabezadoDTO> CargarReader(SqlDataReader sqlReader)
        {
            FacturaEncabezadoDTO _facturaencabezadoDto = null;
            List<FacturaEncabezadoDTO> _listaFacturaEncabezado = new List<FacturaEncabezadoDTO>();

            while (sqlReader.Read())
            {
                _facturaencabezadoDto = new FacturaEncabezadoDTO();

                _facturaencabezadoDto.ID_FACTURA = sqlReader.GetDecimal(0);
                _facturaencabezadoDto.NUMERO = sqlReader.GetString(1); //sqlReader.GetDecimal(1);
                _facturaencabezadoDto.SERIE = sqlReader.GetString(2);
                _facturaencabezadoDto.CENTRO_COSTO = sqlReader.GetString(3);
                _facturaencabezadoDto.ORDEN_COSTO = sqlReader.GetString(4);
                _facturaencabezadoDto.APROBADA = sqlReader.IsDBNull(5) ? (bool?)false : sqlReader.GetBoolean(5);
                _facturaencabezadoDto.FECHA_FACTURA = sqlReader.GetDateTime(6);
                _facturaencabezadoDto.IVA = sqlReader.GetDouble(7);
                _facturaencabezadoDto.ID_CAJA_CHICA = sqlReader.GetDecimal(8);
                _facturaencabezadoDto.CAJA_CHICA.DESCRIPCION = sqlReader.GetString(9);
                _facturaencabezadoDto.ID_PROVEEDOR = sqlReader.GetInt32(10);
                _facturaencabezadoDto.TIPO_FACTURA = sqlReader.GetString(11);
                _facturaencabezadoDto.VALOR_TOTAL = sqlReader.GetDouble(12);
                _facturaencabezadoDto.DETALLE_APROBACION.ID_SOCIEDAD_CENTRO = sqlReader.GetInt32(13);
                _facturaencabezadoDto.DETALLE_APROBACION.ID_APROBDOR_CENTRO = sqlReader.GetInt32(14);
                _facturaencabezadoDto.NIVEL = sqlReader.GetInt16(15);
                _facturaencabezadoDto.DETALLE_APROBACION.APROBADOR = sqlReader.GetString(16);

                _facturaencabezadoDto.NOMBRE_PROVEEDOR = sqlReader.GetString(17);
                _facturaencabezadoDto.NOMBRE_CENTRO_COSTO = sqlReader.IsDBNull(18) ? string.Empty : sqlReader.GetString(18);
                _facturaencabezadoDto.NOMBRE_ORDEN_COSTO = sqlReader.IsDBNull(19) ? string.Empty : sqlReader.GetString(19);
                _facturaencabezadoDto.USUARIO_MANTENIMIENTO.USUARIO_ALTA = sqlReader.IsDBNull(20) ? string.Empty : sqlReader.GetString(20);
                _facturaencabezadoDto.USUARIO_MANTENIMIENTO.FECHA_ALTA = sqlReader.GetDateTime(21);
                _facturaencabezadoDto.DESCRIPCION = sqlReader.IsDBNull(22) ? string.Empty : sqlReader.GetString(22);
                _facturaencabezadoDto.MONEDA = sqlReader.GetString(23);
                _facturaencabezadoDto.IMPUESTO = sqlReader.GetDouble(24);

                _listaFacturaEncabezado.Add(_facturaencabezadoDto);
            }

            sqlReader.Close();
            return _listaFacturaEncabezado;

        }

        public List<FacturaEncabezadoDTO> ListaEncabezadoFactura(string Usuario, string dominio)
        {

            SqlCommand sqlComando;
            string sql = sqlSelect;

            sqlComando = new SqlCommand("BuscarFacturasAprobar", _sqlConn);
            sqlComando.CommandType = CommandType.StoredProcedure;


            if (_sqlTran != null)
                sqlComando.Transaction = _sqlTran;

            sqlComando.Parameters.Add(new SqlParameter("@Usuario", (object)Usuario ?? DBNull.Value));
            sqlComando.Parameters.Add(new SqlParameter("@dominio", (object)dominio ?? DBNull.Value));
            return CargarReader(sqlComando.ExecuteReader());

        }

        public Int32 InsertaRegistroAprobacionFactura(AprobacionFacturasDTO aprobacionFacturaDto)
        {
            SqlCommand sqlCommando = new SqlCommand(sqlInsert, _sqlConn);
            sqlCommando.CommandType = CommandType.Text;

            if (_sqlTran != null)
                sqlCommando.Transaction = _sqlTran;

            sqlCommando.Parameters.Add(new SqlParameter("@IdFactura", (object)aprobacionFacturaDto.ID_FACTURA ?? DBNull.Value));
            sqlCommando.Parameters.Add(new SqlParameter("@IdAprobadorCentro", (object)aprobacionFacturaDto.ID_APROBACION_CENTRO ?? DBNull.Value));
            sqlCommando.Parameters.Add(new SqlParameter("@Estado", (object)aprobacionFacturaDto.ESTADO ?? DBNull.Value));
            sqlCommando.Parameters.Add(new SqlParameter("@UsuarioAlta", (object)aprobacionFacturaDto.USUARIO_MANTENIMIENTO.USUARIO_ALTA ?? DBNull.Value));

            return Convert.ToInt32(sqlCommando.ExecuteScalar());
        }

        public Int32 EjecutarSentenciaUpdateFactura(AprobacionFacturasDTO aprobacionfacturaDto)
        {
            SqlCommand _sqlComando = null;

            _sqlComando = new SqlCommand(sqlUpdate, _sqlConn);

            if (_sqlTran != null)
                _sqlComando.Transaction = _sqlTran;

            _sqlComando.Parameters.Add(new SqlParameter("@IdAprobadorCentro", (object)aprobacionfacturaDto.ID_APROBACION_CENTRO ?? DBNull.Value));
            _sqlComando.Parameters.Add(new SqlParameter("@IdFactura", (object)aprobacionfacturaDto.ID_FACTURA ?? DBNull.Value));
            _sqlComando.Parameters.Add(new SqlParameter("@Estado", (object)aprobacionfacturaDto.ESTADO ?? DBNull.Value));
            _sqlComando.Parameters.Add(new SqlParameter("@UsuarioModificacion", (object)aprobacionfacturaDto.USUARIO_MANTENIMIENTO.USUARIO_MODIFICO ?? DBNull.Value));

            return Convert.ToInt32(_sqlComando.ExecuteNonQuery());
        }

        public bool ExisteFactura(decimal idFactura)
        {
            SqlCommand _sqlComando = null;
            string sql = @" SELECT    COUNT(IdAprobacionFacturas)
                            FROM AprobacionFacturas
                            WHERE IdFactura = @IdFactura";

            _sqlComando = new SqlCommand(sql, _sqlConn);

            if (_sqlTran != null)
                _sqlComando.Transaction = _sqlTran;

            _sqlComando.Parameters.Add(new SqlParameter("@IdFactura", (object)idFactura ?? DBNull.Value));

            return Convert.ToInt32(_sqlComando.ExecuteScalar()) > 0 ? false : true;
        }

        #endregion

    }
}
