using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using SAP.Middleware.Connector;
using System.Transactions;
using LogicaCajasChicas.Entidad;
using DipCmiGT.LogicaComun;
using DipCmiGT.LogicaCajasChicas.Entidad;
using DipCmiGT.LogicaCajasChicas;

namespace LogicaCajasChicas.Sesion
{
    public class GestorDatosSAP : IDisposable
    {

        #region Declaraciones

        SAPOrdenesGastos _sapOrdenesGastos = null;
        SAPCuentasOrdenesGastos _sapCuentaOrdenesGastos = null;
        SAPCentroCosto _sapCentroCosto = null;
        SAPCuentaContableCentroCosto _sapCuentaContableCentroCosto = null;
        SAPCajasChicas _sapCajasChicas = null;
        SAPCuentaContable _sapCuentasContables = null;

        SqlConnection cnnSql = null;

        #endregion

        #region Constructor

        public GestorDatosSAP(string conexion)
        {
            cnnSql = new SqlConnection(conexion);
        }

        #endregion

        #region CajaChica

        public void MigrarCajasChicas(RfcDestination destino, string codigoSociedad)
        {
            RfcRepository SapRfcRepository = destino.Repository;
            IRfcFunction bapiCajaChica = SapRfcRepository.CreateFunction("Z12_MF_LIFNR_CC");

            bapiCajaChica.SetValue("I_BUKRS", codigoSociedad);
            bapiCajaChica.Invoke(destino);
            IRfcTable CajaChicaIRF = bapiCajaChica.GetTable("T_LIFNR");

            if (_sapCajasChicas == null) _sapCajasChicas = new SAPCajasChicas(cnnSql);
            List<SAPCajasChicasDTO> _listaSapCajaChicaDto = new List<SAPCajasChicasDTO>();

            if (CajaChicaIRF.RowCount.Equals(0))
                throw new ExcepcionesDIPCMI("No hay registros para importar");

            for (Int32 x = 0; x <= CajaChicaIRF.RowCount - 1; x++)
            {
                _listaSapCajaChicaDto.Add(new SAPCajasChicasDTO
                {
                    BUKRS = CajaChicaIRF[x].GetString("BUKRS"),
                    LIFNR = Convert.ToInt32(CajaChicaIRF[x].GetString("LIFNR")).ToString(),
                    NAME = CajaChicaIRF[x].GetString("NAME1"),
                    WITHT = CajaChicaIRF[x].GetString("WITHT"),
                    ERDAT = Convert.ToDateTime(CajaChicaIRF[x].GetString("ERDAT"))
                });
            }

            TransactionScope ts = null;
            try
            {
                ts = new TransactionScope();
                cnnSql.Open();

                _sapCajasChicas.EjecutarSentenciaDelete(codigoSociedad);

                foreach (SAPCajasChicasDTO cc in _listaSapCajaChicaDto)
                {
                    _sapCajasChicas.EjecutarSentenciaInsert(cc);
                }

                ts.Complete();
            }
            finally
            {
                if (cnnSql.State != ConnectionState.Closed) cnnSql.Close();
                if (ts != null) ts.Dispose();
            }
        }

        public void MigrarCentroCosto(RfcDestination destino, string codigoSociedad)
        {
            RfcRepository SapRfcRepository = destino.Repository;
            IRfcFunction bapigetCentrosCuentaslist = SapRfcRepository.CreateFunction("ZBAPI_FICSKS01");

            bapigetCentrosCuentaslist.SetValue("I_BUKRS", codigoSociedad);
            bapigetCentrosCuentaslist.Invoke(destino);

            IRfcTable _centroCostoIRF = bapigetCentrosCuentaslist.GetTable("ZITEMDATA");
            IRfcTable _cuentasACentroIRF = bapigetCentrosCuentaslist.GetTable("Z_STR_Z02_GR");

            if (_sapCentroCosto == null) _sapCentroCosto = new SAPCentroCosto(cnnSql);
            if (_sapCuentaContableCentroCosto == null) _sapCuentaContableCentroCosto = new SAPCuentaContableCentroCosto(cnnSql);
            List<SAPCentroCostoDTO> _listaCentroCosto = new List<SAPCentroCostoDTO>();
            List<SAPCuentaContableCentroCostoDTO> _listaCuentaContableCentroCostoDTO = new List<SAPCuentaContableCentroCostoDTO>();

            if (_centroCostoIRF.RowCount.Equals(0) || _cuentasACentroIRF.RowCount.Equals(0))
                throw new ExcepcionesDIPCMI("no hay centros de costo y cuentas a centro de costo a importar.");

            for (Int32 x = 0; x <= _centroCostoIRF.RowCount - 1; x++)
            {
                _listaCentroCosto.Add(new SAPCentroCostoDTO
                {
                    KOKRS = _centroCostoIRF[x].GetString("KOKRS"),
                    KOSTL = _centroCostoIRF[x].GetString("KOSTL"),
                    BUKRS = _centroCostoIRF[x].GetString("BUKRS"),
                    GSBER = _centroCostoIRF[x].GetString("GSBER"),
                    KOSAR = _centroCostoIRF[x].GetString("KOSAR"),
                    VERAK = _centroCostoIRF[x].GetString("VERAK"),
                    KTEXT = _centroCostoIRF[x].GetString("KTEXT"),
                    KHINR = _centroCostoIRF[x].GetString("KHINR"),
                    BKZKP = _centroCostoIRF[x].GetString("BKZKP")
                   // LTEXT = _centroCostoIRF[x].GetString("LTEXT")
                });
            }

            for (Int32 x = 0; x <= _cuentasACentroIRF.RowCount - 1; x++)
            {
                _listaCuentaContableCentroCostoDTO.Add(new SAPCuentaContableCentroCostoDTO
                {
                    KOKRS = _cuentasACentroIRF[x].GetString("KOKRS"),
                    KOSTL = _cuentasACentroIRF[x].GetString("KOSTL"),
                    KTOPL = _cuentasACentroIRF[x].GetString("KTOPL"),
                    HKONT_LOW = _cuentasACentroIRF[x].GetString("HKONT_LOW"),
                    HKONT_HIGH = _cuentasACentroIRF[x].GetString("HKONT_HIGH"),
                    BUKRS = codigoSociedad
                });
            }

            TransactionScope ts = null;

            try
            {
                ts = new TransactionScope();
                cnnSql.Open();

                _sapCentroCosto.EjecutarSentenciaDelete(codigoSociedad);

                _sapCuentaContableCentroCosto.EjecutarSentenciaDelete(codigoSociedad);

                foreach (SAPCentroCostoDTO centroCosto in _listaCentroCosto)
                {
                    _sapCentroCosto.EjecutarSentenciaInsert(centroCosto);
                }

                foreach (SAPCuentaContableCentroCostoDTO ccCC in _listaCuentaContableCentroCostoDTO)
                {
                    _sapCuentaContableCentroCosto.EjecutarSentenciaInsert(ccCC);
                }

                ts.Complete();
            }
            finally
            {
                if (cnnSql.State != ConnectionState.Closed) cnnSql.Close();
                if (ts != null) ts.Dispose();
            }
        }

        public void MigrarOrdenCosto(RfcDestination destino, string codigoSociedad)
        {
            
            RfcRepository SapRfcRepository = destino.Repository;
            IRfcFunction bapigetCentrosCuentaslist = SapRfcRepository.CreateFunction("ZBAPI_FICSKS01");

            bapigetCentrosCuentaslist.SetValue("I_BUKRS", codigoSociedad);
            bapigetCentrosCuentaslist.Invoke(destino);

            IRfcTable _ordenesGastoIRF = bapigetCentrosCuentaslist.GetTable("Z_ZSTR_AUFK");
            IRfcTable _cuentasOrdenesGastoIRF = bapigetCentrosCuentaslist.GetTable("Z_ZSTR_Z02_GR_ACC_AUFNR");

            if (_sapOrdenesGastos == null) _sapOrdenesGastos = new SAPOrdenesGastos(cnnSql);
            if (_sapCuentaOrdenesGastos == null) _sapCuentaOrdenesGastos = new SAPCuentasOrdenesGastos(cnnSql);
            List<SAPOrdenesGastosDTO> _listaOrdenGasto = new List<SAPOrdenesGastosDTO>();
            List<SAPCuentasOrdenesGastosDTO> _listaCuentaContableOrdenGasto = new List<SAPCuentasOrdenesGastosDTO>();

            if (_ordenesGastoIRF.RowCount.Equals(0) || _cuentasOrdenesGastoIRF.RowCount.Equals(0))
                throw new ExcepcionesDIPCMI("no hay centros de costo y cuentas a centro de costo a importar.");

            for (Int32 x = 0; x <= _ordenesGastoIRF.RowCount - 1; x++)
            {
                _listaOrdenGasto.Add(new SAPOrdenesGastosDTO
                {
                    AUFNR = _ordenesGastoIRF[x].GetString("AUFNR"),
                    AUART = _ordenesGastoIRF[x].GetString("AUART"),
                    KTEXT = _ordenesGastoIRF[x].GetString("KTEXT"),
                    BUKRS = _ordenesGastoIRF[x].GetString("BUKRS"),
                    PHAS1 = _ordenesGastoIRF[x].GetString("PHAS1"),
                    PHAS2 = _ordenesGastoIRF[x].GetString("PHAS2"),
                    PHAS3 = _ordenesGastoIRF[x].GetString("PHAS3")

                });
            }

            for (Int32 x = 0; x <= _cuentasOrdenesGastoIRF.RowCount - 1; x++)
            {
                _listaCuentaContableOrdenGasto.Add(new SAPCuentasOrdenesGastosDTO
                {
                    KOKRS = _cuentasOrdenesGastoIRF[x].GetString("KOKRS"),
                    BUKRS = _cuentasOrdenesGastoIRF[x].GetString("BUKRS"),
                    AUART = _cuentasOrdenesGastoIRF[x].GetString("AUART"),
                    KTOPL = _cuentasOrdenesGastoIRF[x].GetString("KTOPL"),
                    KSTAR_LOW = _cuentasOrdenesGastoIRF[x].GetString("KSTAR_LOW"),
                    KSTAR_HIGH = _cuentasOrdenesGastoIRF[x].GetString("KSTAR_HIGH")
                });
            }

            TransactionScope ts = null;

            try
            {
                ts = new TransactionScope();
                cnnSql.Open();

                _sapOrdenesGastos.EjecutarSentenciaDelete(codigoSociedad);

                _sapCuentaOrdenesGastos.EjecutarSentenciaDelete(codigoSociedad);

                foreach (SAPOrdenesGastosDTO ordenCompra in _listaOrdenGasto)
                {
                    _sapOrdenesGastos.EjecutarSentenciaInsert(ordenCompra);
                }

                foreach (SAPCuentasOrdenesGastosDTO ocCC in _listaCuentaContableOrdenGasto)
                {
                    _sapCuentaOrdenesGastos.EjecutarSentenciaInsert(ocCC);
                }

                ts.Complete();
            }
            finally
            {
                if (cnnSql.State != ConnectionState.Closed) cnnSql.Close();
                if (ts != null) ts.Dispose();
            }


        }

        public void MigrarCuentasContables(RfcDestination destino, string codigoSociedad)
        {
            RfcRepository SapRfcRepository = destino.Repository;
            IRfcFunction bapigetCuentaslist = SapRfcRepository.CreateFunction("ZMF_FI_GET_SAKNR");

            bapigetCuentaslist.SetValue("BUKRS", codigoSociedad);
            bapigetCuentaslist.SetValue("KTOPL", "PUC1");
            bapigetCuentaslist.Invoke(destino);

            IRfcTable _cuentasContableIRF = bapigetCuentaslist.GetTable("T_SAKNR");

            if (_sapCuentasContables == null) _sapCuentasContables = new SAPCuentaContable(cnnSql);
            List<SAPCuentaContableDTO> _listaSapCuentasContables = new List<SAPCuentaContableDTO>();

            if (_cuentasContableIRF.RowCount.Equals(0))
                throw new ExcepcionesDIPCMI("No hay registros para importar");

            for (Int32 x = 0; x <= _cuentasContableIRF.RowCount - 1; x++)
            {
                _listaSapCuentasContables.Add(new SAPCuentaContableDTO
                {
                    BUKRS = _cuentasContableIRF[x].GetString("BUKRS"),
                    KTOKS = _cuentasContableIRF[x].GetString("KTOKS"),
                    TXT30 = _cuentasContableIRF[x].GetString("TXT30"),
                    SAKNR = _cuentasContableIRF[x].GetString("SAKNR"),
                    TXT50 = _cuentasContableIRF[x].GetString("TXT50")
                });
            }

            TransactionScope ts = null;
            try
            {
                ts = new TransactionScope();
                cnnSql.Open();

                _sapCuentasContables.EjecutarSentenciaDelete(codigoSociedad);

                foreach (SAPCuentaContableDTO cc in _listaSapCuentasContables)
                {
                    _sapCuentasContables.EjecutarSentenciaInsert(cc);
                }

                ts.Complete();
            }
            finally
            {
                if (cnnSql.State != ConnectionState.Closed) cnnSql.Close();
                if (ts != null) ts.Dispose();
            }
        }

        #endregion

        #region IDisposbleMembers

        private void DisposeConnSql()
        {
            if (cnnSql != null)
            {
                if (cnnSql.State != ConnectionState.Closed)
                    cnnSql.Close();
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
