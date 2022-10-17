using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using LogicaComun.Enum;
using DipCmiGT.LogicaCajasChicas.Entidad;
using DipCmiGT.LogicaComun;
using System.Transactions;
using DipCmiGT.LogicaComun.Util;
using LogicaCajasChicas.Enum;
using LogicaCajasChicas.Entidad;
using LogicaCajasChicas;
using DipCmiGT.LogicaComun.Entidad;

namespace DipCmiGT.LogicaCajasChicas.Sesion
{
    public class GestorCajaChica : IDisposable
    {
        #region Declaracion

        IndicadoresIVA _indicadoresIVA = null;
        Valores_ISR _valoresISR = null;
        Sociedad _sociedad = null;
        Proveedor _proveedor = null;
        FacturaEncabezado _facturaEncabezado = null;
        FacturaDetalle _facturaDetalle = null;
        CajaChica _cajaChica = null;
        SAPCajasChicas _sapCajasChicas = null;
        TipoDocumento _tipoDocumento = null;
        IntermediaFacturaDetalle _intermediaFacturaDetalle = null;
        IntermediaFacturaEncabezado _intermediaFacturaEncabezado = null;
        RegistroContable _registroContable = null;
        LogErrores _logErrores = null;

        SqlConnection cnnSql = null;
        SqlConnection cnnInterfaces = null;
        SqlConnection cnnError = null;

        #endregion

        #region Constructor

        public GestorCajaChica(string conexion)
        {
            cnnSql = new SqlConnection(conexion);
            cnnError = new SqlConnection(conexion);
        }

        public GestorCajaChica(string[] conexion)
        {
            cnnSql = new SqlConnection(conexion[0]);
            cnnInterfaces = new SqlConnection(conexion[1]);
        }

        #endregion

        #region CajasChicas

        public CajaChicaEncabezadoDTO AlmacenarCajaChica(CajaChicaEncabezadoDTO cajaChicaDto)
        {
            if (_cajaChica == null) _cajaChica = new CajaChica(cnnSql);
            List<CajaChicaEncabezadoDTO> _listaCajaChicaDto = new List<CajaChicaEncabezadoDTO>();

            try
            {
                cnnSql.Open();

                cajaChicaDto.CORRELATIVO = _cajaChica.BuscarCorrelativoCC(cajaChicaDto.ID_SOCIEDAD_CENTRO, cajaChicaDto.CAJA_CHICA_SAP);

                cajaChicaDto.ID_CAJA_CHICA = _cajaChica.EjecutarSentenciaInsert(cajaChicaDto);

                return _cajaChica.EjecutarSentenciaSelect(cajaChicaDto.ID_CAJA_CHICA);
            }
            finally
            {
                if (cnnSql.State != ConnectionState.Closed) cnnSql.Close();
            }
        }

        public List<CajaChicaEncabezadoDTO> BuscarCajasChicasUsuario(Int16 idCentro, string usuario, Int16 estado, string codigoCC)
        {
            if (_cajaChica == null) _cajaChica = new CajaChica(cnnSql);

            try
            {
                cnnSql.Open();

                return _cajaChica.BuscarCajasChicasUsuario(idCentro, usuario, estado, codigoCC);
            }
            finally
            {
                if (cnnSql.State != ConnectionState.Closed) cnnSql.Close();
            }
        }

        public List<CajaChicaEncabezadoDTO> BuscarCajasChicas(string codigoSociedad, Int16 idCentro, Int16 estado)
        {
            if (_cajaChica == null) _cajaChica = new CajaChica(cnnSql);

            try
            {
                cnnSql.Open();

                return _cajaChica.BuscarCajasChicas(codigoSociedad, idCentro, estado);
            }
            finally
            {
                if (cnnSql.State != ConnectionState.Closed) cnnSql.Close();
            }
        }

        public List<LlenarDDL_DTO> BuscarCajasChicasSAP(string codigoSociedad, string usuario)
        //public List<LlenarDDL_DTO> BuscarCajasChicasSAP(string codigoSociedad)
        {
            if (_sapCajasChicas == null) _sapCajasChicas = new SAPCajasChicas(cnnSql);

            try
            {
                cnnSql.Open();

                //return _sapCajasChicas.BuscarCajasChicas(codigoSociedad);
                return _sapCajasChicas.BuscarCajasChicas(codigoSociedad, usuario);
            }
            finally
            {
                if (cnnSql.State != ConnectionState.Closed) cnnSql.Close();
            }
        }

        public List<CajaChicaEncabezadoDTO> BuscarCajasChicasRevisadas(string usuario, Int32 idSociedadCentro, Int32 codigoSociedad, Int32 numeroCajachica, Int32 correlativo)
        {
            if (_cajaChica == null) _cajaChica = new CajaChica(cnnSql);

            try
            {
                cnnSql.Open();

                return _cajaChica.BuscarCajasChicasRevisadas(usuario, idSociedadCentro, codigoSociedad, numeroCajachica, correlativo);
            }
            finally
            {
                if (cnnSql.State != ConnectionState.Closed) cnnSql.Close();
            }
        }

        public List<CajaChicaEncabezadoDTO> BuscarCajasRevision(string usuario, Int32 idSociedadCentro, Int32 codigoSociedad, Int32 numeroCajachica, Int32 correlativo)
        {
            if (_cajaChica == null) _cajaChica = new CajaChica(cnnSql);

            try
            {
                cnnSql.Open();

                return _cajaChica.BuscarCajasChicasRevision(usuario, idSociedadCentro, codigoSociedad, numeroCajachica, correlativo);
            }
            finally
            {
                if (cnnSql.State != ConnectionState.Closed) cnnSql.Close();
            }
        }

        public Int32 DarBajaCajaChica(decimal idCajaChica, string usuario)
        {
            if (_cajaChica == null) _cajaChica = new CajaChica(cnnSql);
            if (_facturaEncabezado == null) _facturaEncabezado = new FacturaEncabezado(cnnSql);


            try
            {
                cnnSql.Open();

                if (_facturaEncabezado.BuscarFacturasCajaChica(idCajaChica, 1).Count > 0)
                    throw new ExcepcionesDIPCMI("La caja chica no se puede dar de baja por tener facturas asociadas.");


                return _cajaChica.DarBajaCajaChica(idCajaChica, usuario);
            }
            finally
            {
                if (cnnSql.State != ConnectionState.Closed) cnnSql.Close();
            }
        }

        public Int32 CerrarCajaChica(decimal idCajaChica, string usuario)
        {
            if (_cajaChica == null) _cajaChica = new CajaChica(cnnSql);
            if (_facturaEncabezado == null) _facturaEncabezado = new FacturaEncabezado(cnnSql);

            try
            {
                cnnSql.Open();

                if (_facturaEncabezado.BuscarFacturasAprobadas(idCajaChica, 1).Count > 0)
                    throw new ExcepcionesDIPCMI("La caja chica tiene facturas pendientes de aprobar.");

                if (_facturaEncabezado == null) _facturaEncabezado = new FacturaEncabezado(cnnSql);
                if (_facturaEncabezado.ValidarFactruaDividida(idCajaChica) != null)
                    throw new ExcepcionesDIPCMI("Las facturas  no coinciden con el total registrado");


                return _cajaChica.CerrarCajaChica(idCajaChica, usuario);
            }
            finally
            {
                if (cnnSql.State != ConnectionState.Closed) cnnSql.Close();
            }
        }

        public List<LlenarDDL_DTO> ListarCajaChica(string sociedad, Int16 centro)
        {
            if (_cajaChica == null) _cajaChica = new CajaChica(cnnSql);
            try
            {
                cnnSql.Open();
                return _cajaChica.ListarCahaChicaDDL(sociedad, centro);
            }
            finally
            {
                if (cnnSql.State != ConnectionState.Closed) cnnSql.Close();
            }
        }

        public List<LlenarDDL_DTO> ListarCajaChicaDDL(Int32 IdSociedadCentro)
        {
            if (_cajaChica == null) _cajaChica = new CajaChica(cnnSql);
            try
            {
                cnnSql.Open();
                return _cajaChica.ListarCahasChicasDDL(IdSociedadCentro);
            }
            finally
            {
                if (cnnSql.State != ConnectionState.Closed) cnnSql.Close();
            }
        }


        //--INI---------------------------------------SATB-23.11.2017-----Pantalla Cambio Estado CC-----------------
        public List<CajaChicaDTO> BuscarCajasChicasEstadoCC(string Sociedad, string centro, string numeroCC, int correlativo)
        {
            if (_cajaChica == null) _cajaChica = new CajaChica(cnnSql);

            try
            {
                cnnSql.Open();
                return _cajaChica.CajaChicaEstado(Sociedad, centro, numeroCC, correlativo);
            }
            finally
            {
                if (cnnSql.State != ConnectionState.Closed) cnnSql.Close();
            }
        }

        public Int32 CambiarEstadoCC(decimal idCajaChica, string usuario, string justificacion, int e_actual, int e_nuevo)
        {
            if (_cajaChica == null) _cajaChica = new CajaChica(cnnSql);
            try
            {
                cnnSql.Open();

                return _cajaChica.CambiarEstadoCC(idCajaChica, usuario, justificacion, e_actual, e_nuevo);
            }
            finally
            {
                if (cnnSql.State != ConnectionState.Closed) cnnSql.Close();
            }
        }

        //--FIN---------------------------------------SATB-23.11.2017-----Pantalla Cambio Estado CC-----------------

        #endregion

        #region Facturas

        public void AlmacenarFactura(FacturaEncabezadoDTO _facturaEncabezadoDto)
        {
            SociedadDTO _sociedadDto = null;
            FacturaEncabezadoDTO _facturaEncabezadoBDDto = null;
            List<RegistroContableDTO> _listaRegistroContableDto = null;
            IndicadoresIVADTO _indicadoresIVADto = null;
            DateTime fechaRet;
            double calculoISR = 0;
            double valorIVA = 0;
            Int16 correlativo = 0;
            double retencionIVA = 0;
            double retencionISR = 0;
            double sumaFac = 0;
            double sumaIVA = 0;

            if (_valoresISR == null) _valoresISR = new Valores_ISR(cnnSql);
            if (_proveedor == null) _proveedor = new Proveedor(cnnSql);
            if (_facturaDetalle == null) _facturaDetalle = new FacturaDetalle(cnnSql);
            if (_facturaEncabezado == null) _facturaEncabezado = new FacturaEncabezado(cnnSql);
            if (_sociedad == null) _sociedad = new Sociedad(cnnSql);
            if (_registroContable == null) _registroContable = new RegistroContable(cnnSql);
            if (_cajaChica == null) _cajaChica = new CajaChica(cnnSql);
            if (_indicadoresIVA == null) _indicadoresIVA = new IndicadoresIVA(cnnSql);
            if (_logErrores == null) _logErrores = new LogErrores(cnnError);

            TransactionScope scope = null;

            try
            {
                cnnError.Open();
                scope = new TransactionScope();
                cnnSql.Open();

                //Valida si la caja chia esta abierta 
                if (!_facturaEncabezado.ValidaCajaChicaAbierta(_facturaEncabezadoDto.ID_CAJA_CHICA))
                    throw new ExcepcionesDIPCMI("La Caja Chica no esta Abierta");

                //Valida si la factura tiene marca de DiferentesCO, sino vuelve 0 el TotalFacturaDividida
                if (!Convert.ToBoolean(_facturaEncabezadoDto.FACTURA_DIVIDIDA))
                    _facturaEncabezadoDto.TOTALFACTURADIVIDIDA = 0.00;

                //////Validar que el dato que se estas ingresando no sobrepase el total de la factura 
                //if (Convert.ToBoolean(_facturaEncabezadoDto.FACTURA_DIVIDIDA))
                //    if (_facturaEncabezadoDto.ACUMULADO > (_facturaEncabezadoDto.TOTALFACTURADIVIDIDA))
                //        throw new ExcepcionesDIPCMI("El total parcial supera el total de la factura.");

                //Validar el tamanio del texto de las  observaciones, no mayor a 150 
                if (_facturaEncabezadoDto.OBSERVACIONES.Length > 150)
                    throw new ExcepcionesDIPCMI("El texto de observaciones excede el tamaño permitido");
                //valida que la factura del proveedor no exista en la base de datos.
                //if (_facturaEncabezadoDto.FACTURA_DIVIDIDA)
                //{


                if (!_facturaEncabezado.MarcaFacturaDividida(_facturaEncabezadoDto.ID_PROVEEDOR, _facturaEncabezadoDto.SERIE, _facturaEncabezadoDto.NUMERO, _facturaEncabezadoDto.ID_FACTURA, Convert.ToBoolean(_facturaEncabezadoDto.FACTURA_DIVIDIDA)))
                    throw new ExcepcionesDIPCMI("La factura ya se encuentra registrada.");

                if (!_facturaEncabezado.ExisteFactura(_facturaEncabezadoDto.ID_PROVEEDOR, _facturaEncabezadoDto.SERIE, _facturaEncabezadoDto.NUMERO, _facturaEncabezadoDto.ID_FACTURA, _facturaEncabezadoDto.ID_CAJA_CHICA))
                    throw new ExcepcionesDIPCMI("La factura ya se encuentra grabada en otra caja chica.");


                //}
                //else
                //{
                //    if (!_facturaEncabezado.ExisteFactura(_facturaEncabezadoDto.ID_PROVEEDOR, _facturaEncabezadoDto.SERIE, _facturaEncabezadoDto.NUMERO, _facturaEncabezadoDto.ID_FACTURA, _facturaEncabezadoDto.ID_CAJA_CHICA, _facturaEncabezadoDto.FACTURA_DIVIDIDA))
                //        throw new ExcepcionesDIPCMI("La factura ya se encuentra grabada.");
                //}


                _facturaEncabezadoBDDto = _facturaEncabezado.EjecutarSentenciaSelect(_facturaEncabezadoDto.ID_FACTURA);

                if (_facturaEncabezadoBDDto != null)
                {
                    if (_facturaEncabezadoBDDto.APROBADA != null)
                        throw new ExcepcionesDIPCMI("La factura no puede ser modificada por estar aprobada.");

                    if (_facturaEncabezadoBDDto.ESTADO == 0)
                        throw new ExcepcionesDIPCMI("La factura no puese ser modificada por estar anulada.");

                    ////Validar que el dato que se estas ingresando no sobrepase el total de la factura 
                    if (Convert.ToBoolean(_facturaEncabezadoDto.FACTURA_DIVIDIDA))
                        if ((Convert.ToDouble(_facturaEncabezadoDto.ACUMULADO) - _facturaEncabezadoDto.VALOR_TOTAL) > (_facturaEncabezadoDto.TOTALFACTURADIVIDIDA))
                            throw new ExcepcionesDIPCMI("El total parcial supera el total de la factura.");
                }
                else
                {
                    ////Validar que el dato que se estas ingresando no sobrepase el total de la factura 
                    if (Convert.ToBoolean(_facturaEncabezadoDto.FACTURA_DIVIDIDA))
                    {
                        if (Convert.ToString(_facturaEncabezadoDto.TOTALFACTURADIVIDIDA) == "")
                            throw new ExcepcionesDIPCMI("Debe ingresar el total de la factura");
                        else
                            // if (Convert.ToDouble(_facturaEncabezadoDto.ACUMULADO) > (_facturaEncabezadoDto.TOTALFACTURADIVIDIDA))
                            if (Math.Round(Convert.ToDouble(_facturaEncabezadoDto.ACUMULADO), 2) > (_facturaEncabezadoDto.TOTALFACTURADIVIDIDA))
                                throw new ExcepcionesDIPCMI("El total parcial supera el total de la factura.");
                    }
                }

                //Validaciones IPCR. Solicita que ingrese el valor real de la factura y que este sea mayor o  igual al valor a pagar
                if (_facturaEncabezadoDto.PAIS == "CR")//(_facturaEncabezadoDto.CODIGO_SOCIEDAD == "1440" || _facturaEncabezadoDto.CODIGO_SOCIEDAD == "1630")
                {
                    if (_facturaEncabezadoDto.VALOR_REAL_FACT <= 0 || _facturaEncabezadoDto.VALOR_REAL_FACT == null)
                        throw new ExcepcionesDIPCMI("Debe de ingresar el valor real de la factura.");

                    if (Convert.ToDouble(_facturaEncabezadoDto.VALOR_REAL_FACT) < Convert.ToDouble(_facturaEncabezadoDto.VALOR_TOTAL))
                        throw new ExcepcionesDIPCMI("El valor real de la factura debe ser igual o mayor al valor a pagar.");
                }



                //recupera datos de caja chica
                _facturaEncabezadoDto.CAJA_CHICA = _cajaChica.EjecutarSentenciaSelect(_facturaEncabezadoDto.ID_CAJA_CHICA);

                //recupera datos de sociedad.
                _sociedadDto = _sociedad.EjecutarSentenciaSelect(_facturaEncabezadoDto.CODIGO_SOCIEDAD);

                //Valida si se debe realizar Retencion 1% y 10%(Unicamente IPES) 
                if (_sociedadDto.PAIS == "SV")
                {
                    ////INI --------------- Retencion 1% IPES 14.08.2020 ------------------------
                    double Ret;
                    Ret = 1;
                    foreach (FacturaDetalleDTO facDetalle in _facturaEncabezadoDto.FACTURA_DETALLE)
                    {
                        Ret = _facturaEncabezado.BuscaRetencion(_sociedadDto.PAIS, facDetalle.IDENTIFICADOR_IVA.ToString());

                    }
                    ////FIN --------------- Retencion 1% IPES 14.08.2020 ------------------------
                    if (Ret > 0)
                    {
                        if (((_facturaEncabezadoDto.VALOR_TOTAL - _facturaEncabezadoDto.IVA) / Ret) >= 100)    //((_facturaEncabezadoDto.VALOR_TOTAL / 1.13) >= 100)  
                        {

                            if (!_facturaEncabezado.BuscaRegimenProveedor(_facturaEncabezadoDto.ID_PROVEEDOR))
                            {
                                _facturaEncabezadoDto.RETENCION_IVA = true;
                                //_facturaEncabezadoDto.VALOR_RETENCION_IVA = Convert.ToDouble((((_facturaEncabezadoDto.VALOR_TOTAL / 1.13) - _facturaEncabezadoDto.IMPUESTO) * 0.01).ToString("00000.00"));
                                _facturaEncabezadoDto.VALOR_RETENCION_IVA = Convert.ToDouble(((((_facturaEncabezadoDto.VALOR_TOTAL - _facturaEncabezadoDto.IVA) / Ret) - _facturaEncabezadoDto.IMPUESTO) * 0.01).ToString("00000.00"));
                            }
                        }
                    }

                    if ((!_facturaEncabezado.BuscaTipoProveedor(_facturaEncabezadoDto.ID_PROVEEDOR)) && _facturaEncabezadoDto.RETENCION == true)
                    {
                        _facturaEncabezadoDto.RETENCION_ISR = true;
                        _facturaEncabezadoDto.VALOR_RETENCION_ISR = (_facturaEncabezadoDto.VALOR_TOTAL - _facturaEncabezadoDto.IMPUESTO - _facturaEncabezadoDto.IVA) * 0.10;
                    }
                }


                _facturaEncabezadoDto.NIVEL = 1;
                if (_facturaEncabezadoDto.CAJA_CHICA.TIPO_OPERACION == TipoOperacionEnum.CC.ToString())
                {
                    if (_facturaEncabezadoDto.VALOR_TOTAL <= _sociedadDto.MONTO_COMPRA_CC)
                        _facturaEncabezadoDto.NIVEL = 1;
                    else if ((_facturaEncabezadoDto.VALOR_TOTAL > _sociedadDto.MONTO_COMPRA_CC) && (_facturaEncabezadoDto.VALOR_TOTAL <= Math.Round(_sociedadDto.MONTO_COMPRA_CC * (1 + Convert.ToDouble(_sociedadDto.TOLERANCIA_COMPRA_CC) / 100), 2)))
                        _facturaEncabezadoDto.NIVEL = 2;
                    else
                        throw new Exception("El valor de la factura excede el monto permitido.");
                }

                fechaRet = DateTime.Today.AddMonths(-(_sociedadDto.MESES_FACTURA - 1));

                ////Validación que la factura corresponda el año actual.
                //if (_facturaEncabezadoDto.FECHA_FACTURA.Year < DateTime.Now.Year)
                //    throw new ExcepcionesDIPCMI("El año de la factura no corresponde al año actual.");

                //Validación que la factura este entre el rango comprendido de meses para registro y pago.
                //if (Convert.ToInt32(fechaRet.ToString("yyyyMMdd")) > Convert.ToInt32(_facturaEncabezadoDto.FECHA_FACTURA.ToString("yyyyMMdd")))
                if (Convert.ToInt32(fechaRet.ToString("yyyyMM")) > Convert.ToInt32(_facturaEncabezadoDto.FECHA_FACTURA.ToString("yyyyMM")))
                    throw new ExcepcionesDIPCMI("La factura excedió el límite de tiempo para registro.");

                //Validar que la fecha de la factura no sea mayor al dia que se esta registrando 
                if (Convert.ToInt32(_facturaEncabezadoDto.FECHA_FACTURA.ToString("yyyyMMdd")) > Convert.ToInt32(DateTime.Now.ToString("yyyyMMdd")))
                    throw new ExcepcionesDIPCMI("La fecha de la factura no puede ser mayor al día de hoy.");

                //Campo de observaciones no mayor a 150 caracteres 
                if (_facturaEncabezadoDto.OBSERVACIONES.Length > 150)
                    throw new ExcepcionesDIPCMI("El campo de observaciones excedio los 150 caracteres");

                //Almacena registro a base de datos del encabezado de la factura.
                if (_facturaEncabezadoDto.ID_FACTURA.Equals(0))
                    //Inserta registro.
                    _facturaEncabezadoDto.ID_FACTURA = _facturaEncabezado.EjecutarSentenciaInsert(_facturaEncabezadoDto);


                else
                    //Actualiza registro.
                    _facturaEncabezado.EjecutarSentenciaUpdate(_facturaEncabezadoDto);


                _listaRegistroContableDto = new List<RegistroContableDTO>();
                double cuentaPorPagar = 0;

                foreach (FacturaDetalleDTO facDetalle in _facturaEncabezadoDto.FACTURA_DETALLE)
                {
                    sumaFac += facDetalle.VALOR;
                    sumaIVA += facDetalle.IVA;
                }

                if (sumaFac != _facturaEncabezadoDto.VALOR_TOTAL)
                    throw new ExcepcionesDIPCMI("La suma del encabezado con el detalle del valor total no es igual.");

                if (Math.Round(sumaIVA, 2) != _facturaEncabezadoDto.IVA)
                    throw new ExcepcionesDIPCMI("La suma del IVA del detalle con el IVA total no es igual.");

                foreach (FacturaDetalleDTO facDetalle in _facturaEncabezadoDto.FACTURA_DETALLE)
                {
                    correlativo = 0;
                    calculoISR = 0;
                    valorIVA = 0;
                    cuentaPorPagar = 0;

                    //recupera tipo de iva e importe
                    _indicadoresIVADto = _indicadoresIVA.EjecutarSentenciaSelect(facDetalle.IDENTIFICADOR_IVA, _sociedadDto.CODIGO_SOCIEDAD);

                    if (_indicadoresIVADto == null)
                        throw new ExcepcionesDIPCMI("No hay indicadores de IVA disponibles.");

                    if (_indicadoresIVADto.IMPORTE == 0 && facDetalle.IVA > _indicadoresIVADto.IMPORTE)
                        throw new ExcepcionesDIPCMI("Se calcula IVA a un indicador que no imputa IVA.");

                    if (_indicadoresIVADto.IMPORTE > 0 && facDetalle.IVA == 0)
                        throw new ExcepcionesDIPCMI("No se calcula IVA a un indicador que si imputa IVA.");

                    //Gasto
                    correlativo = Convert.ToInt16(correlativo + 1);
                    _listaRegistroContableDto.Add(LlenarObjetoRegistroContable(correlativo, facDetalle.CUENTA_CONTABLE, facDetalle.DEFINICION_CC, Convert.ToInt16(Cargo_AbonoEnum.CARGO), facDetalle.VALOR - facDetalle.IMPUESTO - facDetalle.IVA, string.Empty));
                    valorIVA = Math.Round(facDetalle.IVA, 2);

                    if (facDetalle.IMPUESTO > 0)
                    {
                        //Impuestos varios
                        correlativo = Convert.ToInt16(correlativo + 1);
                        _listaRegistroContableDto.Add(LlenarObjetoRegistroContable(correlativo, facDetalle.CUENTA_CONTABLE, TipoImpuestosEnum.IMPUESTOS_VARIOS.ToString(), Convert.ToInt16(Cargo_AbonoEnum.CARGO), facDetalle.IMPUESTO, "")); //"V0"
                    }

                    //IVA crédito
                    correlativo = Convert.ToInt16(correlativo + 1);
                    //_listaRegistroContableDto.Add(LlenarObjetoRegistroContable(correlativo, Convert.ToInt16(TipoImpuestosEnum.IVA_CREDITO).ToString(), TipoImpuestosEnum.IVA_CREDITO.ToString(), Convert.ToInt16(Cargo_AbonoEnum.CARGO), valorIVA, facDetalle.IDENTIFICADOR_IVA));
                    _listaRegistroContableDto.Add(LlenarObjetoRegistroContable(correlativo, "1150603000", TipoImpuestosEnum.IVA_CREDITO.ToString(), Convert.ToInt16(Cargo_AbonoEnum.CARGO), valorIVA, facDetalle.IDENTIFICADOR_IVA));
                    if (_facturaEncabezadoDto.ES_ESPECIAL)
                    {
                        //IVA factura especial
                        correlativo = Convert.ToInt16(correlativo + 1);
                        _listaRegistroContableDto.Add(LlenarObjetoRegistroContable(correlativo, Convert.ToInt16(TipoImpuestosEnum.IVA_FACTURA_ESPECIAL).ToString(), TipoImpuestosEnum.IVA_FACTURA_ESPECIAL.ToString(), Convert.ToInt16(Cargo_AbonoEnum.ABONO), valorIVA, facDetalle.IDENTIFICADOR_IVA));

                        //Calculando ISR Factura especial
                        correlativo = Convert.ToInt16(correlativo + 1);
                        calculoISR = CalcularRetencionISR(facDetalle.VALOR - facDetalle.IVA, TipoImpuestosEnum.ISR_SERVICIO, _valoresISR);
                        _listaRegistroContableDto.Add(LlenarObjetoRegistroContable(correlativo, Convert.ToInt16(TipoImpuestosEnum.ISR_FACTURA_ESPECIAL).ToString(), TipoImpuestosEnum.ISR_FACTURA_ESPECIAL.ToString(), Convert.ToInt16(Cargo_AbonoEnum.ABONO), Math.Round(calculoISR, 2), string.Empty));

                        //cuenta por pagar
                        correlativo = Convert.ToInt16(correlativo + 1);
                        cuentaPorPagar = facDetalle.VALOR - calculoISR - valorIVA;
                        _listaRegistroContableDto.Add(LlenarObjetoRegistroContable(correlativo, _facturaEncabezadoDto.CAJA_CHICA.CAJA_CHICA_SAP, _facturaEncabezadoDto.CAJA_CHICA.NOMBRE_CC, Convert.ToInt16(Cargo_AbonoEnum.ABONO), Math.Round(cuentaPorPagar, 2), string.Empty));
                    }
                    else
                    {
                        //cuenta por pagar
                        correlativo = Convert.ToInt16(correlativo + 1);
                        _listaRegistroContableDto.Add(LlenarObjetoRegistroContable(correlativo, _facturaEncabezadoDto.CAJA_CHICA.CAJA_CHICA_SAP, _facturaEncabezadoDto.CAJA_CHICA.NOMBRE_CC, Convert.ToInt16(Cargo_AbonoEnum.ABONO), Math.Round(facDetalle.VALOR, 2), string.Empty));
                    }
                }

                if (_facturaEncabezadoDto.RETENCION_IVA)
                {
                    correlativo = Convert.ToInt16(correlativo + 1);
                    retencionIVA = (double)_facturaEncabezadoDto.VALOR_RETENCION_IVA;
                    if (_sociedadDto.PAIS == "SV")
                        _listaRegistroContableDto.Add(LlenarObjetoRegistroContable(correlativo, "2110302005", TipoImpuestosEnum.RETENCION_IVA.ToString(), Convert.ToInt16(Cargo_AbonoEnum.RETENCION), Math.Round((double)retencionIVA, 2), string.Empty));
                    else
                        _listaRegistroContableDto.Add(LlenarObjetoRegistroContable(correlativo, Convert.ToInt16(TipoImpuestosEnum.RETENCION_IVA).ToString(), TipoImpuestosEnum.RETENCION_IVA.ToString(), Convert.ToInt16(Cargo_AbonoEnum.ABONO), Math.Round((double)retencionIVA, 2), string.Empty));
                }

                if (_facturaEncabezadoDto.RETENCION_ISR)
                {
                    correlativo = Convert.ToInt16(correlativo + 1);
                    retencionISR = Math.Round((double)_facturaEncabezadoDto.VALOR_RETENCION_ISR, 2);
                    if (_sociedadDto.PAIS == "SV")
                        _listaRegistroContableDto.Add(LlenarObjetoRegistroContable(correlativo, "2110301002", TipoImpuestosEnum.RETENCION_ISR.ToString(), Convert.ToInt16(Cargo_AbonoEnum.RETENCION), Math.Round((double)retencionISR, 2), string.Empty));
                    else
                        _listaRegistroContableDto.Add(LlenarObjetoRegistroContable(correlativo, Convert.ToInt16(TipoImpuestosEnum.RETENCION_ISR).ToString(), TipoImpuestosEnum.RETENCION_ISR.ToString(), Convert.ToInt16(Cargo_AbonoEnum.ABONO), Math.Round((double)retencionISR, 2), string.Empty));
                }

                //cuenta por pagar
                if (!_facturaEncabezadoDto.ES_ESPECIAL)
                {
                    correlativo = Convert.ToInt16(correlativo + 1);
                    cuentaPorPagar = _facturaEncabezadoDto.VALOR_TOTAL - retencionISR - retencionIVA;
                    //_listaRegistroContableDto.Add(LlenarObjetoRegistroContable(correlativo, _facturaEncabezadoDto.CAJA_CHICA.NUMERO_CAJA_CHICA, _facturaEncabezadoDto.CAJA_CHICA.DESCRIPCION, Convert.ToInt16(Cargo_AbonoEnum.ABONO), Math.Round(cuentaPorPagar, 2), string.Empty));
                }

                if (!ValidarPartidaContable(ref _listaRegistroContableDto))
                    throw new ExcepcionesDIPCMI("Partida contable no cuadrada");

                //
                if (_facturaDetalle.BuscarDetalleFacturas(_facturaEncabezadoDto.ID_FACTURA).Count > 0)
                    _facturaDetalle.AnularDetalleFactura(_facturaEncabezadoDto.ID_FACTURA);

                //Almacena registro a base de datos del detalle de la factura.
                foreach (FacturaDetalleDTO facDetalleDto in _facturaEncabezadoDto.FACTURA_DETALLE)
                {

                    if (facDetalleDto.ID_FACTURA_DETALLE.Equals(0))
                    {
                        facDetalleDto.ID_FACTURA = _facturaEncabezadoDto.ID_FACTURA;

                        //Inserta registro.
                        _facturaDetalle.EjecutarSentenciaInsert(facDetalleDto);
                    }
                    else
                        //Actualiza registro.
                        _facturaDetalle.EjecutarSenteciaUpdate(facDetalleDto);
                }

                if (_registroContable.BuscarRegistroContable(_facturaEncabezadoDto.ID_FACTURA).Count > 0)
                    _registroContable.AnularRegistroContable(_facturaEncabezadoDto.ID_FACTURA);

                //Almacena el registro contable.
                foreach (RegistroContableDTO regContDto in _listaRegistroContableDto)
                {
                    regContDto.ID_FACTURA = _facturaEncabezadoDto.ID_FACTURA;
                    _registroContable.EjecutarSentenciaInsert(regContDto);
                }


                scope.Complete();
            }
            catch (ExcepcionesDIPCMI ex)
            {

                _logErrores.SentenciaInsert(LlenarError(ex.Message, _facturaEncabezadoDto.USUARIO_MANTENIMIENTO.USUARIO_ALTA, "AlmacenarFactura"));
                throw;
            }
            catch (Exception ex)
            {
                _logErrores.SentenciaInsert(LlenarError(ex.Message, _facturaEncabezadoDto.USUARIO_MANTENIMIENTO.USUARIO_ALTA, "AlmacenarFactura"));
                throw;
            }
            finally
            {
                if (cnnSql.State != ConnectionState.Closed) cnnSql.Close();
                if (cnnError.State != ConnectionState.Closed) cnnError.Close();
                if (scope != null) scope.Dispose();
            }
        }

        private LogErroresDTO LlenarError(string error, string usuario, string funcion)
        {
            LogErroresDTO logError = new LogErroresDTO();
            logError.DESCRIPCION = error;
            logError.USUARIO = usuario;
            logError.FUNCION = funcion;

            return logError;
        }

        private bool ValidarPartidaContable(ref List<RegistroContableDTO> listaRegistroContable)
        {
            double valorAbono = 0;
            double valorCargo = 0;
            double valorRetencion = 0;
            List<RegistroContableDTO> listaRegCont = new List<RegistroContableDTO>();

            listaRegCont = listaRegistroContable.FindAll(x => x.CARGO_ABONO == Convert.ToInt16(Cargo_AbonoEnum.CARGO));
            valorCargo = listaRegCont.Sum(x => x.VALOR);

            listaRegCont = new List<RegistroContableDTO>();
            listaRegCont = listaRegistroContable.FindAll(x => x.CARGO_ABONO == Convert.ToInt16(Cargo_AbonoEnum.ABONO));
            valorAbono = listaRegCont.Sum(x => x.VALOR);

            listaRegCont = new List<RegistroContableDTO>();
            listaRegCont = listaRegistroContable.FindAll(x => x.CARGO_ABONO == Convert.ToInt16(Cargo_AbonoEnum.RETENCION));
            valorRetencion = listaRegCont.Sum(x => x.VALOR);

            return Math.Round(valorAbono, 2) == Math.Round(valorCargo, 2) ? true : false;
        }

        private double CalcularRetencionISR(double valorCompra, TipoImpuestosEnum tipoISR, Valores_ISR _valoresISR)
        {

            FacturaDetalleDTO _facDetalleDto = new FacturaDetalleDTO();
            Valores_ISRDTO _valoresisrDto = new Valores_ISRDTO();

            _valoresisrDto = _valoresISR.BuscarIsrRango(Convert.ToInt16(tipoISR), valorCompra);

            if (_valoresisrDto == null)
                throw new ExcepcionesDIPCMI("No hay configuración de ISR parametrizada.");

            return ((valorCompra - Convert.ToDouble(_valoresisrDto.RANGO_INICIAL + 0.01)) * (Convert.ToDouble(_valoresisrDto.TIPO_IMPOSITIVO) / 100)) + _valoresisrDto.IMPORTE_FIJO;
        }

        public double CalcularRetencionISR(double valorCompra, TipoImpuestosEnum tipoISR)
        {
            FacturaDetalleDTO _facDetalleDto = new FacturaDetalleDTO();
            Valores_ISRDTO _valoresisrDto = new Valores_ISRDTO();

            if (_valoresISR == null) _valoresISR = new Valores_ISR(cnnSql);

            try
            {
                cnnSql.Open();

                if (valorCompra == 0)
                    return 0;

                _valoresisrDto = _valoresISR.BuscarIsrRango(Convert.ToInt16(tipoISR), valorCompra);

                if (_valoresisrDto == null)
                    throw new ExcepcionesDIPCMI("No hay configuración de ISR parametrizada.");

                return ((valorCompra - Convert.ToDouble(_valoresisrDto.RANGO_INICIAL + 0.01)) * (Convert.ToDouble(_valoresisrDto.TIPO_IMPOSITIVO) / 100)) + _valoresisrDto.IMPORTE_FIJO;
            }
            finally
            {
                if (cnnSql.State != ConnectionState.Closed) cnnSql.Close();
            }
        }

        public double CalcularRetencionIVA(double valorCompra, Int16 ivaNormal, Int16 retencionIVA)
        {
            //=((B9/1.12)*0.12)*0.15
            return ((valorCompra / (1 + Convert.ToSingle(ivaNormal) / 100)) * Convert.ToSingle(ivaNormal) / 100) * (Convert.ToSingle(retencionIVA) / 100);
        }

        private RegistroContableDTO LlenarObjetoRegistroContable(Int16 correlativo, string cuentaContable, string definicionCC, Int16 cargoAbono, double valor, string indicadorIva)
        {
            RegistroContableDTO _registroContableDto = new RegistroContableDTO();

            _registroContableDto.CORRELATIVO = correlativo;
            _registroContableDto.CUENTA_CONTABLE = cuentaContable;
            _registroContableDto.DEFINICION_CUENTA_CONTABLE = definicionCC;
            _registroContableDto.CARGO_ABONO = cargoAbono;
            _registroContableDto.VALOR = valor;
            _registroContableDto.INDICADOR_IVA = indicadorIva;
            _registroContableDto.ALTA = true;

            return _registroContableDto;
        }

        public List<FacturaEncabezadoDTO> BuscarFacturasCajaChica(decimal idCajaChica, Int16 estado)
        {
            if (_facturaEncabezado == null) _facturaEncabezado = new FacturaEncabezado(cnnSql);

            try
            {
                cnnSql.Open();

                return _facturaEncabezado.BuscarFacturasCajaChica(idCajaChica, estado);
            }
            finally
            {
                if (cnnSql.State != ConnectionState.Closed) cnnSql.Close();
            }
        }

        public List<FacturaEncabezadoDTO> BuscarFacturasCajaChica(decimal idCajaChica, Int16 estado, string identificacion, string serie, string numero)
        {
            if (_facturaEncabezado == null) _facturaEncabezado = new FacturaEncabezado(cnnSql);

            try
            {
                cnnSql.Open();

                return _facturaEncabezado.BuscarFacturasCajaChica(idCajaChica, estado, identificacion, serie, numero);
            }
            finally
            {
                if (cnnSql.State != ConnectionState.Closed) cnnSql.Close();
            }
        }

        public List<FacturaEncabezadoDTO> BuscarFacturasCajaChica(decimal idCajaChica, Int16 estadoCajaChica, Int16 estadoFactura)
        {
            if (_facturaEncabezado == null) _facturaEncabezado = new FacturaEncabezado(cnnSql);

            try
            {
                cnnSql.Open();

                return _facturaEncabezado.BuscarFacturasCajaChica(idCajaChica, estadoCajaChica, estadoFactura);
            }
            finally
            {
                if (cnnSql.State != ConnectionState.Closed) cnnSql.Close();
            }
        }

        public List<FacturaEncabezadoDTO> BuscarFacturasCajaChica(decimal idCajaChica, Int16 estadoCajaChica, Int16 estadoFactura, string identificacion, string serie, string numero)
        {
            if (_facturaEncabezado == null) _facturaEncabezado = new FacturaEncabezado(cnnSql);

            try
            {
                cnnSql.Open();

                return _facturaEncabezado.BuscarFacturasCajaChica(idCajaChica, estadoCajaChica, estadoFactura, identificacion, serie, numero);
            }
            finally
            {
                if (cnnSql.State != ConnectionState.Closed) cnnSql.Close();
            }
        }

        public FacturaEncabezadoDTO BuscarFactura(decimal idFactura)
        {
            FacturaEncabezadoDTO _facturaDto = null;
            CajaChicaEncabezadoDTO _cajaChicaDto = null;
            List<FacturaDetalleDTO> _listaFacturaDetalleDto = null;
            if (_facturaEncabezado == null) _facturaEncabezado = new FacturaEncabezado(cnnSql);
            if (_facturaDetalle == null) _facturaDetalle = new FacturaDetalle(cnnSql);
            if (_cajaChica == null) _cajaChica = new CajaChica(cnnSql);

            try
            {
                cnnSql.Open();
                _facturaDto = _facturaEncabezado.EjecutarSentenciaSelect(idFactura);

                if (_facturaDto != null)
                {
                    _listaFacturaDetalleDto = _facturaDetalle.BuscarDetalleFacturas(idFactura);
                    _cajaChicaDto = _cajaChica.EjecutarSentenciaSelect(_facturaDto.ID_CAJA_CHICA);
                    _facturaDto.FACTURA_DETALLE.AddRange(_listaFacturaDetalleDto);
                    _facturaDto.CAJA_CHICA = _cajaChicaDto;
                }
            }
            finally
            {
                if (cnnSql.State != ConnectionState.Closed) cnnSql.Close();
            }

            return _facturaDto;
        }

        public string DistribuirNombreProveedor(ref string nombreProveedor)
        {
            string nombreResultado = string.Empty;

            if (nombreProveedor.Length < 35)
            {
                nombreResultado = nombreProveedor;
                nombreProveedor = string.Empty;
                return nombreResultado;
            }

            nombreResultado = nombreProveedor.Substring(0, 35);
            int largoNombre = nombreProveedor.Length;
            int espacioBlanco = 0;

            nombreProveedor = nombreProveedor.Substring(nombreResultado.Length, largoNombre - nombreResultado.Length);
            espacioBlanco = nombreResultado.LastIndexOf(' ');
            nombreProveedor = string.Concat(nombreResultado.Substring(espacioBlanco, nombreResultado.Length - espacioBlanco), nombreProveedor);
            nombreResultado = nombreResultado.Remove(espacioBlanco, nombreResultado.Length - espacioBlanco);

            return nombreResultado;
        }

        public Int32 AceptarFactura(decimal idFactura, string usuario, string codigoSociedad)
        {
            FacturaEncabezadoDTO _facturaEncabezadoDto = null;
            IntermediaFacturaEncabezadoDTO _intermediaFacturaEncabezadoDto = null;
            IntermediaFacturaDetalleDTO _intermediaFacturaDetalleDto = null;
            SociedadDTO _sociedadDto = null;
            TipoDocumentoDTO _tipoDocumentoDto = null;
            Int64 correlativo = 0;
            Int16 posicion = 0;
            SqlTransaction _tranRegFac = null;
            SqlTransaction _tranInterfaces = null;
            string nombreProv = string.Empty;
            string nombreProv1 = string.Empty;
            string nombreProv2 = string.Empty;
            string nombreProv3 = string.Empty;
            string nombreTemp = string.Empty;
            Int32 FacturaDividida = 0;
            Int32 BUZEI = 0;
            double BaseFE = 0.95;
            double BaseIVA = 1.12;
            double BaseISR = 0.05;
            double IVAFE = 0.12;
            //double ISRFE = 0.05;
            decimal ValorconISR = 0;
            decimal TotalBaseFE = 0;
            Int64 NumeroDocumento = 0;

            try
            {
                cnnSql.Open();
                _tranRegFac = cnnSql.BeginTransaction();

                cnnInterfaces.Open();
                _tranInterfaces = cnnInterfaces.BeginTransaction();

                if (_facturaEncabezado == null) _facturaEncabezado = new FacturaEncabezado(cnnSql, _tranRegFac);
                if (_facturaDetalle == null) _facturaDetalle = new FacturaDetalle(cnnSql, _tranRegFac);
                if (_sociedad == null) _sociedad = new Sociedad(cnnSql, _tranRegFac);
                if (_tipoDocumento == null) _tipoDocumento = new TipoDocumento(cnnSql, _tranRegFac);
                if (_intermediaFacturaEncabezado == null) _intermediaFacturaEncabezado = new IntermediaFacturaEncabezado(cnnInterfaces, _tranInterfaces);
                if (_intermediaFacturaDetalle == null) _intermediaFacturaDetalle = new IntermediaFacturaDetalle(cnnInterfaces, _tranInterfaces);
                if (_cajaChica == null) _cajaChica = new CajaChica(cnnSql, _tranRegFac);

                //if (_intermediaFacturaEncabezado == null) _intermediaFacturaEncabezado = new IntermediaFacturaEncabezado(cnnInterfaces, _tranInterfaces);
                //if (_intermediaFacturaDetalle == null) _intermediaFacturaDetalle = new IntermediaFacturaDetalle(cnnInterfaces, _tranInterfaces);

                if (FacturaDividida == null) FacturaDividida = 0;
                //Buscando datos de encabezado de factura.
                _facturaEncabezadoDto = _facturaEncabezado.EjecutarSentenciaSelect(idFactura);
                if (_facturaEncabezadoDto.VALOR_RETENCION_IVA == null) _facturaEncabezadoDto.VALOR_RETENCION_IVA = 0;

                if (_facturaEncabezadoDto.ESTADO > 1)
                    return 0;

                //Validar si la factura tiene marca de dividida, si tiene la marca buscar en la intermedia si ya existe un primer registro 
                if (_facturaEncabezadoDto.FACTURA_DIVIDIDA == true)
                {
                    FacturaDividida = _intermediaFacturaEncabezado.BuscarFacturaDividida(codigoSociedad, _facturaEncabezadoDto.NUMERO, _facturaEncabezadoDto.NUMERO_IDENTIFICACION, _facturaEncabezadoDto.SERIE);
                    BUZEI = _intermediaFacturaEncabezado.ObtenerBUZEIFactDiv(codigoSociedad, FacturaDividida);
                }

                //Actualizando estado de factura.
                _facturaEncabezado.AceptarFactura(idFactura, usuario);

                //Buscando datos de sociedad.
                _sociedadDto = _sociedad.EjecutarSentenciaSelect(codigoSociedad);

                //Buscando datos caja chica.
                _facturaEncabezadoDto.CAJA_CHICA = _cajaChica.EjecutarSentenciaSelect(_facturaEncabezadoDto.ID_CAJA_CHICA);

                //Buscando datos de tipo de documento.
                _tipoDocumentoDto = _tipoDocumento.EjecutarSentenciaSelect(_facturaEncabezadoDto.ID_TIPO_DOCUMENTO);

                //Buscando datos de detalle de factura.
                _facturaEncabezadoDto.FACTURA_DETALLE.AddRange(_facturaDetalle.BuscarDetalleFacturas(idFactura).FindAll(x => x.CANTIDAD > 0));


                //Obteniendo el último correlativo de sociedad.
                correlativo = _intermediaFacturaEncabezado.BuscarCorrelativo(codigoSociedad);

                //////Buscar acumulado de Retencion ISR para factura dividad 
                ////_facturaEncabezadoDto.TOTALFACTURADIVIDADISR = 

                //if (!_intermediaFacturaEncabezado.ExisteRegistro(_facturaEncabezadoDto.CAJA_CHICA.CODIGO_SOCIEDAD, correlativo + 1, DateTime.Now.ToString("yyyyMMdd"), _facturaEncabezadoDto.TIPO_FACTURA))
                //    return 0;

                //Proceso distribuye el nombre en los campos necesarios cuando este es mayor a 35 caracteres.
                nombreTemp = _facturaEncabezadoDto.NOMBRE_PROVEEDOR;

                nombreProv = DistribuirNombreProveedor(ref nombreTemp);
                nombreProv1 = DistribuirNombreProveedor(ref nombreTemp);
                nombreProv2 = DistribuirNombreProveedor(ref nombreTemp);
                nombreProv3 = DistribuirNombreProveedor(ref nombreTemp);


                //Llenando objeto encabezado tabla intermedia.
                _intermediaFacturaEncabezadoDto = new IntermediaFacturaEncabezadoDTO();

                //Valida si la factura esta dividida permanece el mismo Numero de Documento 
                if (FacturaDividida == 0)
                {
                    //_intermediaFacturaEncabezadoDto.DOCUMENT = correlativo + 1;
                    NumeroDocumento = correlativo + 1;
                }
                else
                {
                    //_intermediaFacturaEncabezadoDto.DOCUMENT = FacturaDividida;
                    NumeroDocumento = FacturaDividida;
                }

                _intermediaFacturaEncabezadoDto.BUKRS = codigoSociedad;
                _intermediaFacturaEncabezadoDto.DOCUMENT = NumeroDocumento; // correlativo + 1;
                _intermediaFacturaEncabezadoDto.BLDAT = _facturaEncabezadoDto.FECHA_FACTURA.ToString("yyyyMMdd");
                _intermediaFacturaEncabezadoDto.TYPE = _facturaEncabezadoDto.TIPO_FACTURA;
                //_intermediaFacturaEncabezadoDto.BUDAT = string.IsNullOrEmpty(_facturaEncabezadoDto.USUARIO_MANTENIMIENTO.FECHA_MODIFICACION.ToString()) ? _facturaEncabezadoDto.USUARIO_MANTENIMIENTO.FECHA_ALTA.ToString("yyyyMMdd") : Convert.ToDateTime(_facturaEncabezadoDto.USUARIO_MANTENIMIENTO.FECHA_MODIFICACION).ToString("yyyyMMdd");
                _intermediaFacturaEncabezadoDto.BUDAT = DateTime.Now.ToString("yyyyMMdd");
                _intermediaFacturaEncabezadoDto.XBLNR = _facturaEncabezadoDto.NUMERO.ToString();
                _intermediaFacturaEncabezadoDto.BKTXT = _facturaEncabezadoDto.CAJA_CHICA.DESCRIPCION;
                _intermediaFacturaEncabezadoDto.BLART = _facturaEncabezadoDto.ES_ESPECIAL == true ? TipoFacturaEnum.KE.ToString() : TipoFacturaEnum.KR.ToString();
                _intermediaFacturaEncabezadoDto.CURRENCY = string.IsNullOrEmpty(_facturaEncabezadoDto.CAJA_CHICA.MONEDA) ? _sociedadDto.MONEDA : _facturaEncabezadoDto.CAJA_CHICA.MONEDA;
                //_intermediaFacturaEncabezadoDto.KURSF = 1;
                _intermediaFacturaEncabezadoDto.KURSF = decimal.Zero;
                _intermediaFacturaEncabezadoDto.RECORDMODE = null;
                _intermediaFacturaEncabezadoDto.NAME = nombreProv;
                _intermediaFacturaEncabezadoDto.NAME2 = nombreProv1;
                _intermediaFacturaEncabezadoDto.NAME3 = nombreProv2;
                _intermediaFacturaEncabezadoDto.NAME4 = nombreProv3;

                _intermediaFacturaEncabezadoDto.ORT01 = _sociedadDto.PAIS;
                _intermediaFacturaEncabezadoDto.STCD1 = _facturaEncabezadoDto.TIPO_DOCUMENTO == "NRC" ? (_facturaEncabezadoDto.TIPO_DOCUMENTO2 == "NRC" ? string.Empty : _facturaEncabezadoDto.NUMERO_IDENTIFICACION2) : _facturaEncabezadoDto.NUMERO_IDENTIFICACION;  //_facturaEncabezadoDto.NUMERO_IDENTIFICACION;
                //_intermediaFacturaEncabezadoDto.STCD2 = _facturaEncabezadoDto.TIPO_DOCUMENTO2 == "NRC" ? (_facturaEncabezadoDto.TIPO_DOCUMENTO == "NRC" ? _facturaEncabezadoDto.NUMERO_IDENTIFICACION : string.Empty) : _facturaEncabezadoDto.NUMERO_IDENTIFICACION2;
                _intermediaFacturaEncabezadoDto.STCD2 = _facturaEncabezadoDto.TIPO_DOCUMENTO2 == "NRC" ? _facturaEncabezadoDto.NUMERO_IDENTIFICACION2 : (_facturaEncabezadoDto.TIPO_DOCUMENTO == "NRC" ? _facturaEncabezadoDto.NUMERO_IDENTIFICACION : string.Empty);
                //_facturaEncabezadoDto.TIPO_DOCUMENTO2 == "NRC" ? _facturaEncabezadoDto.NUMERO_IDENTIFICACION2 : string.Empty; //string.Empty;
                _intermediaFacturaEncabezadoDto.DUMMY = null;
                if (_sociedadDto.PAIS == "SV")
                {
                    string tipoproveedor;
                    if (_facturaEncabezadoDto.TIPO == true) tipoproveedor = "1"; else tipoproveedor = "0";
                    _intermediaFacturaEncabezadoDto.ZSTCDT = tipoproveedor;
                }
                else
                {
                    _intermediaFacturaEncabezadoDto.ZSTCDT = _facturaEncabezadoDto.ES_ESPECIAL == true ? _tipoDocumentoDto.TIPO_DOCUMENTO : string.Empty;
                }

                _intermediaFacturaDetalleDto = new IntermediaFacturaDetalleDTO();
                //Valida si la factura esta dividida permanece el mismo Numero de Documento 
                if (FacturaDividida == 0)
                {
                    //Llenado objeto detalle tabla intermedia datos proveedor.
                    // _intermediaFacturaDetalleDto = new IntermediaFacturaDetalleDTO();

                    posicion = Convert.ToInt16(posicion + 1);

                    _intermediaFacturaDetalleDto.BUKRS = codigoSociedad;

                    _intermediaFacturaDetalleDto.DOCUMENT = NumeroDocumento; //correlativo + 1;
                    _intermediaFacturaDetalleDto.BLDAT = _facturaEncabezadoDto.FECHA_FACTURA.ToString("yyyyMMdd");//igual a zfbdt
                    _intermediaFacturaDetalleDto.TYPE = _facturaEncabezadoDto.TIPO_FACTURA;
                    _intermediaFacturaDetalleDto.BUDAT = string.IsNullOrEmpty(_facturaEncabezadoDto.USUARIO_MANTENIMIENTO.FECHA_MODIFICACION.ToString()) ? _facturaEncabezadoDto.USUARIO_MANTENIMIENTO.FECHA_ALTA.ToString("yyyyMMdd") : Convert.ToDateTime(_facturaEncabezadoDto.USUARIO_MANTENIMIENTO.FECHA_MODIFICACION).ToString("yyyyMMdd");
                    _intermediaFacturaDetalleDto.BUZEI = posicion; //revisar
                    _intermediaFacturaDetalleDto.DUMMY = string.Empty;
                    _intermediaFacturaDetalleDto.BSCHL = Convert.ToInt32(ClaveContabilidadEnum.PROVEEDOR).ToString();
                    _intermediaFacturaDetalleDto.HKONT = _facturaEncabezadoDto.CAJA_CHICA.CAJA_CHICA_SAP;// _sociedadDto.CUENTA_PROVEEDOR; //preguntar

                    if (_facturaEncabezadoDto.FACTURA_DIVIDIDA == null) _facturaEncabezadoDto.FACTURA_DIVIDIDA = false;
                    if (_facturaEncabezadoDto.FACTURA_DIVIDIDA == false)
                    {
                        _intermediaFacturaDetalleDto.WRIVA = _facturaEncabezadoDto.ES_ESPECIAL ? 0 : Math.Round((decimal)_facturaEncabezadoDto.IVA, 2);
                        if (_facturaEncabezadoDto.RETENCION_ISR == true)
                        {
                            _intermediaFacturaDetalleDto.WRBTR = Math.Round((Convert.ToDecimal((decimal)_facturaEncabezadoDto.VALOR_TOTAL - _facturaEncabezadoDto.TOTALFACTURADIVIDADISR - (decimal)_facturaEncabezadoDto.IMPUESTO)), 2);
                            // _intermediaFacturaDetalleDto.WRBTR = Math.Round((Convert.ToDecimal((decimal)_facturaEncabezadoDto.VALOR_TOTAL - Math.Round((decimal)(_facturaEncabezadoDto.VALOR_RETENCION_ISR),2) - (decimal)_facturaEncabezadoDto.IMPUESTO)),2);
                            // _intermediaFacturaDetalleDto.WRBTR = Math.Round((Convert.ToDecimal((decimal)_facturaEncabezadoDto.VALOR_TOTAL - Math.Round((decimal)(_facturaEncabezadoDto.TOTALFACTURADIVIDADISR), 2) - (decimal)_facturaEncabezadoDto.IMPUESTO - (decimal)_facturaEncabezadoDto.IVA)), 2);
                        }
                        else
                        {
                            //------------------------SATB--27.03.17--Pruebas para Factura Especial-----------------------------------------
                            //_intermediaFacturaDetalleDto.WRBTR = (decimal)_facturaEncabezadoDto.VALOR_TOTAL;//_facturaEncabezadoDto.ES_ESPECIAL ? (decimal)_facturaEncabezadoDto.VALOR_TOTAL - (decimal)_facturaEncabezadoDto.IVA : (decimal)_facturaEncabezadoDto.VALOR_TOTAL;
                            _intermediaFacturaDetalleDto.WRBTR = _facturaEncabezadoDto.ES_ESPECIAL ? (decimal)Math.Round((((_facturaEncabezadoDto.VALOR_TOTAL) / BaseIVA) * BaseFE), 2) : (decimal)Math.Round((_facturaEncabezadoDto.VALOR_TOTAL), 2);
                            //_intermediaFacturaDetalleDto.WRIVA = _facturaEncabezadoDto.ES_ESPECIAL ? 0 : (decimal)_facturaEncabezadoDto.IVA;
                        }
                    }
                    //------------------------FIN--SATB--27.03.17--Pruebas para Factura Especial-------------------------------------
                    else
                    {
                        _intermediaFacturaDetalleDto.WRIVA = _facturaEncabezadoDto.ES_ESPECIAL ? 0 : (decimal)Math.Round(_facturaEncabezadoDto.IVA, 2);
                        if (_facturaEncabezadoDto.RETENCION_ISR == true)
                        {
                            // _intermediaFacturaDetalleDto.WRIVA = 0;
                            // _intermediaFacturaDetalleDto.WRBTR = Convert.ToDecimal(_facturaEncabezadoDto.VALOR_TOTAL - _facturaEncabezadoDto.VALOR_RETENCION_ISR - _facturaEncabezadoDto.IMPUESTO);
                            _intermediaFacturaDetalleDto.WRBTR = Math.Round(((decimal)_facturaEncabezadoDto.ACUMULADO - (decimal)_facturaEncabezadoDto.TOTALFACTURADIVIDADISR - (decimal)_facturaEncabezadoDto.IMPUESTO), 2);
                        }
                        else
                        {
                            _intermediaFacturaDetalleDto.WRBTR = _facturaEncabezadoDto.ES_ESPECIAL ? (decimal)Math.Round(Convert.ToDecimal(((_facturaEncabezadoDto.TOTALFACTURADIVIDIDA) / BaseIVA) * BaseFE), 2) : (decimal)Math.Round(Convert.ToDecimal(_facturaEncabezadoDto.TOTALFACTURADIVIDIDA), 2);
                            //_intermediaFacturaDetalleDto.WRBTR = _facturaEncabezadoDto.ES_ESPECIAL ? (decimal)_facturaEncabezadoDto.TOTALFACTURADIVIDIDA - (decimal)_facturaEncabezadoDto.IVA : (decimal)_facturaEncabezadoDto.TOTALFACTURADIVIDIDA;
                            // _intermediaFacturaDetalleDto.WRIVA = _facturaEncabezadoDto.ES_ESPECIAL ? 0 : (decimal)_facturaEncabezadoDto.IVA;
                        }
                    }

                    ///-----INI Valida si es escenario IPES Consumidor Final

                    foreach (FacturaDetalleDTO facDetalle in _facturaEncabezadoDto.FACTURA_DETALLE)
                    {
                       // if (facDetalle.IDENTIFICADOR_IVA == "CF" && _facturaEncabezadoDto.RETENCION_IVA == true)
                            if (_facturaEncabezadoDto.RETENCION_IVA == true)
                        {
                            _intermediaFacturaDetalleDto.WRBTR = _intermediaFacturaDetalleDto.WRBTR - Convert.ToDecimal(_facturaEncabezadoDto.VALOR_RETENCION_IVA);
                        }

                    }


                    ///-----INI Valida si es escenario IPES Consumidor Final

                    _intermediaFacturaDetalleDto.MWSKZ = _tipoDocumentoDto.TIPO_DOCUMENTO; //_facturaEncabezadoDto.ES_ESPECIAL ? _tipoDocumentoDto.TIPO_DOCUMENTO : string.Empty;
                    //------------------------FIN--SATB--27.03.17--Pruebas para Factura Especial-------------------------------------
                    _intermediaFacturaDetalleDto.SGTXT = "";
                    _intermediaFacturaDetalleDto.SGTXT2 = null;
                    _intermediaFacturaDetalleDto.KOSTL = string.Empty;
                    _intermediaFacturaDetalleDto.AUFNR = string.Empty;
                    _intermediaFacturaDetalleDto.ZUONR = _facturaEncabezadoDto.SERIE;
                    _intermediaFacturaDetalleDto.GSBER = "";//preguntar
                    _intermediaFacturaDetalleDto.ZFBDT = DateTime.Now.ToString("yyyyMMdd"); //_facturaEncabezadoDto.FECHA_FACTURA.ToString("yyyyMMdd");//igual a bldat
                    _intermediaFacturaDetalleDto.UMSKZ = null;
                    _intermediaFacturaDetalleDto.CO_AREA = null;
                    _intermediaFacturaDetalleDto.S_WORKP = null;
                    _intermediaFacturaDetalleDto.ACCT_TYPE = null;
                    _intermediaFacturaDetalleDto.UNIT = null;

                    _intermediaFacturaEncabezadoDto.INTERMEDIA_FACTURA_DETALLE.Add(_intermediaFacturaDetalleDto);

                }

                //Llenado objeto detalle tabla intermedia datos gasto.
                foreach (FacturaDetalleDTO facDetalle in _facturaEncabezadoDto.FACTURA_DETALLE)
                {
                    _intermediaFacturaDetalleDto = new IntermediaFacturaDetalleDTO();

                    posicion = Convert.ToInt16(posicion + 1);

                    //Valida si la factura esta dividida permanece el mismo Numero de Documento 
                    if (FacturaDividida == 0)
                    {

                        _intermediaFacturaDetalleDto.DOCUMENT = NumeroDocumento; //correlativo + 1;
                        _intermediaFacturaDetalleDto.BUZEI = posicion;
                    }
                    else
                    {
                        _intermediaFacturaDetalleDto.DOCUMENT = NumeroDocumento; // FacturaDividida;
                        _intermediaFacturaDetalleDto.BUZEI = BUZEI + posicion;
                    }

                    _intermediaFacturaDetalleDto.BUKRS = codigoSociedad;
                    _intermediaFacturaDetalleDto.DOCUMENT = NumeroDocumento; // correlativo + 1;
                    _intermediaFacturaDetalleDto.BLDAT = _facturaEncabezadoDto.FECHA_FACTURA.ToString("yyyyMMdd");//igual a zfbdt
                    _intermediaFacturaDetalleDto.TYPE = _facturaEncabezadoDto.TIPO_FACTURA;
                    _intermediaFacturaDetalleDto.BUDAT = string.IsNullOrEmpty(_facturaEncabezadoDto.USUARIO_MANTENIMIENTO.FECHA_MODIFICACION.ToString()) ? _facturaEncabezadoDto.USUARIO_MANTENIMIENTO.FECHA_ALTA.ToString("yyyyMMdd") : Convert.ToDateTime(_facturaEncabezadoDto.USUARIO_MANTENIMIENTO.FECHA_MODIFICACION).ToString("yyyyMMdd");
                    //_intermediaFacturaDetalleDto.BUZEI = posicion;
                    _intermediaFacturaDetalleDto.DUMMY = string.Empty;
                    _intermediaFacturaDetalleDto.BSCHL = Convert.ToInt32(ClaveContabilidadEnum.CUENTA_DE_GASTO).ToString();
                    _intermediaFacturaDetalleDto.HKONT = facDetalle.CUENTA_CONTABLE;
                    _intermediaFacturaDetalleDto.MWSKZ = facDetalle.IDENTIFICADOR_IVA;

                    //------------------------SATB--27.03.17--Pruebas para Factura Especial-----------------------------------------
                    if (_facturaEncabezadoDto.FACTURA_DIVIDIDA == true)
                    {
                        _intermediaFacturaDetalleDto.WRIVA = _facturaEncabezadoDto.ES_ESPECIAL ? 0 : Math.Round((decimal)facDetalle.IVA, 2);
                        //  _intermediaFacturaDetalleDto.WRIVA = _facturaEncabezadoDto.ES_ESPECIAL ? 0 : (decimal)Math.Round(_facturaEncabezadoDto.IVA, 2);
                        if (_facturaEncabezadoDto.RETENCION_ISR == true)
                        {
                            //_intermediaFacturaDetalleDto.WRIVA = 0;
                            _intermediaFacturaDetalleDto.WRBTR = Math.Round((Convert.ToDecimal(_facturaEncabezadoDto.VALOR_TOTAL - _facturaEncabezadoDto.IVA)), 2);
                        }
                        else
                        {
                            decimal TotalFactDivididaEsp = 0;
                            ValorconISR = Convert.ToDecimal(_facturaEncabezadoDto.TOTALFACTURADIVIDIDA / BaseIVA);
                            TotalFactDivididaEsp = Convert.ToDecimal(Math.Round((_facturaEncabezadoDto.VALOR_TOTAL / BaseIVA), 2));
                            _intermediaFacturaDetalleDto.WRBTR = _facturaEncabezadoDto.ES_ESPECIAL ? Math.Round(TotalFactDivididaEsp, 2) : Math.Round(((decimal)facDetalle.VALOR - (decimal)facDetalle.IMPUESTO - (decimal)facDetalle.IVA), 2);
                            //_intermediaFacturaDetalleDto.WRBTR = _facturaEncabezadoDto.ES_ESPECIAL ? (decimal)_facturaEncabezadoDto.VALOR_TOTAL - (decimal)facDetalle.IVA : (decimal)facDetalle.VALOR - (decimal)facDetalle.IMPUESTO - (decimal)facDetalle.IVA;
                            //_intermediaFacturaDetalleDto.WRIVA = _facturaEncabezadoDto.ES_ESPECIAL ? 0 : (decimal)facDetalle.IVA;
                        }
                    }
                    else
                    {
                        _intermediaFacturaDetalleDto.WRIVA = _facturaEncabezadoDto.ES_ESPECIAL ? 0 : Math.Round((decimal)facDetalle.IVA, 2);
                        //_intermediaFacturaDetalleDto.WRIVA = _facturaEncabezadoDto.ES_ESPECIAL ? 0 : (decimal)Math.Round(_facturaEncabezadoDto.IVA, 2);
                        if (_facturaEncabezadoDto.RETENCION_ISR == true)
                        {
                            //_intermediaFacturaDetalleDto.WRIVA = 0;
                            _intermediaFacturaDetalleDto.WRBTR = Math.Round((Convert.ToDecimal(_facturaEncabezadoDto.VALOR_TOTAL - _facturaEncabezadoDto.IVA - facDetalle.IMPUESTO)), 2);
                        }
                        else
                        {
                            //INI------------------------SATB--31.05.17--Pruebas para Factura Especial-----------------------------------------
                            //*Cuando el Impuesto es Excento (V0), se iguala el IVA  a 0 para que no se le reste ninguna cantidad al Total 
                            if ((facDetalle.IDENTIFICADOR_IVA == "V0") || (facDetalle.IDENTIFICADOR_IVA == ""))//"V0")
                                _intermediaFacturaDetalleDto.WRBTR = Math.Round((Convert.ToDecimal(facDetalle.VALOR) - (decimal)facDetalle.IMPUESTO), 2); //Convert.ToDecimal(_facturaEncabezadoDto.VALOR_TOTAL);
                            else
                            {
                                //_intermediaFacturaDetalleDto.WRBTR = _facturaEncabezadoDto.ES_ESPECIAL ? Convert.ToDecimal(Math.Round((_facturaEncabezadoDto.VALOR_TOTAL / BaseFE), 2)) : (decimal)_facturaEncabezadoDto.VALOR_TOTAL - (decimal)facDetalle.IVA;
                                ValorconISR = Convert.ToDecimal(Math.Round((facDetalle.VALOR / BaseIVA), 2));//Convert.ToDecimal(Math.Round((_facturaEncabezadoDto.VALOR_TOTAL / BaseIVA), 2));
                                _intermediaFacturaDetalleDto.WRBTR = _facturaEncabezadoDto.ES_ESPECIAL ? Math.Round(ValorconISR, 2) : Math.Round(((decimal)facDetalle.VALOR - (decimal)facDetalle.IVA - (decimal)facDetalle.IMPUESTO), 2);//(decimal)_facturaEncabezadoDto.VALOR_TOTAL - (decimal)facDetalle.IVA;
                            }
                            //FIN------------------------SATB--31.05.17--Pruebas para Factura Especial-----------------------------------------

                            //_intermediaFacturaDetalleDto.WRBTR = Convert.ToDecimal(Math.Round((_facturaEncabezadoDto.VALOR_TOTAL / BaseFE),2)); //_facturaEncabezadoDto.ES_ESPECIAL ? (decimal)_facturaEncabezadoDto.VALOR_TOTAL - (decimal)_facturaEncabezadoDto.IVA : (decimal)facDetalle.VALOR - (decimal)facDetalle.IMPUESTO - (decimal)facDetalle.IVA;
                            TotalBaseFE = _intermediaFacturaDetalleDto.WRBTR;
                            //_intermediaFacturaDetalleDto.WRIVA = _facturaEncabezadoDto.ES_ESPECIAL ? 0 : (decimal)facDetalle.IVA;
                        }
                    }

                    /////Cambio por Retencion IPSV
                   //// if (facDetalle.IDENTIFICADOR_IVA == "CF" && _facturaEncabezadoDto.RETENCION_IVA == true)
                   // if (_facturaEncabezadoDto.RETENCION_IVA == true )
                   // {
                   //     _intermediaFacturaDetalleDto.WRBTR = _intermediaFacturaDetalleDto.WRBTR + Convert.ToDecimal(_intermediaFacturaDetalleDto.WRIVA);
                   //     _intermediaFacturaDetalleDto.MWSKZ = facDetalle.IDENTIFICADOR_IVA; //"";
                   //     //_intermediaFacturaDetalleDto.WRIVA = 0;   //Cambio por Retencion IPSV
                   //     if (_sociedadDto.PAIS != "SV")
                   //         _intermediaFacturaDetalleDto.WRIVA = 0;
                   // }
                    //------------------------FIN--SATB--27.03.17--Pruebas para Factura Especial-------------------------------------


                    //_intermediaFacturaDetalleDto.MWSKZ = facDetalle.IDENTIFICADOR_IVA;
                    _intermediaFacturaDetalleDto.SGTXT = facDetalle.DESCRIPCION;
                    _intermediaFacturaDetalleDto.SGTXT2 = null;
                    _intermediaFacturaDetalleDto.KOSTL = string.IsNullOrEmpty(_facturaEncabezadoDto.CENTRO_COSTO) ? null : _facturaEncabezadoDto.CENTRO_COSTO;
                    _intermediaFacturaDetalleDto.AUFNR = string.IsNullOrEmpty(_facturaEncabezadoDto.ORDEN_COSTO) ? null : _facturaEncabezadoDto.ORDEN_COSTO;
                    _intermediaFacturaDetalleDto.ZUONR = _facturaEncabezadoDto.SERIE;
                    _intermediaFacturaDetalleDto.GSBER = "";//preguntar
                    _intermediaFacturaDetalleDto.ZFBDT = DateTime.Now.ToString("yyyyMMdd");// _facturaEncabezadoDto.FECHA_FACTURA.ToString("yyyyMMdd");//igual a bldat
                    _intermediaFacturaDetalleDto.UMSKZ = null;
                    _intermediaFacturaDetalleDto.CO_AREA = null;
                    _intermediaFacturaDetalleDto.S_WORKP = null;
                    _intermediaFacturaDetalleDto.ACCT_TYPE = null;
                    _intermediaFacturaDetalleDto.UNIT = null;

                    _intermediaFacturaEncabezadoDto.INTERMEDIA_FACTURA_DETALLE.Add(_intermediaFacturaDetalleDto);

                    if (facDetalle.IMPUESTO > 0)
                    {
                        _intermediaFacturaDetalleDto = new IntermediaFacturaDetalleDTO();
                        posicion = Convert.ToInt16(posicion + 1);

                        //Valida si la factura esta dividida permanece el mismo Numero de Documento 
                        if (FacturaDividida == 0)
                        {

                            _intermediaFacturaDetalleDto.DOCUMENT = NumeroDocumento; //correlativo + 1;
                            _intermediaFacturaDetalleDto.BUZEI = posicion;
                        }
                        else
                        {
                            _intermediaFacturaDetalleDto.DOCUMENT = NumeroDocumento; // FacturaDividida;
                            _intermediaFacturaDetalleDto.BUZEI = BUZEI + posicion;
                        }

                        _intermediaFacturaDetalleDto.BUKRS = codigoSociedad;
                        // _intermediaFacturaDetalleDto.DOCUMENT = correlativo + 1;
                        _intermediaFacturaDetalleDto.BLDAT = _facturaEncabezadoDto.FECHA_FACTURA.ToString("yyyyMMdd");//igual a zfbdt
                        _intermediaFacturaDetalleDto.TYPE = _facturaEncabezadoDto.TIPO_FACTURA;
                        _intermediaFacturaDetalleDto.BUDAT = string.IsNullOrEmpty(_facturaEncabezadoDto.USUARIO_MANTENIMIENTO.FECHA_MODIFICACION.ToString()) ? _facturaEncabezadoDto.USUARIO_MANTENIMIENTO.FECHA_ALTA.ToString("yyyyMMdd") : Convert.ToDateTime(_facturaEncabezadoDto.USUARIO_MANTENIMIENTO.FECHA_MODIFICACION).ToString("yyyyMMdd");
                        // _intermediaFacturaDetalleDto.BUZEI = posicion;
                        _intermediaFacturaDetalleDto.DUMMY = string.Empty;
                        _intermediaFacturaDetalleDto.BSCHL = Convert.ToInt32(ClaveContabilidadEnum.CUENTA_DE_GASTO).ToString();
                        _intermediaFacturaDetalleDto.HKONT = facDetalle.CUENTA_CONTABLE;
                        _intermediaFacturaDetalleDto.WRBTR = Math.Round((decimal)facDetalle.IMPUESTO, 2); //(decimal)facDetalle.VALOR - (decimal)facDetalle.IVA;
                        _intermediaFacturaDetalleDto.WRIVA = 0;
                        if (facDetalle.IDENTIFICADOR_IVA == "NA" && _sociedadDto.PAIS == "CR")//(codigoSociedad == "1440" || codigoSociedad == "1630"))
                            _intermediaFacturaDetalleDto.MWSKZ = "NA";
                        else
                            _intermediaFacturaDetalleDto.MWSKZ = "V0"; //"V0";
                        _intermediaFacturaDetalleDto.SGTXT = facDetalle.DESCRIPCION;
                        _intermediaFacturaDetalleDto.SGTXT2 = null;
                        _intermediaFacturaDetalleDto.KOSTL = string.IsNullOrEmpty(_facturaEncabezadoDto.CENTRO_COSTO) ? null : _facturaEncabezadoDto.CENTRO_COSTO;
                        _intermediaFacturaDetalleDto.AUFNR = string.IsNullOrEmpty(_facturaEncabezadoDto.ORDEN_COSTO) ? null : _facturaEncabezadoDto.ORDEN_COSTO;
                        _intermediaFacturaDetalleDto.ZUONR = _facturaEncabezadoDto.SERIE;
                        _intermediaFacturaDetalleDto.GSBER = "";//preguntar
                        _intermediaFacturaDetalleDto.ZFBDT = DateTime.Now.ToString("yyyyMMdd");// _facturaEncabezadoDto.FECHA_FACTURA.ToString("yyyyMMdd");//igual a bldat
                        _intermediaFacturaDetalleDto.UMSKZ = null;
                        _intermediaFacturaDetalleDto.CO_AREA = null;
                        _intermediaFacturaDetalleDto.S_WORKP = null;
                        _intermediaFacturaDetalleDto.ACCT_TYPE = null;
                        _intermediaFacturaDetalleDto.UNIT = null;

                        _intermediaFacturaEncabezadoDto.INTERMEDIA_FACTURA_DETALLE.Add(_intermediaFacturaDetalleDto);

                    }


                    ///--------INI Agregar posición de Retención de IVA para escencario IPES Consumidor Final----------

                   // if (facDetalle.IDENTIFICADOR_IVA == "CF" && _facturaEncabezadoDto.RETENCION_IVA == true)
                        if (_facturaEncabezadoDto.RETENCION_IVA == true)
                    {
                        _intermediaFacturaDetalleDto = new IntermediaFacturaDetalleDTO();
                        posicion = Convert.ToInt16(posicion + 1);

                        //Valida si la factura esta dividida permanece el mismo Numero de Documento 
                        if (FacturaDividida == 0)
                        {

                            _intermediaFacturaDetalleDto.DOCUMENT = NumeroDocumento; //correlativo + 1;
                            _intermediaFacturaDetalleDto.BUZEI = posicion;
                        }
                        else
                        {
                            _intermediaFacturaDetalleDto.DOCUMENT = NumeroDocumento; // FacturaDividida;
                            _intermediaFacturaDetalleDto.BUZEI = BUZEI + posicion;
                        }

                        _intermediaFacturaDetalleDto.BUKRS = codigoSociedad;
                        _intermediaFacturaDetalleDto.BLDAT = _facturaEncabezadoDto.FECHA_FACTURA.ToString("yyyyMMdd");
                        _intermediaFacturaDetalleDto.TYPE = _facturaEncabezadoDto.TIPO_FACTURA;
                        _intermediaFacturaDetalleDto.BUDAT = string.IsNullOrEmpty(_facturaEncabezadoDto.USUARIO_MANTENIMIENTO.FECHA_MODIFICACION.ToString()) ? _facturaEncabezadoDto.USUARIO_MANTENIMIENTO.FECHA_ALTA.ToString("yyyyMMdd") : Convert.ToDateTime(_facturaEncabezadoDto.USUARIO_MANTENIMIENTO.FECHA_MODIFICACION).ToString("yyyyMMdd");
                        _intermediaFacturaDetalleDto.DUMMY = string.Empty;
                        _intermediaFacturaDetalleDto.BSCHL = "50";
                        _intermediaFacturaDetalleDto.HKONT = "2110302005";//facDetalle.CUENTA_CONTABLE;
                        _intermediaFacturaDetalleDto.WRBTR = Math.Round((decimal)_facturaEncabezadoDto.VALOR_RETENCION_IVA, 2);
                        _intermediaFacturaDetalleDto.WRIVA = 0;
                        _intermediaFacturaDetalleDto.MWSKZ = ""; // facDetalle.IDENTIFICADOR_IVA; //"";
                        _intermediaFacturaDetalleDto.SGTXT = "IVA RET. A PROVEE.";
                        _intermediaFacturaDetalleDto.SGTXT2 = null;
                        _intermediaFacturaDetalleDto.KOSTL = "";
                        _intermediaFacturaDetalleDto.AUFNR = "";
                        _intermediaFacturaDetalleDto.ZUONR = _facturaEncabezadoDto.SERIE;
                        _intermediaFacturaDetalleDto.GSBER = "";
                        _intermediaFacturaDetalleDto.ZFBDT = DateTime.Now.ToString("yyyyMMdd");
                        _intermediaFacturaDetalleDto.UMSKZ = null;
                        _intermediaFacturaDetalleDto.CO_AREA = null;
                        _intermediaFacturaDetalleDto.S_WORKP = null;
                        _intermediaFacturaDetalleDto.ACCT_TYPE = null;
                        _intermediaFacturaDetalleDto.UNIT = null;

                        _intermediaFacturaEncabezadoDto.INTERMEDIA_FACTURA_DETALLE.Add(_intermediaFacturaDetalleDto);

                    }

                    ///--------FIN Agregar posición de Retención de IVA para escencario IPES Consumidor Final----------

                }


                //---------------Inicio Pruebas para Factura Especial SATB 15-03-2017---------
                if ((_facturaEncabezadoDto.ES_ESPECIAL == true) && FacturaDividida == 0)
                {
                    for (Int16 i = 3; i <= 5; i++)
                    {
                        _intermediaFacturaDetalleDto = new IntermediaFacturaDetalleDTO();

                        posicion = i;

                        //Valida si la factura esta dividida permanece el mismo Numero de Documento 
                        if (FacturaDividida == 0)
                        {

                            _intermediaFacturaDetalleDto.DOCUMENT = NumeroDocumento; // correlativo + 1;
                            _intermediaFacturaDetalleDto.BUZEI = posicion;
                        }
                        else
                        {
                            _intermediaFacturaDetalleDto.DOCUMENT = NumeroDocumento; // FacturaDividida;
                            _intermediaFacturaDetalleDto.BUZEI = BUZEI + posicion;
                        }

                        _intermediaFacturaDetalleDto.BUKRS = codigoSociedad;
                        _intermediaFacturaDetalleDto.BLDAT = _facturaEncabezadoDto.FECHA_FACTURA.ToString("yyyyMMdd");//igual a zfbdt
                        _intermediaFacturaDetalleDto.TYPE = _facturaEncabezadoDto.TIPO_FACTURA;
                        _intermediaFacturaDetalleDto.BUDAT = string.IsNullOrEmpty(_facturaEncabezadoDto.USUARIO_MANTENIMIENTO.FECHA_MODIFICACION.ToString()) ? _facturaEncabezadoDto.USUARIO_MANTENIMIENTO.FECHA_ALTA.ToString("yyyyMMdd") : Convert.ToDateTime(_facturaEncabezadoDto.USUARIO_MANTENIMIENTO.FECHA_MODIFICACION).ToString("yyyyMMdd");
                        _intermediaFacturaDetalleDto.DUMMY = string.Empty;
                        if (i == 5) { _intermediaFacturaDetalleDto.BSCHL = "40"; }
                        else
                            _intermediaFacturaDetalleDto.BSCHL = "50";
                        switch (i)
                        {
                            case 3: _intermediaFacturaDetalleDto.HKONT = "2110301001";
                                break;
                            case 4: _intermediaFacturaDetalleDto.HKONT = "2110302004";
                                break;
                            case 5: _intermediaFacturaDetalleDto.HKONT = "1150603010";
                                break;
                        }

                        if (_facturaEncabezadoDto.FACTURA_DIVIDIDA == true)
                        {
                            if (i == 3)
                                _intermediaFacturaDetalleDto.WRBTR = Math.Round((ValorconISR * Convert.ToDecimal(BaseISR)), 2);
                            // _intermediaFacturaDetalleDto.WRBTR = Math.Round((TotalBaseFE * Convert.ToDecimal(ISRFE)), 2);
                            else
                                _intermediaFacturaDetalleDto.WRBTR = Math.Round((ValorconISR * Convert.ToDecimal(IVAFE)), 2);
                            _intermediaFacturaDetalleDto.WRIVA = 0;
                        }
                        else
                        {
                            if (i == 3)
                                _intermediaFacturaDetalleDto.WRBTR = Math.Round((ValorconISR * Convert.ToDecimal(BaseISR)), 2);
                            else
                                _intermediaFacturaDetalleDto.WRBTR = Math.Round((ValorconISR * Convert.ToDecimal(IVAFE)), 2);
                            _intermediaFacturaDetalleDto.WRIVA = 0;
                        }
                        _intermediaFacturaDetalleDto.MWSKZ = "";//facDetalle1.IDENTIFICADOR_IVA;
                        switch (i)
                        {
                            case 3: _intermediaFacturaDetalleDto.SGTXT = "RETENCION ISR DOCUMENTO FE. CC.";
                                break;
                            case 4: _intermediaFacturaDetalleDto.SGTXT = "IVA FE. CC.";
                                break;
                            case 5: _intermediaFacturaDetalleDto.SGTXT = "IVA CREDITO FE. CC.";
                                break;
                        }

                        _intermediaFacturaDetalleDto.SGTXT2 = null;
                        _intermediaFacturaDetalleDto.KOSTL = "";
                        _intermediaFacturaDetalleDto.AUFNR = string.IsNullOrEmpty(_facturaEncabezadoDto.ORDEN_COSTO) ? null : _facturaEncabezadoDto.ORDEN_COSTO;
                        _intermediaFacturaDetalleDto.ZUONR = _facturaEncabezadoDto.SERIE;
                        _intermediaFacturaDetalleDto.GSBER = "";//preguntar
                        _intermediaFacturaDetalleDto.ZFBDT = DateTime.Now.ToString("yyyyMMdd");// _facturaEncabezadoDto.FECHA_FACTURA.ToString("yyyyMMdd");//igual a bldat
                        _intermediaFacturaDetalleDto.UMSKZ = null;
                        _intermediaFacturaDetalleDto.CO_AREA = null;
                        _intermediaFacturaDetalleDto.S_WORKP = null;
                        _intermediaFacturaDetalleDto.ACCT_TYPE = null;
                        _intermediaFacturaDetalleDto.UNIT = null;

                        _intermediaFacturaEncabezadoDto.INTERMEDIA_FACTURA_DETALLE.Add(_intermediaFacturaDetalleDto);
                    }
                }
                //---------------Fin Pruebas para Factura Especial SATB 15-03-2017------------


                //---------------RETENCION 10% IPES SATB 29.06.2018 --------------------------
                if ((_facturaEncabezadoDto.RETENCION_ISR == true) && (FacturaDividida == 0))
                {
                    _intermediaFacturaDetalleDto = new IntermediaFacturaDetalleDTO();

                    posicion = Convert.ToInt16(posicion + 1);

                    _intermediaFacturaDetalleDto.BUKRS = codigoSociedad;
                    _intermediaFacturaDetalleDto.DOCUMENT = NumeroDocumento; //correlativo + 1;
                    _intermediaFacturaDetalleDto.BLDAT = _facturaEncabezadoDto.FECHA_FACTURA.ToString("yyyyMMdd");//igual a zfbdt
                    _intermediaFacturaDetalleDto.TYPE = _facturaEncabezadoDto.TIPO_FACTURA;
                    _intermediaFacturaDetalleDto.BUDAT = string.IsNullOrEmpty(_facturaEncabezadoDto.USUARIO_MANTENIMIENTO.FECHA_MODIFICACION.ToString()) ? _facturaEncabezadoDto.USUARIO_MANTENIMIENTO.FECHA_ALTA.ToString("yyyyMMdd") : Convert.ToDateTime(_facturaEncabezadoDto.USUARIO_MANTENIMIENTO.FECHA_MODIFICACION).ToString("yyyyMMdd");
                    _intermediaFacturaDetalleDto.BUZEI = posicion; //revisar
                    _intermediaFacturaDetalleDto.DUMMY = string.Empty;
                    _intermediaFacturaDetalleDto.BSCHL = "50";
                    _intermediaFacturaDetalleDto.HKONT = "2110301002";
                    _intermediaFacturaDetalleDto.WRBTR = Math.Round(Convert.ToDecimal(_facturaEncabezadoDto.TOTALFACTURADIVIDADISR), 2); //Convert.ToDecimal(_facturaEncabezadoDto.VALOR_RETENCION_ISR);
                    _intermediaFacturaDetalleDto.WRIVA = 0;
                    _intermediaFacturaDetalleDto.MWSKZ = "";
                    _intermediaFacturaDetalleDto.SGTXT = "RETENCION ISR";
                    _intermediaFacturaDetalleDto.SGTXT2 = null;
                    _intermediaFacturaDetalleDto.KOSTL = string.Empty;
                    _intermediaFacturaDetalleDto.AUFNR = string.Empty;
                    _intermediaFacturaDetalleDto.ZUONR = _facturaEncabezadoDto.SERIE;
                    _intermediaFacturaDetalleDto.GSBER = "";//preguntar
                    _intermediaFacturaDetalleDto.ZFBDT = DateTime.Now.ToString("yyyyMMdd");
                    _intermediaFacturaDetalleDto.UMSKZ = null;
                    _intermediaFacturaDetalleDto.CO_AREA = null;
                    _intermediaFacturaDetalleDto.S_WORKP = null;
                    _intermediaFacturaDetalleDto.ACCT_TYPE = null;
                    _intermediaFacturaDetalleDto.UNIT = null;

                    _intermediaFacturaEncabezadoDto.INTERMEDIA_FACTURA_DETALLE.Add(_intermediaFacturaDetalleDto);

                }
                //---------------FIN RETENCION 10% IPES SATB 29.06.2018 ----------------


                //Valida si es la primera vez que se va a insertar el encabezado 
                //si se esta insertando el detalle de la misma factura para otro CO  no se realiza el insert del encabezado 
                if (FacturaDividida == 0)
                    _intermediaFacturaEncabezado.EjecutarSenteciaInsert(_intermediaFacturaEncabezadoDto);

                foreach (IntermediaFacturaDetalleDTO facDetalle in _intermediaFacturaEncabezadoDto.INTERMEDIA_FACTURA_DETALLE)
                {
                    _intermediaFacturaDetalle.EjecutarSenteciaInsert(facDetalle);
                }

                _tranRegFac.Commit();
                _tranInterfaces.Commit();


                //INI   Inserta los datos a la Intermedia de INELDAT IPCR 
                if (_sociedadDto.PAIS == "CR") //(codigoSociedad == "1440" || codigoSociedad == "1630")
                {
                    //Buscando Régimen Proveedor.
                    if(!_facturaEncabezado.BuscaRegimenProveedorCR(_facturaEncabezadoDto.ID_PROVEEDOR))
                    _facturaEncabezado.InsertarIneldat(idFactura);
                }

                //FIN   Inserta los datos a la Intermedia de INELDAT IPCR 

            }
            catch
            {
                if (_tranRegFac != null) _tranRegFac.Rollback();
                if (_tranInterfaces != null) _tranInterfaces.Rollback();
                throw;
            }
            finally
            {
                if (cnnInterfaces.State != ConnectionState.Closed) cnnInterfaces.Close();
                if (cnnSql.State != ConnectionState.Closed) cnnSql.Close();
            }
            return 0;
        }

        public Int32 RechazarFactura(decimal idFactura, string usuario)
        {
            if (_facturaEncabezado == null) _facturaEncabezado = new FacturaEncabezado(cnnSql);

            try
            {
                cnnSql.Open();

                return _facturaEncabezado.RechazarFactura(idFactura, usuario);
            }
            finally
            {
                if (cnnSql.State != ConnectionState.Closed) cnnSql.Close();
            }
        }

        public Int32 DarVigenciaFactura(decimal idFactura, string usuario)
        {
            if (_facturaEncabezado == null) _facturaEncabezado = new FacturaEncabezado(cnnSql);

            try
            {
                cnnSql.Open();

                return _facturaEncabezado.DarVigenciaFactura(idFactura, usuario);
            }
            finally
            {
                if (cnnSql.State != ConnectionState.Closed) cnnSql.Close();
            }
        }

        public Int32 AnularFactura(decimal idFactura, string usuario)
        {
            if (_facturaEncabezado == null) _facturaEncabezado = new FacturaEncabezado(cnnSql);
            FacturaEncabezadoDTO _facturaEncabezadoDto = null;
            try
            {
                cnnSql.Open();

                _facturaEncabezadoDto = _facturaEncabezado.EjecutarSentenciaSelect(idFactura);


                //if (!string.IsNullOrEmpty(_facturaEncabezadoDto.APROBADA.ToString()))
                //{
                //    if ((bool)_facturaEncabezadoDto.APROBADA)
                //        throw new ExcepcionesDIPCMI("No se pueden modificar facturas aprobadas.");
                //}

                if (_facturaEncabezadoDto.ESTADO != 1)
                    throw new ExcepcionesDIPCMI("La factura no esta disponible para edición.");

                return _facturaEncabezado.AnularFactura(idFactura, usuario);
            }
            finally
            {
                if (cnnSql.State != ConnectionState.Closed) cnnSql.Close();
            }
        }

        public List<IndicadoresIVADTO> BuscarIndicadoresActivos(string cajaChicaSAP, string usuario)
        {
            if (_indicadoresIVA == null) _indicadoresIVA = new IndicadoresIVA(cnnSql);

            try
            {
                cnnSql.Open();

                return _indicadoresIVA.BuscarIndicadoresActivos(cajaChicaSAP, usuario);
            }
            finally
            {
                if (cnnSql.State != ConnectionState.Closed) cnnSql.Close();
            }
        }

        public IndicadoresIVADTO BuscarIndicadorIVA(string indicadorIVA, string Sociedad)
        {
            if (_indicadoresIVA == null) _indicadoresIVA = new IndicadoresIVA(cnnSql);

            try
            {
                cnnSql.Open();

                return _indicadoresIVA.EjecutarSentenciaSelect(indicadorIVA, Sociedad);
            }
            finally
            {
                if (cnnSql.State != ConnectionState.Closed) cnnSql.Close();
            }
        }

        //public FacturaEncabezadoDTO BuscarTotalFactura(int idProveedor, decimal NoFactura, string serie)
        public FacturaEncabezadoDTO BuscarTotalFactura(int idProveedor, string NoFactura, string serie)
        {
            if (_facturaEncabezado == null) _facturaEncabezado = new FacturaEncabezado(cnnSql);

            try
            {
                cnnSql.Open();

                return _facturaEncabezado.BuscarTotalFactura(idProveedor, NoFactura, serie);
            }
            finally
            {
                if (cnnSql.State == ConnectionState.Open) cnnSql.Close();
            }
        }

        //Buscar todas las facturas divididas 
        //public List<FacturaEncabezadoDTO> BuscarTodasFacturasDivididas(decimal idCajaChica, string proveedor, string serie, decimal numero)
        public List<FacturaEncabezadoDTO> BuscarTodasFacturasDivididas(decimal idCajaChica, string proveedor, string serie, string numero)
        {
            //FacturaEncabezadoDTO _facturaDto = null;
            if (_facturaEncabezado == null) _facturaEncabezado = new FacturaEncabezado(cnnSql);

            try
            {
                cnnSql.Open();
                //_facturaDto = _facturaEncabezado.BuscarFacturasDivididasRevision(idCajaChica, proveedor, serie, numero);
                return _facturaEncabezado.BuscarFacturasDivididasRevision(idCajaChica, proveedor, serie, numero);
            }
            finally
            {
                if (cnnSql.State != ConnectionState.Closed) cnnSql.Close();
            }
        }


        #endregion

        #region RegistroContable

        public List<RegistroContableSPDTO> BuscarRegistroContableSP(decimal idFactura)
        {
            if (_registroContable == null) _registroContable = new RegistroContable(cnnSql);

            try
            {
                cnnSql.Open();
                return _registroContable.BuscarRegistroContableSP(idFactura);
            }
            finally
            {
                if (cnnSql.State != ConnectionState.Closed) cnnSql.Close();
            }
        }

        #endregion

        #region IDisposable Members

        private void DisposeConnSql()
        {
            if (cnnSql != null)
            {
                if (cnnSql.State != ConnectionState.Closed)
                    cnnSql.Close();

                cnnSql.Dispose();
            }
        }

        public void Dispose()
        {
            try
            {
                DisposeConnSql();
            }
            catch
            {
                GC.SuppressFinalize(this);
            }
        }


        #endregion
    }
}
