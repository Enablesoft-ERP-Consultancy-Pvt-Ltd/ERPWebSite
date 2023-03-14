<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AddCustomerType.aspx.cs"
    Inherits="Masters_Campany_AddCustomerType" EnableEventValidation="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="../../UserControls/SearchA.ascx" TagName="SearchA" TagPrefix="uc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register Src="../../UserControls/ucmenu.ascx" TagName="ucmenu" TagPrefix="uc2" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <script src="../../Scripts/JScript.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery-1.4.1.js" type="text/javascript"></script>
    <link href="../../Styles/vijay.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript">
        function CloseForm() {
            window.opener.document.getElementById('CPH_Form_btnCustomerType').click();
            self.close();
        }
    </script>
    <style type="text/css">
        .style2
        {
            width: 152px;
        }
        .textbox
        {
        }
        .style3
        {
            width: 85px;
        }
    </style>
</head>
<body>
    <form id="frmCarriage" runat="server">
    <asp:ScriptManager ID="scriptmanager1" runat="server">
    </asp:ScriptManager>
    <div>
        <table>
            <tr>
                <td>
                    <asp:TextBox ID="txtid" runat="server" Visible="False"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:UpdatePanel ID="updatepanel1" runat="server">
                        <ContentTemplate>
                            <table>
                                <tr>
                                    <td class="tdstyle">
                                        <asp:Label ID="lblCustomerType" runat="server" Text="Type of Customer" Font-Bold="true"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtCustomerType" runat="server" CssClass="textb" Width="177px"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="Please Enter Customer Type"
                                            ControlToValidate="txtCustomerType" ForeColor="Red" ValidationGroup="m">*</asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="style2" colspan="2" align="center">
                                        <asp:Label ID="lbl" runat="server" ForeColor="Red"></asp:Label>
                                        <asp:GridView ID="gdCarriage" runat="server" Width="250px" AllowPaging="True" PageSize="6"
                                            DataKeyNames="CustomerTypeId" OnPageIndexChanging="gdCarriage_PageIndexChanging"
                                            Style="margin-left: 20px" OnRowDataBound="gdCarriage_RowDataBound" CssClass="grid-views"
                                            OnRowCreated="gdCarriage_RowCreated" AutoGenerateColumns="False" OnSelectedIndexChanged="gdCarriage_SelectedIndexChanged">
                                            <HeaderStyle CssClass="gvheaders" />
                                            <AlternatingRowStyle CssClass="gvalts" />
                                            <RowStyle CssClass="gvrow" />
                                            <Columns>
                                                <asp:BoundField DataField="TypeName" HeaderText="Customer Type">
                                                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                </asp:BoundField>
                                            </Columns>
                                            <EmptyDataRowStyle CssClass="gvemptytext" />
                                        </asp:GridView>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2" align="right">
                                        <asp:Button ID="btnsave" runat="server" CssClass="buttonnorm" Text="Save" OnClick="btnsave_Click"
                                            ValidationGroup="m" />
                                        <asp:Button ID="btnclose" runat="server" CssClass="buttonnorm" Text="Close" OnClientClick="return CloseForm();" />
                                        <asp:Button ID="btndelete" runat="server" Text="Delete" CssClass="buttonnorm" OnClientClick="return confirm('Do you want to Delete data?')"
                                            OnClick="btndelete_Click" Visible="False" />
                                    </td>
                                </tr>
                            </table>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </td>
            </tr>
        </table>
        <%--Page Working table Ends--%>
    </div>
    </form>
</body>
</html>
