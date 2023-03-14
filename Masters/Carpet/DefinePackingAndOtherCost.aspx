<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DefinePackingAndOtherCost.aspx.cs"
    Inherits="Masters_Carpet_DefinePackingAndOtherCost" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script language="javascript" type="text/javascript">
        function Calculate() {
            window.open('PackingCost.aspx', '', 'width=700px,Height=400px');
        }
        function closeForm() {
            window.close();
        }
        function validate() {
            var OrderNo = document.getElementById('DDCompanyName').value;
            if (OrderNo <= 0) {
                alert('Pls Select Company Name')
                return false;
            }
            else
                OrderNo = document.getElementById('DDCustomerName').value;
            if (OrderNo <= 0) {
                alert('Pls Select Customer Name')
                return false;
            }
            else
                OrderNo = document.getElementById('DDOrderNo').value;
            if (OrderNo <= 0) {
                alert('Pls Select Order No')
                return false;
            }
            else
                OrderNo = document.getElementById('txtPackingCost').Text;
            if (OrderNo == "") {
                alert('Pls Enter Packing Amount')
                return false;
            }
            else {
                return true;
            }
        }
    </script>
    <link href="../../App_Themes/Default/Style.css" rel="stylesheet" type="text/css" />
</head>
<body style="width: 680px">
    <form id="form1" runat="server">
    <div>
        <table style="width: 680px">
            <tr>
                <td class="tdstyle">
                    Company Name
                </td>
                <td class="tdstyle">
                    Customer Name
                </td>
                <td class="tdstyle">
                    Order Number
                </td>
                <td class="tdstyle">
                    Cal Type
                </td>
            </tr>
            <tr>
                <td>
                    <asp:DropDownList ID="DDCompanyName" runat="server" OnSelectedIndexChanged="DDCompanyName_SelectedIndexChanged"
                        Width="175px" AutoPostBack="True" CssClass="dropdown">
                    </asp:DropDownList>
                    <cc1:ListSearchExtender ID="ListSearchExtender1" runat="server" TargetControlID="DDCompanyName"
                        ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                    </cc1:ListSearchExtender>
                </td>
                <td>
                    <asp:DropDownList ID="DDCustomerName" runat="server" OnSelectedIndexChanged="DDCustomerName_SelectedIndexChanged"
                        Width="175px" AutoPostBack="True" CssClass="dropdown">
                    </asp:DropDownList>
                    <cc1:ListSearchExtender ID="ListSearchExtender2" runat="server" TargetControlID="DDCustomerName"
                        ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                    </cc1:ListSearchExtender>
                </td>
                <td>
                    <asp:DropDownList ID="DDOrderNo" runat="server" Width="100px" AutoPostBack="True"
                        CssClass="dropdown">
                    </asp:DropDownList>
                    <cc1:ListSearchExtender ID="ListSearchExtender3" runat="server" TargetControlID="DDOrderNo"
                        ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                    </cc1:ListSearchExtender>
                </td>
                <td>
                    <asp:DropDownList ID="ddCalType" runat="server" Width="100px" CssClass="dropdown">
                        <asp:ListItem>AREA WISE</asp:ListItem>
                        <asp:ListItem>PCS WISE</asp:ListItem>
                    </asp:DropDownList>
                    <cc1:ListSearchExtender ID="ListSearchExtender4" runat="server" TargetControlID="ddCalType"
                        ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                    </cc1:ListSearchExtender>
                </td>
            </tr>
            <tr>
                <td class="tdstyle">
                    Packing Cost
                    <asp:Button ID="btnpackingMatCost" OnClientClick="return Calculate();" runat="server"
                        Text="..." Height="19px" CssClass="buttonsmall" />
                </td>
            </tr>
            <tr>
                <td>
                    <asp:TextBox ID="txtPackingCost" runat="server"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <asp:GridView ID="DG" runat="server" CssClass="grid-view" OnRowCreated="DG_RowCreated">
                </asp:GridView>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lblMessage" runat="server" Text="" ForeColor="Red"></asp:Label>
                </td>
                <td>
                </td>
                <td>
                </td>
                <td>
                    <asp:Button ID="BtnSave" runat="server" Text="Save" OnClick="BtnSave_Click" OnClientClick="return validate();"
                        CssClass="buttonnorm" />
                    <asp:Button ID="Close" runat="server" OnClientClick="return closeForm();" Text="Close"
                        CssClass="buttonnorm" />
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
