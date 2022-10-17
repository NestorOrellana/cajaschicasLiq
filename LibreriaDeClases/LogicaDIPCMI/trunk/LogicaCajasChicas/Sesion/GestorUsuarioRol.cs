using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using LogicaComun.Enum;
using DipCmiGT.LogicaCajasChicas.Entidad;
using DipCmiGT.LogicaComun;
using DipCmiGT.LogicaComun.Util;

namespace DipCmiGT.LogicaCajasChicas.Sesion
{
   public class GestorUsuarioRol : IDisposable
    {

        #region Declaraciones

       UsuarioRol _usuarioRol = null;

       Rol _rol = null;

       SqlConnection cnnSql = null;

        #endregion

        #region Constructor

       public GestorUsuarioRol(string conexion)
       {
           cnnSql = new SqlConnection(conexion);
       }

        #endregion

        #region UsuarioRol

       public UsuarioRolDTO EjecutarSentenciaSelect(Int16 IdUsuario, Int16 IdRol)
       {
           if (_usuarioRol == null) _usuarioRol = new UsuarioRol(cnnSql);
           try
           {
               cnnSql.Open();
               return _usuarioRol.EjecutarSentenciaSelect(IdUsuario, IdRol);
           }finally
           {
               if (cnnSql.State == ConnectionState.Open) cnnSql.Close();
           }
       }

       public UsuarioRolDTO AlmacenarUsuarioRol(UsuarioRolDTO usuarioRolDTO)
       {
           Int32 x = 0;
           if (_usuarioRol == null) _usuarioRol = new UsuarioRol(cnnSql);

           try
           {
               cnnSql.Open();
               if(_usuarioRol.ExisteRolAsignado(usuarioRolDTO.ID_USUARIO, usuarioRolDTO.ID_ROL))
               {
                   x = _usuarioRol.EjecutarSentenciaInsert(usuarioRolDTO);
               }else
                   x = _usuarioRol.EjecutarSenteenciaUpdate(usuarioRolDTO);

           }
           finally
           {
               if (cnnSql.State == ConnectionState.Open) cnnSql.Close();
           }

           return usuarioRolDTO;
       }

       
       public List<RolDTO> ListaRol()
       {
           if (_rol == null) _rol = new Rol(cnnSql);
           List<RolDTO> _rolDto = new List<RolDTO>();
           try
           {
               cnnSql.Open();
               return _rol.ListaRoles();
           }
           finally
           {
               if (cnnSql.State != ConnectionState.Closed) cnnSql.Close();
           }
       }

       public List<UsuarioRolDTO> ListaUsuarioRol(Int32 IdUsuario)
       {
           if (_usuarioRol == null) _usuarioRol = new UsuarioRol(cnnSql);
           List<UsuarioRolDTO> _usuarioRolDto = new List<UsuarioRolDTO>();

           try
           {
               cnnSql.Open();
               return _usuarioRol.ListaUsuarioRol(IdUsuario);
           }
           finally
           {
               if (cnnSql.State != ConnectionState.Closed) cnnSql.Close();
           }
       }

       public Int16 DarBajaUsuarioRol(Int16 IdUsuario, Int16 IdRol, string usuario)
       {
           if (_usuarioRol == null) _usuarioRol = new UsuarioRol(cnnSql);
           try
           {
               cnnSql.Open();
               return _usuarioRol.DarBajaUsuarioRol(IdUsuario, IdRol, usuario);
           }
           finally
           {
               if (cnnSql.State != ConnectionState.Closed) cnnSql.Close();
           }
       }

       public Int16 DarAltaUsuarioRol(Int16 IdUsuario, Int16 IdRol, string usuario)
       {
           if (_usuarioRol == null) _usuarioRol = new UsuarioRol(cnnSql);
           try
           {
               cnnSql.Open();
               return _usuarioRol.DarAltaUsuarioRol(IdUsuario, IdRol, usuario);
           }
           finally
           {
               if (cnnSql.State != ConnectionState.Closed) cnnSql.Close();
           }
       }

        #endregion<asp:BoundField DataField="USUARIO" HeaderText="USUARIO " /> 

        #region IDisposble Members
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
