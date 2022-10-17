<%@ Page Title="" Language="C#" MasterPageFile="~/Base.Master" AutoEventWireup="true"
    CodeBehind="Kilometraje.aspx.cs" Inherits="RegistroFacturasWEB.Mantenimientos.Kilometraje" %>

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
        function Limpiar() {
            var nuovourl = window.location.href + '';
            nuovourl = nuovourl + (nuovourl.indexOf('?') > -1 ? "&refreshme=1" : "?refreshme=1");
            window.location.href = nuovourl;
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:HiddenField ID="hfCodigoKilometraje" runat="server" Value="0" />
    <div class="TituloCaja">
        <div class="TituloPagina">
            Mantenimiento de Kilometraje
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
                <asp:Label ID="LabelPais" runat="server" Text="País:"></asp:Label>
            </div>
            <asp:DropDownList ID="DropDownListPais" runat="server" Style="border: solid 1px #ccc;
                width: 210px;">
            </asp:DropDownList>
        </div>
        <br />
        <div style="float: left; padding-bottom: 8px;">
            <div style="float: left; width: 180px;">
                <asp:Label ID="lblOrigen" runat="server" Text="Origen:"></asp:Label>
            </div>
            <input type="text" id="txtOrigen" class="mayuscula" runat="server" />
        </div>
        <br />
         <div style="float: left; padding-bottom: 8px;">
            <div style="float: left; width: 180px;">
                <asp:Label ID="lblDestino" runat="server" Text="Destino:"></asp:Label>
            </div>
            <input type="text" id="txtDestino" class="mayuscula" runat="server" />
        </div>
        <br />
         <div style="float: left; padding-bottom: 8px;">
            <div style="float: left; width: 180px;">
                <asp:Label ID="lblKilometros" runat="server" Text="Km:"></asp:Label>
            </div>
            <input type="text" id="txtKilometros" class="mayuscula" runat="server" />
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
                <asp:Button ID="btnGrabar" class="btn btn-primary" runat="server" Text="Grabar" OnClick="btnGrabar_Click"/>
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
        <asp:GridView ID="gvKilometrajes" runat="server" AutoGenerateColumns="False" EmptyDataText="No hay registros"
            CellPadding="4" GridLines="None" AllowPaging="True" OnRowCommand="gvKilometrajes_RowCommand"
            OnPageIndexChanging="gvKilometrajes_PageIndexChanging">
            <Columns>
                <asp:BoundField DataField="CODIGO_KILOMETRAJE" HeaderText="C&Oacute;DIGO KILOMETRAJE" />
                <asp:BoundField DataField="PAIS" HeaderText="PAIS" />
                <asp:BoundField DataField="ORIGEN" HeaderText="ORIGEN" />
                <asp:BoundField DataField="DESTINO" HeaderText="DESTINO" />
                <asp:BoundField DataField="KILOMETROS" HeaderText="KM" />
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
                            CommandArgument='<%# DataBinder.Eval(Container,"RowIndex") %>' ToolTip="Dar baja kilometraje" />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField>
                    <ItemTemplate>
                        <asp:ImageButton runat="server" ID="imgAlta" CommandName="Alta" ImageUrl="~/Images/up_arrow.png"
                            CommandArgument='<%# DataBinder.Eval(Container,"RowIndex") %>' ToolTip="Dar alta kilometraje" />
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
    </div>
</asp:Content>
