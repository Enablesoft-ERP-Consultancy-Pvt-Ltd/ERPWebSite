<%@ Page Language="C#" AutoEventWireup="true" CodeFile="FrmDepartmentIssueRecDetailReports.aspx.cs"
    Inherits="Masters_ReportForms_FrmDepartmentIssueRecDetailReports" EnableEventValidation="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register Src="~/UserControls/ucmenu.ascx" TagName="ucmenu" TagPrefix="uc1" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <link href="../../Styles/vijay.css" rel="stylesheet" type="text/css" />
    <script src="../../Scripts/JScript.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery-1.4.1.js" type="text/javascript"></script>
    <script type="text/javascript">
        function Preview() {
            window.open('../../reportViewer.aspx', '');
        }
        function CloseForm() {
            window.location.href = "../../main.aspx";
        }
       
    </script>
    <script runat="server">    
        protected void RadioButton_CheckedChanged(object sender, System.EventArgs e)
        {

        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <br />
        <table width="100%" border="1">
            <tr style="width: 100%" align="center">
                <td height="66px" align="center">
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
                <table style="width: 100%">
                    <tr style="width: 80%">
                        <td style="width: 50px">
                        </td>
                        <td>
                            <div style="width: 300px; height: 250px; float: left; border-style: solid; border-width: thin">
                                <asp:RadioButton ID="RDDepartmentIssueRecDetail" Text="Department Iss Rec Detail"
                                    runat="server" CssClass="labelbold" GroupName="OrderType" AutoPostBack="true"
                                    OnCheckedChanged="RadioButton_CheckedChanged" />
                                <br />
                                <asp:RadioButton ID="RDDepartmentRawMaterialIssueReport" Text="Department Raw Material Issue"
                                    runat="server" CssClass="radiobuttonnormal" GroupName="OrderType" AutoPostBack="true"
                                    OnCheckedChanged="RadioButton_CheckedChanged" />
                            </div>
                            <div style="float: left; width: 400px; height: 250px;">
                                <table>
                                    <tr>
                                        <td>
                                            <asp:Label ID="Label1" runat="server" Text="CompanyName" CssClass="labelbold"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="DDCompany" runat="server" Width="250px" CssClass="dropdown"
                                                AutoPostBack="True" OnSelectedIndexChanged="DDCompany_SelectedIndexChanged">
                                            </asp:DropDownList>
                                            <cc1:ListSearchExtender ID="ListSearchExtender12" runat="server" TargetControlID="DDCompany"
                                                ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                                            </cc1:ListSearchExtender>
                                        </td>
                                    </tr>
                                    <tr id="TRCustomerCode" runat="server" visible="true">
                                        <td>
                                            <asp:Label ID="Label2" runat="server" Text="CustomerCode" CssClass="labelbold"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="DDCustCode" runat="server" Width="250px" CssClass="dropdown"
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
                                            <asp:DropDownList ID="DDOrderNo" runat="server" Width="250px" CssClass="dropdown">
                                            </asp:DropDownList>
                                            <cc1:ListSearchExtender ID="ListSearchExtender2" runat="server" TargetControlID="DDOrderNo"
                                                ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                                            </cc1:ListSearchExtender>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="Label4" runat="server" Text="Department Name" CssClass="labelbold"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="DDDepartmentName" runat="server" Width="140px" CssClass="dropdown"
                                                AutoPostBack="True" OnSelectedIndexChanged="DDDepartmentName_SelectedIndexChanged">
                                            </asp:DropDownList>
                                            <cc1:ListSearchExtender ID="ListSearchExtender3" runat="server" TargetControlID="DDDepartmentName"
                                                ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                                            </cc1:ListSearchExtender>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="Label6" runat="server" Text="Issue No" CssClass="labelbold"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="DDIssueNo" runat="server" Width="140px" CssClass="dropdown">
                                            </asp:DropDownList>
                                            <cc1:ListSearchExtender ID="ListSearchExtender4" runat="server" TargetControlID="DDIssueNo"
                                                ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                                            </cc1:ListSearchExtender>
                                    </tr>
                                    <tr id="TrSelectDate" runat="server" visible="true">
                                        <td id="Tdselectdate" runat="server">
                                            <asp:CheckBox ID="ChkselectDate" Text="Select Date" runat="server" CssClass="checkboxbold" />
                                        </td>
                                        <td>
                                            <table>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lblfromdt" Text="From Date" runat="server" CssClass="labelbold" />
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="TxtFromDate" CssClass="textb" runat="server" Width="100px" />
                                                        <asp:CalendarExtender ID="calfrom" runat="server" TargetControlID="TxtFromDate" Format="dd-MMM-yyyy">
                                                        </asp:CalendarExtender>
                                                    </td>
                                                    <td id="Tdtodatelabel" runat="server">
                                                        <asp:Label ID="lbltodt" Text="To Date" runat="server" CssClass="labelbold" />
                                                    </td>
                                                    <td id="Tdtodate" runat="server">
                                                        <asp:TextBox ID="TxtToDate" CssClass="textb" runat="server" Width="100px" />
                                                        <asp:CalendarExtender ID="Caltodate" runat="server" TargetControlID="TxtToDate" Format="dd-MMM-yyyy">
                                                        </asp:CalendarExtender>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2" align="right">
                                            <asp:Button ID="BtnPreview" runat="server" Text="Preview" OnClick="BtnPreview_Click"
                                                CssClass="buttonnorm" />
                                            <asp:Button ID="BtnClose" runat="server" Text="Close" CssClass="buttonnorm" OnClick="BtnClose_Click" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">
                                            <asp:Label ID="lblMessage" runat="server" ForeColor="Red" CssClass="labelbold"></asp:Label>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </td>
                    </tr>
                </table>
            </ContentTemplate>
            <Triggers>
                <asp:PostBackTrigger ControlID="btnpreview" />
            </Triggers>
        </asp:UpdatePanel>
    </div>
    <div style="background-color: #edf3fe; text-align: center; width: 100%; position: relative;
        z-index: 1; margin-top: 100%">
        <span class="labelcopyr">&copy; Enablesoft Erp Consultancy Pvt. Ltd.All rights reserved.</span>
    </div>
    </form>
</body>
</html>
