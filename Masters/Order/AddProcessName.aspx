<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AddProcessName.aspx.cs" Inherits="Masters_Order_AddProcessName"
    EnableEventValidation="false" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script src="../../Scripts/JScript.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery-1.4.1.min.js" type="text/javascript"></script>
    <link href="../../Styles/vijay.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript"></script>
    <script language="javascript" type="text/javascript">
        function CloseForm() {
            var objParent = window.opener;
            if (objParent != null) {
                self.close();
            }
            else {
                window.location.href = "../../main.aspx";
            }
        }        
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager ID="scriptmanager1" runat="server">
    </asp:ScriptManager>
    <table>
        <tr>
            <td align="center" height="inherit" valign="top" class="style1" colspan="2">
                <div id="1" style="height: auto" align="left">
                    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                        <ContentTemplate>
                            <div>
                                <table>
                                    <tr>
                                        <td>
                                            <asp:CheckBox ID="ChkForAllSelect" Text="Select For All Process" CssClass="checkboxbold"
                                                runat="server" AutoPostBack="True" OnCheckedChanged="ChkForAllSelect_CheckedChanged" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="tdstyle" id="TDProcessEmployeeName" runat="server">
                                            <div style="height: 500px; width: 100%; overflow: scroll">
                                                <asp:Label ID="LblProcessName" runat="server" Text="Process Name" CssClass="labelbold"></asp:Label>
                                                <br />
                                                <asp:CheckBoxList ID="ChkBoxListProcessName" runat="server" Width="300px" CssClass="checkboxnormal">
                                                </asp:CheckBoxList>
                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="right">
                                            <asp:Button CssClass="buttonnorm" ID="BtnSave" Text="Save" runat="server" OnClick="BtnSave_Click" />
                                            <asp:Button CssClass="buttonnorm" ID="BtnClose" runat="server" Text="Close" OnClientClick="return CloseForm();" />
                                        </td>
                                    </tr>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </td>
        </tr>
    </table>
    </form>
</body>
</html>
