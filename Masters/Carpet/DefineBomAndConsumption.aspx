<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DefineBomAndConsumption.aspx.cs"
    EnableSessionState="True" EnableViewState="true" Inherits="Masters_Carpet_DefineBomAndConsumption"
    EnableEventValidation="false" Title="Define Consumption" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register Src="~/UserControls/ucmenu.ascx" TagName="ucmenu" TagPrefix="uc1" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <link href="../../Styles/vijay.css" rel="stylesheet" type="text/css" />
    <script src="../../Scripts/JScript.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery-1.4.1.min.js" type="text/javascript"></script>
    <%--<script type="text/javascript" src="../../Scripts/Fixfocus.js"></script>--%>
    <script type="text/javascript">
        function priview() {
            //document.getElementById('BtnPreview').click();
            window.open('../../ReportViewer.aspx');
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

        function ValidateCtrl() {

            if (document.getElementById('ddCategoryName').value == "0") {
                alert("Please Select Category!");
                document.getElementById('ddCategoryName').focus();
                return false;
            }
            else if (document.getElementById('ddItemName').value == "0") {
                alert("Please Select Item! ");
                document.getElementById('ddItemName').focus();
                return false;
            }
            else if (document.getElementById('ddQuality')) {
                if (document.getElementById('ddQuality').value == "0") {
                    alert("Please Select Quality!");
                    document.getElementById('ddQuality').focus();
                    return false;
                }
            }
            if (document.getElementById('ddDesign')) {
                if (document.getElementById('ddDesign').value == "0" && document.getElementById('ddDesign').value == true) {
                    alert("Please Select Design!");
                    document.getElementById('ddDesign').focus();
                    return false;
                }
            }
            else if (document.getElementById('ddColor')) {
                if (document.getElementById('ddColor').value == "0" && document.getElementById('ddColor').value == true) {
                    alert("Please Select Color! ");
                    document.getElementById('ddColor').focus();
                    return false;
                }
            }

            return confirm('Do you want to view defined Consumption?')
        }
        function CloseForm() {
            if (window.opener.document.getElementById('refreshitem')) {
                window.opener.document.getElementById('refreshitem').click();
                self.close();
            }
            else {
                window.location.href = "../../main.aspx";
            }
        }
        function DefineItemcode() {
            var varcode = document.getElementById('TxtProdCode').value;
            window.open('DefineItemCode.aspx?ProdCode=' + varcode + '&ABC=1', '', 'toolbar=0, titlebar=1,  top=100spx, left=95px, scrollbars=1, resizable = yes,width=750px,Height=500px');
        }
        function OtherExpense() {
            var varcode = document.getElementById('TxtProdCode').value;
            window.open('OtherExpense.aspx?ProdCode=' + varcode + '', '', 'width=600px,Height=300px, left=200px, top=120px;menubar=no');
        }
        function PackingCost() {
            var varcode = document.getElementById('TxtProdCode').value;
            window.open('PackingAndOtherCost.aspx?ProdCode=' + varcode + '', '', 'width=900px,Height=350px, left=200px, top=120px;menubar=no');
        }
        function Attachconsumption() {
            window.open('FrmAttachConsumption.aspx', '', 'width=790px,Height=520px,left=180px,top=120px;menubar=no');
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
                var varcode = document.getElementById('ddCategoryName').value;
                var varItcode = document.getElementById('ddItemName').value;
                window.open('AddQuality.aspx?Category=' + varcode + '&Item=' + varItcode + '', '', 'width=701px,Height=501px');
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
                window.open('AddShape.aspx', '', 'width=500px,Height=401px');
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
        function CheckAllCheckBoxes(headercheck, obj) {
            //            document.getElementById('chkAll').checked == true
            if (headercheck.checked) {
                var gvcheck = document.getElementById(obj);
                var i;
                for (i = 1; i < gvcheck.rows.length; i++) {
                    var inputs = gvcheck.rows[i].getElementsByTagName('input');
                    inputs[0].checked = true;
                }
            }
            else {
                //DGInPutProcess
                var gvcheck = document.getElementById(obj);
                var i;
                for (i = 1; i < gvcheck.rows.length; i++) {
                    var inputs = gvcheck.rows[i].getElementsByTagName('input');
                    inputs[0].checked = false;
                }
            }
        }
        function validationsave() {
            var k = 0;
            for (i = 1; i < document.getElementById('DGInPutProcess').rows.length; i++) {
                var inputs = document.getElementById('DGInPutProcess').rows[i].getElementsByTagName('input');
                if (inputs[0].checked == true) {
                    k = k + 1;
                    i = document.getElementById('DGInPutProcess').rows.length;
                }
            }
            if (k == 0) {
                alert("Please select atleast one check box....!");
                return false;
            }
            return confirm('Do You Want To Save?')
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <asp:UpdatePanel runat="server" ID="update1">
        <ContentTemplate>
            <div>
                <table>
                    <tr id="zzz" runat="server">
                        <td>
                            <table width="95%">
                                <tr style="width: 40%" align="center">
                                    <td height="66px" align="center">
                                        <%--style="background-image:url(Images/header.jpg)" --%>
                                        <%--<div><img src="Images/header.jpg" alt="" /></div>--%>
                                        <asp:Image ID="Image1" ImageUrl="~/Images/header.jpg" runat="server" />
                                    </td>
                                    <td style="background-color: #0080C0;" width="80px" valign="bottom">
                                        <asp:Image ID="imgLogo" align="left" runat="server" Height="66px" Width="80px" />
                                        <span style="color: Black; font-family: Arial; font-size: large"><strong><em><i><font
                                            size="3" face="GEORGIA">
                                            <asp:Label ID="LblCompanyName" runat="server" Text=""></asp:Label></font></i> </em>
                                        </strong></span>
                                        <br />
                                        <i><font size="2" face="GEORGIA">
                                            <asp:Label ID="LblUserName" ForeColor="White" runat="server" Text=""></asp:Label></font>
                                        </i>
                                    </td>
                                </tr>
                                <tr bgcolor="#999999">
                                    <td class="style1">
                                        <uc1:ucmenu ID="ucmenu1" runat="server" />
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
                </table>
            </div>
            <div>
                <table>
                    <tr id="Tr5">
                        <td class="tdstyle">
                            <asp:Label ID="lblItemCode" runat="server" Text="Item Code" CssClass="labelbold"></asp:Label>
                            <br />
                            <asp:TextBox ID="TxtProdCode" runat="server" CssClass="textb" Width="90px" AutoPostBack="True"
                                OnTextChanged="TxtProdCode_TextChanged"></asp:TextBox>
                            <cc1:AutoCompleteExtender ID="TxtProdCode_AutoCompleteExtender" runat="server" EnableCaching="true"
                                Enabled="True" MinimumPrefixLength="1" ServiceMethod="GetQuality1" TargetControlID="TxtProdCode"
                                UseContextKey="True">
                            </cc1:AutoCompleteExtender>
                        </td>
                        <td class="tdstyle">
                            <asp:Label ID="lblcategoryname" runat="server" Text="Category Name" CssClass="labelbold"></asp:Label>
                            <asp:Button ID="refreshcategory" runat="server" Height="0px" OnClick="refreshcategory_Click"
                                Text="." Style="display: none" />
                            &nbsp;<asp:Button ID="btnaddcategory" runat="server" CssClass="buttonsmalls" OnClientClick="return Addcategory();"
                                Height="16px" Text=".." Width="20px" />
                            <br />
                            <asp:DropDownList ID="ddCategoryName" runat="server" OnSelectedIndexChanged="ddcategoryname_SelectedIndexChanged"
                                Width="120px" AutoPostBack="True" CssClass="dropdown">
                            </asp:DropDownList>
                            <cc1:ListSearchExtender ID="ListSearchExtender2" runat="server" TargetControlID="ddCategoryName"
                                ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                            </cc1:ListSearchExtender>
                        </td>
                        <td class="tdstyle">
                            <asp:Label ID="lblitemname" runat="server" Text="Item Name" CssClass="labelbold"></asp:Label>
                            <asp:Button ID="refreshitem" runat="server" Style="display: none" OnClick="refreshitem_Click" />
                            &nbsp;<asp:Button ID="btnadditem" runat="server" CssClass="buttonsmalls" OnClientClick="return Additem();"
                                Height="15px" Text=".." Width="20px" />
                            <br />
                            <asp:DropDownList ID="ddItemName" runat="server" OnSelectedIndexChanged="dditemname_SelectedIndexChanged"
                                Width="150px" AutoPostBack="True" CssClass="dropdown">
                            </asp:DropDownList>
                            <cc1:ListSearchExtender ID="ListSearchExtender1" runat="server" TargetControlID="ddItemName"
                                ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                            </cc1:ListSearchExtender>
                        </td>
                        <td id="Quality" runat="server" visible="false" class="tdstyle">
                            <asp:Label ID="lblqualityname" runat="server" Text="Quality" CssClass="labelbold"></asp:Label>
                            <asp:Button ID="refreshquality" runat="server" Style="display: none" OnClick="refreshquality_Click" />
                            &nbsp;<asp:Button ID="btnaddquality" runat="server" Width="20px" CssClass="buttonsmalls"
                                OnClientClick="return Addquality();" Height="15px" Text=".." />
                            <br />
                            <asp:DropDownList ID="ddQuality" runat="server" Width="200px" AutoPostBack="True"
                                CssClass="dropdown" OnSelectedIndexChanged="ddQuality_SelectedIndexChanged">
                            </asp:DropDownList>
                            <cc1:ListSearchExtender ID="ListSearchExtender3" runat="server" TargetControlID="ddQuality"
                                ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                            </cc1:ListSearchExtender>
                        </td>
                        <td id="Design" runat="server" visible="false" class="tdstyle">
                            <asp:Label ID="lbldesignname" runat="server" Text="Design" CssClass="labelbold"></asp:Label>
                            <asp:Button ID="refreshdesign" runat="server" Style="display: none" OnClick="refreshdesign_Click" />
                            &nbsp;<asp:Button ID="btnadddesign" runat="server" Width="20px" CssClass="buttonsmalls"
                                OnClientClick="return Adddesign();" Height="15px" Text=".." />
                            <asp:CheckBox ID="CHKFORALLDESIGN" runat="server" AutoPostBack="True" OnCheckedChanged="CHKFORALLDESIGN_CheckedChanged" />
                            <br />
                            <asp:DropDownList ID="ddDesign" runat="server" Width="200px" AutoPostBack="True"
                                CssClass="dropdown" OnSelectedIndexChanged="ddDesign_SelectedIndexChanged">
                            </asp:DropDownList>
                            <cc1:ListSearchExtender ID="ListSearchExtender4" runat="server" TargetControlID="ddDesign"
                                ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                            </cc1:ListSearchExtender>
                        </td>
                        <td id="Color" runat="server" visible="false" class="tdstyle">
                            <asp:Label ID="lblcolorname" runat="server" Text="Color" CssClass="labelbold"></asp:Label>
                            <asp:Button ID="refreshcolor" runat="server" Style="display: none" OnClick="refreshcolor_Click" />
                            &nbsp;<asp:Button ID="btnaddcolor" runat="server" Width="20px" CssClass="buttonsmalls"
                                OnClientClick="return Addcolor();" Height="15px" Text=".." />
                            <asp:CheckBox ID="CHKFORALLCOLOR" runat="server" AutoPostBack="True" OnCheckedChanged="CHKFORALLCOLOR_CheckedChanged" />
                            <br />
                            <asp:DropDownList ID="ddColor" runat="server" Width="100px" AutoPostBack="True" CssClass="dropdown"
                                OnSelectedIndexChanged="ddColor_SelectedIndexChanged">
                            </asp:DropDownList>
                            <cc1:ListSearchExtender ID="ListSearchExtender5" runat="server" TargetControlID="ddColor"
                                ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                            </cc1:ListSearchExtender>
                        </td>
                        <td id="Shape" runat="server" visible="false" class="tdstyle">
                            <asp:Label ID="lblshapename" runat="server" Text="Shape" CssClass="labelbold"></asp:Label>
                            <asp:Button ID="refreshshape" runat="server" Style="display: none" OnClick="refreshshape_Click" />
                            &nbsp;<asp:Button ID="btnaddshape" runat="server" CssClass="buttonsmalls" OnClientClick="return Addshape();"
                                Height="15px" Text=".." Width="20px" />
                            <br />
                            <asp:DropDownList ID="ddShape" runat="server" Width="100px" AutoPostBack="True" OnSelectedIndexChanged="ddshape_SelectedIndexChanged"
                                CssClass="dropdown">
                            </asp:DropDownList>
                            <cc1:ListSearchExtender ID="ListSearchExtender6" runat="server" TargetControlID="ddShape"
                                ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                            </cc1:ListSearchExtender>
                        </td>
                        <td id="Size" runat="server" visible="false" class="tdstyle">
                            <asp:Label ID="lblsizename" runat="server" Text="Size" CssClass="labelbold"></asp:Label>
                            <asp:Button ID="refreshsize" runat="server" Style="display: none" OnClick="refreshsize_Click" />
                            &nbsp;<asp:Button ID="btnaddsize" runat="server" CssClass="buttonsmalls" OnClientClick="return AddsizeNew();"
                                Height="15px" Text=".." Width="20px" />
                            <asp:CheckBox ID="CHKFORALLSIZE" runat="server" AutoPostBack="True" OnCheckedChanged="CHKFORALLSIZE_CheckedChanged" />
                            <br />
                            <asp:DropDownList ID="ddSize" runat="server" Width="100px" AutoPostBack="True" CssClass="dropdown"
                                OnSelectedIndexChanged="ddSize_SelectedIndexChanged">
                            </asp:DropDownList>
                            <cc1:ListSearchExtender ID="ListSearchExtender7" runat="server" TargetControlID="ddSize"
                                ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                            </cc1:ListSearchExtender>
                            <asp:CheckBox ID="ChMeteerSize" Text="MtrSize" runat="server" OnCheckedChanged="ChMeteerSize_CheckedChanged"
                                AutoPostBack="True" Visible="false" />
                            <asp:DropDownList CssClass="dropdown" Width="50" ID="DDsizetype" runat="server" AutoPostBack="True"
                                OnSelectedIndexChanged="DDsizetype_SelectedIndexChanged" />
                        </td>
                        <td id="TDMtrSize" runat="server" visible="false" class="tdstyle">
                        </td>
                        <td id="Shade" runat="server" visible="false" class="tdstyle">
                            <asp:Label ID="lblshadename" runat="server" Text="Shade" CssClass="labelbold"></asp:Label>
                            <asp:Button ID="refreshshade" runat="server" Style="display: none" OnClick="refreshshade_Click" />
                            <asp:Button ID="btnaddshade" runat="server" CssClass="buttonsmalls" OnClientClick="return AddShade();"
                                Height="15px" Text=".." Width="20px" />
                            <br />
                            <asp:DropDownList ID="ddShade" runat="server" Width="100px" CssClass="dropdown">
                            </asp:DropDownList>
                            <cc1:ListSearchExtender ID="ListSearchExtender8" runat="server" TargetControlID="ddShade"
                                ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                            </cc1:ListSearchExtender>
                        </td>
                    </tr>
                </table>
                <table>
                    <tr>
                        <td>
                            <asp:TextBox ID="txtFinishedid" runat="server" CssClass="textb" Style="display: none"></asp:TextBox>
                            <asp:Button ID="btnrefreshprocess" runat="server" Style="display: none" OnClick="btnrefreshprocess_Click" />
                        </td>
                        <td id="LblSub_Quality" runat="server" align="right" class="tdstyle">
                            <asp:Label ID="Label1" runat="server" Text=" Sub_Quality" CssClass="labelbold"></asp:Label>
                        </td>
                        <td id="dddSub_Quality" runat="server">
                            <asp:DropDownList ID="ddSub_Quality" runat="server" Width="100%" AutoPostBack="True"
                                OnSelectedIndexChanged="ddSub_Quality_SelectedIndexChanged" CssClass="dropdown">
                            </asp:DropDownList>
                            <cc1:ListSearchExtender ID="ListSearchExtender9" runat="server" TargetControlID="ddSub_Quality"
                                ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                            </cc1:ListSearchExtender>
                            <asp:Button ID="btnrefrshsubquality" runat="server" Height="0px" Width="0px" BackColor="White"
                                BorderColor="White" BorderWidth="0px" ForeColor="White" />
                        </td>
                        <td align="right" class="tdstyle">
                            <asp:Button ID="btnsubquality" runat="server" CssClass="buttonsmalls" OnClientClick="return additemcode();"
                                Height="16px" Text=".." Width="20px" />
                            <asp:Button ID="btnrefreshsbqlt" runat="server" Style="display: none" OnClick="btnrefreshsbqlt_Click" />
                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                            <asp:Label ID="Label2" runat="server" Text=" Process Name" CssClass="labelbold"></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddProcessName" runat="server" Width="100px" CssClass="dropdown"
                                AutoPostBack="true" OnSelectedIndexChanged="ddProcessName_SelectedIndexChanged1">
                            </asp:DropDownList>
                            <cc1:ListSearchExtender ID="ListSearchExtender10" runat="server" TargetControlID="ddProcessName"
                                ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                            </cc1:ListSearchExtender>
                            &nbsp;&nbsp;&nbsp;
                            <asp:Button ID="Button1" runat="server" CssClass="buttonsmalls" OnClientClick="return AddProcess();"
                                Height="15px" Text=".." Width="20px" />
                        </td>
                        <td id="TdRemarks" runat="server" visible="false" class="tdstyle">
                            <asp:Label ID="Label3" runat="server" Text=" Description " CssClass="labelbold"></asp:Label>
                            &nbsp;&nbsp;&nbsp;
                            <asp:TextBox ID="TxtRemarks" runat="server" CssClass="textb" Width="350px"></asp:TextBox>
                        </td>
                        <td>
                            <div id="newPreview" runat="server">
                                <asp:Image ID="newPreview1" runat="server" Height="55px" Width="111px" />
                            </div>
                        </td>
                    </tr>
                </table>
            </div>
            <div style="border: 1px; border-style: solid; color: Black">
                <table>
                    <tr id="Tr2" runat="server">
                        <td class="tdstyle">
                            <b>
                                <asp:CheckBox ID="ChkForMtr" runat="server" Text="For Mtr" />
                                &nbsp;&nbsp;
                                <asp:CheckBox ID="ChkForLossPercentage" runat="server" Text="For Loss %" />
                                &nbsp;&nbsp;
                                <asp:CheckBox ID="ChkForProcessInPut" runat="server" AutoPostBack="True" OnCheckedChanged="ChkForProcessInPut_CheckedChanged"
                                    Text="ChkForProcessInPut" />
                                &nbsp;
                                <asp:LinkButton ID="BtnShow" runat="server" OnClientClick="return ValidateCtrl();"
                                    Text="Show Defined Data" OnClick="BtnShow_Click" />
                        </td>
                        <td class="tdstyle" id="INPROCESSNAME" runat="server" visible="false">
                            <asp:Label ID="Label4" runat="server" Text=" Process Name" CssClass="labelbold"></asp:Label>
                            <asp:DropDownList ID="ddInProcessName" runat="server" Width="100px" AutoPostBack="True"
                                CssClass="dropdown" OnSelectedIndexChanged="ddInProcessName_SelectedIndexChanged">
                            </asp:DropDownList>
                            <cc1:ListSearchExtender ID="ListSearchExtender11" runat="server" TargetControlID="ddInProcessName"
                                ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                            </cc1:ListSearchExtender>
                        </td>
                        <td>
                            <b>
                                <asp:LinkButton ID="btnopen" runat="server" Text="Define Item Code" OnClientClick=" return DefineItemcode()" />
                            </b>
                        </td>
                        <td>
                            <asp:Button ID="BtnOpenOtherExpense" runat="server" Text="OtherExpense" CssClass="buttonnorm"
                                OnClientClick=" return OtherExpense()" />
                        </td>
                        <td>
                            <asp:Button ID="BtnOpenPackingCost" runat="server" Text="Packing Cost" CssClass="buttonnorm"
                                OnClientClick=" return PackingCost()" />
                        </td>
                        <td id="TDAttachConsumption" runat="server">
                            <b>
                                <asp:LinkButton ID="BtnattachConsmp" runat="server" Text="Attach Consumption" OnClientClick="return Attachconsumption();" />
                            </b>
                        </td>
                        <td class="tdstyle">
                            <asp:Label ID="LblQty" runat="server" Text="Qty" Font-Bold="false" ForeColor="Red"
                                Style="display: none"></asp:Label>
                        </td>
                    </tr>
                </table>
            </div>
            <legend><b>Issue Item</b></legend>
            <table>
                <tr>
                    <td valign="top">
                        <table width="100%">
                            <tr id="Tr1" runat="server">
                                <td>
                                    <table>
                                        <tr>
                                            <td class="tdstyle">
                                                <asp:Label ID="LblInItemCode" runat="server" Text="Item Code" CssClass="labelbold"></asp:Label>
                                                <br />
                                                <asp:TextBox ID="TxtInProdCode" runat="server" CssClass="textb" Width="90px" AutoPostBack="True"
                                                    OnTextChanged="TxtInProdCode_TextChanged"></asp:TextBox>
                                                <cc1:AutoCompleteExtender ID="AutoCompleteExtender1" runat="server" EnableCaching="true"
                                                    Enabled="True" MinimumPrefixLength="1" ServiceMethod="GetQuality" TargetControlID="TxtInProdCode"
                                                    UseContextKey="True">
                                                </cc1:AutoCompleteExtender>
                                            </td>
                                            <td class="tdstyle">
                                                <asp:Label ID="lblincategoryname" runat="server" Text="Category Name" CssClass="labelbold"></asp:Label>
                                                &nbsp;<br />
                                                <asp:DropDownList ID="ddInCategoryName" CssClass="dropdown" runat="server" Width="150px"
                                                    AutoPostBack="True" OnSelectedIndexChanged="ddInCategoryName_SelectedIndexChanged">
                                                </asp:DropDownList>
                                                <cc1:ListSearchExtender ID="ListSearchExtender12" runat="server" TargetControlID="ddInCategoryName"
                                                    ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                                                </cc1:ListSearchExtender>
                                            </td>
                                            <td class="tdstyle">
                                                <asp:Label ID="lblinitemname" runat="server" Text="Item Name" CssClass="labelbold"></asp:Label>
                                                <br />
                                                <asp:DropDownList ID="ddInItemName" runat="server" Width="150px" AutoPostBack="True"
                                                    CssClass="dropdown" OnSelectedIndexChanged="ddInItemName_SelectedIndexChanged">
                                                </asp:DropDownList>
                                                <cc1:ListSearchExtender ID="ListSearchExtender13" runat="server" TargetControlID="ddInItemName"
                                                    ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                                                </cc1:ListSearchExtender>
                                            </td>
                                            <td class="tdstyle" id="InQuality" runat="server" visible="false">
                                                <asp:Label ID="lblinqualityname" runat="server" Text="Quality" CssClass="labelbold"></asp:Label>
                                                &nbsp;<br />
                                                <asp:DropDownList ID="ddInQuality" runat="server" Width="230px" AutoPostBack="True"
                                                    CssClass="dropdown" OnSelectedIndexChanged="ddInQuality_SelectedIndexChanged">
                                                </asp:DropDownList>
                                                <cc1:ListSearchExtender ID="ListSearchExtender14" runat="server" TargetControlID="ddInQuality"
                                                    ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                                                </cc1:ListSearchExtender>
                                            </td>
                                            <td class="tdstyle" id="InDesign" runat="server" visible="false">
                                                <asp:Label ID="lblindesignname" runat="server" Text="Design" CssClass="labelbold"></asp:Label>
                                                <br />
                                                <asp:DropDownList ID="ddInDesign" runat="server" Width="120px" CssClass="dropdown">
                                                </asp:DropDownList>
                                                <cc1:ListSearchExtender ID="ListSearchExtender15" runat="server" TargetControlID="ddInDesign"
                                                    ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                                                </cc1:ListSearchExtender>
                                            </td>
                                            <td class="tdstyle" id="InColor" runat="server" visible="false">
                                                <asp:Label ID="lblincolorname" runat="server" Text="Color" CssClass="labelbold"></asp:Label>
                                                <br />
                                                <asp:DropDownList ID="ddInColor" runat="server" Width="100px" CssClass="dropdown">
                                                </asp:DropDownList>
                                                <cc1:ListSearchExtender ID="ListSearchExtender16" runat="server" TargetControlID="ddInColor"
                                                    ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                                                </cc1:ListSearchExtender>
                                            </td>
                                            <td class="tdstyle" id="InShape" runat="server" visible="false">
                                                <asp:Label ID="lblinshapename" runat="server" Text="Shape" CssClass="labelbold"></asp:Label>
                                                <br />
                                                <asp:DropDownList ID="ddInShape" runat="server" Width="100px" AutoPostBack="True"
                                                    CssClass="dropdown" OnSelectedIndexChanged="ddInShape_SelectedIndexChanged">
                                                </asp:DropDownList>
                                                <cc1:ListSearchExtender ID="ListSearchExtender17" runat="server" TargetControlID="ddInShape"
                                                    ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                                                </cc1:ListSearchExtender>
                                            </td>
                                            <td class="tdstyle" id="InSize" runat="server" visible="false">
                                                <asp:Label ID="lblinsizename" runat="server" Text="Size" CssClass="labelbold"></asp:Label>
                                                <asp:CheckBox ID="CheckBox1" runat="server" Text="MTR Size" AutoPostBack="True" OnCheckedChanged="CHKFORISSUE_CheckedChanged"
                                                    Visible="false" />
                                                <asp:DropDownList CssClass="dropdown" Width="50" ID="DDInSizeType" runat="server"
                                                    AutoPostBack="True" OnSelectedIndexChanged="DDInSizeType_SelectedIndexChanged" />
                                                <br />
                                                <asp:DropDownList ID="ddInSize" runat="server" Width="100px" CssClass="dropdown">
                                                </asp:DropDownList>
                                                <cc1:ListSearchExtender ID="ListSearchExtender18" runat="server" TargetControlID="ddInSize"
                                                    ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                                                </cc1:ListSearchExtender>
                                            </td>
                                            <td class="tdstyle" id="InShade" runat="server" visible="false">
                                                <asp:Label ID="lblinshade" runat="server" Text="Shade" CssClass="labelbold"></asp:Label>
                                                <asp:Button ID="btnshade" runat="server" CssClass="buttonsmalls" OnClientClick="return AddShade();"
                                                    Height="16px" Text=".." Width="20px" />
                                                <asp:Button ID="btnrefreshshadecolorform" runat="server" Height="0px" Width="0px"
                                                    BackColor="White" BorderColor="White" BorderWidth="0px" ForeColor="White" OnClick="btnrefreshshadecolorform_Click" />
                                                <br />
                                                <asp:DropDownList ID="ddInShade" AppendDataBoundItems="true" runat="server" Width="200px"
                                                    CssClass="dropdown">
                                                </asp:DropDownList>
                                                <cc1:ListSearchExtender ID="ListSearchExtender19" runat="server" TargetControlID="ddInShade"
                                                    ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                                                </cc1:ListSearchExtender>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr id="Tr3" runat="server">
                                <td>
                                    <table>
                                        <tr>
                                            <td class="tdstyle">
                                                <asp:Label ID="Label5" runat="server" Text="Unit" CssClass="labelbold"></asp:Label><br />
                                                <asp:DropDownList ID="ddIUnit" CssClass="dropdown" runat="server" Width="90px">
                                                </asp:DropDownList>
                                                <asp:Button ID="refreshfinishedin" runat="server" OnClick="refreshfinishedin_Click"
                                                    Style="display: none" />
                                            </td>
                                            <td id="TDFinishtype" runat="server" visible="false">
                                                <asp:Label ID="LblIFinish" runat="server" Text="Finish"></asp:Label><br />
                                                <asp:DropDownList ID="ddFinishedIN" runat="server" CssClass="dropdown">
                                                </asp:DropDownList>
                                                <cc1:ListSearchExtender ID="ListSearchExtender20" runat="server" TargetControlID="ddFinishedIN"
                                                    ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                                                </cc1:ListSearchExtender>
                                            </td>
                                            <td class="tdstyle">
                                                <asp:Label ID="Label6" runat="server" Text="Cal Type" CssClass="labelbold"></asp:Label>
                                                <asp:DropDownList CssClass="dropdown" ID="DDICALTYPE" runat="server" Width="100px">
                                                    <asp:ListItem Value="0">Area Wise</asp:ListItem>
                                                    <asp:ListItem Value="1">Pcs Wise</asp:ListItem>
                                                    <asp:ListItem Value="2">W-2 X L-2</asp:ListItem>
                                                    <asp:ListItem Value="3">W-2</asp:ListItem>
                                                    <asp:ListItem Value="4">L-2</asp:ListItem>
                                                </asp:DropDownList>
                                            </td>
                                            <td class="tdstyle">
                                                <asp:Label ID="Label7" runat="server" Text="Consmp Qty" CssClass="labelbold"></asp:Label>
                                                <asp:TextBox ID="TxtInPutQty" runat="server" Width="75px" OnTextChanged="TxtInPutQty_TextChanged"
                                                    AutoPostBack="True" CssClass="textb" onkeypress="return isNumber(event);"></asp:TextBox>
                                            </td>
                                            <td class="tdstyle" id="TDIloss" runat="server">
                                                <asp:Label ID="Label8" runat="server" Text=" Loss" CssClass="labelbold"></asp:Label>
                                                <asp:TextBox ID="TxtLoss" CssClass="textb" runat="server" Width="63px" onkeypress="return isNumber(event);"
                                                    OnTextChanged="TxtLoss_TextChanged" AutoPostBack="True"></asp:TextBox>
                                            </td>
                                            <td class="tdstyle" align="right">
                                                <asp:Label ID="Label9" runat="server" Text=" Rate" CssClass="labelbold"></asp:Label>
                                                <asp:TextBox ID="TxtInPutRate" CssClass="textb" runat="server" Width="48px" onkeydown="return (event.keyCode!=13);"
                                                    onkeypress="return isNumber(event);"></asp:TextBox>
                                            </td>
                                            <td class="tdstyle" align="right">
                                                <asp:Label ID="Label10" runat="server" Text=" Weight" CssClass="labelbold"></asp:Label>
                                                <asp:TextBox ID="TxtIweight" CssClass="textb" runat="server" Width="48px" onkeydown="return (event.keyCode!=13);"
                                                    onkeypress="return isNumber(event);"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="3">
                                                <asp:CheckBox ID="chksamereceiveitem" CssClass="checkboxbold" Text="Fill Same Receive Item"
                                                    runat="server" ForeColor="Red" AutoPostBack="true" OnCheckedChanged="chksamereceiveitem_CheckedChanged" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </td>
                    <td align="justify">
                        <div style="max-height: 150px; width: 80%; overflow: auto">
                            <asp:GridView ID="DGInPutProcess" runat="server" AutoGenerateColumns="False" DataKeyNames="PCMDID"
                                CssClass="grid-view" Width="400px" OnRowDataBound="DGInPutProcess_RowDataBound">
                                <HeaderStyle CssClass="gvheaders" />
                                <AlternatingRowStyle CssClass="gvalts" />
                                <RowStyle CssClass="gvrow" />
                                <EmptyDataRowStyle CssClass="gvemptytext" />
                                <Columns>
                                    <asp:TemplateField HeaderText="">
                                        <HeaderTemplate>
                                            <asp:CheckBox ID="chkAll" runat="server" ToolTip="Check for select all" CssClass="checkboxnormal"
                                                AutoPostBack="true" onclick="return CheckAllCheckBoxes(this,'DGInPutProcess');" />
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:CheckBox ID="Chkbox" runat="server" />
                                        </ItemTemplate>
                                        <HeaderStyle Width="20px" />
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="PCMDID" HeaderText="Sr No">
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Description" HeaderText="Description">
                                        <ControlStyle Height="17px" />
                                        <HeaderStyle Width="150px" />
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
                                        <HeaderStyle Width="70px" />
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Loss">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtDGLoss" CssClass="textb" runat="server" Width="70px" Text='<%# Bind("Loss") %>'
                                                AutoPostBack="true"></asp:TextBox>
                                        </ItemTemplate>
                                        <HeaderStyle Width="70px" />
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="ICalType">
                                        <ItemTemplate>
                                            <asp:DropDownList ID="ddlicaltypedginput" runat="server" Width="100px" CssClass="dropdown"
                                                AutoPostBack="true" OnSelectedIndexChanged="ddlicaltypedginput_SelectedIndexChanged1">
                                                <asp:ListItem Value="0">Area Wise</asp:ListItem>
                                                <asp:ListItem Value="1">Pcs Wise</asp:ListItem>
                                                <asp:ListItem Value="2">W-2 X L-2</asp:ListItem>
                                                <asp:ListItem Value="3">W-2</asp:ListItem>
                                                <asp:ListItem Value="4">L-2</asp:ListItem>
                                            </asp:DropDownList>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblicaltypedginput" Text='<%#Bind("Icaltype") %>' runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </div>
                    </td>
                    <td align="justify">
                        <div style="max-height: 150px; width: 80%; overflow: auto">
                            <asp:GridView ID="GDPurchaseinclude" runat="server" AutoGenerateColumns="False" CssClass="grid-view">
                                <HeaderStyle CssClass="gvheaders" />
                                <AlternatingRowStyle CssClass="gvalts" />
                                <RowStyle CssClass="gvrow" />
                                <EmptyDataRowStyle CssClass="gvemptytext" />
                                <Columns>
                                    <asp:TemplateField HeaderText="">
                                        <HeaderTemplate>
                                            <asp:CheckBox ID="chkAll" runat="server" ToolTip="Check for select all" onclick="return CheckAllCheckBoxes(this,'GDPurchaseinclude');" />
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:CheckBox ID="Chkbox" runat="server" />
                                        </ItemTemplate>
                                        <HeaderStyle Width="20px" />
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="Process_Name" HeaderText="Process">
                                        <HeaderStyle Width="150px" />
                                    </asp:BoundField>
                                    <asp:TemplateField HeaderText="ConsmpQty">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtincludeqty" CssClass="textb" runat="server" Width="70px" onkeypress="return isNumber(event);"></asp:TextBox>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <asp:TemplateField Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblprocessid" Text='<%#Bind("Process_Name_id") %>' runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </div>
                    </td>
                </tr>
            </table>
            </fieldset>
            <div style="border: 1px; border-style: solid; color: Black">
                <table>
                    <tr id="TrOUT" runat="server">
                        <td class="tdstyle">
                            <b>RECEIVE ITEM </b>
                        </td>
                        <td class="tdstyle">
                            <b>
                                <asp:CheckBox ID="ChkForFillSame" runat="server" Text="For Fill Same" OnCheckedChanged="ChkForFillSame_CheckedChanged"
                                    AutoPostBack="True" CssClass="checkboxbold" />
                        </td>
                        <td class="tdstyle">
                            <b>
                                <asp:CheckBox ID="ChkForManyOutPut" runat="server" AutoPostBack="True" OnCheckedChanged="ChkForManyOutPut_CheckedChanged"
                                    Text="ChkForManyOutPut" CssClass="checkboxbold" />
                        </td>
                        <td class="tdstyle">
                            <b>
                                <asp:CheckBox ID="ChkForOneToOne" runat="server" AutoPostBack="True" OnCheckedChanged="ChkForOneToOne_CheckedChanged"
                                    Text="ChkForOneToOne" CssClass="checkboxbold" />
                        </td>
                    </tr>
                </table>
            </div>
            <div>
                <table>
                    <tr id="TrOUT1" runat="server">
                        <td class="tdstyle">
                            <asp:Label ID="LblOutItemCode" runat="server" Text="Item Code" CssClass="labelbold"></asp:Label>
                            <br />
                            <asp:TextBox ID="TxtOutProdCode" runat="server" CssClass="textb" Width="90px" AutoPostBack="True"
                                OnTextChanged="TxtOutProdCode_TextChanged"></asp:TextBox>
                            <cc1:AutoCompleteExtender ID="AutoCompleteExtender2" runat="server" EnableCaching="true"
                                Enabled="True" MinimumPrefixLength="1" ServiceMethod="GetQuality1" TargetControlID="TxtOutProdCode"
                                UseContextKey="True">
                            </cc1:AutoCompleteExtender>
                        </td>
                        <td class="tdstyle">
                            <asp:Label ID="lbloutcategoryname" runat="server" Text="Category Name" CssClass="labelbold"></asp:Label>
                            &nbsp;<br />
                            <asp:DropDownList ID="ddOutCategoryName" runat="server" Width="150px" AutoPostBack="True"
                                OnSelectedIndexChanged="ddOutCategoryName_SelectedIndexChanged" CssClass="dropdown">
                            </asp:DropDownList>
                        </td>
                        <td class="tdstyle">
                            <asp:Label ID="lbloutitemname" runat="server" Text="Item Name" CssClass="labelbold"></asp:Label>
                            <br />
                            <asp:DropDownList ID="ddOutItemName" runat="server" Width="150px" AutoPostBack="True"
                                CssClass="dropdown" OnSelectedIndexChanged="ddOutItemName_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                        <td id="OutQuality" runat="server" visible="false" class="tdstyle">
                            <asp:Label ID="lbloutqualiltlyname" runat="server" Text="Quality" CssClass="labelbold"></asp:Label>
                            &nbsp;<br />
                            <asp:DropDownList ID="ddOutQuality" runat="server" Width="230px" AutoPostBack="True"
                                CssClass="dropdown" OnSelectedIndexChanged="ddOutQuality_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                        <td id="OutDesign" runat="server" visible="false" class="tdstyle">
                            <asp:Label ID="lbloutdesignname" runat="server" Text="Design" CssClass="labelbold"></asp:Label>
                            <br />
                            <asp:DropDownList ID="ddOutDesign" runat="server" Width="200px" AutoPostBack="True"
                                CssClass="dropdown" OnSelectedIndexChanged="ddOutDesign_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                        <td id="OutColor" runat="server" visible="false" class="tdstyle">
                            <asp:Label ID="lbloutcolorname" runat="server" Text="Color" CssClass="labelbold"></asp:Label>
                            <br />
                            <asp:DropDownList ID="ddOutColor" runat="server" Width="100px" CssClass="dropdown">
                            </asp:DropDownList>
                        </td>
                        <td id="OutShape" runat="server" visible="false" class="tdstyle">
                            <asp:Label ID="lbloutshapename" runat="server" Text="Shape" CssClass="labelbold"></asp:Label>
                            <br />
                            <asp:DropDownList ID="ddOutShape" runat="server" Width="100px" AutoPostBack="True"
                                CssClass="dropdown" OnSelectedIndexChanged="ddOutShape_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                        <td id="OutSize" runat="server" visible="false" class="tdstyle">
                            <asp:Label ID="lbloutsizename" runat="server" Text="Size" CssClass="labelbold"></asp:Label>
                            <asp:CheckBox ID="CheckBox2" runat="server" Text="MTR Size" AutoPostBack="True" OnCheckedChanged="CHKFOReceive_CheckedChanged"
                                Visible="false" />
                            <asp:DropDownList CssClass="dropdown" Width="50" ID="DDOutSizeType" runat="server"
                                AutoPostBack="True" OnSelectedIndexChanged="DDOutSizeType_SelectedIndexChanged" />
                            <br />
                            <asp:DropDownList ID="ddOutSize" runat="server" Width="100px" CssClass="dropdown">
                            </asp:DropDownList>
                        </td>
                        <td id="OutShade" runat="server" visible="false" class="tdstyle">
                            <asp:Label ID="lbloutshadename" runat="server" Text="Shade" CssClass="labelbold"></asp:Label>
                            <br />
                            <asp:DropDownList ID="ddOutShade" runat="server" Width="200px" AutoPostBack="true"
                                CssClass="dropdown" OnSelectedIndexChanged="ddOutShade_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                    </tr>
                </table>
                <table>
                    <tr id="TrOUT2" runat="server">
                        <td>
                            <asp:Label ID="Label11" runat="server" Text=" Unit" CssClass="labelbold"></asp:Label>
                            <br />
                            <asp:DropDownList ID="ddOUnit" runat="server" CssClass="dropdown" Width="90px">
                            </asp:DropDownList>
                        </td>
                        <td id="TDOfinishtype" runat="server" visible="false">
                            <asp:Label ID="LblOFinish" runat="server" Text="Finish"></asp:Label>
                            <asp:DropDownList ID="ddFinished" runat="server" CssClass="dropdown">
                            </asp:DropDownList>
                            <asp:Button ID="Btn_FINIE_TYPE" runat="server" CssClass="buttonsmalls" OnClientClick="return ADD_FINISH();"
                                Height="15px" Text=".." Width="20px" />
                        </td>
                        <td class="tdstyle">
                            <asp:Label ID="Label12" runat="server" Text=" Cal Type" CssClass="labelbold"></asp:Label>
                            <asp:DropDownList ID="ddOCalType" runat="server" Width="100px" OnSelectedIndexChanged="ddCalType_SelectedIndexChanged"
                                CssClass="dropdown">
                                <asp:ListItem Value="0">Area Wise</asp:ListItem>
                                <asp:ListItem Value="1">Pcs Wise</asp:ListItem>
                                <asp:ListItem Value="2">W-2 X L-2</asp:ListItem>
                                <asp:ListItem Value="3">W-2</asp:ListItem>
                                <asp:ListItem Value="4">L-2</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                        <td class="tdstyle" id="TDDyeingMatch" runat="server" visible="false">
                            <asp:Label ID="lblmatch" runat="server" Text="Dyeing Match" CssClass="labelbold"></asp:Label>
                            <asp:DropDownList ID="DDDyeingMatch" runat="server" CssClass="dropdown">
                                <asp:ListItem Value="Cut">Cut</asp:ListItem>
                                <asp:ListItem Value="Side">Side</asp:ListItem>
                                <asp:ListItem Value="Cut/Side">Cut/Side</asp:ListItem>
                                <asp:ListItem Value="N/A">N/A</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                        <td class="tdstyle" id="TDDyeing" runat="server" visible="false">
                            <asp:Label ID="Label19" runat="server" Text="Dyeing" CssClass="labelbold"></asp:Label>
                            <asp:DropDownList ID="DDDyeing" runat="server" CssClass="dropdown">
                                <asp:ListItem Value="Boarder">Boarder</asp:ListItem>
                                <asp:ListItem Value="Ground">Ground</asp:ListItem>
                                <asp:ListItem Value="Ascent">Ascent</asp:ListItem>
                                <asp:ListItem Value="Ground/Boarder">Ground/Boarder</asp:ListItem>
                                <asp:ListItem Value="N/A">N/A</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                        <td class="tdstyle" id="TDDyingType" runat="server" visible="false">
                            <asp:Label ID="Label20" runat="server" Text="Dyeing Type" CssClass="labelbold"></asp:Label>
                            <asp:DropDownList ID="DDDyingType" runat="server" CssClass="dropdown">
                                <asp:ListItem Value="Shaded">Shaded</asp:ListItem>
                                <asp:ListItem Value="Natural">Natural</asp:ListItem>
                                <asp:ListItem Value="Plain">Plain</asp:ListItem>
                                <asp:ListItem Value="Gabbeh">Gabbeh</asp:ListItem>
                                <asp:ListItem Value="Multi Dyeing">Multi Dyeing</asp:ListItem>
                                <asp:ListItem Value="N/A">N/A</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                        <td class="tdstyle">
                            <asp:Label ID="Label21" runat="server" Text="Qty Percentage " CssClass="labelbold"></asp:Label>
                            <asp:TextBox ID="txtOutputQtyPercentage" runat="server" Width="75px" AutoPostBack="True"
                                OnTextChanged="txtOutputQtyPercentage_TextChanged" onkeypress="return isNumber(event);"
                                CssClass="textb"></asp:TextBox>
                        </td>
                        <td class="tdstyle">
                            <asp:Label ID="Label13" runat="server" Text="  RecQty " CssClass="labelbold"></asp:Label>
                            <asp:TextBox ID="TxtOutPutQty" runat="server" Width="75px" AutoPostBack="True" OnTextChanged="TxtOutPutQty_TextChanged"
                                onkeypress="return isNumber(event);" CssClass="textb"></asp:TextBox>
                        </td>
                        <td class="tdstyle" id="TDRecLooss" runat="server">
                            <asp:Label ID="Label14" runat="server" Text="  Loss " CssClass="labelbold"></asp:Label>
                            <asp:TextBox ID="TxtRecLoss" CssClass="textb" runat="server" Width="60px" onkeypress="return isNumber(event);"
                                onkeydown="return (event.keyCode!=13);"></asp:TextBox>
                        </td>
                        <td class="tdstyle">
                            <asp:Label ID="Label15" runat="server" Text="  Rate " CssClass="labelbold"></asp:Label>
                            <asp:TextBox ID="TxtOutPutRate" runat="server" Width="60px" onkeydown="return (event.keyCode!=13);"
                                onkeypress="return isNumber(event);" CssClass="textb"></asp:TextBox>
                        </td>
                    </tr>
                </table>
                <table width="80%">
                    <tr>
                        <td align="left">
                            <asp:Label ID="lblMessage" ForeColor="Red" runat="server" Text="...."></asp:Label>
                        </td>
                        <td align="right">
                            <asp:Button ID="BtnSave" runat="server" Text="Save" CssClass="buttonnorm" OnClick="BtnSave_Click"
                                OnClientClick="return validationsave();" />
                            &nbsp;<asp:Button ID="BtnNew" runat="server" Text="New" CssClass="buttonnorm" OnClick="BtnNew_Click" />
                            &nbsp;<asp:Button ID="BtnPreview" runat="server" CssClass="buttonnorm" Text="Preview"
                                OnClientClick="return priview();" />
                            &nbsp;<asp:Button ID="BtncClose" runat="server" CssClass="buttonnorm" Text="Close"
                                OnClick="BtncClose_Click" />
                            &nbsp;<asp:Button ID="BtnExcelFormat" runat="server" Text="Excel Format" CssClass="buttonnorm"
                                OnClick="BtnExcelFormat_Click" />
                        </td>
                    </tr>
                </table>
            </div>
            <table width="90%">
                <tr>
                    <td align="center" height="inher0t" valign="top" class="style1">
                        <div id="1" style="height: auto" align="left">
                            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                <ContentTemplate>
                                    <table width="95%">
                                        <tr align="left">
                                            <td align="left">
                                                <table id="tab" style="width: 100%" runat="server">
                                                    <tr>
                                                        <td>
                                                            <div style="width: 100%; max-height: 500px; overflow: auto;">
                                                                <asp:GridView ID="DG" runat="server" DataKeyNames="PCMDID" OnRowDataBound="DG_RowDataBound"
                                                                    OnSelectedIndexChanged="DG_SelectedIndexChanged" AutoGenerateColumns="False"
                                                                    OnRowCommand="DG_RowCommand" CssClass="grid-views" EmptyDataText="No Records Found!!!">
                                                                    <HeaderStyle CssClass="gvheaders" />
                                                                    <AlternatingRowStyle CssClass="gvalts" />
                                                                    <Columns>
                                                                        <asp:BoundField DataField="PCMDID">
                                                                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                                            <ItemStyle Width="0px" ForeColor="White" HorizontalAlign="Center" VerticalAlign="Middle" />
                                                                        </asp:BoundField>
                                                                        <asp:BoundField DataField="ITEMCODE" HeaderText="ITEM_DESCRIPTION">
                                                                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                                            <ItemStyle Width="300px" HorizontalAlign="Center" VerticalAlign="Middle" />
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
                                                                        <asp:BoundField DataField="OQTYPercentage" HeaderText="OQTYPerecentage">
                                                                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                                            <ItemStyle Width="50px" HorizontalAlign="Center" VerticalAlign="Middle" />
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
                                                                        <asp:BoundField DataField="DyingMatch" HeaderText="DyingMatch">
                                                                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                                            <ItemStyle Width="50px" HorizontalAlign="Center" VerticalAlign="Middle" />
                                                                        </asp:BoundField>
                                                                        <asp:BoundField DataField="Dyeing" HeaderText="Dyeing">
                                                                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                                            <ItemStyle Width="50px" HorizontalAlign="Center" VerticalAlign="Middle" />
                                                                        </asp:BoundField>
                                                                        <asp:BoundField DataField="DyingType" HeaderText="DyingType">
                                                                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                                            <ItemStyle Width="50px" HorizontalAlign="Center" VerticalAlign="Middle" />
                                                                        </asp:BoundField>
                                                                        <asp:BoundField DataField="INCLUDING_PROCESS" HeaderText="INCLUDING_PROCESS"></asp:BoundField>
                                                                        <asp:BoundField DataField="OLOSS" HeaderText="OLOSS">
                                                                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                                            <ItemStyle Width="50px" HorizontalAlign="Center" VerticalAlign="Middle" />
                                                                        </asp:BoundField>
                                                                        <asp:TemplateField HeaderText="DEL">
                                                                            <ItemTemplate>
                                                                                <asp:Button ID="BtnDelete" CssClass="buttonnorm" runat="server" Text="DEL" OnClientClick="return confirm('DO YOU WANT TO DELETE?')"
                                                                                    CommandArgument="<%# ((GridViewRow) Container).RowIndex %>" OnClick="BtnDelete_Click" />
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>
                                                                    </Columns>
                                                                    <SelectedRowStyle CssClass="SelectedRowStyle" />
                                                                </asp:GridView>
                                                            </div>
                                                        </td>
                                                        <td>
                                                            <table id="TblApproval" runat="server" visible="false">
                                                                <tr>
                                                                    <td>
                                                                        <asp:Label ID="LblNetWeight" runat="server" CssClass="labelbold" Text="Net Weight"
                                                                            Width="90px"></asp:Label>
                                                                    </td>
                                                                    <td>
                                                                        <asp:Label ID="LblNetWeightVal" runat="server" CssClass="labelbold" Width="90px"></asp:Label>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                        <asp:Label ID="LBlGrossWeight" runat="server" Text="Gross Weight" Font-Bold="true"
                                                                            Width="90px"></asp:Label>
                                                                    </td>
                                                                    <td>
                                                                        <asp:TextBox ID="txtGrossweight" runat="server" CssClass="textb" onkeypress="return isNumber(event);"
                                                                            Width="90px"></asp:TextBox>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                        <asp:Label ID="LblTotalcost" runat="server" Text="Total Cost" Width="80px" Enabled="false"
                                                                            CssClass="labelbold"></asp:Label>
                                                                    </td>
                                                                    <td>
                                                                        <asp:TextBox ID="TxtTotalCost" runat="server" CssClass="textb" Width="90px" onkeydown="return (event.keyCode!=13);"></asp:TextBox>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                        <asp:Label ID="Label16" runat="server" Text=" Currency" CssClass="labelbold"></asp:Label>
                                                                    </td>
                                                                    <td>
                                                                        <asp:DropDownList ID="DDCurrency" CssClass="dropdown" Width="90px" runat="server"
                                                                            OnSelectedIndexChanged="DDCurrency_SelectedIndexChanged" AutoPostBack="true">
                                                                        </asp:DropDownList>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                        <asp:Label ID="Label17" runat="server" Text="  Cr Rate" CssClass="labelbold"></asp:Label>
                                                                    </td>
                                                                    <td>
                                                                        <asp:TextBox ID="TxtCurRate" runat="server" CssClass="textb" Width="90px" onkeypress="return isNumber(event);"
                                                                            OnTextChanged="TxtCurRate_TextChanged" AutoPostBack="true"></asp:TextBox>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                        <asp:Label ID="Label18" runat="server" Text=" Approval Cost" CssClass="labelbold"></asp:Label>
                                                                    </td>
                                                                    <td>
                                                                        <asp:TextBox ID="TxtApprovalCost" runat="server" CssClass="textb" Width="90px" onkeydown="return (event.keyCode!=13);"
                                                                            onkeypress="return isNumber(event);"></asp:TextBox>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                        <asp:Button ID="BtnApproval" CssClass="buttonnorm" Width="90px" runat="server" Text="Approve"
                                                                            OnClick="BtnApproval_Click" />
                                                                    </td>
                                                                    <td>
                                                                        <asp:Button ID="BtnRefresh" CssClass="buttonnorm" Width="90px" runat="server" Text="Refresh"
                                                                            OnClick="BtnRefresh_Click" />
                                                                    </td>
                                                                </tr>
                                                            </table>
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
                    <td>
                        <div style="background-color: #edf3fe; text-align: center; height: 43px; width: 95%">
                            <span class="labelcopyr">&copy; Enablesoft Erp Consultancy Pvt. Ltd.All rights reserved.</span>
                        </div>
                    </td>
                </tr>
            </table>
            <asp:HiddenField ID="hnqualityid" runat="server" />
            <asp:HiddenField ID="hndesignid" runat="server" />
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="BtnExcelFormat" />
        </Triggers>
    </asp:UpdatePanel>
    <asp:HiddenField ID="HDF1" runat="server" />
    </form>
</body>
</html>
