<%@ Page Language="C#" AutoEventWireup="true" CodeFile="frmShippingAgent.aspx.cs"
    Inherits="ShippingMaster" EnableEventValidation="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register Src="~/UserControls/ucmenu.ascx" TagName="ucmenu" TagPrefix="uc2" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script src="../../Scripts/JScript.js" type="text/javascript"></script>
    <script type="text/javascript">
        function CloseForm() {
            window.location.href = "../../main.aspx"
        }
    </script>
    <style type="text/css">
        .style1
        {
            width: 166px;
        }
        .style2
        {
            width: 152px;
        }
        .textbox
        {
        }
    </style>
    <link href="../../App_Themes/Default/Style.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager ID="scriptmanager1" runat="server">
    </asp:ScriptManager>
    <div style="width: 90%">
        <table>
            <tr>
                <td>
                    <asp:TextBox ID="txtid" runat="server" Visible="False"></asp:TextBox>
                </td>
            </tr>
        </table>
        <asp:UpdatePanel ID="updatepanel1" runat="server">
            <ContentTemplate>
                <table>
                    <tr>
                        <td class="tdstyle">
                            Company Name
                        </td>
                        <td>
                            <asp:TextBox ID="txtCompanyName" runat="server" CssClass="textb">
                            </asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="Please Enter CompanyName"
                                ControlToValidate="txtCompanyName">*</asp:RequiredFieldValidator>
                        </td>
                        <td class="tdstyle">
                            Address
                        </td>
                        <td colspan="3">
                            <asp:TextBox ID="txtAddress" runat="server" CssClass="textb" Width="345px"></asp:TextBox>
                        </td>
                        <td class="tdstyle">
                            Contact Person
                        </td>
                        <td>
                            <asp:TextBox ID="txtContactPerson" runat="server" CssClass="textb">
                            </asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdstyle">
                            Phone
                        </td>
                        <td>
                            <asp:TextBox ID="txtPhone" runat="server" CssClass="textb">
                            </asp:TextBox>
                        </td>
                        <td class="tdstyle">
                            Mobile
                        </td>
                        <td>
                            <asp:TextBox ID="txtMobile" runat="server" CssClass="textb">
                            </asp:TextBox>
                        </td>
                        <td class="tdstyle">
                            Email
                        </td>
                        <td>
                            <asp:TextBox ID="txtEmail" runat="server" CssClass="textb">
                            </asp:TextBox>
                        </td>
                        <td class="tdstyle">
                            Fax
                        </td>
                        <td>
                            <asp:TextBox ID="txtFax" runat="server" CssClass="textb">
                            </asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="style2" colspan="8">
                            <asp:ValidationSummary ID="ValidationSummary1" runat="server" Font-Bold="true" Font-Italic="true"
                                Font-Names="Times new Roman" Font-Overline="false" ForeColor="Red" ShowMessageBox="false"
                                ShowSummary="true" />
                            <br />
                        </td>
                    </tr>
                    <tr>
                        <td class="style2" colspan="8">
                            <asp:GridView ID="gdShippingMaster" runat="server" OnRowDataBound="gdShippingMaster_RowDataBound"
                                OnSelectedIndexChanged="gdShippingMaster_SelectedIndexChanged" Width="840px"
                                AllowPaging="True" CellPadding="4" PageSize="6" ForeColor="#333333" OnPageIndexChanging="gdShippingMaster_PageIndexChanging"
                                DataKeyNames="Agentid" CssClass="grid-view" OnRowCreated="gdShippingMaster_RowCreated">
                                <HeaderStyle CssClass="gvheader" />
                                <AlternatingRowStyle CssClass="gvalt" />
                                <RowStyle CssClass="gvrow" />
                                <PagerStyle CssClass="PagerStyle" />
                                <EmptyDataRowStyle CssClass="gvemptytext" />
                            </asp:GridView>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                        </td>
                        <td colspan="2">
                        </td>
                        <td colspan="2">
                        </td>
                        <td colspan="2">
                            <asp:Button ID="btnsave" runat="server" CssClass="buttonnorm" Text="Save" OnClick="btnsave_Click" />
                            <asp:Button ID="btnclose" runat="server" CssClass="buttonnorm" Text="Close" OnClientClick="return CloseForm();"
                                OnClick="btnclose_Click" />
                            <asp:Label ID="Label1" runat="server" Text=""></asp:Label>
                        </td>
                    </tr>
                </table>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    </form>
</body>
</html>
