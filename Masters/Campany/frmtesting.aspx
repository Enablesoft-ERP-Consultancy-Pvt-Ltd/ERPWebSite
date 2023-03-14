<%@ Page Language="C#" AutoEventWireup="true" CodeFile="frmtesting.aspx.cs" Inherits="Masters_Campany_frmtesting"
    EnableEventValidation="false" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Customer</title>
    <style type="text/css">
        .textbox
        {
        }
    </style>
</head>
<body>
    <form id="CompInfo" runat="server">
    <%--Page Design--%>
    <table width="100%" border="1">
        <tr>
            <td>
                <asp:TextBox ID="txtMobileNo" runat="server" CssClass="textb"></asp:TextBox>
            </td>
            <td>
                <asp:Button Width="100%" BorderStyle="None" BackColor="#999999" ID="BtnLogout" runat="server"
                    Text="Logout" OnClick="BtnLogout_Click" />
            </td>
        </tr>
    </table>
    </form>
</body>
</html>
