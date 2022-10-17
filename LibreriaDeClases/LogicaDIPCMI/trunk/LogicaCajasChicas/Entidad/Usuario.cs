using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using DipCmiGT.LogicaComun;

namespace DipCmiGT.LogicaCajasChicas.Entidad
{
    public class Usuario
    {
        #region Declaraciones

        private SqlConnection _sqlConn = null;
        private SqlTransaction _sqlTran = null;


        protected string sqlSelect = @" SELECT a.IdUsuario, a.Usuario, a.Nombre, a.Alta, a.UsuarioAlta, a.FechaAlta, a.UsuarioModificacion, 
                                        a.FechaModificacion, a.correo, a.dominio, b.Nombre 
                                        FROM Usuario as a
                                        LEFT JOIN Dominio as b
                                        on b.Identificador = a.dominio";

        protected string sqlInsert = @" INSERT INTO Usuario (Usuario, Nombre, UsuarioAlta, correo, dominio)
			                                         VALUES (@Usuario, @Nombre, @UsuarioAlta, @correo, @dominio)
                                        Select @@identity";

        protected string sqlUpdate = @"Update Usuario set Usuario = @Usuario, Nombre = @Nombre, Alta = @Alta, UsuarioModificacion = @UsuarioModificacion, 
                                        FechaModificacion = getdate(), correo = @correo, dominio = @dominio
                                        where IdUsuario = @IdUsuario";

        protected string sqlSelectDominio = @"Select * from Dominio";

        //String para actualizar CentroCosto asociados al usuario
        protected string sqlUpdateCentroCosto = @"Update UsuarioCentroCosto set Usuario = @Usuario WHERE Usuario = @UsuarioAnterior";
        //String para actualizar Sociedad Centro asociado al usuario
        protected string sqlUsuarioSociedadCentro = @"Update UsuarioSociedadCentro set Usuario = @Usuario WHERE Usuario = @UsuarioAnterior";
        //String para actualizar Ordenes de Compras asociadas al usuario
        protected string sqlUsuarioOrdenCompra = @"Update UsuarioOrdenCompra Set Usuario = @Usuario WHERE Usuario = @UsuarioAnterior";
        #endregion

        #region Constructores


        public Usuario(SqlConnection sqlConn)
        {
            _sqlConn = sqlConn;
        }

        public Usuario(SqlConnection sqlConn, SqlTransaction sqlTran)
        {
            _sqlConn = sqlConn;
            _sqlTran = sqlTran;
        }

        #endregion

        #region Metodos

        /// <summary>
        /// Consulta registro usuario.
        /// </summary>
        /// <param name="idProveedor">Identificador único</param>
        /// <returns>RolDTO</returns>
        public UsuarioDTO EjecutarSentenciaSelect(Int32 idUsuario)
        {
            List<UsuarioDTO> listaRol = null;
            SqlCommand sqlComando;
            string sql = sqlSelect + " Where IdRol = @IdRol ";

            sqlComando = new SqlCommand(sql, _sqlConn);
            sqlComando.CommandType = CommandType.Text;

            if (_sqlTran != null)
                sqlComando.Transaction = _sqlTran;

            sqlComando.Parameters.Add(new SqlParameter("@IdUsuario", (object)idUsuario ?? DBNull.Value));

            listaRol = CargarReader(sqlComando.ExecuteReader());

            return listaRol.Count > 0 ? listaRol[0] : null;
        }

        /// <summary>
        /// Metodo que busca a un usuario filtrando por nombre de usuario
        /// </summary>
        /// <param name="usuario">nombre de usuario por el cual se buscara</param>
        /// <returns>usuario encontrado</returns>
        public UsuarioDTO SeleccionarUsuario(short usuario)
        {
            List<UsuarioDTO> usuarioObtenido = null;
            SqlCommand sqlComando;
            string sql = sqlSelect + " WHERE a.IdUsuario = @IdUsuario";

            sqlComando = new SqlCommand(sql, _sqlConn);
            sqlComando.CommandType = CommandType.Text;

            if (_sqlTran != null)
                sqlComando.Transaction = _sqlTran;

            sqlComando.Parameters.Add(new SqlParameter("@IdUsuario", (object)usuario ?? DBNull.Value));

            usuarioObtenido = CargarReader(sqlComando.ExecuteReader());

            return usuarioObtenido.Count > 0 ? usuarioObtenido[0] : null;
        }

        /// <summary>
        /// Inserta registro de usuario.
        /// </summary>
        /// <param name="UsuarioDTO">UsuarioDTO</param>
        /// <returns>IdRol</returns>
        public Int16 EjecutarSenteciaInsert(UsuarioDTO UsuarioDTO)
        {
            SqlCommand sqlComando;

            sqlComando = new SqlCommand(sqlInsert, _sqlConn);
            sqlComando.CommandType = CommandType.Text;

            if (_sqlTran != null)
                sqlComando.Transaction = _sqlTran;

            sqlComando.Parameters.Add(new SqlParameter("@Usuario", (object)UsuarioDTO.USUARIO.Trim() ?? DBNull.Value));
            sqlComando.Parameters.Add(new SqlParameter("@Nombre", (object)UsuarioDTO.NOMBRE.ToUpper() ?? DBNull.Value));
            sqlComando.Parameters.Add(new SqlParameter("@UsuarioAlta", (object)UsuarioDTO.USUARIO_MANTENIMIENTO.USUARIO_ALTA.ToUpper() ?? DBNull.Value));
            sqlComando.Parameters.Add(new SqlParameter("@correo", (object)UsuarioDTO.CORREO ?? DBNull.Value));
            sqlComando.Parameters.Add(new SqlParameter("@Dominio", (object)UsuarioDTO.IDENTIFICADOR ?? DBNull.Value));

            return Convert.ToInt16(sqlComando.ExecuteScalar());
        }


        public Int32 EjecutarSentenciaUpdate(UsuarioDTO usuarioDTO)
        {
            SqlCommand sqlComando;
            SqlCommand sqlCommandUpdate;
            SqlCommand sqlCommandUpdateSC;
            SqlCommand sqlCommandUpdateOC;
            SqlCommand delete;

            delete = new SqlCommand("DELETE FROM MapeoUsuarioCECOCuenta WHERE Usuario = 'nruiz'", _sqlConn);
            delete.CommandType = CommandType.Text;

            if (_sqlTran != null)
                delete.Transaction = _sqlTran;

            var res = delete.ExecuteNonQuery();


            var usuario = SeleccionarUsuario(usuarioDTO.ID_USUARIO);//Se busca al usuario al cual se esta modificando

            sqlCommandUpdateSC = new SqlCommand(@"SPUpdateUsuario", _sqlConn);
            sqlCommandUpdateSC.CommandType = CommandType.StoredProcedure;

            if (_sqlTran != null)
                sqlCommandUpdateSC.Transaction = _sqlTran;

            sqlCommandUpdateSC.Parameters.Add(new SqlParameter("@UsuarioNuevo", (object)@usuarioDTO.USUARIO ?? DBNull.Value));
            sqlCommandUpdateSC.Parameters.Add(new SqlParameter("@UsuarioAnterior", (object)@usuario.USUARIO ?? DBNull.Value));

            var reader = sqlCommandUpdateSC.ExecuteNonQuery();
            if (true)
            {
                sqlComando = new SqlCommand(sqlUpdate, _sqlConn);
                sqlComando.CommandType = CommandType.Text;

                if (_sqlTran != null)
                    sqlComando.Transaction = _sqlTran;

                sqlComando.Parameters.Add(new SqlParameter("@IdUsuario", (object)usuarioDTO.ID_USUARIO ?? DBNull.Value));
                sqlComando.Parameters.Add(new SqlParameter("@Usuario", (object)usuarioDTO.USUARIO ?? DBNull.Value));
                sqlComando.Parameters.Add(new SqlParameter("@Nombre", (object)usuarioDTO.NOMBRE.ToUpper() ?? DBNull.Value));
                sqlComando.Parameters.Add(new SqlParameter("@UsuarioModificacion", (object)usuarioDTO.USUARIO_MANTENIMIENTO.USUARIO_MODIFICO.ToUpper() ?? DBNull.Value));
                sqlComando.Parameters.Add(new SqlParameter("@Alta", (object)usuarioDTO.ALTA ?? DBNull.Value));
                sqlComando.Parameters.Add(new SqlParameter("@correo", (object)usuarioDTO.CORREO ?? DBNull.Value));
                sqlComando.Parameters.Add(new SqlParameter("@dominio", (object)usuarioDTO.IDENTIFICADOR ?? DBNull.Value));

                return Convert.ToInt32(sqlComando.ExecuteNonQuery());
            }
            else
            {
                return 1;
            }
        }
        /// <summary>
        /// Encapsula objeto usuario.
        /// </summary>
        /// <param name="sqlReader">SqlReader</param>
        /// <returns>Lista UsuarioDTO</returns>
        protected List<UsuarioDTO> CargarReader(SqlDataReader sqlReader)
        {
            UsuarioDTO _usuarioDTO = null;
            List<UsuarioDTO> _listaUsuario = new List<UsuarioDTO>();

            while (sqlReader.Read())
            {
                _usuarioDTO = new UsuarioDTO();

                _usuarioDTO.ID_USUARIO = Convert.ToInt16(sqlReader.GetInt32(0));
                _usuarioDTO.USUARIO = sqlReader.GetString(1);
                _usuarioDTO.NOMBRE = sqlReader.GetString(2);
                _usuarioDTO.ALTA = sqlReader.GetBoolean(3);
                _usuarioDTO.USUARIO_MANTENIMIENTO.USUARIO_ALTA = sqlReader.GetString(4);
                _usuarioDTO.USUARIO_MANTENIMIENTO.FECHA_ALTA = sqlReader.GetDateTime(5);
                _usuarioDTO.USUARIO_MANTENIMIENTO.USUARIO_MODIFICO = sqlReader.IsDBNull(6) ? null : sqlReader.GetString(6);
                _usuarioDTO.USUARIO_MANTENIMIENTO.FECHA_MODIFICACION = sqlReader.IsDBNull(7) ? (DateTime?)null : sqlReader.GetDateTime(7);
                _usuarioDTO.CORREO = sqlReader.IsDBNull(8) ? null : sqlReader.GetString(8);
                _usuarioDTO.IDENTIFICADOR = sqlReader.IsDBNull(9) ? null : sqlReader.GetString(9);
                _usuarioDTO.DOMINIO = sqlReader.IsDBNull(10) ? null : sqlReader.GetString(10);
                _listaUsuario.Add(_usuarioDTO);
            }
            sqlReader.Close();

            return _listaUsuario;
        }

        /// <summary>
        /// Lista todos los proveedores
        /// </summary>
        /// <returns>Lista ProveedorDTO</returns>
        public List<UsuarioDTO> ListaUsuarios()
        {
            SqlCommand sqlComando = null;
            string sql = sqlSelect;

            sqlComando = new SqlCommand(sql, _sqlConn);
            sqlComando.CommandType = CommandType.Text;

            if (_sqlTran != null)
                sqlComando.Transaction = _sqlTran;

            return CargarReader(sqlComando.ExecuteReader());
        }



        //INI--------------------SATB-09.06.2017-------------------------
        //---Cargar DDL de Dominio para asginarselo a Usuario 

        public List<LlenarDDL_DTO> ListaDominio()
        {
            SqlCommand _sqlComando = null;
            string sql = sqlSelectDominio;

            _sqlComando = new SqlCommand(sql, _sqlConn);
            _sqlComando.CommandType = CommandType.Text;

            if (_sqlTran != null)
                _sqlComando.Transaction = _sqlTran;

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

        //FIN--------------------SATB-09.06.2017-------------------------

        public List<UsuarioDTO> BuscarUsuario(string usuario, string Nombre, string dominio)
        {
            SqlCommand sqlComando = null;
//            string sql = sqlSelect + @" where (Usuario = case when @Usuario = '' then Usuario else @Usuario end)
//                                        and ((Nombre = case when @Nombre = '' then Nombre else @Nombre end) or 
//                                        Nombre like '%' + @Nombre + '%')";

            //INI -------------SATB-16.06.2017--------------------------------
            string sql = sqlSelect + @" WHERE (Usuario like case when @Usuario = '' then Usuario else '%' + @Usuario + '%' end)
                                        and (a.Nombre like case when @Nombre = '' then a.Nombre else '%' + @Nombre  + '%' end) 
                                        and (b.Identificador = case when @dominio = '0' then b.Identificador else @dominio end)";
            //FIN -------------SATB-16.06.2017--------------------------------

            sqlComando = new SqlCommand(sql, _sqlConn);
            if (_sqlTran != null)
                sqlComando.Transaction = _sqlTran;

            sqlComando.Parameters.Add(new SqlParameter("@Usuario", (object)usuario ?? DBNull.Value));
            sqlComando.Parameters.Add(new SqlParameter("@Nombre", (object)Nombre ?? DBNull.Value));
            sqlComando.Parameters.Add(new SqlParameter("@dominio", (object)dominio ?? DBNull.Value));

            return CargarReader(sqlComando.ExecuteReader());
        }

        /// <summary>
        /// Método que da baja al proveedor.
        /// </summary>
        /// <param name="idProveedor">Número de identificación</param>
        /// <param name="usuario">Usuario da baja</param>
        /// <returns>Registro actualizado</returns>
        public Int16 DarBajaUsuario(Int32 idUsuario, string usuario)
        {
            SqlCommand sqlComando;
            string sql = @"Update Usuario set Alta = 0, UsuarioModificacion = @UsuarioModificacion, FechaModificacion = getdate()
                            where IdUsuario = @IdUsuario";

            sqlComando = new SqlCommand(sql, _sqlConn);
            sqlComando.CommandType = CommandType.Text;

            if (_sqlTran != null)
                sqlComando.Transaction = _sqlTran;

            sqlComando.Parameters.Add(new SqlParameter("@UsuarioModificacion", (object)usuario.ToUpper() ?? DBNull.Value));
            sqlComando.Parameters.Add(new SqlParameter("@IdUsuario", (object)idUsuario ?? DBNull.Value));

            return Convert.ToInt16(sqlComando.ExecuteNonQuery());
        }

        /// <summary>
        /// Método que da alta al proveedor.
        /// </summary>
        /// <param name="idProveedor">Número de identicicación</param>
        /// <param name="usuario">Usuario que da alta</param>
        /// <returns>Registro actualizado</returns>
        public Int16 DarAltaUsuario(Int32 idUsuario, string usuario)
        {
            SqlCommand sqlComando;
            string sql = @"Update Usuario set Alta = 1, UsuarioModificacion = @UsuarioModificacion, FechaModificacion = getdate()
                            where IdUsuario = @IdUsuario";

            sqlComando = new SqlCommand(sql, _sqlConn);
            sqlComando.CommandType = CommandType.Text;

            if (_sqlTran != null)
                sqlComando.Transaction = _sqlTran;

            sqlComando.Parameters.Add(new SqlParameter("@UsuarioModificacion", (object)usuario.ToUpper() ?? DBNull.Value));
            sqlComando.Parameters.Add(new SqlParameter("@IdUsuario", (object)idUsuario ?? DBNull.Value));

            return Convert.ToInt16(sqlComando.ExecuteNonQuery());
        }

        public UsuarioDTO BuscarUsuarioValidar(string usuario, string dominio)
        {
            SqlCommand sqlComando = null;
            List<UsuarioDTO> listaUsuarioDto = null;
            string sql = sqlSelect + @" WHERE usuario = @Usuario
                                            and Alta = 1
                                            and dominio = @dominio";

            sqlComando = new SqlCommand(sql, _sqlConn);
            sqlComando.CommandType = CommandType.Text;

            if (_sqlTran != null)
                sqlComando.Transaction = _sqlTran;

            sqlComando.Parameters.Add(new SqlParameter("@Usuario", (object)usuario));
            sqlComando.Parameters.Add(new SqlParameter("@dominio", (object)dominio));

            listaUsuarioDto = CargarReader(sqlComando.ExecuteReader());

            return listaUsuarioDto.Count > 0 ? listaUsuarioDto[0] : null;
        }

        public bool ExisteUsuario(string Usuario, string dominio)
        {
            SqlCommand sqlComando;
            string sql = @"select COUNT(IdUsuario) from Usuario
                            where Usuario = @Usuario
                                  and dominio = @Dominio ";

            sqlComando = new SqlCommand(sql, _sqlConn);

            if (_sqlTran != null)
                sqlComando.Transaction = _sqlTran;

            sqlComando.Parameters.Add(new SqlParameter("@Usuario", (object)Usuario ?? DBNull.Value));
            sqlComando.Parameters.Add(new SqlParameter("@Dominio", (object)dominio ?? DBNull.Value));

            return Convert.ToInt32(sqlComando.ExecuteScalar()) > 0 ? false : true;
        }


        //INI-----------------------SATB-26.10.2017-------------------------------------------

        public List<DominioDTO> SeleccionarDominio()
        {
            SqlCommand sqlComando;

            sqlComando = new SqlCommand(sqlSelectDominio, _sqlConn);
            sqlComando.CommandType = CommandType.Text;

            if (_sqlTran != null)
                sqlComando.Transaction = _sqlTran;

            return CargarReaderDominio(sqlComando.ExecuteReader());
        }

        protected List<DominioDTO> CargarReaderDominio(SqlDataReader sqlReader)
        {
            DominioDTO _dominioDTO = null;
            List<DominioDTO> _listaDominio = new List<DominioDTO>();

            while (sqlReader.Read())
            {
                _dominioDTO = new DominioDTO();

                _dominioDTO.IDENTIFICADOR = sqlReader.GetString(0);
                _dominioDTO.NOMBRE = sqlReader.GetString(1);

                _listaDominio.Add(_dominioDTO);
            }
            sqlReader.Close();

            return _listaDominio;
        }

        public List<UsuarioDTO> BusquedadUsuario(string usuario, string nombre, string dominio)
        {
            SqlCommand sqlComando = null;
            string sql = @"SELECT Usuario, Nombre FROM Usuario
                           WHERE dominio = @dominio
                           AND Usuario like '%' + @usuario + '%'
                           AND Nombre  like '%' + @nombre + '%'
                           AND Alta = 1";

            sqlComando = new SqlCommand(sql, _sqlConn);
            sqlComando.CommandType = CommandType.Text;

            if (_sqlTran != null)
                sqlComando.Transaction = _sqlTran;

            sqlComando.Parameters.Add(new SqlParameter("@Usuario", (object)usuario ?? DBNull.Value));
            sqlComando.Parameters.Add(new SqlParameter("@dominio", (object)dominio ?? DBNull.Value));
            sqlComando.Parameters.Add(new SqlParameter("@nombre", (object)nombre ?? DBNull.Value));

            return CargarReaderBusquedaUsuario(sqlComando.ExecuteReader());
        }

        protected List<UsuarioDTO> CargarReaderBusquedaUsuario(SqlDataReader sqlReader)
        {
            UsuarioDTO _usuarioDTO = null;
            List<UsuarioDTO> _listaUsuario = new List<UsuarioDTO>();

            while (sqlReader.Read())
            {
                _usuarioDTO = new UsuarioDTO();

                _usuarioDTO.USUARIO = sqlReader.GetString(0);
                _usuarioDTO.NOMBRE = sqlReader.GetString(1);
                _listaUsuario.Add(_usuarioDTO);
            }
            sqlReader.Close();

            return _listaUsuario;
        }

        //FIN-----------------------SATB-26.10.2017-------------------------------------------




        //INI------Registro de Bitacora-----------------SATB-09.01.2018----------------------

        public Int16 RegistrarBitacora(BitacoraDTO BitacoraDTO)
        {
            SqlCommand sqlComando;
            string sqlBitacora = @"INSERT INTO Bitacora_Accesos (IdDominio, IP, FechaIngreso, HoraIngreso, Usuario)
                                   VALUES (@IdDominio, @IP, @FechaIngreso, @HoraIngreso, 
											(SELECT IdUsuario from usuario
											 WHERE  usuario = @Usuario
											 AND    dominio = @IdDominio))";
            sqlComando = new SqlCommand(sqlBitacora, _sqlConn);
            sqlComando.CommandType = CommandType.Text;

            if (_sqlTran != null)
                sqlComando.Transaction = _sqlTran;

            sqlComando.Parameters.Add(new SqlParameter("@Usuario", (object)BitacoraDTO.USUARIO ?? DBNull.Value));
            sqlComando.Parameters.Add(new SqlParameter("@IdDominio", (object)BitacoraDTO.ID_DOMINIO ?? DBNull.Value));
            sqlComando.Parameters.Add(new SqlParameter("@IP", (object)BitacoraDTO.IP ?? DBNull.Value));
            sqlComando.Parameters.Add(new SqlParameter("@FechaIngreso", (object)BitacoraDTO.FECHA_INGRESO ?? DBNull.Value));
            sqlComando.Parameters.Add(new SqlParameter("@HoraIngreso", (object)BitacoraDTO.HORA_INGRESO?? DBNull.Value));

            return Convert.ToInt16(sqlComando.ExecuteScalar());
        }


        //FIN------Registro de Bitacora-----------------SATB-09.01.2018----------------------
        #endregion
    }
}
