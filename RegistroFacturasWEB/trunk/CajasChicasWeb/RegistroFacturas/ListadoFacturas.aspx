<%@ Page Title="" Language="C#" MasterPageFile="~/Base.Master" AutoEventWireup="true"
    CodeBehind="ListadoFacturas.aspx.cs" Inherits="RegistroFacturasWEB.RegistroFacturas.ListadoFacturas" %>

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
            var url = '../../RegistroFacturas/MapeoUsuariosCentros.asmx/BuscarRegistroContableSP';
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
                    $('#Observaciones').html(factura[0].OBSERVACIONES);
                    $('#descripcionGasto').html(factura[0].DESCRIPCION);

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
    <asp:HiddenField ID="hfIdFactura" runat="server" Value="0" />
    <asp:HiddenField ID="hfIdCajaChica" runat="server" Value="0" />
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
    <div class="CentrarDiv" style="background-color: White; width: 945px; padding-right: 10px;
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
    <div class="CentrarDiv" style="background-color: White; width: 95%; padding-right: 10px;">
        <asp:GridView ID="gvListaFacturas" runat="server" AutoGenerateColumns="false" CellPadding="4"
            AllowPaging="True" EmptyDataText="La caja chica no tiene facturas registradas."
            OnRowCommand="gvListaFacturas_RowCommand" OnPageIndexChanging="gvListaFacturas_PageIndexChanging"
            PageSize="20">
            <Columns>
                <asp:BoundField DataField="NUMERO" HeaderText="NUMERO" />
                <asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                    ItemStyle-Width="25px">
                    <ItemTemplate>
                        <asp:Image ID="imgEstado" runat="server" ImageUrl="~/images/cemaforo-verde.png" Visible='<%#(Convert.ToDecimal(DataBinder.Eval(Container.DataItem, "EstadoFac")) == 1) ? true : false %>'
                            ToolTip="Factura vigente." />
                        <asp:Image ID="imgAlto" runat="server" ImageUrl="~/images/cemaforo-rojo.png" Visible='<%#(Convert.ToDecimal(DataBinder.Eval(Container.DataItem, "EstadoFac")) == 2) ? true : false %>'
                            ToolTip="Factura procesada." />
                        <asp:Image ID="Image1" runat="server" ImageUrl="~/images/cemaforo-negro.png" Visible='<%#((Convert.ToDecimal(DataBinder.Eval(Container.DataItem, "EstadoFac")) == 0) || (Convert.ToDecimal(DataBinder.Eval(Container.DataItem, "EstadoFac")) == 3)) ? true : false %>'
                            ToolTip="Factura anulada." />
                    </ItemTemplate>
                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    <ItemStyle HorizontalAlign="Center" Width="25px"></ItemStyle>
                </asp:TemplateField>
                <asp:BoundField DataField="ID_FACTURA" HeaderText="ID_FACTURA" ItemStyle-CssClass="Oculto"
                    FooterStyle-CssClass="Oculto" HeaderStyle-CssClass="Oculto" />
                <asp:BoundField DataField="DOCUMENTO_IDENTIFICACION" HeaderText="DOCUMENTO IDENTIFICACIÓN"
                    ItemStyle-Wrap="false" ItemStyle-Width="80px" HeaderStyle-Width="80px" FooterStyle-Width="80px" />
                <asp:BoundField DataField="NUMERO_IDENTIFICACION" HeaderText="NUMERO IDENTIFICACIÓN"
                    ItemStyle-HorizontalAlign="Center" ItemStyle-Wrap="false" ItemStyle-Width="80px"
                    HeaderStyle-Width="80px" FooterStyle-Width="80px" />
                <asp:BoundField DataField="NOMBRE_PROVEEDOR" HeaderText="NOMBRE PROVEEDOR" ItemStyle-Wrap="false"
                    ItemStyle-Width="80px" HeaderStyle-Width="80px" FooterStyle-Width="80px" />
                <asp:BoundField DataField="SERIE" HeaderText="SERIE" ItemStyle-HorizontalAlign="Center" />
                <asp:BoundField DataField="NUMERO_DOCUMENTO" HeaderText="NUMERO DOCUMENTO" ItemStyle-HorizontalAlign="Center" />
                <asp:BoundField DataField="FECHA_FACTURA" HeaderText="FECHA FACTURA" ItemStyle-HorizontalAlign="Center" />
                <asp:BoundField DataField="ESPECIAL" HeaderText="FACTURA ESPECIAL" ItemStyle-HorizontalAlign="Center" />
                <asp:BoundField DataField="MONEDA" HeaderText="MONEDA" />
                <asp:BoundField DataField="IVA" HeaderText="IVA" ItemStyle-HorizontalAlign="Right " />
                <asp:BoundField DataField="TOTAL" HeaderText="TOTAL" ItemStyle-HorizontalAlign="Right" />
                <asp:BoundField DataField="ESTADO" HeaderText="ESTADO" ItemStyle-CssClass="Oculto"
                    HeaderStyle-CssClass="Oculto" FooterStyle-CssClass="Oculto" />
                <asp:BoundField DataField="APROBACION" HeaderText="APROBACI&Oacute;N" ItemStyle-CssClass="Oculto"
                    HeaderStyle-CssClass="Oculto" FooterStyle-CssClass="Oculto" />
                <asp:BoundField DataField="APROBADA" HeaderText="APROBADA" />
                <asp:BoundField DataField="ESTADO_CC" HeaderText="ESTADO_CC" ItemStyle-CssClass="Oculto"
                    HeaderStyle-CssClass="Oculto" FooterStyle-CssClass="Oculto" />
                <asp:BoundField DataField="CAJA_CHICA_SAP" HeaderText="CAJA_CHICA_SAP" ItemStyle-CssClass="Oculto"
                    FooterStyle-CssClass="Oculto" HeaderStyle-CssClass="Oculto" />
                <asp:BoundField DataField="ID_SOCIEDAD_CENTRO" HeaderText="ID_SOCIEDAD_CENTRO" ItemStyle-CssClass="Oculto"
                    FooterStyle-CssClass="Oculto" HeaderStyle-CssClass="Oculto" />
                <asp:BoundField DataField="SOCIEDAD" HeaderText="SOCIEDAD" ItemStyle-CssClass="Oculto"
                    FooterStyle-CssClass="Oculto" HeaderStyle-CssClass="Oculto" />
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
                        <asp:ImageButton runat="server" ID="imgModificarFactura" CommandName="ModificarFactura"
                            ImageUrl="~/Images/edit.png" CommandArgument='<%# DataBinder.Eval(Container,"RowIndex") %>'
                            Visible='<%#( Convert.ToDecimal(DataBinder.Eval(Container.DataItem, "EstadoFac")) == 1 ) && ( Convert.ToDecimal(DataBinder.Eval(Container.DataItem, "estadoCC")) == 1 ) ? true : false %>'
                            ToolTip="Modificar factura." />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField>
                    <ItemTemplate>
                        <asp:ImageButton runat="server" ID="imgBajaFactura" CommandName="BajaFactura" ImageUrl="~/Images/down_arrow.png"
                            CommandArgument='<%# DataBinder.Eval(Container,"RowIndex") %>' Visible='<%#(Convert.ToDecimal(DataBinder.Eval(Container.DataItem, "ESTADO_CC")) == 1) ? 
                                                                                                       ((Convert.ToString(DataBinder.Eval(Container.DataItem, "APROBADA")) != "RECHAZADA" && Convert.ToDecimal(DataBinder.Eval(Container.DataItem, "EstadoFac")) == 1) ? true : false) : false %>'
                            ToolTip="Anular factura." OnClientClick="javascript:return ConfirmarProceso(this.name,this.alt,'Anular factura');" />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField>
                    <ItemTemplate>
                        <asp:ImageButton runat="server" ID="imgAltaFactura" CommandName="AltaFactura" ImageUrl="~/Images/up_arrow.png"
                            CommandArgument='<%# DataBinder.Eval(Container,"RowIndex") %>' Visible='<%#(Convert.ToDecimal(DataBinder.Eval(Container.DataItem, "ESTADO_CC")) == 1) ? 
                                                                                                       ((Convert.ToString(DataBinder.Eval(Container.DataItem, "APROBADA")) != "RECHAZADA" && Convert.ToDecimal(DataBinder.Eval(Container.DataItem, "EstadoFac")) == 0) ? true : false) : false %>'
                            ToolTip="Activar factura." OnClientClick="javascript:return ConfirmarProceso(this.name,this.alt,'Activar factura');" />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="PAIS" HeaderText="PAIS" ItemStyle-CssClass="Oculto" FooterStyle-CssClass="Oculto"
                    HeaderStyle-CssClass="Oculto" />
            </Columns>
        </asp:GridView>
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
    </div>
</asp:Content>
