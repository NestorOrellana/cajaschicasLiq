<%@ Page Title="" Language="C#" MasterPageFile="~/Base.Master" AutoEventWireup="true"
    CodeBehind="SociedadMoneda.aspx.cs" Inherits="RegistroFacturasWEB.Mantenimientos.SociedadMoneda"
    EnableEventValidation="false" %>

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
            width: 14em;
            margin-right: 2em;
        }
        
        checkbox
        {
            float: left;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:HiddenField ID="hfIdSociedadMoneda" runat="server" Value="0" />
    <asp:HiddenField ID="hfIdSociedad" runat="server" Value="0" />
    <asp:HiddenField ID="hfModeda" runat="server" Value="0" />
    <asp:HiddenField ID="hfIdUsuario" runat="server" Value="0" />
    <div class="TituloCaja">
        <div class="TituloPagina">
            Asignación de Socieda Moneda
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
    <div class="CentrarDiv" style="background-color: White; width: 660px;">
        <div style="float: left; padding-bottom: 8px; width: 644px;">
            <div style="float: left; width: 180px;">
                <asp:Label ID="lbl" runat="server" Text="Sociedad:"></asp:Label>
            </div>
            <div style="float: left; width: 450px;">
                <asp:DropDownList ID="ddlSociedad" runat="server" Width="387px">
                </asp:DropDownList>
            </div>
            <br />
            <br />
            <br />
            <div style="float: right; padding-bottom: 8px; padding-right: 5px;">
                <div style="float: left; padding-left: 5px;">
                    <asp:Button ID="btnGrabar" class="btn btn-primary" runat="server" Text="Grabar" 
                        onclick="btnGrabar_Click" />
                </div>
                <div style="float: left; padding-left: 5px;">
                    <asp:Button ID="btnBuscar" class="btn" runat="server" Text="Buscar" OnClick="btnBuscar_Click" />
                </div>
                <div style="float: left; padding-left: 10px;">
                    <asp:Button ID="btnLimpiar" class="btn" runat="server" Text="Limpiar" 
                        onclick="btnLimpiar_Click" />
                </div>
            </div>
        </div>
    </div>
    <div style="display: block; padding-top: 10px;">
    </div>
    <div class="CentrarDiv" style="width: 900px; background-color: White">
        <asp:CheckBoxList ID="cblMonedas" runat="server" RepeatColumns="3" RepeatDirection="Vertical">
        </asp:CheckBoxList>
    </div>
</asp:Content>
