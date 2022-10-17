<%@ Page Title="" Language="C#" MasterPageFile="~/Base.Master" AutoEventWireup="true"
    CodeBehind="OrdenCosto.aspx.cs" Inherits="RegistroFacturasWEB.Reportes.OrdenCosto" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div>
        <h1>
            Reporte orden costo</h1>
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <rsweb:ReportViewer ID="ReportViewer1" runat="server" Width="100%" ShowPrintButton="true">
        </rsweb:ReportViewer>
    </div>
</asp:Content>
