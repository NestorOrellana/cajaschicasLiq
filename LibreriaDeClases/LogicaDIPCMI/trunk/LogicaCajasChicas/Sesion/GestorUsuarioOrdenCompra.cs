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
    public class GestorUsuarioOrdenCompra : IDisposable
    {
        #region Declaracion
        SAPOrdenesGastos _sapordencompra = null;
        UsuarioOrdenCompra _usuarioordencompra = null;
        SqlConnection cnnSql = null;
        #endregion

        #region Constructor
        public GestorUsuarioOrdenCompra(string conexion)
        {
            cnnSql = new SqlConnection(conexion);
        }
        #endregion

        #region UsuarioOrdeneCompra
        public List<UsuarioOrdenCompraDTO> ListarUsuarioOrdenCompra(string usuario)
        {
            if (_usuarioordencompra == null) _usuarioordencompra = new UsuarioOrdenCompra(cnnSql);
            try
            {
                cnnSql.Open();
                return _usuarioordencompra.ListaUsuarioOrdenCompra(usuario);
            }
            finally
            {
                if (cnnSql.State != ConnectionState.Closed) cnnSql.Close();
            }
        }

        public List<SAPOrdenesGastosDTO> ListaOrdenCompraFacturas()
        {
            if (_sapordencompra == null) _sapordencompra = new SAPOrdenesGastos(cnnSql);
            try
            {
                cnnSql.Open();
                return _sapordencompra.ListaOrdenCompraDDLFacturas();
            }
            finally
            {
                if (cnnSql.State != ConnectionState.Closed) cnnSql.Close();
            }
        }

        public List<SAPOrdenesGastosDTO> ListarOrdenCompraDDL(string usuario)
        {
            if (_sapordencompra == null) _sapordencompra = new SAPOrdenesGastos(cnnSql);
            try
            {
                cnnSql.Open();
                return _sapordencompra.ListaOrdenCompra(usuario);
            }
            finally
            {
                if (cnnSql.State != ConnectionState.Closed) cnnSql.Close();
            }
        }


        //public List<SAPOrdenesGastosDTO> ListarOrdenCompraDDL(string usuario)
        //{
        //    if (_sapordencompra == null) _sapordencompra = new SAPOrdenesGastos(cnnSql);
        //    try
        //    {
        //        cnnSql.Open();
        //        return _sapordencompra.ListarCentrosCostoDDL(usuario);
        //    }
        //    finally
        //    {
        //        if (cnnSql.State != ConnectionState.Closed) cnnSql.Close();
        //    }
        //}

        public UsuarioOrdenCompraDTO AgregarUsuarioOrdenCompra(UsuarioOrdenCompraDTO usuarioordencompraDTO)
        {
            Int32 x = 0;
            if (_usuarioordencompra == null) _usuarioordencompra = new UsuarioOrdenCompra(cnnSql);

            try
            {
                cnnSql.Open();
                x = _usuarioordencompra.EjecutarSentenciaInsert(usuarioordencompraDTO);
            }
            finally
            {
                if (cnnSql.State == ConnectionState.Open) cnnSql.Close();
            }
            return usuarioordencompraDTO;
        }

        public Int16 DarAltaOrdenCompra(Int32 idUsuarioOrdenCompra, string usuario)
        {
            if (_usuarioordencompra == null) _usuarioordencompra = new UsuarioOrdenCompra(cnnSql);

            try
            {
                cnnSql.Open();
                return _usuarioordencompra.DarAltaOrdenCompra(idUsuarioOrdenCompra, usuario);
            }
            finally
            {
                if (cnnSql.State != ConnectionState.Closed) cnnSql.Close();
            }
        }

        public Int16 DarBajaOrdenCompra(Int32 idUsuarioOrdenCompra, string usuario)
        {
            if (_usuarioordencompra == null) _usuarioordencompra = new UsuarioOrdenCompra(cnnSql);

            try
            {
                cnnSql.Open();
                return _usuarioordencompra.DarBajaOrdenCompra(idUsuarioOrdenCompra, usuario);
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
