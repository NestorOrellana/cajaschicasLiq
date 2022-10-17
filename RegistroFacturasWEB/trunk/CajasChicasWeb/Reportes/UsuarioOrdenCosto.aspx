<%@ Page Title="" Language="C#" MasterPageFile="~/Base.Master" AutoEventWireup="true"
    CodeBehind="UsuarioOrdenCosto.aspx.cs" Inherits="RegistroFacturasWEB.Reportes.UsuarioOrdenCosto" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .ReportControl
        {
            background-color: #9EBFF6;
            background-image: url(rs.aspx?image=Toolbar.Toolgrad.gif);
            background-repeat: repeat-x;
            border: 1px solid #95B7F3;
            color: gray;
            font-family: Arial;
            font-size: 65%;
            padding-left: 5px;
            float: left;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div>
        <h1>
            Reporte mapeo usuarios orden costo</h1>
        <rsweb:ReportViewer ID="ReportViewer1" runat="server" Width="100%" ShowPrintButton="true">
        </rsweb:ReportViewer>
    </div>
</asp:Content>
