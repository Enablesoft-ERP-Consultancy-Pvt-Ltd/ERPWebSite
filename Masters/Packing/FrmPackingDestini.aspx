<%@ Page Language="C#" AutoEventWireup="true" CodeFile="FrmPackingDestini.aspx.cs"
    EnableEventValidation="false" Inherits="Masters_Packing_FrmPackingDestini" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register Src="~/UserControls/ucmenu.ascx" TagName="ucmenu" TagPrefix="uc1" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <link rel="Stylesheet" type="text/css" href="../../App_Themes/Default/Style.css" />
    <script src="../../Scripts/JScript.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery-1.4.1.min.js" type="text/javascript"></script>
    <script type="text/javascript">
        function NewForm() {
            window.location.href = "FrmPackingDestini.aspx";
        }
        function priview() {
            window.open('../../ReportViewer.aspx');
        }
        function CloseForm() {
            window.location.href = "../../main.aspx";
        }
        function OPEN() {
            window.open('../../ViewReport.aspx', '');
        }
        function Validate() {

            if (document.getElementById('DDCustomerCode').value == "0") {
                alert("Please Select Customer Code! ");
                document.getElementById('DDCustomerCode').focus();
                return false;
            }
            else if (document.getElementById('DDConsignee').value == "0") {
                alert("Please Select Consignee!");
                document.getElementById('DDConsignee').focus();
                return false;
            }
           
            else if (document.getElementById('TxtInvoiceNo').value == "") {
                alert("InvoiceNo can not be blank!");
               return false;
            }
           
            else {
                return confirm('Do You Want To Save?')
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
            margin-left: 40px;
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
            <td width="75%">
                <uc1:ucmenu ID="ucmenu1" runat="server" />
                <asp:ScriptManager ID="ScriptManager1" runat="server">
                </asp:ScriptManager>
            </td>
            <td width="25%">
                <asp:UpdatePanel ID="up" runat="server">
                    <ContentTemplate>
                        <asp:Button Width="100%" BorderStyle="None" BackColor="#999999" ID="BtnLogout" runat="server"
                            Text="Logout" />
                    </ContentTemplate>
                </asp:UpdatePanel>
            </td>
        </tr>
        <tr>
            <td height="inherit" valign="top" class="style1" colspan="2">
                <div>
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                        <ContentTemplate>
                            <table width="80%">
                                <tr>
                                    <td class="tdstyle">
                                        <asp:CheckBox ID="ChkEditOrder" runat="server" Text="EDIT ORDER" AutoPostBack="True"
                                            OnCheckedChanged="ChkEditOrder_CheckedChanged" />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdstyle">
                                        Consignor
                                        <br />
                                        <asp:DropDownList ID="DDCompanyName" runat="server" Width="150px" CssClass="dropdown"
                                            TabIndex="1" OnSelectedIndexChanged="DDCompanyName_SelectedIndexChanged" AutoPostBack="True">
                                        </asp:DropDownList>
                                        <cc1:ListSearchExtender ID="ListSearchExtender4" runat="server" TargetControlID="DDCompanyName"
                                            ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                                        </cc1:ListSearchExtender>
                                    </td>
                                    <td class="tdstyle">
                                        Customer Code
                                        <br />
                                        <asp:DropDownList ID="DDCustomerCode" runat="server" Width="150px" CssClass="dropdown"
                                            OnSelectedIndexChanged="DDCustomerCode_SelectedIndexChanged" AutoPostBack="True"
                                            TabIndex="2">
                                        </asp:DropDownList>
                                        <cc1:ListSearchExtender ID="ListSearchExtender1" runat="server" TargetControlID="DDCustomerCode"
                                            ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                                        </cc1:ListSearchExtender>
                                    </td>
                                    <td class="tdstyle">
                                        Consignee
                                        <br />
                                        <asp:DropDownList ID="DDConsignee" runat="server" Width="150px" CssClass="dropdown"
                                            TabIndex="3" >
                                        </asp:DropDownList>
                                        <cc1:ListSearchExtender ID="ListSearchExtender2" runat="server" TargetControlID="DDConsignee"
                                            ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                                        </cc1:ListSearchExtender>
                                    </td>
                                    <td class="tdstyle">
                                        Invoice No<br />
                                        <asp:TextBox ID="TxtInvoiceNo" runat="server" Width="75px" CssClass="dropdown"
                                            TabIndex="4"></asp:TextBox>
                                    </td>
                                    <td colspan="2" class="tdstyle">
                                        Currency
                                        <br />
                                        <asp:DropDownList ID="DDCurrency" runat="server" Width="150px" CssClass="dropdown"
                                            TabIndex="5">
                                        </asp:DropDownList>
                                        <cc1:ListSearchExtender ID="ListSearchExtender3" runat="server" TargetControlID="DDCurrency"
                                            ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                                        </cc1:ListSearchExtender>
                                    </td>
                                    <td class="tdstyle">
                                        Unit
                                        <br />
                                        <asp:DropDownList ID="DDUnit" runat="server" Width="100px" CssClass="dropdown"
                                            AutoPostBack="True" OnSelectedIndexChanged="DDUnit_SelectedIndexChanged" TabIndex="6">
                                        </asp:DropDownList>
                                        <cc1:ListSearchExtender ID="ListSearchExtender5" runat="server" TargetControlID="DDUnit"
                                            ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                                        </cc1:ListSearchExtender>
                                    </td>
                                    <td class="tdstyle">
                                        Invoice Date
                                        <br />
                                        <asp:TextBox ID="TxtDate" runat="server" Width="90px" TabIndex="7"></asp:TextBox>
                                        <asp:CalendarExtender ID="CalendarExtender1" runat="server" Format="dd-MMM-yyyy"
                                            TargetControlID="TxtDate">
                                        </asp:CalendarExtender>
                                    </td>
                                    <td>
                                        <br />
                                        <asp:TextBox ID="TxtCompanyID" runat="server" Width="0px" Height="0px" ForeColor="White"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:RadioButton ID="RDAreaWise" class="tdstyle" runat="server" Text="Area Wise"
                                            Font-Bold="True" OnCheckedChanged="RDAreaWise_CheckedChanged" />
                                    </td>
                                    <td>
                                        <asp:RadioButton ID="RDPcsWise" class="tdstyle" runat="server" Text="Pcs Wise" Font-Bold="True"
                                            OnCheckedChanged="RDPcsWise_CheckedChanged" />
                                    </td>
                                    <td colspan="2">
                                        <asp:CheckBox ID="ChkForMulipleRolls" runat="server" Text="Check For Multiple Roll"
                                            Font-Bold="True" ForeColor="Blue" OnCheckedChanged="ChkForMulipleRolls_CheckedChanged"
                                            AutoPostBack="True" />
                                    </td>
                                    <td colspan="2">
                                        <asp:CheckBox ID="ChkForWithoutOrder" class="tdstyle" runat="server" Text="Check For Without Order"
                                            Font-Bold="True" ForeColor="Black" />
                                    </td>
                                    <td colspan="2">
                                        <asp:CheckBox ID="ChkForQtyWise" class="tdstyle" runat="server" Text="Check For Qty Wise"
                                            Font-Bold="True" ForeColor="Black" />
                                    </td>
                                </tr>
                                <tr>
                                    <td id="tdpcsperroll" runat="server" class="tdstyle">
                                        Pcs Per BOX<br />
                                        <asp:TextBox ID="TxtPcsPerRoll" runat="server" Width="75px" TabIndex="8" 
                                          AutoPostBack="true"  ontextchanged="TxtPcsPerRoll_TextChanged"></asp:TextBox>
                                    </td>
                                    <td id="tdtotalpcs" runat="server" class="tdstyle">
                                        Total Pcs<br />
                                        <asp:TextBox ID="TxtTotalPcs" runat="server" Width="75px" AutoPostBack="true" OnTextChanged="TxtTotalPcs_TextChanged"
                                            TabIndex="9"></asp:TextBox>
                                    </td>
                                    <td id="tdrollnoform" runat="server" class="tdstyle">
                                        Box&nbsp; No From<br />
                                        <asp:TextBox ID="TxtRollNoFrom" runat="server" Width="75px" OnTextChanged="TxtRollNoFrom_TextChanged"
                                            AutoPostBack="True" TabIndex="24"></asp:TextBox>
                                    </td>
                                    <td id="tdrollnobales" runat="server" class="tdstyle">
                                        Roll No Bales<br />
                                        <asp:TextBox ID="TxtBales" runat="server" Width="75px" ></asp:TextBox>
                                    </td>
                                    <td id="tdrollno" runat="server" class="tdstyle">
                                        Box&nbsp; No To<br />
                                        <asp:TextBox ID="TxtRollNoTo" runat="server" Width="75px" OnTextChanged="TxtRollNoTo_TextChanged"
                                            AutoPostBack="True" TabIndex="9"></asp:TextBox>
                                    </td>
                                    <td id="tdsrno" runat="server" class="tdstyle">
                                        Sr No<br />
                                        <asp:TextBox ID="TxtSrNo" runat="server" Width="75px"></asp:TextBox>
                                    </td>
                                    <td colspan="2" class="tdstyle">
                                        Customer Order No<br />
                                        <asp:DropDownList ID="DDCustomerOrderNo" runat="server" Width="150px" CssClass="dropdown"
                                            OnSelectedIndexChanged="DDCustomerOrderNo_SelectedIndexChanged" AutoPostBack="True"
                                            TabIndex="11">
                                        </asp:DropDownList>
                                        <cc1:ListSearchExtender ID="ListSearchExtender6" runat="server" TargetControlID="DDCustomerOrderNo"
                                            ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                                        </cc1:ListSearchExtender>
                                    </td>
                                    <td id="tdinvoice" runat="server" visible="false" class="tdstyle">
                                        Invoice No<br />
                                        <asp:DropDownList ID="ddinvoiceno" runat="server" Width="150px" CssClass="dropdown"
                                            AutoPostBack="True" TabIndex="11" OnSelectedIndexChanged="ddinvoiceno_SelectedIndexChanged">
                                        </asp:DropDownList>
                                        <cc1:ListSearchExtender ID="ListSearchExtender7" runat="server" TargetControlID="ddinvoiceno"
                                            ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                                        </cc1:ListSearchExtender>
                                    </td>
                                </tr>
                                <tr>
                                    <td id="TDProdCode" runat="server" visible="false" class="tdstyle">
                                        Prod Code<br />
                                        <asp:TextBox ID="TxtProdCode" runat="server" Width="100px" AutoPostBack="True" OnTextChanged="TxtProdCode_TextChanged"></asp:TextBox>
                                        <cc1:AutoCompleteExtender ID="AutoCompleteExtender1" runat="server" EnableCaching="true"
                                            Enabled="True" MinimumPrefixLength="1" ServiceMethod="GetQuality" TargetControlID="TxtProdCode"
                                            UseContextKey="True">
                                        </cc1:AutoCompleteExtender>
                                    </td>
                                    <td id="TDStockNo" runat="server" visible="false" class="tdstyle">
                                        Stock No<br />
                                        <asp:TextBox ID="TxtStockNo" runat="server" Width="100px" OnTextChanged="TxtStockNo_TextChanged"
                                            AutoPostBack="True"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblCategoryName" runat="server" Text="Category Name"></asp:Label>
                                        &nbsp;<br />
                                        <asp:DropDownList ID="ddCategoryName" runat="server" Width="150px" CssClass="dropdown"
                                            AutoPostBack="True" OnSelectedIndexChanged="ddCategoryName_SelectedIndexChanged"
                                            TabIndex="12">
                                        </asp:DropDownList>
                                        <cc1:ListSearchExtender ID="ListSearchExtender8" runat="server" TargetControlID="ddCategoryName"
                                            ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                                        </cc1:ListSearchExtender>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblItemName" class="tdstyle" runat="server" Text="Item Name"></asp:Label>
                                        <br />
                                        <asp:DropDownList ID="ddItemName" runat="server" Width="150px" CssClass="dropdown"
                                            AutoPostBack="True" OnSelectedIndexChanged="ddItemName_SelectedIndexChanged"
                                            TabIndex="13">
                                        </asp:DropDownList>
                                        <cc1:ListSearchExtender ID="ListSearchExtender9" runat="server" TargetControlID="ddItemName"
                                            ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                                        </cc1:ListSearchExtender>
                                    </td>
                                    <td id="TDQuality" runat="server" visible="false">
                                        <asp:Label ID="lblQualityName" class="tdstyle" runat="server" Text="Quality"></asp:Label>
                                        &nbsp;<br />
                                        <asp:DropDownList ID="ddQuality" runat="server" Width="150px" TabIndex="14" CssClass="dropdown"
                                            AutoPostBack="True" OnSelectedIndexChanged="ddQuality_SelectedIndexChanged">
                                        </asp:DropDownList>
                                        <cc1:ListSearchExtender ID="ListSearchExtender10" runat="server" TargetControlID="ddQuality"
                                            ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                                        </cc1:ListSearchExtender>
                                    </td>
                                    <td id="TDDesign" runat="server" visible="false" colspan="2">
                                        <asp:Label ID="lblDesignName" class="tdstyle" runat="server" Text="Design"></asp:Label>
                                        <br />
                                        <asp:DropDownList ID="ddDesign" runat="server" Width="150px" TabIndex="15" CssClass="dropdown"
                                            AutoPostBack="True" OnSelectedIndexChanged="ddDesign_SelectedIndexChanged">
                                        </asp:DropDownList>
                                        <cc1:ListSearchExtender ID="ListSearchExtender11" runat="server" TargetControlID="ddDesign"
                                            ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                                        </cc1:ListSearchExtender>
                                    </td>
                                    <td id="TDColor" runat="server" visible="false">
                                        <asp:Label ID="lblColorName" class="tdstyle" runat="server" Text="Color"></asp:Label>
                                        <br />
                                        <asp:DropDownList ID="ddColor" runat="server" Width="100px" TabIndex="16" CssClass="dropdown"
                                            AutoPostBack="True" OnSelectedIndexChanged="ddColor_SelectedIndexChanged">
                                        </asp:DropDownList>
                                        <cc1:ListSearchExtender ID="ListSearchExtender12" runat="server" TargetControlID="ddColor"
                                            ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                                        </cc1:ListSearchExtender>
                                    </td>
                                    <td id="TDShape" runat="server" visible="false">
                                        <asp:Label ID="lblShapeName" class="tdstyle" runat="server" Text="Shape"></asp:Label>
                                        <br />
                                        <asp:DropDownList ID="ddShape" runat="server" Width="100px" AutoPostBack="True" CssClass="dropdown"
                                            OnSelectedIndexChanged="ddShape_SelectedIndexChanged" TabIndex="17">
                                        </asp:DropDownList>
                                        <cc1:ListSearchExtender ID="ListSearchExtender13" runat="server" TargetControlID="ddShape"
                                            ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                                        </cc1:ListSearchExtender>
                                    </td>
                                    <td id="TDSize" runat="server" visible="false">
                                        <asp:Label ID="lblSizeName" class="tdstyle" runat="server" Text="Size"></asp:Label>
                                        <br />
                                        <asp:DropDownList ID="ddSize" runat="server" Width="100px" TabIndex="18" CssClass="dropdown"
                                            AutoPostBack="True" OnSelectedIndexChanged="ddSize_SelectedIndexChanged">
                                        </asp:DropDownList>
                                        <cc1:ListSearchExtender ID="ListSearchExtender14" runat="server" TargetControlID="ddSize"
                                            ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                                        </cc1:ListSearchExtender>
                                    </td>
                                    <td id="TDShade" runat="server" visible="false">
                                        <asp:Label ID="lblShade" class="tdstyle" runat="server" Text="Shade"></asp:Label>
                                        <br />
                                        <asp:DropDownList ID="ddShade" runat="server" Width="100px" CssClass="dropdown"
                                            OnSelectedIndexChanged="ddShade_SelectedIndexChanged" AutoPostBack="True">
                                        </asp:DropDownList>
                                        <cc1:ListSearchExtender ID="ListSearchExtender15" runat="server" TargetControlID="ddShade"
                                            ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                                        </cc1:ListSearchExtender>
                                    </td>
                                    <td id="TDFinishType" runat="server" visible="false">
                                        <asp:Label ID="LblFinishType" class="tdstyle" runat="server" Text="Finish Type"></asp:Label>
                                        <br />
                                        <asp:DropDownList ID="DDFinishType" runat="server" Width="100px" CssClass="dropdown">
                                        </asp:DropDownList>
                                        <cc1:ListSearchExtender ID="ListSearchExtender16" runat="server" TargetControlID="DDFinishType"
                                            ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                                        </cc1:ListSearchExtender>
                                    </td>
                                </tr>
                                <tr>
                                    <td id="TDWidth" runat="server" visible="false" class="tdstyle">
                                        Width<br />
                                        <asp:TextBox ID="TxtWidth" runat="server" Width="75px" OnTextChanged="TxtWidth_TextChanged"
                                            AutoPostBack="True" TabIndex="19"></asp:TextBox>
                                    </td>
                                    <td id="TDLength" runat="server" visible="false" class="tdstyle">
                                        Length<br />
                                        <asp:TextBox ID="TxtLength" runat="server" Width="75px" OnTextChanged="TxtLength_TextChanged"
                                            AutoPostBack="True" TabIndex="20"></asp:TextBox>
                                    </td>
                                    <td class="tdstyle">
                                        Price<br />
                                        <asp:TextBox ID="TxtPrice" runat="server" Width="75px" TabIndex="21"></asp:TextBox>
                                    </td>
                                    <td id="TDArea" runat="server" visible="false" class="tdstyle">
                                        Area<br />
                                        <asp:TextBox ID="TxtArea" runat="server" Width="75px" TabIndex="22"></asp:TextBox>
                                    </td>
                                    <td class="tdstyle">
                                        Total Qty<br />
                                        <asp:TextBox ID="TxtTotalQty" runat="server" Width="75px" TabIndex="23"  ></asp:TextBox>
                                    </td>
                                    <td class="tdstyle">
                                        PrePack Qty<br />
                                        <asp:TextBox ID="TxtPrePackQty" runat="server" Width="75px" TabIndex="24"></asp:TextBox>
                                    </td>
                                    <td class="tdstyle">
                                        Pack Qty<br />
                                        <asp:TextBox ID="TxtPackQty" runat="server" Width="75px" TabIndex="25" 
                                         AutoPostBack="true"   ontextchanged="TxtPackQty_TextChanged"></asp:TextBox>
                                    </td>
                                    <td colspan="3" class="tdstyle">
                                        Remarks<br />
                                        <asp:TextBox ID="TxtRemarks" runat="server" Width="350px" TabIndex="26"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="4">
                                        <asp:Label ID="LblErrorMessage" runat="server" ForeColor="Red"></asp:Label>
                                    </td>
                                    <td align="right" class="tdstyle">
                                        Final Packing
                                        <asp:CheckBox ID="chkfinal" runat="server" />
                                    </td>
                                    <td>
                                        <asp:Button ID="BtnSave" runat="server" Text="Save" OnClick="BtnSave_Click" OnClientClick="return Validate();"
                                            TabIndex="27" CssClass="buttonnorm" />
                                    </td>
                                    <td id="tdprev" runat="server" visible="false">
                                        <asp:Button ID="BtnPreview" runat="server" Text="Preview" 
                                            TabIndex="28" CssClass="buttonnorm" onclick="BtnPreview_Click" />
                                    </td>
                                    <td colspan="2">
                                        <asp:Button ID="BtnNew" runat="server" Text="New" OnClientClick="return NewForm();" CssClass="buttonnorm" />
                                        <asp:Button ID="BtnClose" runat="server" CssClass="buttonnorm" OnClientClick="return CloseForm();"
                                            Text="Close" />
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="6">
                                        <div style="width: 100%; height: 200px; overflow: scroll">
                                            <asp:GridView ID="DGOrderDetail" Width="100%" runat="server" DataKeyNames="Sr_No"
                                                AutoGenerateColumns="False" OnRowDataBound="DGOrderDetail_RowDataBound" OnSelectedIndexChanged="DGOrderDetail_SelectedIndexChanged"
                                                CssClass="grid-view" OnRowCreated="DGOrderDetail_RowCreated" 
                                                onrowdeleting="DGOrderDetail_RowDeleting">
                                                <AlternatingRowStyle BackColor="White" />
                                                <Columns>
                                                    <%--<asp:BoundField DataField="Sr_No" HeaderText="Sr_No" />--%>
                                                    <asp:BoundField DataField="CATEGORY" HeaderText="CATEGORY">
                                                        <HeaderStyle Font-Bold="True" HorizontalAlign="Center" />
                                                        <ItemStyle HorizontalAlign="Center" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="ITEMNAME" HeaderText="ITEMNAME">
                                                        <HeaderStyle Font-Bold="True" HorizontalAlign="Center" />
                                                        <ItemStyle HorizontalAlign="Center" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="DESCRIPTION" HeaderText="DESCRIPTION">
                                                        <HeaderStyle Font-Bold="True" HorizontalAlign="Center" />
                                                        <ItemStyle HorizontalAlign="Center" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="QTY" HeaderText="QTY">
                                                        <HeaderStyle Font-Bold="True" HorizontalAlign="Center" />
                                                        <ItemStyle HorizontalAlign="Center" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="RATE" HeaderText="RATE">
                                                        <HeaderStyle Font-Bold="True" HorizontalAlign="Center" />
                                                        <ItemStyle HorizontalAlign="Center" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="AREA" HeaderText="AREA">
                                                        <HeaderStyle Font-Bold="True" HorizontalAlign="Center" />
                                                        <ItemStyle HorizontalAlign="Center" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="AMOUNT" HeaderText="AMOUNT">
                                                        <HeaderStyle Font-Bold="True" HorizontalAlign="Center" />
                                                        <ItemStyle HorizontalAlign="Center" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="StockNo" HeaderText="STOCKNO">
                                                        <HeaderStyle Font-Bold="True" HorizontalAlign="Center" />
                                                        <ItemStyle HorizontalAlign="Center" />
                                                    </asp:BoundField>
                                                    <asp:TemplateField Visible="false">
                                                <ItemTemplate>
                                                     <asp:LinkButton ID="LinkButton1" runat="server" CausesValidation="False" CommandName="Delete"
                                                Text="Del" OnClientClick="return confirm('Do you want to Delete data?')" CssClass="buttonnorm"></asp:LinkButton>
                                                </ItemTemplate>
                                                </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Weight" Visible="false">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblid" runat="server" Visible="false" Text='<%# Bind("Sr_No") %>' />
                                                        </ItemTemplate>
                                                        <HeaderStyle Width="80px" />
                                                    </asp:TemplateField>

                                                </Columns>
                                                <EmptyDataRowStyle CssClass="gvemptytext" />
                                            </asp:GridView>
                                        </div>
                                    </td>
                                    <td valign="top" id="idgrid" runat="server" colspan="2" align="right">
                                        <asp:GridView ID="DGSHOWDATA" runat="server" AllowPaging="true" AutoGenerateColumns="False"
                                            PageSize="5" CssClass="grid-view" OnRowCreated="DGSHOWDATA_RowCreated" 
                                            onrowdatabound="DGSHOWDATA_RowDataBound"
                                            onselectedindexchanged="DGSHOWDATA_SelectedIndexChanged">
                                            <PagerStyle CssClass="PagerStyle" />
                                            <Columns>
                                                <asp:BoundField DataField="PRODUCTCODE" HeaderText="PRODUCTCODE" />
                                            </Columns>
                                        </asp:GridView>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:HiddenField ID="hnid" runat="server" />
                                    </td>
                                </tr>
                            </table>
                        </ContentTemplate>
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
