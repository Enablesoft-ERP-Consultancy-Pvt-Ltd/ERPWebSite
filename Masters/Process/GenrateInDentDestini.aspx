<%@ Page Language="C#" AutoEventWireup="true" EnableEventValidation="false" CodeFile="GenrateInDentDestini.aspx.cs"
    Inherits="Masters_Process_GenrateInDentDestini" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register Src="~/UserControls/ucmenu.ascx" TagName="ucmenu" TagPrefix="uc1" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <link rel="Stylesheet" type="text/css" href="../../App_Themes/Default/Style.css" />
    <script src="../../Scripts/JScript.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery-1.4.1.js" type="text/javascript"></script>
    <script type="text/javascript">
        function CloseForm() {
            window.location.href = "../../main.aspx";
        }
        function NewForm() {
            window.location.href = "GenrateInDentDestini.aspx";
        }
        function Preview() {
            window.open("../../ReportViewer.aspx", "GenrateIndentReport");
        }

        function doConfirm() {
            var r = confirm("Do you want to delete ?");
            document.getElementById("hnsst").value = r;
            if (r == true) {
                x = "You pressed OK!";
                return true;
            }
            else {
                x = "You pressed Cancel!";
                return false;
            }
            //alert(x);
        }
        function CHeckDate() {
            var ODate = document.getElementById('TxtDate').value;
            var DDate = document.getElementById('TxtrecDate').value;
            var day1 = ODate.substring(0, 2);
            var month1 = ODate.substring(3, 6);
            var year1 = ODate.substring(7, 11);

            var day2 = DDate.substring(0, 2);
            var month2 = DDate.substring(3, 6);
            var year2 = DDate.substring(7, 11);

            month1 = changeFormatStringtoNumber(month1);
            month2 = changeFormatStringtoNumber(month2);
            //            alert(month2);
            var d1 = new Date(year1, month1, day1);
            var d2 = new Date(year2, month2, day2);
            if (d1 > d2) {
                alert("Receive date can not be less than Issue date! ");
                document.getElementById('TxtrecDate').value = ODate;
                return false;
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
        <tr>
            <td align="center" height="inherit" valign="top" class="style1" colspan="2">
                <div id="1" style="height: auto" align="left">
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                        <ContentTemplate>
                            <table>
                                <tr>
                                    <td>
                                        <table>
                                            <tr>
                                                <td class="tdstyle">
                                                    <asp:CheckBox ID="ChkEditOrder" runat="server" Text="EDIT ORDER" AutoPostBack="True"
                                                        OnCheckedChanged="ChkEditOrder_CheckedChanged" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="tdstyle">
                                                    CompanyName<br />
                                                    <asp:DropDownList ID="DDCompanyName" runat="server" AutoPostBack="true" CssClass="dropdown"
                                                        OnSelectedIndexChanged="DDCompanyName_SelectedIndexChanged" Width="110">
                                                    </asp:DropDownList>
                                                </td>
                                                <td class="tdstyle">
                                                    Customer Code<asp:RequiredFieldValidator ID="RequiredFieldValidator11" runat="server"
                                                        ControlToValidate="DDCustomerCode" ErrorMessage="please Select Customer" ForeColor="Red"
                                                        SetFocusOnError="true" ValidationGroup="f1">*</asp:RequiredFieldValidator>
                                                    <br />
                                                    <asp:DropDownList ID="DDCustomerCode" runat="server" AutoPostBack="true" CssClass="dropdown"
                                                        OnSelectedIndexChanged="DDCustomerCode_SelectedIndexChanged" Width="110">
                                                    </asp:DropDownList>
                                                </td>
                                                <td class="tdstyle">
                                                    Order No<asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ControlToValidate="ddOrderNo"
                                                        ErrorMessage="please Select Order No" ForeColor="Red" SetFocusOnError="true"
                                                        ValidationGroup="f1">*</asp:RequiredFieldValidator>
                                                    <br />
                                                    <asp:DropDownList ID="ddOrderNo" runat="server" AutoPostBack="true" CssClass="dropdown"
                                                        OnSelectedIndexChanged="ddOrderNo_SelectedIndexChanged" Width="110">
                                                    </asp:DropDownList>
                                                </td>
                                                <td class="tdstyle">
                                                    ProcessName<asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server"
                                                        ControlToValidate="DDProcessName" ErrorMessage="please Select Process" ForeColor="Red"
                                                        SetFocusOnError="true" ValidationGroup="f1">*</asp:RequiredFieldValidator>
                                                    <br />
                                                    <asp:DropDownList ID="DDProcessName" runat="server" AutoPostBack="true" CssClass="dropdown"
                                                        OnSelectedIndexChanged="DDProcessName_SelectedIndexChanged" Width="110">
                                                    </asp:DropDownList>
                                                </td>
                                                <td class="tdstyle">
                                                    PartyName<asp:RequiredFieldValidator ID="RequiredFieldValidator8" runat="server"
                                                        ControlToValidate="DDPartyName" ErrorMessage="please Select PartyName" ForeColor="Red"
                                                        SetFocusOnError="true" ValidationGroup="f1">*</asp:RequiredFieldValidator>
                                                    <br />
                                                    <asp:DropDownList ID="DDPartyName" runat="server" AutoPostBack="true" CssClass="dropdown"
                                                        OnSelectedIndexChanged="DDPartyName_SelectedIndexChanged" Width="110">
                                                    </asp:DropDownList>
                                                </td>
                                                <td class="tdstyle" runat="server" id="tdindentno">
                                                    Indent No<asp:RequiredFieldValidator ID="RequiredFieldValidator12" runat="server"
                                                        ControlToValidate="DDPartyName" ErrorMessage="please Select PartyName" ForeColor="Red"
                                                        SetFocusOnError="true" ValidationGroup="f1">*</asp:RequiredFieldValidator>
                                                    <br />
                                                    <asp:DropDownList ID="ddindentno" runat="server" AutoPostBack="true" CssClass="dropdown"
                                                        OnSelectedIndexChanged="ddindentno_SelectedIndexChanged" Width="110">
                                                    </asp:DropDownList>
                                                </td>
                                                <td class="tdstyle">
                                                    IssueDate<asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server"
                                                        ControlToValidate="TxtDate" ErrorMessage="please Enter Date" ForeColor="Red"
                                                        SetFocusOnError="true" ValidationGroup="f1">*</asp:RequiredFieldValidator>
                                                    <br />
                                                    <asp:TextBox ID="TxtDate" runat="server" CssClass="textb" Width="100px" onchange="return CHeckDate();"></asp:TextBox>
                                                    <asp:CalendarExtender ID="CalendarExtender1" runat="server" Format="dd-MMM-yyyy"
                                                        TargetControlID="TxtDate">
                                                    </asp:CalendarExtender>
                                                </td>
                                                <td class="tdstyle">
                                                    Receive Date<asp:RequiredFieldValidator ID="RequiredFieldValidator10" runat="server"
                                                        ControlToValidate="TxtrecDate" ErrorMessage="please Enter Date" ForeColor="Red"
                                                        SetFocusOnError="true" ValidationGroup="f1">*</asp:RequiredFieldValidator>
                                                    <br />
                                                    <asp:TextBox ID="TxtrecDate" runat="server" CssClass="textb" Width="100px" onchange="return CHeckDate();"></asp:TextBox>
                                                    <asp:CalendarExtender ID="CalendarExtender2" runat="server" Format="dd-MMM-yyyy"
                                                        TargetControlID="TxtrecDate">
                                                    </asp:CalendarExtender>
                                                </td>
                                                <td id="tdindent" runat="server" class="tdstyle">
                                                    Indent No.<br />
                                                    <asp:TextBox ID="TxtIndentNo" runat="server" CssClass="textb" Enabled="false" Width="110"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td id="procode" runat="server" class="tdstyle">
                                                    ProdCode<br />
                                                    <asp:TextBox ID="TxtProdCode" runat="server" AutoPostBack="true" CssClass="textb"
                                                        OnTextChanged="TxtProdCode_TextChanged" Width="100px"></asp:TextBox>
                                                    <cc1:AutoCompleteExtender ID="TxtProdCode_AutoCompleteExtender" runat="server" EnableCaching="true"
                                                        Enabled="True" MinimumPrefixLength="1" ServiceMethod="GetQuality" TargetControlID="TxtProdCode"
                                                        UseContextKey="True">
                                                    </cc1:AutoCompleteExtender>
                                                </td>
                                                <td>
                                                    <asp:Label ID="LblCategory" class="tdstyle" runat="server" Text=""></asp:Label>
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="DDCategory"
                                                        ErrorMessage="please Select Category" ForeColor="Red" SetFocusOnError="true"
                                                        ValidationGroup="f1">*</asp:RequiredFieldValidator>
                                                    <br />
                                                    <asp:DropDownList ID="DDCategory" runat="server" AutoPostBack="true" CssClass="dropdown"
                                                        OnSelectedIndexChanged="DDCategory_SelectedIndexChanged" Width="110">
                                                    </asp:DropDownList>
                                                </td>
                                                <td>
                                                    <asp:Label ID="LblItemName" class="tdstyle" runat="server" Text="Label"></asp:Label>
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ControlToValidate="DDItem"
                                                        ErrorMessage="please Select Item" ForeColor="Red" SetFocusOnError="true" ValidationGroup="f1">*</asp:RequiredFieldValidator>
                                                    <br />
                                                    <asp:DropDownList ID="DDItem" runat="server" AutoPostBack="true" CssClass="dropdown"
                                                        OnSelectedIndexChanged="DDItem_SelectedIndexChanged" Width="110">
                                                    </asp:DropDownList>
                                                </td>
                                                <td id="TdQuality" runat="server" visible="false">
                                                    <asp:Label ID="LblQuality" class="tdstyle" runat="server" Text="Label"></asp:Label>
                                                    <br />
                                                    <asp:DropDownList ID="DDQuality" runat="server" AutoPostBack="true" CssClass="dropdown"
                                                        OnSelectedIndexChanged="DDQuality_SelectedIndexChanged" Width="110">
                                                    </asp:DropDownList>
                                                </td>
                                                <td id="TdDesign" runat="server" visible="false">
                                                    <asp:Label ID="LblDesign" class="tdstyle" runat="server" Text="Label"></asp:Label>
                                                    <br />
                                                    <asp:DropDownList ID="DDDesign" runat="server" AutoPostBack="True" CssClass="dropdown"
                                                        OnSelectedIndexChanged="DDDesign_SelectedIndexChanged" Width="110">
                                                    </asp:DropDownList>
                                                </td>
                                                <td id="TdColor" runat="server" visible="false">
                                                    <asp:Label ID="LblColor" class="tdstyle" runat="server" Text="Label"></asp:Label>
                                                    <br />
                                                    <asp:DropDownList ID="DDColor" runat="server" AutoPostBack="True" CssClass="dropdown"
                                                        OnSelectedIndexChanged="DDColor_SelectedIndexChanged" Width="110">
                                                    </asp:DropDownList>
                                                </td>
                                                <td id="TdColorShade" runat="server" visible="false">
                                                    <asp:Label ID="LblColorShade" class="tdstyle" runat="server" Text="Label"></asp:Label>
                                                    <br />
                                                    <asp:DropDownList ID="DDColorShade" runat="server" AutoPostBack="True" CssClass="dropdown"
                                                        OnSelectedIndexChanged="DDColorShade_SelectedIndexChanged" Width="110">
                                                    </asp:DropDownList>
                                                </td>
                                                <td id="TdShape" runat="server" visible="false">
                                                    <asp:Label ID="LblShape" class="tdstyle" runat="server" Text="Label"></asp:Label>
                                                    <br />
                                                    <asp:DropDownList ID="DDShape" runat="server" AutoPostBack="True" CssClass="dropdown"
                                                        OnSelectedIndexChanged="DDShape_SelectedIndexChanged" Width="110">
                                                    </asp:DropDownList>
                                                </td>
                                                <td id="TdSize" runat="server" class="tdstyle" visible="false">
                                                    &nbsp;&nbsp;
                                                    <asp:Label ID="LblSize" runat="server" Text="Label"></asp:Label>
                                                    <asp:CheckBox ID="ChkFt" runat="server" AutoPostBack="True" OnCheckedChanged="ChkFt_CheckedChanged"
                                                        Text="Ft" Visible="false" />
                                                    <br />
                                                    <asp:DropDownList ID="DDSize" runat="server" AutoPostBack="True" CssClass="dropdown"
                                                        OnSelectedIndexChanged="DDSize_SelectedIndexChanged" Width="110px">
                                                    </asp:DropDownList>
                                                </td>
                                                <td id="TdFinish_Type" runat="server" visible="false">
                                                    <asp:Label ID="LblFinish_Type" class="tdstyle" runat="server" Text="Finish_Type"></asp:Label>
                                                    <br />
                                                    <asp:DropDownList ID="ddFinish_Type" runat="server" CssClass="dropdown">
                                                    </asp:DropDownList>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="tdstyle">
                                                    Unit<br />
                                                    <asp:DropDownList ID="ddUnit" runat="server" AutoPostBack="True" CssClass="dropdown"
                                                        Width="100px">
                                                    </asp:DropDownList>
                                                </td>
                                                <td class="tdstyle">
                                                    Lot No.<br />
                                                    <asp:DropDownList ID="ddllotno" runat="server" AutoPostBack="True" CssClass="dropdown"
                                                        OnSelectedIndexChanged="ddllotno_SelectedIndexChanged" Width="100px">
                                                    </asp:DropDownList>
                                                    <td runat="server" visible="false" class="tdstyle">
                                                        Stock Qty<br />
                                                        <asp:TextBox ID="txtstock" runat="server" CssClass="textb" Width="80px"></asp:TextBox>
                                                    </td>
                                                    <td runat="server" visible="false" class="tdstyle">
                                                        TotalQty
                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtTotalQty"
                                                            ErrorMessage="TotalQty can not be null..." ForeColor="Red" SetFocusOnError="true"
                                                            ValidationGroup="f1">*</asp:RequiredFieldValidator>
                                                        <br />
                                                        <asp:TextBox ID="txtTotalQty" runat="server" CssClass="textb" Enabled="false" Width="80px"></asp:TextBox>
                                                    </td>
                                                    <td class="tdstyle">
                                                        PreQty
                                                        <br />
                                                        <asp:TextBox ID="TxtPreQty" runat="server" CssClass="textb" Enabled="false" Width="80px"></asp:TextBox>
                                                    </td>
                                                    <td class="tdstyle">
                                                        Qty
                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator9" runat="server" ControlToValidate="txtQty"
                                                            ErrorMessage="please Enter Qty" ForeColor="Red" SetFocusOnError="true" ValidationGroup="f1">*</asp:RequiredFieldValidator>
                                                        <br />
                                                        <asp:TextBox ID="txtQty" runat="server" AutoPostBack="True" CssClass="textb" OnTextChanged="TxtQty_TextChanged"
                                                            Width="80px"></asp:TextBox>
                                                    </td>
                                                    <td class="tdstyle">
                                                        Rate
                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="TxtRate"
                                                            ErrorMessage="please Enter Rate" ForeColor="Red" SetFocusOnError="true" ValidationGroup="f1">*</asp:RequiredFieldValidator>
                                                        <br />
                                                        <asp:TextBox ID="TxtRate" runat="server" CssClass="textb" Width="80px"></asp:TextBox>
                                                        <%--<a  href="AddDyeingRat.aspx?keepThis=true&TB_iframe=true&width=700px"  class="thickbox">AddRate</a>--%>
                                                    </td>
                                                    <td class="tdstyle">
                                                        Weight<br />
                                                        <asp:TextBox ID="txtweight" runat="server" CssClass="textb" Width="80px"></asp:TextBox>
                                                    </td>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="2" class="tdstyle">
                                                    Remark
                                                    <br />
                                                    <asp:TextBox ID="txtremark" runat="server" TextMode="MultiLine" CssClass="textb"
                                                        Width="150px"></asp:TextBox>
                                                </td>
                                                <td align="right" colspan="3">
                                                    <br />
                                                    <asp:Button ID="btnnew" runat="server" CssClass="buttonnorm" OnClientClick="return NewForm();"
                                                        Text="New" ValidationGroup="f1" />
                                                    <asp:Button ID="BtnSave" runat="server" CssClass="buttonnorm" OnClick="BtnSave_Click"
                                                        OnClientClick="return confirm('Do you want to save data?')" Text="Save" ValidationGroup="f1" />
                                                    <asp:Button ID="BtnPreview" runat="server" CssClass="buttonnorm" Enabled="true" Text="Preview"
                                                        OnClientClick="javascript: Preview();" />
                                                    <asp:Button ID="BtnClose" runat="server" CssClass="buttonnorm" OnClientClick="return CloseForm();"
                                                        Text="Close" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="4">
                                                    <asp:TextBox ID="TxtFinishedid" runat="server" BorderStyle="None" Height="0px" Width="0px"></asp:TextBox>
                                                    <asp:Label ID="lblMessage" runat="server" Font-Bold="true" ForeColor="Red" Text=""></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="2">
                                                    <asp:ValidationSummary ID="ValidationSummary1" runat="server" ForeColor="RED" ShowMessageBox="true"
                                                        ValidationGroup="f1" />
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                    <td>
                                        <asp:GridView ID="DGSHOWDATA" runat="server" AllowPaging="true" AutoGenerateColumns="False"
                                            OnPageIndexChanging="DGSHOWDATA_PageIndexChanging" PageSize="5" CssClass="grid-view"
                                            OnRowCreated="DGSHOWDATA_RowCreated" OnRowDataBound="DGSHOWDATA_RowDataBound"
                                            OnSelectedIndexChanged="DGSHOWDATA_SelectedIndexChanged">
                                            <PagerStyle CssClass="PagerStyle" />
                                            <Columns>
                                                <%--<asp:BoundField DataField="PRODUCTCODE" HeaderText="PRODUCTCODE" />--%>
                                                <asp:TemplateField HeaderText="PRODUCTCODE">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblPRODUCTCODE" runat="server" Text='<%# Bind("PRODUCTCODE") %>' />
                                                    </ItemTemplate>
                                                    <HeaderStyle Width="80px" />
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                    </td>
                                </tr>
                            </table>
                            <table>
                                <tr>
                                    <td>
                                        <div id="Div1" runat="server">
                                            <asp:GridView ID="DGShowConsumption" runat="server" AutoGenerateColumns="false" DataKeyNames="Item_Finished_Id"
                                                CssClass="grid-view" OnRowCreated="DGShowConsumption_RowCreated">
                                                <Columns>
                                                    <asp:BoundField DataField="ProductCode" HeaderText="ProductCode" />
                                                    <asp:BoundField DataField="Description" HeaderText="Description" />
                                                    <asp:BoundField DataField="OrderQty" HeaderText="OrderQty" />
                                                    <asp:BoundField DataField="Consmp" HeaderText="Consmp" />
                                                    <asp:BoundField DataField="StockQty" HeaderText="StockQty" />
                                                </Columns>
                                            </asp:GridView>
                                        </div>
                                    </td>
                                    <td>
                                    </td>
                                    <td>
                                    </td>
                                    <td>
                                    </td>
                                    <td>
                                    </td>
                                    <td>
                                        <div id="gride" runat="server">
                                            <asp:GridView ID="DGIndentDetail" runat="server" AutoGenerateColumns="false" DataKeyNames="indentdetailid"
                                                OnRowDataBound="DGIndentDetail_RowDataBound" OnSelectedIndexChanged="DGIndentDetail_SelectedIndexChanged"
                                                CssClass="grid-view" OnRowCreated="DGIndentDetail_RowCreated" OnRowDeleting="DGIndentDetail_RowDeleting">
                                                <Columns>
                                                    <asp:TemplateField Visible="false">
                                                        <ItemTemplate>
                                                            <asp:Label ID="LblindentID" runat="server" Visible="false" Text='<%# Bind("IndentId") %>' />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:BoundField DataField="IndentId" HeaderText="IndentId" Visible="false" />
                                                    <asp:BoundField DataField="IndentNo" HeaderText="IndentNo" />
                                                    <asp:BoundField DataField="PPNo" HeaderText="PPNo" Visible="false" />
                                                    <asp:BoundField DataField="InDescription" HeaderText="InDescription" />
                                                    <asp:BoundField DataField="OutDescription" HeaderText="OutDescription" Visible="false" />
                                                    <asp:BoundField DataField="Rate" HeaderText="Rate" />
                                                    <asp:BoundField DataField="Quantity" HeaderText="Qty" />
                                                    <asp:BoundField DataField="Weight" HeaderText="weight" />
                                                    <asp:TemplateField HeaderText=" " Visible="false">
                                                        <ItemTemplate>
                                                            <asp:Label ID="OFinishedId" runat="server" Visible="false" Text='<%# Bind("OFinishedId") %>' />
                                                            <asp:Label ID="O_FINISHED_TYPE_ID" runat="server" Visible="false" Text='<%# Bind("O_FINISHED_TYPE_ID") %>' />
                                                        </ItemTemplate>
                                                        <HeaderStyle Width="80px" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField ShowHeader="False">
                                                        <ItemTemplate>
                                                            <%-- <asp:LinkButton ID="LinkButton1" runat="server" CausesValidation="False" CommandName="Delete"
                                                    Text="Del" OnClientClick="return confirm('Do You Want To Delete ?')"></asp:LinkButton>--%>
                                                            <asp:LinkButton ID="LinkButton1" runat="server" CausesValidation="False" CommandName="Delete"
                                                                Text="Del" OnClientClick="javascript:doConfirm();"></asp:LinkButton>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                </Columns>
                                            </asp:GridView>
                                        </div>
                                        <asp:HiddenField ID="hnsst" runat="server" />
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
