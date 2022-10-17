<%@ Page Title="" Language="C#" MasterPageFile="~/Base.Master" AutoEventWireup="true"
    CodeBehind="AprobadorCentro.aspx.cs" Inherits="RegistroFacturasWEB.Mantenimientos.AprobadorCentro" %>

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
    $.validator.addMethod("validaCentroCosto", ValidaCentroCosto, "Seleccione Centro Costo");
    $.validator.addMethod("validaOrdenCompra", ValidaOrdenCompra, "Seleccione Orden Compra");
    $.validator.addMethod("validaCentro", ValidaCentro, "Seleccione Centro");
    $.validator.addMethod("validaNivel", ValidaNivel, "Seleccione Nivel");

    //Validacion de Campos
    function Validar() {
        $(document).ready(function () {
            $("#form1").validate({
                rules: {
                    '<%= txtUsuario.UniqueID %>': { required: true },
                    '<%=ddlCentroCosto.UniqueID%>': { validaCentroCosto: true },
                    '<%=ddlOrdenCompra.UniqueID%>': { validaOrdenCompra: true },
                    '<%=ddlCentro.UniqueID%>': { validaCentro: true },
                    '<%=ddlNivel.UniqueID %>': { validaNivel: true },
                    '<%=txtPorcentaje.UniqueID %>': { required: true },
                    '<%=txtTolerancia.UniqueID %>': {required: true}
                },
                messages: {
                    '<%= txtUsuario.UniqueID %>': { required: "*Ingrese Usuario" },
                    '<%=ddlCentroCosto.UniqueID%>': { validaCentroCosto: "*Seleccione Centro de Costo" },
                    '<%=ddlOrdenCompra.UniqueID%>': { validaOrdenCompra: "*Seleccione Orden de Compra" },
                    '<%=ddlCentro.UniqueID%>': { validaCentro: "*Seleccione Orden de Compra" },
                    '<%=ddlNivel.UniqueID %>': { validaNivel: "*Seleccione Nivel" },
                    '<%=txtPorcentaje.UniqueID %>': { required: "*Ingrese Porcentaje" },
                    '<%=txtTolerancia.UniqueID %>': { required: "*Ingrese Tolerancia" }
                }
            });
        });
    }

    function ValidaCentroCosto() {
        var ddlCentroCosto = document.getElementById('<%=ddlCentroCosto.ClientID%>').selectedIndex;
        if ((ddlCentroCosto == 0) || (ddlCentroCosto == -1)) {
            return false;
        }
        return true;
    }

    function ValidaOrdenCompra() {
        var ddlOrdenCompra = document.getElementById('<%=ddlOrdenCompra.ClientID%>').selectedIndex;
        if ((ddlOrdenCompra == 0) || (ddlOrdenCompra == -1)) {
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

    function ValidaNivel() {
        var ddlNivel = document.getElementById('<%=ddlNivel.ClientID%>').selectedIndex;
        if ((ddlNivel == 0) || (ddlNivel == -1)) {
            return false;
        }
        return true;
    }

    function ValidarNumero(numObj) {

        var numero = numObj.value;

        if (!/^[0-9]+(\.[0-9]{1,2})?$/.test(numero))
            numObj.value = numero.substring(0, numero.length - 1);
    }

    function Limpiar() {
        var nuovourl = window.location.href + '';
        nuovourl = nuovourl + (nuovourl.indexOf('?') > -1 ? "&refreshme=1" : "?refreshme=1");
        window.location.href = nuovourl;

    }
   </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:HiddenField ID="hfIdAprobadorCentro" runat="server" Value="0" />
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

       <div style="float: left;  width: 450px; padding-left: 73px">
            <div style="float: left; margin-top: 10px; padding-left: 8px;">
                <asp:Label ID="Label7" runat="server" Text="Usuario:"></asp:Label>
                <asp:TextBox ID="txtUsuario" name = "txtUsuario" Text = "1" runat="server" Style="width: 280px;"></asp:TextBox>
            </div>
       </div>
       <br />

       <div style="float: left;  width: 450px; padding-left: 40px">
            <div style="float: left; margin-top: 10px; padding-left: 8px;">
                <asp:Label ID="lblCentroCosto" runat="server" Text="Centro Costo:"></asp:Label>
                <asp:DropDownList ID="ddlCentroCosto" runat="server" Style="width: 280px;">
                </asp:DropDownList>
            </div>
       </div>
        
       <div style="float: left;  width: 420px; padding-left: 5px">
            <div style="float: left; margin-top: 10px; padding-left: 8px;">
                <asp:Label ID="lblOrdenCompra" runat="server" Text="Orden de Compra:"></asp:Label>
                <asp:DropDownList ID="ddlOrdenCompra" runat="server" Style="width: 280px;">
                </asp:DropDownList>
            </div>
       </div>
        <br />


       <div style="float: left;  width: 450px; padding-left: 80px">
            <div style="float: left; margin-top: 10px; padding-left: 8px;">
                <asp:Label ID="Label1" runat="server" Text="Centro:"></asp:Label>
                <asp:DropDownList ID="ddlCentro" runat="server" Style="width: 280px;">
                </asp:DropDownList>
            </div>
       </div>
        
       <div style="float: left;  width: 420px; padding-left: 85px">
            <div style="float: left; margin-top: 10px; padding-left: 8px;">
                <asp:Label ID="Label2" runat="server" Text="Nivel:"></asp:Label>
                <asp:DropDownList ID="ddlNivel" runat="server" Style="width: 280px;">
                </asp:DropDownList>
            </div>
       </div>
        <br />

       <div style="float: left;  width: 450px; padding-left: 0px">
            <div style="float: left; margin-top: 10px; padding-left: 8px;">
                <asp:Label ID="Label3" runat="server" Text="Porcentaje Compra:"></asp:Label>
                <asp:TextBox ID="txtPorcentaje" name = "txtPorcentaje" OnKeyUp="ValidarNumero(this)" runat="server" Style="width: 280px;"></asp:TextBox>
            </div>
       </div>
        
       <div style="float: left;  width: 420px; padding-left: 50px">
            <div style="float: left; margin-top: 10px; padding-left: 8px;">
                <asp:Label ID="Label4" runat="server" Text="Tolerancia:"></asp:Label>
                <asp:TextBox ID="txtTolerancia" name = "txtTolerancia" OnKeyUp="ValidarNumero(this)" runat="server" Style="width: 280px;"></asp:TextBox>
            </div>
       </div>
        <br />

       <div style="float: left;  width: 200px; padding-left: 105px">
            <div style="float: left; margin-top: 10px; padding-left: 1px;">
                <asp:Label ID="Label5" runat="server" Text="Alta:"></asp:Label>
                <asp:CheckBox ID="chAlta" runat="server" />
            </div>
       </div>
       <br />
       <br />
        <div style="float: left; padding-bottom: 8px; padding-right: 5px; width: 100%">
            <asp:Label ID="Label6" runat="server" Text="Usuario Creacion:"></asp:Label>
            <asp:Label ID="lblUsuarioCreacionBD" runat="server" Text=""></asp:Label>
        </div>

        <div style="float: left; padding-bottom: 8px; padding-right: 5px; width: 100%;">
            <asp:Label ID="Label8" runat="server" Text="Fecha Creacion:"></asp:Label>
            <asp:Label ID="lblFechaCreacionBD" runat="server" Text=""></asp:Label>
        </div>

        <br />
        <div style="float: left; padding-bottom: 8px; padding-right: 5px; width: 100%">
            <asp:Label ID="lblFechaModificacion" runat="server" Text="Fecha modificacion:"></asp:Label>
            <asp:Label ID="lblFechaModificacionDB" runat="server" Text=""></asp:Label>
        </div>

        <div style="float: left; padding-bottom: 8px; padding-right: 5px; width: 100%;">
            <asp:Label ID="lblUsuario" runat="server" Text="Usuario modificacion:"></asp:Label>
            <asp:Label ID="lblUsuarioDB" runat="server" Text=""></asp:Label>
        </div>


        <div style="float: right; padding-bottom: 8px; padding-right: 5px;">
            <div style="float: left; padding-left: 5px;">
                <asp:Button ID="btnGrabar" class="btn btn-primary" runat="server" Text="Grabar" OnClick="btnGrabar_Click" OnClientClick = "Validar()"/>
            </div>
            <div style="float: left; padding-left: 10px;">
                <asp:Button ID="btnLimpiar" class="btn" runat="server" Text="Limpiar" OnClick="btnLimpiar_Click" OnClientClick  = "Limpiar()" />
            </div>
        </div>


        <div style="display: block; padding-top: 5px;">
        </div>

    </div>
    <div style="display: block; padding-top: 5px;">
    </div>
    <div class="CentrarDiv" style="width: 1320px;">
        <asp:GridView ID="gvAprobadorCentro" runat="server" AutoGenerateColumns="False" EmptyDataText="No hay Datos para Mostrar."
            CellPadding="4" AllowPaging="True" OnRowCommand="gvAprobadorCentro_RowCommand">
            <Columns>
                <asp:BoundField DataField="NUMERO" HeaderText="No." />
                <asp:BoundField DataField="ID_APROBADORCENTRO" HeaderText="ID" HeaderStyle-CssClass="Oculto"
                    FooterStyle-CssClass="Oculto" ItemStyle-CssClass="Oculto" />
                <asp:BoundField DataField="CENTRO_COSTO" HeaderText="CENTRO DE COSTO" />
                <asp:BoundField DataField="ORDEN_COMPRA" HeaderText="ORDEN DE COMPRA" />
                <asp:BoundField DataField="ID_CENTRO" HeaderText="ID CENTRO" HeaderStyle-CssClass="Oculto"
                    FooterStyle-CssClass="Oculto" ItemStyle-CssClass="Oculto" />
                <asp:BoundField DataField="CENTRO" HeaderText="CENTRO" />
                <asp:BoundField DataField="ID_USUARIO" HeaderText="ID USUARIO" HeaderStyle-CssClass="Oculto"
                    FooterStyle-CssClass="Oculto" ItemStyle-CssClass="Oculto" />
                <asp:BoundField DataField="USUARIO" HeaderText="USUARIO" />
                <asp:BoundField DataField="ID_NIVEL" HeaderText="ID NIVEL" HeaderStyle-CssClass="Oculto"
                    FooterStyle-CssClass="Oculto" ItemStyle-CssClass="Oculto" />
                <asp:BoundField DataField="NIVEL" HeaderText="NIVEL" />
                <asp:BoundField DataField="PORCENTAJE_COMPRA" HeaderText="PORCENTAJE DE COMPRA" />
                <asp:BoundField DataField="TOLERANCIA" HeaderText="TOLERANCIA" />
                <asp:TemplateField HeaderText="ALTA" ItemStyle-HorizontalAlign="Center">
                    <ItemTemplate>
                        <asp:CheckBox ID="idAlta" runat="server" Style="position: static" Checked='<%#bool.Parse(Eval("idAlta").ToString())%>' />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="USUARIO_CREACION" HeaderText="USUARIO CREACI&Oacute;N" />
                <asp:BoundField DataField="FECHA_CREACION" HeaderText="FECHA CREACION" />
                <asp:BoundField DataField="USUARIO_MODIFICACION" HeaderText="USUARIO MODIFICACI&Oacute;N" />
                <asp:BoundField DataField="FECHA_MODIFICACION" HeaderText="FECHA MODIFICACI&Oacute;N" />

                <asp:TemplateField>
                    <ItemTemplate>
                        <asp:ImageButton runat="server" ID="imgEditar" CommandName="Editar" ImageUrl="~/Images/edit.png"
                            CommandArgument='<%# DataBinder.Eval(Container,"RowIndex") %>' ToolTip="Editar" />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField>
                    <ItemTemplate>
                        <asp:ImageButton runat="server" ID="imgBaja" CommandName="Baja" ImageUrl="~/Images/down_arrow.png"
                            CommandArgument='<%# DataBinder.Eval(Container,"RowIndex") %>' ToolTip="Dar baja Aprobador Centro" />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField>
                    <ItemTemplate>
                        <asp:ImageButton runat="server" ID="imgAlta" CommandName="Alta" ImageUrl="~/Images/up_arrow.png"
                            CommandArgument='<%# DataBinder.Eval(Container,"RowIndex") %>' ToolTip="Dar alta Aprobador Centro" />
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
    </div>
</asp:Content>
