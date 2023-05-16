<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Order.aspx.cs" Inherits="Masters_Order_Order"
    EnableEventValidation="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="../../UserControls/ucmenu.ascx" TagName="ucmenu" TagPrefix="uc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta content="blendTrans(Duration=0.5)" http-equiv="Page-Enter" />
    <meta content="blendTrans(Duration=0.5)" http-equiv="Page-Exit" />
    <title>Customer Order</title>
    <script src="../../Scripts/JScript.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery-1.4.1.min.js" type="text/javascript"></script>
    <%--<script type="text/javascript" src="../../Scripts/Fixfocus.js"></script>--%>
    <link href="../../Styles/vijay.css" rel="stylesheet" type="text/css" />
    <%-- <script type="text/javascript" src="../../Scripts/Fixfocus2.js"></script>--%>
    <script type="text/javascript" language="javascript">
       
        jQuery(function ($) {
            var focusedElementSelector = "";
            var prm = Sys.WebForms.PageRequestManager.getInstance();

            prm.add_beginRequest(function (source, args) {
                var fe = document.activeElement;
                focusedElementSelector = "";

                if (fe != null) {
                    if (fe.id) {
                        focusedElementSelector = "#" + fe.id;
                    } else {
                        // Handle Chosen Js Plugin
                        var $chzn = $(fe).closest('.chosen-container[id]');
                        if ($chzn.size() > 0) {
                            focusedElementSelector = '#' + $chzn.attr('id') + ' input[type=text]';
                        }
                    }
                }
            });

            prm.add_endRequest(function (source, args) {
                if (focusedElementSelector) {
                    $(focusedElementSelector).focus();
                }
            });
        });
    </script>
    <script type="text/javascript" language="javascript">

        function reloadPage() {
            window.location.href = "Order.aspx";
        }
        function CloseForm() {
            window.location.href = "../../main.aspx";
        }
        function logout() {
            window.location.href = "../../Login.aspx";
        }
        function AddItum() {
            var a3 = document.getElementById('TxtFinishedid').value;
            window.open('../Carpet/AddItemName.aspx?' + a3, '', 'Height=400px,width=500px');
            // window.open('AddItemt.aspx','popup Form', 'Width=550px, Height=400px') 
        }
        function AddItemCode() {
            var a3 = document.getElementById('TxtFinishedid').value;
            window.open('../Carpet/AddItemCode.aspx?' + a3);
        }
        function AddConsumption(button) {
            var row = button.parentNode.parentNode;
            var label = GetChildControl(row, "lblFinishedid");
            var labelFlag = GetChildControl(row, "lblflagsize");
            var Finishedid = label.innerHTML;
            var flagsize = labelFlag.innerHTML;
            //var Orderid = document.getElementById('TxtOrderID').value;          
            window.open('../Carpet/DefineBomAndConsumption.aspx?finishedid=' + Finishedid + '&ZZZ=1&flagSize=' + flagsize, '', 'Height=600px,width=1000px');

        }

        function AddConsumption(button) {
            var row = button.parentNode.parentNode;
            var label = GetChildControl(row, "lblFinishedid");
            var labelFlag = GetChildControl(row, "lblflagsize");
            var Finishedid = label.innerHTML;
            var flagsize = labelFlag.innerHTML;
            //var Orderid = document.getElementById('TxtOrderID').value;          
            window.open('../Carpet/DefineBomAndConsumption.aspx?finishedid=' + Finishedid + '&ZZZ=1&flagSize=' + flagsize, '', 'Height=600px,width=1000px');

        }
        function GetChildControl(element, id) {
            var child_elements = element.getElementsByTagName("*");
            for (var i = 0; i < child_elements.length; i++) {
                if (child_elements[i].id.indexOf(id) != -1) {
                    return child_elements[i];
                }
            }
        };
        function report() {
            window.open('../../ViewReport.aspx', 'nwwin', 'toolbar=0, titlebar=1,  top=0px, left=0px, scrollbars=1, resizable = yes');
        }
        function Preview() {
            window.open('../../reportViewer.aspx', '');
        }
        function AddItemCategory() {
            window.open('../Carpet/AddItemCategory.aspx', '', 'Height=500px,width=700px');
        }
        function AddQuality() {
            var a3 = document.getElementById('TxtFinishedid').value;
            window.open('../Carpet/AddQuality.aspx?' + a3, '', 'Height=500px,width=500px');
        }
        function AddDesign() {
            window.open('../Carpet/AddDesign.aspx', '', 'Height=500px,width=500px');
        }
        function AddColor() {
            window.open('../Carpet/AddColor.aspx', '', 'Height=500px,width=500px');
        }
        function AddShadecolor() {
            window.open('../Carpet/AddShadeColor.aspx', '', 'Height=500px,width=500px');
        }
        function AddShape() {
            window.open('../Carpet/AddShape.aspx', '', 'Height=400px,width=500px');
        }
        function AddSize() {

            var e = document.getElementById("tbsample_tabrawmaterial_DDShape");
            var shapeid = e.options[e.selectedIndex].value;
            if (document.getElementById('HDF1').value == "7") {
                window.open('../Carpet/frmSizeForLocal.aspx?shapeid=' + shapeid + '', '', 'Height=400px,width=600px');
                return false;
            }
            else {
                window.open('../Carpet/AddSize.aspx?shapeid=' + shapeid + '', '', 'Height=500px,width=1000px');
            }
        }
        function AddQualitySize() {
            var e = document.getElementById("tbsample_tabrawmaterial_DDShape");
            var shapeid = e.options[e.selectedIndex].value;
            if (document.getElementById('HDF1').value == "7") {
                window.open('../Carpet/frmSizeForLocal.aspx?shapeid=' + shapeid + '', '', 'Height=400px,width=600px');
                return false;
            }
            else {
                window.open('../Carpet/FrmNewSize.aspx?shapeid=' + shapeid + '', '', 'Height=500px,width=1000px');
            }
        }
        function AddPhoto() {
            window.open('../Carpet/AddPhotoRefImage.aspx?shapeid=1', '', 'Height=400px,width=700px');
        }
        function AddReferenceImage() {
            window.open('../Carpet/AddPhotoRefImage.aspx?shapeid=2', '', 'Height=400px,width=700px');
        }
        function AddLocalConsumption() {
            var Orderid = document.getElementById('tbsample_TabMainInformation_TxtOrderID').value;
            window.open('FrmTagging.aspx?OrderId=' + Orderid + '', '', 'Height=500px,width=850px,resizable=yes scrollbars=yes');
        }
        function privious() {
            document.getElementById('tdtab2').style.display = 'none';
            document.getElementById('tdtab1').style.display = 'block';
            document.getElementById('trsave').style.display = 'none';
            document.getElementById('tdbtn11').style.display = 'block';

            return false;
        }
        function next() {
            document.getElementById('tdtab2').style.display = 'block';
            document.getElementById('tdtab1').style.display = 'none';
            document.getElementById('trsave').style.display = 'block';
            document.getElementById('tdbtn11').style.display = 'block';

            return false;
        }
        function ontextchanged() {
            var Srcdt = document.getElementById("tbsample_TabMainInformation_TxtDeliveryDate").value
            var Expdt = document.getElementById("tbsample_TabMainInformation_Txtcustorderdt").value
            var dilvdt = document.getElementById("tbsample_TabMainInformation_TxtDueDate").value
            // var dilvdt = document.getElementById("TxtDueDate").
            var DateScrdt = new Date(Srcdt);
            var DateExpdt = new Date(Expdt);
            var Datedilvdt = new Date(dilvdt);
            if (DateScrdt < DateExpdt) {
                alert("Expiry Date must be greater than or equal to SRC Date.");
                return false;
            }
            // Display error message if a field is not completed
            if (DateExpdt > Datedilvdt) {
                alert('Delivery Date must be greater than or equal to Expiry Date.');
                return false;
            }
            else if (Srcdt == '') {
                alert("Please enter SRC Date");
                return false;
            }
            else if (Expdt == '') {
                alert("Please enter Expiry Date");
                return false;
            }
            else if (dilvdt == '') {
                alert("Please enter Delivery Date");
                return false;
            }
            else
                return true;
        }
        function doConfirm() {
            var r = confirm("Do you want to delete ?");
            document.getElementById("hnsst").value = r;
            if (r == true) {
                x = "You pressed OK!";
                return true;
            }
            else {
                x = "You pressed Cancel!";
                return false;
            }
            //alert(x);
        }
        function checkDate(txtno) {
            var txt3 = "N";
            var txt4 = "N";
            var d1;
            var d2;
            var d3;
            var d4;
            var lblorder;
            var lbldelivery;
            var lblcustorder;
            var lblduedate
            lbldelivery = document.getElementById("tbsample_TabMainInformation_LblDelvDate").innerHTML;
            var datevalue1 = document.forms[0].tbsample_TabMainInformation_TxtOrderDate.value;
            var datevalue2 = document.forms[0].tbsample_TabMainInformation_TxtDeliveryDate.value;
            if (document.getElementById("tbsample_TabMainInformation_TDTxtcustorderdt")) {
                if (txtno == "0" || txtno == "3") {
                    lblcustorder = document.getElementById("tbsample_TabMainInformation_LblDelvDate").innerHTML;
                    lbldelivery = document.getElementById("tbsample_TabMainInformation_lblcustname").innerHTML;
                    var datevalue3 = document.forms[0].tbsample_TabMainInformation_Txtcustorderdt.value;
                    var day3 = datevalue3.substring(0, 2);
                    var month3 = datevalue3.substring(3, 6);
                    var year3 = datevalue3.substring(7, 11);
                    month3 = changeFormatStringtoNumber(month3);
                    d3 = new Date(year3, month3, day3);
                }
                else {
                    txt3 = "Null";
                }
            }
            if (document.getElementById("tbsample_TabMainInformation_td")) {
                if (txtno == "3" || txtno == "0") {
                    // do something
                    lblduedate = document.getElementById("tbsample_TabMainInformation_lblduedate").innerHTML;
                    //lblduedate = document.forms[0].lblduedate.value;
                    var datevalue4 = document.forms[0].tbsample_TabMainInformation_TxtDueDate.value;
                    var day4 = datevalue4.substring(0, 2);
                    var month4 = datevalue4.substring(3, 6);
                    var year4 = datevalue4.substring(7, 11);
                    month4 = changeFormatStringtoNumber(month4);
                    d4 = new Date(year4, month4, day4);
                }
                else {
                    txt4 = "Null";
                }
            }
            var day1 = datevalue1.substring(0, 2);
            var month1 = datevalue1.substring(3, 6);
            var year1 = datevalue1.substring(7, 11);

            var day2 = datevalue2.substring(0, 2);
            var month2 = datevalue2.substring(3, 6);
            var year2 = datevalue2.substring(7, 11);

            month1 = changeFormatStringtoNumber(month1);
            month2 = changeFormatStringtoNumber(month2);
            //            alert(month2);
            d1 = new Date(year1, month1, day1);
            d2 = new Date(year2, month2, day2);
            if (d1 > d2) {
                alert("Order date can not be greater than " + tbsample_TabMainInformation_lbldelivery + "!");
                document.getElementById("tbsample_TabMainInformation_TxtDeliveryDate").value = datevalue1;
                return false;
            }
            if (txt3 != "Null") {
                if (d2 > d3) {
                    alert(lbldelivery + " can not be greater than " + tbsample_TabMainInformation_lblcustorder + " !");
                    return false;
                }
            }
            if (txt3 != "Null" && txt4 != "Null") {
                if (d3 > d4) {
                    alert(lblcustorder + " can not be greater than " + tbsample_TabMainInformation_lblduedate + " !");
                    document.getElementById("tbsample_TabMainInformation_TxtDueDate").value = datevalue3;
                    return false;
                }
            }
            if (txtno == "0") {
                if (document.getElementById("tbsample_TabMainInformation_TxtCustOrderNo").value == "") {
                    alert("Customer Order No Cann't Be Blank");
                    return false;
                }
                if (document.getElementById("tbsample_TabMainInformation_TxtLocalOrderNo").value == "") {
                    alert("Local No Cann't Be Blank");
                    return false;
                }

                if (document.getElementById("Lblmessage")) {
                    if (document.getElementById('Lblmessage').innerHTML != "1") {
                        alert("Customer Order is Duplicate");
                        return false;
                    }
                }
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
        function isNumberKey(evt) {
            var charCode = (evt.which) ? evt.which : event.keyCode
            if (charCode > 31 && (charCode < 48 || charCode > 57)) {
                alert('numeric only');
                return false;
            }
            else {
                return true;
            }
        }
        function isNumberKey1(evt) {
            var charCode = (evt.which) ? evt.which : event.keyCode
            if (charCode != 46 && charCode > 31 && (charCode < 48 || charCode > 57)) {
                alert('numeric only');
                return false;
            }
            else {
                return true;
            }
        }
        function AddBuyerMasterCode() {
            var a3 = document.getElementById('tbsample_TabMainInformation_TxtCustomerID').value;
            var e = document.getElementById("tbsample_tabrawmaterial_DDItemCategory");
            var VarCategoryid = e.options[e.selectedIndex].value;
            var e1 = document.getElementById("tbsample_tabrawmaterial_DDItemName");
            var VarItemid = e1.options[e1.selectedIndex].value;
            window.open('../Carpet/BuyerMasterCode.aspx?ABC=1&CustomerID=' + a3 + '&VarCategory=' + VarCategoryid + '&VarItem=' + VarItemid + '', '', 'Height=600px,width=850px');
        }
    </script>
    <%-- <script language="javascript">
        var postbackElement = null;
        function SetFocusToNextControl(newTabIndex) {
            for (var i = 0; i <= document.form1.elements.length; i++) {
                if (document.form1.elements[i].tabIndex == newTabIndex) {
                    document.form1.elements[i].focus();
                    break;
                }
            }
        }
        function RestoreFocus(source, args) {
            document.getElementById(postbackElement.id).focus();
            if (document.getElementById(postbackElement.id).type == 'text')
                SetFocusToNextControl(document.getElementById(postbackElement.id).tabIndex + 1)
        }
        function SavePostbackElement(source, args) {
            postbackElement = args.get_postBackElement();
        }
        function AddRequestHandler() {
            var prm = Sys.WebForms.PageRequestManager.getInstance();
            prm.add_endRequest(RestoreFocus);
            prm.add_beginRequest(SavePostbackElement);
        }
    </script>--%>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <table width="100%">
            <tr style="width: 100%" align="center" id="trHeader" runat="server">
                <td height="66px" align="center" style="width: 80%">
                    <asp:Image ID="Image1" ImageUrl="~/Images/header.jpg" runat="server" Width="100%" />
                </td>
                <td style="background-color: #0080C0;" width="20%" valign="bottom">
                    <table width="100%">
                        <tr>
                            <td>
                                <asp:Image ID="imgLogo" align="left" runat="server" Height="66px" Width="111px" />
                            </td>
                            <td>
                                <asp:Label ID="LblCompanyName" runat="server" Text="" CssClass="labelnormal" Style="font-style: italic"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                &nbsp;
                            </td>
                            <td>
                                <i><font size="2" face="GEORGIA">
                                    <asp:Label ID="LblUserName" ForeColor="White" runat="server" Text=""></asp:Label></font></i>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr bgcolor="#999999">
                <td>
                    <uc1:ucmenu ID="ucmenu1" runat="server" />
                    <asp:ScriptManager ID="ScriptManager1" runat="server">
                    </asp:ScriptManager>
                </td>
                <td width="25%">
                    <asp:UpdatePanel ID="up" runat="server">
                        <ContentTemplate>
                            <asp:Button Width="100%" BorderStyle="None" BackColor="#999999" ID="BtnLogout" OnClientClick="return logout()"
                                runat="server" Text="Logout" UseSubmitBehavior="False" />
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </td>
            </tr>
        </table>
        <asp:UpdatePanel ID="upd1" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <asp:TabContainer ID="tbsample" runat="server" Width="100%" ActiveTabIndex="1">
                    <asp:TabPanel ID="TabMainInformation" HeaderText="Customer Information" runat="server">
                        <HeaderTemplate>
                            Customer Information
                        </HeaderTemplate>
                        <ContentTemplate>
                            <div style="width: 100%; float: left; background-color: #E3E3E3">
                                <div id="DivCustomerDetail" runat="server" style="width: 60%; float: left;">
                                    <table cellpadding="0" cellspacing="5" style="width: 100%">
                                        <tr>
                                            <td id="TDEditOrder" runat="server" style="width: 15%">
                                                <asp:CheckBox ID="ChkEditOrder" runat="server" Text="EDIT ORDER" AutoPostBack="True"
                                                    OnCheckedChanged="ChkEditOrder_CheckedChanged" CssClass="checkboxbold" />
                                            </td>
                                            <td runat="server" id="Tdcustordersearch" visible="False">
                                            <asp:TextBox ID="txtgetvalue" runat="server" Style="display: none"></asp:TextBox>
                                                <asp:TextBox ID="txtcustordersearch" CssClass="textb" Width="95%" runat="server"
                                                    placeholder="Type Cust. Order No. to fill Detail" AutoPostBack="True" OnTextChanged="txtcustordersearch_TextChanged" />
                                                      <cc1:AutoCompleteExtender ID="txtcustordersearch_AutoCompleteExtender" runat="server" BehaviorID="SrchAutoComplete1"
                                                            CompletionInterval="20" Enabled="True" ServiceMethod="GetOrderName" CompletionSetCount="20"
                                                            OnClientItemSelected="" ServicePath="~/Autocomplete.asmx" TargetControlID="txtcustordersearch"
                                                            UseContextKey="True" ContextKey="0" MinimumPrefixLength="2" DelimiterCharacters="">
                                                        </cc1:AutoCompleteExtender>
                                            </td>
                                            <td style="width: 30%">
                                                <asp:CheckBox ID="CHKFORCURRENTCONSUMPTIONNew" runat="server" Text="FOR CURRENT CONSUMPTION"
                                                    Visible="False" CssClass="checkboxbold" />
                                            </td>
                                        </tr>
                                    </table>
                                    <table width="100%">
                                        <tr>
                                            <td class="tdstyle" width="20%">
                                                <asp:TextBox ID="TxtOrderID" runat="server" Style="display: none" onkeydown="return (event.keyCode!=13);"></asp:TextBox>
                                                <asp:Label ID="lbl" Text="COMPANY NAME*" runat="server" CssClass="labelbold" />
                                            </td>
                                            <td class="tdstyle" width="20%">
                                                <asp:Label ID="lblcustcode" runat="server" Text="CUSTOMER CODE*" CssClass="labelbold"></asp:Label>
                                            </td>
                                            <td class="tdstyle" width="20%">
                                                <asp:Label ID="Label1" Text="CUST ORDER NO*" runat="server" CssClass="labelbold" />
                                            </td>
                                            <td class="tdstyle" width="20%">
                                                <asp:Label ID="Label2" Text="ORDER TYPE*" runat="server" CssClass="labelbold" />
                                            </td>
                                            <td class="tdstyle" width="20%">
                                                &nbsp;&nbsp;&nbsp;
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:DropDownList CssClass="dropdown" ID="DDCompanyName" runat="server" AutoPostBack="True"
                                                    OnSelectedIndexChanged="DDCompanyName_SelectedIndexChanged" Width="150px">
                                                </asp:DropDownList>
                                            </td>
                                            <td>
                                                <asp:DropDownList CssClass="dropdown" ID="DDCustomerCode" runat="server" AutoPostBack="True"
                                                    OnSelectedIndexChanged="DDCustomerCode_SelectedIndexChanged" Width="150px">
                                                </asp:DropDownList>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="TxtCustOrderNo" runat="server" CssClass="textb" BackColor="#7B96BB"
                                                    AutoPostBack="True" OnTextChanged="TxtCustOrderNo_TextChanged"></asp:TextBox>
                                                <asp:DropDownList CssClass="dropdown" ID="DDCustOrderNo" runat="server" AutoPostBack="True"
                                                    Visible="False" OnSelectedIndexChanged="DDCustOrderNo_SelectedIndexChanged" Width="150px">
                                                </asp:DropDownList>
                                            </td>
                                            <td>
                                                <asp:DropDownList CssClass="dropdown" ID="ddordertype" runat="server" AutoPostBack="True"
                                                    Width="150px" OnSelectedIndexChanged="ddordertype_SelectedIndexChanged">
                                                </asp:DropDownList>
                                            </td>
                                            <td>
                                                &nbsp;&nbsp;&nbsp;
                                            </td>
                                        </tr>
                                        <tr id="trneworder" runat="server">
                                            <td class="tdstyle" runat="server">
                                                <asp:Label ID="Label3" Text=" OUR ORDER NO*" runat="server" CssClass="labelbold" />
                                            </td>
                                            <td class="tdstyle" runat="server">
                                                <asp:Label ID="Label4" Text="ORDER DATE*" runat="server" CssClass="labelbold" />
                                            </td>
                                            <td id="td1" runat="server" visible="False" class="tdstyle">
                                                <asp:Label ID="lblcustname" runat="server" Text="Cust.Order DATE*" CssClass="labelbold"></asp:Label>
                                            </td>
                                            <td runat="server">
                                                <asp:Label ID="LblDelvDate" runat="server" Text="DELIVERY DATE*" CssClass="labelbold"></asp:Label>
                                            </td>
                                            <td class="tdstyle" runat="server">
                                                <asp:Label ID="lblCurrency" runat="server" Text=" Currency Name" CssClass="labelbold" />
                                            </td>
                                            <td id="td" visible="False" runat="server" class="tdstyle">
                                                <asp:Label ID="lblduedate" Visible="False" runat="server" Text="DUE DATE*" CssClass="labelbold"></asp:Label>
                                            </td>
                                            <td id="TDNewOrderNo" runat="server" visible="False" class="tdstyle">
                                                <asp:Label ID="Label5" runat="server" Text="  NewOrderNo" CssClass="labelbold"></asp:Label>
                                            </td>
                                            <td class="tdstyle" runat="server" id="tdinspdate" visible="False">
                                                <asp:Label ID="Label6" runat="server" Text="INSPECTION DATE*" CssClass="labelbold"></asp:Label>
                                            </td>
                                            <td class="tdstyle" runat="server" id="tdmodeofpayment">
                                                <asp:Label ID="Label9" runat="server" Text="MODE OF PAYMENT*" CssClass="labelbold"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr id="trneworder1" runat="server">
                                            <td runat="server">
                                                <asp:TextBox ID="TxtLocalOrderNo" runat="server" CssClass="textb" Width="150px" BackColor="#7B96BB"
                                                    onkeydown="return (event.keyCode!=13);"></asp:TextBox>
                                            </td>
                                            <td runat="server">
                                                <asp:TextBox CssClass="textb" ID="TxtOrderDate" onchange="javascript:checkDate(0);"
                                                    Width="150px" runat="server" BackColor="#7B96BB"></asp:TextBox>
                                                <asp:CalendarExtender ID="CalendarExtender1" runat="server" Format="dd-MMM-yyyy"
                                                    TargetControlID="TxtOrderDate" Enabled="True">
                                                </asp:CalendarExtender>
                                            </td>
                                            <td runat="server">
                                                <asp:TextBox CssClass="textb" ID="TxtDeliveryDate" runat="server" onchange="javascript:checkDate(1);"
                                                    Width="150px" BackColor="#7B96BB" AutoPostBack="true" OnTextChanged="TxtDeliveryDate_TextChanged"></asp:TextBox>
                                                <asp:CalendarExtender ID="CalendarExtender2" runat="server" Format="dd-MMM-yyyy"
                                                    TargetControlID="txtDeliveryDate" Enabled="True">
                                                </asp:CalendarExtender>
                                            </td>
                                            <td id="Tdcurrency" runat="server">
                                                <asp:DropDownList CssClass="dropdown" ID="DDCurrency" Width="150px" runat="server">
                                                </asp:DropDownList>
                                            </td>
                                            <td id="TDTxtcustorderdt" runat="server" visible="False">
                                                <asp:TextBox CssClass="textb" ID="Txtcustorderdt" runat="server" BackColor="#7B96BB"></asp:TextBox>
                                                <asp:CalendarExtender ID="CalendarExtender5" Format="dd-MMM-yyyy" runat="server"
                                                    TargetControlID="Txtcustorderdt" Enabled="True">
                                                </asp:CalendarExtender>
                                            </td>
                                            <td id="TDDueDate" runat="server" visible="False">
                                                <asp:TextBox CssClass="textb" ID="TxtDueDate" runat="server" Visible="False" BackColor="#7B96BB"
                                                    onchange="javascript:checkDate(3);"></asp:TextBox>
                                                <asp:CalendarExtender ID="CalendarExtender3" Format="dd-MMM-yyyy" runat="server"
                                                    TargetControlID="TxtDueDate" Enabled="True">
                                                </asp:CalendarExtender>
                                            </td>
                                            <td id="TDNewOrder" runat="server" visible="False">
                                                <asp:TextBox CssClass="textb" ID="TxtNewOrderNo" runat="server" AutoPostBack="True"
                                                    Enabled="False" onkeydown="return (event.keyCode!=13);"></asp:TextBox>
                                            </td>
                                            <td id="TDInspection" runat="server" visible="False">
                                                <asp:TextBox CssClass="textb" ID="TxtInspectionDate" runat="server" BackColor="#7B96BB"></asp:TextBox>
                                                <asp:CalendarExtender ID="CalendarExtender6" Format="dd-MMM-yyyy" runat="server"
                                                    TargetControlID="TxtInspectionDate" Enabled="True">
                                                </asp:CalendarExtender>
                                            </td>
                                            <td id="TBTDModeOfPayment" runat="server">
                                                <asp:DropDownList CssClass="dropdown" ID="ddlModeOfPayment" Width="130px" runat="server">
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr id="TrInspectionDate" runat="server" visible="false">
                                            <td>
                                                <asp:Label ID="Label12" Text="Inspection Date" runat="server" CssClass="labelbold" />
                                                <br />
                                                <asp:TextBox CssClass="textb" ID="TxtInspectionDateNew" runat="server" BackColor="#7B96BB"></asp:TextBox>
                                                <asp:CalendarExtender ID="CalendarExtender7" Format="dd-MMM-yyyy" runat="server"
                                                    TargetControlID="TxtInspectionDateNew" Enabled="True">
                                                </asp:CalendarExtender>
                                            </td>
                                            <td>
                                                <asp:Label ID="Label13" Text="Inspection Qty" runat="server" CssClass="labelbold" />
                                                <br />
                                                <asp:TextBox ID="TxtInspectionQtyNew" runat="server" CssClass="textb" Width="150px"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:Label ID="Label24" Text="Final Inspection Date" runat="server" CssClass="labelbold" />
                                                <br />
                                                <asp:TextBox CssClass="textb" ID="TxtFinalInspectionDateNew" runat="server" BackColor="#7B96BB"></asp:TextBox>
                                                <asp:CalendarExtender ID="CalendarExtender8" Format="dd-MMM-yyyy" runat="server"
                                                    TargetControlID="TxtFinalInspectionDateNew" Enabled="True">
                                                </asp:CalendarExtender>
                                            </td>
                                            <td>
                                                <asp:Label ID="Label25" Text="Final Inspection Qty" runat="server" CssClass="labelbold" />
                                                <br />
                                                <asp:TextBox ID="TxtFinalInspectionQtyNew" runat="server" CssClass="textb" Width="150px"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="tdstyle" colspan="4" id="tdmastremark" runat="server" visible="False">
                                                <asp:Label ID="Label7" runat="server" Text=" Remarks" CssClass="labelbold"></asp:Label>
                                                <br />
                                                <asp:TextBox ID="txtmastremark" runat="server" Width="320px" onkeydown="return (event.keyCode!=13);"
                                                    CssClass="textb"></asp:TextBox>
                                            </td>
                                            <td colspan="2" align="right" id="tdNextButton" runat="server" visible="False">
                                                <asp:Button CssClass="buttonnorm" ID="BtnPlaceOrder" Text="Next" runat="server" OnClick="BtnPlaceOrder_Click"
                                                    OnClientClick="return checkDate(2);" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="right" colspan="6" runat="server" class="tdstyle" id="tdchklabel" visible="False">
                                                &nbsp;&nbsp;&nbsp;
                                                <asp:CheckBox ID="Chkbx" runat="server" AutoPostBack="True" OnCheckedChanged="Chkbx_CheckedChanged"
                                                    Enabled="False" />
                                                &nbsp;&nbsp;&nbsp;
                                                <asp:Label ID="Label8" runat="server" Text="CHECK FOR LABEL" CssClass="labelbold"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="right" colspan="6">
                                                &nbsp;&nbsp;&nbsp;
                                                <div id="ch" runat="server" visible="False">
                                                    <asp:GridView ID="Gvchklist" runat="server" AutoGenerateColumns="False" CssClass="grid-views">
                                                        <HeaderStyle CssClass="gvheaders" />
                                                        <RowStyle CssClass="gvalts" />
                                                        <Columns>
                                                            <asp:TemplateField>
                                                                <ItemTemplate>
                                                                    <asp:CheckBox ID="Chkbox" runat="server" />
                                                                </ItemTemplate>
                                                                <HeaderStyle Width="80px" />
                                                            </asp:TemplateField>
                                                            <asp:BoundField DataField="category_name" HeaderText="categoryname" />
                                                            <asp:BoundField DataField="item_name" HeaderText="itemname" />
                                                            <asp:BoundField DataField="item_id" HeaderText="Sr.No" />
                                                        </Columns>
                                                    </asp:GridView>
                                                </div>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="tdstyle" id="tdrunit" runat="server">
                                                <asp:Label ID="Label14" runat="server" Text="RATE UNIT" CssClass="labelbold"></asp:Label>
                                                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp; <b style="color: Red">*</b> &nbsp; &nbsp;
                                            </td>
                                            <td class="tdstyle">
                                                <asp:Label ID="Label15" runat="server" Text="ORDER UNIT" CssClass="labelbold"></asp:Label>
                                                <b style="color: Red">&nbsp;&nbsp; *</b> &nbsp; &nbsp;
                                            </td>
                                            <td id="TDLabelprocode" runat="server" class="tdstyle">
                                                <asp:Label ID="Label16" runat="server" Text="  PRODUCT CODE" CssClass="labelbold"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label ID="lblShippment" runat="server" Text="  Mode of Shipment" CssClass="labelbold" />
                                            </td>
                                            <td>
                                                <asp:Label ID="lblDeliveryTerms" runat="server" Text="Delivery Terms" CssClass="labelbold" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="tdstyle" id="tdunitrate" runat="server">
                                                <asp:RadioButton ID="rdoUnitWise" Text="AREA WISE" CssClass="radiobuttonnormal" runat="server"
                                                    GroupName="OrderType" />
                                                <br />
                                                <asp:RadioButton ID="rdoPcWise" Text="PC WISE" CssClass="radiobuttonnormal" runat="server"
                                                    GroupName="OrderType" />
                                            </td>
                                            <td>
                                                <asp:DropDownList CssClass="dropdown" ID="DDOrderUnit" runat="server" AutoPostBack="True"
                                                    OnSelectedIndexChanged="DDOrderUnit_SelectedIndexChanged" Width="150px">
                                                </asp:DropDownList>
                                            </td>
                                            <td id="TDtxtprocode" runat="server">
                                                <asp:TextBox CssClass="textb" ID="TxtProdCode" Width="200px" runat="server" AutoPostBack="True"
                                                    OnTextChanged="TxtProdCode_TextChanged"></asp:TextBox>
                                                <asp:AutoCompleteExtender ID="TxtProdCode_AutoCompleteExtender" runat="server" DelimiterCharacters=""
                                                    Enabled="True" MinimumPrefixLength="1" ServiceMethod="GetQuality" ServicePath=""
                                                    TargetControlID="TxtProdCode" UseContextKey="True">
                                                </asp:AutoCompleteExtender>
                                            </td>
                                            <td>
                                                <asp:DropDownList CssClass="dropdown" ID="DDShipment" Width="130px" runat="server">
                                                </asp:DropDownList>
                                            </td>
                                            <td>
                                                <asp:DropDownList CssClass="dropdown" ID="ddlDeliveryTerms" Width="130px" runat="server">
                                                </asp:DropDownList>
                                            </td>
                                            <td id="TDExtraflag" runat="server" visible="False">
                                                <asp:CheckBox ID="chkextraflag" Text="Extra % Quantity" CssClass="labelbold" runat="server" />
                                            </td>
                                            <td>
                                                <asp:TextBox ID="TxtCustomerID" runat="server" Style="display: none" onkeydown="return (event.keyCode!=13);"></asp:TextBox>
                                                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;
                                                <asp:Button ID="RefereshBuyerMasterCode" runat="server" Style="display: none" OnClick="RefereshBuyerMasterCode_Click" />
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </div>
                        </ContentTemplate>
                    </asp:TabPanel>
                    <asp:TabPanel ID="tabrawmaterial" HeaderText="Order Information" runat="server">
                        <HeaderTemplate>
                            Order Information
                        </HeaderTemplate>
                        <ContentTemplate>
                            <div style="width: 100%; float: left; background-color: #E3E3E3">
                                <div style="width: 60%; float: left;">
                                    <table cellpadding="0" cellspacing="0" width="100%">
                                        <tr>
                                            <td>
                                                <table>
                                                    <tr>
                                                        <td id="TDSampleCode" runat="server" visible="False">
                                                            <asp:Label ID="lblSampleCode" runat="server" Text="Sample Code" CssClass="labelbold"></asp:Label>
                                                            <b style="color: Red">&nbsp;&nbsp;&nbsp; *</b>&nbsp;&nbsp;&nbsp; &nbsp; &nbsp;
                                                            <br />
                                                            <asp:TextBox ID="TxtSampleCode" CssClass="textboxm" runat="server" AutoPostBack="True"
                                                                Width="200px" OnTextChanged="TxtSampleCode_TextChanged" />
                                                        </td>
                                                         <td id="TDRugIdNo" runat="server" visible="False">
                                                            <asp:Label ID="Label33" runat="server" Text="RugId No" CssClass="labelbold"></asp:Label>
                                                            <b style="color: Red">&nbsp;&nbsp;&nbsp; *</b>&nbsp;&nbsp;&nbsp; &nbsp; &nbsp;
                                                            <br />
                                                            <asp:TextBox ID="txtRugIdNo" CssClass="textboxm" runat="server" AutoPostBack="True"
                                                                Width="200px" OnTextChanged="txtRugIdNo_TextChanged" />
                                                        </td>
                                                        <td>
                                                            <asp:Label ID="lblcategoryname" runat="server" Text="Item Category" CssClass="labelbold"></asp:Label>
                                                            <b style="color: Red">&nbsp;&nbsp;&nbsp; *</b>&nbsp;&nbsp;&nbsp; &nbsp; &nbsp;
                                                            <asp:Button ID="btnadditemcategory" runat="server" CssClass="buttonsmalls" OnClientClick="return AddItemCategory()"
                                                                Text="ADD" />
                                                            <br />
                                                            <asp:DropDownList ID="DDItemCategory" AutoPostBack="True" runat="server" Width="200px"
                                                                OnSelectedIndexChanged="DDItemCategory_SelectedIndexChanged" CssClass="dropdown"
                                                                ValidationGroup="f1">
                                                            </asp:DropDownList>
                                                            <asp:Button CssClass="refreshcategory" ID="refreshcategory" runat="server" Style="display: none"
                                                                OnClick="refreshcategory_Click" />
                                                        </td>
                                                        <td>
                                                            <asp:Label ID="lblitemname" runat="server" Text="Item Name" CssClass="labelbold"></asp:Label>
                                                            <b style="color: Red">*</b> &nbsp; &nbsp;
                                                            <asp:Button ID="BtnAdd0" runat="server" CssClass="buttonsmalls" OnClientClick="return AddItum()"
                                                                Text="ADD" />
                                                            <br />
                                                            <asp:DropDownList CssClass="dropdown" ID="DDItemName" Width="200px" runat="server"
                                                                AutoPostBack="True" OnSelectedIndexChanged="DDItemName_SelectedIndexChanged"
                                                                ValidationGroup="f1">
                                                            </asp:DropDownList>
                                                            <asp:Button CssClass="buttonnorm" ID="BtnRefreshItem" runat="server" Style="display: none"
                                                                OnClick="BtnRefreshItem_Click" />
                                                        </td>
                                                        <td id="TDQuality" runat="server" visible="False">
                                                            <asp:Label ID="lblqualityname" runat="server" Text="Quality" CssClass="labelbold"></asp:Label>
                                                            <b style="color: Red">*</b>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp; &nbsp;
                                                            <asp:Button ID="btnaddquality" runat="server" CssClass="buttonsmalls" OnClientClick="return AddQuality()"
                                                                Text="ADD" />
                                                            <br />
                                                            <asp:DropDownList CssClass="dropdown" ID="DDQuality" runat="server" AutoPostBack="True"
                                                                Width="200px" OnSelectedIndexChanged="DDQuality_SelectedIndexChanged">
                                                            </asp:DropDownList>
                                                            <asp:Button CssClass="refreshquality" ID="refreshquality" runat="server" Style="display: none"
                                                                OnClick="refreshquality_Click" />
                                                            <asp:Button CssClass="buttonnorm" ID="fillitemcode" runat="server" Style="display: none"
                                                                OnClick="fillitemcode_Click" />
                                                        </td>
                                                        <td id="ItemDescription" runat="server" visible="False" align="left" class="tdstyle">
                                                            <asp:Label ID="Label17" runat="server" Text="  ITEM DESCRIPTION " CssClass="labelbold" />
                                                            &nbsp;<asp:Button CssClass="buttonsmalls" ID="BtnQualityCode" runat="server" Text="ADD"
                                                                OnClientClick="return AddItemCode()" Height="22px" />
                                                            <br />
                                                            <asp:DropDownList CssClass="dropdown" ID="DDItemCode" runat="server" AutoPostBack="True"
                                                                Width="200px" Height="16px">
                                                            </asp:DropDownList>
                                                        </td>
                                                        <td align="center">
                                                            <asp:Button ID="BtnAddBuyerMasterCode" runat="server" align="right" CssClass="buttonnorm"
                                                                OnClientClick="return AddBuyerMasterCode()" Width="150px" Text="ADD NEW ITEMS" />
                                                            <br />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td id="TdDESIGN" runat="server" class="tdstyle">
                                                            <asp:Label ID="lbldesignname" runat="server" Text="Design" CssClass="labelbold"></asp:Label>
                                                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                            <asp:Button ID="btnadddesign" runat="server" CssClass="buttonsmalls" OnClientClick="return AddDesign()"
                                                                Text="ADD" />
                                                            <br />
                                                            <asp:DropDownList CssClass="dropdown" ID="DDDesign" runat="server" AutoPostBack="True"
                                                                Width="200px" OnSelectedIndexChanged="DDDesign_SelectedIndexChanged">
                                                            </asp:DropDownList>
                                                            <asp:Button CssClass="refreshdesign" ID="refreshdesign" runat="server" Style="display: none"
                                                                OnClick="refreshdesign_Click" />
                                                        </td>
                                                        <td id="TDColor" runat="server" visible="False">
                                                            <asp:Label ID="lblcolorname" runat="server" Text="Color" CssClass="labelbold"></asp:Label>
                                                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                            <asp:Button ID="btnaddcolor" runat="server" CssClass="buttonsmalls" OnClientClick="return AddColor()"
                                                                Text="ADD" />
                                                            <br />
                                                            <asp:DropDownList CssClass="dropdown" ID="DDColor" runat="server" Width="200px" AutoPostBack="True"
                                                                OnSelectedIndexChanged="DDColor_SelectedIndexChanged">
                                                            </asp:DropDownList>
                                                            <asp:Button CssClass="refreshcolor" ID="refreshcolor" runat="server" Style="display: none"
                                                                OnClick="refreshcolor_Click" />
                                                        </td>
                                                        <td id="TDShape" runat="server" visible="False" class="tdstyle">
                                                            <asp:Label ID="lblshapename" runat="server" Text="Shape" CssClass="labelbold"></asp:Label>
                                                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                            <asp:Button ID="btnaddshape" runat="server" CssClass="buttonsmalls" OnClientClick="return AddShape()"
                                                                Text="ADD" />
                                                            <br />
                                                            <asp:DropDownList CssClass="dropdown" ID="DDShape" runat="server" AutoPostBack="True"
                                                                OnSelectedIndexChanged="DDShape_SelectedIndexChanged" Width="200px">
                                                            </asp:DropDownList>
                                                            <asp:Button CssClass="buttonnorm" ID="refreshshape" runat="server" Style="display: none"
                                                                OnClick="refreshshape_Click" />
                                                        </td>
                                                        <td id="TDSize" runat="server" class="tdstyle">
                                                            <asp:Label ID="lblsizename" runat="server" Text="Size" CssClass="labelbold"></asp:Label>
                                                            <asp:DropDownList CssClass="dropdown" Width="50px" ID="DDsizetype" runat="server"
                                                                AutoPostBack="True" OnSelectedIndexChanged="DDsizetype_SelectedIndexChanged">
                                                            </asp:DropDownList>
                                                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                            <asp:Button ID="btnAddSize" runat="server" CssClass="buttonsmalls" OnClientClick="return AddSize()"
                                                                Text="ADD" />&nbsp;&nbsp;
                                                            <asp:Button ID="btnAddQualitySize" runat="server" CssClass="buttonsmalls" OnClientClick="return AddQualitySize()"
                                                                Text="ADD" Visible="False" />
                                                            <br />
                                                            <asp:DropDownList CssClass="dropdown" ID="DDSize" runat="server" AutoPostBack="True"
                                                                OnSelectedIndexChanged="DDSize_SelectedIndexChanged" Width="200px">
                                                            </asp:DropDownList>
                                                            <asp:Button CssClass="buttonnorm" ID="BtnRefreshSize" runat="server" Style="display: none"
                                                                OnClick="BtnRefreshSize_Click" />
                                                        </td>
                                                        <td id="tdfiller" runat="server" visible="false" class="tdstyle">
                                                          <asp:Label ID="lblFiller" runat="server" Text="Filler" CssClass="labelbold"></asp:Label>
                                                        <asp:DropDownList CssClass="dropdown" ID="ddfiller" runat="server"
                                                                 Width="200px">
                                                                 <asp:ListItem Value="1" Text="YES"></asp:ListItem>
                                                                 <asp:ListItem Value="2" Text="NO"></asp:ListItem>
                                                        </asp:DropDownList>
                                                        </td>
                                                          <td id="tdmer" runat="server" visible="false" class="tdstyle">
                                                            <asp:Label ID="lblmer" runat="server" Text="Merchandiser" CssClass="labelbold"></asp:Label>
                                                          <asp:TextBox ID="txtmer" runat="server" Width="80px" BackColor="#7B96BB"></asp:TextBox>
                                                        </td>
                                                          <td id="tdincharge" runat="server" visible="false" class="tdstyle">
                                                          <asp:Label ID="lblincharge" runat="server" Text="Production Incharge" CssClass="labelbold"></asp:Label>
                                                        <asp:TextBox ID="txtincharge" runat="server" Width="80px" BackColor="#7B96BB"></asp:TextBox>
                                                        </td>
                                                        <td id="TDShadeColor" runat="server" visible="False">
                                                            <asp:Label ID="LblShadeColor" runat="server" Text="SHADE COLOR" CssClass="labelbold"></asp:Label>
                                                            &nbsp;<asp:Button ID="btnaddshadecolor" runat="server" CssClass="buttonsmalls" OnClientClick="return AddShadecolor()"
                                                                Text="ADD" />
                                                            <br />
                                                            <asp:DropDownList CssClass="dropdown" ID="ddshadecolor" runat="server" AutoPostBack="True"
                                                                Width="200px">
                                                            </asp:DropDownList>
                                                            <asp:Button CssClass="buttonnorm" ID="refreshshade" runat="server" Style="display: none"
                                                                OnClick="refreshshade_Click" />
                                                        </td>
                                                        <td id="TDDDRecipeName" runat="server" visible="false">
                                                            <asp:Label ID="lblRecipeName" runat="server" Text="Recipe Name" CssClass="labelbold" />
                                                            <br />
                                                            <asp:DropDownList CssClass="dropdown" ID="DDRecipeName" Width="130px" runat="server">
                                                            </asp:DropDownList>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <table>
                                                    <tr>
                                                        <td id="TDAREA" runat="server" class="tdstyle">
                                                            <asp:Label ID="Label18" runat="server" Text=" AREA" CssClass="labelbold" />
                                                        </td>
                                                        <td class="tdstyle">
                                                            <asp:Label ID="Label19" runat="server" Text="  QUANTITY" CssClass="labelbold" /><b
                                                                style="color: Red">*</b> &nbsp; &nbsp;
                                                        </td>
                                                        <td class="tdstyle" id="tdextraqtylable" visible="false" runat="server">
                                                            <asp:Label ID="Label34" runat="server" Text="EXTRA QUANTITY" CssClass="labelbold" />
                                                        </td>
                                                        <td id="TDPrice" runat="server" class="tdstyle">
                                                            <asp:Label ID="Label23" runat="server" Text="PRICE" CssClass="labelbold" /><b style="color: Red">*</b>
                                                            &nbsp; &nbsp; &nbsp;
                                                        </td>
                                                        <td id="TDWareHouse" runat="server" class="tdstyle">
                                                            <asp:Label ID="Label20" runat="server" Text="  WAREHOUSENAME" CssClass="labelbold" />
                                                        </td>
                                                        <td id="TDWareHouseNameByCodeLabel" runat="server" class="tdstyle" visible="false">
                                                            <asp:Label ID="Label11" runat="server" Text="  WareHouse Name" CssClass="labelbold" />
                                                        </td>
                                                        <td>
                                                            <asp:Label ID="LblDispatchDate" runat="server" Text="DISPATCHDATE" ForeColor="Red"></asp:Label><b
                                                                style="color: Red">*</b> &nbsp; &nbsp;
                                                        </td>
                                                        <td class="tdstyle">
                                                            <asp:Label ID="Label21" runat="server" Text=" TOTAL ORDER QTY" CssClass="labelbold" />
                                                        </td>
                                                        <td runat="server" id="tdordarea">
                                                            <asp:Label ID="lbjjk" Text="ORDER AREA" runat="server" CssClass="labelbold" />
                                                        </td>
                                                        <td runat="server" id="tdtotamt">
                                                            <asp:Label ID="Label32" Text="TOTAL AMOUNT" runat="server" CssClass="labelbold" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td id="TDTxtArea" runat="server">
                                                            <asp:TextBox CssClass="textb" ID="TxtArea" runat="server" ReadOnly="True" Width="80px"
                                                                BackColor="#7B96BB"></asp:TextBox>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox CssClass="textb" ID="TxtQuantity" runat="server" BackColor="#7B96BB"
                                                                Width="80px" onkeypress="return isNumberKey(event);"></asp:TextBox>
                                                        </td>
                                                         <td id="TDextraqty" runat="server" Visible="false">
                                                            <asp:TextBox CssClass="textb" ID="txtextraqty"  runat="server" BackColor="#7B96BB"
                                                                Width="80px" onkeypress="return isNumberKey(event);"></asp:TextBox>
                                                        </td>
                                                        <td id="TDtxtPrice" runat="server">
                                                            <asp:TextBox CssClass="textb" ID="TxtPrice" runat="server" Width="70px" align="left"
                                                                BackColor="#7B96BB" onkeydown="return (event.keyCode!=13);"></asp:TextBox>
                                                        </td>
                                                        <td id="TdDDWareHouse" runat="server">
                                                            <asp:DropDownList CssClass="dropdown" ID="DDWareHouseName" runat="server" AutoPostBack="True"
                                                                Width="100px" OnSelectedIndexChanged="DDWareHouseName_SelectedIndexChanged">
                                                            </asp:DropDownList>
                                                        </td>
                                                        <td id="TDWareHouseNameByCodeDDL" runat="server" visible="false">
                                                            <asp:DropDownList CssClass="dropdown" ID="DDWareHouseNameByCode" runat="server" Width="100px">
                                                            </asp:DropDownList>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox CssClass="textb" ID="TxtDispatchDate" runat="server" Width="100px" BackColor="#7B96BB"
                                                                OnTextChanged="TxtDispatchDate_TextChanged" AutoPostBack="True"></asp:TextBox>
                                                            <asp:CalendarExtender ID="CalendarExtender4" runat="server" Format="dd-MMM-yyyy"
                                                                TargetControlID="TxtDispatchDate" Enabled="True">
                                                            </asp:CalendarExtender>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox CssClass="textb" ID="TxtTotalQtyRequired" Width="100px" Enabled="False"
                                                                BackColor="#7B96BB" runat="server"></asp:TextBox>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox CssClass="textb" ID="TxtOrderArea" BackColor="#7B96BB" runat="server"
                                                                Enabled="False" onkeydown="return (event.keyCode!=13);" Width="80px"></asp:TextBox>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox CssClass="textb" ID="TxtTotalAmount" BackColor="#7B96BB" runat="server"
                                                                Enabled="False" Width="80px" onkeydown="return (event.keyCode!=13);"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr id="TRArticle" runat="server">
                                            <td runat="server">
                                                <table>
                                                    <tr>
                                                        <td id="TDARTICLENo" runat="server" class="tdstyle">
                                                            <asp:Label ID="Label22" runat="server" Text=" ARTICLE NO" CssClass="labelbold" />
                                                        </td>
                                                        <td id="TDlblUPCNO" runat="server" class="tdstyle">
                                                            <asp:Label ID="Label26" runat="server" Text=" UPC NO" CssClass="labelbold" />
                                                        </td>
                                                        <td id="TDOURCODE" runat="server" class="tdstyle">
                                                            <asp:Label ID="Label27" runat="server" Text=" OUR CODE" CssClass="labelbold" />
                                                        </td>
                                                        <td id="TDlblBUYERCODE" runat="server" class="tdstyle">
                                                            <asp:Label ID="Label28" runat="server" Text=" BUYER CODE" CssClass="labelbold" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td id="TDTxtArticleNo" runat="server">
                                                            <asp:TextBox CssClass="textb" ID="TxtArticleNo" runat="server" Width="220px" onkeydown="return (event.keyCode!=13);"
                                                                AutoPostBack="True" BackColor="#7B96BB" OnTextChanged="TxtArticleNo_TextChanged"></asp:TextBox>
                                                        </td>
                                                        <td id="TDTXTUPCNO" runat="server">
                                                            <asp:TextBox CssClass="textb" ID="TXTUPCNO" runat="server" BackColor="#7B96BB" Width="220px"
                                                                onkeydown="return (event.keyCode!=13);"></asp:TextBox>
                                                        </td>
                                                        <td id="TDTXTOURCODE" runat="server">
                                                            <asp:TextBox CssClass="textb" ID="TXTOURCODE" runat="server" BackColor="#7B96BB"
                                                                Width="220px" onkeydown="return (event.keyCode!=13);"></asp:TextBox>
                                                        </td>
                                                        <td id="TDTXTBUYERCODE" runat="server">
                                                            <asp:TextBox CssClass="textb" ID="TXTBUYERCODE" runat="server" BackColor="#7B96BB"
                                                                Width="220px" onkeydown="return (event.keyCode!=13);"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                </table>
                                                <table>
                                                    <tr class="tdstyle" runat="server" id="TRInstructions">
                                                        <td id="trprod" runat="server">
                                                            <asp:Label ID="Label29" runat="server" Text=" PRODUCTION INSTRUCTIONS" CssClass="labelbold" />
                                                            <br />
                                                            <asp:TextBox ID="TxtWeavingInstructions" runat="server" CssClass="textb" Width="220px"
                                                                BackColor="#7B96BB" onkeydown="return (event.keyCode!=13);"></asp:TextBox>
                                                        </td>
                                                        <td id="trfin" runat="server">
                                                            <asp:Label ID="Label30" runat="server" Text=" FINISHING INSTRUCTIONS" CssClass="labelbold" />
                                                            <br />
                                                            <asp:TextBox CssClass="textb" ID="TxtFinishingInstructions" runat="server" Width="220px"
                                                                BackColor="#7B96BB" onkeydown="return (event.keyCode!=13);"></asp:TextBox>
                                                        </td>
                                                        <td id="trdye" runat="server" class="tdstyle">
                                                            <asp:Label ID="Label31" runat="server" Text="DYEING INSTRUCTIONS" CssClass="labelbold" />
                                                            <br />
                                                            <asp:TextBox CssClass="textb" ID="TxtDyeingInstructions" runat="server" Width="220px"
                                                                BackColor="#7B96BB" onkeydown="return (event.keyCode!=13);"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="tdstyle" runat="server" id="tdpkins">
                                                            <asp:Label ID="LblPKGInstruction" runat="server" CssClass="labelbold" Text="PKG Instruction"></asp:Label>
                                                            <br>
                                                            <asp:TextBox ID="TxtPKGInstruction" runat="server" Width="220px" CssClass="textb"
                                                                BackColor="#7B96BB" onkeydown="return (event.keyCode!=13);"></asp:TextBox>
                                                        </td>
                                                        <td id="TDLblLBGInstruction" runat="server" class="tdstyle">
                                                            <asp:Label ID="LblLBGInstruction" runat="server" Text="LBG Instruction" CssClass="labelbold"></asp:Label>
                                                            <br />
                                                            <asp:TextBox ID="TxtLBGInstruction" runat="server" Width="220px" CssClass="textb"
                                                                BackColor="#7B96BB" onkeydown="return (event.keyCode!=13);"></asp:TextBox>
                                                        </td>
                                                        <td class="tdstyle" id="tdremark" runat="server">
                                                            <asp:Label ID="lblremak" runat="server" Text="Remarks" CssClass="labelbold"></asp:Label>
                                                            <br />
                                                            <asp:TextBox ID="txtremark" runat="server" Width="220px" BackColor="#7B96BB" CssClass="textb"
                                                                onkeydown="return (event.keyCode!=13);"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                </table>
                                                <table>
                                                    <tr>
                                                        <td id="trlessadv" runat="server" class="tdstyle">
                                                            <asp:Label ID="lbllesadv" runat="server" Text="Less Advance" CssClass="labelbold"></asp:Label>
                                                            <br />
                                                            <asp:TextBox ID="txtlessadv" runat="server" Width="220px" CssClass="textb" BackColor="#7B96BB"
                                                                onkeydown="return (event.keyCode!=13);"></asp:TextBox>
                                                        </td>
                                                        <td id="trlesscomm" runat="server" class="tdstyle">
                                                            <asp:Label ID="lbllescomm" runat="server" Text="Less Commission" CssClass="labelbold"></asp:Label>
                                                            <br />
                                                            <asp:TextBox ID="txtcomm" runat="server" Width="220px" CssClass="textb" BackColor="#7B96BB"
                                                                onkeydown="return (event.keyCode!=13);"></asp:TextBox>
                                                        </td>
                                                        <td id="TDtxtdiscount" runat="server">
                                                            <asp:Label ID="lbldiss" runat="server" Text="Less Discount" CssClass="labelbold"></asp:Label>
                                                            <br />
                                                            <asp:TextBox ID="Txtdiscount" runat="server" Width="220px" CssClass="textb" BackColor="#7B96BB"
                                                                onkeydown="return (event.keyCode!=13);"></asp:TextBox>
                                                        </td>
                                                        <td align="right">
                                                            <br />
                                                            &nbsp;<asp:Button ID="BtnPhoto" CssClass="buttonnorm" runat="server" Text="Photo"
                                                                OnClientClick="return AddPhoto()" Visible="False" />
                                                            &nbsp;<asp:Button ID="BtnReference" CssClass="buttonnorm" runat="server" Text="Reference Image"
                                                                OnClientClick="return AddReferenceImage()" Visible="False" Width="120px" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </div>
                        </ContentTemplate>
                    </asp:TabPanel>
                </asp:TabContainer>
                <div>
                    <table>
                        <tr>
                            <td align="center">
                                <asp:Button ID="refreshPhotoRefImage" runat="server" Text="" Style="display: none"
                                    OnClick="refreshPhotoRefImage_Click" />
                                <asp:CheckBox ID="Chksupply" Text="Check For DP Order " CssClass="checkboxbold" runat="server"
                                    AutoPostBack="True" OnCheckedChanged="Chksupply_CheckedChanged" Visible="false" />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp
                                <asp:Button ID="BtnShowConsumption" CssClass="buttonnorm" Width="150px" runat="server"
                                    Text="DP Order" Enabled="false" OnClientClick="return AddLocalConsumption()"
                                    Visible="false" />
                                <asp:CheckBox ID="ChkForInternal_OC" Text="Check For Internal (OC)" CssClass="checkboxbold"
                                    runat="server" AutoPostBack="True" OnCheckedChanged="ChkForInternal_OC_CheckedChanged"
                                    Visible="false" />
                                <asp:CheckBox ID="cbExport" Text="Check For Export " CssClass="checkboxbold" runat="server"
                                    Visible="true" />
                                &nbsp;<asp:DropDownList ID="ddPreview" AutoPostBack="true" runat="server" CssClass="dropdown"
                                    OnSelectedIndexChanged="ddPreview_SelectedIndexChanged">
                                </asp:DropDownList>
                                &nbsp;<asp:Button CssClass="buttonnorm" ID="BtnNew" runat="server" Text="New" OnClientClick="return reloadPage();" />
                                <asp:Button CssClass="buttonnorm" ID="BtnSave" Text="Save" runat="server" OnClick="BtnSave_Click"
                                    OnClientClick="return checkDate(0);" ValidationGroup="f1" />
                                <%-- OnClientClick="return confirm('Do you want to save data?')"--%>
                                <asp:Button CssClass="buttonnorm" ID="BtnClose" runat="server" Text="Close" OnClientClick="return CloseForm();" />
                                <asp:Button CssClass="buttonnorm  preview_width" ID="BtnReport" runat="server" Text="Preview"
                                    OnClick="BtnReport_Click" />
                                <asp:Button CssClass="buttonnorm " ID="BtncostReport" runat="server" Text="Costing Report"
                                    Visible="false" OnClick="BtncostReport_Click" />
                            </td>
                            <td id="TDupdateallconsumption" runat="server">
                                <asp:Button ID="btnupdateallconsmp" Text="Update All Consumption" runat="server"
                                    UseSubmitBehavior="false" CssClass="buttonnorm" OnClick="btnupdateallconsmp_Click"
                                    OnClientClick="if (!confirm('Do you want update all item consumption?')) return ;this.disabled=true;this.value = 'wait ...';" />
                            </td>
                            <td>
                                <asp:Button ID="btnchngorderno" CssClass="buttonnorm" Text="Change Cust. Order No."
                                    runat="server" OnClick="btnchngorderno_Click" Visible="false" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CHKFORCURRENTCONSUMPTION" Text="FOR CURRENT CONSUMPTION" runat="server"
                                    Width="200px" Visible="False" />
                            </td>
                            <td>
                                <asp:CheckBox ID="ChkForEditGrid" Text="FOR EDIT" runat="server" Visible="false" />
                            </td>
                            <td>
                                <asp:CheckBox ID="ChkForPerformaInvoiceType2" Text="FOR PERFORMA INVOICE 2" runat="server"
                                    Width="200px" Visible="False" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="LblErrorMessage" runat="server" Font-Bold="true" ForeColor="RED" Text="Some Important Fields are Missing......."
                                    Visible="false"></asp:Label>
                                <asp:Label ID="Lblmessage" runat="server" Text="" ForeColor="red" CssClass="labelbold"
                                    Font-Size="Small" Visible="false"></asp:Label>
                            </td>
                        </tr>
                    </table>
                    <table>
                        <tr>
                            <td>
                                <table width="100%" cellpadding="0" cellspacing="0" border="0">
                                    <tr id="trgrid1" runat="server">
                                        <td>
                                            <asp:TextBox ID="txtgreen" BackColor="Green" Width="15px" CssClass="textb" runat="server"
                                                Enabled="false"></asp:TextBox>
                                            <asp:TextBox ID="Note" runat="server" BorderStyle="None" Width="180px" Text="Consumption Defined"
                                                Enabled="false" CssClass="textb" Font-Bold="true"></asp:TextBox>
                                            <div style="width: 1000px; max-height: 500px; overflow: auto; margin-top: 10px">
                                                <asp:GridView ID="DGOrderDetail" Width="100%" runat="server" DataKeyNames="Sr_No"
                                                    AutoGenerateColumns="False" OnRowDataBound="DGOrderDetail_RowDataBound" OnSelectedIndexChanged="DGOrderDetail_SelectedIndexChanged"
                                                    OnSorting="DGOrderDetail_Sorting" AllowSorting="true" OnRowDeleting="DGOrderDetail_RowDeleting"
                                                    CssClass="grid-views">
                                                    <HeaderStyle CssClass="gvheaders" ForeColor="White" />
                                                    <AlternatingRowStyle CssClass="gvalts" />
                                                    <RowStyle CssClass="gvrow" />
                                                    <Columns>
                                                        <asp:TemplateField HeaderText="SrNo" Visible="true" SortExpression="srno">
                                                            <ItemTemplate>
                                                                <%-- <asp:Label ID="lblId2" Text='<%#Container.DataItemIndex+1 %>' runat="server" />--%>
                                                                <asp:Label ID="lblId2" Text='<%#Bind("srno") %>' runat="server" />
                                                            </ItemTemplate>
                                                            <ItemStyle Width="100px" HorizontalAlign="Center" />
                                                        </asp:TemplateField>
                                                        <asp:BoundField DataField="ITEM_NAME" HeaderText="ITEM_NAME" SortExpression="ITEM_NAME">
                                                            <HeaderStyle HorizontalAlign="Left" Width="100px" />
                                                            <ItemStyle HorizontalAlign="Left" Width="100px" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="Quality" HeaderText="Quality" SortExpression="Quality">
                                                            <HeaderStyle HorizontalAlign="Left" Width="250px" />
                                                            <ItemStyle HorizontalAlign="Left" Width="250px" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="Design" HeaderText="Design" SortExpression="Design">
                                                            <HeaderStyle HorizontalAlign="Left" Width="250px" />
                                                            <ItemStyle HorizontalAlign="Left" Width="250px" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="Color" HeaderText="Color" SortExpression="Color">
                                                            <HeaderStyle HorizontalAlign="Left" Width="250px" />
                                                            <ItemStyle HorizontalAlign="Left" Width="250px" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="Shape" HeaderText="Shape" SortExpression="Shape">
                                                            <HeaderStyle HorizontalAlign="Left" Width="250px" />
                                                            <ItemStyle HorizontalAlign="Left" Width="250px" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="Size" HeaderText="Size" SortExpression="Size">
                                                            <HeaderStyle HorizontalAlign="Left" Width="250px" />
                                                            <ItemStyle HorizontalAlign="Left" Width="250px" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="Description" HeaderText="Description" SortExpression="Description">
                                                            <HeaderStyle HorizontalAlign="Left" Width="350px" />
                                                            <ItemStyle HorizontalAlign="Left" Width="350px" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="Qty" HeaderText="Qty" SortExpression="Qty">
                                                            <HeaderStyle HorizontalAlign="Left" Width="30px" />
                                                            <ItemStyle HorizontalAlign="Left" Width="30px" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="Area" HeaderText="Area">
                                                            <HeaderStyle HorizontalAlign="Left" Width="50px" />
                                                            <ItemStyle HorizontalAlign="Left" Width="50px" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="Rate" HeaderText="Rate">
                                                            <HeaderStyle HorizontalAlign="Left" Width="50px" />
                                                            <ItemStyle HorizontalAlign="Left" Width="50px" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="Amount" HeaderText="Amount">
                                                            <HeaderStyle HorizontalAlign="Left" Width="70px" />
                                                            <ItemStyle HorizontalAlign="Left" Width="70px" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="CurrencyName" HeaderText="CurrencyName">
                                                            <HeaderStyle HorizontalAlign="Left" Width="90px" />
                                                            <ItemStyle HorizontalAlign="Left" Width="90px" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="ArticalNo" HeaderText="ArticalNo">
                                                            <HeaderStyle HorizontalAlign="Left" Width="100px" />
                                                            <ItemStyle HorizontalAlign="Left" Width="100px" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="WeavingInstruction" HeaderText="W Inst">
                                                            <HeaderStyle HorizontalAlign="Left" Width="150px" />
                                                            <ItemStyle HorizontalAlign="Left" Width="150px" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="FinishingInstructions" HeaderText="F Inst">
                                                            <HeaderStyle HorizontalAlign="Left" Width="150px" />
                                                            <ItemStyle HorizontalAlign="Left" Width="150px" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="DyeingInstructions" HeaderText="D Inst">
                                                            <HeaderStyle HorizontalAlign="Left" Width="150px" />
                                                            <ItemStyle HorizontalAlign="Left" Width="150px" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="Remark" HeaderText="Remark">
                                                            <HeaderStyle HorizontalAlign="Left" Width="150px" />
                                                            <ItemStyle HorizontalAlign="Left" Width="150px" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="PKGInstruction" HeaderText="PKGInst">
                                                            <HeaderStyle HorizontalAlign="Left" Width="150px" />
                                                            <ItemStyle HorizontalAlign="Left" Width="150px" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="LBGInstruction" HeaderText="LBGInst">
                                                            <HeaderStyle HorizontalAlign="Left" Width="150px" />
                                                            <ItemStyle HorizontalAlign="Left" Width="150px" />
                                                        </asp:BoundField>
                                                        <asp:TemplateField ShowHeader="False">
                                                            <ItemTemplate>
                                                                <asp:LinkButton ID="LinkButton1" runat="server" CausesValidation="False" CommandName="Delete"
                                                                    Text="Del" OnClientClick="return doConfirm();"></asp:LinkButton>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Left" Width="100px" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField ShowHeader="true" HeaderText="AddImage">
                                                            <ItemTemplate>
                                                                <asp:HyperLink ID="hlDetails1" runat="server" NavigateUrl='<%# "../Carpet/AddPhotoRefImage.aspx?shapeid=1&orderid=" + Eval("orderid") + "&orderdetailid=" + Eval("Sr_No")%>'
                                                                    Target="_blank" Text="ADD IMAGE" />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Add Consumption">
                                                            <ItemTemplate>
                                                                <asp:Button ID="hnaddconsumption" runat="server" Text="ADD CONSUMPTION" OnClientClick="return AddConsumption(this);"
                                                                    CssClass="buttonnorm" />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <%--<asp:TemplateField ShowHeader="true" HeaderText="AddProcess">
                                                            <ItemTemplate>
                                                                <asp:HyperLink ID="hlProcessName" runat="server" NavigateUrl='<%# "../Order/AddProcessName.aspx?OrderID=" + Eval("orderid") + "&OrderDetailID=" + Eval("Sr_No")%>'
                                                                    Target="_blank" Text="ADD PROCESS" />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>--%>
                                                        <asp:TemplateField HeaderText="Consmpflag" Visible="false">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblconsumpflag" Text='<%#Bind("Consmpflag") %>' runat="server" />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Finished_ID" HeaderStyle-Width="0px">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblFinishedid" Text='<%#Bind("finished") %>' runat="server" />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="FlagSize" HeaderStyle-Width="0px">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblflagsize" Text='<%#Bind("flagsize") %>' runat="server" />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                    </Columns>
                                                    <EmptyDataRowStyle CssClass="gvemptytext" />
                                                </asp:GridView>
                                            </div>
                                        </td>
                                    </tr>
                                    <tr id="trgrid2" runat="server" visible="false">
                                        <td>
                                            <div style="width: 1000px; height: 200px; overflow: scroll">
                                                <asp:GridView ID="DGOrderDetail2" runat="server" AutoGenerateColumns="False" DataKeyNames="Sr_No"
                                                    OnRowDataBound="DGOrderDetail_RowDataBound" OnRowDeleting="DGOrderDetail_RowDeleting"
                                                    OnSelectedIndexChanged="DGOrderDetail_SelectedIndexChanged" Width="100%" CssClass="grid-views">
                                                    <HeaderStyle CssClass="gvheaders" />
                                                    <AlternatingRowStyle CssClass="gvalts" />
                                                    <RowStyle CssClass="gvrow" />
                                                    <Columns>
                                                        <asp:BoundField DataField="productCode" HeaderText="ITEM_CODE">
                                                            <HeaderStyle HorizontalAlign="Left" Width="100px" />
                                                            <ItemStyle HorizontalAlign="Left" Width="100px" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="ITEM_NAME" HeaderText="ITEM_NAME">
                                                            <HeaderStyle HorizontalAlign="Left" Width="100px" />
                                                            <ItemStyle HorizontalAlign="Left" Width="100px" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="Description1" HeaderText="Description">
                                                            <HeaderStyle HorizontalAlign="Left" Width="350px" />
                                                            <ItemStyle HorizontalAlign="Left" Width="350px" />
                                                        </asp:BoundField>
                                                        <asp:TemplateField HeaderText="PHOTO">
                                                            <ItemTemplate>
                                                                <%--<asp:Image ID="Image1" runat="server" Height="50px" ImageUrl='<%# "~/ImageHandler.ashx?ID=" + Eval("Sr_No")+"&img=4"%>'
                                                                                Width="100px" />--%>
                                                                <asp:Image ID="Image" runat="server" ImageUrl='<%# Bind("photo") %>' Height="70px"
                                                                    Width="100px" />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:BoundField DataField="Qty" HeaderText="Qty">
                                                            <HeaderStyle HorizontalAlign="Left" Width="30px" />
                                                            <ItemStyle HorizontalAlign="Left" Width="30px" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="WeavingInstruction" HeaderText="W Inst" Visible="false">
                                                            <HeaderStyle HorizontalAlign="Left" Width="150px" />
                                                            <ItemStyle HorizontalAlign="Left" Width="150px" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="FinishingInstructions" HeaderText="F Inst" Visible="false">
                                                            <HeaderStyle HorizontalAlign="Left" Width="150px" />
                                                            <ItemStyle HorizontalAlign="Left" Width="150px" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="DyeingInstructions" HeaderText="D Inst" Visible="false">
                                                            <HeaderStyle HorizontalAlign="Left" Width="150px" />
                                                            <ItemStyle HorizontalAlign="Left" Width="150px" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="Remark" HeaderText="Remark">
                                                            <HeaderStyle HorizontalAlign="Left" Width="150px" />
                                                            <ItemStyle HorizontalAlign="Left" Width="150px" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="PKGInstruction" HeaderText="PKGInst" Visible="false">
                                                            <HeaderStyle HorizontalAlign="Left" Width="150px" />
                                                            <ItemStyle HorizontalAlign="Left" Width="150px" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="LBGInstruction" HeaderText="LBGInst" Visible="false">
                                                            <HeaderStyle HorizontalAlign="Left" Width="150px" />
                                                            <ItemStyle HorizontalAlign="Left" Width="150px" />
                                                        </asp:BoundField>
                                                        <asp:TemplateField HeaderText="CONSUMPTION" Visible="false">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblconsumption" runat="server" Text='<%# getgiven(DataBinder.Eval(Container.DataItem, "Sr_No").ToString(),DataBinder.Eval(Container.DataItem, "orderid").ToString()) %>' />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Raw Material">
                                                            <ItemTemplate>
                                                                <asp:HyperLink ID="hlDetails1" runat="server" NavigateUrl='<%# "FrmLocalConsumptionForOrder.aspx?orderid=" + Eval("orderid") + "&orderdetailid=" + Eval("Sr_No") + "&finishedid=" + Eval("finished") + "&qty=" + Eval("Qty")%>'
                                                                    Target="_blank" Text="Details" />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField ShowHeader="true">
                                                            <ItemTemplate>
                                                                <asp:HyperLink ID="hlDetails2" runat="server" NavigateUrl='<%# "../Carpet/AddPhotoRefImage.aspx?shapeid=1&orderid=" + Eval("orderid") + "&orderdetailid=" + Eval("Sr_No")%>'
                                                                    Target="_blank" Text="ADD IMAGE" />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Consmpflag" Visible="false">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblconsumpflag" Text='<%#Bind("Consmpflag") %>' runat="server" />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Finished_ID" HeaderStyle-Width="0px">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblFinishedid" Text='<%#Bind("finished") %>' runat="server" />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="FlagSize" HeaderStyle-Width="0px">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblflagsize" Text='<%#Bind("flagsize") %>' runat="server" />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField ShowHeader="False">
                                                            <ItemTemplate>
                                                                <asp:LinkButton ID="LinkButton1" runat="server" CausesValidation="False" CommandName="Delete"
                                                                    OnClientClick="return doConfirm();" Text="Del"></asp:LinkButton>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                    </Columns>
                                                    <EmptyDataRowStyle CssClass="gvemptytext" />
                                                </asp:GridView>
                                            </div>
                                        </td>
                                    </tr>
                                </table>
                                <table>
                                    <tr>
                                        <td>
                                            <asp:TextBox ID="TxtFinishedid" Style="display: none" runat="server"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr runat="server" id="trtot">
                                        <td>
                                            <asp:HiddenField ID="HDF1" runat="server" />
                                            <asp:HiddenField ID="hnsst" runat="server" />
                                            <asp:HiddenField ID="HNLBL" runat="server" />
                                            <asp:HiddenField ID="hnqualityid" runat="server" />
                                            <asp:HiddenField ID="hnCQsrno" Value="0" runat="server" />
                                            <asp:HiddenField ID="hndesignid" runat="server" />
                                            <asp:HiddenField ID="hnUpdateStatus" runat="server" />
                                            <asp:HiddenField ID="hnOldFinishedId" runat="server" Value="0" />
                                            <asp:HiddenField ID="hnorderphoto" runat="server" Value="" />
                                            <asp:HiddenField ID="hncustomerorderNo" runat="server" Value="" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Button ID="refreshgrid" runat="server" Text="" Style="display: none" OnClick="Refreshgrid_Click" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </div>
                <div>
                    <asp:Button runat="server" ID="btnModalPopUp" Style="display: none" />
                    <asp:ModalPopupExtender ID="ModalPopupExtender1" runat="server" PopupControlID="pnModelPopup"
                        TargetControlID="btnModalPopUp" DropShadow="true" BackgroundCssClass="modalBackground"
                        CancelControlID="btnCancel" PopupDragHandleControlID="pnModelPopup" OnOkScript="onOk()">
                    </asp:ModalPopupExtender>
                    <asp:Panel ID="pnModelPopup" runat="server" Style="background-color: White; border: 3px solid #0DA9D0;
                        border-radius: 12px; padding: 0; display: none" Height="100px" Width="330px">
                        <table>
                            <tr>
                                <td>
                                    <asp:Label ID="lbloldorderno" runat="server" Text="OLD Cust. Order No." ForeColor="Red"
                                        CssClass="labelbold"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtoldcustorderno" Enabled="false" Width="200px" runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="Label10" runat="server" Text="NEW Cust. Order No." ForeColor="Red"
                                        CssClass="labelbold"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtnewcustorderno" Width="200px" runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <td align="right" colspan="2">
                                    <asp:Button ID="btnchangeorderno" Text="Change Order No." runat="server" CssClass="buttonnorm"
                                        OnClick="btnchangeorderno_Click" />
                                    <asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="buttonnorm" Width="100px" />
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                </div>
                <div id="mask">
                </div>
                <script type="text/javascript" language="javascript">
                    function ShowPopup() {
                        $('#mask').show();
                        $('#<%=pnModelPopup.ClientID %>').show();
                    }
                    function HidePopup() {
                        $('#mask').hide();
                        $('#<%=pnModelPopup.ClientID %>').hide();
                    }
                    $(".btnPwd").live('click', function () {
                        HidePopup();
                    });
                </script>
                <style type="text/css">
                    #mask
                    {
                        position: fixed;
                        left: 0px;
                        top: 0px;
                        z-index: 4;
                        opacity: 0.4;
                        -ms-filter: "progid:DXImageTransform.Microsoft.Alpha(Opacity=40)"; /* first!*/
                        filter: alpha(opacity=40); /* second!*/
                        background-color: Gray;
                        display: none;
                        width: 100%;
                        height: 100%;
                    }
                </style>
            </ContentTemplate>
            <Triggers>
                <asp:PostBackTrigger ControlID="BtnReport" />
            </Triggers>
        </asp:UpdatePanel>
    </div>
    </form>
</body>
</html>
