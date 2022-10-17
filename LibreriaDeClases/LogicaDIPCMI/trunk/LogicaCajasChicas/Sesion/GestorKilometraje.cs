using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using DipCmiGT.LogicaComun;
using LogicaCajasChicas.Entidad;

namespace DipCmiGT.LogicaCajasChicas.Sesion
{
    public class GestorKilometraje : IDisposable
    {
         #region Declaracion
        SqlConnection cnnSql = null;
        Kilometraje _kilometraje;
        #endregion

        #region Constructor
        public GestorKilometraje(string conexion)
        {
            cnnSql = new SqlConnection(conexion);
        }
        # endregion

        #region Metodos
        public KilometrajeDTO AlmacenarKilometraje(KilometrajeDTO kilometrajeDTO)
        {
            Int32 x = 0;
            if (_kilometraje == null) _kilometraje = new Kilometraje(cnnSql);

            try
            {
                cnnSql.Open();

                if (kilometrajeDTO.CodigoKilometraje.ToString() == "" || kilometrajeDTO.CodigoKilometraje.ToString() == "0")
                {
                    x = _kilometraje.EjecutarSentenciaInsert(kilometrajeDTO);
                }
                else
                    x = _kilometraje.EjectuarSentenciaUpdate(kilometrajeDTO);
            }
            finally
            {
                if (cnnSql.State == ConnectionState.Open) cnnSql.Close();
            }

            return kilometrajeDTO;
        }

        public Int16 DarBajaKilometraje(long codigoKilometraje, string usuario)
        {
            if (_kilometraje == null) _kilometraje = new Kilometraje(cnnSql);

            try
            {
                cnnSql.Open();

                return _kilometraje.DarBajaKilometraje(codigoKilometraje, usuario);
            }
            finally
            {
                if (cnnSql.State != ConnectionState.Closed) cnnSql.Close();
            }
        }

        public Int16 DarAltaKilometraje(long codigoKilometraje, string usuario)
        {
            if (_kilometraje == null) _kilometraje = new Kilometraje(cnnSql);

            try
            {
                cnnSql.Open();

                return _kilometraje.DarAltaKilometraje(codigoKilometraje, usuario);
            }
            finally
            {
                if (cnnSql.State != ConnectionState.Closed) cnnSql.Close();
            }
        }

        public List<KilometrajeDTO> ListaKilometrajes()
        {
            if (_kilometraje == null) _kilometraje = new Kilometraje(cnnSql);
            List<KilometrajeDTO> _kilometrajeDTO = new List<KilometrajeDTO>();

            try
            {
                cnnSql.Open();
                return _kilometraje.ListaKilometrajes();
            }
            finally
            {
                if (cnnSql.State != ConnectionState.Closed) cnnSql.Close();
            }
        }

        public List<LlenarDDL_DTO> ListaPaisesDDL()
        {
            if (_kilometraje == null) _kilometraje = new Kilometraje(cnnSql);

            try
            {
                cnnSql.Open();
                return _kilometraje.ListaPaisesDDL();
            }
            finally
            {
                if (cnnSql.State != ConnectionState.Closed) cnnSql.Close();
            }
        }

        public bool ValidaExisteKilometraje(string pais, string origen, string destino)
        {
            if (_kilometraje == null) _kilometraje = new Kilometraje(cnnSql);

            try
            {
                cnnSql.Open();
                return _kilometraje.ExisteKilometraje(pais, origen, destino);
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
