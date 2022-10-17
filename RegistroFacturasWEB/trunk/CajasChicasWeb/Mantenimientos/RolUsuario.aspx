<%@ Page Title="" Language="C#" MasterPageFile="~/Base.Master" AutoEventWireup="true"
    CodeBehind="RolUsuario.aspx.cs" Inherits="RegistroFacturasWEB.Mantenimientos.RolUsuario" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="../Scripts/jquery-1.10.2.js" type="text/javascript"></script>
    <script src="../Scripts/jquery-1.7.2.min.js" type="text/javascript"></script>
    <script type="text/javascript" src="../Scripts/jquery.min.js"></script>
    <script type="text/javascript" src="/Scripts/jquery.validate.js"></script>
    <script src="../Scripts/jquery-ui.min.js" type="text/javascript"></script>
    <link href="../App_Themes/Tema/jquery-ui.css" rel="stylesheet" type="text/css" />
    <script src="/bootstrap/js/bootstrap.js" type="text/javascript"></script>
    <link href="/bootstrap/css/bootstrap.css" rel="stylesheet" type="text/css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="TituloCaja">
        <div class="TituloPagina">
            Mantenimiento usuarios
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
                <asp:Label ID="Label1" runat="server" Text="Nombre rol"></asp:Label>
            </div>
            <div style="float: left; width: 180px;">
                <asp:DropDownList ID="ddlRoles" runat="server">
                </asp:DropDownList>
            </div>
        </div>
        <div style="float: left; padding-bottom: 8px;">
            <div style="float: left; width: 180px;">
                <asp:Label ID="lblNombreRol" runat="server" Text="Nombre rol"></asp:Label>
            </div>
            <div style="float: left; width: 180px;">
                <asp:TextBox ID="txtNombreRol" name="txtNombreRol" runat="server"></asp:TextBox>
            </div>
        </div>
        <div style="width: 100%; float: left;">
            <div style="float: left; padding-bottom: 8px; padding-left: 8px;">
                <div style="float: left;">
                    <asp:Label ID="lblAlta" runat="server" Text="Alta"></asp:Label>
                </div>
                <div style="float: left; padding-bottom: 8px; padding-left: 8px;">
                    <asp:CheckBox ID="cbAlta" runat="server" />
                </div>
            </div>
        </div>
        <div style="float: left; padding-bottom: 8px; padding-right: 5px; width: 100%">
            <asp:Label ID="lblFechaModificacion" runat="server" Text="Fecha modificacion:" CssClass="EtiquetaNormalNegrita"></asp:Label>
            <asp:Label ID="lblFechaModificacionDB" runat="server" CssClass="EtiquetaNormal" Text=""></asp:Label>
        </div>
        <div style="float: left; padding-bottom: 8px; padding-right: 5px; width: 100%;">
            <asp:Label ID="lblUsuario" runat="server" Text="Usuario modificacion:" CssClass="EtiquetaNormalNegrita"></asp:Label>
            <asp:Label ID="lblUsuarioDB" runat="server" CssClass="EtiquetaNormal" Text=""></asp:Label>
        </div>
        <div style="float: right; padding-bottom: 8px; padding-right: 5px;">
            <div style="float: left; padding-left: 5px;">
                <asp:Button ID="btnGrabar" class="btn btn-primary" runat="server" Text="Grabar" />
            </div>
            <div style="float: left; padding-left: 10px;">
                <asp:Button ID="btnLimpiar" class="btn" runat="server" Text="Limpiar" />
            </div>
        </div>
        <div style="display: block; padding-top: 5px;">
        </div>
    </div>
    <div style="display: block; padding-top: 5px;">
    </div>
    <div class="CentrarDiv">
        <%--<asp:UpdatePanel ID="upProveedor" runat="server">
            <ContentTemplate>--%>
        <asp:GridView ID="gvRol" runat="server" AutoGenerateColumns="False" EmptyDataText="No hay proveedores."
            CellPadding="4" AllowPaging="True">
            <Columns>
                <asp:BoundField DataField="NUMERO" HeaderText="No." />
                <asp:BoundField DataField="ID_ROL" HeaderText="ID_ROL" HeaderStyle-CssClass="Oculto"
                    FooterStyle-CssClass="Oculto" ItemStyle-CssClass="Oculto" />
                <asp:BoundField DataField="NOMBRE_ROL" HeaderText="NOMBRE ROL" />
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
        <%--</ContentTemplate>
        </asp:UpdatePanel>--%>
    </div>
</asp:Content>
