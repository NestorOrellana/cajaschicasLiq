<%@ Page Title="" Language="C#" MasterPageFile="~/Base.Master" AutoEventWireup="true"
    CodeBehind="SuperUsuarioCC.aspx.cs" Inherits="RegistroFacturasWEB.Seguridad.SuperUsuarioCC" %>

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

        function Validar() {
            $(document).ready(function () {
                $("#form1").validate({
                    rules: {
                        '<%=txtJustificacionCC.UniqueID %>': { maxlength: 150, required: true }
                    },
                    messages: {
                        '<%=txtJustificacionCC.UniqueID %>': { required: "* Ingrese Justificación", maxlength: "La Justificación debe de contener menos de 150 Caracteres" }
                    }
                });
            });
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:HiddenField ID="hfEstado" runat="server" Value="0" />
    <asp:HiddenField ID="hfIdCC" runat="server" Value="0" />
    <div class="TituloCaja">
        <div class="TituloPagina">
            Modificación de Estados de Caja Chica
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
    <div class="CentrarDiv" style="background-color: White; width: 960px; height: 250px;">
        <div style="float: left; padding-bottom: 8px;">
        <br />
            <div style="float: left; width: 350px;">
                <asp:Label ID="lblCC" runat="server" Text="CAJA CHICA"></asp:Label>
            </div>
            <div style="float: left; width: 90px;">
                <asp:Label ID="lblNumeroCC" runat="server" Text="Numero"></asp:Label>
            </div>
            <div style="float: left; width: 380px;">
                <asp:TextBox ID="txtNumeroCC" name="txtNumeroCC" CssClass="mayuscula" runat="server"
                    Width="350px"></asp:TextBox>
            </div>
            <div style="float: left; width: 100px;">
                <div style="float: left; padding-left: 10px;">
                    <asp:Button ID="btbBuscar" class="btn" runat="server" Text="Buscar" OnClick="btnBuscar_Click"/>
                </div>
            </div>
            <br />
            <div style="float: left; width: 350px;">
                <div style="float: left; width: 300px;">
                    <asp:Label ID="lblEstadoCC" runat="server" Text="Estado Actual: "></asp:Label>
                </div>
                <br />
                <div style="float: left; width: 300px;">
                    <asp:Label ID="lblIdCC" runat="server" Text="Id CC: "></asp:Label>
                </div>
                <br />
                <div style="float: left; width: 300px;">
                    <asp:Label ID="lblNombreCC" runat="server" Text="Nombre: "></asp:Label>
                </div>
            </div>
            <div style="float: left; width: 600px;">
                <div style="float: left; width: 90px;">
                    <asp:Label ID="lblEstadoCCN" runat="server" Text="Nuevo"></asp:Label>
                </div>
                <div style="float: left; width: 250px;">
                    <asp:DropDownList ID="ddlEstado" runat="server">
                    </asp:DropDownList>
                </div>
                <br />
                <div style="float: left; width: 90px;">
                    <asp:Label ID="lblJustificacionCC" runat="server" Text="Justificación: "></asp:Label>
                </div>
                <div style="float: left; width: 380px;">
                    <asp:TextBox ID="txtJustificacionCC" name="txtJustificacionCC" runat="server" textmode =MultiLine
                        Width="350px" Height="80px"></asp:TextBox>
                </div>
                <div style="float: left; width: 100px;">
                    <div style="float: left; padding-left: 10px;">
                        <asp:Button ID="btbGuardarCC" class="btn btn-primary" runat="server" Text="Guardar"
                        OnClick="btnGrabarFact_Click" OnClientClick="Validar()"/>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
