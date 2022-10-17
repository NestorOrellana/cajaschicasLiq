<%@ Page Title="" Language="C#" MasterPageFile="~/Base.Master" AutoEventWireup="true"
    CodeBehind="Moneda.aspx.cs" Inherits="RegistroFacturasWEB.Mantenimientos.Moneda" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="/bootstrap/js/bootstrap.js" type="text/javascript"></script>
    <link href="/bootstrap/css/bootstrap.css" rel="stylesheet" type="text/css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:HiddenField ID="hfIdMoneda" runat="server" Value="0" />
    <div class="TituloCaja">
        <div class="TituloPagina">
            Mantenimiento Monedas
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
                <asp:Label ID="lblMoneda" runat="server" Text="Moneda"></asp:Label>
            </div>
            <div style="float: left; width: 180px;">
                <asp:TextBox ID="txtMoneda" name="txtMoneda" CssClass="mayuscula" runat="server"></asp:TextBox>
            </div>
        </div>
        <br />
        <div style="float: left; padding-bottom: 8px;">
            <div style="float: left; width: 180px;">
                <asp:Label ID="lblDescripcion" runat="server" Text="Descripcion"></asp:Label>
            </div>
            <div style="float: left; width: 180px;">
                <asp:TextBox ID="txtDescripcion" name="txtDescripcion" CssClass="mayuscula" runat="server"></asp:TextBox>
            </div>
        </div>
                <br />
        <div style="float: left; padding-bottom: 8px;">
            <div style="float: left; width: 180px;">
                <asp:Label ID="Label1" runat="server" Text="Simbolo"></asp:Label>
            </div>
            <div style="float: left; width: 180px;">
                <asp:TextBox ID="txtSimbolo" name="txtSimbolo" CssClass="mayuscula" runat="server"></asp:TextBox>
            </div>
        </div>
        <br />
        <div style="float: right; padding-bottom: 8px; padding-right: 5px;">
            <div style="float: left; padding-left: 5px;">
                <asp:Button ID="btnGrabar" class="btn btn-primary" runat="server" Text="Grabar" OnClick="btnGrabar_Click" />
            </div>
            <div style="float: left; padding-left: 10px;">
                <asp:Button ID="btnBuscar" class="btn" runat="server" Text="Buscar" OnClick="btnBuscar_Click" />
            </div>
            <div style="float: left; padding-left: 10px;">
                <asp:Button ID="btnLimpiar" class="btn" runat="server" Text="Limpiar" OnClick="btnLimpiar_Click" />
            </div>
        </div>
        <div style="display: block; padding-top: 5px;">
        </div>
    </div>
    <div style="display: block; padding-top: 5px;">
    </div>
    <div class="CentrarDiv" style="width: 1000px;">
        <%--<asp:UpdatePanel ID="upProveedor" runat="server">
            <ContentTemplate>--%>
        <asp:GridView ID="gvMoneda" runat="server" AutoGenerateColumns="False" EmptyDataText="No hay monedas."
            CellPadding="4" AllowPaging="True" onrowcommand="gvMoneda_RowCommand">
            <Columns>
                <asp:BoundField DataField="NUMERO" HeaderText="No." />
                <asp:BoundField DataField="MONEDA" HeaderText="MONEDA" />
                <asp:BoundField DataField="DESCRIPCION" HeaderText="DESCRIPCION" />
                <asp:TemplateField HeaderText="ALTA" ItemStyle-HorizontalAlign="Center">
                    <ItemTemplate>
                        <asp:CheckBox ID="idAlta" runat="server" Style="position: static" Checked='<%#bool.Parse(Eval("idAlta").ToString())%>' />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="USUARIO_ALTA" HeaderText="USUARIO CREACI&Oacute;N" />
                <asp:BoundField DataField="FECHA_ALTA" HeaderText="FECHA ALTA" />
                <asp:BoundField DataField="USUARIO_MODIFICACION" HeaderText="USUARIO MODIFICACI&Oacute;N" />
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
                            CommandArgument='<%# DataBinder.Eval(Container,"RowIndex") %>' ToolTip="Dar baja proveedor" />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField>
                    <ItemTemplate>
                        <asp:ImageButton runat="server" ID="imgAlta" CommandName="Alta" ImageUrl="~/Images/up_arrow.png"
                            CommandArgument='<%# DataBinder.Eval(Container,"RowIndex") %>' ToolTip="Dar alta proveedor" />
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
    </div>
</asp:Content>
