<%@ Page Title="" Language="C#" MasterPageFile="~/Base.Master" AutoEventWireup="true"
    CodeBehind="ListadoCajasChicas.aspx.cs" Inherits="RegistroFacturasWEB.RegistroFacturas.ListadoCajasChicas"
    EnableEventValidation="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="../Scripts/jquery-1.10.2.js" type="text/javascript"></script>
    <script src="../Scripts/jquery-1.7.2.min.js" type="text/javascript"></script>
    <script type="text/javascript" src="../Scripts/jquery.min.js"></script>
    <script type="text/javascript" src="/Scripts/jquery.validate.js"></script>
    <script src="../Scripts/jquery-ui.min.js" type="text/javascript"></script>
    <link href="../App_Themes/Tema/jquery-ui.css" rel="stylesheet" type="text/css" />
    <script src="/bootstrap/js/bootstrap.js" type="text/javascript"></script>
    <link href="/bootstrap/css/bootstrap.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript">
        $(document).ready(function () {

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

            //Set controls instances
            var ddlSociedad = $('#<%= ddlSociedad.ClientID %>');
            var ddlCentro = $('#<%=ddlCentro.ClientID %>');

            //Hirend Field
            var hfSociedad = $('#<%= hfSociedad.ClientID %>');
            var hfCentro = $('#<%= hfCentro.ClientID %>');

            //Uri WebServices
            var ajaxUrlForChild1 = '../../RegistroFacturas/MapeoUsuariosCentros.asmx/ListarCentrosUsuario';

            //Bind events
            ddlSociedad.bind('change', ddlSociedadChange);
            ddlCentro.bind('change', ddlCentroChange);

            //Events handlers
            function ddlSociedadChange() {
                hfSociedad.val(this.value);
                hfCentro.val('0');
                doAjaxCall(ajaxUrlForChild1, '{codigoSociedad: ' + ddlSociedad.val() + '}', CargarCentro);
            }

            function ddlCentroChange() {
                hfCentro.val(this.value);
            };

            //Disabled them initially
            //ddlCentro.attr('disabled', 'disabled');

            //Populate child1 dropdown if the parent has some selected value
            if (ddlSociedad.val() != 0) {
                doAjaxCall(ajaxUrlForChild1, '{CodigoSociedad: ' + ddlSociedad.val() + '}', CargarCentro);
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

            function CargarCentro(response) {
                var centro = (typeof response.d) == 'string' ? eval('(' + response.d + ')') : response.d;
                ddlCentro.find('option').remove();


                //Append default option
                ddlCentro.attr('disabled', false).append($('<option></option>').attr('value', 0).text('--Seleccione centro--'));
                var doc = $('<div></div>');
                for (var i = 0; i < centro.length; i++) {

                    doc.append($('<option></option>').attr('value', centro[i].IDENTIFICADOR).text(centro[i].DESCRIPCION));
                }
                ddlCentro.append(doc.html());
                doc.remove();

                //Set selected value if there is any value in hidden field
                ddlCentro.val(hfCentro.val());
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


    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <asp:HiddenField ID="hfSociedad" runat="server" Value="0" />
    <asp:HiddenField ID="hfCentro" runat="server" Value="0" />
    <div class="TituloCaja">
        <div class="TituloPagina">
            Listado de cajas chicas</div>
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
    <div style="display: block; padding-top: 5px;">
    </div>
    <div class="CentrarDiv" style="background-color: White; width: 1300px; height: 45px;">
        <div style="float: left; padding-left: 8px; height: 31px;">
            <div style="float: left; margin-top: 10px; padding-left: 8px;">
                <asp:Label ID="lblSociedad" runat="server" Text="Sociedad:"></asp:Label>
            </div>
            <div style="float: left; padding-left: 8px;">
                <asp:DropDownList ID="ddlSociedad" runat="server" Style="width: 250px;">
                </asp:DropDownList>
            </div>
        </div>
        <div style="float: left; padding-left: 8px; height: 31px;">
            <div style="float: left; margin-top: 10px; padding-left: 8px;">
                <asp:Label ID="lblCentro" runat="server" Text="Centro:"></asp:Label>
            </div>
            <div style="float: left; padding-left: 8px;">
                <asp:DropDownList ID="ddlCentro" runat="server" Style="width: 250px;">
                </asp:DropDownList>
            </div>
        </div>
        <div style="float: left; padding-left: 8px; height: 31px;">
            <div style="float: left; margin-top: 10px; padding-left: 8px;">
                <asp:Label ID="lblEstado" runat="server" Text="Estado:"></asp:Label>
            </div>
            <div style="float: left; padding-left: 8px;">
                <asp:DropDownList ID="ddlEstado" runat="server" Style="width: 180px;">
                </asp:DropDownList>
            </div>
        </div>
        <div style="float: left; padding-left: 8px; height: 31px;">
            <div style="float: left; margin-top: 1px; padding-left: 8px;">
                <asp:Label ID="labelCC" runat="server" Text="Código </br>Caja Chica:" Style="width: 200px"></asp:Label>
            </div>
            <div style="float: left; padding-left: 8px;">
                <asp:TextBox ID="txtCodigoCC" name="txtCodigoCC" runat="server"></asp:TextBox>
            </div>
        </div>
        <div style="float: left; padding-left: 8px;">
            <asp:Button ID="btnBuscar" runat="server" Text="Buscar" OnClick="btnBuscar_Click" />
        </div>
    </div>
    <div style="display: block; padding-top: 5px;">
    </div>
    <div class="CentrarDiv" style="width: 1050px;">
        <div class="CentrarDiv" style="background-color: White; width: 1150px; padding-right: 10px;">
            <asp:GridView ID="gvListadoCajasChicas" runat="server" AutoGenerateColumns="false"
                EmptyDataText="El usuario no ha registrado cajas chicas." CellPadding="4" AllowPaging="True"
                OnRowCommand="gvListadoCajasChicas_RowCommand" PageSize="20" ShowFooter="True"
                OnPageIndexChanging="gvListadoCajasChicas_PageIndexChanging">
                <Columns>
                    <asp:BoundField DataField="NUMERO" HeaderText="NUMERO" />
                    <asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                        ItemStyle-Width="25px">
                        <ItemTemplate>
                            <asp:Image ID="imgEstado" runat="server" ImageUrl="~/images/cemaforo-verde.png" Visible='<%#( Convert.ToDecimal(DataBinder.Eval(Container.DataItem, "EstadoCC")) == 1 ) ? true : false %>'
                                ToolTip="Caja chica abierta." />
                            <asp:Image ID="imgAlto" runat="server" ImageUrl="~/images/cemaforo-rojo.png" Visible='<%#( Convert.ToDecimal(DataBinder.Eval(Container.DataItem, "EstadoCC")) == 2 ) ? true : false %>'
                                ToolTip="Caja chica cerrada." />
                            <asp:Image ID="Image1" runat="server" ImageUrl="~/images/cemaforo-negro.png" Visible='<%#( Convert.ToDecimal(DataBinder.Eval(Container.DataItem, "EstadoCC")) == 0 ) ? true : false %>'
                                ToolTip="Caja chica anulada." />
                        </ItemTemplate>
                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                        <ItemStyle HorizontalAlign="Center" Width="25px"></ItemStyle>
                    </asp:TemplateField>
                    <asp:BoundField DataField="ID_CAJA_CHICA" HeaderText="ID_CAJA_CHICA" HeaderStyle-CssClass="Oculto"
                        FooterStyle-CssClass="Oculto" ItemStyle-CssClass="Oculto" />
                    <asp:BoundField DataField="ID_SOCIEDAD_CENTRO" HeaderText="ID_SOCIEDAD_CENTRO" HeaderStyle-CssClass="Oculto"
                        ItemStyle-CssClass="Oculto" FooterStyle-CssClass="Oculto" />
                    <asp:BoundField DataField="CODIGO_SOCIEDAD" HeaderText="CODIGO_SOCIEDAD" HeaderStyle-CssClass="Oculto"
                        ItemStyle-CssClass="Oculto" FooterStyle-CssClass="Oculto" />
                    <asp:BoundField DataField="NOMBRE_SOCIEDAD" HeaderText="NOMBRE SOCIEDAD" />
                    <asp:BoundField DataField="ID_CENTRO" HeaderText="ID_CENTRO" ItemStyle-CssClass="Oculto"
                        HeaderStyle-CssClass="Oculto" FooterStyle-CssClass="Oculto" />
                    <asp:BoundField DataField="NOMBRE_CENTRO" HeaderText="NOMBRE CENTRO" />
                    <asp:BoundField DataField="CODIGO_CAJA_CHICA" HeaderText="CODIGO CAJA CHICA" ItemStyle-HorizontalAlign="Center" />
                    <asp:BoundField DataField="NUMERO_CAJA_CHICA" HeaderText="NUMERO CAJA CHICA" ItemStyle-CssClass="Oculto"
                        FooterStyle-CssClass="Oculto" HeaderStyle-CssClass="Oculto" />
                    <asp:BoundField DataField="OPERACION" HeaderText="OPERACION" />
                    <asp:BoundField DataField="ESTADO" HeaderText="ESTADO" HeaderStyle-CssClass="Oculto"
                        FooterStyle-CssClass="Oculto" ItemStyle-CssClass="Oculto" />
                    <asp:BoundField DataField="DESCRIPCION" HeaderText="DESCRIPCI&Oacute;N" ItemStyle-Wrap="true"
                        ItemStyle-Width="200px" HeaderStyle-Width="200px" FooterStyle-Width="200px" />
                    <asp:BoundField DataField="FACTURAS_CC" HeaderText="FACTURAS" ItemStyle-HorizontalAlign="Center" />
                    <asp:BoundField DataField="MONEDA" HeaderText="MONEDA" />
                    <asp:BoundField DataField="VALOR_CC" HeaderText="MONTO" ItemStyle-HorizontalAlign="Right" />
                    <asp:TemplateField>
                        <ItemTemplate>
                            <asp:ImageButton runat="server" ID="imgAgregarFactura" CommandName="AgregarFactura"
                                ImageUrl="~/Images/plus.png" CommandArgument='<%# DataBinder.Eval(Container,"RowIndex") %>'
                                Visible='<%#( Convert.ToDecimal(DataBinder.Eval(Container.DataItem, "EstadoCC")) == 1 ) ? true : false %>'
                                ToolTip="Agregar factura de compra." />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <ItemTemplate>
                            <asp:ImageButton runat="server" ID="imgListarFacturas" CommandName="ListarFacturas"
                                ImageUrl="~/Images/pages.png" CommandArgument='<%# DataBinder.Eval(Container,"RowIndex") %>'
                                Visible='<%#( Convert.ToDecimal(DataBinder.Eval(Container.DataItem, "EstadoCC")) != 0 ) ? true : false %>'
                                ToolTip="Listar facturas." />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <ItemTemplate>
                            <asp:ImageButton runat="server" ID="imgImprimirLiquidacion" CommandName="ImprimirLiquidacion"
                                ImageUrl="~/Images/print.png" CommandArgument='<%# DataBinder.Eval(Container,"RowIndex") %>'
                                Visible='<%#( Convert.ToDecimal(DataBinder.Eval(Container.DataItem, "EstadoCC")) == 2 ) ? true : false %>'
                                ToolTip="Imprimir liquidaci&oacute;n." OnClientClick="myFunction()" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <ItemTemplate>
                            <asp:ImageButton runat="server" ID="imgCerrarCajaChica" CommandName="CerrarCajaChica"
                                ImageUrl="~/Images/locked.png" CommandArgument='<%# DataBinder.Eval(Container,"RowIndex") %>'
                                Visible='<%#( Convert.ToDecimal(DataBinder.Eval(Container.DataItem, "EstadoCC")) == 0 ) ? false : ( Convert.ToDecimal(DataBinder.Eval(Container.DataItem, "EstadoCC")) == 2 ) ? false : true %>'
                                ToolTip="Cerrar caja chica." OnClientClick="javascript:return ConfirmarProceso(this.name,this.alt,'Cerrar caja chica');" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <ItemTemplate>
                            <asp:ImageButton runat="server" ID="imgBajaCajaChica" CommandName="BajaCajaChica"
                                ImageUrl="~/Images/minus.png" CommandArgument='<%# DataBinder.Eval(Container,"RowIndex") %>'
                                Visible='<%#( Convert.ToDecimal(DataBinder.Eval(Container.DataItem, "EstadoCC")) == 1 ) ? true : false %>'
                                ToolTip="Anular caja chica." OnClientClick="javascript:return ConfirmarProceso(this.name,this.alt,'Anular caja chica');" />
                        </ItemTemplate>
                    </asp:TemplateField>
                     <asp:BoundField DataField="NOMBRE_CC" HeaderText="NOMBRE_CC" HeaderStyle-CssClass="Oculto"
                        FooterStyle-CssClass="Oculto" ItemStyle-CssClass="Oculto" />
                        <asp:BoundField DataField="PAIS" HeaderText="PAIS" HeaderStyle-CssClass="Oculto"
                        FooterStyle-CssClass="Oculto" ItemStyle-CssClass="Oculto" />
                </Columns>
            </asp:GridView>
        </div>
        </div>
</asp:Content>
