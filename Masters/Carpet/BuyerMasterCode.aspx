<%@ Page Language="C#" AutoEventWireup="true" CodeFile="BuyerMasterCode.aspx.cs"
    Inherits="Masters_Carpet_BuyerMasterCode" EnableEventValidation="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register Src="~/UserControls/ucmenu.ascx" TagName="ucmenu" TagPrefix="uc1" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta content="blendTrans(Duration=0.5)" http-equiv="Page-Enter" />
    <meta content="blendTrans(Duration=0.5)" http-equiv="Page-Exit" />
    <title></title>
    <script src="../../Scripts/JScript.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery-1.4.1.js" type="text/javascript"></script>
    <%--<script type="text/javascript" src="../../Scripts/Fixfocus.js"></script>--%>
    <link href="../../Styles/vijay.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript">
        function reloadPage() {
            window.location.href = "BuyerMasterCode.aspx";
        }
        function CloseForm() {
            if (window.opener.document.getElementById('RefereshBuyerMasterCode')) {
                window.opener.document.getElementById('RefereshBuyerMasterCode').click();
                self.close();
            }
        }
        function AddCustomer() {
            var answer = confirm("Do you want to ADD?")
            if (answer) {
                window.open('../Campany/frmCustomer.aspx', '', 'width=1100px,Height=600px,resizable=yes,scrollbars=yes,align=center');
            }
        }
        function Addcategory() {
            var answer = confirm("Do you want to ADD?")
            if (answer) {
                window.open('AddItemCategory.aspx', '', 'width=500px,Height=500px');
            }
        }
        function Addshape() {
            var answer = confirm("Do you want to ADD?")
            if (answer) {
                window.open('AddShape.aspx', '', 'width=901px,Height=401px');
            }
        }
        function Additem() {
            var answer = confirm("Do you want to ADD?")
            if (answer) {
                var e = document.getElementById("ddCategory");
                var Categoryid = e.options[e.selectedIndex].value;
                window.open('AddItemName.aspx?Category=' + Categoryid + '', '', 'width=550px,Height=500px');
            }
        }
        function Addquality() {
            var answer = confirm("Do you want to ADD?")
            var e = document.getElementById("ddCategory");
            var Categoryid = e.options[e.selectedIndex].value;
            var e1 = document.getElementById("ddItemname");
            var Itemid = e1.options[e1.selectedIndex].value;
            if (answer) {
                window.open('AddQuality.aspx?Category=' + Categoryid + '&Item=' + Itemid + '', '', 'width=701px,Height=501px');
            }
        }
        function AddsizeNew() {
            var answer = confirm("Do you want to ADD?")
            if (answer) {
                var e = document.getElementById("ddShape");
                var Shapeid = e.options[e.selectedIndex].value;
                var VarUnitID = document.getElementById('TxtUnitID').value;
                window.open('AddSize.aspx?shapeid=' + Shapeid + '&Varid=' + VarUnitID + '', '', 'width=1000px,Height=501px');
            }
        }
        function AddQualitysizeNew() {
            var answer = confirm("Do you want to ADD?")
            if (answer) {
                var e = document.getElementById("ddShape");
                var Shapeid = e.options[e.selectedIndex].value;
                var VarUnitID = document.getElementById('TxtUnitID').value;
                window.open('FrmNewSize.aspx?shapeid=' + Shapeid + '&Varid=' + VarUnitID + '', '', 'width=1000px,Height=501px');
            }
        }
        function Adddesign() {
            var answer = confirm("Do you want to ADD?")
            if (answer) {
                var e = document.getElementById("ddLocalQuality");
                var qualityid = e.options[e.selectedIndex].value;
                window.open('AddDesign.aspx?Qid=' + qualityid + '', '', 'width=601px,Height=401px');
            }
        }
        function Addcolor() {
            var answer = confirm("Do you want to ADD?")
            if (answer) {
                var e = document.getElementById("ddLocalDesign");
                var Designid = e.options[e.selectedIndex].value;
                window.open('AddColor.aspx?Did=' + Designid + '', '', 'width=501px,Height=501px', 'resizeable=yes');
            }
        }
    </script>
    <script language="javascript" type="text/javascript">

        function ShowEditModal(ExpanseID) {
            var frame = $get('IframeEdit');
            frame.src = "frmDummy.aspx?UIMODE=EDIT&EID=" + ExpanseID;
            $find('EditModalPopup').show();
        }
        function ShowEditModal(ExpanseID) {
            var frame = $get('IframeEdit');
            frame.src = "Masters/Carpet/frmitemcatagory.aspx?UIMODE=EDIT&EID=" + ExpanseID;
            $find('EditModalPopup').show();
        }
        function ShowEditModal(ExpanseID) {
            var frame = $get('IframeEdit');
            frame.src = "Masters/Carpet/ItemName.aspx?UIMODE=EDIT&EID=" + ExpanseID;
            $find('EditModalPopup').show();
        }
        function ShowEditModal(ExpanseID) {
            var frame = $get('IframeEdit');
            frame.src = "Masters/Carpet/Shape.aspx?UIMODE=EDIT&EID=" + ExpanseID;
            $find('EditModalPopup').show();
        }
        function ShowEditModal(ExpanseID) {
            var frame = $get('IframeEdit');
            frame.src = "Masters/Carpet/QualityMaster.aspx?UIMODE=EDIT&EID=" + ExpanseID;
            $find('EditModalPopup').show();
        }
        function ShowEditModal(ExpanseID) {
            var frame = $get('IframeEdit');
            frame.src = "Masters/Carpet/Design.aspx?UIMODE=EDIT&EID=" + ExpanseID;
            $find('EditModalPopup').show();
        }
        function ShowEditModal(ExpanseID) {
            var frame = $get('IframeEdit');
            frame.src = "Masters/Carpet/frmColor.aspx?UIMODE=EDIT&EID=" + ExpanseID;
            $find('EditModalPopup').show();
        }
        function ShowEditModal(ExpanseID) {
            var frame = $get('IframeEdit');
            frame.src = "Masters/Carpet/frmSize.aspx?UIMODE=EDIT&EID=" + ExpanseID;
            $find('EditModalPopup').show();
        }
        function ShowEditModal(ExpanseID) {
            var frame = $get('IframeEdit');
            frame.src = "Masters/Carpet/FrmNewSize.aspx?UIMODE=EDIT&EID=" + ExpanseID;
            $find('EditModalPopup').show();
        }
        function EditCancelScript() {
            var frame = $get('IframeEdit');
            frame.src = "frmCarriage.aspx";
        }
        function EditOkayScript() {
            RefreshDataGrid();
            EditCancelScript();
        }
        function RefreshDataGrid() {
            $get('btnSearch').click();
        }
        function NewExpanseOkay() {
            $get('btnSearch').click();
        }
        function okay() {
            $find('btnCategory_ModalPopupExtender').hide();
            $get('btnSearch').click();
        }
        function okay() {
            $find('btnItem_ModalPopupExtender').hide();
            $get('btnSearch').click();
        }
        function okay() {
            $find('btnShape_ModalPopupExtender').hide();
            $get('btnSearch').click();
        }
        function okay() {
            $find('btnQuality_ModalPopupExtender').hide();
            $get('btnSearch').click();
        }
        function okay() {
            $find('btnSize_ModalPopupExtender').hide();
            $get('btnSearch').click();
        }
        function okay() {
            $find('btnColor_ModalPopupExtender').hide();
            $get('btnSearch').click();
        }
        function okay() {
            $find('btnDesign_ModalPopupExtender').hide();
            $get('btnSearch').click();
        }
        function priview() {
            window.open('../../ReportViewer.aspx', '');
        }
        function Validate() {
            if (document.getElementById('ddBuyerCode')) {
                if (document.getElementById("ddBuyerCode").selectedIndex == 0) {
                    alert("Please Select Buyer Code....");
                    document.getElementById("ddBuyerCode").focus();
                    return false;
                }
            }
            if (document.getElementById('ddCategory')) {
                if (document.getElementById("ddCategory").selectedIndex == 0) {
                    alert("Please Select Cateogory....");
                    document.getElementById("ddCategory").focus();
                    return false;
                }
            }
            if (document.getElementById('ddItemname')) {
                if (document.getElementById("ddItemname").selectedIndex == 0) {
                    alert("Please Select Item Name....");
                    document.getElementById("ddItemname").focus();
                    return false;
                }
            }
            //            if (document.getElementById('ddLocalQuality')) {
            //                if (document.getElementById("ddLocalQuality").selectedIndex > 0) {
            //                    if (document.getElementById("txtBuyerQuality").value == "") {
            //                        alert("Please Fill Buyer Quality....");
            //                        document.getElementById("txtBuyerQuality").focus();
            //                        return false;
            //                    }
            //                }
            //            }
            //            if (document.getElementById('ddLocalDesign')) {
            //                if (document.getElementById("ddLocalDesign").selectedIndex > 0) {
            //                    if (document.getElementById("txtBuyerDesign").value == "") {
            //                        alert("Please Fill Buyer Design....");
            //                        document.getElementById("txtBuyerDesign").focus();
            //                        return false;
            //                    }
            //                }
            //            }
            //            if (document.getElementById('ddLocalColor')) {
            //                if (document.getElementById("ddLocalColor").selectedIndex > 0) {
            //                    if (document.getElementById("txtBuyerColor").value == "") {
            //                        alert("Please Fill Buyer Color....");
            //                        document.getElementById("txtBuyerColor").focus();
            //                        return false;
            //                    }
            //                }
            //            }
            return confirm('Do You Want To Save?');
        }

    </script>
</head>
<body>
    <form id="form" runat="server">
    <table width="100%" border="1">
        <tr id="zzz" runat="server">
            <td>
                <table width="100%">
                    <tr style="background-color: #edf3fe; width: 100%" align="center" id="trheader" runat="server">
                        <td height="66px" align="center">
                            <asp:Image ID="imgLogo" align="left" runat="server" Height="66px" Width="111px" /><span
                                style="color: Black; margin-left: 30px; font-family: Arial; font-size: xx-large"><strong><em><i><font
                                    size="6" face="GEORGIA"><asp:Label ID="LblCompanyName" runat="server" Text=""></asp:Label></font></i></em></strong></span>
                        </td>
                        <td width="100px" valign="bottom">
                            <i><font size="4" face="GEORGIA">
                                <asp:Label ID="LblUserName" runat="server" Text=""></asp:Label></font></i>
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
                    <table width="70%" align="center">
                        <tr>
                            <td width="75%" height="400" rowspan="2">
                                <%--Page Design--%>
                                <%--Page Working table--%>
                                <table width="100%">
                                    <tr>
                                        <td>
                                            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                                <ContentTemplate>
                                                    <table>
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="labled" Text="BUYER CODE" runat="server" CssClass="labelbold" />
                                                                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                            </td>
                                                            <td>
                                                                <asp:Label ID="lblCategoryName" runat="server" Text="Category Name" CssClass="labelbold"></asp:Label>
                                                            </td>
                                                            <td>
                                                                <asp:Label ID="lblItemName" runat="server" Text="Item Name" CssClass="labelbold"></asp:Label>
                                                            </td>
                                                            <td>
                                                                <asp:Label ID="lblSkuNo" runat="server" Text="SKU No" CssClass="labelbold" ></asp:Label>
                                                            </td>
                                                             <td>
                                                                <asp:Label ID="lblskudesc" runat="server" Text="SKU Desc" CssClass="labelbold" ></asp:Label>
                                                            </td>
                                                             <td>
                                                                <asp:Label ID="lblcomposition" runat="server" Text="Composition" CssClass="labelbold" ></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:DropDownList ID="ddBuyerCode" runat="server" AutoPostBack="True" CssClass="dropdown"
                                                                    OnSelectedIndexChanged="ddBuyerCode_SelectedIndexChanged" Width="150px">
                                                                </asp:DropDownList>
                                                                <asp:Button ID="BtnRefereceCustomer" runat="server" Style="display: none" OnClick="BtnRefereceCustomer_Click" />
                                                                &nbsp;&nbsp;&nbsp;&nbsp;<asp:Button ID="btnbuyercode" runat="server" CssClass="buttonsmalls"
                                                                    OnClientClick="return AddCustomer();" Width="45px" Text=".." />
                                                                <cc1:ListSearchExtender ID="ListSearchExtender2" runat="server" TargetControlID="ddBuyerCode"
                                                                    ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                                                                </cc1:ListSearchExtender>
                                                                <%--<cc1:ModalPopupExtender ID="btnBuyerCode_ModalPopupExtender" runat="server" 
                                                                        BackgroundCssClass="modalBackground" CancelControlID="btnCancel" Drag="true" 
                                                                        OkControlID="btnOkay" OnOkScript="okay()" PopupControlID="Panel1" 
                                                                        TargetControlID="btnBuyerCode">
                                                                    </cc1:ModalPopupExtender>--%>
                                                            </td>
                                                            <td>
                                                                <asp:DropDownList ID="ddCategory" runat="server" AutoPostBack="True" CssClass="dropdown"
                                                                    Width="150px" OnSelectedIndexChanged="ddCategory_SelectedIndexChanged">
                                                                </asp:DropDownList>
                                                                <asp:Button ID="refreshcategory" runat="server" OnClick="refreshcategory_Click" Style="display: none" />
                                                                &nbsp;&nbsp;&nbsp;
                                                                <asp:Button ID="btnaddcategory" runat="server" CssClass="buttonsmalls" OnClientClick="return Addcategory();"
                                                                    Width="45px" Text=".." />
                                                                <cc1:ListSearchExtender ID="ListSearchExtender1" runat="server" TargetControlID="ddCategory"
                                                                    ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                                                                </cc1:ListSearchExtender>
                                                            </td>
                                                            <td>
                                                                <asp:DropDownList ID="ddItemname" runat="server" AutoPostBack="True" CssClass="dropdown"
                                                                    Width="150px" OnSelectedIndexChanged="ddItemname_SelectedIndexChanged">
                                                                </asp:DropDownList>
                                                                <asp:Button ID="refreshitem" runat="server" Style="display: none" OnClick="refreshitem_Click" />
                                                                &nbsp;
                                                                <asp:Button ID="btnadditem" runat="server" CssClass="buttonsmalls" OnClientClick="return Additem();"
                                                                    Width="45px" Text=".." />
                                                                <cc1:ListSearchExtender ID="ListSearchExtender3" runat="server" TargetControlID="ddItemname"
                                                                    ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                                                                </cc1:ListSearchExtender>
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtSKUNo" runat="server" Width="120px" CssClass="textb">
                                                                </asp:TextBox>
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtskudesc" runat="server" Width="120px" CssClass="textb">
                                                                </asp:TextBox>
                                                            </td>
                                                             <td>
                                                                <asp:TextBox ID="txtcomposition" TextMode="MultiLine" runat="server" Width="120px" Height="100px" CssClass="textb">
                                                                </asp:TextBox>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:TextBox ID="TxtUnitID" runat="server" Height="0px" Width="0px" ForeColor="White"></asp:TextBox>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="Label1" Text=" BUYER QUALITY" runat="server" CssClass="labelbold" />
                                                            </td>
                                                            <td>
                                                                <asp:Label ID="lblQualityName" runat="server" Text="Quality" CssClass="labelbold"></asp:Label>
                                                            </td>
                                                            <td>
                                                                <asp:Label ID="lblShapeName" runat="server" Text="Shape" CssClass="labelbold"></asp:Label>
                                                            </td>
                                                            <td>
                                                                <asp:Label ID="lblSizeName" runat="server" Text="Size" CssClass="labelbold"></asp:Label>
                                                                <asp:CheckBox ID="ChkForMtrSize" runat="server" Text="Chk For Mtr" AutoPostBack="True"
                                                                    OnCheckedChanged="ChkForMtrSize_CheckedChanged" Visible="false" />
                                                                <asp:DropDownList CssClass="dropdown" Width="50" ID="DDsizetype" runat="server" AutoPostBack="True"
                                                                    OnSelectedIndexChanged="DDsizetype_SelectedIndexChanged" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:TextBox ID="txtBuyerQuality" runat="server" Width="120px" CssClass="textb"></asp:TextBox>
                                                                &nbsp;&nbsp;
                                                            </td>
                                                            <td>
                                                                <asp:DropDownList ID="ddLocalQuality" runat="server" AutoPostBack="True" CssClass="dropdown"
                                                                    OnSelectedIndexChanged="ddLocalQuality_SelectedIndexChanged" Width="150px">
                                                                </asp:DropDownList>
                                                                <asp:Button ID="refreshquality" runat="server" Style="display: none" OnClick="refreshquality_Click" />
                                                                &nbsp;
                                                                <asp:Button ID="btnaddquality" runat="server" CssClass="buttonsmalls" OnClientClick="return Addquality();"
                                                                    Width="45px" Text=".." />
                                                                <cc1:ListSearchExtender ID="ListSearchExtender5" runat="server" TargetControlID="ddLocalQuality"
                                                                    ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                                                                </cc1:ListSearchExtender>
                                                            </td>
                                                            <td>
                                                                <asp:DropDownList ID="ddShape" runat="server" AutoPostBack="true" CssClass="dropdown"
                                                                    OnSelectedIndexChanged="ddShape_SelectedIndexChanged" Width="130px">
                                                                </asp:DropDownList>
                                                                <asp:Button ID="refreshshape" runat="server" Style="display: none" OnClick="refreshshape_Click" />
                                                                <asp:Button ID="btnaddshape" runat="server" CssClass="buttonsmalls" OnClientClick="return Addshape();"
                                                                    Width="45px" Text=".." />
                                                                <cc1:ListSearchExtender ID="ListSearchExtender4" runat="server" TargetControlID="ddShape"
                                                                    ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                                                                </cc1:ListSearchExtender>
                                                            </td>
                                                            <td>
                                                                <asp:DropDownList ID="ddSize" runat="server" AutoPostBack="true" CssClass="dropdown"
                                                                    OnSelectedIndexChanged="ddSize_SelectedIndexChanged" Width="130px">
                                                                </asp:DropDownList>
                                                                <asp:Button ID="BtnRefreshSize" runat="server" Style="display: none" OnClick="BtnRefreshSize_Click" />
                                                                &nbsp;&nbsp;
                                                                <asp:Button ID="btnaddsize" runat="server" CssClass="buttonsmalls" OnClientClick="return AddsizeNew();"
                                                                    Width="45px" Text=".." />
                                                                <asp:Button ID="btnAddQualitySize" runat="server" CssClass="buttonsmalls" OnClientClick="return AddQualitysizeNew();"
                                                                    Width="45px" Text=".." Visible="false" />
                                                                &nbsp;&nbsp;&nbsp;
                                                                <cc1:ListSearchExtender ID="ListSearchExtender6" runat="server" TargetControlID="ddSize"
                                                                    ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                                                                </cc1:ListSearchExtender>
                                                            </td>
                                                            <td>
                                                                &nbsp;
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                &nbsp;
                                                            </td>
                                                            <td>
                                                                &nbsp;
                                                            </td>
                                                            <td colspan="3" align="center">
                                                                <asp:Label ID="LblShowSize" runat="server" Text="" Font-Bold="True" ForeColor="Red"
                                                                    CssClass="labelbold"></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="Label2" runat="server" Text=" BUYER DESIGN" CssClass="labelbold"></asp:Label>
                                                            </td>
                                                            <td>
                                                                <asp:Label ID="lblDesignName" runat="server" Text="Design" CssClass="labelbold"></asp:Label>
                                                            </td>
                                                            <td colspan="3" align="center">
                                                                <asp:Label ID="Label3" runat="server" Text="WIDTH" CssClass="labelbold"></asp:Label>
                                                                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                                <asp:Label ID="Label4" runat="server" Text="  Buyer" CssClass="labelbold"></asp:Label>
                                                                <asp:Label ID="Label6" runat="server" Text="  Ft Size" CssClass="labelbold"></asp:Label>
                                                                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                                <asp:Label ID="Label5" runat="server" Text="  LENGTH" CssClass="labelbold"></asp:Label>
                                                                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                                <asp:Label ID="Label16" runat="server" Text="  Buyer" CssClass="labelbold"></asp:Label>
                                                                <asp:Label ID="Label17" runat="server" Text="  Ft Size" CssClass="labelbold"></asp:Label>
                                                                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                                <asp:Label ID="Label18" runat="server" Text="  HEIGHT" CssClass="labelbold"></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:TextBox ID="txtBuyerDesign" runat="server" Width="120px" CssClass="textb"></asp:TextBox>
                                                                &nbsp;&nbsp;<asp:Button ID="Btnbakword" runat="server" Text=" << " Width="45px" CssClass="buttonsmalls"
                                                                    OnClick="Btnbakword_Click" />
                                                            </td>
                                                            <td>
                                                                <asp:DropDownList ID="ddLocalDesign" runat="server" AutoPostBack="True" CssClass="dropdown"
                                                                    OnSelectedIndexChanged="ddLocalDesign_SelectedIndexChanged" Width="150px">
                                                                </asp:DropDownList>
                                                                <cc1:ListSearchExtender ID="ListSearchExtender7" runat="server" TargetControlID="ddLocalDesign"
                                                                    ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                                                                </cc1:ListSearchExtender>
                                                                <asp:Button ID="refreshdesign" runat="server" Style="display: none" OnClick="refreshdesign_Click" />
                                                                &nbsp;&nbsp;
                                                                <asp:Button ID="btnadddesign" runat="server" CssClass="buttonsmalls" OnClientClick="return Adddesign();"
                                                                    Width="45px" Text=".." Visible="true" />
                                                            </td>
                                                            <td align="center">
                                                                <asp:TextBox ID="TxtBuyerFtWidth" runat="server" Width="120px" CssClass="textb">
                                                                </asp:TextBox>
                                                            </td>
                                                            <td align="center">
                                                                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                                <asp:TextBox ID="TxtBuyerFtLength" runat="server" Width="120px" CssClass="textb">
                                                                </asp:TextBox>
                                                            </td>
                                                            <td align="center">
                                                                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                                <asp:TextBox ID="TxtBuyerFtHeight" runat="server" Width="120px" CssClass="textb">
                                                                </asp:TextBox>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="Label7" runat="server" Text="  BUYER COLOR" CssClass="labelbold"></asp:Label>
                                                            </td>
                                                            <td>
                                                                <asp:Label ID="lblColorName" runat="server" Text="Color" CssClass="labelbold"></asp:Label>
                                                            </td>
                                                            <td colspan="3" align="center">
                                                                <asp:Label ID="Label8" runat="server" Text=" WIDTH" CssClass="labelbold"></asp:Label>
                                                                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                                <asp:Label ID="Label9" runat="server" Text=" Buyer" CssClass="labelbold"></asp:Label>
                                                                <asp:Label ID="lblbuyermtrSize" runat="server" Text=" Mtr Size" CssClass="labelbold"></asp:Label>
                                                                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                                <asp:Label ID="Label11" runat="server" Text="LENGTH" CssClass="labelbold"></asp:Label>
                                                                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                                <asp:Label ID="Label19" runat="server" Text=" Buyer" CssClass="labelbold"></asp:Label>
                                                                <asp:Label ID="Label20" runat="server" Text=" Mtr Size" CssClass="labelbold"></asp:Label>
                                                                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                                <asp:Label ID="Label21" runat="server" Text="HEIGHT" CssClass="labelbold"></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:TextBox ID="txtBuyerColor" runat="server" Width="114px" CssClass="textb"></asp:TextBox>
                                                                &nbsp;&nbsp;
                                                            </td>
                                                            <td>
                                                                <asp:DropDownList ID="ddLocalColor" runat="server" AutoPostBack="true" Width="150px"
                                                                    CssClass="dropdown" OnSelectedIndexChanged="ddLocalColor_SelectedIndexChanged">
                                                                </asp:DropDownList>
                                                                <cc1:ListSearchExtender ID="ListSearchExtender8" runat="server" TargetControlID="ddLocalColor"
                                                                    ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                                                                </cc1:ListSearchExtender>
                                                                <asp:Button ID="refreshcolor" runat="server" Style="display: none" OnClick="refreshcolor_Click" />
                                                                &nbsp;&nbsp;<asp:Button ID="btnaddcolor" runat="server" CssClass="buttonsmalls" Width="45px"
                                                                    OnClientClick="return Addcolor();" Text=".." Visible="true" />
                                                            </td>
                                                            <td align="center">
                                                                <asp:TextBox ID="TxtBuyerMtrWidth" runat="server" Width="120px" CssClass="textb">
                                                                </asp:TextBox>
                                                            </td>
                                                            <td align="center">
                                                                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                                <asp:TextBox ID="TxtBuyerMtrLenth" runat="server" Width="120px" CssClass="textb">
                                                                </asp:TextBox>
                                                            </td>
                                                            <td align="center">
                                                                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                                <asp:TextBox ID="TxtBuyerMtrHeight" runat="server" Width="120px" CssClass="textb">
                                                                </asp:TextBox>
                                                            </td>
                                                        </tr>
                                                        <tr id="BuyerInch" runat="server">
                                                            <td colspan="2">
                                                                <asp:Label ID="Label10" runat="server" Text="Sample Code" CssClass="labelbold"></asp:Label>
                                                            </td>
                                                            <td colspan="3" align="center">
                                                                <asp:Label ID="Label12" runat="server" Text="  WIDTH" CssClass="labelbold"></asp:Label>
                                                                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                                <asp:Label ID="Label13" runat="server" Text="  Buyer" CssClass="labelbold"></asp:Label>
                                                                <asp:Label ID="Label14" runat="server" Text="  Inch Size" CssClass="labelbold"></asp:Label>
                                                                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                                <asp:Label ID="Label15" runat="server" Text="  LENGTH" CssClass="labelbold"></asp:Label>
                                                                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                                <asp:Label ID="Label22" runat="server" Text="  Buyer" CssClass="labelbold"></asp:Label>
                                                                <asp:Label ID="Label23" runat="server" Text="  Inch Size" CssClass="labelbold"></asp:Label>
                                                                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                                <asp:Label ID="Label24" runat="server" Text="  HEIGHT" CssClass="labelbold"></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr id="BuyerInchtxtb" runat="server">
                                                            <td colspan="2">
                                                                <asp:TextBox ID="TxtSampleCode" runat="server" Width="200px" CssClass="textb" OnTextChanged="TxtSampleCode_TextChanged"
                                                                    AutoPostBack="true"></asp:TextBox>
                                                            </td>
                                                            <td align="center">
                                                                <asp:TextBox ID="txtInchWidth" runat="server" Width="120px" CssClass="textb">
                                                                </asp:TextBox>
                                                            </td>
                                                            <td align="center">
                                                                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                                <asp:TextBox ID="txtinchlength" runat="server" Width="120px" CssClass="textb">
                                                                </asp:TextBox>
                                                            </td>
                                                            <td align="center">
                                                                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                                <asp:TextBox ID="txtinchHeight" runat="server" Width="120px" CssClass="textb">
                                                                </asp:TextBox>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td colspan="2">
                                                                <asp:Label ID="lblErr" runat="server" CssClass="labelbold" ForeColor="Red" Font-Size="Small"></asp:Label>
                                                            </td>
                                                            <td colspan="2">
                                                                <asp:Button ID="BtnClear" runat="server" Text="Clear" OnClientClick="return reloadPage();"
                                                                    Width="70px" CssClass="buttonsmalls" />
                                                                <asp:Button ID="BtnSave" runat="server" Text="Save" Width="70px" CssClass="buttonsmalls"
                                                                    OnClick="BtnSave_Click" OnClientClick="return Validate()" />
                                                                <asp:Button ID="BtnClose" runat="server" Text="Close" OnClick="BtnClose_Click" CssClass="buttonsmalls" />
                                                                <%-- <asp:Button ID="btnrpt" runat="server" Text="Preview" Enabled="false" OnClientClick="return priview();"
                                                                    CssClass="buttonsmalls" />--%>
                                                                <asp:Button ID="BtnPreview" runat="server" Text="Preview" Width="70px" CssClass="buttonsmalls"
                                                                    OnClick="BtnPreview_Click" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                    <table>
                                                        <tr>
                                                            <td>
                                                                <div id="DivQuality" style="overflow: scroll; width: 250px; height: 300px">
                                                                    <asp:GridView ID="gdBuyerQuality" runat="server" DataKeyNames="Sr_No" AutoGenerateColumns="False"
                                                                        OnRowDataBound="gdBuyerQuality_RowDataBound" OnSelectedIndexChanged="gdBuyerQuality_SelectedIndexChanged"
                                                                        OnRowDeleting="gdBuyerQuality_RowDeleting" Width="100%">
                                                                        <HeaderStyle CssClass="gvheaders" />
                                                                        <AlternatingRowStyle CssClass="gvalts" />
                                                                        <Columns>
                                                                            <asp:BoundField DataField="QUALITY" HeaderText="QUALITY NAME">
                                                                                <HeaderStyle Font-Bold="True" HorizontalAlign="Center" />
                                                                                <ItemStyle HorizontalAlign="Center" />
                                                                            </asp:BoundField>
                                                                            <asp:TemplateField HeaderText="Id" Visible="false">
                                                                                <ItemTemplate>
                                                                                    <asp:Label ID="lblId2" Text='<%#Bind("Sr_No") %>' runat="server" />
                                                                                </ItemTemplate>
                                                                                <ItemStyle Width="100px" />
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField ShowHeader="False">
                                                                                <ItemTemplate>
                                                                                    <asp:Button ID="btnDelete" runat="server" OnClientClick='return confirm("Are you sure you want to delete this data?");'
                                                                                        Text="Delete" CommandName="Delete" CssClass="buttonnorm" />
                                                                                </ItemTemplate>
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField HeaderText="Enable/Disable">
                                                                                <ItemTemplate>
                                                                                    <asp:LinkButton ID="lnkquality_ED" Text='<%#Bind("Status") %>' runat="server" OnClick="lnkquality_ED"
                                                                                        OnClientClick="return confirm('Do you want to Enable_Disable Quality')" />
                                                                                </ItemTemplate>
                                                                                <ItemStyle HorizontalAlign="Center" />
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField Visible="false">
                                                                                <ItemTemplate>
                                                                                    <asp:Label ID="lblqualityenable_disable" Text='<%#Bind("Enable_Disable") %>' runat="server" />
                                                                                </ItemTemplate>
                                                                            </asp:TemplateField>
                                                                        </Columns>
                                                                        <EmptyDataRowStyle CssClass="gvemptytext" />
                                                                    </asp:GridView>
                                                                </div>
                                                            </td>
                                                            <td>
                                                                <div id="DivDesign" style="overflow: scroll; width: 250px; height: 300px">
                                                                    <asp:GridView ID="gdBuyerDesign" runat="server" DataKeyNames="Sr_No" AutoGenerateColumns="False"
                                                                        OnRowDataBound="gdBuyerDesign_RowDataBound" OnSelectedIndexChanged="gdBuyerDesign_SelectedIndexChanged"
                                                                        OnRowDeleting="gdBuyerDesign_RowDeleting" Width="100%">
                                                                        <AlternatingRowStyle BackColor="White" />
                                                                        <HeaderStyle CssClass="gvheaders" />
                                                                        <AlternatingRowStyle CssClass="gvalts" />
                                                                        <Columns>
                                                                            <asp:BoundField DataField="DESIGN" HeaderText="DESIGN NAME">
                                                                                <HeaderStyle Font-Bold="True" HorizontalAlign="Center" />
                                                                                <ItemStyle HorizontalAlign="Center" />
                                                                            </asp:BoundField>
                                                                            <asp:TemplateField HeaderText="Id" Visible="false">
                                                                                <ItemTemplate>
                                                                                    <asp:Label ID="lblDesignId" Text='<%#Bind("Sr_No") %>' runat="server" />
                                                                                </ItemTemplate>
                                                                                <ItemStyle Width="100px" />
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField ShowHeader="False">
                                                                                <ItemTemplate>
                                                                                    <asp:Button ID="btnDelete" runat="server" OnClientClick='return confirm("Are you sure you want to delete this data?");'
                                                                                        Text="Delete" CommandName="Delete" CssClass="buttonnorm" />
                                                                                    <%--<asp:ImageButton ID="ImageButton1" runat="server" CausesValidation="False" OnClientClick='return confirm("Are you sure you want to delete this data?");'
                                                                                    CommandName="Delete" ImageUrl="~/images/remove.png" Text="Delete" />--%>
                                                                                </ItemTemplate>
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField HeaderText="Enable/Disable">
                                                                                <ItemTemplate>
                                                                                    <asp:LinkButton ID="lnkdesign_ED" Text='<%#Bind("Status") %>' runat="server" OnClick="lnkdesign_ED"
                                                                                        OnClientClick="return confirm('Do you want to Enable_Disable Design')" />
                                                                                </ItemTemplate>
                                                                                <ItemStyle HorizontalAlign="Center" />
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField Visible="false">
                                                                                <ItemTemplate>
                                                                                    <asp:Label ID="lbldesignenable_disable" Text='<%#Bind("Enable_Disable") %>' runat="server" />
                                                                                </ItemTemplate>
                                                                            </asp:TemplateField>
                                                                        </Columns>
                                                                        <EmptyDataRowStyle CssClass="gvemptytext" />
                                                                    </asp:GridView>
                                                                </div>
                                                            </td>
                                                            <td>
                                                                <div id="DivColor" style="overflow: scroll; width: 250px; height: 300px">
                                                                    <asp:GridView ID="gdBuyerColor" runat="server" DataKeyNames="Sr_No" AutoGenerateColumns="False"
                                                                        OnSelectedIndexChanged="gdBuyerColor_SelectedIndexChanged" OnRowDataBound="gdBuyerColor_RowDataBound"
                                                                        OnRowDeleting="gdBuyerColor_RowDeleting" Width="100%">
                                                                        <HeaderStyle CssClass="gvheaders" />
                                                                        <AlternatingRowStyle CssClass="gvalts" />
                                                                        <Columns>
                                                                            <asp:BoundField DataField="COLOR" HeaderText="COLOR NAME">
                                                                                <HeaderStyle Font-Bold="True" HorizontalAlign="Center" />
                                                                                <ItemStyle HorizontalAlign="Center" />
                                                                            </asp:BoundField>
                                                                            <asp:TemplateField HeaderText="Id" Visible="false">
                                                                                <ItemTemplate>
                                                                                    <asp:Label ID="lblColorId" Text='<%#Bind("Sr_No") %>' runat="server" />
                                                                                </ItemTemplate>
                                                                                <ItemStyle Width="100px" />
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField ShowHeader="False">
                                                                                <ItemTemplate>
                                                                                    <asp:Button ID="btnDelete" runat="server" OnClientClick='return confirm("Are you sure you want to delete this data?");'
                                                                                        Text="Delete" CommandName="Delete" CssClass="buttonnorm" />
                                                                                    <%--<asp:ImageButton ID="ImageButton1" runat="server" CausesValidation="False" OnClientClick='return confirm("Are you sure you want to delete this data?");'
                                                                                    CommandName="Delete" ImageUrl="~/images/remove.png" Text="Delete" />--%>
                                                                                </ItemTemplate>
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField HeaderText="Enable/Disable">
                                                                                <ItemTemplate>
                                                                                    <asp:LinkButton ID="lnkcolor_ED" Text='<%#Bind("Status") %>' runat="server" OnClick="lnkcolor_ED"
                                                                                        OnClientClick="return confirm('Do you want to Enable_Disable Color')" />
                                                                                </ItemTemplate>
                                                                                <ItemStyle HorizontalAlign="Center" />
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField Visible="false">
                                                                                <ItemTemplate>
                                                                                    <asp:Label ID="lblcolorenable_disable" Text='<%#Bind("Enable_Disable") %>' runat="server" />
                                                                                </ItemTemplate>
                                                                            </asp:TemplateField>
                                                                        </Columns>
                                                                        <EmptyDataRowStyle CssClass="gvemptytext" />
                                                                    </asp:GridView>
                                                                </div>
                                                            </td>
                                                            <td>
                                                                <div id="DivSize" style="overflow: auto; width: 350px; height: 300px">
                                                                    <asp:GridView ID="gdBuyerSize" runat="server" DataKeyNames="Sr_No" AutoGenerateColumns="False"
                                                                        OnRowDataBound="gdBuyerSize_RowDataBound" Width="100%">
                                                                        <HeaderStyle CssClass="gvheaders" />
                                                                        <AlternatingRowStyle CssClass="gvalts" />
                                                                        <Columns>
                                                                            <asp:BoundField DataField="SIZEFt" HeaderText="Size Ft Format">
                                                                                <HeaderStyle Font-Bold="True" HorizontalAlign="Center" />
                                                                                <ItemStyle HorizontalAlign="Center" />
                                                                            </asp:BoundField>
                                                                            <asp:BoundField DataField="SIZEMtr" HeaderText="Size Mtr Format">
                                                                                <HeaderStyle Font-Bold="True" HorizontalAlign="Center" />
                                                                                <ItemStyle HorizontalAlign="Center" />
                                                                            </asp:BoundField>
                                                                            <asp:BoundField DataField="SizeInch" HeaderText="Size Inch Format">
                                                                                <HeaderStyle Font-Bold="True" HorizontalAlign="Center" />
                                                                                <ItemStyle HorizontalAlign="Center" />
                                                                            </asp:BoundField>
                                                                            <asp:BoundField DataField="ProdSizeft" HeaderText="Prod. Ft. Format">
                                                                                <HeaderStyle Font-Bold="True" HorizontalAlign="Center" />
                                                                                <ItemStyle HorizontalAlign="Center" />
                                                                            </asp:BoundField>
                                                                            <asp:BoundField DataField="ProdSizeMtr" HeaderText="Prod. Mtr Format">
                                                                                <HeaderStyle Font-Bold="True" HorizontalAlign="Center" />
                                                                                <ItemStyle HorizontalAlign="Center" />
                                                                            </asp:BoundField>
                                                                        </Columns>
                                                                        <EmptyDataRowStyle CssClass="gvemptytext" />
                                                                    </asp:GridView>
                                                                </div>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                    <asp:HiddenField ID="hnCQsrno" Value="0" runat="server" />
                                                    <asp:HiddenField ID="hnCDsrno" Value="0" runat="server" />
                                                    <asp:HiddenField ID="hnQSRNO" Value="0" runat="server" />
                                                    <asp:HiddenField ID="hnDSRNO" Value="0" runat="server" />
                                                    <asp:HiddenField ID="hnCSRNO" Value="0" runat="server" />
                                                </ContentTemplate>
                                                <Triggers>
                                                    <asp:PostBackTrigger ControlID="btnPreview" />
                                                </Triggers>
                                            </asp:UpdatePanel>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </div>
            </td>
        </tr>
    </table>
    </form>
</body>
</html>
