<%@ Page Language="C#" AutoEventWireup="true" CodeFile="frmReportForInvoiceAmtDetail.aspx.cs"
    Inherits="Masters_ReportForms_frmReportForInvoiceAmtDetail" MasterPageFile="~/ERPmaster.master"
    Title="INVOICE PAYMENT DETAILS" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="page" ContentPlaceHolderID="CPH_Form" runat="server">
    <script src="../../Scripts/JScript.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery-1.4.1.js" type="text/javascript"></script>
    <%--<script type="text/javascript" src="../../Scripts/Fixfocus.js"></script>--%>
    <asp:UpdatePanel ID="Updatepanel1" runat="server">
        <ContentTemplate>
            <div style="width: 800px; background-color: #edf3fe; height: 600px; margin: 0px 0px 0px 0px">
                <div style="margin: auto; width: 500px;">
                    <div style="width: 420px; margin-top: 20px; position: relative; float: right; top: 0px;
                        left: 0px; height: 150px; padding-top: 10px; padding-left: 10px">
                        <table style="height: 150px; border: 5px solid #c8e5f6;">
                            <tr>
                                <td>
                                    <table style="border: 1px solid #c4e4f2; background-color: #effaff" cellspacing="0"
                                        cellpadding="0" border="0">
                                        <tr style="height: 25px" runat="server">
                                            <td>
                                                <asp:Label ID="lblFromDate" runat="server" CssClass="labelbold" Text="From Date"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtFromDate" runat="server" Width="100px" CssClass="textb" BackColor="Yellow"
                                                    ToolTip="Click to Change Date"></asp:TextBox>
                                                <asp:CalendarExtender ID="CalenderExtenderfromDate" runat="server" TargetControlID="txtFromDate"
                                                    Format="dd-MMM-yyyy">
                                                </asp:CalendarExtender>
                                            </td>
                                            <td>
                                                <asp:Label ID="lblTodate" runat="server" CssClass="labelbold" Text="To Date"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txttoDate" runat="server" Width="100px" BackColor="Yellow" CssClass="textb"
                                                    ToolTip="Click to Change Date"></asp:TextBox>
                                                <asp:CalendarExtender ID="CalendarExtendertoDate" runat="server" TargetControlID="txttoDate"
                                                    Format="dd-MMM-yyyy">
                                                </asp:CalendarExtender>
                                            </td>
                                        </tr>
                                        <tr style="height: 25px">
                                            <td>
                                                <asp:Label ID="lblCustomerCode" runat="server" Text="CustomerCode" CssClass="labelbold"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="DDCustomerCode" runat="server" CssClass="dropdown" Width="150px"
                                                    OnSelectedIndexChanged="DDCustomerCode_SelectedIndexChanged" AutoPostBack="true">
                                                </asp:DropDownList>
                                                <asp:ListSearchExtender ID="ListsearchExtender1" runat="server" TargetControlID="DDCustomerCode"
                                                    PromptCssClass="labelbold" PromptPosition="Bottom" ViewStateMode="Disabled">
                                                </asp:ListSearchExtender>
                                            </td>
                                        </tr>
                                        <tr style="height: 25px">
                                            <td>
                                                <asp:Label ID="lblInvoiceNo" runat="server" Text="Invoice No" CssClass="labelbold"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="DDInvoiceNo" runat="server" CssClass="dropdown" Width="150px">
                                                </asp:DropDownList>
                                                <asp:ListSearchExtender ID="Listsearchextender2" runat="server" TargetControlID="DDinvoiceNo"
                                                    PromptCssClass="labelbold" PromptPosition="Bottom" ViewStateMode="Disabled">
                                                </asp:ListSearchExtender>
                                            </td>
                                        </tr>
                                        <tr style="height: 25px">
                                            <td colspan="4" align="right">
                                                <asp:Button ID="btnPrint" Text="Print" runat="server" Width="100px" CssClass="buttonnorm"
                                                    OnClick="btnPrint_Click" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="4">
                                                <asp:Label ID="lblErrormsg" runat="server" Text="" CssClass="labelbold" ForeColor="Red"></asp:Label>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </div>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
