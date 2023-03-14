<%@ Page Language="C#" AutoEventWireup="true" CodeFile="FrmOrderWiseConsumption.aspx.cs"
    EnableSessionState="True" EnableViewState="true" Inherits="Masters_Carpet_FrmOrderWiseConsumption"
    EnableEventValidation="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register Src="~/UserControls/ucmenu.ascx" TagName="ucmenu" TagPrefix="uc1" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <link rel="Stylesheet" type="text/css" href="../../App_Themes/Default/Style.css" />
    <script src="../../Scripts/JScript.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery-1.4.1.min.js" type="text/javascript"></script>
    <%--<script type="text/javascript" src="../../Scripts/Fixfocus.js"></script>--%>
    <script type="text/javascript">
        function priview() {
            document.getElementById('BtnPreview').click();
            window.open('../../ReportViewer.aspx');
        }
        function CloseForm() {
            window.location.href = "../../main.aspx";
        }
        function DefineItemcode() {
            var varcode = document.getElementById('TxtProdCode').value;
            window.open('DefineItemCode.aspx?ProdCode=' + varcode + '', '', 'width=950px,Height=500px');
        }
        function OtherExpense() {
            var varcode = document.getElementById('TxtProdCode').value;
            window.open('OtherExpense.aspx?ProdCode=' + varcode + '', '', 'width=1100px,Height=500px,menubar=no');
        }
        function PackingCost() {
            var varcode = document.getElementById('TxtProdCode').value;
            window.open('PackingAndOtherCost.aspx?ProdCode=' + varcode + '', '', 'width=950px,Height=500px');
        }

        function validate() {
            var doc, msg;
            doc = document.forms[0];
            msg = "";
            if (doc.ddProcessName.value == "0")
            { msg = "Select ProcessName "; }
            else if (doc.ddCategoryName.vaue = "0") {
                msg = "select CategoryName";
            }
            if (msg == "")
            { return true; }
            else {
                alert(msg);
                return false;
            }
        }
        function Addcategory() {
            var answer = confirm("Do you want to ADD?")
            if (answer) {
                window.open('AddItemCategory.aspx', '', 'width=500px,Height=500px');
            }
        }
        function Additem() {
            var answer = confirm("Do you want to ADD?")
            if (answer) {
                window.open('AddItemName.aspx', '', 'width=550px,Height=500px');
            }
        }
        function Addquality() {
            var answer = confirm("Do you want to ADD?")
            if (answer) {
                window.open('AddQuality.aspx', '', 'width=701px,Height=501px');
            }
        }
        function Adddesign() {
            var answer = confirm("Do you want to ADD?")
            if (answer) {
                window.open('AddDesign.aspx', '', 'width=601px,Height=401px');
            }
        }

        function Addcolor() {
            var answer = confirm("Do you want to ADD?")
            if (answer) {
                window.open('AddColor.aspx', '', 'width=501px,Height=501px', 'resizeable=yes');
            }
        }
        function Addshape() {
            var answer = confirm("Do you want to ADD?")
            if (answer) {
                window.open('AddShape.aspx', '', 'width=901px,Height=401px');
            }
        }
        function AddsizeNew() {
            var answer = confirm("Do you want to ADD?")
            if (answer) {
                window.open('AddSize.aspx', '', 'width=1000px,Height=401px');
            }
        }

        function AddShade() {
            var answer = confirm("Do you want to ADD?")
            if (answer) {
                window.open('AddShadeColor.aspx', '', 'width=901px,Height=401px');
            }
        }

        function AddProcess() {
            var answer = confirm("Do you want to ADD?")
            if (answer) {
                window.open('AddProcess.aspx', '', 'width=901px,Height=401px');
            }
        }
        function ADD_FINISH() {
            var answer = confirm("Do you want to ADD?")
            if (answer) {
                window.open('FinishType.aspx', '', 'width=901px,Height=401px');
            }
        }
        function addsubquality() {
            var answer = confirm("Do you want to ADD?")
            if (answer) {
                window.open('FinishType.aspx', '', 'width=901px,Height=401px');
            }
        }

        function additemcode() {
            var answer = confirm("Do you want to ADD?")
            if (answer) {
                window.open('Additemcode.aspx', '', 'width=901px,Height=401px');
            }
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div style="overflow: scroll">
        <table width="100%" border="1">
            <tr id="zzz" runat="server">
                <td>
                    <table width="100%">
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
                <td align="center" height="inherit" valign="top" class="style1" colspan="2">
                    <div id="1" style="height: auto" align="left">
                        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                            <ContentTemplate>
                                <table width="100%">
                                    <tr align="left">
                                        <td align="left">
                                            <div style="border: 1px; border-style: solid; border-color: Black">
                                                <table>
                                                    <tr id="Tr5">
                                                        <td class="tdstyle">
                                                            Customer Code
                                                            <br />
                                                            <asp:DropDownList CssClass="dropdown" ID="DDCustomerCode" runat="server" AutoPostBack="True"
                                                                OnSelectedIndexChanged="DDCustomerCode_SelectedIndexChanged" Width="150px" TabIndex="2">
                                                            </asp:DropDownList>
                                                            <cc1:ListSearchExtender ID="ListSearchExtender2" runat="server" TargetControlID="DDCustomerCode"
                                                                ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                                                            </cc1:ListSearchExtender>
                                                        </td>
                                                        <td class="tdstyle">
                                                            Order No
                                                            <br />
                                                            <asp:DropDownList CssClass="dropdown" ID="DDOrderNo" runat="server" AutoPostBack="True"
                                                                Width="150px" OnSelectedIndexChanged="DDOrderNo_SelectedIndexChanged">
                                                            </asp:DropDownList>
                                                            <cc1:ListSearchExtender ID="ListSearchExtender1" runat="server" TargetControlID="DDOrderNo"
                                                                ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                                                            </cc1:ListSearchExtender>
                                                        </td>
                                                        <td class="tdstyle">
                                                            <asp:Button ID="btnrefreshprocess" runat="server" Height="0px" Width="0px" BackColor="White"
                                                                BorderColor="White" BorderWidth="0px" ForeColor="White" OnClick="btnrefreshprocess_Click" />
                                                            Process Name&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                            <asp:Button ID="Button1" runat="server" CssClass="buttonsmall" OnClientClick="return AddProcess();"
                                                                Height="15px" Text=".." TabIndex="25" />
                                                            <br />
                                                            <asp:DropDownList ID="ddProcessName" runat="server" Width="150px" TabIndex="24" AutoPostBack="True"
                                                                CssClass="dropdown" OnSelectedIndexChanged="ddProcessName_SelectedIndexChanged">
                                                            </asp:DropDownList>
                                                            <cc1:ListSearchExtender ID="ListSearchExtender3" runat="server" TargetControlID="ddProcessName"
                                                                ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                                                            </cc1:ListSearchExtender>
                                                        </td>
                                                        <td class="tdstyle">
                                                            <asp:Label ID="lblItemCode" runat="server" Text="Item Code"></asp:Label>
                                                            <br />
                                                            <asp:TextBox ID="TxtProdCode" runat="server" CssClass="textb" Width="100px" AutoPostBack="True"
                                                                OnTextChanged="TxtProdCode_TextChanged" TabIndex="1"></asp:TextBox>
                                                            <cc1:AutoCompleteExtender ID="TxtProdCode_AutoCompleteExtender" runat="server" EnableCaching="true"
                                                                Enabled="True" MinimumPrefixLength="1" ServiceMethod="GetQuality1" TargetControlID="TxtProdCode"
                                                                UseContextKey="True">
                                                            </cc1:AutoCompleteExtender>
                                                        </td>
                                                        <td class="tdstyle">
                                                            <asp:Label ID="lblcategoryname" runat="server" Text="Category Name"></asp:Label>
                                                            <asp:Button ID="refreshcategory" runat="server" Height="0px" OnClick="refreshcategory_Click"
                                                                Text="." Width="0px" BackColor="White" BorderColor="White" BorderWidth="0px"
                                                                ForeColor="White" />
                                                            &nbsp;<asp:Button ID="btnaddcategory" runat="server" CssClass="buttonsmall" OnClientClick="return Addcategory();"
                                                                Height="16px" Text=".." TabIndex="3" />
                                                            <br />
                                                            <asp:DropDownList ID="ddCategoryName" runat="server" OnSelectedIndexChanged="ddcategoryname_SelectedIndexChanged"
                                                                Width="150px" AutoPostBack="True" TabIndex="2" CssClass="dropdown">
                                                            </asp:DropDownList>
                                                            <cc1:ListSearchExtender ID="ListSearchExtender4" runat="server" TargetControlID="ddCategoryName"
                                                                ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                                                            </cc1:ListSearchExtender>
                                                        </td>
                                                        <td class="tdstyle">
                                                            <asp:Label ID="lblitemname" runat="server" Text="Item Name"></asp:Label>
                                                            <asp:Button ID="refreshitem" runat="server" Height="0px" Width="0px" BackColor="White"
                                                                BorderColor="White" BorderWidth="0px" ForeColor="White" OnClick="refreshitem_Click" />
                                                            &nbsp;<asp:Button ID="btnadditem" runat="server" CssClass="buttonsmall" OnClientClick="return Additem();"
                                                                Height="15px" Text=".." TabIndex="5" />
                                                            <br />
                                                            <asp:DropDownList ID="ddItemName" runat="server" OnSelectedIndexChanged="dditemname_SelectedIndexChanged"
                                                                Width="150px" AutoPostBack="True" TabIndex="4" CssClass="dropdown">
                                                            </asp:DropDownList>
                                                            <cc1:ListSearchExtender ID="ListSearchExtender5" runat="server" TargetControlID="ddItemName"
                                                                ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                                                            </cc1:ListSearchExtender>
                                                        </td>
                                                        <td id="Quality" runat="server" visible="false" class="tdstyle">
                                                            <asp:Label ID="lblqualityname" runat="server" Text="Quality"></asp:Label>
                                                            <asp:Button ID="refreshquality" runat="server" Height="0px" Width="0px" BackColor="White"
                                                                BorderColor="White" BorderWidth="0px" ForeColor="White" OnClick="refreshquality_Click" />
                                                            &nbsp;<asp:Button ID="btnaddquality" runat="server" CssClass="buttonsmall" OnClientClick="return Addquality();"
                                                                Height="15px" Text=".." TabIndex="7" />
                                                            <br />
                                                            <asp:DropDownList ID="ddQuality" runat="server" Width="150px" TabIndex="6" AutoPostBack="True"
                                                                CssClass="dropdown" OnSelectedIndexChanged="ddQuality_SelectedIndexChanged">
                                                            </asp:DropDownList>
                                                            <cc1:ListSearchExtender ID="ListSearchExtender6" runat="server" TargetControlID="ddQuality"
                                                                ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                                                            </cc1:ListSearchExtender>
                                                        </td>
                                                        <td id="Design" runat="server" visible="false" class="tdstyle">
                                                            <asp:Label ID="lbldesignname" runat="server" Text="Design"></asp:Label>
                                                            <asp:Button ID="refreshdesign" runat="server" Height="0px" Width="0px" BackColor="White"
                                                                BorderColor="White" BorderWidth="0px" ForeColor="White" OnClick="refreshdesign_Click" />
                                                            &nbsp;<asp:Button ID="btnadddesign" runat="server" CssClass="buttonsmall" OnClientClick="return Adddesign();"
                                                                Height="15px" Text=".." TabIndex="8" />
                                                            <asp:CheckBox ID="CHKFORALLDESIGN" runat="server" AutoPostBack="True" OnCheckedChanged="CHKFORALLDESIGN_CheckedChanged" />
                                                            <br />
                                                            <asp:DropDownList ID="ddDesign" runat="server" Width="150px" TabIndex="7" AutoPostBack="True"
                                                                CssClass="dropdown" OnSelectedIndexChanged="ddDesign_SelectedIndexChanged">
                                                            </asp:DropDownList>
                                                            <cc1:ListSearchExtender ID="ListSearchExtender7" runat="server" TargetControlID="ddDesign"
                                                                ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                                                            </cc1:ListSearchExtender>
                                                        </td>
                                                        <td id="Color" runat="server" visible="false" class="tdstyle">
                                                            <asp:Label ID="lblcolorname" runat="server" Text="Color"></asp:Label>
                                                            <asp:Button ID="refreshcolor" runat="server" Height="0px" Width="0px" BackColor="White"
                                                                BorderColor="White" BorderWidth="0px" ForeColor="White" OnClick="refreshcolor_Click" />
                                                            &nbsp;<asp:Button ID="btnaddcolor" runat="server" CssClass="buttonsmall" OnClientClick="return Addcolor();"
                                                                Height="15px" Text=".." TabIndex="10" />
                                                            <asp:CheckBox ID="CHKFORALLCOLOR" runat="server" AutoPostBack="True" OnCheckedChanged="CHKFORALLCOLOR_CheckedChanged"
                                                                TabIndex="11" />
                                                            <br />
                                                            <asp:DropDownList ID="ddColor" runat="server" Width="100px" TabIndex="9" AutoPostBack="True"
                                                                CssClass="dropdown" OnSelectedIndexChanged="ddColor_SelectedIndexChanged">
                                                            </asp:DropDownList>
                                                            <cc1:ListSearchExtender ID="ListSearchExtender8" runat="server" TargetControlID="ddColor"
                                                                ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                                                            </cc1:ListSearchExtender>
                                                        </td>
                                                        <td id="Shade" runat="server" visible="false" class="tdstyle">
                                                            <asp:Label ID="lblshadename" runat="server" Text="Shade"></asp:Label>
                                                            <asp:Button ID="refreshshade" runat="server" Height="0px" Width="0px" BackColor="White"
                                                                BorderColor="White" BorderWidth="0px" ForeColor="White" OnClick="refreshshade_Click" />
                                                            <asp:Button ID="btnaddshade" runat="server" CssClass="buttonsmall" OnClientClick="return AddShade();"
                                                                Height="15px" Text=".." TabIndex="21" />
                                                            <br />
                                                            <asp:DropDownList ID="ddShade" runat="server" Width="100px" TabIndex="20" CssClass="dropdown">
                                                            </asp:DropDownList>
                                                            <cc1:ListSearchExtender ID="ListSearchExtender9" runat="server" TargetControlID="ddShade"
                                                                ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                                                            </cc1:ListSearchExtender>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td id="Shape" runat="server" visible="false" class="tdstyle">
                                                            <asp:Label ID="lblshapename" runat="server" Text="Shape"></asp:Label>
                                                            <asp:Button ID="refreshshape" runat="server" Height="0px" Width="0px" BackColor="White"
                                                                BorderColor="White" BorderWidth="0px" ForeColor="White" OnClick="refreshshape_Click" />
                                                            &nbsp;<asp:Button ID="btnaddshape" runat="server" CssClass="buttonsmall" OnClientClick="return Addshape();"
                                                                Height="15px" Text=".." TabIndex="15" />
                                                            <br />
                                                            <asp:DropDownList ID="ddShape" runat="server" Width="100px" AutoPostBack="True" OnSelectedIndexChanged="ddshape_SelectedIndexChanged"
                                                                CssClass="dropdown" TabIndex="14">
                                                            </asp:DropDownList>
                                                            <cc1:ListSearchExtender ID="ListSearchExtender10" runat="server" TargetControlID="ddShape"
                                                                ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                                                            </cc1:ListSearchExtender>
                                                        </td>
                                                        <td id="Size" runat="server" visible="false" class="tdstyle">
                                                            <asp:Label ID="lblsizename" runat="server" Text="Size"></asp:Label>
                                                            <asp:Button ID="refreshsize" runat="server" Height="0px" Width="0px" BackColor="White"
                                                                BorderColor="White" BorderWidth="0px" ForeColor="White" OnClick="refreshsize_Click" />
                                                            &nbsp;<asp:Button ID="btnaddsize" runat="server" CssClass="buttonsmall" OnClientClick="return AddsizeNew();"
                                                                Height="15px" Text=".." TabIndex="17" />
                                                            <asp:CheckBox ID="CHKFORALLSIZE" runat="server" AutoPostBack="True" OnCheckedChanged="CHKFORALLSIZE_CheckedChanged"
                                                                TabIndex="18" />
                                                            <br />
                                                            <asp:DropDownList ID="ddSize" runat="server" Width="100px" TabIndex="16" AutoPostBack="True"
                                                                CssClass="dropdown" OnSelectedIndexChanged="ddSize_SelectedIndexChanged">
                                                            </asp:DropDownList>
                                                            <cc1:ListSearchExtender ID="ListSearchExtender11" runat="server" TargetControlID="ddSize"
                                                                ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                                                            </cc1:ListSearchExtender>
                                                        </td>
                                                        <td id="TDMtrSize" runat="server" visible="false" class="tdstyle">
                                                            <asp:CheckBox ID="ChMeteerSize" Text="MtrSize" runat="server" OnCheckedChanged="ChMeteerSize_CheckedChanged"
                                                                AutoPostBack="True" TabIndex="19" />
                                                            &nbsp;&nbsp;&nbsp;&nbsp;
                                                            <asp:TextBox ID="txtFinishedid" runat="server" Width="0px" Height="0px"></asp:TextBox>
                                                        </td>
                                                        <td id="LblSub_Quality" runat="server" class="tdstyle" colspan="2">
                                                            <asp:Button ID="btnrefreshsbqlt" runat="server" Height="0px" Width="0px" BackColor="White"
                                                                BorderColor="White" BorderWidth="0px" ForeColor="White" OnClick="btnrefreshsbqlt_Click" />
                                                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp; Sub_Quality&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                            <asp:Button ID="btnsubquality" runat="server" CssClass="buttonsmall" OnClientClick="return additemcode();"
                                                                Height="16px" Text=".." TabIndex="23" />
                                                            <br />
                                                            <asp:DropDownList ID="ddSub_Quality" runat="server" Width="100%" AutoPostBack="True"
                                                                OnSelectedIndexChanged="ddSub_Quality_SelectedIndexChanged" TabIndex="22" CssClass="dropdown">
                                                            </asp:DropDownList>
                                                            <cc1:ListSearchExtender ID="ListSearchExtender12" runat="server" TargetControlID="ddSub_Quality"
                                                                ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                                                            </cc1:ListSearchExtender>
                                                            <asp:Button ID="btnrefrshsubquality" runat="server" Height="0px" Width="0px" BackColor="White"
                                                                BorderColor="White" BorderWidth="0px" ForeColor="White" />
                                                        </td>
                                                        <td id="TdRemarks" runat="server" colspan="2" visible="false" class="tdstyle">
                                                            Description
                                                            <br />
                                                            <asp:TextBox ID="TxtRemarks" runat="server" CssClass="textb" Width="450px" TabIndex="26"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </div>
                                            <table>
                                                <tr>
                                                    <td>
                                                        <table>
                                                            <tr id="Tr2" runat="server">
                                                                <td class="tdstyle">
                                                                    <b>ISSUE ITEM</b>
                                                                </td>
                                                                <td class="tdstyle" colspan="2">
                                                                    <b>
                                                                        <asp:CheckBox ID="ChkForProcessInPut" runat="server" AutoPostBack="True" OnCheckedChanged="ChkForProcessInPut_CheckedChanged"
                                                                            TabIndex="27" Text="ChkForProcessInPut" />
                                                                </td>
                                                                <td class="tdstyle" id="INPROCESSNAME" runat="server" visible="false" colspan="3">
                                                                    Process Name
                                                                    <asp:DropDownList ID="ddInProcessName" runat="server" Width="150px" AutoPostBack="True"
                                                                        CssClass="dropdown" OnSelectedIndexChanged="ddInProcessName_SelectedIndexChanged"
                                                                        TabIndex="28">
                                                                    </asp:DropDownList>
                                                                    <cc1:ListSearchExtender ID="ListSearchExtender13" runat="server" TargetControlID="ddInProcessName"
                                                                        ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                                                                    </cc1:ListSearchExtender>
                                                                </td>
                                                                <td>
                                                                    <asp:Button ID="btnopen" runat="server" Text="DefineItemCode" Width="110px" CssClass="buttonnorm"
                                                                        OnClientClick=" return DefineItemcode()" />
                                                                </td>
                                                                <td>
                                                                    <asp:Button ID="BtnOpenOtherExpense" runat="server" Text="OtherExpense" CssClass="buttonnorm"
                                                                        OnClientClick=" return OtherExpense()" Width="100px" />
                                                                </td>
                                                                <td>
                                                                    <asp:Button ID="BtnOpenPackingCost" runat="server" Text="Packing Cost" CssClass="buttonnorm"
                                                                        OnClientClick=" return PackingCost()" Width="100px" />
                                                                </td>
                                                                <td class="tdstyle">
                                                                    <asp:Label ID="LblQty" runat="server" Text="Qty" Font-Bold="false" ForeColor="Red"></asp:Label>
                                                                </td>
                                                            </tr>
                                                            <tr id="Tr1" runat="server">
                                                                <td class="tdstyle">
                                                                    <asp:Label ID="LblInItemCode" runat="server" Text="Item Code"></asp:Label>
                                                                    <br />
                                                                    <asp:TextBox ID="TxtInProdCode" runat="server" CssClass="textb" Width="100px" AutoPostBack="True"
                                                                        OnTextChanged="TxtInProdCode_TextChanged" TabIndex="29"></asp:TextBox>
                                                                    <cc1:AutoCompleteExtender ID="AutoCompleteExtender1" runat="server" EnableCaching="true"
                                                                        Enabled="True" MinimumPrefixLength="1" ServiceMethod="GetQuality" TargetControlID="TxtInProdCode"
                                                                        UseContextKey="True">
                                                                    </cc1:AutoCompleteExtender>
                                                                </td>
                                                                <td class="tdstyle">
                                                                    <asp:Label ID="lblincategoryname" runat="server" Text="Category Name"></asp:Label>
                                                                    &nbsp;<br />
                                                                    <asp:DropDownList ID="ddInCategoryName" CssClass="dropdown" runat="server" Width="150px"
                                                                        AutoPostBack="True" OnSelectedIndexChanged="ddInCategoryName_SelectedIndexChanged"
                                                                        TabIndex="30">
                                                                    </asp:DropDownList>
                                                                    <cc1:ListSearchExtender ID="ListSearchExtender14" runat="server" TargetControlID="ddInCategoryName"
                                                                        ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                                                                    </cc1:ListSearchExtender>
                                                                </td>
                                                                <td class="tdstyle">
                                                                    <asp:Label ID="lblinitemname" runat="server" Text="Item Name"></asp:Label>
                                                                    <br />
                                                                    <asp:DropDownList ID="ddInItemName" runat="server" Width="150px" AutoPostBack="True"
                                                                        CssClass="dropdown" OnSelectedIndexChanged="ddInItemName_SelectedIndexChanged"
                                                                        TabIndex="31">
                                                                    </asp:DropDownList>
                                                                    <cc1:ListSearchExtender ID="ListSearchExtender15" runat="server" TargetControlID="ddInItemName"
                                                                        ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                                                                    </cc1:ListSearchExtender>
                                                                </td>
                                                                <td class="tdstyle" id="InQuality" runat="server" visible="false" colspan="2">
                                                                    <asp:Label ID="lblinqualityname" runat="server" Text="Quality"></asp:Label>
                                                                    &nbsp;<br />
                                                                    <asp:DropDownList ID="ddInQuality" runat="server" Width="150px" TabIndex="32" AutoPostBack="True"
                                                                        CssClass="dropdown" OnSelectedIndexChanged="ddInQuality_SelectedIndexChanged">
                                                                    </asp:DropDownList>
                                                                    <cc1:ListSearchExtender ID="ListSearchExtender16" runat="server" TargetControlID="ddInQuality"
                                                                        ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                                                                    </cc1:ListSearchExtender>
                                                                </td>
                                                                <td class="tdstyle" id="InDesign" runat="server" visible="false">
                                                                    <asp:Label ID="lblindesignname" runat="server" Text="Design"></asp:Label>
                                                                    <br />
                                                                    <asp:DropDownList ID="ddInDesign" runat="server" Width="150px" TabIndex="33" CssClass="dropdown">
                                                                    </asp:DropDownList>
                                                                    <cc1:ListSearchExtender ID="ListSearchExtender17" runat="server" TargetControlID="ddInDesign"
                                                                        ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                                                                    </cc1:ListSearchExtender>
                                                                </td>
                                                                <td class="tdstyle" id="InColor" runat="server" visible="false">
                                                                    <asp:Label ID="lblincolorname" runat="server" Text="Color"></asp:Label>
                                                                    <br />
                                                                    <asp:DropDownList ID="ddInColor" runat="server" Width="100px" TabIndex="34" CssClass="dropdown">
                                                                    </asp:DropDownList>
                                                                    <cc1:ListSearchExtender ID="ListSearchExtender18" runat="server" TargetControlID="ddInColor"
                                                                        ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                                                                    </cc1:ListSearchExtender>
                                                                </td>
                                                                <td class="tdstyle" id="InShape" runat="server" visible="false">
                                                                    <asp:Label ID="lblinshapename" runat="server" Text="Shape"></asp:Label>
                                                                    <br />
                                                                    <asp:DropDownList ID="ddInShape" runat="server" Width="100px" AutoPostBack="True"
                                                                        CssClass="dropdown" TabIndex="35" OnSelectedIndexChanged="ddInShape_SelectedIndexChanged">
                                                                    </asp:DropDownList>
                                                                    <cc1:ListSearchExtender ID="ListSearchExtender19" runat="server" TargetControlID="ddInShape"
                                                                        ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                                                                    </cc1:ListSearchExtender>
                                                                </td>
                                                                <td class="tdstyle" id="InSize" runat="server" visible="false">
                                                                    <asp:Label ID="lblinsizename" runat="server" Text="Size"></asp:Label>
                                                                    <br />
                                                                    <asp:DropDownList ID="ddInSize" runat="server" Width="100px" TabIndex="36" AutoPostBack="True"
                                                                        CssClass="dropdown">
                                                                    </asp:DropDownList>
                                                                    <cc1:ListSearchExtender ID="ListSearchExtender20" runat="server" TargetControlID="ddInSize"
                                                                        ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                                                                    </cc1:ListSearchExtender>
                                                                </td>
                                                                <td class="tdstyle" id="InShade" runat="server" visible="false">
                                                                    <asp:Label ID="lblinshade" runat="server" Text="Shade"></asp:Label>
                                                                    <asp:Button ID="btnshade" runat="server" CssClass="buttonsmall" OnClientClick="return AddShade();"
                                                                        Height="16px" Text=".." TabIndex="38" />
                                                                    <asp:Button ID="btnrefreshshadecolorform" runat="server" Height="0px" Width="0px"
                                                                        BackColor="White" BorderColor="White" BorderWidth="0px" ForeColor="White" OnClick="btnrefreshshadecolorform_Click" />
                                                                    <br />
                                                                    <asp:DropDownList ID="ddInShade" runat="server" Width="100px" TabIndex="37" CssClass="dropdown">
                                                                    </asp:DropDownList>
                                                                    <cc1:ListSearchExtender ID="ListSearchExtender21" runat="server" TargetControlID="ddInShade"
                                                                        ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                                                                    </cc1:ListSearchExtender>
                                                                </td>
                                                            </tr>
                                                            <tr id="Tr3" runat="server">
                                                                <td class="tdstyle" align="right">
                                                                    Unit
                                                                </td>
                                                                <td class="tdstyle">
                                                                    <asp:DropDownList ID="ddIUnit" CssClass="dropdown" runat="server" TabIndex="39">
                                                                    </asp:DropDownList>
                                                                    <cc1:ListSearchExtender ID="ListSearchExtender22" runat="server" TargetControlID="ddIUnit"
                                                                        ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                                                                    </cc1:ListSearchExtender>
                                                                    <asp:Label ID="LblIFinish" runat="server" Text="Finish"></asp:Label>
                                                                    <asp:Button ID="refreshfinishedin" runat="server" Height="0px" Width="0px" BackColor="White"
                                                                        BorderColor="White" BorderWidth="0px" ForeColor="White" OnClick="refreshfinishedin_Click" />
                                                                </td>
                                                                <td>
                                                                    <asp:DropDownList ID="ddFinishedIN" runat="server" Width="60%" TabIndex="40" CssClass="dropdown">
                                                                    </asp:DropDownList>
                                                                    <cc1:ListSearchExtender ID="ListSearchExtender23" runat="server" TargetControlID="ddFinishedIN"
                                                                        ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                                                                    </cc1:ListSearchExtender>
                                                                </td>
                                                                <td class="tdstyle">
                                                                    Cal Type
                                                                </td>
                                                                <td>
                                                                    <asp:DropDownList CssClass="dropdown" ID="DDICALTYPE" runat="server" Width="90px"
                                                                        TabIndex="41">
                                                                        <asp:ListItem Value="0">Area Wise</asp:ListItem>
                                                                        <asp:ListItem Value="1">Pcs Wise</asp:ListItem>
                                                                        <asp:ListItem Value="2">W-2 X L-2</asp:ListItem>
                                                                        <asp:ListItem Value="3">W-2</asp:ListItem>
                                                                        <asp:ListItem Value="4">L-2</asp:ListItem>
                                                                    </asp:DropDownList>
                                                                </td>
                                                                <td class="tdstyle">
                                                                    Consmp Qty
                                                                    <asp:TextBox ID="TxtInPutQty" runat="server" Width="80px" OnTextChanged="TxtInPutQty_TextChanged"
                                                                        AutoPostBack="True" CssClass="textb" TabIndex="42"></asp:TextBox>
                                                                </td>
                                                                <td class="tdstyle">
                                                                    Loss
                                                                    <asp:TextBox ID="TxtLoss" CssClass="textb" runat="server" Width="63px" TabIndex="43"
                                                                        AutoPostBack="True" OnTextChanged="TxtLoss_TextChanged"></asp:TextBox>
                                                                </td>
                                                                <td class="tdstyle" align="right">
                                                                    Rate
                                                                    <asp:TextBox ID="TxtInPutRate" CssClass="textb" runat="server" Width="48px" TabIndex="44"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                    <td align="justify">
                                                        <div style="height: 50%">
                                                            <asp:GridView ID="DGInPutProcess" runat="server" AutoGenerateColumns="False" DataKeyNames="PCMDID"
                                                                CssClass="grid-view" OnRowCreated="DGInPutProcess_RowCreated">
                                                                <HeaderStyle CssClass="gvheader" />
                                                                <AlternatingRowStyle CssClass="gvalt" />
                                                                <RowStyle CssClass="gvrow" />
                                                                <EmptyDataRowStyle CssClass="gvemptytext" />
                                                                <Columns>
                                                                    <asp:BoundField DataField="PCMDID" HeaderText="Sr No">
                                                                        <ItemStyle HorizontalAlign="Center" />
                                                                    </asp:BoundField>
                                                                    <asp:BoundField DataField="Description" HeaderText="Description">
                                                                        <ControlStyle Height="17px" />
                                                                        <HeaderStyle Width="200px" />
                                                                        <ItemStyle HorizontalAlign="Center" />
                                                                    </asp:BoundField>
                                                                    <asp:BoundField DataField="PreQty" HeaderText="PreQty">
                                                                        <ItemStyle HorizontalAlign="Center" />
                                                                    </asp:BoundField>
                                                                    <asp:TemplateField HeaderText="Qty">
                                                                        <ItemTemplate>
                                                                            <asp:TextBox ID="txtp_tage" CssClass="textb" runat="server" Width="70px" Text='<%# Bind("Qty") %>'
                                                                                AutoPostBack="true"></asp:TextBox>
                                                                        </ItemTemplate>
                                                                        <HeaderStyle Width="80px" />
                                                                        <ItemStyle HorizontalAlign="Center" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Loss">
                                                                        <ItemTemplate>
                                                                            <asp:TextBox ID="txtDGLoss" CssClass="textb" runat="server" Width="70px" Text='<%# Bind("Loss") %>'
                                                                                AutoPostBack="true"></asp:TextBox>
                                                                        </ItemTemplate>
                                                                        <HeaderStyle Width="80px" />
                                                                        <ItemStyle HorizontalAlign="Center" />
                                                                    </asp:TemplateField>
                                                                </Columns>
                                                            </asp:GridView>
                                                        </div>
                                                        <div style="height: 100%; width: 100%">
                                                        </div>
                                                    </td>
                                                </tr>
                                            </table>
                                            <table id="tab" runat="server">
                                                <tr id="TrOUT" runat="server">
                                                    <td class="tdstyle">
                                                        <b>RECEIVE ITEM </b>
                                                    </td>
                                                    <td class="tdstyle">
                                                        <b>
                                                            <asp:CheckBox ID="ChkForFillSame" runat="server" Text="For Fill Same" OnCheckedChanged="ChkForFillSame_CheckedChanged"
                                                                AutoPostBack="True" />
                                                    </td>
                                                    <td colspan="2" class="tdstyle">
                                                        <b>
                                                            <asp:CheckBox ID="ChkForManyOutPut" runat="server" AutoPostBack="True" OnCheckedChanged="ChkForManyOutPut_CheckedChanged"
                                                                TabIndex="45" Text="ChkForManyOutPut" />
                                                    </td>
                                                    <td colspan="2" class="tdstyle">
                                                        <b>
                                                            <asp:CheckBox ID="ChkForOneToOne" runat="server" AutoPostBack="True" OnCheckedChanged="ChkForOneToOne_CheckedChanged"
                                                                TabIndex="46" Text="ChkForOneToOne" />
                                                    </td>
                                                </tr>
                                                <tr id="TrOUT1" runat="server">
                                                    <td class="tdstyle">
                                                        <asp:Label ID="LblOutItemCode" runat="server" Text="Item Code"></asp:Label>
                                                        <br />
                                                        <asp:TextBox ID="TxtOutProdCode" runat="server" CssClass="textb" Width="100px" AutoPostBack="True"
                                                            OnTextChanged="TxtOutProdCode_TextChanged" TabIndex="47"></asp:TextBox>
                                                        <cc1:AutoCompleteExtender ID="AutoCompleteExtender2" runat="server" EnableCaching="true"
                                                            Enabled="True" MinimumPrefixLength="1" ServiceMethod="GetQuality" TargetControlID="TxtOutProdCode"
                                                            UseContextKey="True">
                                                        </cc1:AutoCompleteExtender>
                                                    </td>
                                                    <td class="tdstyle">
                                                        <asp:Label ID="lbloutcategoryname" runat="server" Text="Category Name"></asp:Label>
                                                        &nbsp;<br />
                                                        <asp:DropDownList ID="ddOutCategoryName" runat="server" Width="150px" AutoPostBack="True"
                                                            OnSelectedIndexChanged="ddOutCategoryName_SelectedIndexChanged" TabIndex="48"
                                                            CssClass="dropdown">
                                                        </asp:DropDownList>
                                                    </td>
                                                    <td class="tdstyle">
                                                        <asp:Label ID="lbloutitemname" runat="server" Text="Item Name"></asp:Label>
                                                        <br />
                                                        <asp:DropDownList ID="ddOutItemName" runat="server" Width="150px" AutoPostBack="True"
                                                            CssClass="dropdown" OnSelectedIndexChanged="ddOutItemName_SelectedIndexChanged"
                                                            TabIndex="49">
                                                        </asp:DropDownList>
                                                    </td>
                                                    <td id="OutQuality" runat="server" visible="false" class="tdstyle">
                                                        <asp:Label ID="lbloutqualiltlyname" runat="server" Text="Quality"></asp:Label>
                                                        &nbsp;<br />
                                                        <asp:DropDownList ID="ddOutQuality" runat="server" Width="150px" TabIndex="50" CssClass="dropdown">
                                                        </asp:DropDownList>
                                                    </td>
                                                    <td id="OutDesign" runat="server" visible="false" class="tdstyle">
                                                        <asp:Label ID="lbloutdesignname" runat="server" Text="Design"></asp:Label>
                                                        <br />
                                                        <asp:DropDownList ID="ddOutDesign" runat="server" Width="150px" TabIndex="51" CssClass="dropdown">
                                                        </asp:DropDownList>
                                                    </td>
                                                    <td id="OutColor" runat="server" visible="false" class="tdstyle">
                                                        <asp:Label ID="lbloutcolorname" runat="server" Text="Color"></asp:Label>
                                                        <br />
                                                        <asp:DropDownList ID="ddOutColor" runat="server" Width="100px" TabIndex="52" CssClass="dropdown">
                                                        </asp:DropDownList>
                                                    </td>
                                                    <td id="OutShape" runat="server" visible="false" class="tdstyle">
                                                        <asp:Label ID="lbloutshapename" runat="server" Text="Shape"></asp:Label>
                                                        <br />
                                                        <asp:DropDownList ID="ddOutShape" runat="server" Width="100px" AutoPostBack="True"
                                                            CssClass="dropdown" TabIndex="53" OnSelectedIndexChanged="ddOutShape_SelectedIndexChanged">
                                                        </asp:DropDownList>
                                                    </td>
                                                    <td id="OutSize" runat="server" visible="false" class="tdstyle">
                                                        <asp:Label ID="lbloutsizename" runat="server" Text="Size"></asp:Label>
                                                        <br />
                                                        <asp:DropDownList ID="ddOutSize" runat="server" Width="100px" TabIndex="54" AutoPostBack="True"
                                                            CssClass="dropdown">
                                                        </asp:DropDownList>
                                                    </td>
                                                    <td id="OutShade" runat="server" visible="false" class="tdstyle">
                                                        <asp:Label ID="lbloutshadename" runat="server" Text="Shade"></asp:Label>
                                                        <br />
                                                        <asp:DropDownList ID="ddOutShade" runat="server" Width="100px" TabIndex="55" CssClass="dropdown">
                                                        </asp:DropDownList>
                                                    </td>
                                                </tr>
                                                <tr id="TrOUT2" runat="server">
                                                    <td class="tdstyle" align="right">
                                                        Unit
                                                    </td>
                                                    <td>
                                                        <asp:DropDownList ID="ddOUnit" runat="server" TabIndex="56" CssClass="dropdown">
                                                        </asp:DropDownList>
                                                    </td>
                                                    <td colspan="2" class="tdstyle">
                                                        <asp:Label ID="LblOFinish" runat="server" Text="Finish"></asp:Label>
                                                        <asp:DropDownList ID="ddFinished" runat="server" Width="60%" TabIndex="57" CssClass="dropdown">
                                                        </asp:DropDownList>
                                                        <asp:Button ID="Btn_FINIE_TYPE" runat="server" CssClass="buttonsmall" OnClientClick="return ADD_FINISH();"
                                                            Height="15px" Text=".." TabIndex="58" />
                                                    </td>
                                                    <td align="right" class="tdstyle">
                                                        Cal Type
                                                        <asp:DropDownList ID="ddOCalType" runat="server" Width="100px" OnSelectedIndexChanged="ddCalType_SelectedIndexChanged"
                                                            TabIndex="59" CssClass="dropdown">
                                                            <asp:ListItem Value="0">Area Wise</asp:ListItem>
                                                            <asp:ListItem Value="1">Pcs Wise</asp:ListItem>
                                                            <asp:ListItem Value="2">W-2 X L-2</asp:ListItem>
                                                            <asp:ListItem Value="3">W-2</asp:ListItem>
                                                            <asp:ListItem Value="4">L-2</asp:ListItem>
                                                        </asp:DropDownList>
                                                    </td>
                                                    <td class="tdstyle">
                                                        RecQty
                                                        <asp:TextBox ID="TxtOutPutQty" runat="server" Width="75px" AutoPostBack="True" OnTextChanged="TxtOutPutQty_TextChanged"
                                                            TabIndex="60" CssClass="textb"></asp:TextBox>
                                                    </td>
                                                    <td class="tdstyle">
                                                        Loss
                                                        <asp:TextBox ID="TxtRecLoss" CssClass="textb" runat="server" Width="60px" TabIndex="43"></asp:TextBox>
                                                    </td>
                                                    <td colspan="2" class="tdstyle">
                                                        Rate
                                                        <asp:TextBox ID="TxtOutPutRate" runat="server" Width="60px" TabIndex="61" CssClass="textb"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="6" align="left">
                                                        <asp:Label ID="lblMessage" ForeColor="Red" runat="server" Text="...."></asp:Label>
                                                    </td>
                                                    <td colspan="3">
                                                        <asp:Button ID="BtnSave" runat="server" Text="Save" CssClass="buttonnorm" OnClick="BtnSave_Click"
                                                            OnClientClick="return confirm('Do you want to save data?')" TabIndex="62" />
                                                        &nbsp;<asp:Button ID="BtnNew" runat="server" Text="New" CssClass="buttonnorm" OnClick="BtnNew_Click1" />
                                                        &nbsp;<asp:Button ID="BtnPreview" runat="server" CssClass="buttonnorm" Text="Preview"
                                                            Visible="false" OnClientClick="return priview();" />
                                                        &nbsp;<asp:Button ID="BtncClose" runat="server" CssClass="buttonnorm" Text="Close"
                                                            OnClientClick="return CloseForm();" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="9">
                                                        <div style="width: 90%; height: 200px; overflow: scroll;">
                                                            <asp:GridView ID="DG" runat="server" DataKeyNames="PCMDID" OnRowDataBound="DG_RowDataBound"
                                                                OnSelectedIndexChanged="DG_SelectedIndexChanged" AutoGenerateColumns="False"
                                                                CssClass="grid-view" OnRowCreated="DG_RowCreated" OnRowDeleting="DG_RowDeleting">
                                                                <Columns>
                                                                    <asp:BoundField DataField="ITEMCODE" HeaderText="ITEM CODE">
                                                                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                                        <ItemStyle Width="250px" HorizontalAlign="Center" VerticalAlign="Middle" />
                                                                    </asp:BoundField>
                                                                    <asp:BoundField DataField="PROCESS_NAME" HeaderText="PROCESS_NAME">
                                                                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                                        <ItemStyle Width="120px" HorizontalAlign="Center" VerticalAlign="Middle" />
                                                                    </asp:BoundField>
                                                                    <asp:BoundField DataField="INPUT_ITEM" HeaderText="INPUT_ITEM">
                                                                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                                        <ItemStyle Width="250px" HorizontalAlign="Center" VerticalAlign="Middle" />
                                                                    </asp:BoundField>
                                                                    <asp:BoundField DataField="IQTY" HeaderText="IQTY">
                                                                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                                        <ItemStyle Width="50px" HorizontalAlign="Center" VerticalAlign="Middle" />
                                                                    </asp:BoundField>
                                                                    <asp:BoundField DataField="ILOSS" HeaderText="ILOSS">
                                                                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                                        <ItemStyle Width="50px" HorizontalAlign="Center" VerticalAlign="Middle" />
                                                                    </asp:BoundField>
                                                                    <asp:BoundField DataField="IRATE" HeaderText="IRATE">
                                                                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                                        <ItemStyle Width="50px" HorizontalAlign="Center" VerticalAlign="Middle" />
                                                                    </asp:BoundField>
                                                                    <asp:BoundField DataField="I_UNIT" HeaderText="I_UNIT">
                                                                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                                        <ItemStyle Width="50px" HorizontalAlign="Center" VerticalAlign="Middle" />
                                                                    </asp:BoundField>
                                                                    <asp:BoundField DataField="OUTPUT_ITEM" HeaderText="OUTPUT_ITEM">
                                                                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                                        <ItemStyle Width="250px" HorizontalAlign="Center" VerticalAlign="Middle" />
                                                                    </asp:BoundField>
                                                                    <asp:BoundField DataField="OQTY" HeaderText="OQTY">
                                                                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                                        <ItemStyle Width="50px" HorizontalAlign="Center" VerticalAlign="Middle" />
                                                                    </asp:BoundField>
                                                                    <asp:BoundField DataField="ORATE" HeaderText="ORATE">
                                                                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                                        <ItemStyle Width="50px" HorizontalAlign="Center" VerticalAlign="Middle" />
                                                                    </asp:BoundField>
                                                                    <asp:BoundField DataField="O_UNIT" HeaderText="O_UNIT">
                                                                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                                        <ItemStyle Width="50px" HorizontalAlign="Center" VerticalAlign="Middle" />
                                                                    </asp:BoundField>
                                                                    <asp:BoundField DataField="QUALITYCODEID" HeaderText="QUALITYCODEID">
                                                                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                                        <ItemStyle Width="50px" HorizontalAlign="Center" VerticalAlign="Middle" />
                                                                    </asp:BoundField>
                                                                    <asp:TemplateField ShowHeader="False">
                                                                        <ItemTemplate>
                                                                            <asp:LinkButton ID="BtnDelete" runat="server" CausesValidation="False" CommandName="Delete"
                                                                                Text="DEL" OnClientClick="return confirm('Do You Want To Delete Data?')"></asp:LinkButton>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                </Columns>
                                                                <SelectedRowStyle CssClass="SelectedRowStyle" />
                                                            </asp:GridView>
                                                        </div>
                                                    </td>
                                                </tr>
                                            </table>
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
    </div>
    </form>
</body>
</html>
