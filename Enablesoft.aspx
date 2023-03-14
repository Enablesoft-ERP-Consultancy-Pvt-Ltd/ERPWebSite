<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Enablesoft.aspx.cs" Inherits="Enablesoft" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <link href="Styles/vijay.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
    <div style="margin: 5% 20% 0% 20%">
        <table style="width: 100%">
            <tr>
                <td style="width: 60%">
                    <asp:TextBox ID="txtpwd" CssClass="textb" placeholder="Type Encrypted value" Width="95%"
                        runat="server" />
                </td>
                <td>
                    <asp:Button ID="btngetdecryptvalue" Text="Decrypt" CssClass="buttonnorm" runat="server"
                        OnClick="btngetdecryptvalue_Click" />
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <asp:TextBox ID="lblval" Text="Decrypted value is...." ForeColor="Red" Font-Bold="true"
                        runat="server" Width="60%" />
                </td>
            </tr>
            <tr>
                <td style="width: 60%">
                    <asp:TextBox ID="txtencryptval" CssClass="textb" placeholder="Type Decrypted value"
                        Width="95%" runat="server" />
                </td>
                <td>
                    <asp:Button ID="btnencrypted" Text="Encrypt" CssClass="buttonnorm" runat="server"
                        OnClick="btnencrypted_Click" />
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <asp:TextBox ID="lblencryptval" Text="Encrypted value is...." ForeColor="Red" Font-Bold="true"
                        runat="server" Width="60%" />
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
