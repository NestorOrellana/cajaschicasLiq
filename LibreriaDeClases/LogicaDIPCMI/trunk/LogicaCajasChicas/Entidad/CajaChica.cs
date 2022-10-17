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
    public class CajaChica
    {
        #region Declaraciones

        private SqlConnection _sqlConn = null;
        private SqlTransaction _sqlTran = null;



        protected string sqlSelect = @" select distinct a.IdCajaChica, a.IdSociedadCentro, a.Correlativo, a.NumeroCajaChica, a.Descripcion, a.Estado, a.UsuarioAlta,
                                        a.FechaCreacion, a.UsuarioModificacion, a.FechaModificacion, c.CodigoSociedad, c.Nombre, cast(d.IdCentro as smallint) IdCentro, d.Nombre,
                                        COUNT(e.IdCajaChica) facturas, SUM(isnull(e.ValorTotal,0)) TotalCC,
                                        cast(c.CodigoSociedad as varchar(4))+ '-' + cast(d.IdCentro as varchar(4)) + '-' +a.NumeroCajaChica +'-' + RIGHT(REPLICATE('0', 6)+ CAST(a.Correlativo  AS VARCHAR(6)), 6)   CodigoCC, TipoOperacion,
                                        a.EncargadoCC, a.IdSociedadMoneda, case when g.Moneda Is null then c.Moneda else g.Moneda end moneda, c.Pais,
                                        a.FechaInicioViaje, a.FechaFinViaje, a.Objetivo, a.NumeroDias, a.ViaticosRecibidos, a.IdNivel, a.ViaticosLocales
                                        from CajaChicaEncabezado a
                                        inner join SociedadCentro b on b.IdSociedadCentro = a.IdSociedadCentro and b.Alta = 1
                                        inner join Sociedad c on c.CodigoSociedad = b.CodigoSociedad and c.Alta = 1
                                        inner join Centro d on d.IdCentro = b.IdCentro and d.Alta = 1
                                        left join FacturaEncabezado e on e.IdCajaChica = a.IdCajaChica and (e.Estado != 0 and e.Estado != 3) and e.Aprobada = 1 
                                        left join SociedadMoneda f on f.IdSociedadMoneda = a.IdSociedadMoneda
                                        left join Moneda g on g.Moneda = f.Moneda ";

        protected string sqlInsert = @" INSERT INTO CajaChicaEncabezado (IdSociedadCentro, Correlativo, NumeroCajaChica, Descripcion, UsuarioAlta, TipoOperacion, EncargadoCC, IdSociedadMoneda)
                                                                 VALUES (@IdSociedadCentro, @Correlativo, @NumeroCajaChica, @Descripcion, @UsuarioAlta, @TipoOperacion, @EncargadoCC, @IdSociedadMoneda)
                                        SELECT @@IDENTITY ";

        protected string sqlInsertVl = @" INSERT INTO CajaChicaEncabezado (IdSociedadCentro, Correlativo, NumeroCajaChica, Descripcion, UsuarioAlta, TipoOperacion, EncargadoCC, IdSociedadMoneda, 
                                        FechaInicioViaje, FechaFinViaje, Objetivo, NumeroDias, ViaticosRecibidos, IdNivel, ViaticosLocales) 
                                        VALUES (@IdSociedadCentro, @Correlativo, @NumeroCajaChica, @Descripcion, @UsuarioAlta, @TipoOperacion, @EncargadoCC, @IdSociedadMoneda, 
                                        @FechaInicioViaje, @FechaFinViaje, @Objetivo, @NumeroDias, @ViaticosRecibidos, @IdNivel, @ViaticosLocales)
                                        SELECT @@IDENTITY ";

        protected string sqlUpdate = @" UPDATE CajaChicaEncabezado SET IdSociedadCentro = @IdSociedadCentro, NumeroCajaChica = @NumeroCajaChica, Descripcion = @Descripcion, UsuarioModificacion = @UsuarioModificacion, 
                                        FechaModificacion = GETDATE(), TipoOperacion = @TipoOperacion
                                        WHERE     (IdCajaChica = @IdCajaChica) ";

        protected string sqlUpdateVl = @" UPDATE CajaChicaEncabezado SET IdSociedadCentro = @IdSociedadCentro, NumeroCajaChica = @NumeroCajaChica, Descripcion = @Descripcion, 
                                        FechaInicioViaje = @FechaInicioViaje, FechaFinViaje = @FechaFinViaje, Objetivo = @Objetivo, NumeroDias = @NumeroDias, ViaticosRecibidos = @ViaticosRecibidos, 
                                        IdNivel = @IdNivel, ViaticosLocales = @ViaticosLocales, UsuarioModificacion = @UsuarioModificacion, 
                                        FechaModificacion = GETDATE(), TipoOperacion = @TipoOperacion
                                        WHERE     (IdCajaChica = @IdCajaChica) ";

        #endregion

        #region Constructores

        public CajaChica(SqlConnection sqlConn)
        {
            _sqlConn = sqlConn;
        }

        public CajaChica(SqlConnection sqlConn, SqlTransaction sqlTran)
        {
            _sqlConn = sqlConn;
            _sqlTran = sqlTran;
        }

        #endregion

        #region Metodos

        public CajaChicaEncabezadoDTO EjecutarSentenciaSelect(decimal idCajaChica)
        {
            List<CajaChicaEncabezadoDTO> _listaCajaChica = null;
            SqlCommand _sqlComando = null;
            string sql = sqlSelect + @"Where a.idCajaChica = @IdCajaChica 
                                       group by a.IdCajaChica, a.IdSociedadCentro, a.Correlativo, a.NumeroCajaChica, a.Descripcion, a.Estado, a.UsuarioAlta,
	                                   a.FechaCreacion, a.UsuarioModificacion, a.FechaModificacion, c.CodigoSociedad, c.Nombre, d.IdCentro, d.Nombre,
                                       b.CodigoSociedad, d.IdCentro, a.correlativo, TipoOperacion, a.EncargadoCC, a.IdSociedadMoneda, g.Moneda, c.Moneda, c.Pais, 
                                       a.FechaInicioViaje, a.FechaFinViaje, a.Objetivo, a.NumeroDias, a.ViaticosRecibidos, a.IdNivel, a.ViaticosLocales
                                       order by a.IdCajaChica desc ";

            _sqlComando = new SqlCommand(sql, _sqlConn);
            _sqlComando.CommandType = CommandType.Text;

            if (_sqlTran != null)
                _sqlComando.Transaction = _sqlTran;

            _sqlComando.Parameters.Add(new SqlParameter("@IdCajaChica", (object)idCajaChica ?? DBNull.Value));

            _listaCajaChica = CargarReader(_sqlComando.ExecuteReader());

            return _listaCajaChica.Count > 0 ? _listaCajaChica[0] : null;
        }

        public Int32 EjecutarSentenciaInsert(CajaChicaEncabezadoDTO _cajaChicaDto)
        {
            SqlCommand _sqlComando = null;

            if (_cajaChicaDto.ViaticosLocales)
            {
                _sqlComando = new SqlCommand(sqlInsertVl, _sqlConn);
            }
            else
            {
                _sqlComando = new SqlCommand(sqlInsert, _sqlConn);
            }
            
            _sqlComando.CommandType = CommandType.Text;

            if (_sqlTran != null)
                _sqlComando.Transaction = _sqlTran;

            _sqlComando.Parameters.Add(new SqlParameter("@IdSociedadCentro", (object)_cajaChicaDto.ID_SOCIEDAD_CENTRO ?? DBNull.Value));
            _sqlComando.Parameters.Add(new SqlParameter("@Correlativo", (object)_cajaChicaDto.CORRELATIVO ?? DBNull.Value));
            _sqlComando.Parameters.Add(new SqlParameter("@NumeroCajaChica", (object)_cajaChicaDto.CAJA_CHICA_SAP ?? DBNull.Value));
            _sqlComando.Parameters.Add(new SqlParameter("@Descripcion", (object)_cajaChicaDto.DESCRIPCION ?? DBNull.Value));
            _sqlComando.Parameters.Add(new SqlParameter("@UsuarioAlta", (object)_cajaChicaDto.USUARIO_MANTENIMIENTO.USUARIO_ALTA ?? DBNull.Value));
            _sqlComando.Parameters.Add(new SqlParameter("@TipoOperacion", (object)_cajaChicaDto.TIPO_OPERACION ?? DBNull.Value));
            _sqlComando.Parameters.Add(new SqlParameter("@EncargadoCC", (object)_cajaChicaDto.NOMBRE_CC ?? DBNull.Value));
            _sqlComando.Parameters.Add(new SqlParameter("@IdSociedadMoneda", (object)_cajaChicaDto.ID_SOCIEDAD_MONEDA ?? DBNull.Value));

            if (_cajaChicaDto.ViaticosLocales)
            {
                _sqlComando.Parameters.Add(new SqlParameter("@FechaInicioViaje", (object)_cajaChicaDto.FechaInicioViaje ?? DBNull.Value));
                _sqlComando.Parameters.Add(new SqlParameter("@FechaFinViaje", (object)_cajaChicaDto.FechaFinViaje ?? DBNull.Value));
                _sqlComando.Parameters.Add(new SqlParameter("@Objetivo", (object)_cajaChicaDto.Objetivo ?? DBNull.Value));
                _sqlComando.Parameters.Add(new SqlParameter("@NumeroDias", (object)_cajaChicaDto.NumeroDias ?? DBNull.Value));
                _sqlComando.Parameters.Add(new SqlParameter("@ViaticosRecibidos", (object)_cajaChicaDto.ViaticosRecibidos ?? DBNull.Value));
                _sqlComando.Parameters.Add(new SqlParameter("@IdNivel", (object)_cajaChicaDto.IdNivel ?? DBNull.Value));
                _sqlComando.Parameters.Add(new SqlParameter("@ViaticosLocales", (object)_cajaChicaDto.ViaticosLocales ?? DBNull.Value));
            }



            return Convert.ToInt32(_sqlComando.ExecuteScalar());
        }

        public decimal EjecutarSentenciaUpdate(CajaChicaEncabezadoDTO _cajaChicaDto)
        {
            SqlCommand _sqlComando = null;

            if (_cajaChicaDto.ViaticosLocales)
            {
                _sqlComando = new SqlCommand(sqlUpdateVl, _sqlConn);
            }
            else
            {
                _sqlComando = new SqlCommand(sqlUpdate, _sqlConn);
            }

            _sqlComando.CommandType = CommandType.Text;

            if (_sqlTran != null)
                _sqlComando.Transaction = _sqlTran;


            _sqlComando.Parameters.Add(new SqlParameter("@IdSociedadCentro", (object)_cajaChicaDto.ID_SOCIEDAD_CENTRO ?? DBNull.Value));
            _sqlComando.Parameters.Add(new SqlParameter("@NumeroCajaChica", (object)_cajaChicaDto.CAJA_CHICA_SAP ?? DBNull.Value));
            _sqlComando.Parameters.Add(new SqlParameter("@Descripcion", (object)_cajaChicaDto.DESCRIPCION ?? DBNull.Value));
            _sqlComando.Parameters.Add(new SqlParameter("@UsuarioModificacion", (object)_cajaChicaDto.USUARIO_MANTENIMIENTO.USUARIO_MODIFICO ?? DBNull.Value));
            _sqlComando.Parameters.Add(new SqlParameter("@TipoOperacion", (object)_cajaChicaDto.TIPO_OPERACION ?? DBNull.Value));
            _sqlComando.Parameters.Add(new SqlParameter("@IdCajaChica", (object)_cajaChicaDto.ID_CAJA_CHICA ?? DBNull.Value));
            _sqlComando.Parameters.Add(new SqlParameter("@IdSociedadMoneda", (object)_cajaChicaDto.ID_SOCIEDAD_MONEDA ?? DBNull.Value));


            if (_cajaChicaDto.ViaticosLocales)
            {
                _sqlComando.Parameters.Add(new SqlParameter("@FechaInicioViaje", (object)_cajaChicaDto.FechaInicioViaje ?? DBNull.Value));
                _sqlComando.Parameters.Add(new SqlParameter("@FechaFinViaje", (object)_cajaChicaDto.FechaFinViaje ?? DBNull.Value));
                _sqlComando.Parameters.Add(new SqlParameter("@Objetivo", (object)_cajaChicaDto.Objetivo ?? DBNull.Value));
                _sqlComando.Parameters.Add(new SqlParameter("@NumeroDias", (object)_cajaChicaDto.NumeroDias ?? DBNull.Value));
                _sqlComando.Parameters.Add(new SqlParameter("@ViaticosRecibidos", (object)_cajaChicaDto.ViaticosRecibidos ?? DBNull.Value));
                _sqlComando.Parameters.Add(new SqlParameter("@IdNivel", (object)_cajaChicaDto.IdNivel ?? DBNull.Value));
                _sqlComando.Parameters.Add(new SqlParameter("@ViaticosLocales", (object)_cajaChicaDto.ViaticosLocales ?? DBNull.Value));
            }


            return Convert.ToDecimal(_sqlComando.ExecuteScalar());
        }

        public Int32 EjecutarSentenciaDelete()
        {
            throw new NotImplementedException("Método no implementado.");
        }

        protected List<CajaChicaEncabezadoDTO> CargarReader(SqlDataReader sqlReader)
        {
            CajaChicaEncabezadoDTO _cajaChica = null;
            List<CajaChicaEncabezadoDTO> _listaCajaChica = new List<CajaChicaEncabezadoDTO>();

            try
            {
                while (sqlReader.Read())
                {
                    _cajaChica = new CajaChicaEncabezadoDTO();

                    _cajaChica.ID_CAJA_CHICA = sqlReader.GetDecimal(0);
                    _cajaChica.ID_SOCIEDAD_CENTRO = sqlReader.GetInt32(1);
                    _cajaChica.CORRELATIVO = sqlReader.GetInt32(2);
                    _cajaChica.CAJA_CHICA_SAP = sqlReader.GetString(3);
                    _cajaChica.DESCRIPCION = sqlReader.GetString(4);
                    _cajaChica.ESTADO = sqlReader.GetInt16(5);
                    _cajaChica.USUARIO_MANTENIMIENTO.USUARIO_ALTA = sqlReader.GetString(6);
                    _cajaChica.USUARIO_MANTENIMIENTO.FECHA_ALTA = sqlReader.GetDateTime(7);
                    _cajaChica.USUARIO_MANTENIMIENTO.USUARIO_MODIFICO = sqlReader.IsDBNull(8) ? null : sqlReader.GetString(8);
                    _cajaChica.USUARIO_MANTENIMIENTO.FECHA_MODIFICACION = sqlReader.IsDBNull(9) ? (DateTime?)null : sqlReader.GetDateTime(9);
                    _cajaChica.CODIGO_SOCIEDAD = sqlReader.GetString(10);
                    _cajaChica.NOMBRE_EMPRESA = sqlReader.GetString(11);
                    _cajaChica.CODIGO_CENTRO = sqlReader.GetInt16(12);
                    _cajaChica.NOMBRE_CENTRO = sqlReader.GetString(13);
                    _cajaChica.FACTURAS_CC = sqlReader.GetInt32(14);
                    _cajaChica.MONTO_CC = sqlReader.GetDouble(15);
                    _cajaChica.CODIGO_CC = sqlReader.GetString(16);
                    _cajaChica.TIPO_OPERACION = sqlReader.GetString(17);
                    _cajaChica.NOMBRE_CC = sqlReader.IsDBNull(18) ? string.Empty : sqlReader.GetString(18);
                    _cajaChica.ID_SOCIEDAD_MONEDA = sqlReader.IsDBNull(19) ? (Int16?)null : sqlReader.GetInt16(19);
                    _cajaChica.MONEDA = sqlReader.GetString(20);
                    _cajaChica.PAIS = sqlReader.GetString(21);
                    _cajaChica.FechaInicioViaje = sqlReader.IsDBNull(22) ? (DateTime?)null : sqlReader.GetDateTime(22);
                    _cajaChica.FechaFinViaje = sqlReader.IsDBNull(23) ? (DateTime?)null : sqlReader.GetDateTime(23);
                    _cajaChica.Objetivo = sqlReader.IsDBNull(24) ? "" : sqlReader.GetString(24);
                    _cajaChica.NumeroDias = sqlReader.IsDBNull(25) ? 0 : sqlReader.GetInt32(25);
                    _cajaChica.ViaticosRecibidos = sqlReader.IsDBNull(26) ? 0 : sqlReader.GetDecimal(26);
                    _cajaChica.IdNivel = sqlReader.IsDBNull(27) ? 0 : sqlReader.GetInt32(27);
                    _cajaChica.ViaticosLocales = sqlReader.IsDBNull(28) ? false : sqlReader.GetBoolean(28);

                    _listaCajaChica.Add(_cajaChica);
                }
            }
            finally
            {
                if (sqlReader != null) sqlReader.Close();
            }
            return _listaCajaChica;
        }

        public List<CajaChicaEncabezadoDTO> ListarCajasChicas()
        {
            SqlCommand _sqlComando = null;
            string sql = sqlSelect;

            _sqlComando = new SqlCommand(sql, _sqlConn);
            _sqlComando.CommandType = CommandType.Text;

            if (_sqlTran != null)
                _sqlComando.Transaction = _sqlTran;

            return CargarReader(_sqlComando.ExecuteReader());
        }

        public Int32 BuscarCorrelativoCC(int idSociedadCentro, string numeroCajaChicaSAP)
        {
            SqlCommand _sqlComando = null;
            string sql = @" select count(IdCajaChica) 
                            from CajaChicaEncabezado
                            where IdSociedadCentro = @IdSociedadCentro 
                            and NumeroCajaChica = @NumeroCajaChica";

            _sqlComando = new SqlCommand(sql, _sqlConn);
            _sqlComando.CommandType = CommandType.Text;

            if (_sqlTran != null)
                _sqlComando.Transaction = _sqlTran;

            _sqlComando.Parameters.Add(new SqlParameter("@IdSociedadCentro", (object)idSociedadCentro ?? DBNull.Value));
            _sqlComando.Parameters.Add(new SqlParameter("@NumeroCajaChica", (object)numeroCajaChicaSAP ?? DBNull.Value));

            return Convert.ToInt32(_sqlComando.ExecuteScalar()) + 1;
        }

        public List<CajaChicaEncabezadoDTO> BuscarCajasChicasUsuario(Int16 idCentro, string usuario, Int16 estado, string codigoCC)
        {
            string[] CajaChica = codigoCC.Split('-');
            string codigosociedad = "";
            string idcentro = "";
            string numerocc = "";
            string correlativo = "";

            if (CajaChica.Length > 3)
            {
                codigosociedad = CajaChica[0];
                idcentro = CajaChica[1];
                numerocc = CajaChica[2];
                correlativo = CajaChica[3].ToString();
            }
            else if (CajaChica.Length > 2)
            {
                codigosociedad = CajaChica[0];
                idcentro = CajaChica[1];
                numerocc = CajaChica[2];
            }
            else if (CajaChica.Length > 1)
            {
                codigosociedad = CajaChica[0];
                idcentro = CajaChica[1];
            }
            else if (CajaChica.Length > 1)
            {
                codigosociedad = CajaChica[0];
            }

            SqlCommand _sqlComando = null;
            string sql = sqlSelect + @"where b.IdSociedadCentro = case when @IdCentro = 0 then b.IdSociedadCentro else @IdCentro end
                                        and a.UsuarioAlta = @Usuario 
                                        and (a.Estado = case when @Estado = -1 then a.Estado else @Estado end )
                                        and (c.CodigoSociedad = case when @CodigoSociedad = '' then c.CodigoSociedad else @CodigoSociedad end)
                                        and (d.IdCentro = case when @IdCentroCC = '' then d.IdCentro else @IdCentroCC end)
                                        and (a.NumeroCajaChica = case when @NumeroCajaChica = '' then a.NumeroCajaChica else @NumeroCajaChica end)
                                        and (a.Correlativo = case when @Correlativo = '' then a.Correlativo else @Correlativo end)
                                        group by a.IdCajaChica, a.IdSociedadCentro, a.Correlativo, a.NumeroCajaChica, a.Descripcion, a.Estado, a.UsuarioAlta,
                                        a.FechaCreacion, a.UsuarioModificacion, a.FechaModificacion, c.CodigoSociedad, c.Nombre, d.IdCentro, d.Nombre,
                                        b.CodigoSociedad, d.IdCentro, a.correlativo, TipoOperacion, a.EncargadoCC, a.IdSociedadMoneda, g.Moneda, c.Moneda, c.Pais, 
                                        a.FechaInicioViaje, a.FechaFinViaje, a.Objetivo, a.NumeroDias, a.ViaticosRecibidos, a.IdNivel, a.ViaticosLocales
                                        order by a.IdCajaChica desc";


            //                        @" where b.IdSociedadCentro = case when @IdCentro = 0 then b.IdSociedadCentro else @IdCentro end
            //                                        and a.UsuarioAlta = @Usuario 
            //                                        and a.Estado = case when @Estado = -1 then a.Estado else @Estado end 
            //                                        group by a.IdCajaChica, a.IdSociedadCentro, a.Correlativo, a.NumeroCajaChica, a.Descripcion, a.Estado, a.UsuarioAlta,
            //                                        a.FechaCreacion, a.UsuarioModificacion, a.FechaModificacion, c.CodigoSociedad, c.Nombre, d.IdCentro, d.Nombre,
            //                                        b.CodigoSociedad, d.IdCentro, a.correlativo, TipoOperacion ";

            _sqlComando = new SqlCommand(sql, _sqlConn);
            _sqlComando.CommandType = CommandType.Text;

            if (_sqlTran != null)
                _sqlComando.Transaction = _sqlTran;

            _sqlComando.Parameters.Add(new SqlParameter("@IdCentro", (object)idCentro ?? DBNull.Value));
            _sqlComando.Parameters.Add(new SqlParameter("@Usuario", (object)usuario ?? DBNull.Value));
            _sqlComando.Parameters.Add(new SqlParameter("@Estado", (object)estado ?? DBNull.Value));

            _sqlComando.Parameters.Add(new SqlParameter("@CodigoSociedad", (object)codigosociedad ?? DBNull.Value));
            _sqlComando.Parameters.Add(new SqlParameter("@IdCentroCC", (object)idcentro ?? DBNull.Value));
            _sqlComando.Parameters.Add(new SqlParameter("@NumeroCajaChica", (object)numerocc ?? DBNull.Value));
            _sqlComando.Parameters.Add(new SqlParameter("@Correlativo", (object)correlativo ?? DBNull.Value));

            return CargarReader(_sqlComando.ExecuteReader());
        }

        public List<CajaChicaEncabezadoDTO> BuscarCajasChicas(string codigoSociedad, Int16 idCentro, Int16 estado)
        {
            SqlCommand _sqlComando = null;
            string sql = sqlSelect + @" where a.CodigoSociedad = case when @CodigoSociedad = '' then a.CodigoSociedad else @CodigoSociedad end
										and a.IdCentro = case when @IdCentro = 0 then a.IdCentro else @IdCentro end 
                                        and a.Estado = @Estado
                                        group by a.IdCajaChica, a.IdSociedadCentro, a.Correlativo, a.NumeroCajaChica, a.Descripcion, a.Estado, a.UsuarioAlta,
	                                    a.FechaCreacion, a.UsuarioModificacion, a.FechaModificacion, c.CodigoSociedad, c.Nombre, d.IdCentro, d.Nombre,
	                                    b.CodigoSociedad, d.IdCentro, a.correlativo, TipoOperacion, a.EncargadoCC, a.IdSociedadMoneda, g.Moneda, c.Moneda, c.Pais, 
                                        a.FechaInicioViaje, a.FechaFinViaje, a.Objetivo, a.NumeroDias, a.ViaticosRecibidos, a.IdNivel, a.ViaticosLocales
                                        order by a.IdCajaChica desc ";

            _sqlComando = new SqlCommand(sql, _sqlConn);
            _sqlComando.CommandType = CommandType.Text;

            if (_sqlTran != null)
                _sqlComando.Transaction = _sqlTran;

            _sqlComando.Parameters.Add(new SqlParameter("@CodigoSociedad", (object)codigoSociedad ?? DBNull.Value));
            _sqlComando.Parameters.Add(new SqlParameter("@IdCentro", (object)idCentro ?? DBNull.Value));
            _sqlComando.Parameters.Add(new SqlParameter("@Estado", (object)estado ?? DBNull.Value));

            return CargarReader(_sqlComando.ExecuteReader());
        }

        public List<CajaChicaEncabezadoDTO> BuscarCajasChicasRevision(string usuario, Int32 idSociedadCentro, Int32 codigoSociedad, Int32 numeroCajachica, Int32 correlativo)
        {
            SqlCommand _sqlComando = null;
            //            string sql = sqlSelect + @" inner join UsuarioSociedadCentro f on f.IdSociedadCentro = b.IdSociedadCentro
            //                                        where f.Usuario = @usuario
            //		                                and f.IdSociedadCentro = @idSociedadCentro
            //		                                and a.Estado = 2
            //		                                group by a.IdCajaChica, a.IdSociedadCentro, a.Correlativo, a.NumeroCajaChica, a.Descripcion, a.Estado, a.UsuarioAlta,
            //		                                a.FechaCreacion, a.UsuarioModificacion, a.FechaModificacion, c.CodigoSociedad, c.Nombre, d.IdCentro, d.Nombre,
            //		                                b.CodigoSociedad, d.IdCentro, a.correlativo, TipoOperacion ";

            string sql = sqlSelect + @" inner join UsuarioSociedadCentro h on h.IdSociedadCentro = b.IdSociedadCentro and h.alta = 1
                                        where h.Usuario = @Usuario
                                        and h.IdSociedadCentro = case when @idSociedadCentro = '0' then h.IdSociedadCentro else @idSociedadCentro end
                                        and c.CodigoSociedad = case when @CodigoSociedad = '0' then c.CodigoSociedad else @CodigoSociedad end
                                        and a.NumeroCajaChica = case when @NumeroCajaChica = '0' then a.NumeroCajaChica else @NumeroCajaChica end
                                        and a.Correlativo = case when @Correlativo = '0' then a.Correlativo else @Correlativo end
                                        and a.Estado = 2
                                        and e.Estado = 1
                                        group by a.IdCajaChica, a.IdSociedadCentro, a.Correlativo, a.NumeroCajaChica, a.Descripcion, a.Estado, a.UsuarioAlta,
                                        a.FechaCreacion, a.UsuarioModificacion, a.FechaModificacion, c.CodigoSociedad, c.Nombre, d.IdCentro, d.Nombre,
                                        b.CodigoSociedad, d.IdCentro, a.correlativo, TipoOperacion, a.EncargadoCC, a.IdSociedadMoneda, g.Moneda, c.Moneda, c.Pais, 
                                        a.FechaInicioViaje, a.FechaFinViaje, a.Objetivo, a.NumeroDias, a.ViaticosRecibidos, a.IdNivel, a.ViaticosLocales
                                        order by a.IdCajaChica desc ";

            _sqlComando = new SqlCommand(sql, _sqlConn);
            _sqlComando.CommandType = CommandType.Text;

            if (_sqlTran != null)
                _sqlComando.Transaction = _sqlTran;

            _sqlComando.Parameters.Add(new SqlParameter("@usuario", (object)usuario ?? DBNull.Value));
            _sqlComando.Parameters.Add(new SqlParameter("@idSociedadCentro", (object)idSociedadCentro ?? DBNull.Value));
            _sqlComando.Parameters.Add(new SqlParameter("@CodigoSociedad", (object)codigoSociedad ?? DBNull.Value));
            _sqlComando.Parameters.Add(new SqlParameter("@NumeroCajaChica", (object)numeroCajachica ?? DBNull.Value));
            _sqlComando.Parameters.Add(new SqlParameter("@Correlativo", (object)correlativo ?? DBNull.Value));

            return CargarReader(_sqlComando.ExecuteReader());
        }

        public List<CajaChicaEncabezadoDTO> BuscarCajasChicasRevisadas(string usuario, Int32 idSociedadCentro, Int32 codigoSociedad, Int32 numeroCajachica, Int32 correlativo)
        {
            SqlCommand _sqlComando = null;
            //            string sql = sqlSelect + @" inner join UsuarioSociedadCentro f on f.IdSociedadCentro = b.IdSociedadCentro
            //                                        where f.Usuario = @usuario
            //		                                and f.IdSociedadCentro = @idSociedadCentro
            //		                                and a.Estado = 2
            //		                                group by a.IdCajaChica, a.IdSociedadCentro, a.Correlativo, a.NumeroCajaChica, a.Descripcion, a.Estado, a.UsuarioAlta,
            //		                                a.FechaCreacion, a.UsuarioModificacion, a.FechaModificacion, c.CodigoSociedad, c.Nombre, d.IdCentro, d.Nombre,
            //		                                b.CodigoSociedad, d.IdCentro, a.correlativo, TipoOperacion ";

            string sql = sqlSelect + @" inner join UsuarioSociedadCentro h on h.IdSociedadCentro = b.IdSociedadCentro
                                        where h.Usuario = @Usuario
                                        and h.IdSociedadCentro = case when @idSociedadCentro = '0' then h.IdSociedadCentro else @idSociedadCentro end
                                        and c.CodigoSociedad = case when @CodigoSociedad = '0' then c.CodigoSociedad else @CodigoSociedad end
                                        and a.NumeroCajaChica = case when @NumeroCajaChica = '0' then a.NumeroCajaChica else @NumeroCajaChica end
                                        and a.Correlativo = case when @Correlativo = '0' then a.Correlativo else @Correlativo end
                                        and a.Estado = 2
                                        and e.Estado != 1
                                        group by a.IdCajaChica, a.IdSociedadCentro, a.Correlativo, a.NumeroCajaChica, a.Descripcion, a.Estado, a.UsuarioAlta,
                                        a.FechaCreacion, a.UsuarioModificacion, a.FechaModificacion, c.CodigoSociedad, c.Nombre, d.IdCentro, d.Nombre,
                                        b.CodigoSociedad, d.IdCentro, a.correlativo, TipoOperacion, a.EncargadoCC, a.IdSociedadMoneda, g.Moneda, c.Moneda, c.Pais, 
                                        a.FechaInicioViaje, a.FechaFinViaje, a.Objetivo, a.Numerodias, a.ViaticosRecibidos, a.IdNivel, a.ViaticosLocales
                                        order by a.IdCajaChica desc ";

            _sqlComando = new SqlCommand(sql, _sqlConn);
            _sqlComando.CommandType = CommandType.Text;

            if (_sqlTran != null)
                _sqlComando.Transaction = _sqlTran;

            _sqlComando.Parameters.Add(new SqlParameter("@usuario", (object)usuario ?? DBNull.Value));
            _sqlComando.Parameters.Add(new SqlParameter("@idSociedadCentro", (object)idSociedadCentro ?? DBNull.Value));
            _sqlComando.Parameters.Add(new SqlParameter("@CodigoSociedad", (object)codigoSociedad ?? DBNull.Value));
            _sqlComando.Parameters.Add(new SqlParameter("@NumeroCajaChica", (object)numeroCajachica ?? DBNull.Value));
            _sqlComando.Parameters.Add(new SqlParameter("@Correlativo", (object)correlativo ?? DBNull.Value));

            return CargarReader(_sqlComando.ExecuteReader());
        }

        public Int32 DarBajaCajaChica(decimal idCajaChica, string usuario)
        {
            SqlCommand _sqlComando = null;
            string sql = @" update CajaChicaEncabezado set estado = 0, UsuarioModificacion = @UsuarioModificacion, FechaModificacion = getdate()
                            where IdCajaChica = @IdCajaChica";

            _sqlComando = new SqlCommand(sql, _sqlConn);
            _sqlComando.CommandType = CommandType.Text;

            if (_sqlTran != null)
                _sqlComando.Transaction = _sqlTran;

            _sqlComando.Parameters.Add(new SqlParameter("@IdCajaChica", (object)idCajaChica ?? DBNull.Value));
            _sqlComando.Parameters.Add(new SqlParameter("@UsuarioModificacion", (object)usuario ?? DBNull.Value));

            return _sqlComando.ExecuteNonQuery();
        }

        public Int32 CerrarCajaChica(decimal idCajaChica, string usuario)
        {
            SqlCommand _sqlComando = null;
            string sql = @" update CajaChicaEncabezado set estado = 2, UsuarioModificacion = @UsuarioModificacion, FechaModificacion = getdate()
                            where IdCajaChica = @IdCajaChica";

            _sqlComando = new SqlCommand(sql, _sqlConn);
            _sqlComando.CommandType = CommandType.Text;

            if (_sqlTran != null)
                _sqlComando.Transaction = _sqlTran;

            _sqlComando.Parameters.Add(new SqlParameter("@IdCajaChica", (object)idCajaChica ?? DBNull.Value));
            _sqlComando.Parameters.Add(new SqlParameter("@UsuarioModificacion", (object)usuario ?? DBNull.Value));

            return _sqlComando.ExecuteNonQuery();
        }

        public List<LlenarDDL_DTO> ListarCahaChicaDDL(string sociedad, Int16 centro)
        {
            SqlCommand _sqlComando = null;
            string sql = @"SELECT IdCajaChica, Descripcion FROM CajaChicaEncabezado where estado = 1
                           and IdCentro = @IdCentro and CodigoSociedad = @CodigoSociedad";

            _sqlComando = new SqlCommand(sql, _sqlConn);
            _sqlComando.CommandType = CommandType.Text;

            if (_sqlTran != null)
                _sqlComando.Transaction = _sqlTran;

            _sqlComando.Parameters.Add(new SqlParameter("@CodigoSociedad", (object)sociedad ?? DBNull.Value));
            _sqlComando.Parameters.Add(new SqlParameter("@IdCentro", (object)centro ?? DBNull.Value));

            return CargarReaderDDL(_sqlComando.ExecuteReader());
        }

        public List<LlenarDDL_DTO> ListarCahasChicasDDL(Int32 idSociedadCentro)
        {
            SqlCommand _sqlComando = null;
            string sql = @"SELECT cast(IdCajaChica as char), Descripcion FROM CajaChicaEncabezado where estado = 1
                           and IdSociedadCentro = @IdSociedadCentro";

            _sqlComando = new SqlCommand(sql, _sqlConn);
            _sqlComando.CommandType = CommandType.Text;

            if (_sqlTran != null)
                _sqlComando.Transaction = _sqlTran;

            _sqlComando.Parameters.Add(new SqlParameter("@IdSociedadCentro", (object)idSociedadCentro ?? DBNull.Value));

            return CargarReaderDDL(_sqlComando.ExecuteReader());
        }

        public List<LlenarDDL_DTO> CargarReaderDDL(SqlDataReader sqlDataReader)
        {
            List<LlenarDDL_DTO> _listaLlenarDDL = new List<LlenarDDL_DTO>();
            LlenarDDL_DTO _llenarDDL = null;

            while (sqlDataReader.Read())
            {
                _llenarDDL = new LlenarDDL_DTO();
                _llenarDDL.IDENTIFICADOR = sqlDataReader.GetString(0);
                _llenarDDL.DESCRIPCION = sqlDataReader.GetString(1);
                _listaLlenarDDL.Add(_llenarDDL);
            }

            return _listaLlenarDDL;
        }

        //--INI---------------------------------------SATB-23.11.2017-----Pantalla Cambio Estado CC-----------------
        public List<CajaChicaDTO> CajaChicaEstado(String sociedad, string centro, string numeroCC, int correlativo)
        {
            SqlCommand sqlComando;
            string sql = @"	SELECT * FROM VEstado_CC
	                        WHERE CodigoSociedad = @Sociedad
		                      AND IdCentro = @Centro
		                      AND NumeroCajaChica = @NumeroCC
		                      AND Correlativo = @Correlativo";

            sqlComando = new SqlCommand(sql, _sqlConn);
            sqlComando.CommandType = CommandType.Text;

            if (_sqlTran != null)
                sqlComando.Transaction = _sqlTran;

            sqlComando.Parameters.Add(new SqlParameter("Sociedad", (object)sociedad ?? DBNull.Value));
            sqlComando.Parameters.Add(new SqlParameter("Centro", (object)centro ?? DBNull.Value));
            sqlComando.Parameters.Add(new SqlParameter("NumeroCC", (object)numeroCC ?? DBNull.Value));
            sqlComando.Parameters.Add(new SqlParameter("Correlativo", (object)correlativo));

            return CargarReaderCCEstado(sqlComando.ExecuteReader());
        }

        protected List<CajaChicaDTO> CargarReaderCCEstado(SqlDataReader sqlReader)
        {
            List<CajaChicaDTO> _listaCajaChica = new List<CajaChicaDTO>();
            CajaChicaDTO _cajaChicaDto = null;

            try
            {
                while (sqlReader.Read())
                {
                    _cajaChicaDto = new CajaChicaDTO();

                    _cajaChicaDto.CODIGO_SOCIEDAD = sqlReader.GetString(0);
                    _cajaChicaDto.CAJA_ENCABEZADO.NOMBRE_EMPRESA = sqlReader.GetString(1);
                    //_cajaChicaDto.CAJA_ENCABEZADO.CODIGO_CENTRO = sqlReader.GetInt16(2);
                    _cajaChicaDto.CAJA_ENCABEZADO.NOMBRE_CENTRO = sqlReader.GetString(3);
                    _cajaChicaDto.NUMERO_CAJA_CHICA = sqlReader.GetString(11);
                    _cajaChicaDto.CORRELATIVO = sqlReader.GetInt32(5);
                    _cajaChicaDto.ID_CAJA_CHICA = sqlReader.GetDecimal(6);
                    _cajaChicaDto.CAJA_ENCABEZADO.ID_SOCIEDAD_CENTRO = sqlReader.GetInt32(7);
                    _cajaChicaDto.DESCRIPCION = sqlReader.GetString(8);
                    _cajaChicaDto.CAJA_ENCABEZADO.ESTADO = sqlReader.GetInt16(9);
                    _cajaChicaDto.USUARIO_MANTENIMIENTO.FECHA_ALTA = sqlReader.GetDateTime(10);
                    _cajaChicaDto.CAJA_ENCABEZADO.ESTADO_DESC = sqlReader.GetString(12);
                    _cajaChicaDto.CAJA_ENCABEZADO.MONEDA = sqlReader.GetString(13);
                    _cajaChicaDto.CAJA_ENCABEZADO.FACTURAS_CC = sqlReader.GetInt32(14);
                    _cajaChicaDto.CAJA_ENCABEZADO.MONTO_CC = sqlReader.GetDouble(15);

                    _listaCajaChica.Add(_cajaChicaDto);
                }

                sqlReader.Close();
                return _listaCajaChica;
            }
            finally
            {
                if (sqlReader != null) sqlReader.Close();
            }
        }

        public Int32 CambiarEstadoCC(decimal IdCC, string usuario, string justificacion, int e_actual, int e_nuevo)
        {
            SqlCommand _sqlComando = null;

            string sql;
            sql = "SP_CambioEstadoCC";

            _sqlComando = new SqlCommand(sql, _sqlConn);
            _sqlComando.CommandType = CommandType.StoredProcedure;

            if (_sqlTran != null)
                _sqlComando.Transaction = _sqlTran;

            _sqlComando.Parameters.Add(new SqlParameter("@IdCajaChica", (object) IdCC));
            _sqlComando.Parameters.Add(new SqlParameter("@Usuario", (object) usuario ?? DBNull.Value));
            _sqlComando.Parameters.Add(new SqlParameter("@Justificacion", (object) justificacion ?? DBNull.Value));
            _sqlComando.Parameters.Add(new SqlParameter("@E_Actual", (object)e_actual));
            _sqlComando.Parameters.Add(new SqlParameter("@E_Nuevo", (object)e_nuevo));

            return Convert.ToInt32(_sqlComando.ExecuteScalar());
        }


        //--FIN---------------------------------------SATB-23.11.2017-----Pantalla Cambio Estado CC-----------------
        #endregion

    }
}
