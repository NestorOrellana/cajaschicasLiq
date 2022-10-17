<%@ Page Title="" Language="C#" MasterPageFile="~/Base.Master" AutoEventWireup="true"
    CodeBehind="ListadoAprobadores.aspx.cs" Inherits="RegistroFacturasWEB.Aprobaciones.ListadoAprobadores" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript" src="/Scripts/jquery.min.js"></script>
    <script type="text/javascript" src="/Scripts/jquery.validate.js"></script>
    <script type="text/javascript">

        //Validacion de Campos
        function Validar() {
            $(document).ready(function () {
                $("#form1").validate({
                    rules: {
                        '<%= txtUsuario.UniqueID %>': { required: true },
                        '<%=txtPorcentaje.UniqueID %>': { required: true },
                        '<%=txtTolerancia.UniqueID %>': { required: true }
                    },
                    messages: {
                        '<%= txtUsuario.UniqueID %>': { required: "*Ingrese Usuario" },
                        '<%=txtPorcentaje.UniqueID %>': { required: "*Ingrese Porcentaje" },
                        '<%=txtTolerancia.UniqueID %>': { required: "*Ingrese Tolerancia" }
                    }
                });
            });
        }

        function ValidarNumero(numObj) {

            var numero = numObj.value;

            if (!/^[0-9]+(\.[0-9]{1,2})?$/.test(numero))
                numObj.value = numero.substring(0, numero.length - 1);
        }

        function Limpiar() {
            var nuovourl = window.location.href + '';
            nuovourl = nuovourl + (nuovourl.indexOf('?') > -1 ? "&refreshme=1" : "?refreshme=1");
            window.location.href = nuovourl;

        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:HiddenField ID="hfIdAprobadorCentro" runat="server" Value="0" />
    <asp:HiddenField ID="hfIdUsuario" runat="server" Value="0" />
    <asp:HiddenField ID="hfUsuario" runat="server" Value="0" />
    <div class="TituloCaja">
        <div class="TituloPagina">
            Aprobadores
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
    <div class="CentrarDiv" style="background-color: White; width: 500px;">
        <div style="float: left; width: 450px; padding-left: 73px">
            <div style="float: left; margin-top: 10px; padding-left: 8px;">
                <asp:Label ID="Label7" runat="server" Text="Usuario:"></asp:Label>
                <asp:TextBox ID="txtUsuario" name="txtUsuario" Text="1" runat="server" Style="width: 280px;"></asp:TextBox>
            </div>
        </div>
        <br />
        <div style="float: left; width: 450px; padding-left: 0px">
            <div style="float: left; margin-top: 10px; padding-left: 8px;">
                <asp:Label ID="Label3" runat="server" Text="Porcentaje Compra:"></asp:Label>
                <asp:TextBox ID="txtPorcentaje" name="txtPorcentaje" OnKeyUp="ValidarNumero(this)"
                    runat="server" Style="width: 280px;"></asp:TextBox>
            </div>
        </div>
        <br />
        <div style="float: left; width: 420px; padding-left: 55px">
            <div style="float: left; margin-top: 10px; padding-left: 8px;">
                <asp:Label ID="Label4" runat="server" Text="Tolerancia:"></asp:Label>
                <asp:TextBox ID="txtTolerancia" name="txtTolerancia" OnKeyUp="ValidarNumero(this)"
                    runat="server" Style="width: 280px;"></asp:TextBox>
            </div>
        </div>
        <br />
        <div style="float: left; width: 200px; padding-left: 105px">
            <div style="float: left; margin-top: 10px; padding-left: 1px;">
                <asp:Label ID="Label5" runat="server" Text="Alta:"></asp:Label>
                <asp:CheckBox ID="chAlta" runat="server" />
            </div>
        </div>
        <br />
        <br />
        <div style="float: left; padding-bottom: 8px; padding-right: 5px; width: 100%">
            <asp:Label ID="Label6" runat="server" Text="Usuario Creacion:"></asp:Label>
            <asp:Label ID="lblUsuarioCreacionBD" runat="server" Text=""></asp:Label>
        </div>
        <div style="float: left; padding-bottom: 8px; padding-right: 5px; width: 100%;">
            <asp:Label ID="Label8" runat="server" Text="Fecha Creacion:"></asp:Label>
            <asp:Label ID="lblFechaCreacionBD" runat="server" Text=""></asp:Label>
        </div>
        <br />
        <div style="float: left; padding-bottom: 8px; padding-right: 5px; width: 100%">
            <asp:Label ID="lblFechaModificacion" runat="server" Text="Fecha modificacion:"></asp:Label>
            <asp:Label ID="lblFechaModificacionDB" runat="server" Text=""></asp:Label>
        </div>
        <div style="float: left; padding-bottom: 8px; padding-right: 5px; width: 100%;">
            <asp:Label ID="lblUsuario" runat="server" Text="Usuario modificacion:"></asp:Label>
            <asp:Label ID="lblUsuarioDB" runat="server" Text=""></asp:Label>
        </div>
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
        <div style="display: block; padding-top: 5px;">
        </div>
    </div>
    <div style="display: block; padding-top: 5px;">
    </div>
    <div class="CentrarDiv" style="width: 800px;">
        <asp:GridView ID="gvAprobadores" runat="server" AutoGenerateColumns="False" EmptyDataText="No hay Datos para Mostrar."
            CellPadding="4" AllowPaging="True" OnRowCommand="gvAprobadores_RowCommand"
            onpageindexchanging="gvAprobadores_PageIndexChanging">
            <Columns>
                <asp:BoundField DataField="NUMERO" HeaderText="No." />
                <asp:BoundField DataField="ID_APROBACION_ENCABEZADO" HeaderText="ID" HeaderStyle-CssClass="Oculto"
                    FooterStyle-CssClass="Oculto" ItemStyle-CssClass="Oculto" />
                <asp:BoundField DataField="ID_USUARIO" HeaderText="ID USUARIO" HeaderStyle-CssClass="Oculto"
                    FooterStyle-CssClass="Oculto" ItemStyle-CssClass="Oculto" />
                <asp:BoundField DataField="USUARIO" HeaderText="USUARIO" />
                <asp:BoundField DataField="PORCENTAJE" HeaderText="PORCENTAJE" />
                <asp:BoundField DataField="TOLERANCIA" HeaderText="TOLERANCIA" />
                <asp:TemplateField HeaderText="ALTA" ItemStyle-HorizontalAlign="Center">
                    <ItemTemplate>
                        <asp:CheckBox ID="idAlta" runat="server" Style="position: static" Checked='<%#bool.Parse(Eval("idAlta").ToString())%>' />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="USUARIO_CREACION" HeaderText="USUARIO CREACI&Oacute;N" />
                <asp:BoundField DataField="FECHA_CREACION" HeaderText="FECHA CREACI&Oacute;N" />
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
                            CommandArgument='<%# DataBinder.Eval(Container,"RowIndex") %>' ToolTip="Dar baja Aprobador Centro" />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField>
                    <ItemTemplate>
                        <asp:ImageButton runat="server" ID="imgAlta" CommandName="Alta" ImageUrl="~/Images/up_arrow.png"
                            CommandArgument='<%# DataBinder.Eval(Container,"RowIndex") %>' ToolTip="Dar alta Aprobador Centro" />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField>
                    <ItemTemplate>
                        <asp:ImageButton runat="server" ID="imgDetalle" CommandName="Detalle" ImageUrl="~/Images/pages.png"
                            CommandArgument='<%# DataBinder.Eval(Container,"RowIndex") %>' ToolTip="Agregar Detalle"
                            Visible='<%#( Convert.ToDecimal(DataBinder.Eval(Container.DataItem, "EstadoDetalle")) == 0 ) ? false : true %>' />
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
    </div>
</asp:Content>
