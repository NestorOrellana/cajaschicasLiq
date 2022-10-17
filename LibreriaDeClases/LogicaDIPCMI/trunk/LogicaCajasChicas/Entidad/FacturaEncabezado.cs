using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using DipCmiGT.LogicaCajasChicas;
using System.Data;
using DipCmiGT.LogicaComun;

namespace DipCmiGT.LogicaCajasChicas.Entidad
{
    public class FacturaEncabezado
    {

        #region Declaraciones

        private SqlConnection _sqlConn = null;
        private SqlTransaction _sqlTran = null;

        //        protected string sqlSelect = @" SELECT distinct IdFactura, c.IdTipoDocumento, c.Descripcion, b.NumeroIdentificacion, b.Nombre, Serie, Numero, FechaFactura, 
        //				                        EsEspecial, RetencionIVA, RetencionISR, IVA, a.ValorTotal, a.Estado, UsuarioCreacion, a.FechaCreacion, UsuarioModifico, 
        //				                        a.FechaModificacion, a.IdProveedor, a.IdCajaChica, CentroCosto, OrdenesCosto, aprobada, Correlativo, NumeroCajaChica, 
        //                                        d.Estado, a.TipoFactura, a.NumeroRetencionISR, a.NumeroRetencionIVA, a.ValorRetencionISR, 
        //                                        a.ValorRetencionIVA, d.NumeroCajaChica, Nivel, d.IdSociedadCentro
        //                                        FROM FacturaEncabezado a
        //                                        inner join Proveedor b on b.IdProveedor = a.IdProveedor
        //                                        inner join TipoDocumento c on c.IdTipoDocumento = b.IdTipoDocumento 
        //                                        inner join CajaChicaEncabezado d on d.IdCajaChica = a.IdCajaChica ";

        protected string sqlSelect = @" SELECT distinct IdFactura, c.IdTipoDocumento, c.Descripcion, b.NumeroIdentificacion, b.Nombre, Serie, Numero, FechaFactura, 
                                        EsEspecial, RetencionIVA, RetencionISR, IVA, a.ValorTotal, a.Estado, a.UsuarioCreacion, a.FechaCreacion, UsuarioModifico, 
                                        a.FechaModificacion, a.IdProveedor, a.IdCajaChica, CentroCosto, OrdenesCosto, aprobada, Correlativo, NumeroCajaChica, 
                                        d.Estado, a.TipoFactura, a.NumeroRetencionISR, a.NumeroRetencionIVA, a.ValorRetencionISR, 
                                        a.ValorRetencionIVA, d.NumeroCajaChica, Nivel, d.IdSociedadCentro, case when g.Moneda is null then h.Moneda else g.Moneda end Moneda,
                                        a.FacturaDividida, a.TotalFacturaDividida,
                                        
                                        (SELECT ISNULL(SUM(IVA),0) AS IVA
                                        FROM FacturaEncabezado as Suma
                                        WHERE Suma.Serie = a.Serie 
                                        AND   Suma.IdProveedor = a.IdProveedor
                                        AND	  Suma.Numero = a.Numero
                                        AND (Suma.Estado = 1 or Suma.Estado = 2)) as IVAacumulado,
                                        
                                        ISNULL(a.Observaciones,''),
                                        
                                        (SELECT ISNULL(SUM(SumaAcumulado.Valortotal),0) 
                                        FROM FacturaEncabezado as SumaAcumulado
                                        WHERE SumaAcumulado.Serie = a.Serie 
                                        AND   SumaAcumulado.IdProveedor = a.IdProveedor
                                        AND	  SumaAcumulado.Numero = a.Numero
                                        AND (SumaAcumulado.Estado = 1 or SumaAcumulado.Estado = 2)) as Acumulado,
                                        ISNULL(i.IdTipoDocumento,'') IdTipoDocumento2 , ISNULL(i.Descripcion,'') Descripcion2, 
										ISNULL(b.NumeroIdentificacion2,'') NumeroIdentificacion2,

                                        CONVERT(decimal(10,2), (SELECT ISNULL(SUM(SumaAcumulado.ValorRetencionISR),0) 
                                        FROM FacturaEncabezado as SumaAcumulado
                                        WHERE SumaAcumulado.Serie = a.Serie 
                                        AND   SumaAcumulado.IdProveedor = a.IdProveedor
                                        AND	  SumaAcumulado.Numero = a.Numero
                                        AND (SumaAcumulado.Estado = 1 or SumaAcumulado.Estado = 2))) as AcumuladoISR,
                                        b.Tipo, e.CodigoSociedad, a.ValorRealFact, a.TipoCambio, ISNULL(a.RetServicios,0), h.Pais

                                        FROM FacturaEncabezado a
                                        inner join Proveedor b on b.IdProveedor = a.IdProveedor
                                        inner join TipoDocumento c on c.IdTipoDocumento = b.IdTipoDocumento 
                                        inner join CajaChicaEncabezado d on d.IdCajaChica = a.IdCajaChica
                                        inner join SociedadCentro e on e.IdSociedadCentro = d.IdSociedadCentro
                                        left join SociedadMoneda f on f.CodigoSociedad = e.CodigoSociedad and f.IdSociedadMoneda = d.IdSociedadMoneda
                                        left join Moneda g on g.Moneda = f.Moneda
                                        inner join Sociedad h on h.CodigoSociedad = e.CodigoSociedad
                                        left join TipoDocumento i on i.IdTipoDocumento = b.IdTipoDocumento2 and b.IdTipoDocumento2 is not null";


        protected string sqlInsert = @" INSERT INTO FacturaEncabezado (CentroCosto, OrdenesCosto, Serie, Numero, FechaFactura, EsEspecial, RetencionIVA, RetencionISR, IVA, a.ValorTotal, UsuarioCreacion, IdProveedor, IdCajaChica, TipoFactura, 
                                        NumeroRetencionISR, NumeroRetencionIVA, ValorRetencionISR, ValorRetencionIVA, Nivel, FacturaDividida, TotalFacturaDividida, Observaciones, ValorRealFact, TipoCambio, RetServicios)
                                        VALUES (@CentroCosto, @OrdenesCosto, @Serie, RTRIM(@Numero), @FechaFactura, @EsEspecial, @RetencionIVA, @RetencionISR, @IVA, @ValorTotal, @UsuarioCreacion, @IdProveedor, @IdCajaChica, @TipoFactura, 
                                        @NumeroRetencionISR, @NumeroRetencionIVA, @ValorRetencionISR, @ValorRetencionIVA, @Nivel, @FacturaDividida, @TotalFacturaDividida,  @Observaciones, @ValorRealFact, ISNULL(@TipoCambio,1), @RetServicios)
                                        SELECT @@identity ";

        protected string sqlUpdate = @" UPDATE FacturaEncabezado SET CentroCosto = @CentroCosto, OrdenesCosto = @OrdenesCosto, Serie = @Serie, Numero = RTRIM(@Numero), FechaFactura = @FechaFactura, EsEspecial = @EsEspecial, RetencionIVA = @RetencionIVA, RetencionISR = @RetencionISR, IVA = @IVA, 
                                                                     ValorTotal = @ValorTotal, UsuarioModifico = @UsuarioModifico, FechaModificacion = getdate(), IdProveedor = @IdProveedor, IdCajaChica = @IdCajaChica, TipoFactura = @TipoFactura, NumeroRetencionISR = @NumeroRetencionISR, 
                                                                     NumeroRetencionIVA = @NumeroRetencionIVA, ValorRetencionISR = @ValorRetencionISR, ValorRetencionIVA = @ValorRetencionIVA, Nivel = @nivel, FacturaDividida = @FacturaDividida, TotalFacturaDividida = @TotalFacturaDividida, 
                                                                     Observaciones = @Observaciones, ValorRealFact = @ValorRealFact, RetServicios = @RetServicios
                                        WHERE IdFactura = @IdFactura ";

        #endregion

        #region Constructores

        public FacturaEncabezado(SqlConnection sqlConn)
        {
            _sqlConn = sqlConn;
        }

        public FacturaEncabezado(SqlConnection sqlConn, SqlTransaction sqlTran)
        {
            _sqlConn = sqlConn;
            _sqlTran = sqlTran;
        }

        #endregion

        #region Metodos

        public FacturaEncabezadoDTO EjecutarSentenciaSelect(decimal idFactura)
        {
            List<FacturaEncabezadoDTO> _listaFacturaEncabezadoDto = null;
            SqlCommand _sqlComando = null;
            string sql = sqlSelect + @" WHERE IdFactura = @IdFactura
                                        ORDER BY IdFactura DESC ";

            _sqlComando = new SqlCommand(sql, _sqlConn);

            if (_sqlTran != null)
                _sqlComando.Transaction = _sqlTran;

            _sqlComando.Parameters.Add(new SqlParameter("@IdFactura", (object)idFactura ?? DBNull.Value));

            _listaFacturaEncabezadoDto = CargarReader(_sqlComando.ExecuteReader());

            return _listaFacturaEncabezadoDto.Count > 0 ? _listaFacturaEncabezadoDto[0] : null;
        }

        public decimal EjecutarSentenciaInsert(FacturaEncabezadoDTO facturaEncabezadoDTO)
        {
            SqlCommand _sqlComando = null;

            _sqlComando = new SqlCommand(sqlInsert, _sqlConn);

            if (_sqlTran != null)
                _sqlComando.Transaction = _sqlTran;

            _sqlComando.Parameters.Add(new SqlParameter("@CentroCosto", (object)facturaEncabezadoDTO.CENTRO_COSTO ?? DBNull.Value));
            _sqlComando.Parameters.Add(new SqlParameter("@OrdenesCosto", (object)facturaEncabezadoDTO.ORDEN_COSTO ?? DBNull.Value));
            _sqlComando.Parameters.Add(new SqlParameter("@Serie", (object)facturaEncabezadoDTO.SERIE.ToUpper() ?? DBNull.Value));
            _sqlComando.Parameters.Add(new SqlParameter("@Numero", (object)facturaEncabezadoDTO.NUMERO ?? DBNull.Value));
            _sqlComando.Parameters.Add(new SqlParameter("@FechaFactura", (object)facturaEncabezadoDTO.FECHA_FACTURA ?? DBNull.Value));
            _sqlComando.Parameters.Add(new SqlParameter("@EsEspecial", (object)facturaEncabezadoDTO.ES_ESPECIAL ?? DBNull.Value));
            _sqlComando.Parameters.Add(new SqlParameter("@RetencionIVA", (object)facturaEncabezadoDTO.RETENCION_IVA ?? DBNull.Value));
            _sqlComando.Parameters.Add(new SqlParameter("@RetencionISR", (object)facturaEncabezadoDTO.RETENCION_ISR ?? DBNull.Value));
            _sqlComando.Parameters.Add(new SqlParameter("@IVA", (object)facturaEncabezadoDTO.IVA ?? DBNull.Value));
            _sqlComando.Parameters.Add(new SqlParameter("@ValorTotal", (object)facturaEncabezadoDTO.VALOR_TOTAL ?? DBNull.Value));
            _sqlComando.Parameters.Add(new SqlParameter("@UsuarioCreacion", (object)facturaEncabezadoDTO.USUARIO_MANTENIMIENTO.USUARIO_ALTA.ToUpper() ?? DBNull.Value));
            _sqlComando.Parameters.Add(new SqlParameter("@IdProveedor", (object)facturaEncabezadoDTO.ID_PROVEEDOR ?? DBNull.Value));
            _sqlComando.Parameters.Add(new SqlParameter("@IdCajaChica", (object)facturaEncabezadoDTO.ID_CAJA_CHICA ?? DBNull.Value));
            _sqlComando.Parameters.Add(new SqlParameter("@TipoFactura", (object)facturaEncabezadoDTO.TIPO_FACTURA ?? DBNull.Value));
            _sqlComando.Parameters.Add(new SqlParameter("@NumeroRetencionISR", (object)facturaEncabezadoDTO.NUMERO_RETENCION_ISR ?? DBNull.Value));
            _sqlComando.Parameters.Add(new SqlParameter("@NumeroRetencionIVA", (object)facturaEncabezadoDTO.NUMERO_RETENCION_IVA ?? DBNull.Value));
            _sqlComando.Parameters.Add(new SqlParameter("@ValorRetencionISR", (object)facturaEncabezadoDTO.VALOR_RETENCION_ISR ?? DBNull.Value));
            _sqlComando.Parameters.Add(new SqlParameter("@ValorRetencionIVA", (object)facturaEncabezadoDTO.VALOR_RETENCION_IVA ?? DBNull.Value));
            _sqlComando.Parameters.Add(new SqlParameter("@Nivel", (object)facturaEncabezadoDTO.NIVEL ?? DBNull.Value));
            _sqlComando.Parameters.Add(new SqlParameter("@FacturaDividida", (object)facturaEncabezadoDTO.FACTURA_DIVIDIDA ?? DBNull.Value));
            _sqlComando.Parameters.Add(new SqlParameter("@TotalFacturaDividida", (object)facturaEncabezadoDTO.TOTALFACTURADIVIDIDA ?? DBNull.Value));
            _sqlComando.Parameters.Add(new SqlParameter("@Observaciones", (object)facturaEncabezadoDTO.OBSERVACIONES ?? DBNull.Value));
            _sqlComando.Parameters.Add(new SqlParameter("@ValorRealFact", (object)facturaEncabezadoDTO.VALOR_REAL_FACT ?? DBNull.Value));
            _sqlComando.Parameters.Add(new SqlParameter("@TipoCambio", (object)facturaEncabezadoDTO.TIPO_CAMBIO ?? DBNull.Value));
            _sqlComando.Parameters.Add(new SqlParameter("@RetServicios", (object)facturaEncabezadoDTO.RETSERVICIOS ?? DBNull.Value));

            return Convert.ToDecimal(_sqlComando.ExecuteScalar());
        }

        public Int32 EjecutarSentenciaUpdate(FacturaEncabezadoDTO facturaEncabezadoDTO)
        {
            SqlCommand _sqlComando = null;

            _sqlComando = new SqlCommand(sqlUpdate, _sqlConn);

            if (_sqlTran != null)
                _sqlComando.Transaction = _sqlTran;

            _sqlComando.Parameters.Add(new SqlParameter("@CentroCosto", (object)facturaEncabezadoDTO.CENTRO_COSTO ?? DBNull.Value));
            _sqlComando.Parameters.Add(new SqlParameter("@OrdenesCosto", (object)facturaEncabezadoDTO.ORDEN_COSTO ?? DBNull.Value));
            _sqlComando.Parameters.Add(new SqlParameter("@Serie", (object)facturaEncabezadoDTO.SERIE.ToUpper() ?? DBNull.Value));
            _sqlComando.Parameters.Add(new SqlParameter("@Numero", (object)facturaEncabezadoDTO.NUMERO ?? DBNull.Value));
            _sqlComando.Parameters.Add(new SqlParameter("@FechaFactura", (object)facturaEncabezadoDTO.FECHA_FACTURA ?? DBNull.Value));
            _sqlComando.Parameters.Add(new SqlParameter("@EsEspecial", (object)facturaEncabezadoDTO.ES_ESPECIAL ?? DBNull.Value));
            _sqlComando.Parameters.Add(new SqlParameter("@RetencionIVA", (object)facturaEncabezadoDTO.RETENCION_IVA ?? DBNull.Value));
            _sqlComando.Parameters.Add(new SqlParameter("@RetencionISR", (object)facturaEncabezadoDTO.RETENCION_ISR ?? DBNull.Value));
            _sqlComando.Parameters.Add(new SqlParameter("@IVA", (object)facturaEncabezadoDTO.IVA ?? DBNull.Value));
            _sqlComando.Parameters.Add(new SqlParameter("@ValorTotal", (object)facturaEncabezadoDTO.VALOR_TOTAL ?? DBNull.Value));
            _sqlComando.Parameters.Add(new SqlParameter("@UsuarioModifico", (object)facturaEncabezadoDTO.USUARIO_MANTENIMIENTO.USUARIO_MODIFICO.ToUpper() ?? DBNull.Value));
            _sqlComando.Parameters.Add(new SqlParameter("@IdProveedor", (object)facturaEncabezadoDTO.ID_PROVEEDOR ?? DBNull.Value));
            _sqlComando.Parameters.Add(new SqlParameter("@IdCajaChica", (object)facturaEncabezadoDTO.ID_CAJA_CHICA ?? DBNull.Value));
            _sqlComando.Parameters.Add(new SqlParameter("@TipoFactura", (object)facturaEncabezadoDTO.TIPO_FACTURA ?? DBNull.Value));
            _sqlComando.Parameters.Add(new SqlParameter("@NumeroRetencionISR", (object)facturaEncabezadoDTO.NUMERO_RETENCION_ISR ?? DBNull.Value));
            _sqlComando.Parameters.Add(new SqlParameter("@NumeroRetencionIVA", (object)facturaEncabezadoDTO.NUMERO_RETENCION_IVA ?? DBNull.Value));
            _sqlComando.Parameters.Add(new SqlParameter("@ValorRetencionISR", (object)facturaEncabezadoDTO.VALOR_RETENCION_ISR ?? DBNull.Value));
            _sqlComando.Parameters.Add(new SqlParameter("@ValorRetencionIVA", (object)facturaEncabezadoDTO.VALOR_RETENCION_IVA ?? DBNull.Value));
            _sqlComando.Parameters.Add(new SqlParameter("@Nivel", (object)facturaEncabezadoDTO.NIVEL ?? DBNull.Value));
            _sqlComando.Parameters.Add(new SqlParameter("@IdFactura", (object)facturaEncabezadoDTO.ID_FACTURA ?? DBNull.Value));
            _sqlComando.Parameters.Add(new SqlParameter("@FacturaDividida", (object)facturaEncabezadoDTO.FACTURA_DIVIDIDA ?? DBNull.Value));
            _sqlComando.Parameters.Add(new SqlParameter("@TotalFacturaDividida", (object)facturaEncabezadoDTO.TOTALFACTURADIVIDIDA ?? DBNull.Value));
            _sqlComando.Parameters.Add(new SqlParameter("@Observaciones", (object)facturaEncabezadoDTO.OBSERVACIONES ?? DBNull.Value));
            _sqlComando.Parameters.Add(new SqlParameter("@ValorRealFact", (object)facturaEncabezadoDTO.VALOR_REAL_FACT ?? DBNull.Value));
            _sqlComando.Parameters.Add(new SqlParameter("@TipoCambio", (object)facturaEncabezadoDTO.TIPO_CAMBIO ?? DBNull.Value));
            _sqlComando.Parameters.Add(new SqlParameter("@RetServicios", (object)facturaEncabezadoDTO.RETSERVICIOS ?? DBNull.Value));

            return Convert.ToInt32(_sqlComando.ExecuteNonQuery());
        }

        protected List<FacturaEncabezadoDTO> CargarDatosFacturaDividida(SqlDataReader sqlDataReader)
        {
            List<FacturaEncabezadoDTO> _listaFacturaDivididaEncabezadoDto = new List<FacturaEncabezadoDTO>();
            FacturaEncabezadoDTO _FacturaDivididaEncabezadoDt = new FacturaEncabezadoDTO();

            try
            {
                while (sqlDataReader.Read())
                {
                    _FacturaDivididaEncabezadoDt = new FacturaEncabezadoDTO();

                    _FacturaDivididaEncabezadoDt.TOTALFACTURADIVIDIDA = sqlDataReader.GetDouble(0);
                    _FacturaDivididaEncabezadoDt.VALOR_TOTAL = sqlDataReader.GetDouble(1);

                    _listaFacturaDivididaEncabezadoDto.Add(_FacturaDivididaEncabezadoDt);
                }

                return _listaFacturaDivididaEncabezadoDto;

            }
            finally
            {
                if (sqlDataReader != null) sqlDataReader.Close();
            }
        }

        protected List<FacturaEncabezadoDTO> CargarReader(SqlDataReader sqlDataReader)
        {
            List<FacturaEncabezadoDTO> _listaFacturaEncabezadoDto = new List<FacturaEncabezadoDTO>();
            FacturaEncabezadoDTO _facturaEncabezadoDto = new FacturaEncabezadoDTO();

            try
            {
                while (sqlDataReader.Read())
                {
                    _facturaEncabezadoDto = new FacturaEncabezadoDTO();

                    _facturaEncabezadoDto.ID_FACTURA = sqlDataReader.GetDecimal(0);
                    _facturaEncabezadoDto.ID_TIPO_DOCUMENTO = sqlDataReader.GetInt16(1);
                    _facturaEncabezadoDto.TIPO_DOCUMENTO = sqlDataReader.IsDBNull(2) ? string.Empty : sqlDataReader.GetString(2);
                    _facturaEncabezadoDto.NUMERO_IDENTIFICACION = sqlDataReader.GetString(3);
                    _facturaEncabezadoDto.NOMBRE_PROVEEDOR = sqlDataReader.GetString(4);
                    _facturaEncabezadoDto.SERIE = sqlDataReader.GetString(5);
                    _facturaEncabezadoDto.NUMERO = sqlDataReader.GetString(6);//sqlDataReader.GetDecimal(6);
                    _facturaEncabezadoDto.FECHA_FACTURA = sqlDataReader.GetDateTime(7);
                    _facturaEncabezadoDto.ES_ESPECIAL = sqlDataReader.GetBoolean(8);
                    _facturaEncabezadoDto.RETENCION_IVA = sqlDataReader.GetBoolean(9);
                    _facturaEncabezadoDto.RETENCION_ISR = sqlDataReader.GetBoolean(10);
                    //_facturaEncabezadoDto.IVA = sqlDataReader.GetDouble(11);
                    _facturaEncabezadoDto.VALOR_TOTAL = sqlDataReader.GetDouble(12);
                    _facturaEncabezadoDto.ESTADO = sqlDataReader.GetInt16(13);
                    _facturaEncabezadoDto.USUARIO_MANTENIMIENTO.USUARIO_ALTA = sqlDataReader.GetString(14);
                    _facturaEncabezadoDto.USUARIO_MANTENIMIENTO.FECHA_ALTA = sqlDataReader.GetDateTime(15);
                    _facturaEncabezadoDto.USUARIO_MANTENIMIENTO.USUARIO_MODIFICO = sqlDataReader.IsDBNull(16) ? null : sqlDataReader.GetString(16);
                    _facturaEncabezadoDto.USUARIO_MANTENIMIENTO.FECHA_MODIFICACION = sqlDataReader.IsDBNull(17) ? (DateTime?)null : sqlDataReader.GetDateTime(17);
                    _facturaEncabezadoDto.ID_PROVEEDOR = sqlDataReader.GetInt32(18);
                    _facturaEncabezadoDto.ID_CAJA_CHICA = sqlDataReader.GetDecimal(19);
                    _facturaEncabezadoDto.CENTRO_COSTO = sqlDataReader.IsDBNull(20) ? null : sqlDataReader.GetString(20);
                    _facturaEncabezadoDto.ORDEN_COSTO = sqlDataReader.IsDBNull(21) ? null : sqlDataReader.GetString(21);
                    _facturaEncabezadoDto.APROBADA = sqlDataReader.IsDBNull(22) ? (bool?)null : sqlDataReader.GetBoolean(22);
                    _facturaEncabezadoDto.CAJA_CHICA.CORRELATIVO = sqlDataReader.GetInt32(23);
                    _facturaEncabezadoDto.CAJA_CHICA.CAJA_CHICA_SAP = sqlDataReader.GetString(24);
                    _facturaEncabezadoDto.CAJA_CHICA.ESTADO = sqlDataReader.GetInt16(25);
                    _facturaEncabezadoDto.TIPO_FACTURA = sqlDataReader.GetString(26);
                    _facturaEncabezadoDto.NUMERO_RETENCION_ISR = sqlDataReader.IsDBNull(27) ? (decimal?)null : sqlDataReader.GetDecimal(27);
                    _facturaEncabezadoDto.NUMERO_RETENCION_IVA = sqlDataReader.IsDBNull(28) ? (decimal?)null : sqlDataReader.GetDecimal(28);
                    _facturaEncabezadoDto.VALOR_RETENCION_ISR = sqlDataReader.IsDBNull(29) ? (double?)null : sqlDataReader.GetDouble(29);
                    _facturaEncabezadoDto.VALOR_RETENCION_IVA = sqlDataReader.IsDBNull(30) ? (double?)null : sqlDataReader.GetDouble(30);
                    _facturaEncabezadoDto.CAJA_CHICA.CAJA_CHICA_SAP = sqlDataReader.GetString(31);
                    _facturaEncabezadoDto.NIVEL = sqlDataReader.GetInt16(32);
                    _facturaEncabezadoDto.CAJA_CHICA.ID_SOCIEDAD_CENTRO = sqlDataReader.GetInt32(33);
                    _facturaEncabezadoDto.MONEDA = sqlDataReader.GetString(34);
                    _facturaEncabezadoDto.FACTURA_DIVIDIDA = sqlDataReader.IsDBNull(35) ? (bool?)null : sqlDataReader.GetBoolean(35);
                    _facturaEncabezadoDto.TOTALFACTURADIVIDIDA = sqlDataReader.IsDBNull(36) ? (double?)null : sqlDataReader.GetDouble(36);
                    _facturaEncabezadoDto.OBSERVACIONES = sqlDataReader.GetString(38);
                    _facturaEncabezadoDto.ID_TIPO_DOCUMENTO2 = sqlDataReader.GetInt16(40);
                    _facturaEncabezadoDto.TIPO_DOCUMENTO2 = sqlDataReader.GetString(41);
                    _facturaEncabezadoDto.NUMERO_IDENTIFICACION2 = sqlDataReader.GetString(42);
                    _facturaEncabezadoDto.TOTALFACTURADIVIDADISR = sqlDataReader.GetDecimal(43);
                    _facturaEncabezadoDto.TIPO = sqlDataReader.GetBoolean(44);
                    _facturaEncabezadoDto.CODIGO_SOCIEDAD = sqlDataReader.GetString(45);
                    _facturaEncabezadoDto.VALOR_REAL_FACT = sqlDataReader.IsDBNull(46) ? (double?)null : sqlDataReader.GetDouble(46);
                    _facturaEncabezadoDto.TIPO_CAMBIO = sqlDataReader.IsDBNull(47) ? (double?)null : sqlDataReader.GetDouble(47);
                    _facturaEncabezadoDto.RETSERVICIOS = sqlDataReader.IsDBNull(48) ? (bool?)null : sqlDataReader.GetBoolean(48);
                    _facturaEncabezadoDto.PAIS = sqlDataReader.IsDBNull(49) ? null : sqlDataReader.GetString(49);
                   
                    if (_facturaEncabezadoDto.FACTURA_DIVIDIDA == true)
                    {
                        _facturaEncabezadoDto.IVA = sqlDataReader.GetDouble(37);
                        //_facturaEncabezadoDto.ACUMULADO = sqlDataReader.GetDecimal(39);
                        _facturaEncabezadoDto.ACUMULADO = Convert.ToDecimal(sqlDataReader.GetDouble(39));
                       // _facturaEncabezadoDto.TOTALFACTURADIVIDADISR = sqlDataReader.GetDouble(43);
                    }
                    else
                    {
                        _facturaEncabezadoDto.IVA = sqlDataReader.GetDouble(11);
                        _facturaEncabezadoDto.ACUMULADO = 0;//0.00;
                        //_facturaEncabezadoDto.TOTALFACTURADIVIDADISR = 0;
                        //_facturaEncabezadoDto.ACUMULADO = sqlDataReader.GetDouble(37);
                    }

                    _listaFacturaEncabezadoDto.Add(_facturaEncabezadoDto);
                }

                return _listaFacturaEncabezadoDto;
            }
            finally
            {
                if (sqlDataReader != null) sqlDataReader.Close();
            }
        }

        //public bool ExisteFactura(int idProveedor, string serie, decimal numero, decimal idFactura, decimal idCajaChica)
        public bool ExisteFactura(int idProveedor, string serie, string numero, decimal idFactura, decimal idCajaChica)
        {
            SqlCommand _sqlComando = null;
            string sql = @" SELECT    COUNT(IdFactura)
                            FROM FacturaEncabezado
                            WHERE IdProveedor = @IdProveedor
                            and Serie = @Serie
                            and CONVERT(decimal, Numero) = @Numero
                            and (Estado = 1 or Estado = 2)
                            and IdFactura != @IdFactura
                            and IdCajaChica != @idCajaChica";
            decimal NumeroFactura;
            NumeroFactura = Convert.ToDecimal(numero);
            // and Numero = @Numero
            // CONVERT(bigint, Numero) =  CONVERT(bigint, @Numero)
            //and FacturaDividida != @facturaDividida"; //Se agrega validación para que permita registrar varias veces una misma factura,
            //siempre y cuando tenga la marca de que la factura es dividida para diferentes CO

            _sqlComando = new SqlCommand(sql, _sqlConn);

            if (_sqlTran != null)
                _sqlComando.Transaction = _sqlTran;

            _sqlComando.Parameters.Add(new SqlParameter("@IdProveedor", (object)idProveedor ?? DBNull.Value));
            _sqlComando.Parameters.Add(new SqlParameter("@Serie", (object)serie ?? DBNull.Value));
            _sqlComando.Parameters.Add(new SqlParameter("@Numero", (object)NumeroFactura ?? DBNull.Value));
            _sqlComando.Parameters.Add(new SqlParameter("@IdFactura", (object)idFactura ?? DBNull.Value));
            _sqlComando.Parameters.Add(new SqlParameter("@idCajaChica", (object)idCajaChica ?? DBNull.Value));

            return Convert.ToInt32(_sqlComando.ExecuteScalar()) > 0 ? false : true;
        }

        //Validar si la factura ya existe, que la que se va a ingresar tenga la marca de factura dividida 
        //public bool MarcaFacturaDividida(int idProveedor, string serie, decimal numero, decimal idFactura, bool facturadividida)
        public bool MarcaFacturaDividida(int idProveedor, string serie, string numero, decimal idFactura, bool facturadividida)
        {
            SqlCommand _sqlComando = null;
            string sql = @" SELECT    COUNT(IdFactura)
                            FROM FacturaEncabezado
                            WHERE IdProveedor = @IdProveedor
                            and Serie = @Serie
                            and CONVERT(decimal, Numero) = @Numero
                            and (Estado = 1 or Estado = 2)
                            and IdFactura != @IdFactura
                            and FacturaDividida != 1";
            // and FacturaDividida != @facturadividida";
            //and FacturaDividida != @facturaDividida"; //Se agrega validación para que permita registrar varias veces una misma factura,
            //siempre y cuando tenga la marca de que la factura es dividida para diferentes CO
            decimal NumeroFactura;
            NumeroFactura = Convert.ToDecimal(numero);

            _sqlComando = new SqlCommand(sql, _sqlConn);

            if (_sqlTran != null)
                _sqlComando.Transaction = _sqlTran;

            _sqlComando.Parameters.Add(new SqlParameter("@IdProveedor", (object)idProveedor ?? DBNull.Value));
            _sqlComando.Parameters.Add(new SqlParameter("@Serie", (object)serie ?? DBNull.Value));
            _sqlComando.Parameters.Add(new SqlParameter("@Numero", (object)NumeroFactura ?? DBNull.Value));
            _sqlComando.Parameters.Add(new SqlParameter("@IdFactura", (object)idFactura ?? DBNull.Value));
            _sqlComando.Parameters.Add(new SqlParameter("@FacturaDividida", (object)facturadividida ?? DBNull.Value));

            return Convert.ToInt32(_sqlComando.ExecuteScalar()) > 0 ? false : true;
        }

        //Obtener Datos de Factura Dividida
        public List<FacturaEncabezadoDTO> BuscarFacturaDivididaCajaChica(decimal idCajaChica, Int16 proveedor, string serie, string numero)
        {
            SqlCommand _sqlComando = null;
            string sql = sqlSelect + @" SELECT TOP 1 TotalFacturaDividida, ValorTotal
                                        FROM FacturaEncabezado 
                                        WHERE IdCajaChica = @IdCajaChica 
                                        AND   IdProveedor = @IdProveedor 
                                        AND   Numero = @Numero
                                        AND   Serie = @Serie 
                                        ORDER BY FechaCreacion";


            _sqlComando = new SqlCommand(sql, _sqlConn);

            if (_sqlTran != null)
                _sqlComando.Transaction = _sqlTran;

            _sqlComando.Parameters.Add(new SqlParameter("@IdCajaChica", (object)idCajaChica ?? DBNull.Value));
            _sqlComando.Parameters.Add(new SqlParameter("@IdProveedor", (object)proveedor ?? DBNull.Value));
            _sqlComando.Parameters.Add(new SqlParameter("@Numero", (object)numero ?? DBNull.Value));
            _sqlComando.Parameters.Add(new SqlParameter("@Serie", (object)serie ?? DBNull.Value));


            return CargarReader(_sqlComando.ExecuteReader());
        }


        //Obtener todas las facturas divididas 
        //        public List<FacturaEncabezadoDTO> BuscarFacturasDivididasRevision(decimal idCajaChica, string proveedor, string serie, decimal numero)
        public List<FacturaEncabezadoDTO> BuscarFacturasDivididasRevision(decimal idCajaChica, string proveedor, string serie, string numero)
        {
            List<FacturaEncabezadoDTO> _listaFacturaEncabezadoDto = null;
            SqlCommand _sqlComando = null;
            string sql = sqlSelect + @" WHERE a.IdCajaChica = @IdCajaChica 
                                        AND  b.NumeroIdentificacion = REPLACE(@proveedor, '&#241;', 'Ñ')
                                        AND   Numero = @Numero
                                        AND   Serie = @Serie
                                        AND   a.Aprobada = 1
                                        AND   a.Estado = 1";


            _sqlComando = new SqlCommand(sql, _sqlConn);

            if (_sqlTran != null)
                _sqlComando.Transaction = _sqlTran;

            _sqlComando.Parameters.Add(new SqlParameter("@IdCajaChica", (object)idCajaChica ?? DBNull.Value));
            _sqlComando.Parameters.Add(new SqlParameter("@proveedor", (object)proveedor ?? DBNull.Value));
            _sqlComando.Parameters.Add(new SqlParameter("@Numero", (object)numero ?? DBNull.Value));
            _sqlComando.Parameters.Add(new SqlParameter("@Serie", (object)serie ?? DBNull.Value));


            // _listaFacturaEncabezadoDto = CargarReader(_sqlComando.ExecuteReader());

            return CargarReader(_sqlComando.ExecuteReader());

            // return _listaFacturaEncabezadoDto.Count > 0 ? _listaFacturaEncabezadoDto[0] : null;
        }

        public List<FacturaEncabezadoDTO> BuscarFacturasCajaChica(decimal idCajaChica, Int16 estado)
        {
            SqlCommand _sqlComando = null;
            string sql = sqlSelect + @" WHERE a.IdCajaChica = @IdCajaChica 
                                        and a.Estado = case when @Estado = -1 then a.Estado else @Estado end  
                                        ORDER BY IdFactura DESC ";

            _sqlComando = new SqlCommand(sql, _sqlConn);

            if (_sqlTran != null)
                _sqlComando.Transaction = _sqlTran;

            _sqlComando.Parameters.Add(new SqlParameter("@IdCajaChica", (object)idCajaChica ?? DBNull.Value));
            _sqlComando.Parameters.Add(new SqlParameter("@Estado", (object)estado ?? DBNull.Value));

            return CargarReader(_sqlComando.ExecuteReader());
        }

        //public List<FacturaEncabezadoDTO> BuscarFacturasCajaChica(decimal idCajaChica, Int16 estado, string identificacion, string serie, decimal numero)
        public List<FacturaEncabezadoDTO> BuscarFacturasCajaChica(decimal idCajaChica, Int16 estado, string identificacion, string serie, string numero)
        {
            SqlCommand _sqlComando = null;
            string sql = sqlSelect + @" WHERE a.IdCajaChica = @IdCajaChica 
                                        and a.Estado = case when @Estado = -1 then a.Estado else @Estado end  
                                        and b.NumeroIdentificacion = case when @NumeroIdentificacion = '' then b.NumeroIdentificacion else @NumeroIdentificacion end
                                        and a.Serie = case when @Serie = '' then a.Serie else @Serie end
                                        and a.Numero = case when @Numero = '-1' then a.Numero else @Numero end 
                                        ORDER BY IdFactura DESC ";




            _sqlComando = new SqlCommand(sql, _sqlConn);

            if (_sqlTran != null)
                _sqlComando.Transaction = _sqlTran;

            _sqlComando.Parameters.Add(new SqlParameter("@IdCajaChica", (object)idCajaChica ?? DBNull.Value));
            _sqlComando.Parameters.Add(new SqlParameter("@Estado", (object)estado ?? DBNull.Value));
            _sqlComando.Parameters.Add(new SqlParameter("@NumeroIdentificacion", (object)identificacion ?? DBNull.Value));
            _sqlComando.Parameters.Add(new SqlParameter("@Serie", (object)serie ?? DBNull.Value));
            _sqlComando.Parameters.Add(new SqlParameter("@Numero", (object)numero ?? DBNull.Value));

            return CargarReader(_sqlComando.ExecuteReader());
        }

        public List<FacturaEncabezadoDTO> BuscarFacturasCajaChica(decimal idCajaChica, Int16 estadoCajaChica, Int16 estadoFactura)
        {
            SqlCommand _sqlComando = null;
            string sql = sqlSelect + @" WHERE a.IdCajaChica = @IdCajaChica 
                                        and a.Estado = case when @Estado = -1 then a.Estado else @Estado end  
                                        and d.Estado = @EstadoCajaChica and aprobada = 1 
                                        ORDER BY IdFactura DESC ";

            _sqlComando = new SqlCommand(sql, _sqlConn);

            if (_sqlTran != null)
                _sqlComando.Transaction = _sqlTran;

            _sqlComando.Parameters.Add(new SqlParameter("@IdCajaChica", (object)idCajaChica ?? DBNull.Value));
            _sqlComando.Parameters.Add(new SqlParameter("@Estado", (object)estadoFactura ?? DBNull.Value));
            _sqlComando.Parameters.Add(new SqlParameter("@EstadoCajaChica", (object)estadoCajaChica ?? DBNull.Value));

            return CargarReader(_sqlComando.ExecuteReader());
        }

        //public List<FacturaEncabezadoDTO> BuscarFacturasCajaChica(decimal idCajaChica, Int16 estadoCajaChica, Int16 estadoFactura, string identificacion, string serie, decimal numero)
        public List<FacturaEncabezadoDTO> BuscarFacturasCajaChica(decimal idCajaChica, Int16 estadoCajaChica, Int16 estadoFactura, string identificacion, string serie, string numero)
        {
           

//            string sql = @"   SELECT MIN(Encabezado.IdFactura) as IdFactura, Doc.TipoDocumento, Proveedor.NumeroIdentificacion,
//                              Proveedor.Nombre, Encabezado.Serie, Encabezado.Numero, Encabezado.FechaFactura, 
//                              Encabezado.EsEspecial, SUM(Encabezado.IVA) as IVA, SUM(Encabezado.ValorTotal) as ValorTotal, Encabezado.Estado, 
//                              case when Moneda.Moneda is null then Sociedad.Moneda else Moneda.Moneda end Moneda,
//                              ISNULL(Encabezado.FacturaDividida,0) as FacturaDividida
//                              FROM dbo.FacturaEncabezado as Encabezado 
//                              INNER JOIN dbo.Proveedor as Proveedor 
//                                        ON Proveedor.IdProveedor = Encabezado.IdProveedor 
//                              INNER JOIN dbo.TipoDocumento as Doc
//                                        ON doc.IdTipoDocumento = Proveedor.IdTipoDocumento  
//                              INNER JOIN CajaChicaEncabezado as CC
//                                        ON cc.IdCajaChica = Encabezado.IdCajaChica
//                              INNER JOIN dbo.SociedadCentro as SosCent
//                                        ON SosCent.IdSociedadCentro = CC.IdSociedadCentro
//                              LEFT JOIN dbo.SociedadMoneda as SosMon
//                                        ON SosMon.CodigoSociedad = SosCent.CodigoSociedad
//                                        AND SosMon.IdSociedadMoneda = cc.IdSociedadMoneda
//                              LEFT JOIN dbo.Moneda as Moneda 
//                                        ON Moneda.Moneda = SosMon.Moneda
//                              INNER JOIN Sociedad as Sociedad 
//                                        ON Sociedad.CodigoSociedad = SosCent.CodigoSociedad 
//                              WHERE     Encabezado.IdCajaChica = @IdCajaChica 
//                                        and Encabezado.Estado = case when @Estado = -1 then Encabezado.Estado else @Estado end  
//                                        and Proveedor.NumeroIdentificacion = case when @NumeroIdentificacion = '' then Proveedor.NumeroIdentificacion else @NumeroIdentificacion end
//                                        and Encabezado.Serie = case when @Serie = '' then Encabezado.Serie else @Serie end
//                                        and Encabezado.Numero = case when @Numero = '-1' then Encabezado.Numero else @Numero end 
//                              GROUP BY TipoDocumento, NumeroIdentificacion, Proveedor.Nombre, Serie, Numero, FechaFactura, EsEspecial, Encabezado.Estado, 
//                                       Moneda.Moneda, Sociedad.Moneda, Encabezado.FacturaDividida";
                       SqlCommand _sqlComando = null;

            _sqlComando = new SqlCommand("BuscarFacturasRevision", _sqlConn);

            _sqlComando.CommandType = CommandType.StoredProcedure;

            if (_sqlTran != null)
                _sqlComando.Transaction = _sqlTran;

            _sqlComando.Parameters.Add(new SqlParameter("@IdCajaChica", (object)idCajaChica ?? DBNull.Value));
            _sqlComando.Parameters.Add(new SqlParameter("@Estado", (object)estadoFactura ?? DBNull.Value));
            _sqlComando.Parameters.Add(new SqlParameter("@NumeroIdentificacion", (object)identificacion ?? DBNull.Value));
            _sqlComando.Parameters.Add(new SqlParameter("@Serie", (object)serie ?? DBNull.Value));
            _sqlComando.Parameters.Add(new SqlParameter("@Numero", (object)numero ?? DBNull.Value));
           // _sqlComando.Parameters.Add(new SqlParameter("@EstadoCajaChica", (object)estadoCajaChica ?? DBNull.Value));

            return CargarReaderListaRevision(_sqlComando.ExecuteReader());

            //return CargarReader(_sqlComando.ExecuteReader());

        }

        protected List<FacturaEncabezadoDTO> CargarReaderListaRevision(SqlDataReader sqlDataReader)
        {
            List<FacturaEncabezadoDTO> _listaFacturaEncabezadoDto = new List<FacturaEncabezadoDTO>();
            FacturaEncabezadoDTO _facturaEncabezadoDto = new FacturaEncabezadoDTO();

            try
            {
                while (sqlDataReader.Read())
                {
                    _facturaEncabezadoDto = new FacturaEncabezadoDTO();

                    _facturaEncabezadoDto.ID_FACTURA = sqlDataReader.GetDecimal(0);
                    _facturaEncabezadoDto.TIPO_DOCUMENTO = sqlDataReader.IsDBNull(1) ? string.Empty : sqlDataReader.GetString(1);
                    _facturaEncabezadoDto.NUMERO_IDENTIFICACION = sqlDataReader.GetString(2);
                    _facturaEncabezadoDto.NOMBRE_PROVEEDOR = sqlDataReader.GetString(3);
                    _facturaEncabezadoDto.SERIE = sqlDataReader.GetString(4);
                    _facturaEncabezadoDto.NUMERO = sqlDataReader.GetString(5); //sqlDataReader.GetDecimal(5);
                    _facturaEncabezadoDto.FECHA_FACTURA = sqlDataReader.GetDateTime(6);
                    _facturaEncabezadoDto.ES_ESPECIAL = sqlDataReader.GetBoolean(7);
                    _facturaEncabezadoDto.IVA = sqlDataReader.GetDouble(8);
                    _facturaEncabezadoDto.VALOR_TOTAL = sqlDataReader.GetDouble(9);
                    _facturaEncabezadoDto.ESTADO = sqlDataReader.GetInt16(10);
                    _facturaEncabezadoDto.MONEDA = sqlDataReader.GetString(11);
                    _facturaEncabezadoDto.FACTURA_DIVIDIDA = sqlDataReader.IsDBNull(12) ? (bool?)null : sqlDataReader.GetBoolean(12);
                    _facturaEncabezadoDto.IMPUESTO = sqlDataReader.GetDouble(13);
                    _facturaEncabezadoDto.MANDANTE = sqlDataReader.IsDBNull(15) ? string.Empty : sqlDataReader.GetString(15);
                    _facturaEncabezadoDto.INDICADOR = sqlDataReader.IsDBNull(16) ? string.Empty : sqlDataReader.GetString(16);
                    _listaFacturaEncabezadoDto.Add(_facturaEncabezadoDto);
                }

                return _listaFacturaEncabezadoDto;
            }
            finally
            {
                if (sqlDataReader != null) sqlDataReader.Close();
            }
        }

        public List<FacturaEncabezadoDTO> BuscarFacturasAprobadas(decimal idCajaChica, Int16 estado)
        {
            SqlCommand _sqlComando = null;
            string sql = sqlSelect + @" WHERE a.IdCajaChica = @IdCajaChica 
                                        and a.Estado = case when @Estado = -1 then a.Estado else @Estado end  
                                        and a.aprobada is null
                                        ORDER BY IdFactura DESC ";

            _sqlComando = new SqlCommand(sql, _sqlConn);

            if (_sqlTran != null)
                _sqlComando.Transaction = _sqlTran;

            _sqlComando.Parameters.Add(new SqlParameter("@IdCajaChica", (object)idCajaChica ?? DBNull.Value));
            _sqlComando.Parameters.Add(new SqlParameter("@Estado", (object)estado ?? DBNull.Value));

            return CargarReader(_sqlComando.ExecuteReader());
        }

        public Int32 AprobarFactura(decimal idFactura, string usuario)
        {
            SqlCommand _sqlCommando = null;
            string sql = @" UPDATE FacturaEncabezado SET UsuarioModifico = @UsuarioModifico, FechaModificacion = GETDATE(), Aprobada = 1
                            WHERE IdFactura = @IdFactura ";

            _sqlCommando = new SqlCommand(sql, _sqlConn);

            if (_sqlTran != null)
                _sqlCommando.Transaction = _sqlTran;

            _sqlCommando.Parameters.Add(new SqlParameter("@UsuarioModifico", (object)usuario));
            _sqlCommando.Parameters.Add(new SqlParameter("@IdFactura", (object)idFactura));

            return _sqlCommando.ExecuteNonQuery();
        }

        public Int32 RechazarLaFactura(decimal idFactura, string usuario)
        {
            SqlCommand _sqlCommando = null;
            string sql = @" UPDATE FacturaEncabezado SET UsuarioModifico = @UsuarioModifico, FechaModificacion = GETDATE(), Aprobada = 0, Estado = 0
                            WHERE IdFactura = @IdFactura ";

            _sqlCommando = new SqlCommand(sql, _sqlConn);

            if (_sqlTran != null)
                _sqlCommando.Transaction = _sqlTran;

            _sqlCommando.Parameters.Add(new SqlParameter("@UsuarioModifico", (object)usuario));
            _sqlCommando.Parameters.Add(new SqlParameter("@IdFactura", (object)idFactura));

            return _sqlCommando.ExecuteNonQuery();
        }

        public Int32 AceptarFactura(decimal idFactura, string usuario)
        {
            SqlCommand _sqlCommando = null;
            string sql = @" UPDATE FacturaEncabezado SET UsuarioModifico = @UsuarioModifico, FechaModificacion = GETDATE(), Estado = 2
                            WHERE IdFactura = @IdFactura ";

            _sqlCommando = new SqlCommand(sql, _sqlConn);

            if (_sqlTran != null)
                _sqlCommando.Transaction = _sqlTran;

            _sqlCommando.Parameters.Add(new SqlParameter("@UsuarioModifico", (object)usuario));
            _sqlCommando.Parameters.Add(new SqlParameter("@IdFactura", (object)idFactura));

            return _sqlCommando.ExecuteNonQuery();
        }

        public Int32 RechazarFactura(decimal idFactura, string usuario)
        {
            SqlCommand _sqlCommando = null;
            string sql = @" UPDATE FacturaEncabezado SET UsuarioModifico = @UsuarioModifico, FechaModificacion = GETDATE(), Estado = 3
                            WHERE IdFactura = @IdFactura ";

            _sqlCommando = new SqlCommand(sql, _sqlConn);

            if (_sqlTran != null)
                _sqlCommando.Transaction = _sqlTran;

            _sqlCommando.Parameters.Add(new SqlParameter("@UsuarioModifico", (object)usuario));
            _sqlCommando.Parameters.Add(new SqlParameter("@IdFactura", (object)idFactura));

            return _sqlCommando.ExecuteNonQuery();
        }

        public Int32 AnularFactura(decimal idFactura, string usuario)
        {
            SqlCommand _sqlCommando = null;
            string sql = @" UPDATE FacturaEncabezado SET UsuarioModifico = @UsuarioModifico, FechaModificacion = GETDATE(), Estado = 0
                            WHERE IdFactura = @IdFactura ";

            _sqlCommando = new SqlCommand(sql, _sqlConn);

            if (_sqlTran != null)
                _sqlCommando.Transaction = _sqlTran;

            _sqlCommando.Parameters.Add(new SqlParameter("@UsuarioModifico", (object)usuario));
            _sqlCommando.Parameters.Add(new SqlParameter("@IdFactura", (object)idFactura));

            return _sqlCommando.ExecuteNonQuery();
        }

        public Int32 DarVigenciaFactura(decimal idFactura, string usuario)
        {
            SqlCommand _sqlCommando = null;
            string sql = @" UPDATE FacturaEncabezado SET UsuarioModifico = @UsuarioModifico, FechaModificacion = GETDATE(), Estado = 1
                            WHERE IdFactura = @IdFactura ";

            _sqlCommando = new SqlCommand(sql, _sqlConn);

            if (_sqlTran != null)
                _sqlCommando.Transaction = _sqlTran;

            _sqlCommando.Parameters.Add(new SqlParameter("@UsuarioModifico", (object)usuario));
            _sqlCommando.Parameters.Add(new SqlParameter("@IdFactura", (object)idFactura));

            return _sqlCommando.ExecuteNonQuery();
        }


        //Validar facturas divididas que no tengan completo el total 

        public FacturaEncabezadoDTO ValidarFactruaDividida(decimal idCajaChica)
        {
            List<FacturaEncabezadoDTO> listaFacturas = null;
            SqlCommand sqlComando;
            string sql = @"SELECT  distinct idProveedor, Numero, Serie
                            FROM FacturaEncabezado as Fact
                            WHERE 
                            IdCajaChica = @idCajaChica
                            and (Estado = 1 or Estado = 2)
                            and FacturaDividida = 1
                            AND 
                            (Select TOP 1 CONVERT(decimal(20,2),TotalFacturaDividida)
                            FROM FacturaEncabezado as Total
                            Where Total.IdProveedor = Fact.IdProveedor
                            AND   Total.Numero = Fact.Numero
                            AND	  Total.Serie = Fact.Serie
                            and (Estado = 1 or Estado = 2)
                            Order by IdFactura) <> 
                            (Select SUM(convert(decimal(20,2), ValorTotal))
                            FROM FacturaEncabezado as Total
                            Where Total.IdProveedor = Fact.IdProveedor
                            AND   Total.Numero = Fact.Numero
                            AND	  Total.Serie = Fact.Serie
                            AND   (Estado = 1 or Estado = 2))";
            sqlComando = new SqlCommand(sql, _sqlConn);
            sqlComando.CommandType = CommandType.Text;

            if (_sqlTran != null)
                sqlComando.Transaction = _sqlTran;

            sqlComando.Parameters.Add(new SqlParameter("@idCajaChica", (object)idCajaChica ?? DBNull.Value));

            listaFacturas = CargarValidacionFacturaDividida(sqlComando.ExecuteReader());

            return listaFacturas.Count > 0 ? listaFacturas[0] : null;


        }

        protected List<FacturaEncabezadoDTO> CargarValidacionFacturaDividida(SqlDataReader sqlReader)
        {
            FacturaEncabezadoDTO _factruaEncabezadoDto = null;
            List<FacturaEncabezadoDTO> _listaFacturaEncabezadoDto = new List<FacturaEncabezadoDTO>();
            int cont = 0;
            while (sqlReader.Read())
            {
                cont = 1;
                _factruaEncabezadoDto = new FacturaEncabezadoDTO();

                _factruaEncabezadoDto.SERIE = sqlReader.GetString(2);
                _factruaEncabezadoDto.NUMERO = sqlReader.GetString(1);//sqlReader.GetDecimal(1);


                _listaFacturaEncabezadoDto.Add(_factruaEncabezadoDto);
                break;
            }
            sqlReader.Close();
            if (cont == 1)
                throw new ExcepcionesDIPCMI("La factura " + _factruaEncabezadoDto.SERIE + "-" + _factruaEncabezadoDto.NUMERO + " no coinciden con el total registrado");
            return _listaFacturaEncabezadoDto;


        }

        //Buscar Total de Factura y Acumulado 
        //public FacturaEncabezadoDTO BuscarTotalFactura(int idProveedor, decimal NoFactura, string serie)
        public FacturaEncabezadoDTO BuscarTotalFactura(int idProveedor, string NoFactura, string serie)
        {
            List<FacturaEncabezadoDTO> listaFacturas = null;
            SqlCommand sqlComando;
            string sql = @"SELECT  TOP 1 TotalFacturaDividida,

                        (SELECT SUM(ValorTotal) 
                        FROM FacturaEncabezado as Suma
                        WHERE Suma.Serie = total.Serie 
                        AND   Suma.IdProveedor = total.IdProveedor
                        AND	  Suma.Numero = total.Numero
                        AND (Suma.Estado = 1 or Suma.Estado = 2)) as acumulado, 
                        FechaFactura, FacturaDividida, IdCajaChica
                       
                            FROM FacturaEncabezado AS total
                            WHERE IdProveedor = @idProveedor
                            and Serie = @serie
                            and  CONVERT(decimal, Numero) = @NoFactura
                            and (Estado = 1 or Estado = 2)
                            ORDER BY IdFactura";

            sqlComando = new SqlCommand(sql, _sqlConn);
            sqlComando.CommandType = CommandType.Text;

            if (_sqlTran != null)
                sqlComando.Transaction = _sqlTran;
            decimal FacturaNumero;
            FacturaNumero = Convert.ToDecimal(NoFactura);
            sqlComando.Parameters.Add(new SqlParameter("@IdProveedor", (object)idProveedor ?? DBNull.Value));
            sqlComando.Parameters.Add(new SqlParameter("@serie", (object)serie ?? DBNull.Value));
            sqlComando.Parameters.Add(new SqlParameter("@NoFactura", (object)FacturaNumero ?? DBNull.Value));

            listaFacturas = CargarReaderTotalFactura(sqlComando.ExecuteReader());

            return listaFacturas.Count > 0 ? listaFacturas[0] : null;
        }

        protected List<FacturaEncabezadoDTO> CargarReaderTotalFactura(SqlDataReader sqlReader)
        {
            FacturaEncabezadoDTO _factruaEncabezadoDto = null;
            List<FacturaEncabezadoDTO> _listaFacturaEncabezadoDto = new List<FacturaEncabezadoDTO>();

            while (sqlReader.Read())
            {
                _factruaEncabezadoDto = new FacturaEncabezadoDTO();

                _factruaEncabezadoDto.FACTURA_DIVIDIDA = sqlReader.IsDBNull(3) ? (bool?)null : sqlReader.GetBoolean(3);
                if (_factruaEncabezadoDto.FACTURA_DIVIDIDA == true)
                {
                    _factruaEncabezadoDto.TOTALFACTURADIVIDIDA = sqlReader.GetDouble(0);
                    _factruaEncabezadoDto.ACUMULADO = Convert.ToDecimal(sqlReader.GetDouble(1));
                    _factruaEncabezadoDto.FECHA_FACTURA = sqlReader.GetDateTime(2);
                    _factruaEncabezadoDto.OBSERVACIONES = _factruaEncabezadoDto.FECHA_FACTURA.ToString("yyyy/MM/dd");
                    _factruaEncabezadoDto.ID_CAJA_CHICA = sqlReader.GetDecimal(4);
                }


                _listaFacturaEncabezadoDto.Add(_factruaEncabezadoDto);
            }
            sqlReader.Close();

            return _listaFacturaEncabezadoDto;
        }


        //INI------------------------Valida si la caja chica esta abierta------------------------------------
        public bool ValidaCajaChicaAbierta(decimal idCajaChica)
        {
            SqlCommand _sqlComando = null;
            string sql = @" SELECT COUNT(IdCajaChica) FROM CajaChicaEncabezado
                            WHERE Estado <> 1 
                            AND   IdCajaChica = @idCajaChica";

            _sqlComando = new SqlCommand(sql, _sqlConn);

            if (_sqlTran != null)
                _sqlComando.Transaction = _sqlTran;

            _sqlComando.Parameters.Add(new SqlParameter("@idCajaChica", (object)idCajaChica ?? DBNull.Value));

            return Convert.ToInt32(_sqlComando.ExecuteScalar()) > 0 ? false : true;
        }

        //FIN------------------------Valida si la caja chica esta abierta------------------------------------
        public bool BuscaRegimenProveedor(Int32 idproveedor)
        {
            SqlCommand _sqlComando = null;
            string sql = @" SELECT count(IdProveedor) FROM dbo.proveedor    
                            WHERE idproveedor = @idproveedor
                            AND Regimen IN ('Pequeño Contribuyente', 'Mediano Contribuyente')";

            _sqlComando = new SqlCommand(sql, _sqlConn);

            if (_sqlTran != null)
                _sqlComando.Transaction = _sqlTran;

            _sqlComando.Parameters.Add(new SqlParameter("@idproveedor", (object)idproveedor ?? DBNull.Value));

            return Convert.ToInt32(_sqlComando.ExecuteScalar()) > 0 ? false : true;
        }

        public bool BuscaTipoProveedor(Int32 idproveedor)
        {
            SqlCommand _sqlComando = null;
            string sql = @" SELECT count(IdProveedor) FROM dbo.proveedor    
                            WHERE idproveedor = @idproveedor
                            AND TIPO = 1";

            _sqlComando = new SqlCommand(sql, _sqlConn);

            if (_sqlTran != null)
                _sqlComando.Transaction = _sqlTran;

            _sqlComando.Parameters.Add(new SqlParameter("@idproveedor", (object)idproveedor ?? DBNull.Value));

            return Convert.ToInt32(_sqlComando.ExecuteScalar()) > 0 ? false : true;
        }

        //INI--------------------INSERTA DATOS INELDAT------------------------------------------------------
        public Int32 InsertarIneldat(decimal IdFactura)
        {
            SqlCommand _sqlComando = null;


            _sqlComando = new SqlCommand("SPIneldat", _sqlConn);
            _sqlComando.CommandType = CommandType.StoredProcedure;

            if (_sqlTran != null)
                _sqlComando.Transaction = _sqlTran;

            _sqlComando.Parameters.Add(new SqlParameter("@IdFactura", (object)IdFactura ?? DBNull.Value));

            return Convert.ToInt32(_sqlComando.ExecuteNonQuery());
        }
        //FIN--------------------INSERTA DATOS INELDAT------------------------------------------------------

        public bool BuscaRegimenProveedorCR(Int32 idproveedor)
        {
            SqlCommand _sqlComando = null;
            string sql = @" SELECT count(IdProveedor) FROM dbo.proveedor    
                            WHERE idproveedor = @idproveedor
                            AND Regimen IN ('Regimen Simplificado')";

            _sqlComando = new SqlCommand(sql, _sqlConn);

            if (_sqlTran != null)
                _sqlComando.Transaction = _sqlTran;

            _sqlComando.Parameters.Add(new SqlParameter("@idproveedor", (object)idproveedor ?? DBNull.Value));

            return Convert.ToInt32(_sqlComando.ExecuteScalar()) > 0 ? false : true;
        }

        //INI------------------------Retención 1% IPES------------------------------------
        public double BuscaRetencion(string pais, string identificador)
        {
            SqlCommand _sqlComando = null;
            string sql = @" SELECT TOP 1 Retencion FROM dbo.IdentificadoresIVA
                            WHERE dominio = @pais AND IdentificadorIVA = @identificador";

            _sqlComando = new SqlCommand(sql, _sqlConn);

            if (_sqlTran != null)
                _sqlComando.Transaction = _sqlTran;

            _sqlComando.Parameters.Add(new SqlParameter("@pais", (object)pais ?? DBNull.Value));
            _sqlComando.Parameters.Add(new SqlParameter("@identificador", (object)identificador ?? DBNull.Value));

            return Convert.ToDouble(_sqlComando.ExecuteScalar());

            //return Convert.ToInt32(_sqlComando.ExecuteScalar()) > 0 ? false : true;
        }

        //FIN------------------------Retención 1% IPES------------------------------------
        #endregion
    }
}
