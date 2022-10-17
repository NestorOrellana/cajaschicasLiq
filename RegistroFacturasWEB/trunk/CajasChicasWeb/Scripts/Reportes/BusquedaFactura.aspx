<%@ Page Title="" Language="C#" MasterPageFile="~/Base.Master" AutoEventWireup="true"
    CodeBehind="BusquedaFactura.aspx.cs" Inherits="RegistroFacturasWEB.Reportes.BusquedaFactura" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div>
        <h1>
            Busqueda de Facturas</h1>
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <rsweb:ReportViewer ID="ReportViewer1" runat="server" Width="100%" Style="background-color: white;">
        </rsweb:ReportViewer>
    </div>
</asp:Content>
<a href="CentroCosto.aspx">CentroCosto.aspx</a>