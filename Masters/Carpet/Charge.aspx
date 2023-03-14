<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Charge.aspx.cs" Inherits="Charge"
    EnableEventValidation="false" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <script src="../../Scripts/JScript.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery-1.4.1.js" type="text/javascript"></script>
    <link href="../../App_Themes/Default/Style.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript">
        function refresh() {
            window.opener.document.getElementById('refresh').click();
            self.close();
        }
    </script>
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <table>
            <tr>
                <td class="tdstyle">
                    Charge Name
                </td>
                <td>
                    <asp:TextBox ID="txtchargename" runat="server" Width="150px"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="tdstyle">
                    Percentage
                </td>
                <td>
                    <asp:TextBox ID="txtpercentage" runat="server" Width="150px"></asp:TextBox>
                </td>
            </tr>
        </table>
        <table>
            <tr>
                <td>
                    <asp:GridView ID="Gvchargename" runat="server" DataKeyNames="SrNo" OnSelectedIndexChanged="Gvchargename_SelectedIndexChanged"
                        OnRowDataBound="Gvchargename_RowDataBound" CssClass="grid-view" OnRowCreated="Gvchargename_RowCreated">
                        <HeaderStyle CssClass="gvheader" />
                        <AlternatingRowStyle CssClass="gvalt" />
                        <RowStyle CssClass="gvrow" />
                        <EmptyDataRowStyle CssClass="gvemptytext" />
                    </asp:GridView>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="LblError" runat="server" ForeColor="Red" Text=""></asp:Label>
                </td>
            </tr>
            <tr>
                <td align="right">
                    <asp:Button ID="btnnew" runat="server" Text="New" OnClick="btnnew_Click" CssClass="buttonnorm" />
                    <asp:Button ID="btnsave" runat="server" OnClientClick="return confirm('Do you want to save data?')"
                        Text="Save" OnClick="btnsave_Click" CssClass="buttonnorm" />
                    <asp:Button ID="btnclose" runat="server" Text="Close" OnClientClick="refresh();"
                        CssClass="buttonnorm" />
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
