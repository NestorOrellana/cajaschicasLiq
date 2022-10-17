using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using LogicaCajasChicas;

namespace DipCmiGT.LogicaCajasChicas.Entidad
{
    public class IndicadoresIVA
    {
        #region Declaraciones

        private SqlConnection _sqlConn = null;
        private SqlTransaction _sqlTran = null;

        protected string sqlSelect = @" SELECT distinct IdentificadorIVA, 
                                        (IdentificadorIVA + '-'+ Descripcion) as Descripcion,
                                        Importe, Activo
                                        FROM IdentificadoresIVA a ";

        #endregion

        #region Constructores

        public IndicadoresIVA(SqlConnection sqlConn)
        {
            _sqlConn = sqlConn;
        }

        public IndicadoresIVA(SqlConnection sqlConn, SqlTransaction sqlTran)
        {
            _sqlConn = sqlConn;
            _sqlTran = sqlTran;
        }

        #endregion

        #region Metodos

        public IndicadoresIVADTO EjecutarSentenciaSelect(string identificadorIVA, string Sociedad)
        {
            List<IndicadoresIVADTO> _listaCajaChica = null;
            SqlCommand _sqlComando = null;
            string sql = sqlSelect + @"Where IdentificadorIVA =  @IdentificadorIVA
                                            AND dominio = (	SELECT TOP 1 Pais FROM dbo.Sociedad
															WHERE CodigoSociedad = @Sociedad
															AND Alta = 1 )";

            _sqlComando = new SqlCommand(sql, _sqlConn);
            _sqlComando.CommandType = CommandType.Text;

            if (_sqlTran != null)
                _sqlComando.Transaction = _sqlTran;

            _sqlComando.Parameters.Add(new SqlParameter("@IdentificadorIVA", (object)identificadorIVA ?? DBNull.Value));
            _sqlComando.Parameters.Add(new SqlParameter("@Sociedad", (object)Sociedad ?? DBNull.Value));

            _listaCajaChica = CargarReader(_sqlComando.ExecuteReader());

            return _listaCajaChica.Count > 0 ? _listaCajaChica[0] : null;
        }

        public Int32 EjecutarSentenciaInsert(IndicadoresIVADTO _cajaChicaDto)
        {
            throw new NotImplementedException("");
        }


        public decimal EjecutarSentenciaUpdate(IndicadoresIVADTO _cajaChicaDto)
        {
            throw new NotImplementedException("");
        }

        public Int32 EjecutarSentenciaDelete()
        {
            throw new NotImplementedException("Método no implementado.");
        }

        protected List<IndicadoresIVADTO> CargarReader(SqlDataReader sqlReader)
        {
            IndicadoresIVADTO _indicadorIVADto = null;
            List<IndicadoresIVADTO> _listaIndicadorIVA = new List<IndicadoresIVADTO>();


            try
            {

                while (sqlReader.Read())
                {
                    _indicadorIVADto = new IndicadoresIVADTO();

                    _indicadorIVADto.INDICADOR_IVA = sqlReader.GetString(0);
                    _indicadorIVADto.DESCRIPCION = sqlReader.GetString(1);
                    _indicadorIVADto.IMPORTE = sqlReader.GetDouble(2);
                    _indicadorIVADto.ACTIVO = sqlReader.GetBoolean(3);

                    _listaIndicadorIVA.Add(_indicadorIVADto);
                }
            }
            finally
            {
                if (sqlReader != null) sqlReader.Close();
            }
            return _listaIndicadorIVA;
        }

        public List<IndicadoresIVADTO> BuscarIndicadoresActivos(string cajaChica, string usuario)
        {
            SqlCommand _sqlComando = null;
//            string sql = @" SELECT distinct IdentificadorIVA, 
//                            (IdentificadorIVA + '-'+ Descripcion) as Descripcion,
//                            Importe, Activo
//                            FROM IdentificadoresIVA a
//                            INNER JOIN Usuario as b 
//                            on a.dominio = SUBSTRING(b.dominio,1,2)
//                            where b.Usuario = @usuario and a.activo = 1";

            string sql = @"SELECT distinct IdentificadorIVA, 
                            (IdentificadorIVA + '-'+ Descripcion) as Descripcion,
                            Importe, Activo
                            FROM IdentificadoresIVA a
                            WHERE a.activo = 1
                              AND a.Sociedad = @CajaChicaSAP";
                            //AND dominio IN (SELECT Pais FROM dbo.Sociedad
                            //    WHERE codigoSociedad = @CajaChicaSAP)";

            _sqlComando = new SqlCommand(sql, _sqlConn);
            _sqlComando.CommandType = CommandType.Text;

            if (_sqlTran != null)
                _sqlComando.Transaction = _sqlTran;

            _sqlComando.Parameters.Add(new SqlParameter("@CajaChicaSAP", (object)cajaChica ?? DBNull.Value));
            _sqlComando.Parameters.Add(new SqlParameter("@usuario", (object)usuario ?? DBNull.Value));

            return CargarReader(_sqlComando.ExecuteReader());
        }

        #endregion

    }
}
