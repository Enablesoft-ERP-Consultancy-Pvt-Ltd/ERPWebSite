<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Draft_Order_New.aspx.cs"
    ViewStateMode="Enabled" Inherits="Masters_Order_Draft_order" EnableEventValidation="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register Src="~/UserControls/ucmenu.ascx" TagName="ucmenu" TagPrefix="uc2" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>OrderPlanning</title>
    <link href="../../Styles/vijay.css" rel="stylesheet" type="text/css" />
    <script src="../../Scripts/JScript.js" type="text/javascript"></script>
    <script src="../../Scripts/JScript.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery-1.4.1.min.js" type="text/javascript"></script>
    <script language="javascript" type="text/javascript">
        function CloseForm() {
            window.location.href = "../../main.aspx";
        }
        function priview() {
            window.open('../../ReportViewer.aspx');
        }
        function Addcons() {
            window.open('../Carpet/DefineBomAndConsumption.aspx?ZZZ=1', '', 'Height=600px,width=1000px');
        }
        function AdddateName() {
            window.open('../Carpet/AddInspectionDate.aspx', '', 'Height=600px,width=1000px');
        }
        function AddItum() {
            var a3 = 0;
            window.open('../Carpet/AddItemName.aspx?' + a3, '', 'Height=400px,width=500px');
            // window.open('AddItemt.aspx','popup Form', 'Width=550px, Height=400px') 
        }
        function checkcheck() {
            var isValid = false;
            var j = 0;
            var gridView = document.getElementById('DGOrderDetail');
            for (var i = 1; i < gridView.rows.length; i++) {
                var inputs = gridView.rows[i].getElementsByTagName('input');
                if (inputs != null) {
                    if (inputs[0].type == "checkbox") {
                        if (inputs[0].checked) {
                            j = j + 1;
                            if (j > 1) {
                                alert("Please Select Only One Item");
                                inputs[0].checked = false;
                                return false;
                            }
                        }
                    }
                }
            }
        }
        function validate() {
            if (document.getElementById('DDCompanyName').value == "0") {
                alert("Pls Select Company Name");
                document.getElementById('DDCompanyName').focus();
                return false;
            }
            if (document.getElementById('DDCustomerCode').value == "0") {
                alert("Pls Select Customer Name");
                document.getElementById('DDCustomerCode').focus();
                return false;
            }
            if (document.getElementById('ddorderno').value == "0") {
                alert("Pls Select Order No");
                document.getElementById('ddorderno').focus();
                return false;
            }
            if (document.getElementById('chklabel').checked == true) {
                var isValid = false;
                var gridView = document.getElementById('grdlabel');
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
                    alert("Please select atleast one Lable");
                    return false;
                }
            }
            if (document.getElementById('chkinspectiondate').checked == true) {
                var isValid = false;
                var gridView = document.getElementById('grdinspecctiondate');
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
                    alert("Please select atleast one Inspection Date");
                    return false;
                }
            }
            {
                var isValid = false;
                var i = 0;
                var gridView = document.getElementById('DGOrderDetail');
                for (var i = 1; i < gridView.rows.length; i++) {
                    var inputs = gridView.rows[i].getElementsByTagName('input');
                    if (inputs != null) {
                        if (inputs[0].type == "checkbox") {
                            if (inputs[0].checked) {
                                isValid = true;
                                return confirm('Do You Want To Save?')
                            }
                        }
                    }
                }
                alert("Please select atleast one checkbox");
                return false;
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
        
    </script>
    <style type="text/css">
        .style1
        {
            width: 815px;
        }
    </style>
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
                        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                            <ContentTemplate>
                                <table>
                                    <tr>
                                        <td>
                                            <asp:TextBox ID="TxtOrderDetailId" runat="server" Width="0px" Height="0px"></asp:TextBox>
                                        </td>
                                        <td class="tdstyle">
                                            <asp:CheckBox ID="ChkEditOrder" runat="server" CssClass="checkboxbold" Text="Edit Order"
                                                AutoPostBack="True" Enabled="false" OnCheckedChanged="ChkEditOrder_CheckedChanged" />
                                        </td>
                                        <td colspan="3">
                                            <br />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="tdstyle">
                                            <asp:Label ID="LBL" Text="COMPANY NAME" runat="server" CssClass="labelbold" />
                                            <b style="color: Red">*</b>
                                        </td>
                                        <td class="tdstyle">
                                            <asp:Label ID="Label1" Text=" CUST CODE" runat="server" CssClass="labelbold" />
                                            <b style="color: Red">*</b>
                                        </td>
                                        <td class="tdstyle">
                                            <asp:Label ID="Label2" Text="  CUST.ORD. NO" runat="server" CssClass="labelbold" />
                                        </td>
                                        <td class="tdstyle">
                                            <asp:Label ID="Label3" Text=" LOCAL ORDER" runat="server" CssClass="labelbold" />
                                            <b style="color: Red">*</b>
                                        </td>
                                        <td class="tdstyle">
                                            <asp:Label ID="Label4" Text=" ORDER DATE" runat="server" CssClass="labelbold" />
                                            <b style="color: Red">*</b>
                                        </td>
                                        <td class="tdstyle">
                                            <asp:Label ID="Label5" Text=" DELIVERY DATE" runat="server" CssClass="labelbold" />
                                            <b style="color: Red">*</b>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:DropDownList CssClass="dropdown" ID="DDCompanyName" runat="server" AutoPostBack="True"
                                                Width="200px">
                                            </asp:DropDownList>
                                        </td>
                                        <td>
                                            <asp:DropDownList CssClass="dropdown" ID="DDCustomerCode" runat="server" AutoPostBack="True"
                                                Width="250px" OnSelectedIndexChanged="DDCustomerCode_SelectedIndexChanged">
                                            </asp:DropDownList>
                                        </td>
                                        <td>
                                            <asp:DropDownList CssClass="dropdown" ID="ddorderno" runat="server" Width="90px"
                                                AutoPostBack="True" OnSelectedIndexChanged="ddorderno_SelectedIndexChanged">
                                            </asp:DropDownList>
                                        </td>
                                        <td>
                                            <asp:TextBox CssClass="textb" ID="Textlocalorder" runat="server" Width="100px"></asp:TextBox>
                                            <td>
                                                <asp:TextBox CssClass="textb" ID="TxtOrderDate" runat="server" Width="100px"></asp:TextBox>
                                                <asp:CalendarExtender ID="CalendarExtender1" runat="server" Format="dd-MMM-yyyy"
                                                    TargetControlID="TxtOrderDate">
                                                </asp:CalendarExtender>
                                            </td>
                                            <td>
                                                <asp:TextBox CssClass="textb" ID="TxtDeliveryDate" runat="server" Width="120px"></asp:TextBox>
                                                <asp:CalendarExtender ID="CalendarExtender2" runat="server" Format="dd-MMM-yyyy"
                                                    TargetControlID="TxtDeliveryDate">
                                                </asp:CalendarExtender>
                                            </td>
                                    </tr>
                                </table>
                                <table width="100%" border="1">
                                    <tr>
                                        <td>
                                            <asp:Label ID="LblErrorMessage" runat="server" ForeColor="RED" Text="Some IMportent Fields are Missing......."
                                                Visible="false" CssClass="labelbold"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr id="tr1" runat="server" style="display: none">
                                        <td valign="top" class="tdstyle">
                                            <asp:Label ID="Label6" Text="  Remark" runat="server" CssClass="labelbold" />
                                            <asp:TextBox ID="Txtremark" runat="server" TextMode="MultiLine" Width="300px" Height="100Px"
                                                CssClass="textb"></asp:TextBox>
                                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                            <asp:Button CssClass="buttonnorm" ID="BtnSave" Text="Save" runat="server" OnClick="BtnSave_Click"
                                                OnClientClick="return confirm('Do you want to save data?')" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td runat="server" id="tdinstruction">
                                            <asp:Label ID="TxtLabel" runat="server" Text="Green Rows Shows That Items Consumption Was Not Defined"
                                                ForeColor="Green" Font-Bold="True"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr align="center" id="trdrig" runat="server" style="display: none">
                                        <td align="center">
                                            <div style="width: 100%; height: 250px; overflow: auto">
                                                <asp:GridView ID="DGOrderDetail" Width="100%" runat="server" DataKeyNames="Sr_No"
                                                    AutoGenerateColumns="False" OnRowDataBound="DGOrderDetail_RowDataBound" OnRowCommand="DGOrderDetail_RowCommand"
                                                    CssClass="grid-views">
                                                    <HeaderStyle CssClass="gvheaders" />
                                                    <AlternatingRowStyle CssClass="gvalts" />
                                                    <Columns>
                                                        <asp:TemplateField HeaderText="">
                                                            <ItemTemplate>
                                                                <asp:CheckBox ID="Chkbox" runat="server" OnCheckedChanged="Chkbox_checked" />
                                                            </ItemTemplate>
                                                            <HeaderStyle Width="80px" />
                                                        </asp:TemplateField>
                                                        <asp:BoundField DataField="Sr_No" HeaderText="Sr_No" />
                                                        <asp:BoundField DataField="ourcode" HeaderText="Our Code" />
                                                        <asp:BoundField DataField="buyercode" HeaderText="Buyer Code" />
                                                        <asp:BoundField DataField="CATEGORY" HeaderText="CATEGORY" />
                                                        <asp:BoundField DataField="ITEMNAME" HeaderText="ITEMNAME" />
                                                        <asp:BoundField DataField="Qty" HeaderText="Qty" />
                                                        <asp:BoundField DataField="Area" HeaderText="Area" />
                                                        <asp:BoundField DataField="DESCRIPTION" HeaderText="DESCRIPTION" />
                                                        <asp:TemplateField HeaderText="PHOTO">
                                                            <ItemTemplate>
                                                                <asp:Image ID="Image1" runat="server" ImageUrl='<%# Bind("photo") %>' Height="70px"
                                                                    Width="100px" />
                                                                <%--<asp:Image ID="Image1" Width="100px" Height="50px" runat="server" ImageUrl='<%# "~/ImageHandler.ashx?ID=" + Eval("Sr_No")+"&img=4"%>' />--%>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:BoundField DataField="PPInstruction" HeaderText="PP.Instruction" />
                                                        <asp:TemplateField HeaderText="Description">
                                                            <ItemTemplate>
                                                                <asp:Button ID="BTNAdd" CssClass="buttonnorm" runat="server" Text="Add" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>"
                                                                    OnClick="BTNadd_Click" />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="consumption">
                                                            <ItemTemplate>
                                                                <asp:Button ID="BTNconsum" CssClass="buttonnorm" runat="server" Text="Show" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>"
                                                                    OnClick="BTNcon_Click" />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="ADD consumption">
                                                            <ItemTemplate>
                                                                <asp:Button CssClass="buttonsmalls" ID="btnconsump" Text="Add consumption" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>"
                                                                    runat="server" OnClientClick="return Addcons()" />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                    </Columns>
                                                    <EmptyDataRowStyle CssClass="gvemptytext" />
                                                </asp:GridView>
                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td id="tdlabinsp" runat="server" visible="false">
                                            <table>
                                                <tr>
                                                    <td>
                                                        <asp:CheckBox ID="chklabel" Text="Label" runat="server" AutoPostBack="true" OnCheckedChanged="chklabel_CheckedChanged"
                                                            CssClass="checkboxbold" />
                                                        <asp:Button ID="BtnAdd0" runat="server" CssClass="buttonsmalls" OnClientClick="return AddItum()"
                                                            Text="ADD" TabIndex="14" />
                                                        <asp:Button CssClass="buttonnorm" ID="refreshitem11" runat="server" Text="" BorderWidth="0px"
                                                            Height="1px" Width="1px" BackColor="White" BorderColor="White" BorderStyle="None"
                                                            ForeColor="White" OnClick="refreshitem11_Click" />
                                                    </td>
                                                    <td>
                                                        <asp:CheckBox ID="chkinspectiondate" Text="Inspection Dates" runat="server" AutoPostBack="true"
                                                            OnCheckedChanged="chkinspectiondate_CheckedChanged" CssClass="checkboxbold" />
                                                        &nbsp;<asp:Button ID="btnadddatename" runat="server" CssClass="buttonsmalls" OnClientClick="return AdddateName()"
                                                            Text="ADD" TabIndex="24" />
                                                        <asp:Button CssClass="buttonnorm" ID="refreshdatename" runat="server" Text="" BorderWidth="0px"
                                                            Height="1px" Width="1px" BackColor="White" BorderColor="White" BorderStyle="None"
                                                            ForeColor="White" OnClick="refreshdatename_Click" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td id="tdlabel" runat="server" visible="false">
                                                        <div style="width: 300px; height: 150px; overflow: scroll">
                                                            <asp:GridView ID="grdlabel" Width="300px" runat="server" DataKeyNames="Sr_No" AutoGenerateColumns="False"
                                                                CssClass="grid-views">
                                                                <HeaderStyle CssClass="gvheaders" />
                                                                <AlternatingRowStyle CssClass="gvalts" />
                                                                <Columns>
                                                                    <asp:TemplateField HeaderText="">
                                                                        <ItemTemplate>
                                                                            <asp:CheckBox ID="Chkbox" runat="server" />
                                                                        </ItemTemplate>
                                                                        <HeaderStyle Width="80px" />
                                                                    </asp:TemplateField>
                                                                    <asp:BoundField DataField="label" HeaderText="Label" />
                                                                </Columns>
                                                            </asp:GridView>
                                                        </div>
                                                    </td>
                                                    <td id="tdinspec" runat="server" visible="false">
                                                        <div style="width: 400px; height: 150px; overflow: scroll">
                                                            <asp:GridView ID="grdinspecctiondate" Width="300px" runat="server" DataKeyNames="Sr_No"
                                                                AutoGenerateColumns="False" CssClass="grid-views">
                                                                <HeaderStyle CssClass="gvheaders" />
                                                                <AlternatingRowStyle CssClass="gvalts" />
                                                                <Columns>
                                                                    <asp:TemplateField HeaderText="">
                                                                        <ItemTemplate>
                                                                            <asp:CheckBox ID="Chkbox" runat="server" />
                                                                        </ItemTemplate>
                                                                        <HeaderStyle Width="80px" />
                                                                    </asp:TemplateField>
                                                                    <asp:BoundField DataField="inspection" HeaderText="Inspection Date" />
                                                                    <asp:TemplateField HeaderText="Date">
                                                                        <ItemTemplate>
                                                                            <asp:TextBox ID="Txtdate" runat="server" CssClass="textb" Width="90px" TabIndex="39"></asp:TextBox>
                                                                            <asp:CalendarExtender ID="CalendarExtender2" runat="server" Format="dd-MMM-yyyy"
                                                                                TargetControlID="Txtdate">
                                                                            </asp:CalendarExtender>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                </Columns>
                                                            </asp:GridView>
                                                        </div>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="Label7" Text="   Inner Packing" runat="server" CssClass="labelbold" />
                                                        <br />
                                                        <asp:TextBox ID="Txtinnerpacking" runat="server" onkeypress="return isNumber(event);"></asp:TextBox>PCS
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="Label8" Text=" Master Packing" runat="server" CssClass="labelbold" />
                                                        <br />
                                                        <asp:TextBox ID="TxtMasterpacking" runat="server" onkeypress="return isNumber(event);"></asp:TextBox>PCS
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="Label9" Text=" Carton/Bales Size" runat="server" CssClass="labelbold" />
                                                        <br />
                                                        <asp:TextBox ID="TxtMiddlepacking" runat="server" Width="200px"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="Label10" Text="  Bales/Carton" runat="server" CssClass="labelbold" />
                                                        <br />
                                                        <asp:DropDownList CssClass="dropdown" ID="ddcarton" runat="server" Width="100px">
                                                            <asp:ListItem Value="0">Bales</asp:ListItem>
                                                            <asp:ListItem Value="1">Carton</asp:ListItem>
                                                        </asp:DropDownList>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="Label11" Text=" Testing Status" runat="server" CssClass="labelbold" />
                                                        <br />
                                                        <asp:DropDownList CssClass="dropdown" ID="DDStatus" runat="server" Width="100px">
                                                            <asp:ListItem Value="0">Yes</asp:ListItem>
                                                            <asp:ListItem Value="1">No</asp:ListItem>
                                                        </asp:DropDownList>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="Label12" Text="Testing Remark" runat="server" CssClass="labelbold" />
                                                        <br />
                                                        <asp:TextBox ID="TxtTestRemark" TextMode="MultiLine" Width="250px" runat="server"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="Label13" Text=" Item Remark" runat="server" CssClass="labelbold" />
                                                        <br />
                                                        <asp:TextBox ID="txtitemremark" TextMode="MultiLine" Width="250px" runat="server"></asp:TextBox>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="right">
                                            <asp:Button CssClass="buttonnorm" ID="BtnSave1" Text="Save" Visible="false" runat="server"
                                                OnClientClick="return validate();" OnClick="BtnSave1_Click" />
                                            <asp:Button CssClass="buttonnorm  preview_width" ID="btnreport" Text="Preview" runat="server"
                                                OnClick="btnreport_Click" />
                                        </td>
                                        <td>
                                            <asp:Button ID="refreshitem" runat="server" Style="display: none" OnClick="refreshitem_Click" />
                                        </td>
                                    </tr>
                                    <tr align="center" id="trgridcus" runat="server" style="display: none">
                                        <td align="center">
                                            <div style="width: 100%; height: 200px; overflow: auto">
                                                <asp:GridView ID="gv_cus" Width="100%" runat="server" AutoGenerateColumns="False">
                                                    <HeaderStyle CssClass="gvheaders" />
                                                    <AlternatingRowStyle CssClass="gvalts" />
                                                    <Columns>
                                                        <asp:BoundField DataField="Item" HeaderText="ITEM" />
                                                        <asp:BoundField DataField="process_name" HeaderText="PROCESS" />
                                                        <asp:BoundField DataField="input_item" HeaderText="INPUT ITEM" />
                                                        <asp:BoundField DataField="INPUT_QTY" HeaderText="INPUT QTY" />
                                                        <asp:BoundField DataField="INPUT_LOSS" HeaderText="INPUT LOSS" />
                                                        <asp:BoundField DataField="INPUT_RATE" HeaderText="INPUT RATE" />
                                                        <asp:BoundField DataField="output_item" HeaderText="OUTPUT ITEM" />
                                                        <asp:BoundField DataField="OUTPUT_QNT" HeaderText="OUTPUT QTY" />
                                                        <asp:BoundField DataField="OUTPUT_RATE" HeaderText="OUTPUT RATE" />
                                                    </Columns>
                                                    <EmptyDataRowStyle CssClass="gvemptytext" />
                                                </asp:GridView>
                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="right">
                                            <asp:Button CssClass="buttonnorm" ID="BtnOrderDetailWithConsumption" Text="OrderDetail With Consumption"
                                                runat="server" OnClick="BtnOrderDetailWithConsumption_Click" />
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
