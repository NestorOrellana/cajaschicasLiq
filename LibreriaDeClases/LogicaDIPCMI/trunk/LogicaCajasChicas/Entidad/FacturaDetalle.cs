using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using DipCmiGT.LogicaCajasChicas;
using System.Data;

namespace DipCmiGT.LogicaCajasChicas.Entidad
{
    public class FacturaDetalle
    {
        #region Declaraciones

        private SqlConnection _sqlConn = null;
        private SqlTransaction _sqlTran = null;

        protected string sqlSelect = @" SELECT a.IdFacturaDetalle, a.Numero, a.Cantidad, a.IVA, a.Valor, a.Impuesto, a.CuentaContable, a.DefinicionCuentaContable, a.CargoAbono, a.IdentificadorIVA, a.Descripcion, a.FechaModificacion, a.IdFactura, b.Descripcion AS DescripcionTI, a.Alta, b.importe, a.Detalle
                                        FROM FacturaDetalle AS a 
                                        INNER JOIN IdentificadoresIVA   AS b ON b.IdentificadorIVA = a.IdentificadorIVA 
                                        INNER JOIN FacturaEncabezado	AS c ON c.IdFactura = a.IdFactura               
										INNER JOIN CajaChicaEncabezado	AS d ON	d.IdCajaChica = c.IdCajaChica
										INNER JOIN SociedadCentro		AS e ON	e.IdsociedadCentro = d.IdSociedadCentro
										INNER JOIN Sociedad				AS f ON f.CodigoSociedad = e.CodigoSociedad";

        protected string sqlInsert = @" INSERT INTO FacturaDetalle (Numero,   Cantidad,  Iva,  Valor,  Impuesto,  CuentaContable,  DefinicionCuentaContable,  CargoAbono,  IdentificadorIVA,  Descripcion,  IdFactura, Detalle)
					                                        VALUES (@Numero, @Cantidad, @Iva, @Valor, @Impuesto, @CuentaContable, @DefinicionCuentaContable, @CargoAbono, @IdentificadorIVA, @Descripcion, @IdFactura, @Detalle) 
                                         SELECT @@identity ";

        protected string sqlUpdate = @" UPDATE FacturaDetalle SET Numero = @Numero,   Cantidad = @Cantidad,  Iva = @Iva,  Valor = @Valor,  Impuesto = @Impuesto,  CuentaContable = @CuentaContable,  DefinicionCuentaContable = @DefinicionCuentaContable,  
                                                                  CargoAbono = @CargoAbono,  IdentificadorIVA = @IdentificadorIVA,  Descripcion = @Descripcion, Detalle = @Detalle
                                         ";

        #endregion

        #region Constructores


        public FacturaDetalle(SqlConnection sqlConn)
        {
            _sqlConn = sqlConn;
        }

        public FacturaDetalle(SqlConnection sqlConn, SqlTransaction sqlTran)
        {
            _sqlConn = sqlConn;
            _sqlTran = sqlTran;
        }

        #endregion

        #region Metodos

        public FacturaDetalleDTO EjecutarSentenciaSelect()
        {
            throw new NotImplementedException("");
        }

        public decimal EjecutarSentenciaInsert(FacturaDetalleDTO _facturaDetalleDto)
        {
            SqlCommand _sqlComando = null;
            string sql = sqlInsert;

            _sqlComando = new SqlCommand(sql, _sqlConn);
            _sqlComando.CommandType = CommandType.Text;

            if (_sqlTran != null)
                _sqlComando.Transaction = _sqlTran;

            _sqlComando.Parameters.Add(new SqlParameter("@Numero", (object)_facturaDetalleDto.NUMERO ?? DBNull.Value));
            _sqlComando.Parameters.Add(new SqlParameter("@Cantidad", (object)_facturaDetalleDto.CANTIDAD ?? DBNull.Value));
            _sqlComando.Parameters.Add(new SqlParameter("@Iva", (object)_facturaDetalleDto.IVA ?? DBNull.Value));
            _sqlComando.Parameters.Add(new SqlParameter("@Valor", (object)_facturaDetalleDto.VALOR ?? DBNull.Value));
            _sqlComando.Parameters.Add(new SqlParameter("@Impuesto", (object)_facturaDetalleDto.IMPUESTO ?? DBNull.Value));
            _sqlComando.Parameters.Add(new SqlParameter("@CuentaContable", (object)_facturaDetalleDto.CUENTA_CONTABLE ?? DBNull.Value));
            _sqlComando.Parameters.Add(new SqlParameter("@DefinicionCuentaContable", (object)_facturaDetalleDto.DEFINICION_CC ?? DBNull.Value));
            _sqlComando.Parameters.Add(new SqlParameter("@CargoAbono", (object)_facturaDetalleDto.CARGO_ABONO ?? DBNull.Value));
            _sqlComando.Parameters.Add(new SqlParameter("@IdentificadorIVA", (object)_facturaDetalleDto.IDENTIFICADOR_IVA ?? DBNull.Value));
            _sqlComando.Parameters.Add(new SqlParameter("@Descripcion", (object)_facturaDetalleDto.DESCRIPCION.ToUpper() ?? DBNull.Value));
            _sqlComando.Parameters.Add(new SqlParameter("@IdFactura", (object)_facturaDetalleDto.ID_FACTURA ?? DBNull.Value));
            _sqlComando.Parameters.Add(new SqlParameter("@Detalle", (object)_facturaDetalleDto.DETALLE ?? DBNull.Value));

            return Convert.ToDecimal(_sqlComando.ExecuteScalar());
        }

        public Int32 EjecutarSenteciaUpdate(FacturaDetalleDTO _facturaDetalleDto)
        {
            SqlCommand _sqlComando = null;
            string sql = sqlUpdate;

            _sqlComando = new SqlCommand(sql, _sqlConn);
            _sqlComando.CommandType = CommandType.Text;

            if (_sqlTran != null)
                _sqlComando.Transaction = _sqlTran;

            _sqlComando.Parameters.Add(new SqlParameter("@Numero", (object)_facturaDetalleDto.NUMERO ?? DBNull.Value));
            _sqlComando.Parameters.Add(new SqlParameter("@Cantidad", (object)_facturaDetalleDto.CANTIDAD ?? DBNull.Value));
            _sqlComando.Parameters.Add(new SqlParameter("@Iva", (object)_facturaDetalleDto.IVA ?? DBNull.Value));
            _sqlComando.Parameters.Add(new SqlParameter("@Valor", (object)_facturaDetalleDto.VALOR ?? DBNull.Value));
            _sqlComando.Parameters.Add(new SqlParameter("@Impuesto", (object)_facturaDetalleDto.IMPUESTO ?? DBNull.Value));
            _sqlComando.Parameters.Add(new SqlParameter("@CuentaContable", (object)_facturaDetalleDto.CUENTA_CONTABLE ?? DBNull.Value));
            _sqlComando.Parameters.Add(new SqlParameter("@DefinicionCuentaContable", (object)_facturaDetalleDto.DEFINICION_CC ?? DBNull.Value));
            _sqlComando.Parameters.Add(new SqlParameter("@CargoAbono", (object)_facturaDetalleDto.CARGO_ABONO ?? DBNull.Value));
            _sqlComando.Parameters.Add(new SqlParameter("@IdentificadorIVA", (object)_facturaDetalleDto.IDENTIFICADOR_IVA ?? DBNull.Value));
            _sqlComando.Parameters.Add(new SqlParameter("@Descripcion", (object)_facturaDetalleDto.DESCRIPCION.ToUpper() ?? DBNull.Value));
            _sqlComando.Parameters.Add(new SqlParameter("@IdFacturaDetalle", (object)_facturaDetalleDto.ID_FACTURA_DETALLE ?? DBNull.Value));
            _sqlComando.Parameters.Add(new SqlParameter("@Detalle", (object)_facturaDetalleDto.DETALLE ?? DBNull.Value));

            return _sqlComando.ExecuteNonQuery();
        }

        public List<FacturaDetalleDTO> CargarReader(SqlDataReader sqlDataReader)
        {
            List<FacturaDetalleDTO> _listaFacturaDetalleDto = new List<FacturaDetalleDTO>();
            FacturaDetalleDTO _facturaDetalleDto = null;

            try
            {
                while (sqlDataReader.Read())
                {
                    _facturaDetalleDto = new FacturaDetalleDTO();

                    _facturaDetalleDto.ID_FACTURA_DETALLE = sqlDataReader.GetDecimal(0);
                    _facturaDetalleDto.NUMERO = sqlDataReader.GetInt32(1);
                    _facturaDetalleDto.CANTIDAD = sqlDataReader.GetDouble(2);
                    _facturaDetalleDto.IVA = sqlDataReader.GetDouble(3);
                    _facturaDetalleDto.VALOR = sqlDataReader.GetDouble(4);
                    _facturaDetalleDto.IMPUESTO = sqlDataReader.GetDouble(5);
                    _facturaDetalleDto.CUENTA_CONTABLE = sqlDataReader.GetString(6);
                    _facturaDetalleDto.DEFINICION_CC = sqlDataReader.GetString(7);
                    _facturaDetalleDto.CARGO_ABONO = sqlDataReader.GetInt16(8);
                    _facturaDetalleDto.IDENTIFICADOR_IVA = sqlDataReader.GetString(9);
                    _facturaDetalleDto.DESCRIPCION = sqlDataReader.GetString(10);
                    _facturaDetalleDto.FECHA_MODIFICACION = sqlDataReader.IsDBNull(11) ? (DateTime?)null : sqlDataReader.GetDateTime(11); //FechaModificacion, 
                    _facturaDetalleDto.ID_FACTURA = sqlDataReader.GetDecimal(12);
                    _facturaDetalleDto.DEFINICION_TIPO_IVA = sqlDataReader.GetString(13);
                    _facturaDetalleDto.ALTA = sqlDataReader.GetBoolean(14);
                    _facturaDetalleDto.IMPORTE = sqlDataReader.GetInt16(15);
                    _facturaDetalleDto.DETALLE = sqlDataReader.IsDBNull(16) ? null:  sqlDataReader.GetString(16);

                    _listaFacturaDetalleDto.Add(_facturaDetalleDto);
                }
                return _listaFacturaDetalleDto;
            }
            finally
            {
                if (sqlDataReader != null) sqlDataReader.Close();
            }
        }

        public List<FacturaDetalleDTO> BuscarDetalleFacturas(decimal idFactura)
        {
            SqlCommand _sqlComando = null;
            string sql = sqlSelect + @" WHERE a.IdFactura = @IdFactura and a.Alta = 1 and b.Activo = 1 
                                        AND   f.Pais = b.dominio 
                                        AND   b.Sociedad = f.CodigoSociedad"; //SATB Se agrego validacion de Indicador por sociedad

            _sqlComando = new SqlCommand(sql, _sqlConn);
            _sqlComando.CommandType = CommandType.Text;

            if (_sqlTran != null)
                _sqlComando.Transaction = _sqlTran;

            _sqlComando.Parameters.Add(new SqlParameter("@IdFactura", (object)idFactura ?? DBNull.Value));

            return CargarReader(_sqlComando.ExecuteReader());
        }

        public Int32 AnularDetalleFactura(decimal idFactura)
        {
            SqlCommand _sqlComando = null;
            string sql = @" UPDATE FacturaDetalle Set Alta = 0, FechaModificacion = getdate()  WHERE IdFactura = @IdFactura ";

            _sqlComando = new SqlCommand(sql, _sqlConn);
            _sqlComando.CommandType = CommandType.Text;

            if (_sqlTran != null)
                _sqlComando.Transaction = _sqlTran;

            _sqlComando.Parameters.Add(new SqlParameter("@IdFactura", (object)idFactura ?? DBNull.Value));

            return _sqlComando.ExecuteNonQuery();
        }

        #endregion

    }
}
