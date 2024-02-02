<%@ Page Title="Partner Employee Vendor Master" Language="C#" MasterPageFile="~/ERPmaster.master"
    AutoEventWireup="true" CodeFile="FrmPartnerEmployeeVendorMaster.aspx.cs" Inherits="Masters_ReportForms_FrmPartnerEmployeeVendorMaster" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content2" ContentPlaceHolderID="CPH_Form" runat="Server">
    <script src="../../Scripts/JScript.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery-1.4.1.js" type="text/javascript"></script>
    <script type="text/javascript">
        function CloseForm() {
            window.location.href = "../../main.aspx";
        }
        function New() {
            window.location.href = "FrmPartnerEmployeeVendorMaster.aspx";
        }
    </script>
    <asp:UpdatePanel ID="Upd1" runat="server">
        <ContentTemplate>
            <div>
                <asp:Panel ID="pnl2" runat="server">
                    <table>
                        <tr>
                            <td>
                                <asp:Label ID="lblType" runat="server" Text="Type" Width="100%" CssClass="labelbold"></asp:Label>
                                <br />
                                <asp:DropDownList ID="ddType" runat="server" Width="200px" CssClass="dropdown" AutoPostBack="true"
                                    OnSelectedIndexChanged="ddType_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                            <td class="tdstyle">
                                <asp:Label ID="lblOwnerPartnerVendor" runat="server" Text="Owner Partner Vendor"
                                    Width="100%" CssClass="labelbold"></asp:Label>
                                <br />
                                <asp:TextBox ID="txtOwnerPartnerVendor" CssClass="textb" runat="server" Width="250px" />
                            </td>
                            <td class="tdstyle">
                                <asp:Label ID="lblAddress" runat="server" Text="Address" CssClass="labelbold"></asp:Label>
                                <br />
                                <asp:TextBox ID="TxtAddress" CssClass="textb" runat="server" Width="250px" />
                            </td>
                            <td class="tdstyle">
                                <asp:Label ID="Label1" runat="server" Text="Phone No" CssClass="labelbold"></asp:Label>
                                <br />
                                <asp:TextBox ID="TxtPhoneNo" CssClass="textb" runat="server" Width="250px" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdstyle" colspan="2">
                                <asp:Label ID="Label2" runat="server" Text="Mail ID" CssClass="labelbold"></asp:Label>
                                <br />
                                <asp:TextBox ID="TxtMailID" CssClass="textb" runat="server" Width="250px" />
                            </td>
                        </tr>
                        <tr>
                            <td align="right" colspan="5">
                                <asp:Label ID="lblmsg" Text="" runat="server" CssClass="labelbold" ForeColor="Red" />
                                <asp:Button ID="btnNew" runat="server" Text="New" OnClientClick="return New();" CssClass="buttonnorm" />
                                <asp:Button ID="btnsave" runat="server" Text="Save" OnClientClick="return SaveData();"
                                    OnClick="btnsave_Click" CssClass="buttonnorm" />
                                <asp:Button ID="btnClose" runat="server" Text="Close" OnClientClick="return CloseForm();"
                                    CssClass="buttonnorm" />
                                <asp:Button ID="BtnPreview" runat="server" Text="Preview" Visible="False" CssClass="buttonnorm preview_width"
                                    OnClick="BtnPreview_Click" />
                            </td>
                        </tr>
                    </table>
                    <table>
                        <tr>
                            <td>
                                <div style="width: 100%; max-height: 300px; overflow: auto">
                                    <asp:GridView ID="DGPPDetail" runat="server" Width="100%" Style="margin-left: 10px"
                                        ForeColor="#333333" AutoGenerateColumns="False" CssClass="grid-view">
                                        <HeaderStyle CssClass="gvheader" Height="20px" />
                                        <AlternatingRowStyle CssClass="gvalt" />
                                        <RowStyle CssClass="gvrow" HorizontalAlign="Center" Height="20px" />
                                        <PagerStyle CssClass="PagerStyle" />
                                        <EmptyDataRowStyle CssClass="gvemptytext" />
                                        <Columns>
                                            <asp:TemplateField Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblEmployeeID" runat="server" Text='<%#Bind("EmployeeID") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Employee Name">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblEmployeeName" runat="server" Text='<%#Bind("EmployeeName") %>'></asp:Label>
                                                </ItemTemplate>
                                                <HeaderStyle Width="200px" />
                                                <ItemStyle Width="200px" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Address">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblAddress" runat="server" Text='<%#Bind("Address") %>'></asp:Label>
                                                </ItemTemplate>
                                                <HeaderStyle Width="250px" />
                                                <ItemStyle Width="250px" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Phone No">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblPhoneNo" runat="server" Text='<%#Bind("PhoneNo") %>'></asp:Label>
                                                </ItemTemplate>
                                                <HeaderStyle Width="100px" />
                                                <ItemStyle Width="100px" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Mail ID">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblMailID" runat="server" Text='<%#Bind("EMailID") %>'></asp:Label>
                                                </ItemTemplate>
                                                <HeaderStyle Width="100px" />
                                                <ItemStyle Width="100px" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Status">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblStatus" runat="server" Text='<%#Bind("ActiveStatus") %>'></asp:Label>
                                                </ItemTemplate>
                                                <HeaderStyle Width="100px" />
                                                <ItemStyle Width="100px" />
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </div>
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
