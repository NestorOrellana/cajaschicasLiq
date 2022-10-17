<%@ Page Title="" Language="C#" MasterPageFile="~/Base.Master" AutoEventWireup="true"
    CodeBehind="Proveedor.aspx.cs" Inherits="RegistroFacturasWEB.Mantenimientos.Proveedor" %>

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
                        '<%=txtNumeroIdentificacion.UniqueID %>': { maxlength: 14, required: true },
                        '<%=txtNumeroIdentificacion2.UniqueID %>': { maxlength: 14 },
                        '<%=txtNombre.UniqueID %>': { maxlength: 100, required: true },
                        '<%=txtDireccion.UniqueID %>': { maxlength: 60, required: true }
                    },
                    //                messages: {'<%=txtNombre.UniqueID %>':{required: "* Ingrese Nombre del Centro", },
                    messages: {
                        '<%=txtNumeroIdentificacion.UniqueID %>': { required: "* Ingrese No. de Identificación", maxlength: "El No. de Identificación debe de contener menos de 15 Caracteres" },
                        '<%=txtNumeroIdentificacion2.UniqueID %>': { maxlength: "El No. de Identificación debe de contener menos de 15 Caracteres" },
                        '<%=txtNombre.UniqueID %>': { required: "* Ingrese Nombre", maxlength: "El nombre debe de contener menos de 100 Caracteres" },
                        '<%=txtDireccion.UniqueID %>': { required: "* Ingrese Dirección", maxlength: "La Dirección debe de contener menos de 60 Caracteres" }
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
    <asp:HiddenField ID="hfIdProveedor" runat="server" Value="0" />
    <div class="TituloCaja">
        <div class="TituloPagina">
            Mantenimiento Proveedores
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
                <asp:Label ID="lblPais" runat="server" Text="Pais"></asp:Label>
            </div>
            <div style="float: left; width: 180px;">
                <asp:DropDownList ID="ddlPais" runat="server" AutoPostBack="True" 
                    onselectedindexchanged="ddlPais_SelectedIndexChanged">
                </asp:DropDownList>
            </div>
        </div>
        <br />

        <div style="float: left; padding-bottom: 8px;">
            <div style="float: left; width: 180px;">
                <asp:Label ID="lblTipoDocumento" runat="server" Text="Tipo documento"></asp:Label>
            </div>
            <div style="float: left; width: 180px;">
                <asp:DropDownList ID="ddlTipoDocumento" runat="server">
                </asp:DropDownList>
            </div>
        </div>
        <br />
        <div style="float: left; padding-bottom: 8px;">
            <div style="float: left; width: 180px;">
                <asp:Label ID="lblNumeroIdentificacion" runat="server" Text="Numero identificaci&oacute;n"></asp:Label>
            </div>
            <div style="float: left; width: 180px;">
                <asp:TextBox ID="txtNumeroIdentificacion" name="txtNumeroIdentificacion" CssClass="mayuscula"
                    runat="server"></asp:TextBox>
            </div>
        </div>
        <br />
        <div style="float: left; padding-bottom: 8px;">
            <div style="float: left; width: 180px;">
                <asp:Label ID="lblTipoDocumento2" runat="server" Text="Tipo documento"></asp:Label>
            </div>
            <div style="float: left; width: 180px;">
                <asp:DropDownList ID="ddlTipoDocumento2" runat="server">
                </asp:DropDownList>
            </div>
        </div>
        <br />
        <div style="float: left; padding-bottom: 8px;">
            <div style="float: left; width: 180px;">
                <asp:Label ID="lblNumeroIdentificacion2" runat="server" Text="N&uacute;mero identificaci&oacute;n"></asp:Label>
            </div>
            <div style="float: left; width: 180px;">
                <asp:TextBox ID="txtNumeroIdentificacion2" name="txtNumeroIdentificacion" CssClass="mayuscula"
                    runat="server"></asp:TextBox>
            </div>
        </div>
        <br />
        <div style="float: left; padding-bottom: 8px;">
            <div style="float: left; width: 180px;">
                <asp:Label ID="lblNombre" runat="server" Text="Nombre proveedor"></asp:Label>
            </div>
            <div style="float: left; width: 180px;">
                <asp:TextBox ID="txtNombre" name="txtNombre" CssClass="mayuscula" runat="server"></asp:TextBox>
            </div>
        </div>
        <br />
        <div style="float: left; padding-bottom: 8px;">
            <div style="float: left; width: 180px;">
                <asp:Label ID="lblDireccion" runat="server" Text="Direcci&oacute;n"></asp:Label>
            </div>
            <div style="float: left; width: 180px;">
                <asp:TextBox ID="txtDireccion" name="txtDireccion" CssClass="mayuscula" runat="server"></asp:TextBox>
            </div>
        </div>
        <br />
        <div style="width: 100%; float: left;">
            <div style="float: left; padding-bottom: 8px;">
                <div style="float: left; width: 180px;">
                    <asp:Label ID="lblRegimen" runat="server" Text="Regimen"></asp:Label>
                </div>
                <div style="float: left; width: 180px;">
                    <asp:DropDownList ID="ddlRegimen" runat="server">
                    </asp:DropDownList>
                </div>
            </div>
            <br />
            <div style="float: left; padding-bottom: 8px;">
                <div style="float: left;">
                    <asp:Label ID="lblTipo" runat="server" Text="Persona F&iacute;sica"></asp:Label>
                </div>
                <div style="float: left; padding-bottom: 8px; padding-left: 8px;">
                    <asp:CheckBox ID="chTipo" runat="server" />
                </div>
            </div>
            <br />
            <div style="float: left; padding-bottom: 8px;">
                <div style="float: left;">
                    <asp:Label ID="lblAlta" runat="server" Text="Alta"></asp:Label>
                </div>
                <div style="float: left; padding-bottom: 8px; padding-left: 8px;">
                    <asp:CheckBox ID="cbAlta" runat="server" />
                </div>
            </div>
        </div>
        <br />
        <div style="float: left; padding-bottom: 8px; padding-right: 5px; width: 100%">
            <asp:Label ID="lblFechaModificacion" runat="server" Text="Fecha modificaci&oacute;n:"></asp:Label>
            <asp:Label ID="lblFechaModificacionDB" runat="server" Text=""></asp:Label>
        </div>
        <div style="float: left; padding-bottom: 8px; padding-right: 5px; width: 100%;">
            <asp:Label ID="lblUsuario" runat="server" Text="Usuario modificaci&oacute;n:"></asp:Label>
            <asp:Label ID="lblUsuarioDB" runat="server" Text=""></asp:Label>
        </div>
        >
        <div style="float: right; padding-bottom: 8px; padding-right: 5px;">
            <div style="float: left; padding-left: 5px;">
                <asp:Button ID="btnGrabar" class="btn btn-primary" runat="server" Text="Grabar" OnClick="btnGrabar_Click"
                    OnClientClick="Validar()" />
            </div>
            <div style="float: left; padding-left: 10px;">
                <asp:Button ID="btnBuscar" class="btn" runat="server" Text="Buscar" OnClick="btnBuscar_Click" />
            </div>
            <div style="float: left; padding-left: 10px;">
                <asp:Button ID="btnLimpiar" class="btn" runat="server" Text="Limpiar" OnClick="btnLimpiar_Click"
                    OnClientClick="Limpiar()" />
            </div>
        </div>
        <div style="display: block; padding-top: 5px;">
        </div>
    </div>
    <div style="display: block; padding-top: 5px;">
    </div>
    <div class="CentrarDiv" style="width: 1300px;">
        <%--<asp:UpdatePanel ID="upProveedor" runat="server">
            <ContentTemplate>--%>
        <asp:GridView ID="gvProveedor" runat="server" AutoGenerateColumns="False" EmptyDataText="No hay proveedores."
            CellPadding="4" AllowPaging="True" OnRowCommand="gvProveedor_RowCommand" OnPageIndexChanging="gvProveedor_PageIndexChanging">
            <Columns>
                <asp:BoundField DataField="NUMERO" HeaderText="No." />
                <asp:BoundField DataField="ID_PROVEEDOR" HeaderText="ID_PROVEEDOR" HeaderStyle-CssClass="Oculto"
                    FooterStyle-CssClass="Oculto" ItemStyle-CssClass="Oculto" />
                <asp:BoundField DataField="ID_TIPO_DOCUMENTO" HeaderText="ID_TIPO_DOCUMENTO" HeaderStyle-CssClass="Oculto"
                    FooterStyle-CssClass="Oculto" ItemStyle-CssClass="Oculto" />
                <asp:BoundField DataField="TIPO_DOCUMENTO" HeaderText="TIPO DOCUMENTO" />
                <asp:BoundField DataField="NUMERO_IDENTIFICACION" HeaderText="N&Uacute;MERO IDENTIFICACI&Oacute;N" />
                <asp:BoundField DataField="ID_TIPO_DOCUMENTO2" HeaderText="ID_TIPO_DOCUMENTO2" HeaderStyle-CssClass="Oculto"
                    FooterStyle-CssClass="Oculto" ItemStyle-CssClass="Oculto" />
                <asp:BoundField DataField="TIPO_DOCUMENTO2" HeaderText="TIPO DOCUMENTO" />
                <asp:BoundField DataField="NUMERO_IDENTIFICACION2" HeaderText="N&Uacute;MERO IDENTIFICACI&Oacute;N" />
                <asp:BoundField DataField="NOMBRE" HeaderText="NOMBRE" />
                <asp:BoundField DataField="DIRECCION" HeaderText="DIRECCI&Oacute;N" />
                <asp:BoundField DataField="REGIMEN" HeaderText="REGIMEN" />
                <asp:TemplateField HeaderText="ALTA" ItemStyle-HorizontalAlign="Center">
                    <ItemTemplate>
                        <asp:CheckBox ID="idAlta" runat="server" Style="position: static" Checked='<%#bool.Parse(Eval("idAlta").ToString())%>' />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="TIPO" ItemStyle-HorizontalAlign="Center">
                    <ItemTemplate>
                        <asp:CheckBox ID="idTipo" runat="server" Style="position: static" Checked='<%#bool.Parse(Eval("idTipo").ToString())%>' />
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
