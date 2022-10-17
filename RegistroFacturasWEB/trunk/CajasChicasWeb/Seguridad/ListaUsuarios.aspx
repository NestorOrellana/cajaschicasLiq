<%@ Page Title="" Language="C#" MasterPageFile="~/Base.Master" AutoEventWireup="true"
    CodeBehind="ListaUsuarios.aspx.cs" Inherits="RegistroFacturasWEB.Seguridad.ListaUsuarios" %>

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
            Mantenimiento Centros
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
                <asp:Label ID="lblNombre" runat="server" Text="Usuario"></asp:Label>
            </div>
            <div style="float: left; width: 180px;">
                <asp:TextBox ID="txtUsuario" name="txtUsuario" runat="server" Text=""></asp:TextBox>
            </div>
        </div>
        <br />
        <div style="float: right; padding-bottom: 8px; padding-right: 5px;">
            <div style="float: left; padding-left: 10px;">
                <asp:Button ID="btnLimpiar" class="btn btn-primary" runat="server" Text="Buscar"
                    OnClick="btnBuscar_Click" />
            </div>
        </div>
    </div>
    <div style="display: block; padding-top: 10px;">
    </div>
    <div class="CentrarDiv" style="background-color: ; width: 320px;">
        <asp:GridView ID="gvListaUsuarios" runat="server" AutoGenerateColumns="false" EmptyDataText="No hay Usuarios Registrados."
            CellPadding="4" AllowPaging="True" OnRowCommand="gvListaUsuarios_RowCommand"
            onpageindexchanging="gvListaUsuarios_PageIndexChanging">
            <Columns>
                <asp:BoundField DataField="NUMERO" HeaderText="NUMERO" />
                <asp:BoundField DataField="ID_USUARIO" HeaderText="ID_USUARIO" ItemStyle-CssClass="Oculto"
                    HeaderStyle-CssClass="Oculto" FooterStyle-CssClass="Oculto" />
                <asp:BoundField DataField="USUARIO" HeaderText="USUARIO" />
                <asp:BoundField DataField="NOMBRE" HeaderText="NOMBRE" />
                <asp:TemplateField>
                    <ItemTemplate>
                        <asp:ImageButton runat="server" ID="imgAgregarCentro" CommandName="AgregarCentroCosto"
                            ImageUrl="~/Images/home.png" CommandArgument='<%# DataBinder.Eval(Container,"RowIndex") %>'
                            ToolTip="Agregar Centro Costo." />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField>
                    <ItemTemplate>
                        <asp:ImageButton runat="server" ID="imgAgregarOrden" CommandName="AgregarOrdenCompra"
                            ImageUrl="~/Images/shopping_cart.png" CommandArgument='<%# DataBinder.Eval(Container,"RowIndex") %>'
                            ToolTip="Agregar Orden Compra." />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField>
                    <ItemTemplate>
                        <asp:ImageButton runat="server" ID="imgAsignarRol" CommandName="AsignarRol" ImageUrl="~/Images/users.png"
                            CommandArgument='<%# DataBinder.Eval(Container,"RowIndex") %>' ToolTip="Asignar Rol." />
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
    </div>
</asp:Content>
