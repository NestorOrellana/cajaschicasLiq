using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using LogicaComun.Enum;
using DipCmiGT.LogicaCajasChicas;
using DipCmiGT.LogicaComun;
using DipCmiGT.LogicaCajasChicas.Sesion;
using System.Configuration;
using System.Drawing;

namespace RegistroFacturasWEB.Mantenimientos
{
    public partial class AprobacionFacturas : System.Web.UI.Page
    {
        #region Declaraciones
        string cnnApl = ConfigurationManager.ConnectionStrings["CnnApl"].ToString();
        string usuario = "WPISQUIY";
        #endregion

        #region Eventos
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CargarGrid();
                //CargarDDLCentroCosto();
                CargarDDLOrdenCompra();
                CargarDDLProveedor();
                CargarDDLSociedad();
            }
        }
        protected void btnGrabar_Click(object sender, EventArgs e)
        {
            AlmacenarRegistro();
        }
        protected void gvAprobacionFactura_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName.Equals("Aprobar"))
            {
                OcultarAvisos();
                AprobarFactura(e);
            }
            if (e.CommandName.Equals("Rechazar"))
            {
                OcultarAvisos();
                RechazarFactura(e);
            }
        }
        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            CargarGridBusqueda(ddlCentroCosto.SelectedValue, ddlOrdenCosto.SelectedValue, Convert.ToInt32(ddlProveedor.SelectedValue), Convert.ToInt32(ddlCajaChica.SelectedValue));
        }
        //protected void btnAprobar_Click(object sender, EventArgs e)
        //{
        //    bool estado = true;
        //    if (btnAprobar.Text == "Ninguno")
        //    {
        //        estado = false;
        //        btnAprobar.Text = "Todos";
        //    }
        //    else
        //    {
        //        estado = true;
        //        btnAprobar.Text = "Ninguno";
        //    }
        //    foreach (GridViewRow row in gvAprobacionFactura.Rows)
        //    {
        //        ((CheckBox)row.FindControl("idAprobada")).Checked = estado;
        //    }
        //}
        #endregion

        #region Metodos
        private void CargarDDLCentro(string sociedad)
        {
            GestorSociedad gestorsociedad = null;
            try
            {
                gestorsociedad = GestorSociedad();
                //List<LlenarDDL_DTO> _listacentroDto = gestorsociedad.ListarCentrosMapeados(usuario, sociedad);

                ddlCentro.Items.Clear();
                ddlCentro.Items.Add(new ListItem("::ELIJA UNA OPCION::", "-1"));
                ddlCentro.AppendDataBoundItems = true;
                //ddlCentro.DataSource = _listacentroDto;
                ddlCentro.DataTextField = "DESCRIPCION";
                ddlCentro.DataValueField = "IDENTIFICADOR";
                ddlCentro.DataBind();
            }
            catch
            {
            }
            finally
            {
            }
        }
        private void CargarDDLSociedad()
        {
            GestorSociedad gestorsociedad = null;
            try
            {
                gestorsociedad = GestorSociedad();
                //List<LlenarDDL_DTO> _listasociedadDto = gestorsociedad.ListarSociedadesMapeadas(usuario);


                ddlSociedad.Items.Clear();
                ddlSociedad.Items.Add(new ListItem("::ELIJA UNA OPCION::", "-1"));
                ddlSociedad.AppendDataBoundItems = true;
                //ddlSociedad.DataSource = _listasociedadDto;
                ddlSociedad.DataTextField = "DESCRIPCION";
                ddlSociedad.DataValueField = "IDENTIFICADOR";
                ddlSociedad.DataBind();

            }
            catch
            {
                DesplegarError("Error al recuperar Sociedades");
            }
            finally
            {
                if (gestorsociedad != null) gestorsociedad.Dispose();
            }
        }

        //private void CargarDDLCentroCosto()
        //{
        //     GestorUsuarioCentroCosto gestorcentrocosto = null;


        //private void CargarDDLCajaChica()
        //{
        //    GestorAprobacionFactura gestoraprobacionfactura = null;
        //    try
        //    {
        //        gestorcentrocosto = GestorUsuarioCentroCosto();
        //        List<UsuarioCentroCostoDTO> _listacentrocostoDto = gestorcentrocosto.ListarUsuarioCentroCosto(usuario);
        ////    try
        ////    {
        ////        gestoraprobacionfactura = GestorAprobacionfactura();
        ////        List<CajaChicaDTO> _listacajachica = gestoraprobacionfactura.ListarCajaChica();
        //        ddlCentroCosto.Items.Clear();
        //        ddlCentroCosto.Items.Add(new ListItem("::ELIJA UNA OPCION::", "-1"));
        //        ddlCentroCosto.AppendDataBoundItems = true;
        //        ddlCentroCosto.DataSource = _listacentrocostoDto;
        //        ddlCentroCosto.DataTextField = "CENTRO_COSTO";
        //        ddlCentroCosto.DataValueField = "CENTRO_COSTO";
        //        ddlCentroCosto.DataBind();
        //    }
        //    catch
        //    {
        //        DesplegarError("Error al recuperar Centros de Costo");
        //    }
        //    finally
        //    {
        //        if (gestorcentrocosto != null) gestorcentrocosto.Dispose();
        //    }

        //        ddlCajaChica.Items.Clear();
        //        ddlCajaChica.Items.Add(new ListItem("::ELIJA UNA OPCION::", "-1"));
        //        ddlCajaChica.AppendDataBoundItems = true;
        //        ddlCajaChica.DataSource = _listacajachica;
        //        ddlCajaChica.DataTextField = "Descripcion";
        //        ddlCajaChica.DataValueField = "ID_CAJA_CHICA";
        //        ddlCajaChica.DataBind();
        //    }
        //    catch
        //    {
        //        DesplegarError("Error al recuperar Cajas Chicas");
        //    }
        //    finally
        //    {
        //        if (gestoraprobacionfactura != null) gestoraprobacionfactura.Dispose();
        //    }


        //}

        //private void CargarDDLCentroCosto(string codigoSociedad)
        //{
        //     GestorUsuarioCentroCosto gestorcentrocosto = null;

        //    try
        //    {
        //        gestorcentrocosto = GestorUsuarioCentroCosto();
        //        List<SAPCentroCostoDTO> _listacentrocostoDto = gestorcentrocosto.ListarCentroCosto();

        //        ddlCentroCosto.Items.Clear();
        //        ddlCentroCosto.Items.Add(new ListItem("::ELIJA UNA OPCION::", "-1"));
        //        ddlCentroCosto.AppendDataBoundItems = true;
        //        ddlCentroCosto.DataSource = _listacentrocostoDto;
        //        ddlCentroCosto.DataTextField = "KOSTL";
        //        ddlCentroCosto.DataValueField = "KOSTL";
        //        ddlCentroCosto.DataBind();
        //    }
        //    catch
        //    {
        //        DesplegarError("Error al recuperar Centros de Costo");
        //    }
        //    finally
        //    {
        //        if (gestorcentrocosto != null) gestorcentrocosto.Dispose();
        //    }

        //}


        private void CargarDDLProveedor()
        {
            GestorProveedor gestorproveedor = null;

            try
            {
                gestorproveedor = GestorProveedor();
                List<ProveedorDTO> _listaproveedorDto = gestorproveedor.ListaProveedoresDDL();

                ddlProveedor.Items.Clear();
                ddlProveedor.Items.Add(new ListItem("::ELIJA UNA OPCION::", "-1"));
                ddlProveedor.AppendDataBoundItems = true;
                ddlProveedor.DataSource = _listaproveedorDto;
                ddlProveedor.DataTextField = "NOMBRE";
                ddlProveedor.DataValueField = "ID_PROVEEDOR";
                ddlProveedor.DataBind();
            }
            catch
            {
                DesplegarError("Error al recuperar Proveedores");
            }
            finally
            {
                if (gestorproveedor != null) gestorproveedor.Dispose();
            }
        }

        private void CargarDDLOrdenCompra()
        {
            GestorSociedad gestorordencompra = null;

            try
            {
                gestorordencompra = GestorSociedad();
                List<UsuarioOrdenCostoDTO> _listaordencompraDto = gestorordencompra.ListarUsuarioOrdenCompra(usuario);

                ddlOrdenCosto.Items.Clear();
                ddlOrdenCosto.Items.Add(new ListItem("::ELIJA UNA OPCION::", "-1"));
                ddlOrdenCosto.AppendDataBoundItems = true;
                ddlOrdenCosto.DataSource = _listaordencompraDto;
                ddlOrdenCosto.DataTextField = "ORDEN_COMPRA";
                ddlOrdenCosto.DataValueField = "ORDEN_COMPRA";
                ddlOrdenCosto.DataBind();
            }
            catch
            {
                DesplegarError("Error al recuperar Centros de Costo");
            }
            finally
            {
                if (gestorordencompra != null) gestorordencompra.Dispose();
            }


        }

        private void AprobarFactura(GridViewCommandEventArgs e)
        {
            int indice = Int32.Parse(e.CommandArgument.ToString());
            GridViewRow fila = gvAprobacionFactura.Rows[indice];
            //GestorAprobacionFactura gestoraprobacionfactura = null;
            GestorAprobadores gestoraprobadores = null;
            AprobacionFacturasDTO _aprobacionfacturaDto = new AprobacionFacturasDTO();

            try
            {
                gestoraprobadores = GestorAprobadores();
                _aprobacionfacturaDto.ID_FACTURA = Convert.ToInt32(fila.Cells[0].Text);
                _aprobacionfacturaDto.ID_APROBACION_CENTRO = 1;
                _aprobacionfacturaDto.ESTADO = 1;
                _aprobacionfacturaDto.USUARIO_MANTENIMIENTO.USUARIO_ALTA = usuario;
                _aprobacionfacturaDto.USUARIO_MANTENIMIENTO.USUARIO_MODIFICO = usuario;
                gestoraprobadores.AlmacenarRegistro(_aprobacionfacturaDto);

                DesplegarAviso("La Factura fue Aprobada");
                CargarGrid();
            }
            catch
            {
                DesplegarError("Error al Aprobar la Factura");
            }
            finally
            {
                if (gestoraprobadores != null) gestoraprobadores.Dispose();
            }
        }

        private void RechazarFactura(GridViewCommandEventArgs e)
        {
            int indice = Int32.Parse(e.CommandArgument.ToString());
            GridViewRow fila = gvAprobacionFactura.Rows[indice];
            GestorAprobadores gestoraprobadores = null;
            AprobacionFacturasDTO _aprobacionfacturaDto = new AprobacionFacturasDTO();

            try
            {
                gestoraprobadores = GestorAprobadores();
                _aprobacionfacturaDto.ID_FACTURA = Convert.ToInt32(fila.Cells[0].Text);
                _aprobacionfacturaDto.ID_APROBACION_CENTRO = 1;
                _aprobacionfacturaDto.ESTADO = 0;
                _aprobacionfacturaDto.USUARIO_MANTENIMIENTO.USUARIO_ALTA = usuario;
                _aprobacionfacturaDto.USUARIO_MANTENIMIENTO.USUARIO_MODIFICO = usuario;
                gestoraprobadores.AlmacenarRegistro(_aprobacionfacturaDto);

                DesplegarAviso("La Factura fue Rechazada");
                CargarGrid();
            }
            catch
            {
                DesplegarError("Error al Rechazar la Factura");
            }
            finally
            {
                if (gestoraprobadores != null) gestoraprobadores.Dispose();
            }
        }

        public void CargarGrid()
        {
            FacturaEncabezadoDTO _facturaDto = new FacturaEncabezadoDTO();
            GestorAprobadores gestoraprobadores = null;

            try
            {
                gestoraprobadores = GestorAprobadores();
                gvAprobacionFactura.DataSource = (from factura in gestoraprobadores.ListaFacturas()
                                                  select new
                                                  {
                                                      idAprobada = false,
                                                      idRechazada = false,
                                                      ID_FACTURA = factura.ID_FACTURA,
                                                      SERIE = factura.SERIE,
                                                      NUMERO = factura.NUMERO,
                                                      FECHA = factura.FECHA_FACTURA,
                                                      IDespecial = factura.ES_ESPECIAL,
                                                      ID_PROVEEDOR = factura.ID_PROVEEDOR,
                                                      PROVEEDOR = factura.NOMBRE_PROVEEDOR,
                                                      ID_CAJA_CHICA = factura.ID_CAJA_CHICA,
                                                      CAJA_CHICA = factura.CAJA_CHICA.DESCRIPCION,
                                                      retIVA = factura.RETENCION_IVA,
                                                      retISR = factura.RETENCION_ISR,
                                                      IVA = factura.IVA,
                                                      TOTAL = factura.VALOR_TOTAL
                                                  }).ToArray();
                gvAprobacionFactura.DataBind();
            }
            catch
            {
                DesplegarError("Error al Desplegar Las Facturas");
            }
            finally
            {
                if (gestoraprobadores != null) gestoraprobadores.Dispose();
            }
        }

        public void CargarGridBusqueda(string centrocosto, string cuentacontable, int proveedor, int cajachica)
        {
            FacturaEncabezadoDTO _facturaDto = new FacturaEncabezadoDTO();
            GestorAprobadores gestoraprobadores = null;

            try
            {
                gestoraprobadores = GestorAprobadores();
                gvAprobacionFactura.DataSource = (from factura in gestoraprobadores.BuscarFacturas(centrocosto, cuentacontable, proveedor, cajachica)
                                                  select new
                                                  {
                                                      idAprobada = false,
                                                      idRechazada = false,
                                                      ID_FACTURA = factura.ID_FACTURA,
                                                      SERIE = factura.SERIE,
                                                      NUMERO = factura.NUMERO,
                                                      FECHA = factura.FECHA_FACTURA,
                                                      IDespecial = factura.ES_ESPECIAL,
                                                      ID_PROVEEDOR = factura.ID_PROVEEDOR,
                                                      PROVEEDOR = factura.NOMBRE_PROVEEDOR,
                                                      ID_CAJA_CHICA = factura.ID_CAJA_CHICA,
                                                      CAJA_CHICA = factura.CAJA_CHICA.DESCRIPCION,
                                                      retIVA = factura.RETENCION_IVA,
                                                      retISR = factura.RETENCION_ISR,
                                                      IVA = factura.IVA,
                                                      TOTAL = factura.VALOR_TOTAL
                                                  }).ToArray();
                gvAprobacionFactura.DataBind();
            }
            catch
            {
                DesplegarError("Error al Desplegar Las Facturas");
            }
            finally
            {
                if (gestoraprobadores != null) gestoraprobadores.Dispose();
            }
        }

        private void DesplegarAviso(string Mensaje)
        {
            divMensaje.Attributes.CssStyle.Add("display", "table");
            divMensaje.InnerHtml = Mensaje;
        }

        private void DesplegarError(string Mensaje)
        {
            divMensajeError.Attributes.CssStyle.Add("display", "table");
            divMensajeError.InnerHtml = Mensaje;
        }

        private void OcultarAvisos()
        {
            divMensaje.Attributes.CssStyle.Add("display", "none");
            divMensajeError.Attributes.CssStyle.Add("display", "none");
        }

        private void AlmacenarRegistro()
        {
            AprobacionFacturasDTO _aprobacionfacturaDto = new AprobacionFacturasDTO();
            GestorAprobadores gestoraprobadores = null;

            int aprob = 0;
            int rechaz = 0;
            Int16 estado = 0;
            foreach (GridViewRow row in gvAprobacionFactura.Rows)
            {
                //gestoraprobadorfactura = GestorAprobacionfactura();
                aprob = Convert.ToInt32(((CheckBox)row.FindControl("idAprobada")).Checked);
                rechaz = Convert.ToInt32(((CheckBox)row.FindControl("idRechazada")).Checked);
                if ((aprob == 1) || (rechaz == 1))
                {

                    if (aprob == 1) { estado = 1; }
                    if (rechaz == 1) { estado = 0; }
                    _aprobacionfacturaDto.ID_FACTURA = Convert.ToInt32(row.Cells[0].Text);
                    _aprobacionfacturaDto.ID_APROBACION_CENTRO = 1;
                    _aprobacionfacturaDto.ESTADO = estado;
                    _aprobacionfacturaDto.USUARIO_MANTENIMIENTO.USUARIO_ALTA = usuario;
                    _aprobacionfacturaDto.USUARIO_MANTENIMIENTO.USUARIO_MODIFICO = usuario;
                    // gestoraprobadorfactura.AlmacenarRegistro(_aprobacionfacturaDto);
                    gestoraprobadores = GestorAprobadores();
                    aprob = Convert.ToInt32(((CheckBox)row.FindControl("idAprobada")).Checked);
                    rechaz = Convert.ToInt32(((CheckBox)row.FindControl("idRechazada")).Checked);
                    if ((aprob == 1) || (rechaz == 1))
                    {
                        if (aprob == 1) { estado = 1; }
                        if (rechaz == 1) { estado = 0; }
                        _aprobacionfacturaDto.ID_FACTURA = Convert.ToInt32(row.Cells[0].Text);
                        _aprobacionfacturaDto.ID_APROBACION_CENTRO = 1;
                        _aprobacionfacturaDto.ESTADO = estado;
                        _aprobacionfacturaDto.USUARIO_MANTENIMIENTO.USUARIO_ALTA = usuario;
                        _aprobacionfacturaDto.USUARIO_MANTENIMIENTO.USUARIO_MODIFICO = usuario;
                        gestoraprobadores.AlmacenarRegistro(_aprobacionfacturaDto);

                    }
                }
            }
            // return _aprobacionfacturaDto;
        }

        private void ListaSeleccion(ref List<AprobacionFacturasDTO> _listaAprobacion)
        {
            AprobacionFacturasDTO _aprobacionFacturasDto = new AprobacionFacturasDTO();

            int aprob = 0;
            int rechaz = 0;
            Int16 estado = 0;
            foreach (GridViewRow row in gvAprobacionFactura.Rows)
            {
                aprob = Convert.ToInt32(((CheckBox)row.FindControl("idAprobada")).Checked);
                rechaz = Convert.ToInt32(((CheckBox)row.FindControl("idRechazada")).Checked);
                if ((aprob == 1) || (rechaz == 1))
                {
                    _aprobacionFacturasDto = new AprobacionFacturasDTO();
                    if (aprob == 1) { estado = 1; }
                    if (rechaz == 1) { estado = 0; }
                    _aprobacionFacturasDto.ID_FACTURA = Convert.ToInt32(row.Cells[0].Text);
                    _aprobacionFacturasDto.ID_APROBACION_CENTRO = 1;
                    _aprobacionFacturasDto.ESTADO = estado;
                    _aprobacionFacturasDto.USUARIO_MANTENIMIENTO.USUARIO_ALTA = usuario;
                    _aprobacionFacturasDto.USUARIO_MANTENIMIENTO.USUARIO_MODIFICO = usuario;
                    _listaAprobacion.Add(_aprobacionFacturasDto);
                }
            }
        }

        #endregion

        #region Gestores
        protected GestorAprobadores GestorAprobadores()
        {
            GestorAprobadores gestoraprovadores = new GestorAprobadores(cnnApl);
            return gestoraprovadores;
        }

        protected GestorSociedad GestorSociedad()
        {
            GestorSociedad gestorordencompra = new GestorSociedad(cnnApl);
            return gestorordencompra;
        }

        protected GestorProveedor GestorProveedor()
        {
            GestorProveedor getorproveedor = new GestorProveedor(cnnApl);
            return getorproveedor;
        }

        #endregion
    }
}