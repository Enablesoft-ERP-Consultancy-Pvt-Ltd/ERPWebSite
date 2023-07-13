<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PurchageIndentIssue.aspx.cs"
    Title="Purchase Order" EnableEventValidation="false" MasterPageFile="~/ERPmaster.master"
    Inherits="Masters_Purchase_PurchageIndentIssue" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="page" ContentPlaceHolderID="CPH_Form" runat="server">
    <script src="../../Scripts/JScript.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery-1.4.1.js" type="text/javascript"></script>
    <%--<script type="text/javascript" src="../../Scripts/Fixfocus.js"></script>--%>
    <script type="text/javascript">
        function Addquality() {
            var answer = confirm("Do you want to ADD?")
            var left = (screen.width / 2) - (701 / 2);
            var top = (screen.height / 2) - (501 / 2);
            if (answer) {
                var varcode = document.getElementById('CPH_Form_ddCatagory').value;
                var varcode1 = document.getElementById('CPH_Form_dditemname').value;
                window.open('../Carpet/AddQuality.aspx?Category=' + varcode + '&Item=' + varcode1 + '', '', 'width=701px,Height=501px,top=' + top + ',left=' + left);
            }
        }
        function priview() {
            window.open('../../ReportViewer.aspx', '');
        }
        function CloseForm() {
            window.location.href = "../../main.aspx";
        }
        function NewForm() {
            window.location.href = "PurchageIndentIssue.aspx";
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
        function AddDeliveryTerms() {
            window.open('../Campany/AddTerm.aspx', '', 'width=501px,Height=401px', 'resizeable=yes');
        }

        function AddPaymentMode() {
            window.open('../Campany/AddPaymentDetail.aspx', '', 'width=501px,Height=401px', 'resizeable=yes');
        }

        function CHeckDate() {
            var ODate = document.getElementById("<%=txtdate.ClientID %>").value;
            var DDate = document.getElementById("<%=txtcomp_date.ClientID %>").value;
            var DelDate = document.getElementById("<%=txtduedate.ClientID %>").value;
            var day1 = ODate.substring(0, 2);
            var month1 = ODate.substring(3, 6);
            var year1 = ODate.substring(7, 11);

            var day2 = DDate.substring(0, 2);
            var month2 = DDate.substring(3, 6);
            var year2 = DDate.substring(7, 11);

            var day3 = DelDate.substring(0, 2);
            var month3 = DelDate.substring(3, 6);
            var year3 = DelDate.substring(7, 11);

            month1 = changeFormatStringtoNumber(month1);
            month2 = changeFormatStringtoNumber(month2);
            month3 = changeFormatStringtoNumber(month3);
            //            alert(month2);
            var d1 = new Date(year1, month1, day1);
            var d2 = new Date(year2, month2, day2);
            var d3 = new Date(year3, month3, day3);
            if (d1 > d2) {
                alert("Delivery Date Should be Grater than Order Date! ");
                document.getElementById("<%=txtcomp_date.ClientID %>").value = ODate;
                //                document.getElementById("<%=txtduedate.ClientID %>").value = ODate;
                return false;
            }
            else if (d2 > d3) {
                alert("Due Date can not be shorter than Delivery Date! ");
                document.getElementById("<%=txtduedate.ClientID %>").value = DDate;

                return false;
            }
        }
        function changeFormatStringtoNumber(monthval) {
            // var m=0;
            if (monthval == "Jan") {
                monthval = 0;
            }
            else if (monthval == "Feb") {
                monthval = 1;
            }
            else if (monthval == "Mar") {
                monthval = 2;
            }
            else if (monthval == "Apr") {
                monthval = 3;
            }
            else if (monthval == "May") {
                monthval = 4;
            }
            else if (monthval == "Jun") {
                monthval = 5;
            }
            else if (monthval == "Jul") {
                monthval = 6;
            }
            else if (monthval == "Aug") {
                monthval = 7;
            }
            else if (monthval == "Sep") {
                monthval = 8;
            }
            else if (monthval == "Oct") {
                monthval = 9;
            }
            else if (monthval == "NOV") {
                monthval = 10;
            }
            else if (monthval == "Nov") {
                monthval = 10;
            }
            else if (monthval == "Dec") {
                monthval = 11;
            }
            return (monthval);
        }

        function OrderDetail() {
            var e = document.getElementById("CPH_Form_ddempname");
            var varcode = e.options[e.selectedIndex].value;
            if (varcode > 0) {
                window.open('../order/frmorderdetail.aspx?Vendor=' + varcode + '&Type=PO', '', 'width=950px,Height=500px');
            }
            else {
                alert("Plz select Vendor name");
            }
        }
        function OrderUnCompleteValidate() {
            if (document.getElementById("<%=ChkEditOrder.ClientID %>").checked == false) {
                alert("Pls Check For Edit Order");
                document.getElementById("<%=ChkEditOrder.ClientID %>").focus();
                return false;
            }
            if (document.getElementById("<%=ddCompName.ClientID %>").value <= "0") {
                alert("Plz select Comapny Name");
                document.getElementById("<%=ddCompName.ClientID %>").focus();
                return false;
            }
            if (document.getElementById("<%=ddempname.ClientID %>").value <= "0") {
                alert("Plz select Vendor Name");
                document.getElementById("<%=ddempname.ClientID %>").focus();
                return false;
            }
            if (document.getElementById("<%=DDPoNoNew.ClientID %>").value <= "0") {
                alert("Plz select p.o no new");
                document.getElementById("<%=DDPoNoNew.ClientID %>").focus();
                return false;
            }
            return confirm('Do You Want To Complete Purchase Order?')
        }
        function OrderCompleteValidate() {
            if (document.getElementById("<%=ChkEditOrder.ClientID %>").checked == false) {
                alert("Pls Check For Edit Order");
                document.getElementById("<%=ChkEditOrder.ClientID %>").focus();
                return false;
            }
            if (document.getElementById("<%=ddCompName.ClientID %>").value <= "0") {
                alert("Plz select Comapny Name");
                document.getElementById("<%=ddCompName.ClientID %>").focus();
                return false;
            }
            if (document.getElementById("<%=ddempname.ClientID %>").value <= "0") {
                alert("Plz select Vendor Name");
                document.getElementById("<%=ddempname.ClientID %>").focus();
                return false;
            }
            if (document.getElementById("<%=ddlchalanno.ClientID %>").value <= "0") {
                alert("Plz select P.O No");
                document.getElementById("<%=ddlchalanno.ClientID %>").focus();
                return false;
            }
            return confirm('Do You Want To Complete Purchase Order?')
        }
        function DefineItemcode() {

            window.open('../Carpet/DefineItemCode.aspx?ABC=1', '', 'width=950px,Height=500px');
        }
        function validate() {
            if (document.getElementById("<%=ddCompName.ClientID %>").value <= "0") {
                alert("Plz select Comapny Name");
                return false;
            }
            if (document.getElementById('CPH_Form_ddcustomercode') != null) {
                if (document.getElementById('CPH_Form_ddcustomercode').options.length == 0) {
                    alert("CustomerCode  must have a value....!");
                    document.getElementById("CPH_Form_ddcustomercode").focus();
                    return false;
                }
                else if (document.getElementById('CPH_Form_ddcustomercode').options[document.getElementById('CPH_Form_ddcustomercode').selectedIndex].value == 0) {
                    alert("Please select CustomerCode....!");
                    document.getElementById("CPH_Form_ddcustomercode").focus();
                    return false;
                }
            }
            if (document.getElementById('CPH_Form_ddorderno') != null) {
                if (document.getElementById('CPH_Form_ddorderno').options.length == 0) {
                    alert("OrderNo  must have a value....!");
                    document.getElementById("CPH_Form_ddorderno").focus();
                    return false;
                }
                else if (document.getElementById('CPH_Form_ddorderno').options[document.getElementById('CPH_Form_ddorderno').selectedIndex].value == 0) {
                    alert("Please select OrderNo....!");
                    document.getElementById("CPH_Form_ddorderno").focus();
                    return false;
                }
            }
            if (document.getElementById('CPH_Form_ddindentno') != null) {
                if (document.getElementById('CPH_Form_ddindentno').options.length == 0) {
                    alert("Indent No.  must have a value....!");
                    document.getElementById("CPH_Form_ddindentno").focus();
                    return false;
                }
                else if (document.getElementById('CPH_Form_ddindentno').options[document.getElementById('CPH_Form_ddindentno').selectedIndex].value == 0) {
                    alert("Please select IndentNo....!");
                    document.getElementById("CPH_Form_ddindentno").focus();
                    return false;
                }
            }
            if (document.getElementById('CPH_Form_ddlchalanno') != null) {
                if (document.getElementById('CPH_Form_ddlchalanno').options.length == 0) {
                    alert("PO No.  must have a value....!");
                    document.getElementById("CPH_Form_ddlchalanno").focus();
                    return false;
                }
                else if (document.getElementById('CPH_Form_ddlchalanno').options[document.getElementById('CPH_Form_ddlchalanno').selectedIndex].value == 0) {
                    alert("Please select PO No....!");
                    document.getElementById("CPH_Form_ddlchalanno").focus();
                    return false;
                }
            }
            if (document.getElementById("<%=ddempname.ClientID %>").value <= "0") {
                alert("Plz select Vendor Name");
                return false;
            }

            if (document.getElementById("<%=ChkEditOrder.ClientID %>").checked == false) {
                if (document.getElementById('CPH_Form_ddCatagory').value <= "0") {
                    alert("Please Select Category Name....!");
                    document.getElementById("CPH_Form_ddCatagory").focus();
                    return false;
                }
                if (document.getElementById('CPH_Form_dditemname').value <= "0") {
                    alert("Please Select Item Name....!");
                    document.getElementById("CPH_Form_dditemname").focus();
                    return false;
                }
                if (document.getElementById("<%=ql.ClientID %>")) {
                    if (document.getElementById('CPH_Form_dquality').value <= "0") {
                        alert("Please Select Quality Name....!");
                        document.getElementById("CPH_Form_dquality").focus();
                        return false;
                    }
                }
                if (document.getElementById("<%=dsn.ClientID %>")) {
                    if (document.getElementById('CPH_Form_dddesign').options[document.getElementById('CPH_Form_dddesign').selectedIndex].value <= 0) {
                        alert("Please Select Design Name....!");
                        document.getElementById("CPH_Form_dddesign").focus();
                        return false;
                    }
                }
                if (document.getElementById("<%=clr.ClientID %>")) {
                    if (document.getElementById('CPH_Form_ddcolor').options[document.getElementById('CPH_Form_ddcolor').selectedIndex].value <= 0) {
                        alert("Please Select Colour Name....!");
                        document.getElementById("CPH_Form_ddcolor").focus();
                        return false;
                    }
                }
                if (document.getElementById("<%=shd.ClientID %>")) {
                    if (document.getElementById('CPH_Form_ddlshade').options[document.getElementById('CPH_Form_ddlshade').selectedIndex].value <= 0) {
                        alert("Please Select Shade Color Name....!");
                        document.getElementById("CPH_Form_ddlshade").focus();
                        return false;
                    }
                }
                if (document.getElementById("<%=shp.ClientID %>")) {
                    if (document.getElementById('CPH_Form_ddshape').options[document.getElementById('CPH_Form_ddshape').selectedIndex].value <= 0) {
                        alert("Please Select Shape Name....!");
                        document.getElementById("CPH_Form_ddshape").focus();
                        return false;
                    }
                }
                if (document.getElementById("<%=sz.ClientID %>")) {
                    if (document.getElementById('CPH_Form_ddsize').options[document.getElementById('CPH_Form_ddsize').selectedIndex].value <= 0) {
                        alert("Please Select Size Name....!");
                        document.getElementById("CPH_Form_ddsize").focus();
                        return false;
                    }
                }
            }
            if (document.getElementById("<%=ChkEditOrder.ClientID %>").checked == false) {
                if (document.getElementById("<%=ddlunit.ClientID %>").value <= "0") {
                    alert("Pls Select Unit Name");
                    return false;
                }
            }
            if (document.getElementById("<%=ChkEditOrder.ClientID %>").checked == false) {
                if (document.getElementById("<%=txtqty.ClientID %>").value == "" || document.getElementById("<%=txtqty.ClientID %>").value == "0") {
                    alert("Issue QTY Cannot Be Blank");
                    document.getElementById("<%=txtqty.ClientID %>").focus();
                    return false;
                }
            }
            return confirm('Do You Want To Save?')
        }
        function AddAddEmp() {
            window.open('../Campany/frmWeaver.aspx?ABC=1', '', 'Height=600px,width=1000px');
        }
    </script>
    <asp:UpdatePanel ID="R" runat="server">
        <ContentTemplate>
            <div>
                <div style="width: 1060px;">
                    <div style="float: left; width: 930px;">
                        <asp:Panel ID="pnl1" runat="server">
                            <div style="width: 930px; background-color: #8B7B8B;">
                                <table>
                                    <tr>
                                        <td class="tdstyle">
                                            <asp:CheckBox ID="ChkEditOrder" runat="server" Text="EDIT ORDER" AutoPostBack="True"
                                                ForeColor="White" OnCheckedChanged="ChkEditOrder_CheckedChanged" CssClass="checkboxbold" />
                                        </td>
                                        <td id="chkindent" runat="server">
                                            <asp:CheckBox ID="chkindentvise" runat="server" AutoPostBack="True" Enabled="false"
                                                OnCheckedChanged="chkindentvise_CheckedChanged" Text="INDENT WISE" CssClass="checkboxbold"
                                                ForeColor="White" />
                                        </td>
                                        <td>
                                            <asp:CheckBox ID="chkcustomervise" runat="server" AutoPostBack="True" OnCheckedChanged="chkcustomervise_CheckedChanged"
                                                Text="CUSTOMER WISE" CssClass="checkboxbold" ForeColor="White" />
                                        </td>
                                        <td>
                                            <asp:CheckBox ID="chkforsample" runat="server" Visible="false"
                                                Text="Sample Order" CssClass="checkboxbold" Font-Bold="true" ForeColor="White"
                                                AutoPostBack="True" OnCheckedChanged="chkforsample_CheckedChanged" />
                                        </td>
                                        <td id="TdrequirDate" runat="server" visible="false">
                                            <asp:Label ID="lblreqdate" Font-Bold="true" ForeColor="Red" runat="server" Text=""></asp:Label>
                                        </td>
                                        <td id="TdWithoutOrder" runat="server" visible="false">
                                            <asp:CheckBox ID="ChkWithoutOrder" runat="server" AutoPostBack="True" Enabled="true"
                                                Text="WITHOUT ORDER" CssClass="checkboxbold" Font-Bold="true" ForeColor="White" />
                                        </td>
                                        <td id="TDCheckForComplete" runat="server" visible="false">
                                            <asp:CheckBox ID="ChkForComplete" runat="server" Text="Check For Complete" CssClass="checkboxbold"
                                                Font-Bold="true" ForeColor="White" />
                                        </td>
                                        <td id="TDChkForSampleFlag" runat="server" visible="false">
                                            <asp:CheckBox ID="ChkForForSampleFlag" runat="server" Text="For Sample" CssClass="checkboxbold"
                                                Font-Bold="true" ForeColor="White" AutoPostBack="True" OnCheckedChanged="ChkForForSampleFlag_CheckedChanged"/>
                                        </td>
                                        <td class="tdstyle" id="TDPoNoNew" runat="server" visible="false">
                                            <asp:DropDownList ID="DDPoNoNew" runat="server" CssClass="dropdown" Width="110px">
                                            </asp:DropDownList>
                                            <asp:Button ID="BtnOrderUnComplete" runat="server" CssClass="buttonnorm" OnClick="BtnOrderUnComplete_Click"
                                                OnClientClick="return OrderUnCompleteValidate()" Text="Order Un Complete" />
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtpindentissueid" Visible="false" Width="0px" runat="server"></asp:TextBox>
                                            <asp:TextBox ID="txtpindentissuedetailid" runat="server" Width="0px" Visible="false"></asp:TextBox>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </asp:Panel>
                        <fieldset>
                            <legend>
                                <asp:Label ID="lblMasterDetail" runat="server" Text="Master Details" CssClass="labelbold"
                                    ForeColor="Red" Font-Bold="true"></asp:Label></legend>
                            <table>
                                <tr id="Tr1" runat="server">
                                    <td id="tdchalanno" runat="server" visible="false" class="tdstyle">
                                        <asp:Label ID="Label7" runat="server" Text="PO No." CssClass="labelbold"></asp:Label><br />
                                        <asp:TextBox ID="txtchalan_no" runat="server" Width="100px" AutoPostBack="True" CssClass="textb"
                                            OnTextChanged="Txtchalan_no_TextChanged"></asp:TextBox>
                                    </td>
                                    <td id="tdIndenttext" runat="server" visible="false" class="tdstyle">
                                        <asp:Label ID="Label8" runat="server" Text="Indent No." CssClass="labelbold"></asp:Label><br />
                                        <asp:TextBox ID="TxtIndentNo" runat="server" Width="100px" AutoPostBack="True" CssClass="textb"
                                            OnTextChanged="TxtIndentNo_TextChanged"></asp:TextBox>
                                    </td>
                                    <td id="Td1" class="tdstyle">
                                        <asp:Label ID="Label9" runat="server" Text="Company Name" CssClass="labelbold"></asp:Label><br />
                                        <asp:DropDownList ID="ddCompName" runat="server" CssClass="dropdown" OnSelectedIndexChanged="ddCompName_SelectedIndexChanged"
                                            AutoPostBack="true" Width="150px">
                                        </asp:DropDownList>
                                    </td>
                                    <td id="TDBranchName" runat="server" class="tdstyle">
                                        <asp:Label ID="Label64" runat="server" Text="Branch Name" CssClass="labelbold"></asp:Label><br />
                                        <asp:DropDownList ID="DDBranchName" runat="server" CssClass="dropdown" Width="150px">
                                        </asp:DropDownList>
                                    </td>
                                    <td id="tdcustomer" runat="server" visible="false" class="tdstyle">
                                        <asp:Label ID="Label10" runat="server" Text="Customer Code" CssClass="labelbold"></asp:Label><br />
                                        <asp:DropDownList ID="ddcustomercode" runat="server" CssClass="dropdown" AutoPostBack="True"
                                            OnSelectedIndexChanged="ddcustomercode_SelectedIndexChanged" Width="150px">
                                        </asp:DropDownList>
                                    </td>
                                    <td id="tdorderno" runat="server" visible="false" class="tdstyle">
                                        <asp:Label ID="Label11" runat="server" Text="Order No." CssClass="labelbold"></asp:Label><br />
                                        <asp:DropDownList ID="ddorderno" runat="server" CssClass="dropdown" AutoPostBack="True"
                                            OnSelectedIndexChanged="ddorderno_SelectedIndexChanged" Width="150px">
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                        <asp:Label ID="Label61" Text="Order No" runat="server" CssClass="labelbold"> </asp:Label>
                                        <br />
                                        <asp:TextBox ID="TxtOrderNo" Width="100px" runat="server" CssClass="textb" onkeydown="return (event.keyCode!=13);"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td id="Td4" class="tdstyle">
                                        <asp:Label ID="Label12" runat="server" Text="Vendor Name" CssClass="labelbold"></asp:Label>
                                        <b style="color: Red">*</b><asp:Button ID="BtnAddEmp" runat="server" Text="ADD" CssClass="buttonsmalls"
                                            Width="40px" OnClientClick="return AddAddEmp()" /><br />
                                        <asp:DropDownList ID="ddempname" runat="server" CssClass="dropdown" AutoPostBack="True"
                                            Width="150px" OnSelectedIndexChanged="ddempname_SelectedIndexChanged">
                                        </asp:DropDownList>
                                        <asp:Button ID="refreshEmp2" runat="server" Visible="true" Text="" BorderWidth="0px"
                                            Height="1px" Width="1px" BackColor="White" BorderColor="White" BorderStyle="None"
                                            ForeColor="White" OnClick="refreshEmp_Click" />
                                    </td>
                                    <td id="tdindentno" runat="server" visible="false" class="tdstyle">
                                        <asp:Label ID="Label13" runat="server" Text="Indent No." CssClass="labelbold"></asp:Label><br />
                                        <asp:DropDownList ID="ddindentno" runat="server" CssClass="dropdown" AutoPostBack="True"
                                            OnSelectedIndexChanged="ddindentno_SelectedIndexChanged" Width="100px">
                                        </asp:DropDownList>
                                    </td>
                                    <td id="tdchalan" runat="server" visible="false" class="tdstyle">
                                        <asp:Label ID="Label14" runat="server" Text="P.O. No." CssClass="labelbold"></asp:Label><br />
                                        <asp:DropDownList ID="ddlchalanno" runat="server" CssClass="dropdown" AutoPostBack="True"
                                            OnSelectedIndexChanged="ddlchalanno_SelectedIndexChanged" Width="150px">
                                        </asp:DropDownList>
                                    </td>
                                    <td runat="server" id="Tdlegalvendor" visible="false">
                                        <asp:Label ID="Label60" runat="server" Text="Legal Vendor" CssClass="labelbold"></asp:Label>
                                        <br />
                                        <asp:DropDownList CssClass="dropdown" ID="DDlegalvendor" Width="200px" runat="server">
                                        </asp:DropDownList>
                                    </td>
                                    <td id="Td5" align="center" class="tdstyle">
                                        <asp:Label ID="Label15" runat="server" Text="Order Date" CssClass="labelbold"></asp:Label><br />
                                        <asp:TextBox ID="txtdate" runat="server" Width="90px" AutoPostBack="True" CssClass="textb"
                                            onchange="return CHeckDate();"></asp:TextBox>
                                        <%-- <asp:RequiredFieldValidator SetFocusOnError="true" ID="RequiredFieldValidator1" runat="server"
                                            ErrorMessage="please Enter Date" ControlToValidate="txtdate" ValidationGroup="f1"
                                            ForeColor="Red">*</asp:RequiredFieldValidator>--%>
                                        <asp:CalendarExtender ID="CalendarExtender1" runat="server" Format="dd-MMM-yyyy"
                                            TargetControlID="txtdate">
                                        </asp:CalendarExtender>
                                    </td>
                                    <td class="tdstyle">
                                        <asp:Label ID="Label49" runat="server" Text="Due Date" CssClass="labelbold"></asp:Label><br />
                                        <asp:TextBox ID="txtduedate" runat="server" CssClass="textb" Width="90px" AutoPostBack="true"
                                            OnTextChanged="txtduedate_TextChanged" BackColor="#8B7B8B" onchange="return CHeckDate();"></asp:TextBox>
                                        <asp:CalendarExtender ID="CalendarExtender2" runat="server" Format="dd-MMM-yyyy"
                                            TargetControlID="txtduedate">
                                        </asp:CalendarExtender>
                                        <b style="color: Red">&nbsp; *</b>
                                    </td>
                                    <td id="TDManualOrderNo" class="tdstyle" runat="server" visible="false">
                                        <asp:Label ID="Label58" Text="Manual Order No." runat="server" CssClass="labelbold"> </asp:Label>
                                        <br />
                                        <asp:TextBox ID="txtManualChallanNo" Width="100px" runat="server" CssClass="textb"
                                            onkeydown="return (event.keyCode!=13);"></asp:TextBox>
                                    </td>
                                    <td id="Td6" class="tdstyle">
                                        <asp:Label ID="LblorderNo" Text="Order No." runat="server" CssClass="labelbold"> </asp:Label>
                                        <br />
                                        <asp:TextBox ID="txtchalanno" Width="100px" runat="server" CssClass="textb" onkeydown="return (event.keyCode!=13);"></asp:TextBox>
                                        <asp:Button ID="btnopen" runat="server" Width="80px" Text="ADD ITEMS" CssClass="buttonnorm"
                                            OnClientClick=" return DefineItemcode()" />
                                    </td>
                                    <td>
                                        <asp:HiddenField ID="hncunsp" runat="server" />
                                    </td>
                                </tr>
                            </table>
                        </fieldset>
                        <fieldset>
                            <legend>
                                <asp:Label ID="Label5" runat="server" Text="Item Details" CssClass="labelbold" ForeColor="Red"
                                    Font-Bold="true"></asp:Label></legend>
                            <table>
                                <tr id="Tr3" runat="server">
                                    <td id="tdProCode" runat="server" visible="false" class="tdstyle">
                                        <span class="labelbold">ProdCode</span>
                                        <br />
                                        <asp:TextBox ID="TxtProdCode" CssClass="textb" runat="server" Width="100px" AutoPostBack="True"
                                            OnTextChanged="TxtProdCode_TextChanged"></asp:TextBox>
                                        <cc1:AutoCompleteExtender ID="TxtProdCode_AutoCompleteExtender" runat="server" EnableCaching="true"
                                            Enabled="True" MinimumPrefixLength="1" ServiceMethod="GetQuality" TargetControlID="TxtProdCode"
                                            UseContextKey="True">
                                        </cc1:AutoCompleteExtender>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblcategoryname" runat="server" class="tdstyle" Text="Category Name"
                                            CssClass="labelbold"></asp:Label>
                                        <br />
                                        <asp:DropDownList ID="ddCatagory" runat="server" Width="125px" CssClass="dropdown"
                                            AutoPostBack="True" OnSelectedIndexChanged="ddCatagory_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblitemname" class="tdstyle" runat="server" Text="Item Name" CssClass="labelbold"></asp:Label>
                                        <br />
                                        <asp:DropDownList ID="dditemname" runat="server" Width="150px" CssClass="dropdown"
                                            AutoPostBack="True" OnSelectedIndexChanged="dditemname_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </td>
                                    <td id="ql" runat="server" visible="false">
                                        <asp:Label ID="lblqualityname" class="tdstyle" runat="server" Text="Quality" CssClass="labelbold"></asp:Label>
                                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                        <asp:Button ID="btnaddquality" runat="server" CssClass="buttonsmall" OnClientClick="return Addquality();"
                                            Text="&#43;" />
                                        <br />
                                        <asp:DropDownList ID="dquality" runat="server" CssClass="dropdown" AutoPostBack="True"
                                            OnSelectedIndexChanged="dquality_SelectedIndexChanged" Width="150px">
                                        </asp:DropDownList>
                                        <asp:Button ID="refreshquality" runat="server" Style="display: none" OnClick="refreshquality_Click" />
                                    </td>
                                    <td id="dsn" runat="server" visible="false" class="style5">
                                        <asp:Label ID="lbldesignname" runat="server" class="tdstyle" Text="Design" CssClass="labelbold"></asp:Label>
                                        <br />
                                        <asp:DropDownList ID="dddesign" runat="server" Width="150px" CssClass="dropdown"
                                            AutoPostBack="True" OnSelectedIndexChanged="dddesign_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </td>
                                    <td id="clr" runat="server" visible="false">
                                        <asp:Label ID="lblcolorname" runat="server" class="tdstyle" Text="Color" CssClass="labelbold"></asp:Label>
                                        <br />
                                        <asp:DropDownList ID="ddcolor" runat="server" Width="130px" CssClass="dropdown" AutoPostBack="True"
                                            OnSelectedIndexChanged="ddcolor_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td id="shp" runat="server" visible="false">
                                        <asp:Label ID="lblshapename" class="tdstyle" runat="server" Text="Shape" CssClass="labelbold"></asp:Label>
                                        <br />
                                        <asp:DropDownList ID="ddshape" runat="server" Width="100px" AutoPostBack="True" CssClass="dropdown"
                                            OnSelectedIndexChanged="ddshape_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </td>
                                    <td id="sz" runat="server" class="tdstyle" visible="false">
                                        <asp:Label ID="lblsizename" runat="server" Text="Size" CssClass="labelbold"></asp:Label>
                                        <%--<asp:CheckBox ID="ChkForMtr" runat="server" AutoPostBack="True" OnCheckedChanged="ChkForMtr_CheckedChanged" />Check
                                    for Mtr--%><asp:DropDownList CssClass="dropdown" Width="50" ID="DDsizetype" runat="server"
                                        AutoPostBack="True" OnSelectedIndexChanged="DDsizetype_SelectedIndexChanged">
                                        <%--  <asp:ListItem Value="0" Selected="True">Ft</asp:ListItem>
                                        <asp:ListItem Value="1">MTR</asp:ListItem>
                                        <asp:ListItem Value="2">Inch</asp:ListItem>--%>
                                    </asp:DropDownList>
                                        <br />
                                        <asp:DropDownList ID="ddsize" runat="server" Width="125px" CssClass="dropdown" AutoPostBack="True"
                                            OnSelectedIndexChanged="ddsize_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </td>
                                    <td id="shd" runat="server" visible="false">
                                        <asp:Label ID="lblshadecolor" runat="server" class="tdstyle" Text="ShadeColor" CssClass="labelbold"></asp:Label>
                                        &nbsp;<br />
                                        <asp:DropDownList ID="ddlshade" runat="server" Width="150px" CssClass="dropdown"
                                            OnSelectedIndexChanged="ddlshade_SelectedIndexChanged" AutoPostBack="True">
                                        </asp:DropDownList>
                                    </td>
                                    <td id="TDshdNew" runat="server" visible="false">
                                        <asp:Label ID="lblshadecolorNew" runat="server" class="tdstyle" Text="ShadeColor"
                                            CssClass="labelbold"></asp:Label>
                                        &nbsp;<br />
                                        <asp:DropDownList ID="ddlshadeNew" runat="server" Width="150px" CssClass="dropdown">
                                        </asp:DropDownList>
                                    </td>
                                    <td id="TdFinish_Type" runat="server" visible="false" class="tdstyle">
                                        <asp:Label ID="LblFinish_Type" runat="server" Text="Finish_Type" CssClass="labelbold"></asp:Label>
                                        &nbsp;<br />
                                        <asp:DropDownList ID="ddFinish_Type" runat="server" Width="150px" CssClass="dropdown"
                                            AutoPostBack="True">
                                        </asp:DropDownList>
                                    </td>
                                    <td class="tdstyle">
                                        <asp:Label ID="Label16" runat="server" Text="Unit" CssClass="labelbold"></asp:Label>
                                        <br />
                                        <asp:DropDownList ID="ddlunit" runat="server" Width="115px" AutoPostBack="true" CssClass="dropdown">
                                        </asp:DropDownList>
                                    </td>
                                    <td id="trimage" runat="server" visible="true">
                                        <asp:Image ID="lblimage" runat="server" Height="66px" Width="132px" />
                                    </td>
                                    <td class="tdstyle">
                                        <asp:FileUpload ID="PhotoImage" onchange="PreviewImg(this)" ViewStateMode="Enabled"
                                            runat="server" TabIndex="49" />
                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ErrorMessage="Only .gif,.jpeg or.jpg files are allowed!"
                                            ValidationExpression="^.*\.(jpg|JPG|gif|GIF|jpeg|JPEG|BMP|bmp)$" ControlToValidate="PhotoImage"></asp:RegularExpressionValidator>
                                    </td>
                                   
                                </tr>
                                  <tr runat="server" id="trrevisedremark" visible="false">
                                            <td>
                                                <asp:Label ID="Label52" runat="server" Text="Item Remarks" CssClass="labelbold"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="TxtItemRemark" runat="server" Width="250px" CssClass="textboxremark"
                                                    TextMode="MultiLine"></asp:TextBox>
                                            </td>
                                            <td>
                                            </td>
                                            <td id="revisedremark" runat="server" class="tdstyle" visible="false">
                                                <asp:Label ID="Label53" runat="server" CssClass="labelbold" Text="Revised Remarks"></asp:Label>
                                                <asp:TextBox ID="TXTreviseRemark" runat="server" CssClass="textboxremark" TextMode="MultiLine"
                                                    Width="250px"></asp:TextBox>
                                            </td>
                                    </tr>
                            </table>
                        </fieldset>
                        <fieldset>
                            <legend>
                                <asp:Label ID="Label6" runat="server" Text="...." CssClass="labelbold" ForeColor="Red"
                                    Font-Bold="true"></asp:Label></legend>
                            <table>
                                <tr>
                                    <td id="AQty" runat="server" visible="false" class="tdstyle">
                                        <asp:Label ID="Label17" runat="server" Text="Approved Qty." CssClass="labelbold"></asp:Label>
                                        <br />
                                        <asp:TextBox ID="TxtApprovedQty" runat="server" Enabled="false" Width="90px" CssClass="textb"></asp:TextBox>
                                        <%--   <asp:RequiredFieldValidator SetFocusOnError="true" ID="RequiredFieldValidator4" runat="server"
                                            ErrorMessage="please Enter Date" ControlToValidate="txtqty" ValidationGroup="f1"
                                            ForeColor="Red">*</asp:RequiredFieldValidator>--%>
                                    </td>
                                    <td id="PQty" runat="server" visible="false" class="tdstyle">
                                        <asp:Label ID="Label18" runat="server" Text="PreIssue Qty." CssClass="labelbold"></asp:Label>
                                        <br />
                                        <asp:TextBox ID="TxtPreIssueQty" runat="server" Enabled="false" Width="95px" CssClass="textb"></asp:TextBox>
                                    </td>
                                    <td class="tdstyle">
                                        <asp:Label ID="Label19" runat="server" Text="Order Qty." CssClass="labelbold"></asp:Label>
                                        <br />
                                        <asp:TextBox ID="txtqty" runat="server" Width="90px" OnTextChanged="txtqty_TextChanged"
                                            CssClass="textb" AutoPostBack="True" BackColor="#8B7B8B"></asp:TextBox>
                                        <%--  <asp:RequiredFieldValidator SetFocusOnError="true" ID="RequiredFieldValidator2" runat="server"
                                            ErrorMessage="please Enter Date" ControlToValidate="txtqty" ValidationGroup="f1"
                                            ForeColor="Red">*</asp:RequiredFieldValidator>--%>
                                    </td>
                                    <td id="tdrate" runat="server" class="tdstyle">
                                        <asp:Label ID="Label20" runat="server" Text="Rate" CssClass="labelbold"></asp:Label>
                                        <br />
                                        <asp:TextBox ID="TxtRate" runat="server" Width="90px" AutoPostBack="True" OnTextChanged="TxtRate_TextChanged"
                                            CssClass="textb"></asp:TextBox>
                                        <%--  <asp:RequiredFieldValidator SetFocusOnError="true" ID="RequiredFieldValidator3" runat="server"
                                            ErrorMessage="please Enter Date" ControlToValidate="txtqty" ValidationGroup="f1"
                                            ForeColor="Red">*</asp:RequiredFieldValidator>--%>
                                    </td>
                                    <td id="tdlotno" runat="server" class="tdstyle">
                                        <asp:Label ID="Label21" runat="server" Text="Lot No" CssClass="labelbold"></asp:Label>
                                        <br />
                                        <asp:TextBox ID="TxtLotNo" runat="server" Width="150px" CssClass="textb" onkeydown="return (event.keyCode!=13);"></asp:TextBox>
                                    </td>
                                    <td id="tdamt" runat="server" class="tdstyle">
                                        <asp:Label ID="Label22" runat="server" Text="Amount" CssClass="labelbold"></asp:Label>
                                        <br />
                                        <asp:TextBox ID="TxtAmount" runat="server" Enabled="false" Width="85px" CssClass="textb"></asp:TextBox>
                                    </td>
                                    <td id="tdreqby" runat="server" visible="false" class="tdstyle">
                                        <asp:Label ID="lblreqby" runat="server" Text="Requested By" CssClass="labelbold"></asp:Label><span style="color:Red">*</span>
                                        <br />
                                        <asp:TextBox ID="txtReqBy" runat="server"  Width="85px" CssClass="textb"></asp:TextBox>
                                    </td>
                                     <td id="tdreqfor" runat="server" visible="false" class="tdstyle">
                                        <asp:Label ID="lblreqfor" runat="server" Text="Requested For" CssClass="labelbold"></asp:Label><span style="color:Red">*</span>
                                        <br />
                                        <asp:TextBox ID="txtreqfor" runat="server"  Width="85px" CssClass="textb"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdstyle">
                                        <asp:Label ID="Label23" runat="server" Text="Delivery Date" CssClass="labelbold"></asp:Label>
                                        <b style="color: Red">&nbsp; *</b><br />
                                        <asp:TextBox ID="txtcomp_date" runat="server" CssClass="textb" AutoPostBack="true"
                                            Width="90px" BackColor="#8B7B8B" onchange="return CHeckDate();"></asp:TextBox>
                                        <asp:CalendarExtender ID="CalendarExtender3" runat="server" Format="dd-MMM-yyyy"
                                            TargetControlID="txtcomp_date">
                                        </asp:CalendarExtender>
                                        <br>
                                    </td>
                                    <td id="Tdweig" runat="server" class="tdstyle">
                                        <asp:Label ID="Label24" runat="server" Text="Weight." CssClass="labelbold"></asp:Label>
                                        <br />
                                        <asp:TextBox ID="txtweig" Width="80px" runat="server" CssClass="textb" onkeydown="return (event.keyCode!=13);"></asp:TextBox>
                                    </td>
                                    <td id="Tdnextdate" runat="server" class="tdstyle" visible="false">
                                        <asp:Label ID="Label25" runat="server" Text="Next Date" CssClass="labelbold"></asp:Label>
                                        <b style="color: Red">&nbsp; *</b><br />
                                        <asp:TextBox ID="txtNextdate" runat="server" CssClass="textb" AutoPostBack="true"
                                            Width="90px" BackColor="#8B7B8B"></asp:TextBox>
                                        <asp:CalendarExtender ID="CalendarExtender4" runat="server" Format="dd-MMM-yyyy"
                                            TargetControlID="txtNextdate">
                                        </asp:CalendarExtender>
                                    </td>
                                    <td id="tdthanlength" runat="server" visible="false" class="tdstyle">
                                        <asp:Label ID="Label26" runat="server" Text="ThanLength." CssClass="labelbold"></asp:Label>
                                        <br />
                                        <asp:TextBox ID="Txtthanlength" Enabled="false" Width="70px" runat="server" CssClass="textb"></asp:TextBox>
                                    </td>
                                    <td id="TDGSTType" runat="server" class="tdstyle" visible="true">
                                        <asp:Label ID="Label59" runat="server" Text="GST Type" CssClass="labelbold"></asp:Label><br />
                                        <asp:DropDownList CssClass="dropdown" Width="150px" ID="DDGSType" runat="server"
                                            AutoPostBack="True" OnSelectedIndexChanged="DDGSType_SelectedIndexChanged">
                                            <asp:ListItem Value="0" Selected="True">---Select----</asp:ListItem>
                                            <asp:ListItem Value="1">CGST/SGST</asp:ListItem>
                                            <asp:ListItem Value="2">IGST</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                    <td id="TDTCSType" runat="server" class="tdstyle" visible="true">
                                        <asp:Label ID="Label62" runat="server" Text="TCS Type" CssClass="labelbold"></asp:Label><br />
                                        <asp:DropDownList CssClass="dropdown" Width="150px" ID="DDTCSType" runat="server"
                                            AutoPostBack="True" OnSelectedIndexChanged="DDTCSType_SelectedIndexChanged">
                                            <asp:ListItem Value="0" Selected="True">---Select----</asp:ListItem>
                                            <asp:ListItem Value="1">TCS</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                    <td colspan="6">
                                        <%--<asp:Button ID="Button1" ForeColor="White" BorderStyle="None" runat="server" Height="0px" Width="0px" Text="New"/>--%>
                                        <asp:Label ID="Label2" runat="server" Text="ProdCode doesnot exist" ForeColor="Red"
                                            Font-Size="X-Small" Visible="false"></asp:Label>
                                        <br />
                                        <asp:Label ID="Lblfinished" runat="server" Text="Allready Issued" ForeColor="Red"
                                            Font-Size="X-Small" Visible="False"></asp:Label>
                                        <asp:Label ID="Label4" runat="server" Text="" ForeColor="Black" Font-Size="Small"
                                            Font-Bold="true"></asp:Label>
                                    </td>
                                </tr>
                            </table>
                            <table>
                                <tr id="Tr2" runat="server">
                                    <td id="TDVAT" runat="server" visible="false" class="tdstyle">
                                        <asp:Label ID="Label27" runat="server" Text="VAT(%)" CssClass="labelbold"></asp:Label>
                                        <br />
                                        <asp:TextBox ID="TxtExceisDuty" runat="server" Width="80px" AutoPostBack="True" CssClass="textb"
                                            OnTextChanged="TxtExceisDuty_TextChanged"></asp:TextBox>
                                    </td>
                                    <td id="Td3" runat="server" visible="false" class="tdstyle">
                                        <asp:Label ID="Label28" runat="server" Text="SAT(%)" CssClass="labelbold"></asp:Label>
                                        <br />
                                        <asp:TextBox ID="TxtEduCess" runat="server" Width="80px" AutoPostBack="True" CssClass="textb"
                                            OnTextChanged="TxtEduCess_TextChanged"></asp:TextBox>
                                    </td>
                                    <td id="TDCST" runat="server" visible="false" class="tdstyle">
                                        <asp:Label ID="Label29" runat="server" Text="CST(%)" CssClass="labelbold"></asp:Label>
                                        <br />
                                        <asp:TextBox ID="TxtCst" runat="server" Width="80px" AutoPostBack="True" CssClass="textb"
                                            OnTextChanged="TxtCst_TextChanged"></asp:TextBox>
                                    </td>
                                    <td id="TDCGST" runat="server" visible="false">
                                        <asp:Label ID="Label55" runat="server" Text="CGST(%)" CssClass="labelbold"></asp:Label>
                                        <br />
                                        <asp:TextBox ID="txtCGST" runat="server" Width="80px" CssClass="textb" Enabled="false"></asp:TextBox>
                                    </td>
                                    <td id="TDSGST" runat="server" visible="false">
                                        <asp:Label ID="Label56" runat="server" Text="SGST(%)" CssClass="labelbold"></asp:Label>
                                        <br />
                                        <asp:TextBox ID="txtSGST" runat="server" Width="80px" AutoPostBack="True" CssClass="textb"
                                            Enabled="false" OnTextChanged="txtSGST_TextChanged"></asp:TextBox>
                                    </td>
                                    <td id="TDIGST" runat="server" visible="false">
                                        <asp:Label ID="Label57" runat="server" Text="IGST(%)" CssClass="labelbold"></asp:Label>
                                        <br />
                                        <asp:TextBox ID="txtIGST" runat="server" Width="80px" AutoPostBack="True" CssClass="textb"
                                            Enabled="false" OnTextChanged="txtIGST_TextChanged"></asp:TextBox>
                                    </td>
                                    <td id="TDTCS" runat="server" visible="false">
                                        <asp:Label ID="Label63" runat="server" Text="TCS(%)" CssClass="labelbold"></asp:Label>
                                        <br />
                                        <asp:TextBox ID="txtTCS" runat="server" Width="80px" AutoPostBack="True" CssClass="textb"
                                            Enabled="false" OnTextChanged="txtTCS_TextChanged"></asp:TextBox>
                                    </td>
                                    <td id="Td2" runat="server" visible="false" class="tdstyle">
                                        <asp:Label ID="Label30" runat="server" Text="Total Amount" CssClass="labelbold"></asp:Label>
                                        <br />
                                        <asp:TextBox ID="TxtTotalAmount" runat="server" CssClass="textb" Enabled="false"
                                            Width="80px"></asp:TextBox>
                                    </td>
                                    <td class="tdstyle">
                                        <asp:Label ID="Label31" runat="server" Text="Net Amount" CssClass="labelbold"></asp:Label>
                                        <br />
                                        <asp:TextBox ID="TxtNetAmount" runat="server" Enabled="false" CssClass="textb" Width="80px"></asp:TextBox>
                                    </td>
                                    <td class="tdstyle" style="text-align: center;">
                                        <asp:Label ID="Label51" runat="server" Text="Remarks" CssClass="labelbold"></asp:Label><br />
                                        <asp:TextBox ID="txtremarks" runat="server" TextMode="MultiLine" Width="350px" Height="100px"
                                            CssClass="textboxremark"></asp:TextBox>
                                    </td>
                                    <%-- <td colspan="2" id="trimage" runat="server" visible="true">
                                        <asp:Image ID="lblimage" runat="server" Height="66px" Width="132px" />
                                    </td>--%>
                                </tr>
                                <%--<tr id="trimage" runat="server" visible="false">
                                <td colspan="2">
                                    <asp:Image ID="lblimage" runat="server" Height="66px" Width="132px" />
                                </td>
                            </tr>--%>
                            </table>
                        </fieldset>
                        <div>
                            <div style="width: 890px; height: auto">
                                <div style="width: 930px; background-color: #8B7B8B;">
                                    <table>
                                        <tr>
                                            <td>
                                                <asp:CheckBox ID="chkotherinformation" runat="server" Text=" ADITIONAL INFORMATION "
                                                    Font-Bold="true" ForeColor="White" AutoPostBack="true" CssClass="labelbold" OnCheckedChanged="chkotherinformation_CheckedChanged" />
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                                <div style="width: 690px;" runat="server" visible="false" id="DivOtherInformation">
                                    <table>
                                        <tr>
                                            <td id="TdlblCurrency" runat="server" visible="true">
                                                <asp:Label ID="Label32" runat="server" Text="Currency" CssClass="labelbold"></asp:Label>
                                            </td>
                                            <td id="TdDDCurrency" runat="server" visible="true">
                                                <asp:DropDownList ID="DDCurrency" runat="server" CssClass="dropdown" Width="115px">
                                                </asp:DropDownList>
                                            </td>
                                            <td id="tddes" runat="server" class="tdstyle">
                                                <asp:Label ID="Label33" runat="server" Text="Destination" CssClass="labelbold"></asp:Label>
                                            </td>
                                            <td id="tddes1" runat="server">
                                                <asp:TextBox ID="txtdestination" runat="server" Width="115px" CssClass="textb" onkeydown="return (event.keyCode!=13);"></asp:TextBox>
                                            </td>
                                            <td id="tdinsu" runat="server" class="tdstyle">
                                                <asp:Label ID="Label34" runat="server" Text="Insurance" CssClass="labelbold"></asp:Label>
                                            </td>
                                            <td id="tdinsu1" runat="server">
                                                <asp:TextBox ID="txtinsurence" runat="server" Width="150px" CssClass="textb" onkeydown="return (event.keyCode!=13);"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td id="TdlblSupplierRef" runat="server">
                                                <asp:Label ID="Label35" runat="server" Text="Supplier Ref:" CssClass="labelbold"></asp:Label>
                                            </td>
                                            <td id="TdtxtSupplierRef" runat="server">
                                                <asp:TextBox ID="txtSupplierRef" runat="server" Width="115px" CssClass="textb" onkeydown="return (event.keyCode!=13);"></asp:TextBox>
                                            </td>
                                            <td id="TdlblSupplerRefDate" runat="server">
                                                <asp:Label ID="Label36" runat="server" Text="Ref. Date" CssClass="labelbold"></asp:Label>
                                            </td>
                                            <td id="TdtxtSupplierRefDate" runat="server">
                                                <asp:TextBox ID="txtSupplierRefDate" runat="server" Width="115px" CssClass="textb"></asp:TextBox>
                                                <asp:CalendarExtender ID="calendertxtrefdate" runat="server" TargetControlID="txtSupplierRefDate"
                                                    Format="dd-MMM-yyyy">
                                                </asp:CalendarExtender>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td id="TdlblVendorref" runat="server">
                                                <asp:Label ID="Label37" runat="server" Text="Vendor Ref.(S)" CssClass="labelbold"></asp:Label>
                                            </td>
                                            <td id="TdtxtVendorRef" runat="server">
                                                <asp:TextBox ID="txtvendorRef" runat="server" Width="115px" CssClass="textb" onkeydown="return (event.keyCode!=13);"></asp:TextBox>
                                            </td>
                                            <td id="TdlblVenodrRefDate" runat="server">
                                                <asp:Label ID="Label38" runat="server" Text="Vendor Ref. Date" CssClass="labelbold"></asp:Label>
                                            </td>
                                            <td id="TdtxtVendorRefDate" runat="server">
                                                <asp:TextBox ID="txtVendorRefDate" runat="server" Width="115px" CssClass="textb"></asp:TextBox>
                                                <asp:CalendarExtender ID="CalendarExtender6" runat="server" TargetControlID="txtVendorRefDate"
                                                    Format="dd-MMM-yyyy">
                                                </asp:CalendarExtender>
                                            </td>
                                        </tr>
                                        <tr id="trpament1" runat="server">
                                            <td class="tdstyle">
                                                <asp:Label ID="Label39" runat="server" Text="Payment Term" CssClass="labelbold"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddpayement" runat="server" CssClass="dropdown" Width="115px">
                                                </asp:DropDownList>
                                                 &nbsp;&nbsp;<asp:Button ID="BtnAddPaymentModeCustom" runat="server" CssClass="buttonsmalls"
                                                        OnClientClick="return AddPaymentMode(); " Text="&#43;" Width="35px" />
                                            </td>
                                            <td id="TdlblFrieghtRate" runat="server" class="tdstyle">
                                                <asp:Label ID="Label40" runat="server" Text="Freight Rate" CssClass="labelbold"></asp:Label>
                                            </td>
                                            <td id="TdtxtfrieghtRate" runat="server">
                                                <asp:TextBox ID="txtfrieghtrate" runat="server" CssClass="textb" Width="115px" AutoPostBack="True"
                                                    OnTextChanged="txtfrieghtrate_TextChanged"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr id="trfright" runat="server">
                                            <td class="tdstyle">
                                                <asp:Label ID="Label41" runat="server" Text="Freight/Transport Charges" CssClass="labelbold"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtfrieght" runat="server" CssClass="textb" Width="95px" onkeydown="return (event.keyCode!=13);"></asp:TextBox>
                                            </td>
                                            <td class="tdstyle">
                                                <asp:Label ID="Label42" runat="server" Text="Delivery Terms" CssClass="labelbold"></asp:Label>
                                            </td>
                                            <td colspan="4">
                                                <asp:DropDownList ID="dddelivery" CssClass="dropdown" runat="server" Width="114px">
                                                </asp:DropDownList>
                                                 &nbsp;<asp:Button ID="Button1" runat="server" CssClass="buttonsmalls"
                                                        OnClientClick="return AddDeliveryTerms(); " Text="&#43;" Width="35px" />
                                            </td>
                                        </tr>
                                        <tr id="trtransort" runat="server">
                                            <td class="tdstyle">
                                                <asp:Label ID="Label43" runat="server" Text="Transport Mode" CssClass="labelbold"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddtransprt" runat="server" CssClass="dropdown" Width="115px">
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="tdstyle" id="TdlblPurchaseAgainstForm" runat="server">
                                                <asp:Label ID="Label44" runat="server" Text="Purchase Against Form" CssClass="labelbold"></asp:Label>
                                            </td>
                                            <td id="TdlblTypeofForm" runat="server">
                                                <asp:Label ID="Label45" runat="server" Text="Type of Form" CssClass="labelbold"></asp:Label>
                                                <asp:TextBox ID="txtTypeofForm" runat="server" Width="100px" CssClass="textb" onkeydown="return (event.keyCode!=13);"></asp:TextBox>
                                            </td>
                                            <td id="TdlblFormNumber" runat="server">
                                                <asp:Label ID="Label46" runat="server" Text="Form Number" CssClass="labelbold"></asp:Label>
                                                <asp:TextBox ID="txtform" runat="server" Width="100px" CssClass="textb" onkeydown="return (event.keyCode!=13);"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr id="tragent" runat="server">
                                            <td class="tdstyle" id="TdlblAgentName" runat="server">
                                                <asp:Label ID="Label47" runat="server" Text="AgentName" CssClass="labelbold"></asp:Label>
                                            </td>
                                            <td id="TdtxtAgentName" runat="server">
                                                <asp:TextBox ID="TxtAgentName" runat="server" CssClass="textb" Width="100px" onkeydown="return (event.keyCode!=13);"></asp:TextBox>
                                            </td>
                                            <td class="tdstyle">
                                                <asp:Label ID="Label48" runat="server" Text="PackingCharges" CssClass="labelbold"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="DDPackingCharges" runat="server" CssClass="dropdown">
                                                    <asp:ListItem Text="Yes" Value="0"></asp:ListItem>
                                                    <asp:ListItem Text="No" Value="1"></asp:ListItem>
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="Label50" runat="server" Text="Mill" CssClass="labelbold"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtMill" runat="server" Width="250px" CssClass="textboxremark" TextMode="MultiLine"></asp:TextBox>
                                            </td>
                                        </tr>
                                       
                                        <tr>
                                            <td class="tdstyle">
                                                <asp:Label ID="Label54" runat="server" Text="Delivery Address" CssClass="labelbold"></asp:Label>
                                            </td>
                                            <td>
                                             <asp:DropDownList Width="100px" ID="ddlDeliveryAddress" runat="server" CssClass="dropdown" Visible="false">
                                                  
                                                </asp:DropDownList>
                                                <asp:TextBox ID="txtDeliveryAddress" runat="server" Width="250px" CssClass="textboxremark"
                                                    TextMode="MultiLine" Visible="false"></asp:TextBox>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </div>
                        </div>
                        <div style="width: 1000px">
                            <div style="float: left">
                                <table style="width: 600px">
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblerrormessage" runat="server" Visible="false" Font-Bold="true" ForeColor="RED"
                                                Text=""></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="right">
                                            <asp:Label ID="Label1" runat="server" Text="" ForeColor="Red" Visible="false"></asp:Label>
                                            <asp:Label ID="Label3" runat="server" Text="Label" ForeColor="Red" Visible="false"></asp:Label>
                                            <asp:Label ID="lblqty" runat="server" Text="Label" ForeColor="Red" Visible="false"></asp:Label>
                                            <asp:Label ID="Label65" runat="server" Text="" ForeColor="Red" Visible="false"></asp:Label>
                                            <asp:DropDownList ID="DDPreviewType" runat="server" CssClass="dropdown" OnSelectedIndexChanged="DDPreviewType_SelectedIndexChanged"
                                                AutoPostBack="True">
                                                <asp:ListItem Text="Purchase With Image" Value="0"></asp:ListItem>
                                                <asp:ListItem Text="Purchase WithOut Rate" Value="1"></asp:ListItem>
                                                <asp:ListItem Text="WithOut Image" Value="2"></asp:ListItem>
                                            </asp:DropDownList>
                                            <b>
                                                <asp:CheckBox ID="chkforSms" Visible="false" AutoPostBack="true" runat="server" Text="Check For SMS"
                                                    OnCheckedChanged="chkforSms_CheckedChanged" /></b>
                                            <asp:Button ID="Btnforsms" Visible="false" runat="server" Width="80px" Text="Send Sms"
                                                CssClass="buttonnorm" OnClick="Btnforsms_Click" />
                                            <asp:Button ID="btnnew" runat="server" Text="New" CssClass="buttonnorm" Width="50px"
                                                OnClientClick="return NewForm()" />
                                            <asp:Button ID="btnsave" runat="server" Text="Save" CssClass="buttonnorm" OnClick="btnsave_Click"
                                                OnClientClick="return validate()" Width="50px" ValidationGroup="f1" />
                                            <asp:Button ID="btnpriview" runat="server" Text="Preview" CssClass="buttonnorm preview_width"
                                                OnClick="btnpriview_Click" />
                                            <asp:Button ID="BTNCLOSE" runat="server" Text="Close" OnClientClick="return CloseForm();"
                                                CssClass="buttonnorm" Width="50px" />
                                            <asp:Button ID="BtnOrderComplete" runat="server" Text="Order Complete" Visible="false"
                                                Width="120px" CssClass="buttonnorm " OnClick="BtnOrderComplete_Click" OnClientClick="return OrderCompleteValidate()" />
                                            <asp:Button ID="Btnorder" runat="server" Text="WorkLoad" Visible="false" CssClass="buttonnorm "
                                                OnClientClick="return OrderDetail()" Width="120px" />
                                            <asp:Button ID="BtnSendMail" runat="server" Text="Send Mail" CssClass="buttonnorm"
                                                Width="80px" OnClick="BtnSendMail_Click" Visible="false" />
                                            <asp:Button ID="BtnChangeVendorName" runat="server" Text="Change VendorName" CssClass="buttonnorm"
                                                Width="130px" OnClick="BtnChangeVendorName_Click" Visible="false" />
                                            <asp:Button ID="BtnUpdateGSTType" runat="server" Text="Update GSTType" CssClass="buttonnorm"
                                                Width="130px" OnClick="BtnUpdateGSTType_Click" Visible="false" />
                                        </td>
                                    </tr>
                                </table>
                                <td>
                                    <asp:ValidationSummary ID="ValidationSummary1" runat="server" ValidationGroup="f1"
                                        ForeColor="Red" />
                                </td>
                            </div>
                            <div style="float: right">
                                <%--<table>
                                    <tr valign="top">
                                        <td id="TDGridShow" colspan="3" align="center" visible="false" runat="server">--%>
                                <div style="width: 365px; height: 163px; overflow: scroll;" id="TDGridShow" runat="server"
                                    visible="false">
                                    <asp:GridView ID="DGShowConsumption" runat="server" Width="500px" AutoGenerateColumns="False"
                                        CssClass="grid-view" OnSelectedIndexChanged="DGShowConsumption_SelectedIndexChanged"
                                        DataKeyNames="finishedid" OnRowDataBound="DGShowConsumption_RowDataBound">
                                        <HeaderStyle CssClass="gvheaders" />
                                        <AlternatingRowStyle CssClass="gvalts" />
                                        <RowStyle CssClass="gvrow" />
                                        <EmptyDataRowStyle CssClass="gvemptytext" />
                                        <Columns>
                                            <%--<asp:BoundField DataField="CATEGORY_NAME" HeaderText="Catagory">
                                            <HeaderStyle Width="150px" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="ITEM_NAME" HeaderText="Item Name">
                                            <HeaderStyle Width="100px" />
                                        </asp:BoundField>--%>
                                            <asp:BoundField DataField="DESCRIPTION" HeaderText="Description">
                                                <HeaderStyle Width="230px" HorizontalAlign="Center" />
                                                <ItemStyle HorizontalAlign="Left" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="Qty" HeaderText="Qty"></asp:BoundField>
                                            <asp:TemplateField HeaderText="Ordered Qty" Visible="true">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblfinishedid" runat="server" Text='<%# Bind("finishedid") %>' Visible="false"></asp:Label>
                                                    <asp:Label ID="lblcategoryid" runat="server" Text='<%# Bind("category_id") %>' Visible="false"></asp:Label>
                                                    <asp:Label ID="lblitem_id" runat="server" Text='<%# Bind("item_id") %>' Visible="false"></asp:Label>
                                                    <asp:Label ID="lblQualityid" runat="server" Text='<%# Bind("Qualityid") %>' Visible="false"></asp:Label>
                                                    <asp:Label ID="lblColorid" runat="server" Text='<%# Bind("Colorid") %>' Visible="false"></asp:Label>
                                                    <asp:Label ID="lbldesignid" runat="server" Text='<%# Bind("designid") %>' Visible="false"></asp:Label>
                                                    <asp:Label ID="lblshapeid" runat="server" Text='<%# Bind("shapeid") %>' Visible="false"></asp:Label>
                                                    <asp:Label ID="thanlength" runat="server" Text='<%# Bind("thanlength") %>' Visible="false"></asp:Label>
                                                    <asp:Label ID="flagsize" runat="server" Text='<%# Bind("flagsize") %>' Visible="false"></asp:Label>
                                                    <asp:Label ID="lblshadecolorid" runat="server" Text='<%# Bind("shadecolorid") %>'
                                                        Visible="false"></asp:Label>
                                                    <asp:Label ID="lblsizeid" runat="server" Text='<%# Bind("sizeid") %>' Visible="false"></asp:Label>
                                                    <asp:Label ID="lblqty" runat="server" Text='<%# Bind("Qty") %>' Visible="false"></asp:Label>
                                                    <asp:Label ID="lblUnitId" runat="server" Text='<%# Bind("UnitId") %>' Visible="false"></asp:Label>
                                                    <asp:Label ID="lblmastremark" runat="server" Text='<%# Bind("remark") %>' Visible="false"></asp:Label>
                                                    <asp:Label ID="lblRate" runat="server" Text='<%# Bind("IRATE") %>' Visible="false"></asp:Label>
                                                    <asp:Label ID="lblFinishType" runat="server" Text='<%# Bind("I_FINISHED_Type_ID") %>'
                                                        Visible="false"></asp:Label>
                                                    <%--<asp:Label ID="LblIWeight" runat="server" Text='<%# Bind("Iweight") %>' Visible="false"></asp:Label>--%>
                                                    <asp:Label ID="lblitemremark" runat="server" Text='<%# Bind("itemremark") %>' Visible="false"></asp:Label>
                                                    <asp:Label ID="lblorderedqty" runat="server" ForeColor="Black" Visible="true" Text='<%# getgiven(DataBinder.Eval(Container.DataItem, "finishedid").ToString(),DataBinder.Eval(Container.DataItem, "I_FINISHED_Type_ID").ToString()) %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </div>
                                <%--   </td>
                                    </tr>
                                </table>--%>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div>
                <div style="width: 980px; overflow: auto; float: left">
                    <asp:GridView ID="gddetail" runat="server" AutoGenerateColumns="False" OnPageIndexChanging="gddetail_PageIndexChanging"
                        AllowPaging="True" PageSize="8" DataKeyNames="Pindentissuetranid" OnRowDeleting="gddetail_RowDeleting"
                        CssClass="grid-view" OnRowDataBound="gddetail_RowDataBound2" Width="980px" OnRowCommand="gddetail_RowCommand"
                        ShowFooter="true">
                        <PagerStyle CssClass="PagerStyle" />
                        <Columns>
                            <asp:BoundField DataField="Pindentissuetranid" Visible="false" HeaderText="Pindentissuetranid" />
                            <asp:BoundField DataField="Category_name" HeaderText="Category" />
                            <asp:BoundField DataField="Item_Name" HeaderText="Item Name" />
                            <asp:BoundField DataField="Description" HeaderText="Description" />
                            <asp:BoundField DataField="Lotno" HeaderText="Lotno" />
                            <asp:TemplateField HeaderText="Rate">
                                <ItemTemplate>
                                    <asp:TextBox ID="TXTRate1" runat="server" Width="70px" Text='<%# Bind("rate") %>'
                                        AutoPostBack="true" OnTextChanged="txtrate1_changed" onkeypress="return isNumberKey(event)"></asp:TextBox>
                                    <asp:Label ID="lbldetailid" runat="server" Visible="false" Text='<%# Bind("Pindentissuetranid") %>' />
                                </ItemTemplate>
                                <HeaderStyle Width="80px" />
                                <FooterTemplate>
                                    <asp:Label ID="lblGrandTotal" runat="server" Text="Grand Total" />
                                </FooterTemplate>
                                <FooterStyle BackColor="Gray" Height="30px" />
                            </asp:TemplateField>
                            <%--<asp:BoundField DataField="Rate" HeaderText="Rate" />--%>
                            <asp:TemplateField HeaderText="Qty">
                                <ItemTemplate>
                                    <asp:TextBox ID="TXTQTY1" runat="server" Width="70px" Text='<%# Bind("quantity") %>'
                                        AutoPostBack="true" OnTextChanged="txtqnt1_changed" onkeypress="return isNumberKey(event)"></asp:TextBox>
                                    <%--<asp:Label ID="lbldetailid" runat="server" Visible="false" Text='<%# Bind("pdetail") %>' />--%>
                                </ItemTemplate>
                                <HeaderStyle Width="80px" />
                                <FooterTemplate>
                                    <asp:Label ID="lblTotalQty" runat="server" />
                                </FooterTemplate>
                                <FooterStyle BackColor="Gray" />
                            </asp:TemplateField>
                            <%--<asp:BoundField DataField="Quantity" HeaderText="Quantity" />--%>
                            <%--  <asp:BoundField DataField="Amount" HeaderText="Amount" />--%>
                            <asp:TemplateField HeaderText="Amount">
                                <ItemTemplate>
                                    <asp:Label ID="lblAmount" runat="server" Visible="true" Text='<%# Bind("Amount") %>' />
                                </ItemTemplate>
                                <HeaderStyle Width="80px" />
                                <FooterTemplate>
                                    <asp:Label ID="lblTotalAmount" runat="server" />
                                </FooterTemplate>
                                <FooterStyle BackColor="Gray" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Weight">
                                <ItemTemplate>
                                    <asp:TextBox ID="TXTweig1" runat="server" Width="70px" onkeypress="return isNumberKey(event)"
                                        Text='<%# Bind("weight") %>'></asp:TextBox>
                                    <%--<asp:Label ID="lbldetailid" runat="server" Visible="false" Text='<%# Bind("pdetail") %>' />--%>
                                </ItemTemplate>
                                <HeaderStyle Width="80px" />
                                <FooterTemplate>
                                    <asp:Label ID="lblTotalWeight" runat="server" />
                                </FooterTemplate>
                                <FooterStyle BackColor="Gray" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Delivery Date">
                                <ItemTemplate>
                                    <asp:TextBox ID="txtdivery_date" runat="server" CssClass="textb" Width="90px" Text='<%# Bind("ddate") %>'
                                        AutoPostBack="true" OnTextChanged="txtqnt1_changed"></asp:TextBox>
                                    <asp:CalendarExtender ID="CalendarExtender5" runat="server" Format="dd-MMM-yyyy"
                                        TargetControlID="txtdivery_date">
                                    </asp:CalendarExtender>
                                </ItemTemplate>
                                <HeaderStyle Width="80px" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Vat %" Visible="false">
                                <ItemTemplate>
                                    <asp:TextBox ID="TXTvat" runat="server" Width="70px" Text='<%# Bind("vat") %>' AutoPostBack="true"
                                        OnTextChanged="txtqnt1_changed" onkeypress="return isNumberKey(event)"></asp:TextBox>
                                </ItemTemplate>
                                <HeaderStyle Width="80px" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="CST %" Visible="false">
                                <ItemTemplate>
                                    <asp:TextBox ID="TXTcst" runat="server" Width="70px" Text='<%# Bind("cst") %>' AutoPostBack="true"
                                        OnTextChanged="txtqnt1_changed" onkeypress="return isNumberKey(event)"></asp:TextBox>
                                </ItemTemplate>
                                <HeaderStyle Width="80px" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="CGST %" Visible="true">
                                <ItemTemplate>
                                    <asp:TextBox ID="TXTCGST" runat="server" Width="70px" Text='<%# Bind("CGST") %>'
                                        Enabled="false" AutoPostBack="true" OnTextChanged="txtqnt1_changed" onkeypress="return isNumberKey(event)"></asp:TextBox>
                                </ItemTemplate>
                                <HeaderStyle Width="80px" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="SGST %" Visible="true">
                                <ItemTemplate>
                                    <asp:TextBox ID="TXTSGST" runat="server" Width="70px" Text='<%# Bind("SGST") %>'
                                        Enabled="false" AutoPostBack="true" OnTextChanged="txtqnt1_changed" onkeypress="return isNumberKey(event)"></asp:TextBox>
                                </ItemTemplate>
                                <HeaderStyle Width="80px" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="IGST %" Visible="true">
                                <ItemTemplate>
                                    <asp:TextBox ID="TXTIGST" runat="server" Width="70px" Text='<%# Bind("IGST") %>'
                                        Enabled="false" AutoPostBack="true" OnTextChanged="txtqnt1_changed" onkeypress="return isNumberKey(event)"></asp:TextBox>
                                </ItemTemplate>
                                <HeaderStyle Width="80px" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="TCS %" Visible="true">
                                <ItemTemplate>
                                    <asp:TextBox ID="txtTCS" runat="server" Width="70px" Text='<%# Bind("TCS") %>' Enabled="false"
                                        onkeypress="return isNumberKey(event)"></asp:TextBox>
                                </ItemTemplate>
                                <HeaderStyle Width="80px" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Net Amount">
                                <ItemTemplate>
                                    <asp:TextBox ID="TXTnetamount" runat="server" Width="70px" Text='<%# Bind("NetAmount") %>'
                                        Enabled="false"></asp:TextBox>
                                </ItemTemplate>
                                <HeaderStyle Width="80px" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Can.Qty">
                                <ItemTemplate>
                                    <asp:TextBox ID="Txtcancel" runat="server" CssClass="textb" Width="90px" Text='<%# Bind("Cancel") %>'
                                        AutoPostBack="true" OnTextChanged="txtcanqnt_changed" onkeypress="return isNumberKey(event)"></asp:TextBox>
                                </ItemTemplate>
                                <HeaderStyle Width="80px" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Item Remark">
                                <ItemTemplate>
                                    <asp:TextBox ID="Txtitemremark" runat="server" CssClass="textb" Width="90px" Text='<%# Bind("itemremark") %>'></asp:TextBox>
                                </ItemTemplate>
                                <HeaderStyle Width="80px" />
                            </asp:TemplateField>
                            <asp:TemplateField ShowHeader="False">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lnkAddImage" runat="server" CausesValidation="False" CommandName="AddImage"
                                        Width="80px" Text="Add Image" CssClass="buttonnorm"></asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField ShowHeader="False">
                                <ItemTemplate>
                                    <asp:LinkButton ID="LinkButton1" runat="server" CausesValidation="False" CommandName="Delete"
                                        Width="50px" Text="DEL" OnClientClick="return confirm('Do you want to Delete data?')"
                                        CssClass="buttonnorm"></asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField Visible="false">
                                <ItemTemplate>
                                    <asp:Label ID="lblFinishedid" runat="server" Text='<%# Bind("Finishedid") %>'>></asp:Label>
                                    <asp:Label ID="lblhrate" runat="server" Text='<%# Bind("rate") %>'>></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <HeaderStyle CssClass="gvheaders" />
                        <AlternatingRowStyle CssClass="gvalts" />
                        <RowStyle CssClass="gvrow" />
                        <EmptyDataRowStyle CssClass="gvemptytext" />
                    </asp:GridView>
                </div>
                <div>
                    <asp:HiddenField ID="hncompid" runat="server" />
                    <asp:HiddenField ID="hnreqdate" runat="server" />
                    <asp:HiddenField ID="hnqty" runat="server" />
                    <%--  <asp:HiddenField ID="hfImgURL" runat="server" />    --%>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
