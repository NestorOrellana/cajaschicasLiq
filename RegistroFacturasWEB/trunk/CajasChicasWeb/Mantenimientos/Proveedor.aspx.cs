using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using LogicaComun.Enum;
using DipCmiGT.LogicaCajasChicas;
using System.Configuration;
using DipCmiGT.LogicaCajasChicas.Sesion;
using DipCmiGT.LogicaComun;
using LogicaCajasChicas;
using System.Text.RegularExpressions;


namespace RegistroFacturasWEB.Mantenimientos
{
    public partial class Proveedor : System.Web.UI.Page
    {
        #region Declaraciones

        string cnnApl = ConfigurationManager.ConnectionStrings["CnnApl"].ToString();
        string usuario;
        bool result;
        string dominio;

        #endregion

        #region Eventos

        protected void Page_Load(object sender, EventArgs e)
        {
            dominio = (string)Session["dominio"].ToString();
            usuario = Context.User.Identity.Name.ToString();
            if (!IsPostBack)
            {
                CargarDDLPais();
                CargarDDLRegimen();
             //   CargarDDLTipoDocumento(dominio);
                CargarProveedoresGrid();
            }

        }

        protected void btnLimpiar_Click(object sender, EventArgs e)
        {
            LimpiarControles();
        }

        protected void btnGrabar_Click(object sender, EventArgs e)
        {

            AlmacenarProveedor();
        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            CargarProveedoresGridBusqueda(Convert.ToInt16(ddlTipoDocumento.SelectedValue), txtNumeroIdentificacion.Text, txtNombre.Text);
        }

        protected void gvProveedor_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            this.gvProveedor.PageIndex = e.NewPageIndex;
            CargarProveedoresGrid();
        }

        protected void gvProveedor_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName.Equals("Editar"))
            {
                OcultarAvisos();
                CargarProveedorEdicion(e);
            }

            if (e.CommandName.Equals("Baja"))
            {
                OcultarAvisos();
                DarBajaProveedor(e);
            }

            if (e.CommandName.Equals("Alta"))
            {
                OcultarAvisos();
                DarAltaProveedor(e);
            }
        }

        #endregion

        #region Metodos

        private void DesplegarAviso(string mensaje)
        {
            divMensaje.Attributes.CssStyle.Add("display", "table");
            divMensaje.InnerHtml = mensaje;
        }

        private void DesplegarError(string mensaje)
        {
            divMensajeError.Attributes.CssStyle.Add("display", "table");
            divMensajeError.InnerHtml = mensaje;
        }

        private void OcultarAvisos()
        {
            divMensaje.Attributes.CssStyle.Add("display", "none");
            divMensajeError.Attributes.CssStyle.Add("display", "none");
        }

        private void CargarDDLRegimen()
        {

            try
            {
            List<LlenarDDL_DTO> listaDDLDto = new List<LlenarDDL_DTO>();

            listaDDLDto.Add(new LlenarDDL_DTO() { IDENTIFICADOR = "-1", DESCRIPCION = "::Seleccione Régimen::" });
            listaDDLDto.Add(new LlenarDDL_DTO() { IDENTIFICADOR = "No Aplica", DESCRIPCION = "No Aplica" });
            listaDDLDto.Add(new LlenarDDL_DTO() { IDENTIFICADOR = "Regimen Simplificado", DESCRIPCION = "Régimen Simplificado" });
            listaDDLDto.Add(new LlenarDDL_DTO() { IDENTIFICADOR = "Pequeño Contribuyente", DESCRIPCION = "Pequeño Contribuyente" });
            listaDDLDto.Add(new LlenarDDL_DTO() { IDENTIFICADOR = "Mediano Contribuyente", DESCRIPCION = "Mediano Contribuyente" });
            listaDDLDto.Add(new LlenarDDL_DTO() { IDENTIFICADOR = "Grande Contribuyente", DESCRIPCION = "Grande Contribuyente" });

               
            ddlRegimen.DataSource = listaDDLDto;
            ddlRegimen.DataTextField = "DESCRIPCION";
            ddlRegimen.DataValueField = "IDENTIFICADOR";
            ddlRegimen.DataBind();
            }
            catch
            {
                DesplegarError("Error al cargar Regimen de Proveedor");
            }

        }

        private void CargarDDLTipoDocumento(string dominio)
        {
            GestorProveedor gestorProveedor = null;
            try
            {
                gestorProveedor = GestorProveedor();

                List<TipoDocumentoDTO> _listaTipoDocumentoDto = gestorProveedor.ListaTipoDocumentoPais(dominio);

                ddlTipoDocumento.Items.Clear();
                ddlTipoDocumento.Items.Add(new ListItem("::ELIJA UNA OPCION::", "-1"));
                ddlTipoDocumento.AppendDataBoundItems = true;
                ddlTipoDocumento.DataSource = _listaTipoDocumentoDto;
                ddlTipoDocumento.DataTextField = "Descripcion";
                ddlTipoDocumento.DataValueField = "Id_Tipo_Documento";
                ddlTipoDocumento.DataBind();

                ddlTipoDocumento2.Items.Clear();
                ddlTipoDocumento2.Items.Add(new ListItem("::ELIJA UNA OPCION::", "-1"));
                ddlTipoDocumento2.AppendDataBoundItems = true;
                ddlTipoDocumento2.DataSource = _listaTipoDocumentoDto;
                ddlTipoDocumento2.DataTextField = "Descripcion";
                ddlTipoDocumento2.DataValueField = "Id_Tipo_Documento";
                ddlTipoDocumento2.DataBind();

            }
            catch
            {
                DesplegarError("Error al recuperar tipos de documentos");
            }
            finally
            {
                if (gestorProveedor != null) gestorProveedor.Dispose();
            }


        }

        private void CargarDDLPais()
        {

            try
            {
                List<LlenarDDL_DTO> listaDDLDto = new List<LlenarDDL_DTO>();

                listaDDLDto.Add(new LlenarDDL_DTO() { IDENTIFICADOR = "-1", DESCRIPCION = "::SELECCIONE PAIS::" });
                listaDDLDto.Add(new LlenarDDL_DTO() { IDENTIFICADOR = "GTDIPCMI", DESCRIPCION = "Guatemala" });
                listaDDLDto.Add(new LlenarDDL_DTO() { IDENTIFICADOR = "SVDIPCMI", DESCRIPCION = "El Salvador" });
                listaDDLDto.Add(new LlenarDDL_DTO() { IDENTIFICADOR = "HNDPCMI", DESCRIPCION = "Honduras" });
                listaDDLDto.Add(new LlenarDDL_DTO() { IDENTIFICADOR = "CRDIPCMI", DESCRIPCION = "Costa Rica" });

                ddlPais.DataSource = listaDDLDto;
                ddlPais.DataTextField = "DESCRIPCION";
                ddlPais.DataValueField = "IDENTIFICADOR";
                ddlPais.DataBind();
            }
            catch
            {
                DesplegarError("Error al cargar Paises");
            }

        }


        private void LimpiarControles()
        {
            hfIdProveedor.Value = "0";
            ddlTipoDocumento.DataValueField = "-1";
            ddlPais.DataValueField = "-1";
            txtNumeroIdentificacion.Text = string.Empty;
            txtNombre.Text = string.Empty;
            txtDireccion.Text = string.Empty;
            chTipo.Checked = false;
            cbAlta.Checked = false;
            lblFechaModificacionDB.Text = string.Empty;
            lblUsuarioDB.Text = string.Empty;
            OcultarAvisos();
        }

        private ProveedorDTO CargarObjetoProveedor(ref ProveedorDTO _proveedorDto)
        {
            string nit = Regex.Replace(txtNumeroIdentificacion.Text.Trim(), "-", "");
            string nit2 = Regex.Replace(txtNumeroIdentificacion2.Text.Trim(), "-", "");
            txtNumeroIdentificacion.Text = nit;
            txtNumeroIdentificacion2.Text = nit2;


            _proveedorDto.ID_PROVEEDOR = Convert.ToInt32(hfIdProveedor.Value);
            _proveedorDto.ID_TIPO_DOCUMENTO = Convert.ToInt16(ddlTipoDocumento.SelectedValue);
            _proveedorDto.NUMERO_IDENTIFICACION = txtNumeroIdentificacion.Text.Trim();
            _proveedorDto.NOMBRE = txtNombre.Text;
            _proveedorDto.DIRECCION = txtDireccion.Text;
            _proveedorDto.REGIMEN = ddlRegimen.SelectedValue.ToString();
            _proveedorDto.ALTA = _proveedorDto.ID_PROVEEDOR.Equals(0) ? (bool?)null : cbAlta.Checked;
            _proveedorDto.TIPO = chTipo.Checked;
            _proveedorDto.USUARIO_MANTENIMIENTO.USUARIO_ALTA = usuario;
            _proveedorDto.USUARIO_MANTENIMIENTO.USUARIO_MODIFICO = usuario;
            if (ddlTipoDocumento2.SelectedValue == "-1")
            {
                _proveedorDto.ID_TIPO_DOCUMENTO2 = 0;
            }
            else _proveedorDto.ID_TIPO_DOCUMENTO2 = Convert.ToInt16(ddlTipoDocumento2.SelectedValue);
            _proveedorDto.NUMERO_IDENTIFICACION2 = txtNumeroIdentificacion2.Text.Trim();

            return _proveedorDto;
        }

        private bool ValidaCampos()
        {
            if ((txtNumeroIdentificacion.Text != "") && (txtNumeroIdentificacion.Text.Length <= 14)) { result = true; } else { result = false; return false; }
            if ((txtDireccion.Text != "") && (txtDireccion.Text.Length <= 60)) { result = true; } else { result = false; return false; }
            if ((txtNombre.Text != "") && (txtNombre.Text.Length <= 100)) { result = true; } else { result = false; return false; }
            return result;
        }

        private void AlmacenarProveedor()
        {
            ProveedorDTO _proveedorDto = new ProveedorDTO();
            GestorProveedor gestorProveedor = null;

            OcultarAvisos();
            try
            {
                if (ddlTipoDocumento.SelectedValue != "-1")
                {
                    if (ValidaCampos())
                    {
                        gestorProveedor = GestorProveedor();
                        CargarObjetoProveedor(ref _proveedorDto);
                        gestorProveedor.AlmacenarProveedor(_proveedorDto);
                        hfIdProveedor.Value = _proveedorDto.ID_PROVEEDOR.ToString();
                        DesplegarAviso("El proveedor fue almacenado correctamente.");
                        CargarProveedoresGrid();
                    }
                    else DesplegarError("Debe Completar todos los datos correctamente");
                }
                else
                    DesplegarError("Debe Seleccionar el Tipo de Documento");
            }
            catch (ExcepcionesDIPCMI ex)
            {
                DesplegarError(ex.Message);
            }
            catch
            {
                DesplegarError("Error al almacenar proveedor.");
            }
            finally
            {
                if (gestorProveedor != null) gestorProveedor.Dispose();
            }
        }

        public void CargarProveedoresGrid()
        {
            ProveedorDTO _proveedorDto = new ProveedorDTO();
            GestorProveedor gestorProveedor = null;
            int x = 1;
            try
            {
                gestorProveedor = GestorProveedor();

                gvProveedor.DataSource = (from proveedor in gestorProveedor.ListaProveedores()
                                          select new
                                          {
                                              NUMERO = x++,
                                              ID_PROVEEDOR = proveedor.ID_PROVEEDOR,
                                              ID_TIPO_DOCUMENTO = proveedor.ID_TIPO_DOCUMENTO,
                                              TIPO_DOCUMENTO = proveedor.TIPO_DOCUMENTO,
                                              NUMERO_IDENTIFICACION = proveedor.NUMERO_IDENTIFICACION,
                                              NOMBRE = proveedor.NOMBRE,
                                              DIRECCION = proveedor.DIRECCION,
                                              REGIMEN = proveedor.REGIMEN,
                                             // idPequeñoContribuyente = proveedor.ES_PEQUEÑO_CONTRIBUYENTE,
                                              idAlta = proveedor.ALTA,
                                              idTipo = proveedor.TIPO,
                                              USUARIO_ALTA = proveedor.USUARIO_MANTENIMIENTO.USUARIO_ALTA,
                                              FECHA_ALTA = proveedor.USUARIO_MANTENIMIENTO.FECHA_ALTA.ToShortDateString(),
                                              USUARIO_MODIFICACION = proveedor.USUARIO_MANTENIMIENTO.USUARIO_MODIFICO,
                                              FECHA_MODIFICACION = string.IsNullOrEmpty(proveedor.USUARIO_MANTENIMIENTO.FECHA_MODIFICACION.ToString()) ? string.Empty : Convert.ToDateTime(proveedor.USUARIO_MANTENIMIENTO.FECHA_MODIFICACION).ToShortDateString(),
                                              ID_TIPO_DOCUMENTO2 = proveedor.ID_TIPO_DOCUMENTO2,
                                              NUMERO_IDENTIFICACION2 = proveedor.NUMERO_IDENTIFICACION2,
                                              TIPO_DOCUMENTO2 = proveedor.TIPO_DOCUMENTO2

                                          }).ToArray();
                gvProveedor.DataBind();
            }
            catch
            {
                DesplegarError("Error al desplegar los proveedores");
            }
            finally
            {
                if (gestorProveedor != null) gestorProveedor.Dispose();
            }
        }

        private void CargarProveedorEdicion(GridViewCommandEventArgs e)
        {
            try
            {
                int indice = Int32.Parse(e.CommandArgument.ToString());
                GridViewRow fila = gvProveedor.Rows[indice];

                hfIdProveedor.Value = HttpUtility.HtmlDecode(fila.Cells[1].Text);
                ddlTipoDocumento.SelectedValue = HttpUtility.HtmlDecode(fila.Cells[2].Text);
                txtNumeroIdentificacion.Text = HttpUtility.HtmlDecode(fila.Cells[4].Text);
                if (HttpUtility.HtmlDecode(fila.Cells[5].Text) == "0")
                {
                    ddlTipoDocumento2.SelectedValue = "-1";
                }
                else
                    ddlTipoDocumento2.SelectedValue = HttpUtility.HtmlDecode(fila.Cells[5].Text);
                txtNumeroIdentificacion2.Text = HttpUtility.HtmlDecode(fila.Cells[7].Text);
                txtNombre.Text = HttpUtility.HtmlDecode(fila.Cells[8].Text);
                txtDireccion.Text = HttpUtility.HtmlDecode(fila.Cells[9].Text);
                if ((fila.Cells[10].Text) == "&nbsp;")
                {
                    ddlRegimen.SelectedValue = "-1";
                }
                else
                    ddlRegimen.SelectedValue = HttpUtility.HtmlDecode(fila.Cells[10].Text);
                cbAlta.Checked = (((CheckBox)fila.FindControl("idAlta")).Checked);
                chTipo.Checked = (((CheckBox)fila.FindControl("idTipo")).Checked);
                lblFechaModificacionDB.Text = HttpUtility.HtmlDecode(fila.Cells[16].Text);
                lblUsuarioDB.Text = HttpUtility.HtmlDecode(fila.Cells[15].Text);
            }
            catch (Exception ex)
            {
                DesplegarError("Error al cargar los datos");
            }
        }

        private void DarBajaProveedor(GridViewCommandEventArgs e)
        {
            int indice = Int32.Parse(e.CommandArgument.ToString());
            GridViewRow fila = gvProveedor.Rows[indice];
            GestorProveedor gestorProveedor = null;

            try
            {
                gestorProveedor = GestorProveedor();
                gestorProveedor.DarBajaProveedor(Convert.ToInt16(fila.Cells[1].Text), usuario);

                DesplegarAviso("El proveedor fue dado de baja.");
                CargarProveedoresGrid();
            }
            catch
            {
                DesplegarError("Error al dar baja al proveedor");
            }
            finally
            {
                if (gestorProveedor != null) gestorProveedor.Dispose();
            }
        }

        private void DarAltaProveedor(GridViewCommandEventArgs e)
        {
            int indice = Int32.Parse(e.CommandArgument.ToString());
            GridViewRow fila = gvProveedor.Rows[indice];
            GestorProveedor gestorProveedor = null;

            try
            {
                gestorProveedor = GestorProveedor();
                gestorProveedor.DarAltaProveedor(Convert.ToInt16(fila.Cells[1].Text), usuario);

                DesplegarAviso("El proveedor fue dado de alta.");
                CargarProveedoresGrid();
            }
            catch
            {
                DesplegarError("Error al dar alta al proveedor");
            }
            finally
            {
                if (gestorProveedor != null) gestorProveedor.Dispose();
            }
        }
        
        public void CargarProveedoresGridBusqueda(Int16 tipoDoc, string identificacion, string nombre)
        {
            ProveedorDTO _proveedorDto = new ProveedorDTO();
            GestorProveedor gestorProveedor = null;
            int x = 1;
            try
            {
                gestorProveedor = GestorProveedor();

                gvProveedor.DataSource = (from proveedor in gestorProveedor.ListaProveedoresBusqueda(tipoDoc, identificacion, nombre)
                                          select new
                                          {
                                              NUMERO = x++,
                                              ID_PROVEEDOR = proveedor.ID_PROVEEDOR,
                                              ID_TIPO_DOCUMENTO = proveedor.ID_TIPO_DOCUMENTO,
                                              TIPO_DOCUMENTO = proveedor.TIPO_DOCUMENTO,
                                              NUMERO_IDENTIFICACION = proveedor.NUMERO_IDENTIFICACION,
                                              NOMBRE = proveedor.NOMBRE,
                                              DIRECCION = proveedor.DIRECCION,
                                              REGIMEN = proveedor.REGIMEN,
                                              idAlta = proveedor.ALTA,
                                              idTipo = proveedor.TIPO,
                                              USUARIO_ALTA = proveedor.USUARIO_MANTENIMIENTO.USUARIO_ALTA,
                                              FECHA_ALTA = proveedor.USUARIO_MANTENIMIENTO.FECHA_ALTA.ToShortDateString(),
                                              USUARIO_MODIFICACION = proveedor.USUARIO_MANTENIMIENTO.USUARIO_MODIFICO,
                                              FECHA_MODIFICACION = string.IsNullOrEmpty(proveedor.USUARIO_MANTENIMIENTO.FECHA_MODIFICACION.ToString()) ? string.Empty : Convert.ToDateTime(proveedor.USUARIO_MANTENIMIENTO.FECHA_MODIFICACION).ToShortDateString(),
                                              ID_TIPO_DOCUMENTO2 = proveedor.ID_TIPO_DOCUMENTO2,
                                              TIPO_DOCUMENTO2 = proveedor.TIPO_DOCUMENTO2,
                                              NUMERO_IDENTIFICACION2 = proveedor.NUMERO_IDENTIFICACION2
                                          }).ToArray();
                gvProveedor.DataBind();
            }
            catch
            {
                DesplegarError("Error al desplegar los proveedores");
            }
            finally
            {
                if (gestorProveedor != null) gestorProveedor.Dispose();
            }
        }

        protected void ddlPais_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlPais.SelectedValue == "-1")
                DesplegarError("Debe seleccionar un país");
            else
                CargarDDLTipoDocumento(ddlPais.SelectedValue.ToString());
        }

        #endregion

        #region Gestores

        protected GestorProveedor GestorProveedor()
        {
            GestorProveedor gestorSeguridad = new GestorProveedor(cnnApl);
            return gestorSeguridad;
        }
        #endregion

    }
}