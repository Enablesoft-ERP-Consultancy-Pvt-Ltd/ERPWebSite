<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CONFIRMDRAFTORDR.aspx.cs"
    ViewStateMode="Enabled" Inherits="Masters_Order_CONFIRMDRAFTORDR" EnableEventValidation="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register Src="~/UserControls/ucmenu.ascx" TagName="ucmenu" TagPrefix="uc2" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Confirm Draft Order</title>
    <link href="../../Styles/vijay.css" rel="stylesheet" type="text/css" />
    <script src="../../Scripts/JScript.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery-1.4.1.min.js" type="text/javascript"></script>
    <%--<script type="text/javascript" src="../../Scripts/Fixfocus.js"></script>--%>
    <script language="javascript" type="text/javascript">
        function CloseForm() {
            window.location.href = "../../main.aspx";
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
        function report() {
            window.open('../../ReportViewer.aspx', '');
        }
        function AddItemCategory() {
            window.open('../Carpet/AddItemCategory.aspx', '', 'Height=400px,width=500px');
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
            window.open('../Carpet/AddSize.aspx', '', 'Height=400px,width=1000px');
        }
        function CalculateContainer() {
            var varcode = document.getElementById('TxtOrderDetailId').value;
            window.open('../Carpet/ContainerCost.aspx?itemcode=' + varcode + '&PackingType=5', '', 'width=700px,Height=400px');
        }
        function checkDate(condition) {
            var datevalue1 = document.forms[0].TxtOrderDate.value;
            var datevalue2 = document.forms[0].TxtDeliveryDate.value;

            var day1 = datevalue1.substring(0, 2);
            var month1 = datevalue1.substring(3, 6);
            var year1 = datevalue1.substring(7, 11);

            var day2 = datevalue2.substring(0, 2);
            var month2 = datevalue2.substring(3, 6);
            var year2 = datevalue2.substring(7, 11);


            month1 = changeFormatStringtoNumber(month1);
            month2 = changeFormatStringtoNumber(month2);
            //            alert(month2);

            var d1 = new Date(year1, month1, day1);
            var d2 = new Date(year2, month2, day2);
            //            alert("-----");
            //            alert(d1);
            //            alert(d2);
            if (d1 > d2) {
                if (condition == 1) {
                    document.getElementById("TxtDeliveryDate").value = document.getElementById("TxtOrderDate").value;
                    return false;
                }
                else {
                    alert("Delivery date can not be less than Order date !");
                    document.getElementById("TxtDeliveryDate").value = document.getElementById("TxtOrderDate").value;
                    return false;
                }
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
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <table width="100%" border="1">
        <tr style="width: 100%" align="center">
            <td height="66px" align="center">
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
                    <div>
                        <asp:UpdatePanel runat="server">
                            <ContentTemplate>
                                <table>
                                    <tr>
                                        <td class="tdstyle">
                                            <asp:RadioButton ID="rdoUnitWise" Text="UNIT WISE" runat="server" GroupName="OrderType"
                                                CssClass="radiobuttonnormal" />
                                        </td>
                                        <td class="tdstyle">
                                            <asp:RadioButton ID="rdoPcWise" Text="PC WISE" runat="server" GroupName="OrderType"
                                                CssClass="radiobuttonnormal" />
                                        </td>
                                        <td class="tdstyle">
                                            <asp:Button ID="btnContainerPackingCost" runat="server" Style="display: none" EnableTheming="true" />
                                        </td>
                                        <td class="tdstyle">
                                            <asp:CheckBox ID="ChkEditOrder" runat="server" Text="EDIT ORDER" AutoPostBack="True"
                                                OnCheckedChanged="ChkEditOrder_CheckedChanged" CssClass="checkboxbold" />
                                        </td>
                                        <td>
                                            <asp:TextBox ID="TxtOrderDetailId" runat="server" Style="display: none"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="tdstyle">
                                            <asp:Label ID="labeljh" Text="COMPANY NAME" runat="server" CssClass="labelbold" />
                                            <b style="color: Red">*</b>
                                        </td>
                                        <td class="tdstyle">
                                            <asp:Label ID="label1" Text="CUST CODE" runat="server" CssClass="labelbold" />
                                            <b style="color: Red">*</b>
                                        </td>
                                        <td id="TDDraftOrderNo" runat="server" class="tdstyle">
                                            <asp:Label ID="label2" Text="DRAFT ORDER NO" runat="server" CssClass="labelbold" />
                                            <b style="color: Red">*</b>
                                        </td>
                                        <td class="tdstyle">
                                            <asp:Label ID="label3" Text="CUSTOMER ORDER NO" runat="server" CssClass="labelbold" />
                                        </td>
                                        <td class="tdstyle">
                                            <asp:Label ID="label4" Text=" OUR ORDER NO" runat="server" CssClass="labelbold" />
                                        </td>
                                        <td class="tdstyle">
                                            <asp:Label ID="label5" Text=" ORDER UNIT" runat="server" CssClass="labelbold" />
                                            <b style="color: Red">*</b>
                                        </td>
                                        <td class="tdstyle">
                                            <asp:Label ID="label6" Text=" ORDER DATE" runat="server" CssClass="labelbold" />
                                            <b style="color: Red">*</b>
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
                                        <td id="TDDDCustomerOrderNo" runat="server">
                                            <asp:DropDownList CssClass="dropdown" ID="DDCustOrderNo" runat="server" AutoPostBack="True"
                                                OnSelectedIndexChanged="DDCustOrderNo_SelectedIndexChanged" Width="150px">
                                            </asp:DropDownList>
                                        </td>
                                        <td>
                                            <asp:TextBox CssClass="textb" ID="TxtCustomerOrderNo" runat="server" onkeydown="return (event.keyCode!=13);"></asp:TextBox>
                                        </td>
                                        <td>
                                            <asp:TextBox CssClass="textb" ID="txtlocalorderno" runat="server" onkeydown="return (event.keyCode!=13);"></asp:TextBox>
                                        </td>
                                        <td>
                                            <asp:DropDownList CssClass="dropdown" ID="DDOrderUnit" runat="server" Width="150px">
                                            </asp:DropDownList>
                                        </td>
                                        <td>
                                            <asp:TextBox CssClass="textb" ID="TxtOrderDate" runat="server" Width="100px" onchange="javascript: checkDate(1);"></asp:TextBox>
                                            <asp:CalendarExtender ID="CalendarExtender1" runat="server" Format="dd-MMM-yyyy"
                                                TargetControlID="TxtOrderDate">
                                            </asp:CalendarExtender>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="tdstyle">
                                            <asp:Label ID="label7" Text="  DELIVERY DATE" runat="server" CssClass="labelbold" />
                                            <b style="color: Red">*</b>
                                        </td>
                                        <td class="tdstyle">
                                            <asp:Label ID="label8" Text=" Rate Type" runat="server" CssClass="labelbold" />
                                            <b style="color: Red">*</b>
                                        </td>
                                        <td class="tdstyle">
                                            <asp:Label ID="label9" Text=" Payment Term" runat="server" CssClass="labelbold" />
                                            <b style="color: Red">*</b>
                                        </td>
                                        <td class="tdstyle">
                                            <asp:Label ID="label15" Text="By Air/Sea" runat="server" CssClass="labelbold" />
                                            <b style="color: Red">*</b>
                                        </td>
                                        <td class="tdstyle">
                                            <asp:Label ID="label10" Text="  Port Of Loading" runat="server" CssClass="labelbold" />
                                            <b style="color: Red">*</b>
                                        </td>
                                        <td class="tdstyle">
                                            <asp:Label ID="label11" Text="    Sea Port" runat="server" CssClass="labelbold" />
                                            <b style="color: Red">*</b>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:TextBox CssClass="textb" ID="TxtDeliveryDate" runat="server" Width="120px" onchange="javascript: checkDate(0);"></asp:TextBox>
                                            <asp:CalendarExtender ID="CalendarExtender2" runat="server" Format="dd-MMM-yyyy"
                                                TargetControlID="TxtDeliveryDate">
                                            </asp:CalendarExtender>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddDeliveryTerms" runat="server" Width="150px" CssClass="dropdown">
                                            </asp:DropDownList>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddPaymentMode" runat="server" Width="150px" CssClass="dropdown">
                                            </asp:DropDownList>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddlByAirSea" runat="server" CssClass="dropdown" Width="150px">
                                            </asp:DropDownList>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddlPortOfLoading" runat="server" CssClass="dropdown" Width="150px">
                                            </asp:DropDownList>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtSeaPort" runat="server" CssClass="textb" onkeydown="return (event.keyCode!=13);"></asp:TextBox>
                                        </td>
                                    </tr>
                                </table>
                                <table>
                                    <tr>
                                        <td>
                                            <asp:Button ID="btnContainerpackingMatCost" OnClientClick="return CalculateContainer();"
                                                runat="server" Text="Packing Cost" Visible="false" OnClick="btnContainerpackingMatCost_Click"
                                                CssClass="buttonnorm" />
                                        </td>
                                    </tr>
                                    <tr id="trReferenceImage" runat="server">
                                        <td>
                                            <asp:Button ID="BtnReferenceImageSave" runat="server" Text="RefImageSave" Visible="false"
                                                OnClientClick="return confirm('Do you want to save data?')" CssClass="buttonnorm" />
                                        </td>
                                    </tr>
                                </table>
                                <table>
                                    <tr>
                                        <td colspan="2">
                                            <asp:Label ID="LblErrorMessage" runat="server" Font-Bold="true" ForeColor="RED" Text="Some IMportent Fields are Missing......."
                                                Visible="false"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="4">
                                            <div style="width: 85%; height: 300px; overflow: AUTO">
                                                <asp:GridView ID="DGOrderDetail" runat="server" DataKeyNames="Sr_No" AutoGenerateColumns="False"
                                                    OnRowCommand="DGOrderDetail_RowCommand" CssClass="grid-views" OnRowDataBound="DGOrderDetail_RowDataBound">
                                                    <AlternatingRowStyle BackColor="White" />
                                                    <HeaderStyle CssClass="gvheaders" />
                                                    <RowStyle CssClass="gvalts" />
                                                    <Columns>
                                                        <asp:BoundField DataField="Sr_No" HeaderText="Sr_No" />
                                                        <asp:BoundField DataField="CATEGORY" HeaderText="CATEGORY" />
                                                        <asp:BoundField DataField="ITEMNAME" HeaderText="ITEMNAME" />
                                                        <asp:BoundField DataField="DESCRIPTION" HeaderText="DESCRIPTION" />
                                                        <asp:TemplateField HeaderText="Qty">
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="TxtQtyGD" runat="server" Width="80px" AutoPostBack="true" Text='<%# Bind("QTY") %>'
                                                                    onkeypress="return isNumber(event);" CssClass="textb" OnTextChanged="TextQtyChanged_Event"></asp:TextBox>
                                                                <asp:Label runat="server" ID="area1" Text='<%# Bind("AREA1") %>' Visible="false"></asp:Label>
                                                                <asp:Label runat="server" ID="Ordercal" Text='<%# Bind("OrderCalType") %>' Visible="false"></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="RATE">
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="TxtRateGD" runat="server" Width="80px" AutoPostBack="true" Text='<%# Bind("RATE") %>'
                                                                    onkeypress="return isNumber(event);" CssClass="textb" OnTextChanged="TextRateChanged_Event"></asp:TextBox>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:BoundField DataField="AREA" HeaderText="AREA" />
                                                        <asp:BoundField DataField="AMOUNT" HeaderText="AMOUNT" />
                                                        <asp:BoundField DataField="SUBQUALITY" HeaderText="SUBQUALITY" />
                                                        <asp:TemplateField HeaderText="CONSUMPTION">
                                                            <ItemTemplate>
                                                                <asp:Button ID="BTNCONSUMPTION" CssClass="buttonnorm" Width="120PX" runat="server"
                                                                    Text="CONSUMPTION" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>"
                                                                    OnClick="BTNCONSUMPTION_Click" />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="EXPENCE">
                                                            <ItemTemplate>
                                                                <asp:Button ID="BTNEXPENCE" CssClass="buttonnorm " Width="120PX" runat="server" Text="EXPENCE"
                                                                    CommandArgument="<%# ((GridViewRow) Container).RowIndex %>" OnClick="BTNEXPENCE_Click" />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="PACKING">
                                                            <ItemTemplate>
                                                                <asp:Button ID="BTNPACKING" CssClass="buttonnorm " Width="100PX" runat="server" Text="PACKING"
                                                                    CommandArgument="<%# ((GridViewRow) Container).RowIndex %>" OnClick="BTNPACKING_Click" />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                    </Columns>
                                                    <EmptyDataRowStyle CssClass="gvemptytext" />
                                                </asp:GridView>
                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                        </td>
                                        <td>
                                        </td>
                                        <td id="TDCancelDraftOrder" runat="server" visible="false" align="right">
                                            <asp:Button CssClass="buttonnorm" ID="BtnCancelDraftOrder" runat="server" Text="Cancel"
                                                OnClick="BtnCancelDraftOrder_Click" OnClientClick="return confirm('ARE U SURE TO CANCEL THIS ORDER?')" />
                                        </td>
                                        <td>
                                            <asp:Button CssClass="buttonnorm" ID="BtnNew" runat="server" Text="New" />
                                            <asp:Button CssClass="buttonnorm" ID="BtnSave" Text="Save" runat="server" OnClick="BtnSave_Click"
                                                OnClientClick="return confirm('Do you want to save data?')" ValidationGroup="f1" />
                                            <asp:Button CssClass="buttonnorm" ID="BtnClose" runat="server" Text="Close" OnClientClick="return CloseForm();" />
                                            <asp:Button CssClass="buttonnorm" ID="BtncostReport" runat="server" Text="Costing Report"
                                                OnClick="BtncostReport_Click" />
                                            <asp:Button CssClass="buttonnorm preview_width" ID="BtnReport" runat="server" Text="Preview"
                                                OnClick="BtnReport_Click1" />
                                            <asp:DropDownList ID="DDPreviewType" runat="server" CssClass="dropdown" OnSelectedIndexChanged="DDPreviewType_SelectedIndexChanged"
                                                AutoPostBack="True">
                                                <asp:ListItem Text="PerForma Invoice HSCODE" Value="0"></asp:ListItem>
                                                <asp:ListItem Text="PerForma Invoice BUYERCODE" Value="1"></asp:ListItem>
                                                <asp:ListItem Text="Photo Quotation" Value="2"></asp:ListItem>
                                                <asp:ListItem Text="Sample Order" Value="3"></asp:ListItem>
                                                <asp:ListItem Text="PerForma Invoice" Value="4"></asp:ListItem>
                                                <asp:ListItem Text="PerForma Invoice 2" Value="5"></asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="4">
                                            <div style="height: 50%">
                                                <asp:GridView ID="DGConsumption" runat="server" AutoGenerateColumns="False" DataKeyNames="PCMDID"
                                                    CssClass="grid-views">
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
                                                                <asp:TextBox ID="TXTIQTY" runat="server" Width="70px" Text='<%# Bind("IQty") %>'
                                                                    AutoPostBack="true"></asp:TextBox>
                                                            </ItemTemplate>
                                                            <HeaderStyle Width="80px" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="ILoss">
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="TXTILOSS" runat="server" Width="70px" Text='<%# Bind("ILoss") %>'
                                                                    AutoPostBack="true"></asp:TextBox>
                                                            </ItemTemplate>
                                                            <HeaderStyle Width="80px" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="IRate">
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="TXTIRate" runat="server" Width="70px" Text='<%# Bind("IRate") %>'
                                                                    AutoPostBack="true"></asp:TextBox>
                                                            </ItemTemplate>
                                                            <HeaderStyle Width="80px" />
                                                        </asp:TemplateField>
                                                        <asp:BoundField DataField="OCategory" HeaderText="OCategory" />
                                                        <asp:BoundField DataField="OItem" HeaderText="OItem" />
                                                        <asp:BoundField DataField="ODescription" HeaderText="ODescription" />
                                                        <asp:BoundField DataField="OUnitName" HeaderText="OUnitName" />
                                                        <asp:TemplateField HeaderText="OQty">
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="TXTOQTY" runat="server" Width="70px" Text='<%# Bind("OQty") %>'
                                                                    AutoPostBack="true"></asp:TextBox>
                                                            </ItemTemplate>
                                                            <HeaderStyle Width="80px" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="ORate">
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="TXTORAte" runat="server" Width="70px" Text='<%# Bind("ORate") %>'
                                                                    AutoPostBack="true"></asp:TextBox>
                                                            </ItemTemplate>
                                                            <HeaderStyle Width="80px" />
                                                        </asp:TemplateField>
                                                    </Columns>
                                                </asp:GridView>
                                                <asp:GridView ID="DGExpence" runat="server" AutoGenerateColumns="False" DataKeyNames="CWOEID"
                                                    CssClass="grid-views">
                                                    <HeaderStyle CssClass="gvheaders" />
                                                    <AlternatingRowStyle CssClass="gvalts" />
                                                    <RowStyle CssClass="gvrow" />
                                                    <EmptyDataRowStyle CssClass="gvemptytext" />
                                                    <Columns>
                                                        <asp:BoundField DataField="CWOEID" HeaderText="CWOEID" />
                                                        <asp:BoundField DataField="ChargeName" HeaderText="ChargeName" />
                                                        <asp:TemplateField HeaderText="Percentage">
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="TXTPercentage" runat="server" Width="70px" Text='<%# Bind("Percentage") %>'
                                                                    AutoPostBack="true"></asp:TextBox>
                                                            </ItemTemplate>
                                                            <HeaderStyle Width="80px" />
                                                        </asp:TemplateField>
                                                    </Columns>
                                                </asp:GridView>
                                                <asp:GridView ID="DGPacking" runat="server" AutoGenerateColumns="False" DataKeyNames="PRMCID"
                                                    CssClass="grid-views" OnRowCreated="DGPacking_RowCreated">
                                                    <HeaderStyle CssClass="gvheaders" />
                                                    <AlternatingRowStyle CssClass="gvalts" />
                                                    <RowStyle CssClass="gvrow" />
                                                    <EmptyDataRowStyle CssClass="gvemptytext" />
                                                    <Columns>
                                                        <asp:BoundField DataField="PRMCID" HeaderText="PRMCID" />
                                                        <asp:TemplateField HeaderText="INNERAMT">
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="TXTINNERAMT" runat="server" Width="70px" Text='<%# Bind("INNERAMT") %>'
                                                                    AutoPostBack="true"></asp:TextBox>
                                                            </ItemTemplate>
                                                            <HeaderStyle Width="80px" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="MIDDLEAMT">
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="TXTMIDDLEAMT" runat="server" Width="70px" Text='<%# Bind("MIDDLEAMT") %>'
                                                                    AutoPostBack="true"></asp:TextBox>
                                                            </ItemTemplate>
                                                            <HeaderStyle Width="80px" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="MASTERAMT">
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="TXTMASTERAMT" runat="server" Width="70px" Text='<%# Bind("MASTERAMT") %>'
                                                                    AutoPostBack="true"></asp:TextBox>
                                                            </ItemTemplate>
                                                            <HeaderStyle Width="80px" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="OTHERAMT">
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="TXTOTHERAMT" runat="server" Width="70px" Text='<%# Bind("OTHERAMT") %>'
                                                                    AutoPostBack="true"></asp:TextBox>
                                                            </ItemTemplate>
                                                            <HeaderStyle Width="80px" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="CONTAINERAMT">
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="TXTCONTAINERAMT" runat="server" Width="70px" Text='<%# Bind("CONTAINERAMT") %>'
                                                                    AutoPostBack="true"></asp:TextBox>
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
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="label12" Text=" TOTAL AREA" runat="server" CssClass="labelbold" />
                                            <asp:TextBox CssClass="textb" ID="TxtOrderArea" runat="server"></asp:TextBox>
                                        </td>
                                        <td colspan="2">
                                            <asp:Label ID="label13" Text="TOTAL QTY" runat="server" CssClass="labelbold" />
                                            <asp:TextBox CssClass="textb" ID="TxtTotalQtyRequired" runat="server"></asp:TextBox>
                                        </td>
                                        <td>
                                            <asp:Label ID="label14" Text=" TOTAL AMT" runat="server" CssClass="labelbold" />
                                            <asp:TextBox CssClass="textb" ID="TxtTotalAmount" runat="server"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">
                                            <asp:TextBox ID="TxtFinishedid" runat="server" ForeColor="White" Width="0px"></asp:TextBox>
                                        </td>
                                    </tr>
                                </table>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </div>
                <div style="background-color: #edf3fe; text-align: center; height: 43px; width: 100%">
                    <span class="labelcopyr">&copy; Enablesoft Erp Consultancy Pvt. Ltd.All rights reserved.</span>
                </div>
            </td>
        </tr>
    </table>
    </form>
</body>
</html>
