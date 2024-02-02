<%@ Page Title="Partner Employee Vendor Master" Language="C#" MasterPageFile="~/ERPmaster.master"
    AutoEventWireup="true" CodeFile="FrmBankAccountMaster.aspx.cs" Inherits="Masters_ZDevi_FrmBankAccountMaster" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content2" ContentPlaceHolderID="CPH_Form" runat="Server">
    <script src="../../Scripts/JScript.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery-1.4.1.js" type="text/javascript"></script>
    <script type="text/javascript">
        function CloseForm() {
            window.location.href = "../../main.aspx";
        }
        function New() {
            window.location.href = "FrmBankAccountMaster.aspx";
        }
    </script>
    <asp:UpdatePanel ID="Upd1" runat="server">
        <ContentTemplate>
            <div>
                <asp:Panel ID="pnl2" runat="server">
                    <table>
                        <tr>
                            <td>
                                <asp:Label ID="lblType" runat="server" Text="Owner Partner Vendor Name" Width="100%" CssClass="labelbold"></asp:Label>
                                <br />
                                <asp:DropDownList ID="ddOwnerPartnerVendorName" runat="server" Width="250px" CssClass="dropdown" AutoPostBack="true"
                                    OnSelectedIndexChanged="ddOwnerPartnerVendorName_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                            <td class="tdstyle">
                                <asp:Label ID="lblBankName" runat="server" Text="Bank Name"
                                    Width="100%" CssClass="labelbold"></asp:Label>
                                <br />
                                <asp:TextBox ID="txtBankName" CssClass="textb" runat="server" Width="250px" />
                            </td>
                            <td class="tdstyle">
                                <asp:Label ID="lblAddress" runat="server" Text="Address" CssClass="labelbold"></asp:Label>
                                <br />
                                <asp:TextBox ID="TxtAddress" CssClass="textb" runat="server" Width="250px" />
                            </td>
                            <td class="tdstyle">
                                <asp:Label ID="Label1" runat="server" Text="Account No" CssClass="labelbold"></asp:Label>
                                <br />
                                <asp:TextBox ID="TxtAccountNo" CssClass="textb" runat="server" Width="250px" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdstyle">
                                <asp:Label ID="Label2" runat="server" Text="IFSC Code" CssClass="labelbold"></asp:Label>
                                <br />
                                <asp:TextBox ID="TxtIFSCCode" CssClass="textb" runat="server" Width="250px" />
                            </td>
                            <td class="tdstyle">
                                <asp:Label ID="Label3" runat="server" Text="MICR Code" CssClass="labelbold"></asp:Label>
                                <br />
                                <asp:TextBox ID="txtMicrCode" CssClass="textb" runat="server" Width="250px" />
                            </td>
                            <td class="tdstyle">
                                <asp:Label ID="Label4" runat="server" Text="Nick Name" CssClass="labelbold"></asp:Label>
                                <br />
                                <asp:TextBox ID="TxtNickName" CssClass="textb" runat="server" Width="250px" />
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
                                                    <asp:Label ID="lblID" runat="server" Text='<%#Bind("ID") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="bank Name">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblBankName" runat="server" Text='<%#Bind("BankName") %>'></asp:Label>
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
                                            <asp:TemplateField HeaderText="Account No">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblAccountNo" runat="server" Text='<%#Bind("AccountNo") %>'></asp:Label>
                                                </ItemTemplate>
                                                <HeaderStyle Width="100px" />
                                                <ItemStyle Width="100px" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="IFSC Code">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblIFSCCode" runat="server" Text='<%#Bind("IFSCCode") %>'></asp:Label>
                                                </ItemTemplate>
                                                <HeaderStyle Width="100px" />
                                                <ItemStyle Width="100px" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Nick Name">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblNickName" runat="server" Text='<%#Bind("NickName") %>'></asp:Label>
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
