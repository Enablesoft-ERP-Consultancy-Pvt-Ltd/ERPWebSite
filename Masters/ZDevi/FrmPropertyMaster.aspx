<%@ Page Title="Partner Employee Vendor Master" Language="C#" MasterPageFile="~/ERPmaster.master" EnableEventValidation="false"
    AutoEventWireup="true" CodeFile="FrmPropertyMaster.aspx.cs" Inherits="Masters_ReportForms_FrmPropertyMaster" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content2" ContentPlaceHolderID="CPH_Form" runat="Server">
    <script src="../../Scripts/JScript.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery-1.4.1.js" type="text/javascript"></script>
    <script type="text/javascript">
        function CloseForm() {
            window.location.href = "../../main.aspx";
        }
        function New() {
            window.location.href = "FrmPropertyMaster.aspx";
        }
    </script>
    <asp:UpdatePanel ID="Upd1" runat="server">
        <ContentTemplate>
            <div>
                <asp:Panel ID="pnl2" runat="server">
                    <table>
                        <tr>
                            <td class="tdstyle">
                                <asp:Label ID="Label14" runat="server" Text="Property Owner" CssClass="labelbold"></asp:Label>
                                <br />
                                <asp:DropDownList ID="ddPropertyOwner" runat="server" Width="200px" 
                                    CssClass="dropdown" AutoPostBack="true" 
                                    onselectedindexchanged="ddPropertyOwner_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                            <td class="tdstyle">
                                <asp:Label ID="lblPropertyName" runat="server" Text="Property Name" Width="100%"
                                    CssClass="labelbold"></asp:Label>
                                <br />
                                <asp:TextBox ID="txtPropertyName" CssClass="textb" runat="server" Width="250px" />
                            </td>
                            <td class="tdstyle">
                                <asp:Label ID="lblSize" runat="server" Text="Size" CssClass="labelbold"></asp:Label>
                                <br />
                                <asp:TextBox ID="TxtSize" CssClass="textb" runat="server" Width="100px" />
                            </td>
                            <td class="tdstyle">
                                <asp:Label ID="Label1" runat="server" Text="Rate" CssClass="labelbold"></asp:Label>
                                <br />
                                <asp:TextBox ID="TxtRate" CssClass="textb" runat="server" Width="100px" />
                            </td>
                            <td class="tdstyle">
                                <asp:Label ID="Label2" runat="server" Text="Cash Amount" CssClass="labelbold"></asp:Label>
                                <br />
                                <asp:TextBox ID="TxtCashAmount" CssClass="textb" runat="server" Width="100px" />
                            </td>
                            <td class="tdstyle">
                                <asp:Label ID="Label3" runat="server" Text="Cheque Amount" CssClass="labelbold"></asp:Label>
                                <br />
                                <asp:TextBox ID="txtChequeAmount" CssClass="textb" runat="server" Width="100px" />
                            </td>
                            <td class="tdstyle">
                                <asp:Label ID="Label4" runat="server" Text="Registration Date" CssClass="labelbold"></asp:Label>
                                <br />
                                <asp:TextBox ID="TxtRegistrationDate" CssClass="textb" runat="server" Width="100px" />
                                <asp:CalendarExtender ID="cal1" TargetControlID="TxtRegistrationDate" Format="dd-MMM-yyyy"
                                    runat="server">
                                </asp:CalendarExtender>
                            </td>
                        </tr>
                    </table>
                    <table>
                        <tr>
                            <td class="tdstyle">
                                <asp:Label ID="Label5" runat="server" Text="Stamp Duty" CssClass="labelbold"></asp:Label>
                                <br />
                                <asp:TextBox ID="TxtStampDuty" CssClass="textb" runat="server" Width="100px" />
                            </td>
                            <td class="tdstyle">
                                <asp:Label ID="Label6" runat="server" Text="Govt Fee" CssClass="labelbold"></asp:Label>
                                <br />
                                <asp:TextBox ID="TxtGovtFee" CssClass="textb" runat="server" Width="100px" />
                            </td>
                            <td class="tdstyle">
                                <asp:Label ID="Label7" runat="server" Text="Registry Expence" CssClass="labelbold"></asp:Label>
                                <br />
                                <asp:TextBox ID="TxtRegistryExpence" CssClass="textb" runat="server" Width="100px" />
                            </td>
                            <td class="tdstyle">
                                <asp:Label ID="Label8" runat="server" Text="Dealer Comm" CssClass="labelbold"></asp:Label>
                                <br />
                                <asp:TextBox ID="TxtDealerComm" CssClass="textb" runat="server" Width="100px" />
                            </td>
                            <td class="tdstyle">
                                <asp:Label ID="Label9" runat="server" Text="Other Charge" CssClass="labelbold"></asp:Label>
                                <br />
                                <asp:TextBox ID="TxtOtherCharge" CssClass="textb" runat="server" Width="100px" />
                            </td>
                            <td class="tdstyle">
                                <asp:Label ID="Label10" runat="server" Text="Loan Amount" CssClass="labelbold"></asp:Label>
                                <br />
                                <asp:TextBox ID="TxtLoanAmount" CssClass="textb" runat="server" Width="100px" />
                            </td>
                            <td class="tdstyle">
                                <asp:Label ID="Label11" runat="server" Text="Loan Date" CssClass="labelbold"></asp:Label>
                                <br />
                                <asp:TextBox ID="txtLoanDate" CssClass="textb" runat="server" Width="100px" />
                                <asp:CalendarExtender ID="CalendarExtender1" TargetControlID="txtLoanDate" Format="dd-MMM-yyyy"
                                    runat="server">
                                </asp:CalendarExtender>
                            </td>
                        </tr>
                    </table>
                    <table>
                        <tr>
                            <td class="tdstyle">
                                <asp:Label ID="Label12" runat="server" Text="Address" CssClass="labelbold"></asp:Label>
                                <br />
                                <asp:TextBox ID="TxtAddress" CssClass="textb" runat="server" Width="500px" />
                            </td>
                            <td class="tdstyle">
                                <asp:Label ID="Label13" runat="server" Text="Remark" CssClass="labelbold"></asp:Label>
                                <br />
                                <asp:TextBox ID="TxtRemark" CssClass="textb" runat="server" Width="500px" />
                            </td>
                        </tr>
                        <tr>
                            <td align="right" colspan="5">
                                <asp:Label ID="lblmsg" Text="" runat="server" CssClass="labelbold" ForeColor="Red" />
                                <asp:Button ID="btnNew" runat="server" Text="New" OnClientClick="return New();" CssClass="buttonnorm" />
                                <asp:Button ID="btnsave" runat="server" Text="Save" OnClientClick="return SaveData();"
                                    OnClick="btnsave_Click" CssClass="buttonnorm" />
                                <asp:Button ID="BtnPreview" runat="server" Text="Preview" Visible="False" CssClass="buttonnorm preview_width"
                                    OnClick="BtnPreview_Click" />
                                <asp:Button ID="btnClose" runat="server" Text="Close" OnClientClick="return CloseForm();"
                                    CssClass="buttonnorm" />
                            </td>
                        </tr>
                    </table>
                    <table>
                        <tr>
                            <td>
                                <div style="width: 100%; max-height: 300px; overflow: auto">
                                    <asp:GridView ID="DGPropertDetail" runat="server" Width="100%" Style="margin-left: 10px"
                                        ForeColor="#333333" AutoGenerateColumns="False" CssClass="grid-view" OnRowDataBound="DGPropertDetail_RowDataBound"
                                        DataKeyNames="PropertyID" OnSelectedIndexChanged="DGPropertDetail_SelectedIndexChanged">
                                        <HeaderStyle CssClass="gvheader" Height="20px" />
                                        <AlternatingRowStyle CssClass="gvalt" />
                                        <RowStyle CssClass="gvrow" HorizontalAlign="Center" Height="20px" />
                                        <PagerStyle CssClass="PagerStyle" />
                                        <EmptyDataRowStyle CssClass="gvemptytext" />
                                        <Columns>
                                            <asp:TemplateField Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblPropertyID" runat="server" Text='<%#Bind("PropertyID") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Property Name">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblPropertyName" runat="server" Text='<%#Bind("PropertyName") %>'></asp:Label>
                                                </ItemTemplate>
                                                <HeaderStyle Width="200px" />
                                                <ItemStyle Width="200px" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Size">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblSize" runat="server" Text='<%#Bind("Size") %>'></asp:Label>
                                                </ItemTemplate>
                                                <HeaderStyle Width="100px" />
                                                <ItemStyle Width="100px" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Rate">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblRate" runat="server" Text='<%#Bind("Rate") %>'></asp:Label>
                                                </ItemTemplate>
                                                <HeaderStyle Width="100px" />
                                                <ItemStyle Width="100px" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Cash Amount">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblCashAmount" runat="server" Text='<%#Bind("CashAmount") %>'></asp:Label>
                                                </ItemTemplate>
                                                <HeaderStyle Width="100px" />
                                                <ItemStyle Width="100px" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Chq Amount">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblChqAmount" runat="server" Text='<%#Bind("ChqAmount") %>'></asp:Label>
                                                </ItemTemplate>
                                                <HeaderStyle Width="100px" />
                                                <ItemStyle Width="100px" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Total Amount">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblTotalAmount" runat="server" Text='<%#Bind("TotalAmount") %>'></asp:Label>
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
