<%@ Page Language="C#" AutoEventWireup="true" CodeFile="FrmInvoiceNewByGrid.aspx.cs"
    Inherits="Masters_Packing_FrmInvoiceNewByGrid" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register Src="~/UserControls/ucmenu.ascx" TagName="ucmenu" TagPrefix="uc1" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <link href="../../Styles/vijay.css" rel="stylesheet" type="text/css" />
    <script src="../../Scripts/JScript.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery-1.4.1.min.js" type="text/javascript"></script>
    <script type="text/javascript">
        function getNum(val) {
            if (isNaN(val)) {
                return 0;
            }
            return val;
        }
        function keypress() {

            var cmb = 0;
            var TxtBales = getNum(parseFloat($("#TxtBales").val()));
            if (TxtBales <= 0) {
                alert("Please enter No. of bales!!");
                this.value = "";
                return false;
            }
            //Assign the total to label
            //.toFixed() method will roundoff the final sum to 2 decimal places

            var L = getNum(parseFloat($("#txtlengthBale").val()));
            var W = getNum(parseFloat($("#txtwidthBale").val()));
            var H = getNum(parseFloat($("#txtheightbale").val()));
            var cbm = ((L * W * H / parseFloat(1000000))) * TxtBales;
            cbm = cbm == "Infinity" ? 0 : cbm;
            document.getElementById("txtcbmbale").value = (getNum(parseFloat(cbm)).toFixed(2));
        }
        function NewForm() {
            window.location.href = "FrmInvoiceNewByGrid.aspx";
        }
        function priview() {
            window.open('../../ReportViewer.aspx');
        }
        function CloseForm() {
            window.location.href = "../../main.aspx";
        }
        function AddCollection() {
            var answer = confirm("Do you want to ADD?")
            if (answer) {

                var left = (screen.width / 2) - (500 / 2);
                var top = (screen.height / 2) - (370 / 2);

                window.open('FrmAddCollection.aspx', 'ADD collection', 'width=500px, height=370px, top=' + top + ', left=' + left);
                //window.open('FrmAddCollection.aspx', '', 'width=950px,Height=500px');
            }
        }
      
    </script>
    <style type="text/css">
        .style1
        {
            width: 238px;
        }
        .style2
        {
            width: 218px;
        }
    </style>
</head>
<body>
    <script type="text/javascript">
        function CheckOne(obj) {
            var isValid = false;
            var j = 0;
            var gridView = document.getElementById("<%=GVPackingOrderDetail.ClientID %>");
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
    
    </script>
    <form id="form1" runat="server">
    <table width="100%" border="1">
        <tr style="width: 100%" align="center">
            <td height="66px" align="center">
                <%--style="background-image:url(Images/header.jpg)" --%>
                <%--<div><img src="Images/header.jpg" alt="" /></div>--%>
                <asp:Image ID="Image1" ImageUrl="~/Images/header.jpg" runat="server" />
            </td>
            <td style="background-color: #0080C0;" width="100px" valign="bottom">
                <asp:Image ID="imgLogo" align="left" runat="server" Height="66px" Width="111px" />
                <asp:Label ID="LblCompanyName" runat="server" Text="" ForeColor="White" Font-Bold="true"
                    CssClass="labelbold" Style="font-style: italic" Font-Size="Small"></asp:Label>
                <br />
                <i><font size="2" face="GEORGIA">
                    <asp:Label ID="LblUserName" ForeColor="White" runat="server" Text=""></asp:Label></font>
                </i>
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
                            Text="Logout" OnClick="BtnLogout_Click" Style="cursor: pointer; text-decoration: underline;
                            font-weight: bold" />
                    </ContentTemplate>
                </asp:UpdatePanel>
            </td>
        </tr>
        <tr>
            <td height="inherit" valign="top" class="style1" colspan="2">
                <div>
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                        <ContentTemplate>
                            <table>
                                <tr id="TRSearchInvoiceNo" runat="server" visible="false">
                                    <td colspan="2">
                                        <asp:TextBox ID="TxtSearchInvoiceNo" CssClass="textb" Width="240px" placeholder="Type here Invoice No. to Search"
                                            runat="server" AutoPostBack="true" OnTextChanged="TxtSearchInvoiceNo_TextChanged" />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdstyle">
                                        <asp:Label ID="Label1" runat="server" Text="Consignor" CssClass="labelbold"></asp:Label>
                                        <br />
                                        <asp:DropDownList ID="DDCompanyName" runat="server" Width="150px" CssClass="dropdown">
                                        </asp:DropDownList>
                                        <cc1:ListSearchExtender ID="ListSearchExtender4" runat="server" TargetControlID="DDCompanyName"
                                            ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                                        </cc1:ListSearchExtender>
                                    </td>
                                    <td class="tdstyle">
                                        <asp:Label ID="Label2" class="tdstyle" runat="server" Text=" Customer Code" CssClass="labelbold"></asp:Label>
                                        <br />
                                        <asp:DropDownList ID="DDCustomerCode" runat="server" Width="150px" CssClass="dropdown"
                                            OnSelectedIndexChanged="DDCustomerCode_SelectedIndexChanged" AutoPostBack="True">
                                        </asp:DropDownList>
                                        &nbsp;<asp:Button ID="btnaddcontinent" runat="server" Text="&#43;" CssClass="buttonsmall"
                                            Style="margin-top: 0px" ToolTip="Click For Add New Collection" OnClientClick="return AddCollection();" />
                                        <cc1:ListSearchExtender ID="ListSearchExtender1" runat="server" TargetControlID="DDCustomerCode"
                                            ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                                        </cc1:ListSearchExtender>
                                    </td>
                                    <td class="tdstyle">
                                        <asp:Label ID="Label3" class="tdstyle" runat="server" Text=" Consignee" CssClass="labelbold"></asp:Label>
                                        &nbsp;&nbsp;
                                        <asp:CheckBox ID="ChkForEdit" runat="server" CssClass="checkboxbold" Text="For Edit"
                                            AutoPostBack="True" OnCheckedChanged="ChkForEdit_CheckedChanged" />
                                        <br />
                                        <asp:DropDownList ID="DDConsignee" runat="server" Width="150px" CssClass="dropdown">
                                        </asp:DropDownList>
                                        <cc1:ListSearchExtender ID="ListSearchExtender2" runat="server" TargetControlID="DDConsignee"
                                            ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                                        </cc1:ListSearchExtender>
                                    </td>
                                    <td id="TDDDInvoiceNo" runat="server" visible="false" class="tdstyle">
                                        <asp:Label ID="Label4" class="tdstyle" runat="server" Text="   Invoice No" CssClass="labelbold"></asp:Label>
                                        <br />
                                        <asp:DropDownList ID="ddInvoiceNo" runat="server" Width="100px" CssClass="dropdown"
                                            AutoPostBack="True" OnSelectedIndexChanged="ddInvoiceNo_SelectedIndexChanged">
                                        </asp:DropDownList>
                                        <cc1:ListSearchExtender ID="ListSearchExtender3" runat="server" TargetControlID="ddInvoiceNo"
                                            ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                                        </cc1:ListSearchExtender>
                                    </td>
                                    <td class="tdstyle">
                                        <asp:Label ID="Label5" class="tdstyle" ForeColor="Red" runat="server" Text="   Invoice No"
                                            CssClass="labelbold"></asp:Label>
                                        <br />
                                        <asp:TextBox ID="TxtInvoiceNo" runat="server" Width="100px" CssClass="textb" AutoPostBack="True" Enabled="false"
                                            OnTextChanged="TxtInvoiceNo_TextChanged"></asp:TextBox>
                                    </td>
                                    <td colspan="2" class="tdstyle">
                                        <asp:Label ID="Label6" class="tdstyle" runat="server" Text="   Currency" CssClass="labelbold"></asp:Label>
                                        <br />
                                        <asp:DropDownList ID="DDCurrency" runat="server" Width="100px" CssClass="dropdown"
                                            AutoPostBack="True">
                                        </asp:DropDownList>
                                        <cc1:ListSearchExtender ID="ListSearchExtender5" runat="server" TargetControlID="DDCurrency"
                                            ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                                        </cc1:ListSearchExtender>
                                    </td>
                                    <td class="tdstyle">
                                        <asp:Label ID="Label7" class="tdstyle" runat="server" Text="Unit" CssClass="labelbold"></asp:Label>
                                        <br />
                                        <asp:DropDownList ID="DDUnit" runat="server" Width="100px" CssClass="dropdown" AutoPostBack="True"
                                            OnSelectedIndexChanged="DDUnit_SelectedIndexChanged">
                                        </asp:DropDownList>
                                        <cc1:ListSearchExtender ID="ListSearchExtender6" runat="server" TargetControlID="DDUnit"
                                            ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                                        </cc1:ListSearchExtender>
                                    </td>
                                    <td class="tdstyle">
                                        <asp:Label ID="Label8" class="tdstyle" runat="server" Text=" Invoice Date" CssClass="labelbold"></asp:Label>
                                        <br />
                                        <asp:TextBox ID="TxtDate" runat="server" Width="90px" CssClass="textb"></asp:TextBox>
                                        <asp:CalendarExtender ID="CalendarExtender1" runat="server" Format="dd-MMM-yyyy"
                                            TargetControlID="TxtDate">
                                        </asp:CalendarExtender>
                                    </td>
                                    <%-- <td id ="TDProdCode" runat="server" visible="false" class="tdstyle">
     Prod Code<br />
         <asp:TextBox ID="TxtProdCode" runat="server" Width="75px"  CssClass="textb" AutoPostBack="True"></asp:TextBox></td>--%>
                                </tr>
                                <tr>
                                    <td class="tdstyle">
                                        <asp:RadioButton ID="RDAreaWise" runat="server" Text="Area Wise" GroupName="OrderType"
                                            CssClass="radiobuttonnormal" />
                                    </td>
                                    <td class="tdstyle">
                                        <asp:RadioButton ID="RDPcsWise" runat="server" Text="Pcs Wise" Font-Bold="True" GroupName="OrderType"
                                            CssClass="radiobuttonnormal" />
                                    </td>  
                                    
                                </tr>
                            </table>
                            <table>
                                <tr>                                    
                                    <td class="tdstyle">
                                        <asp:Label ID="Label16" class="tdstyle" runat="server" Text="  Customer Order No"
                                            CssClass="labelbold"></asp:Label>
                                        <br />
                                        <asp:DropDownList ID="DDCustomerOrderNo" runat="server" Width="175px" CssClass="dropdown"
                                            OnSelectedIndexChanged="DDCustomerOrderNo_SelectedIndexChanged" AutoPostBack="True">
                                        </asp:DropDownList>
                                        <cc1:ListSearchExtender ID="ListSearchExtender7" runat="server" TargetControlID="DDCustomerOrderNo"
                                            ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                                        </cc1:ListSearchExtender>
                                    </td>
                                    <%--</tr>
     <tr>--%>
                                    <td id="TDProdCode" runat="server" visible="false" class="tdstyle">
                                        <asp:Label ID="Label17" class="tdstyle" runat="server" Text="  Prod Code" CssClass="labelbold"></asp:Label>
                                        <br />
                                        <asp:TextBox ID="TxtProdCode" runat="server" Width="75px" CssClass="textb" AutoPostBack="True"
                                            OnTextChanged="TxtProdCode_TextChanged"></asp:TextBox>
                                    </td>


                                     <td class="tdstyle">
                                            <asp:Label ID="Label9" runat="server" Text="BILLED TO" CssClass="labelbold"></asp:Label>
                                            <br />
                                            <asp:TextBox ID="txtBilledTo" runat="server"  CssClass="textb" TextMode="MultiLine" Width="250px"  Height="100px"
                                             onkeydown="return (event.keyCode!=13);"></asp:TextBox>
                                        </td>

                                         <td class="tdstyle">
                                            <asp:Label ID="Label10" runat="server" Text="SHIPPED TO" CssClass="labelbold"></asp:Label>
                                            <br />
                                            <asp:TextBox ID="txtShippedTo" runat="server"  CssClass="textb" TextMode="MultiLine" Width="250px" Height="100px"
                                             onkeydown="return (event.keyCode!=13);"></asp:TextBox>
                                        </td>

                                </tr>                              
                            </table>                            
                            <table>
                                <tr>
                                    <td>
                                        <div style="width: 900px; max-height: 300px; overflow: scroll">
                                            <asp:GridView ID="GVPackingOrderDetail" runat="server" AutoGenerateColumns="False"
                                                CssClass="grid-views" AllowPaging="true" PageSize="50" OnPageIndexChanging="GVPackingOrderDetail_PageIndexChanging"
                                                OnSorting="GVPackingOrderDetail_Sorting" AllowSorting="true">
                                                <HeaderStyle CssClass="gvheaders" />
                                                <AlternatingRowStyle CssClass="gvalts" />
                                                <RowStyle CssClass="gvrow" />
                                                <PagerStyle CssClass="PagerStyle" />
                                                <Columns>
                                                    <asp:TemplateField HeaderText="">
                                                        <%--<HeaderTemplate>
                                                    <asp:CheckBox ID="ChkAllItem" runat="server" onclick="return CheckAll(this);" />
                                                </HeaderTemplate>--%>
                                                        <ItemTemplate>
                                                            <asp:CheckBox ID="Chkboxitem" runat="server" OnCheckedChanged="Chkboxitem_CheckedChanged"
                                                                onclick="CheckOne(this)" AutoPostBack="true" Width="10px" />
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Center" Width="70px" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="ITEMNAME">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblitemname" runat="server" Text='<%#Bind("Item_Name") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <HeaderStyle Font-Bold="True" HorizontalAlign="Center" />
                                                        <ItemStyle HorizontalAlign="Center" Width="150px" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="QUALITY">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblQualityName" runat="server" Text='<%#Bind("QualityName") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <HeaderStyle Font-Bold="True" HorizontalAlign="Center" />
                                                        <ItemStyle HorizontalAlign="Center" />
                                                        <ItemStyle HorizontalAlign="Center" Width="250px" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="DESIGN">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblDesignName" runat="server" Text='<%#Bind("DesignName") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <HeaderStyle Font-Bold="True" HorizontalAlign="Center" />
                                                        <ItemStyle HorizontalAlign="Center" />
                                                        <ItemStyle HorizontalAlign="Center" Width="250px" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="COLOR">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblcolorname" runat="server" Text='<%#Bind("ColorName") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <HeaderStyle Font-Bold="True" HorizontalAlign="Center" />
                                                        <ItemStyle HorizontalAlign="Center" />
                                                        <ItemStyle HorizontalAlign="Center" Width="250px" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Shape">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblshapename" runat="server" Text='<%#Bind("shapename") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <HeaderStyle Font-Bold="True" HorizontalAlign="Center" />
                                                        <ItemStyle HorizontalAlign="Center" />
                                                        <ItemStyle HorizontalAlign="Center" Width="150px" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="SIZE">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblSIZEname" runat="server" Text='<%#Bind("Size") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <HeaderStyle Font-Bold="True" HorizontalAlign="Center" />
                                                        <ItemStyle HorizontalAlign="Center" />
                                                        <ItemStyle HorizontalAlign="Center" Width="150px" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Order Qty">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblOrderQty" runat="server" Text='<%#Bind("OrderQty") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <HeaderStyle Font-Bold="True" HorizontalAlign="Center" />
                                                        <ItemStyle HorizontalAlign="Center" Width="150px" />
                                                    </asp:TemplateField>   
                                                    
                                                     <asp:TemplateField HeaderText="Packed Qty">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblPackQty" runat="server" Text='<%#Bind("PackQty") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <HeaderStyle Font-Bold="True" HorizontalAlign="Center" />
                                                        <ItemStyle HorizontalAlign="Center" Width="150px" />
                                                    </asp:TemplateField> 

                                                     <asp:TemplateField HeaderText="Invoice Qty">
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="TxtInvoiceQty" runat="server" Width="50px" Text='<%#Bind("BalanceQty") %>'></asp:TextBox>                                                            
                                                        </ItemTemplate>
                                                        <HeaderStyle Font-Bold="True" HorizontalAlign="Center" />
                                                        <ItemStyle HorizontalAlign="Center" Width="150px" />
                                                    </asp:TemplateField>
                                                                                                     
                                                    <asp:TemplateField Visible="false">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblOrderId" Text='<%#Bind("OrderID") %>' runat="server" Width="50px" />
                                                            <asp:Label ID="lblCustomerOrderNo" Text='<%#Bind("CustomerOrderNo") %>' runat="server"
                                                                Width="50px" />
                                                            <asp:Label ID="lblItemId" Text='<%#Bind("Item_Id") %>' runat="server" Width="50px" />
                                                            <asp:Label ID="lblQualityId" Text='<%#Bind("QualityId") %>' runat="server" Width="50px" />
                                                            <asp:Label ID="lblDesignId" Text='<%#Bind("DesignId") %>' runat="server" Width="50px" />
                                                            <asp:Label ID="lblColorId" Text='<%#Bind("ColorId") %>' runat="server" Width="50px" />
                                                            <asp:Label ID="lblShapeId" Text='<%#Bind("ShapeId") %>' runat="server" Width="50px" />
                                                            <asp:Label ID="lblSizeId" Text='<%#Bind("SizeId") %>' runat="server" Width="50px" />
                                                            <asp:Label ID="lblShadeColorId" Text='<%#Bind("ShadeColorId") %>' runat="server"
                                                                Width="50px" />
                                                                 <asp:Label ID="lblInvoiceGenerateQty" Text='<%#Bind("InvoiceQty") %>' runat="server" Width="50px" />
                                                                  <asp:Label ID="lblOrderDetailId" Text='<%#Bind("OrderDetailId") %>' runat="server" Width="50px" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <%--<asp:CommandField ShowEditButton="True" />--%>
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
                                        <asp:Label ID="Label19" runat="server" Text="  Width" CssClass="labelbold"></asp:Label>
                                        <br />
                                        <asp:TextBox ID="TxtWidth" runat="server" Width="75px" CssClass="textb" OnTextChanged="TxtWidth_TextChanged" Enabled="false"
                                            AutoPostBack="True"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:Label ID="Label20" runat="server" Text=" Length" CssClass="labelbold"></asp:Label>
                                        <br />
                                        <asp:TextBox ID="TxtLength" runat="server" Width="75px" CssClass="textb" OnTextChanged="TxtLength_TextChanged" Enabled="false"
                                            AutoPostBack="True"></asp:TextBox>
                                    </td>
                                    <td class="tdstyle">
                                        <asp:Label ID="Label21" runat="server" Text="   Price" CssClass="labelbold"></asp:Label>
                                        <br />
                                        <asp:TextBox ID="TxtPrice" runat="server" Width="75px" CssClass="textb" onkeydown="return (event.keyCode!=13);"></asp:TextBox>
                                    </td>
                                    
                                    <td class="tdstyle">
                                        <asp:Label ID="Label22" runat="server" Text="  Area" CssClass="labelbold"></asp:Label>
                                        <br />
                                        <asp:TextBox ID="TxtArea" runat="server" Width="75px" CssClass="textb" onkeydown="return (event.keyCode!=13);" Enabled="false"></asp:TextBox>
                                    </td>
                                    <td class="tdstyle">
                                        <asp:Label ID="Label23" runat="server" Text="   Total Qty" CssClass="labelbold"></asp:Label>
                                        <br />
                                        <asp:TextBox ID="TxtTotalQty" runat="server" Width="75px" CssClass="textb" ReadOnly="True"
                                            onkeydown="return (event.keyCode!=13);"></asp:TextBox>
                                    </td>
                                    <td class="tdstyle">
                                        <asp:Label ID="Label24" runat="server" Text="   Pre Invoice Qty" CssClass="labelbold"></asp:Label>
                                        <br />
                                        <asp:TextBox ID="TxtPreInvoiceQty" runat="server" Width="75px" CssClass="textb" ReadOnly="True"
                                            onkeydown="return (event.keyCode!=13);"></asp:TextBox>
                                    </td>
                                   <%-- <td class="tdstyle">
                                        <asp:Label ID="Label25" runat="server" Text="  Invoice Qty" CssClass="labelbold"></asp:Label>
                                        <br />
                                        <asp:TextBox ID="TxtInvoiceQty" runat="server" Width="75px" CssClass="textb" AutoPostBack="True"
                                            OnTextChanged="TxtInvoiceQty_TextChanged" Enabled="false"></asp:TextBox>
                                    </td>--%>
                                   
                                    <td class="tdstyle">
                                        <asp:Label ID="Label26" runat="server" Text="  Remarks" CssClass="labelbold"></asp:Label>
                                        <br />
                                        <asp:TextBox ID="TxtRemarks" runat="server" Width="350px" CssClass="textb" onkeydown="return (event.keyCode!=13);"></asp:TextBox>
                                    </td>                                   
                                </tr>
                            </table>
                            <table>
                                
                                <table>
                                    <tr>
                                        <td class="tdstyle">
                                            <asp:Label ID="Label40" runat="server" Text="CGST" CssClass="labelbold"></asp:Label>
                                            <br />
                                            <asp:TextBox ID="txtCGST" runat="server" Width="150px" CssClass="textb" onkeydown="return (event.keyCode!=13);"></asp:TextBox>
                                        </td>
                                        <td class="tdstyle">
                                            <asp:Label ID="Label41" runat="server" Text="SGST" CssClass="labelbold"></asp:Label>
                                            <br />
                                            <asp:TextBox ID="txtSGST" runat="server" Width="150px" CssClass="textb" onkeydown="return (event.keyCode!=13);"></asp:TextBox>
                                        </td>
                                        <td class="tdstyle">
                                            <asp:Label ID="Label11" runat="server" Text="IGST" CssClass="labelbold"></asp:Label>
                                            <br />
                                            <asp:TextBox ID="txtIGST" runat="server" Width="150px" CssClass="textb" onkeydown="return (event.keyCode!=13);"></asp:TextBox>
                                        </td>
                                        <td> <asp:Label ID="Label42" runat="server" Text="Vehicle No" CssClass="labelbold"></asp:Label>
                                            <br />
                                            <asp:TextBox ID="txtVehicleNo" runat="server" Width="150px" CssClass="textb" onkeydown="return (event.keyCode!=13);"></asp:TextBox></td>

                                        <td> <asp:Label ID="Label43" runat="server" Text="EWayBill No" CssClass="labelbold"></asp:Label>
                                            <br />
                                            <asp:TextBox ID="txtEWayBillBo" runat="server" Width="150px" CssClass="textb" onkeydown="return (event.keyCode!=13);"></asp:TextBox></td>
                                        <td>&nbsp;</td>
                                    </tr>
                                </table>
                                <table>
                                    <tr>
                                        <td>
                                            <asp:Label ID="LblErrorMessage" runat="server" Font-Size="Small" CssClass="labelbold"
                                                ForeColor="Red"></asp:Label>
                                        </td>
                                        <td colspan="2" align="right">
                                            <%--<asp:LinkButton ID="lnkchnginvoice" Text="Change Invoice No. & Date" CssClass="labelbold"
                                                ForeColor="Red" runat="server" OnClientClick="return confirm('Do you want to change Invoice No. & Date?')"
                                                OnClick="lnkchnginvoice_Click" />--%>
                                            <asp:Button ID="BtnSave" runat="server" Text="Save" OnClick="BtnSave_Click" OnClientClick="return confirm('Do you want to save data?')"
                                                CssClass="buttonnorm" />
                                            &nbsp;<asp:Button ID="BtnNew" runat="server" Text="New" OnClientClick="return NewForm();"
                                                CssClass="buttonnorm" />
                                                  &nbsp; <asp:Button ID="BtnPreview" runat="server" Text="Preview" OnClick="BtnPreview_Click" CssClass="buttonnorm" Visible="true" />                                               
                                            &nbsp;<asp:Button ID="BtnClose" runat="server" Text="Close" OnClientClick="return CloseForm();"
                                                CssClass="buttonnorm" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">
                                            <div style="width: 800px; max-height: 300px; overflow: scroll">
                                                <asp:GridView ID="DGOrderDetail" runat="server" DataKeyNames="InvoiceDetailId" AutoGenerateColumns="False"
                                                    CssClass="grid-views" OnRowDataBound="DGOrderDetail_RowDataBound" OnRowDeleting="DGOrderDetail_RowDeleting"
                                                    OnRowCancelingEdit="DGOrderDetail_RowCancelingEdit" OnRowEditing="DGOrderDetail_RowEditing"
                                                    OnRowUpdating="DGOrderDetail_RowUpdating" AllowPaging="true" PageSize="50" OnPageIndexChanging="DGOrderDetail_PageIndexChanging">
                                                    <HeaderStyle CssClass="gvheaders" />
                                                    <AlternatingRowStyle CssClass="gvalts" />
                                                    <RowStyle CssClass="gvrow" />
                                                    <PagerStyle CssClass="PagerStyle" />
                                                    <Columns>
                                                        <asp:TemplateField HeaderText="Rate" Visible="false">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblInvoiceId" runat="server" Visible="false" Text='<%# Bind("InvoiceId") %>' />
                                                                <asp:Label ID="lblInvoiceDetailId" runat="server" Visible="false" Text='<%# Bind("InvoiceDetailId") %>' />
                                                            </ItemTemplate>
                                                            <HeaderStyle Width="80px" />
                                                        </asp:TemplateField>                                                       
                                                        <asp:TemplateField HeaderText="ITEMNAME">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblitemname" runat="server" Text='<%#Bind("ITEMNAME") %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <HeaderStyle Font-Bold="True" HorizontalAlign="Center" />
                                                            <ItemStyle HorizontalAlign="Center" Width="150px" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="QUALITY">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblqulaityname" runat="server" Text='<%#Bind("Quality") %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <HeaderStyle Font-Bold="True" HorizontalAlign="Center" />
                                                            <ItemStyle HorizontalAlign="Center" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="DESIGN">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblDESIGNname" runat="server" Text='<%#Bind("Design") %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <HeaderStyle Font-Bold="True" HorizontalAlign="Center" />
                                                            <ItemStyle HorizontalAlign="Center" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="COLOR">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblcolorname" runat="server" Text='<%#Bind("Color") %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <HeaderStyle Font-Bold="True" HorizontalAlign="Center" />
                                                            <ItemStyle HorizontalAlign="Center" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Shape">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblshapename" runat="server" Text='<%#Bind("shapename") %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <HeaderStyle Font-Bold="True" HorizontalAlign="Center" />
                                                            <ItemStyle HorizontalAlign="Center" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="SIZE">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblSIZEname" runat="server" Text='<%#Bind("Size") %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <HeaderStyle Font-Bold="True" HorizontalAlign="Center" />
                                                            <ItemStyle HorizontalAlign="Center" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="QTY">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblQty" runat="server" Text='<%#Bind("QTY") %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <HeaderStyle Font-Bold="True" HorizontalAlign="Center" />
                                                            <ItemStyle HorizontalAlign="Center" Width="75px" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="RATE">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblrate" runat="server" Text='<%#Bind("RATE") %>'></asp:Label>
                                                            </ItemTemplate>
                                                          <EditItemTemplate>
                                                                <asp:TextBox ID="txtRate" runat="server" Text='<%#Bind("RATE") %>'></asp:TextBox>
                                                            </EditItemTemplate>
                                                            <HeaderStyle Font-Bold="True" HorizontalAlign="Center" />
                                                            <ItemStyle HorizontalAlign="Center" Width="75px" />
                                                        </asp:TemplateField>                                                       
                                                        <asp:TemplateField HeaderText="AREA">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblArea" runat="server" Text='<%#Bind("AREA") %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <HeaderStyle Font-Bold="True" HorizontalAlign="Center" />
                                                            <ItemStyle HorizontalAlign="Center" Width="80px" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="AMOUNT">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblamount" runat="server" Text='<%#Bind("AMOUNT") %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <HeaderStyle Font-Bold="True" HorizontalAlign="Center" />
                                                            <ItemStyle HorizontalAlign="Center" Width="75px" />
                                                        </asp:TemplateField>
                                                       
                                                        <asp:TemplateField HeaderText="ORDER NO.">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblorderno" runat="server" Text='<%#Bind("customerorderno") %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <HeaderStyle Font-Bold="True" HorizontalAlign="Center" />
                                                            <ItemStyle HorizontalAlign="Center" />
                                                        </asp:TemplateField>
                                                     
                                                        <asp:TemplateField>
                                                            <ItemTemplate>
                                                                <asp:LinkButton ID="lnkDel" Text="Delete" runat="server" OnClientClick="return confirm('Do you want to delete this row?');"
                                                                    CommandName="Delete"></asp:LinkButton>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:CommandField ShowEditButton="True" />
                                                    </Columns>
                                                    <EmptyDataRowStyle CssClass="gvemptytext" />
                                                </asp:GridView>
                                            </div>
                                        </td>
                                        
                                    </tr>
                                    
                                    
                                    
                                    <tr>
                                        <asp:HiddenField ID="hnInvoiceID" runat="server" />
                                        <asp:HiddenField ID="hnid" runat="server" />
                                        <asp:HiddenField ID="hnfinished" runat="server" />
                                    </tr>
                                </table>
                                <asp:HiddenField ID="hnsampletype" runat="server" Value="1" />
                        </ContentTemplate>
                        <Triggers>
                            <%--<asp:PostBackTrigger ControlID="btnshow" />--%>
                            <asp:PostBackTrigger ControlID="BtnPreview" />
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <div style="background-color: #edf3fe; text-align: center; height: 43px; width: 100%">
                    <span class="labelcopyr">&copy; Enablesoft Erp Consultancy Pvt. Ltd.All rights reserved.</span>
                </div>
            </td>
        </tr>
    </table>
    </form>
</body>
</html>
