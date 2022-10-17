<%@ Page Title="" Language="C#" MasterPageFile="~/Base.Master" AutoEventWireup="true"
    CodeBehind="UsuarioRol.aspx.cs" Inherits="RegistroFacturasWEB.Seguridad.UsuarioRol" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="../Scripts/jquery-1.10.2.js" type="text/javascript"></script>
    <script src="../Scripts/jquery-1.7.2.min.js" type="text/javascript"></script>
    <script type="text/javascript" src="../Scripts/jquery.min.js"></script>
    <script type="text/javascript" src="/Scripts/jquery.validate.js"></script>
    <script src="../Scripts/jquery-ui.min.js" type="text/javascript"></script>
    <link href="../App_Themes/Tema/jquery-ui.css" rel="stylesheet" type="text/css" />
    <script src="/bootstrap/js/bootstrap.js" type="text/javascript"></script>
    <link href="/bootstrap/css/bootstrap.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        table, th, td
        {
            border: 1px solid black;
        }
        
        th, td
        {
            width: 250px;
            height: auto;
        }
        
        label
        {
            float: right;
            width: 12em;
            margin-right: 2em;
        }
        
        checkbox
        {
            float: left;
        }
    </style>
    <script type="text/javascript">
        function Validar() {
            $(document).ready(function () {
                $("#form1").validate({
                    rules: {
                        '<%=txtUsuario.UniqueID %>': { required: true }
                    },
                    messages: {
                        '<%=txtUsuario.UniqueID %>': { required: "* Ingrese Usuario" }
                    }
                });
            });
        }
        
        $(document).ready(function () {
            $('#btnRegresar').click(function () {
                window.location.replace($('#<%= hfUrl.ClientID %>').val());
            });
        });
        
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:HiddenField ID="hfIdUsuarioRol" runat="server" Value="0" />
    <asp:HiddenField ID="hfIdUsuario" runat="server" Value="0" />
    <asp:HiddenField ID="hfUrl" runat="server" Value="" />
    <div class="TituloCaja">
        <div class="TituloPagina">
            Asiganción de Roles a Usuarios
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
                <asp:Label ID="lblUsuario" runat="server" Text="Usuario"></asp:Label>
            </div>
            <div style="float: left; width: 180px;">
                <asp:TextBox ID="txtUsuario" name="txtNumeroIdentificacion" CssClass="mayuscula"
                    runat="server"></asp:TextBox>
            </div>
        </div>
        <br />
        <div style="float: left; padding-bottom: 5px;">
            <div style="float: left; width: 180px;">
                <asp:Label ID="Roles" runat="server" Text="Roles"></asp:Label>
            </div>
            <div class="CentrarDiv">
                <asp:CheckBoxList ID="cblRoles" runat="server" RepeatColumns="2" RepeatDirection="Vertical">
                </asp:CheckBoxList>
            </div>
        </div>
        <div style="display: block; padding-top: 5px;">
        </div>
        <br />
        <div style="float: left; padding-bottom: 8px; padding-right: 5px;">
            <input type="button" id="btnRegresar" name="Regresar" class="btn" />
        </div>
        <div style="float: right; padding-bottom: 8px; padding-right: 5px;">
            <div style="float: left; padding-left: 5px;">
                <asp:Button ID="btnGrabar" class="btn btn-primary" runat="server" Text="Grabar" OnClick="btnGrabar_Click"
                    OnClientClick="Validar()" />
            </div>
            <div style="float: left; padding-left: 10px;">
                <asp:Button ID="btnLimpiar" class="btn" runat="server" Text="Limpiar" OnClick="btnLimpiar_Click" />
            </div>
        </div>
    </div>
    <br />
    <%--<div style="float: left; padding-bottom: 8px;">
        <div style="float: left; width: 180px;">
            <asp:Label ID="lblAlta" runat="server" Text="Alta"></asp:Label>
        </div>
        <div style="float: left; width: 180px;">
            <asp:CheckBox ID="cbAlta" runat="server" />
        </div>
    </div>
    <br />
    <div style="float: left; padding-bottom: 8px; padding-right: 5px; width: 100%">
        <asp:Label ID="lblUsuarioAlta" runat="server" Text="Usuario Alta:"></asp:Label>
        <asp:Label ID="lblUsuarioAltaBD" runat="server" Text=""></asp:Label>
    </div>
    <div style="float: left; padding-bottom: 8px; padding-right: 5px; width: 100%">
        <asp:Label ID="lblFechaalta" runat="server" Text="Fecha Alta:"></asp:Label>
        <asp:Label ID="lblFechaAltaBD" runat="server" Text=""></asp:Label>
    </div>
    <div style="float: left; padding-bottom: 8px; padding-right: 5px; width: 100%">
        <asp:Label ID="lblUsuarioModificacion" runat="server" Text="Usuario Modificaci&oacute;n:"></asp:Label>
        <asp:Label ID="lblUsuarioModificacionBD" runat="server" Text=""></asp:Label>
    </div>
    <div style="float: left; padding-bottom: 8px; padding-right: 5px; width: 100%">
        <asp:Label ID="lblFechaModificacion" runat="server" Text="Fecha modificaci&oacute;n:"></asp:Label>
        <asp:Label ID="lblFechaModificacionDB" runat="server" Text=""></asp:Label>
    </div><div style="display: block; padding-top: 10px;">
    </div><div class="CentrarDiv" style="width: 950px;">
        <asp:GridView ID="gvUsuarioRol" runat="server" AutoGenerateColumns="False" EmptyDataText="No hay Datos."
            CellPadding="4" AllowPaging="True" OnRowCommand="gvUsuarioRol_RowCommand" OnPageIndexChanging="gvUsuarioRol_PageIndexChanging"
            PageSize="10">
            <Columns>
                <asp:BoundField DataField="NUMERO" HeaderText="No." />
                <asp:BoundField DataField="USUARIO" HeaderText="ID USUARIO " />
                <asp:BoundField DataField="NOMBRE_USER" HeaderText="USUARIO " />
                <asp:BoundField DataField="ROL" HeaderText="ID ROL" />
                <asp:BoundField DataField="NOMBRE_ROL" HeaderText="ROL " />
                <asp:TemplateField HeaderText="ALTA" ItemStyle-HorizontalAlign="Center">
                    <ItemTemplate>
                        <asp:CheckBox ID="idAltaRol" runat="server" Style="position: static" Checked='<%#bool.Parse(Eval("idAltaRol").ToString())%>' />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="ID_USUARIOALTA" HeaderText="USUARIO ALTA" />
                <asp:BoundField DataField="FECHA_ALTA" HeaderText="FECHA ALTA" />
                <asp:BoundField DataField="ID_USUARIOMODIFICACION" HeaderText="USUARIO MODIFICACI&Oacute;N" />
                <asp:BoundField DataField="FECHA_MODIFICACION" HeaderText="FECHA MODIFICACION&Oacute;N" />
                <asp:TemplateField>
                    <ItemTemplate>
                        <asp:ImageButton runat="server" ID="imgEditar" CommandName="Editar" ImageUrl="~/Images/edit.png"
                            CommandArgument='<%# DataBinder.Eval(Container,"RowIndex") %>' ToolTip="Editar" />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField>
                    <ItemTemplate>
                        <asp:ImageButton runat="server" ID="imgBaja" CommandName="Baja" ImageUrl="~/Images/down_arrow.png"
                            CommandArgument='<%# DataBinder.Eval(Container,"RowIndex") %>' ToolTip="Dar baja Rol para este usuario" />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField>
                    <ItemTemplate>
                        <asp:ImageButton runat="server" ID="imgAlta" CommandName="Alta" ImageUrl="~/Images/up_arrow.png"
                            CommandArgument='<%# DataBinder.Eval(Container,"RowIndex") %>' ToolTip="Dar alta Rol para este usuario" />
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
    </div>--%>
</asp:Content>
