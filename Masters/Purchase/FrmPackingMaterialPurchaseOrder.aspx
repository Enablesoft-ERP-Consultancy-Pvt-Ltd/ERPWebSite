<%@ Page Language="C#" AutoEventWireup="true" CodeFile="FrmPackingMaterialPurchaseOrder.aspx.cs"
    EnableEventValidation="false" Inherits="Masters_Purchase_FrmPackingMaterialPurchaseOrder" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Src="~/UserControls/ucmenu.ascx" TagName="ucmenu" TagPrefix="uc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<script src="../../Scripts/JScript.js" type="text/javascript"></script>
<script src="../../Scripts/jquery-1.4.1.min.js" type="text/javascript"></script>
<%--<script type="text/javascript" src="../../Scripts/Fixfocus.js"></script>--%>
<script type="text/javascript">
    function CloseForm() {
        window.location.href = "../../main.aspx";
    }
    function NewForm() {
        window.location.href = "FrmPackingMaterialPurchaseOrder.aspx";
    }
    function report() {
        //var varReportName = "Reports/BankReport.rpt";
        //window.open('../../ReportViewer.aspx?ReportName=' + varReportName + '& CommanFn='+""+'', '');
        window.open('../../ReportViewer.aspx', '');
    }
    function YourFunctionName(msg) {
        var txt = msg;
        alert(txt);
    }
    function Validate() {

        if (document.getElementById('ddcustomercode').value == "0") {
            alert("Please Select Customer Code! ");
            document.getElementById('ddcustomercode').focus();
            return false;
        }
        else if (document.getElementById('ddorderno').value == "0") {
            alert("Please Select Order No!");
            document.getElementById('ddorderno').focus();
            return false;
        }
        else if (document.getElementById('ddempname').value == "0") {
            alert("Please Select party!");
            document.getElementById('ddempname').focus();
            return false;
        }
        else if (document.getElementById('ChkEditOrder').checked == true) {
            if (document.getElementById('ddchalanno').value == "0") {
                alert("Please Select Challan!");
                document.getElementById('ddchalanno').focus();
                return false;
            }
        }
        else if (document.getElementById('txtqty').value == "") {
            alert("Qty can not be blank!");
            document.getElementById('txtqty').focus();
            return false;
        }
        else if (document.getElementById('txtrate').value == "") {
            alert("Rate can not be blank!");
            document.getElementById('txtrate').focus();
            return false;
        }
        else {
            return confirm('Do You Want To Save?')
        }

    }
    function OnRateQtyChange() {

        var qty = document.getElementById("txtqty").value;
        var rate = document.getElementById("txtrate").value;
        var Amt = 0;

        Amt = qty * rate;
        Amt = Math.round(Amt * Math.pow(10, 2)) / Math.pow(10, 2);
        // Amt = Math.round(Amt, 5)
        document.getElementById("Txtamount").value = Amt;
    }


    function CheckDate(id) {
        try {

            var ODate = document.getElementById("txtdate").value;
            var DueDate = document.getElementById("txtduedate").value;
            var inputs = document.getElementById("txtdeldate").value;
            var day1 = ODate.substring(0, 2);
            var month1 = ODate.substring(3, 6);
            var year1 = ODate.substring(7, 11);


            var day3 = DueDate.substring(0, 2);
            var month3 = DueDate.substring(3, 6);
            var year3 = DueDate.substring(7, 11);

            month1 = changeFormatStringtoNumber(month1);
            month3 = changeFormatStringtoNumber(month3);
            var d1 = new Date(year1, month1, day1);
            var d3 = new Date(year3, month3, day3);
            //            alert(month2);

            var DDate = inputs;
            var day2 = DDate.substring(0, 2);
            var month2 = DDate.substring(3, 6);
            var year2 = DDate.substring(7, 11);

            month2 = changeFormatStringtoNumber(month2);
            var d2 = new Date(year2, month2, day2);
            if (d1 > d2) {

                alert("Delivery Date Should be Grater than Order Date! ");
                if (id == 1) {
                    document.getElementById("txtdeldate").value = ODate;
                }
                else {
                    document.getElementById("txtdate").value = ODate;
                    document.getElementById("txtduedate").value = ODate;
                    document.getElementById("txtdeldate").value = ODate;

                }
                return false;

            }
            else if (d2 > d3) {
                alert("Due Date can not be shorter than Delivery Date! ");
                document.getElementById("txtdate").value = ODate;
                document.getElementById("txtduedate").value = DueDate;
                document.getElementById("txtdeldate").value = inputs;
                if (id == 1) {
                    document.getElementById("txtdeldate").value = DueDate;

                }
                else {
                    document.getElementById("txtduedate").value = DDate;
                }

                return false;
            }

        }
        catch (err) {
            alert(err.Message);
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

</script>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <link href="../../App_Themes/Default/Style.css" rel="stylesheet" type="text/css" />
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <%-- <table width="100%">
            <tr>
                <td width="100%" colspan="2" valign="top">--%>
        <table width="100%">
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
                <td width="75%">
                    <uc1:ucmenu ID="ucmenu1" runat="server" />
                    <asp:ScriptManager ID="ScriptManager1" runat="server">
                    </asp:ScriptManager>
                </td>
                <td width="25%">
                    <asp:UpdatePanel ID="up" runat="server">
                        <ContentTemplate>
                            <asp:Button Width="100%" BorderStyle="None" BackColor="#999999" ID="BtnLogout" runat="server"
                                Text="Logout" OnClick="BtnLogout_Click" />
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </td>
            </tr>
        </table>
        <%-- </td>
            </tr>
            </table>--%>
        <table>
            <tr>
                <td>
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                        <ContentTemplate>
                            <table width="105%">
                                <tr>
                                    <td valign="top">
                                        <table width="100%">
                                            <tr>
                                                <td>
                                                    <asp:CheckBox ID="ChkEditOrder" runat="server" Text="EDIT ORDER" AutoPostBack="True"
                                                        OnCheckedChanged="ChkEditOrder_CheckedChanged" />
                                                </td>
                                                <%-- <td id="td_ord" runat="server" style="display: none">
                                        Order No :&nbsp;
                                        <asp:TextBox ID="TxtOrderId" runat="server" Width="90px" Height="20px" AutoPostBack="True"
                                            OnTextChanged="TxtOrderId_TextChanged"></asp:TextBox>
                                    </td>--%>
                                            </tr>
                                            <tr id="Tr1" runat="server">
                                                <td id="Td1" width="100px">
                                                    Company Name<br />
                                                    <asp:DropDownList ID="ddCompName" runat="server" TabIndex="1" CssClass="dropdown">
                                                    </asp:DropDownList>
                                                    <cc1:ListSearchExtender ID="ListSearchExtender4" runat="server" TargetControlID="ddCompName"
                                                        ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                                                    </cc1:ListSearchExtender>
                                                </td>
                                                <td id="tdcustomer" runat="server" width="100px">
                                                    Customer Code<br />
                                                    <asp:DropDownList ID="ddcustomercode" runat="server" CssClass="dropdown" AutoPostBack="True"
                                                        OnSelectedIndexChanged="ddcustomercode_SelectedIndexChanged">
                                                    </asp:DropDownList>
                                                    <cc1:ListSearchExtender ID="ListSearchExtender1" runat="server" TargetControlID="ddcustomercode"
                                                        ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                                                    </cc1:ListSearchExtender>
                                                </td>
                                                <td id="tdorderno" runat="server" width="100px">
                                                    OrderNo.<br />
                                                    <asp:DropDownList ID="ddorderno" runat="server" CssClass="dropdown" AutoPostBack="True"
                                                        OnSelectedIndexChanged="ddorderno_SelectedIndexChanged">
                                                    </asp:DropDownList>
                                                    <cc1:ListSearchExtender ID="ListSearchExtender2" runat="server" TargetControlID="ddorderno"
                                                        ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                                                    </cc1:ListSearchExtender>
                                                </td>
                                                <td id="Td4" width="100px">
                                                    Party Name<br />
                                                    <asp:DropDownList ID="ddempname" runat="server" CssClass="dropdown" AutoPostBack="True"
                                                        OnSelectedIndexChanged="ddempname_SelectedIndexChanged">
                                                    </asp:DropDownList>
                                                    <cc1:ListSearchExtender ID="ListSearchExtender3" runat="server" TargetControlID="ddempname"
                                                        ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                                                    </cc1:ListSearchExtender>
                                                </td>
                                                <td id="tdchalanno" runat="server" visible="false">
                                                    Chalan.No.<br />
                                                    <asp:DropDownList ID="ddchalanno" runat="server" CssClass="dropdown" AutoPostBack="True"
                                                        OnSelectedIndexChanged="ddchalanno_SelectedIndexChanged">
                                                    </asp:DropDownList>
                                                    <cc1:ListSearchExtender ID="ListSearchExtender5" runat="server" TargetControlID="ddchalanno"
                                                        ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                                                    </cc1:ListSearchExtender>
                                                </td>
                                            </tr>
                                        </table>
                                        <table>
                                            <tr class="RowStyle">
                                                <td id="Td5">
                                                    OrderDate<br />
                                                    <asp:TextBox ID="txtdate" Width="100px" runat="server" TabIndex="5" AutoPostBack="false"
                                                        CssClass="textb" onchange="return CheckDate(0);"></asp:TextBox>
                                                    <asp:RequiredFieldValidator SetFocusOnError="true" ID="RequiredFieldValidator1" runat="server"
                                                        ErrorMessage="please Enter Date" ControlToValidate="txtdate" ValidationGroup="f1"
                                                        ForeColor="Red">*</asp:RequiredFieldValidator>
                                                    <asp:CalendarExtender ID="CalendarExtender1" runat="server" Format="dd-MMM-yyyy"
                                                        TargetControlID="txtdate">
                                                    </asp:CalendarExtender>
                                                </td>
                                                <td id="Td6" runat="server" class="tdstyle" visible="true">
                                                    Item Code<br />
                                                    <asp:TextBox ID="txtItemCode" Width="100px" runat="server" CssClass="textb"></asp:TextBox>
                                                </td>
                                                <td class="tdstyle">
                                                    Length<br />
                                                    <asp:TextBox ID="txtlen" Width="100px" runat="server" CssClass="textb" onkeypress="return isNumber(event);"></asp:TextBox>
                                                </td>
                                                <td class="tdstyle">
                                                    Height<br />
                                                    <asp:TextBox ID="txtheight" Width="100px" runat="server" CssClass="textb" onkeypress="return isNumber(event);"></asp:TextBox>
                                                </td>
                                                <td class="tdstyle">
                                                    Width<br />
                                                    <asp:TextBox ID="txtwidth" Width="100px" runat="server" CssClass="textb" onkeypress="return isNumber(event);"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr class="RowStyle">
                                                <td class="tdstyle">
                                                    GSM<br />
                                                    <asp:TextBox ID="txtgsm" Width="100px" runat="server" CssClass="textb" onkeypress="return isNumber(event);"></asp:TextBox>
                                                </td>
                                                <td>
                                                    GSM2<br />
                                                    <asp:TextBox ID="txtgsm2" Width="100px" runat="server" CssClass="textb" onkeypress="return isNumber(event);"></asp:TextBox>
                                                </td>
                                                <td>
                                                    Ply<br />
                                                    <asp:TextBox ID="txtply" Width="100px" runat="server" CssClass="textb" onkeypress="return isNumber(event);"></asp:TextBox>
                                                </td>
                                                <td>
                                                    Qty<br />
                                                    <asp:TextBox ID="txtqty" Width="100px" runat="server" CssClass="textb" onchange="return OnRateQtyChange();"
                                                        onkeypress="return isNumber(event);"></asp:TextBox>
                                                </td>
                                                <td>
                                                    Pending Qty<br />
                                                    <asp:TextBox ID="txtPqty" Width="100px" runat="server" CssClass="textb" Enabled="false"></asp:TextBox>
                                                </td>
                                                <tr class="RowStyle">
                                                    <td>
                                                        Rate<br />
                                                        <asp:TextBox ID="txtrate" Width="100px" runat="server" CssClass="textb" onkeypress="return isNumber(event);"
                                                            onchange="return OnRateQtyChange();"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        Pcs<br />
                                                        <asp:TextBox ID="txtpcs" Width="100px" runat="server" CssClass="textb" onkeypress="return isNumber(event);"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        Weight<br />
                                                        <asp:TextBox ID="txtweight" Width="100px" runat="server" CssClass="textb" onkeypress="return isNumber(event);"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        Amount<br />
                                                        <asp:TextBox ID="Txtamount" Width="100px" runat="server" CssClass="textb" onkeypress="return isNumber(event);"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        Delivery Date<br />
                                                        <asp:TextBox ID="txtdeldate" Width="100px" runat="server" CssClass="textb" onchange="return CheckDate(1);"
                                                            AutoPostBack="true"></asp:TextBox>
                                                        <asp:CalendarExtender ID="CalendarExtender4" runat="server" Format="dd-MMM-yyyy"
                                                            TargetControlID="txtdeldate">
                                                        </asp:CalendarExtender>
                                                    </td>
                                                </tr>
                                                <tr class="RowStyle">
                                                    <td class="tdstyle">
                                                        Remarks
                                                    </td>
                                                    <td colspan="3">
                                                        <asp:TextBox ID="txtremarks" runat="server" ForeColor="Red" Height="25px" Width="100%"
                                                            CssClass="textb" TextMode="MultiLine"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        Packing Type<br />
                                                        <asp:DropDownList ID="DDpackingType" runat="server" CssClass="dropdown">
                                                            <asp:ListItem Value="1">Inner</asp:ListItem>
                                                            <asp:ListItem Value="2">Middle</asp:ListItem>
                                                            <asp:ListItem Value="3">Master</asp:ListItem>
                                                        </asp:DropDownList>
                                                    </td>
                                                </tr>
                                            </tr>
                                            <tr>
                                                <td colspan="5" align="right">
                                                    <asp:Button ID="btnnew" runat="server" Text="New" CssClass="buttonnorm" OnClientClick="return NewForm();"
                                                        OnClick="btnnew_Click" />
                                                    <asp:Button ID="btnsave" runat="server" Text="Save" TabIndex="18" CssClass="buttonnorm"
                                                        OnClientClick="return Validate();" OnClick="btnsave_Click" />
                                                    <asp:Button ID="btnpriview" runat="server" Text="Preview" Visible="false" OnClick="btnpriview_Click"
                                                        CssClass="buttonnorm" />
                                                    <asp:Button ID="BTNCLOSE" runat="server" Text="Close" OnClientClick="return CloseForm();"
                                                        CssClass="buttonnorm" />
                                                    <asp:Button ID="btndelete" runat="server" Text="Delete" Visible="false" CssClass="buttonnorm" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="2">
                                                    <asp:Label ID="LblErrorMessage" runat="server" Font-Bold="true" ForeColor="RED" Text="ONE OR MORE MANDATORY FIELDS ARE MISSING......."
                                                        Visible="false"></asp:Label>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                    <td>
                                        <table>
                                            <tr>
                                                <td class="tdstyle" colspan="4">
                                                    Destination
                                                    <asp:TextBox ID="txtdestination" runat="server" Width="114px" CssClass="textb"></asp:TextBox>
                                                </td>
                                                <tr>
                                                    <td class="tdstyle">
                                                        Payment Term
                                                    </td>
                                                    <td colspan="2">
                                                        <asp:DropDownList ID="ddpayement" runat="server" Height="21px" Width="115px" CssClass="dropdown">
                                                        </asp:DropDownList>
                                                    </td>
                                                    <td class="tdstyle">
                                                        Insurence
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtinsurence" runat="server" Width="94px" CssClass="textb"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="tdstyle">
                                                        Frieght
                                                    </td>
                                                    <td colspan="2">
                                                        <asp:TextBox ID="txtfrieght" runat="server" CssClass="textb" Width="93px"></asp:TextBox>
                                                    </td>
                                                    <td class="tdstyle">
                                                        Frieght Rate
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtfrieghtrate" runat="server" CssClass="textb" Height="18px" onkeypress="return isNumber(event);"
                                                            Width="94px" AutoPostBack="True" OnTextChanged="txtfrieghtrate_TextChanged"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="tdstyle">
                                                        Transport Mode
                                                    </td>
                                                    <td colspan="4">
                                                        <asp:DropDownList ID="ddtransprt" runat="server" Height="21px" Width="116px" CssClass="dropdown">
                                                        </asp:DropDownList>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="tdstyle">
                                                        Delivery Terms
                                                    </td>
                                                    <td colspan="4">
                                                        <asp:DropDownList ID="dddelivery" runat="server" Height="21px" Width="114px" CssClass="dropdown">
                                                        </asp:DropDownList>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="tdstyle">
                                                        Form No.
                                                    </td>
                                                    <td colspan="4">
                                                        <asp:TextBox ID="txtform" runat="server" Width="100px" CssClass="textb"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="tdstyle">
                                                        AgentName
                                                    </td>
                                                    <td colspan="2">
                                                        <asp:TextBox ID="TxtAgentName" runat="server" CssClass="textb"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="tdstyle">
                                                        PackingCharges
                                                    </td>
                                                    <td>
                                                        <asp:DropDownList ID="DDPackingCharges" runat="server" CssClass="dropdown">
                                                            <asp:ListItem Text="Yes" Value="0"></asp:ListItem>
                                                            <asp:ListItem Text="No" Value="1"></asp:ListItem>
                                                        </asp:DropDownList>
                                                    </td>
                                                    <td class="tdstyle">
                                                        Due Date
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtduedate" runat="server" CssClass="textb" Width="90px" onchange="return CheckDate(2);"
                                                            AutoPostBack="true"></asp:TextBox>
                                                        <asp:CalendarExtender ID="CalendarExtender2" runat="server" Format="dd-MMM-yyyy"
                                                            TargetControlID="txtduedate">
                                                        </asp:CalendarExtender>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="5">
                                                        <div style="width: 100%; height: 100px; overflow: scroll;">
                                                            <asp:GridView ID="GDVSHOWORDER" runat="server" AutoGenerateColumns="false" OnRowCreated="GDVSHOWORDER_RowCreated"
                                                                CssClass="grid-view" OnRowDataBound="GDVSHOWORDER_RowDataBound" DataKeyNames="SrNo"
                                                                OnSelectedIndexChanged="GDVSHOWORDER_SelectedIndexChanged">
                                                                <HeaderStyle CssClass="gvheader" />
                                                                <AlternatingRowStyle CssClass="gvalt" />
                                                                <RowStyle CssClass="gvrow" />
                                                                <EmptyDataRowStyle CssClass="gvemptytext" />
                                                                <Columns>
                                                                    <asp:BoundField DataField="orderno" HeaderText="OrderNo" />
                                                                    <asp:BoundField DataField="ProductCode" HeaderText="Item" />
                                                                    <asp:TemplateField Visible="false">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="LblOrderID" runat="server" Text='<%# Bind("OrderID") %>'></asp:Label>
                                                                            <asp:Label ID="Lbldetailid" runat="server" Text='<%# Bind("pdetail") %>'></asp:Label>
                                                                            <asp:Label ID="lbllength" runat="server" Text='<%# Bind("Length") %>'></asp:Label>
                                                                            <asp:Label ID="lblwidth" runat="server" Text='<%# Bind("width") %>'></asp:Label>
                                                                            <asp:Label ID="lblHeight" runat="server" Text='<%# Bind("Height") %>'></asp:Label>
                                                                            <asp:Label ID="lblgsm" runat="server" Text='<%# Bind("gsm") %>'></asp:Label>
                                                                            <asp:Label ID="lblgsm2" runat="server" Text='<%# Bind("gsm2") %>'></asp:Label>
                                                                            <asp:Label ID="lblply" runat="server" Text='<%# Bind("ply") %>'></asp:Label>
                                                                            <asp:Label ID="lblunit" runat="server" Text='<%# Bind("weight") %>'></asp:Label>
                                                                            <asp:Label ID="Lblremark" runat="server" Text='<%# Bind("remark") %>'></asp:Label>
                                                                            <asp:Label ID="Lbldeldate" runat="server" Text='<%# Bind("ddate") %>'></asp:Label>
                                                                            <asp:Label ID="lblodate" runat="server" Text='<%# Bind("Odate") %>'></asp:Label>
                                                                            <asp:Label ID="lblduedate" runat="server" Text='<%# Bind("Duedate") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:BoundField DataField="PackingType" HeaderText="PackingType" />
                                                                    <asp:BoundField DataField="unit" HeaderText="Unit" />
                                                                    <asp:BoundField DataField="Qty" HeaderText="Qty" />
                                                                    <asp:BoundField DataField="Rate" HeaderText="Rate" />
                                                                    <asp:BoundField DataField="amt" HeaderText="Amount" />
                                                                    <asp:BoundField DataField="pcs" HeaderText="pcs" />
                                                                </Columns>
                                                                <HeaderStyle CssClass="gvheader" />
                                                                <AlternatingRowStyle CssClass="gvalt" />
                                                                <RowStyle CssClass="gvrow" />
                                                                <EmptyDataRowStyle CssClass="gvemptytext" />
                                                            </asp:GridView>
                                                    </td>
                                                </tr>
                                                </div>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                            <table>
                                <tr>
                                    <td colspan="5" width="600px">
                                        <asp:GridView ID="DGSHOWDATA" CssClass="grid-view" runat="server" AutoGenerateColumns="False"
                                            OnRowCreated="DGSHOWDATA_RowCreated" DataKeyNames="pdetail" OnRowDataBound="DGSHOWDATA_RowDataBound"
                                            OnRowDeleting="DGSHOWDATA_RowDeleting" OnSelectedIndexChanged="DGSHOWDATA_SelectedIndexChanged">
                                            <HeaderStyle CssClass="gvheader" />
                                            <AlternatingRowStyle CssClass="gvalt" />
                                            <RowStyle CssClass="gvrow" />
                                            <EmptyDataRowStyle CssClass="gvemptytext" />
                                            <Columns>
                                                <asp:BoundField DataField="Srno" HeaderText="Srno" />
                                                <asp:BoundField DataField="orderno" HeaderText="OrderNo" />
                                                <asp:BoundField DataField="PackingType" HeaderText="PackingType" />
                                                <asp:BoundField DataField="Length" HeaderText="Length" />
                                                <asp:BoundField DataField="width" HeaderText="Width" />
                                                <asp:BoundField DataField="Height" HeaderText="Height" />
                                                <asp:BoundField DataField="gsm" HeaderText="GSM" />
                                                <asp:BoundField DataField="gsm2" HeaderText="GSM2" />
                                                <asp:BoundField DataField="ply" HeaderText="Ply" />
                                                <asp:BoundField DataField="unit" HeaderText="Unit" />
                                                <asp:TemplateField HeaderText="Qty">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="TXTQTY" runat="server" Width="100px" Text='<%# Bind("Qty") %>' AutoPostBack="true"
                                                            OnTextChanged="Txtqty_TextChanged"></asp:TextBox>
                                                        <asp:Label ID="lbldetailid" runat="server" Visible="false" Text='<%# Bind("pdetail") %>' />
                                                    </ItemTemplate>
                                                    <HeaderStyle Width="80px" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Rate">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="TXTRAte" runat="server" Width="70px" Text='<%# Bind("Rate") %>'
                                                            AutoPostBack="true" OnTextChanged="Txtrate_TextChanged"></asp:TextBox>
                                                    </ItemTemplate>
                                                    <HeaderStyle Width="80px" />
                                                </asp:TemplateField>
                                                <asp:BoundField DataField="amt" HeaderText="Amount" />
                                                <asp:TemplateField HeaderText="Pcs">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="TXTPCS" runat="server" Width="70px" Text='<%# Bind("pcs") %>' AutoPostBack="true"
                                                            OnTextChanged="Txtpc_TextChanged"></asp:TextBox>
                                                    </ItemTemplate>
                                                    <HeaderStyle Width="80px" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Weight">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="TXTweight" runat="server" Width="70px" Text='<%# Bind("weight") %>'
                                                            AutoPostBack="true"></asp:TextBox>
                                                    </ItemTemplate>
                                                    <HeaderStyle Width="80px" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Remark">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="TXTrem" runat="server" Width="150px" TextMode="MultiLine" Text='<%# Bind("remark") %>'
                                                            AutoPostBack="true"></asp:TextBox>
                                                    </ItemTemplate>
                                                    <HeaderStyle Width="80px" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Delivery Date">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtdivery_date" runat="server" CssClass="textb" Width="90px" Text='<%# Bind("ddate") %>'
                                                            TabIndex="39"></asp:TextBox>
                                                        <asp:CalendarExtender ID="CalendarExtender3" runat="server" Format="dd-MMM-yyyy"
                                                            TargetControlID="txtdivery_date">
                                                        </asp:CalendarExtender>
                                                    </ItemTemplate>
                                                    <HeaderStyle Width="80px" />
                                                </asp:TemplateField>
                                                <asp:TemplateField Visible="true">
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="LinkButton1" runat="server" CausesValidation="False" CommandName="Delete"
                                                            Text="Del" OnClientClick="return confirm('Do you want to Delete data?')" CssClass="buttonnorm"></asp:LinkButton>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                    </td>
                                </tr>
                                <tr id="trtax" runat="server" style="display: none">
                                    <td class="tdstyle">
                                        Vat(%)<br />
                                        <asp:TextBox ID="TxtExceisDuty" runat="server" Width="80px" AutoPostBack="True"></asp:TextBox>
                                    </td>
                                    <td class="tdstyle">
                                        Sat(%)
                                        <br />
                                        <asp:TextBox ID="TxtEduCess" runat="server" Width="80px" AutoPostBack="True"></asp:TextBox>
                                    </td>
                                    <td class="tdstyle">
                                        CST(%)
                                        <br />
                                        <asp:TextBox ID="TxtCst" runat="server" Width="80px" AutoPostBack="True"></asp:TextBox>
                                    </td>
                                    <td class="tdstyle">
                                        Total Amount<br />
                                        <asp:TextBox ID="TxtTotalAmount" runat="server" Width="80px"></asp:TextBox>
                                    </td>
                                    <td class="tdstyle">
                                        Net Amount<br />
                                        <asp:TextBox ID="TxtNetAmount" runat="server" Width="80px"></asp:TextBox>
                                    </td>
                                </tr>
                            </table>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
