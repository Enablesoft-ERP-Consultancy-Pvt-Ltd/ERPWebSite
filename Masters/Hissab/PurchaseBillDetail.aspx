<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PurchaseBillDetail.aspx.cs"
    MasterPageFile="~/ERPmaster.master" Inherits="Masters_Hissab_PurchaseBillDetail"
    EnableEventValidation="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="page" ContentPlaceHolderID="CPH_Form" runat="server">
    <br />
    <script src="../../Scripts/JScript.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery-1.4.1.js" type="text/javascript"></script>
    <%--<script type="text/javascript" src="../../Scripts/Fixfocus.js"></script>--%>
    <script type="text/javascript">
        function CloseForm() {
            window.location.href = "../../main.aspx"
        }
        function Preview() {
            window.open("../../ReportViewer.aspx", "PurchaseReceive");
        } 
    </script>
    <asp:UpdatePanel ID="up1" runat="server">
        <ContentTemplate>
            <div>
                <table>
                    <tr>
                        <td class="tdstyle">
                            CompanyName<asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server"
                                ControlToValidate="DDCompanyName" ErrorMessage="please fill Company......." ForeColor="Red"
                                SetFocusOnError="true" ValidationGroup="f1">*</asp:RequiredFieldValidator>
                            <br />
                            <asp:DropDownList CssClass="dropdown" ID="DDCompanyName" runat="server" TabIndex="2">
                            </asp:DropDownList>
                            <cc1:ListSearchExtender ID="ListSearchExtender2" runat="server" TargetControlID="DDCompanyName"
                                ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                            </cc1:ListSearchExtender>
                        </td>
                        <td class="tdstyle">
                            Bills
                            <br />
                            <asp:DropDownList CssClass="dropdown" ID="DDbillName" runat="server" Width="200px"
                                TabIndex="2" OnSelectedIndexChanged="DDbillName_SelectedIndexChanged" AutoPostBack="True">
                                <asp:ListItem Value="0">--Select--</asp:ListItem>
                                <asp:ListItem Value="1">Pending</asp:ListItem>
                                <asp:ListItem Value="2">Compleded</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>
                </table>
                <table>
                    <tr>
                        <td>
                            <asp:GridView ID="DGbillnoDetail" Width="100%" runat="server" AutoGenerateColumns="False"
                                OnRowCommand="DGbillnoDetail_RowCommand" CssClass="grid-view" OnRowCreated="DGbillnoDetail_RowCreated">
                                <AlternatingRowStyle BackColor="White" />
                                <Columns>
                                    <asp:TemplateField HeaderText="">
                                        <ItemTemplate>
                                            <asp:CheckBox ID="Chkbox" runat="server" />
                                        </ItemTemplate>
                                        <HeaderStyle Width="80px" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Bill NO" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblbillno" runat="server" Visible="true" Text='<%# Bind("Billno") %>' />
                                            <asp:Label ID="lblphissabidid" runat="server" Visible="false" Text='<%# Bind("PhissabId") %>' />
                                            <asp:Label ID="lblpartyid" runat="server" Visible="false" Text='<%# Bind("partyid") %>' />
                                        </ItemTemplate>
                                        <HeaderStyle Width="80px" />
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="billno" HeaderText="Bill NO" />
                                    <asp:BoundField DataField="date" HeaderText="Date" />
                                    <asp:BoundField DataField="party" HeaderText="Party Name" />
                                    <asp:BoundField DataField="Amount" HeaderText="Total Amount" />
                                    <asp:TemplateField HeaderText="Balance Amount">
                                        <ItemTemplate>
                                            <asp:Label ID="lblbalnce" runat="server" Text='<%# getgiven(DataBinder.Eval(Container.DataItem, "phissabid").ToString(),DataBinder.Eval(Container.DataItem, "Amount").ToString()) %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Amount Pay">
                                        <ItemTemplate>
                                            <asp:TextBox ID="TXTamt" runat="server" Width="70px" Text='<%# getamt().ToString() %>'></asp:TextBox>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Detail">
                                        <ItemTemplate>
                                            <asp:Button ID="BTNdetail" CssClass="buttonnorm" runat="server" Text="Detail" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>"
                                                OnClick="BTNdetail_Click" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                                <EmptyDataRowStyle CssClass="gvemptytext" />
                            </asp:GridView>
                        </td>
                        <td align="center">
                            <asp:GridView ID="DGchallanDETAIL" Width="100%" runat="server" DataKeyNames="challanno"
                                AutoGenerateColumns="False" CssClass="grid-view" OnRowCreated="DGchallanDETAIL_RowCreated">
                                <AlternatingRowStyle BackColor="White" />
                                <Columns>
                                    <asp:BoundField DataField="challanno" HeaderText="Challan NO" />
                                    <asp:BoundField DataField="Amt" HeaderText=" Amount" />
                                    <asp:BoundField DataField="date" HeaderText="Date" />
                                </Columns>
                                <EmptyDataRowStyle CssClass="gvemptytext" />
                            </asp:GridView>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td class="tdstyle">
                            Date
                            <asp:TextBox CssClass="textb" ID="TxtDate" runat="server" Width="100px"></asp:TextBox>
                            <asp:CalendarExtender ID="CalendarExtender1" runat="server" Format="dd-MMM-yyyy"
                                TargetControlID="TxtDate">
                            </asp:CalendarExtender>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="LblErrorMessage" runat="server" Font-Bold="true" ForeColor="RED" Text=""
                                Visible="false"></asp:Label>
                        </td>
                        <td align="right">
                            <asp:Button CssClass="buttonnorm" ID="BtnSave" Text="Approved" runat="server" OnClick="BtnSave_Click"
                                OnClientClick="return confirm('Do you want to save data?')" ValidationGroup="f1"
                                TabIndex="45" />
                            <asp:Button CssClass="buttonnorm" ID="BtnClose" runat="server" Text="Close" OnClientClick="return CloseForm();" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:HiddenField ID="hnrow" runat="server" />
                            <asp:HiddenField ID="hnbill" runat="server" />
                            <asp:HiddenField ID="hnpartyid" runat="server" />
                            <asp:HiddenField ID="hnhissabid" runat="server" />
                            <asp:HiddenField ID="hnbal" runat="server" />
                            <asp:HiddenField ID="hnbutt" runat="server" />
                        </td>
                    </tr>
                </table>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
