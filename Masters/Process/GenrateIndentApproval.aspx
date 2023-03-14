<%@ Page Title="" Language="C#" MasterPageFile="~/ERPmaster.master" AutoEventWireup="true"
    CodeFile="GenrateIndentApproval.aspx.cs" Inherits="Masters_Process_GenrateIndentApproval" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="form" ContentPlaceHolderID="CPH_Form" runat="Server">
    <script src="../../Scripts/JScript.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery-1.4.1.js" type="text/javascript"></script>
    <%--<script type="text/javascript" src="../../Scripts/Fixfocus.js"></script>--%>
    <link href="../../Styles/vijay.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript">
        function NewForm() {
            window.location.href = "GenrateIndentApproval.aspx";
        }
        function CloseForm() {
            window.location.href = "../../main.aspx";
        }
        function Priview() {
            window.open("../../ReportViewer.aspx");
        }
        function validation() {
            if (document.getElementById('CPH_Form_DDCompanyName').value <= "0") {
                alert("Please Select Company Name....!");
                document.getElementById("CPH_Form_DDCompanyName").focus();
                return false;
            }
            if (document.getElementById('CPH_Form_DDprocess').value <= "0") {
                alert("Please Select Department Name....!");
                document.getElementById("CPH_Form_DDprocess").focus();
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
                            <asp:Label ID="Label5" Text=" CompanyName" runat="server" CssClass="labelbold" />
                            <br />
                            <asp:DropDownList CssClass="dropdown" ID="DDCompanyName" runat="server">
                            </asp:DropDownList>
                            <cc1:ListSearchExtender ID="ListSearchExtender4" runat="server" TargetControlID="DDCompanyName"
                                ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                            </cc1:ListSearchExtender>
                        </td>
                        <td class="tdstyle">
                            <asp:Label ID="Label1" Text="  Process" runat="server" CssClass="labelbold" />
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="DDprocess"
                                ErrorMessage="please fill Department......." ForeColor="Red" SetFocusOnError="true"
                                ValidationGroup="f1">*</asp:RequiredFieldValidator>
                            <br />
                            <asp:DropDownList CssClass="dropdown" ID="DDprocess" runat="server" OnSelectedIndexChanged="DDprocess_SelectedIndexChanged"
                                AutoPostBack="True" TabIndex="1">
                            </asp:DropDownList>
                            <cc1:ListSearchExtender ID="ListSearchExtender1" runat="server" TargetControlID="DDprocess"
                                ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                            </cc1:ListSearchExtender>
                        </td>
                        <td id="Td1" class="tdstyle">
                            <asp:Label ID="Label2" Text="   Party Name" runat="server" CssClass="labelbold" />
                            <br />
                            <asp:DropDownList CssClass="dropdown" Width="100px" ID="DDPartyName" runat="server"
                                TabIndex="2" OnSelectedIndexChanged="DDPartyName_SelectedIndexChanged" AutoPostBack="True">
                            </asp:DropDownList>
                            <cc1:ListSearchExtender ID="ListSearchExtender2" runat="server" TargetControlID="DDPartyName"
                                ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                            </cc1:ListSearchExtender>
                        </td>
                        <td id="TDppno" runat="server" class="tdstyle">
                            <asp:Label ID="Label3" Text="    PPNo" runat="server" CssClass="labelbold" />
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator11" runat="server" ControlToValidate="DDProcessProgramNo"
                                ErrorMessage="please Select ProcessProgramNo" ForeColor="Red" SetFocusOnError="true"
                                ValidationGroup="f1">*</asp:RequiredFieldValidator>
                            <br />
                            <asp:DropDownList CssClass="dropdown" ID="DDProcessProgramNo" Width="100" AutoPostBack="true"
                                runat="server" OnSelectedIndexChanged="DDProcessProgramNo_SelectedIndexChanged">
                            </asp:DropDownList>
                            <cc1:ListSearchExtender ID="ListSearchExtender3" runat="server" TargetControlID="DDProcessProgramNo"
                                ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                            </cc1:ListSearchExtender>
                        </td>
                        <td class="tdstyle">
                            <asp:Label ID="Label4" Text="Approval Date" runat="server" CssClass="labelbold" />
                            <br />
                            <asp:TextBox ID="TxtApprovalDate" runat="server" CssClass="textb" Width="100px"></asp:TextBox>
                            <cc1:CalendarExtender ID="CalendarExtender1" runat="server" Format="dd-MMM-yyyy"
                                TargetControlID="TxtApprovalDate">
                            </cc1:CalendarExtender>
                        </td>
                        <td class="tdstyle">
                            <asp:Label ID="Label6" Text="ApprovedBy" runat="server" CssClass="labelbold" />
                            <br />
                            <asp:TextBox ID="TxtApprovedBy" runat="server" CssClass="textb" Width="250px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="5">
                            <asp:Label ID="LblErr" ForeColor="Red" runat="server" Text="" CssClass="labelbold"></asp:Label>
                        </td>
                        <td>
                            <asp:Button CssClass="buttonnorm" ID="BtnNew" runat="server" Text="New" OnClientClick="return NewForm();"
                                TabIndex="7" />
                            <asp:Button CssClass="buttonnorm" ID="BtnSave" runat="server" Text="Save" OnClientClick="return validation();"
                                OnClick="BtnSave_Click" TabIndex="4" />
                            <asp:Button CssClass="buttonnorm preview_width" ID="BtnPreview" runat="server" Text="Preview"
                                Enabled="false" OnClick="BtnPreview_Click" TabIndex="5" />
                            <asp:Button CssClass="buttonnorm" ID="BtnClose" runat="server" Text="Close" OnClientClick="return CloseForm()"
                                TabIndex="6" />
                        </td>
                    </tr>
                    <tr>
                        <td style="padding-top: 10px">
                            <div id="d23" runat="server" style="padding-top: 10px">
                                <asp:CheckBoxList ID="CHkPindentNo" runat="server" CssClass="dropdown" OnSelectedIndexChanged="CHkPindentNo_SelectedIndexChanged"
                                    AutoPostBack="True" TabIndex="3">
                                </asp:CheckBoxList>
                            </div>
                        </td>
                        <td colspan="4">
                            <div style="width: 100%; height: 200px; overflow: scroll">
                                <asp:GridView ID="DGIndentDetail" AutoGenerateColumns="False" runat="server" DataKeyNames="IndentDetailId"
                                    CssClass="grid-views">
                                    <HeaderStyle CssClass="gvheaders" />
                                    <AlternatingRowStyle CssClass="gvalts" />
                                    <RowStyle CssClass="gvrow" />
                                    <EmptyDataRowStyle CssClass="gvemptytext" />
                                    <Columns>
                                        <asp:BoundField DataField="IndentDetailId" HeaderText="IndentDetailId" Visible="false" />
                                        <asp:BoundField DataField="IndentNo" HeaderText="IndentNo">
                                            <HeaderStyle Width="100px" HorizontalAlign="Center" />
                                            <ItemStyle Width="100px" HorizontalAlign="Center" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="PPNo" HeaderText="PPNo">
                                            <HeaderStyle Width="100px" HorizontalAlign="Center" />
                                            <ItemStyle Width="100px" HorizontalAlign="Center" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="InDescription" HeaderText="InDescription">
                                            <HeaderStyle Width="250px" HorizontalAlign="Center" />
                                            <ItemStyle Width="250px" HorizontalAlign="Center" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="OutDescription" HeaderText="OutDescription">
                                            <HeaderStyle Width="250px" HorizontalAlign="Center" />
                                            <ItemStyle Width="250px" HorizontalAlign="Center" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="Rate" HeaderText="Rate">
                                            <HeaderStyle Width="70px" HorizontalAlign="Center" />
                                            <ItemStyle Width="70px" HorizontalAlign="Center" />
                                        </asp:BoundField>
                                        <asp:TemplateField ShowHeader="False">
                                            <ItemTemplate>
                                                <asp:TextBox ID="TxtQty" runat="server" Width="70px" Text='<%# Bind("Quantity") %>'
                                                    AutoPostBack="true" CssClass="textb"></asp:TextBox>
                                            </ItemTemplate>
                                            <HeaderStyle HorizontalAlign="Center" Width="20px" />
                                            <ItemStyle HorizontalAlign="Center" Width="20px" />
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </div>
                        </td>
                    </tr>
                </table>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</asp:Content>
