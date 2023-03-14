<%@ Page Language="C#" AutoEventWireup="true" CodeFile="FrmRawMaterialLikeDyingHissab.aspx.cs"
    MasterPageFile="~/ERPmaster.master" Inherits="Masters_Hissab_FrmRawMaterialLikeDyingHissab"
    EnableEventValidation="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="page" ContentPlaceHolderID="CPH_Form" runat="server">
    <script src="../../Scripts/JScript.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery-1.4.1.js" type="text/javascript"></script>
    <script src="../../Scripts/FixFocus2.js" type="text/javascript"></script>
    <script type="text/javascript">
        function CloseForm() {
            window.location.href = "../../main.aspx";
        }
        function NewForm() {
            window.location.href = "FrmRawMaterialLikeDyingHissab.aspx";
        }
        function Preview() {
            window.open("../../ReportViewer.aspx");
        }
        function validate() {
            if (document.getElementById("<%=DDPartyName.ClientID %>").value <= "0") {
                alert("Pls Select Party Name");
                document.getElementById("<%=DDPartyName.ClientID %>").focus();
                return false;
            }
            var isValid = false;
            var i = 0;

            var gridView = document.getElementById('CPH_Form_DGIndentDetail');

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
            if (document.getElementById("<%=Textbillno.ClientID %>").value == "") {
                alert("Bill No. Cannot Be Blank");
                document.getElementById("<%=Textbillno.ClientID %>").focus();
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
        function isNumber(evt) {
            var charCode = (evt.which) ? evt.which : event.keyCode
            if (charCode != 46 && charCode > 31 && (charCode < 48 || charCode > 57)) {
                alert('numeric only');
                return false;
            }
            else {
                return true;
            }
        }
        function CheckAll(objref) {
            var gridview = objref.parentNode.parentNode.parentNode;
            var inputlist = gridview.getElementsByTagName("input");
            for (var i = 0; i < inputlist.length; i++) {
                var row = inputlist[i].parentNode.parentNode;
                if (inputlist[i].type == "checkbox" && objref != inputlist[i]) {
                    if (objref.checked) {

                        inputlist[i].checked = true;
                        row.style.backgroundColor = "Orange";
                    }
                    else {
                        inputlist[i].checked = false;
                        row.style.backgroundColor = "White";

                    }
                }
            }

        }
    </script>
    <asp:UpdatePanel ID="up1" runat="server">
        <ContentTemplate>
            <div>
                <table>
                    <tr>
                        <td id="TDForsample" runat="server">
                            <asp:CheckBox ID="chksample" Text="For Sample" runat="server" AutoPostBack="true"
                                CssClass="checkboxbold" OnCheckedChanged="chksample_CheckedChanged" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdstyle">
                            <asp:Label ID="lbl" Text=" CompanyName" runat="server" CssClass="labelbold" />
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="DDCompanyName"
                                ErrorMessage="please fill Company......." ForeColor="Red" SetFocusOnError="true"
                                ValidationGroup="f1">*</asp:RequiredFieldValidator>
                            <br />
                            <asp:DropDownList CssClass="dropdown" ID="DDCompanyName" runat="server" Width="250px"
                                OnSelectedIndexChanged="DDCompanyName_SelectedIndexChanged">
                            </asp:DropDownList>
                            <cc1:ListSearchExtender ID="ListSearchExtender2" runat="server" TargetControlID="DDCompanyName"
                                ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                            </cc1:ListSearchExtender>
                        </td>
                        <td class="tdstyle">
                            <asp:Label ID="Label1" Text="  Process Name" runat="server" CssClass="labelbold" />
                            <br />
                            <asp:DropDownList CssClass="dropdown" ID="DDProcess" runat="server" Width="150px"
                                OnSelectedIndexChanged="DDProcess_SelectedIndexChanged" AutoPostBack="True">
                            </asp:DropDownList>
                            <cc1:ListSearchExtender ID="ListSearchExtender3" runat="server" TargetControlID="DDProcess"
                                ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                            </cc1:ListSearchExtender>
                        </td>
                        <td class="tdstyle">
                            <asp:Label ID="Label2" Text=" PartyName" runat="server" CssClass="labelbold" />
                            <asp:CheckBox ID="ChkEditOrder" runat="server" Text=" Edit Order" CssClass="checkboxbold"
                                AutoPostBack="True" OnCheckedChanged="ChkEditOrder_CheckedChanged" />
                            <br />
                            <asp:DropDownList CssClass="dropdown" ID="DDPartyName" runat="server" Width="150px"
                                OnSelectedIndexChanged="DDPartyName_SelectedIndexChanged" AutoPostBack="True">
                            </asp:DropDownList>
                            <cc1:ListSearchExtender ID="ListSearchExtender1" runat="server" TargetControlID="DDPartyName"
                                ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                            </cc1:ListSearchExtender>
                        </td>
                        <td id="TDBillNo" runat="server">
                            <asp:Label ID="Label3" Text=" Bill No" runat="server" CssClass="labelbold" />
                            <br />
                            <asp:DropDownList CssClass="dropdown" ID="DDBillNo" runat="server" Width="150px"
                                OnSelectedIndexChanged="DDBillNo_SelectedIndexChanged" AutoPostBack="True">
                            </asp:DropDownList>
                            <cc1:ListSearchExtender ID="ListSearchExtender4" runat="server" TargetControlID="DDBillNo"
                                ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                            </cc1:ListSearchExtender>
                        </td>
                    </tr>
                </table>
                <table>
                    <tr>
                        <td>
                            <div style="height: 180px; width: 400px; overflow: scroll;">
                                <asp:GridView ID="DGIndentDetail" Width="100%" runat="server" AutoGenerateColumns="False"
                                    OnRowDataBound="DGIndentDetail_RowDataBound" CssClass="grid-views">
                                    <HeaderStyle CssClass="gvheaders" />
                                    <AlternatingRowStyle CssClass="gvalts" />
                                    <Columns>
                                        <asp:TemplateField HeaderText="">
                                            <HeaderTemplate>
                                                <asp:CheckBox ID="ChkAllItem" runat="server" onclick="return CheckAll(this);"  Visible="false"/>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:CheckBox ID="Chkbox" runat="server" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>"
                                                    OnCheckedChanged="Chkchallan_CheckedChanged" />
                                                <asp:Label ID="lblIndentId" runat="server" Visible="false" Text='<%# Bind("IndentId") %>' />
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" Width="20px" />
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="challanno" HeaderText="Indent No" />
                                        
                                        <asp:TemplateField  HeaderText="Total Amount">
                                            <ItemTemplate>
                                                <asp:Label ID="lblTotal" runat="server" Text='<%#Bind("total") %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <%--<asp:BoundField DataField="total" HeaderText="Total Amount" />--%>
                                        <asp:BoundField DataField="Flag" HeaderStyle-ForeColor="AntiqueWhite" ItemStyle-ForeColor="AntiqueWhite">
                                            <HeaderStyle ForeColor="AntiqueWhite"></HeaderStyle>
                                            <ItemStyle ForeColor="AntiqueWhite"></ItemStyle>
                                            <HeaderStyle HorizontalAlign="Center" Width="0px" />
                                            <ItemStyle HorizontalAlign="Center" Width="0px" />
                                        </asp:BoundField>
                                        <asp:TemplateField Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblProcessRec_PrmId" runat="server" Text='<%#Bind("ProcessRec_PrmId") %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                    <EmptyDataRowStyle CssClass="gvemptytext" />
                                </asp:GridView>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4">
                            <table>
                                <tr>
                                    <td class="tdstyle">
                                        <asp:Label ID="Label4" Text=" Total Amount" runat="server" CssClass="labelbold" />
                                        <br />
                                        <asp:TextBox ID="Txttotamt" runat="server" Enabled="false" Width="80px" CssClass="textb"></asp:TextBox>
                                    </td>
                                    <td class="tdstyle">
                                        <asp:Label ID="Label5" Text="  Bill No" runat="server" CssClass="labelbold" />
                                        <br />
                                        <asp:TextBox CssClass="textb" ID="Textbillno" runat="server" AutoPostBack="True"
                                            Width="80px"></asp:TextBox>
                                    </td>
                                    <td class="tdstyle" id="TDAddition" runat="server">
                                        <asp:Label ID="Label14" Text=" Addition Amt" runat="server" CssClass="labelbold" />
                                        <br />
                                        <asp:TextBox CssClass="textb" ID="txtAdditionAmt" runat="server" AutoPostBack="True"
                                            Width="70px" onkeypress="return isNumber(event);" OnTextChanged="txtAdditionAmt_TextChanged"
                                            BackColor="Beige"></asp:TextBox>
                                    </td>
                                    <td class="tdstyle" id="TDDeductionAmt" runat="server">
                                        <asp:Label ID="Label15" Text=" Deduction Amt" runat="server" CssClass="labelbold" />
                                        <br />
                                        <asp:TextBox CssClass="textb" ID="txtDeductionAmt" runat="server" AutoPostBack="True"
                                            Width="80px" onkeypress="return isNumber(event);" OnTextChanged="txtDeductionAmt_TextChanged"
                                            BackColor="Beige"></asp:TextBox>
                                    </td>
                                    <td class="tdstyle" id="TDGst" runat="server">
                                        <asp:Label ID="Label16" Text="GST(%)" runat="server" CssClass="labelbold" />
                                        <br />
                                        <asp:TextBox CssClass="textb" ID="txtGst" runat="server" AutoPostBack="True" Width="70px"
                                            onkeypress="return isNumber(event);" OnTextChanged="txtGst_TextChanged" BackColor="Beige"></asp:TextBox>
                                    </td>
                                    <td class="tdstyle" id="TDConDyes" runat="server" visible="false">
                                        <asp:Label ID="Label6" Text=" Con.Dyes(%)" runat="server" CssClass="labelbold" />
                                        <br />
                                        <asp:TextBox CssClass="textb" ID="txtConsDyes" runat="server" AutoPostBack="True"
                                            Width="70px" onkeypress="return isNumber(event);" OnTextChanged="txtConsDyes_TextChanged"
                                            BackColor="Beige"></asp:TextBox>
                                    </td>
                                    <td class="tdstyle" id="TDVat" runat="server" visible="false">
                                        <asp:Label ID="Label7" Text="  Vat(%)" runat="server" CssClass="labelbold" />
                                        <br />
                                        <asp:TextBox CssClass="textb" ID="txtvat" runat="server" AutoPostBack="True" Width="70px"
                                            onkeypress="return isNumber(event);" OnTextChanged="txtvat_TextChanged" BackColor="Beige"></asp:TextBox>
                                    </td>
                                    <td class="tdstyle" id="TDSat" runat="server" visible="false">
                                        <asp:Label ID="Label8" Text="  Sat(%)" runat="server" CssClass="labelbold" />
                                        <br />
                                        <asp:TextBox CssClass="textb" ID="txtsat" runat="server" AutoPostBack="True" Width="70px"
                                            onkeypress="return isNumber(event);" OnTextChanged="txtsat_TextChanged" BackColor="Beige"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:Label ID="Label9" Text=" Debit Amt." runat="server" CssClass="labelbold" />
                                        <br />
                                        <asp:TextBox ID="txtDebitamt" runat="server" Width="80px" Enabled="false" CssClass="textb"
                                            BackColor="Beige" Text="0"></asp:TextBox>
                                    </td>
                                    <td class="tdstyle">
                                        <asp:Label ID="Label10" Text="  Amount" runat="server" CssClass="labelbold" />
                                        <br />
                                        <asp:TextBox CssClass="textb" ID="Textpamt" runat="server" AutoPostBack="True" Width="80px"
                                            onkeypress="return isNumber(event);"></asp:TextBox>
                                    </td>
                                    <td class="tdstyle">
                                        <asp:Label ID="Label11" Text=" Date" runat="server" CssClass="labelbold" />
                                        <br />
                                        <asp:TextBox CssClass="textb" ID="TxtDate" runat="server" Width="90px"></asp:TextBox>
                                        <asp:CalendarExtender ID="CalendarExtender1" runat="server" Format="dd-MMM-yyyy"
                                            TargetControlID="TxtDate">
                                        </asp:CalendarExtender>
                                    </td>
                                    <td class="tdstyle">
                                        <asp:Label ID="Label12" Text="  Remark" runat="server" CssClass="labelbold" />
                                        <br />
                                        <asp:TextBox CssClass="textb" ID="txtremark" runat="server" AutoPostBack="True" TextMode="MultiLine"
                                            Width="150px"></asp:TextBox>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="LblErrorMessage" runat="server" Font-Bold="true" ForeColor="RED" Text=""
                                Visible="false"></asp:Label>
                        </td>
                        <td align="right">
                            <asp:Button CssClass="buttonnorm" ID="BtnDeductionAmountDetail" runat="server" Text="Ded Amt Detail"
                                OnClick="BtnDeductionAmountDetail_Click" Visible="false" />
                            <asp:Button CssClass="buttonnorm" ID="BtnIndentPreview" runat="server" Text="Indent Detail"
                                OnClick="BtnIndentPreview_Click" />
                            <asp:Button CssClass="buttonnorm" ID="BtnNew" runat="server" Text="New" OnClientClick="return NewForm();" />
                            <asp:Button CssClass="buttonnorm" ID="BtnSave" Text="Save" runat="server" OnClick="BtnSave_Click"
                                OnClientClick="return validate()" ValidationGroup="f1" />
                            <asp:Button CssClass="buttonnorm" ID="BtnClose" runat="server" Text="Close" OnClientClick="return CloseForm();" />
                            <asp:Button CssClass="buttonnorm" ID="Btnpreview" runat="server" Text="Preview" OnClick="Btnpreview_Click" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <table border="1">
                                <tr id="TRBilldetails" runat="server">
                                    <td>
                                        <asp:Label ID="lblfromdate" Text="From Date" runat="server" CssClass="labelbold" />
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtfromdate" CssClass="textb" runat="server" Width="100px" />
                                        <asp:CalendarExtender ID="calfrom" runat="server" TargetControlID="txtfromdate" Format="dd-MMM-yyyy">
                                        </asp:CalendarExtender>
                                    </td>
                                    <td>
                                        <asp:Label ID="Label13" Text="To Date" runat="server" CssClass="labelbold" />
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txttodate" CssClass="textb" runat="server" Width="100px" />
                                        <asp:CalendarExtender ID="CalendarExtender2" runat="server" TargetControlID="txttodate"
                                            Format="dd-MMM-yyyy">
                                        </asp:CalendarExtender>
                                    </td>
                                    <td>
                                        <asp:Button ID="btnbilldetails" Text="Dyer Bill Details" CssClass="buttonnorm" runat="server"
                                            OnClick="btnbilldetails_Click" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
