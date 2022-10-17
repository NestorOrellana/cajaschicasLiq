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
using System.Text;

namespace RegistroFacturasWEB.Aprobaciones
{
    public partial class AprobacionFacturas : System.Web.UI.Page
    {
        #region Declaraciones
        string cnnApl = ConfigurationManager.ConnectionStrings["CnnApl"].ToString();
        string usuario;
        string dominio;
        #endregion

        #region Eventos

        protected void Page_Load(object sender, EventArgs e)
        {
            usuario = Context.User.Identity.Name.ToString();
            dominio = (string)Session["dominio"].ToString();
            hfUsuario.Value = usuario;
            if (!IsPostBack)
            {
                CargarDDLSociedad();
               // CargarDDLRegistrador();
                CargarEstadoDDL();
                CargarNivelDDL();
                CargarGrid();
            }

        }

        protected void btnGrabar_Click(object sender, EventArgs e)
        {
            AlmacenarRegistro();
        }

        protected void gvAprobacionFactura_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            this.gvAprobacionFactura.PageIndex = e.NewPageIndex;


            CargarDDLCentro(hfIdSociedad.Value);
            ddlCentro.SelectedValue = hfIdCentro.Value;

            CargarDDLCentroCosto(hfUsuario.Value, hfIdSociedad.Value);
            ddlCentroCosto.SelectedValue = hfIdCentroCosto.Value;

            CargarDDLOrdenCompra(hfUsuario.Value, hfIdSociedad.Value);
            ddlOrdenCosto.SelectedValue = hfIdCentroCompra.Value;

            CargarGridBusqueda(hfUsuario.Value, ddlSociedad.SelectedValue, Convert.ToInt32(ddlCentro.SelectedValue), Convert.ToInt16(ddlNivel.SelectedValue), ddlCentroCosto.SelectedValue, ddlOrdenCosto.SelectedValue, ddlEstado.SelectedValue, dominio, ddlRegistrador.SelectedValue.ToString());

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
            OcultarAvisos();
            BuscarFacturas();
            // string centrocosto, centrocompra;
            // if (hfIdCentroCosto.Value == "0") centrocosto = ""; else centrocosto = hfIdCentroCosto.Value;
            // if (hfIdCentroCompra.Value == "0") centrocompra = ""; else centrocompra = hfIdCentroCompra.Value;
            // //CargarGridBusqueda(ddlCentroCosto.SelectedValue, ddlOrdenCosto.SelectedValue,  Convert.ToInt32(ddlProveedor.SelectedValue), Convert.ToInt32(ddlCajaChica.SelectedValue));
            //// CargarGridBusqueda(Convert.ToInt32(hfIdCentro.Value), centrocosto, centrocompra,  usuario);
        }

        protected void btnAprobar_Click(object sender, EventArgs e)
        {
            AprobarFactura();
        }

        protected void btnRechazar_Click(object sender, EventArgs e)
        {
            RechazarFactura();
        }

        #endregion

        #region Metodos
        private void BuscarFacturas()
        {
            CargarDDLCentro(hfIdSociedad.Value);
            ddlCentro.SelectedValue = hfIdCentro.Value;

            CargarDDLCentroCosto(hfUsuario.Value, hfIdSociedad.Value);
            ddlCentroCosto.SelectedValue = hfIdCentroCosto.Value;

            CargarDDLOrdenCompra(hfUsuario.Value, hfIdSociedad.Value);
            ddlOrdenCosto.SelectedValue = hfIdCentroCompra.Value;

            CargarGridBusqueda(hfUsuario.Value, ddlSociedad.SelectedValue, Convert.ToInt32(ddlCentro.SelectedValue), Convert.ToInt16(ddlNivel.SelectedValue), ddlCentroCosto.SelectedValue, ddlOrdenCosto.SelectedValue, ddlEstado.SelectedValue, dominio, ddlRegistrador.SelectedValue.ToString());
        }

        private void CargarNivelDDL()
        {
            GestorAprobadores gestoraprobadores = null;
            try
            {
                gestoraprobadores = GestorAprobadores();
                List<LlenarDDL_DTO> _listanivelDto = gestoraprobadores.ListarNivelesDDL();
                ddlNivel.Items.Clear();
                ddlNivel.Items.Add(new ListItem("::ELIJA UNA OPCION::", "-1"));
                ddlNivel.AppendDataBoundItems = true;
                ddlNivel.DataSource = _listanivelDto;
                ddlNivel.DataTextField = "DESCRIPCION";
                ddlNivel.DataValueField = "IDENTIFICADOR";
                ddlNivel.DataBind();
            }
            catch
            {
                DesplegarError("Error al recuperar Niveles");
            }
            finally
            {
                if (gestoraprobadores != null) gestoraprobadores.Dispose();
            }
        }
        private void CargarEstadoDDL()
        {
            try
            {

                List<LlenarDDL_DTO> listaDDLDto = new List<LlenarDDL_DTO>();

                // listaDDLDto.Add(new LlenarDDL_DTO() { IDENTIFICADOR = "2", DESCRIPCION = "::ELIJA UNA OPCION::" });
                listaDDLDto.Add(new LlenarDDL_DTO() { IDENTIFICADOR = "3", DESCRIPCION = "NO APROBADAS" });
                listaDDLDto.Add(new LlenarDDL_DTO() { IDENTIFICADOR = "0", DESCRIPCION = "RECHAZADAS" });
                listaDDLDto.Add(new LlenarDDL_DTO() { IDENTIFICADOR = "1", DESCRIPCION = "APROBADAS" });


                ddlEstado.DataSource = listaDDLDto;
                ddlEstado.DataTextField = "DESCRIPCION";
                ddlEstado.DataValueField = "IDENTIFICADOR";
                ddlEstado.DataBind();
            }
            catch
            {
                DesplegarError("Error Estados");
            }
        }

        private void CargarDDLCentro(string sociedad)
        {
            GestorSociedad gestorsociedad = null;
            try
            {
                gestorsociedad = GestorSociedad();
                //  List<LlenarDDL_DTO> _listacentroDto = gestorsociedad.ListarCentrosMapeados(usuario, sociedad);
                List<LlenarDDL_DTO> _listacentroDto = gestorsociedad.ListarCentroMapeado(sociedad);
                ddlCentro.Items.Clear();
                ddlCentro.Items.Add(new ListItem("::ELIJA UNA OPCION::", "-1"));
                ddlCentro.AppendDataBoundItems = true;
                ddlCentro.DataSource = _listacentroDto;
                ddlCentro.DataTextField = "DESCRIPCION";
                ddlCentro.DataValueField = "IDENTIFICADOR";
                ddlCentro.DataBind();
            }
            catch
            {
                DesplegarError("Error al recuperar Centros");
            }
            finally
            {
                if (gestorsociedad != null) gestorsociedad.Dispose();
            }
        }

        private void CargarDDLSociedad()
        {
            GestorSociedad gestorSociedad = null;

            try
            {
                gestorSociedad = GestorSociedad();

                List<LlenarDDL_DTO> _listacentrocostsoDto = gestorSociedad.ListaSociedadesActivas();
                ddlSociedad.Items.Clear();
                ddlSociedad.Items.Add(new ListItem("::ELIJA UNA OPCION::", "-1"));
                ddlSociedad.AppendDataBoundItems = true;
                ddlSociedad.DataSource = _listacentrocostsoDto;
                ddlSociedad.DataTextField = "DESCRIPCION";
                ddlSociedad.DataValueField = "IDENTIFICADOR";
                ddlSociedad.DataBind();
            }
            catch
            {
                DesplegarError("Error al recuperar los Centros de Costo");
            }
            finally
            {
                if (gestorSociedad != null) gestorSociedad.Dispose();
            }
        }

        private void CargarDDLCentroCosto(string usuario, string sociedad)
        {
            GestorSociedad gestorsociedad = null;
            try
            {
                gestorsociedad = GestorSociedad();
                List<LlenarDDL_DTO> _listacentrocostoDto = gestorsociedad.ListarCentroCostoUsuariosDDL(usuario, sociedad); //ListarCentroCostoDDL(sociedad);

                ddlCentroCosto.Items.Clear();
                ddlCentroCosto.Items.Add(new ListItem("::ELIJA UNA OPCION::", "-1"));
                ddlCentroCosto.AppendDataBoundItems = true;
                ddlCentroCosto.DataSource = _listacentrocostoDto;
                ddlCentroCosto.DataTextField = "DESCRIPCION";
                ddlCentroCosto.DataValueField = "IDENTIFICADOR";
                ddlCentroCosto.DataBind();
            }
            catch
            {
                DesplegarError("Error al recuperar Centros de Costo");
            }
            finally
            {
                if (gestorsociedad != null) gestorsociedad.Dispose();
            }

        }

        private void CargarDDLOrdenCompra(string usuario, string sociedad)
        {
            GestorSociedad gestorsociedad = null;

            try
            {
                gestorsociedad = GestorSociedad();
                //List<UsuarioOrdenCompraDTO> _listaordencompraDto = gestorsociedad.ListarUsuarioOrdenCompra(usuario);
                List<LlenarDDL_DTO> _listaordencompraDto = gestorsociedad.ListarOrdenCostoUsuarioDDL(usuario, sociedad); //ListarOrdenCostoDDL(sociedad);
                ddlOrdenCosto.Items.Clear();
                ddlOrdenCosto.Items.Add(new ListItem("::ELIJA UNA OPCION::", "-1"));
                ddlOrdenCosto.AppendDataBoundItems = true;
                ddlOrdenCosto.DataSource = _listaordencompraDto;
                ddlOrdenCosto.DataTextField = "DESCRIPCION";
                ddlOrdenCosto.DataValueField = "IDENTIFICADOR";
                ddlOrdenCosto.DataBind();
            }
            catch
            {
                DesplegarError("Error al recuperar Centros de Compra");
            }
            finally
            {
                if (gestorsociedad != null) gestorsociedad.Dispose();
            }


        }


        private void CargarDDLRegistrador()
        {
            GestorSociedad gestorSociedad = null;

            try
            {
                gestorSociedad = GestorSociedad();

                List<LlenarDDL_DTO> _listacentrocostsoDto = gestorSociedad.ListaRegistradores(usuario);
                ddlRegistrador.Items.Clear();
                ddlRegistrador.Items.Add(new ListItem("::ELIJA UNA OPCION::", "-1"));
                ddlRegistrador.AppendDataBoundItems = true;
                ddlRegistrador.DataSource = _listacentrocostsoDto;
                ddlRegistrador.DataTextField = "DESCRIPCION";
                ddlRegistrador.DataValueField = "IDENTIFICADOR";
                ddlRegistrador.DataBind();
            }
            catch
            {
                DesplegarError("Error al recuperar los Registradores");
            }
            finally
            {
                if (gestorSociedad != null) gestorSociedad.Dispose();
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
                _aprobacionfacturaDto.ID_APROBACION_CENTRO = Convert.ToInt32(fila.Cells[19].Text);
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
                OcultarAvisos();
                gestoraprobadores = GestorAprobadores();
                _aprobacionfacturaDto.ID_FACTURA = Convert.ToInt32(fila.Cells[0].Text);
                _aprobacionfacturaDto.ID_APROBACION_CENTRO = Convert.ToInt32(fila.Cells[19].Text);
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

        private void AprobarFactura()
        {
            FacturaEncabezadoDTO _facturaEncabezado = null;
            GestorAprobadores gestoraprobadores = null;
            AprobacionFacturasDTO _aprobacionfacturaDto = new AprobacionFacturasDTO();

            try
            {
                OcultarAvisos();
                gestoraprobadores = GestorAprobadores();
                _facturaEncabezado = gestoraprobadores.BuscarFacturaAprobar(usuario, Convert.ToInt64(hfIdFactura.Value), dominio);

                if (_facturaEncabezado == null)
                    throw new ExcepcionesDIPCMI("La factura no esta disponible");


                _aprobacionfacturaDto.ID_FACTURA = Convert.ToInt32(hfIdFactura.Value);
                _aprobacionfacturaDto.ID_APROBACION_CENTRO = _facturaEncabezado.DETALLE_APROBACION.ID_APROBDOR_CENTRO;
                _aprobacionfacturaDto.ESTADO = 1;
                _aprobacionfacturaDto.USUARIO_MANTENIMIENTO.USUARIO_ALTA = usuario;
                _aprobacionfacturaDto.USUARIO_MANTENIMIENTO.USUARIO_MODIFICO = usuario;


                gestoraprobadores.AlmacenarRegistro(_aprobacionfacturaDto);

                DesplegarAviso("La Factura fue Aprobada");
                BuscarFacturas();
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

        private void RechazarFactura()
        {
            FacturaEncabezadoDTO _facturaEncabezado = null;
            GestorAprobadores gestoraprobadores = null;
            AprobacionFacturasDTO _aprobacionfacturaDto = new AprobacionFacturasDTO();

            try
            {
                OcultarAvisos();
                gestoraprobadores = GestorAprobadores();

                _facturaEncabezado = gestoraprobadores.BuscarFacturaAprobar(usuario, Convert.ToInt64(hfIdFactura.Value), dominio);

                if (_facturaEncabezado == null)
                    throw new ExcepcionesDIPCMI("La factura no esta disponible");


                _aprobacionfacturaDto.ID_FACTURA = Convert.ToInt32(hfIdFactura.Value);
                _aprobacionfacturaDto.ID_APROBACION_CENTRO = _facturaEncabezado.DETALLE_APROBACION.ID_APROBDOR_CENTRO;
                _aprobacionfacturaDto.ESTADO = 0;
                _aprobacionfacturaDto.USUARIO_MANTENIMIENTO.USUARIO_ALTA = usuario;
                _aprobacionfacturaDto.USUARIO_MANTENIMIENTO.USUARIO_MODIFICO = usuario;

                gestoraprobadores.AlmacenarRegistro(_aprobacionfacturaDto);

                DesplegarAviso("La Factura fue Rechazada");
                BuscarFacturas();
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
                gvAprobacionFactura.DataSource = (from factura in gestoraprobadores.ListaFacturas(usuario, dominio)
                                                  select new
                                                  {
                                                      idAprobada = false,
                                                      idRechazada = false,
                                                      ID_FACTURA = factura.ID_FACTURA,
                                                      SERIE = factura.SERIE,
                                                      NUMERO = factura.NUMERO,
                                                      CENTRO_COSTO = factura.CENTRO_COSTO,
                                                      ORDEN_COSTO = factura.ORDEN_COSTO,
                                                      FECHA = factura.FECHA_FACTURA.ToShortDateString(),
                                                      ID_PROVEEDOR = factura.ID_PROVEEDOR,
                                                      ID_CAJA_CHICA = factura.ID_CAJA_CHICA,
                                                      CAJA_CHICA = factura.CAJA_CHICA.DESCRIPCION,
                                                      IMPUESTO = factura.IMPUESTO,
                                                      IVA = factura.IVA.ToString("F"),
                                                      TOTAL = factura.VALOR_TOTAL.ToString("F"),
                                                      TIPO_FACTURA = factura.TIPO_FACTURA,
                                                      ID_APROBADOR_CENTRO = factura.DETALLE_APROBACION.ID_APROBDOR_CENTRO,
                                                      ID_SOCIEDAD_CENTRO = factura.DETALLE_APROBACION.ID_SOCIEDAD_CENTRO,
                                                      btbRechazar = Convert.ToInt32(ddlEstado.SelectedValue),
                                                      btbAprobar = Convert.ToInt32(ddlEstado.SelectedValue),
                                                      PROVEEDOR = factura.NOMBRE_PROVEEDOR,
                                                      DESCRIPCION = factura.DESCRIPCION,
                                                      NOMBRE_CENTRO_COSTO = factura.NOMBRE_CENTRO_COSTO,
                                                      NOMBRE_ORDEN_COSTO = factura.NOMBRE_ORDEN_COSTO,
                                                      FECHA_CREACION = factura.USUARIO_MANTENIMIENTO.FECHA_ALTA.ToShortDateString(),
                                                      USUARIO_CREACION = factura.USUARIO_MANTENIMIENTO.USUARIO_ALTA,
                                                      MONEDA = factura.MONEDA
                                                      // btnAprobar = factura.APROBADA
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

        public void CargarGridBusqueda(string usuario, string codSociedad, Int32 idSociedadCentro, Int16 Nivel, string CentroCosto, string OrdenCosto, string estado, string dominio, string registrador)
        {
            FacturaEncabezadoDTO _facturaDto = new FacturaEncabezadoDTO();
            GestorAprobadores gestoraprobadores = null;

            try
            {
                gestoraprobadores = GestorAprobadores();
                gvAprobacionFactura.DataSource = (from factura in gestoraprobadores.BuscarFacturas(usuario, codSociedad, idSociedadCentro, Nivel, CentroCosto, OrdenCosto, estado, dominio, registrador)
                                                  select new
                                                  {
                                                      //idAprobada = false,
                                                      //idRechazada = false,
                                                      //ID_FACTURA = factura.ID_FACTURA,
                                                      //SERIE = factura.SERIE,
                                                      //NUMERO = factura.NUMERO,
                                                      //CENTRO_COSTO = factura.CENTRO_COSTO,
                                                      //ORDEN_COMPRA = factura.ORDEN_COSTO,
                                                      //FECHA = factura.FECHA_FACTURA,
                                                      //ID_PROVEEDOR = factura.ID_PROVEEDOR,
                                                      //ID_CAJA_CHICA = factura.ID_CAJA_CHICA,
                                                      //CAJA_CHICA = factura.CAJA_CHICA.DESCRIPCION,
                                                      //IVA = factura.IVA,
                                                      //TOTAL = factura.VALOR_TOTAL,
                                                      //TIPO_FACTURA = factura.TIPO_FACTURA,
                                                      //NIVEL = factura.NIVEL,
                                                      //APROBADOR = factura.DETALLE_APROBACION.APROBADOR,
                                                      //ID_APROBADOR_CENTRO = factura.DETALLE_APROBACION.ID_APROBDOR_CENTRO,
                                                      //ID_SOCIEDAD_CENTRO = factura.DETALLE_APROBACION.ID_SOCIEDAD_CENTRO,
                                                      //btbRechazar = Convert.ToInt32(ddlEstado.SelectedValue),
                                                      //btbAprobar = Convert.ToInt32(ddlEstado.SelectedValue)
                                                      ////btnAprobar = factura.APROBADA

                                                      idAprobada = false,
                                                      idRechazada = false,
                                                      ID_FACTURA = factura.ID_FACTURA,
                                                      SERIE = factura.SERIE,
                                                      NUMERO = factura.NUMERO,
                                                      CENTRO_COSTO = factura.CENTRO_COSTO,
                                                      ORDEN_COSTO = factura.ORDEN_COSTO,
                                                      FECHA = factura.FECHA_FACTURA.ToShortDateString(),
                                                      ID_PROVEEDOR = factura.ID_PROVEEDOR,
                                                      ID_CAJA_CHICA = factura.ID_CAJA_CHICA,
                                                      CAJA_CHICA = factura.CAJA_CHICA.DESCRIPCION,
                                                      IVA = factura.IVA.ToString("F"),
                                                      TOTAL = factura.VALOR_TOTAL.ToString("F"),
                                                      TIPO_FACTURA = factura.TIPO_FACTURA,
                                                      ID_APROBADOR_CENTRO = factura.DETALLE_APROBACION.ID_APROBDOR_CENTRO,
                                                      ID_SOCIEDAD_CENTRO = factura.DETALLE_APROBACION.ID_SOCIEDAD_CENTRO,
                                                      btbRechazar = Convert.ToInt32(ddlEstado.SelectedValue),
                                                      btbAprobar = Convert.ToInt32(ddlEstado.SelectedValue),
                                                      PROVEEDOR = factura.NOMBRE_PROVEEDOR,
                                                      NOMBRE_CENTRO_COSTO = factura.NOMBRE_CENTRO_COSTO,
                                                      NOMBRE_ORDEN_COSTO = factura.NOMBRE_ORDEN_COSTO,
                                                      FECHA_CREACION = factura.USUARIO_MANTENIMIENTO.FECHA_ALTA.ToShortDateString(),
                                                      USUARIO_CREACION = factura.USUARIO_MANTENIMIENTO.USUARIO_ALTA,
                                                      DESCRIPCION = factura.DESCRIPCION,
                                                      MONEDA = factura.MONEDA, 
                                                      IMPUESTO = factura.IMPUESTO
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
            try
            {
                OcultarAvisos();
                foreach (GridViewRow row in gvAprobacionFactura.Rows)
                {
                    gestoraprobadores = GestorAprobadores();
                    aprob = Convert.ToInt32(((CheckBox)row.FindControl("idAprobada")).Checked);
                    rechaz = Convert.ToInt32(((CheckBox)row.FindControl("idRechazada")).Checked);
                    if ((aprob == 1) || (rechaz == 1))
                    {
                        if (aprob == 1) { estado = 1; }
                        if (rechaz == 1) { estado = 0; }
                        _aprobacionfacturaDto.ID_FACTURA = Convert.ToInt32(row.Cells[0].Text);
                        _aprobacionfacturaDto.ID_APROBACION_CENTRO = Convert.ToInt32(row.Cells[19].Text);
                        _aprobacionfacturaDto.ESTADO = estado;
                        _aprobacionfacturaDto.USUARIO_MANTENIMIENTO.USUARIO_ALTA = usuario;
                        _aprobacionfacturaDto.USUARIO_MANTENIMIENTO.USUARIO_MODIFICO = usuario;
                        gestoraprobadores.AlmacenarRegistro(_aprobacionfacturaDto);
                    }
                    DesplegarAviso("Facturas Actualizadas Correctamente.");
                }
                CargarGrid();
                // return _aprobacionfacturaDto;
            }
            catch
            {
                DesplegarError("Error al Actualizar Información");
            }
            finally
            {
                if (gestoraprobadores != null) gestoraprobadores.Dispose();
            }
        }

        private void ListaSeleccion(ref List<AprobacionFacturasDTO> _listaAprobacion)
        {
            AprobacionFacturasDTO _aprobacionFacturasDto = new AprobacionFacturasDTO();

            int aprob = 0;
            int rechaz = 0;
            Int16 estado = 0;
            try
            {
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
                        _aprobacionFacturasDto.ID_APROBACION_CENTRO = Convert.ToInt32(row.Cells[19].Text);
                        _aprobacionFacturasDto.ESTADO = estado;
                        _aprobacionFacturasDto.USUARIO_MANTENIMIENTO.USUARIO_ALTA = usuario;
                        _aprobacionFacturasDto.USUARIO_MANTENIMIENTO.USUARIO_MODIFICO = usuario;
                        _listaAprobacion.Add(_aprobacionFacturasDto);
                    }
                }
            }
            catch
            {
                DesplegarError("Error al Listar Información");
            }
        }

        #endregion

        #region Gestores
        protected GestorAprobadores GestorAprobadores()
        {
            GestorAprobadores gestoraprobadores = new GestorAprobadores(cnnApl);
            return gestoraprobadores;
        }

        protected GestorProveedor GestorProveedor()
        {
            GestorProveedor getorproveedor = new GestorProveedor(cnnApl);
            return getorproveedor;
        }

        protected GestorSociedad GestorSociedad()
        {
            GestorSociedad gestorsociedad = new GestorSociedad(cnnApl);
            return gestorsociedad;
        }
        #endregion
    }
}