<%@ Page Language="C#" AutoEventWireup="true" CodeFile="FrmGateInOfAllItems.aspx.cs"
    Inherits="Masters_Carpet_FrmGateInOfAllItems" MasterPageFile="~/ERPmaster.master"
    EnableEventValidation="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="CPH_Form" runat="server">
    <script src="../../Scripts/JScript.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery-1.4.1.js" type="text/javascript"></script>
    <script type="text/javascript">
        function NewForm() {
            window.location.href = "FrmGateInOfAllItems.aspx";
        }
        function closeform() {
            window.location.href = "../../main.aspx";
        }
        function Preview() {
            window.open('../../ReportViewer.aspx', '');
        }
        function isNumberKey(evt) {
            var charCode = (evt.which) ? evt.which : event.keyCode
            if (charCode != 46 && charCode > 31 && (charCode < 48 || charCode > 57)) {
                return false;
            }
            else {
                return true;
            }
        }
        function CheckQty() {
            var VarIssQty = 0;
            var VarStockQty = 0;
            if (document.getElementById('CPH_Form_TxtIssueQty').value != "") {
                VarIssQty = document.getElementById('CPH_Form_TxtIssueQty').value;
            }
            if (document.getElementById('CPH_Form_TxtStock').value != "") {
                VarStockQty = document.getElementById('CPH_Form_TxtStock').value;
            }
            if (parseFloat(VarIssQty) > parseFloat(VarStockQty)) {
                alert("Invalid qty (Valid qty is not more than " + VarStockQty + ") ....!");
                document.getElementById('CPH_Form_TxtIssueQty').value = "";
                document.getElementById("CPH_Form_TxtIssueQty").focus();
                return false;
            }
        }
        function CalculateAmount() {
            var VarIssQty = 0;
            var VarQty = 0;
            var VarRate = 0;
            var VarVat = 0;
            var VarSat = 0;
            var VarCST = 0;
            var FrieghtCharge = 0;
            var VarRetQty = 0;
            var VarPQty = 0;
            var VarTotalIssQty = 0;
            if (document.getElementById('CPH_Form_TxtIssueQty').value != "") {
                VarIssQty = document.getElementById('CPH_Form_TxtIssueQty').value;
            }
            if (document.getElementById('CPH_Form_TxtQty').value != "") {
                VarQty = document.getElementById('CPH_Form_TxtQty').value;
            }
            if (document.getElementById('CPH_Form_TxtPQty').value != "") {
                VarPQty = document.getElementById('CPH_Form_TxtPQty').value;
            }
            VarTotalIssQty = (parseFloat(VarIssQty) * 110) / 100;
            VarPQty = parseFloat(VarTotalIssQty) - parseFloat(VarIssQty) + parseFloat(VarPQty)
            if ((parseFloat(VarQty) - parseFloat(VarPQty)) > 0) {
                alert("Pls Enter Correct Qty ....!");
                document.getElementById('CPH_Form_TxtQty').value = "";
                document.getElementById("CPH_Form_TxtQty").focus();
                return false;
            }
            if (document.getElementById('CPH_Form_TxtExceisDuty').value != "") {
                VarVat = parseFloat(document.getElementById('CPH_Form_TxtExceisDuty').value);
            }
            if (document.getElementById('CPH_Form_TxtEduCess').value != "") {
                VarSat = parseFloat(document.getElementById('CPH_Form_TxtEduCess').value);
            }
            if (document.getElementById('CPH_Form_TxtCst').value != "") {
                VarCST = parseFloat(document.getElementById('CPH_Form_TxtCst').value);
            }
            if (document.getElementById('CPH_Form_TxtRate').value != "") {
                VarRate = document.getElementById('CPH_Form_TxtRate').value;
            }
            if (document.getElementById('CPH_Form_TxtFrieghtCharge').value != "") {
                FrieghtCharge = document.getElementById('CPH_Form_TxtFrieghtCharge').value;
            }
            if (document.getElementById('CPH_Form_ChkForReturn').checked == true) {
                if (document.getElementById('CPH_Form_TxtReturnQty').value != "") {
                    VarRetQty = document.getElementById('CPH_Form_TxtReturnQty').value;
                }
            }
            VarQty = (parseFloat(VarQty) - parseFloat(VarRetQty));
            var Amount1 = (VarQty * VarRate);
            var Amount = parseFloat(Amount1);
            document.getElementById('CPH_Form_TxtAmount').value = Amount;
            document.getElementById('CPH_Form_TxtNetAmount').value = (Amount) + (parseFloat(VarVat + VarCST + VarSat) * Amount / 100) + (parseFloat(FrieghtCharge));
        }

        function ValidateSave() {
            if (document.getElementById('CPH_Form_DDCompanyName').options[document.getElementById('CPH_Form_DDCompanyName').selectedIndex].value == 0) {
                alert("Please select company name....!");
                document.getElementById("CPH_Form_DDCompanyName").focus();
                return false;
            }
            if (document.getElementById('CPH_Form_DDDepartment').options[document.getElementById('CPH_Form_DDDepartment').selectedIndex].value == 0) {
                alert("Please select department....!");
                document.getElementById("CPH_Form_DDDepartment").focus();
                return false;
            }
            if (document.getElementById('CPH_Form_DDPartyName').options[document.getElementById('CPH_Form_DDPartyName').selectedIndex].value == 0) {
                alert("Please select party name....!");
                document.getElementById("CPH_Form_DDPartyName").focus();
                return false;
            }
            if (document.getElementById('CPH_Form_ChkForEdit').checked == true) {
                if (document.getElementById('CPH_Form_DDIssueNo').options[document.getElementById('CPH_Form_DDIssueNo').selectedIndex].value == 0) {
                    alert("Please select Issue no....!");
                    document.getElementById("CPH_Form_DDIssueNo").focus();
                    return false;
                }
            }
            if (document.getElementById('CPH_Form_TxtIssueDate').value == "") {
                alert("Pls fill issue date....!");
                document.getElementById('CPH_Form_TxtIssueDate').focus();
                return false;
            }
            if (document.getElementById('CPH_Form_DDCategory').options[document.getElementById('CPH_Form_DDCategory').selectedIndex].value == 0) {
                alert("Please select category name....!");
                document.getElementById("CPH_Form_DDCategory").focus();
                return false;
            }
            if (document.getElementById('CPH_Form_DDItem').options[document.getElementById('CPH_Form_DDItem').selectedIndex].value == 0) {
                alert("Please Select Item Name....!");
                document.getElementById("CPH_Form_DDItem").focus();
                return false;
            }
            if (document.getElementById('CPH_Form_TdQuality')) {
                if (document.getElementById('CPH_Form_DDQuality').options[document.getElementById('CPH_Form_DDQuality').selectedIndex].value == 0) {
                    alert("Please select quality....!");
                    document.getElementById("CPH_Form_DDQuality").focus();
                    return false;
                }
            }
            if (document.getElementById('CPH_Form_TdDesign')) {
                if (document.getElementById('CPH_Form_DDDesign').options[document.getElementById('CPH_Form_DDDesign').selectedIndex].value == 0) {
                    alert("Please select design....!");
                    document.getElementById("CPH_Form_DDDesign").focus();
                    return false;
                }
            }
            if (document.getElementById('CPH_Form_TdColor')) {
                if (document.getElementById('CPH_Form_DDColor').options[document.getElementById('CPH_Form_DDColor').selectedIndex].value == 0) {
                    alert("Please select color....!");
                    document.getElementById("CPH_Form_DDColor").focus();
                    return false;
                }
            }
            if (document.getElementById('CPH_Form_TdShape')) {
                if (document.getElementById('CPH_Form_DDShape').options[document.getElementById('CPH_Form_DDShape').selectedIndex].value == 0) {
                    alert("Please select shape....!");
                    document.getElementById("CPH_Form_DDShape").focus();
                    return false;
                }
            }
            if (document.getElementById('CPH_Form_TdSize')) {
                if (document.getElementById('CPH_Form_DDSize').options[document.getElementById('CPH_Form_DDSize').selectedIndex].value == 0) {
                    alert("Please select size....!");
                    document.getElementById("CPH_Form_DDSize").focus();
                    return false;
                }
            }
            if (document.getElementById('CPH_Form_TdColorShade')) {
                if (document.getElementById('CPH_Form_DDColorShade').options[document.getElementById('CPH_Form_DDColorShade').selectedIndex].value == 0) {
                    alert("Please select shadecolor....!");
                    document.getElementById("CPH_Form_DDColorShade").focus();
                    return false;
                }
            }



            if (document.getElementById('CPH_Form_DDUnit').options[document.getElementById('CPH_Form_DDUnit').selectedIndex].value == 0) {
                alert("Please select unit....!");
                document.getElementById("CPH_Form_DDUnit").focus();
                return false;
            }
            if (document.getElementById('CPH_Form_DDGodownName').options[document.getElementById('CPH_Form_DDGodownName').selectedIndex].value == 0) {
                alert("Please select godown name....!");
                document.getElementById("CPH_Form_DDGodownName").focus();
                return false;
            }
            if (document.getElementById('CPH_Form_DDLotNo').options[document.getElementById('CPH_Form_DDLotNo').selectedIndex].value == 0) {
                alert("Please select lot no....!");
                document.getElementById("CPH_Form_DDLotNo").focus();
                return false;
            }
            if (document.getElementById('CPH_Form_TxtStock').value == "") {
                alert("Pls fill stock....!");
                document.getElementById('CPH_Form_TxtStock').focus();
                return false;
            }
            if (document.getElementById('CPH_Form_TxtIssueQty').value == "") {
                alert("Pls fill issue qty....!");
                document.getElementById('CPH_Form_TxtIssueQty').focus();
                return false;
            }
            return confirm('Do You Want To Save?')
        }
    </script>
    <asp:UpdatePanel ID="updatepanel1" runat="server">
        <ContentTemplate>
            <div>
                <table>
                    <tr>
                        <td id="TdIssueNoNew" runat="server" class="tdstyle" visible="false">
                            Issue No<br />
                            <asp:TextBox ID="TxtIssueNoNew" CssClass="textb" runat="server" Width="100px"></asp:TextBox>
                        </td>
                        <td class="tdstyle">
                            Company Name
                            <br />
                            <asp:DropDownList ID="DDCompanyName" CssClass="dropdown" runat="server" Width="200px">
                            </asp:DropDownList>
                            <cc1:ListSearchExtender ID="ListSearchExtender1" runat="server" TargetControlID="DDCompanyName"
                                ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                            </cc1:ListSearchExtender>
                        </td>
                        <td class="tdstyle">
                            Deparment
                            <br />
                            <asp:DropDownList ID="DDDepartment" CssClass="dropdown" runat="server" Width="145px"
                                OnSelectedIndexChanged="DDDepartment_SelectedIndexChanged" AutoPostBack="true">
                            </asp:DropDownList>
                            <cc1:ListSearchExtender ID="ListSearchExtender5" runat="server" TargetControlID="DDDepartment"
                                ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                            </cc1:ListSearchExtender>
                        </td>
                        <td class="tdstyle">
                            Party Name
                            <asp:CheckBox ID="ChkForEdit" runat="server" AutoPostBack="True" Text="  For Edit"
                                OnCheckedChanged="ChkForEdit_CheckedChanged" CssClass="checkboxbold" />
                            <br />
                            <asp:DropDownList CssClass="dropdown" ID="DDPartyName" runat="server" OnSelectedIndexChanged="DDPartyName_SelectedIndexChanged"
                                AutoPostBack="True" Width="200px">
                            </asp:DropDownList>
                            <cc1:ListSearchExtender ID="ListSearchDDPartyName" runat="server" TargetControlID="DDPartyName"
                                ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                            </cc1:ListSearchExtender>
                        </td>
                        <td id="TDDDIssueNo" class="tdstyle" runat="server" visible="false">
                            Issue No
                            <br />
                            <asp:DropDownList CssClass="dropdown" ID="DDIssueNo" runat="server" OnSelectedIndexChanged="DDIssueNo_SelectedIndexChanged"
                                AutoPostBack="True" Width="120px">
                            </asp:DropDownList>
                            <cc1:ListSearchExtender ID="ListSearchDDChallanNo" runat="server" TargetControlID="DDIssueNo"
                                ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                            </cc1:ListSearchExtender>
                        </td>
                        <td class="tdstyle">
                            Iss Date
                            <br />
                            <asp:TextBox ID="TxtIssueDate" CssClass="textb" runat="server" Width="90px"></asp:TextBox>
                            <cc1:CalendarExtender ID="CalendarExtender1" runat="server" Format="dd-MMM-yyyy"
                                TargetControlID="TxtIssueDate">
                            </cc1:CalendarExtender>
                        </td>
                        <td id="TdIssueNo" runat="server" class="tdstyle">
                            Issue No<br />
                            <asp:TextBox ID="TxtIssueNo" CssClass="textb" runat="server" Width="90px"></asp:TextBox>
                        </td>
                        <td class="tdstyle">
                            Category Type
                            <br />
                            <asp:DropDownList ID="DDCategoryType" CssClass="dropdown" AutoPostBack="true" runat="server"
                                Width="125px" OnSelectedIndexChanged="DDCategoryType_SelectedIndexChanged">
                                <asp:ListItem Value="1">Raw Material</asp:ListItem>
                                <asp:ListItem Value="0">Finished Item</asp:ListItem>
                            </asp:DropDownList>
                            <cc1:ListSearchExtender ID="ListSearchExtender3" runat="server" TargetControlID="DDCategoryType"
                                ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                            </cc1:ListSearchExtender>
                        </td>
                    </tr>
                </table>
                <table>
                    <tr>
                        <td id="TDProductCode" runat="server" visible="false">
                            Product Code<br />
                            <asp:TextBox ID="TxtProductCode" CssClass="textb" runat="server" Width="100px" OnTextChanged="TxtProductCode_TextChanged"
                                AutoPostBack="True"></asp:TextBox>
                            <cc1:AutoCompleteExtender ID="TxtProdCode_AutoCompleteExtender" runat="server" EnableCaching="true"
                                Enabled="True" MinimumPrefixLength="1" ServiceMethod="GetQuality" TargetControlID="TxtProductCode"
                                UseContextKey="True">
                            </cc1:AutoCompleteExtender>
                        </td>
                        <td class="tdstyle">
                            <asp:Label ID="LblCategory" runat="server" AutoPostBack="true" Text="CategoryName"></asp:Label>
                            <br />
                            <asp:DropDownList CssClass="dropdown" ID="DDCategory" Width="150px" AutoPostBack="True"
                                runat="server" OnSelectedIndexChanged="DDCategory_SelectedIndexChanged">
                            </asp:DropDownList>
                            <cc1:ListSearchExtender ID="ListSearchDDCategory" runat="server" TargetControlID="DDCategory"
                                ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                            </cc1:ListSearchExtender>
                        </td>
                        <td class="tdstyle">
                            <asp:Label ID="LblItemName" runat="server" AutoPostBack="true" Text="ItemName"></asp:Label>
                            <br />
                            <asp:DropDownList CssClass="dropdown" ID="DDItem" Width="150px" AutoPostBack="True"
                                runat="server" OnSelectedIndexChanged="DDItem_SelectedIndexChanged">
                            </asp:DropDownList>
                            <cc1:ListSearchExtender ID="ListSearchDDItem" runat="server" TargetControlID="DDItem"
                                ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                            </cc1:ListSearchExtender>
                        </td>
                        <td id="TdQuality" runat="server" visible="false">
                            <asp:Label ID="LblQuality" runat="server" Text="Label"></asp:Label><br />
                            <asp:DropDownList CssClass="dropdown" ID="DDQuality" Width="150px" runat="server"
                                AutoPostBack="True" OnSelectedIndexChanged="DDQuality_SelectedIndexChanged">
                            </asp:DropDownList>
                            <cc1:ListSearchExtender ID="ListSearchDDQuality" runat="server" TargetControlID="DDQuality"
                                ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                            </cc1:ListSearchExtender>
                        </td>
                        <td id="TdDesign" runat="server" visible="false">
                            <asp:Label ID="LblDesign" runat="server" Text="Label"></asp:Label><br />
                            <asp:DropDownList CssClass="dropdown" Width="150px" ID="DDDesign" runat="server"
                                AutoPostBack="True" OnSelectedIndexChanged="DDDesign_SelectedIndexChanged">
                            </asp:DropDownList>
                            <cc1:ListSearchExtender ID="ListSearchDDDesign" runat="server" TargetControlID="DDDesign"
                                ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                            </cc1:ListSearchExtender>
                        </td>
                        <td id="TdColor" runat="server" visible="false">
                            <asp:Label ID="LblColor" runat="server" Text=""></asp:Label><br />
                            <asp:DropDownList CssClass="dropdown" Width="150px" ID="DDColor" runat="server" AutoPostBack="True"
                                OnSelectedIndexChanged="DDColor_SelectedIndexChanged">
                            </asp:DropDownList>
                            <cc1:ListSearchExtender ID="ListSearchDDColor" runat="server" TargetControlID="DDColor"
                                ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                            </cc1:ListSearchExtender>
                        </td>
                        <td id="TdColorShade" runat="server" visible="false">
                            <asp:Label ID="LblColorShade" runat="server" Text=""></asp:Label><br />
                            <asp:DropDownList CssClass="dropdown" Width="150px" ID="DDColorShade" runat="server"
                                AutoPostBack="True" OnSelectedIndexChanged="DDColorShade_SelectedIndexChanged">
                            </asp:DropDownList>
                            <cc1:ListSearchExtender ID="ListSearchDDColorShade" runat="server" TargetControlID="DDColorShade"
                                ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                            </cc1:ListSearchExtender>
                        </td>
                    </tr>
                </table>
                <table>
                    <tr>
                        <td id="TdShape" runat="server" visible="false">
                            <asp:Label ID="LblShape" runat="server" Text="Label"></asp:Label><br />
                            <asp:DropDownList CssClass="dropdown" Width="100px" ID="DDShape" runat="server" AutoPostBack="True"
                                OnSelectedIndexChanged="DDShape_SelectedIndexChanged">
                            </asp:DropDownList>
                            <cc1:ListSearchExtender ID="ListSearchDDShape" runat="server" TargetControlID="DDShape"
                                ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                            </cc1:ListSearchExtender>
                        </td>
                        <td id="TdSize" runat="server" visible="false">
                            <asp:Label ID="LblSize" runat="server" Text="Label"></asp:Label>
                            <asp:CheckBox ID="ChkFt" runat="server" AutoPostBack="True" Text="Ft" OnCheckedChanged="ChkFt_CheckedChanged"
                                Visible="false" />
                            <br />
                            <asp:DropDownList CssClass="dropdown" Width="100px" ID="DDSize" runat="server" AutoPostBack="True"
                                OnSelectedIndexChanged="DDSize_SelectedIndexChanged">
                            </asp:DropDownList>
                            <cc1:ListSearchExtender ID="ListSearchDDSize" runat="server" TargetControlID="DDSize"
                                ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                            </cc1:ListSearchExtender>
                        </td>
                        <td class="tdstyle">
                            Unit<br />
                            <asp:DropDownList CssClass="dropdown" Width="90px" ID="DDUnit" runat="server">
                            </asp:DropDownList>
                            <cc1:ListSearchExtender ID="ListSearchDDUnit" runat="server" TargetControlID="DDUnit"
                                ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                            </cc1:ListSearchExtender>
                        </td>
                        <td>
                            Godown Name<br />
                            <asp:DropDownList CssClass="dropdown" Width="150px" ID="DDGodownName" runat="server"
                                AutoPostBack="True" OnSelectedIndexChanged="DDGodownName_SelectedIndexChanged">
                            </asp:DropDownList>
                            <cc1:ListSearchExtender ID="ListSearchExtender2" runat="server" TargetControlID="DDGodownName"
                                ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                            </cc1:ListSearchExtender>
                        </td>
                        <td>
                            Lot No<br />
                            <asp:DropDownList CssClass="dropdown" Width="150px" ID="DDLotNo" runat="server" AutoPostBack="True"
                                OnSelectedIndexChanged="DDLotNo_SelectedIndexChanged">
                            </asp:DropDownList>
                            <cc1:ListSearchExtender ID="ListSearchExtender4" runat="server" TargetControlID="DDLotNo"
                                ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                            </cc1:ListSearchExtender>
                        </td>
                        <td id="TdStock" runat="server" class="tdstyle">
                            Stock<br />
                            <asp:TextBox ID="TxtStock" CssClass="textb" runat="server" Width="100px" ReadOnly="true"></asp:TextBox>
                        </td>
                        <td runat="server" class="tdstyle">
                            Issue Qty<br />
                            <asp:TextBox ID="TxtIssueQty" CssClass="textb" runat="server" Width="100px" onchange="return CheckQty();"
                                onkeypress="return isNumberKey(event);"></asp:TextBox>
                        </td>
                    </tr>
                </table>
                <table>
                    <tr>
                        <td align="right" style="width: 900px">
                            <asp:Label ID="LblMessage" runat="server" ForeColor="Red"></asp:Label>
                            <asp:Button ID="BtnNew" runat="server" Text="New" CssClass="buttonnorm" OnClientClick="return NewForm();" />
                            &nbsp;<asp:Button ID="BtnSave" runat="server" Text="Save" CssClass="buttonnorm" OnClick="BtnSave_Click"
                                OnClientClick="return ValidateSave();" />
                            &nbsp;<asp:Button ID="BtnPreview" runat="server" Text="Preview" CssClass="buttonnorm"
                                OnClick="BtnPreview_Click" />
                            &nbsp;<asp:Button ID="BtnClose" runat="server" Text="Close" CssClass="buttonnorm"
                                OnClientClick="return closeform();" />
                        </td>
                    </tr>
                </table>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
