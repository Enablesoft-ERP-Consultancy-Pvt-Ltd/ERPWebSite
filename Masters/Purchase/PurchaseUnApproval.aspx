<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PurchaseUnApproval.aspx.cs"
    MasterPageFile="~/ERPmaster.master" Inherits="Masters_Purchase_PurchaseUnApproval" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="form" ContentPlaceHolderID="CPH_Form" runat="server">
    <script src="../../Scripts/JScript.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery-1.4.1.js" type="text/javascript"></script>
    <%--<script type="text/javascript" src="../../Scripts/Fixfocus.js"></script>--%>
    <script type="text/javascript">
        function NewForm() {
            window.location.href = "PurchaseApproval.aspx";
        }
        function CloseForm() {
            window.location.href = "../../main.aspx";
        }
        function Priview() {
            window.open("../../ReportViewer.aspx", "GenrateIndentReport");
        }
        function validation() {
            if (document.getElementById('CPH_Form_DDCompanyName').value <= "0") {
                alert("Please Select Company Name....!");
                document.getElementById("CPH_Form_DDCompanyName").focus();
                return false;
            }
            if (document.getElementById('CPH_Form_DDDepartment').value <= "0") {
                alert("Please Select Department Name....!");
                document.getElementById("CPH_Form_DDDepartment").focus();
                return false;
            }
            if (document.getElementById('CPH_Form_TxtApprovalDate').value == "") {
                alert("Please Select Approval Date....!");
                document.getElementById("CPH_Form_TxtApprovalDate").focus();
                return false;
            }
            else {
                var checked = false;
                var chkBoxList = document.getElementById("CPH_Form_CHkPindentNo");
                var count = chkBoxList.getElementsByTagName("input");
                for (var i = 0; i < count.length; i++) {
                    if (count[i].checked) {
                        checked = true;
                        break;
                    }
                }
                if (checked == false) {
                    alert("Please select atleast one IndentNo.......");
                    return false;
                }
                return confirm('Do You Want To Save?');
            }
        }
    </script>
    <div id="maindiv" runat="server" style="min-height: 500px; overflow: scroll">
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <table>
                    <tr>
                        <td class="tdstyle">
                            <span class="labelbold">PIndentNo</span>
                            <br />
                            <asp:TextBox ID="TxtPindentNo" runat="server" Width="100px" CssClass="textb" OnTextChanged="TxtPindentNo_TextChanged"
                                AutoPostBack="true" ToolTip="Enter Purchase IndentNo.."></asp:TextBox>
                        </td>
                        <td class="tdstyle">
                            <span class="labelbold">CompanyName</span>
                            <br />
                            <asp:DropDownList CssClass="dropdown" ID="DDCompanyName" runat="server" Width="150px">
                            </asp:DropDownList>
                            <cc1:ListSearchExtender ID="ListSearchExtender4" runat="server" TargetControlID="DDCompanyName"
                                ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                            </cc1:ListSearchExtender>
                        </td>
                        <td class="tdstyle">
                            <span class="labelbold">Department</span>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="DDDepartment"
                                ErrorMessage="please fill Department......." ForeColor="Red" SetFocusOnError="true"
                                ValidationGroup="f1">*</asp:RequiredFieldValidator>
                            <br />
                            <asp:DropDownList CssClass="dropdown" ID="DDDepartment" runat="server" OnSelectedIndexChanged="DDDepartment_SelectedIndexChanged"
                                AutoPostBack="True" TabIndex="1" Width="150px">
                            </asp:DropDownList>
                            <cc1:ListSearchExtender ID="ListSearchExtender1" runat="server" TargetControlID="DDDepartment"
                                ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                            </cc1:ListSearchExtender>
                        </td>
                        <td class="tdstyle">
                            <span class="labelbold">Vendor Name</span>
                            <br />
                            <asp:DropDownList CssClass="dropdown" Width="170px" ID="DDvendorName" runat="server"
                                TabIndex="2" OnSelectedIndexChanged="DDvendorName_SelectedIndexChanged" AutoPostBack="true">
                            </asp:DropDownList>
                            <cc1:ListSearchExtender ID="ListSearchExtender2" runat="server" TargetControlID="DDvendorName"
                                ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                            </cc1:ListSearchExtender>
                        </td>
                        <td class="tdstyle">
                            <span class="labelbold">Employee Name</span>
                            <br />
                            <asp:DropDownList CssClass="dropdown" Width="170px" ID="DDpartyName" runat="server"
                                TabIndex="2" OnSelectedIndexChanged="DDpartyName_SelectedIndexChanged" AutoPostBack="true">
                            </asp:DropDownList>
                            <cc1:ListSearchExtender ID="ListSearchExtender3" runat="server" TargetControlID="DDpartyName"
                                ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                            </cc1:ListSearchExtender>
                        </td>
                    </tr>
                    <tr>
                        <td align="right" colspan="6">
                            <asp:Button CssClass="buttonnorm" ID="BtnClose" runat="server" Text="Close" TabIndex="6"
                                OnClick="BtnClose_Click" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4">
                            <asp:Label ID="LblErr" ForeColor="Red" runat="server" Text=""></asp:Label>
                        </td>
                    </tr>
                </table>
                <table width="100%">
                    <tr>
                        <td colspan="5" align="center">
                            <asp:GridView ID="DGPIndentDetail" AutoGenerateColumns="False" AllowPaging="True"
                                OnPageIndexChanging="DGPIndentDetail_PageIndexChanging" CellPadding="4" runat="server"
                                DataKeyNames="PIndentId" CssClass="grid-views" OnRowDeleting="DGPIndentDetail_RowDeleting">
                                <HeaderStyle CssClass="gvheaders" />
                                <AlternatingRowStyle CssClass="gvalts" />
                                <RowStyle CssClass="gvrow" />
                                <PagerStyle CssClass="PagerStyle" />
                                <EmptyDataRowStyle CssClass="gvemptytext" />
                                <Columns>
                                    <asp:BoundField DataField="PIndentNo" HeaderText="PIndentNo">
                                        <HeaderStyle Width="250px" />
                                    </asp:BoundField>
                                    <asp:TemplateField ShowHeader="False">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="LinkButton1" runat="server" CausesValidation="False" CommandName="Delete"
                                                Text="UnApprove" OnClientClick="return confirm('Do You Want to UnApprove this IndentNo');"
                                                Style="color: Purple; font-weight: bold" ToolTip="Click to unApprove"></asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </td>
                    </tr>
                </table>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</asp:Content>
