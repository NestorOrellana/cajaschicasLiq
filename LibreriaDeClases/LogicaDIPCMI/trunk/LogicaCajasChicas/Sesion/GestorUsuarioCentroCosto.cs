using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using LogicaCajasChicas.Entidad;
using System.Data;
using DipCmiGT.LogicaCajasChicas;
using DipCmiGT.LogicaCajasChicas.Entidad;
using System.Transactions;
using DipCmiGT.LogicaComun;

namespace DipCmiGT.LogicaCajasChicas.Sesion
{
    public class GestorUsuarioCentroCosto : IDisposable
    {
        #region Declaracion
        SAPCentroCosto _sapCentroCosto = null;
        UsuarioCentroCosto _usuarioCentroCosto = null;
        SAPCuentaContableCentroCosto _sapCuentaContableCentroCosto = null;

        SqlConnection cnnSql = null;
        #endregion

        #region Constructor
        public GestorUsuarioCentroCosto(string conexion)
        {
            cnnSql = new SqlConnection(conexion);
        }
        #endregion

        #region UsuarioCentroCosto
        public List<UsuarioCentroCostoDTO> ListarUsuarioCentroCosto(string usuario)
        {
            if (_usuarioCentroCosto == null) _usuarioCentroCosto = new UsuarioCentroCosto(cnnSql);
            try
            {
                cnnSql.Open();
                return _usuarioCentroCosto.ListaCentroCostoUsuario(usuario);
            }
            finally
            {
                if (cnnSql.State != ConnectionState.Closed) cnnSql.Close();
            }
        }

        //public List<SAPCentroCostoDTO> ListarCentroCosto()
        //{
        //    if (_sapCentroCosto == null) _sapCentroCosto = new SAPCentroCosto(cnnSql);
        //    try
        //    {
        //        cnnSql.Open();
        //        return _sapCentroCosto.ListarCentrosCosto();
        //    }
        //    finally
        //    {
        //        if (cnnSql.State != ConnectionState.Closed) cnnSql.Close();
        //    }
        //}

        public List<LlenarDDL_DTO> ListarCentroCostoDDL(string codigoSociedad)
        {
            if (_sapCuentaContableCentroCosto == null) _sapCuentaContableCentroCosto = new SAPCuentaContableCentroCosto(cnnSql);
            try
            {
                cnnSql.Open();
                return _sapCuentaContableCentroCosto.ListarCentrosCosto(codigoSociedad);
            }
            finally
            {
                if (cnnSql.State != ConnectionState.Closed) cnnSql.Close();
            }
        }

        public UsuarioCentroCostoDTO AgregarUsuarioCentroCosto(UsuarioCentroCostoDTO usuariocentroCostoDTO)
        {
            Int32 x = 0;
            if (_usuarioCentroCosto == null) _usuarioCentroCosto = new UsuarioCentroCosto(cnnSql);

            try
            {
                cnnSql.Open();

                if (!_usuarioCentroCosto.ExisteUsuarioCentroCosto(usuariocentroCostoDTO.CODIGO_SOCIEDAD, usuariocentroCostoDTO.CENTRO_COSTO, usuariocentroCostoDTO.USUARIO, usuariocentroCostoDTO.ID_USUARIO_CENTRO_COSTO))
                    throw new ExcepcionesDIPCMI("El centro de costo ya se encuentra asignado.");

                if (usuariocentroCostoDTO.ID_USUARIO_CENTRO_COSTO == 0)
                    usuariocentroCostoDTO.ID_USUARIO_CENTRO_COSTO = _usuarioCentroCosto.EjecutarSentenciaInsert(usuariocentroCostoDTO);
                else
                    _usuarioCentroCosto.EjecutarSentenciaUpdate(usuariocentroCostoDTO);
            }
            finally
            {
                if (cnnSql.State == ConnectionState.Open) cnnSql.Close();
            }
            return usuariocentroCostoDTO;
        }

        public Int16 DarAltaCentroCosto(Int32 idCentro, string usuario)
        {
            if (_usuarioCentroCosto == null) _usuarioCentroCosto = new UsuarioCentroCosto(cnnSql);

            try
            {
                cnnSql.Open();
                return _usuarioCentroCosto.DarAltaCentroCosto(idCentro, usuario);
            }
            finally
            {
                if (cnnSql.State != ConnectionState.Closed) cnnSql.Close();
            }
        }

        public Int16 DarBajaCentroCosto(Int32 idCentro, string usuario)
        {
            if (_usuarioCentroCosto == null) _usuarioCentroCosto = new UsuarioCentroCosto(cnnSql);

            try
            {
                cnnSql.Open();
                return _usuarioCentroCosto.DarBajaCentroCosto(idCentro, usuario);
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
