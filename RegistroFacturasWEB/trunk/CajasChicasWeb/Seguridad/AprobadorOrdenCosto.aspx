<%@ Page Title="" Language="C#" MasterPageFile="~/Base.Master" AutoEventWireup="true"
    CodeBehind="AprobadorOrdenCosto.aspx.cs" Inherits="RegistroFacturasWEB.Seguridad.AprobadorOrdenCosto"
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

            $(function () {
                $("#tabs").tabs();
            });

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
            var hfIdCentroCosto = $('#<%= hfIdCentroCosto.ClientID %>');

            //Uri WebServices
            var ajaxUrlForChild2 = '../../Seguridad/MapeoUsuariosCentros.asmx/ListarCentroMapeado';

            //Bind events
            ddlSociedad.bind('change', ddlSociedadChange);
            ddlCentro.bind('change', ddlCentroChange);

            //Events handlers
            function ddlSociedadChange() {
                hfIdSociedad.val(this.value);
                hfIdCentroCosto.val('0');
                hfIdCentro.val('0');
                doAjaxCall(ajaxUrlForChild2, '{codigoSociedad:"' + ddlSociedad.val() + '"}', CargarCentro);
                $('#<%= cblCentroCosto.ClientID %>').remove();
            }

            function ddlCentroChange() {
                hfIdCentro.val(this.value);
                $('#<%= cblCentroCosto.ClientID %>').remove();
            };

            //Populate child1 dropdown if the parent has some selected value
            if (ddlSociedad.val() > 0) {
                doAjaxCall(ajaxUrlForChild2, '{CodigoSociedad:"' + ddlSociedad.val() + '"}', CargarCentro);
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
        });

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
    <asp:HiddenField ID="hfIdAprobadorCentro" runat="server" Value="0" />
    <asp:HiddenField ID="hfIdUsuario" runat="server" Value="0" />
    <asp:HiddenField ID="hfUsuario" runat="server" Value="0" />
    <asp:HiddenField ID="hfIdSociedad" runat="server" Value="0" />
    <asp:HiddenField ID="hfIdCentro" runat="server" Value="0" />
    <asp:HiddenField ID="hfIdCentroCosto" runat="server" Value="0" />
    <div class="TituloCaja">
        <div class="TituloPagina">
            Mapedo aprobador orden de costo
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
    <div class="CentrarDiv" style="background-color: White; width: 910px;">
        <div style="float: left; width: 450px;">
            <div style="float: left; width: 75px;">
                <asp:Label ID="Label7" runat="server" Text="Aprobador:"></asp:Label>
            </div>
            <div style="float: left; width: 180px; padding-left: 5px;">
                <asp:TextBox ID="txtUsuario" name="txtUsuario" Enabled="false" runat="server" Style="width: 280px;">
                </asp:TextBox>
            </div>
        </div>
        <div style="float: left; width: 450px;">
            <div style="float: left; width: 75px;">
                <asp:Label ID="Label3" runat="server" Text="Sociedad:"></asp:Label>
            </div>
            <div style="float: left; width: 180px;">
                <asp:DropDownList ID="ddlSociedad" runat="server" Style="width: 280px;">
                </asp:DropDownList>
            </div>
        </div>
        <div style="float: left; width: 450px;">
            <div style="float: left; width: 75px;">
                <asp:Label ID="Label1" runat="server" Text="Centro:"></asp:Label>
            </div>
            <div style="float: left; width: 180px;">
                <asp:DropDownList ID="ddlCentro" runat="server" Style="width: 280px;">
                </asp:DropDownList>
            </div>
        </div>
        <div style="float: left; width: 450px;">
            <div style="float: left; width: 75px;">
                <asp:Label ID="Label2" runat="server" Text="Nivel:"></asp:Label>
            </div>
            <div style="float: left; width: 180px;">
                <asp:DropDownList ID="ddlNivel" runat="server" Style="width: 280px;">
                </asp:DropDownList>
            </div>
        </div>
        <div style="float: left; width: 450px;">
            <div style="float: left; width: 75px;">
                <asp:Label ID="lbl" runat="server" Text="Centro Costo:"></asp:Label>
            </div>
            <div style="float: left; width: 180px; padding-top: 10px;">
                <asp:TextBox ID="txtCentroCosto" runat="server" Width="280px"></asp:TextBox>
            </div>
        </div>
        <br />
        <div style="float: right; padding-bottom: 8px; padding-right: 5px;">
            <div style="float: left; padding-left: 5px;">
                <asp:Button ID="btnGrabar" class="btn btn-primary" runat="server" Text="Grabar" />
            </div>
            <div style="float: left; padding-left: 10px;">
                <asp:Button ID="btnBuscar" class="btn" runat="server" Text="Buscar" />
            </div>
            <div style="float: left; padding-left: 10px;">
                <asp:Button ID="btnLimpiar" class="btn" runat="server" Text="Limpiar" />
            </div>
        </div>
    </div>
    <div style="display: block; padding-top: 5px;">
    </div>
    <div class="CentrarDiv" style="width: 100%; background-color: White">
        <asp:CheckBoxList ID="cblCentroCosto" runat="server" RepeatColumns="5" RepeatDirection="Vertical">
        </asp:CheckBoxList>
    </div>
</asp:Content>
