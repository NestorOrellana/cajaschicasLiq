<%@ Page Title="" Language="C#" MasterPageFile="~/Base.Master" AutoEventWireup="true"
    CodeBehind="UsuarioSociedadCentro.aspx.cs" Inherits="RegistroFacturasWEB.Seguridad.UsuarioSociedadCentro"
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
    <script type="text/javascript">
        $(document).ready(function () {
            $('#dialog-confirm').dialog({
                autoOpen: false,
                resizable: false,
                height: 140,
                modal: true,
                buttons: {
                    "Aceptar": function () {
                        $(this).dialog("close");
                    },
                    "Cancelar": function () {
                        $(this).dialog("close");
                    }
                }
            });


            $('#<%= ddlSociedad.ClientID %>').change(function () {
                $('#<%= cblUsuSociedadCentro.ClientID %>').remove();
            });


            //Set controls instances
            var ddlSociedad = $('#<%= ddlSociedad.ClientID %>');

            //Hirend Field
            var hfIdSociedad = $('#<%= hfIdSociedad.ClientID %>');
            var hfIdCentro = $('#<%= hfIdCentro.ClientID %>');

            //Uri WebServices
            var ajaxUrlForChild = '../../RegistroFacturas/MapeoUsuariosCentros.asmx/ListarCentroMapeado';

            //Bind events
            ddlSociedad.bind('change', ddlSociedadChange);

            function OcultarMensajes() {
                var divMensajeError = document.getElementById('<%= divMensajeError.ClientID %>');
                var divMensaje = document.getElementById('<%= divMensaje.ClientID %>');

                divMensajeError.style.display = "none";
                divMensaje.style.display = "none";
            }

            //Events handlers
            function ddlSociedadChange() {
                hfIdSociedad.val(this.value);
                OcultarMensajes();
            }

            function ddlCentroCostoChange() {
                hfIdOrdenCosto.val(this.value);
            };

            function doAjaxCall(url, data, successHandler) {
                $.ajax({
                    type: 'POST',
                    url: url,
                    data: data,
                    contentType: 'application/json; charset=utf-8',
                    dataType: 'json',
                    success: function (response) {
                        successHandler(response);
                    }
                });
            }

            $('#btnRegresar').click(function () {
                window.location.replace($('#<%= hfUrl.ClientID %>').val());
            });
        });
-
        function ConfirmarProceso(uniqueID, itemID, evento) {

            $("#dialog-confirm").dialog({
                title: evento,

                resizable: false,
                height: 200,
                width: 350,
                modal: true,
                buttons: {
                    "Aceptar": function () {
                        __doPostBack(uniqueID, '');
                        $(this).dialog("close");

                    },
                    "Cancelar": function () { $(this).dialog("close"); }
                }
            });

            $('#dialog-confirm').dialog('open');
            return false;
        }

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:HiddenField ID="hfIdUsuarioSociedadCentro" runat="server" Value="0" />
    <asp:HiddenField ID="hfIdSociedad" runat="server" Value="0" />
    <asp:HiddenField ID="hfIdCentro" runat="server" Value="0" />
    <asp:HiddenField ID="hfUrl" runat="server" Value="" />
    <div class="TituloCaja">
        <div class="TituloPagina">
            Asignación de sociedad y centro a usuarios
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
                <asp:Label ID="lblUsuario" runat="server" Text="Usuario"></asp:Label>
            </div>
            <div style="float: left; width: 180px;">
                <asp:TextBox ID="txtUsuario" name="txtUsuario" runat="server" ReadOnly="true" Text=""></asp:TextBox>
            </div>
        </div>
        <br />
        <div style="float: left; padding-bottom: 8px;">
            <div style="float: left; padding-bottom: 8px;">
                <div style="float: left; padding-left: 8px; width: 75px">
                    <asp:Label ID="lbl" runat="server" Text="Sociedad:"></asp:Label>
                </div>
                <div style="float: left; padding-left: 8px;">
                    <asp:DropDownList ID="ddlSociedad" runat="server" Width="350px">
                    </asp:DropDownList>
                </div>
            </div>
            <br />
            <div style="float: right; padding-bottom: 8px; padding-right: 5px;">
                <div style="float: left; padding-left: 8px;">
                    <asp:Button ID="btnGrabar" class="btn btn-primary" runat="server" Text="Grabar" OnClick="btnGrabar_Click"
                        OnClientClick="Validar()" />
                </div>
                <div style="float: left; padding-left: 8px;">
                    <asp:Button ID="btnBuscar" class="btn" runat="server" Text="Buscar" OnClick="btnBuscar_Click" />
                </div>
                <div style="float: left; padding-left: 8px;">
                    <asp:Button ID="btnLimpiar" class="btn" runat="server" Text="Limpiar" OnClientClick="Limpiar()"
                        OnClick="btnLimpiar_Click" />
                </div>
            </div>
            <input type="button" id="btnRegresar" name="Regresar" class="btn" />
        </div>
    </div>
    <div style="display: block; padding-top: 10px;">
    </div>
    <div class="CentrarDiv" style="width: 100%; background-color: White">
        <asp:CheckBoxList ID="cblUsuSociedadCentro" runat="server" RepeatColumns="5" RepeatDirection="Vertical">
        </asp:CheckBoxList>
    </div>
</asp:Content>
