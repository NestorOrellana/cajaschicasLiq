<%@ Page Title="" Language="C#" MasterPageFile="~/Base.Master" AutoEventWireup="true"
    CodeBehind="CentroCosto.aspx.cs" Inherits="RegistroFacturasWEB.Reportes.CentroCosto" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div>
        <h1>
            Reporte centro costo</h1>
        <rsweb:ReportViewer ID="ReportViewer1" runat="server" Width="100%" ShowPrintButton="true">
        </rsweb:ReportViewer>
    </div>
</asp:Content>
