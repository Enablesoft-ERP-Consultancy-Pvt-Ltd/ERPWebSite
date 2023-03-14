<%@ Page Title="Bazaar Report Lot TagNo Wise" Language="C#" MasterPageFile="~/ERPmaster.master"
    AutoEventWireup="true" CodeFile="FrmBazaarReportLotTagNoWise.aspx.cs" Inherits="Masters_ReportForms_FrmBazaarReportLotTagNoWise" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content2" ContentPlaceHolderID="CPH_Form" runat="Server">
    <script type="text/javascript">
        function CloseForm() {
            window.location.href = "../../main.aspx";
        }
        function ClickNew() {
            window.location.href = "FrmBazaarReportLotTagNoWise.aspx";
        }
       
    </script>
    <asp:UpdatePanel ID="upd1" runat="server">
        <ContentTemplate>
            <table border="1" cellpadding="10" cellspacing="0" style="width: 100%">
                <%-- <tr>
                    <td colspan="2">
                        <asp:CheckBox ID="chkstockqtyzero" Text="Having Stock Qty. Zero" runat="server" CssClass="checkboxbold"
                            Font-Size="Small" />
                    </td>
                </tr>--%>
                <tr>
                    <td style="width: 5%; padding: 2px 2px 2px 2px">
                        <asp:Label Text="Lot No." CssClass="labelbold" runat="server" />
                    </td>
                    <td style="width: 13%; padding: 2px 2px 2px 2px">
                        <asp:TextBox ID="txtlotno" CssClass="textb" runat="server" Width="90%" placeholder="Type here to search" />
                    </td>
                    <td style="width: 5%; padding: 2px 2px 2px 2px">
                        <asp:Label ID="Label1" Text="Tag No." CssClass="labelbold" runat="server" />
                    </td>
                    <td style="width: 13%; padding: 2px 2px 2px 2px">
                        <asp:TextBox ID="txttagno" CssClass="textb" runat="server" Width="90%" placeholder="Type here to search" />
                    </td>
                    <td style="width: 5%; padding: 2px 2px 2px 2px">
                        <asp:Label ID="Label3" Text="Shade Color" CssClass="labelbold" runat="server" />
                    </td>
                    <td style="width: 15%; padding: 2px 2px 2px 2px">
                        <asp:DropDownList ID="DDshadeno" CssClass="dropdown" runat="server">
                        </asp:DropDownList>
                    </td>
                    <td style="width: 5%; padding: 2px 2px 2px 2px">
                        <asp:Label ID="Label2" Text="Company Name" CssClass="labelbold" runat="server" />
                    </td>
                    <td style="width: 15%; padding: 2px 2px 2px 2px">
                        <asp:DropDownList ID="DDcompanyName" CssClass="dropdown" Width="95%" runat="server">
                        </asp:DropDownList>
                    </td>
                </tr>
            </table>
            <table width="100%" border="1" cellspacing="0">
                <tr>
                    <td style="width: 5%; padding: 2px 2px 2px 2px">
                        <asp:Label ID="Label5" Text="From Date" CssClass="labelbold" runat="server" />
                    </td>
                    <td style="width: 10%;">
                        <asp:TextBox CssClass="textb" ID="TxtFromDate" runat="server" Width="100px"></asp:TextBox>
                        <asp:CalendarExtender ID="CalendarExtender1" runat="server" Format="dd-MMM-yyyy"
                            TargetControlID="TxtFromDate">
                        </asp:CalendarExtender>
                    </td>
                    <td style="width: 5%; padding: 2px 2px 2px 2px">
                        <asp:Label ID="Label4" Text="To Date" CssClass="labelbold" runat="server" />
                    </td>
                    <td style="width: 10%;">
                        <asp:TextBox CssClass="textb" ID="TxtToDate" runat="server" Width="100px"></asp:TextBox>
                        <asp:CalendarExtender ID="CalendarExtender2" runat="server" Format="dd-MMM-yyyy"
                            TargetControlID="TxtToDate">
                        </asp:CalendarExtender>
                    </td>
                    <td style="width: 60%; text-align: center">
                        <asp:Button Text="Preview" CssClass="buttonnorm" ID="btnPreview" runat="server" OnClick="btnPreview_Click" />
                        <asp:Button Text="Clear Search" ID="btnnewsearch" CssClass="buttonnorm" runat="server"
                            OnClientClick="return ClickNew();" />
                    </td>
                </tr>
                <tr>
                    <td colspan="7">
                        <asp:Label ID="lblmsg" CssClass="labelbold" ForeColor="Red" Text="" runat="server" />
                    </td>
                </tr>
            </table>
            <%-- <table style="width: 80%">
                <tr>
                    <td style="width: 100%">
                        <div style="width: 100%; overflow: auto; max-height: 500px">
                            <asp:GridView ID="DgDetail" runat="server" CssClass="grid-views">
                                <HeaderStyle CssClass="gvheaders" />
                                <AlternatingRowStyle CssClass="gvalts" />
                                <RowStyle CssClass="gvrow" />
                                <EmptyDataRowStyle CssClass="gvemptytext" />
                                <Columns>
                                </Columns>
                            </asp:GridView>
                        </div>
                    </td>
                </tr>
            </table>--%>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="btnPreview" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
