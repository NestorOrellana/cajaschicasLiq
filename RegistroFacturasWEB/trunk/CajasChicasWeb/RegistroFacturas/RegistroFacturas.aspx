<%@ Page Title="" Language="C#" MasterPageFile="~/Base.Master" AutoEventWireup="true"
    CodeBehind="RegistroFacturas.aspx.cs" Inherits="RegistroFacturasWEB.RegistroFacturas.RegistroFacturas"
    EnableEventValidation="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="../Scripts/jquery-1.10.2.js" type="text/javascript"></script>
    <script src="../Scripts/jquery-1.7.2.min.js" type="text/javascript"></script>
    <script type="text/javascript" src="/Scripts/jquery.min.js"></script>
    <script type="text/javascript" src="/Scripts/jquery.validate.js"></script>
    <script src="../Scripts/jquery-ui.min.js" type="text/javascript"></script>
    <link href="../App_Themes/Tema/jquery-ui.css" rel="stylesheet" type="text/css" />
    <script src="/bootstrap/js/bootstrap.js" type="text/javascript"></script>
    <link href="/bootstrap/css/bootstrap.css" rel="stylesheet" type="text/css" />
    <script src="RegistroFactura.js" type="text/javascript"></script>
    <script type="text/javascript">
        $.validator.addMethod("validTipoDoc", ValidaTipoDoc, "Seleccione el Tipo de Documento");
        $.validator.addMethod("validaCentroCosto", ValidaCentroCosto, "Seleccione el Centro Costo");
        $.validator.addMethod("validaOrdenCosto", ValidaOrdenCosto, "Seleccione Orden de Costo");

        var ddlTipoDocumento = null;
        var txtNumeroIdentificacion = null;
        var txtNombreProveedor = null;
        var hfNombreProveedor = null;
        var hfSumaCompra = null;
        var hfSumaIVA = null;
        var hfIVA = null;
        var lblSumaCompra = null;
        var lblSumaIVA = null;
        var hfCodigoSociedad = null;
        var chDiferentesCO = null;
        var txtTotalFactCO = null;
        var txtTotalAcumulado = null;
        var hfImpuesto = null;
        var hfPais = null;

        $(document).ready(function () {

            var ddlCentroCosto = $('#<%= ddlCentroCosto.ClientID %>');
            var ddlOrdenCosto = $('#<%= ddlOrdenCosto.ClientID %>');
            var ddlTipoDocumento = $('#<%= ddlTipoDocumento.ClientID %>');
            var chImpuestoServ = $('#<%= chImpuestoServ.ClientID %>');
            var txtValorTotal = $('[id*=gvDetalleFactura][id*=txtValorTotal]');
            var txtNumeroFactura = $('#<%= txtNumeroFactura.ClientID %>');

            ddlCentroCosto.bind('change', ddlCentroCostoChange);
            ddlOrdenCosto.bind('change', ddlOrdenCostoChange);
            ddlTipoDocumento.bind('change', ddlTipoDocumentoChange);
            chImpuestoServ.bind('change', chImpuestoServChange);
            txtNumeroFactura.bind('change', txtNumeroFacturaChange);


            $('#<%= txtObservaciones.ClientID %>').keypress(function (e) {
                OcultarMensaje();
                if ($(this).val().length > 150) {
                    DesplegarError("El tamaño de las Observaciones es mayor al permitido");
                } else
                    OcultarMensaje();
            });


            var documentoIdent = $('#<%= ddlTipoDocumento.ClientID %>').text();
            if (documentoIdent == "VALE") {
                $('[id*=gvDetalleFactura][id*=ddlTipoIVA]').prop("disabled", true);
            }


            $('#<%=txtNumeroIdentificacion.ClientID %>').blur(function () {
                OcultarMensaje();
                var proveedor = null;
                $('#<%=txtNombreProveedor.ClientID %>').val('')
                $('#<%=hfIdProveedor.ClientID %>').val(0);

                if ($('#<%=ddlTipoDocumento.ClientID %>').val() == '-1') {
                    DesplegarError("Debe seleccionar un tipo de documento de identificacion.");
                    return;
                }

                if ($('#<%=txtNumeroIdentificacion.ClientID %>').val() == '') {
                    return;
                }


                var url = '../../RegistroFacturas/MapeoUsuariosCentros.asmx/BuscarProveedor';
                var data = '{idTipoDocumento: ' + $('#<%= ddlTipoDocumento.ClientID %>').val() + ', numeroIdentificacion: "' + $('#<%= txtNumeroIdentificacion.ClientID%>').val() + '" }';

                $.ajax({
                    type: 'POST',
                    url: url,
                    data: data,
                    contentType: 'application/json; charset=utf-8',
                    dataType: 'json',
                    success: function (response) {
                        proveedor = (typeof response.d) == 'string' ? eval('(' + response.d + ')') : response.d;

                        if (proveedor != null) {
                            $('#<%= hfIdProveedor.ClientID %>').val(proveedor.ID_PROVEEDOR);
                            $('#<%= txtNombreProveedor.ClientID %>').val(proveedor.NOMBRE);

//                            if (($('#<%= hfCodigoSociedad.ClientID %>').val() == '1440') || ($('#<%= hfCodigoSociedad.ClientID %>').val() == '1630')) {
                              if ($('#<%= hfPais.ClientID %>').val() == 'CR'){
                                $('[id*=gvDetalleFactura][id*=txtDescripcion]').prop("disabled", true)
                                $('[id*=gvDetalleFactura][id*=txtDescripcion]').val($('#<%= txtNumeroFactura.ClientID %>').val() + " " + $('#<%= txtNombreProveedor.ClientID %>').val());
                            }
                        }
                        else {
                            $('#<%= txtNombreProveedor.ClientID%>').val('');
                            var pruebasoc = $('#<%= hfCodigoSociedad.ClientID %>').val();
                            if (($('#<%= hfCodigoSociedad.ClientID %>').val() == '1240') || ($('#<%= hfCodigoSociedad.ClientID %>').val() == '1250') ||
                              ($('#<%= hfCodigoSociedad.ClientID %>').val() == '1270') || ($('#<%= hfCodigoSociedad.ClientID %>').val() == '1550') || ($('#<%= hfCodigoSociedad.ClientID %>').val() == '1570')) {
                                DesplegarError("El proveedor no existe, solicite la creación para continuar");
                                return;
                            } else {
                                LlamarVentanaProveedor();
                            }
                        }
                    },
                    error: function (request, status, error) {
                        //DesplegarError(JSON.parse(request.responseText).Message)
                        DesplegarError("Error al buscar el proveedor.");
                    }
                });

            });



            $('#btnNuevaFactura').click(function () {
                NuevaFactura();
            });

            function LlamarVentanaProveedor() {

                $('#popuporderedit').dialog({
                    autoOpen: false,
                    modal: true,
                    resizable: false,
                    width: 500,
                    heigth: 250,
                    title: 'Registro de proveedores:',
                    open: function (event, ui) {

                        initialize();
                    },
                    close: function (event, ui) {

                        //limpia todos los textbox del popup
                        $('#popuporderedit :text').val('');
                    },
                    buttons: {
                        Almacenar: function () {
                            OcultarMensaje();
                            AlmacenarProveedor();
                        },
                        Cancelar: function () {
                            $(this).dialog("close");
                            return;
                        }
                    }
                });

                $('#popuporderedit').dialog('open');
                return false;
            }

            function AlmacenarProveedor() {


                var proveedorDto = new Object();

                proveedorDto.ID_TIPO_DOCUMENTO = $('#ddlProveedor').val();
                proveedorDto.NUMERO_IDENTIFICACION = $('#txtIdentificacion').val();
                proveedorDto.NOMBRE = $('#txtNombre').val();
                proveedorDto.DIRECCION = $('#txtDireccion').val();
                proveedorDto.ES_PEQUEÑO_CONTRIBUYENTE = $('#cbPC').is(':checked') ? true : false;
                proveedorDto.NUMERO_IDENTIFICACION2 = $('#txtIdentificacion2').val();
                proveedorDto.ID_TIPO_DOCUMENTO2 = $('#ddlProveedor2').val();
                proveedorDto.REGIMEN = $('#ddlRegimen').val();
                proveedorDto.TIPO = $('#chTipo').is(':checked') ? true : false;


                if (($.trim(proveedorDto.REGIMEN) == '') || (proveedorDto.REGIMEN == null) || (proveedorDto.REGIMEN == '-1')) {
                    proveedorDto.REGIMEN = '0';
                }


                if (($.trim(proveedorDto.NUMERO_IDENTIFICACION2) == '') || (proveedorDto.NUMERO_IDENTIFICACION2 == null)) {
                    proveedorDto.NUMERO_IDENTIFICACION2 = '';
                }

                if (($.trim(proveedorDto.ID_TIPO_DOCUMENTO2) == '') || (proveedorDto.ID_TIPO_DOCUMENTO2 == null) || (proveedorDto.ID_TIPO_DOCUMENTO2 == '-1')) {
                    proveedorDto.ID_TIPO_DOCUMENTO2 = '0';
                }

                if (proveedorDto.ID_TIPO_DOCUMENTO == '-1') {
                    alert("Debe seleccionar un tipo de documento de identificación");
                    return;
                }

                if (($.trim(proveedorDto.NUMERO_IDENTIFICACION) == '') || (proveedorDto.NUMERO_IDENTIFICACION == null)) {
                    alert("Debe escribir el número de identificación del proveedor.");
                    return;
                }

                if (($.trim(proveedorDto.NOMBRE) == '') || (proveedorDto.NOMBRE == null)) {
                    alert("Debe escribir el nombre del proveedor.");
                    return;
                }

                if (($.trim(proveedorDto.DIRECCION) == '') || (proveedorDto.DIRECCION == null)) {
                    alert("Debe escribir la dirección del proveedor.");
                    return;
                }

                if (/\s/.test(proveedorDto.NUMERO_IDENTIFICACION)) {
                    alert("No se permiten espacios en blanco en el número de identificación");
                    return;
                }

                var params = new Object();
                params.proveedorDto = proveedorDto;

                $.ajax({
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    url: "../../RegistroFacturas/MapeoUsuariosCentros.asmx/AlmacenarProveedor",
                    data: JSON.stringify(params),
                    dataType: "json",
                    async: true,
                    success: function (response) {
                        var proveedor = (typeof response.d) == 'string' ? eval('(' + response.d + ')') : response.d;

                        if (proveedor.CODIGO == 0) {
                            $("#<%= hfIdProveedor.ClientID%>").val('0');
                            DesplegarError(proveedor.MENSAJE);
                        }
                        else {
                            $('#<%= hfIdProveedor.ClientID%>').val(proveedor.CODIGO);
                            $('#<%= txtNumeroIdentificacion.ClientID%>').val($('#txtIdentificacion').val());
                            $('#<%= txtNumeroIdentificacion.ClientID%>').focus();
                            $('#<%= txtNombreProveedor.ClientID%>').val($('#txtNombre').val())
                            DesplegarMensaje('Se Almaceno correctamente al proveedor');
                        }

                        $('#popuporderedit').dialog('close');
                    },
                    error: function (request, status, error) {
                        DesplegarError("Error al almacenar el error.");
                    }
                });
            }

            function ddlCentroCostoChange() {
                OcultarMensaje();
                if ((ddlCentroCosto.val() > 0) && (ddlOrdenCosto.val() > 0)) {
                    ddlCentroCosto.val('-1');
                    ddlOrdenCosto.val('-1');

                    DesplegarError("Debe seleccionar únicamente orden de costo o centro de costo");
                }
                else {

                    hfCodigoSociedad = $('#<%= hfCodigoSociedad.ClientID %>');
                    var url = '../../RegistroFacturas/MapeoUsuariosCentros.asmx/BuscarCuentasContablesCentroCosto';
                    //var data = '{codigoSociedad:"' + hfCodigoSociedad.val() + '"}';
                    var data = '{centroCosto:"' + ddlCentroCosto.val() + '",codigoSociedad:"' + hfCodigoSociedad.val() + '"}';

                    $.ajax({
                        type: 'POST',
                        url: url,
                        data: data,
                        contentType: 'application/json; charset=utf-8',
                        dataType: 'json',
                        success: function (response) {
                            var cuentaContable = (typeof response.d) == 'string' ? eval('(' + response.d + ')') : response.d;
                            LlenarCuentasContables(cuentaContable);
                        },
                        error: function (request, status, error) {
                            DesplegarError(JSON.parse(request.responseText).Message)
                        }
                    });
                }
            }

            function LlenarCuentasContables(cuentaContable) {
                $('#<%=gvDetalleFactura.ClientID%> tr:not(:first)').each(function () {

                    $(this).closest('tr').find('[id*=lblCuentaContable]').text('');
                    $(this).closest('tr').find('[id*=lblDefinicionCC]').text('');

                    ddlCuentaContable = $(this).closest('tr').find('[id*=ddlCuentaContable]');
                    ddlCuentaContable.find('option').remove();

                    //Append default option
                    ddlCuentaContable.attr('disabled', false).append($('<option></option>').attr('value', '').text('::Seleccione cuenta contable::'));
                    var doc = $('<div></div>');
                    for (var i = 0; i < cuentaContable.length; i++) {
                        doc.append($('<option></option>').attr('value', cuentaContable[i].IDENTIFICADOR).text(cuentaContable[i].DESCRIPCION));
                    }
                    ddlCuentaContable.append(doc.html());
                    doc.remove();
                });
            }

            function ddlOrdenCostoChange() {
                OcultarMensaje();
                if ((ddlCentroCosto.val() > 0) && (ddlOrdenCosto.val() > 0)) {
                    ddlCentroCosto.val("-1");
                    ddlOrdenCosto.val("-1");

                    DesplegarError("Debe seleccionar únicamente orden de costo o centro de costo");
                }
                else {
                    hfCodigoSociedad = $('#<%= hfCodigoSociedad.ClientID %>');
                    var url = '../../RegistroFacturas/MapeoUsuariosCentros.asmx/BuscarCuentasContablesOrdenCosto'; ;
                    var data = '{ordenCompra:"' + ddlOrdenCosto.val() + '",codigoSociedad:"' + hfCodigoSociedad.val() + '"}';

                    $.ajax({
                        type: 'POST',
                        url: url,
                        data: data,
                        contentType: 'application/json; charset=utf-8',
                        dataType: 'json',
                        success: function (response) {
                            var cuentaContable = (typeof response.d) == 'string' ? eval('(' + response.d + ')') : response.d;
                            LlenarCuentasContables(cuentaContable);
                        },
                        error: function (request, status, error) {
                            //DesplegarError(JSON.parse(request.responseText).Message)
                            DesplegarError("Error al buscar cuentas contables.");
                        }
                    });
                }
            }

            $('#<%=txtNumeroFactura.ClientID %>').blur(function () {
                OcultarMensaje();
                if (($('#<%= hfIdProveedor.ClientID %>').val() != "") && ($('#<%= txtNumeroFactura.ClientID%>').val() != "")) {
                    OcultarMensaje();
                    var TextBoxTotal = document.getElementById('<%= txtTotalFactCO.ClientID %>');
                    //                chDiferentesCO = $('#<%= chDiferentesCO.ClientID %>');

                    var url = '../../RegistroFacturas/MapeoUsuariosCentros.asmx/BuscarTotalFactura';
                    var data = '{IdProveedor:' + $('#<%= hfIdProveedor.ClientID %>').val() + ',NoFactura:"' + $('#<%= txtNumeroFactura.ClientID%>').val() + '",serie:"' + $('#<%= txtSerie.ClientID%>').val() + '"}';

                    $.ajax({
                        type: 'POST',
                        url: url,
                        data: data,
                        contentType: 'application/json; charset=utf-8',
                        dataType: 'json',
                        success: function (response) {
                            var TotalFActura = (typeof response.d) == 'string' ? eval('(' + response.d + ')') : response.d;
                            if (TotalFActura != null) {

                                if (TotalFActura.ID_CAJA_CHICA == $('#<%=hfIdCajaChica.ClientID %>').val()) {
                                    $('#<%= txtTotalFactCO.ClientID %>').val(TotalFActura.TOTALFACTURADIVIDIDA);
                                    $('#<%= hfAcumulado.ClientID %>').val(TotalFActura.ACUMULADO);
                                    $('#<%= lblAcumulado.ClientID %>').text("Acumulado Factura Dividida:" + $('#<%= hfAcumulado.ClientID %>').val());

                                    $('#<%= txtFechaFactura.ClientID %>').prop("disabled", true);
                                    $('#<%= txtTotalFactCO.ClientID %>').prop("disabled", true);

                                    var fecha = TotalFActura.OBSERVACIONES;
                                    $('#<%= txtFechaFactura.ClientID %>').val(fecha);

                                    $('#<%= chDiferentesCO.ClientID %>').prop("checked", TotalFActura.FACTURA_DIVIDIDA);
                                    $('#<%= chDiferentesCO.ClientID %>').prop("disabled", true);

                                } else {
                                    DesplegarError("La Factura se encuentra registrada en otra caja chica");
                                    $('#<%= txtTotalFactCO.ClientID %>').prop("disabled", true);
                                    $('#<%= txtFechaFactura.ClientID %>').prop("disabled", false);
                                    $('#<%= chDiferentesCO.ClientID %>').prop("checked", false);
                                    $('#<%= chDiferentesCO.ClientID %>').prop("disabled", false);
                                    $('#<%= txtTotalFactCO.ClientID %>').val('0');
                                    $("#<%= txtTotalFactCO.ClientID %>").text($('#<%=txtTotalFactCO.ClientID %>').val());
                                    $('#<%= txtFechaFactura.ClientID %>').val('');
                                    $('#<%= lblAcumulado.ClientID %>').text('');
                                }
                            } else {
                                $('#<%= txtTotalFactCO.ClientID %>').prop("disabled", false);
                                $('#<%= txtFechaFactura.ClientID %>').prop("disabled", false);
                                $('#<%= chDiferentesCO.ClientID %>').prop("checked", false);
                                $('#<%= chDiferentesCO.ClientID %>').prop("disabled", false);
                                $('#<%= txtTotalFactCO.ClientID %>').val('0');
                                $("#<%= txtTotalFactCO.ClientID %>").text($('#<%=txtTotalFactCO.ClientID %>').val());
                                $('#<%= txtFechaFactura.ClientID %>').val('');
                                $('#<%= lblAcumulado.ClientID %>').text('');
                            }


                        },
                        error: function (request, status, error) {
                            DesplegarError(JSON.parse(request.responseText).Message)
                        }

                    });
                } else {
                    DesplegarError("Debe completar datos de factura y proveedor.");
                    $('#<%= chDiferentesCO.ClientID %>').prop("checked", "");
                }

            });





            $('#<%=txtFechaFactura.ClientID%>').datepicker({
                dateFormat: 'yy/mm/dd'
            });

            $('[id*=ddlTipoDocumento]').live('change', function (e) {
                OcultarMensaje();
            });

            $('[id*=gvDetalleFactura][id*=ddlTipoIVA]').live('change', function (e) {

                hfIVA = null;
                var valorTI = $(e.target).closest('tr').find('[id*=ddlTipoIVA]').val();
                var ajaxUrlForChild1 = '../../RegistroFacturas/MapeoUsuariosCentros.asmx/BuscarImporteIVA';
                var Soc = $('#<%= hfCodigoSociedad.ClientID %>').val();
                doAjaxCall1(ajaxUrlForChild1, '{indicadorIVA:"' + valorTI + '", Sociedad: "' + Soc + '"}', e);
            });

            $('[id*=gvDetalleFactura][id*=ddlCuentaContable]').live('change', function (e) {
                var valorCC = $(e.target).closest('tr').find('[id*=ddlCuentaContable]').val();
                var definicionCC = $('option:selected', $(this)).text();

                $(e.target).closest('tr').find('[id*=lblCuentaContable]').text(valorCC);
                $(e.target).closest('tr').find('[id*=lblDefinicionCC]').text(definicionCC);

                $(e.target).closest('tr').find('input[type=hidden][id*=hfDefinicionCC]').val(definicionCC);
                $(e.target).closest('tr').find('input[type=hidden][id*=hfCuentaContable]').val(valorCC);
            });

            $('[id*=gvDetalleFactura]input[type=text][id*=txtValorTotal]').live('keyup', function (e) {
                //var impuesto = $(e.target).closest('tr').find('input[type=text][id*=txtImpuesto]').val();
                var valorTotal = $(this).closest('tr').find('input[type=text][id*=txtValorTotal]').val();
                var tipoIVA = $(e.target).closest('tr').find('[id*=ddlTipoIVA]').val();

                if (hfImpuesto == "0" || hfImpuesto == null) {
                    var impuesto = $(e.target).closest('tr').find('input[type=text][id*=txtImpuesto]').val();
                } else {

                    var cantidad = $(e.target).closest('tr').find('input[type=text][id*=txtCantidad]').val();
                    var impuesto = cantidad * hfImpuesto.valueOf();
                    $(this).closest('tr').find('input[type=text][id*=txtImpuesto]').val(impuesto.toFixed(2));
                }
                OcultarMensaje();
                CalcularIVA(e, impuesto, valorTotal, tipoIVA);
                SumarCompras();
                SumarIVA();
            });

            $('[id*=gvDetalleFactura]input[type=text][id*=txtImpuesto]').live('keyup', function (e) {
                //  var impuesto = $(this).closest('tr').find('input[type=text][id*=txtImpuesto]').val();
                var valorTotal = $(e.target).closest('tr').find('input[type=text][id*=txtValorTotal]').val();
                var tipoIVA = $(e.target).closest('tr').find('[id*=ddlTipoIVA]').val();

                if (hfImpuesto == "0" || hfImpuesto == null) {
                    var impuesto = $(e.target).closest('tr').find('input[type=text][id*=txtImpuesto]').val();
                } else {

                    var cantidad = $(e.target).closest('tr').find('input[type=text][id*=txtCantidad]').val();
                    var impuesto = cantidad * hfImpuesto.valueOf();
                    $(this).closest('tr').find('input[type=text][id*=txtImpuesto]').val(impuesto.toFixed(2));
                }

                OcultarMensaje();
                CalcularIVA(e, impuesto, valorTotal, tipoIVA);

                SumarCompras();
                SumarIVA();
            });

            $('#<%=btnAgregarFila.ClientID %>').bind('click', function (event) {
                var $grid = $('#<%=gvDetalleFactura.ClientID %>');
                var $row = $grid.find('tr:last').clone().appendTo($grid);
                SumarCompras();
                SumarIVA();
                return false;
            });


            $('[id*=gvDetalleFactura][id*=ddlImpuestos]').live('change', function (e) {
                hfImpuesto = $(e.target).closest('tr').find('[id*=ddlImpuestos]').val();
                //DesplegarError(hfImpuesto.valueOf());
                if (hfImpuesto == "0" || hfImpuesto == null) {
                    $(this).closest('tr').find('input[type=text][id*=txtImpuesto]').prop("disabled", false);
                    $(this).closest('tr').find('input[type=text][id*=txtImpuesto]').val(0);
                } else {
                    $(this).closest('tr').find('input[type=text][id*=txtImpuesto]').prop("disabled", true);
                    var cantidad = $(e.target).closest('tr').find('input[type=text][id*=txtCantidad]').val();
                    var impuesto = cantidad * hfImpuesto.valueOf();
                    $(this).closest('tr').find('input[type=text][id*=txtImpuesto]').val(impuesto.toFixed(2));

                }

            });

            $('[id*=gvDetalleFactura]input[type=text][id*=txtCantidad]').live('keyup', function (e) {
                if (hfImpuesto == "0" || hfImpuesto == null) {
                    var impuesto = $(e.target).closest('tr').find('input[type=text][id*=txtImpuesto]').val();
                } else {

                    var cantidad = $(e.target).closest('tr').find('input[type=text][id*=txtCantidad]').val();
                    var impuesto = cantidad * hfImpuesto.valueOf();
                    $(this).closest('tr').find('input[type=text][id*=txtImpuesto]').val(impuesto.toFixed(2));
                }
            });

        });


        function InicializarIdFactura() {
            $('#<%=hfIdFactura.ClientID %>').val('0');
            $('#<%=hfImpuesto.ClientID %>').val('0');
        }

        function InicializarFactura() {
            //Resto el Total de la Factura con la Suma de lo Acumulado y lo ingresado, para validar si aun tiene pendiente de ingresar mas facturas 
            var total = (parseFloat($('#<%= txtTotalFactCO.ClientID %>').val())) - ((parseFloat($('#<%= hfAcumulado.ClientID %>').val())) + (parseFloat($('#<%= lblSumaCompra.ClientID %>').text())));
            $('#<%=hfIdFactura.ClientID %>').val('0');
            $('#<%=hfImpuesto.ClientID %>').val('0');
            hfImpuesto = 0;
            $("#<%= lblIdFactura.ClientID %>").text($('#<%=hfIdFactura.ClientID %>').val());
            //$("#<%= ddlCentroCosto.ClientID%>").val("-1");
            //$("#<%= ddlOrdenCosto.ClientID %>").val("-1");
            //$("#<%= ddlTipoDocumento.ClientID %>").val("-1");
            //$("#<%= txtNumeroIdentificacion.ClientID %>").val("");
            //$("#<%= txtNombreProveedor.ClientID %>").val("");

            $('#<%= txtFechaFactura.ClientID %>').val('');
            //$('#<%= lblSumaCompra.ClientID %>').val('0.00');
            //$('#<%= lblSumaIVA.ClientID %>').val('0.00');
            $('#<%= txtTotalFactCO.ClientID %>').val('');
            // $('#<%= cbFacturaEspecial.ClientID %>').attr('checked', false);
            $('#<%= txtObservaciones.ClientID %>').val('');
            $('#<%= hfAcumulado.ClientID %>').val('0');
            $('#<%= lblAcumulado.ClientID %>').text($('#<%= hfAcumulado.ClientID %>').val());
            $('#<%= txtTotalFactCO.ClientID %>').prop("disabled", true);
            $('#<%= txtFechaFactura.ClientID %>').prop("disabled", false);
            $('#<%= txtFechaFactura.ClientID %>').val('');
            $('#<%= lblAcumulado.ClientID %>').text('');
            $('#<%= txtValReal.ClientID %>').val('');
            $('[id*=gvDetalleFactura][id*=txtImpuesto]').val('');
            $('[id*=gvDetalleFactura][id*=txtImpuesto]').prop("disabled", false);
//            $('[id*=gvDetalleFactura][id*=txtDescripcion]').prop("disabled", false);
            $('#<%= txtTCambio.ClientID %>').val('');
            $('#<%= chImpuestoServ.ClientID %>').prop('checked', false);

            $('[id*=gvDetalleFactura][id*=txtDetalle]').val('');

            var marcaFE = ($('#<%= cbFacturaEspecial.ClientID %>').prop("checked")) ? true : false;
            var marcado = ($('#<%= chDiferentesCO.ClientID %>').prop("checked")) ? true : false;
            if (marcado == false) {
                $('#<%= txtSerie.ClientID%>').val('');
                $('#<%= txtNumeroFactura.ClientID %>').val('');
                $('#<%= chDiferentesCO.ClientID %>').attr('checked', false);
                $('#<%= chDiferentesCO.ClientID %>').prop("disabled", false);
                $('#<%= cbFacturaEspecial.ClientID %>').prop('checked', false);
                $('#<%= chRetencion.ClientID %>').prop('checked', false);
                $('[id*=gvDetalleFactura][id*=txtDescripcion]').prop("disabled", false);
                $('[id*=gvDetalleFactura][id*=txtDescripcion]').val('');
                $('#<%= txtValReal.ClientID %>').val('0');

            } else if (total < 1) {
                $('#<%= txtSerie.ClientID%>').val('');
                $('#<%= txtNumeroFactura.ClientID %>').val('');
                $('#<%= chDiferentesCO.ClientID %>').attr('checked', false);
                $('#<%= chDiferentesCO.ClientID %>').prop("disabled", false);
                $('#<%= cbFacturaEspecial.ClientID %>').prop('checked', false);
                $('#<%= chRetencion.ClientID %>').prop('checked', false);
            }



            if ((marcado == true) && (total > 0)) {
                if (($('#<%= hfIdProveedor.ClientID %>').val() != "") && ($('#<%= txtNumeroFactura.ClientID%>').val() != "")) {
                    OcultarMensaje();
                    var TextBoxTotal = document.getElementById('<%= txtTotalFactCO.ClientID %>');

                    var url = '../../RegistroFacturas/MapeoUsuariosCentros.asmx/BuscarTotalFactura';
                    var data = '{IdProveedor:' + $('#<%= hfIdProveedor.ClientID %>').val() + ',NoFactura:"' + $('#<%= txtNumeroFactura.ClientID%>').val() + '",serie:"' + $('#<%= txtSerie.ClientID%>').val() + '"}';

                    $.ajax({
                        type: 'POST',
                        url: url,
                        data: data,
                        contentType: 'application/json; charset=utf-8',
                        dataType: 'json',
                        success: function (response) {
                            var TotalFActura = (typeof response.d) == 'string' ? eval('(' + response.d + ')') : response.d;
                            if (TotalFActura != null) {

                                if (TotalFActura.ID_CAJA_CHICA == $('#<%=hfIdCajaChica.ClientID %>').val()) {
                                    $('#<%= txtTotalFactCO.ClientID %>').val(TotalFActura.TOTALFACTURADIVIDIDA);
                                    $('#<%= hfAcumulado.ClientID %>').val(TotalFActura.ACUMULADO);
                                    $('#<%= lblAcumulado.ClientID %>').text("Acumulado Factura Dividida:" + $('#<%= hfAcumulado.ClientID %>').val());

                                    $('#<%= txtFechaFactura.ClientID %>').prop("disabled", true);
                                    $('#<%= txtTotalFactCO.ClientID %>').prop("disabled", true);

                                    var fecha = TotalFActura.OBSERVACIONES;
                                    $('#<%= txtFechaFactura.ClientID %>').val(fecha);

                                    $('#<%= chDiferentesCO.ClientID %>').prop("checked", TotalFActura.FACTURA_DIVIDIDA);
                                    $('#<%= chDiferentesCO.ClientID %>').prop("disabled", true);

                                } else {
                                    DesplegarError("La Factura se encuentra registrada en otra caja chica");
                                    $('#<%= txtTotalFactCO.ClientID %>').prop("disabled", true);
                                    $('#<%= txtFechaFactura.ClientID %>').prop("disabled", false);
                                    $('#<%= chDiferentesCO.ClientID %>').prop("checked", false);
                                    $('#<%= chDiferentesCO.ClientID %>').prop("disabled", false);
                                    $('#<%= chRetencion.ClientID %>').prop('checked', false);
                                    $('#<%= txtTotalFactCO.ClientID %>').val('0');
                                    $("#<%= txtTotalFactCO.ClientID %>").text($('#<%=txtTotalFactCO.ClientID %>').val());
                                    $('#<%= txtFechaFactura.ClientID %>').val('');
                                    $('#<%= lblAcumulado.ClientID %>').text('');
                                }
                            } else {
                                $('#<%= txtTotalFactCO.ClientID %>').prop("disabled", false);
                                $('#<%= txtFechaFactura.ClientID %>').prop("disabled", false);
                                $('#<%= chDiferentesCO.ClientID %>').prop("checked", false);
                                $('#<%= chDiferentesCO.ClientID %>').prop("disabled", false);
                                $('#<%= chRetencion.ClientID %>').prop('checked', false);
                                $('#<%= txtTotalFactCO.ClientID %>').val('0');
                                $("#<%= txtTotalFactCO.ClientID %>").text($('#<%=txtTotalFactCO.ClientID %>').val());
                                $('#<%= txtFechaFactura.ClientID %>').val('');
                                $('#<%= lblAcumulado.ClientID %>').text('');
                            }


                        },
                        error: function (request, status, error) {
                            DesplegarError(JSON.parse(request.responseText).Message)
                        }

                    });
                } else {
                    DesplegarError("Debe completar datos de factura y proveedor.");
                    $('#<%= chDiferentesCO.ClientID %>').prop("checked", "");
                }

            } else {
                $('[id*=gvDetalleFactura][id*=txtDescripcion]').prop("disabled", false);
                $('[id*=gvDetalleFactura][id*=txtDescripcion]').val('');
                $('#<%= txtValReal.ClientID %>').val('');
            }


            var $grid = $('#<%=gvDetalleFactura.ClientID %>');
            var $row = $grid.find('tr:last').clone().appendTo($grid);

            $('#<%=gvDetalleFactura.ClientID%> tr:not(:first)').each(function () {
                $('td:eq(0)').html('0');
                $('td:eq(1)').html('0');
              //  $(this).find('input[type=text][id*=txtDescripcion]').val('');
                $(this).find('input[type=text][id*=txtImpuesto]').val('0');
                $(this).closest('tr').find('[id*=lblCuentaContable]').html('');
                $(this).closest('tr').find('[id*=lblDefinicionCC]').html('');
                $(this).find('input[type=text][id*=txtCantidad]').val('0');
                $(this).find('span[id*=lblIVA]').html('0');
                $(this).find('input[type=text][id*=txtValorTotal]').val('0');
                $(this).find('span[id*=lblCuentaContable]').html('');
                $(this).find('span[id*=lblDefinicionCC]').html('');
                $(this).find('span[id*=lblTipoIva]').html('');
                $(this).find('input[type=hidden][id*=hfImporte]').val('0');
            });

            $('#<%=gvDetalleFactura.ClientID%> > tbody > tr').not(':first').not(':last').addClass('highlight').each(function () {
                var img = $(this);
                var row = $(this).closest('tr');
                $(row).remove();
                SumarCompras();
                SumarIVA();
            });
        }


        function ConstruirObjetoJason() {
            var facturaEncabezadoDto = new Object();

            var acumulado;
            var sumaCompra;
            facturaEncabezadoDto.ID_FACTURA = $('#<%=hfIdFactura.ClientID %>').val();
            facturaEncabezadoDto.CENTRO_COSTO = $('#<%=ddlCentroCosto.ClientID %>').val() == '-1' ? '' : $('#<%=ddlCentroCosto.ClientID %>').val();
            facturaEncabezadoDto.ORDEN_COSTO = $('#<%=ddlOrdenCosto.ClientID %>').val() == '-1' ? '' : $('#<%=ddlOrdenCosto.ClientID %>').val();
            facturaEncabezadoDto.SERIE = $('#<%=txtSerie.ClientID %>').val();
            facturaEncabezadoDto.NUMERO = $('#<%=txtNumeroFactura.ClientID %>').val();
            facturaEncabezadoDto.FECHA_FACTURA = $('#<%=txtFechaFactura.ClientID %>').val();
            facturaEncabezadoDto.ES_ESPECIAL = $('#<%=cbFacturaEspecial.ClientID %>').is(':checked') ? true : false;
            facturaEncabezadoDto.IVA = $('#<%=lblSumaIVA.ClientID %>').text();
            facturaEncabezadoDto.VALOR_TOTAL = $('#<%= lblSumaCompra.ClientID %>').text();
            facturaEncabezadoDto.ESTADO = $('#<%=hfEstado.ClientID %>').val() == 0 ? null : $('#<%=hfEstado.ClientID %>').val
            facturaEncabezadoDto.ID_PROVEEDOR = $('#<%=hfIdProveedor.ClientID %>').val();
            facturaEncabezadoDto.ID_CAJA_CHICA = $('#<%=hfIdCajaChica.ClientID %>').val();
            facturaEncabezadoDto.CODIGO_SOCIEDAD = $('#<%=hfCodigoSociedad.ClientID %>').val();
            facturaEncabezadoDto.FACTURA_DIVIDIDA = $('#<%=chDiferentesCO.ClientID %>').is(':checked') ? true : false;
            facturaEncabezadoDto.TOTALFACTURADIVIDIDA = $('#<%= txtTotalFactCO.ClientID %>').val();
            facturaEncabezadoDto.OBSERVACIONES = $('#<%= txtObservaciones.ClientID %>').val();
            facturaEncabezadoDto.RETENCION = $('#<%= chRetencion.ClientID %>').is(':checked') ? true : false;
            acumulado = parseFloat($('#<%=hfAcumulado.ClientID %>').val());
            sumaCompra = parseFloat($('#<%=lblSumaCompra.ClientID %>').text());
            facturaEncabezadoDto.VALOR_REAL_FACT = $('#<%= txtValReal.ClientID %>').val();
            facturaEncabezadoDto.TIPO_CAMBIO = $('#<%= txtTCambio.ClientID %>').val();
            facturaEncabezadoDto.RETSERVICIOS = $('#<%=chImpuestoServ.ClientID %>').is(':checked') ? true : false;
            facturaEncabezadoDto.ACUMULADO = acumulado + sumaCompra;
            facturaEncabezadoDto.PAIS = $('#<%=hfPais.ClientID %>').val();
            //            facturaEncabezadoDto.ACUMULADO = parseInt($('#<%=hfAcumulado.ClientID %>').val()) + parseInt($('#<%=lblSumaCompra.ClientID %>').text());


            var facDetalle = [];
            var FACTURA_DETALLE = new Object();
            var numero = 1;

            $('#<%=gvDetalleFactura.ClientID%> tr:not(:first)').each(function () {
                facDetalle.push({
                    'ID_FACTURA_DETALLE': $('td:eq(1)').html(),
                    'DESCRIPCION': $(this).find('input[type=text][id*=txtDescripcion]').val().substring(0,50),
                    'NUMERO': numero++,
                    'CANTIDAD': $(this).find('input[type=text][id*=txtCantidad]').val(),
                    'IVA': $(this).find('span[id*=lblIVA]').html(),
                    'VALOR': $(this).find('input[type=text][id*=txtValorTotal]').val(),
                    'CUENTA_CONTABLE': $(this).find('span[id*=lblCuentaContable]').html(),
                    'DEFINICION_CC': $(this).find('span[id*=lblDefinicionCC]').html(),
                    'IDENTIFICADOR_IVA': $(this).find('span[id*=lblTipoIva]').html(),
                    'IMPORTE': $(this).find('input[type=hidden][id*=hfImporte]').val(),
                    'IMPUESTO': $(this).find('input[type=text][id*=txtImpuesto]').val(),
                    'DETALLE': $(this).find('input[type=text][id*=txtDetalle]').val()
                });
            });

            FACTURA_DETALLE = facDetalle;

            facturaEncabezadoDto.FACTURA_DETALLE = FACTURA_DETALLE;
            return facturaEncabezadoDto;
        }

        function deleteRow(imageElement) {
            OcultarMensaje();
            var totalRows = $('#<%=gvDetalleFactura.ClientID %> tr').length;

            if (totalRows <= 2) {
                DesplegarError('No se puede eliminar la fila');
                return;
            }

            var img = $(imageElement);
            var row = $(imageElement).closest('tr');
            $(row).remove();
            SumarCompras();
            SumarIVA();
        }

        function initialize() {

            var combo = $('#ddlProveedor');
            var combo2 = $('#ddlProveedor2');
            var combo3 = $('#ddlRegimen');

            $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: "../../RegistroFacturas/MapeoUsuariosCentros.asmx/BuscarTipoDocumento",
                data: '{IdSociedadCentro:"' + $('#<%=hfSociedadCentro.ClientID %>').val() + '" }',
                dataType: "json",
                async: true,
                success: function (response) {
                    var centro = (typeof response.d) == 'string' ? eval('(' + response.d + ')') : response.d;
                    combo.find('option').remove();
                    combo2.find('option').remove();

                    //Append default option
                    combo.attr('disabled', false).append($('<option></option>').attr('value', "").text('::Tipo documento::'));
                    var doc = $('<div></div>');
                    for (var i = 0; i < centro.length; i++) {
                        doc.append($('<option></option>').attr('value', centro[i].ID_TIPO_DOCUMENTO).text(centro[i].DESCRIPCION));
                    }
                    combo.append(doc.html());
                    doc.remove();

                    combo.val($("#<%=ddlTipoDocumento.ClientID %>").val());
                    $('#txtIdentificacion').attr("value", ($('#<%=txtNumeroIdentificacion.ClientID %>').val()));

                    //Llenar combo de tipo de documento 2 
                    combo2.attr('disabled', false).append($('<option></option>').attr('value', "").text('::Tipo documento::'));
                    var doc2 = $('<div></div>');
                    for (var i = 0; i < centro.length; i++) {
                        doc2.append($('<option></option>').attr('value', centro[i].ID_TIPO_DOCUMENTO).text(centro[i].DESCRIPCION));
                    }
                    combo2.append(doc.html());
                    doc2.remove();

                    //Llenar combo de Regimen
                    combo3.attr('disabled', false).append($('<option></option>').attr('value', "-1").text(':: Seleccione Regimen ::'));
                    combo3.attr('disabled', false).append($('<option></option>').attr('value', "No Aplica").text('No Aplica'));
                    combo3.attr('disabled', false).append($('<option></option>').attr('value', "Regimen Simplificado").text('Régimen Simplificado'));
                    combo3.attr('disabled', false).append($('<option></option>').attr('value', "Pequeño Contribuyente").text('Pequeño Contribuyente'));
                    combo3.attr('disabled', false).append($('<option></option>').attr('value', "Mediano Contribuyent").text('Mediano Contribuyente'));
                    combo3.attr('disabled', false).append($('<option></option>').attr('value', "Grande Contribuyente").text('Grande Contribuyente'));

                },
                error: function (request, status, error) {
                    //DesplegarError(JSON.parse(request.responseText).Message);
                    DesplegarError("Error al cargar documentos de identificación.");
                }
            });
        }

        function SumarCompras() {
            var valorCompra = 0;

            hfSumaCompra = document.getElementById('<%= hfSumaCompra.ClientID %>');
            lblSumaCompra = document.getElementById('<%= lblSumaCompra.ClientID %>');

            $("#<%=gvDetalleFactura.ClientID%> input[type=text][id*=txtValorTotal]").each(function (index) {
                //Check if number is not empty                
                if ($.trim($(this).val()) != '')
                //Check if number is a valid integer                    
                    if (!isNaN($(this).val()))
                        valorCompra = valorCompra + parseFloat($(this).val());
            });

            hfSumaCompra.value = valorCompra.toFixed(2).toString().replace(',', '.');
            lblSumaCompra.innerHTML = valorCompra.toFixed(2).toString().replace(',', '.');
        }

        function NuevaFactura() {
            SumarCompras();
            SumarIVA();
            OcultarMensaje();
            InicializarFactura();
        };

        function CalcularIVA(e, impuesto, valorTotal, tipoIVA) {
            var importeIVA = $(e.target).closest('tr').find('input[type=hidden][id*=hfImporte]').val();
            var valorIVA = 0.00;

            var impuestoInt = impuesto.toString().replace(',', '');
            var valorTotalInt = valorTotal.toString().replace(',', ''); 

            //if ((parseFloat(impuesto) >= parseFloat(valorTotal)) && ((parseFloat(impuesto) != 0) && (parseFloat(valorTotal) != 0))) {
            if ((parseFloat(impuestoInt) >= parseFloat(valorTotalInt)) && ((parseFloat(impuestoInt) != 0) && (parseFloat(valorTotalInt) != 0))) {
                DesplegarError('El impuesto no puede ser mayor o igual al valor total.');
                return;
            }

            if (importeIVA > 0) {
                valorIVA = (parseFloat(valorTotalInt == '' ? 0 : valorTotalInt) -
                           parseFloat(impuestoInt == '' ? 0 : impuestoInt)) - ((parseFloat(valorTotalInt == '' ? 0 : valorTotalInt) -
                           parseFloat(impuestoInt == '' ? 0 : impuestoInt)) / (1 + (importeIVA / 100)));
            }
            else {
                valorIVA = 0.00;
            }

            $(e.target).closest('tr').find('input[type=hidden][id*=hfIVAgd]').val(valorIVA.toFixed(2));
            $(e.target).closest('tr').find('[id*=lblIVA]').html(valorIVA.toFixed(2));
        };

        function SumarIVA() {
            var valorIVA = 0.00;

            hfSumaIVA = document.getElementById('<%= hfSumaIva.ClientID %>');
            lblSumaIVA = document.getElementById('<%= lblSumaIVA.ClientID %>');

            $('#<%=gvDetalleFactura.ClientID%> span[id*=lblIVA]').each(function (index) {
                //Check if number is not empty                
                if ($.trim($(this).text()) != "")
                //Check if number is a valid integer                    
                    if (!isNaN($(this).text()))
                        valorIVA = valorIVA + parseFloat($(this).text());
            });

            hfSumaIVA.value = valorIVA.toFixed(2).toString().replace(',', '.');
            lblSumaIVA.innerHTML = valorIVA.toFixed(2).toString().replace(',', '.');
        }

        function DesplegarError(mensaje) {
            var divMensaje = document.getElementById('<%= divMensajeError.ClientID %>');
            divMensaje.style.display = "table";
            divMensaje.innerHTML = mensaje;
        }

        function DesplegarMensaje(mensaje) {
            var divMensaje = document.getElementById('<%= divMensaje.ClientID %>');
            divMensaje.style.display = "table";
            divMensaje.innerHTML = mensaje;
        }

        function OcultarMensaje() {
            var divMensaje = document.getElementById('<%= divMensajeError.ClientID %>');
            divMensaje.style.display = "none";
            var divMensaje = document.getElementById('<%= divMensaje.ClientID %>');
            divMensaje.style.display = "none";
        }

        function ValidarNumero(numObj) {

            var numero = numObj.value;

            if (!/^[0-9]+.?([0-9]{1,2})?$/.test(numero))
                numObj.value = numero.substring(0, numero.length - 1);
        }

        function doAjaxCall(url, data, successHandler) {
            $.ajax({
                type: 'POST',
                url: url,
                data: data,
                contentType: 'application/json; charset=utf-8',
                dataType: 'json',
                success: function (response) {
                    successHandler(response);
                },
                error: function (request, status, error) {
                    DesplegarError(JSON.parse(request.responseText).Message)
                }
            });
        };

        function doAjaxCall1(url, data, e) {
            $.ajax({
                type: 'POST',
                url: url,
                data: data,
                contentType: 'application/json; charset=utf-8',
                dataType: 'json',
                success: function (response) {
                    //var cars = response.d;
                    var cars = (typeof response.d) == 'string' ? eval('(' + response.d + ')') : response.d;
                    hfIVA = 0;
                    if (cars != null) {
                        hfIVA = document.getElementById('<%= hfIVA.ClientID %>').value;
                        hfIVA = cars.IMPORTE == null ? 0 : cars.IMPORTE;
                    }
                    $(e.target).closest('tr').find('input[type=hidden][id*=hfImporte]').val(hfIVA);
                    CargarDatosCalculo(e);
                },
                error: function (request, status, error) {
                    //DesplegarError(JSON.parse(request.responseText).Message)
                    DesplegarError("Error al calcular IVA.");
                }
            });
        };

        function CargarDatosCalculo(e) {
            var valorTI = $(e.target).closest('tr').find('[id*=ddlTipoIVA]').val();
            var definicionTI = $('option:selected', $(e.target)).text();
            var impuesto = $(e.target).closest('tr').find('input[type=text][id*=txtImpuesto]').val();
            var importeIVA = $(e.target).closest('tr').find('input[type=hidden][id*=hfImporte]').val();
            var valorTotal = $(e.target).closest('tr').find('input[type=text][id*=txtValorTotal]').val();

            $(e.target).closest('tr').find('[id*=lblTipoIva]').text(valorTI);
            $(e.target).closest('tr').find('[id*=lblTipoIvaDefinicion]').text(definicionTI);

            $(e.target).closest('tr').find('input[type=hidden][id*=hfTipoIVAD]').val(definicionTI);
            $(e.target).closest('tr').find('input[type=hidden][id*=hfTipoIva]').val(valorTI);

            CalcularIVA(e, impuesto, valorTotal, valorTI)
            SumarIVA();
        }

        function Validar() {
            $(document).ready(function () {
                $("#form1").validate({
                    rules: {
                        '<%=txtSerie.UniqueID %>': { required: true, maxlength: 17 },
                        '<%=txtNumeroFactura.UniqueID %>': { required: true, maxlength: 20, digits: true },
                        '<%= txtNumeroIdentificacion.UniqueID %>': { required: true, maxlength: 15 },
                        '<%= txtNombreProveedor.UniqueID %>': { required: true, maxlenght: 40 },
                        '<%=ddlTipoDocumento.UniqueID %>': { validTipoDoc: true },
                        '<%= txtFechaFactura.UniqueID %>': { required: true }

                    },
                    messages: {
                        '<%=txtSerie.UniqueID %>': { required: "* Ingrese Numero de Serie", maxlength: "*La Serie debe de contener menos de 17 caracteres" },
                        '<%=txtNumeroFactura.UniqueID %>': { required: "* Ingrese Número de Factura", maxlength: "*La Factura debe de contener menos de 20 caracteres", digits: "*El Numero de Factura no acepta letras. " },
                        '<%= txtNumeroIdentificacion.UniqueID %>': { required: "*Ingrese Número de Identificación", maxlength: "*El Número debe contener menos de 15 caracteres" },
                        '<%=txtSerie.UniqueID %>': { required: "* Ingrese Numero de Serie", maxlength: "*La serie debe de contener menos de 17 caracteres" },
                        '<%=txtNumeroFactura.UniqueID %>': { required: "* Ingrese Numero de Factura", maxlength: "*La Factura debe de contener menos de 17 caracteres", digits: "*El Numero de Factura no acepta letras. " },
                        '<%= txtNumeroIdentificacion.UniqueID %>': { required: "*Ingrese Numero de Identificación", maxlength: "*El Numero debe contener menos de 15 caracteres" },
                        '<%= txtNombreProveedor.UniqueID %>': { required: "*Proveedor Requerido", maxlength: "*El Proveedor debe contener menos de 40 caracteres" },
                        '<%=ddlTipoDocumento.UniqueID %>': { validTipoDoc: "*Debe Seleccionar Tipo de Documento" },
                        '<%= txtFechaFactura.UniqueID %>': { required: "*Ingrese Fecha" }
                    }
                });
            });
        }


        function ValidaTipoDoc() {
            var ddlTipo = document.getElementById('<%=ddlTipoDocumento.ClientID%>').selectedIndex;
            if ((ddlTipo == 0) || (ddlTipo == -1)) {
                return false;
            }
            return true;
        }

        function ValidaCentroCosto() {
            var ddlCentroCosto = document.getElementById('<%=ddlCentroCosto.ClientID%>').selectedIndex;
            if ((ddlCentroCosto == 0) || (ddlCentroCosto == -1)) {
                return false;
            }
            return true;
        }

        function ValidaOrdenCosto() {
            var ddlOrdenCosto = document.getElementById('<%= ddlOrdenCosto.ClientID %>').selectedIndex;
            if ((ddlOrdenCosto == 0) || (ddlOrdenCosto == -1)) {
                return false;
            }
            return true;
        }

        function Limpiar() {
            var nuovourl = window.location.href + '';
            nuovourl = nuovourl + (nuovourl.indexOf('?') > -1 ? '&refreshme=1' : '?refreshme=1');
            window.location.href = nuovourl;
        }

        function BuscarAprobador() {

            var data = '{idSociedadCentro:"' + $('#<%=hfSociedadCentro.ClientID %>').val() + '",centroCosto:"' + $('#<%=ddlCentroCosto.ClientID %>').val() + '",ordenCosto:"' + $('#<%=ddlOrdenCosto.ClientID %>').val() + '"}';

            $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: "../../RegistroFacturas/MapeoUsuariosCentros.asmx/BuscarAprobadores",
                data: data,
                dataType: "json",
                async: true,
                success: function (response) {
                    var aprobador = (typeof response.d) == 'string' ? eval('(' + response.d + ')') : response.d;

                    var ret = '';
                    for (var i = 0; i < aprobador.length; i++) {
                        ret = ret + (aprobador[i].NOMBRE + "<BR />");
                    }

                    var divMensaje = document.getElementById('<%= nombres.ClientID %>');
                    divMensaje.innerHTML = '';
                    divMensaje.style.display = "table";
                    divMensaje.innerHTML = ret;

                },
                error: function (response, status, error) {
                    //alert(JSON.parse(request.responseText).Message);
                    DesplegarError("Error al recuperar aprobadores.");
                }
            });

        }

        function ddlTipoDocumentoChange() {
            $('#<%= txtNumeroIdentificacion.ClientID %>').prop("disabled", false);
            $('#<%=txtNumeroIdentificacion.ClientID %>').val("");

            $('[id*=gvDetalleFactura]').find('span[id*=lblTipoIva]').html("");
            $('[id*=gvDetalleFactura][id*=ddlTipoIVA]').val("");
            $('[id*=gvDetalleFactura][id*=ddlTipoIVA]').prop("disabled", false)
            $('#<%= btnAgregarFila.ClientID %>').prop("disabled", false);


            var text = $(this).find('option:selected').text();
            if (text == 'VALE') {
                $('#<%=txtNumeroIdentificacion.ClientID %>').val(" ");
                $('#<%= txtNumeroIdentificacion.ClientID %>').prop("disabled", true);
                $('#<%=txtNumeroIdentificacion.ClientID %>').blur();

                $('[id*=gvDetalleFactura]').find('span[id*=lblTipoIva]').html("NA");
                $('[id*=gvDetalleFactura][id*=ddlTipoIVA]').val("NA");
                $('[id*=gvDetalleFactura][id*=ddlTipoIVA]').prop("disabled", true)
                $('#<%= btnAgregarFila.ClientID %>').prop("disabled", true);

            }
        }

        function chImpuestoServChange() {
            if ($('#<%= chImpuestoServ.ClientID %>').attr('checked')) {
                $('[id*=gvDetalleFactura][id*=txtImpuesto]').prop("disabled", true)
                $('[id*=gvDetalleFactura][id*=txtImpuesto]').val($('[id*=gvDetalleFactura][id*=txtValorTotal]').val() * 0.10);
                $('[id*=gvDetalleFactura][id*=txtImpuesto]').blur();

            } 
            else {

                $('[id*=gvDetalleFactura][id*=txtImpuesto]').prop("disabled", false);
                $('[id*=gvDetalleFactura][id*=txtImpuesto]').val(0);
                $('[id*=gvDetalleFactura][id*=txtImpuesto]').blur();
            }
        }

        $('[id*=gvDetalleFactura]input[type=text][id*=txtImpuesto]').live('blur', function (e) {
            var valorTotal = $(this).closest('tr').find('input[type=text][id*=txtValorTotal]').val();
            var tipoIVA = $(e.target).closest('tr').find('[id*=ddlTipoIVA]').val();

            if (hfImpuesto == "0" || hfImpuesto == null) {
                var impuesto = $(e.target).closest('tr').find('input[type=text][id*=txtImpuesto]').val();
            } else {

                var cantidad = $(e.target).closest('tr').find('input[type=text][id*=txtCantidad]').val();
                var impuesto = cantidad * hfImpuesto.valueOf();
                $(this).closest('tr').find('input[type=text][id*=txtImpuesto]').val(impuesto.toFixed(2));
            }
            OcultarMensaje();
            CalcularIVA(e, impuesto, valorTotal, tipoIVA);
            SumarIVA();
        });


        $('[id*=gvDetalleFactura][id*=txtValorTotal]').live('change', function (e) {
            if ($('#<%= chImpuestoServ.ClientID %>').attr('checked')) {
                $('[id*=gvDetalleFactura][id*=txtImpuesto]').prop("disabled", true)
                $('[id*=gvDetalleFactura][id*=txtImpuesto]').val($('[id*=gvDetalleFactura][id*=txtValorTotal]').val() * 0.10);
            }
            CargarDatosCalculo(e);
        });

        function txtNumeroFacturaChange() {
//            if (($('#<%= hfCodigoSociedad.ClientID %>').val() == '1440') || ($('#<%= hfCodigoSociedad.ClientID %>').val() == '1630')) {
              if ($('#<%= hfPais.ClientID %>').val() == 'CR'){
                $('[id*=gvDetalleFactura][id*=txtDescripcion]').prop("disabled", true)
                $('[id*=gvDetalleFactura][id*=txtDescripcion]').val($('#<%= txtNumeroFactura.ClientID %>').val() + " " + $('#<%= txtNombreProveedor.ClientID %>').val());
            }
        }

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <asp:HiddenField ID="hfEstado" runat="server" Value="0" />
    <asp:HiddenField ID="hfCajaChicaSAP" runat="server" Value="0" />
    <asp:HiddenField ID="hfIdCajaChica" runat="server" Value="0" />
    <input type="hidden" id="hfIdFactura" runat="server" value="0" />
    <asp:HiddenField ID="hfCodigoSociedad" runat="server" Value="0" />
    <asp:HiddenField ID="hfCodigoCentro" runat="server" Value="0" />
    <asp:HiddenField ID="hfIdProveedor" runat="server" Value="0" />
    <asp:HiddenField ID="hfIVA" runat="server" Value="0" />
    <asp:HiddenField ID="hfSumaCompra" runat="server" Value="0" />
    <asp:HiddenField ID="hfSumaIva" runat="server" Value="0" />
    <asp:HiddenField ID="hfSociedadCentro" runat="server" Value="0" />
    <asp:HiddenField ID="hfNombreProveedor" runat="server" Value="0" />
    <asp:HiddenField ID="hfAcumulado" runat="server" Value="0" />
    <asp:HiddenField ID="hfPais" runat="server" Value="0" />
    <asp:HiddenField ID="hfImpuesto" runat="server" Value="0" />
    <asp:HiddenField ID="hfNombreCC" runat="server" Value="0" />
    <div class="TituloCaja">
        <div class="TituloPagina">
            Registro de facturas de compra</div>
    </div>
    <div id="dialog-confirm">
        <p>
            ¿Desea confirmar el proceso?
        </p>
    </div>
    <div style="margin: 10px 0 10px 0;">
        <div id="divMensajeError" class="ca-MensajeError" style="display: none; font-size: 1.87em;"
            runat="server">
        </div>
        <div id="divMensaje" class="ca-MensajeOK" style="display: none; font-size: 1.87em;"
            runat="server">
        </div>
    </div>
    <div style="display: block; padding-top: 5px;">
    </div>
    <div class="CentrarDiv" style="background-color: White; height: 260px;">
        <fieldset style="width: 99%;">
            <legend>Encabezado Factura
                <div style="float: right;">
                    <asp:Label ID="lblIdFactura" runat="server" Text="0"></asp:Label>
                </div>
            </legend>
        </fieldset>
        <div style="float: left; padding-left: 16px;">
            <asp:Label ID="lblInforCC" runat="server" Text=""></asp:Label>
        </div>
        <br />
        <div style="float: left; width: 380px; padding-left: 5px">
            <div style="float: left; margin-top: 10px; padding-left: 8px;">
                <asp:Label ID="lblCentroCosto" runat="server" Text="Centro de costo:"></asp:Label>
                <asp:DropDownList ID="ddlCentroCosto" runat="server">
                </asp:DropDownList>
            </div>
        </div>
        <div style="float: left; width: 380px; padding-left: 5px">
            <div style="float: left; margin-top: 10px; padding-left: 8px;">
                <asp:Label ID="lblOrdenCosto" runat="server" Text="Orden de costo:"></asp:Label>
                <asp:DropDownList ID="ddlOrdenCosto" runat="server" Width="208px">
                </asp:DropDownList>
            </div>
        </div>
        <br />
        <div style="float: left; width: 350px; padding-left: 1px">
            <div style="float: left; margin-top: 10px; padding-left: 1px;">
                <asp:Label ID="lblTipoDocumento" runat="server" Text="Doc. identificaci&oacute;n:"></asp:Label>
                <asp:DropDownList ID="ddlTipoDocumento" runat="server" Width="209px">
                </asp:DropDownList>
            </div>
        </div>
        <div style="float: left; width: 380px; padding-left: 1px">
            <div style="float: left; margin-top: 10px; padding-left: 1px;">
                <asp:Label ID="lblIdentificador" runat="server" Text="N&uacute;mero identificaci&oacute;n:"></asp:Label>
                <asp:TextBox ID="txtNumeroIdentificacion" runat="server" CssClass="mayuscula"></asp:TextBox>
            </div>
        </div>
        <div style="float: left; width: 380px; padding-left: 1px">
            <div style="float: left; margin-top: 10px; padding-left: 1px;">
                <asp:Label ID="lblProveedor" runat="server" Text="Nombre proveedor:"></asp:Label>
                <asp:TextBox ID="txtNombreProveedor" runat="server" CssClass="mayuscula" ReadOnly="true"></asp:TextBox>
            </div>
        </div>
        <br />
        <div style="float: left; width: 390px; padding-left: 5px">
            <div style="float: left; margin-top: 10px; padding-left: 75px;">
                <asp:Label ID="lblSerie" runat="server" Text="Serie:"></asp:Label>
                <asp:TextBox ID="txtSerie" runat="server" MaxLength="17" CssClass="mayuscula"></asp:TextBox>
            </div>
        </div>
        <div style="float: left; width: 365px; padding-left: 1px">
            <div style="float: left; margin-top: 10px; padding-left: 2px;">
                <asp:Label ID="lblNumero" runat="server" Text="N&uacute;mero factura:"></asp:Label>
                <asp:TextBox ID="txtNumeroFactura" runat="server" CssClass="mayuscula" MaxLength="17"
                    OnKeyUp="ValidarNumero(this)"></asp:TextBox>
            </div>
        </div>
        <div style="float: left; width: 330px; padding-left: 4px">
            <div style="float: left; margin-top: 10px; padding-left: 4px;">
                <asp:Label ID="lblFechaFactura" runat="server" Text="Fecha factura:"></asp:Label>
                <asp:TextBox ID="txtFechaFactura" Text="" CssClass="mayuscula" runat="server"></asp:TextBox>
            </div>
        </div>
        <div style="float: left; width: 150px; padding-left: 5px">
            <div style="float: left; margin-top: 10px; padding-left: 4px;">
                <asp:Label ID="lblFacturaEspecial" runat="server"  Text="Factura especial:"></asp:Label>
                <asp:CheckBox ID="cbFacturaEspecial"  runat="server" />
            </div>
        </div>
        <br />
        <%-- <div style="float: left; width: 380px; padding-left: 5px">--%>
        <div style="float: left; width: 390px; padding-left: 3px">
            <div style="float: left; width: 100%; padding-left: 19px">
                <asp:Label ID="Label1" runat="server" Text="Observaciones: "></asp:Label>
                <asp:TextBox ID="txtObservaciones" runat="server" MaxLength="150" TextMode="MultiLine"></asp:TextBox>
            </div>
        </div>
        <div style="float: left; width: 360px; padding-left: 1px">
            <div style="float: left; margin-top: 10px; padding-left: 2px;">
                <asp:Label ID="lblDiferentesCO" runat="server" Text="Diferentes CO:"></asp:Label>
                <asp:CheckBox ID="chDiferentesCO" runat="server" OnCheckedChanged="chDiferentesCO_CheckedChanged" />
                <asp:TextBox ID="txtTotalFactCO" runat="server" Enabled="false" Width="202px" OnKeyUp="ValidarNumero(this)"></asp:TextBox>
                <div style="float: left; margin-top: 0px; padding-left: 115px;">
                    <asp:Label ID="lblDescripcionTtoal" runat="server" Text="Total Factura Dividida"></asp:Label>
                </div>
            </div>
        </div>
        <div style="float: left; width: 330px; padding-left: 1px">
            <div style="float: left; margin-top: 10px; padding-left: 2px;">
                <asp:Label ID="lblValReal" runat="server" Visible="false" Text="Valor Real Fact.:"></asp:Label>
                <asp:TextBox ID="txtValReal" runat="server" Visible="false" OnKeyUp="ValidarNumero(this)"></asp:TextBox>
            </div>
        </div>
        <br />
        <div style="float: left; width: 390px; padding-left: 3px">
            <div style="float: left; width: 100%; padding-left: 19px">
                <asp:Label ID="lblRetencion" runat="server" Text="Retención 10%: " Visible="false"></asp:Label>
                <asp:CheckBox ID="chRetencion" runat="server" Visible="false" />
                <asp:Label ID="lblImpuestoServ" runat="server" Text="Servicios 10%: " Visible="false"></asp:Label>
                <asp:CheckBox ID="chImpuestoServ" runat="server" Visible="false" />
            </div>
        </div>
        <div style="float: left; width: 365px; padding-left: 1px">
            <div style="float: left; margin-top: 10px; padding-left: 2px;">
                <asp:Label ID="lblTipoCambio" runat="server" Text="Tipo de Cambio:" Visible="false"></asp:Label>
                <asp:TextBox ID="txtTCambio" runat="server" Width="210px" OnKeyUp="ValidarNumero(this)"
                    Visible="false"></asp:TextBox>
            </div>
        </div>
        <br />
        <br />
        <div style="float: left; padding-left: 16px; height: 31px;">
            <asp:Label ID="lblSuma" runat="server" Text="Total compra: "></asp:Label>
            <asp:Label ID="lblSumaCompra" runat="server" Text="0.0"></asp:Label>
            <asp:Label ID="lblIVA" runat="server" Text="Total IVA: "></asp:Label>
            <asp:Label ID="lblSumaIVA" runat="server" Text="0.00"></asp:Label>
            <asp:Label ID="lblAcumulado" runat="server" Text=""></asp:Label>
        </div>
        <div style="float: right; padding-right: 8px; height: 31px;">
            <input type="button" id="AlmacenarFactura" value="Grabar factura" class="btn btn-primary" />
            <input type="button" id="btnNuevaFactura" value="Nueva factura" class="btn" />
            <input type="button" id="btnAutorizadores" value="Autorizadores" class="btn" />
            <%--            <input type="button" id="btnVisible" value="Invisible" class="btn" />--%>
            <%--<asp:Button ID="btnNuevaFactura" runat="server" class="btn" Text="Nueva factura"
                OnClick="btnNuevaFactura_Click" />--%>
        </div>
    </div>
    <div style="display: block; padding-top: 5px;">
    </div>
    <div class="CentrarDiv">
        <div style="float: right; padding-left: 8px;">
            <asp:Button ID="btnAgregarFila" runat="server" class="btn" Text="Agregar fila" />
        </div>
        <asp:GridView ID="gvDetalleFactura" runat="server" AutoGenerateColumns="false" CellPadding="4">
            <Columns>
                <asp:BoundField DataField="NUMERO" HeaderText="No." HeaderStyle-CssClass="Oculto"
                    FooterStyle-CssClass="Oculto" ItemStyle-CssClass="Oculto" />
                <asp:BoundField DataField="ID_FACTURA_DETALLE" HeaderText="ID_FACTURA_DETALLE" HeaderStyle-CssClass="Oculto"
                    FooterStyle-CssClass="Oculto" ItemStyle-CssClass="Oculto" />
                <asp:TemplateField HeaderText="DESCRIPCION GASTO" ItemStyle-HorizontalAlign="Center">
                    <ItemTemplate>
                        <asp:TextBox ID="txtDescripcion" runat="server" CssClass="mayuscula" Width="150px"
                            MaxLength="50" Text='<%# Bind("descripcion") %>'></asp:TextBox>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="CANTIDAD" ItemStyle-HorizontalAlign="Center">
                    <ItemTemplate>
                        <asp:TextBox ID="txtCantidad" runat="server" OnKeyUp="ValidarNumero(this)" Width="60px"
                            Text='<%# Bind("cantidad") %>'></asp:TextBox>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="IVA" ItemStyle-HorizontalAlign="Center">
                    <ItemTemplate>
                        <asp:Label ID="lblIVA" runat="server" Width="45px" Text='<%# Bind("iva") %>'></asp:Label>
                        <asp:HiddenField ID="hfIVAgd" runat="server" EnableViewState="true" Value='<%# Bind("hfiva") %>' />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="VALOR TOTAL" ItemStyle-HorizontalAlign="Center">
                    <ItemTemplate>
                        <asp:TextBox ID="txtValorTotal" runat="server" Width="80px" OnKeyUp="ValidarNumero(this)"
                            onblur="SumarCompras()" Text='<%# Bind("valorTotal") %>'></asp:TextBox>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="IMPUESTO" ItemStyle-HorizontalAlign="Center" >
                    <ItemTemplate>
                        <asp:TextBox ID="txtImpuesto" runat="server" Width="60px" OnKeyUp="ValidarNumero(this)" 
                            onblur="SumarCompras()" Text='<%# Bind("impuesto") %>'> </asp:TextBox>
                    </ItemTemplate>
                </asp:TemplateField>
<%--                <asp:TemplateField HeaderText="IMPUESTO" HeaderStyle-CssClass="Oculto"
                    FooterStyle-CssClass="Oculto" ItemStyle-CssClass="Oculto">--%>
                <asp:TemplateField HeaderText="IMPUESTO" >
                    <ItemTemplate>
                        <asp:DropDownList ID="ddlImpuestos" runat="server" Width="80px">
                        </asp:DropDownList>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="CUENTA CONTABLE">
                    <ItemTemplate>
                        <asp:Label ID="lblCuentaContable" runat="server" Text='<%# Eval("cuentacontable")%>'></asp:Label>
                        <asp:HiddenField ID="hfCuentaContable" runat="server" EnableViewState="true" Value='<%# Bind("hfCC") %>' />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="DEFINICI&Oacute;N CUENTA CONTABLE" ItemStyle-Wrap="true">
                    <ItemTemplate>
                        <asp:Label ID="lblDefinicionCC" runat="server" Text='<%# Eval("definicionCC")%>'
                            Width="200px"></asp:Label>
                        <asp:HiddenField ID="hfDefinicionCC" runat="server" EnableViewState="true" Value='<%# Bind("hfDCC") %>' />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField>
                    <ItemTemplate>
                        <asp:DropDownList ID="ddlCuentaContable" runat="server" Width="150px">
                        </asp:DropDownList>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="TIPO IVA">
                    <ItemTemplate>
                        <asp:Label ID="lblTipoIva" runat="server" Text='<%# Eval("tipoIVA")%>'></asp:Label>
                        <asp:HiddenField ID="hfTipoIva" runat="server" EnableViewState="true" Value='<%# Bind("hfTIVA") %>' />
                        <asp:HiddenField ID="hfImporte" runat="server" EnableViewState="true" Value='<%# Bind("hfImporteIVA") %>' />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="DEFINICI&Oacute;N TIPO IVA">
                    <ItemTemplate>
                        <asp:Label ID="lblTipoIvaDefinicion" runat="server" Text='<%# Eval("tipoIVADefinicion")%>'></asp:Label>
                        <asp:HiddenField ID="hfTipoIVAD" runat="server" EnableViewState="true" Value='<%# Bind("hfTID") %>' />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField>
                    <ItemTemplate>
                        <asp:DropDownList ID="ddlTipoIVA" runat="server" Width="80px">
                        </asp:DropDownList>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="DETALLE" ItemStyle-HorizontalAlign="Center">
                    <ItemTemplate>
                        <asp:TextBox ID="txtDetalle" runat="server" Width="150px" MaxLength="50" Text='<%# Bind("detalleCR") %>'></asp:TextBox>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField>
                    <ItemTemplate>
                        <input type="button" value="Eliminar" onclick="deleteRow(this)">
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
    </div>
    <div style="display: block; padding-top: 5px;">
    </div>
    <%--<div class="CentrarDiv" style="width: 600px; background-color: White; height: 115px;">
        <div class="CentrarDiv" style="width: 400px;">
            <div style="float: left; padding-bottom: 8px;">
                <div style="float: left; padding-bottom: 8px;">
                    <asp:CheckBox ID="cbISR" runat="server" />
                </div>
                <div style="float: left; padding-bottom: 8px;">
                    <asp:Label ID="lblRetenciónISR" runat="server" OnKeyUp="ValidarNumero(this)" Text="Retenci&oacute;n ISR:"></asp:Label>
                </div>
                <div style="float: left; padding-bottom: 8px;">
                    <asp:TextBox ID="txtRetencionISR" runat="server" Width="100px"></asp:TextBox>
                </div>
                <div style="float: left; padding-bottom: 8px;">
                    <asp:Label ID="lblValorISR" runat="server" Text="Valor ISR"></asp:Label>
                </div>
                <div style="float: left; padding-bottom: 8px;">
                    <asp:TextBox ID="txtValorISR" runat="server" OnKeyUp="ValidarNumero(this)" Width="100px"></asp:TextBox>
                </div>
            </div>
            <div style="float: left; padding-bottom: 8px;">
                <div style="float: left; padding-bottom: 8px;">
                    <asp:CheckBox ID="cbIVA" runat="server" />
                </div>
                <div style="float: left; padding-bottom: 8px;">
                    <asp:Label ID="lblRetencionIVA" runat="server" Text="Retenci&oacute;n IVA:"></asp:Label>
                </div>
                <div style="float: left; padding-bottom: 8px;">
                    <asp:TextBox ID="txtRetencionIVA" runat="server" OnKeyUp="ValidarNumero(this)" Width="100px"></asp:TextBox>
                </div>
                <div style="float: left; padding-bottom: 8px;">
                    <asp:Label ID="lblValorIVAV" runat="server" Text="Valor IVA"></asp:Label>
                </div>
                <div style="float: left; padding-bottom: 8px;">
                    <asp:TextBox ID="txtValorIVA" runat="server" OnKeyUp="ValidarNumero(this)" Width="100px"></asp:TextBox>
                </div>
            </div>
        </div>
    </div>--%>
    <div id="autorizadores">
        <div id="nombres" runat="server">
        </div>
    </div>
    <div id="popuporderedit">
        <div id="popupcontainer" style="width: 450px">
            <div class="row">
                <div class="cell popupcontainercell">
                    <table id="orderedittable">
                        <tr>
                            <td>
                                Tipo de documento
                            </td>
                            <td class="cell">
                                <select id="ddlProveedor" style="width: 250px;">
                                </select>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                N&uacute;mero identificaci&oacuten
                            </td>
                            <td>
                                <input type="text" id="txtIdentificacion" maxlength="15" style="text-transform: uppercase;
                                    width: 250px;" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Tipo de documento
                            </td>
                            <td class="cell">
                                <select id="ddlProveedor2" style="width: 250px;">
                                </select>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                N&uacute;mero identificaci&oacuten
                            </td>
                            <td>
                                <input type="text" id="txtIdentificacion2" maxlength="15" style="text-transform: uppercase;
                                    width: 250px;" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Nombre
                            </td>
                            <td>
                                <input type="text" id="txtNombre" maxlength="40" style="text-transform: uppercase;
                                    width: 250px;" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Dirección
                            </td>
                            <td>
                                <input type="text" id="txtDireccion" maxlength="60" style="text-transform: uppercase;
                                    width: 250px;" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Regimen
                            </td>
                            <td class="cell">
                                <select id="ddlRegimen" style="width: 250px;">
                                </select>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Persona F&iacute;sica
                            </td>
                            <td>
                                <input type="checkbox" id="chTipo" style="width: 20px;" />
                            </td>
                        </tr>
                    </table>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
