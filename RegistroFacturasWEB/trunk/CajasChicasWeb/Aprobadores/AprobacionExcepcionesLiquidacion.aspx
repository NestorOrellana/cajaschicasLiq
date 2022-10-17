<%@ Page Title="" Language="C#" MasterPageFile="~/Base.Master" AutoEventWireup="true"
    CodeBehind="AprobacionExcepcionesLiquidacion.aspx.cs" Inherits="RegistroFacturasWEB.Aprobaciones.AprobacionExcepcionesLiquidacion"
    EnableEventValidation="false" EnableViewState="true" %> 

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript" src="/Scripts/jquery.min.js"></script>
    <script type="text/javascript" src="/Scripts/jquery.validate.js"></script>
    <script src="../Scripts/jquery-1.10.2.js" type="text/javascript"></script>
    <script src="/Scripts/jquery-1.7.2.min.js" type="text/javascript"></script>
    <script src="/Scripts/jquery.min.js" type="text/javascript"></script>
    <script src="/Scripts/jquery-ui.min.js" type="text/javascript"></script>
    <link href="/App_Themes/Tema/jquery-ui.css" rel="stylesheet" type="text/css" />
    <script src="/bootstrap/js/bootstrap.js" type="text/javascript"></script>
    <link href="/bootstrap/css/bootstrap.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript">

        //Checkbox Seleccion Multiple
        var allCheckBoxSelectorAprob = '#<%=gvAprobacionFactura.ClientID%> input[id*="idAprobTodos"]:checkbox';
        var checkBoxSelectorAprob = '#<%=gvAprobacionFactura.ClientID%> input[id*="idAprobada"]:checkbox';

        var allCheckBoxSelectorRechaz = '#<%=gvAprobacionFactura.ClientID%> input[id*="idRechTodos"]:checkbox';
        var checkBoxSelectorRechaz = '#<%=gvAprobacionFactura.ClientID%> input[id*="idRechazada"]:checkbox';

        function ToggleCheckUncheckAllOptionAsNeededAprob() {
            var totalCheckboxes = $(checkBoxSelectorAprob),
         checkedCheckboxes = totalCheckboxes.filter(":checked"),
         noCheckboxesAreChecked = (checkedCheckboxes.length === 0),
         allCheckboxesAreChecked = (totalCheckboxes.length === checkedCheckboxes.length);

            $(allCheckBoxSelectorAprob).attr('checked', allCheckboxesAreChecked);
        }

        function ToggleCheckUncheckAllOptionAsNeededRechaz() {
            var totalCheckboxes = $(checkBoxSelectorRechaz),
         checkedCheckboxes = totalCheckboxes.filter(":checked"),
         noCheckboxesAreChecked = (checkedCheckboxes.length === 0),
         allCheckboxesAreChecked = (totalCheckboxes.length === checkedCheckboxes.length);

            $(allCheckBoxSelectorRechaz).attr('checked', allCheckboxesAreChecked);
        }

        $(document).ready(function () {
            $('#popuporderedit').dialog({
                autoOpen: false,
                modal: true,
                resizable: false,
                width: 485,
                heigth: 800,
                title: 'Registro contable:',
                open: function (event, ui) {


                },
                close: function (event, ui) {

                    //limpia todos los textbox del popup
                    $('#popuporderedit :text').val('');
                },
                buttons: {
                    Cerrar: function () {
                        $(this).dialog("close");
                        return;
                    }
                }
            });


            $('#dialog-confirm').dialog({
                autoOpen: false,
                resizable: false,
                height: 140,
                modal: true,
                buttons: {
                    "Aceptar": function () {
                        $(this).dialog("close");
                    },
                    "Cancelar": function () {
                        $(this).dialog("close");
                    }
                }

            });

            //Validacion Checkbox Aprobacion
            $(checkBoxSelectorAprob).live('click', function () {
                var $this = $(this); $this.addClass("ui-selected");
                var nombreObj;
                var nom = $this.attr("id");
                var dato
                if (nom.length == 52);
                {
                    nombreObj = "#ContentPlaceHolder1_gvAprobacionFactura_idRechazada_" + $this.attr("id").substring(nom.length - 1, nom.length);
                    $(nombreObj).attr('checked', $(this).is('not:checked'));
                }
                if (nom.length == 53);
                {
                    nombreObj = "ContentPlaceHolder1_gvAprobacionFactura_idRechazada_" + $this.attr("id").substring(nom.length - 2, nom.length);
                    $(nombreObj).attr('checked', $(this).is('not:checked'));
                }
                if (nom.length == 54);
                {
                    nombreObj = "ContentPlaceHolder1_gvAprobacionFactura_idRechazada_" + $this.attr("id").substring(nom.length - 3, nom.length);
                    $(nombreObj).attr('checked', $(this).is('not:checked'));
                }

            });

            //Validacion Checkbox Rechazo
            $(checkBoxSelectorRechaz).live('click', function () {
                var $this = $(this); $this.addClass("ui-selected");
                var nombreObj;
                var nom = $this.attr("id");
                var dato
                if (nom.length == 52);
                {
                    nombreObj = "#ContentPlaceHolder1_gvAprobacionFactura_idAprobada_" + $this.attr("id").substring(nom.length - 1, nom.length);
                    $(nombreObj).attr('checked', $(this).is('not:checked'));
                }
                if (nom.length == 53);
                {
                    nombreObj = "ContentPlaceHolder1_gvAprobacionFactura_idAprobada_" + $this.attr("id").substring(nom.length - 2, nom.length);
                    $(nombreObj).attr('checked', $(this).is('not:checked'));
                }
                if (nom.length == 54);
                {
                    nombreObj = "ContentPlaceHolder1_gvAprobacionFactura_idAprobada_" + $this.attr("id").substring(nom.length - 3, nom.length);
                    $(nombreObj).attr('checked', $(this).is('not:checked'));
                }

            });


            //checkbox Seleccion Multiple
            $(allCheckBoxSelectorAprob).live('click', function () {
                $(checkBoxSelectorAprob).attr('checked', $(this).is(':checked'));
                $(checkBoxSelectorRechaz).attr('checked', $(this).is('not:checked'));
                $(allCheckBoxSelectorRechaz).attr('checked', $(this).is('not:checked'));
                ToggleCheckUncheckAllOptionAsNeededAprob();
            });

            $(checkBoxSelectorAprob).live('click', ToggleCheckUncheckAllOptionAsNeededAprob);

            ToggleCheckUncheckAllOptionAsNeededAprob();

            $(allCheckBoxSelectorRechaz).live('click', function () {
                $(checkBoxSelectorRechaz).attr('checked', $(this).is(':checked'));
                $(checkBoxSelectorAprob).attr('checked', $(this).is('not:checked'));
                $(allCheckBoxSelectorAprob).attr('checked', $(this).is('not:checked'));
                ToggleCheckUncheckAllOptionAsNeededRechaz();
            });

            $(checkBoxSelectorRechaz).live('click', ToggleCheckUncheckAllOptionAsNeededRechaz);

            ToggleCheckUncheckAllOptionAsNeededRechaz();


            //Set controls instances
            var ddlSociedad = $('#<%= ddlSociedad.ClientID %>');
            var ddlCentro = $('#<%= ddlCentro.ClientID %>');
            var ddlCentroCosto = $('#<%=ddlCentroCosto.ClientID %>');
            var ddlOrdenCompra = $('#<%= ddlOrdenCosto.ClientID %>');

            //Hirend Field
            var hfIdSociedad = $('#<%= hfIdSociedad.ClientID %>');
            var hfIdCentro = $('#<%= hfIdCentro.ClientID %>');
            var hfIdCentroCosto = $('#<%= hfIdCentroCosto.ClientID %>');
            var hfIdCentroCompra = $('#<%=hfIdCentroCompra.ClientID %>');
            var hfUsuario = $('#<%=hfUsuario.ClientID %>');

            //Uri WebServices
            var ajaxUrlForChild1 = '../../RegistroFacturas/MapeoUsuariosCentros.asmx/ListarCentroCostoUsuarioDDL';
            var ajaxUrlForChild2 = '../../RegistroFacturas/MapeoUsuariosCentros.asmx/ListarCentroMapeado';
            var ajaxUrlForChild3 = '../../RegistroFacturas/MapeoUsuariosCentros.asmx/ListarOrdenCostoUsuarioDDL';
            //var ajaxUrlForChild4 = '../../RegistroFacturas/MapeoUsuariosCentros.asmx/BuscarCajasChicasSAP';

            //Bind events
            ddlSociedad.bind('change', ddlSociedadChange);
            ddlCentro.bind('change', ddlCentroChange);
            ddlCentroCosto.bind('change', ddlCentroCostoChange);
            ddlOrdenCompra.bind('change', ddlCentroCompraChange);
            //ddlCajaChica.bind('change', ddlCajaChicaChange);

            //Events handlers
            function ddlSociedadChange() {
                hfIdSociedad.val(this.value);
                hfIdCentroCosto.val('-1');
                hfIdCentro.val('-1');
                hfIdCentroCompra.val('-1');
                doAjaxCall(ajaxUrlForChild1, '{usuario: "' + hfUsuario.val() + '",codigoSociedad:"' + ddlSociedad.val() + '"}', CargarCentroCosto);
                doAjaxCall(ajaxUrlForChild2, '{codigoSociedad: ' + ddlSociedad.val() + '}', CargarCentro);
                doAjaxCall(ajaxUrlForChild3, '{usuario: "' + hfUsuario.val() + '",codigoSociedad:"' + ddlSociedad.val() + '"}', CargarCentroCompra);
                //doAjaxCall(ajaxUrlForChild4, '{codigoSociedad: ' + ddlSociedad.val() + '}', CargarCajaChica);
            }


            function ddlCentroChange() {
                hfIdCentro.val(this.value);
            }

            function ddlCentroCostoChange() {
                hfIdCentroCosto.val(this.value);
            };

            function ddlCentroCompraChange() {
                hfIdCentroCompra.val(this.value);
            };

            //Populate child1 dropdown if the parent has some selected value
            if (ddlSociedad.val() > 0) {
                doAjaxCall(ajaxUrlForChild1, '{usuario: "' + hfUsuario.val() + '",codigoSociedad:"' + ddlSociedad.val() + '"}', CargarCentroCosto);
                doAjaxCall(ajaxUrlForChild2, '{CodigoSociedad: ' + ddlSociedad.val() + '}', CargarCentro);
                doAjaxCall(ajaxUrlForChild3, '{usuario: "' + hfUsuario.val() + '",codigoSociedad:"' + ddlSociedad.val() + '"}', CargarCentroCompra);
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
                    }
                });
            }

            function CargarCentroCosto(response) {
                var centro = (typeof response.d) == 'string' ? eval('(' + response.d + ')') : response.d;
                ddlCentroCosto.find('option').remove();


                //Append default option
                ddlCentroCosto.attr('disabled', false).append($('<option></option>').attr('value', -1).text('::Seleccione centro costo::'));
                var doc = $('<div></div>');
                for (var i = 0; i < centro.length; i++) {
                    doc.append($('<option></option>').
                            attr('value', centro[i].IDENTIFICADOR).text(centro[i].DESCRIPCION));
                }
                ddlCentroCosto.append(doc.html());
                doc.remove();

                //Set selected value if there is any value in hidden field
                ddlCentroCosto.val(hfIdCentroCosto.val());
            }

            function CargarCentroCompra(response) {
                var centroCompra = (typeof response.d) == 'string' ? eval('(' + response.d + ')') : response.d;
                ddlOrdenCompra.find('option').remove();


                //Append default option
                ddlOrdenCompra.attr('disabled', false).append($('<option></option>').attr('value', -1).text('::Seleccione centro compra::'));
                var doc = $('<div></div>');
                for (var i = 0; i < centroCompra.length; i++) {
                    doc.append($('<option></option>').
                            attr('value', centroCompra[i].IDENTIFICADOR).text(centroCompra[i].DESCRIPCION));
                }
                ddlOrdenCompra.append(doc.html());
                doc.remove();

                //Set selected value if there is any value in hidden field
                ddlOrdenCompra.val(hfIdCentroCompra.val());
            }


            function CargarCentro(response) {
                var centro = (typeof response.d) == 'string' ? eval('(' + response.d + ')') : response.d;
                ddlCentro.find('option').remove();


                //Append default option
                ddlCentro.attr('disabled', false).append($('<option></option>').attr('value', -1).text('::Seleccione centro::'));
                var doc = $('<div></div>');
                for (var i = 0; i < centro.length; i++) {
                    doc.append($('<option></option>').
                            attr('value', centro[i].IDENTIFICADOR).text(centro[i].DESCRIPCION));
                }
                ddlCentro.append(doc.html());
                doc.remove();

                //Set selected value if there is any value in hidden field
                ddlCentro.val(hfIdCentro.val());
            }
        });

        function BuscarRegistroContable(idFactura) {
            $('#popuporderedit').dialog({
                autoOpen: false,
                modal: true,
                resizable: false,
                width: 485,
                heigth: 800,
                title: 'Registro contable:',
                open: function (event, ui) {

                    initialize(idFactura);
                },
                close: function (event, ui) {

                    //limpia todos los textbox del popup
                    $("#tbl-RR td").remove();
                    $('#popuporderedit :text').val('');
                },
                buttons: {
                    Cerrar: function () {
                        $(this).dialog("close");
                        return;
                    }
                }
            });

            $('#popuporderedit').dialog('open');
            return false;
        }


        function initialize(idFactura) {

            //            var url = '../../RegistroFacturas/MapeoUsuariosCentros.asmx/BuscarRegistroContableSP';
            var url = '../../Aprobadores/RevisionFacturas.asmx/BuscarRegistroContableSP';
            var data = '{idFactura: ' + idFactura + ' }';


            $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: url,
                data: data,
                dataType: "json",
                async: true,
                success: function (response) {
                    var factura = (typeof response.d) == 'string' ? eval('(' + response.d + ')') : response.d;
                    //var factura = JSON.parse(responce.d);

                    $('#tipoDocumento').html(factura[0].DOCUMENTO_IDENTIFICACION);
                    $('#numeroDocumento').html(factura[0].NUMERO_IDENTIFICACION);
                    $('#nombreProveedor').html(factura[0].NOMBRE_PROVEEDOR);
                    $('#direccion').html(factura[0].DIRECCION_PROVEEDOR);
                    $('#serie').html(factura[0].SERIE);
                    $('#numero').html(factura[0].NUMERO);
                    $('#<%=hfIdFactura.ClientID %>').val(factura[0].ID_FACTURA);
                    $('#codigoCC').html(factura[0].CODIGO_CC);
                    $('#Observaciones').html(factura[0].OBSERVACIONES);
                    $('#descripcionGasto').html(factura[0].DESCRIPCION);


                    for (var i = 0; i < factura.length; i++) {
                        $("#tbl-RR tbody").append("<tr><td>" + factura[i].CUENTA_CONTABLE + "</td>" +
                                                              "<td>" + factura[i].INDICADOR_IVA + "</td>" +
                                                              "<td>" + factura[i].DEFINICION_CUENTA_CONTABLE + "</td>" +
                                                              "<td>" + factura[i].CARGO.toFixed(2) + "</td>" +
                                                              "<td>" + factura[i].ABONO.toFixed(2) + "</td>");
                    }
                    $("#tbl-RR tbody").append("<tr><td></td><td></td><td></td><td></td><td></td>" +
                                                              "<tr><td></td>" +
                                                              "<td></td>" +
                                                              "<td> TOTAL</td>" +
                                                              "<td>" + factura[0].SUMA_CARGO.toFixed(2) + "</td>" +
                                                              "<td>" + factura[0].SUMA_ABONO.toFixed(2) + "</td>");
                },
                error: function (request, status, error) {
                    alert(JSON.parse[request.responseText].Message);
                }

            });
        }
              

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:HiddenField ID="hfIdAprobacionFacturas" runat="server" Value="0" />
    <asp:HiddenField ID="hfIdUsuario" runat="server" Value="0" />
    <asp:HiddenField ID="hfUsuario" runat="server" Value="0" />
    <asp:HiddenField ID="hfIdSociedad" runat="server" Value="-1" />
    <asp:HiddenField ID="hfIdCentro" runat="server" Value="-1" />
    <asp:HiddenField ID="hfIdCentroCosto" runat="server" Value="-1" />
    <asp:HiddenField ID="hfIdCentroCompra" runat="server" Value="-1" />
    <asp:HiddenField ID="hfIdFactura" runat="server" Value="-1" />
    <div class="TituloCaja">
        <div class="TituloPagina">
            Aprobación de Facturas
        </div>
    </div>
    <div style="margin: 10px 0 10px 0;">
        <div id="divMensajeError" class="ca-MensajeError" style="display: none" runat="server">
        </div>
        <div id="divMensaje" class="ca-MensajeOK" style="display: none" runat="server">
        </div>
    </div>
    <div style="display: block; padding-top: 5px;">
    </div>
    <div class="CentrarDiv" style="background-color: White; width: 825px;">
        <div style="float: left; width: 400px; padding-left: 5px">
            <div style="float: left; margin-top: 10px; padding-left: 38px;">
                <asp:Label ID="Label7" runat="server" Text="Usuario:" Visible="false"></asp:Label>
                <asp:TextBox ID="txtUsuario" name="txtUsuario" Text="" Visible="false" runat="server"
                    Style="width: 280px;"></asp:TextBox>
            </div>
        </div>
        <br />
        <div style="float: left; width: 400px; padding-left: 30px">
            <div style="float: left; margin-top: 10px; padding-left: 8px;">
                <asp:Label ID="Label1" runat="server" Text="Sociedad:"></asp:Label>
                <asp:DropDownList ID="ddlSociedad" runat="server" Style="width: 280px;">
                </asp:DropDownList>
            </div>
        </div>
        <div style="float: left; width: 400px; padding-left: 42px">
            <div style="float: left; margin-top: 10px; padding-left: 8px;">
                <asp:Label ID="Label2" runat="server" Text="Centro:"></asp:Label>
                <asp:DropDownList ID="ddlCentro" runat="server" Style="width: 280px;">
                </asp:DropDownList>
            </div>
        </div>
        <br />
        <div style="float: left; width: 400px; padding-left: 5px">
            <div style="float: left; margin-top: 10px; padding-left: 8px;">
                <asp:Label ID="lblCentroCosto" runat="server" Text="Centro Costo:"></asp:Label>
                <asp:DropDownList ID="ddlCentroCosto" runat="server" Style="width: 280px;">
                </asp:DropDownList>
            </div>
        </div>
        <div style="float: left; width: 400px; padding-left: 5px">
            <div style="float: left; margin-top: 10px; padding-left: 8px;">
                <asp:Label ID="lblOrdenCosto" runat="server" Text="Orden Costo:"></asp:Label>
                <asp:DropDownList ID="ddlOrdenCosto" runat="server" Style="width: 280px;">
                </asp:DropDownList>
            </div>
        </div>
        <br />
        <div style="float: left; width: 400px; padding-left: 43px">
            <div style="float: left; margin-top: 10px; padding-left: 8px;">
                <asp:Label ID="lblEstado" runat="server" Text="Estado:"></asp:Label>
                <asp:DropDownList ID="ddlEstado" runat="server" Style="width: 280px;">
                </asp:DropDownList>
            </div>
        </div>
        <div style="float: left; width: 400px; padding-left: 50px">
            <div style="float: left; margin-top: 10px; padding-left: 8px;">
                <asp:Label ID="Label3" runat="server" Text="Nivel:"></asp:Label>
                <asp:DropDownList ID="ddlNivel" runat="server" Style="width: 280px;">
                </asp:DropDownList>
            </div>
        </div>
        <br />
        <div style="float: left; width: 400px; padding-left: 15px">
            <div style="float: left; margin-top: 10px; padding-left: 8px;">
                <asp:Label ID="lblRegistrador" runat="server" Text="Registrador:"></asp:Label>
                <asp:DropDownList ID="ddlRegistrador" runat="server" Style="width: 280px;">
                </asp:DropDownList>
            </div>
        </div>
        <div style="float: left; width: 400px; padding-left: 300px">
            <div style="float: left; margin-top: 10px; padding-left: 8px;">
                <asp:Button ID="btnBuscar" class="btn btn-primary" runat="server" Text="Buscar" OnClick="btnBuscar_Click" />
            </div>
        </div>
        <br />
        <div style="display: block; padding-top: 5px;">
        </div>
    </div>
    <div style="display: block; padding-top: 5px;">
    </div>
    <div style="float: left; padding-left: 10px;">
        <asp:Button ID="btnGrabar" class="btn btn-primary" runat="server" Text="Guardar"
            OnClick="btnGrabar_Click" />
    </div>
    <div class="CentrarDiv" style="width: 98%;">
        <asp:GridView ID="gvAprobacionFactura" runat="server" AutoGenerateColumns="False"
            EmptyDataText="No hay Datos para Mostrar." CellPadding="4" AllowPaging="True"
            OnRowCommand="gvAprobacionFactura_RowCommand" OnPageIndexChanging="gvAprobacionFactura_PageIndexChanging"
            PageSize="15">
            <Columns>
                <asp:BoundField DataField="ID_FACTURA" HeaderText="ID FACTURA" HeaderStyle-CssClass="Oculto"
                    FooterStyle-CssClass="Oculto" ItemStyle-CssClass="Oculto" />
                <asp:TemplateField HeaderText="APROBAR" ItemStyle-HorizontalAlign="Center">
                    <HeaderTemplate>
                        APROBAR<br />
                        <asp:CheckBox TextAlign="Left" Style="position: static" ID="idAprobTodos" name="idAprobTodos"
                            runat="server" />
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:CheckBox ID="idAprobada" runat="server" Style="position: static" Checked='<%#bool.Parse(Eval("idAprobada").ToString())%>'
                            Visible='<%#( Convert.ToDecimal(DataBinder.Eval(Container.DataItem, "btbAprobar")) == 1 ) ? false : true %>' />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="RECHAZAR" ItemStyle-HorizontalAlign="Center">
                    <HeaderTemplate>
                        RECHAZAR<br />
                        <asp:CheckBox TextAlign="Left" Style="position: static" ID="idRechTodos" runat="server" />
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:CheckBox ID="idRechazada" runat="server" Style="position: static" Checked='<%#bool.Parse(Eval("idRechazada").ToString())%>'
                            Visible='<%#( Convert.ToDecimal(DataBinder.Eval(Container.DataItem, "btbRechazar")) == 0 ) ? false : ( Convert.ToDecimal(DataBinder.Eval(Container.DataItem, "btbRechazar")) == 1 ) ? false : true %>' />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="PROVEEDOR" HeaderText="PROVEEDOR" ItemStyle-Wrap="false" />
                <asp:BoundField DataField="DESCRIPCION" HeaderText="DESCRIPCIÓN GASTO" ItemStyle-Wrap="false"
                    ItemStyle-Width="50px" />
                <asp:BoundField DataField="SERIE" HeaderText="SERIE" ItemStyle-Wrap="false" ItemStyle-Width="40px" />
                <asp:BoundField DataField="NUMERO" HeaderText="NUMERO" />
                <asp:BoundField DataField="CENTRO_COSTO" HeaderText="CENTRO DE COSTO" ItemStyle-Wrap="false"
                    ItemStyle-Width="50px" />
                <asp:BoundField DataField="NOMBRE_CENTRO_COSTO" HeaderText="NOMBRE CENTRO COSTO"
                    ItemStyle-Wrap="false" ItemStyle-Width="115px" />
                <asp:BoundField DataField="ORDEN_COSTO" HeaderText="ORDEN DE COSTO" ItemStyle-Wrap="false"
                    ItemStyle-Width="50px" />
                <asp:BoundField DataField="NOMBRE_ORDEN_COSTO" HeaderText="NOMBRE ORDEN CO" ItemStyle-Wrap="false"
                    ItemStyle-Width="115px" />
                <asp:BoundField DataField="ID_PROVEEDOR" HeaderText="ID PROVEEDOR" HeaderStyle-CssClass="Oculto"
                    FooterStyle-CssClass="Oculto" ItemStyle-CssClass="Oculto" />
                <asp:BoundField DataField="ID_CAJA_CHICA" HeaderText="ID CAJA CHICA" HeaderStyle-CssClass="Oculto"
                    FooterStyle-CssClass="Oculto" ItemStyle-CssClass="Oculto" />
                <asp:BoundField DataField="MONEDA" HeaderText="MONEDA" />
                <asp:BoundField DataField="IMPUESTO" HeaderText="IMPUESTO" ItemStyle-HorizontalAlign="Right" />
                <asp:BoundField DataField="IVA" HeaderText="IVA" ItemStyle-HorizontalAlign="Right" />
                <asp:BoundField DataField="TOTAL" HeaderText="TOTAL" ItemStyle-HorizontalAlign="Right" />
                <asp:BoundField DataField="FECHA" HeaderText="FECHA FACTURA" />
                <asp:BoundField DataField="TIPO_FACTURA" HeaderText="TIPO DE FACTURA" ItemStyle-Wrap="false"
                    ItemStyle-Width="50px" ItemStyle-HorizontalAlign="Center" />
                <asp:BoundField DataField="ID_APROBADOR_CENTRO" HeaderText="ID APROBADOR_CENTRO"
                    HeaderStyle-CssClass="Oculto" FooterStyle-CssClass="Oculto" ItemStyle-CssClass="Oculto" />
                <asp:BoundField DataField="ID_SOCIEDAD_CENTRO" HeaderText="ID SOCIEDAD CENTRO" HeaderStyle-CssClass="Oculto"
                    FooterStyle-CssClass="Oculto" ItemStyle-CssClass="Oculto" />
                <asp:BoundField DataField="USUARIO_CREACION" HeaderText="USUARIO CREACIÓN" />
                <asp:BoundField DataField="FECHA_CREACION" HeaderText="FECHA CREACION" />
                <asp:TemplateField>
                    <ItemTemplate>
                        <input type="image" src="../Images/info.png" id="imgRegistroContable" onclick="return BuscarRegistroContable(<%# DataBinder.Eval(Container.DataItem, "ID_FACTURA") %>)" />
                        <%--<asp:ImageButton runat="server" ID="imgRegistroContable" CommandName="RegistroContable"
                            ImageUrl="~/Images/info.png" CommandArgument='<%# DataBinder.Eval(Container,"RowIndex") %>'
                            ToolTip="Registro contable." />--%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField>
                    <ItemTemplate>
                        <asp:ImageButton runat="server" ID="imgAprobar" CommandName="Aprobar" ImageUrl="~/Images/like.png"
                            CommandArgument='<%# DataBinder.Eval(Container,"RowIndex") %>' ToolTip="Aprobar Factura"
                            Visible='<%#( Convert.ToDecimal(DataBinder.Eval(Container.DataItem, "btbAprobar")) == 1 ) ? false : true %>' />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField>
                    <ItemTemplate>
                        <asp:ImageButton runat="server" ID="imgRechazar" CommandName="Rechazar" ImageUrl="~/Images/dislike.png"
                            CommandArgument='<%# DataBinder.Eval(Container,"RowIndex") %>' ToolTip="Rechazar Factura"
                            Visible='<%#( Convert.ToDecimal(DataBinder.Eval(Container.DataItem, "btbRechazar")) == 0 ) ? false : ( Convert.ToDecimal(DataBinder.Eval(Container.DataItem, "btbRechazar")) == 1 ) ? false : true %>' />
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
        <br />
    </div>
    <div id="popuporderedit">
        <div id="popupcontainer" style="width: 450px">
            <div class="row">
                <div class="cell popupcontainercell">
                    <table id="orderedittable">
                        <tr>
                            <td>
                                Tipo documento:
                            </td>
                            <td>
                                <span id="tipoDocumento"></span>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Numero documento:
                            </td>
                            <td>
                                <span id="numeroDocumento"></span>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Nombre proveedor:
                            </td>
                            <td>
                                <span id="nombreProveedor"></span>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Dirección
                            </td>
                            <td>
                                <span id="direccion"></span>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Serie
                            </td>
                            <td>
                                <span id="serie"></span>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Numero
                            </td>
                            <td>
                                <span id="numero"></span>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Caja chica
                            </td>
                            <td>
                                <span id="codigoCC"></span>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Observaciones
                            </td>
                            <td>
                                <span id="Observaciones"></span>
                            </td>
                        </tr>
                        <%--<tr>
                            <td>
                                Centro/Orden costo
                            </td>
                            <td>
                                <span id="centroCosto"></span>
                            </td>
                        </tr>--%>
                    </table>
                    <table class="ui-widget ui-widget-content">
                        <tr>
                            <td>
                                <span id="descripcionGasto"></span>
                            </td>
                        </tr>
                    </table>
                    <table id="tbl-RR" class="ui-widget ui-widget-content">
                        <thead class="ui-widget-header">
                            <tr>
                                <th>
                                    Cuenta contable
                                </th>
                                <th>
                                    Indicador IVA
                                </th>
                                <th>
                                    Definicion
                                </th>
                                <th>
                                    Cargo
                                </th>
                                <th>
                                    Abono
                                </th>
                            </tr>
                        </thead>
                        <tbody>
                        </tbody>
                    </table>
                </div>
            </div>
        </div>
        <div id="divBotones" style="display: block; padding-top: 5px; float: left;">
            <input id="btnAprobar" runat="server" class="button right" onserverclick="btnAprobar_Click"
                type="button" value="Aprobar" />
            <input id="btnRechazar" runat="server" class="button right" onserverclick="btnRechazar_Click"
                type="button" value="Rechazar" />
        </div>
    </div>
</asp:Content>
