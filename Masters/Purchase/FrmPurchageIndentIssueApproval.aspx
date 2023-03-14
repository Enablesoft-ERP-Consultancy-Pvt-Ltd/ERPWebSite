<%@ Page Title="Purchase Order Approval" Language="C#" AutoEventWireup="true" CodeFile="FrmPurchageIndentIssueApproval.aspx.cs"
    MasterPageFile="~/ERPmaster.master" Inherits="Masters_Purchase_FrmPurchageIndentIssueApproval"
    EnableEventValidation="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="CPH_Form" runat="server">
    <script src="../../Scripts/JScript.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery-1.4.1.min.js" type="text/javascript"></script>
    <%--<script type="text/javascript" src="../../Scripts/Fixfocus.js"></script>--%>
    <script type="text/javascript">
        function CloseForm() {
            window.location.href = "../../main.aspx";
        }
        function addpriview() {
            window.open("../../ReportViewer.aspx");
        }
        function validate() {
            if (document.getElementById("<%=DDPartyName.ClientID %>").value == "0") {
                alert("Pls Select Vendor Name");
                document.getElementById("<%=DDPartyName.ClientID %>").focus();
                return false;
            }
            else if (document.getElementById("<%=DDChallanNo.ClientID %>").value == "0") {
                alert("Pls Select Challan No");
                document.getElementById("<%=DDChallanNo.ClientID %>").focus();
                return false;
            }
            if (document.getElementById("<%=TxtDate.ClientID %>").value == "") {
                alert("Pls Select Date");
                document.getElementById("<%=TxtDate.ClientID %>").focus();
                return false;
            }
            else {
                return confirm('Do You Want To Save?')
            }
        }
        
    </script>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div>
                <table>
                    <tr>
                        <td class="tdstyle">
                            VendorName
                            <br />
                            <asp:DropDownList ID="DDPartyName" CssClass="dropdown" runat="server" Width="250px"
                                OnSelectedIndexChanged="DDPartyName_SelectedIndexChanged" AutoPostBack="true">
                            </asp:DropDownList>
                            <cc1:ListSearchExtender ID="ListSearchExtender4" runat="server" TargetControlID="DDPartyName"
                                ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                            </cc1:ListSearchExtender>
                        </td>
                        <td class="tdstyle">
                            P.O No
                            <br />
                            <asp:DropDownList CssClass="dropdown" ID="DDChallanNo" runat="server" AutoPostBack="True"
                                Width="150px" TabIndex="4" OnSelectedIndexChanged="DDChallanNo_SelectedIndexChanged">
                            </asp:DropDownList>
                            <cc1:ListSearchExtender ID="ListSearchExtender1" runat="server" TargetControlID="DDChallanNo"
                                ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                            </cc1:ListSearchExtender>
                        </td>
                        <td class="tdstyle">
                            Date
                            <br />
                            <asp:TextBox CssClass="textb" ID="TxtDate" Width="100px" runat="server"></asp:TextBox>
                            <asp:CalendarExtender ID="CalendarExtender1" runat="server" Format="dd-MMM-yyyy"
                                TargetControlID="TxtDate">
                            </asp:CalendarExtender>
                        </td>
                        <td class="tdstyle">
                            Remarks
                            <br />
                            <asp:TextBox CssClass="textb" ID="txtRemarks" Width="250px" runat="server"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="5" style="text-align: right">
                            <asp:Button ID="btnSave" Text="Save" runat="server" CssClass="buttonnorm" Width="53px"
                                OnClientClick="return validate();" OnClick="btnSave_Click" />
                            &nbsp;<asp:Button ID="btnPreview" Text="Preview" runat="server" CssClass="buttonnorm"
                                Width="57px" OnClick="btnPreview_Click" />
                            &nbsp;<asp:Button ID="btnClose" Text="Close" runat="server" CssClass="buttonnorm"
                                Width="53px" OnClientClick="return CloseForm();" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4">
                            <asp:Label ID="lblMessage" runat="server" ForeColor="Red"></asp:Label>
                            <asp:GridView ID="DG" runat="server" DataKeyNames="Sr_No" OnRowDataBound="DG_RowDataBound"
                                AutoGenerateColumns="False" OnRowDeleting="DG_RowDeleting" CssClass="grid-view"
                                OnRowCreated="DG_RowCreated">
                                <Columns>
                                    <asp:BoundField DataField="ChallanNo" HeaderText="ChallanNo">
                                        <HeaderStyle HorizontalAlign="Left" />
                                        <ItemStyle HorizontalAlign="Left" Width="150px" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Date" HeaderText="Date">
                                        <HeaderStyle HorizontalAlign="Left" />
                                        <ItemStyle HorizontalAlign="Left" Width="100px" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Remarks" HeaderText="Remarks">
                                        <HeaderStyle HorizontalAlign="Left" />
                                        <ItemStyle HorizontalAlign="Left" Width="250px" />
                                    </asp:BoundField>
                                    <asp:TemplateField ShowHeader="False">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="LinkButton1" runat="server" CausesValidation="False" CommandName="Delete"
                                                Text="Del" OnClientClick="return confirm('Do You Want To Deleted?')"></asp:LinkButton>
                                        </ItemTemplate>
                                        <ItemStyle Width="30px" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderStyle-Width="50" ItemStyle-HorizontalAlign="Center" HeaderText="Preview">
                                        <ItemTemplate>
                                            <asp:CheckBox ID="Chk1" runat="server" OnCheckedChanged="Chk1_CheckedChanged" AutoPostBack="true" />
                                        </ItemTemplate>
                                        <HeaderStyle HorizontalAlign="Center" Width="50px" />
                                        <ItemStyle HorizontalAlign="Center" Width="50px" />
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </td>
                    </tr>
                </table>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
