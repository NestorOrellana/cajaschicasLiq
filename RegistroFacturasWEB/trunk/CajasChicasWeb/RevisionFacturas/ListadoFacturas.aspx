<%@ Page Title="" Language="C#" MasterPageFile="~/Base.Master" AutoEventWireup="true"
    CodeBehind="ListadoFacturas.aspx.cs" Inherits="RegistroFacturasWEB.RevisionFacturas.ListadoFacturas" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="../Scripts/jquery-1.10.2.js" type="text/javascript"></script>
    <script src="../Scripts/jquery-1.7.2.min.js" type="text/javascript"></script>
    <script type="text/javascript" src="../Scripts/jquery.min.js"></script>
    <script type="text/javascript" src="/Scripts/jquery.validate.js"></script>
    <script src="../Scripts/jquery-ui.min.js" type="text/javascript"></script>
    <link href="../App_Themes/Tema/jquery-ui.css" rel="stylesheet" type="text/css" />
    <script src="/bootstrap/js/bootstrap.js" type="text/javascript"></script>
    <link href="/bootstrap/css/bootstrap.css" rel="stylesheet" type="text/css" />
    <script language="javascript" type="text/javascript">

        //Checkbox Seleccion Multiple
        var allCheckBoxSelectorAprob = '#<%=gvListaFacturas.ClientID%> input[id*="idAprobTodos"]:checkbox';
        var checkBoxSelectorAprob = '#<%=gvListaFacturas.ClientID%> input[id*="idAprobada"]:checkbox';

        var allCheckBoxSelectorRechaz = '#<%=gvListaFacturas.ClientID%> input[id*="idRechTodos"]:checkbox';
        var checkBoxSelectorRechaz = '#<%=gvListaFacturas.ClientID%> input[id*="idRechazada"]:checkbox';

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


        });

        //Validacion Checkbox Aprobacion
        $(checkBoxSelectorAprob).live('click', function () {
            var $this = $(this); $this.addClass("ui-selected");
            var nombreObj;
            var nom = $this.attr("id");
            var dato
            if (nom.length == 52);
            {
                nombreObj = "#ContentPlaceHolder1_gvListaFacturas_idRechazada_" + $this.attr("id").substring(nom.length - 1, nom.length);
                $(nombreObj).attr('checked', $(this).is('not:checked'));
            }
            if (nom.length == 53);
            {
                nombreObj = "ContentPlaceHolder1_gvListaFacturas_idRechazada_" + $this.attr("id").substring(nom.length - 2, nom.length);
                $(nombreObj).attr('checked', $(this).is('not:checked'));
            }
            if (nom.length == 54);
            {
                nombreObj = "ContentPlaceHolder1_gvListaFacturas_idRechazada_" + $this.attr("id").substring(nom.length - 3, nom.length);
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
                nombreObj = "#ContentPlaceHolder1_gvListaFacturas_idAprobada_" + $this.attr("id").substring(nom.length - 1, nom.length);
                $(nombreObj).attr('checked', $(this).is('not:checked'));
            }
            if (nom.length == 53);
            {
                nombreObj = "ContentPlaceHolder1_gvListaFacturas_idAprobada_" + $this.attr("id").substring(nom.length - 2, nom.length);
                $(nombreObj).attr('checked', $(this).is('not:checked'));
            }
            if (nom.length == 54);
            {
                nombreObj = "ContentPlaceHolder1_gvListaFacturas_idAprobada_" + $this.attr("id").substring(nom.length - 3, nom.length);
                $(nombreObj).attr('checked', $(this).is('not:checked'));
            }

        });



        function ConfirmarProceso(uniqueID, itemID, evento) {

            $("#dialog-confirm").dialog({
                title: evento,

                resizable: false,
                height: 200,
                width: 350,
                modal: true,
                buttons: {
                    "Aceptar": function () {
                        __doPostBack(uniqueID, '');
                        $(this).dialog("close");

                    },
                    "Cancelar": function () { $(this).dialog("close"); }
                }
            });

            $('#dialog-confirm').dialog('open');
            return false;
        }

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
            var url = '../../RevisionFacturas/RevisionFacturas.asmx/BuscarRegistroContableSP';
            var data = '{idFactura: ' + idFactura + '}';


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
                    //$('#centroCosto').html(factura[0].NUMERO);


                    //var dout = factura[0].FECHA_FACTURA;
                    //$("#fechaFactura").text(dout.toString('M-d-yyyy'));
                    $('#codigoCC').html(factura[0].CODIGO_CC);

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
                    alert(JSON.parse(request.responseText).Message);
                }
            });

        }

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <asp:HiddenField ID="hfIdCajaChica" runat="server" Value="0" />
    <asp:HiddenField ID="hfCodigoSociedad" runat="server" Value="0" />
    <asp:HiddenField ID="hfFacturaDividida" runat="server" Value="0" />
    <div class="TituloCaja">
        <div class="TituloPagina">
            Listado de facturas</div>
    </div>
    <div id="dialog-confirm">
        <p>
            ¿Desea confirmar el proceso?
        </p>
    </div>
    <div style="margin: 10px 0 10px 0;">
        <div id="divMensajeError" class="ca-MensajeError" style="display: none" runat="server">
        </div>
        <div id="divMensaje" class="ca-MensajeOK" style="display: none" runat="server">
        </div>
    </div>
    <div class="CentrarDiv" style="background-color: White; width: 65%; padding-right: 10px;
        height: 40px">
        <div style="float: left; padding-left: 16px;">
            <asp:Label ID="lblInforCC" runat="server" Text=""></asp:Label>
        </div>
        <div style="float: left; padding-left: 8px; height: 31px;">
            <div style="float: left; margin-top: 10px; padding-left: 8px;">
                <asp:Label ID="lblNumeroIdentificacion" runat="server" Text="Numero identificación:"></asp:Label>
            </div>
            <div style="float: left; padding-left: 8px; padding-top: 8px;">
                <asp:TextBox ID="txtNumeroIdentificacion" runat="server" Width="150px"></asp:TextBox>
            </div>
        </div>
        <div style="float: left; padding-left: 8px; height: 31px;">
            <div style="float: left; margin-top: 10px; padding-left: 8px;">
                <asp:Label ID="lblSerie" runat="server" Text="Serie:"></asp:Label>
            </div>
            <div style="float: left; padding-left: 8px; padding-top: 8px;">
                <asp:TextBox ID="txtSerie" runat="server" Width="150px"></asp:TextBox>
            </div>
        </div>
        <div style="float: left; padding-left: 8px; height: 31px;">
            <div style="float: left; margin-top: 10px; padding-left: 8px;">
                <asp:Label ID="lblNumero" runat="server" Text="Numero:"></asp:Label>
            </div>
            <div style="float: left; padding-left: 8px; padding-top: 8px;">
                <asp:TextBox ID="txtNumero" runat="server" Width="150px"></asp:TextBox>
            </div>
        </div>
        <div style="float: left; padding-left: 8px;">
            <asp:Button ID="btnBuscar" runat="server" Text="Buscar" OnClick="btnBuscar_Click" />
        </div>
    </div>
    <div style="display: block; padding-top: 5px;">
    </div>
    <div class="CentrarDiv" style="background-color: White; width: 100%; padding-right: 10px;">
        <asp:GridView ID="gvListaFacturas" runat="server" AutoGenerateColumns="false" CellPadding="4"
            AllowPaging="True" EmptyDataText="La caja chica no tiene facturas registradas."
            OnRowCommand="gvListaFacturas_RowCommand" OnPageIndexChanging="gvListaFacturas_PageIndexChanging"
            PageSize="20">
            <Columns>
                <asp:TemplateField HeaderText="APROBAR" ItemStyle-HorizontalAlign="Center">
                    <HeaderTemplate>
                        APROBAR<br />
                        <asp:CheckBox TextAlign="Left" Style="position: static" ID="idAprobTodos" name="idAprobTodos"
                            runat="server" />
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:CheckBox ID="idAprobada" runat="server" Style="position: static" Checked='<%#bool.Parse(Eval("idAprobada").ToString())%>' />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="RECHAZAR" ItemStyle-HorizontalAlign="Center">
                    <HeaderTemplate>
                        RECHAZAR<br />
                        <asp:CheckBox TextAlign="Left" Style="position: static" ID="idRechTodos" runat="server" />
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:CheckBox ID="idRechazada" runat="server" Style="position: static" Checked='<%#bool.Parse(Eval("idRechazada").ToString())%>' />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="NUMERO" HeaderText="NUMERO" ItemStyle-HorizontalAlign="Center" />
                <asp:BoundField DataField="ID_FACTURA" HeaderText="ID_FACTURA" ItemStyle-CssClass="Oculto"
                    FooterStyle-CssClass="Oculto" HeaderStyle-CssClass="Oculto" />
                <asp:BoundField DataField="TIPO_DOCUMENTO" HeaderText="TIPO DOCUMENTO" ItemStyle-Wrap="false"
                    ItemStyle-Width="80px" HeaderStyle-Width="80px" FooterStyle-Width="80px" />
                <asp:BoundField DataField="NIT" HeaderText="DOCUMENTO IDENTIFICACION" ItemStyle-Wrap="false"
                    ItemStyle-Width="80px" HeaderStyle-Width="80px" FooterStyle-Width="80px" />
                <asp:BoundField DataField="NOMBRE_PROVEEDOR" HeaderText="NOMBRE PROVEEDOR" ItemStyle-Wrap="false"
                    ItemStyle-Width="150px" HeaderStyle-Width="150px" FooterStyle-Width="150px" />
                <asp:BoundField DataField="SERIE" HeaderText="SERIE" />
                <asp:BoundField DataField="NUMERO_DOCUMENTO" HeaderText="N&Uacute;MERO DOCUMENTO" />
                <asp:BoundField DataField="FECHA_FACTURA" HeaderText="FECHA FACTURA" />
                <asp:BoundField DataField="ESPECIAL" HeaderText="FACTURA ESPECIAL" ItemStyle-HorizontalAlign="Center" />
                <asp:BoundField DataField="MONEDA" HeaderText="MONEDA" />
                <asp:BoundField DataField="IMPUESTO" HeaderText="IMPUESTO" />
                <asp:BoundField DataField="IVA" HeaderText="IVA" />
                <asp:BoundField DataField="TOTAL" HeaderText="TOTAL" />
                <asp:BoundField DataField="VIGENTE" HeaderText="VIGENTE" ItemStyle-CssClass="Oculto"
                    FooterStyle-CssClass="Oculto" HeaderStyle-CssClass="Oculto" />
                <asp:BoundField DataField="INDICADOR" HeaderText="INDICADOR" />
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
                        <asp:ImageButton runat="server" ID="imgAceptar" CommandName="AceptarFactura" ImageUrl="~/Images/like.png"
                            CommandArgument='<%# DataBinder.Eval(Container,"RowIndex") %>' ToolTip="Aceptar factura."
                            OnClientClick="javascript:return ConfirmarProceso(this.name,this.alt,'Aceptar registro');" />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField>
                    <ItemTemplate>
                        <asp:ImageButton runat="server" ID="imgRechazar" CommandName="RechazarFactura" ImageUrl="~/Images/dislike.png"
                            CommandArgument='<%# DataBinder.Eval(Container,"RowIndex") %>' ToolTip="Rechazar factura."
                            OnClientClick="javascript:return ConfirmarProceso(this.name,this.alt,'Rechazar registro');" />
                    </ItemTemplate>
                </asp:TemplateField>

                 <asp:BoundField DataField="FACTURA_DIVIDIDA" HeaderText="Factura Dividida" HeaderStyle-CssClass="Oculto"
                    FooterStyle-CssClass="Oculto" ItemStyle-CssClass="Oculto" />
                 <asp:BoundField DataField="MANDANTE" HeaderText="Mandante" HeaderStyle-CssClass="Oculto"
                    FooterStyle-CssClass="Oculto" ItemStyle-CssClass="Oculto" />
            </Columns>
        </asp:GridView>
        <br />
        <div style="float: left; padding-left: 10px;">
            <asp:Button ID="btnGrabar" class="btn btn-primary" runat="server" Text="Guardar"
                OnClick="btnGrabar_Click" />
        </div>
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
                        <%--<tr>
                            <td>
                                Centro/Orden costo
                            </td>
                            <td>
                                <span id="centroCosto"></span>
                            </td>
                        </tr>--%>
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
    </div>
</asp:Content>
