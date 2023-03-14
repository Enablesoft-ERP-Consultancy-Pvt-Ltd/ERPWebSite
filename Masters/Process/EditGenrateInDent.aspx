<%@ Page Title="Edit Genrate Indent" Language="C#" AutoEventWireup="true" MasterPageFile="~/ERPmaster.master"
    CodeFile="EditGenrateInDent.aspx.cs" EnableEventValidation="false" Inherits="GenrateInDent"
    ViewStateMode="Enabled" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Page" ContentPlaceHolderID="CPH_Form" runat="server">
    <script src="../../Scripts/JScript.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery-1.4.1.js" type="text/javascript"></script>
    <script type="text/javascript" src="../../Scripts/FixFocus2.js"></script>
    <script type="text/javascript" src="../../Scripts/thickbox.js"></script>
    <link href="../../Styles/vijay.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript">
        function CloseForm() {
            window.location.href = "../../main.aspx";
        }
        function Preview() {
            window.open("../../ReportViewer.aspx", "GenrateIndentReport");
        }
        function AddDyeingRat() {
            var a3 = document.getElementById('CPH_Form_TxtFinishedid').value;
            window.open('AddDyeingRat.aspx?' + a3);
        }
        function ClickNew() {
            window.location.href = "EditGenrateInDent.aspx";
        }
        function CheckQty(Condition) {
            var VarQty = 0;
            var VarCanQty = 0;
            if (Condition == "1") {
                if (document.getElementById('CPH_Form_txtQty').value != "") {
                    VarQty = document.getElementById('CPH_Form_txtQty').value;
                }
                if (document.getElementById('CPH_Form_txtCanQty').value != "") {
                    VarCanQty = document.getElementById('CPH_Form_txtCanQty').value;
                }
                if (VarCanQty != 0) {

                    if (parseFloat(VarCanQty) < parseFloat(VarQty)) {
                        alert("Issue Qty Can not be greater than CancelQty ....!");
                        document.getElementById('CPH_Form_txtCanQty').value = "0";
                        document.getElementById("CPH_Form_txtCanQty").focus();
                        // return false;
                    }
                }
            }
            else {
                if (document.getElementById('CPH_Form_txtQty').value != "") {
                    VarQty = document.getElementById('CPH_Form_txtQty').value;
                }
                if (document.getElementById('CPH_Form_txtCanQty').value != "") {
                    VarCanQty = document.getElementById('CPH_Form_txtCanQty').value;
                }
                if (parseFloat(VarCanQty) > parseFloat(VarQty)) {
                    alert("Cancel Qty Can not be greater than IssueQty ....!");
                    document.getElementById('CPH_Form_txtCanQty').value = "0";
                    document.getElementById("CPH_Form_txtCanQty").focus();
                    return false;
                }
            }
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
        function ontextchanged() {
            var dtDate = new Date(document.getElementById("CPH_Form_TxtDate").value).toDateString();
            var dtDate1 = new Date(document.getElementById("CPH_Form_TxtReqDate").value).toDateString();
            if (dtDate > dtDate1) {
                alert("Required Date Cann't Be Shorter Than Order Date \n");
                document.getElementById("CPH_Form_TxtReqDate").value = document.getElementById("CPH_Form_TxtDate").value;
                return false;
            }
            else {
                return true;
            }

        }
        function checkDate() {
            // define date string to test
            var SDate = new date(document.getElementById(TxtOrderDate).value).toDateString();
            var EDate = new date(document.getElementById(TxtDeliveryDate).value).toDateString();
            var alertReason1 = 'Dispatch Date must be greater than or equal to Order Date.'
            var alertReason2 = 'Dispatch Date can not be less than Order Date.';
            var endDate = new Date(EDate);
            var startDate = new Date(SDate);
            if (SDate != '' && EDate != '' && startDate > endDate) {
                alert(alertReason1);
                document.getElementById(DispatchDate).value = "";
                return false;
            }
            else if (SDate == '') {
                alert("Please enter Start Date");
                return false;
            }
            else if (EDate == '') {
                alert("Please enter End Date");
                return false;
            }
        }
        function validate() {
            if (document.getElementById("<%=DDCompanyName.ClientID %>").value <= "0") {
                alert("Plz select Comapny Name");
                return false;
            }

            if (document.getElementById('CPH_Form_DDPartyName') != null) {
                if (document.getElementById('CPH_Form_DDPartyName').options.length == 0) {
                    alert("PartyName must have a value....!");
                    document.getElementById("CPH_Form_DDPartyName").focus();
                    return false;
                }
                else if (document.getElementById('CPH_Form_DDPartyName').options[document.getElementById('CPH_Form_DDPartyName').selectedIndex].value == 0) {
                    alert("Please select PartyName....!");
                    document.getElementById("CPH_Form_DDPartyName").focus();
                    return false;
                }
            }
            if (document.getElementById('CPH_Form_DDProcessName') != null) {
                if (document.getElementById('CPH_Form_DDProcessName').options.length == 0) {
                    alert("Process Name  must have a value....!");
                    document.getElementById("CPH_Form_DDProcessName").focus();
                    return false;
                }
                else if (document.getElementById('CPH_Form_DDProcessName').options[document.getElementById('CPH_Form_DDProcessName').selectedIndex].value == 0) {
                    alert("Please select ProcessName....!");
                    document.getElementById("CPH_Form_DDProcessName").focus();
                    return false;
                }
            }
            if (document.getElementById('CPH_Form_DDProcessProgramNo') != null) {
                if (document.getElementById('CPH_Form_DDProcessProgramNo').options.length == 0) {
                    alert("PPNo  must have a value....!");
                    document.getElementById("CPH_Form_DDProcessProgramNo").focus();
                    return false;
                }
                else if (document.getElementById('CPH_Form_DDProcessProgramNo').options[document.getElementById('CPH_Form_DDProcessProgramNo').selectedIndex].value == 0) {
                    alert("Please select PPNo....!");
                    document.getElementById("CPH_Form_DDProcessProgramNo").focus();
                    return false;
                }
            }
            if (document.getElementById('CPH_Form_DDIndentNo') != null) {
                if (document.getElementById('CPH_Form_DDIndentNo').options.length == 0) {
                    alert("Indent No  must have a value....!");
                    document.getElementById("CPH_Form_DDIndentNo").focus();
                    return false;
                }
                else if (document.getElementById('CPH_Form_DDIndentNo').options[document.getElementById('CPH_Form_DDIndentNo').selectedIndex].value == 0) {
                    alert("Please select IndentNo....!");
                    document.getElementById("CPH_Form_DDIndentNo").focus();
                    return false;
                }
            }
            if (document.getElementById('CPH_Form_ddllotno') != null) {
                if (document.getElementById('CPH_Form_ddllotno').options.length == 0) {
                    alert("Lot No.  must have a value....!");
                    document.getElementById("CPH_Form_ddllotno").focus();
                    return false;
                }
                else if (document.getElementById('CPH_Form_ddllotno').options[document.getElementById('CPH_Form_ddllotno').selectedIndex].value == 0) {
                    alert("Please select Lot No....!");
                    document.getElementById("CPH_Form_ddllotno").focus();
                    return false;
                }
            }
            //            if (document.getElementById("<%=TDTagNo.ClientID %>")) {
            //                if (document.getElementById("<%=txtTagno.ClientID %>").value == "") {
            //                    alert("Please Fill Tag No.");
            //                    document.getElementById("<%=txtTagno.ClientID %>").focus();
            //                    return false;
            //                }
            //            }

            return confirm('Do You Want To Save?')
        }
    </script>
    <script language="JavaScript" type="text/javascript">

        function confirmSubmit() {

            var returnvalue;
            var agree = confirm("Are you sure you wish to continue with zero Loss Percentage value?");
            if (agree) {
                document.getElementById('<%=hnRetunTypeValue.ClientID %>').value = "1";
                returnvalue = 1;
                //return true;
            }
            else {
                document.getElementById('<%=hnRetunTypeValue.ClientID %>').value = "0";
                returnvalue = 0;
                //document.getElementById('<%=BtnSave.ClientID %>').style.display = 'none';                           
                //return false;
            }
        }

    </script>
    <asp:UpdatePanel ID="up1" runat="server">
        <ContentTemplate>
            <div>
                <fieldset>
                    <legend>
                        <asp:Label ID="Label30" Text="Master Details" CssClass="labelbold" ForeColor="Red"
                            runat="server" />
                    </legend>
                    <table>
                        <tr>
                            <td id="Tdcomplete" runat="server" visible="false" colspan="2">
                                <asp:CheckBox ID="chkcomplete" runat="server" Text="Check For Complete" CssClass="checkboxbold" />
                            </td>
                            <td>
                            </td>
                            <td colspan="2" align="center" id="TDChkForOrder" runat="server" visible="false">
                                <asp:CheckBox ID="ChkForOrder" runat="server" Text="Check For OrderWise" ForeColor="Red"
                                    AutoPostBack="True" CssClass="checkboxbold" OnCheckedChanged="ChkEditOrder_CheckedChanged" />
                            </td>
                            <td>
                            </td>
                            <td id="TDReDyeing" runat="server" visible="false">
                                <asp:CheckBox ID="chkredyeing" runat="server" Text="Check For RE-DYEING" ForeColor="Red"
                                    AutoPostBack="true" CssClass="checkboxbold" OnCheckedChanged="chkredyeing_CheckedChanged" />
                            </td>
                            <td colspan="4" align="center" id="TDLblReqDate" runat="server" visible="false">
                                <asp:Label ID="lblfinalDate" runat="server" Text="Stock At PH Date:" CssClass="labelbold"
                                    ForeColor="Red"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td class="tdstyle">
                                <asp:Label Text="Indent No" runat="server" ID="labelsd" CssClass="labelbold" />
                                <br />
                                <asp:TextBox ID="txtidnt" runat="server" OnTextChanged="txtidnt_TextChanged" AutoPostBack="True"
                                    CssClass="textb" Width="100px"></asp:TextBox>
                            </td>
                            <td class="tdstyle">
                                <asp:Label Text="CompanyName" runat="server" ID="label3" CssClass="labelbold" />
                                <br />
                                <asp:DropDownList CssClass="dropdown" ID="DDCompanyName" Width="150" runat="server"
                                    AutoPostBack="true" OnSelectedIndexChanged="DDCompanyName_SelectedIndexChanged">
                                </asp:DropDownList>
                                <cc1:ListSearchExtender ID="ListSearchExtender4" runat="server" TargetControlID="DDCompanyName"
                                    ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                                </cc1:ListSearchExtender>
                            </td>
                            <td class="tdstyle">
                                <asp:Label ID="Label33" runat="server" Text="BranchName" CssClass="labelbold"></asp:Label>
                                <br />
                                <asp:DropDownList CssClass="dropdown" ID="DDBranchName" Width="150" runat="server">
                                </asp:DropDownList>
                            </td>
                            <td id="TDCustCode" visible="false" runat="server" class="tdstyle">
                                <asp:Label Text="CustomerCode" runat="server" ID="label4" CssClass="labelbold" />
                                <br />
                                <asp:DropDownList ID="DDCustomerCode" runat="server" Width="150px" CssClass="dropdown"
                                    AutoPostBack="True" OnSelectedIndexChanged="DDCustomerCode_SelectedIndexChanged">
                                </asp:DropDownList>
                                <cc1:ListSearchExtender ID="ListSearchExtender17" runat="server" TargetControlID="DDCustomerCode"
                                    ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                                </cc1:ListSearchExtender>
                            </td>
                            <td id="TDOrderNo" runat="server" visible="false" class="tdstyle">
                                <asp:Label Text=" OrderNo" runat="server" ID="label5" CssClass="labelbold" />
                                <br />
                                <asp:DropDownList CssClass="dropdown" ID="DDOrderNo" Width="110" AutoPostBack="true"
                                    runat="server" OnSelectedIndexChanged="DDOrderNo_SelectedIndexChanged">
                                </asp:DropDownList>
                                <cc1:ListSearchExtender ID="ListSearchExtender18" runat="server" TargetControlID="DDOrderNo"
                                    ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                                </cc1:ListSearchExtender>
                            </td>
                            <td class="tdstyle">
                                <asp:Label Text="ProcessName" runat="server" ID="label7" CssClass="labelbold" />
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" ControlToValidate="DDProcessName"
                                    ErrorMessage="please Select Process" ForeColor="Red" SetFocusOnError="true" ValidationGroup="f1">*</asp:RequiredFieldValidator>
                                <br />
                                <asp:DropDownList CssClass="dropdown" ID="DDProcessName" Width="150" AutoPostBack="true"
                                    runat="server" OnSelectedIndexChanged="DDProcessName_SelectedIndexChanged">
                                </asp:DropDownList>
                                <cc1:ListSearchExtender ID="ListSearchExtender2" runat="server" TargetControlID="DDProcessName"
                                    ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                                </cc1:ListSearchExtender>
                            </td>
                            <td class="tdstyle">
                                <asp:Label Text="   PartyName" runat="server" ID="label6" CssClass="labelbold" />
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator8" runat="server" ControlToValidate="DDPartyName"
                                    ErrorMessage="please Select PartyName" ForeColor="Red" SetFocusOnError="true"
                                    ValidationGroup="f1">*</asp:RequiredFieldValidator>
                                <br />
                                <asp:DropDownList CssClass="dropdown" ID="DDPartyName" Width="150" AutoPostBack="true"
                                    runat="server" OnSelectedIndexChanged="DDPartyName_SelectedIndexChanged">
                                </asp:DropDownList>
                                <cc1:ListSearchExtender ID="ListSearchExtender1" runat="server" TargetControlID="DDPartyName"
                                    ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                                </cc1:ListSearchExtender>
                            </td>
                            <td id="TDppno" runat="server" class="tdstyle">
                                <asp:Label Text="PPNo" runat="server" ID="label8" CssClass="labelbold" />
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
                            <td id="tdindent" runat="server" class="tdstyle">
                                <asp:Label Text="Indent No." runat="server" ID="label9" CssClass="labelbold" />
                                <br />
                                <asp:DropDownList ID="DDIndentNo" runat="server" AutoPostBack="true" CssClass="dropdown"
                                    Width="110" OnSelectedIndexChanged="DDIndentNo_SelectedIndexChanged">
                                </asp:DropDownList>
                                <cc1:ListSearchExtender ID="ListSearchExtender5" runat="server" TargetControlID="DDIndentNo"
                                    ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                                </cc1:ListSearchExtender>
                            </td>
                            <td class="tdstyle">
                                <asp:Label Text=" Date" runat="server" ID="label10" CssClass="labelbold" />
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="TxtDate"
                                    ErrorMessage="please Enter Date" ForeColor="Red" SetFocusOnError="true" ValidationGroup="f1">*</asp:RequiredFieldValidator>
                                <br />
                                <asp:TextBox CssClass="textb" ID="TxtDate" runat="server" Width="100px" BackColor="#7b96bb "
                                    onchange="javascript: ontextchanged();"></asp:TextBox>
                                <asp:CalendarExtender ID="CalendarExtender1" runat="server" Format="dd-MMM-yyyy"
                                    TargetControlID="TxtDate">
                                </asp:CalendarExtender>
                            </td>
                            <td class="tdstyle">
                                <asp:Label Text=" Req.Date" runat="server" ID="label11" CssClass="labelbold" />
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ControlToValidate="TxtDate"
                                    ErrorMessage="please Enter Date" ForeColor="Red" SetFocusOnError="true" ValidationGroup="f1">*</asp:RequiredFieldValidator>
                                <br />
                                <asp:TextBox CssClass="textb" ID="TxtReqDate" runat="server" Width="100px" onchange="javascript: ontextchanged();"
                                    BackColor="#7b96bb " OnTextChanged="TxtReqDate_TextChanged" AutoPostBack="true"></asp:TextBox>
                                <asp:CalendarExtender ID="CalendarExtender2" runat="server" Format="dd-MMM-yyyy"
                                    TargetControlID="TxtReqDate">
                                </asp:CalendarExtender>
                            </td>
                            <td id="q3" runat="server" visible="false" class="tdstyle">
                                <asp:Label Text=" ProdCode" runat="server" ID="label12" CssClass="labelbold" />
                                <br />
                                <asp:TextBox CssClass="textb" ID="TxtProdCode" Width="100px" runat="server" OnTextChanged="TxtProdCode_TextChanged"
                                    AutoPostBack="true"></asp:TextBox>
                            </td>
                        </tr>
                    </table>
                </fieldset>
                <fieldset>
                    <legend>
                        <asp:Label ID="Label31" Text="Item Details" CssClass="labelbold" ForeColor="Red"
                            runat="server" />
                    </legend>
                    <table>
                        <tr>
                            <td>
                                <asp:Label ID="LblCategory" class="tdstyle" runat="server" Text="" CssClass="labelbold"></asp:Label>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="DDCategory"
                                    ErrorMessage="please Select Category" ForeColor="Red" SetFocusOnError="true"
                                    ValidationGroup="f1">*</asp:RequiredFieldValidator>
                                <br />
                                <asp:DropDownList CssClass="dropdown" ID="DDCategory" Width="110px" AutoPostBack="true"
                                    runat="server" OnSelectedIndexChanged="DDCategory_SelectedIndexChanged">
                                </asp:DropDownList>
                                <cc1:ListSearchExtender ID="ListSearchExtender6" runat="server" TargetControlID="DDCategory"
                                    ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                                </cc1:ListSearchExtender>
                            </td>
                            <td>
                                <asp:Label ID="LblItemName" class="tdstyle" runat="server" Text="Label" CssClass="labelbold"></asp:Label>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ControlToValidate="DDItem"
                                    ErrorMessage="please Select Item" ForeColor="Red" SetFocusOnError="true" ValidationGroup="f1">*</asp:RequiredFieldValidator>
                                <br />
                                <asp:DropDownList CssClass="dropdown" ID="DDItem" Width="110px" AutoPostBack="true"
                                    runat="server" OnSelectedIndexChanged="DDItem_SelectedIndexChanged">
                                </asp:DropDownList>
                                <cc1:ListSearchExtender ID="ListSearchExtender7" runat="server" TargetControlID="DDItem"
                                    ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                                </cc1:ListSearchExtender>
                            </td>
                            <td id="TdQuality" runat="server" visible="false">
                                <asp:Label ID="LblQuality" class="tdstyle" runat="server" Text="Label" CssClass="labelbold"></asp:Label><br />
                                <asp:DropDownList CssClass="dropdown" ID="DDQuality" Width="150px" AutoPostBack="true"
                                    runat="server" OnSelectedIndexChanged="DDQuality_SelectedIndexChanged">
                                </asp:DropDownList>
                                <cc1:ListSearchExtender ID="ListSearchExtender8" runat="server" TargetControlID="DDQuality"
                                    ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                                </cc1:ListSearchExtender>
                            </td>
                            <td id="TdDesign" runat="server" visible="false">
                                <asp:Label ID="LblDesign" runat="server" class="tdstyle" Text="Label" CssClass="labelbold"></asp:Label><br />
                                <asp:DropDownList CssClass="dropdown" Width="110px" ID="DDDesign" runat="server"
                                    AutoPostBack="True" OnSelectedIndexChanged="DDDesign_SelectedIndexChanged">
                                </asp:DropDownList>
                                <cc1:ListSearchExtender ID="ListSearchExtender9" runat="server" TargetControlID="DDDesign"
                                    ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                                </cc1:ListSearchExtender>
                            </td>
                            <td id="TdColor" runat="server" visible="false">
                                <asp:Label ID="LblColor" runat="server" class="tdstyle" Text="Label" CssClass="labelbold"></asp:Label><br />
                                <asp:DropDownList CssClass="dropdown" Width="110px" ID="DDColor" runat="server" AutoPostBack="True"
                                    OnSelectedIndexChanged="DDColor_SelectedIndexChanged">
                                </asp:DropDownList>
                                <cc1:ListSearchExtender ID="ListSearchExtender10" runat="server" TargetControlID="DDColor"
                                    ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                                </cc1:ListSearchExtender>
                            </td>
                            <td id="TdColorShade" runat="server" visible="false">
                                <asp:Label ID="LblColorShade" runat="server" CssClass="labelbold" Text="Label"></asp:Label><br />
                                <asp:DropDownList CssClass="dropdown" Width="130px" ID="DDColorShade" runat="server"
                                    AutoPostBack="True" OnSelectedIndexChanged="DDColorShade_SelectedIndexChanged">
                                </asp:DropDownList>
                                <cc1:ListSearchExtender ID="ListSearchExtender11" runat="server" TargetControlID="DDColorShade"
                                    ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                                </cc1:ListSearchExtender>
                            </td>
                            <td id="TDISSUESHADE" runat="server" visible="false">
                                <asp:Label ID="Label29" runat="server" Text="ISSUE SHADE" CssClass="labelbold"></asp:Label><br />
                                <asp:DropDownList CssClass="dropdown" Width="150" ID="DDISSUESHADE" AutoPostBack="true"
                                    runat="server" OnSelectedIndexChanged="DDISSUESHADE_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                            <td id="TdShape" runat="server" visible="false">
                                <asp:Label ID="LblShape" runat="server" Text="Label" CssClass="labelbold"></asp:Label><br />
                                <asp:DropDownList CssClass="dropdown" Width="100px" ID="DDShape" runat="server" AutoPostBack="True"
                                    OnSelectedIndexChanged="DDShape_SelectedIndexChanged">
                                </asp:DropDownList>
                                <cc1:ListSearchExtender ID="ListSearchExtender12" runat="server" TargetControlID="DDShape"
                                    ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                                </cc1:ListSearchExtender>
                            </td>
                            <td id="TdSize" runat="server" visible="false" class="tdstyle">
                                <asp:Label ID="LblSize" runat="server" Text="Label" CssClass="labelbold"></asp:Label>
                                <%--<asp:CheckBox ID="ChkFt" runat="server" AutoPostBack="True" Text="Ft" OnCheckedChanged="ChkFt_CheckedChanged"
                                Visible="false" />--%>
                                <asp:DropDownList CssClass="dropdown" Width="50" ID="DDsizetype" runat="server" AutoPostBack="True"
                                    OnSelectedIndexChanged="DDsizetype_SelectedIndexChanged">
                                    <asp:ListItem Value="0" Selected="True">Ft</asp:ListItem>
                                    <asp:ListItem Value="1">MTR</asp:ListItem>
                                    <asp:ListItem Value="2">Inch</asp:ListItem>
                                </asp:DropDownList>
                                <br />
                                <asp:DropDownList ID="DDSize" runat="server" AutoPostBack="True" CssClass="dropdown"
                                    OnSelectedIndexChanged="DDSize_SelectedIndexChanged" Width="100px">
                                </asp:DropDownList>
                                <cc1:ListSearchExtender ID="ListSearchExtender13" runat="server" TargetControlID="DDSize"
                                    ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                                </cc1:ListSearchExtender>
                            </td>
                            <td class="tdstyle">
                                <asp:Label ID="Label13" runat="server" Text="  Unit" CssClass="labelbold"></asp:Label>
                                <br />
                                <asp:DropDownList ID="ddUnit" runat="server" AutoPostBack="True" CssClass="dropdown"
                                    Width="100px">
                                </asp:DropDownList>
                                <cc1:ListSearchExtender ID="ListSearchExtender16" runat="server" TargetControlID="ddUnit"
                                    ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                                </cc1:ListSearchExtender>
                            </td>
                        </tr>
                        <tr>
                            <td class="tdstyle" id="TDDyeingMatch" runat="server" visible="false">
                                <asp:Label ID="lblmatch" runat="server" Text="Dyeing Match" CssClass="labelbold"></asp:Label>
                                <br />
                                <asp:DropDownList ID="DDDyeingMatch" runat="server" CssClass="dropdown" Enabled="false">
                                    <asp:ListItem Value="Cut">Cut</asp:ListItem>
                                    <asp:ListItem Value="Side">Side</asp:ListItem>
                                    <asp:ListItem Value="Cut/Side">Cut/Side</asp:ListItem>
                                    <asp:ListItem Value="N/A">N/A</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <td class="tdstyle" id="TDDyeing" runat="server" visible="false">
                                <asp:Label ID="Label26" runat="server" Text="Dyeing" CssClass="labelbold"></asp:Label><br />
                                <asp:DropDownList ID="DDDyeing" runat="server" CssClass="dropdown" Enabled="false">
                                    <asp:ListItem Value="Boarder">Boarder</asp:ListItem>
                                    <asp:ListItem Value="Ground">Ground</asp:ListItem>
                                    <asp:ListItem Value="Ascent">Ascent</asp:ListItem>
                                    <asp:ListItem Value="Ground/Boarder">Ground/Boarder</asp:ListItem>
                                    <asp:ListItem Value="N/A">N/A</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <td class="tdstyle" id="TDDyingType" runat="server" visible="false">
                                <asp:Label ID="Label27" runat="server" Text="Dyeing Type" CssClass="labelbold"></asp:Label><br />
                                <asp:DropDownList ID="DDDyingType" runat="server" CssClass="dropdown" Enabled="false">
                                    <asp:ListItem Value="Shaded">Shaded</asp:ListItem>
                                    <asp:ListItem Value="Natural">Natural</asp:ListItem>
                                    <asp:ListItem Value="Plain">Plain</asp:ListItem>
                                    <asp:ListItem Value="Gabbeh">Gabbeh</asp:ListItem>
                                    <asp:ListItem Value="Multi Dyeing">Multi Dyeing</asp:ListItem>
                                    <asp:ListItem Value="N/A">N/A</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <td class="tdstyle" id="TDitemremark" runat="server" colspan="5">
                                <asp:Label ID="Label25" runat="server" Text="Item Remark" CssClass="labelbold"></asp:Label>
                                <br />
                                <asp:TextBox ID="txtitemremark" runat="server" Width="90%" Height="33px" CssClass="textb"
                                    TextMode="MultiLine"></asp:TextBox>
                            </td>
                        </tr>
                    </table>
                </fieldset>
                <fieldset>
                    <legend>
                        <asp:Label Text="..." CssClass="labelbold" ForeColor="Red" runat="server" />
                    </legend>
                    <table>
                        <tr>
                            <td id="TDCaltype" runat="server" class="tdstyle">
                                <asp:Label ID="Label14" runat="server" Text="  CalType" CssClass="labelbold"></asp:Label>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator10" runat="server" ControlToValidate="TxtQty"
                                    ErrorMessage="please Select CalType" ForeColor="Red" SetFocusOnError="true" ValidationGroup="f1">*</asp:RequiredFieldValidator>
                                <br />
                                <asp:DropDownList CssClass="dropdown" ID="DDcaltype" runat="server" Width="110px"
                                    AutoPostBack="True" OnSelectedIndexChanged="DDcaltype_SelectedIndexChanged">
                                </asp:DropDownList>
                                <cc1:ListSearchExtender ID="ListSearchExtender14" runat="server" TargetControlID="DDcaltype"
                                    ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                                </cc1:ListSearchExtender>
                            </td>
                            <td id="TDGodownName" runat="server" class="tdstyle" visible="false">
                                <asp:Label ID="Label34" runat="server" Text=" Godown Name" CssClass="labelbold"></asp:Label>
                                <br />
                                <asp:DropDownList ID="DDGodownName" CssClass="dropdown" Width="130px" runat="server"
                                    AutoPostBack="True" OnSelectedIndexChanged="DDGodownName_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                            <td id="TDLotNo" runat="server" class="tdstyle">
                                <asp:Label ID="Label15" runat="server" Text="  LotNo." CssClass="labelbold"></asp:Label>
                                <br />
                                <asp:DropDownList ID="ddllotno" CssClass="dropdown" Width="110px" runat="server"
                                    AutoPostBack="True" OnSelectedIndexChanged="ddllotno_SelectedIndexChanged">
                                </asp:DropDownList>
                                <cc1:ListSearchExtender ID="ListSearchExtender15" runat="server" TargetControlID="ddllotno"
                                    ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                                </cc1:ListSearchExtender>
                            </td>
                            <td id="TDTagNo" runat="server" visible="false">
                                <asp:Label ID="Label28" runat="server" Text="Tag No." CssClass="labelbold"></asp:Label>
                                <br />
                                <asp:TextBox ID="txtTagno" CssClass="textb" Width="120px" runat="server" />
                            </td>
                            <td id="TDDDTagNo" runat="server" visible="false">
                                <table>
                                    <tr>
                                        <td>
                                            <asp:Label ID="Label35" runat="server" Text="UCN No." CssClass="labelbold"></asp:Label>
                                            <br />
                                            <asp:DropDownList ID="DDTagNo" CssClass="dropdown" Width="130px" runat="server" AutoPostBack="True"
                                                OnSelectedIndexChanged="DDTagNo_SelectedIndexChanged">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                            <td id="TDStockQty" runat="server" class="tdstyle">
                                <asp:Label ID="Label16" runat="server" Text="  Stock Qty" CssClass="labelbold"></asp:Label>
                                <br />
                                <asp:TextBox ID="txtstock" CssClass="textb" Width="80px" runat="server" Enabled="false"
                                    onkeydown="return (event.keyCode!=13);"></asp:TextBox>
                            </td>
                            <td id="TDLoss" runat="server" class="tdstyle">
                                <asp:Label ID="Label17" runat="server" Text=" Loss%" CssClass="labelbold"></asp:Label>
                                <br />
                                <asp:TextBox ID="TxtLoss" CssClass="textb" Width="60px" runat="server" Enabled="false"
                                    onkeydown="return (event.keyCode!=13);"></asp:TextBox>
                            </td>
                            <td id="TDTotalQty" runat="server" class="tdstyle">
                                <asp:Label ID="Label18" runat="server" Text="TotalQty." CssClass="labelbold"></asp:Label>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtTotalQty"
                                    ErrorMessage="TotalQty can not be null..." ForeColor="Red" SetFocusOnError="true"
                                    ValidationGroup="f1">*</asp:RequiredFieldValidator>
                                <br />
                                <asp:TextBox CssClass="textb" Enabled="false" ID="txtTotalQty" Width="80px" runat="server"
                                    onkeydown="return (event.keyCode!=13);"></asp:TextBox>
                            </td>
                            <td id="TDPreQty" runat="server" class="tdstyle">
                                <asp:Label ID="Label19" runat="server" Text=" PreQty." CssClass="labelbold"></asp:Label>
                                <br />
                                <asp:TextBox CssClass="textb" Enabled="false" ID="TxtPreQty" Width="80px" runat="server"
                                    onkeydown="return (event.keyCode!=13);"></asp:TextBox>
                            </td>
                            <td class="tdstyle">
                                <asp:Label ID="Label20" runat="server" Text="Qty" CssClass="labelbold"></asp:Label>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator9" runat="server" ControlToValidate="txtQty"
                                    ErrorMessage="please Enter Qty" ForeColor="Red" SetFocusOnError="true" ValidationGroup="f1">*</asp:RequiredFieldValidator>
                                <br />
                                <asp:TextBox CssClass="textb" ID="txtQty" Width="80px" runat="server" AutoPostBack="True"
                                    onkeypress="return isNumberKey(event);" BackColor="#7b96bb " onchange="javascript:CheckQty(1);"
                                    OnTextChanged="txtQty_TextChanged"></asp:TextBox>
                                <asp:Label ID="LblKg" runat="server" Text=" kg." Visible="false"></asp:Label>
                            </td>
                            <td class="tdstyle">
                                <asp:Label ID="Label21" runat="server" Text=" Extra Qty" CssClass="labelbold"></asp:Label>
                                <br />
                                <asp:TextBox CssClass="textb" ID="txtextraQty" Width="70px" runat="server" onkeydown="return (event.keyCode!=13);"
                                    onkeypress="return isNumberKey(event);" BackColor="#7b96bb " AutoPostBack="true"
                                    OnTextChanged="txtextraQty_TextChanged"></asp:TextBox>
                                <asp:Label ID="Label2" runat="server" Text=" kg." Visible="false"></asp:Label>
                            </td>
                            <td class="tdstyle">
                                <asp:Label ID="Label22" runat="server" Text="Cancel Qty" CssClass="labelbold"></asp:Label>
                                <br />
                                <asp:TextBox CssClass="textb" ID="txtCanQty" Width="70px" runat="server" onkeypress="return isNumberKey(event);"
                                    onchange="return CheckQty(2);" BackColor="#7b96bb " OnTextChanged="txtCanQty_TextChanged"
                                    AutoPostBack="true"></asp:TextBox>
                                <asp:Label ID="Label1" runat="server" Text=" kg." Visible="false"></asp:Label>
                            </td>
                            <td class="tdstyle" runat="server" id="tdrate">
                                <table>
                                    <tr>
                                        <td>
                                            <asp:Label ID="Label23" runat="server" Text=" Rate" CssClass="labelbold"></asp:Label>
                                            <br />
                                            <asp:TextBox CssClass="textb" ID="TxtRate" Width="50px" runat="server" Enabled="False"
                                                onkeydown="return (event.keyCode!=13);" AutoPostBack="true" OnTextChanged="TxtRate_TextChanged"></asp:TextBox>
                                        </td>
                                        <td id="TDBtnAddRate" runat="server" visible="true" valign="bottom">
                                            <br />
                                            <asp:Button CssClass="buttonnorm" ID="BtnAddRate" OnClientClick="return AddDyeingRat();"
                                                runat="server" Text="+" ToolTip="Add Rate" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                            <td>
                                <asp:Label ID="Label32" Text="Amount" CssClass="labelbold" runat="server" />
                                <br />
                                <asp:TextBox ID="txtamt" CssClass="textb" Width="80px" runat="server" Enabled="false"
                                    BackColor="LightGray" />
                            </td>
                        </tr>
                    </table>
                </fieldset>
                <table style="width: 100%">
                    <tr>
                        <td style="width: 40%" id="TDtxtremarks" runat="server">
                            <asp:Label ID="Label24" runat="server" Text=" Remark" CssClass="labelbold"></asp:Label>
                            <br />
                            <asp:TextBox ID="txtremarks" runat="server" Width="90%" Height="33px" CssClass="textb"
                                TextMode="MultiLine"></asp:TextBox>
                        </td>
                        <td style="width: 60%; text-align: right">
                            <asp:Button CssClass="buttonnorm" ID="BtnEmployeeWisePPDetail" runat="server" Text="PP Detail"
                                OnClick="BtnEmployeeWisePPDetail_Click" Visible="false" />
                            <asp:Button CssClass="buttonnorm" ID="BtnRateUpdate" runat="server" Text="Update Rate"
                                OnClick="BtnRateUpdate_Click" Visible="false" />
                            <asp:CheckBox ID="ChkForWithoutRate" runat="server" Text="Without Rate Print" class="tdstyle"
                                Visible="false" CssClass="checkboxbold" />
                            <asp:Button CssClass="buttonnorm" ID="btnnew" runat="server" Text="New" OnClientClick="return ClickNew();" />
                            <asp:Button ID="BtnSave" runat="server" CssClass="buttonnorm" OnClick="BtnSave_Click"
                                OnClientClick="return validate();" Text="Save" ValidationGroup="f1" />
                            <asp:Button CssClass="buttonnorm preview_width" ID="BtnPreview" Enabled="false" runat="server"
                                Text="Preview" OnClick="BtnPreview_Click" />
                            <asp:Button CssClass="buttonnorm" ID="BtnCancel" OnClientClick="return confirm('Do you want to Cancel Indent?')"
                                runat="server" Text="Cancel Indent" OnClick="BtnCancel_Click" />
                            <asp:Button CssClass="buttonnorm" ID="BtnClose" OnClientClick="return CloseForm();"
                                runat="server" Text="Close" />
                            &nbsp;<asp:Button ID="Btnorder" runat="server" Text="OrderDetail" Visible="false"
                                CssClass="buttonnorm" OnClientClick="return OrderDetail()" />
                        </td>
                    </tr>
                </table>
                <table style="width: 100%">
                    <tr>
                        <td>
                            <asp:Label ID="lblMessage" Font-Bold="true" ForeColor="Red" runat="server" Text=""></asp:Label>
                        </td>
                    </tr>
                </table>
                <table style="width: 100%">
                    <tr>
                        <td colspan="6">
                            <%--<div id="gride" runat="server" style="height: 250px">--%>
                            <div style="width: 100%; max-height: 200px; overflow: auto">
                                <asp:GridView ID="DGIndentDetail" AutoGenerateColumns="False" runat="server" DataKeyNames="IndentDetailId"
                                    OnRowDataBound="DGIndentDetail_RowDataBound" OnSelectedIndexChanged="DGIndentDetail_SelectedIndexChanged"
                                    OnRowDeleting="DGIndentDetail_RowDeleting" CssClass="grid-views" Width="100%">
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
                                        <asp:BoundField DataField="Quantity" HeaderText="Qty">
                                            <HeaderStyle Width="75px" HorizontalAlign="Center" />
                                            <ItemStyle Width="75px" HorizontalAlign="Center" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="ExtraQty" HeaderText="ExtraQty">
                                            <HeaderStyle Width="75px" HorizontalAlign="Center" />
                                            <ItemStyle Width="75px" HorizontalAlign="Center" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="CancelQty" HeaderText="CancelQty">
                                            <HeaderStyle Width="75px" HorizontalAlign="Center" />
                                            <ItemStyle Width="75px" HorizontalAlign="Center" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="LotNo" HeaderText="LotNo" />
                                        <%--  <asp:BoundField DataField="TagNo" HeaderText="TagNo" />--%>
                                        <asp:TemplateField HeaderText="Tag No.">
                                            <ItemTemplate>
                                                <asp:Label ID="lbltagno" Text='<%#Bind("TagNo") %>' runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField ShowHeader="False">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="LinkButton1" runat="server" CausesValidation="False" CommandName="Delete"
                                                    Text="Del" OnClientClick="return confirm('Do you want to Delete data?')"></asp:LinkButton>
                                            </ItemTemplate>
                                            <HeaderStyle HorizontalAlign="Center" Width="20px" />
                                            <ItemStyle HorizontalAlign="Center" Width="20px" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Update Tag No." Visible="false">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lnkupdatetagno" runat="server" CausesValidation="False" OnClick="lnkupdatetagno_Click"
                                                    Text="Update Tag No." OnClientClick="return confirm('Do you want to Update Tag No.?')"></asp:LinkButton>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:TextBox ID="TxtFinishedid" runat="server" Height="0px" Width="0px" BorderStyle="None"></asp:TextBox>
                            <asp:HiddenField ID="hncomp" runat="server" />
                            <asp:HiddenField ID="hnorderid" runat="server" Visible="false" />
                            <asp:HiddenField ID="HNPENQTY" runat="server" Visible="false" />
                            <asp:HiddenField ID="Hnqty" runat="server" Visible="false" />
                            <asp:HiddenField ID="hnRetunTypeValue" runat="server" />
                        </td>
                    </tr>
                </table>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
