<%@ Page Title="" Language="C#" MasterPageFile="~/Base.Master" AutoEventWireup="true"
    CodeBehind="Sociedad.aspx.cs" Inherits="RegistroFacturasWEB.Mantenimientos.Sociedad" %>

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
        function Validar() {
            $(document).ready(function () {
                $("#form1").validate({
                    rules: {
                        '<%=txtCodigo.UniqueID %>': { required: true, maxlength: 4 },
                        '<%=txtNombre.UniqueID %>': { required: true, maxlength: 50 },
                        '<%= txtMesesFactura.UniqueID %>': { required: true, maxlength: 5, digits: true }
                    },
                    messages: {
                        '<%=txtCodigo.UniqueID %>': { required: "* Ingrese Código de la Sociedad", maxlength: "*El Código debe de contener menos de 4 caracteres" },
                        '<%=txtNombre.UniqueID %>': { required: "* Ingrese Nombre de la Sociedad", maxlength: "*El Nombre debe de contener menos de 50 caracteres" },
                        '<%= txtMesesFactura.UniqueID %>': { required: "* Ingrese Mes", maxlength: "*El Mes debe contener menos de 5 digitos", digits: "* El Mes solo acepta dígitos" }
                    }
                });
            });
        }

        function Limpiar() {
            var nuovourl = window.location.href + '';
            nuovourl = nuovourl + (nuovourl.indexOf('?') > -1 ? "&refreshme=1" : "?refreshme=1");
            window.location.href = nuovourl;
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:HiddenField ID="hfIdSociedad" runat="server" Value="0" />
    <div class="TituloCaja">
        <div class="TituloPagina">
            Mantenimiento Sociedades
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
    <div class="CentrarDiv" style="background-color: White; width: 560px;">
        <div style="float: left; padding-bottom: 8px;">
            <div style="float: left; width: 180px;">
                <asp:Label ID="lblCodigo" runat="server" Text="C&oacute;digo"></asp:Label>
            </div>
            <div style="float: left; width: 180px;">
                <asp:TextBox ID="txtCodigo" name="txtCodigo" runat="server" class="mayuscula" Text=""></asp:TextBox>
            </div>
        </div>
        <br />
        <div style="float: left; padding-bottom: 8px;">
            <div style="float: left; width: 180px;">
                <asp:Label ID="lblNombre" runat="server" Text="Nombre"></asp:Label>
            </div>
            <div style="float: left; width: 180px;">
                <asp:TextBox ID="txtNombre" name="txtNombre" runat="server" class="mayuscula" Text=""></asp:TextBox>
            </div>
        </div>
        <br />
        <div style="float: left; padding-bottom: 8px;">
            <div style="float: left; width: 180px;">
                <asp:Label ID="Label1" runat="server" Text="Meses"></asp:Label>
            </div>
            <div style="float: left; width: 180px;">
                <asp:TextBox ID="txtMesesFactura" name="txtMesesFactura" runat="server" Text=""></asp:TextBox>
            </div>
        </div>
        <br />
        <div style="float: left; padding-bottom: 8px;">
            <div style="float: left; width: 180px;">
                <asp:Label ID="lblPais" runat="server" Text="Pa&iacute;s"></asp:Label>
            </div>
            <div style="float: left; width: 180px;">
                <input type="text" id="txtPais" class="mayuscula" runat="server" />
            </div>
        </div>
        <br />
        <div style="float: left; padding-bottom: 8px;">
            <div style="float: left; width: 180px;">
                <asp:Label ID="Label2" runat="server" Text="Moneda"></asp:Label>
            </div>
            <div style="float: left; width: 180px;">
                <input type="text" id="txtMoneda" class="mayuscula" runat="server" />
            </div>
        </div>
        <br />
        <div style="float: left; padding-bottom: 8px;">
            <div style="float: left; width: 180px;">
                <asp:Label ID="Label3" runat="server" Text="Valor maximo compra CC"></asp:Label>
            </div>
            <div style="float: left; width: 180px;">
                <input type="text" id="txtValorCompraCC" class="mayuscula" runat="server" />
            </div>
        </div>
        <br />
        <div style="float: left; padding-bottom: 8px;">
            <div style="float: left; width: 180px;">
                <asp:Label ID="Label4" runat="server" Text="% tolerancia compra CC:"></asp:Label>
            </div>
            <div style="float: left; width: 180px;">
                <input type="text" id="txtTolerancia" class="mayuscula" runat="server" />
            </div>
        </div>
        <br />
         <div style="float: left; padding-bottom: 8px;">
            <div style="float: left; width: 180px;">
                <asp:Label ID="lblTiempoLiquidacion" runat="server" Text="Tiempo de Liquidacion:"></asp:Label>
            </div>
            <div style="float: left; width: 180px;">
                <input type="text" id="txtTiempoLiquidacion" class="mayuscula" runat="server" />
            </div>
        </div>
        <br />
        <div style="float: left; padding-bottom: 8px;">
            <div style="float: left; width: 180px;">
                <asp:Label ID="Label5" runat="server" Text="Mandante:"></asp:Label>
            </div>
            <asp:DropDownList ID="ddlMandante" runat="server" Style="border: solid 1px #ccc;
                width: 210px;">
                <asp:ListItem Selected="True" Value="0"> :: Seleccione Mandante :: </asp:ListItem>
                <asp:ListItem Value="IP"> IP </asp:ListItem>
                <asp:ListItem Value="MM"> MM </asp:ListItem>
            </asp:DropDownList>
        </div>
        <br />
        <div style="float: left; padding-bottom: 8px;">
            <div style="float: left; width: 180px;">
                <asp:Label ID="lblAlta" runat="server" Text="Alta"></asp:Label>
            </div>
            <div style="float: left; width: 180px;">
                <asp:CheckBox ID="cbAlta" runat="server" />
            </div>
        </div>
        <br />
        <div style="float: left; padding-bottom: 8px; width: 400px;">
            <asp:Label ID="lblUsuarioAlta" runat="server" Text="Usuario Alta:"></asp:Label>
            <asp:Label ID="lblUsuarioAltaBD" runat="server" Text=""></asp:Label>
        </div>
        <br />
        <div style="float: left; padding-bottom: 8px; width: 400px;">
            <asp:Label ID="lblFechaalta" runat="server" Text="Fecha Alta:"></asp:Label>
            <asp:Label ID="lblFechaAltaBD" runat="server" Text=""></asp:Label>
        </div>
        <br />
        <div style="float: left; padding-bottom: 8px; width: 400px;">
            <asp:Label ID="lblUsuarioModificacion" runat="server" Text="Usuario Modificaci&oacute;n:"></asp:Label>
            <asp:Label ID="lblUsuarioModificacionBD" runat="server" Text=""></asp:Label>
        </div>
        <br />
        <div style="float: left; padding-bottom: 8px; width: 400px;">
            <asp:Label ID="lblFechaModificacion" runat="server" Text="Fecha modificaci&oacute;n:"></asp:Label>
            <asp:Label ID="lblFechaModificacionDB" runat="server" Text=""></asp:Label>
        </div>
        <br />
        <div style="float: right; padding-bottom: 8px; padding-right: 5px;">
            <div style="float: left; padding-left: 5px;">
                <asp:Button ID="btnGrabar" class="btn btn-primary" runat="server" Text="Grabar" OnClick="btnGrabar_Click"
                    OnClientClick="Validar()" />
            </div>
            <div style="float: left; padding-left: 10px;">
                <asp:Button ID="btnLimpiar" class="btn" runat="server" Text="Limpiar" OnClick="btnLimpiar_Click"
                    OnClientClick="Limpiar()" />
            </div>
        </div>
    </div>
    <div style="display: block; padding-top: 10px;">
    </div>
    <div class="CentrarDiv" style="width: 1100px;">
        <asp:GridView ID="gvSociedad" runat="server" AutoGenerateColumns="False" EmptyDataText="No hay Sociedades."
            CellPadding="4" GridLines="None" AllowPaging="True" OnRowCommand="gvSociedad_RowCommand"
            OnPageIndexChanging="gvSociedad_PageIndexChanging">
            <Columns>
                <asp:BoundField DataField="NUMERO" HeaderText="No." />
                <asp:BoundField DataField="CODIGO_SOCIEDAD" HeaderText="C&Oacute;DIGO SOCIEDAD" />
                <%--HeaderStyle-CssClass="Oculto"
                    FooterStyle-CssClass="Oculto" ItemStyle-CssClass="Oculto" />--%>
                <asp:BoundField DataField="NOMBRE" HeaderText="NOMBRE" />
                <asp:BoundField DataField="MESESFACTURA" HeaderText="MESES" />
                <%--<asp:CheckBoxField DataField="ALTA" HeaderText="ALTA" />--%>
                <asp:BoundField DataField="PAIS" HeaderText="PA&Iacute;S" />
                <asp:BoundField DataField="MONEDA" HeaderText="MONEDA" />
                <asp:BoundField DataField="TIEMPO_LIQUIDACION" HeaderText="TIEMPO LIQUIDACION"/>
                <asp:TemplateField HeaderText="ALTA" ItemStyle-HorizontalAlign="Center">
                    <ItemTemplate>
                        <asp:CheckBox ID="idAlta2" runat="server" Style="position: static" Checked='<%#bool.Parse(Eval("idAlta2").ToString())%>' />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="ID_USUARIOALTA" HeaderText="USUARIO ALTA" />
                <asp:BoundField DataField="FECHA_ALTA" HeaderText="FECHA ALTA" />
                <asp:BoundField DataField="ID_USUARIOMODIFICACION" HeaderText="USUARIO MODIFICACI&Oacute;N" />
                <asp:BoundField DataField="FECHA_MODIFICACION" HeaderText="FECHA MODIFICACI&Oacute;N" />
                <asp:TemplateField>
                    <ItemTemplate>
                        <asp:ImageButton runat="server" ID="imgEditar" CommandName="Editar" ImageUrl="~/Images/edit.png"
                            CommandArgument='<%# DataBinder.Eval(Container,"RowIndex") %>' ToolTip="Editar" />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField>
                    <ItemTemplate>
                        <asp:ImageButton runat="server" ID="imgBaja" CommandName="Baja" ImageUrl="~/Images/down_arrow.png"
                            CommandArgument='<%# DataBinder.Eval(Container,"RowIndex") %>' ToolTip="Dar baja sociedad" />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField>
                    <ItemTemplate>
                        <asp:ImageButton runat="server" ID="imgAlta" CommandName="Alta" ImageUrl="~/Images/up_arrow.png"
                            CommandArgument='<%# DataBinder.Eval(Container,"RowIndex") %>' ToolTip="Dar alta sociedad" />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="COMPRA" HeaderText="COMPRA" HeaderStyle-CssClass="Oculto"
                    FooterStyle-CssClass="Oculto" ItemStyle-CssClass="Oculto" />
                <asp:BoundField DataField="TOLERANCIA" HeaderText="TOLERANCIA" HeaderStyle-CssClass="Oculto"
                    FooterStyle-CssClass="Oculto" ItemStyle-CssClass="Oculto" />
                <asp:BoundField DataField="MANDANTE" HeaderText="MANDANTE" HeaderStyle-CssClass="Oculto"
                    FooterStyle-CssClass="Oculto" ItemStyle-CssClass="Oculto" />
            </Columns>
        </asp:GridView>
    </div>
</asp:Content>
