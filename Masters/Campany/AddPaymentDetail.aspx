<%@ Page Language="C#" Title="PaymentDetail" AutoEventWireup="true" CodeFile="AddPaymentDetail.aspx.cs"
    Inherits="Masters_Campany_AddPaymentDetail" EnableEventValidation="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register Src="~/UserControls/ucmenu.ascx" TagName="ucmenu" TagPrefix="uc2" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Customer</title>
    <script src="../../Scripts/JScript.js" type="text/javascript"></script>
    <script src="../../Scripts/JScript.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery-1.4.1.min.js" type="text/javascript"></script>
    <link href="../../Styles/vijay.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript">
        function CloseForm() {
            var opener = window.opener;
            if (opener != null) {
                window.opener.document.getElementById('CPH_Form_BtnRefPaymentMode').click();
                self.close();
            }
            else if (opener != null) {
                window.opener.document.getElementById('CPH_Form_BtnRefPaymentModeCustom').click();
                self.close();
            }
            else {
                window.opener.document.getElementById('BtnRefPaymentMode').click();
                self.close();
            }

        }

      
    </script>
</head>
<body>
    <form id="CompInfo" runat="server">
    <%--Page Design--%>
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <table width="100%" border="1">
        <tr>
            <td align="center" height="inherit" valign="top" class="style1" colspan="2">
                <div id="1" style="height: auto" align="center">
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
                                        <asp:Label ID="Label1" Text=" Payment Name" runat="server" Font-Bold="true" />
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtPayment" runat="server" CssClass="textb" Height="16px" Width="182px"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="Please Enter PaymentName"
                                            ControlToValidate="txtPayment" ForeColor="Red" ValidationGroup="m">*</asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdstyle">
                                        <asp:Label ID="Label2" Text="  Description" runat="server" Font-Bold="true" />
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtdescription" runat="server" TextMode="MultiLine" CssClass="textb"
                                            Width="177px"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="style2" colspan="2">
                                        <asp:Label ID="lbl" runat="server" ForeColor="Red"></asp:Label>
                                        <asp:GridView ID="gdPayment" runat="server" Width="280px" AllowPaging="True" PageSize="6"
                                            OnPageIndexChanging="gdPayment_PageIndexChanging" OnRowDataBound="gdPayment_RowDataBound"
                                            OnSelectedIndexChanged="gdPayment_SelectedIndexChanged" DataKeyNames="SrNo" CssClass="grid-views"
                                            OnRowCreated="gdPayment_RowCreated">
                                            <HeaderStyle CssClass="gvheaders" />
                                            <AlternatingRowStyle CssClass="gvalts" />
                                            <RowStyle CssClass="gvrow" />
                                            <EmptyDataRowStyle CssClass="gvemptytext" />
                                        </asp:GridView>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2" align="right">
                                        <asp:Button ID="btnsave" runat="server" CssClass="buttonnorm" Text="Save" OnClientClick="return confirm('Do you want to save data?')"
                                            OnClick="btnsave_Click" ValidationGroup="m" />
                                        <asp:Button ID="btnclose" runat="server" CssClass="buttonnorm" Text="Close" OnClientClick="return CloseForm();" />
                                        <asp:Button ID="btndelete" runat="server" Text="Delete" OnClientClick="return confirm('Do you want to Delete data?')"
                                            CssClass="buttonnorm" Visible="false" OnClick="btndelete_Click" />
                                    </td>
                                </tr>
                            </table>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </td>
        </tr>
    </table>
    </form>
</body>
</html>
