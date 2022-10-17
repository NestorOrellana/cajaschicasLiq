<%@ Page Title="" Language="C#" MasterPageFile="~/Base.Master" AutoEventWireup="true"
    CodeBehind="MapeoUsuarioCaja.aspx.cs" Inherits="RegistroFacturasWEB.RegistroFacturas.MapeoUsuarioCaja" %>

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
            Mapeo Usuario Caja
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
                <asp:Label ID="lblNombre" runat="server" Text="Sociedad"></asp:Label>
            </div>
            <div style="float: left; width: 180px;">
                <asp:DropDownList ID="ddlSociedad" runat="server" Width="300px">
                </asp:DropDownList>
            </div>
        </div>
        <br />
          <div style="float: left; padding-bottom: 8px;">
            <div style="float: left; width: 180px;">
                <asp:Label ID="lblBuscar" runat="server" Text="Buscar"></asp:Label>
            </div>
            <div style="float: left; width: 180px;">
                <asp:TextBox ID="txtBuscar" name ="txtBuscar" CssClass="mayuscula" runat="server"></asp:TextBox>
            </div>
        </div>
       
        <br />
      
        <div style="float: right; padding-bottom: 8px; padding-right: 5px;">
            <div style="float: left; padding-left: 5px;">
                <asp:Button ID="btnVisualizar" class="btn btn-primary" runat="server" Text="Visualizar" OnClick="btnVisualizar_Click"
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
    <div class="CentrarDiv" style="width: 550px;">
        <asp:GridView ID="gvMapeo" runat="server" AutoGenerateColumns="False" EmptyDataText="No hay Asignaciones."
            CellPadding="4" GridLines="None" AllowPaging="True" OnRowCommand="gvMapeo_RowCommand"
            onpageindexchanging="gvMapeo_PageIndexChanging">
            <Columns>
                <asp:BoundField DataField="NUMERO" HeaderText="No." />
                <asp:BoundField DataField="BUKRS" HeaderText="SOCIEDAD"/>
                <asp:BoundField DataField="LIFNR" HeaderText="PROVEEDOR" />
                <asp:BoundField DataField="NAME" HeaderText="DESCRIPCION" />

                <asp:TemplateField HeaderText="ESTADO" ItemStyle-HorizontalAlign="Center">
                    <ItemTemplate>
                        <asp:CheckBox Enabled="false" ID="Estado" runat="server" Style="position: static" Checked='<%#bool.Parse(Eval("Estado").ToString())%>'  />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField>
                    <ItemTemplate>
                        <asp:ImageButton runat="server" ID="Alta" CommandName="Alta" ImageUrl="~/Images/up_arrow.png"
                            CommandArgument='<%# DataBinder.Eval(Container,"RowIndex") %>' ToolTip="Agregar" 
                            Visible='<%#( Convert.ToDecimal(DataBinder.Eval(Container.DataItem, "ESTADO")) == 0 ) ? true : false %>' />
                            <asp:ImageButton runat="server" ID="Baja" CommandName="Baja" ImageUrl="~/Images/down_arrow.png"
                            CommandArgument='<%# DataBinder.Eval(Container,"RowIndex") %>' ToolTip="Quitar"
                            Visible='<%#( Convert.ToDecimal(DataBinder.Eval(Container.DataItem, "ESTADO")) == 1 ) ? true : false %>' />
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
    </div>
</asp:Content>
