<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AddDescriptionGoods.aspx.cs"
    Inherits="Masters_Carpet_AddDescriptionGoods" EnableEventValidation="false" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script src="../../Scripts/JScript.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery-1.4.1.min.js" type="text/javascript"></script>
    <script type="text/javascript">
        function NewForm() {
            window.location.href = "AddDescriptionGoods.aspx";
        }
        function CloseForm() {
            window.opener.document.getElementById('refreshcolor').click();
            self.close();
        }
    </script>
    <link href="../../App_Themes/Default/Style.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <div style="margin-left: 15%; margin-right: 15%">
                    <table>
                        <tr>
                            <td>
                                <asp:Label ID="lblDescriptionGoods" class="tdstyle" runat="server" Text="Description Of Goods"></asp:Label>
                                <br />
                                <br />
                            </td>
                            <td>
                                <asp:TextBox CssClass="textb" ID="TxtDescriptionOfGoods" runat="server" Width="250px"></asp:TextBox>
                                <br />
                                <br />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <asp:Label ID="lblErrorMessage" runat="server" Text="Error Message"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2" style="text-align: right">
                                <asp:Button ID="BtnNew" runat="server" Text="New" CssClass="buttonnorm" OnClientClick="return NewForm();" />
                                <asp:Button ID="btnSave" Text="Save" runat="server" CssClass="buttonnorm" Width="53px"
                                    OnClientClick="return confirm('Do You Want To Save Data?')" OnClick="btnSave_Click" />
                                <asp:Button ID="btnClear" Text="Close" runat="server" CssClass="buttonnorm" Width="53px"
                                    OnClientClick="return CloseForm();" />
                                <asp:Button ID="btndelete" runat="server" Text="Delete" OnClientClick="return confirm('Do You Want To Delete Data?')"
                                    CssClass="buttonnorm" Visible="False" />
                            </td>
                        </tr>
                    </table>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    </form>
</body>
</html>
