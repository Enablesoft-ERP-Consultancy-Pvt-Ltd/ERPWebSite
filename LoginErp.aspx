<%@ Page Language="C#" AutoEventWireup="true" CodeFile="LoginErp.aspx.cs" Inherits="Login" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

  
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Login</title>
    <meta content="blendTrans(Duration=0.5)" http-equiv="Page-Enter" />
    <meta content="blendTrans(Duration=0.5)" http-equiv="Page-Exit" />
    <script src="../../Scripts/jquery-1.4.1.min.js" type="text/javascript"></script>
    <%-- <link href="../../App_Themes/Default/Style.css" rel="stylesheet" type="text/css" />--%>
    <link href="App_Themes/Default/Style.css" rel="Stylesheet" type="text/css" />
    <script src="../../Scripts/JScript.js" type="text/javascript">    
    </script>
    <script type="text/javascript">
        function refresh() {
            document.getElementById("<%=txtUser.ClientID%>").value = "";
            document.getElementById("<%=txtPassword.ClientID%>").value = "";
        }
    </script>
</head>
<body bgcolor="#edf3fe">
    <form id="form1" runat="server">
    <div>
        <%--<span style="color:Black; margin-left:40%; font-family:Arial; font-size:xx-large"><strong ><em >Export-Erp...</em></strong></span>--%>
        <div align="right" style="margin-top: 5%; margin-right: 20%">
            <table>
                <tr>
                    <td align="right" style="">
                        <asp:Label ID="Label1" runat="server" Text="Export" Font-Bold="True" Font-Italic="True"
                            Font-Size="XX-Large" ForeColor="Red"></asp:Label>
                        <asp:Label ID="Label2" runat="server" Text="-Erp..." Font-Bold="True" Font-Italic="True"
                            Font-Size="X-Large"></asp:Label>
                        <br />
                        <asp:Label ID="Label3" runat="server" Text="Entrerprise Resource Planning" Font-Bold="True"
                            Font-Italic="True" Font-Size="Small"></asp:Label>
                    </td>
                </tr>
            </table>
            <table>
                <tr>
                    <td>
                        <table>
                            <tr>
                                <td class="labelbold">
                                    Username
                                </td>
                                <td>
                                    <asp:TextBox ID="txtUser" Width="160px" runat="server"></asp:TextBox>
                                </td>
                                <td>
                                    <asp:RequiredFieldValidator ID="reqUser" runat="server" ControlToValidate="txtUser"
                                        CssClass="errormsg" ErrorMessage="Please, Enter Login Name!" ValidationGroup="form1">*</asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <td class="labelbold">
                                    Password
                                </td>
                                <td>
                                    <asp:TextBox ID="txtPassword" runat="server" Width="160px" TextMode="Password"></asp:TextBox>
                                </td>
                                <td>
                                    <asp:RequiredFieldValidator ID="reqPassword" runat="server" ControlToValidate="txtPassword"
                                        CssClass="errormsg" ErrorMessage="Please , Enter Password!" ValidationGroup="form1">*</asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <asp:Button ID="btnLogin" runat="server" Text="Login" CssClass="buttonnorm" OnClick="btnLogin_Click"
                                        ValidationGroup="form1" />&nbsp;
                                    <asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="buttonnorm" OnClientClick="javascript:refresh();" /><br />
                                    <asp:HyperLink ID="forgotpwd" runat="server" NavigateUrl="~/ForgotPassword.aspx">Forgot Password</asp:HyperLink>
                                    <br />
                                    <br />
                                    <br />
                                </td>
                            </tr>
                            <tr>
                                <td colspan="3">
                                    <asp:Button ID="btnnew" runat="server" Text="Create New User&gt;&gt;" OnClick="btnnew_Click"
                                        CssClass="buttonnorm" Width="200px" />
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td>
                        <b></b>
                        <asp:Label ID="lblErr" runat="server" CssClass="errormsg" ForeColor="Red"></asp:Label>
                    </td>
                </tr>
            </table>
            <div id="divStatus" visible="false" runat="server">
                <asp:Image ID="Image1" ImageUrl="~/Images/CancelImage.jpg" runat="server" Style="margin-top: 0px"
                    Width="20px" />
                <asp:Label ID="Label4" runat="server" Text="Sorry ! We can not complete your request."
                    ForeColor="Red"></asp:Label><br />
                <asp:Label ID="Label5" runat="server" Text="Either Username OR Password is invalid. Kindly try again.!"
                    ForeColor="Red"></asp:Label>
            </div>
        </div>
    </div>
    <br />
    <br />
    <br />
    <br />
    <br />
    <br />
    <br />
    <br />
    <br />
    <br />
    <br />
    <asp:ScriptManager ID="scr" runat="server">
    </asp:ScriptManager>
    <table width="100%">
    <tr><asp:Button ID="btnshow" runat="server" Style="display: none" />
    <td>
    <asp:ModalPopupExtender ID="ModalPopupExtender1" runat="server"  TargetControlID="btnshow"
     PopupControlID="uprgWorkRequest" BackgroundCssClass="modalBackground" ></asp:ModalPopupExtender>
    <asp:UpdateProgress ID="uprgWorkRequest"  runat="server">
        <ProgressTemplate>
            <div id="IMGDIV1" align="center" valign="middle" runat="server" style="position: absolute;
                visibility: visible; vertical-align: middle; border-style: none; border-color: black;
                background-color: transparent;">
                <asp:Image ID="Image2" runat="server" ImageUrl="~/Images/ajax-loader.gif" />Loading...
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>
    </td>
    </tr>
    </table>
    <div align="right" style="padding-right: 40%; color: Gray">
        <span class="labelcopyr">&copy; Enablesoft Erp Consultancy Pvt. Ltd.All rights reserved.</span></div>
    </form>
</body>
</html>
