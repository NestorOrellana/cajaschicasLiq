<%@ Page Title="" Language="C#" MasterPageFile="~/Base.Master" AutoEventWireup="true"
    CodeBehind="AprobacionFacturas.aspx.cs" Inherits="RegistroFacturasWEB.Mantenimientos.AprobacionFacturas" %>

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

        $(function () {

            //Set controls instances
            var ddlSociedad = $('#<%= ddlSociedad.ClientID %>');
            var ddlCentro = $('#<%=ddlCentro.ClientID %>');
            var ddlCajaChica = $('#<%= ddlCajaChica.ClientID %>');

            //Hirend Field
            var hfSociedad = $('#<%= hfSociedad.ClientID %>');
            var hfCentro = $('#<%= hfCentro.ClientID %>');
            var hfCajaChica = $('#<%= hfCajaChica.ClientID %>');

            //Uri WebServices
            var ajaxUrlForChild1 = '../../RegistroFacturas/MapeoUsuariosCentros.asmx/ListarCentrosMapeados';
            var ajaxUrlForChild2 = '../../RegistroFacturas/MapeoUsuariosCentros.asmx/ListarCajaChicaMapeadas';

            //Bind events
            ddlSociedad.bind('change', ddlSociedadChange);
            ddlCentro.bind('change', ddlCentroChange);
            ddlCajaChica.bind('change', ddlCajaChica);

            //Events handlers
            function ddlSociedadChange() {
                hfSociedad.val(this.value);
                hfCentro.val('0');
                doAjaxCall(ajaxUrlForChild1, '{codigoSociedad: ' + ddlSociedad.val() + '}', CargarCentro);
            }

            //Disabled them initially
            //ddlCentro.attr('disabled', 'disabled');

            //Populate child1 dropdown if the parent has some selected value
            if (ddlSociedad.val() != 0) {
                doAjaxCall(ajaxUrlForChild1, '{CodigoSociedad: ' + ddlSociedad.val() + '}', CargarCentro);
            }

            function ddlCentroChange() {
                hfCentro.val(this.value);
                hfCajaChica.val('0');
                doAjaxCall(ajaxUrlForChild2, '{CodigoSociedad: ' + ddlSociedad.val() + ', CodigoCentro: ' + ddlCentro.val() + ' }', CargarCajaChica);
            }

            if (ddlCentro.val() != 0) {
                doAjaxCall(ajaxUrlForChild2, '{CodigoSociedad: ' + ddlSociedad.val() + ', CodigoCentro: ' + ddlCentro.val() + ' }', CargarCajaChica);
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
                ddlCentro.attr('disabled', false).append($('<option></option>').attr('value', 0).text('::ELIJA UNA OPCION::'));
                var doc = $('<div></div>');
                for (var i = 0; i < centro.length; i++) {
                    doc.append($('<option></option>').
                            attr('value', centro[i].IDENTIFICADOR).text(centro[i].DESCRIPCION));
                }
                ddlCentro.append(doc.html());
                doc.remove();

                //Set selected value if there is any value in hidden field
                ddlCentro.val(hfCentro.val());
            }

            //CargarCajaChica
            function CargarCajaChica(response) {
                var cajachica = (typeof response.d) == 'string' ? eval('(' + response.d + ')') : response.d;
                ddlCajaChica.find('option').remove();


                //Append default option
                ddlCajaChica.attr('disabled', false).append($('<option></option>').attr('value', 0).text('::ELIJA UNA OPCION::'));
                var doc = $('<div></div>');
                for (var i = 0; i < centro.length; i++) {
                    doc.append($('<option></option>').
                            attr('value', centro[i].IDENTIFICADOR).text(centro[i].DESCRIPCION));
                }
                ddlCajaChica.append(doc.html());
                doc.remove();

                //Set selected value if there is any value in hidden field
                ddlCajaChica.val(hfCajaChica.val());
            }
        });

 

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:HiddenField ID="hfIdAprobacionFacturas" runat="server" Value="0" />
    <asp:HiddenField ID="hfIdUsuario" runat="server" Value="0" />
    <asp:HiddenField ID="hfSociedad" runat="server" Value="0" />
    <asp:HiddenField ID="hfCentro" runat="server" Value="0" />
    <asp:HiddenField ID="hfCajaChica" runat="server" Value="0" />
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
    <div class="CentrarDiv" style="background-color: White; width: 950px;">
        <div style="float: left; width: 400px; padding-left: 5px">
            <div style="float: left; margin-top: 10px; padding-left: 38px;">
                <asp:Label ID="Label7" runat="server" Text="Usuario:"></asp:Label>
                <asp:TextBox ID="txtUsuario" name="txtUsuario" Text="1" runat="server" Style="width: 280px;"></asp:TextBox>
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
        <div style="float: left; width: 400px; padding-left: 20px">
            <div style="float: left; margin-top: 10px; padding-left: 8px;">
                <asp:Label ID="lblProveedor" runat="server" Text="Proveedor:"></asp:Label>
                <asp:DropDownList ID="ddlProveedor" runat="server" Style="width: 280px;">
                </asp:DropDownList>
            </div>
        </div>
        <div style="float: left; width: 400px; padding-left: 15px">
            <div style="float: left; margin-top: 10px; padding-left: 8px;">
                <asp:Label ID="lblCajaChica" runat="server" Text="Caja Chica:"></asp:Label>
                <asp:DropDownList ID="ddlCajaChica" runat="server" Style="width: 280px;">
                </asp:DropDownList>
            </div>
            <div style="float: left; width: 400px; padding-left: 350px">
                <div style="float: left; margin-top: 10px; padding-left: 8px;">
                    <asp:Button ID="btnBuscar" class="btn btn-primary" runat="server" Text="Buscar" OnClick="btnBuscar_Click" />
                </div>
            </div>
        </div>
        <br />
        <br />
        <div style="display: block; padding-top: 5px;">
        </div>
    </div>
    <div style="display: block; padding-top: 5px;">
    </div>
    <div class="CentrarDiv" style="width: 1050px;">
        <asp:GridView ID="gvAprobacionFactura" runat="server" AutoGenerateColumns="False"
            EmptyDataText="No hay Datos para Mostrar." CellPadding="4" PageSize="10" AllowPaging="True"
            OnRowCommand="gvAprobacionFactura_RowCommand">
            <Columns>
                <asp:BoundField DataField="ID_FACTURA" HeaderText="ID FACTURA" HeaderStyle-CssClass="Oculto"
                    FooterStyle-CssClass="Oculto" ItemStyle-CssClass="Oculto" />
                <asp:TemplateField HeaderText="APROBAR" ItemStyle-HorizontalAlign="Center">
                    <HeaderTemplate>
                        <asp:CheckBox Text="APROBAR" TextAlign="Left" Style="position: static" ID="idAprobTodos"
                            name="idAprobTodos" runat="server" />
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:CheckBox ID="idAprobada" runat="server" Style="position: static" Checked='<%#bool.Parse(Eval("idAprobada").ToString())%>' />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="RECHAZAR" ItemStyle-HorizontalAlign="Center">
                    <HeaderTemplate>
                        <asp:CheckBox Text="RECHAZAR" TextAlign="Left" Style="position: static" ID="idRechTodos"
                            runat="server" />
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:CheckBox ID="idRechazada" runat="server" Style="position: static" Checked='<%#bool.Parse(Eval("idRechazada").ToString())%>' />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="SERIE" HeaderText="SERIE" />
                <asp:BoundField DataField="NUMERO" HeaderText="NUMERO" />
                <asp:BoundField DataField="FECHA" HeaderText="FECHA" />
                <asp:TemplateField HeaderText="ESPECIAL" ItemStyle-HorizontalAlign="Center">
                    <ItemTemplate>
                        <asp:CheckBox ID="especial" runat="server" Enabled="false" Style="position: static"
                            Checked='<%#bool.Parse(Eval("IDespecial").ToString())%>' />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="ID_PROVEEDOR" HeaderText="ID PROVEEDOR" HeaderStyle-CssClass="Oculto"
                    FooterStyle-CssClass="Oculto" ItemStyle-CssClass="Oculto" />
                <asp:BoundField DataField="PROVEEDOR" HeaderText="PROVEEDOR" />
                <asp:BoundField DataField="ID_CAJA_CHICA" HeaderText="ID CAJA CHICA" HeaderStyle-CssClass="Oculto"
                    FooterStyle-CssClass="Oculto" ItemStyle-CssClass="Oculto" />
                <asp:BoundField DataField="CAJA_CHICA" HeaderText="CAJA CHICA" />
                <asp:TemplateField HeaderText="RETENCION IVA" ItemStyle-HorizontalAlign="Center">
                    <ItemTemplate>
                        <asp:CheckBox ID="RETENCION_IVA" runat="server" Enabled="false" Style="position: static"
                            Checked='<%#bool.Parse(Eval("retIVA").ToString())%>' />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="RETENCION ISR" ItemStyle-HorizontalAlign="Center">
                    <ItemTemplate>
                        <asp:CheckBox ID="RETENCION_ISR" runat="server" Enabled="false" Style="position: static"
                            Checked='<%#bool.Parse(Eval("retISR").ToString())%>' />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="IVA" HeaderText="IVA" />
                <asp:BoundField DataField="TOTAL" HeaderText="TOTAL" />
                <asp:TemplateField>
                    <ItemTemplate>
                        <asp:ImageButton runat="server" ID="imgAprobar" CommandName="Aprobar" ImageUrl="~/Images/like.png"
                            CommandArgument='<%# DataBinder.Eval(Container,"RowIndex") %>' ToolTip="Aprobar Factura" />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField>
                    <ItemTemplate>
                        <asp:ImageButton runat="server" ID="imgRechazar" CommandName="Rechazar" ImageUrl="~/Images/dislike.png"
                            CommandArgument='<%# DataBinder.Eval(Container,"RowIndex") %>' ToolTip="Rechazar Factura" />
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
        <br />
        <div style="float: left; padding-left: 10px;">
            <asp:Button ID="btnGrabar" class="btn btn-primary" runat="server" Text="Guardar"
                OnClick="btnGrabar_Click" />
        </div>
    </div>
</asp:Content>
