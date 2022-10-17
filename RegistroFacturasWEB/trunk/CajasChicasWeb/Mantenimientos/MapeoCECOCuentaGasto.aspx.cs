using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Configuration;
using System.Web.UI.WebControls;
using DipCmiGT.LogicaCajasChicas;
using DipCmiGT.LogicaCajasChicas.Sesion;
using DipCmiGT.LogicaComun;

namespace RegistroFacturasWEB.Mantenimientos
{
    public partial class MapeoCECOCuentaGasto : System.Web.UI.Page
    {
        #region Declaraciones

        string cnnApl = ConfigurationManager.ConnectionStrings["CnnApl"].ToString();
        string usuario;
        bool result;

        #endregion

        #region Eventos

        protected void Page_Load(object sender, EventArgs e)
        {
            usuario = Context.User.Identity.Name.ToString();
            if (!IsPostBack)
            {
                CargarDDLPais();
                CargarGridMapeos();
            }
        }

        protected void btnLimpiar_Click(object sender, EventArgs e)
        {
            LimpiarControles();
        }

        protected void btnGrabar_Click(object sender, EventArgs e)
        {
            GuardarMapeo();
        }

        protected void gvMapeos_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            this.gvMapeos.PageIndex = e.NewPageIndex;
            CargarGridMapeos();
        }


        protected void CambioPais(object sender, EventArgs e)
        {
            string pais = DropDownListPais.SelectedValue;
            CargarDDLCentroCosto(pais);
            CargarDDLOrdenCosto(pais);
            CargarDDLTipoGastos(pais);
        }

        protected void CambioCentro(object sender, EventArgs e)
        {
            string centro = DropDownListCentroCosto.SelectedValue;
            CargarDDLCuentaContable(centro);
        }

        protected void gvMapeos_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName.Equals("Editar"))
            {
                CargarMapeoEdicion(e);
                OcultarAvisos();
            }
            if (e.CommandName.Equals("Baja"))
            {
                OcultarAvisos();
                DarBajaMapeo(e);
            }
            if (e.CommandName.Equals("Alta"))
            {
                OcultarAvisos();
                DarAltaMapeo(e);
            }
        }
        #endregion

        #region Metodos

        public void CargarGridMapeos()
        {
            GestorMapeoCECOCuentaGasto gestorMapeo = null;
            try
            {
                gestorMapeo = GestorMapeo();
                gvMapeos.DataSource = (from mapeo in gestorMapeo.ListaMapeos()
                                              select new
                                              {
                                                  CODIGO_MAPEO = mapeo.CodigoMapeo,
                                                  PAIS = mapeo.Pais,
                                                  ID_CENTRO = mapeo.CentroCosto,
                                                  CENTRO_COSTO = mapeo.CentroCostoStr,
                                                  ID_ORDEN = mapeo.OrdenCosto,
                                                  ORDEN_COSTO = mapeo.OrdenCostoStr,
                                                  ID_GASTO = mapeo.TipoGasto,
                                                  TIPO_GASTO = mapeo.TipoGastoStr,
                                                  ID_CUENTA = mapeo.CuentaContable,
                                                  CUENTA_CONTABLE = mapeo.CuentaContableStr,
                                                  idAlta2 = mapeo.Alta,
                                                  ID_USUARIOALTA = mapeo.USUARIO_MANTENIMIENTO.USUARIO_ALTA,
                                                  FECHA_ALTA = mapeo.USUARIO_MANTENIMIENTO.FECHA_ALTA,
                                                  ID_USUARIOMODIFICACION = mapeo.USUARIO_MANTENIMIENTO.USUARIO_MODIFICO,
                                                  FECHA_MODIFICACION = mapeo.USUARIO_MANTENIMIENTO.FECHA_MODIFICACION,
                                              }).ToArray();

                gvMapeos.DataBind();


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                DesplegarError("Error al desplegar los registros");
            }
            finally
            {
                if (gestorMapeo != null) gestorMapeo.Dispose();
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

        private void LimpiarControles()
        {

            hfCodigoMapeo.Value = "0";
            DropDownListPais.SelectedIndex = 0;
            DropDownListCentroCosto.SelectedIndex = 0;
            DropDownListOrdenCosto.SelectedIndex = 0;
            DropDownListTipoGasto.SelectedIndex = 0;
            DropDownListCuentaContable.SelectedIndex = 0;
            cbAlta.Checked = false;
            lblFechaModificacionDB.Text = string.Empty;
            lblFechaAltaBD.Text = string.Empty;
            lblUsuarioAltaBD.Text = string.Empty;
            lblUsuarioModificacionBD.Text = string.Empty;
            OcultarAvisos();
        }

        private MapeoCECOCuentaGastoDTO CargarObjetoMapeo(ref MapeoCECOCuentaGastoDTO _mapeoDTO)
        {
            _mapeoDTO.CodigoMapeo = int.Parse(hfCodigoMapeo.Value);
            _mapeoDTO.Pais = DropDownListPais.SelectedValue;
            _mapeoDTO.CentroCosto = DropDownListCentroCosto.SelectedValue;
            _mapeoDTO.OrdenCosto = DropDownListOrdenCosto.SelectedValue;
            _mapeoDTO.TipoGasto = int.Parse(DropDownListTipoGasto.SelectedValue);
            _mapeoDTO.CuentaContable = DropDownListCuentaContable.SelectedValue;
            _mapeoDTO.USUARIO_MANTENIMIENTO.USUARIO_ALTA = usuario;
            _mapeoDTO.USUARIO_MANTENIMIENTO.USUARIO_MODIFICO = usuario;
            _mapeoDTO.Alta = cbAlta.Checked;

            return _mapeoDTO;
        }

        private bool ValidaCampos()
        {
            if (DropDownListPais.SelectedIndex != 0)
            {
                if (DropDownListCentroCosto.SelectedIndex != 0)
                {
                    if (DropDownListOrdenCosto.SelectedIndex != 0)
                    {
                        if (DropDownListTipoGasto.SelectedIndex != 0)
                        {
                            if (DropDownListCuentaContable.SelectedIndex != 0)
                            {
                                result = true;
                            }
                        }
                    }
                }
            }
            return result;
        }

        private void GuardarMapeo()
        {
            MapeoCECOCuentaGastoDTO _mapeoDTO = new MapeoCECOCuentaGastoDTO();
            GestorMapeoCECOCuentaGasto gestorMapeo = null;
            OcultarAvisos();
            try
            {
                if (ValidaCampos())
                {
                    gestorMapeo = GestorMapeo();
                    int codigoGasto = int.Parse(DropDownListTipoGasto.SelectedValue);

                    if (!gestorMapeo.ValidaExisteMapeo(DropDownListPais.SelectedValue, DropDownListCentroCosto.SelectedValue, 
                        DropDownListOrdenCosto.SelectedValue, codigoGasto, DropDownListCuentaContable.SelectedValue))
                    {
                        CargarObjetoMapeo(ref _mapeoDTO);
                        gestorMapeo.AlmacenarMapeo(_mapeoDTO);
                        hfCodigoMapeo.Value = _mapeoDTO.CodigoMapeo.ToString();
                        LimpiarControles();
                        DesplegarAviso("El mapeo fue registrado correctamente");
                        CargarGridMapeos();
                    }
                    else
                        DesplegarError("Ya existe un mapeo registrado con los mismos datos que intenta guardar");
                }
                else
                    DesplegarError("Debe Completar los campos correctamente");
            }
            catch (ExcepcionesDIPCMI ex)
            {
                DesplegarError(ex.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                DesplegarError("Error al registrar el mapeo");
            }
            finally
            {
                if (gestorMapeo != null) gestorMapeo.Dispose();
            }
        }

        private void CargarMapeoEdicion(GridViewCommandEventArgs e)
        {
            int indice = Int32.Parse(e.CommandArgument.ToString());
            GridViewRow fila = gvMapeos.Rows[indice];

            hfCodigoMapeo.Value = HttpUtility.HtmlDecode(fila.Cells[0].Text);
            string pais = HttpUtility.HtmlDecode(fila.Cells[1].Text);

            DropDownListPais.SelectedValue = pais;
            CargarDDLCentroCosto(pais);
            CargarDDLOrdenCosto(pais);
            CargarDDLTipoGastos(pais);
            CargarDDLCuentaContable(pais);

            string idCentro = (((HiddenField)fila.FindControl("ID_CENTRO")).Value);
            string idOrden = (((HiddenField)fila.FindControl("ID_ORDEN")).Value);
            string idTipoGasto = (((HiddenField)fila.FindControl("ID_GASTO")).Value);
            string idCuenta = (((HiddenField)fila.FindControl("ID_CUENTA")).Value);

            DropDownListCentroCosto.SelectedValue = idCentro;
            DropDownListOrdenCosto.SelectedValue = idOrden;
            DropDownListTipoGasto.SelectedValue = idTipoGasto;
            DropDownListCuentaContable.SelectedValue = idCuenta;

            cbAlta.Checked = (((CheckBox)fila.FindControl("idAlta2")).Checked);
            lblUsuarioAltaBD.Text = HttpUtility.HtmlDecode(fila.Cells[6].Text);
            lblFechaAltaBD.Text = HttpUtility.HtmlDecode(fila.Cells[7].Text);
            lblUsuarioModificacionBD.Text = HttpUtility.HtmlDecode(fila.Cells[8].Text);
            lblFechaModificacionDB.Text = HttpUtility.HtmlDecode(fila.Cells[9].Text);
        }

        private void DarBajaMapeo(GridViewCommandEventArgs e)
        {
            int indice = Int32.Parse(e.CommandArgument.ToString());
            GridViewRow fila = gvMapeos.Rows[indice];
            GestorMapeoCECOCuentaGasto gestorMapeo = null;

            try
            {
                gestorMapeo = GestorMapeo();
                gestorMapeo.DarBajaMapeo(Convert.ToInt32(fila.Cells[0].Text), usuario);

                DesplegarAviso("El mapeo fue dado de baja");
                CargarGridMapeos();
            }
            catch
            {
                DesplegarError("Error al dar de baja el mapeo");
            }
            finally
            {
                if (gestorMapeo != null) gestorMapeo.Dispose();
            }
        }

        private void DarAltaMapeo(GridViewCommandEventArgs e)
        {
            int indice = Int32.Parse(e.CommandArgument.ToString());
            GridViewRow fila = gvMapeos.Rows[indice];
            GestorMapeoCECOCuentaGasto gestorMapeo = null;

            try
            {
                gestorMapeo = GestorMapeo();
                gestorMapeo.DarAltaMapeo(Convert.ToInt32(fila.Cells[0].Text), usuario);

                DesplegarAviso("El mapeo se ha dado de alta con exito");
                CargarGridMapeos();
            }
            catch
            {
                DesplegarError("Error al dar de alta el mapeo");
            }
            finally
            {
                if (gestorMapeo != null) gestorMapeo.Dispose();
            }
        }

        private void CargarDDLPais()
        {
            GestorMapeoCECOCuentaGasto gestorMapeo = null;
            try
            {
                gestorMapeo = GestorMapeo();
                List<LlenarDDL_DTO> _listacentroDto = gestorMapeo.ListaPaisesDDL();

                DropDownListPais.Items.Clear();
                DropDownListPais.Items.Add(new ListItem("::ELIJA UN PAIS::", "-1"));
                DropDownListPais.AppendDataBoundItems = true;
                DropDownListPais.DataSource = _listacentroDto;
                DropDownListPais.DataTextField = "DESCRIPCION";
                DropDownListPais.DataValueField = "IDENTIFICADOR";
                DropDownListPais.DataBind();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                DesplegarError("Error al recuperar Paises");
            }
            finally
            {
                if (gestorMapeo != null) gestorMapeo.Dispose();
            }
        }

        private void CargarDDLCentroCosto(string pais)
        {
            GestorMapeoCECOCuentaGasto gestorMapeo = null;
            try
            {
                gestorMapeo = GestorMapeo();
                List<LlenarDDL_DTO> _listacentroDto = gestorMapeo.ListaCentrosCosto(pais);

                DropDownListCentroCosto.Items.Clear();
                DropDownListCentroCosto.Items.Add(new ListItem("::ELIJA UN CENTRO DE COSTO::", "-1"));
                DropDownListCentroCosto.AppendDataBoundItems = true;
                DropDownListCentroCosto.DataSource = _listacentroDto;
                DropDownListCentroCosto.DataTextField = "DESCRIPCION";
                DropDownListCentroCosto.DataValueField = "IDENTIFICADOR";
                DropDownListCentroCosto.DataBind();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                DesplegarError("Error al recuperar los centros de costo");
            }
            finally
            {
                if (gestorMapeo != null) gestorMapeo.Dispose();
            }
        }

        private void CargarDDLOrdenCosto(string pais)
        {
            GestorMapeoCECOCuentaGasto gestorMapeo = null;
            try
            {
                gestorMapeo = GestorMapeo();
                List<LlenarDDL_DTO> _listaOrdenCosto = gestorMapeo.ListaOrdenesCosto(pais);

                DropDownListOrdenCosto.Items.Clear();
                DropDownListOrdenCosto.Items.Add(new ListItem("::ELIJA UNA ORDEN DE COSTO::", "-1"));
                DropDownListOrdenCosto.AppendDataBoundItems = true;
                DropDownListOrdenCosto.DataSource = _listaOrdenCosto;
                DropDownListOrdenCosto.DataTextField = "DESCRIPCION";
                DropDownListOrdenCosto.DataValueField = "IDENTIFICADOR";
                DropDownListOrdenCosto.DataBind();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                DesplegarError("Error al recuperar las ordenes de costo");
            }
            finally
            {
                if (gestorMapeo != null) gestorMapeo.Dispose();
            }
        }

        private void CargarDDLTipoGastos(string pais)
        {
            GestorMapeoCECOCuentaGasto gestorMapeo = null;
            try
            {
                gestorMapeo = GestorMapeo();
                List<LlenarDDL_DTO> _listacentroDto = gestorMapeo.ListaTiposGastos(pais);

                DropDownListTipoGasto.Items.Clear();
                DropDownListTipoGasto.Items.Add(new ListItem("::ELIJA UN TIPO DE GASTO::", "-1"));
                DropDownListTipoGasto.AppendDataBoundItems = true;
                DropDownListTipoGasto.DataSource = _listacentroDto;
                DropDownListTipoGasto.DataTextField = "DESCRIPCION";
                DropDownListTipoGasto.DataValueField = "IDENTIFICADOR";
                DropDownListTipoGasto.DataBind();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                DesplegarError("Error al recuperar Tipos de Gastos");
            }
            finally
            {
                if (gestorMapeo != null) gestorMapeo.Dispose();
            }
        }

        private void CargarDDLCuentaContable(string centro)
        {
            GestorMapeoCECOCuentaGasto gestorMapeo = null;
            try
            {
                gestorMapeo = GestorMapeo();
                List<LlenarDDL_DTO> _listaCuentas = gestorMapeo.ListaCuentasContables(centro);

                DropDownListCuentaContable.Items.Clear();
                DropDownListCuentaContable.Items.Add(new ListItem("::ELIJA UNA CUENTA CONTABLE::", "-1"));
                DropDownListCuentaContable.AppendDataBoundItems = true;
                DropDownListCuentaContable.DataSource = _listaCuentas;
                DropDownListCuentaContable.DataTextField = "DESCRIPCION";
                DropDownListCuentaContable.DataValueField = "IDENTIFICADOR";
                DropDownListCuentaContable.DataBind();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                DesplegarError("Error al recuperar las cuentas contables");
            }
            finally
            {
                if (gestorMapeo != null) gestorMapeo.Dispose();
            }
        }
        #endregion

        #region Gestores

        protected GestorMapeoCECOCuentaGasto GestorMapeo()
        {
            GestorMapeoCECOCuentaGasto gestorMapeo = new GestorMapeoCECOCuentaGasto(cnnApl);
            return gestorMapeo;
        }
        #endregion
    }
}