<%@ Page Title="" Language="C#" MasterPageFile="~/Base.Master" AutoEventWireup="true"
    CodeBehind="UsuarioCentroCosto.aspx.cs" Inherits="RegistroFacturasWEB.Mantenimientos.UsuarioCentroCosto"
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
            var ddlCentroCosto = $('#<%=ddlCentroCosto.ClientID %>');

            //Hirend Field
            var hfIdSociedad = $('#<%= hfIdSociedad.ClientID %>');
            var hfIdCentro = $('#<%= hfIdCentro.ClientID %>');
            var hfIdCentroCosto = $('#<%= hfIdCentroCosto.ClientID %>');

            //Uri WebServices
            var ajaxUrlForChild1 = '../../RegistroFacturas/MapeoUsuariosCentros.asmx/ListarCentroCostoDDL';
            var ajaxUrlForChild2 = '../../RegistroFacturas/MapeoUsuariosCentros.asmx/ListarCentroMapeado';

            //Bind events
            ddlSociedad.bind('change', ddlSociedadChange);
            ddlCentro.bind('change', ddlCentroChange);
            ddlCentroCosto.bind('change', ddlCentroCostoChange);

            //Events handlers
            function ddlSociedadChange() {
                hfIdSociedad.val(this.value);
                hfIdCentroCosto.val('0');
                hfIdCentro.val('0');
                doAjaxCall(ajaxUrlForChild1, '{codigoSociedad:' + ddlSociedad.val() + '}', CargarCentroCosto);
                doAjaxCall(ajaxUrlForChild2, '{codigoSociedad:"' + ddlSociedad.val() + '"}', CargarCentro);
            }

            function ddlCentroCostoChange() {
                hfIdCentroCosto.val(this.value);
            };

            function ddlCentroChange() {
                hfIdCentro.val(this.value);
            };

            //Disabled them initially
            //ddlCentro.attr('disabled', 'disabled');

            //Populate child1 dropdown if the parent has some selected value
            if (ddlSociedad.val() > 0) {
                doAjaxCall(ajaxUrlForChild1, '{CodigoSociedad: ' + ddlSociedad.val() + '}', CargarCentroCosto);
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
    <asp:HiddenField ID="hfIdUsuarioCentroCosto" runat="server" Value="0" />
    <asp:HiddenField ID="hfIdSociedad" runat="server" Value="0" />
    <asp:HiddenField ID="hfIdCentroCosto" runat="server" Value="0" />
    <asp:HiddenField ID="hfIdCentro" runat="server" Value="0" />
    <div class="TituloCaja">
        <div class="TituloPagina">
            Asignación de Usuarios a Centro de Costo
        </div>
    </div>
    <div id="dialog-confirm">
        <p>
            ¿Desea confirmar el proceso?
        </p>
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
                    <asp:Label ID="lblCentroCosto" runat="server" Text="Centro de Costo"></asp:Label>
                </div>
                <div style="float: left; padding-left: 8px;">
                    <asp:DropDownList ID="ddlCentroCosto" runat="server" Width="350px">
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
                    <asp:Button ID="btnLimpiar" class="btn" runat="server" Text="Limpiar" OnClientClick="Limpiar()"
                        OnClick="btnLimpiar_Click" />
                </div>
            </div>
            <%--<div style="float: right; padding-bottom: 8px; padding-right: 5px;">
                <div style="float: left; padding-left: 5px;">
                    <asp:Button ID="btnGrabar" class="btn btn-primary" runat="server" Text="Grabar" OnClick="btnGrabar_Click" />
                </div>
            </div>--%>
        </div>
    </div>
    <div style="display: block; padding-top: 10px;">
    </div>
    <div class="CentrarDiv" style="width: 950px;">
        <asp:GridView ID="gvCentroCosto" runat="server" AutoGenerateColumns="False" EmptyDataText="No hay Datos."
            CellPadding="4" AllowPaging="True" OnRowCommand="gvCentroCosto_RowCommand"
            onpageindexchanging="gvCentroCosto_PageIndexChanging">
            <Columns>
                <asp:BoundField DataField="NUMERO" HeaderText="No." />
                <asp:BoundField DataField="ID_USUARIO_CENTRO_COSTO" HeaderText="ID_USUARIO_CENTRO_COSTO"
                    HeaderStyle-CssClass="Oculto" FooterStyle-CssClass="Oculto" ItemStyle-CssClass="Oculto" />
                <asp:BoundField DataField="CODIGO_SOCIEDAD" HeaderText="CODIGO_SOCIEDAD" HeaderStyle-CssClass="Oculto"
                    FooterStyle-CssClass="Oculto" ItemStyle-CssClass="Oculto" />
                <asp:BoundField DataField="NOMBRE_SOCIEDAD" HeaderText="NOMBRE SOCIEDAD" />
                <asp:BoundField DataField="CODIGO_CENTRO" HeaderText="CODIGO_CENTRO" HeaderStyle-CssClass="Oculto"
                    FooterStyle-CssClass="Oculto" ItemStyle-CssClass="Oculto" />
                <asp:BoundField DataField="NOMBRE_CENTRO" HeaderText="NOMBRE CENTRO" />
                <asp:BoundField DataField="CENTRO_COSTO" HeaderText="CENTRO DE COSTO" />
                <asp:BoundField DataField="DESCRIPCION" HeaderText="DESCRIPCI&Oacute;N" />
                <asp:TemplateField HeaderText="ALTA" ItemStyle-HorizontalAlign="Center">
                    <ItemTemplate>
                        <asp:CheckBox ID="idAltacc" runat="server" Style="position: static" Checked='<%#bool.Parse(Eval("idAltacc").ToString())%>' />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="USUARIO_CREACION" HeaderText="USUARIO CREACI&Oacute;N" />
                <asp:BoundField DataField="FECHA_CREACION" HeaderText="FECHA CREACI&Oacute;N" />
                <asp:BoundField DataField="USUARIO_MODIFICACION" HeaderText="USUARIO MODIFICACI&Oacute;N" />
                <asp:BoundField DataField="FECHA_MODIFICACION" HeaderText="FECHA MODIFICACI&Oacute;N" />
                <asp:TemplateField>
                    <ItemTemplate>
                        <asp:ImageButton runat="server" ID="imgAlta" CommandName="Agregar" ImageUrl="~/Images/up_arrow.png"
                            CommandArgument='<%# DataBinder.Eval(Container,"RowIndex") %>' ToolTip="Dar alta centro costo usuario" />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField>
                    <ItemTemplate>
                        <asp:ImageButton runat="server" ID="imgBaja" CommandName="Quitar" ImageUrl="~/Images/down_arrow.png"
                            CommandArgument='<%# DataBinder.Eval(Container,"RowIndex") %>' ToolTip="Dar baja centro costo usuario" />
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
    </div>
</asp:Content>
