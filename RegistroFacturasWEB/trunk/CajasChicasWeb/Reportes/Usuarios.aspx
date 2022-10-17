<%@ Page Title="" Language="C#" MasterPageFile="~/Base.Master" AutoEventWireup="true"
    CodeBehind="Usuarios.aspx.cs" Inherits="RegistroFacturasWEB.Reportes.Usuarios" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div>
        <h1>
            Reporte usuarios</h1>
        <rsweb:ReportViewer ID="ReportViewer1" runat="server" Width="100%" ShowPrintButton="true">
        </rsweb:ReportViewer>
    </div>
</asp:Content>
