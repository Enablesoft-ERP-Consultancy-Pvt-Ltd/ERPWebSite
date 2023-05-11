<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PurchaseReceive.aspx.cs"
    Title="Purchase Receive" MasterPageFile="~/ERPmaster.master" Inherits="Masters_Purchase_PurchaseReceive" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="page" ContentPlaceHolderID="CPH_Form" runat="server">
    <br />
    <script src="../../Scripts/JScript.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery-1.4.1.js" type="text/javascript"></script>
    <%--<script type="text/javascript" src="../../Scripts/Fixfocus.js"></script>--%>  
    <script type="text/javascript">
        function CloseForm() {
            window.location.href = "../../main.aspx";
        }
        function Preview() {
            window.open("../../ReportViewer.aspx", "PurchaseReceive");
        }
        function NewForm() {
            window.location.href = "PurchaseReceive.aspx";
        }
        function validate() {

            var varcompanyNo = document.getElementById('CPH_Form_hncomp').value

            if (document.getElementById("<%=DDCompanyName.ClientID %>").value <= "0") {
                alert("Pls Select Company Name");
                document.getElementById("<%=DDCompanyName.ClientID %>").focus();
                return false;
            }
            else if (document.getElementById("<%=DDPartyName.ClientID %>").value <= "0") {
                alert("Pls Select Vendor Name");
                document.getElementById("<%=DDPartyName.ClientID %>").focus();
                return false;
            }
            else if (document.getElementById("<%=DDChallanNo.ClientID %>").value <= "0") {
                alert("Pls Select Order No.");
                document.getElementById("<%=DDChallanNo.ClientID %>").focus();
                return false;
            }
            else if (document.getElementById("<%=TxtBillNo.ClientID %>").value == "") {
                alert("Challan No. Cannot Be Blank");
                document.getElementById("<%=TxtBillNo.ClientID %>").focus();
                return false;
            }

            if (varcompanyNo == "22") {

                if (document.getElementById("<%=txtbillno1.ClientID %>").value == "") {
                    alert("Bill No Cannot Be Blank");
                    document.getElementById("<%=txtbillno1.ClientID %>").focus();
                    return false;
                }
                if (document.getElementById("<%=txtBillDate.ClientID %>").value == "") {
                    alert("Bill Date Cannot Be Blank");
                    document.getElementById("<%=txtBillDate.ClientID %>").focus();
                    return false;
                }
                if (document.getElementById("<%=txtbaleno.ClientID %>").value == "") {
                    alert("Bale No Cannot Be Blank");
                    document.getElementById("<%=txtbaleno.ClientID %>").focus();
                    return false;
                }
            }

            if (document.getElementById('CPH_Form_DDCategory').value <= "0") {
                alert("Please Select Category Name....!");
                document.getElementById("CPH_Form_DDCategory").focus();
                return false;
            }
            if (document.getElementById('CPH_Form_DDItem').value <= "0") {
                alert("Please Select Item Name....!");
                document.getElementById("CPH_Form_DDItem").focus();
                return false;
            }
            if (document.getElementById("<%=TdQuality.ClientID %>")) {
                if (document.getElementById('CPH_Form_DDQuality').value <= "0") {
                    alert("Please Select Quality Name....!");
                    document.getElementById("CPH_Form_DDQuality").focus();
                    return false;
                }
            }            
            if (document.getElementById("<%=TdDesign.ClientID %>")) {
                if (document.getElementById('CPH_Form_DDDesign').options[document.getElementById('CPH_Form_DDDesign').selectedIndex].value <= 0) {
                    alert("Please Select Design Name....!");
                    document.getElementById("CPH_Form_DDDesign").focus();
                    return false;
                }
            }
            if (document.getElementById("<%=TdColor.ClientID %>")) {
                if (document.getElementById('CPH_Form_DDColor').options[document.getElementById('CPH_Form_DDColor').selectedIndex].value <= 0) {
                    alert("Please Select Colour Name....!");
                    document.getElementById("CPH_Form_DDColor").focus();
                    return false;
                }
            }
            if (document.getElementById("<%=TdColorShade.ClientID %>")) {
                if (document.getElementById('CPH_Form_DDColorShade').options[document.getElementById('CPH_Form_DDColorShade').selectedIndex].value <= 0) {
                    alert("Please Select Shade Color Name....!");
                    document.getElementById("CPH_Form_DDColorShade").focus();
                    return false;
                }
            }
            if (document.getElementById("<%=TdShape.ClientID %>")) {
                if (document.getElementById('CPH_Form_DDShape').options[document.getElementById('CPH_Form_DDShape').selectedIndex].value <= 0) {
                    alert("Please Select Shape Name....!");
                    document.getElementById("CPH_Form_DDShape").focus();
                    return false;
                }
            }
            if (document.getElementById("<%=TdSize.ClientID %>")) {
                if (document.getElementById('CPH_Form_DDSize').options[document.getElementById('CPH_Form_DDSize').selectedIndex].value <= 0) {
                    alert("Please Select Size Name....!");
                    document.getElementById("CPH_Form_DDSize").focus();
                    return false;
                }
            }
            else if (document.getElementById("<%=DDGodown.ClientID %>").value <= "0") {
                alert("Pls Select Godown Name");
                document.getElementById("<%=DDGodown.ClientID %>").focus();
                return false;
            }
            else if (document.getElementById("<%=TxtQty.ClientID %>").value == "") {
                alert("Receive QTY Cannot Be Blank");
                document.getElementById("<%=TxtQty.ClientID %>").focus()
                return false;
            }
            else if (document.getElementById("<%=TxtQty.ClientID %>").value == "0" || document.getElementById("<%=TxtQty.ClientID %>").value == "") {
                alert("Receive QTY Cannot Be Zero");
                document.getElementById("<%=TxtQty.ClientID %>").focus();
                return false;
            }
            else {
                return confirm('Do You Want To Save?')
            }
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
        function doconfirm() {
            var r = confirm("Do you want to Delete this row..");
            document.getElementById("CPH_Form_HnForDeleteCommand").value = r;
            if (r == true) {
                return true;
            }
            else {
                return false;
            }

        }
        function validateDate() {

            var varcompanyNo = document.getElementById('CPH_Form_hncomp').value
            //6 For ArtIndia
            if (varcompanyNo == "6") {

                var dt = new Date();
                dt.setDate(dt.getDate() - 3);
                var recDate = new Date(document.getElementById('CPH_Form_TxtReceiveDate').value);

                if (recDate < dt) {
                    alert('Date Can not be less than  ' + dt.format('dd-MMM-yyyy'));
                    date = new Date();
                    var month = new Array();
                    month[0] = "Jan";
                    month[1] = "Feb";
                    month[2] = "Mar";
                    month[3] = "Apr";
                    month[4] = "May";
                    month[5] = "Jun";
                    month[6] = "Jul";
                    month[7] = "Aug";
                    month[8] = "Sep";
                    month[9] = "Oct";
                    month[10] = "Nov";
                    month[11] = "Dec";
                    var month1 = month[date.getMonth()];
                    var day = date.getDate();
                    var year = date.getFullYear();
                    document.getElementById('CPH_Form_TxtReceiveDate').value = day + '-' + month1 + '-' + year;
                }
            }
        }
        function showHideTransportDiv(chkTransport) {

            if (chkTransport.checked) {
                document.getElementById('CPH_Form_DivTransPort').style.display = 'block'
            }
            else {
                document.getElementById('CPH_Form_DivTransPort').style.display = 'none'
            }
        }
    </script>
   
    <asp:UpdatePanel ID="up1" runat="server">
        <ContentTemplate>
            <div>
                <fieldset style="margin-left: 10px; width: 789px;">
                    <legend>
                        <asp:Label ID="lblmasterDetail" runat="server" Text="Master Detail" CssClass="labelbold"
                            Font-Bold="true" ForeColor="Red"></asp:Label>
                    </legend>
                    <table>
                        <tr>
                            <td>
                                <asp:CheckBox ID="ChkEditOrder" runat="server" Text=" EDIT ORDER" AutoPostBack="True"
                                    OnCheckedChanged="ChkEditOrder_CheckedChanged" CssClass="checkboxbold" />
                            </td>
                            <td>
                                &nbsp;
                            </td>
                            <td>
                                <asp:Label ID="lblsrno" Text="SR No. Generated." runat="server" CssClass="labelbold"
                                    ForeColor="Red" />
                            </td>
                        </tr>
                    </table>
                    <table>
                        <tr>
                            <td id="tdchalanno" runat="server" visible="false" class="tdstyle">
                                <asp:Label ID="lblChallanNo" runat="server" Text="Challan No" CssClass="labelbold"></asp:Label>
                                <br />
                                <asp:TextBox ID="txtchalan_no" runat="server" Width="100px" AutoPostBack="True" CssClass="textb"
                                    OnTextChanged="Txtchalan_no_TextChanged"></asp:TextBox>
                            </td>
                            <td class="tdstyle">
                                <asp:Label ID="Label5" runat="server" Text="Company Name" CssClass="labelbold"></asp:Label>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="DDCompanyName"
                                    ErrorMessage="please fill Company......." ForeColor="Red" SetFocusOnError="true"
                                    ValidationGroup="f1">*</asp:RequiredFieldValidator>
                                <br />
                                <asp:DropDownList CssClass="dropdown" ID="DDCompanyName" runat="server" OnSelectedIndexChanged="DDCompanyName_SelectedIndexChanged"
                                    AutoPostBack="true" Width="170px">
                                </asp:DropDownList>
                            </td>
                            <td id="TDBranchName" runat="server" class="tdstyle">
                                <asp:Label ID="Label64" runat="server" Text="Branch Name" CssClass="labelbold"></asp:Label><br />
                                <asp:DropDownList ID="DDBranchName" Enabled="false" runat="server" CssClass="dropdown"
                                    Width="150px">
                                </asp:DropDownList>
                            </td>
                            <td class="tdstyle">
                                <asp:Label ID="Label6" runat="server" Text="VendorName" CssClass="labelbold"></asp:Label>
                                <br />
                                <asp:DropDownList CssClass="dropdown" ID="DDPartyName" runat="server" OnSelectedIndexChanged="DDPartyName_SelectedIndexChanged"
                                    AutoPostBack="True">
                                </asp:DropDownList>
                            </td>
                            <td runat="server" id="Tdlegalvendor" visible="false">
                                <asp:Label ID="Label46" runat="server" Text="Legal Vendor" CssClass="labelbold"></asp:Label>
                                <br />
                                <asp:DropDownList CssClass="dropdown" ID="DDlegalvendor" Width="200px" runat="server">
                                </asp:DropDownList>
                            </td>
                            <td class="tdstyle">
                                <asp:Label ID="Label7" runat="server" Text="Order No" CssClass="labelbold"></asp:Label>
                                <br />
                                <asp:DropDownList CssClass="dropdown" ID="DDChallanNo" runat="server" OnSelectedIndexChanged="DDChallanNo_SelectedIndexChanged"
                                    AutoPostBack="True" Width="150px">
                                </asp:DropDownList>
                            </td>
                            <td id="tdchalan" runat="server" visible="false" class="tdstyle">
                                <asp:Label ID="Label8" runat="server" Text="Challan No." CssClass="labelbold"></asp:Label><br />
                                <asp:DropDownList ID="ddlrecchalanno" runat="server" CssClass="dropdown" AutoPostBack="True"
                                    OnSelectedIndexChanged="ddlrecchalanno_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                        </tr>
                    </table>
                    <table>
                        <tr>
                            <td class="tdstyle">
                                <asp:Label ID="Label9" runat="server" Text="ReceiveDate" CssClass="labelbold"></asp:Label>
                                <br />
                                <asp:TextBox ID="TxtReceiveDate" CssClass="textb" runat="server" Width="90px" onchange="return validateDate();"></asp:TextBox>
                                <cc1:CalendarExtender ID="CalendarExtender1" runat="server" Format="dd-MMM-yyyy"
                                    TargetControlID="TxtReceiveDate">
                                </cc1:CalendarExtender>
                            </td>
                            <td id="TdReceiveNo" runat="server" visible="false" class="tdstyle">
                                <asp:Label ID="Label10" runat="server" Text="ReceiveNo" CssClass="labelbold"></asp:Label>
                                <br />
                                <asp:TextBox ID="TxtReceiveNo" CssClass="textb" runat="server" Width="100px"></asp:TextBox>
                            </td>
                            <td class="tdstyle">
                                <asp:Label ID="Label11" runat="server" Text="Gate In No." CssClass="labelbold"></asp:Label><br />
                                <asp:TextBox ID="TxtGateInNo" CssClass="textb" runat="server" Width="100px" onkeydown="return (event.keyCode!=13);"></asp:TextBox>
                            </td>
                            <td class="tdstyle">
                                <table>
                                    <tr>
                                        <td>
                                            <asp:Label ID="Label12" runat="server" Text="Challan No." CssClass="labelbold"></asp:Label><br />
                                            <asp:TextBox ID="TxtBillNo" CssClass="textb" runat="server" Width="100px" onkeydown="return (event.keyCode!=13);"
                                                BackColor="Beige"></asp:TextBox>
                                        </td>
                                        <td class="tdstyle">
                                            <asp:Label ID="Label13" runat="server" Text="Bill No." CssClass="labelbold"></asp:Label><br />
                                            <asp:TextBox ID="txtbillno1" CssClass="textb" runat="server" Width="100px" OnTextChanged="TxtBillNo1_TextChanged"
                                                onkeydown="return (event.keyCode!=13);"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2" runat="server" id="TdlnkupdatebillNo" visible="false">
                                            <asp:LinkButton ID="lnkupdatebillNo" Text="Update Bill No./Challan No." CssClass="lnkbtnClass"
                                                runat="server" OnClick="lnkupdatebillNo_Click" OnClientClick="return confirm('Do you want to update Bill No./Challan No.')" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                            <td class="tdstyle" id="TDBillDate" runat="server" visible="false">
                                <asp:Label ID="Label38" runat="server" Text="Bill Date" CssClass="labelbold"></asp:Label>
                                <br />
                                <asp:TextBox ID="txtBillDate" CssClass="textb" runat="server" Width="90px"></asp:TextBox>
                                <cc1:CalendarExtender ID="CalendarExtender3" runat="server" Format="dd-MMM-yyyy"
                                    TargetControlID="txtBillDate">
                                </cc1:CalendarExtender>
                            </td>
                            <td class="tdstyle">
                                <asp:Label ID="Label14" runat="server" Text="Return Qty Date" CssClass="labelbold"></asp:Label>
                                <br />
                                <asp:TextBox ID="txtretdate" CssClass="textb" runat="server" Width="90px"></asp:TextBox>
                                <cc1:CalendarExtender ID="CalendarExtender2" runat="server" Format="dd-MMM-yyyy"
                                    TargetControlID="txtretdate">
                                </cc1:CalendarExtender>
                            </td>
                            <td id="Td2" align="left" runat="server">
                                <asp:Label ID="Label17" runat="server" Text="Freight Charge" CssClass="labelbold"></asp:Label>
                                <br />
                                <asp:TextBox ID="txtfreight" runat="server" CssClass="textb" Width="80px" onkeypress="return isNumber(event);"></asp:TextBox>
                            </td>
                            <td id="Td3" align="left" runat="server">
                                <asp:Label ID="Label45" runat="server" Text="Other Charges" CssClass="labelbold"></asp:Label>
                                <br />
                                <asp:TextBox ID="txtothercharges" runat="server" CssClass="textb" Width="80px" onkeypress="return isNumber(event);"></asp:TextBox>
                            </td>
                        </tr>
                    </table>
                </fieldset>
                <fieldset style="margin-left: 10px; width: 789px;">
                    <legend>
                        <asp:Label ID="Label4" runat="server" Text="Item Detail" CssClass="labelbold" Font-Bold="true"
                            ForeColor="Red"></asp:Label></legend>
                    <table>
                        <tr>
                            <td id="itmcod" runat="server" visible="false" class="tdstyle">
                                <asp:Label ID="Label15" runat="server" Text="Product Code" CssClass="labelbold"></asp:Label><br />
                                <asp:TextBox ID="TextItemCode" CssClass="textb" runat="server" Width="100px" OnTextChanged="TextItemCode_TextChanged"
                                    AutoPostBack="True" Enabled="true"></asp:TextBox>
                                <cc1:AutoCompleteExtender ID="TxtProdCode_AutoCompleteExtender" runat="server" EnableCaching="true"
                                    Enabled="True" MinimumPrefixLength="1" ServiceMethod="GetQuality" TargetControlID="TextItemCode"
                                    UseContextKey="True">
                                </cc1:AutoCompleteExtender>
                            </td>
                            <td>
                                <asp:Label ID="LblCategory" class="tdstyle" runat="server" AutoPostBack="true" Text=""
                                    CssClass="labelbold"></asp:Label>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="DDCategory"
                                    ErrorMessage="please Select Category" ForeColor="Red" SetFocusOnError="true"
                                    ValidationGroup="f1">*</asp:RequiredFieldValidator>
                                <br />
                                <asp:DropDownList CssClass="dropdown" ID="DDCategory" Width="110px" AutoPostBack="True"
                                    runat="server" OnSelectedIndexChanged="DDCategory_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:Label ID="LblItemName" class="tdstyle" runat="server" AutoPostBack="true" Text="Label"
                                    CssClass="labelbold"></asp:Label>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ControlToValidate="DDItem"
                                    ErrorMessage="please Select Item" ForeColor="Red" SetFocusOnError="true" ValidationGroup="f1">*</asp:RequiredFieldValidator>
                                <br />
                                <asp:DropDownList CssClass="dropdown" ID="DDItem" Width="110" AutoPostBack="True"
                                    runat="server" OnSelectedIndexChanged="DDItem_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                            <td id="TdQuality" runat="server" visible="false">
                                <asp:Label ID="LblQuality" class="tdstyle" runat="server" Text="Label" CssClass="labelbold"></asp:Label><br />
                                <asp:DropDownList CssClass="dropdown" ID="DDQuality" Width="110" runat="server" AutoPostBack="True"
                                    OnSelectedIndexChanged="DDQuality_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                            <td id="TdDesign" runat="server" visible="false">
                                <asp:Label ID="LblDesign" class="tdstyle" runat="server" Text="Label" CssClass="labelbold"></asp:Label><br />
                                <asp:DropDownList CssClass="dropdown" Width="110" ID="DDDesign" runat="server" AutoPostBack="True"
                                    OnSelectedIndexChanged="DDDesign_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                            <td id="TdColor" runat="server" visible="false">
                                <asp:Label ID="LblColor" runat="server" Text="" CssClass="labelbold"></asp:Label><br />
                                <asp:DropDownList CssClass="dropdown" Width="110" ID="DDColor" runat="server" AutoPostBack="True"
                                    OnSelectedIndexChanged="DDColor_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                            <td id="TdColorShade" runat="server" visible="false">
                                <asp:Label ID="LblColorShade" class="tdstyle" runat="server" Text="" CssClass="labelbold"></asp:Label><br />
                                <asp:DropDownList CssClass="dropdown" Width="110" ID="DDColorShade" runat="server"
                                    AutoPostBack="True" OnSelectedIndexChanged="DDColorShade_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                            <td id="TDCustomerOrderNo" runat="server" visible="false">
                                <asp:Label ID="Label49" class="tdstyle" runat="server" Text="Order No" CssClass="labelbold"></asp:Label><br />
                                <asp:DropDownList CssClass="dropdown" Width="110" ID="DDCustomerOrderNo" runat="server"
                                    AutoPostBack="True" OnSelectedIndexChanged="DDCustomerOrderNo_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                        </tr>
                    </table>
                    <table>
                        <tr>
                            <td id="TdShape" runat="server" visible="false">
                                <asp:Label ID="LblShape" class="tdstyle" runat="server" Text="Label" CssClass="labelbold"></asp:Label><br />
                                <asp:DropDownList CssClass="dropdown" Width="100px" ID="DDShape" runat="server" AutoPostBack="True"
                                    OnSelectedIndexChanged="DDShape_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                            <td id="TdSize" runat="server" visible="false">
                                <asp:Label ID="LblSize" class="tdstyle" runat="server" Text="Label" CssClass="labelbold"></asp:Label>
                                <asp:CheckBox ID="ChkFt" runat="server" class="tdstyle" AutoPostBack="True" Text="Ft"
                                    OnCheckedChanged="ChkFt_CheckedChanged" Visible="false" />
                                <br />
                                <asp:DropDownList CssClass="dropdown" Width="100px" ID="DDSize" runat="server" AutoPostBack="True"
                                    OnSelectedIndexChanged="DDSize_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                            <td id="TdFinish_Type" runat="server" visible="false">
                                <asp:Label ID="LblFinish_Type" runat="server" class="tdstyle" Text="Finish_Type"
                                    CssClass="labelbold"></asp:Label>
                                &nbsp;<br />
                                <asp:DropDownList ID="ddFinish_Type" runat="server" Width="115px" CssClass="dropdown"
                                    AutoPostBack="True">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:Label ID="Label16" runat="server" Text="Godown" CssClass="labelbold"></asp:Label><br />
                                <asp:DropDownList ID="DDGodown" CssClass="dropdown" Width="110px" runat="server"
                                    AutoPostBack="True" OnSelectedIndexChanged="DDGodown_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                            <td class="style1" id="TDBINNO" runat="server" visible="false">
                                <asp:Label ID="Label36" runat="server" Text="Bin No." CssClass="labelbold"></asp:Label><br />
                                <asp:DropDownList ID="DDBinNo" CssClass="dropdown" Width="110px" runat="server">
                                </asp:DropDownList>
                            </td>
                            <td class="style1" id="tdlot" runat="server">
                                <asp:Label ID="lblIsslotNo" runat="server" Text="Iss Lot No." CssClass="labelbold"></asp:Label><br />
                                <asp:DropDownList ID="DDLotNo" CssClass="dropdown" Width="110px" runat="server" AutoPostBack="True"
                                    OnSelectedIndexChanged="DDLotNo_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                            <td class="style1" id="TDRecLotNo" runat="server">
                                <asp:Label ID="lblLotno" runat="server" Text="Rec. Lot No." CssClass="labelbold"></asp:Label><br />
                                <asp:TextBox ID="txtLotNo" CssClass="textb" runat="server" Width="100px" />
                            </td>
                            <td class="style1" id="TDRectagNo" visible="false" runat="server">
                                <asp:Label ID="lbltagno" runat="server" Text="Rec. Tag No." CssClass="labelbold"></asp:Label><br />
                                <asp:TextBox ID="txttagNo" CssClass="textb" runat="server" Width="100px" />
                            </td>
                            <td class="style1" id="TDCompanyLotNo" runat="server" visible="false">
                                <table>
                                    <tr>
                                        <td>
                                            <asp:CheckBox ID="chkoldlotno" Text="For Old LotNo." CssClass="checkboxbold" AutoPostBack="true"
                                                ForeColor="Red" runat="server" OnCheckedChanged="chkoldlotno_CheckedChanged" /><br />
                                            <asp:Label ID="Label35" runat="server" Text="Company Lot No." CssClass="labelbold"></asp:Label><br />
                                            <asp:TextBox ID="txtcomplotno" CssClass="textb" Enabled="false" BackColor="LightGray"
                                                runat="server" Width="100px" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                            <td class="style1" id="TDBaleNo" runat="server" visible="false">
                                <asp:Label ID="Label31" runat="server" Text="Bale No." CssClass="labelbold"></asp:Label><br />
                                <asp:TextBox ID="txtbaleno" CssClass="textb" runat="server" Width="100px" />
                            </td>
                            <td class="style1">
                                <asp:Label ID="Label18" runat="server" Text="Unit" CssClass="labelbold"></asp:Label><br />
                                <asp:DropDownList CssClass="dropdown" Width="110" ID="DDUnit" runat="server">
                                </asp:DropDownList>
                            </td>
                        </tr>
                    </table>
                </fieldset>
                <fieldset style="margin-left: 10px; width: 789px;">
                    <legend>
                        <asp:Label ID="lblQtyRemark" runat="server" Text="Qty And Remarks" CssClass="labelbold"
                            Font-Bold="true" ForeColor="Red"></asp:Label></legend>
                    <table>
                        <tr id="trvat" runat="server">
                            <td class="style1">
                                <asp:Label ID="Label19" runat="server" Text="Order Qty." CssClass="labelbold"></asp:Label>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ControlToValidate="TxtorderQty"
                                    ErrorMessage="Invalid total Qty...." ForeColor="Red" SetFocusOnError="true" ValidationGroup="f1">*</asp:RequiredFieldValidator>
                                <br />
                                <asp:TextBox ID="txtorderqty" runat="server" Enabled="False" Width="80px" onkeypress="return isNumber(event);"
                                    CssClass="textb"></asp:TextBox>
                            </td>
                            <td class="style1">
                                <asp:Label ID="Label20" runat="server" Text="Pending Qty." CssClass="labelbold"></asp:Label>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="TxtPQty"
                                    ErrorMessage="Invalid Pending Qty...." ForeColor="Red" SetFocusOnError="true"
                                    ValidationGroup="f1">*</asp:RequiredFieldValidator>
                                <br />
                                <asp:TextBox ID="TxtPQty" runat="server" CssClass="textb" Enabled="False" Width="80px"
                                    onkeypress="return isNumber(event);"></asp:TextBox>
                            </td>
                            <td align="left" class="style1">
                                <asp:Label ID="Label21" runat="server" Text="Receive Qty." CssClass="labelbold"></asp:Label>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="TxtQty"
                                    ErrorMessage="please fill float value in Qty.........." ForeColor="Red" SetFocusOnError="true"
                                    ValidationGroup="f1">*</asp:RequiredFieldValidator>
                                <br />
                                <asp:TextBox ID="TxtQty" runat="server" CssClass="textb" AutoPostBack="True" OnTextChanged="TxtQty_TextChanged"
                                    onkeypress="return isNumber(event);" Width="80px" BackColor="Beige"></asp:TextBox>
                            </td>
                            <td align="left" class="style1" id="TDBillQty" runat="server" visible="false">
                                <asp:Label ID="Label39" runat="server" Text="Bill Qty." CssClass="labelbold"></asp:Label>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" ControlToValidate="txtBillQty"
                                    ErrorMessage="please fill Bill Qty.........." ForeColor="Red" SetFocusOnError="true"
                                    ValidationGroup="f1">*</asp:RequiredFieldValidator>
                                <br />
                                <asp:TextBox ID="txtBillQty" runat="server" CssClass="textb" onkeypress="return isNumber(event);"
                                    Width="80px" BackColor="Beige">
                                </asp:TextBox>
                            </td>
                            <td align="left" class="style1">
                                <asp:Label ID="Label22" runat="server" Text="Return Qty." CssClass="labelbold"></asp:Label>
                                <br />
                                <asp:TextBox ID="Txtreturnqty" runat="server" AutoPostBack="True" CssClass="textb"
                                    ReadOnly="true" onkeypress="return isNumber(event);" OnTextChanged="TxtQty1_TextChanged"
                                    Width="70px"></asp:TextBox>
                            </td>
                            <td align="left" id="TDBellWt" runat="server" visible="false">
                                <asp:Label ID="Label32" runat="server" Text="Bell Wt." CssClass="labelbold"></asp:Label>
                                <br />
                                <asp:TextBox ID="txtbellwt" runat="server" CssClass="textb" onkeypress="return isNumber(event);"
                                    Width="70px" BackColor="Beige" AutoPostBack="true" OnTextChanged="txtbellwt_TextChanged"></asp:TextBox>
                            </td>
                            <td id="TDLShort" runat="server" visible="false">
                                <asp:Label ID="lblLShort" runat="server" Text="L-Short(%)" CssClass="labelbold"></asp:Label>
                                <br />
                                <asp:TextBox ID="txtLshort" runat="server" AutoPostBack="True" CssClass="textb" onkeypress="return isNumber(event);"
                                    Width="60px" OnTextChanged="txtLshort_TextChanged"></asp:TextBox>
                            </td>
                            <td align="left" id="tdrate" runat="server" class="style1">
                                <asp:Label ID="Label23" runat="server" Text="Rate" CssClass="labelbold"></asp:Label>
                                <br />
                                <asp:TextBox ID="txtrate" runat="server" Enabled="false" CssClass="textb" Width="60px"
                                    AutoPostBack="true" OnTextChanged="txtrate_TextChanged" onkeypress="return isNumber(event);"></asp:TextBox>
                            </td>
                            <td align="left" id="tdamout" runat="server" class="style1">
                                <asp:Label ID="Label24" runat="server" Text="Amount" CssClass="labelbold"></asp:Label>
                                <br />
                                <asp:TextBox ID="TxtAmount" runat="server" Enabled="false" CssClass="textb" Width="80px"
                                    onkeypress="return isNumber(event);"></asp:TextBox>
                            </td>
                            <td class="tdstyle" id="TD1" runat="server">
                                <asp:Label ID="Label25" runat="server" Text="Penalty" CssClass="labelbold"></asp:Label>
                                <br />
                                <asp:TextBox ID="TxtPenalty" runat="server" CssClass="textb" Width="60px" onkeypress="return isNumber(event);"
                                    AutoPostBack="true" OnTextChanged="TxtPenalty_TextChanged"></asp:TextBox>
                            </td>
                            <td align="left" class="tdstyle" id="TDtxtVat" runat="server" visible="false">
                                <asp:Label ID="Label26" runat="server" Text="VAT" CssClass="labelbold"></asp:Label>
                                <br />
                                <asp:TextBox ID="txtvat" runat="server" CssClass="textb" Width="60px" onkeypress="return isNumber(event);"
                                    AutoPostBack="true" OnTextChanged="txtvat_TextChanged"></asp:TextBox>
                            </td>
                            <td align="left" class="tdstyle" id="TDtxtcst" runat="server" visible="false">
                                <asp:Label ID="Label27" runat="server" Text="CST" CssClass="labelbold"></asp:Label>
                                <br />
                                <asp:TextBox ID="txtcst" runat="server" CssClass="textb" Width="60px" onkeypress="return isNumber(event);"
                                    AutoPostBack="true" OnTextChanged="txtcst_TextChanged"></asp:TextBox>
                            </td>
                            <td align="left" class="tdstyle" id="TDtxtCGST" runat="server">
                                <asp:Label ID="Label47" runat="server" Text="CGST" CssClass="labelbold"></asp:Label>
                                <br />
                                <asp:TextBox ID="txtCGST" runat="server" CssClass="textb" Width="60px" onkeypress="return isNumber(event);"
                                    Enabled="false"></asp:TextBox>
                            </td>
                            <td align="left" class="tdstyle" id="TDtxtSGST" runat="server">
                                <asp:Label ID="Label33" runat="server" Text="SGST" CssClass="labelbold"></asp:Label>
                                <br />
                                <asp:TextBox ID="txtSGST" runat="server" CssClass="textb" Width="60px" onkeypress="return isNumber(event);"
                                    Enabled="false" AutoPostBack="true" OnTextChanged="txtSGST_TextChanged"></asp:TextBox>
                            </td>
                            <td align="left" class="tdstyle" id="TDtxtIGST" runat="server">
                                <asp:Label ID="Label34" runat="server" Text="IGST" CssClass="labelbold"></asp:Label>
                                <br />
                                <asp:TextBox ID="txtIGST" runat="server" CssClass="textb" Width="60px" onkeypress="return isNumber(event);"
                                    Enabled="false" AutoPostBack="true" OnTextChanged="txtIGST_TextChanged"></asp:TextBox>
                            </td>
                            <td align="left" class="tdstyle" id="TDTCS" runat="server">
                                <asp:Label ID="Label50" runat="server" Text="TCS" CssClass="labelbold"></asp:Label>
                                <br />
                                <asp:TextBox ID="txtTCS" runat="server" CssClass="textb" Width="60px" onkeypress="return isNumber(event);"
                                    Enabled="false" AutoPostBack="true" OnTextChanged="txtTCS_TextChanged"></asp:TextBox>
                            </td>
                            <td align="left" class="tdstyle" id="TDTxtnetamount" runat="server">
                                <asp:Label ID="Label28" runat="server" Text="Net Amount" CssClass="labelbold"></asp:Label>
                                <br />
                                <asp:TextBox ID="Txtnetamount" runat="server" Enabled="false" CssClass="textb" onkeypress="return isNumber(event);"
                                    Width="80px"></asp:TextBox>
                            </td>
                            <td align="left" class="tdstyle" id="TD50MWait" runat="server" visible="false">
                                <asp:Label ID="Label43" runat="server" Text="50Mt Wait" CssClass="labelbold"></asp:Label>
                                <br />
                                <asp:TextBox ID="txt50MWait" runat="server" CssClass="textb" onkeypress="return isNumber(event);"
                                    Width="80px"></asp:TextBox>
                            </td>
                        </tr>
                    </table>
                    <table>
                        <tr>
                            <td align="left" class="tdstyle" id="TDMoisture" runat="server" visible="false">
                                <asp:Label ID="Label48" runat="server" Text="Moisture" CssClass="labelbold"></asp:Label>
                                <br />
                                <asp:TextBox ID="TxtMoisture" runat="server" CssClass="textb" Width="80px"></asp:TextBox>
                            </td>
                            <td align="left" class="tdstyle" id="TDYarnShape" runat="server" visible="false">
                                <asp:Label ID="Label44" runat="server" Text="Yarn Shape" CssClass="labelbold"></asp:Label>
                                <br />
                                <asp:TextBox ID="txtYarnShape" runat="server" CssClass="textb" Width="80px"></asp:TextBox>
                            </td>

                             <td align="left" class="tdstyle" id="TDQtyWeight" runat="server" visible="false">
                                <asp:Label ID="Label51" runat="server" Text="Qty Weight" CssClass="labelbold"></asp:Label>
                                <br />
                                <asp:TextBox ID="txtQtyWeight" runat="server" CssClass="textb" Width="80px"></asp:TextBox>
                            </td>

                            <td class="tdstyle" id="TDtxtremarks" runat="server" colspan="3">
                                <asp:Label ID="Label29" runat="server" Text="Item Remarks" CssClass="labelbold"></asp:Label>
                                <br />
                                <asp:TextBox ID="txtremarks" runat="server" Width="250px" CssClass="textboxremark"
                                    TextMode="MultiLine"></asp:TextBox>
                            </td>
                            <td class="tdstyle" id="TDtxtmastremark" runat="server" colspan="3">
                                <asp:Label ID="Label30" runat="server" Text="MRemarks" CssClass="labelbold"></asp:Label>
                                <br />
                                <asp:TextBox ID="txtmastremark" runat="server" Width="250px" CssClass="textboxremark"
                                    TextMode="MultiLine"></asp:TextBox>
                            </td>
                            <td class="tdstyle" id="TDPenalityRemarks" runat="server" colspan="3">
                                <asp:Label ID="Label37" runat="server" Text="Penality Remarks" CssClass="labelbold"></asp:Label>
                                <br />
                                <asp:TextBox ID="txtPenalityRemark" runat="server" Width="250px" CssClass="textboxremark"
                                    TextMode="MultiLine"></asp:TextBox>
                            </td>

                             <td colspan="2" runat="server" id="TDUpdateMainRemark" visible="false">
                                 <asp:LinkButton ID="lnkUpdateMainRemark" Text="Update Main Remark" CssClass="lnkbtnClass"
                                      runat="server" OnClick="lnkUpdateMainRemark_Click" OnClientClick="return confirm('Do you want to update main remark')" />
                             </td>

                        </tr>
                    </table>
                </fieldset>
                <div>
                    <div style="width: 800px; margin-left: 10px; height: auto">
                        <div style="width: 780px; background-color: #8B7B8B; padding-left: 20px">
                            <table>
                                <tr>
                                    <td>
                                        <asp:CheckBox ID="chkTransportInformation" runat="server" Text=" TRANSPORT INFORMATION "
                                            ForeColor="White" CssClass="labelbold" AutoPostBack="true" OnCheckedChanged="chkTransportInformation_CheckedChanged" />
                                    </td>
                                </tr>
                            </table>
                        </div>
                        <div id="DivTransPort" runat="server" style="border: 1px Solid; width: 805px;" visible="false">
                            <table>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblTName" runat="server" Text="NAME OF TRANSPORT" CssClass="labelbold"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtTransportName" runat="server" Width="250px" CssClass="textb"
                                            Height="25px"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblTAddress" runat="server" Text="ADDRESS OF TRANSPORT" CssClass="labelbold"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtTransportAddress" runat="server" Width="264px" TextMode="MultiLine"
                                            CssClass="textboxremark"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right">
                                        <asp:Label ID="lblDName" Text="DRIVER NAME" runat="server" CssClass="labelbold"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtDriver" runat="server" Width="200px" CssClass="textb" Height="25px"></asp:TextBox>
                                    </td>
                                    <td align="right">
                                        <asp:Label ID="lblTruckNumber" Text="VEHICLE NO." runat="server" CssClass="labelbold"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtVehicleNo" runat="server" Width="200px" CssClass="textb" Height="25px"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right">
                                        <asp:Label ID="lblBiltyNo" Text="BILTY NUMBER" runat="server" CssClass="labelbold"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtBiltyNo" runat="server" Width="200px" CssClass="textb" Height="25px"></asp:TextBox>
                                    </td>
                                    <td align="right">
                                        <asp:Label ID="lblBiltyDate" Text="BILTY DATE" runat="server" CssClass="labelbold"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtBiltyDate" runat="server" Width="100px" CssClass="textb" Height="25px"
                                            BackColor="Beige"></asp:TextBox>
                                        <asp:CalendarExtender ID="calnderExtenderforBilty" TargetControlID="txtBiltyDate"
                                            runat="server" Format="dd-MMM-yyyy">
                                        </asp:CalendarExtender>
                                    </td>
                                </tr>
                                <tr id="TRTransportBuilty" runat="server" visible="false">
                                    <td align="right">
                                        <asp:Label ID="Label40" Text="Transport BuiltyAmt" runat="server" CssClass="labelbold"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtTransportBuiltyAmt" runat="server" Width="200px" CssClass="textb"
                                            Height="25px"></asp:TextBox>
                                    </td>
                                    <td align="right">
                                        <asp:Label ID="Label41" Text="Unloading Expenses" runat="server" CssClass="labelbold"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtUnloadingExpenses" runat="server" Width="100px" CssClass="textb"
                                            Height="25px" BackColor="Beige"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr id="TRParticular" runat="server" visible="false">
                                    <td align="right">
                                        <asp:Label ID="Label42" Text="Particular" runat="server" CssClass="labelbold"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtParticular" runat="server" Width="200px" CssClass="textb" Height="25px"></asp:TextBox>
                                    </td>
                                    <td align="right">
                                        &nbsp;
                                    </td>
                                    <td>
                                        &nbsp;
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </div>
                </div>
                <table style="width: 800px">
                    <tr>
                        <td align="right" colspan="7">
                            <asp:DropDownList ID="DDPreviewType" runat="server" CssClass="dropdown" OnSelectedIndexChanged="DDPreviewType_SelectedIndexChanged"
                                AutoPostBack="True">
                                <asp:ListItem Text="Purchase With Rate" Value="0"></asp:ListItem>
                                <asp:ListItem Text="Purchase WithOut Rate" Value="1"></asp:ListItem>
                            </asp:DropDownList>
                            <asp:CheckBox ID="chklotsummary" Text="Lot Wise Summary" runat="server" CssClass="checkboxbold" />
                            <asp:Button ID="btnnew" runat="server" Text="New" OnClientClick="return NewForm();"
                                CssClass="buttonnorm" />
                            <asp:Button ID="BtnSave" runat="server" Text="Save" CssClass="buttonnorm" OnClientClick="return validate()"
                                OnClick="BtnSave_Click" ValidationGroup="f1" />
                            <asp:Button ID="BtnUpdateLegalVendor" runat="server" Text="Update L Vendor" CssClass="buttonnorm"
                                OnClick="BtnUpdateLegalVendor_Click" />
                            <asp:Button ID="BtnPreview" runat="server" Text="Preview" CssClass="buttonnorm" OnClick="BtnPreview_Click"
                                Enabled="false" />
                            <asp:Button ID="btnqcchkpreview" runat="server" CssClass="buttonnorm" Text="QCReport"
                                OnClick="btnqcchkpreview_Click" />
                            <asp:Button ID="BtnComplete" runat="server" Visible="false" CssClass="buttonnorm"
                                Text="Complete" OnClick="BtnComplete_Click" />
                            <asp:Button ID="BtnClose" runat="server" Text="Close" OnClientClick="return CloseForm();"
                                CssClass="buttonnorm" />
                        </td>
                    </tr>
                </table>
                <table>
                    <tr>
                        <td colspan="6">
                            <asp:Label ID="Label3" Font-Size="Small" Font-Bold="true" runat="server" Text=""></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4">
                            <table>
                                <tr>
                                    <td colspan="7">
                                        <asp:Label ID="Lblmessage" runat="server" ForeColor="Red" Text=""></asp:Label>
                                        <asp:ValidationSummary ID="ValidationSummary1" runat="server" ValidationGroup="f1"
                                            ForeColor="Red" />
                                    </td>
                                </tr>
                                <tr id="qulitychk" visible="false" runat="server">
                                    <td colspan="7">
                                        <asp:GridView ID="grdqualitychk" runat="server" AutoGenerateColumns="False" DataKeyNames="ID"
                                            CssClass="grid-view" OnRowCreated="grdqualitychk_RowCreated">
                                            <Columns>
                                                <asp:TemplateField>
                                                    <EditItemTemplate>
                                                        <asp:CheckBox ID="CheckBox1" runat="server" />
                                                    </EditItemTemplate>
                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="CheckBox1" runat="server" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="SrNo">
                                                    <EditItemTemplate>
                                                        <asp:TextBox ID="TextBox2" runat="server" Text='<%# Bind("SrNo") %>'></asp:TextBox>
                                                    </EditItemTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label ID="Label2" runat="server" Text='<%# Bind("SrNo") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="ParaName">
                                                    <EditItemTemplate>
                                                        <asp:TextBox ID="TextBox1" runat="server" Text='<%# Bind("ParaName") %>'></asp:TextBox>
                                                    </EditItemTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label ID="Label1" runat="server" Text='<%# Bind("ParaName") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                    </td>
                                </tr>
                            </table>
                        </td>
                        <%--<td colspan="5"> <div style="width: 100%; height: 150px; overflow: scroll;">
                                <asp:GridView ID="DGporder" runat="server" AutoGenerateColumns="False" 
                                    CssClass="grid-view"  
                                     
                                    DataKeyNames="finishedid"  >
                                    <Columns>
                                        
                                        <asp:BoundField DataField="DESCRIPTION" HeaderText="Description">
                                            <HeaderStyle Width="250px" HorizontalAlign="Center"  />
                                            <ItemStyle HorizontalAlign="Left" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="Qty" HeaderText="Qty" />
                                        <asp:TemplateField HeaderText="Net Amount" Visible="false" >
                                        <ItemTemplate>
                                         <asp:Label ID="lblfinishedid" runat="server" Text='<%# Bind("finishedid") %>' Visible="false"></asp:Label>
                                         <asp:Label ID="lblcategoryid" runat="server"  Text='<%# Bind("category_id") %>' Visible="false"></asp:Label>
                                         <asp:Label ID="lblitem_id" runat="server"  Text='<%# Bind("item_id") %>' Visible="false"></asp:Label>
                                         <asp:Label ID="lblQualityid" runat="server"  Text='<%# Bind("Qualityid") %>' Visible="false"></asp:Label>
                                         <asp:Label ID="lblColorid" runat="server"  Text='<%# Bind("Colorid") %>' Visible="false"></asp:Label>
                                         <asp:Label ID="lbldesignid" runat="server"  Text='<%# Bind("designid") %>' Visible="false"></asp:Label>
                                         <asp:Label ID="lblshapeid" runat="server"  Text='<%# Bind("shapeid") %>' Visible="false"></asp:Label>
                                         <asp:Label ID="lblshadecolorid" runat="server"  Text='<%# Bind("shadecolorid") %>' Visible="false"></asp:Label>
                                         <asp:Label ID="lblsizeid" runat="server"  Text='<%# Bind("sizeid") %>' Visible="false"></asp:Label>
                                         <asp:Label ID="lblqty" runat="server"  Text='<%# Bind("Qty") %>' Visible="false"></asp:Label>
                                        </ItemTemplate>               
                                     </asp:TemplateField>
                                    </Columns>
                                    <HeaderStyle CssClass="gvheader" />
                                    <AlternatingRowStyle CssClass="gvalt" />
                                    <RowStyle CssClass="gvrow" />
                                    <EmptyDataRowStyle CssClass="gvemptytext" />
                                </asp:GridView>
                            </div></td>--%></tr>
                </table>
                <table style="width: 950px">
                    <tr>
                        <td colspan="7" align="left">
                            <div style="width: 500px; height: 250px; overflow: auto;">
                                <asp:GridView ID="DGPurchaseReceiveDetail" AutoGenerateColumns="False" OnRowDataBound="DGPurchaseReceiveDetail_RowDataBound"
                                    CellPadding="4" runat="server" OnRowDeleting="DGPurchaseReceiveDetail_RowDeleting"
                                    DataKeyNames="PurchaseReceiveDetailId" OnRowEditing="DGPurchaseReceiveDetail_RowEditing"
                                    OnRowCancelingEdit="DGPurchaseReceiveDetail_RowCancelingEdit" OnRowUpdating="DGPurchaseReceiveDetail_RowUpdating"
                                    Width="100%">
                                    <HeaderStyle CssClass="gvheaders" />
                                    <AlternatingRowStyle CssClass="gvalts" />
                                    <RowStyle CssClass="gvrow" />
                                    <EmptyDataRowStyle CssClass="gvemptytext" />
                                    <Columns>
                                        <asp:BoundField DataField="PurchaseReceiveDetailId" HeaderText="PurchaseReceiveDetailId"
                                            Visible="false" />
                                        <%--<asp:BoundField DataField="ChallanNo" HeaderText="" >
                                        <HeaderStyle Width="0px" />
                                        <ItemStyle Width="0px" />
                                        </asp:BoundField>--%>
                                        <asp:TemplateField HeaderText="Item Description">
                                            <ItemTemplate>
                                                <asp:Label ID="lblDGItemDescription" Text='<%#Bind("ItemDescription") %>' runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Godown">
                                            <ItemTemplate>
                                                <asp:Label ID="lblDGGodownName" Text='<%#Bind("GodownName") %>' runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Qty">
                                            <ItemTemplate>
                                                <asp:Label ID="lblDGQty" Text='<%#Bind("Qty") %>' runat="server" />
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:TextBox ID="TxtDGQty" Text='<%#Bind("Qty") %>' Width="50px" runat="server" />
                                            </EditItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Unit">
                                            <ItemTemplate>
                                                <asp:Label ID="lblDGUnitName" Text='<%#Bind("UnitName") %>' runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="LOTNO">
                                            <ItemTemplate>
                                                <asp:Label ID="lblDGLOTNO" Text='<%#Bind("LOTNO") %>' runat="server" />
                                            </ItemTemplate>
                                             <EditItemTemplate>
                                                <asp:TextBox ID="txtLotNo" Text='<%#Bind("LOTNO") %>' Width="50px" runat="server" Enabled="false" />
                                            </EditItemTemplate>
                                        </asp:TemplateField>
                                         <asp:TemplateField HeaderText="TAGNO">
                                            <ItemTemplate>
                                                <asp:Label ID="lblDGTAGNO" Text='<%#Bind("TAGNO") %>' runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <%--<asp:BoundField DataField="ItemDescription" HeaderText="Item Description" />
                                        <asp:BoundField DataField="GodownName" HeaderText="GodownName" />
                                        <asp:BoundField DataField="Qty" HeaderText="Qty" />
                                        <asp:BoundField DataField="UnitName" HeaderText="Unit" />
                                        <asp:BoundField DataField="LOTNO" HeaderText="LOTNO" />--%>
                                        <asp:TemplateField HeaderText="Rate" Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="LblChallanNo" runat="server" Visible="false" Text='<%# Bind("ChallanNo") %>' />
                                                <asp:Label ID="lblitem" runat="server" Visible="false" Text='<%# Bind("item") %>' />
                                                <asp:Label ID="lbldetailid" runat="server" Visible="false" Text='<%# Bind("PurchaseReceiveDetailId") %>' />
                                                <asp:Label ID="lblqnt" runat="server" Visible="false" Text='<%# Bind("Qty") %>' />
                                                <asp:Label ID="lblretqty" runat="server" Visible="false" Text='<%# Bind("qtyreturn") %>' />
                                                <asp:Label ID="lblamount" runat="server" Visible="false" Text='<%# Bind("amount") %>' />
                                                <asp:Label ID="lblvat" runat="server" Visible="false" Text='<%# Bind("vat") %>' />
                                                <asp:Label ID="lblcst" runat="server" Visible="false" Text='<%# Bind("cst") %>' />
                                                <asp:Label ID="lblnetamount" runat="server" Visible="false" Text='<%# Bind("NetAmount") %>' />
                                                <asp:Label ID="LblPenality" runat="server" Visible="false" Text='<%# Bind("Penalty") %>' />
                                                <asp:Label ID="Remark" runat="server" Visible="false" Text='<%# Bind("remark") %>' />
                                                <asp:Label ID="MRemark" runat="server" Visible="false" Text='<%# Bind("Mremark") %>' />
                                                <asp:Label ID="lblSGST" runat="server" Visible="false" Text='<%# Bind("SGST") %>' />
                                                <asp:Label ID="lblIGST" runat="server" Visible="false" Text='<%# Bind("IGST") %>' />
                                                <asp:Label ID="lblTCS" runat="server" Visible="false" Text='<%# Bind("TCS") %>' />
                                                <asp:Label ID="lblPenalityRemarks" runat="server" Visible="false" Text='<%# Bind("PenalityRemarks") %>' />
                                            </ItemTemplate>
                                            <HeaderStyle Width="80px" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Bell Wt." Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblbellwt" runat="server" Text='<%# Bind("Bellwt") %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="BillQty" Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblBillQty" runat="server" Text='<%# Bind("BillQty") %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="BuiltyAmt" Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblBuiltyAmt" runat="server" Text='<%# Bind("TransportBuiltyAmt") %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="UnloadExp" Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblUnloadingExpenses" runat="server" Text='<%# Bind("UnloadingExpenses") %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Particular" Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblParticular" runat="server" Text='<%# Bind("Particular") %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="50MWait" Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblFiftyMWait" runat="server" Text='<%# Bind("FiftyMWait") %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="YarnShape" Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblYarnShape" runat="server" Text='<%# Bind("YarnShape") %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Rate">
                                            <EditItemTemplate>
                                                <asp:TextBox ID="TxtDGRate" Text='<%#Bind("Rate") %>' Width="50px" runat="server" />
                                            </EditItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField ShowHeader="False">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="LinkButton1" runat="server" CausesValidation="False" CommandName="Delete"
                                                    Width="50px" Text="DEL" OnClientClick="return doconfirm('Do you want to Delete data?')"
                                                    CssClass="buttonnorm"></asp:LinkButton>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:CommandField DeleteText="" ShowEditButton="true" />
                                    </Columns>
                                    <RowStyle CssClass="gvrow" />
                                    <EmptyDataRowStyle CssClass="gvemptytext" />
                                </asp:GridView>
                            </div>
                        </td>
                        <td colspan="1">
                            <div id="gride" runat="server">
                                <asp:GridView ID="DGSHOWDATA" runat="server" AutoGenerateColumns="False" AllowPaging="true"
                                    PageSize="10" OnPageIndexChanging="DGSHOWDATA_PageIndexChanging" CssClass="grid-view"
                                    OnRowCreated="DGSHOWDATA_RowCreated" OnRowDataBound="DGSHOWDATA_RowDataBound"
                                    OnSelectedIndexChanged="DGSHOWDATA_SelectedIndexChanged">
                                    <Columns>
                                        <%--<asp:BoundField DataField="PRODUCTCODE" HeaderText="PRODUCTCODE" />--%>
                                        <asp:TemplateField HeaderText="PRODUCTCODE">
                                            <ItemTemplate>
                                                <asp:Label ID="lblPRODUCTCODE" runat="server" Text='<%# Bind("PRODUCTCODE") %>' />
                                            </ItemTemplate>
                                            <HeaderStyle Width="80px" />
                                        </asp:TemplateField>
                                    </Columns>
                                    <PagerStyle CssClass="PagerStyle" />
                                </asp:GridView>
                            </div>
                        </td>
                        <td colspan="3" id="tdporder" runat="server" visible="false" align="right">
                            <div style="width: 100%; height: 250px; overflow: scroll;">
                                <asp:GridView ID="DGporder" runat="server" AutoGenerateColumns="False" CssClass="grid-view"
                                    AutoGenerateSelectButton="true" DataKeyNames="finishedid" OnRowDataBound="DGporder_RowDataBound"
                                    OnSelectedIndexChanged="DGporder_SelectedIndexChanged">
                                    <Columns>
                                        <asp:BoundField DataField="DESCRIPTION" HeaderText="Description">
                                            <HeaderStyle Width="200px" HorizontalAlign="left" />
                                            <ItemStyle HorizontalAlign="Left" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="Qty" HeaderText="Qty" />
                                        <asp:BoundField DataField="RECQty" HeaderText="Rec Qty" />
                                        <asp:BoundField DataField="Canqty" HeaderText="Cancel Qty" />
                                        <asp:TemplateField HeaderText="Balance">
                                            <ItemTemplate>
                                                <asp:Label ID="lblfinishedid" runat="server" Text='<%# Bind("finishedid") %>' Visible="false"></asp:Label>
                                                <asp:Label ID="lblcategoryid" runat="server" Text='<%# Bind("category_id") %>' Visible="false"></asp:Label>
                                                <asp:Label ID="lblitem_id" runat="server" Text='<%# Bind("item_id") %>' Visible="false"></asp:Label>
                                                <asp:Label ID="lblQualityid" runat="server" Text='<%# Bind("Qualityid") %>' Visible="false"></asp:Label>
                                                <asp:Label ID="lblColorid" runat="server" Text='<%# Bind("Colorid") %>' Visible="false"></asp:Label>
                                                <asp:Label ID="lbldesignid" runat="server" Text='<%# Bind("designid") %>' Visible="false"></asp:Label>
                                                <asp:Label ID="lblshapeid" runat="server" Text='<%# Bind("shapeid") %>' Visible="false"></asp:Label>
                                                <asp:Label ID="lblshadecolorid" runat="server" Text='<%# Bind("shadecolorid") %>'
                                                    Visible="false"></asp:Label>
                                                <asp:Label ID="lblsizeid" runat="server" Text='<%# Bind("sizeid") %>' Visible="false"></asp:Label>
                                                <asp:Label ID="lblqty" runat="server" Text='<%# Bind("Qty") %>' Visible="false"></asp:Label>
                                                <asp:Label ID="lblrecqty" runat="server" Text='<%# Bind("RECQty") %>' Visible="false"></asp:Label>
                                                <asp:Label ID="lblcanqty" runat="server" Text='<%# Bind("canqty") %>' Visible="false"></asp:Label>
                                                <asp:Label ID="lblorderid" runat="server" Text='<%# Bind("orderid") %>' Visible="false"></asp:Label>
                                                <asp:Label ID="lbllotno" runat="server" Text='<%# Bind("Lotno") %>' Visible="false"></asp:Label>
                                                <asp:Label ID="lblbalnce" runat="server" Text='<%# getgiven(DataBinder.Eval(Container.DataItem, "Qty").ToString(),DataBinder.Eval(Container.DataItem, "RECQty").ToString(),DataBinder.Eval(Container.DataItem, "canqty").ToString()) %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                    <HeaderStyle CssClass="gvheaders" />
                                    <AlternatingRowStyle CssClass="gvalts" />
                                    <RowStyle CssClass="gvrow" />
                                    <EmptyDataRowStyle CssClass="gvemptytext" />
                                </asp:GridView>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:HiddenField ID="hnprid" runat="server" />
                            <asp:HiddenField ID="hncomp" runat="server" />
                            <asp:HiddenField ID="hnqty" runat="server" />
                            <asp:HiddenField ID="HnForDeleteCommand" runat="server" />
                        </td>
                    </tr>
                </table>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
