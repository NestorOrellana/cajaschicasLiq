<%@ Page Title="" Language="C#" MasterPageFile="~/Base.Master" AutoEventWireup="true"
    CodeBehind="MapeoUsuarioCaja_MM.aspx.cs" Inherits="RegistroFacturasWEB.RegistroFacturas.MapeoUsuarioCaja_MM" %>

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

        var hfIdCentro = null;
        var hfSociedad = null; 


        function Limpiar() {
            var nuovourl = window.location.href + '';
            nuovourl = nuovourl + (nuovourl.indexOf('?') > -1 ? "&refreshme=1" : "?refreshme=1");
            window.location.href = nuovourl;
        }


        $(document).ready(function () {
            $('#popuporderedit').dialog({
                autoOpen: false,
                modal: true,
                resizable: false,
                width: 485,
                heigth: 800,
                title: 'Buscaqueda de Usuarios',
                open: function (event, ui) {


                },
                close: function (event, ui) {

                    //limpia todos los textbox del popup
                    $('#popuporderedit :text').val('');
                },
                buttons: {
                    Cerrar: function () {
                        $(this).dialog("close");
                        return;
                    }
                }
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
        });

        //      Mostrar ventana para buscar usuario. 

        function tecla(e) {
            var evt = e ? e : event;
            var key = window.Event ? evt.which : evt.keyCode;
            alert(key);
        }



        function VentanaBuscarUsuario() {
            var usuario = $('#txtUsuarioB');
            var nombre = $('#txtNombreB');
            var dominio = $('#ddlDominio');

            $('#popuporderedit').dialog({
                autoOpen: false,
                modal: true,
                resizable: false,
                width: 400,
                heigth: 250,
                title: 'Busqueda de Usuarios',
                open: function (event, ui) {

                    initialize();
                },
                close: function (event, ui) {

                    //limpia todos los textbox del popup
                    $("#tbl-RR td").remove();
                    $('#popuporderedit :text').val('');
                },
                buttons: {
                    Buscar: function () {
                        //  OcultarMensaje();
                        BuscarUsuario(usuario, nombre, dominio)
                    },
                    Cancelar: function () {
                        $(this).dialog("close");
                        return;
                    }
                }
            });
            $('#popuporderedit').dialog('open');
            return false;

        }
        $(function () {
            $('#btnBuscar').click(function () {
                VentanaBuscarUsuario();
            });

        });



        function initialize() {

            var combo = $('#ddlDominio');

            $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: "../../RegistroFacturas/MapeoUsuariosCentros.asmx/BuscarDominio",
                data: '{}',
                dataType: "json",
                async: true,
                success: function (response) {
                    var dominio = (typeof response.d) == 'string' ? eval('(' + response.d + ')') : response.d;
                    combo.find('option').remove();

                    //Append default option
                    combo.attr('disabled', false).append($('<option></option>').attr('value', "").text('::Dominio::'));
                    var doc = $('<div></div>');
                    for (var i = 0; i < dominio.length; i++) {
                        doc.append($('<option></option>').attr('value', dominio[i].IDENTIFICADOR).text(dominio[i].NOMBRE));

                    }
                    combo.append(doc.html());
                    doc.remove();
                },
                error: function (request, status, error) {
                    alert(JSON.parse(request.responseText).Message);
                }
            });
        }


        function BuscarUsuario(usuario, nombre, dominio) {
            var url = '../../RegistroFacturas/MapeoUsuariosCentros.asmx/BusquedaUsuario';
            var data = '{usuario:"' + usuario.val() + '", nombre:"' + nombre.val() + '", dominio:"' + dominio.val() + '"}';


            $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: url,
                data: data,
                dataType: "json",
                async: true,
                success: function (response) {
                    var busqueda = (typeof response.d) == 'string' ? eval('(' + response.d + ')') : response.d;
                    //var factura = JSON.parse(responce.d);
                    $("#tbl-RR td").remove();
                    for (var i = 0; i < busqueda.length; i++) {
                        $("#tbl-RR tbody").append('<tr><td id="usuario">' + busqueda[i].USUARIO + '</td>' +
                                                      "<td>" + busqueda[i].NOMBRE + "</td>" +
                                                      '<td><button id="btbSeleccion"><img src="~/Images/accept.png">...</button></td>');
                    }

                },
                error: function (request, status, error) {
                    alert(JSON.parse(request.responseText).Message);
                }
            });

        }

        $(function () {
            $("#tbl-RR tbody").on("click", "#btbSeleccion", function () {
                var Seldominio = $('#ddlDominio').find('option:selected').text();
                var seleccion = $(this).parents("tr").find("#usuario").text()
                $('#<%= txtDominio.ClientID%>').val(Seldominio);
                $('#<%= txtUsuario.ClientID%>').val(seleccion);
                $('#popuporderedit').dialog('close');
                $('#<%= txtUsuario.ClientID%>').change();
                //                CargarSociedad(seleccion);
            });
        });


        function CargarSociedad(usuarioB) {

            var combo = $('#<%=ddlSociedad.ClientID %>');
            $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: "../../RegistroFacturas/MapeoUsuariosCentros.asmx/BuscarSociedad",
                data: '{usuarioB:"' + usuarioB + '"}',
                dataType: "json",
                async: true,
                success: function (response) {
                    var sociedad = (typeof response.d) == 'string' ? eval('(' + response.d + ')') : response.d;
                    combo.find('option').remove();

                    //Append default option
                    combo.attr('disabled', false).append($('<option></option>').attr('value', "").text('::Sociedad::'));
                    var doc = $('<div></div>');
                    for (var i = 0; i < sociedad.length; i++) {
                        doc.append($('<option></option>').attr('value', sociedad[i].IDENTIFICADOR).text(sociedad[i].DESCRIPCION));

                    }
                    combo.append(doc.html());
                    doc.remove();
                },
                error: function (request, status, error) {
                    alert(JSON.parse(request.responseText).Message);
                }
            });
        }




    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:HiddenField ID="hfSociedad" runat="server" Value="0" />
    <div class="TituloCaja">
        <div class="TituloPagina">
            Mapeo Usuario Caja Chica (Proveedor Esporadico) 
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
    <div class="CentrarDiv" style="background-color: White; width: 590px;">
        <div style="float: left; padding-bottom: 8px;">
            <div style="float: left; width: 180px;">
                <asp:Label ID="Label1" runat="server" Text="Usuario"></asp:Label>
            </div>
            <div style="float: left; width: 180px;">
                <asp:TextBox ID="txtUsuario" name="txtUsuario" CssClass="mayuscula" runat="server"
                AutoPostBack="true"></asp:TextBox>
            </div>
            <div style="float: left; padding-left: 30px;">
                <input type="button" id="btnBuscar" value="Buscar" class="btn btn-primary" />
            </div>
        </div>
        <div style="float: left; padding-bottom: 8px;">
            <div style="float: left; width: 180px;">
                <asp:Label ID="Label2" runat="server" Text="Dominio"></asp:Label>
            </div>
            <div style="float: left; width: 180px;">
                <asp:TextBox ID="txtDominio" name="txtDominio" CssClass="mayuscula" runat="server" AutoPostBack="true"></asp:TextBox>
            </div>
        </div>
        <div style="float: left; padding-bottom: 8px;">
            <div style="float: left; width: 180px;">
                <asp:Label ID="lblNombre" runat="server" Text="Sociedad"></asp:Label>
            </div>
            <div style="float: left; width: 180px;">
                <asp:DropDownList ID="ddlSociedad" runat="server" Width="300px">
                </asp:DropDownList>
            </div>
        </div>
        <br />
        <div style="float: left; padding-bottom: 8px;">
            <div style="float: left; width: 180px;">
                <asp:Label ID="lblBuscar" runat="server" Text="Buscar"></asp:Label>
            </div>
            <div style="float: left; width: 180px;">
                <asp:TextBox ID="txtBuscar" name="txtBuscar" CssClass="mayuscula" runat="server"></asp:TextBox>
            </div>
        </div>
        <br />
        <div style="float: right; padding-bottom: 8px; padding-right: 5px;">
            <div style="float: left; padding-left: 5px;">
                <asp:Button ID="btnVisualizar" class="btn btn-primary" runat="server" Text="Visualizar"
                    OnClick="btnVisualizar_Click" />
            </div>
            <div style="float: left; padding-left: 10px;">
                <asp:Button ID="btnLimpiar" class="btn" runat="server" Text="Limpiar" OnClick="btnLimpiar_Click"
                    OnClientClick="Limpiar()" />
            </div>
        </div>
    </div>
    <div style="display: block; padding-top: 10px;">
    </div>
    <div class="CentrarDiv" style="width: 550px;">
        <asp:GridView ID="gvMapeo" runat="server" AutoGenerateColumns="False" EmptyDataText="No hay Asignaciones."
            CellPadding="4" GridLines="None" AllowPaging="True" OnRowCommand="gvMapeo_RowCommand"
            OnPageIndexChanging="gvMapeo_PageIndexChanging">
            <Columns>
                <asp:BoundField DataField="NUMERO" HeaderText="No." />
                <asp:BoundField DataField="BUKRS" HeaderText="SOCIEDAD" />
                <asp:BoundField DataField="LIFNR" HeaderText="CAJA" />
                <asp:BoundField DataField="NAME" HeaderText="DESCRIPCION" />
                <asp:TemplateField HeaderText="ESTADO" ItemStyle-HorizontalAlign="Center">
                    <ItemTemplate>
                        <asp:CheckBox Enabled="false" ID="Estado" runat="server" Style="position: static"
                            Checked='<%#bool.Parse(Eval("Estado").ToString())%>' />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField>
                    <ItemTemplate>
                        <asp:ImageButton runat="server" ID="Alta" CommandName="Alta" ImageUrl="~/Images/up_arrow.png"
                            CommandArgument='<%# DataBinder.Eval(Container,"RowIndex") %>' ToolTip="Agregar"
                            Visible='<%#( Convert.ToDecimal(DataBinder.Eval(Container.DataItem, "ESTADO")) == 0 ) ? true : false %>' />
                        <asp:ImageButton runat="server" ID="Baja" CommandName="Baja" ImageUrl="~/Images/down_arrow.png"
                            CommandArgument='<%# DataBinder.Eval(Container,"RowIndex") %>' ToolTip="Quitar"
                            Visible='<%#( Convert.ToDecimal(DataBinder.Eval(Container.DataItem, "ESTADO")) == 1 ) ? true : false %>' />
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
    </div>
    <div id="popuporderedit">
        <div id="popupcontainer" style="width: 450px">
            <div class="row">
                <div class="cell popupcontainercell">
                    <table id="orderedittable">
                        <tr>
                            <td>
                                País
                            </td>
                            <td class="cell">
                                <select id="ddlDominio" style="width: 250px;">
                                </select>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Usuario
                            </td>
                            <td>
                                <input type="text" id="txtUsuarioB" maxlength="15 width: 250px;" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Nombre
                            </td>
                            <td>
                                <input type="text" id="txtNombreB" maxlength="40 width: 250px;" />
                            </td>
                        </tr>
                    </table>
                    <table class="ui-widget ui-widget-content">
                        <tr>
                            <td>
                                <span id="ResultadoBusqueda"></span>
                            </td>
                        </tr>
                    </table>
                    <table id="tbl-RR" class="ui-widget ui-widget-content">
                        <thead class="ui-widget-header">
                            <tr>
                                <th>
                                    Usuario
                                </th>
                                <th>
                                    Nombre
                                </th>
                                <th>
                                    Seleccionar
                                </th>
                            </tr>
                        </thead>
                        <tbody>
                        </tbody>
                    </table>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
