<%@ Page Title="" Language="C#" MasterPageFile="~/Base.Master" AutoEventWireup="true"
    CodeBehind="Proveedores.aspx.cs" Inherits="RegistroFacturasWEB.Reportes.Proveedores" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div>
        <h1>
            Reporte proveedores</h1>
        <rsweb:reportviewer ID="ReportViewer1" runat="server" Width="100%">
        </rsweb:reportviewer>
    </div>
</asp:Content>
