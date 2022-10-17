<%@ Page Title="" Language="C#" MasterPageFile="~/Base.Master" AutoEventWireup="true"
    CodeBehind="ListadoCajasChicas.aspx.cs" Inherits="RegistroFacturasWEB.RevisionFacturas.ListadoCajasChicas"
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
        $(function () {

            //Set controls instances
            var ddlSociedad = $('#<%= ddlSociedad.ClientID %>');
            var ddlCentro = $('#<%=ddlCentro.ClientID %>');

            //Hirend Field
            var hfSociedad = $('#<%= hfSociedad.ClientID %>');
            var hfCentro = $('#<%= hfCentro.ClientID %>');

            //Uri WebServices
            var ajaxUrlForChild1 = '../../RevisionFacturas/RevisionFacturas.asmx/ListarUsuarioSociedadCentro';

            //Bind events
            ddlSociedad.bind('change', ddlSociedadChange);
            ddlCentro.bind('change', ddlCentroChange);

            //Events handlers
            function ddlSociedadChange() {
                hfSociedad.val(this.value);
                hfCentro.val('0');
                doAjaxCall(ajaxUrlForChild1, '{codigoSociedad: "' + ddlSociedad.val() + '"}', CargarCentro);
            }

            function ddlCentroChange() {
                hfCentro.val(this.value);
            };

            //Disabled them initially
            //ddlCentro.attr('disabled', 'disabled');

            //Populate child1 dropdown if the parent has some selected value
            if (ddlSociedad.val() != 0) {
                doAjaxCall(ajaxUrlForChild1, '{CodigoSociedad: "' + ddlSociedad.val() + '"}', CargarCentro);
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
                    doc.append($('<option></option>').
                            attr('value', centro[i].IDENTIFICADOR).text(centro[i].DESCRIPCION));
                }
                ddlCentro.append(doc.html());
                doc.remove();

                //Set selected value if there is any value in hidden field
                ddlCentro.val(hfCentro.val());
            }
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:HiddenField ID="hfSociedad" runat="server" Value="0" />
    <asp:HiddenField ID="hfCentro" runat="server" Value="0" />
    <div class="TituloCaja">
        <div class="TituloPagina">
            Listado de cajas chicas para revisión</div>
    </div>
    <div style="margin: 10px 0 10px 0;">
        <div id="divMensajeError" class="ca-MensajeError" style="display: none" runat="server">
        </div>
        <div id="divMensaje" class="ca-MensajeOK" style="display: none" runat="server">
        </div>
    </div>
    <div style="display: block; padding-top: 5px;">
    </div>
    <div class="CentrarDiv" style="background-color: White; width: 100%; height: 45px;">
        <div style="float: left; padding-left: 8px; height: 31px;">
            <div style="float: left; margin-top: 10px; padding-left: 8px;">
                <asp:Label ID="lblSociedad" runat="server" Text="Sociedad:"></asp:Label>
            </div>
            <div style="float: left; padding-left: 8px;">
                <asp:DropDownList ID="ddlSociedad" runat="server" Style="width: 280px;">
                </asp:DropDownList>
            </div>
        </div>
        <div style="float: left; padding-left: 8px; height: 31px;">
            <div style="float: left; margin-top: 10px; padding-left: 8px;">
                <asp:Label ID="lblCentro" runat="server" Text="Centro:"></asp:Label>
            </div>
            <div style="float: left; padding-left: 8px;">
                <asp:DropDownList ID="ddlCentro" runat="server" Style="width: 280px;">
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
            <div style="float: left; margin-top: 10px; padding-left: 8px;">
                <asp:Label ID="lblCodigoCC" runat="server" Text="Código CC:"></asp:Label>
            </div>
            <div style="float: left; padding-left: 8px; padding-top: 8px;">
                <asp:TextBox ID="txtCogidoCC" runat="server" Width="150px"></asp:TextBox>
            </div>
        </div>
        <div style="float: left; padding-left: 8px;">
            <asp:Button ID="btnBuscar" runat="server" Text="Buscar" OnClick="btnBuscar_Click" />
        </div>
    </div>
    <div style="display: block; padding-top: 5px;">
    </div>
    <div class="CentrarDiv" style="width: 1050px;">
        <div class="CentrarDiv" style="background-color: White; width: 1050px; padding-right: 10px;">
            <asp:GridView ID="gvListadoCajasChicas" runat="server" AutoGenerateColumns="false"
                EmptyDataText="No hay cajas chicas para revisión." CellPadding="4" AllowPaging="True"
                OnRowCommand="gvListadoCajasChicas_RowCommand" OnPageIndexChanging="gvListadoCajasChicas_PageIndexChanging"
                PageSize="20">
                <Columns>
                    <asp:BoundField DataField="NUMERO" HeaderText="NUMERO" />
                    <asp:BoundField DataField="ID_CAJA_CHICA" HeaderText="ID_CAJA_CHICA" ItemStyle-CssClass="Oculto"
                        HeaderStyle-CssClass="Oculto" FooterStyle-CssClass="Oculto" />
                    <asp:BoundField DataField="CODIGO_SOCIEDAD" HeaderText="SOCIEDAD" ItemStyle-CssClass="Oculto"
                        HeaderStyle-CssClass="Oculto" FooterStyle-CssClass="Oculto" />
                    <asp:BoundField DataField="NOMBRE_SOCIEDAD" HeaderText="NOMBRE SOCIEDAD" />
                    <asp:BoundField DataField="ID_CENTRO" HeaderText="ID_CENTRO" ItemStyle-CssClass="Oculto"
                        HeaderStyle-CssClass="Oculto" FooterStyle-CssClass="Oculto" />
                    <asp:BoundField DataField="NOMBRE_CENTRO" HeaderText="NOMBRE CENTRO" />
                    <asp:BoundField DataField="NUMERO_CAJA_CHICA" HeaderText="N&Uacute;MERO CAJA CHICA"
                        ItemStyle-HorizontalAlign="Center" ItemStyle-CssClass="Oculto" HeaderStyle-CssClass="Oculto"
                        FooterStyle-CssClass="Oculto" />
                    <asp:BoundField DataField="CODIGO_CAJA_CHICA" HeaderText="C&Oacute;DIGO CAJA CHICA"
                        ItemStyle-HorizontalAlign="Center" />
                    <asp:BoundField DataField="CORRELATIVO" HeaderText="CORRELATIVO" ItemStyle-CssClass="Oculto"
                        HeaderStyle-CssClass="Oculto" FooterStyle-CssClass="Oculto" />
                    <asp:BoundField DataField="MONEDA" HeaderText="MONEDA" />
                    <asp:BoundField DataField="MONTO" HeaderText="MONTO" />
                    <asp:BoundField DataField="DESCRIPCION" HeaderText="DESCRIPCI&Oacute;N" ItemStyle-Wrap="false"
                        ItemStyle-Width="200px" HeaderStyle-Width="200px" FooterStyle-Width="200px" />
                    <asp:BoundField DataField="USUARIO" HeaderText="USUARIO" />
                    <asp:BoundField DataField="FECHA_CREACION" HeaderText="FECHA CREACI&Oacute;N" ItemStyle-Wrap="false" />
                    <asp:TemplateField>
                        <ItemTemplate>
                            <asp:ImageButton runat="server" ID="imgListarFacturas" CommandName="ListarFacturas"
                                ImageUrl="~/Images/database.png" CommandArgument='<%# DataBinder.Eval(Container,"RowIndex") %>'
                                ToolTip="Listar facturas." />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <ItemTemplate>
                            <asp:ImageButton runat="server" ID="imgImprimirLiquidacion" CommandName="ImprimirLiquidacion"
                                ImageUrl="~/Images/print.png" CommandArgument='<%# DataBinder.Eval(Container,"RowIndex") %>'
                                ToolTip="Imprimir liquidaci&oacute;n." OnClientClick="myFunction()" />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>
    </div>
</asp:Content>
