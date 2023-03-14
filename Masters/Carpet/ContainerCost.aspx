<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ContainerCost.aspx.cs" Inherits="Masters_Carpet_PackingCost" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script type="text/javascript">
        function RefreshCombo() {
            var VarCompanyNo = document.getElementById('txtVarCompanyNo').value;
            var PackingType = document.getElementById('TxtPackingType').value;

            if (PackingType == 5 && VarCompanyNo == 2) {
                window.opener.document.getElementById('txtContainerPackingCost').value = document.getElementById('TxtNetcost').value;
                self.close();
            }
            else {
                self.close();
            }
        }
    </script>
    <link href="../../App_Themes/Default/Style.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <table width="600px">
            <tr>
                <td>
                </td>
                <td align="center" style="width: 80px" class="tdstyle">
                    Unit
                </td>
                <td>
                    <asp:DropDownList ID="DDunit" runat="server" Width="80px" AutoPostBack="True" CssClass="dropdown">
                        <asp:ListItem>Inch</asp:ListItem>
                        <asp:ListItem>Cms</asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td class="style2">
                </td>
                <td align="center" style="width: 100px" class="tdstyle">
                    Length
                </td>
                <td>
                    <asp:TextBox ID="TxtLength" runat="server" Width="100px" AutoPostBack="True" OnTextChanged="TxtLength_TextChanged"></asp:TextBox>
                </td>
                <td align="center" class="tdstyle">
                    Width
                </td>
                <td>
                    <asp:TextBox ID="TxtWidth" runat="server" Width="100px" AutoPostBack="True" OnTextChanged="TxtWidth_TextChanged"></asp:TextBox>
                </td>
                <td align="center" style="width: 80px" class="tdstyle">
                    Height
                </td>
                <td>
                    <asp:TextBox ID="TxtHeight" runat="server" Width="100px" AutoPostBack="True" OnTextChanged="TxtHeight_TextChanged"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="style2">
                </td>
                <td align="center" style="width: 80px" class="tdstyle">
                    PCS
                </td>
                <td>
                    <asp:TextBox ID="TxtPCS" runat="server" Width="100px" AutoPostBack="True" OnTextChanged="TxtPCS_TextChanged"></asp:TextBox>
                </td>
                <td align="center" class="tdstyle">
                    Container CBM
                </td>
                <td>
                    <asp:TextBox ID="TxtContainerCBM" runat="server" Width="100px" AutoPostBack="True"
                        OnTextChanged="TxtContainerCBM_TextChanged"> </asp:TextBox>
                </td>
                <td align="center" class="tdstyle">
                    Container Cost
                </td>
                <td>
                    <asp:TextBox ID="TxtContainerCost" runat="server" Width="100px" AutoPostBack="True"
                        OnTextChanged="TxtContainerCost_TextChanged"> </asp:TextBox>
                </td>
                <td class="tdstyle">
                    <asp:Label ID="lblBoxCBM" runat="server" Text="BoxCBM"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                </td>
                <td>
                </td>
                <td>
                </td>
                <td>
                </td>
                <td>
                </td>
                <td>
                </td>
                <td>
                </td>
                <td class="tdstyle">
                    <asp:Label ID="lblPcsCBM" runat="server" Text="PCSCBM"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                </td>
                <td>
                    <asp:TextBox ID="TxtPackingType" runat="server" Height="0px" Width="0px"></asp:TextBox>
                </td>
                <td>
                    <asp:TextBox ID="txtVarCompanyNo" runat="server" Height="0px" Width="0px"></asp:TextBox>
                </td>
                <td>
                </td>
                <td>
                </td>
                <td>
                </td>
                <td>
                </td>
                <td class="tdstyle">
                    <asp:Label ID="lblNoOFPCS" runat="server" Text="NoOFPCS"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                </td>
                <td>
                </td>
                <td class="tdstyle">
                    Net Cost
                </td>
                <td>
                    <asp:TextBox ID="TxtNetcost" runat="server"></asp:TextBox>
                </td>
                <td>
                </td>
                <td>
                </td>
                <td>
                    <asp:Button ID="BtnClose" OnClientClick="RefreshCombo();" runat="server" Text="Close"
                        OnClick="BtnClose_Click" CssClass="buttonnorm" />
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
