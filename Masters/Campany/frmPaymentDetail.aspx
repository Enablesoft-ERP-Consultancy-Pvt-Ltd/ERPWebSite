<%@ Page Language="C#" Title="PaymentDetail" AutoEventWireup="true" MasterPageFile="~/ERPmaster.master"
    CodeFile="frmPaymentDetail.aspx.cs" Inherits="Masters_Campany_frmPaymentDetail"
    EnableEventValidation="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Page" ContentPlaceHolderID="CPH_Form" runat="server">
    <script src="../../Scripts/JScript.js" type="text/javascript"></script>
    <link href="../../Styles/vijay.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript">
        function CloseForm() {
            window.location.href = "../../main.aspx"
        }
    </script>
    <script src="../../Scripts/jquery-1.4.1.js" type="text/javascript"></script>
    <div style="margin-left: 15%; margin-right: 15%; height: 480px">
        <table>
            <tr>
                <td>
                    <asp:TextBox ID="txtid" runat="server" Visible="False"></asp:TextBox>
                </td>
            </tr>
        </table>
        <asp:UpdatePanel ID="updatepanel1" runat="server">
            <ContentTemplate>
                <table style="width: 30">
                    <tr>
                        <td class="tdstyle">
                            <asp:Label ID="lbl5" Text="Payment Name" runat="server" Font-Bold="true" />
                        </td>
                        <td>
                            <asp:TextBox ID="txtPayment" runat="server" CssClass="textb" Height="16px" Width="177px"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="Please Enter PaymentName"
                                ControlToValidate="txtPayment" ForeColor="Red" ValidationGroup="m">*</asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdstyle">
                            <asp:Label ID="Label1" Text="Description" runat="server" Font-Bold="true" />
                        </td>
                        <td>
                            <asp:TextBox ID="txtdescription" runat="server" TextMode="MultiLine" CssClass="textb"
                                Height="16px" Width="177px" onkeydown="return (event.keyCode!=13);"></asp:TextBox>
                        </td>
                    </tr>
                </table>
                <table>
                    <tr>
                        <td class="style2" colspan="2">
                            <asp:Label ID="lbl" runat="server" ForeColor="Red"></asp:Label>
                            <asp:GridView ID="gdPayment" runat="server" Width="280px" AllowPaging="True" CellPadding="4"
                                PageSize="6" ForeColor="#333333" OnPageIndexChanging="gdPayment_PageIndexChanging"
                                OnRowDataBound="gdPayment_RowDataBound" OnSelectedIndexChanged="gdPayment_SelectedIndexChanged"
                                DataKeyNames="SrNo" CssClass="grid-views">
                                <HeaderStyle CssClass="gvheaders" />
                                <AlternatingRowStyle CssClass="gvalts" />
                                <RowStyle CssClass="gvrow" />
                                <PagerStyle CssClass="PagerStyle" />
                                <EmptyDataRowStyle CssClass="gvemptytext" />
                            </asp:GridView>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <asp:Button ID="btnsave" runat="server" CssClass="buttonnorm" Text="Save" OnClientClick="return confirm('Do you want to save data?')"
                                OnClick="btnsave_Click" ValidationGroup="m" Style="margin-left: 180px" />
                            <asp:Button ID="btnclose" runat="server" CssClass="buttonnorm" Text="Close" OnClientClick="return CloseForm();" />
                            <asp:Button ID="btndelete" runat="server" Text="Delete" OnClientClick="return confirm('Do you want to Delete data?')"
                                CssClass="buttonnorm" Visible="false" OnClick="btndelete_Click" />
                        </td>
                    </tr>
                </table>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</asp:Content>
