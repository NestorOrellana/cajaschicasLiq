<%@ Page Title="" Language="C#" MasterPageFile="~/Base.Master" AutoEventWireup="true"
    CodeBehind="AprobadorCentro.aspx.cs" Inherits="RegistroFacturasWEB.Seguridad.AprobadorCentro"
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
    <script type="text/javascript">

        $(document).ready(function () {

            //Set controls instances
            var ddlSociedad = $('#<%= ddlSociedad.ClientID %>');
            var ddlCentro = $('#<%= ddlCentro.ClientID %>');
            var ddlCentroCosto = $('#<%=ddlCentroCosto.ClientID %>');
            var ddlOrdenCompra = $('#<%= ddlOrdenCompra.ClientID %>');

            //Hirend Field
            var hfIdSociedad = $('#<%= hfIdSociedad.ClientID %>');
            var hfIdCentro = $('#<%= hfIdCentro.ClientID %>');
            var hfIdCentroCosto = $('#<%= hfIdCentroCosto.ClientID %>');
            var hfIdCentroCompra = $('#<%=hfIdCentroCompra.ClientID %>');
            var hfUsuario = $('#<%=hfUsuario.ClientID %>');

            //WebServices Mapeo por usuarios 
            var ajaxUrlForChild1 = '../../Seguridad/MapeoUsuariosCentros.asmx/ListarCentroCostoDDL';
            var ajaxUrlForChild2 = '../../Seguridad/MapeoUsuariosCentros.asmx/ListarCentroMapeado';
            var ajaxUrlForChild3 = '../../Seguridad/MapeoUsuariosCentros.asmx/ListarOrdenCostoDDL';

            //Bind events
            ddlSociedad.bind('change', ddlSociedadChange);
            ddlCentro.bind('change', ddlCentroChange);
            ddlCentroCosto.bind('change', ddlCentroCostoChange);
            ddlOrdenCompra.bind('change', ddlCentroCompraChange);

            //Events handlers
            function ddlSociedadChange() {
                hfIdSociedad.val(this.value);
                hfIdCentroCosto.val('0');
                hfIdCentro.val('0');
                hfIdCentroCompra.val('0');
                doAjaxCall(ajaxUrlForChild1, '{usuario: "' + hfUsuario.val() + '",codigoSociedad:"' + ddlSociedad.val() + '"}', CargarCentroCosto);
                doAjaxCall(ajaxUrlForChild2, '{codigoSociedad: ' + ddlSociedad.val() + '}', CargarCentro);
                doAjaxCall(ajaxUrlForChild3, '{usuario: "' + hfUsuario.val() + '",codigoSociedad:"' + ddlSociedad.val() + '"}', CargarCentroCompra);
            }

            function ddlCentroCostoChange() {
                if (ddlOrdenCompra.val('0')) {
                    hfIdCentroCosto.val(this.value);
                    hfIdCentroCompra.val('0');
                    DesplegarError('Debe Seleccionar Centro de Costo o Centro de Compra');
                } else
                    hfIdCentroCosto.val(this.value);
            };

            function ddlCentroChange() {
                hfIdCentro.val(this.value);
            };

            function ddlCentroCompraChange() {
                if (ddlCentroCosto.val('0')) {
                    hfIdCentroCompra.val(this.value);
                    hfIdCentroCosto.val('0');
                    DesplegarError('Debe Seleccionar Centro de Costo o Centro de Compra');
                } else
                    hfIdCentroCompra.val(this.value);
            };

            //Disabled them initially
            //ddlCentro.attr('disabled', 'disabled');

            //Populate child1 dropdown if the parent has some selected value
            if (ddlSociedad.val() > 0) {
                doAjaxCall(ajaxUrlForChild1, '{usuario: "' + hfUsuario.val() + '",codigoSociedad:"' + ddlSociedad.val() + '"}', CargarCentroCosto);
                doAjaxCall(ajaxUrlForChild2, '{CodigoSociedad: ' + ddlSociedad.val() + '}', CargarCentro);
                doAjaxCall(ajaxUrlForChild3, '{usuario: "' + hfUsuario.val() + '",codigoSociedad:"' + ddlSociedad.val() + '"}', CargarCentroCompra);
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

            function CargarCentroCosto(response) {
                var centro = (typeof response.d) == 'string' ? eval('(' + response.d + ')') : response.d;
                ddlCentroCosto.find('option').remove();


                //Append default option
                ddlCentroCosto.attr('disabled', false).append($('<option></option>').attr('value', 0).text('::Seleccione centro costo::'));
                var doc = $('<div></div>');
                for (var i = 0; i < centro.length; i++) {
                    doc.append($('<option></option>').
                            attr('value', centro[i].IDENTIFICADOR).text(centro[i].DESCRIPCION));
                }
                ddlCentroCosto.append(doc.html());
                doc.remove();

                //Set selected value if there is any value in hidden field
                ddlCentroCosto.val(hfIdCentroCosto.val());
            }

            function CargarCentroCompra(response) {
                var centroCompra = (typeof response.d) == 'string' ? eval('(' + response.d + ')') : response.d;
                ddlOrdenCompra.find('option').remove();


                //Append default option
                ddlOrdenCompra.attr('disabled', false).append($('<option></option>').attr('value', 0).text('::Seleccione orden compra::'));
                var doc = $('<div></div>');
                for (var i = 0; i < centroCompra.length; i++) {
                    doc.append($('<option></option>').
                            attr('value', centroCompra[i].IDENTIFICADOR).text(centroCompra[i].DESCRIPCION));
                }
                ddlOrdenCompra.append(doc.html());
                doc.remove();

                //Set selected value if there is any value in hidden field
                ddlOrdenCompra.val(hfIdCentroCompra.val());
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


        function DesplegarError(mensaje) {
            var divMensaje = document.getElementById('<%= divMensajeError.ClientID %>');
            divMensaje.style.display = "table";
            divMensaje.innerHTML = mensaje;
        }

        function OcultarMensaje() {
            var divMensaje = document.getElementById('<%= divMensajeError.ClientID %>');
            divMensaje.style.display = "none";
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
    <asp:HiddenField ID="hfIdCentroCompra" runat="server" Value="0" />
    <div class="TituloCaja">
        <div class="TituloPagina">
            Aprobador por Centro
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
        <div style="float: left; width: 450px; padding-left: 54px">
            <div style="float: left; margin-top: 10px; padding-left: 8px;">
                <asp:Label ID="Label7" runat="server" Text="Aprobador:"></asp:Label>
                <asp:TextBox ID="txtUsuario" name="txtUsuario" Enabled="false" runat="server" Style="width: 280px;">
                </asp:TextBox>
            </div>
        </div>
        <br />
        <div style="float: left; width: 450px; padding-left: 60px">
            <div style="float: left; margin-top: 10px; padding-left: 8px;">
                <asp:Label ID="Label3" runat="server" Text="Sociedad:"></asp:Label>
                <asp:DropDownList ID="ddlSociedad" runat="server" Style="width: 280px;">
                </asp:DropDownList>
            </div>
        </div>
        <div style="float: left; width: 450px; padding-left: 75px">
            <div style="float: left; margin-top: 10px; padding-left: 8px;">
                <asp:Label ID="Label1" runat="server" Text="Centro:"></asp:Label>
                <asp:DropDownList ID="ddlCentro" runat="server" Style="width: 280px;">
                </asp:DropDownList>
            </div>
        </div>
        <br />
        <div style="float: left; width: 450px; padding-left: 39px">
            <div style="float: left; margin-top: 10px; padding-left: 8px;">
                <asp:Label ID="lblCentroCosto" runat="server" Text="Centro Costo:"></asp:Label>
                <asp:DropDownList ID="ddlCentroCosto" runat="server" Style="width: 280px;" 
                    onselectedindexchanged="ddlCentroCosto_SelectedIndexChanged">
                </asp:DropDownList>
            </div>
        </div>
        <div style="float: left; width: 420px; padding-left: 5px">
            <div style="float: left; margin-top: 10px; padding-left: 8px;">
                <asp:Label ID="lblOrdenCompra" runat="server" Text="Orden de Compra:"></asp:Label>
                <asp:DropDownList ID="ddlOrdenCompra" runat="server" Style="width: 280px;">
                </asp:DropDownList>
            </div>
        </div>



        <br />
        <div style="float: left; width: 420px; padding-left: 5px">
            <div style="float: left; width: 550px; padding-left: 18px">
                <asp:Label ID="Label4" runat="server" Text="Centros de Costo: "></asp:Label>
                <asp:TextBox ID="txtCeco" runat="server" MaxLength="550" TextMode="MultiLine" Style="width: 280px;"></asp:TextBox>
            </div>
        </div>
        
        <div style="float: left; width: 450px; padding-left: 35px">
            <div style="float: left; width: 550px; padding-left: 10px">
                <asp:Label ID="Label9" runat="server" Text="Ordenes de Costo: "></asp:Label>
                <asp:TextBox ID="txtOrden" runat="server" MaxLength="550" TextMode="MultiLine" Style="width: 280px;"></asp:TextBox>
            </div>
        </div>

        <br />
        <div style="float: left; width: 420px; padding-left: 85px">
            <div style="float: left; margin-top: 10px; padding-left: 8px;">
                <asp:Label ID="Label2" runat="server" Text="Nivel:"></asp:Label>
                <asp:DropDownList ID="ddlNivel" runat="server" Style="width: 280px;">
                </asp:DropDownList>
            </div>
        </div>
        <br />
        <div style="float: left; width: 200px; padding-left: 105px">
            <div style="float: left; margin-top: 10px; padding-left: 1px;">
                <asp:Label ID="Label5" runat="server" Text="Alta:"></asp:Label>
                <asp:CheckBox ID="chAlta" runat="server" />
            </div>
        </div>
        <br />
        <br />
        <div style="float: left; width: 420px; padding-left: 5px">
            <div style="float: left; margin-top: 10px; padding-left: 8px;">
                <asp:Label ID="Label6" runat="server" Text="Usuario Creacion:"></asp:Label>
                <asp:Label ID="lblUsuarioCreacionBD" runat="server" Text=""></asp:Label>
            </div>
        </div>
        <div style="float: left; width: 420px; padding-left: 5px">
            <div style="float: left; margin-top: 10px; padding-left: 8px;">
                <asp:Label ID="lblFechaModificacion" runat="server" Text="Fecha modificacion:"></asp:Label>
                <asp:Label ID="lblFechaModificacionDB" runat="server" Text=""></asp:Label>
            </div>
        </div>
        <br />
        <div style="float: left; width: 420px; padding-left: 5px">
            <div style="float: left; margin-top: 10px; padding-left: 8px;">
                <asp:Label ID="Label8" runat="server" Text="Fecha Creacion:"></asp:Label>
                <asp:Label ID="lblFechaCreacionBD" runat="server" Text=""></asp:Label>
            </div>
        </div>
        <div style="float: left; width: 420px; padding-left: 5px">
            <div style="float: left; margin-top: 10px; padding-left: 8px;">
                <asp:Label ID="lblUsuario" runat="server" Text="Usuario modificacion:"></asp:Label>
                <asp:Label ID="lblUsuarioDB" runat="server" Text=""></asp:Label>
            </div>
        </div>
        <br />
        <div style="float: right; padding-bottom: 8px; padding-right: 5px;">
            <div style="float: left; padding-left: 5px;">
                <asp:Button ID="btnGrabar" class="btn btn-primary" runat="server" Text="Grabar" OnClick="btnGrabar_Click" />
            </div>
            <div style="float: left; padding-left: 10px;">
                <asp:Button ID="btnLimpiar" class="btn" runat="server" Text="Limpiar" OnClick="btnLimpiar_Click" />
            </div>
        </div>
        <div style="display: block; padding-top: 5px;">
        </div>
    </div>
    <div style="display: block; padding-top: 5px;">
    </div>
    <div class="CentrarDiv" style="width: 85%;">
        <asp:GridView ID="gvAprobadorCentro" runat="server" AutoGenerateColumns="false" EmptyDataText="No hay registros."
            CellPadding="4" AllowPaging="True" OnRowCommand="gvAprobadorCentro_RowCommand"
            onpageindexchanging="gvAprobadorCentro_PageIndexChanging" PageSize="15">
            <columns>
                <asp:BoundField DataField="NUMERO" HeaderText="No." />
                <asp:BoundField DataField="ID_APROBADORCENTRO" HeaderText="ID" HeaderStyle-CssClass="Oculto"
                    FooterStyle-CssClass="Oculto" ItemStyle-CssClass="Oculto" />
                <asp:BoundField DataField="ID_USUARIO" HeaderText="ID_USUARIO" HeaderStyle-CssClass="Oculto"
                    FooterStyle-CssClass="Oculto" ItemStyle-CssClass="Oculto" />
                <asp:BoundField DataField="CENTRO_COSTO" HeaderText="CENTRO DE COSTO" />
                <asp:BoundField DataField="ORDEN_COMPRA" HeaderText="ORDEN DE COMPRA" />
                <asp:BoundField DataField="ID_CENTRO" HeaderText="ID CENTRO" HeaderStyle-CssClass="Oculto"
                    FooterStyle-CssClass="Oculto" ItemStyle-CssClass="Oculto" />
                <asp:BoundField DataField="CENTRO" HeaderText="CENTRO" />
                <asp:BoundField DataField="ID_NIVEL" HeaderText="ID NIVEL" HeaderStyle-CssClass="Oculto"
                    FooterStyle-CssClass="Oculto" ItemStyle-CssClass="Oculto" />
                <asp:BoundField DataField="NIVEL" HeaderText="NIVEL" />
                <asp:TemplateField HeaderText="ALTA" ItemStyle-HorizontalAlign="Center">
                    <ItemTemplate>
                        <asp:CheckBox ID="idAlta" runat="server" Style="position: static" Checked='<%#bool.Parse(Eval("idAlta").ToString())%>' />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="USUARIO_CREACION" HeaderText="USUARIO CREACI&Oacute;N" />
                <asp:BoundField DataField="FECHA_CREACION" HeaderText="FECHA CREACION" />
                <asp:BoundField DataField="USUARIO_MODIFICACION" HeaderText="USUARIO MODIFICACI&Oacute;N" />
                <asp:BoundField DataField="FECHA_MODIFICACION" HeaderText="FECHA MODIFICACI&Oacute;N" />
                <asp:BoundField DataField="CODIGO_SOCIEDAD" HeaderText="CODIGO SOCIEDAD" HeaderStyle-CssClass="Oculto"
                    FooterStyle-CssClass="Oculto" ItemStyle-CssClass="Oculto" />
                <asp:TemplateField>
                    <ItemTemplate>
                        <asp:ImageButton runat="server" ID="imgEditar" CommandName="Editar" ImageUrl="~/Images/edit.png"
                            CommandArgument='<%# DataBinder.Eval(Container, "RowIndex") %>' ToolTip="Edición" />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField>
                    <ItemTemplate>
                        <asp:ImageButton runat="server" ID="imgAlta" CommandName="Alta" ImageUrl="~/Images/up_arrow.png"
                            CommandArgument='<%# DataBinder.Eval(Container, "RowIndex") %>' ToolTip="Dar Alta Aprobador" />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField>
                    <ItemTemplate>
                        <asp:ImageButton runat="server" ID="imgBaja" CommandName="Baja" ImageUrl="~/Images/down_arrow.png"
                            CommandArgument='<%# DataBinder.Eval(Container, "RowIndex") %>' ToolTip="Dar Baja Aprobador" />
                    </ItemTemplate>
                </asp:TemplateField>
            </columns>
        </asp:GridView>
    </div>
</asp:Content>
