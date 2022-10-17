<%@ Page Title="" Language="C#" MasterPageFile="~/Base.Master" AutoEventWireup="true"
    CodeBehind="SuperUsuarioSincro.aspx.cs" Inherits="RegistroFacturasWEB.Seguridad.SuperUsuarioSincro" %>

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
        $(document).ready(function () {
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

            $('#<%=txtFechaFactura.ClientID%>').datepicker({
                dateFormat: 'yy/mm/dd'
            });

        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:HiddenField ID="hfIdEstado" runat="server" Value="0" />
    <div class="TituloCaja">
        <div class="TituloPagina">
            Anulacion de Sincronizacion
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
    <div class="CentrarDiv" style="background-color: White; width: 960px; height: 200px;">
        <div style="float: left; padding-bottom: 8px;">
            <div style="float: left; width: 180px;">
                <asp:Label ID="lblTraslado" runat="server" Text="SINCRONIZACION"></asp:Label>
            </div>
            <br />
            <br />
            <div style="float: left; width: 90px;">
                <asp:Label ID="lblDocumentoCorr" runat="server" Text="Documento"></asp:Label>
            </div>
            <div style="float: left; width: 250px;">
                <asp:TextBox ID="txtDocumento" name="txtDocumento" CssClass="mayuscula" runat="server"></asp:TextBox>
            </div>
            <div style="float: left; width: 90px;">
                <asp:Label ID="lblNoFactura" runat="server" Text="No. Factura"></asp:Label>
            </div>
            <div style="float: left; width: 250px;">
                <asp:TextBox ID="txtNoFactura" name="txtNoFactura" CssClass="mayuscula" runat="server"></asp:TextBox>
            </div>
            <div style="float: left; width: 80px;">
                <asp:Label ID="lblSerie" runat="server" Text="Serie"></asp:Label>
            </div>
            <div style="float: left; width: 90px;">
                <asp:TextBox ID="txtSerie" name="txtSerie" CssClass="mayuscula" runat="server" Width="150px"></asp:TextBox>
            </div>
            <br />
            <div style="float: left; width: 90px;">
                <asp:Label ID="lblSociedad" runat="server" Text="Sociedad"></asp:Label>
            </div>
            <div style="float: left; width: 90px;">
                <asp:DropDownList ID="ddlSociedad" runat="server" Width="150px">
                </asp:DropDownList>
                <%--  <asp:TextBox ID="txtSociedad" name="txtSociedad" CssClass="mayuscula" runat="server"
                    Width="50px" MaxLength="4"></asp:TextBox>--%>
            </div>
            <br />
            <div style="float: left; width: 90px;">
                <asp:Label ID="lblFechaFactura" runat="server" Text="Fecha Factura"></asp:Label>
            </div>
            <div style="float: left; width: 250px;">
                <asp:TextBox ID="txtFechaFactura" Text="" CssClass="mayuscula" runat="server"></asp:TextBox>
            </div>
            <div style="float: left; width: 90px;">
                <asp:Label ID="lblDocProveedor" runat="server" Text="Doc. Proveedor"></asp:Label>
            </div>
            <div style="float: left; width: 250px;">
                <asp:TextBox ID="txtDocProveedor" name="txtDocProveedor" CssClass="mayuscula" runat="server"></asp:TextBox>
            </div>
            <div style="float: left; width: 100px;">
                <div style="float: left; padding-left: 10px;">
                    <asp:Button ID="btbBuscarSincro" class="btn" runat="server" Text="Buscar" OnClick="btnBuscar_Click" />
                </div>
            </div>
            <br />
            <div style="float: left; width: 350px;">
                <div style="float: left; width: 300px;">
                    <asp:Label ID="hlblNFactura" runat="server" Visible="false"></asp:Label>
                    <asp:Label ID="lblNFactura" runat="server" Text="Numero de Factura: "></asp:Label>
                </div>
                <br />
                <div style="float: left; width: 300px;">
                    <asp:Label ID="hlblNSerie" runat="server" Visible="false"></asp:Label>
                    <asp:Label ID="lblNSerie" runat="server" Text="Serie: "></asp:Label>
                </div>
                <br />
                <div style="float: left; width: 300px;">
                    <asp:Label ID="lblEstadoSincro" runat="server" Text="Estado Actual: "></asp:Label>
                </div>
                <br />
                <div style="float: left; width: 300px;">
                    <asp:Label ID="lblDescripcion" runat="server" Text="Descripcion: "></asp:Label>
                </div>
                <br />
                <div style="float: left; width: 300px;">
                    <asp:Label ID="lblProveedor" runat="server" Text="Proveedor: "></asp:Label>
                </div>
                <br />
                <div style="float: left; width: 300px;">
                    <asp:Label ID="hlblDocId" runat="server" Visible="false"></asp:Label>
                    <asp:Label ID="lblDocId" runat="server" Text="Documento de Identificación: "></asp:Label>
                </div>
                <br />
                <div style="float: left; width: 300px;">
                    <asp:Label ID="hlblCodSincro" runat="server" Visible="false"></asp:Label>
                    <asp:Label ID="lblCodSincro" runat="server" Text="Codigo de Sincronizacion: "></asp:Label>
                </div>
                <br />
            </div>
            <div style="float: left; width: 600px;">
                <div style="float: left; width: 90px;">
                    <asp:Label ID="lblJuestificacion" runat="server" Text="Justificación: "></asp:Label>
                </div>
                <div style="float: left; width: 380px;">
                    <asp:TextBox ID="txtJustificacion" name="txtJusitificacionCC" runat="server" TextMode="MultiLine"
                        Width="350px" Height="80px"></asp:TextBox>
                </div>
                <div style="float: left; width: 100px;">
                    <div style="float: left; padding-left: 10px;">
                        <asp:Button ID="btbAnular" class="btn btn-primary" runat="server" Text="Anular" OnClick="btnAnular_Click" />
                        <%-- OnClientClick="Validar()"--%>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
