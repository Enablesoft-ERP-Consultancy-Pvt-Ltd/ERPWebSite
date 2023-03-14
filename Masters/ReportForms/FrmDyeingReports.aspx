<%@ Page Language="C#" AutoEventWireup="true" CodeFile="FrmDyeingReports.aspx.cs"
    Inherits="Masters_ReportForms_FrmDyeingReports" %>

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
    <%--<script type="text/javascript" src="../../Scripts/Fixfocus.js"></script>--%>
    <script type="text/javascript">
        function priview() {
            var A = document.getElementById('TxtExcessMatBal').value;
            var B = document.getElementById('TxtexcessMatBalAmt').value;
            var c = document.getElementById('Txtprice').value;
            var D = document.getElementById('TxtFinalHissab').value;
            window.open('../../ReportViewer2.aspx?A=' + A + '&B=' + B + '&c=' + c + '&D=' + D + '');
        }
        function MsgPopUp(msg) {
            var txt = msg;
            alert(txt);
        }
        function priview1() {
            window.open('../../ReportViewer.aspx');
        }
        //window.open('../../ReportViewer1.aspx?Category=' + A + '&Item=' + Itemid + '');
        function CloseForm() {
            window.location.href = "../../main.aspx";
        }
    </script>
    <style type="text/css">
        .style1
        {
            width: 100px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <table width="100%" border="1">
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
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <table style="width: 87%">
                    <tr>
                        <td class="style1">
                        </td>
                        <td>
                            <div style="width: 287px; height: 287px; float: left; border-style: solid; border-width: thin">
                                &nbsp;&nbsp;
                                <asp:RadioButton ID="RDDyingProgramDelStatus" Text="Order Wise Delivery Status" runat="server"
                                    GroupName="OrderType" CssClass="labelbold" />
                                &nbsp;&nbsp;
                                <br />
                                &nbsp;&nbsp;
                                <asp:RadioButton ID="RDDyeingHissab1" Text="Order Wise Dyeing Hissab" runat="server"
                                    GroupName="OrderType" CssClass="labelbold" /><br />
                                &nbsp;&nbsp;
                                <asp:RadioButton ID="RDDyingProgAgainstDyer" Text="Dyeing Prog. Delivery Status Against Dyer"
                                    runat="server" GroupName="OrderType" CssClass="labelbold" />
                                <br />
                                &nbsp;&nbsp;
                                <asp:RadioButton ID="RDdyingLedger" Text="Dyeing Ledger" runat="server" GroupName="OrderType"
                                    CssClass="labelbold" />
                                <br />
                                &nbsp;&nbsp;
                                <asp:RadioButton ID="RDDHDyerWise" Text="Dyeing Hissab Dyer Wise" runat="server"
                                    GroupName="OrderType" CssClass="labelbold" />
                            </div>
                            <div style="float: left; width: 412px; height: 497px;">
                                <table>
                                    <tr>
                                        <td>
                                            <asp:Label ID="Label1" runat="server" Text="CompanyName" CssClass="labelbold"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="DDCompany" runat="server" Width="230px" CssClass="dropdown"
                                                AutoPostBack="True" OnSelectedIndexChanged="DDCompany_SelectedIndexChanged">
                                            </asp:DropDownList>
                                            <cc1:ListSearchExtender ID="ListSearchExtender2" runat="server" TargetControlID="DDCompany"
                                                ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                                            </cc1:ListSearchExtender>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="Label2" runat="server" Text="CustomerCode" CssClass="labelbold"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="DDCustCode" runat="server" Width="230px" CssClass="dropdown"
                                                AutoPostBack="True" OnSelectedIndexChanged="DDCustCode_SelectedIndexChanged">
                                            </asp:DropDownList>
                                            <cc1:ListSearchExtender ID="ListSearchExtender1" runat="server" TargetControlID="DDCustCode"
                                                ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                                            </cc1:ListSearchExtender>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="Label3" runat="server" Text="OrderNo" CssClass="labelbold"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="DDOrderNo" runat="server" Width="230px" CssClass="dropdown"
                                                AutoPostBack="True">
                                            </asp:DropDownList>
                                            <cc1:ListSearchExtender ID="ListSearchExtender3" runat="server" TargetControlID="DDOrderNo"
                                                ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                                            </cc1:ListSearchExtender>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="Label4" runat="server" Text="DyerName" CssClass="labelbold"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="DDDyerName" runat="server" Width="230px" CssClass="dropdown"
                                                AutoPostBack="True" OnSelectedIndexChanged="DDDyerName_SelectedIndexChanged">
                                            </asp:DropDownList>
                                            <cc1:ListSearchExtender ID="ListSearchExtender4" runat="server" TargetControlID="DDDyerName"
                                                ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                                            </cc1:ListSearchExtender>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="Label5" runat="server" Text="IndentNo" CssClass="labelbold"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="DDIndentno" runat="server" Width="230px" CssClass="dropdown"
                                                AutoPostBack="True">
                                            </asp:DropDownList>
                                            <cc1:ListSearchExtender ID="ListSearchExtender5" runat="server" TargetControlID="DDIndentno"
                                                ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                                            </cc1:ListSearchExtender>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="Label11" runat="server" Text="Category" CssClass="labelbold"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="DDCategory" runat="server" Width="230px" CssClass="dropdown"
                                                AutoPostBack="True" OnSelectedIndexChanged="DDCategory_SelectedIndexChanged">
                                            </asp:DropDownList>
                                            <cc1:ListSearchExtender ID="ListSearchExtender6" runat="server" TargetControlID="DDCategory"
                                                ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                                            </cc1:ListSearchExtender>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="Label6" runat="server" Text="ItemName" CssClass="labelbold"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="DDItem" runat="server" Width="230px" CssClass="dropdown" AutoPostBack="True"
                                                OnSelectedIndexChanged="DDItem_SelectedIndexChanged">
                                            </asp:DropDownList>
                                            <cc1:ListSearchExtender ID="ListSearchExtender7" runat="server" TargetControlID="DDItem"
                                                ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                                            </cc1:ListSearchExtender>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="Label7" runat="server" Text="SubItem" CssClass="labelbold"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="DDSubItem" runat="server" Width="230px" CssClass="dropdown"
                                                AutoPostBack="True" OnSelectedIndexChanged="DDSubItem_SelectedIndexChanged">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="Label8" runat="server" Text="ColorName" CssClass="labelbold"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="DDColor" runat="server" Width="230px" CssClass="dropdown" AutoPostBack="True">
                                            </asp:DropDownList>
                                            <cc1:ListSearchExtender ID="ListSearchExtender8" runat="server" TargetControlID="DDColor"
                                                ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                                            </cc1:ListSearchExtender>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="Label9" runat="server" Text="From Date:" CssClass="labelbold"></asp:Label>
                                        </td>
                                        <%--  <td style="display: none">--%>
                                        <td>
                                            <asp:TextBox ID="TxtFdate" runat="server" Width="90px" CssClass="textb"></asp:TextBox>
                                            <asp:CalendarExtender ID="CalendarExtender2" runat="server" Format="dd-MMM-yyyy"
                                                TargetControlID="TxtFdate">
                                            </asp:CalendarExtender>
                                            &nbsp;
                                            <asp:Label ID="Label10" runat="server" Text="To Date:" CssClass="labelbold"></asp:Label>
                                            <asp:TextBox ID="TxtToDate" runat="server" Width="90px" CssClass="textb"></asp:TextBox>
                                            <asp:CalendarExtender ID="CalendarExtender1" runat="server" Format="dd-MMM-yyyy"
                                                TargetControlID="TxtToDate">
                                            </asp:CalendarExtender>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2" align="right">
                            <asp:Button ID="BtnPreview" runat="server" Text="Preview" OnClick="BtnPreview_Click"
                                CssClass="buttonnorm" />
                            &nbsp;<asp:Button ID="BtnClose" runat="server" Text="Close" CssClass="buttonnorm"
                                OnClick="BtnClose_Click" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <asp:Label ID="lblMessage" runat="server" ForeColor="Red" CssClass="labelbold"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2" style="display: none">
                            <asp:TextBox ID="TxtExcessMatBal" runat="server" Width="0px" Height="0px" value="0"></asp:TextBox>
                            <asp:TextBox ID="TxtexcessMatBalAmt" runat="server" Width="0px" Height="0px" value="0"></asp:TextBox>
                            <asp:TextBox ID="Txtprice" runat="server" Width="0px" Height="0px" value="0"></asp:TextBox>
                            <asp:TextBox ID="TxtFinalHissab" runat="server" Width="0px" Height="0px" value="0"></asp:TextBox>
                        </td>
                    </tr>
                </table>
                </div> </td> </tr> </table>
            </ContentTemplate>
        </asp:UpdatePanel>
        </td>
    </div>
    <div style="background-color: #edf3fe; text-align: center; height: 43px; width: 100%">
        <span class="labelcopyr">&copy; Enablesoft Erp Consultancy Pvt. Ltd.All rights reserved.</span>
    </div>
    </form>
</body>
</html>
