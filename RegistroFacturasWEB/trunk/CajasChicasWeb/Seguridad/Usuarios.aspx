<%@ Page Title="" Language="C#" MasterPageFile="~/Base.Master" AutoEventWireup="true"
    CodeBehind="Usuarios.aspx.cs" Inherits="RegistroFacturasWEB.Seguridad.Usuarios" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript" src="/Scripts/jquery.min.js"></script>
    <script src="../Scripts/jquery-1.10.2.js" type="text/javascript"></script>
    <script src="/Scripts/jquery-1.7.2.min.js" type="text/javascript"></script>
    <script src="/Scripts/jquery.min.js" type="text/javascript"></script>
    <script src="/Scripts/jquery-ui.min.js" type="text/javascript"></script>
    <link href="/App_Themes/Tema/jquery-ui.css" rel="stylesheet" type="text/css" />
    <script src="/bootstrap/js/bootstrap.js" type="text/javascript"></script>
    <link href="/bootstrap/css/bootstrap.css" rel="stylesheet" type="text/css" />
    <script src="/Scripts/jquery.validate.js" type="text/javascript"></script>
    <script type="text/javascript">
        $.validator.addMethod("validarcorreo", ValidarCorreo, "Correo no Valido");

        function Validar() {
            $(document).ready(function () {
                $("#form1").validate({
                    rules: {
                        '<%=txtNombreUsuario.UniqueID %>': { required: true, maxlength: 60 },
                        '<%=txtUsuario.UniqueID %>': { required: true, maxlength: 30 },
                        '<%=txtCorreo.UniqueID %>': { validarcorreo: true, required: true }
                    },
                    messages: {
                        '<%=txtNombreUsuario.UniqueID %>': { required: "* Ingrese Nombre de Usuario", maxlength: "*El Nombre debe de contener menos de 60 caracteres" },
                        '<%=txtUsuario.UniqueID %>': { required: "* Ingrese Usuario", maxlength: "*El Usuario debe de contener menos de 30 caracteres" },
                        '<%=txtCorreo.UniqueID %>': { validarcorreo: "* Correo Invalido", required: "* Ingrese correo electrónico " }

                    }
                });

            });
        }

        function Limpiar() {
            var nuovourl = window.location.href + '';
            nuovourl = nuovourl + (nuovourl.indexOf('?') > -1 ? "&refreshme=1" : "?refreshme=1");
            window.location.href = nuovourl;
        }

        function ValidarCorreo() {

            var emailReg = /^([\w-\.]+@([\w-]+\.)+[\w-]{2,4})?$/;
            if (!emailReg.test($("#<%=txtCorreo.ClientID%>").val())) {
                return false;
            } else {
                return true;
            }

        }
        function ValidarCorreo1() {
            //var regex = /[\w-\.]{2,}@([\w-]{2,}\.)*([\w-]{2,}\.)[\w-]{2,4}/;
            //var correo = document.getElementById('<%=txtCorreo.ClientID%>');
            //if (regex.test(correo.val())) 

            var correo = document.getElementById('<%=txtCorreo.ClientID%>');

            if (/[\w-\.]{2,}@([\w-]{2,}\.)*([\w-]{2,}\.)[\w-]{2,4}/.test(correo))
                return false;

        }

        
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:HiddenField ID="hfIdUsuario" runat="server" Value="0" />
    <div class="TituloCaja">
        <div class="TituloPagina">
            Mantenimiento Usuarios
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
                <asp:Label ID="lblNombreUsuario" runat="server" Text="Nombre Usuario"></asp:Label>
            </div>
            <div style="float: left; width: 180px;">
                <asp:TextBox ID="txtNombreUsuario" name="txtNombreUsuario" CssClass="mayuscula" runat="server"></asp:TextBox>
            </div>
        </div>
        <br />
        <div style="float: left; padding-bottom: 8px;">
            <div style="float: left; width: 180px;">
                <asp:Label ID="Label1" runat="server" Text="Usuario"></asp:Label>
            </div>
            <div style="float: left; width: 180px;">
                <asp:TextBox ID="txtUsuario" name="txtUsuario" CssClass="mayuscula" runat="server"></asp:TextBox>
            </div>
        </div>
        <br />
        <div style="float: left; padding-bottom: 8px;">
            <div style="float: left; width: 180px;">
                <asp:Label ID="Label2" runat="server" Text="Correo"></asp:Label>
            </div>
            <div style="float: left; width: 180px;">
                <asp:TextBox ID="txtCorreo" name="txtCorreo" runat="server"></asp:TextBox>
            </div>
        </div>
        <br />
        <div style="float: left; padding-bottom: 8px;">
            <div style="float: left; width: 180px;">
                <asp:Label ID="Label3" runat="server" Text="Dominio"></asp:Label>
            </div>
            <div style="float: left; width: 180px;">
                <asp:DropDownList ID="ddlDominio" runat="server">
                </asp:DropDownList>
            </div>
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
        <div style="float: left; padding-bottom: 8px; padding-right: 5px; width: 100%">
            <asp:Label ID="lblFechaModificacion" runat="server" Text="Fecha modificaci&oacute;n:"
                CssClass="EtiquetaNormalNegrita"></asp:Label>
            <asp:Label ID="lblFechaModificacionDB" runat="server" CssClass="EtiquetaNormal" Text=""></asp:Label>
        </div>
        <div style="float: left; padding-bottom: 8px; padding-right: 5px; width: 100%;">
            <asp:Label ID="lblUsuario" runat="server" Text="Usuario modificaci&oacute;n:" CssClass="EtiquetaNormalNegrita"></asp:Label>
            <asp:Label ID="lblUsuarioDB" runat="server" CssClass="EtiquetaNormal" Text=""></asp:Label>
        </div>
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
    <div class="CentrarDiv" style="width: 90%;">
        <%--</ContentTemplate>
        </asp:UpdatePanel>--%>
        <asp:GridView ID="gvUsuario" runat="server" AutoGenerateColumns="False" EmptyDataText="No hay Usuarios."
            CellPadding="4" AllowPaging="True" OnRowCommand="gvUsuario_RowCommand" OnPageIndexChanging="gvUsuario_PageIndexChanging">
            <Columns>
                <asp:BoundField DataField="NUMERO" HeaderText="No." />
                <asp:BoundField DataField="ID_USUARIO" HeaderText="ID_USUSARIO" HeaderStyle-CssClass="Oculto"
                    FooterStyle-CssClass="Oculto" ItemStyle-CssClass="Oculto" >
<FooterStyle CssClass="Oculto"></FooterStyle>

<HeaderStyle CssClass="Oculto"></HeaderStyle>

<ItemStyle CssClass="Oculto"></ItemStyle>
                </asp:BoundField>
                <asp:BoundField DataField="USUARIO" HeaderText="USUARIO" />
                <asp:BoundField DataField="NOMBRE" HeaderText="NOMBRE" />
                <asp:BoundField DataField="CORREO" HeaderText="CORREO" />
                <asp:BoundField DataField="ID_DOMINIO" HeaderText="ID_DOMINIO"  HeaderStyle-CssClass="Oculto"
                    FooterStyle-CssClass="Oculto" ItemStyle-CssClass="Oculto"  
                    ConvertEmptyStringToNull="true" NullDisplayText="0">
<FooterStyle CssClass="Oculto"></FooterStyle>

<HeaderStyle CssClass="Oculto"></HeaderStyle>

<ItemStyle CssClass="Oculto"></ItemStyle>
                </asp:BoundField>
                <asp:BoundField DataField="DOMINIO" HeaderText="DOMINIO" />
                <asp:TemplateField HeaderText="ALTA" ItemStyle-HorizontalAlign="Center">
                    <ItemTemplate>
                        <asp:CheckBox ID="idAlta" runat="server" Style="position: static" Checked='<%#bool.Parse(Eval("idAltaUser").ToString())%>' />
                    </ItemTemplate>

<ItemStyle HorizontalAlign="Center"></ItemStyle>
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
                            CommandArgument='<%# DataBinder.Eval(Container,"RowIndex") %>' ToolTip="Dar baja Usuario" />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField>
                    <ItemTemplate>
                        <asp:ImageButton runat="server" ID="imgAlta" CommandName="Alta" ImageUrl="~/Images/up_arrow.png"
                            CommandArgument='<%# DataBinder.Eval(Container,"RowIndex") %>' ToolTip="Dar alta Usuario" />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField>
                    <ItemTemplate>
                        <asp:ImageButton runat="server" ID="imgAgregarCentro" CommandName="AgregarCentroCosto"
                            ImageUrl="~/Images/network.png" CommandArgument='<%# DataBinder.Eval(Container,"RowIndex") %>'
                            ToolTip="Agregar centro costo." />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField>
                    <ItemTemplate>
                        <asp:ImageButton runat="server" ID="imgAgregarOrden" CommandName="AgregarOrdenCompra"
                            ImageUrl="~/Images/shopping_cart.png" CommandArgument='<%# DataBinder.Eval(Container,"RowIndex") %>'
                            ToolTip="Agregar orden costo." />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField>
                    <ItemTemplate>
                        <asp:ImageButton runat="server" ID="imgAsignarRol" CommandName="AsignarRol" ImageUrl="~/Images/users.png"
                            CommandArgument='<%# DataBinder.Eval(Container,"RowIndex") %>' ToolTip="Asignar rol." />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField>
                    <ItemTemplate>
                        <asp:ImageButton runat="server" ID="imgAprobador" CommandName="AsignarAprobador"
                            ImageUrl="~/Images/checkmark.png" CommandArgument='<%# DataBinder.Eval(Container,"RowIndex") %>'
                            ToolTip="Asignar Aprobador." />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField>
                    <ItemTemplate>
                        <asp:ImageButton runat="server" ID="imgAgregaSociedaCentro" CommandName="MapeaSociedadCentro"
                            ImageUrl="~/Images/home.png" CommandArgument='<%# DataBinder.Eval(Container,"RowIndex") %>'
                            ToolTip="Mapeo sociedad centro." />
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
        <%--</ContentTemplate>
        </asp:UpdatePanel>--%>
    </div>
</asp:Content>
