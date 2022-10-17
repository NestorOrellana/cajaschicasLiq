<%@ Page Title="" Language="C#" MasterPageFile="~/Base.Master" AutoEventWireup="true"
    CodeBehind="UsuarioOrdenCompra.aspx.cs" Inherits="RegistroFacturasWEB.Seguridad.UsuarioOrdenCompra"
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

            //Set controls instances
            var ddlSociedad = $('#<%= ddlSociedad.ClientID %>');
            var ddlCentro = $('#<%= ddlCentro.ClientID %>');

            //Hirend Field
            var hfIdSociedad = $('#<%= hfIdSociedad.ClientID %>');
            var hfIdCentro = $('#<%= hfIdCentro.ClientID %>');
            var hfIdOrdenCosto = $('#<%= hfIdOrdenCosto.ClientID %>');

            //Uri WebServices
            var ajaxUrlForChild1 = '../../Seguridad/MapeoUsuariosCentros.asmx/ListarOrdenCostoDDL';
            var ajaxUrlForChild2 = '../../Seguridad/MapeoUsuariosCentros.asmx/ListarCentroMapeado';

            //Bind events
            ddlSociedad.bind('change', ddlSociedadChange);
            ddlCentro.bind('change', ddlCentroChange);

            function OcultarMensajes() {
                var divMensajeError = document.getElementById('<%= divMensajeError.ClientID %>');
                var divMensaje = document.getElementById('<%= divMensaje.ClientID %>');

                divMensajeError.style.display = "none";
                divMensaje.style.display = "none";
            }

            //Events handlers
            function ddlSociedadChange() {
                hfIdSociedad.val(this.value);
                hfIdOrdenCosto.val('0');
                hfIdCentro.val('0');
                OcultarMensajes();
                doAjaxCall(ajaxUrlForChild2, '{codigoSociedad: ' + ddlSociedad.val() + '}', CargarCentro);
                $('#<%= cblCentroCosto.ClientID %>').remove();
            }

            function ddlCentroChange() {
                hfIdCentro.val(this.value);
                $('#<%= cblCentroCosto.ClientID %>').remove();
            };

            $('#<%= btnGrabar.ClientID %>').click(function () {

                if ($('#<%= cblCentroCosto.ClientID %>').length == 0) {
                    DesplegarError("No hay centros de costo desplegados");
                    return false;
                }
            });

            //Populate child1 dropdown if the parent has some selected value
            if (ddlSociedad.val() > 0) {
                doAjaxCall(ajaxUrlForChild2, '{CodigoSociedad: ' + ddlSociedad.val() + '}', CargarCentro);
            }

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

            function CargarCentro(response) {
                var centro = (typeof response.d) == 'string' ? eval('(' + response.d + ')') : response.d;
                ddlCentro.find('option').remove();


                //Append default option
                ddlCentro.attr('disabled', false).append($('<option></option>').attr('value', 0).text('::Seleccione centro::'));
                var doc = $('<div></div>');
                for (var i = 0; i < centro.length; i++) {
                    doc.append($('<option></option>').
                            attr('value', centro[i].IDENTIFICADOR).text(centro[i].DESCRIPCION));
                }
                ddlCentro.append(doc.html());
                doc.remove();

                //Set selected value if there is any value in hidden field
                ddlCentro.val(hfIdCentro.val());
            }

            $('#btnRegresar').click(function () {
                window.location.replace($('#<%= hfUrl.ClientID %>').val());
            });



        });

        function Validar() {

            if (($('#<%= ddlSociedad.ClientID %>').val() == "-1") || ($('#<%= ddlSociedad.ClientID %>').val() == "0")) {
                DesplegarError("Debe seleccionar una sociedad");
                return false;
            }

            if (($('#<%= ddlCentro.ClientID %>').val() == "-1") || ($('#<%= ddlCentro.ClientID %>').val() == "0")) {
                DesplegarError("Debe seleccionar un centro");
                return false;
            }
        }

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
    <asp:HiddenField ID="hfIdUsuarioOrdenCompra" runat="server" Value="0" />
    <asp:HiddenField ID="hfIdSociedad" runat="server" Value="0" />
    <asp:HiddenField ID="hfIdOrdenCosto" runat="server" Value="0" />
    <asp:HiddenField ID="hfIdCentro" runat="server" Value="0" />
    <asp:HiddenField ID="hfUrl" runat="server" Value="" />
    <div class="TituloCaja">
        <div class="TituloPagina">
            Asignación de orden de compra a usuarios
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
    <div class="CentrarDiv" style="background-color: White; width: 950px;">
        <div style="float: left; padding-bottom: 8px; width: 433px;">
            <div style="float: left; width: 180px;">
                <asp:Label ID="lblUsuario" runat="server" Text="Usuario"></asp:Label>
            </div>
            <div style="float: left; width: 180px;">
                <asp:TextBox ID="txtUsuario" name="txtUsuario" runat="server" ReadOnly="true" Text=""></asp:TextBox>
            </div>
        </div>
        <div style="float: left; padding-bottom: 8px;">
            <div style="float: left; padding-left: 8px; width: 75px">
                <asp:Label ID="lbl" runat="server" Text="Sociedad:"></asp:Label>
            </div>
            <div style="float: left; padding-left: 8px;">
                <asp:DropDownList ID="ddlSociedad" runat="server" Width="350px">
                </asp:DropDownList>
            </div>
        </div>
        <div style="float: left; padding-bottom: 8px;">
            <div style="float: left; padding-left: 8px; width: 75px">
                <asp:Label ID="lblCentro" runat="server" Text="Centro:"></asp:Label>
            </div>
            <div style="float: left; padding-left: 8px;">
                <asp:DropDownList ID="ddlCentro" runat="server" Width="350px">
                </asp:DropDownList>
            </div>
        </div>
        <div style="float: left; padding-bottom: 8px;">
            <div style="float: left; padding-left: 8px; width: 75px">
                <asp:Label ID="lblOrdenCosto" runat="server" Text="Orden de Costo:"></asp:Label>
            </div>
            <div style="float: left; padding-left: 8px;">
                <div style="float: left; padding-left: 8px;">
                    <asp:TextBox ID="txtOrdenCosto" runat="server" Width="350px"></asp:TextBox>
                </div>
            </div>
        </div>
        <br />
        <div style="float: right; padding-bottom: 8px; padding-right: 5px;">
            <div style="float: left; padding-left: 8px;">
                <asp:Button ID="btnGrabar" class="btn btn-primary" runat="server" Text="Grabar" OnClick="btnGrabar_Click" />
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
    <div style="display: block; padding-top: 10px;">
    </div>
    <div class="CentrarDiv" style="width: 100%; background-color: White">
        <asp:CheckBoxList ID="cblCentroCosto" runat="server" RepeatColumns="5" RepeatDirection="Vertical">
        </asp:CheckBoxList>
    </div>
</asp:Content>
