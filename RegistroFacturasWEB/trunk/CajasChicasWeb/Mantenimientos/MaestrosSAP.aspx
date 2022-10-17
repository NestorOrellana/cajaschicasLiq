<%@ Page Title="" Language="C#" MasterPageFile="~/Base.Master" AutoEventWireup="true"
    CodeBehind="MaestrosSAP.aspx.cs" Inherits="RegistroFacturasWEB.Mantenimientos.MaestrosSAP" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="../Scripts/jquery-1.10.2.js" type="text/javascript"></script>
    <script src="../Scripts/jquery-ui.min.js" type="text/javascript"></script>
    <link href="../App_Themes/Tema/jquery-ui.css" rel="stylesheet" type="text/css" />
    <script src="/bootstrap/js/bootstrap.js" type="text/javascript"></script>
    <link href="/bootstrap/css/bootstrap.css" rel="stylesheet" type="text/css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="TituloCaja">
        <div class="TituloPagina">
            Migrar centros SAP.
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
        <div style="float: left; padding-bottom: 8px; width: 100%">
            <fieldset style="width: 99%;">
                <legend>Sociedades</legend>
            </fieldset>
            <div style="float: left; padding-bottom: 8px;">
                <div style="float: left; width: 180px;">
                    <asp:Label ID="Label4" runat="server" Text="Sociedad"></asp:Label>
                </div>
                <div style="float: left; width: 300px;">
                    <asp:DropDownList ID="ddlSociedad" Width="300px" runat="server">
                    </asp:DropDownList>
                </div>
            </div>
        </div>
        <div style="float: left; padding-bottom: 8px;">
            <fieldset style="width: 99%;">
                <legend>Centro costo</legend>
            </fieldset>
            <div style="float: left; padding-bottom: 8px;">
                <div style="float: left; width: 180px;">
                    <asp:Label ID="lblNombre" runat="server" Text="Importar centros costos:"></asp:Label>
                </div>
                <div style="float: left; width: 180px;">
                    <asp:Button ID="btnCentroCosto" runat="server" Text="Migrar" class="btn" OnClick="btnCentroCosto_Click" />
                </div>
            </div>
        </div>
        <div style="float: left; padding-bottom: 8px;">
            <fieldset style="width: 99%;">
                <legend>Orden costo</legend>
            </fieldset>
            <div style="float: left; padding-bottom: 8px;">
                <div style="float: left; width: 180px;">
                    <asp:Label ID="Label1" runat="server" Text="Importar ordenes costo"></asp:Label>
                </div>
                <div style="float: left; width: 180px;">
                    <asp:Button ID="btnOrdenCosto" runat="server" Text="Migrar" class="btn" OnClick="btnOrdenCosto_Click" />
                </div>
            </div>
        </div>
        <div style="float: left; padding-bottom: 8px;">
            <fieldset style="width: 99%;">
                <legend>Cuentas contables</legend>
            </fieldset>
            <div style="float: left; padding-bottom: 8px;">
                <div style="float: left; width: 180px;">
                    <asp:Label ID="Label2" runat="server" Text="Importar cuentas contables"></asp:Label>
                </div>
                <div style="float: left; width: 180px;">
                    <asp:Button ID="btnCuentaContable" runat="server" Text="Migrar" class="btn" 
                        onclick="btnCuentaContable_Click" />
                </div>
            </div>
        </div>
        <div style="float: left; padding-bottom: 8px;">
            <fieldset style="width: 99%;">
                <legend>Proveedores esporadicos</legend>
            </fieldset>
            <div style="float: left; padding-bottom: 8px;">
                <div style="float: left; width: 180px;">
                    <asp:Label ID="Label3" runat="server" Text="Importar proveedores exporadicos:"></asp:Label>
                </div>
                <div style="float: left; width: 180px;">
                    <asp:Button ID="btnCajaChica" runat="server" Text="Migrar" class="btn" OnClick="btnCajaChica_Click" />
                </div>
            </div>
        </div>
    </div>
</asp:Content>
