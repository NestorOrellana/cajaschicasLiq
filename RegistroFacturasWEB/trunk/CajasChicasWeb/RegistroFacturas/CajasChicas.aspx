<%@ Page Language="C#" MasterPageFile="~/Base.Master" AutoEventWireup="true" CodeBehind="CajasChicas.aspx.cs"
    Inherits="RegistroFacturasWEB.RegistroFacturas.CajasChicas" EnableEventValidation="false"
    EnableViewState="true" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="../Scripts/jquery-1.10.2.js" type="text/javascript"></script>
    <script src="../Scripts/jquery-1.7.2.min.js" type="text/javascript"></script>
    <script type="text/javascript" src="../Scripts/jquery.min.js"></script>
    <script type="text/javascript" src="/Scripts/jquery.validate.js"></script>
    <script src="../Scripts/jquery-ui.min.js" type="text/javascript"></script>
    <link href="../App_Themes/Tema/jquery-ui.css" rel="stylesheet" type="text/css" />
    <script src="/bootstrap/js/bootstrap.js" type="text/javascript"></script>
    <link href="/bootstrap/css/bootstrap.css" rel="stylesheet" type="text/css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">

        $.validator.addMethod("validaSociedad", ValidaSociedad, "Seleccione Sociedad");
        $.validator.addMethod("validaCentro", ValidaCentro, "Seleccione Centro");
        $.validator.addMethod("validaTipoCajaChica", ValidaTipoCajaChica, "Seleccione Tipo Caja Chica");
        $.validator.addMethod("ValidaTipoOperacion", ValidaTipoOperacion, "Seleccione Tipo Operacion");
        $.validator.addMethod("ValidaMoneda", ValidaMoneda, "Seleccione Moneda");

        $(function () {

            //Set controls instances
            var ddlSociedad = $('#<%= ddlSociedad.ClientID %>');
            var ddlCentro = $('#<%=ddlCentro.ClientID %>');
            var ddlCajaChica = $('#<%=ddlCajaChica.ClientID %>');
            var ddlMoneda = $('#<%=ddlMoneda.ClientID %>');
            var ddlOperacion = $('#<%=ddlTipoOperacion.ClientID %>');
            var ddlNiveles = $('#<%=ddlNivel.ClientID %>');
            var divCamposLiquidacion = document.getElementById("camposLiquidacion");

            //Hirend Field
            var hfSociedad = $('#<%= hfSociedad.ClientID %>');
            var hfCentro = $('#<%= hfCentro.ClientID %>');
            var hfCajaChicaSAP = $('#<%= hfCajaChicaSAP.ClientID %>');
            var hfMoneda = $('#<%=hfMoneda.ClientID %>');
            var hfNivel = $('#<%=hfNivel.ClientID %>');

            //Uri WebServices
            var ajaxUrlForChild1 = '../../RegistroFacturas/MapeoUsuariosCentros.asmx/ListarCentrosUsuario';
            var ajaxUrlForChild2 = '../../RegistroFacturas/MapeoUsuariosCentros.asmx/BuscarCajasChicasSAP';
            var ajaxUrlForChild3 = '../../RegistroFacturas/MapeoUsuariosCentros.asmx/ListarSociedadMoneda';
            var ajaxUrlForChild4 = '../../RegistroFacturas/MapeoUsuariosCentros.asmx/ListarNiveles';

            //Bind events
            ddlSociedad.bind('change', ddlSociedadChange);
            ddlCentro.bind('change', ddlCentroChange);
            ddlCajaChica.bind('change', ddlCajaChicaChange);
            ddlMoneda.bind('change', ddlMonedaChange);
            ddlOperacion.bind('change', ddlOperacionChange);
            ddlNiveles.bind('change', ddlNivelChange);

            //Events handlers
            function ddlOperacionChange() {
                if (this.value == "VL") {
                    divCamposLiquidacion.style.display = "block";
                }
                else {
                    divCamposLiquidacion.style.display = "none";
                }
            }

            function ddlSociedadChange() {
                hfSociedad.val(this.value);
                hfCentro.val('0');
                hfCajaChicaSAP.val('0');
                hfMoneda.val('0');
                hfNivel.val('0');
                doAjaxCall(ajaxUrlForChild1, '{codigoSociedad: ' + ddlSociedad.val() + '}', CargarCentro);
                doAjaxCall(ajaxUrlForChild2, '{codigoSociedad: ' + ddlSociedad.val() + '}', CargarCajaChica);
                doAjaxCall(ajaxUrlForChild3, '{codigoSociedad: ' + ddlSociedad.val() + '}', CargarMoneda);
                doAjaxCall(ajaxUrlForChild4, '{codigoSociedad: ' + ddlSociedad.val() + '}', CargarNiveles);
            }

            function ddlCentroChange() {
                hfCentro.val(this.value);
            };

            function ddlCajaChicaChange() {
                hfCajaChicaSAP.val(this.value);
            };

            function ddlMonedaChange() {
                hfMoneda.val(this.value);
            };

            function ddlNivelChange() {
                hfNivel.val(this.value);
            };

            //Disabled them initially
            //ddlCentro.attr('disabled', 'disabled');

            //Populate child1 dropdown if the parent has some selected value
            if (ddlSociedad.val() != 0) {
                doAjaxCall(ajaxUrlForChild1, '{CodigoSociedad: ' + ddlSociedad.val() + '}', CargarCentro);
                doAjaxCall(ajaxUrlForChild2, '{CodigoSociedad: ' + ddlSociedad.val() + '}', CargarCajaChica);
                doAjaxCall(ajaxUrlForChild3, '{CodigoSociedad: ' + ddlSociedad.val() + '}', CargarMoneda);
                doAjaxCall(ajaxUrlForChild4, '{codigoSociedad: ' + ddlSociedad.val() + '}', CargarNiveles);
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
                ddlCentro.val(hfCentro.val());
            }

            function CargarCajaChica(response) {
                var centro = (typeof response.d) == 'string' ? eval('(' + response.d + ')') : response.d;
                ddlCajaChica.find('option').remove();

                //Append default option
                ddlCajaChica.attr('disabled', false).append($('<option></option>').attr('value', 0).text('::Seleccione caja chica::'));
                var doc = $('<div></div>');
                for (var i = 0; i < centro.length; i++) {
                    doc.append($('<option></option>').
                            attr('value', centro[i].IDENTIFICADOR).text(centro[i].DESCRIPCION));
                }
                ddlCajaChica.append(doc.html());
                doc.remove();

                //Set selected value if there is any value in hidden field
                ddlCajaChica.val(hfCajaChicaSAP.val());
            }

            function CargarMoneda(response) {
                var moneda = (typeof response.d) == 'string' ? eval('(' + response.d + ')') : response.d;
                ddlMoneda.find('option').remove();


                //Append default option
                ddlMoneda.attr('disabled', false).append($('<option></option>').attr('value', 0).text('::Seleccione Moneda::'));
                var doc = $('<div></div>');
                for (var i = 0; i < moneda.length; i++) {
                    doc.append($('<option></option>').
                            attr('value', moneda[i].IDENTIFICADOR).text(moneda[i].DESCRIPCION));
                }
                ddlMoneda.append(doc.html());
                doc.remove();

                //Set selected value if there is any value in hidden field
                ddlMoneda.val(hfMoneda.val());
            }

            function CargarNiveles(response) {
                var nivel = (typeof response.d) == 'string' ? eval('(' + response.d + ')') : response.d;
                ddlNiveles.find('option').remove();


                //Append default option
                ddlNiveles.attr('disabled', false).append($('<option></option>').attr('value', 0).text('::Seleccione Nivel::'));
                var doc = $('<div></div>');
                for (var i = 0; i < nivel.length; i++) {
                    doc.append($('<option></option>').
                            attr('value', nivel[i].IDENTIFICADOR).text(nivel[i].DESCRIPCION));
                }
                ddlNiveles.append(doc.html());
                doc.remove();

                //Set selected value if there is any value in hidden field
                ddlNiveles.val(hfMoneda.val());
            }

            $('#<%=txtFechaInicioViaje.ClientID%>').datepicker({
                dateFormat: 'yy/mm/dd'
            });

            $('#<%=txtFechaFinViaje.ClientID%>').datepicker({
                dateFormat: 'yy/mm/dd'
            });
        });

        //Validacion de Campos
        function Validar() {
            $(document).ready(function () {
                $("#form1").validate({
                    rules: {
                        '<%=ddlSociedad.UniqueID%>': { ValidaSociedad: true },
                        '<%=ddlCentro.UniqueID%>': { ValidaCentro: true },
                        '<%=ddlTipoOperacion.UniqueID%>': { ValidaTipoOperacion: true },
                        '<%=ddlCajaChica.UniqueID%>': { ValidaTipoCajaChica: true },
                        '<%=ddlMoneda.UniqueID %>': { ValidaMoneda: true },
                        '<%=txtDescripcion.UniqueID %>': { required: true, maxlength: 150 }
                    },
                    messages: {
                        '<%=ddlSociedad.UniqueID%>': { ValidaSociedad: "*Seleccione Sociedad" },
                        '<%=ddlCentro.UniqueID%>': { ValidaCentro: "*Seleccione Centro" },
                        '<%=ddlTipoOperacion.UniqueID%>': { ValidaTipoOperacion: "*Seleccione Tipo Operacion" },
                        '<%=ddlCajaChica.UniqueID%>': { ValidaTipoCajaChica: "*Seleccione Tipo de Caja Chica" },
                        '<%=ddlMoneda.UniqueID %>': { ValidaMoneda: "*Seleccion Moneda" },
                        '<%=txtDescripcion.UniqueID %>': { required: "*Ingrese Descripción", maxlength: "*La Descripción debe de contener menos de 150 caracteres" }
                    }
                });
            });
        }

        function ValidaTipoOperacion() {
            var ddlTipoOperacion = document.getElementById('<%=ddlTipoOperacion.ClientID%>').selectedIndex;
            if ((ddlTipoOperacion == 0) || (ddlTipoOperacion == -1)) {
                return false;
            }
            return true;
        }

        function ValidaMoneda() {
            var ddlMoneda = document.getElementById('<%=ddlMoneda.ClientID%>').selectedIndex;
            if ((ddlMoneda == 0) || (ddlMoneda == -1)) {
                return false;
            }
            return true;
        }

        function ValidaSociedad() {
            var ddlSociedad = document.getElementById('<%=ddlSociedad.ClientID%>').selectedIndex;
            if ((ddlSociedad == 0) || (ddlSociedad == -1)) {
                return false;
            }
            return true;
        }

        function ValidaCentro() {
            var ddlCentro = document.getElementById('<%=ddlCentro.ClientID%>').selectedIndex;
            if ((ddlCentro == 0) || (ddlCentro == -1)) {
                return false;
            }
            return true;
        }

        function ValidaTipoCajaChica() {
            var ddlTipoCajaChica = document.getElementById('<%=ddlCajaChica.ClientID%>').selectedIndex;
            if ((ddlTipoCajaChica == 0) || (ddlTipoCajaChica == -1)) {
                return false;
            }
            return true;
        }

        function Limpiar() {
            var nuovourl = window.location.href + '';
            nuovourl = nuovourl + (nuovourl.indexOf('?') > -1 ? "&refreshme=1" : "?refreshme=1");
            window.location.href = nuovourl;
        }
    </script>
    <asp:HiddenField ID="hfIdCajaChica" runat="server" Value="0" />
    <asp:HiddenField ID="hfSociedad" runat="server" Value="0" />
    <asp:HiddenField ID="hfCentro" runat="server" Value="0" />
    <asp:HiddenField ID="hfCajaChicaSAP" runat="server" Value="0" />
    <asp:HiddenField ID="hfMoneda" runat="server" Value="0" />
    <asp:HiddenField ID="hfNivel" runat="server" Value="0" />
    <div class="TituloCaja">
        <div class="TituloPagina">
            Registro de cajas chicas</div>
    </div>
    <div style="margin: 10px 0 10px 0;">
        <div id="divMensajeError" class="ca-MensajeError" style="display: none" runat="server">
        </div>
        <div id="divMensaje" class="ca-MensajeOK" style="display: none" runat="server">
        </div>
    </div>
    <div style="display: block; padding-top: 5px;">
    </div>
    <div class="CentrarDiv" style="background-color: White; width: 750px;">
        <div style="float: left; padding-left: 5px">
            <div style="float: left; margin-top: 10px; padding-left: 8px; width: 180px;">
                <asp:Label ID="lblSociedad" runat="server" Text="Sociedad:"></asp:Label>
            </div>
            <div style="float: left; margin-top: 10px; padding-left: 8px;">
                <asp:DropDownList ID="ddlSociedad" runat="server" Width="350px">
                </asp:DropDownList>
            </div>
        </div>
        <div style="float: left; padding-left: 5px">
            <div style="float: left; margin-top: 10px; padding-left: 8px; width: 180px;">
                <asp:Label ID="lblCentro" runat="server" Text="Centro:"></asp:Label>
            </div>
            <div style="float: left; margin-top: 10px; padding-left: 8px;">
                <asp:DropDownList ID="ddlCentro" runat="server" Width="350px">
                </asp:DropDownList>
            </div>
        </div>
        <div style="float: left; padding-left: 5px">
            <div style="float: left; margin-top: 10px; padding-left: 8px; width: 180px;">
                <asp:Label ID="lblTipoCajaChica" runat="server" Text="Asignado a:"></asp:Label>
            </div>
            <div style="float: left; margin-top: 10px; padding-left: 8px;">
                <asp:DropDownList ID="ddlCajaChica" runat="server" Width="350px">
                </asp:DropDownList>
            </div>
        </div>
        <div style="float: left; padding-left: 5px">
            <div style="float: left; margin-top: 10px; padding-left: 8px; width: 180px;">
                <asp:Label ID="lblMoneda" runat="server" Text="Moneda:"></asp:Label>
            </div>
            <div style="float: left; margin-top: 10px; padding-left: 8px;">
                <asp:DropDownList ID="ddlMoneda" runat="server" Width="350px">
                </asp:DropDownList>
            </div>
        </div>
        <div style="float: left; padding-left: 5px">
            <div style="float: left; margin-top: 10px; padding-left: 8px; width: 180px;">
                <asp:Label ID="lblDescripcion" runat="server" Text="Descripci&oacute;n:"></asp:Label>
            </div>
            <div style="float: left; margin-top: 10px; padding-left: 8px; width: 350px;">
                <asp:TextBox ID="txtDescripcion" runat="server" CssClass="mayuscula" Width="350px"
                    MaxLength="25"></asp:TextBox>
            </div>
        </div>
        <div style="float: left; padding-left: 5px">
            <div style="float: left; margin-top: 10px; padding-left: 8px; width: 180px;">
                <asp:Label ID="lblTipoOperacion" runat="server" Text="Tipo operacion:"></asp:Label>
            </div>
            <div style="float: left; margin-top: 10px; padding-left: 8px;">
                <asp:DropDownList ID="ddlTipoOperacion" runat="server" Width="350px">
                </asp:DropDownList> 
            </div>    
        </div>
        <div id="camposLiquidacion" style="display: none">
            <div style="float: left; padding-left: 5px">
                <div style="float: left; margin-top: 10px; padding-left: 8px; width: 180px;">
                    <asp:Label ID="lblFechaInicioViaje" runat="server" Text="Fecha Inicio Viaje:"></asp:Label>
                </div>
                <div style="float: left; margin-top: 10px; padding-left: 8px; width: 350px;">
                    <asp:TextBox ID="txtFechaInicioViaje" runat="server" CssClass="mayuscula" Width="350px"
                        MaxLength="25"></asp:TextBox>
                </div>
            </div>
            <div style="float: left; padding-left: 5px">
                <div style="float: left; margin-top: 10px; padding-left: 8px; width: 180px;">
                    <asp:Label ID="lblFechaFinViaje" runat="server" Text="Fecha Inicio Viaje:"></asp:Label>
                </div>
                <div style="float: left; margin-top: 10px; padding-left: 8px; width: 350px;">
                    <asp:TextBox ID="txtFechaFinViaje" runat="server" CssClass="mayuscula" Width="350px"
                        MaxLength="25"></asp:TextBox>
                </div>
            </div>
            <div style="float: left; padding-left: 5px">
                <div style="float: left; margin-top: 10px; padding-left: 8px; width: 180px;">
                    <asp:Label ID="lblObjetivo" runat="server" Text="Objetivo:"></asp:Label>
                </div>
                <div style="float: left; margin-top: 10px; padding-left: 8px; width: 350px;">
                    <asp:TextBox ID="txtObjetivo" runat="server" CssClass="mayuscula" Width="350px"
                        MaxLength="25"></asp:TextBox>
                </div>
            </div>
            <div style="float: left; padding-left: 5px">
                <div style="float: left; margin-top: 10px; padding-left: 8px; width: 180px;">
                    <asp:Label ID="lblNumeroDias" runat="server" Text="Numero de Dias:"></asp:Label>
                </div>
                <div style="float: left; margin-top: 10px; padding-left: 8px; width: 350px;">
                    <asp:TextBox ID="txtNumeroDias" runat="server" CssClass="mayuscula" Width="350px"
                        MaxLength="25"></asp:TextBox>
                </div>
            </div>
            <div style="float: left; padding-left: 5px">
                <div style="float: left; margin-top: 10px; padding-left: 8px; width: 180px;">
                    <asp:Label ID="lblViaticosRecibidos" runat="server" Text="Viaticos Recibidos:"></asp:Label>
                </div>
                <div style="float: left; margin-top: 10px; padding-left: 8px; width: 350px;">
                    <asp:TextBox ID="txtViaticosRecibidos" runat="server" CssClass="mayuscula" Width="350px"
                        MaxLength="25"></asp:TextBox>
                </div>
            </div>
            <div style="float: left; padding-left: 5px">
                <div style="float: left; margin-top: 10px; padding-left: 8px; width: 180px;">
                    <asp:Label ID="lblNivel" runat="server" Text="Nivel:"></asp:Label>
                </div>
                <div style="float: left; margin-top: 10px; padding-left: 8px;">
                    <asp:DropDownList ID="ddlNivel" runat="server" Width="350px">
                    </asp:DropDownList>
                </div>
            </div>
        </div>
        <div style="float: left; padding-left: 5px; width: 551px;">
            <div style="float: left; margin-top: 10px; padding-left: 8px; width: 180px;">
                <asp:Label ID="lblCorrelativo" runat="server" Text="Correlativo:"></asp:Label>
            </div>
            <div style="float: left; margin-top: 10px; padding-left: 8px;">
                <asp:Label ID="lblCorrelativoDB" runat="server" Text=""></asp:Label>
            </div>
        </div>
        <div style="float: left; padding-left: 5px; width: 551px;">
            <div style="float: left; margin-top: 10px; padding-left: 8px; width: 180px;">
                <asp:Label ID="lblCajaChicaAlta" runat="server" Text="Vigente:"></asp:Label>
            </div>
            <div style="float: left; margin-top: 10px; padding-left: 8px;">
                <asp:CheckBox ID="cbAlta" runat="server" />
            </div>
        </div>
        <div style="float: left; padding-left: 5px; width: 300px;">
            <div style="float: left; padding-left: 8px; width: 180px;">
                <asp:Label ID="lblFechaAlta" runat="server" Text="Fecha alta:"></asp:Label>
            </div>
            <div style="float: left; padding-left: 8px;">
                <asp:Label ID="lblFechaAltaDB" runat="server" Text=""></asp:Label>
            </div>
        </div>
        <div style="float: left; padding-left: 5px; width: 300px;">
            <div style="float: left; padding-left: 8px; width: 180px;">
                <asp:Label ID="lblFechaModificacion" runat="server" Text="Fecha modificaci&oacute;n:"></asp:Label>
            </div>
            <div style="float: left; padding-left: 8px;">
                <asp:Label ID="lblFechaModificacionDB" runat="server" Text=""></asp:Label>
            </div>
        </div>
        <div style="float: left; padding-left: 5px; width: 300px;">
            <div style="float: left; padding-left: 8px; width: 180px;">
                <asp:Label ID="lblUsuarioAlta" runat="server" Text="Usuario alta"></asp:Label>
            </div>
            <div style="float: left; padding-bottom: 8px;">
                <asp:Label ID="lblUsuarioAltaDB" runat="server" Text=""></asp:Label>
            </div>
        </div>
        <div style="float: left; padding-left: 5px; width: 300px;">
            <div style="float: left; padding-left: 8px; width: 180px;">
                <asp:Label ID="lblUsuarioModificacion" runat="server" Text="Usuario modificaci&oacute;n"></asp:Label>
            </div>
            <div style="float: left; padding-bottom: 8px;">
                <asp:Label ID="lblUsuarioModificacionDB" runat="server" Text=""></asp:Label>
            </div>
        </div>
        <div style="float: right; padding-left: 5px; padding-bottom: 10px; width: 150px;">
            <div style="float: left; margin-top: 10px; padding-left: 8px;">
                <asp:Button ID="btnGrabar" runat="server" class="btn btn-primary" Text="Crear caja chica"
                    OnClick="btnGrabar_Click" OnClientClick="Validar()" />
            </div>
        </div>
    </div>
</asp:Content>
