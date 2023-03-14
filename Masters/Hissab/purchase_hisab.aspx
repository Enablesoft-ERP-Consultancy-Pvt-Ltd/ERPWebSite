<%@ Page Language="C#" AutoEventWireup="true" CodeFile="purchase_hisab.aspx.cs" MasterPageFile="~/ERPmaster.master"
    Inherits="Masters_Hissab_purchase_hisab" EnableEventValidation="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="page" ContentPlaceHolderID="CPH_Form" runat="server">
    <script src="../../Scripts/JScript.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery-1.4.1.js" type="text/javascript"></script>
    <%--<script type="text/javascript" src="../../Scripts/Fixfocus.js"></script>--%>
    <script type="text/javascript">
        function CloseForm() {
            window.location.href = "../../main.aspx";
        }
        function NewForm() {
            window.location.href = "purchase_hisab.aspx";
        }
        function Preview() {
            window.open("../../ReportViewer.aspx", "PurchaseReceive");
        }
        function isNumber(evt) {
            var charCode = (evt.which) ? evt.which : event.keyCode
            if (charCode != 46 && charCode > 31 && (charCode < 48 || charCode > 57)) {
                return false;
            }
            else {
                return true;
            }
        }
        function validate() {
            if (document.getElementById("<%=DDPartyName.ClientID %>").value <= "0") {
                alert("Pls Select Party Name");
                document.getElementById("<%=DDPartyName.ClientID %>").focus();
                return false;
            }
            var isValid = false;
            var i = 0;
            var gridView = document.getElementById('CPH_Form_DGChallanDetail');
            for (var i = 1; i < gridView.rows.length; i++) {
                var inputs = gridView.rows[i].getElementsByTagName('input');
                if (inputs != null) {
                    if (inputs[0].type == "checkbox") {
                        if (inputs[0].checked) {
                            isValid = true;
                        }
                    }
                }
            }
            if (isValid == false) {
                alert("Please select at least one item");
                inputs[0].checked = false;
                return false;
            }
            if (document.getElementById('CPH_Form_ChkEditOrder').checked == true) {

                if (document.getElementById("<%=DDBillNo.ClientID %>").value <= "0") {
                    alert("Pls select bill no...");
                    document.getElementById("<%=DDBillNo.ClientID %>").focus();
                    return false;
                }
            }
            if (document.getElementById("<%=Textbillno.ClientID %>").value == "") {
                alert("Bill No. Cannot Be Blank");
                document.getElementById("<%=Textbillno.ClientID %>").focus();
                return false;
            }
            if (document.getElementById("<%=TxtBillAmount.ClientID %>").value == "") {
                alert("Bill amount cannot be blank");
                document.getElementById("<%=TxtBillAmount.ClientID %>").focus();
                return false;
            }

            if (document.getElementById("<%=TxtBillDate.ClientID %>").value == "") {
                alert("Bill date cannot be blank");
                document.getElementById("<%=TxtBillDate.ClientID %>").focus();
                return false;
            }
            if (document.getElementById("<%=TxtDate.ClientID %>").value == "") {
                alert("Date Cannot Be Blank");
                document.getElementById("<%=TxtDate.ClientID %>").focus();
                return false;
            }
            if (document.getElementById("<%=Textpamt.ClientID %>").value == "") {
                alert("Amount Cannot Be Blank");
                document.getElementById("<%=Textpamt.ClientID %>").focus();
                return false;
            }
            return confirm('Do You Want To Save?')
        }
    </script>
    <asp:UpdatePanel ID="up1" runat="server">
        <ContentTemplate>
            <div>
                <table>
                    <tr>
                        <td class="tdstyle">
                            <asp:Label Text="CompanyName" runat="server" ID="lbl" CssClass="labelbold" />
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="DDCompanyName"
                                ErrorMessage="please fill Company......." ForeColor="Red" SetFocusOnError="true"
                                ValidationGroup="f1">*</asp:RequiredFieldValidator>
                            <br />
                            <asp:DropDownList CssClass="dropdown" ID="DDCompanyName" runat="server" TabIndex="2"
                                Width="250px">
                            </asp:DropDownList>
                            <cc1:ListSearchExtender ID="ListSearchExtender2" runat="server" TargetControlID="DDCompanyName"
                                ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                            </cc1:ListSearchExtender>
                        </td>
                        <td class="tdstyle">
                            <asp:Label Text=" PartyName" runat="server" ID="Label1" CssClass="labelbold" />
                            <asp:CheckBox ID="ChkEditOrder" runat="server" Text=" Edit Order" CssClass="checkboxbold"
                                AutoPostBack="True" OnCheckedChanged="ChkEditOrder_CheckedChanged" />
                            <br />
                            <asp:DropDownList CssClass="dropdown" ID="DDPartyName" runat="server" Width="200px"
                                OnSelectedIndexChanged="DDPartyName_SelectedIndexChanged" AutoPostBack="True"
                                TabIndex="3">
                            </asp:DropDownList>
                            <cc1:ListSearchExtender ID="ListSearchExtender1" runat="server" TargetControlID="DDPartyName"
                                ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                            </cc1:ListSearchExtender>
                        </td>
                        <td id="TDBillNo" runat="server">
                            <asp:Label Text=" Bill No" runat="server" ID="Label2" CssClass="labelbold" />
                            <br />
                            <asp:DropDownList CssClass="dropdown" ID="DDBillNo" runat="server" Width="200px"
                                OnSelectedIndexChanged="DDBillNo_SelectedIndexChanged" AutoPostBack="True" TabIndex="3">
                            </asp:DropDownList>
                            <cc1:ListSearchExtender ID="ListSearchExtender3" runat="server" TargetControlID="DDBillNo"
                                ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                            </cc1:ListSearchExtender>
                        </td>
                    </tr>
                </table>
                <table>
                    <tr>
                        <td>
                            <div style="height: 180px; width: 400px; overflow: scroll;">
                                <asp:GridView ID="DGChallanDetail" Width="100%" runat="server" AutoGenerateColumns="False"
                                    OnRowDataBound="DGChallanDetail_RowDataBound" CssClass="grid-views">
                                    <HeaderStyle CssClass="gvheaders" />
                                    <AlternatingRowStyle CssClass="gvalts" />
                                    <Columns>
                                        <asp:TemplateField HeaderText="">
                                            <ItemTemplate>
                                                <asp:CheckBox ID="Chkbox" runat="server" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>"
                                                    OnCheckedChanged="Chkchallan_CheckedChanged" />
                                                <asp:Label ID="lblpreceiveid" runat="server" Visible="false" Text='<%# Bind("purchasereceiveid") %>' />
                                            </ItemTemplate>
                                            <HeaderStyle Width="80px" />
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="challanno" HeaderText="Challan NO" />
                                        <asp:BoundField DataField="total" HeaderText="Total Amount" />
                                        <asp:BoundField DataField="Flag" HeaderStyle-ForeColor="AntiqueWhite" ItemStyle-ForeColor="AntiqueWhite">
                                            <HeaderStyle ForeColor="AntiqueWhite"></HeaderStyle>
                                            <ItemStyle ForeColor="AntiqueWhite"></ItemStyle>
                                            <HeaderStyle HorizontalAlign="Center" Width="0px" />
                                            <ItemStyle HorizontalAlign="Center" Width="0px" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="DebitAmt" HeaderText="Debit Amount" />
                                    </Columns>
                                    <EmptyDataRowStyle CssClass="gvemptytext" />
                                </asp:GridView>
                            </div>
                        </td>
                    </tr>
                </table>
                <table>
                    <tr>
                        <td class="tdstyle">
                            <asp:Label Text=" Total Amount" runat="server" ID="Label3" CssClass="labelbold" />
                            <br />
                            <asp:TextBox ID="Txttotamt" runat="server" Enabled="false" Width="80px" TabIndex="24"
                                CssClass="textb"></asp:TextBox>
                        </td>
                        <td class="tdstyle">
                            <asp:Label Text="  Bill No" runat="server" ID="Label4" CssClass="labelbold" />
                            <br />
                            <asp:TextBox ID="Textbillno" runat="server" AutoPostBack="True" Width="80px" TabIndex="24"
                                CssClass="textb"></asp:TextBox>
                        </td>
                        <td class="tdstyle">
                            <asp:Label Text=" Bill Amount" runat="server" ID="Label5" CssClass="labelbold" />
                            <br />
                            <asp:TextBox ID="TxtBillAmount" runat="server" Width="80px" TabIndex="24" CssClass="textb"
                                onkeypress="return isNumber(event);" onkeydown="return (event.keyCode!=13);"></asp:TextBox>
                        </td>
                        <td class="tdstyle">
                            <asp:Label Text=" Bill Date" runat="server" ID="Label6" CssClass="labelbold" />
                            <br />
                            <asp:TextBox CssClass="textb" ID="TxtBillDate" runat="server" Width="100px"></asp:TextBox>
                            <asp:CalendarExtender ID="CalendarExtender2" runat="server" Format="dd-MMM-yyyy"
                                TargetControlID="TxtBillDate">
                            </asp:CalendarExtender>
                        </td>
                        <td>
                            <asp:Label Text="Debit Amt." runat="server" ID="Label7" CssClass="labelbold" />
                            <br />
                            <asp:TextBox ID="txtDebitAmt" runat="server" CssClass="textb" Width="80px" Enabled="false"
                                BackColor="Beige" Text="0"></asp:TextBox>
                        </td>
                        <td id="TDDeductionAmt" runat="server" visible="false">
                            <asp:Label Text="Deduction Amt." runat="server" ID="Label11" CssClass="labelbold" />
                            <br />
                            <asp:TextBox ID="txtDeductionAmt" runat="server" CssClass="textb" Width="80px" Enabled="true"
                                BackColor="Beige" Text="0" AutoPostBack="true" OnTextChanged="txtDeductionAmt_TextChanged"></asp:TextBox>
                        </td>
                        <td class="tdstyle">
                            <asp:Label Text="Amount" runat="server" ID="Label8" CssClass="labelbold" />
                            <br />
                            <asp:TextBox ID="Textpamt" runat="server" AutoPostBack="True" Width="80px" TabIndex="24"
                                CssClass="textb" onkeypress="return isNumber(event);"></asp:TextBox>
                        </td>
                        <td class="tdstyle">
                            <asp:Label Text="Date" runat="server" ID="Label9" CssClass="labelbold" />
                            <br />
                            <asp:TextBox CssClass="textb" ID="TxtDate" runat="server" Width="100px"></asp:TextBox>
                            <asp:CalendarExtender ID="CalendarExtender1" runat="server" Format="dd-MMM-yyyy"
                                TargetControlID="TxtDate">
                            </asp:CalendarExtender>
                        </td>
                        <td class="tdstyle">
                            <asp:Label Text=" Remark" runat="server" ID="Label10" CssClass="labelbold" />
                            <br />
                            <asp:TextBox ID="txtremark" runat="server" AutoPostBack="True" TextMode="MultiLine"
                                Width="200px" TabIndex="24"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td align="right" colspan="8">
                            <asp:Label ID="LblErrorMessage" runat="server" Font-Bold="true" ForeColor="RED" Text=""
                                Visible="false"></asp:Label>
                            <asp:Button CssClass="buttonnorm" ID="BtnPurchasePreview" runat="server" Text="Purchase Detail"
                                OnClick="BtnPurchasePreview_Click" />
                            <asp:Button CssClass="buttonnorm" ID="BtnNew" runat="server" Text="New" OnClientClick="return NewForm();" />
                            <asp:Button CssClass="buttonnorm" ID="BtnSave" Text="Save" runat="server" OnClick="BtnSave_Click"
                                OnClientClick="return validate()" ValidationGroup="f1" TabIndex="45" />
                                  <asp:Button ID="BtnPreview" runat="server" CssClass="buttonnorm preview_width" OnClick="Preview_Click"
                            Visible="false" Text="Preview" />

                            <asp:Button CssClass="buttonnorm" ID="BtnClosse" runat="server" Text="Close" OnClientClick="return CloseForm();" />
                        </td>
                    </tr>
                </table>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
