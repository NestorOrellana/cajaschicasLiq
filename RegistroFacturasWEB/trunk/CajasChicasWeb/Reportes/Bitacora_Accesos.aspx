<%@ Page Title="" Language="C#" MasterPageFile="~/Base.Master" AutoEventWireup="true"
    CodeBehind="Bitacora_Accesos.aspx.cs" Inherits="RegistroFacturasWEB.Reportes.Bitacora_Accesos" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        table
        {
            display: table;
            border-collapse: separate;
            border-spacing: 2px;
            border-color: gray;
        }
        
        thead
        {
            display: table-header-group;
            vertical-align: middle;
            border-color: inherit;
        }
        
        tbody
        {
            display: table-row-group;
            vertical-align: middle;
            border-color: inherit;
        }
        
        tfoot
        {
            display: table-footer-group;
            vertical-align: middle;
            border-color: inherit;
        }
        
        table > tr
        {
            vertical-align: middle;
        }
        
        col
        {
            display: table-column;
        }
        
        colgroup
        {
            display: table-column-group;
        }
        
        tr
        {
            display: table-row;
            vertical-align: inherit;
            border-color: inherit;
        }
        
        td, th
        {
            display: table-cell;
            vertical-align: inherit;
        }
        
        th
        {
            font-weight: bold;
        }
        
        caption
        {
            display: table-caption;
            text-align: -webkit-center;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div>
        <h1>
            Reporte de Bitacora de Accesos
        </h1>
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <rsweb:ReportViewer ID="ReportViewer1" runat="server" Width="100%">
        </rsweb:ReportViewer>
    </div>
</asp:Content>
