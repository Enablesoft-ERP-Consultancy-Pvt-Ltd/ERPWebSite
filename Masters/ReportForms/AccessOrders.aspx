<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AccessOrders.aspx.cs" Inherits="AccessOrders"
    MasterPageFile="~/ERPmaster.master" Title="Confirm/Unconfirm DraftOrder" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="page" ContentPlaceHolderID="CPH_Form" runat="server">
    <!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
    <%@ register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>
    <link href="Styles/vijay.css" rel="stylesheet" type="text/css" />
    <%--<script type="text/javascript" src="../../Scripts/Fixfocus.js"></script>--%>
    <asp:UpdatePanel runat="server">
        <ContentTemplate>
            <div style="width: 100%; margin-left: 30%">
                <table>
                    <tr>
                        <td>
                            <asp:Label Text="Company Name" ID="Companylbl" runat="server" CssClass="labelbold" />
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlCompanyname" runat="server" Width="300px" CssClass="dropdown"
                                OnSelectedIndexChanged="ddlCompanyname_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="Customerlbl" runat="server" CssClass="labelbold" Text="Customer Code" />
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlCustCode" runat="server" CssClass="dropdown" Width="300px">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td>
                        </td>
                        <td align="center" colspan="0">
                            <asp:RadioButton ID="radioconfirm" runat="server" CssClass="labelbold" GroupName="orders"
                                Text="Confirm" />
                            &nbsp;&nbsp;
                            <asp:RadioButton ID="radioUnconfirm" runat="server" CssClass="labelbold" GroupName="orders"
                                Text="UnConfirm" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:CheckBox ID="chkdate" runat="server" AutoPostBack="true" CssClass="labelbold"
                                OnCheckedChanged="chkdate_CheckedChanged" Text="Check for Date" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                        </td>
                    </tr>
                    <tr id="TRDate" runat="server" visible="false">
                        <td>
                        </td>
                        <td>
                            <table>
                                <tr>
                                    <td>
                                        <asp:Label ID="Fromdate" runat="server" CssClass="labelbold" Text="From Date" />
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtfrmdate" runat="server" CssClass="textb" onchange="return checkDateOnTextBox();"
                                            Width="90px" />
                                        <asp:CalendarExtender ID="calenderdate" runat="server" Format="dd-MMM-yyyy" TargetControlID="txtfrmdate">
                                        </asp:CalendarExtender>
                                    </td>
                                    <td>
                                        <asp:Label ID="todate" runat="server" CssClass="labelbold" Text="To Date" />
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txttodate" runat="server" CssClass="textb" Width="90px" />
                                        <asp:CalendarExtender ID="todatecal" runat="server" Format="dd-MMM-yyyy" TargetControlID="txttodate">
                                        </asp:CalendarExtender>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td align="right" colspan="2">
                            <asp:Button ID="btnprint" runat="server" CssClass="buttonnorm" OnClick="btnprint_Click"
                                Text="Print" />
                        </td>
                    </tr>
                </table>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <%--   </form>--%>
    <%--</body>
</html>--%>
    s
</asp:Content>
