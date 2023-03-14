<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PurchaseApproval.aspx.cs"
    MasterPageFile="~/ERPmaster.master" Inherits="Masters_Purchase_PurchaseApproval" %>

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
                            <span class="labelbold">CompanyName</span>
                            <br />
                            <asp:DropDownList CssClass="dropdown" ID="DDCompanyName" runat="server" OnSelectedIndexChanged="DDCompanyName_SelectedIndexChanged"
                                AutoPostBack="true">
                            </asp:DropDownList>
                            <cc1:ListSearchExtender ID="ListSearchExtender4" runat="server" TargetControlID="DDCompanyName"
                                ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                            </cc1:ListSearchExtender>
                        </td>
                        <td class="tdstyle">
                            <span class="labelbold">Process</span>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="DDDepartment"
                                ErrorMessage="please fill Department......." ForeColor="Red" SetFocusOnError="true"
                                ValidationGroup="f1">*</asp:RequiredFieldValidator>
                            <br />
                            <asp:DropDownList CssClass="dropdown" ID="DDDepartment" runat="server" OnSelectedIndexChanged="DDDepartment_SelectedIndexChanged"
                                AutoPostBack="True" TabIndex="1">
                            </asp:DropDownList>
                            <cc1:ListSearchExtender ID="ListSearchExtender1" runat="server" TargetControlID="DDDepartment"
                                ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                            </cc1:ListSearchExtender>
                        </td>
                        <td runat="server" visible="false" class="tdstyle">
                            <span class="labelbold">Party Name</span>
                            <br />
                            <asp:DropDownList CssClass="dropdown" Width="100px" ID="DDPartyName" runat="server"
                                TabIndex="2">
                            </asp:DropDownList>
                            <cc1:ListSearchExtender ID="ListSearchExtender2" runat="server" TargetControlID="DDPartyName"
                                ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                            </cc1:ListSearchExtender>
                        </td>
                        <td class="tdstyle">
                            <span class="labelbold">Approval Date</span>
                            <br />
                            <asp:TextBox ID="TxtApprovalDate" runat="server" CssClass="textb" Width="100px"></asp:TextBox>
                            <cc1:CalendarExtender ID="CalendarExtender1" runat="server" Format="dd-MMM-yyyy"
                                TargetControlID="TxtApprovalDate">
                            </cc1:CalendarExtender>
                        </td>
                        <td class="tdstyle">
                            <span class="labelbold">ApprovedBy</span>
                            <br />
                            <asp:TextBox ID="TxtApprovedBy" runat="server" CssClass="textb" Width="250px" onkeydown="return (event.keyCode!=13);"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td align="right" colspan="6">
                            <asp:Button CssClass="buttonnorm" ID="BtnNew" runat="server" Text="New" OnClientClick="return NewForm();"
                                TabIndex="7" Width="70px" />
                            <asp:Button CssClass="buttonnorm" ID="BtnSave" runat="server" Text="Save" OnClientClick="return validation();"
                                OnClick="BtnSave_Click" TabIndex="4" Width="70px" />
                            <asp:Button CssClass="buttonnorm preview_width" ID="BtnPreview" runat="server" Text="Preview"
                                Enabled="false" OnClick="BtnPreview_Click" TabIndex="5" Width="70px" />
                            <asp:Button CssClass="buttonnorm" ID="BtnClose" runat="server" Text="Close" OnClick="BtnClose_Click"
                                TabIndex="6" Width="70px" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4">
                            <asp:Label ID="LblErr" ForeColor="Red" runat="server" Text=""></asp:Label>
                        </td>
                    </tr>
                </table>
                <table>
                    <tr>
                        <td style="padding-top: 10px">
                            <div id="d23" runat="server" style="padding-top: 10px">
                                <asp:CheckBoxList ID="CHkPindentNo" runat="server" CssClass="dropdown" OnSelectedIndexChanged="CHkPindentNo_SelectedIndexChanged"
                                    AutoPostBack="True" TabIndex="3">
                                </asp:CheckBoxList>
                            </div>
                        </td>
                        <td colspan="5">
                            <asp:GridView ID="DGPIndentDetail" AutoGenerateColumns="false" AllowPaging="True"
                                OnPageIndexChanging="DGPIndentDetail_PageIndexChanging" CellPadding="4" PageSize="10"
                                runat="server" DataKeyNames="PIndentDetailId" CssClass="grid-views">
                                <HeaderStyle CssClass="gvheaders" />
                                <AlternatingRowStyle CssClass="gvalts" />
                                <RowStyle CssClass="gvrow" />
                                <PagerStyle CssClass="PagerStyle" />
                                <EmptyDataRowStyle CssClass="gvemptytext" />
                                <Columns>
                                    <asp:BoundField DataField="PIndentDetailId" HeaderText="PIndentDetailId" Visible="false" />
                                    <asp:BoundField DataField="PIndentNo" HeaderText="PIndentNo" />
                                    <asp:BoundField DataField="DepartmentName" HeaderText="Department" />
                                    <asp:BoundField DataField="PartyName" HeaderText="PartyName" />
                                    <asp:BoundField DataField="ItemDescription" HeaderText="Item Description" />
                                    <asp:TemplateField HeaderText="Qty">
                                        <ItemTemplate>
                                            <asp:TextBox ID="TxtQty" runat="server" Width="70px" Text='<%# Bind("Qty") %>' AutoPostBack="true"
                                                CssClass="textb"></asp:TextBox>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="UnitName" HeaderText="Unit" />
                                    <asp:BoundField DataField="iRemark" HeaderText="Remark" />
                                    <asp:BoundField DataField="Rate" HeaderText="Rate" />
                                </Columns>
                            </asp:GridView>
                        </td>
                    </tr>
                </table>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</asp:Content>
