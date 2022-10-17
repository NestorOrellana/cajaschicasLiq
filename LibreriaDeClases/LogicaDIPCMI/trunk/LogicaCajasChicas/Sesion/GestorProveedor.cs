using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using DipCmiGT.LogicaCajasChicas.Entidad;
using System.Data;
using DipCmiGT.LogicaComun.Util;
using DipCmiGT.LogicaComun;
using LogicaComun.Enum;
using LogicaCajasChicas.Entidad;
using LogicaCajasChicas;
using System.Text.RegularExpressions;

namespace DipCmiGT.LogicaCajasChicas.Sesion
{
    public class GestorProveedor : IDisposable
    {
        #region Declaracion

        Proveedor _proveedor = null;
        TipoDocumento _tipoDocumento = null;
        SqlConnection cnnSql = null;

        #endregion

        #region Constructor

        public GestorProveedor(string conexion)
        {
            cnnSql = new SqlConnection(conexion);
        }

        #endregion

        #region Proveedor

        /// <summary>
        /// Método que busca un proveedor
        /// </summary>
        /// <param name="idProveedor">Id del proveedor</param>
        /// <returns>ProveedorDTO</returns>
        public ProveedorDTO BuscarProveedor(Int32 idProveedor)
        {
            if (_proveedor == null) _proveedor = new Proveedor(cnnSql);

            try
            {
                cnnSql.Open();

                return _proveedor.EjecutarSentenciaSelect(idProveedor);
            }
            finally
            {
                if (cnnSql.State == ConnectionState.Open) cnnSql.Close();
            }
        }

        public ProveedorDTO BuscarProveedor(Int16 tipoIdentificacion, string numeroIdentificacion)
        {
            if (_proveedor == null) _proveedor = new Proveedor(cnnSql);

            try
            {
                cnnSql.Open();

                return _proveedor.BuscarProveedor(tipoIdentificacion, numeroIdentificacion);
            }
            finally
            {
                if (cnnSql.State == ConnectionState.Open) cnnSql.Close();
            }
        }



        /// <summary>
        /// Método que almacena o actualiza un proveedor.
        /// </summary>
        /// <param name="proveedorDTO">ProveedorDTO</param>
        /// <returns>ProveedorDTO</returns>
        public ProveedorDTO AlmacenarProveedor(ProveedorDTO proveedorDTO)
        {
            double j;
            Int32 x = 0;
            if (_proveedor == null) _proveedor = new Proveedor(cnnSql);


            try
            {
                cnnSql.Open();

                if (Regex.IsMatch(proveedorDTO.NUMERO_IDENTIFICACION, @"\s"))
                    throw new ExcepcionesDIPCMI("No se permite espacios en blanco en el Número de Identificación");

                if (proveedorDTO.ID_TIPO_DOCUMENTO == (Int16)DocumentoIdentificacionEnum.NIT)
                {
                    if (!ValidaciónNIT.ValidarNit(proveedorDTO.NUMERO_IDENTIFICACION))
                        throw new ExcepcionesDIPCMI(string.Format("El NIT {0} no es valido", proveedorDTO.NUMERO_IDENTIFICACION));
                }

                //INI---------------SATB-12.06.2017--------------------------------
                //---Validación para documento RTN-Honduras---Documento de Identificación numerico 14 digitos
                if (proveedorDTO.ID_TIPO_DOCUMENTO == (Int16)DocumentoIdentificacionEnum.RTN)
                {
                    //double j;
                    bool rtn = double.TryParse(proveedorDTO.NUMERO_IDENTIFICACION, out j);
                    if (!rtn)
                        throw new ExcepcionesDIPCMI(string.Format("El RTN {0} no tiene el formato valido", proveedorDTO.NUMERO_IDENTIFICACION));
                    if ((proveedorDTO.NUMERO_IDENTIFICACION.Length > 14) || (proveedorDTO.NUMERO_IDENTIFICACION.Length < 14))
                        throw new ExcepcionesDIPCMI(string.Format("El RTN {0} tiene una cantidad de digitos diferente a los permitidos", proveedorDTO.NUMERO_IDENTIFICACION));
                }
                //FIN---------------SATB-12.06.2017--------------------------------

                //---Validación para documento RCN---Documento de Identificación numerico 10 digitos
                if (proveedorDTO.ID_TIPO_DOCUMENTO == (Int16)DocumentoIdentificacionEnum.NRC)
                {
                    //double j;
                    bool nrc = double.TryParse(proveedorDTO.NUMERO_IDENTIFICACION, out j);
                    if (!nrc)
                        throw new ExcepcionesDIPCMI(string.Format("El NRC {0} no tiene el formato valido", proveedorDTO.NUMERO_IDENTIFICACION));
                    if (proveedorDTO.NUMERO_IDENTIFICACION.Length > 11)
                        throw new ExcepcionesDIPCMI(string.Format("El NRC {0} tiene una cantidad de digitos diferente a los permitidos", proveedorDTO.NUMERO_IDENTIFICACION));
                }
                if (proveedorDTO.ID_TIPO_DOCUMENTO2 == (Int16)DocumentoIdentificacionEnum.NRC)
                {
                    //double j;
                    bool nrc2 = double.TryParse(proveedorDTO.NUMERO_IDENTIFICACION2, out j);
                    if (!nrc2)
                        throw new ExcepcionesDIPCMI(string.Format("El NRC {0} no tiene el formato valido", proveedorDTO.NUMERO_IDENTIFICACION2));
                    if (proveedorDTO.NUMERO_IDENTIFICACION2.Length > 11)
                        throw new ExcepcionesDIPCMI(string.Format("El NRC {0} tiene una cantidad de digitos diferente a los permitidos", proveedorDTO.NUMERO_IDENTIFICACION2));
                }

                //---Validación para los documentos de IPCR que cumplan con la cantidad de digitos permitidos. 
                bool numero = double.TryParse(proveedorDTO.NUMERO_IDENTIFICACION, out j);
                string tipo = _proveedor.TipoDocumento(proveedorDTO.ID_TIPO_DOCUMENTO);
                switch (tipo)
                {
                    case "01":
                        if (!numero)
                            throw new ExcepcionesDIPCMI(string.Format("El Documento {0} no tiene el formato valido", proveedorDTO.NUMERO_IDENTIFICACION));
                        if (proveedorDTO.NUMERO_IDENTIFICACION.Length != 9)
                            throw new ExcepcionesDIPCMI(string.Format("El Documento {0} tiene una cantidad de digitos diferente a los permitidos", proveedorDTO.NUMERO_IDENTIFICACION));
                        break;
                    case "02":
                        if (!numero)
                            throw new ExcepcionesDIPCMI(string.Format("El Documento {0} no tiene el formato valido", proveedorDTO.NUMERO_IDENTIFICACION));
                        if (proveedorDTO.NUMERO_IDENTIFICACION.Length != 10)
                            throw new ExcepcionesDIPCMI(string.Format("El Documento {0} tiene una cantidad de digitos diferente a los permitidos", proveedorDTO.NUMERO_IDENTIFICACION));
                        break;

                    case "03":
                        if (!numero)
                            throw new ExcepcionesDIPCMI(string.Format("El Documento {0} no tiene el formato valido", proveedorDTO.NUMERO_IDENTIFICACION));
                        if (!(proveedorDTO.NUMERO_IDENTIFICACION.Length == 11 || proveedorDTO.NUMERO_IDENTIFICACION.Length == 12))
                            throw new ExcepcionesDIPCMI(string.Format("El Documento {0} tiene una cantidad de digitos diferente a los permitidos", proveedorDTO.NUMERO_IDENTIFICACION));
                        break;

                    case "04":
                        if (!numero)
                            throw new ExcepcionesDIPCMI(string.Format("El Documento {0} no tiene el formato valido", proveedorDTO.NUMERO_IDENTIFICACION));
                        if (proveedorDTO.NUMERO_IDENTIFICACION.Length != 10)
                            throw new ExcepcionesDIPCMI(string.Format("El Documento {0} tiene una cantidad de digitos diferente a los permitidos", proveedorDTO.NUMERO_IDENTIFICACION));
                        break;
                }

                //---Validación para los documentos de IPCR que cumplan con la cantidad de digitos permitidos. 


                if (proveedorDTO.ID_PROVEEDOR == 0)
                {
                    if (!_proveedor.ExisteDocumento(proveedorDTO.ID_TIPO_DOCUMENTO, proveedorDTO.NUMERO_IDENTIFICACION))
                        throw new ExcepcionesDIPCMI(string.Format("El Documento del proveedor {0} ya esta registrado.", proveedorDTO.NOMBRE));
                    if (proveedorDTO.NUMERO_IDENTIFICACION2 != "")
                    {
                        if (!_proveedor.ExisteDocumento(proveedorDTO.ID_TIPO_DOCUMENTO2, proveedorDTO.NUMERO_IDENTIFICACION2))
                            throw new ExcepcionesDIPCMI(string.Format("El Documento del proveedor {0} ya esta registrado.", proveedorDTO.NOMBRE));
                    }

                    proveedorDTO.ID_PROVEEDOR = _proveedor.EjecutarSenteciaInsert(proveedorDTO);
                }
                else
                    x = _proveedor.EjecutarSentenciaUpdate(proveedorDTO);
            }
            finally
            {
                if (cnnSql.State == ConnectionState.Open) cnnSql.Close();
            }

            return proveedorDTO;
        }

        /// <summary>
        /// Función que lista todos los proveedores registrados.
        /// </summary>
        /// <returns>Lista ProveedorDTO</returns>
        public List<ProveedorDTO> ListaProveedores()
        {
            if (_proveedor == null) _proveedor = new Proveedor(cnnSql);
            List<ProveedorDTO> _proveedorDto = new List<ProveedorDTO>();

            try
            {
                cnnSql.Open();

                return _proveedor.ListaProveedores();
            }
            finally
            {
                if (cnnSql.State != ConnectionState.Closed) cnnSql.Close();
            }
        }

        public List<ProveedorDTO> ListaProveedoresBusqueda(Int16 tipoDoc, string identificacion, string nombre)
        {
            if (_proveedor == null) _proveedor = new Proveedor(cnnSql);
            List<ProveedorDTO> _proveedorDto = new List<ProveedorDTO>();

            try
            {
                cnnSql.Open();

                return _proveedor.BusquedaProveedores(tipoDoc, identificacion, nombre);
            }
            finally
            {
                if (cnnSql.State != ConnectionState.Closed) cnnSql.Close();
            }
        }

        public List<ProveedorDTO> ListaProveedoresDDL()
        {
            if (_proveedor == null) _proveedor = new Proveedor(cnnSql);
            List<ProveedorDTO> _proveedorDto = new List<ProveedorDTO>();

            try
            {
                cnnSql.Open();

                return _proveedor.ListaProveedoresDDL();
            }
            finally
            {
                if (cnnSql.State != ConnectionState.Closed) cnnSql.Close();
            }
        }

        /// <summary>
        /// Método que da baja al proveedor.
        /// </summary>
        /// <param name="idProveedor">Número de identificación</param>
        /// <param name="usuario">Usuario da baja</param>
        /// <returns>Registro actualizado</returns>
        public Int16 DarBajaProveedor(Int32 idProveedor, string usuario)
        {
            if (_proveedor == null) _proveedor = new Proveedor(cnnSql);

            try
            {
                cnnSql.Open();

                return _proveedor.DarBajaProveedor(idProveedor, usuario);
            }
            finally
            {
                if (cnnSql.State != ConnectionState.Closed) cnnSql.Close();
            }
        }

        /// <summary>
        /// Método que da alta al proveedor.
        /// </summary>
        /// <param name="idProveedor">Número de identicicación</param>
        /// <param name="usuario">Usuario que da alta</param>
        /// <returns>Registro actualizado</returns>
        public Int16 DarAltaProveedor(Int32 idProveedor, string usuario)
        {
            if (_proveedor == null) _proveedor = new Proveedor(cnnSql);

            try
            {
                cnnSql.Open();

                return _proveedor.DarAltaProveedor(idProveedor, usuario);
            }
            finally
            {
                if (cnnSql.State != ConnectionState.Closed) cnnSql.Close();
            }
        }

        #endregion

        #region Tipo Documento

        /// <summary>
        /// Función que retorna los tipos de documentos de identificación de los proveedores activos.
        /// </summary>
        /// <returns>Lista tipo documento</returns>
        public List<TipoDocumentoDTO> ListaTipoDocumentoActivo(int IdCodigoSociedad)
        {
            if (_tipoDocumento == null) _tipoDocumento = new TipoDocumento(cnnSql);

            try
            {
                cnnSql.Open();

                return _tipoDocumento.ListaTipoDocumentoActivo(IdCodigoSociedad);
            }
            finally
            {
                if (cnnSql.State != ConnectionState.Closed) cnnSql.Close();
            }

        }

        public List<TipoDocumentoDTO> ListaTipoDocumentoPais(string dominio)
        {
            if (_tipoDocumento == null) _tipoDocumento = new TipoDocumento(cnnSql);

            try
            {
                cnnSql.Open();

                return _tipoDocumento.ListaTipoDocumentoPais(dominio);
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
