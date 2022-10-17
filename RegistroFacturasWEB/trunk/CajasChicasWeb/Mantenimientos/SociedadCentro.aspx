<%@ Page Title="" Language="C#" MasterPageFile="~/Base.Master" AutoEventWireup="true"
    CodeBehind="SociedadCentro.aspx.cs" Inherits="RegistroFacturasWEB.Mantenimientos.SociedadCentro"
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
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:HiddenField ID="hfIdSociedadCentro" runat="server" Value="0" />
    <asp:HiddenField ID="hfIdSociedad" runat="server" Value="0" />
    <asp:HiddenField ID="hfIdCentro" runat="server" Value="0" />
    <asp:HiddenField ID="hfIdUsuario" runat="server" Value="0" />
    <div class="TituloCaja">
        <div class="TituloPagina">
            Asignación de Sociedad Centro
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
                <asp:Label ID="lbl" runat="server" Text="Sociedad:"></asp:Label>
            </div>
            <div style="float: left; width: 180px;">
                <asp:DropDownList ID="ddlSociedad" runat="server">
                </asp:DropDownList>
            </div>
            <div style="float: left; padding-bottom: 8px;">
                <div style="float: left; width: 180px;">
                    <asp:Label ID="lblCentro" runat="server" Text="Centro:"></asp:Label>
                </div>
                <div style="float: left; width: 180px;">
                    <asp:DropDownList ID="ddlCentro" runat="server">
                    </asp:DropDownList>
                </div>
            </div>
            <br />
            <div style="float: right; padding-bottom: 8px; padding-right: 5px;">
                <div style="float: left; padding-left: 5px;">
                    <asp:Button ID="btnGrabar" class="btn btn-primary" runat="server" Text="Grabar" OnClick="btnGrabar_Click" />
                </div>
                <div style="float: left; padding-left: 10px;">
                    <asp:Button ID="btnLimpiar" class="btn" runat="server" Text="Limpiar" OnClick="btnLimpiar_Click"/>
                </div>
            </div>
        </div>
    </div>
    <div style="display: block; padding-top: 10px;">
    </div>
    <div class="CentrarDiv" style="width: 950px;">
        <asp:GridView ID="gvSociedadCentro" runat="server" AutoGenerateColumns="False" EmptyDataText="No hay Datos."
            CellPadding="4" AllowPaging="True" OnRowCommand="gvSociedadCentro_RowCommand"
            onpageindexchanging="gvSociedadCentro_PageIndexChanging">
            <Columns>
                <asp:BoundField DataField="NUMERO" HeaderText="No." />
                <asp:BoundField DataField="ID_SOCIEDAD_CENTRO" HeaderText="ID SOCIEDAD CENTRO" HeaderStyle-CssClass="Oculto"
                    FooterStyle-CssClass="Oculto" ItemStyle-CssClass="Oculto" />
                <asp:BoundField DataField="ID_SOCIEDAD" HeaderText="ID SOCIEDAD" HeaderStyle-CssClass="Oculto"
                    FooterStyle-CssClass="Oculto" ItemStyle-CssClass="Oculto" />
                <asp:BoundField DataField="SOCIEDAD" HeaderText="SOCIEDAD" />
                <asp:BoundField DataField="ID_CENTRO" HeaderText="ID CENTRO" HeaderStyle-CssClass="Oculto"
                    FooterStyle-CssClass="Oculto" ItemStyle-CssClass="Oculto" />
                <asp:BoundField DataField="CENTRO" HeaderText="CENTRO" />
                <asp:TemplateField HeaderText="ALTA" ItemStyle-HorizontalAlign="Center">
                    <ItemTemplate>
                        <asp:CheckBox ID="idAlta" runat="server" Style="position: static" Checked='<%#bool.Parse(Eval("idAlta").ToString())%>' />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="USUARIO_CREACION" HeaderText="USUARIO CREACI&Oacute;N" />
                <asp:BoundField DataField="FECHA_CREACION" HeaderText="FECHA CREACI&Oacute;N" />
                <asp:BoundField DataField="USUARIO_MODIFICACION" HeaderText="USUARIO MODIFICACI&Oacute;N" />
                <asp:BoundField DataField="FECHA_MODIFICACION" HeaderText="FECHA MODIFICACI&Oacute;N" />
                <asp:TemplateField>
                    <ItemTemplate>
                        <asp:ImageButton runat="server" ID="imgBaja" CommandName="Alta" ImageUrl="~/Images/up_arrow.png"
                            CommandArgument='<%# DataBinder.Eval(Container,"RowIndex") %>' ToolTip="Dar Alta Sociedad Centro" />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField>
                    <ItemTemplate>
                        <asp:ImageButton runat="server" ID="imgAlta" CommandName="Baja" ImageUrl="~/Images/down_arrow.png"
                            CommandArgument='<%# DataBinder.Eval(Container,"RowIndex") %>' ToolTip="Dar Baja Sociedad Centro" />
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
    </div>
</asp:Content>
