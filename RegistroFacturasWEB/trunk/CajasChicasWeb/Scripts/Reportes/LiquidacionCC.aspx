<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="LiquidacionCC.aspx.cs"
    Inherits="RegistroFacturasWEB.Reportes.LiquidacionCC" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:HiddenField ID="hfIdCajaChica" runat="server" Value="0" />
        <asp:HiddenField ID="hfOpcion" runat="server" Value="0" />
        <asp:Table ID="tblPaginaEncabezado" runat="server" Width="100%">
            <asp:TableRow>
                <asp:TableCell CssClass="headerPage">
                Liquidación de caja chica
                </asp:TableCell>
            </asp:TableRow>
        </asp:Table>
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <rsweb:ReportViewer ID="ReportViewer1" runat="server" Width="100%">
        </rsweb:ReportViewer>
    </div>
    </form>
</body>
</html>
