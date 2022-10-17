<%@ Page Title="" Language="C#" MasterPageFile="~/Base.Master" AutoEventWireup="true"
    CodeBehind="SuperUsuario.aspx.cs" Inherits="RegistroFacturasWEB.Seguridad.SuperUsuario" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="../Scripts/jquery-1.10.2.js" type="text/javascript"></script>
    <script src="../Scripts/jquery-1.7.2.min.js" type="text/javascript"></script>
    <script type="text/javascript" src="/Scripts/jquery.min.js"></script>
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
                        '<%=txtJustificacion.UniqueID %>': { maxlength: 150, required: true }
                    },
                    messages: {
                        '<%=txtJustificacion.UniqueID %>': { required: "* Ingrese Justificación", maxlength: "La Justificación debe de contener menos de 150 Caracteres" }
                    }
                });
            });
        }

       
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:HiddenField ID="hfIdFactura" runat="server" Value="0" />
    <asp:HiddenField ID="hfIdCC" runat="server" Value="0" />
    <asp:HiddenField ID="hfEstado" runat="server" Value="0" />
    <div class="TituloCaja">
        <div class="TituloPagina">
            Modificación de Estados para Facturas
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
        <div style="float: left; padding-bottom: 28px;">
            <div style="float: left; width: 130px;">
                <asp:Label ID="lblFactura" runat="server" Text="FACTURA"></asp:Label>
            </div>
            <div style="float: left; width: 800px;">
                <div style="float: left; width: 55px;">
                    <asp:Label ID="lblNumero" runat="server" Text="Numero"></asp:Label>
                </div>
                <div style="float: left; width: 170px;">
                    <asp:TextBox ID="txtNumeroFact" name="txtNumeroFact" CssClass="mayuscula" runat="server"
                        Width="150px"></asp:TextBox>
                </div>
                <div style="float: left; width: 85px;">
                    <asp:Label ID="lblSerie" runat="server" Text="Serie"></asp:Label>
                </div>
                <div style="float: left; width: 150px;">
                    <asp:TextBox ID="txtSerie" name="txtSerie" CssClass="mayuscula" runat="server" Width="100px"></asp:TextBox>
                </div>
                <br />
                <div style="float: left; width: 55px;">
                    <asp:Label ID="lblPais" runat="server" Text="Sociedad"></asp:Label>
                </div>
                <div style="float: left; width: 170px;">
                    <asp:DropDownList ID="ddlSociedad" runat="server" Width="155px"  AutoPostBack="True" 
                        onselectedindexchanged="ddlSociedad_SelectedIndexChanged">
                    </asp:DropDownList>
               <%--     <asp:TextBox ID="txtPais" name="txtPais" CssClass="mayuscula" runat="server" AutoPostBack="True" Width="150px"
                        OnTextChanged="txtPais_TextChanged"></asp:TextBox>--%>
                </div>
                <div style="float: left; width: 185px;">
                    <asp:Label ID="lblProveedor" runat="server" Text="Provedor" Width="250px"></asp:Label>
                </div>
                <br />
                <div style="float: left; width: 55px;">
                    <asp:Label ID="lblDocumento" runat="server" Text="Doc."></asp:Label>
                </div>
                <div style="float: left; width: 170px;">
                    <asp:DropDownList ID="ddlTipoDocumento" runat="server" Width="155px">
                    </asp:DropDownList>
                </div>
                <div style="float: left; width: 80px;">
                    <asp:Label ID="lblNumDoc" runat="server" Text="Numero"></asp:Label>
                </div>
                <div style="float: left; width: 380px;">
                    <asp:TextBox ID="txtNumDoc" name="txtNumDoc" CssClass="mayuscula" runat="server"></asp:TextBox>
                </div>
                <div style="float: left; width: 100px;">
                    <div style="float: left; padding-left: 10px;">
                        <asp:Button ID="btbBuscarFact" class="btn" runat="server" Text="Buscar" OnClick="btnBuscar_Click" />
                    </div>
                </div>
            </div>
            <br />
            <br />
            <div style="float: left; width: 350px;">
                <div style="float: left; width: 250px;">
                    <asp:Label ID="lblEstado" runat="server" Text="Estado Actual: "></asp:Label>
                </div>
                <br />
                <div style="float: left; width: 250px;">
                    <asp:Label ID="lblIdFact" runat="server" Text="Id Factura: "></asp:Label>
                </div>
                <br />
                <div style="float: left; width: 250px;">
                    <asp:Label ID="lblMonto" runat="server" Text="Monto: "></asp:Label>
                </div>
                <br />
                <div style="float: left; width: 100px;">
                    <asp:Label ID="lblDividida" runat="server" Text="Dividida: "></asp:Label>
                </div>
                <br />
                <div style="float: left; width: 150px;">
                    <asp:Label ID="lblFecha" runat="server" Text="Fecha: "></asp:Label>
                </div>
                <br />
                <div style="float: left; width: 100px;">
                    <asp:Label ID="lblIdCCF" runat="server" Text="ID CC: "></asp:Label>
                </div>
                <br />
                <div style="float: left; width: 250px;">
                    <asp:Label ID="lblCCF" runat="server" Text="Caja Chica: "></asp:Label>
                </div>
            </div>
            <div style="float: left; width: 600px;">
                <div style="float: left; width: 90px;">
                    <asp:Label ID="lblEstadoN" runat="server" Text="Nuevo"></asp:Label>
                </div>
                <div style="float: left; width: 250px;">
                    <asp:DropDownList ID="ddlEstado" runat="server">
                    </asp:DropDownList>
                </div>
                <br />
                <div style="float: left; width: 90px;">
                    <asp:Label ID="lblJustificacion" runat="server" Text="Justificación: "></asp:Label>
                </div>
                <div style="float: left; width: 380px;">
                    <asp:TextBox ID="txtJustificacion" name="txtJustificacion" runat="server" TextMode="MultiLine"
                        Width="350px" Height="80px"></asp:TextBox>
                </div>
                <div style="float: left; width: 100px;">
                    <div style="float: left; padding-left: 10px;">
                        <asp:Button ID="btbGrabarFact" class="btn btn-primary" runat="server" Text="Guardar"
                            OnClick="btnGrabarFact_Click" OnClientClick="Validar()" />
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
