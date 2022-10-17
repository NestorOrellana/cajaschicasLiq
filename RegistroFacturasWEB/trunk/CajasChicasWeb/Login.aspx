<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="RegistroFacturasWEB.Login" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script type="text/javascript" src="Scripts/jquery.min.js"></script>
    <script type="text/javascript" src="Scripts/jquery-ui.min.js"></script>
    <link href="App_Themes/Tema/Base.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="/Scripts/jquery.validate.js"></script>
    <script src="../Scripts/jquery-1.10.2.js" type="text/javascript"></script>
    <script src="/Scripts/jquery-1.7.2.min.js" type="text/javascript"></script>
    <script src="/Scripts/jquery.min.js" type="text/javascript"></script>
    <script src="/Scripts/jquery-ui.min.js" type="text/javascript"></script>
    <link href="/App_Themes/Tema/jquery-ui.css" rel="stylesheet" type="text/css" />
    <script src="/bootstrap/js/bootstrap.js" type="text/javascript"></script>
    <link href="/bootstrap/css/bootstrap.css" rel="stylesheet" type="text/css" />
    <link rel="shortcut icon" href="Images/favicon.ico" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge" /> 
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:ScriptManager ID="Scriptmanager1" runat="server">
        </asp:ScriptManager>
        <script type="text/javascript">
            Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(pageLoadedHandler);
            function pageLoadedHandler() {
                $('.Login .Textbox').focus(function () {
                    $(this).attr('class', 'Hover');
                });

                $('.Login .Textbox').blur(function () {
                    $(this).attr('class', 'Textbox');
                });

                $('.Login .Button2').blur(function () {
                    $(this).attr('class', 'Button2');
                });

                $('.Login .Button2').focus(function () {
                    $(this).attr('class', 'Hover2');
                });
            }
        </script>
    </div>
    <div class="content">
        <asp:Table ID="Table2" runat="server" HorizontalAlign="Center">
            <asp:TableRow>
                <asp:TableCell HorizontalAlign="Center" VerticalAlign="Bottom" CssClass="alert" Width="50px"
                    Height="50px">
                   
                </asp:TableCell>
                <asp:TableCell HorizontalAlign="Center" Font-Size="Larger" VerticalAlign="Bottom">
                    <asp:Label ID="Label1" runat="server" Text="SISTEMA PARA REGISTRO DE FACTURAS"></asp:Label>
                </asp:TableCell>
            </asp:TableRow>
        </asp:Table>
        <asp:Table ID="tblBackground" CssClass="LoginBackGround" runat="server" HorizontalAlign="Center"
            Height="28px" Width="360px">
            <asp:TableRow Height="30px">
                <asp:TableCell>
                </asp:TableCell>
            </asp:TableRow>
            <asp:TableRow ID="TableRow1" runat="server">
                <asp:TableCell ID="TableCell1" runat="server">
                    <asp:Table ID="tblLogin" runat="server" HorizontalAlign="Center">
                        <asp:TableRow ID="TableRow2" runat="server">
                            <asp:TableCell ID="TableCell2" runat="server">
                                <div style="float: left; margin-top: 10px; padding-left: 8px;">
                                    Dominio:
                                    <br>
                                    <asp:DropDownList ID="ddlDominio" runat="server" Style="border: solid 1px #ccc; background-color: #FFFFCC;
                                        width: 295px;" OnSelectedIndexChanged="ddlDominio_SelectedIndexChanged">
                                    </asp:DropDownList>
                                </div>
                                <asp:Login ID="lCMI" runat="server" CssClass="Login" TextLayout="TextOnTop" TitleText=""
                                    Width="22px" UserNameLabelText="Usuario" PasswordLabelText="Contrase&#241;a"
                                    RememberMeText="Recordar mi usuario." LoginButtonText="Conectarse" OnAuthenticate="lCMI_Authenticate">
                                    <FailureTextStyle Font-Size="Smaller" />
                                    <LoginButtonStyle CssClass="Button2" />
                                    <TextBoxStyle CssClass="Textbox" />
                                </asp:Login>
                            </asp:TableCell>
                        </asp:TableRow>
                    </asp:Table>
                </asp:TableCell>
            </asp:TableRow>
        </asp:Table>
        <asp:Table ID="tblAviso" runat="server" HorizontalAlign="Center">
            <asp:TableRow>
                <asp:TableCell HorizontalAlign="Center" VerticalAlign="Bottom" CssClass="alert" Width="50px"
                    Height="50px">
                   
                </asp:TableCell>
                <asp:TableCell HorizontalAlign="Center" Font-Size="Larger" VerticalAlign="Bottom">
                    <asp:Label ID="lblAlert" runat="server" Text="Ingrese el mismo usuario y contraseña que utiliza para Windows."></asp:Label>
                </asp:TableCell>
            </asp:TableRow>
        </asp:Table>
    </div>
    </form>
</body>
</html>
