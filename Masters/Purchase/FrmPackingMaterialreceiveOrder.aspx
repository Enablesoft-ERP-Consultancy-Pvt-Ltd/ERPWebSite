<%@ Page Language="C#" AutoEventWireup="true" CodeFile="FrmPackingMaterialreceiveOrder.aspx.cs"
    EnableEventValidation="false" Inherits="Masters_Purchase_FrmPackingMaterialreceiveOrder" %>

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
        window.location.href = "FrmPackingMaterialreceiveOrder.aspx";
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

    function CheckAllCheckBoxes() {
        if (document.getElementById('ChkForAllSelect').checked == true) {
            var gvcheck = document.getElementById('DGSHOWDATA');
            var i;
            for (i = 1; i < gvcheck.rows.length; i++) {
                var inputs = gvcheck.rows[i].getElementsByTagName('input');
                inputs[0].checked = true;
            }
        }
        else {
            var gvcheck = document.getElementById('DGSHOWDATA');
            var i;
            for (i = 1; i < gvcheck.rows.length; i++) {
                var inputs = gvcheck.rows[i].getElementsByTagName('input');
                inputs[0].checked = false;
            }
        }
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
        else if (document.getElementById('ddchalanno').value == "0") {
            alert("Please Select Challan!");
            document.getElementById('ddchalanno').focus();
            return false;
        }
        else if (document.getElementById('ddgodown').value == "0") {
            alert("Please Select Godown!");
            document.getElementById('ddgodown').focus();
            return false;
        }
        else if (document.getElementById('ChkEditOrder').checked == true) {

            if (document.getElementById('ddBillno').value == "0") {
                alert("Please select bill no.!");
                document.getElementById('ddBillno').focus();
                return false;
            }
        }
        else if (document.getElementById('ChkEditOrder').checked == false) {
            if (document.getElementById('txtBillno').value == "") {
                alert("Bill No can not be blank!");
                document.getElementById('txtBillno').focus();
                return false;
            }
        }


        else {
            return confirm('Do You Want To Save?')
        }

    }
</script>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <link href="../../App_Themes/Default/Style.css" rel="stylesheet" type="text/css" />
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <table width="100%">
            <tr>
                <td width="100%" colspan="2">
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
                </td>
            </tr>
            <tr>
                <td>
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                        <ContentTemplate>
                            <table width="60%">
                                <tr>
                                    <td>
                                        <asp:CheckBox ID="ChkEditOrder" runat="server" Text="EDIT ORDER" AutoPostBack="True"
                                            OnCheckedChanged="ChkEditOrder_CheckedChanged" />
                                    </td>
                                    <td id="td_ord" runat="server" class="tdstyle" visible="false">
                                        Order No :&nbsp;
                                        <asp:TextBox ID="TxtOrderId" runat="server" Width="90px" Height="20px"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr id="Tr1" runat="server">
                                    <td id="Td1" class="tdstyle">
                                        Company Name<br />
                                        <asp:DropDownList ID="ddCompName" runat="server" TabIndex="1" CssClass="dropdown">
                                        </asp:DropDownList>
                                        <cc1:ListSearchExtender ID="ListSearchExtender4" runat="server" TargetControlID="ddCompName"
                                            ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                                        </cc1:ListSearchExtender>
                                    </td>
                                    <td id="tdcustomer" runat="server" class="tdstyle">
                                        Customer Code<br />
                                        <asp:DropDownList ID="ddcustomercode" runat="server" CssClass="dropdown" AutoPostBack="True"
                                            OnSelectedIndexChanged="ddcustomercode_SelectedIndexChanged">
                                        </asp:DropDownList>
                                        <cc1:ListSearchExtender ID="ListSearchExtender1" runat="server" TargetControlID="ddcustomercode"
                                            ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                                        </cc1:ListSearchExtender>
                                    </td>
                                    <td id="tdorderno" runat="server" class="tdstyle">
                                        OrderNo.<br />
                                        <asp:DropDownList ID="ddorderno" runat="server" CssClass="dropdown" AutoPostBack="True"
                                            OnSelectedIndexChanged="ddorderno_SelectedIndexChanged">
                                        </asp:DropDownList>
                                        <cc1:ListSearchExtender ID="ListSearchExtender2" runat="server" TargetControlID="ddorderno"
                                            ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                                        </cc1:ListSearchExtender>
                                    </td>
                                    <td id="Td4" class="tdstyle">
                                        Party Name<br />
                                        <asp:DropDownList ID="ddempname" runat="server" CssClass="dropdown" AutoPostBack="True"
                                            OnSelectedIndexChanged="ddempname_SelectedIndexChanged">
                                        </asp:DropDownList>
                                        <cc1:ListSearchExtender ID="ListSearchExtender3" runat="server" TargetControlID="ddempname"
                                            ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                                        </cc1:ListSearchExtender>
                                    </td>
                                    <td id="tdchalanno" runat="server" class="tdstyle">
                                        Challan.No.<br />
                                        <asp:DropDownList ID="ddchalanno" runat="server" CssClass="dropdown" AutoPostBack="True"
                                            OnSelectedIndexChanged="ddchalanno_SelectedIndexChanged">
                                        </asp:DropDownList>
                                        <cc1:ListSearchExtender ID="ListSearchExtender5" runat="server" TargetControlID="ddchalanno"
                                            ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                                        </cc1:ListSearchExtender>
                                    </td>
                                    <td id="tdrec" runat="server" visible="false" class="tdstyle">
                                        Bill.No.<br />
                                        <asp:DropDownList ID="ddBillno" runat="server" CssClass="dropdown" AutoPostBack="True"
                                            OnSelectedIndexChanged="ddBillno_SelectedIndexChanged">
                                        </asp:DropDownList>
                                        <cc1:ListSearchExtender ID="ListSearchExtender6" runat="server" TargetControlID="ddBillno"
                                            ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                                        </cc1:ListSearchExtender>
                                    </td>
                                    <td id="tdbillno" runat="server" class="tdstyle">
                                        Bill.No.<br />
                                        <asp:TextBox ID="txtBillno" runat="server" Width="90px" TabIndex="5" AutoPostBack="True"
                                            CssClass="textb" OnTextChanged="txtBillno_TextChanged"></asp:TextBox>
                                        <asp:Label ID="lbl" runat="server" Text="*" ForeColor="Red"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td id="Td2" align="center" class="tdstyle">
                                        ReceiveDate<br />
                                        <asp:TextBox ID="txtdate" runat="server" Width="90px" TabIndex="5" AutoPostBack="True"
                                            CssClass="textb"></asp:TextBox>
                                        <asp:RequiredFieldValidator SetFocusOnError="true" ID="RequiredFieldValidator1" runat="server"
                                            ErrorMessage="please Enter Date" ControlToValidate="txtdate" ValidationGroup="f1"
                                            ForeColor="Red">*</asp:RequiredFieldValidator>
                                        <asp:CalendarExtender ID="CalendarExtender1" runat="server" Format="dd-MMM-yyyy"
                                            TargetControlID="txtdate">
                                        </asp:CalendarExtender>
                                    </td>
                                    <td id="Td6" class="tdstyle" runat="server" visible="false">
                                        Order No.<br />
                                        <asp:TextBox ID="txtchalanno" Width="70px" runat="server" CssClass="textb"></asp:TextBox>
                                    </td>
                                    <td>
                                        Godown </br>
                                        <asp:DropDownList ID="ddgodown" runat="server" AutoPostBack="true" CssClass="dropdown">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                            </table>
                            <%--   <table>
                                <tr>
                                <td>
                               
                                <table>
                               
                                <tr class="RowStyle">
                                     <td id="Td2" align="center" class="tdstyle">
                                        ReceiveDate<br />
                                        <asp:TextBox ID="txtdate" runat="server" Width="90px" TabIndex="5" AutoPostBack="True"
                                            CssClass="textb"></asp:TextBox>
                                        <asp:RequiredFieldValidator SetFocusOnError="true" ID="RequiredFieldValidator1" runat="server"
                                            ErrorMessage="please Enter Date" ControlToValidate="txtdate" ValidationGroup="f1"
                                            ForeColor="Red">*</asp:RequiredFieldValidator>
                                        <asp:CalendarExtender ID="CalendarExtender1" runat="server" Format="dd-MMM-yyyy"
                                            TargetControlID="txtdate">
                                        </asp:CalendarExtender>
                                    </td>
                                    <td id="Td3" runat="server" class="tdstyle" visible="true">
                                        Item Code<br />
                                        <asp:TextBox ID="txtItemCode" Width="100px" runat="server" CssClass="textb"></asp:TextBox>
                                    </td>
                                    <td class="tdstyle">
                                       Length<br />
                                        <asp:TextBox ID="txtlen" Width="100px" runat="server" CssClass="textb"  onkeypress="return isNumber(event);"></asp:TextBox>
                                    </td>
                                    <td  class="tdstyle">
                                       Height<br />
                                        <asp:TextBox ID="txtheight" Width="100px" runat="server" CssClass="textb"  onkeypress="return isNumber(event);"></asp:TextBox>
                                    </td>
                                    <td  class="tdstyle">
                                       Width<br />
                                        <asp:TextBox ID="txtwidth" Width="100px" runat="server" CssClass="textb"  onkeypress="return isNumber(event);"></asp:TextBox>
                                    </td>
                                    </tr>
                                    <tr class="RowStyle">
                                    <td  class="tdstyle">
                                       GSM<br />
                                        <asp:TextBox ID="txtgsm" Width="100px" runat="server" CssClass="textb"  onkeypress="return isNumber(event);"></asp:TextBox>
                                    </td>
                                    <td>
                                       GSM2<br />
                                        <asp:TextBox ID="txtgsm2" Width="100px" runat="server" CssClass="textb"  onkeypress="return isNumber(event);"></asp:TextBox>
                                    </td>
                                     <td>
                                       Ply<br />
                                        <asp:TextBox ID="txtply" Width="100px" runat="server" CssClass="textb"  onkeypress="return isNumber(event);"></asp:TextBox>
                                    </td>
                                    <td>
                                       Qty<br />
                                        <asp:TextBox ID="txtqty" Width="100px" runat="server" CssClass="textb"  onkeypress="return isNumber(event);"></asp:TextBox>
                                    </td>
                                    <td>
                                       Rate<br />
                                        <asp:TextBox ID="txtrate" Width="100px" runat="server" CssClass="textb"  onkeypress="return isNumber(event);"></asp:TextBox>
                                    </td>
                                    <tr class="RowStyle">
                                   
                                    <td>
                                       Pcs<br />
                                        <asp:TextBox ID="txtpcs" Width="100px" runat="server" CssClass="textb"  onkeypress="return isNumber(event);"></asp:TextBox>
                                    </td>
                                    <td>
                                       Weight<br />
                                        <asp:TextBox ID="txtweight" Width="100px" runat="server" CssClass="textb"  onkeypress="return isNumber(event);"></asp:TextBox>
                                    </td>
                                    <td>
                                       Amount<br />
                                        <asp:TextBox ID="Txtamount" Width="100px" runat="server" CssClass="textb"  onkeypress="return isNumber(event);"></asp:TextBox>
                                    </td>
                                    <td>
                                       Delivery Date<br />
                                        <asp:TextBox ID="txtdeldate" Width="100px" runat="server" CssClass="textb"  AutoPostBack="true"></asp:TextBox>
                                         <asp:CalendarExtender ID="CalendarExtender4" runat="server" Format="dd-MMM-yyyy"
                                            TargetControlID="txtdeldate">
                                        </asp:CalendarExtender>
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
                                     <tr class="RowStyle">
                            <td class="tdstyle">
                                Remarks
                            </td>
                            <td colspan="4">
                                <asp:TextBox ID="txtremarks" runat="server" ForeColor="Red" Height="25px" Width="100%" CssClass="textb"
                                    TextMode="MultiLine"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                                    <td align="right" colspan="5">
                                        <asp:Button ID="btnnew" runat="server" Text="New" CssClass="buttonnorm" OnClientClick="return NewForm();" />
                                        <asp:Button ID="btnsave" runat="server" Text="Save" TabIndex="18" CssClass="buttonnorm"
                                            OnClientClick="return confirm('Do You Want To Save?')" OnClick="btnsave_Click" />
                                        <asp:Button ID="btnpriview" runat="server" Text="Preview" Visible="false" OnClick="btnpriview_Click"
                                            CssClass="buttonnorm" />
                                        <asp:Button ID="BTNCLOSE" runat="server" Text="Close" OnClientClick="return CloseForm();"
                                            CssClass="buttonnorm" />
                                        <asp:Button ID="btndelete" runat="server" Text="Delete" Visible="false" CssClass="buttonnorm" />
                                    </td>
                                </tr>
                                </tr>
                                </table>
                                 </td>
                                   <td>
                        <div style="width: 100%; height:170px; overflow: scroll;">
                        <asp:GridView ID="GDVSHOWORDER" runat="server" AutoGenerateColumns="false" 
                                onrowcreated="GDVSHOWORDER_RowCreated"  CssClass="grid-view"
                                onrowdatabound="GDVSHOWORDER_RowDataBound" DataKeyNames="SrNo"
                                onselectedindexchanged="GDVSHOWORDER_SelectedIndexChanged">
                                 <HeaderStyle CssClass="gvheader" />
                                            <AlternatingRowStyle CssClass="gvalt" />
                                            <RowStyle CssClass="gvrow" />
                                            <EmptyDataRowStyle CssClass="gvemptytext" />
                                            <Columns>
                                              
                                               <asp:BoundField DataField="orderno" HeaderText="OrderNo" />
                                                <asp:BoundField DataField="ProductCode" HeaderText="Item" />
                                                 <asp:TemplateField Visible="false">
                                               <ItemTemplate>
                                                <asp:Label ID="Lbldetailid" runat="server" Text='<%# Bind("detailid") %>'></asp:Label>
                                               <asp:Label ID="lbllength" runat="server" Text='<%# Bind("Length") %>'></asp:Label>
                                               <asp:Label ID="lblwidth" runat="server" Text='<%# Bind("width") %>'></asp:Label>
                                               <asp:Label ID="lblHeight" runat="server" Text='<%# Bind("Height") %>'></asp:Label>
                                               <asp:Label ID="lblgsm" runat="server" Text='<%# Bind("gsm") %>'></asp:Label>
                                               <asp:Label ID="lblgsm2" runat="server" Text='<%# Bind("gsm2") %>'></asp:Label>
                                               <asp:Label ID="lblply" runat="server" Text='<%# Bind("ply") %>'></asp:Label>
                                               <asp:Label ID="lblunit" runat="server" Text='<%# Bind("weight") %>'></asp:Label>
                                               <asp:Label ID="Lblremark" runat="server" Text='<%# Bind("remark") %>'></asp:Label>
                                           
                                               <asp:Label ID="lblodate" runat="server" Text='<%# Bind("Recdate") %>'></asp:Label>
                                              
                                                
                                                                                          
                                               </ItemTemplate>
                                                </asp:TemplateField>
                                               <asp:BoundField DataField="PackingType" HeaderText="PackingType" />
                                                <asp:BoundField DataField="unit" HeaderText="Unit" />
                                                <asp:BoundField DataField="qty" HeaderText="Qty" />
                                                 <asp:BoundField DataField="pqty" HeaderText="PQty" />
                                               
                                                <asp:BoundField DataField="rate" HeaderText="Rate" />
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
                                </table>--%>
                            <table>
                                <tr>
                                    <td>
                                        <asp:Label ID="LblErrorMessage" runat="server" Font-Bold="true" ForeColor="RED" Text="ONE OR MORE MANDATORY FIELDS ARE MISSING......."
                                            Visible="false"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left">
                                        <asp:Button ID="btnnew" runat="server" Text="New" CssClass="buttonnorm" OnClientClick="return NewForm();" />
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
                                    <td id="selectall" runat="server" visible="false">
                                        <asp:CheckBox ID="ChkForAllSelect" runat="server" AutoPostBack="True" CssClass="checkboxnormal"
                                            ForeColor="Red" onclick="return CheckAllCheckBoxes();" Text="Select All" />
                                        <%--&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                        <asp:CheckBox ID="ChkForRawMaterial" runat="server" Text="For Raw Material" Checked="true"
                                            ForeColor="Red" CssClass="checkboxnormal" AutoPostBack="True" OnCheckedChanged="ChkForRawMaterial_CheckedChanged" />--%>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:GridView ID="DGSHOWDATA" runat="server" AutoGenerateColumns="False" CssClass="grid-view"
                                            OnRowCreated="DGSHOWDATA_RowCreated" DataKeyNames="packingprocessreceivedetailid"
                                            OnRowDeleting="DGSHOWDATA_RowDeleting">
                                            <HeaderStyle CssClass="gvheader" />
                                            <AlternatingRowStyle CssClass="gvalt" />
                                            <RowStyle CssClass="gvrow" />
                                            <EmptyDataRowStyle CssClass="gvemptytext" />
                                            <Columns>
                                                <asp:TemplateField HeaderText="">
                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="Chkbox" runat="server" />
                                                    </ItemTemplate>
                                                    <HeaderStyle Width="20px" />
                                                </asp:TemplateField>
                                                <%-- <asp:BoundField DataField="srno" HeaderText="SrNo" />--%>
                                                <asp:BoundField DataField="orderno" HeaderText="OrderNo" />
                                                <asp:BoundField DataField="ProductCode" HeaderText="Item Code" />
                                                <asp:TemplateField HeaderText="PackingType">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblpacktype" runat="server" Width="70px" Text='<%# Bind("PackingType") %>'
                                                            AutoPostBack="true"></asp:Label>
                                                    </ItemTemplate>
                                                    <HeaderStyle Width="80px" />
                                                </asp:TemplateField>
                                                <asp:BoundField DataField="Length" HeaderText="Length" />
                                                <asp:BoundField DataField="width" HeaderText="Width" />
                                                <asp:BoundField DataField="Height" HeaderText="Height" />
                                                <asp:BoundField DataField="gsm" HeaderText="GSM" />
                                                <asp:BoundField DataField="gsm2" HeaderText="GSM2" />
                                                <asp:BoundField DataField="ply" HeaderText="Ply" />
                                                <%--<asp:BoundField DataField="unit" HeaderText="Unit" />--%>
                                                <asp:TemplateField HeaderText="Qty">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblqty" runat="server" Text='<%# Bind("qty") %>' />
                                                    </ItemTemplate>
                                                    <HeaderStyle Width="80px" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="PQty">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="TXTQTY" runat="server" Width="70px" Text='<%# Bind("pQty") %>' AutoPostBack="true"
                                                            OnTextChanged="Txtqty_TextChanged"></asp:TextBox>
                                                    </ItemTemplate>
                                                    <HeaderStyle Width="80px" />
                                                </asp:TemplateField>
                                                <%--<asp:BoundField DataField="QTY" HeaderText="Qty" />--%>
                                                <%--<asp:BoundField DataField="rate" HeaderText="Rate" />--%>
                                                <asp:TemplateField HeaderText="Rate">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="TXTRAte" runat="server" Width="70px" Text='<%# Bind("Rate") %>'
                                                            AutoPostBack="true" Enabled="false" OnTextChanged="Txtrate_TextChanged"></asp:TextBox>
                                                        <asp:Label ID="lbldetailid" runat="server" Visible="false" Text='<%# Bind("detailid") %>' />
                                                        <asp:Label ID="LblpkgdetailID" runat="server" Visible="false" Text='<%# Bind("packingprocessreceivedetailid") %>' />
                                                    </ItemTemplate>
                                                    <HeaderStyle Width="80px" />
                                                </asp:TemplateField>
                                                <asp:BoundField DataField="amt" HeaderText="Amount" />
                                                <%--<asp:BoundField DataField="pcs" HeaderText="Pcs" />--%>
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
                                                <asp:TemplateField Visible="false">
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="LinkButton1" runat="server" CausesValidation="False" CommandName="Delete"
                                                            Text="Del" OnClientClick="return confirm('Do you want to Delete data?')" CssClass="buttonnorm"></asp:LinkButton>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
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
