<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DRAFTORDRNEW.aspx.cs" ViewStateMode="Enabled"
    Title="Draft Order" Inherits="Masters_Order_DRAFTORDRNEW" EnableEventValidation="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%--<%@ Register Assembly="RbmControls" Namespace="RbmControls" TagPrefix="Rbm" %>--%>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register Src="~/UserControls/ucmenu.ascx" TagName="ucmenu" TagPrefix="uc2" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Company</title>
    <script src="../../Scripts/JScript.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery-1.4.1.min.js" type="text/javascript"></script>
    <%--<script type="text/javascript" src="../../Scripts/Fixfocus.js"></script>--%>
    <link href="../../Styles/vijay.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        #newPreview
        {
            filter: progid:DXImageTransform.Microsoft.AlphaImageLoader(sizingMethod=scale);
        }
        #DivReferenceImage
        {
            filter: progid:DXImageTransform.Microsoft.AlphaImageLoader(sizingMethod=scale);
        }
    </style>
    <script language="javascript" type="text/javascript">
        function RefreshForm(id) {
            if (document.getElementById('DDQuality')) {
                document.getElementById('DDQuality').selectedIndex = 0;
            }
            if (document.getElementById('DDDesign')) {
                document.getElementById('DDDesign').selectedIndex = 0;
            }
            if (document.getElementById('DDColor')) {
                document.getElementById('DDColor').selectedIndex = 0;
            }
            if (document.getElementById('DDShape')) {
                document.getElementById('DDShape').selectedIndex = 0;
            }
            if (document.getElementById('DDSize')) {
                document.getElementById('DDSize').selectedIndex = 0;
            }
            if (document.getElementById('TxtArea')) {
                document.getElementById('TxtArea').Text = "";
            }
            if (document.getElementById('TxtQuantity')) {
                document.getElementById('TxtQuantity').Text = "";
            }
            if (document.getElementById('TxtPrice')) {
                document.getElementById('TxtPrice').Text = "";
            }
            if (document.getElementById('TxtTotalAmount')) {
                document.getElementById('TxtTotalAmount').Text = "";
            }
            if (document.getElementById('TxtTotalQtyRequired')) {
                document.getElementById('TxtTotalQtyRequired').Text = "";
            }
            if (document.getElementById('TxtOrderArea')) {
                document.getElementById('TxtOrderArea').Text = "";
            }
            if (document.getElementById('TXTRemarks')) {
                document.getElementById('TXTRemarks').Text = "";
            }
            if (document.getElementById('TxtUPCNO')) {
                document.getElementById('TxtUPCNO').Text = "";
            }
            if (document.getElementById('TxtWeight')) {
                document.getElementById('TxtWeight').Text = "";
            }
            if (document.getElementById('TxtOurCode')) {
                document.getElementById('TxtOurCode').Text = "";
            }
            if (document.getElementById('TxtBuyerCode')) {
                document.getElementById('TxtBuyerCode').Text = "";
            }
            if (document.getElementById('TxtPKGInstruction')) {
                document.getElementById('TxtPKGInstruction').Text = "";
            }
            if (document.getElementById('TxtLBGInstruction')) {
                document.getElementById('TxtLBGInstruction').Text = "";
            }
            if (document.getElementById('TxtDescription')) {
                document.getElementById('TxtDescription').Text = "";
            }
            if (document.getElementById('TxtGST')) {
                document.getElementById('TxtGST').Text = "";
            }
            if (document.getElementById('TxtIGST')) {
                document.getElementById('TxtIGST').Text = "";
            }
            if (document.getElementById('txtLocationType')) {
                document.getElementById('txtLocationType').Text = "";
            }
            if (document.getElementById('txtMaterial')) {
                document.getElementById('txtMaterial').Text = "";
            }
            if (document.getElementById('txtTexture')) {
                document.getElementById('txtTexture').Text = "";
            }
            if (document.getElementById('txtReportRef')) {
                document.getElementById('txtReportRef').Text = "";
            }
            if (id = 1) {
                ONnew();
            }
        }
        function doClick(buttonName, e) {
            //the purpose of this function is to allow the enter key to 
            //point to the correct button to click.
            var key;

            if (window.event)
                key = window.event.keyCode;     //IE
            else
                key = e.which;     //firefox

            if (key == 13) {
                //Get the button the user wants to have clicked
                var btn = document.getElementById(buttonName);
                if (btn != null) { //If we find the button click it
                    btn.click();
                    event.keyCode = 0
                }
            }
        }
        function PreviewImg(imgFile) {
            var newPreview = document.getElementById("newPreview");
            document.getElementById("newPreview").value = "";
            newPreview.filters.item("DXImageTransform.Microsoft.AlphaImageLoader").src = imgFile.value;
            newPreview.style.width = "111px";
            newPreview.style.height = "66px";
            var control = document.getElementById("newPreview1");
            control.style.visibility = "hidden";
        }
        function PreviewReferenceImage(imgFile) {
            var newPreviewRef = document.getElementById("DivReferenceImage");
            document.getElementById("DivReferenceImage").value = "";
            newPreviewRef.filters.item("DXImageTransform.Microsoft.AlphaImageLoader").src = imgFile.value;
            newPreviewRef.style.width = "111px";
            newPreviewRef.style.height = "66px";
            var controlRef = document.getElementById("ImageReferenceImage");
            controlRef.style.visibility = "hidden";
        }
        //        function CloseForm() {
        //            window.location.href = "../../main.aspx";
        //        }
        function AddItum() {
            var a3 = document.getElementById('TxtFinishedid').value;
            window.open('../Carpet/AddItemName.aspx?' + a3, '', 'Height=400px,width=500px');
            // window.open('AddItemt.aspx','popup Form', 'Width=550px, Height=400px') 
        }
        function AddItemCode() {
            var a3 = document.getElementById('TxtFinishedid').value;
            window.open('../Carpet/AddItemCode.aspx?' + a3);
        }
        function report() {
            window.open('../../ReportViewer.aspx', '');
        }
        function AddItemCategory() {
            window.open('../Carpet/AddItemCategory.aspx', '', 'Height=400px,width=600px');
        }
        function AddQuality() {
            var a3 = document.getElementById('TxtFinishedid').value;
            window.open('../Carpet/AddQuality.aspx?' + a3, '', 'Height=400px,width=500px');
        }
        function AddDesign() {
            window.open('../Carpet/AddDesign.aspx', '', 'Height=400px,width=500px');
        }
        function AddColor() {
            window.open('../Carpet/AddColor.aspx', '', 'Height=400px,width=500px');
        }
        function AddShadecolor() {
            window.open('../Carpet/AddShadeColor.aspx', '', 'Height=400px,width=500px');
        }
        function AddShape() {
            window.open('../Carpet/AddShape.aspx', '', 'Height=400px,width=500px');
        }
        function AddSize() {
            window.open('../Carpet/AddSize.aspx', '', 'Height=500px,width=1000px');
        }
        function CalculateContainer() {
            var varcode = document.getElementById('TxtOrderDetailId').value;
            window.open('../Carpet/ContainerCost.aspx?itemcode=' + varcode + '&PackingType=5', '', 'width=700px,Height=400px');
        }
        function AddLocalConsumption() {
            //var Orderid = document.getElementById('TxtOrderID').value;
            var companyId = document.getElementById('HfCompanyId');
            if (companyId.value == "2") {
                window.open('../Carpet/FrmOrderWiseConsumption.aspx', '', 'Height=500px,width=850px');
            }
            else {
                window.open('../Carpet/DefineBomAndConsumption.aspx?ZZZ=1', '', 'Height=600px,width=1000px');
            }


        }
        function ONnew() {
            if (document.getElementById('TxtProdCode')) {
                document.getElementById('TxtProdCode').value = "";
            }
            if (document.getElementById('TxtQuantity')) {
                document.getElementById('TxtQuantity').value = "";
            }
            if (document.getElementById('TxtPrice')) {
                document.getElementById('TxtPrice').value = "";
            }
            if (document.getElementById('TxtWeight')) {
                document.getElementById('TxtWeight').value = "";
            }
            if (document.getElementById('BtnSave')) {
                document.getElementById('BtnSave').value = "Save";
            }
        }

        function checkDateOnTextBox() {
            var datevalue1 = document.forms[0].TxtOrderDate.value;
            var datevalue2 = document.forms[0].TxtDeliveryDate.value;
            var datevalue3 = document.forms[0].txtduedate.value;
            var datevalue4 = document.forms[0].Txtcustorderdt.value;

            var day1 = datevalue1.substring(0, 2);
            var month1 = datevalue1.substring(3, 6);
            var year1 = datevalue1.substring(7, 11);

            var day2 = datevalue2.substring(0, 2);
            var month2 = datevalue2.substring(3, 6);
            var year2 = datevalue2.substring(7, 11);

            var day3 = datevalue3.substring(0, 2);
            var month3 = datevalue3.substring(3, 6);
            var year3 = datevalue3.substring(7, 11);

            var day4 = datevalue4.substring(0, 2);
            var month4 = datevalue4.substring(3, 6);
            var year4 = datevalue4.substring(7, 11);

            month1 = changeFormatStringtoNumber(month1);
            month2 = changeFormatStringtoNumber(month2);
            month3 = changeFormatStringtoNumber(month3);
            month4 = changeFormatStringtoNumber(month4);
            //            alert(month2);

            var d2 = new Date(year1, month1, day1);
            var d3 = new Date(year2, month2, day2);
            var d4 = new Date(year3, month3, day3);
            var d1 = new Date(year4, month4, day4);
            //            alert("-----");
            //            alert(d1);
            //            alert(d2);
            if (d1 > d2) {

                //                alert("Order date can not be less than Cust Order date !");
                //                return false;

            }
            else if (d2 > d3) {


                alert("Order date can not be less than Delivery date !");

                return false;

            }
            else if (d3 > d4) {


                alert("Due date can not be less than Delivery Order date !");

                return false;

            }
            else {

            }
        }

        function checkDate() {
            var datevalue1 = document.forms[0].TxtOrderDate.value;
            var datevalue2 = document.forms[0].TxtDeliveryDate.value;
            var datevalue3 = document.forms[0].txtduedate.value;
            var datevalue4 = document.forms[0].Txtcustorderdt.value;

            var day1 = datevalue1.substring(0, 2);
            var month1 = datevalue1.substring(3, 6);
            var year1 = datevalue1.substring(7, 11);

            var day2 = datevalue2.substring(0, 2);
            var month2 = datevalue2.substring(3, 6);
            var year2 = datevalue2.substring(7, 11);

            var day3 = datevalue3.substring(0, 2);
            var month3 = datevalue3.substring(3, 6);
            var year3 = datevalue3.substring(7, 11);

            var day4 = datevalue4.substring(0, 2);
            var month4 = datevalue4.substring(3, 6);
            var year4 = datevalue4.substring(7, 11);

            month1 = changeFormatStringtoNumber(month1);
            month2 = changeFormatStringtoNumber(month2);
            month3 = changeFormatStringtoNumber(month3);
            month4 = changeFormatStringtoNumber(month4);
            //            alert(month2);

            var d2 = new Date(year1, month1, day1);
            var d3 = new Date(year2, month2, day2);
            var d4 = new Date(year3, month3, day3);
            var d1 = new Date(year4, month4, day4);
            //            alert("-----");
            //            alert(d1);
            //            alert(d2);
            if (d1 > d2) {

                //                alert("Order date can not be less than Cust Order date !");
                //                document.getElementById("TxtOrderDate").value = document.getElementById("Txtcustorderdt").value;
                //                return false;

            }
            else if (d2 > d3) {


                alert("Order date can not be less than Delivery date !");
                document.getElementById("TxtDeliveryDate").value = document.getElementById("TxtOrderDate").value;

                return false;

            }
            else if (d3 > d4) {


                alert("Due date can not be less than Delivery Order date !");
                document.getElementById("txtduedate").value = document.getElementById("TxtDeliveryDate").value;
                return false;

            }
            else {
                alert('Do you want to save data?');
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

            //            alert("---****----");
            //            alert(m);
            return (monthval);

        }
        function isNumberKey1(evt) {
            var charCode = (evt.which) ? evt.which : event.keyCode
            if (charCode > 31 && (charCode < 48 || charCode > 57)) {
                alert('numeric only');
                return false;
            }
            else {
                return true;
            }
        }
        function isNumberDecimal(evt) {
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
            var a3 = document.getElementById('DDCustomerCode').value;
            var e = document.getElementById("DDItemCategory");
            var VarCategoryid = e.options[e.selectedIndex].value;
            var e1 = document.getElementById("DDItemName");
            var VarItemid = e1.options[e1.selectedIndex].value;
            window.open('../Carpet/BuyerMasterCode.aspx?ABC=1&CustomerID=' + a3 + '&VarCategory=' + VarCategoryid + '&VarItem=' + VarItemid + '', '', 'Height=600px,width=850px');
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <%--<div style="position: absolute; left: -10000px; top: 0px; width: 1px; height: 1px;
        overflow-x: hidden; overflow-y: hidden;" id="_mcePaste">
        // Set default Button
    </div>
    <div style="position: absolute; left: -10000px; top: 0px; width: 1px; height: 1px;
        overflow-x: hidden; overflow-y: hidden;" id="_mcePaste">
        //Page.Form.DefaultButton = btnSave.UniqueID;
    </div>--%><table width="100%" border="1">
        <tr style="width: 100%" align="center">
            <td height="66px" align="center">
                <%--style="background-image:url(Images/header.jpg)" --%>
                <%--<div><img src="Images/header.jpg" alt="" /></div>--%>
                <asp:Image ID="Image1" ImageUrl="~/Images/header.jpg" runat="server" />
            </td>
            <td style="background-color: #0080C0;" width="100px" valign="bottom">
                <asp:Image ID="imgLogo" align="left" runat="server" Height="66px" Width="111px" />
                <span style="color: Black; margin-left: 30px; font-family: Arial; font-size: xx-large">
                    <strong><em><i><font size="4" face="GEORGIA">
                        <asp:Label ID="LblCompanyName" runat="server" Text=""></asp:Label></font></i></em></strong></span>
                <br />
                <i><font size="2" face="GEORGIA">
                    <asp:Label ID="LblUserName" ForeColor="White" runat="server" Text=""></asp:Label></font></i>
            </td>
        </tr>
        <tr bgcolor="#999999">
            <td class="style1">
                <uc2:ucmenu ID="ucmenu1" runat="server" />
                <asp:ScriptManager ID="ScriptManager1" runat="server">
                </asp:ScriptManager>
            </td>
            <td width="25%">
                <asp:Button Width="100%" BorderStyle="None" BackColor="#999999" ID="BtnLogout" runat="server"
                    Text="Logout" OnClick="BtnLogout_Click" />
            </td>
        </tr>
        <tr>
            <td align="center" height="inherit" valign="top" class="style1" colspan="2">
                <div id="1" style="height: auto" align="left">
                    <asp:UpdatePanel runat="server">
                        <ContentTemplate>
                            <table>
                                <tr>
                                    <td class="tdstyle">
                                        <asp:RadioButton ID="rdoUnitWise" Text="UNIT WISE" runat="server" GroupName="OrderType" />
                                    </td>
                                    <td class="tdstyle">
                                        <asp:RadioButton ID="rdoPcWise" Text="PC WISE" runat="server" GroupName="OrderType" />
                                    </td>
                                    <td class="tdstyle">
                                        <asp:CheckBox ID="ChkEditOrder" runat="server" Text="EDIT ORDER" AutoPostBack="True"
                                            OnCheckedChanged="ChkEditOrder_CheckedChanged" />
                                    </td>
                                    <td class="tdstyle">
                                        <asp:CheckBox ID="CHKFORCURRENTCONSUMPTION" Text="CHK FOR CURRENT CONSUMPTION" runat="server" />
                                    </td>
                                    <td class="tdstyle">
                                        <asp:CheckBox ID="ChkGeneratePINo" Text="CHK FOR GENERATE PI NO" runat="server" AutoPostBack="True"
                                            OnCheckedChanged="ChkGeneratePINo_CheckedChanged" />
                                    </td>
                                    <td>
                                        <asp:Button ID="btnContainerPackingCost" runat="server" Style="display: none" EnableTheming="true" />
                                    </td>
                                    <td>
                                        <asp:TextBox ID="TxtOrderDetailId" runat="server" Style="display: none"></asp:TextBox>
                                    </td>
                                </tr>
                                <table>
                                    <tr>
                                        <td class="tdstyle">
                                            <asp:Label ID="lbl" Text="COMPANY NAME " runat="server" CssClass="labelbold" />
                                            <b style="color: Red">*</b><br />
                                            <asp:DropDownList CssClass="dropdown" ID="DDCompanyName" runat="server" AutoPostBack="True"
                                                OnSelectedIndexChanged="DDCompanyName_SelectedIndexChanged" Width="150px" TabIndex="1">
                                            </asp:DropDownList>
                                        </td>
                                        <td class="tdstyle">
                                            CUST CODE<b style="color: Red">*</b><br />
                                            <asp:DropDownList CssClass="dropdown" ID="DDCustomerCode" runat="server" AutoPostBack="True"
                                                OnSelectedIndexChanged="DDCustomerCode_SelectedIndexChanged" Width="150px" TabIndex="2">
                                            </asp:DropDownList>
                                        </td>
                                        <td class="tdstyle" id="TDDDCustomerOrderNo" runat="server" visible="false">
                                            DRAFT ORDER NO<b style="color: Red">*</b><br />
                                            <asp:DropDownList CssClass="dropdown" ID="DDCustOrderNo" runat="server" AutoPostBack="True"
                                                Visible="false" OnSelectedIndexChanged="DDCustOrderNo_SelectedIndexChanged" TabIndex="3"
                                                Width="150px">
                                            </asp:DropDownList>
                                        </td>
                                        <td id="TDTxtLocalOrderNo" runat="server" visible="false">
                                            DRAFT ORDER NO<b style="color: Red">*</b><br />
                                            <asp:TextBox ID="TxtLocalOrderNo" runat="server" CssClass="textb" Width="150px" AutoPostBack="True"
                                                OnTextChanged="TxtLocalOrderNo_TextChanged" TabIndex="4"></asp:TextBox>
                                        </td>
                                        <td class="tdstyle">
                                            CUSTOMER ORDER NO<br />
                                            <asp:TextBox CssClass="textb" ID="TxtCustomerOrderNo" runat="server" onkeydown="return (event.keyCode!=13);"
                                                TabIndex="5"></asp:TextBox>
                                        </td>
                                        <td class="tdstyle">
                                            ORDER UNIT<b style="color: Red">*</b>
                                            <br />
                                            <asp:DropDownList CssClass="dropdown" ID="DDOrderUnit" runat="server" AutoPostBack="True"
                                                OnSelectedIndexChanged="DDOrderUnit_SelectedIndexChanged" Width="150px" TabIndex="6">
                                            </asp:DropDownList>
                                        </td>
                                        <td class="tdstyle" id="TDPINo" runat="server">
                                            PI NO<br />
                                            <asp:TextBox CssClass="textb" ID="txtPINo" runat="server" ReadOnly="true" onkeydown="return (event.keyCode!=13);"
                                                TabIndex="5"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="tdstyle">
                                            ORDER DATE<b style="color: Red">*</b><br />
                                            <asp:TextBox CssClass="textb" ID="TxtOrderDate" runat="server" Width="100px" TabIndex="7"
                                                onchange="return checkDateOnTextBox();"></asp:TextBox>
                                            <asp:CalendarExtender ID="CalendarExtender1" runat="server" Format="dd-MMM-yyyy"
                                                TargetControlID="TxtOrderDate">
                                            </asp:CalendarExtender>
                                        </td>
                                        <td class="tdstyle">
                                            DELIVERY DATE<b style="color: Red">*</b><br />
                                            <asp:TextBox CssClass="textb" ID="TxtDeliveryDate" runat="server" Width="120px" TabIndex="8"
                                                onchange="return checkDateOnTextBox();"></asp:TextBox>
                                            <asp:CalendarExtender ID="CalendarExtender2" runat="server" Format="dd-MMM-yyyy"
                                                TargetControlID="TxtDeliveryDate">
                                            </asp:CalendarExtender>
                                        </td>
                                        <td class="tdstyle">
                                            DUE DATE<b style="color: Red">*</b><br />
                                            <asp:TextBox CssClass="textb" ID="txtduedate" runat="server" Width="120px" TabIndex="8"
                                                onchange="return checkDateOnTextBox();"></asp:TextBox>
                                            <asp:CalendarExtender ID="CalendarExtender4" runat="server" Format="dd-MMM-yyyy"
                                                TargetControlID="txtduedate">
                                            </asp:CalendarExtender>
                                        </td>
                                        <td class="tdstyle">
                                            Rate Type<b style="color: Red">*</b><br />
                                            <asp:DropDownList ID="ddDeliveryTerms" runat="server" Width="150px" TabIndex="9"
                                                CssClass="dropdown">
                                            </asp:DropDownList>
                                        </td>
                                        <td class="tdstyle">
                                            Payment Term<b style="color: Red">*</b><br />
                                            <asp:DropDownList ID="ddPaymentMode" runat="server" Width="150px" TabIndex="10" CssClass="dropdown">
                                            </asp:DropDownList>
                                        </td>
                                        <td class="tdstyle">
                                            By Air/Sea<b style="color: Red">*</b><br />
                                            <asp:DropDownList ID="ddlByAirSea" runat="server" CssClass="dropdown" Width="150px"
                                                TabIndex="11">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                </table>
                                <table>
                                    <tr>
                                        <td class="tdstyle">
                                            Port Of Loading<b style="color: Red">*</b><br />
                                            <asp:DropDownList ID="ddlPortOfLoading" runat="server" CssClass="dropdown" Width="150px"
                                                TabIndex="12">
                                            </asp:DropDownList>
                                        </td>
                                        <td class="tdstyle">
                                            Sea Port<br />
                                            <asp:TextBox ID="txtSeaPort" runat="server" CssClass="textb" TabIndex="13" onkeydown="return (event.keyCode!=13);"></asp:TextBox>
                                        </td>
                                        <td class="tdstyle" id="tdDeliveryComments" runat="server">
                                            Del Comments<br />
                                            <asp:TextBox ID="TxtDeliveryComments" runat="server" Width="235px" CssClass="textb"
                                                TabIndex="14" onkeydown="return (event.keyCode!=13);"></asp:TextBox>
                                        </td>
                                        <td class="tdstyle">
                                            Proforma Invoice Date<br />
                                            <asp:TextBox CssClass="textb" ID="Txtcustorderdt" runat="server" Width="100px" TabIndex="14"></asp:TextBox>
                                            <asp:CalendarExtender ID="CalendarExtender3" runat="server" Format="dd-MMM-yyyy"
                                                TargetControlID="Txtcustorderdt">
                                            </asp:CalendarExtender>
                                            <%-- <asp:TextBox ID="txtContainerPackingCost" runat="server" Width="10px" Height="0px"></asp:TextBox>--%>
                                        </td>
                                        <td class="tdstyle" id="procode" runat="server">
                                            <asp:Label ID="lblprodcode" Text="Product Code" runat="server"></asp:Label><br />
                                            <asp:TextBox CssClass="textb" ID="TxtProdCode" Width="150px" runat="server" AutoPostBack="True"
                                                OnTextChanged="TxtProdCode_TextChanged" TabIndex="15"></asp:TextBox>
                                            <cc1:AutoCompleteExtender ID="AutoCompleteExtender1" runat="server" EnableCaching="true"
                                                Enabled="True" MinimumPrefixLength="1" ServiceMethod="GetQuality" TargetControlID="TxtProdCode"
                                                UseContextKey="True">
                                            </cc1:AutoCompleteExtender>
                                        </td>
                                        <td class="tdstyle">
                                            Ex-Factory Date<br />
                                            <asp:TextBox CssClass="textb" ID="txtexfactorydate" runat="server" Width="100px"
                                                TabIndex="14"></asp:TextBox>
                                            <asp:CalendarExtender ID="CalendarExtender5" runat="server" Format="dd-MMM-yyyy"
                                                TargetControlID="txtexfactorydate">
                                            </asp:CalendarExtender>
                                            <%-- <asp:TextBox ID="txtContainerPackingCost" runat="server" Width="10px" Height="0px"></asp:TextBox>--%>
                                        </td>
                                        <td class="tdstyle">
                                            Reverse Charges Applicable<br />
                                            <asp:DropDownList ID="ddlReverseChargesApplicable" runat="server" CssClass="dropdown"
                                                Width="150px" TabIndex="11">
                                                <asp:ListItem Value="No" Selected="True">No</asp:ListItem>
                                                <asp:ListItem Value="Yes">Yes</asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                </table>
                                <table>
                                    <tr id="TRStockNo" runat="server" visible="false">
                                        <td class="tdstyle">
                                            <asp:Label ID="Label1" runat="server" Text="StockNo"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtStockNo" runat="server" CssClass="textb" Width="100px" onkeydown="return (event.keyCode!=13);"
                                                AutoPostBack="true" OnTextChanged="txtStockNo_TextChanged"></asp:TextBox>
                                            <%-- <asp:TextBox CssClass="textb" ID="TextBox2" runat="server" Width="100px" onkeydown="return (event.keyCode!=13);"
                                                AutoPostBack="True" OnTextChanged="txtWidth_TextChanged"></asp:TextBox>--%>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="tdstyle">
                                            <asp:Label ID="lblcategoryname" runat="server" Text="Category"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="DDItemCategory" AutoPostBack="true" runat="server" Width="150px"
                                                OnSelectedIndexChanged="DDItemCategory_SelectedIndexChanged" TabIndex="16" CssClass="dropdown"
                                                ValidationGroup="f1">
                                            </asp:DropDownList>
                                            <asp:Button ID="btnadditemcategory" runat="server" align="center" CssClass="buttonsmalls"
                                                OnClientClick="return AddItemCategory()" Text="ADD" TabIndex="17" />
                                            <asp:Button CssClass="refreshcategory" ID="refreshcategory" runat="server" Text=""
                                                Style="display: none" OnClick="refreshcategory_Click" />
                                        </td>
                                        <td class="tdstyle">
                                            <asp:Label ID="lblitemname" runat="server" Text="ItemName"></asp:Label>
                                            <asp:Button CssClass="buttonnorm" ID="BtnRefreshItem" runat="server" Text="" Style="display: none"
                                                OnClick="BtnRefreshItem_Click" />
                                        </td>
                                        <td>
                                            <asp:DropDownList CssClass="dropdown" ID="DDItemName" runat="server" TabIndex="18"
                                                AutoPostBack="True" Width="90%" OnSelectedIndexChanged="DDItemName_SelectedIndexChanged"
                                                ValidationGroup="f1">
                                            </asp:DropDownList>
                                            <b style="color: Red">*</b>
                                        </td>
                                        <td align="left">
                                            <asp:Button ID="BtnAdd0" runat="server" align="left" CssClass="buttonsmalls" OnClientClick="return AddItum()"
                                                Text="ADD" TabIndex="19" />
                                        </td>
                                    </tr>
                                    <tr id="DivQuality" runat="server" visible="false">
                                        <td class="tdstyle">
                                            <asp:Label ID="lblqualityname" runat="server" Text="Quality"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:DropDownList CssClass="dropdown" ID="DDQuality" runat="server" AutoPostBack="True"
                                                Width="150px" OnSelectedIndexChanged="DDQuality_SelectedIndexChanged" TabIndex="20">
                                            </asp:DropDownList>
                                            <%--<b style="color: Red">*</b>--%>
                                            <asp:Button ID="btnaddquality" runat="server" align="center" CssClass="buttonsmalls"
                                                OnClientClick="return AddQuality()" Text="ADD" TabIndex="21" />
                                            <asp:Button CssClass="refreshquality" ID="refreshquality" runat="server" Text=""
                                                BorderWidth="0px" Height="1px" Width="1px" BackColor="White" BorderColor="White"
                                                BorderStyle="None" ForeColor="White" OnClick="refreshquality_Click" />
                                            <asp:Button CssClass="buttonnorm" ID="fillitemcode" runat="server" Text="" BorderWidth="0px"
                                                Height="1px" Width="1px" BackColor="White" BorderColor="White" BorderStyle="None"
                                                ForeColor="White" OnClick="fillitemcode_Click" />
                                        </td>
                                        <td colspan="3" id="ItemDescription" runat="server" visible="false" align="left">
                                            ITEM DESCRIPTION
                                            <asp:DropDownList CssClass="dropdown" ID="DDItemCode" runat="server" AutoPostBack="True"
                                                Width="50.5%" Height="16px" TabIndex="22">
                                            </asp:DropDownList>
                                            &nbsp;<asp:Button CssClass="buttonsmalls" ID="BtnQualityCode" runat="server" Text="ADD"
                                                OnClientClick="return AddItemCode()" Height="22px" TabIndex="23" />
                                        </td>
                                        <td align="center">
                                            <asp:Button ID="BtnAddBuyerMasterCode" runat="server" align="right" CssClass="buttonnorm"
                                                OnClientClick="return AddBuyerMasterCode()" Width="150px" Text="ADD NEW ITEMS" />
                                            <asp:Button ID="RefereshBuyerMasterCode" runat="server" Text="" BorderWidth="0px"
                                                Height="1px" Width="1px" BackColor="White" BorderColor="White" BorderStyle="None"
                                                ForeColor="White" OnClick="RefereshBuyerMasterCode_Click" />
                                        </td>
                                    </tr>
                                    <tr id="trDesign" runat="server" visible="false">
                                        <td class="tdstyle">
                                            <asp:Label ID="lbldesignname" runat="server" Text="Design"></asp:Label>&nbsp;
                                        </td>
                                        <td>
                                            <asp:DropDownList CssClass="dropdown" ID="DDDesign" runat="server" AutoPostBack="True"
                                                TabIndex="24" Width="150px" OnSelectedIndexChanged="DDDesign_SelectedIndexChanged">
                                            </asp:DropDownList>
                                            <asp:Button ID="btnadddesign" runat="server" align="center" CssClass="buttonsmalls"
                                                OnClientClick="return AddDesign()" Text="ADD" TabIndex="25" />
                                            <asp:Button CssClass="refreshdesign" ID="refreshdesign" runat="server" Text="" Style="display: none"
                                                OnClick="refreshdesign_Click" />
                                        </td>
                                    </tr>
                                    <tr id="trColor" runat="server" visible="false">
                                        <td class="tdstyle">
                                            <asp:Label ID="lblcolorname" runat="server" Text="Color"></asp:Label>&nbsp;
                                        </td>
                                        <td>
                                            <asp:DropDownList CssClass="dropdown" ID="DDColor" runat="server" AutoPostBack="True"
                                                TabIndex="26" Width="150px" OnSelectedIndexChanged="DDColor_SelectedIndexChanged">
                                            </asp:DropDownList>
                                            <asp:Button ID="btnaddcolor" runat="server" align="center" CssClass="buttonsmalls"
                                                OnClientClick="return AddColor()" Text="ADD" TabIndex="27" />
                                            <asp:Button CssClass="refreshcolor" ID="refreshcolor" runat="server" Text="" Style="display: none"
                                                OnClick="refreshcolor_Click" />
                                        </td>
                                    </tr>
                                    <tr id="trShadeColor" runat="server" visible="false">
                                        <td class="tdstyle">
                                            <asp:Label ID="LblShadeColor" runat="server" Text="SHADE COLOR"></asp:Label>&nbsp;
                                        </td>
                                        <td>
                                            <asp:DropDownList CssClass="dropdown" ID="ddshadecolor" runat="server" AutoPostBack="True"
                                                Width="150px" TabIndex="28">
                                            </asp:DropDownList>
                                            <asp:Button ID="btnaddshadecolor" runat="server" align="center" CssClass="buttonsmalls"
                                                OnClientClick="return AddShadecolor()" Text="ADD" TabIndex="29" />
                                            <asp:Button CssClass="buttonnorm" ID="refreshshade" runat="server" Text="" Style="display: none"
                                                OnClick="refreshshade_Click" />
                                        </td>
                                    </tr>
                                    <tr id="trShape" runat="server" visible="false">
                                        <td class="tdstyle">
                                            <asp:Label ID="lblshapename" runat="server" Text="Shape"></asp:Label>&nbsp;
                                        </td>
                                        <td>
                                            <asp:DropDownList CssClass="dropdown" ID="DDShape" runat="server" AutoPostBack="True"
                                                OnSelectedIndexChanged="DDShape_SelectedIndexChanged" Width="150px" TabIndex="30">
                                            </asp:DropDownList>
                                            <asp:Button ID="btnaddshape" runat="server" align="center" CssClass="buttonsmalls"
                                                OnClientClick="return AddShape()" Text="ADD" TabIndex="31" />
                                            <asp:Button CssClass="buttonnorm" ID="refreshshape" runat="server" Text="" BorderWidth="0px"
                                                Height="1px" Width="1px" BackColor="White" BorderColor="White" BorderStyle="None"
                                                ForeColor="White" OnClick="refreshshape_Click" />
                                        </td>
                                    </tr>
                                    <tr id="trSize" runat="server" visible="false">
                                        <td class="tdstyle">
                                            <asp:Label ID="lblsizename" runat="server" Text="Size"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:DropDownList CssClass="dropdown" Width="50" ID="DDsizetype" runat="server" AutoPostBack="True"
                                                OnSelectedIndexChanged="DDsizetype_SelectedIndexChanged">
                                            </asp:DropDownList>
                                            <br />
                                            <asp:DropDownList CssClass="dropdown" ID="DDSize" runat="server" AutoPostBack="True"
                                                OnSelectedIndexChanged="DDSize_SelectedIndexChanged" Width="150px" TabIndex="32">
                                            </asp:DropDownList>
                                            <asp:Button CssClass="buttonsmalls" ID="btnaddsize" runat="server" Text="ADD" OnClientClick="return AddSize()"
                                                Height="22px" TabIndex="33" />
                                            <asp:Button CssClass="buttonnorm" ID="refreshsize" runat="server" Text="" Style="display: none"
                                                OnClick="refreshsize_Click" />
                                        </td>
                                        <%--ID="BtnRefreshSize"--%>
                                    </tr>
                                </table>
                                <table>
                                    <tr id="TrtxtArea" runat="server" visible="false">
                                        <td class="tdstyle">
                                            Width
                                        </td>
                                        <td>
                                            <asp:TextBox CssClass="textb" ID="txtWidth" runat="server" Width="100px" onkeydown="return (event.keyCode!=13);"
                                                AutoPostBack="True" OnTextChanged="txtWidth_TextChanged"></asp:TextBox>
                                        </td>
                                        <td class="tdstyle">
                                            Length
                                        </td>
                                        <td>
                                            <asp:TextBox CssClass="textb" ID="txtLength" runat="server" Width="100px" onkeydown="return (event.keyCode!=13);"
                                                AutoPostBack="True" OnTextChanged="txtLength_TextChanged"></asp:TextBox>
                                        </td>
                                        <td class="tdstyle">
                                            Area
                                        </td>
                                        <td>
                                            <asp:TextBox CssClass="textb" ID="TxtArea" runat="server" Width="100px" onkeydown="return (event.keyCode!=13);"></asp:TextBox>
                                            <%--<asp:TextBox CssClass="textb" ID="TxtArea2" runat="server" Width="150px" onkeydown="return (event.keyCode!=13);"></asp:TextBox>--%>
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                    </tr>
                                </table>
                                <table>
                                    <tr>
                                        <td class="tdstyle">
                                            <asp:TextBox ID="TxtFinishedid" Style="display: none" runat="server"></asp:TextBox>
                                            Qty
                                        </td>
                                        <td>
                                            <asp:TextBox CssClass="textb" ID="TxtQuantity" runat="server" Width="80px" AutoPostBack="True"
                                                onkeypress="return isNumberKey1(event);" OnTextChanged="TxtQuantity_TextChanged"
                                                TabIndex="35"></asp:TextBox><b style="color: Red">*</b>
                                        </td>
                                        <td class="tdstyle">
                                            Rate
                                        </td>
                                        <td>
                                            <asp:TextBox CssClass="textb" ID="TxtPrice" runat="server" Width="80px" align="left"
                                                onkeypress="return isNumberDecimal(event);" onkeydown="return (event.keyCode!=13);"
                                                TabIndex="36"></asp:TextBox><b style="color: Red">*</b>Currency
                                            <asp:DropDownList CssClass="dropdown" ID="DDCurrency" Width="120px" align="right"
                                                runat="server" TabIndex="37" AutoPostBack="true" OnSelectedIndexChanged="DDCurrency_SelectedIndexChanged">
                                            </asp:DropDownList>
                                            <b style="color: Red">*</b>
                                        </td>
                                        <td class="tdstyle">
                                            Weight
                                        </td>
                                        <td>
                                            <asp:TextBox CssClass="textb" ID="TxtWeight" runat="server" Width="80px" TabIndex="38"
                                                onkeypress="return isNumberDecimal(event);" onkeydown="return (event.keyCode!=13);"></asp:TextBox>
                                        </td>
                                        <td class="tdstyle">
                                            GST
                                        </td>
                                        <td>
                                            <asp:TextBox CssClass="textb" ID="TxtGST" runat="server" Width="80px" TabIndex="39"
                                                onkeypress="return isNumberDecimal(event);" onkeydown="return (event.keyCode!=13);"></asp:TextBox>
                                        </td>
                                        <td class="tdstyle">
                                            IGST
                                        </td>
                                        <td>
                                            <asp:TextBox CssClass="textb" ID="TxtIGST" runat="server" Width="80px" TabIndex="40"
                                                onkeypress="return isNumberDecimal(event);" onkeydown="return (event.keyCode!=13);"></asp:TextBox>
                                        </td>
                                    </tr>
                                </table>
                                <div>
                                    <div style="width: 890px; height: auto">
                                        <div style="width: 930px; background-color: #8B7B8B;">
                                            <table>
                                                <tr>
                                                    <td>
                                                        <asp:CheckBox ID="chkotherinformation" runat="server" Text=" OTHER INFORMATION "
                                                            Font-Bold="true" ForeColor="White" AutoPostBack="true" CssClass="labelbold" OnCheckedChanged="chkotherinformation_CheckedChanged" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </div>
                                        <div style="width: 690px;" runat="server" visible="false" id="DivOtherInformation">
                                            <table>
                                                <tr>
                                                    <td style="width: 5%">
                                                        Location Type
                                                    </td>
                                                    <td style="width: 10%;">
                                                        <asp:TextBox CssClass="textb" ID="txtLocationType" runat="server" Width="200px" TabIndex="41"></asp:TextBox>
                                                    </td>
                                                    <td style="width: 5%">
                                                        Material
                                                    </td>
                                                    <td style="width: 10%">
                                                        <asp:TextBox ID="txtMaterial" runat="server" Width="100px" TabIndex="42" CssClass="textb"></asp:TextBox>
                                                    </td>
                                                    <td style="width: 5%">
                                                        Texture
                                                    </td>
                                                    <td style="width: 10%">
                                                        <asp:TextBox ID="txtTexture" runat="server" Width="200px" TabIndex="43" CssClass="textb"></asp:TextBox>
                                                    </td>
                                                    <td style="width: 5%">
                                                        ReportRef
                                                    </td>
                                                    <td style="width: 10%">
                                                        <asp:TextBox ID="txtReportRef" runat="server" Width="200px" TabIndex="44" CssClass="textb"
                                                            TextMode="MultiLine"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        &nbsp;
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        Material Description
                                                    </td>
                                                    <td>
                                                        <asp:TextBox CssClass="textb" ID="txtMaterialFormDescription" runat="server" Width="200px"
                                                            TextMode="MultiLine" TabIndex="45"></asp:TextBox>
                                                    </td>
                                                    <td colspan="0">
                                                        Material Rate
                                                    </td>
                                                    <td colspan="0">
                                                        <asp:TextBox CssClass="textb" ID="txtMaterialRate" runat="server" Width="80px" TabIndex="46"
                                                            onkeypress="return isNumberDecimal(event);" onkeydown="return (event.keyCode!=13);"></asp:TextBox>
                                                    </td>
                                                    <td colspan="0">
                                                        Job Work
                                                    </td>
                                                    <td colspan="0">
                                                        <asp:TextBox ID="txtJobWork" runat="server" Width="200px" TabIndex="47" TextMode="MultiLine"
                                                            CssClass="textb"></asp:TextBox>
                                                    </td>
                                                    <td colspan="0">
                                                        Job Rate
                                                    </td>
                                                    <td colspan="0">
                                                        <asp:TextBox CssClass="textb" ID="txtJobRate" runat="server" Width="80px" TabIndex="48"
                                                            onkeypress="return isNumberDecimal(event);" onkeydown="return (event.keyCode!=13);"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        Description
                                                    </td>
                                                    <td colspan="0">
                                                        <asp:TextBox CssClass="textb" ID="TxtDescription" runat="server" Width="200px" TabIndex="45"
                                                            TextMode="MultiLine" onkeydown="return (event.keyCode!=13);"></asp:TextBox>
                                                    </td>
                                                    <td colspan="0">
                                                        Remarks
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="TXTRemarks" runat="server" Width="200px" TabIndex="46" CssClass="textb"
                                                            TextMode="MultiLine" onkeydown="return (event.keyCode!=13);"></asp:TextBox>
                                                    </td>
                                                    <td colspan="1">
                                                        Material/JobWork GST
                                                    </td>
                                                    <td colspan="2">
                                                        <asp:TextBox CssClass="textb" ID="txtMaterialJobWorkGST" runat="server" Width="80px"
                                                            TabIndex="46" onkeypress="return isNumberDecimal(event);" onkeydown="return (event.keyCode!=13);"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td id="tdUPCNO" runat="server" class="tdstyle">
                                                        UPC NO
                                                        <asp:TextBox ID="TxtUPCNO" runat="server" TabIndex="47" CssClass="textb" onkeydown="return (event.keyCode!=13);"></asp:TextBox>
                                                    </td>
                                                    <td id="tdOurCode" runat="server" class="tdstyle">
                                                        <asp:Label ID="lblourcode" runat="server" Text="Our Code"></asp:Label>
                                                        <asp:TextBox ID="TxtOurCode" runat="server" TabIndex="48" CssClass="textb" onkeydown="return (event.keyCode!=13);"></asp:TextBox>
                                                        <asp:TextBox ID="TxtCRBCode" runat="server" Visible="false" TabIndex="48" CssClass="textb"
                                                            onkeydown="return (event.keyCode!=13);"></asp:TextBox>
                                                    </td>
                                                    <td id="tdBuyerCode" runat="server" class="tdstyle">
                                                        Buyer Code
                                                        <asp:TextBox ID="TxtBuyerCode" runat="server" TabIndex="49" CssClass="textb" onkeydown="return (event.keyCode!=13);"></asp:TextBox>
                                                    </td>
                                                    <td id="tdHTS" runat="server" class="tdstyle" visible="true">
                                                        HTS Code
                                                        <asp:TextBox ID="TxtHtsCode" runat="server" TabIndex="50" CssClass="textb" onkeydown="return (event.keyCode!=13);"></asp:TextBox>
                                                    </td>
                                                    <td id="tdperformainvoiceno" runat="server" class="tdstyle">
                                                        Proforma Invoice no
                                                        <asp:TextBox ID="txtperformainvoiceno" runat="server" CssClass="textb" TabIndex="51"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lblReportKindAtten" runat="server" Text="Kind Atten"></asp:Label>
                                                    </td>
                                                    <td id="tdKindAtten" runat="server" class="tdstyle">
                                                        <asp:TextBox ID="txtReportKindAtten" runat="server" CssClass="textb" TextMode="MultiLine"
                                                            Width="200px" TabIndex="51"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        Material/JobWork IGST
                                                    </td>
                                                    <td>
                                                        <asp:TextBox CssClass="textb" ID="txtMaterialJobWorkIGST" runat="server" Width="80px"
                                                            TabIndex="46" onkeypress="return isNumberDecimal(event);" onkeydown="return (event.keyCode!=13);"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="tdstyle">
                                                        <asp:Label ID="LblPKGInstruction" runat="server" Text="PKG Instruction"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="TxtPKGInstruction" runat="server" Width="200px" TabIndex="52" CssClass="textb"
                                                            TextMode="MultiLine" onkeydown="return (event.keyCode!=13);"></asp:TextBox>
                                                        <%--  <asp:HtmlEditorExtender ID="HtmlEditorExtender1" runat="server" TargetControlID="TxtPKGInstruction" EnableSanitization="false">
                                            </asp:HtmlEditorExtender>--%>
                                                    </td>
                                                    <td class="tdstyle">
                                                        <asp:Label ID="lbllesadv" runat="server" Text="Less Advance"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtlessadv" runat="server" Width="200px" TabIndex="53" CssClass="textb"
                                                            TextMode="MultiLine" onkeypress="return isNumberDecimal(event);" onkeydown="return (event.keyCode!=13);"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lblshipto" runat="server" Text="Ship To"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtShipto" runat="server" Width="200px" TextMode="MultiLine" TabIndex="54"
                                                            CssClass="textb" onkeydown="return (event.keyCode!=13);"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        &nbsp;
                                                    </td>
                                                    <td>
                                                        &nbsp;
                                                    </td>
                                                    <td>
                                                        &nbsp;
                                                    </td>
                                                </tr>
                                                <tr runat="server" id="trless">
                                                    <td class="tdstyle">
                                                        <asp:Label ID="LblLBGInstruction" runat="server" Text="LBG Instruction"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="TxtLBGInstruction" runat="server" Width="200px" TabIndex="55" CssClass="textb"
                                                            onkeydown="return (event.keyCode!=13);"></asp:TextBox>
                                                    </td>
                                                    <td class="tdstyle">
                                                        <asp:Label ID="lbllescomm" runat="server" Text="Less Commission"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtcomm" runat="server" Width="200px" TabIndex="56" CssClass="textb"
                                                            onkeypress="return isNumberDecimal(event);" onkeydown="return (event.keyCode!=13);"></asp:TextBox>
                                                    </td>
                                                    <td class="tdstyle">
                                                        <asp:Label ID="lbldiss" runat="server" Text="Less Discount"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="Txtdiscount" runat="server" Width="200px" TabIndex="57" CssClass="textb"
                                                            onkeypress="return isNumberDecimal(event);" onkeydown="return (event.keyCode!=13);"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lblPackingForwardingCharges" runat="server" Text="PackingForwardingCharges"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtPackingForwardingCharges" runat="server" Width="200px" TabIndex="58"
                                                            CssClass="textb" onkeypress="return isNumberDecimal(event);" onkeydown="return (event.keyCode!=13);"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        &nbsp;
                                                    </td>
                                                </tr>
                                            </table>
                                        </div>
                                    </div>
                                </div>
                                <table>
                                    <tr runat="server" id="TRPrice" visible="false">
                                        <td>
                                            Gross Weight:
                                        </td>
                                        <td>
                                            <asp:TextBox ID="TxtGrsweight" CssClass="textb" onkeypress="return isNumberDecimal(event);"
                                                runat="server"></asp:TextBox>
                                        </td>
                                        <%--<td>Price:</td>
                                    <td><asp:TextBox ID="TxtNetPrice" CssClass="textb" runat="server"  onkeypress="return isNumberDecimal(event);"></asp:TextBox> </td>--%>
                                    </tr>
                                </table>
                            </table>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                    <table>
                        <tr>
                            <td class="tdstyle">
                                Photo
                            </td>
                            <td colspan="2">
                                <div id="newPreview" runat="server">
                                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                        <ContentTemplate>
                                            <asp:Image ID="newPreview1" runat="server" Height="66px" Width="111px" />
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </div>
                            </td>
                            <td colspan="2" class="tdstyle">
                                <asp:FileUpload ID="PhotoImage" onchange="PreviewImg(this)" ViewStateMode="Enabled"
                                    runat="server" TabIndex="49" />
                                <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ErrorMessage="Only .gif,.jpeg or.jpg files are allowed!"
                                    ValidationExpression="^.*\.(jpg|JPG|gif|GIF|jpeg|JPEG|BMP|bmp)$" ControlToValidate="PhotoImage"></asp:RegularExpressionValidator>
                            </td>
                            <td>
                                <asp:Button ID="btnContainerpackingMatCost" OnClientClick="return CalculateContainer();"
                                    runat="server" Text="Packing Cost" Visible="false" OnClick="btnContainerpackingMatCost_Click"
                                    CssClass="buttonnorm" />
                            </td>
                        </tr>
                        <tr id="trReferenceImage" runat="server">
                            <td>
                                Ref Image
                            </td>
                            <td colspan="2">
                                <div id="DivReferenceImage" runat="server">
                                    <asp:Image ID="ImageReferenceImage" runat="server" Height="66px" Width="111px" />
                                </div>
                            </td>
                            <td colspan="2" class="tdstyle">
                                <asp:FileUpload ID="FileReferenceImage" onchange="PreviewReferenceImage(this)" runat="server"
                                    TabIndex="50" />
                                <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" ErrorMessage="Only .gif,.jpeg or.jpg files are allowed!"
                                    ValidationExpression="^.*\.(jpg|JPG|gif|GIF|jpeg|JPEG|BMP|bmp)$" ControlToValidate="FileReferenceImage"></asp:RegularExpressionValidator>
                            </td>
                            <td>
                                <asp:Button ID="BtnReferenceImageSave" runat="server" Text="RefImageSave" Visible="false"
                                    OnClick="BtnReferenceImageSave_Click" OnClientClick="return confirm('Do you want to save data?')"
                                    CssClass="buttonnorm" />
                            </td>
                        </tr>
                    </table>
                    <table>
                        <tr>
                            <td colspan="3">
                                <asp:Label ID="LblErrorMessage" runat="server" Font-Bold="true" ForeColor="RED" Text="Some IMportent Fields are Missing......."
                                    Visible="false"></asp:Label>
                            </td>
                            <td colspan="3">
                                <asp:Button ID="BtnShowConsumption" CssClass="buttonnorm" runat="server" Text="ORDER CONSUMPTION"
                                    OnClientClick="return AddLocalConsumption()" TabIndex="47" Visible="False" />
                                <asp:Button CssClass="buttonnorm" ID="BTNEXCEL" runat="server" Text="EXCEL-REP" OnClick="BTNEXCEL_Click" />
                                <asp:Button CssClass="buttonnorm" ID="BtnNew" runat="server" Text="Refresh" OnClick="BtnNew_Click"
                                    OnClientClick="return RefreshForm(0);" />
                                <asp:Button CssClass="buttonnorm" ID="Btn" runat="server" Text="New" OnClientClick="return RefreshForm(1);"
                                    OnClick="Btn_Click" />
                                <asp:Button CssClass="buttonnorm" ID="BtnSave" Text="Save" runat="server" OnClick="BtnSave_Click"
                                    OnClientClick="return checkDate();" ValidationGroup="f1" TabIndex="52" />
                                <asp:Button CssClass="buttonnorm" ID="BtnClose" runat="server" Text="Close" OnClick="BtnClose_Click" />
                                <asp:Button CssClass="buttonnorm preview_width" ID="BtnReport" runat="server" Text="Preview"
                                    OnClick="BtnReport_Click" />&nbsp;&nbsp;
                                <asp:DropDownList ID="DDPreviewType" runat="server" CssClass="dropdown">
                                    <asp:ListItem Text="PerForma Invoice HSCODE" Value="0"></asp:ListItem>
                                    <asp:ListItem Text="PerForma Invoice BUYERCODE" Value="1"></asp:ListItem>
                                    <asp:ListItem Text="Photo Quotation" Value="2"></asp:ListItem>
                                    <asp:ListItem Text="Sample Order" Value="3"></asp:ListItem>
                                    <asp:ListItem Text="PerForma Invoice" Value="4"></asp:ListItem>
                                    <asp:ListItem Text="PerForma Invoice2" Value="5"></asp:ListItem>
                                    <asp:ListItem Text="Buyer Order" Value="6"></asp:ListItem>
                                    <asp:ListItem Text="PerForma Invoice3" Value="7"></asp:ListItem>
                                    <asp:ListItem Text="Photo Quotation2" Value="8"></asp:ListItem>
                                    <asp:ListItem Text="Area Rug Retail" Value="9"></asp:ListItem>
                                    <asp:ListItem Text="Customization Quotation" Value="10"></asp:ListItem>
                                    <asp:ListItem Text="Underlay Supply Quotation" Value="11"></asp:ListItem>
                                    <asp:ListItem Text="Installation Wall Quotation" Value="12"></asp:ListItem>
                                    <asp:ListItem Text="Proforma Invoice New" Value="13"></asp:ListItem>
                                </asp:DropDownList>
                            </td>
                        </tr>
                    </table>
                    <table>
                        <tr>
                            <td>
                                <asp:TextBox ID="txtgreen" BackColor="Green" Width="15px" CssClass="textb" runat="server"
                                    Enabled="false"></asp:TextBox>
                                <asp:TextBox ID="Note" runat="server" BorderStyle="None" Width="180px" Text="Consumption Defined"
                                    Enabled="false" CssClass="textb" Font-Bold="true"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="4">
                                <div style="width: 1000px; height: 200px; overflow: auto">
                                    <asp:GridView ID="DGOrderDetail" Width="100%" runat="server" DataKeyNames="Sr_No"
                                        AutoGenerateColumns="False" OnRowCommand="DGOrderDetail_RowCommand" OnRowDataBound="DGOrderDetail_RowDataBound"
                                        OnSelectedIndexChanged="DGOrderDetail_SelectedIndexChanged" OnRowDeleting="DGOrderDetail_RowDeleting"
                                        OnRowUpdating="DGOrderDetail_RowUpdating" EmptyDataText="No records found...">
                                        <AlternatingRowStyle CssClass="gvalts" />
                                        <HeaderStyle CssClass="gvheaders" />
                                        <RowStyle CssClass="gvrow" />
                                        <Columns>
                                            <asp:BoundField DataField="Sr_No" HeaderText="Sr_No" />
                                            <asp:BoundField DataField="CATEGORY" HeaderText="CATEGORY" />
                                            <asp:BoundField DataField="ITEMNAME" HeaderText="ITEMNAME" />
                                            <asp:BoundField DataField="DESCRIPTION" HeaderText="DESCRIPTION" />
                                            <asp:BoundField DataField="QTY" HeaderText="QTY" />
                                            <asp:BoundField DataField="RATE" HeaderText="RATE" />
                                            <asp:BoundField DataField="AREA" HeaderText="AREA" />
                                            <asp:BoundField DataField="AMOUNT" HeaderText="AMOUNT" />
                                            <asp:BoundField DataField="SUBQUALITY" HeaderText="SUBQUALITY" />
                                            <asp:TemplateField HeaderText="PHOTO">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblorderdetailid" runat="server" Visible="false" Text='<%#Bind("Sr_No") %>'></asp:Label>
                                                    <%--<asp:Image ID="Image1" Width="100px" Height="50px" runat="server" ImageUrl='<%# "~/ImageHandler.ashx?ID=" + Eval("Sr_No")+"&img=1"%>' />--%>
                                                    <asp:Image ID="Image" runat="server" ImageUrl='<%# Bind("photo") %>' Height="70px"
                                                        Width="100px" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Delete">
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="LinkButton1" runat="server" CommandName="Delete" Text="Delete"
                                                        CommandArgument="<%# ((GridViewRow) Container).RowIndex %>"></asp:LinkButton>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="CONS">
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="LinkButton2" runat="server" CausesValidation="False" CommandName="New"
                                                        Text="SHOW CONSUMP." CommandArgument="<%# ((GridViewRow) Container).RowIndex %>"></asp:LinkButton>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="PGK">
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="LinkButton3" runat="server" CausesValidation="False" CommandName="Update"
                                                        Text="PGK" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>"></asp:LinkButton>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="EXP">
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="BTNEXPENCE" runat="server" Text="EXP" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>"
                                                        CommandName="Exp"></asp:LinkButton>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="RMG">
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="BTNREFIMAGE" runat="server" Text="RMG" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>"
                                                        CommandName="RefImage" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Consumpflag" Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblconsumpflag" Text='<%#Bind("Consumpflag")%>' runat="server" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                        <EmptyDataRowStyle CssClass="gvemptytext" />
                                    </asp:GridView>
                                </div>
                            </td>
                            <td colspan="10">
                                <div style="width: 30%; height: 200px; overflow: scroll">
                                    <asp:GridView ID="REFIMAGEDG" Width="158%" runat="server" DataKeyNames="ID" AutoGenerateColumns="False"
                                        CssClass="grid-view">
                                        <AlternatingRowStyle BackColor="White" />
                                        <Columns>
                                            <asp:BoundField DataField="ID" HeaderText="ID" />
                                            <asp:TemplateField HeaderText="REFIMAGE">
                                                <ItemTemplate>
                                                    <asp:Image ID="Image1" Width="100px" Height="50px" runat="server" ImageUrl='<%# "~/ImageHandler.ashx?ID=" + Eval("ID")+"&img=3"%>' />
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
                            <td colspan="10">
                                <div style="width: 80%; height: 50%; overflow: auto">
                                    <asp:GridView ID="DGConsumption" runat="server" AutoGenerateColumns="False" DataKeyNames="PCMDID"
                                        EmptyDataText="No records found..." CssClass="grid-view">
                                        <HeaderStyle CssClass="gvheaders" />
                                        <AlternatingRowStyle CssClass="gvalts" />
                                        <RowStyle CssClass="gvrow" />
                                        <EmptyDataRowStyle CssClass="gvemptytext" />
                                        <Columns>
                                            <asp:BoundField DataField="PCMDID" HeaderText="SrNo" />
                                            <asp:BoundField DataField="PROCESSNAME" HeaderText="PROCESSNAME" />
                                            <asp:BoundField DataField="ICategory" HeaderText="ICategory">
                                                <ControlStyle Height="17px" />
                                                <HeaderStyle Width="200px" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="IItem" HeaderText="IItem" />
                                            <asp:BoundField DataField="IDescription" HeaderText="IDescription" />
                                            <asp:BoundField DataField="IUnitName" HeaderText="IUnitName" />
                                            <asp:TemplateField HeaderText="IQty">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="TXTIQTY" runat="server" Width="70px" Text='<%# Bind("IQty") %>'></asp:TextBox>
                                                </ItemTemplate>
                                                <HeaderStyle Width="80px" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="ILoss">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="TXTILOSS" runat="server" Width="70px" Text='<%# Bind("ILoss") %>'></asp:TextBox>
                                                </ItemTemplate>
                                                <HeaderStyle Width="80px" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="IRate">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="TXTIRate" runat="server" Width="70px" Text='<%# Bind("IRate") %>'></asp:TextBox>
                                                </ItemTemplate>
                                                <HeaderStyle Width="80px" />
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="OCategory" HeaderText="OCategory" />
                                            <asp:BoundField DataField="OItem" HeaderText="OItem" />
                                            <asp:BoundField DataField="ODescription" HeaderText="ODescription" />
                                            <asp:BoundField DataField="OUnitName" HeaderText="OUnitName" />
                                            <asp:TemplateField HeaderText="OQty">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="TXTOQTY" runat="server" Width="70px" Text='<%# Bind("OQty") %>'></asp:TextBox>
                                                </ItemTemplate>
                                                <HeaderStyle Width="80px" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="ORate">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="TXTORAte" runat="server" Width="70px" Text='<%# Bind("ORate") %>'></asp:TextBox>
                                                </ItemTemplate>
                                                <HeaderStyle Width="80px" />
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                    <asp:GridView ID="DGExpence" runat="server" AutoGenerateColumns="False" DataKeyNames="CWOEID"
                                        CssClass="grid-view">
                                        <HeaderStyle CssClass="gvheaders" />
                                        <AlternatingRowStyle CssClass="gvalts" />
                                        <RowStyle CssClass="gvrow" />
                                        <EmptyDataRowStyle CssClass="gvemptytext" />
                                        <Columns>
                                            <asp:BoundField DataField="CWOEID" HeaderText="CWOEID" />
                                            <asp:BoundField DataField="ChargeName" HeaderText="ChargeName" />
                                            <asp:TemplateField HeaderText="Percentage">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="TXTPercentage" runat="server" Width="70px" Text='<%# Bind("Percentage") %>'></asp:TextBox>
                                                </ItemTemplate>
                                                <HeaderStyle Width="80px" />
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                    <asp:GridView ID="DGPacking" runat="server" AutoGenerateColumns="False" DataKeyNames="PRMCID"
                                        CssClass="grid-view">
                                        <HeaderStyle CssClass="gvheader" />
                                        <AlternatingRowStyle CssClass="gvalt" />
                                        <RowStyle CssClass="gvrow" />
                                        <EmptyDataRowStyle CssClass="gvemptytext" />
                                        <Columns>
                                            <asp:BoundField DataField="PRMCID" HeaderText="PRMCID" />
                                            <asp:TemplateField HeaderText="INNERAMT">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="TXTINNERAMT" runat="server" Width="70px" Text='<%# Bind("INNERAMT") %>'></asp:TextBox>
                                                </ItemTemplate>
                                                <HeaderStyle Width="80px" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="MIDDLEAMT">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="TXTMIDDLEAMT" runat="server" Width="70px" Text='<%# Bind("MIDDLEAMT") %>'></asp:TextBox>
                                                </ItemTemplate>
                                                <HeaderStyle Width="80px" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="MASTERAMT">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="TXTMASTERAMT" runat="server" Width="70px" Text='<%# Bind("MASTERAMT") %>'></asp:TextBox>
                                                </ItemTemplate>
                                                <HeaderStyle Width="80px" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="OTHERAMT">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="TXTOTHERAMT" runat="server" Width="70px" Text='<%# Bind("OTHERAMT") %>'></asp:TextBox>
                                                </ItemTemplate>
                                                <HeaderStyle Width="80px" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="CONTAINERAMT">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="TXTCONTAINERAMT" runat="server" Width="70px" Text='<%# Bind("CONTAINERAMT") %>'></asp:TextBox>
                                                </ItemTemplate>
                                                <HeaderStyle Width="80px" />
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </div>
                                <br />
                                <asp:Button CssClass="buttonnorm" ID="BtnNewSave" Text="Save" runat="server" Visible="false"
                                    OnClientClick="return confirm('Do You Want To Modify Data?')" OnClick="BtnNewSave_Click" />
                            </td>
                            <%--<td>
                                <asp:Button ID="button1" Text="btn1" runat="server" Visible="false"/>
                                <asp:Panel DefaultButton="button2" runat="server" Visible="false">
                                    <asp:TextBox ID="textbox1" runat="server" Visible="false" />
                                    <asp:Button ID="button2" Text="btn2" runat="server" Visible="false" />
                                </asp:Panel>
                            </td>--%>
                            <%--<asp:Button ID="btndefault" runat="server" Visible="false" />--%>
                        </tr>
                        <tr id="trtot" runat="server">
                            <td colspan="2" class="tdstyle">
                                TOTAL AREA<asp:TextBox CssClass="textb" ID="TxtOrderArea" runat="server" onkeydown="return (event.keyCode!=13);"></asp:TextBox>
                            </td>
                            <td colspan="2" class="tdstyle">
                                TOTAL QTY<asp:TextBox CssClass="textb" ID="TxtTotalQtyRequired" runat="server" onkeydown="return (event.keyCode!=13);"></asp:TextBox>
                            </td>
                            <td colspan="2" class="tdstyle">
                                TOTAL AMT<asp:TextBox CssClass="textb" ID="TxtTotalAmount" runat="server" onkeydown="return (event.keyCode!=13);"></asp:TextBox>
                            </td>
                        </tr>
                    </table>
                </div>
                <div style="background-color: #edf3fe; text-align: center; height: 43px; width: 100%">
                    <span class="labelcopyr">&copy; Enablesoft Erp Consultancy Pvt. Ltd.All rights reserved.</span>
                </div>
            </td>
        </tr>
    </table>
    <asp:HiddenField ID="HfCompanyId" runat="server" />
    </form>
</body>
</html>
