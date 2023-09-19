<%@ Page Title="Indent Raw Issue Detail" Language="C#" MasterPageFile="~/ERPmaster.master"
    AutoEventWireup="true" CodeFile="FrmPNMRawReceiveDetail.aspx.cs" Inherits="Masters_ReportForms_FrmPNMRawReceiveDetail" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content2" ContentPlaceHolderID="CPH_Form" runat="Server">
    <script src="../../Scripts/JScript.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery-1.4.1.js" type="text/javascript"></script>
    <script type="text/javascript">
        function CloseForm() {
            window.location.href = "../../main.aspx";
        }
    </script>
    <asp:UpdatePanel ID="Upd1" runat="server">
        <ContentTemplate>
            <div style="margin: 2% 20% 0% 20%">
                <asp:Panel ID="pnl2" runat="server">
                    <table>
                        <tr>
                            <td>
                                <asp:Label ID="lblfromdt" Text="From Date" runat="server" CssClass="labelbold" />
                                <br />
                                <asp:TextBox ID="txtfromDate" CssClass="textb" runat="server" Width="100px" />
                                <asp:CalendarExtender ID="calfrom" runat="server" TargetControlID="txtfromDate" Format="dd-MMM-yyyy">
                                </asp:CalendarExtender>
                            </td>
                            <td>
                                <asp:Label ID="lbltodt" Text="To Date" runat="server" CssClass="labelbold" />
                                <br />
                                <asp:TextBox ID="txttodate" CssClass="textb" runat="server" Width="100px" />
                                <asp:CalendarExtender ID="Caltodate" runat="server" TargetControlID="txttodate" Format="dd-MMM-yyyy">
                                </asp:CalendarExtender>
                            </td>
                            <td>
                                <br />
                                <asp:Button ID="BtnShow" Text="Show Data" runat="server" CssClass="buttonnorm" OnClick="BtnShow_Click" />
                            </td>
                        </tr>
                    </table>
                    <table>
                        <tr>
                            <td>
                                <div style="width: 100%; max-height: 300px; overflow: auto">
                                    <asp:GridView ID="DGIndentRawIssueDetail" runat="server" Width="100%" Style="margin-left: 10px"
                                        ForeColor="#333333" AutoGenerateColumns="False" CssClass="grid-view">
                                        <HeaderStyle CssClass="gvheader" Height="20px" />
                                        <AlternatingRowStyle CssClass="gvalt" />
                                        <RowStyle CssClass="gvrow" HorizontalAlign="Center" Height="20px" />
                                        <PagerStyle CssClass="PagerStyle" />
                                        <EmptyDataRowStyle CssClass="gvemptytext" />
                                        <Columns>
                                            <asp:TemplateField Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblPRMID" runat="server" Text='<%#Bind("PRMID") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Challan No">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblChallanNo" runat="server" Text='<%#Bind("ChallanNo") %>'></asp:Label>
                                                </ItemTemplate>
                                                <HeaderStyle Width="100px" />
                                                <ItemStyle Width="100px" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Challan Date">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblChallanDate" runat="server" Text='<%#Bind("ChallanDate") %>'></asp:Label>
                                                </ItemTemplate>
                                                <HeaderStyle Width="100px" />
                                                <ItemStyle Width="100px" />
                                            </asp:TemplateField>
                                            <asp:TemplateField>
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="LinkForOpenPRMDetail" runat="server" CommandName="" Text="Receive Detail"
                                                        OnClick="lnkbtnToOpenPRMDetail_Click" Font-Size="12px">
                                                    </asp:LinkButton>
                                                </ItemTemplate>
                                                <HeaderStyle Width="100px" />
                                                <ItemStyle Width="100px" />
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2" align="right">
                                <asp:Label ID="lblmsg" Text="" runat="server" CssClass="labelbold" ForeColor="Red" />
                                <asp:Button ID="btnclose" Text="Close" runat="server" CssClass="buttonnorm" OnClientClick="CloseForm();" />
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
