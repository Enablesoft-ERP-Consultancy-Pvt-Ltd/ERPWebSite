<%@ Page Language="C#" AutoEventWireup="true" CodeFile="FrmPacking.aspx.cs" Inherits="Masters_Packing_FrmPacking" %>

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
            window.location.href = "FrmPacking.aspx";
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
                                        <asp:Label ID="Label5" class="tdstyle" runat="server" Text="   Invoice No" CssClass="labelbold"></asp:Label>
                                        <br />
                                        <asp:TextBox ID="TxtInvoiceNo" runat="server" Width="200px" CssClass="textb" AutoPostBack="True"
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
                                    <td colspan="2" class="tdstyle">
                                        <asp:CheckBox ID="ChkForMulipleRolls" runat="server" Text="Check For Muliple Roll"
                                            Font-Bold="True" ForeColor="Blue" OnCheckedChanged="ChkForMulipleRolls_CheckedChanged"
                                            AutoPostBack="True" CssClass="checkboxbold" />
                                    </td>
                                     <td colspan="2" class="tdstyle" id="TDCheckForWithoutStockNo" runat="server" visible="false">
                                        <asp:CheckBox ID="ChkForWithoutStockNo" runat="server" Text="Check For Without StockNo"
                                            Font-Bold="True" ForeColor="Black" CssClass="checkboxbold" />
                                    </td>
                                    <td colspan="2" class="tdstyle">
                                        <asp:CheckBox ID="ChkForWithoutOrder" runat="server" Text="Check For Without Order"
                                            Font-Bold="True" ForeColor="Black" OnCheckedChanged="ChkForWithoutOrder_CheckedChanged"
                                            AutoPostBack="True" CssClass="checkboxbold" />
                                    </td>
                                   
                                    <%--     <td colspan="2">
         <asp:CheckBox ID="ChkForQtyWise" runat="server" 
             Text ="Check For Qty Wise" Font-Bold="True" ForeColor="Black" AutoPostBack="True"/></td>--%>
                                    <td id="TDChkForChangeQDC" runat="server" visible="false" colspan="2">
                                        <asp:CheckBox ID="ChkForChangeQDC" runat="server" class="tdstyle" Text="Check For Change QDC"
                                            Font-Bold="True" CssClass="checkboxbold" ForeColor="Black" AutoPostBack="True"
                                            OnCheckedChanged="ChkForChangeQDC_CheckedChanged" />
                                    </td>
                                    <td>
                                        <asp:CheckBox ID="chksamplepack" runat="server" class="tdstyle" Text="Pack For Sample"
                                            Font-Bold="True" CssClass="checkboxbold" ForeColor="Black" />
                                    </td>
                                </tr>
                            </table>
                            <table>
                                <tr>
                                    <td class="tdstyle">
                                        <asp:Label ID="Label9" class="tdstyle" runat="server" Text=" Roll No From" CssClass="labelbold"></asp:Label>
                                        <br />
                                        <asp:TextBox ID="TxtRollNoFrom" runat="server" Width="75px" CssClass="textb" OnTextChanged="TxtRollNoFrom_TextChanged"
                                            AutoPostBack="True"></asp:TextBox>
                                    </td>
                                    <td class="tdstyle">
                                        <asp:Label ID="Label10" class="tdstyle" runat="server" Text="No OF Bales" CssClass="labelbold"></asp:Label>
                                        <br />
                                        <asp:TextBox ID="TxtBales" runat="server" Width="75px" CssClass="textb" onkeydown="return (event.keyCode!=13);"
                                            OnTextChanged="TxtBales_TextChanged" AutoPostBack="True"></asp:TextBox>
                                    </td>
                                    <td class="tdstyle">
                                        <asp:Label ID="Label11" class="tdstyle" runat="server" Text="Roll No To" CssClass="labelbold"></asp:Label>
                                        <br />
                                        <asp:TextBox ID="TxtRollNoTo" runat="server" Width="75px" CssClass="textb" OnTextChanged="TxtRollNoTo_TextChanged"
                                            AutoPostBack="True"></asp:TextBox>
                                    </td>
                                    <td class="tdstyle">
                                        <asp:Label ID="Label12" class="tdstyle" runat="server" Text=" Pcs Per Roll" CssClass="labelbold"></asp:Label>
                                        <br />
                                        <asp:TextBox ID="TxtPcsPerRoll" runat="server" Width="75px" CssClass="textb" OnTextChanged="TxtPcsPerRoll_TextChanged"
                                            AutoPostBack="True"></asp:TextBox>
                                    </td>
                                    <td class="tdstyle">
                                        <asp:Label ID="Label13" class="tdstyle" runat="server" Text=" Total Pcs" CssClass="labelbold"></asp:Label>
                                        <br />
                                        <asp:TextBox ID="TxtTotalPcs" runat="server" Width="75px" CssClass="textb" onkeydown="return (event.keyCode!=13);"></asp:TextBox>
                                    </td>
                                    <td class="tdstyle">
                                        <asp:Label ID="Label14" class="tdstyle" runat="server" Text=" Sr No" CssClass="labelbold"></asp:Label>
                                        <br />
                                        <asp:TextBox ID="TxtSrNo" runat="server" Width="75px" CssClass="textb" onkeydown="return (event.keyCode!=13);"></asp:TextBox>
                                    </td>
                                    <td id="TDStockNo" runat="server" class="tdstyle">
                                        <asp:Label ID="Label15" class="tdstyle" runat="server" Text=" Stock No" CssClass="labelbold"></asp:Label>
                                        <br />
                                        <asp:TextBox ID="TxtStockNo" runat="server" Width="100px" CssClass="textb" OnTextChanged="TxtStockNo_TextChanged"
                                            AutoPostBack="True"></asp:TextBox>
                                    </td>
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
                                </tr>
                                <tr id="Trstockmsg" runat="server" visible="false">
                                    <td colspan="9">
                                        <asp:Label ID="lblstockmsg" Text="" CssClass="labelbold" runat="server" ForeColor="Red"
                                            Font-Size="Small" Font-Bold="true" />
                                    </td>
                                </tr>
                            </table>
                            <table>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblCategoryName" class="tdstyle" runat="server" Text="Category Name"
                                            CssClass="labelbold"></asp:Label>
                                        &nbsp;<br />
                                        <asp:DropDownList ID="ddCategoryName" runat="server" Width="150px" CssClass="dropdown"
                                            AutoPostBack="True" OnSelectedIndexChanged="ddCategoryName_SelectedIndexChanged">
                                        </asp:DropDownList>
                                        <cc1:ListSearchExtender ID="ListSearchExtender16" runat="server" TargetControlID="ddCategoryName"
                                            ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                                        </cc1:ListSearchExtender>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblItemName" class="tdstyle" runat="server" Text="Item Name" CssClass="labelbold"></asp:Label>
                                        <br />
                                        <asp:DropDownList ID="ddItemName" runat="server" Width="150px" CssClass="dropdown"
                                            AutoPostBack="True" OnSelectedIndexChanged="ddItemName_SelectedIndexChanged">
                                        </asp:DropDownList>
                                        <cc1:ListSearchExtender ID="ListSearchExtender8" runat="server" TargetControlID="ddItemName"
                                            ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                                        </cc1:ListSearchExtender>
                                    </td>
                                    <td id="TDQuality" runat="server" visible="false">
                                        <asp:Label ID="lblQualityName" class="tdstyle" runat="server" Text="Quality" CssClass="labelbold"></asp:Label>
                                        &nbsp;<br />
                                        <asp:DropDownList ID="ddQuality" runat="server" Width="150px" CssClass="dropdown"
                                            AutoPostBack="True" OnSelectedIndexChanged="ddQuality_SelectedIndexChanged">
                                        </asp:DropDownList>
                                        <cc1:ListSearchExtender ID="ListSearchExtender9" runat="server" TargetControlID="ddQuality"
                                            ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                                        </cc1:ListSearchExtender>
                                    </td>
                                    <td id="TDDesign" runat="server" visible="false">
                                        <asp:Label ID="lblDesignName" class="tdstyle" runat="server" Text="Design" CssClass="labelbold"></asp:Label>
                                        <br />
                                        <asp:DropDownList ID="ddDesign" runat="server" Width="175px" CssClass="dropdown"
                                            AutoPostBack="True" OnSelectedIndexChanged="ddDesign_SelectedIndexChanged">
                                        </asp:DropDownList>
                                        <cc1:ListSearchExtender ID="ListSearchExtender10" runat="server" TargetControlID="ddDesign"
                                            ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                                        </cc1:ListSearchExtender>
                                    </td>
                                    <td id="TDColor" runat="server" visible="false">
                                        <asp:Label ID="lblColorName" class="tdstyle" runat="server" Text="Color" CssClass="labelbold"></asp:Label>
                                        <br />
                                        <asp:DropDownList ID="ddColor" runat="server" Width="100px" CssClass="dropdown" AutoPostBack="True"
                                            OnSelectedIndexChanged="ddColor_SelectedIndexChanged">
                                        </asp:DropDownList>
                                        <cc1:ListSearchExtender ID="ListSearchExtender11" runat="server" TargetControlID="ddColor"
                                            ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                                        </cc1:ListSearchExtender>
                                    </td>
                                    <td id="TDShape" runat="server" visible="false">
                                        <asp:Label ID="lblShapeName" class="tdstyle" runat="server" Text="Shape" CssClass="labelbold"></asp:Label>
                                        <br />
                                        <asp:DropDownList ID="ddShape" runat="server" Width="100px" AutoPostBack="True" CssClass="dropdown"
                                            OnSelectedIndexChanged="ddShape_SelectedIndexChanged">
                                        </asp:DropDownList>
                                        <cc1:ListSearchExtender ID="ListSearchExtender12" runat="server" TargetControlID="ddShape"
                                            ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                                        </cc1:ListSearchExtender>
                                    </td>
                                    <td id="TDSize" runat="server" visible="false">
                                        <asp:Label ID="lblSizeName" class="tdstyle" runat="server" Text="Size" CssClass="labelbold"></asp:Label>
                                        <br />
                                        <asp:DropDownList ID="ddSize" runat="server" Width="100px" CssClass="dropdown" AutoPostBack="True"
                                            OnSelectedIndexChanged="ddSize_SelectedIndexChanged">
                                        </asp:DropDownList>
                                        <cc1:ListSearchExtender ID="ListSearchExtender13" runat="server" TargetControlID="ddSize"
                                            ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                                        </cc1:ListSearchExtender>
                                    </td>
                                    <td id="TDShade" runat="server" visible="false">
                                        <asp:Label ID="lblShade" runat="server" class="tdstyle" Text="Shade" CssClass="labelbold"></asp:Label>
                                        <br />
                                        <asp:DropDownList ID="ddShade" runat="server" Width="100px" CssClass="dropdown" OnSelectedIndexChanged="ddShade_SelectedIndexChanged"
                                            AutoPostBack="True">
                                        </asp:DropDownList>
                                        <cc1:ListSearchExtender ID="ListSearchExtender14" runat="server" TargetControlID="ddShade"
                                            ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                                        </cc1:ListSearchExtender>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="7" class="tdstyle" id="tdsubquality" runat="server">
                                        <asp:Label ID="Label18" class="tdstyle" runat="server" Text="  Sub Quality" CssClass="labelbold"></asp:Label>
                                        &nbsp;&nbsp;<asp:DropDownList ID="DDSubQuality" runat="server" Width="300px" CssClass="dropdown"
                                            AutoPostBack="True">
                                        </asp:DropDownList>
                                        <cc1:ListSearchExtender ID="ListSearchExtender15" runat="server" TargetControlID="DDSubQuality"
                                            ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                                        </cc1:ListSearchExtender>
                                    </td>
                                </tr>
                            </table>
                            <table>
                                <tr>
                                    <td>
                                        <asp:Label ID="Label19" runat="server" Text="  Width" CssClass="labelbold"></asp:Label>
                                        <br />
                                        <asp:TextBox ID="TxtWidth" runat="server" Width="75px" CssClass="textb" OnTextChanged="TxtWidth_TextChanged"
                                            AutoPostBack="True"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:Label ID="Label20" runat="server" Text=" Length" CssClass="labelbold"></asp:Label>
                                        <br />
                                        <asp:TextBox ID="TxtLength" runat="server" Width="75px" CssClass="textb" OnTextChanged="TxtLength_TextChanged"
                                            AutoPostBack="True"></asp:TextBox>
                                    </td>
                                    <td class="tdstyle">
                                        <asp:Label ID="Label21" runat="server" Text="   Price" CssClass="labelbold"></asp:Label>
                                        <br />
                                        <asp:TextBox ID="TxtPrice" runat="server" Width="75px" CssClass="textb" onkeydown="return (event.keyCode!=13);"></asp:TextBox>
                                    </td>
                                    <td class="tdstyle" id="TDRatePerPcs" runat="server" visible="false">
                                        <asp:Label ID="Label38" runat="server" Text="Rate Per Pcs" CssClass="labelbold"></asp:Label>
                                        <br />
                                        <asp:TextBox ID="TxtRatePerPcs" runat="server" Width="75px" CssClass="textb" onkeydown="return (event.keyCode!=13);"></asp:TextBox>
                                    </td>
                                    <td class="tdstyle">
                                        <asp:Label ID="Label22" runat="server" Text="  Area" CssClass="labelbold"></asp:Label>
                                        <br />
                                        <asp:TextBox ID="TxtArea" runat="server" Width="75px" CssClass="textb" onkeydown="return (event.keyCode!=13);"></asp:TextBox>
                                    </td>
                                    <td class="tdstyle">
                                        <asp:Label ID="Label23" runat="server" Text="   Total Qty" CssClass="labelbold"></asp:Label>
                                        <br />
                                        <asp:TextBox ID="TxtTotalQty" runat="server" Width="75px" CssClass="textb" ReadOnly="True"
                                            onkeydown="return (event.keyCode!=13);"></asp:TextBox>
                                    </td>
                                    <td class="tdstyle">
                                        <asp:Label ID="Label24" runat="server" Text="   Pre Pack Qty" CssClass="labelbold"></asp:Label>
                                        <br />
                                        <asp:TextBox ID="TxtPrePackQty" runat="server" Width="75px" CssClass="textb" ReadOnly="True"
                                            onkeydown="return (event.keyCode!=13);"></asp:TextBox>
                                    </td>
                                    <td class="tdstyle">
                                        <asp:Label ID="Label25" runat="server" Text="  Pack Qty" CssClass="labelbold"></asp:Label>
                                        <br />
                                        <asp:TextBox ID="TxtPackQty" runat="server" Width="75px" CssClass="textb" AutoPostBack="True"
                                            OnTextChanged="TxtPackQty_TextChanged" Enabled="false"></asp:TextBox>
                                    </td>
                                    <td class="tdstyle">
                                        <asp:Label ID="Label26" runat="server" Text="  Remarks" CssClass="labelbold"></asp:Label>
                                        <br />
                                        <asp:TextBox ID="TxtRemarks" runat="server" Width="350px" CssClass="textb" onkeydown="return (event.keyCode!=13);"></asp:TextBox>
                                    </td>
                                    <td class="tdstyle">
                                        <asp:Label ID="Label28" runat="server" Text="  Buyer Code" CssClass="labelbold"></asp:Label>
                                        <br />
                                        <asp:TextBox ID="TxtBuyer" runat="server" Width="150px" CssClass="textb" onkeydown="return (event.keyCode!=13);"></asp:TextBox>
                                    </td>
                                </tr>
                            </table>
                            <table>
                                <table>
                                    <tr>
                                        <td class="tdstyle">
                                            <asp:Label ID="Label27" runat="server" Text="   Purchase Code" CssClass="labelbold"></asp:Label>
                                            <br />
                                            <asp:TextBox ID="Txtpurchasecode" runat="server" Width="150px" CssClass="textb" onkeydown="return (event.keyCode!=13);"></asp:TextBox>
                                        </td>
                                        <td class="tdstyle">
                                            <asp:Label ID="Label29" runat="server" Text="  UCC Number" CssClass="labelbold"></asp:Label>
                                            <br />
                                            <asp:TextBox ID="txtuccnumber" runat="server" Width="150px" CssClass="textb" onkeydown="return (event.keyCode!=13);"></asp:TextBox>
                                        </td>
                                        <td class="tdstyle">
                                            <asp:Label ID="Label30" runat="server" Text="   RUG Id" CssClass="labelbold"></asp:Label>
                                            <br />
                                            <asp:TextBox ID="txtrugid" runat="server" Width="150px" CssClass="textb" onkeydown="return (event.keyCode!=13);"></asp:TextBox>
                                        </td>
                                        <td class="tdstyle">
                                            <asp:Label ID="Label39" runat="server" Text="Style No" CssClass="labelbold"></asp:Label>
                                            <br />
                                            <asp:TextBox ID="txtStyleNo" runat="server" Width="150px" CssClass="textb" onkeydown="return (event.keyCode!=13);"></asp:TextBox>
                                        </td>
                                        <td runat="server">
                                            <fieldset>
                                                <legend>
                                                    <asp:Label ID="lblbaled" runat="server" Text="BALE DIMENSION" CssClass="labelbold"></asp:Label>
                                                </legend>
                                                <table border="0" cellpadding="1" cellspacing="0">
                                                    <tr>
                                                        <td>
                                                            <asp:Label ID="lbllength" Text="L" CssClass="labelbold" runat="server" />
                                                            <br />
                                                            <asp:TextBox ID="txtlengthBale" CssClass="ShellTxtToCalculate" Width="70px" runat="server"
                                                                onchange="return keypress();" />
                                                        </td>
                                                        <td>
                                                            <asp:Label ID="Label33" Text="W" CssClass="labelbold" runat="server" />
                                                            <br />
                                                            <asp:TextBox ID="txtwidthBale" CssClass="ShellTxtToCalculate" Width="70px" runat="server"
                                                                onchange="return keypress();" />
                                                        </td>
                                                        <td>
                                                            <asp:Label ID="Label34" Text="H" CssClass="labelbold" runat="server" />
                                                            <br />
                                                            <asp:TextBox ID="txtheightbale" CssClass="ShellTxtToCalculate" Width="70px" runat="server"
                                                                onchange="return keypress();" />
                                                        </td>
                                                        <td>
                                                            <asp:Label ID="Label35" Text="CBM" CssClass="labelbold" runat="server" />
                                                            <br />
                                                            <asp:TextBox ID="txtcbmbale" Width="70px" runat="server" />
                                                        </td>
                                                        <td>
                                                            <asp:Label ID="Label36" CssClass="labelbold" runat="server" />
                                                            <br />
                                                            <asp:TextBox ID="txtSinglePcsNetWt" Width="70px" runat="server" />
                                                        </td>
                                                        <td>
                                                            <asp:Label ID="Label37" CssClass="labelbold" runat="server" />
                                                            <br />
                                                            <asp:TextBox ID="txtSinglePcsGrossWt" Width="70px" runat="server" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </fieldset>
                                        </td>
                                    </tr>
                                </table>
                                <table>
                                    <tr>
                                        <td>
                                            <asp:Label ID="LblErrorMessage" runat="server" Font-Size="Small" CssClass="labelbold"
                                                ForeColor="Red"></asp:Label>
                                        </td>
                                        <td colspan="2" align="right">
                                            <asp:LinkButton ID="lnkchnginvoice" Text="Change Invoice No. & Date" CssClass="labelbold"
                                                ForeColor="Red" runat="server" OnClientClick="return confirm('Do you want to change Invoice No. & Date?')"
                                                OnClick="lnkchnginvoice_Click" />
                                            <asp:Button ID="BtnSave" runat="server" Text="Save" OnClick="BtnSave_Click" OnClientClick="return confirm('Do you want to save data?')"
                                                CssClass="buttonnorm" />
                                            &nbsp;<asp:Button ID="BtnNew" runat="server" Text="New" OnClientClick="return NewForm();"
                                                CssClass="buttonnorm" />
                                            &nbsp;<asp:Button ID="BtnClose" runat="server" Text="Close" OnClientClick="return CloseForm();"
                                                CssClass="buttonnorm" />
                                                &nbsp; <asp:Button ID="BtnPreview" runat="server" Text="Preview" OnClick="BtnPreview_Click" CssClass="buttonnorm" Visible="false" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">
                                            <div style="width: 800px; max-height: 300px; overflow: scroll">
                                                <asp:GridView ID="DGOrderDetail" runat="server" DataKeyNames="Sr_No" AutoGenerateColumns="False"
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
                                                                <asp:Label ID="Lblpacking" runat="server" Visible="false" Text='<%# Bind("PackingId") %>' />
                                                            </ItemTemplate>
                                                            <HeaderStyle Width="80px" />
                                                        </asp:TemplateField>
                                                        <%--<asp:BoundField DataField="Sr_No" HeaderText="Sr_No" />--%>
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
                                                        <asp:TemplateField HeaderText="RATE PER PCS" Visible="false">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblrateperpcs" runat="server" Text='<%#Bind("RatePerPcs") %>'></asp:Label>
                                                            </ItemTemplate>
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
                                                        <asp:TemplateField HeaderText="STOCK NO">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblStockNo" runat="server" Text='<%#Bind("StockNo") %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <HeaderStyle Font-Bold="True" HorizontalAlign="Center" Width="250px" />
                                                            <ItemStyle HorizontalAlign="Left" Width="250px" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Roll From">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblRollfrom" runat="server" Text='<%#Bind("RollFrom") %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <HeaderStyle Font-Bold="True" HorizontalAlign="Center" />
                                                            <ItemStyle HorizontalAlign="Center" Width="75px" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Roll To">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblRollTo" runat="server" Text='<%#Bind("RollTo") %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <HeaderStyle Font-Bold="True" HorizontalAlign="Center" />
                                                            <ItemStyle HorizontalAlign="Center" Width="75px" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="UCC Number">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblUccNumber" runat="server" Text='<%#Bind("UCCNumber") %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <EditItemTemplate>
                                                                <asp:TextBox ID="txtUccNumber" runat="server" Text='<%#Bind("UCCNumber") %>'></asp:TextBox>
                                                            </EditItemTemplate>
                                                            <HeaderStyle Font-Bold="True" HorizontalAlign="Center" />
                                                            <ItemStyle HorizontalAlign="Center" Width="75px" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="RUGID">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblrugid" runat="server" Text='<%#Bind("RUGID") %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <EditItemTemplate>
                                                                <asp:TextBox ID="txtrugid" runat="server" Text='<%#Bind("RUGID") %>'></asp:TextBox>
                                                            </EditItemTemplate>
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
                                                        <asp:TemplateField HeaderText="SinglePcs NetWt">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblSinglePcsNetWt" runat="server" Text='<%#Bind("SinglePcsNetWt") %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <EditItemTemplate>
                                                                <asp:TextBox ID="txtSinglePcsNetWt" runat="server" Text='<%#Bind("SinglePcsNetWt") %>'></asp:TextBox>
                                                            </EditItemTemplate>
                                                            <HeaderStyle Font-Bold="True" HorizontalAlign="Center" />
                                                            <ItemStyle HorizontalAlign="Center" Width="75px" />
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
                                        <td valign="top">
                                            <div style="width: 200px; height: 200px; overflow: scroll">
                                                <asp:GridView ID="DGStock" Width="150px" runat="server" DataKeyNames="Sr_No" AutoGenerateColumns="False"
                                                    CssClass="grid-views">
                                                    <HeaderStyle CssClass="gvheaders" />
                                                    <AlternatingRowStyle CssClass="gvalts" />
                                                    <Columns>
                                                        <asp:TemplateField HeaderText="">
                                                            <ItemTemplate>
                                                                <asp:CheckBox ID="Chkbox" runat="server" Enabled="false" />
                                                            </ItemTemplate>
                                                            <HeaderStyle Width="20px" />
                                                        </asp:TemplateField>
                                                        <asp:BoundField DataField="StockNo" HeaderText="StockNo" />
                                                        <asp:TemplateField HeaderText="Pack Status" Visible="false">
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="TxtPack" runat="server" Visible="false" Text='<%# Bind("Pack") %>' />
                                                            </ItemTemplate>
                                                            <HeaderStyle Width="80px" />
                                                        </asp:TemplateField>
                                                    </Columns>
                                                </asp:GridView>
                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="3">
                                            <table>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lbltotalpcs" Text="Total Pcs" runat="server" CssClass="labelbold" />
                                                        <br />
                                                        <asp:TextBox ID="txttotalpcsgrid" CssClass="textb" Width="120px" runat="server" BackColor="Yellow"
                                                            Enabled="false" />
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="Label31" Text="Total Area" runat="server" CssClass="labelbold" />
                                                        <br />
                                                        <asp:TextBox ID="txttotalareagrid" CssClass="textb" Width="120px" runat="server"
                                                            BackColor="Yellow" Enabled="false" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr id="TRcarpetSet" runat="server" visible="false">
                                        <td colspan="3">
                                            <fieldset>
                                                <legend>
                                                    <asp:Label ID="lblcarpetset" Text="Carpet Sets" CssClass="labelbold" ForeColor="Red"
                                                        Font-Bold="true" runat="server" />
                                                </legend>
                                                <table>
                                                    <tr>
                                                        <td>
                                                            <asp:Label ID="lblfrom" Text="From Roll" CssClass="labelbold" runat="server" />
                                                            <br />
                                                            <asp:DropDownList ID="DDfromroll" CssClass="dropdown" Width="150px" runat="server"
                                                                AutoPostBack="true" OnSelectedIndexChanged="DDfromroll_SelectedIndexChanged">
                                                            </asp:DropDownList>
                                                        </td>
                                                        <td>
                                                            <asp:Label ID="Label32" Text="To Roll" CssClass="labelbold" runat="server" />
                                                            <br />
                                                            <asp:DropDownList ID="DDtoroll" CssClass="dropdown" Width="150px" runat="server">
                                                            </asp:DropDownList>
                                                        </td>
                                                        <td>
                                                            <br />
                                                            <asp:CheckBox ID="ChkForExcel" Text="For Excel" runat="server" />
                                                            <asp:Button ID="btnshow" Text="Show" runat="server" CssClass="buttonnorm" OnClick="btnshow_Click" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </fieldset>
                                        </td>
                                    </tr>
                                    <tr id="TrDownGrid" runat="server" visible="false">
                                        <td colspan="3">
                                            <div style="width: 100%; height: 200px; overflow: scroll">
                                                <asp:GridView ID="DGChangeQDC" Width="100%" runat="server" DataKeyNames="SrNo" AutoGenerateColumns="False"
                                                    CssClass="grid-view">
                                                    <HeaderStyle CssClass="gvheaders" />
                                                    <AlternatingRowStyle CssClass="gvalts" />
                                                    <Columns>
                                                        <asp:BoundField DataField="CATEGORY" HeaderText="CATEGORY">
                                                            <HeaderStyle Font-Bold="True" HorizontalAlign="Center" />
                                                            <ItemStyle HorizontalAlign="Center" />
                                                        </asp:BoundField>
                                                        <%--<asp:TemplateField HeaderText="CATEGORY" >
                                    <ItemTemplate>
                                    <asp:TextBox ID="TxtCATEGORY" runat="server" Width="200px" Text='<%# Bind("CATEGORY") %>' AutoPostBack="true" CssClass="textb"
                                    ></asp:TextBox>
                                    </ItemTemplate>
                                    </asp:TemplateField>--%>
                                                        <asp:BoundField DataField="ITEM" HeaderText="ITEM">
                                                            <HeaderStyle Font-Bold="True" HorizontalAlign="Center" />
                                                            <ItemStyle HorizontalAlign="Center" />
                                                        </asp:BoundField>
                                                        <%--<asp:TemplateField HeaderText="ITEM" >
                                    <ItemTemplate>
                                    <asp:TextBox ID="TxtITEM" runat="server" Width="200px" Text='<%# Bind("ITEM") %>' AutoPostBack="true" CssClass="textb"
                                    ></asp:TextBox>
                                    </ItemTemplate>
                                    </asp:TemplateField>--%>
                                                        <%--<asp:BoundField DataField="QUALITY" HeaderText="QUALITY" >
                                     <HeaderStyle Font-Bold="True" HorizontalAlign="Center" />
                                     <ItemStyle HorizontalAlign="Center" />
                                     </asp:BoundField>--%>
                                                        <asp:TemplateField HeaderText="QUALITY">
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="TxtQUALITY" runat="server" Width="250px" Text='<%# Bind("QUALITY") %>'
                                                                    AutoPostBack="true" CssClass="textb"></asp:TextBox>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <%--<asp:BoundField DataField="DESIGN" HeaderText="DESIGN" >
                                     <HeaderStyle Font-Bold="True" HorizontalAlign="Center" />
                                     <ItemStyle HorizontalAlign="Center" />
                                     </asp:BoundField>--%>
                                                        <asp:TemplateField HeaderText="DESIGN">
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="TxtDESIGN" runat="server" Width="250px" Text='<%# Bind("DESIGN") %>'
                                                                    AutoPostBack="true" CssClass="textb"></asp:TextBox>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <%--<asp:BoundField DataField="COLOR" HeaderText="COLOR" >
                                     <HeaderStyle Font-Bold="True" HorizontalAlign="Center" />
                                     <ItemStyle HorizontalAlign="Center" />
                                     </asp:BoundField>--%>
                                                        <asp:TemplateField HeaderText="COLOR">
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="TxtCOLOR" runat="server" Width="250px" Text='<%# Bind("COLOR") %>'
                                                                    AutoPostBack="true" CssClass="textb"></asp:TextBox>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <%--<asp:BoundField DataField="QTY" HeaderText="QTY">
                                     <HeaderStyle Font-Bold="True" HorizontalAlign="Center" />
                                     <ItemStyle HorizontalAlign="Center" />
                                     </asp:BoundField>
                                     <asp:BoundField DataField="PRICE" HeaderText="PRICE" >
                                     <HeaderStyle Font-Bold="True" HorizontalAlign="Center" />
                                     <ItemStyle HorizontalAlign="Center" />
                                     </asp:BoundField>
                                     <asp:BoundField DataField="AREA" HeaderText="AREA" >
                                     <HeaderStyle Font-Bold="True" HorizontalAlign="Center" />
                                     <ItemStyle HorizontalAlign="Center" />
                                     </asp:BoundField>--%>
                                                    </Columns>
                                                    <EmptyDataRowStyle CssClass="gvemptytext" />
                                                </asp:GridView>
                                            </div>
                                            <asp:Button ID="BtnSaveChangeQDC" runat="server" Text="Save" OnClick="BtnSaveChangeQDC_Click"
                                                OnClientClick="return confirm('Do you want to save data?')" CssClass="buttonnorm" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <asp:HiddenField ID="hnpackingid" runat="server" />
                                        <asp:HiddenField ID="hnid" runat="server" />
                                        <asp:HiddenField ID="hnfinished" runat="server" />
                                        <asp:HiddenField ID="hndesignid" runat="server" />
                                        <asp:HiddenField ID="hnCarpetNoTypeId" runat="server" />
                                    </tr>
                                </table>
                                <asp:HiddenField ID="hnsampletype" runat="server" Value="1" />
                        </ContentTemplate>
                         <Triggers>
            <asp:PostBackTrigger ControlID="btnshow" />
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
