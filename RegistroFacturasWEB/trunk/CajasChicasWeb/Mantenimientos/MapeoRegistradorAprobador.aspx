<%@ Page Title="" Language="C#" MasterPageFile="~/Base.Master" AutoEventWireup="true"
    CodeBehind="MapeoRegistradorAprobador.aspx.cs" Inherits="RegistroFacturasWEB.RegistroFacturas.MapeoRegistradorAprobador" %>

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
    <asp:HiddenField ID="hfIdCentro" runat="server" Value="0" />
    <div class="TituloCaja">
        <div class="TituloPagina">
            Mapeo Registrador / Aprobador
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
    <div class="CentrarDiv" style="background-color: White; width: 590px;">
        <div style="float: left; padding-bottom: 8px;">
            <div style="float: left; width: 180px;">
                <asp:Label ID="lblCeco" runat="server" Text="Centro de Costo / Orden CO"></asp:Label>
            </div>
            <div style="float: left; width: 300px;">
                <asp:TextBox ID="txtCeco" name="txtCeco" CssClass="mayuscula" runat="server"></asp:TextBox>
                <asp:Button ID="btbBuscar" class="btn btn-primary" runat="server" Text="Buscar" 
                    onclick="btbBuscar_Click" />
            </div>
        </div>
        <br />
        <div style="float: left; padding-bottom: 8px;">
            <div style="float: left; width: 180px;">
                <asp:Label ID="lblCentro" runat="server" Text="Centro"></asp:Label>
            </div>
            <div style="float: left; width: 180px;">
                <asp:DropDownList ID="ddlCentro" runat="server" Width="300px" autopostback = "true"
                    onselectedindexchanged="ddlCentro_SelectedIndexChanged">
                </asp:DropDownList>
            </div>
        </div>
        <br />
        <div style="float: left; padding-bottom: 8px;">
            <div style="float: left; width: 180px;">
                <asp:Label ID="lblAprobador" runat="server" Text="Aprobador"></asp:Label>
            </div>
            <div style="float: left; width: 180px;">
                <asp:DropDownList ID="ddlAprobador" runat="server" Width="300px" autopostback = "true"
                    onselectedindexchanged="ddlAprobador_SelectedIndexChanged">
                </asp:DropDownList>
            </div>
        </div>
        <br />
        <div style="float: left; padding-bottom: 8px;">
            <div style="float: left; width: 180px;">
                <asp:Label ID="lblRegistrador" runat="server" Text="Registrador"></asp:Label>
            </div>
            <div style="float: left; width: 180px;">
                <asp:DropDownList ID="ddlRegistrador" runat="server" Width="300px" autopostback = "true"
                    onselectedindexchanged="ddlRegistrador_SelectedIndexChanged">
                </asp:DropDownList>
            </div>
        </div>
        <br />
        <div style="float: right; padding-bottom: 8px; padding-right: 5px;">
            <div style="float: left; padding-left: 5px;">
                <asp:Button ID="btbAsignar" class="btn btn-primary" runat="server" 
                    Text="Asignar" onclick="btbAsignar_Click" />
            </div>
            <div style="float: left; padding-left: 10px;">
                <asp:Button ID="btnLimpiar" class="btn" runat="server" Text="Limpiar" 
                    onclick="btnLimpiar_Click1" />
            </div>
        </div>
    </div>
    <div style="display: block; padding-top: 10px;">
    </div>
    <div class="CentrarDiv" style="width: 630px;">
        <asp:GridView ID="gvMapeo" runat="server" AutoGenerateColumns="False" EmptyDataText="No hay Asignaciones."
            CellPadding="4" GridLines="None" AllowPaging="True" 
            onpageindexchanging="gvMapeo_PageIndexChanging1" 
            onrowcommand="gvMapeo_RowCommand">
            <%-- OnRowCommand="gvMapeo_RowCommand">--%>
            <Columns>
                <asp:BoundField DataField="NUMERO" HeaderText="NUMERO" />
                <asp:BoundField DataField="CECO_ORDEN" HeaderText="CECO / ORDEN" />
                <asp:BoundField DataField="ID_CENTRO" HeaderText="ID_CENTRO" HeaderStyle-CssClass="Oculto"
                    FooterStyle-CssClass="Oculto" ItemStyle-CssClass="Oculto" />
                <asp:BoundField DataField="CENTRO" HeaderText="CENTRO" />
                <asp:BoundField DataField="APROBADOR_US" HeaderText="APROBADOR_US" HeaderStyle-CssClass="Oculto"
                    FooterStyle-CssClass="Oculto" ItemStyle-CssClass="Oculto" />
                <asp:BoundField DataField="APROBADOR" HeaderText="APROBADOR" />
                <asp:BoundField DataField="REGISTRADOR_US" HeaderText="REGISTRADOR_US" HeaderStyle-CssClass="Oculto"
                    FooterStyle-CssClass="Oculto" ItemStyle-CssClass="Oculto" />
                <asp:BoundField DataField="REGISTRADOR" HeaderText="REGISTRADOR" />
                <asp:BoundField DataField="USUARIO_ALTA" HeaderText="USUARIO ALTA" />
                <asp:TemplateField>
                    <ItemTemplate>
                        <asp:ImageButton runat="server" ID="Baja" CommandName="Baja" ImageUrl="~/Images/down_arrow.png"
                            CommandArgument='<%# DataBinder.Eval(Container,"RowIndex") %>' ToolTip="Quitar"
                            Visible='<%#( Convert.ToDecimal(DataBinder.Eval(Container.DataItem, "ESTADO")) == 1 ) ? true : false %>' />
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
    </div>
</asp:Content>
