<%@ Page Language="C#" AutoEventWireup="true" CodeFile="FrmProductionReports.aspx.cs"
    Inherits="Masters_ReportForms_FrmProductionReports" EnableEventValidation="false" %>

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
    <%--<script type="text/javascript" src="../../Scripts/Fixfocus.js"></script>--%>
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
            DDItemName.Visible = true;
            TRToDate.Visible = false;
            ChkForFt.Visible = true;
            TDWithOrderWiseSummary.Visible = false;
            if (RDFinishingConsumptionReport.Checked == true)
            {
                DDItemName.Visible = false;
                TRToDate.Visible = false;
                ChkForFt.Visible = false;
            }
            else if (RDProcessHissabFromDateToDate.Checked == true)
            {
                TRToDate.Visible = true;
            }
            else if (RDProcessDetailWithConsumption.Checked == true)
            {
                TRToDate.Visible = true;
            }
            else if (RDProcessIssRecWthConsumption.Checked == true)
            {
                if (Session["VarCompanyNo"].ToString() == "39")
                {
                    TDWithOrderWiseSummary.Visible = true;
                }
                else
                {
                    TDWithOrderWiseSummary.Visible = false;
                }
            }
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
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
                        <td style="width: 100px">
                        </td>
                        <td>
                            <div style="width: 380px; height: 250px; float: left; border-style: solid; border-width: thin">
                                <asp:RadioButton ID="RDProductionDeliveryStatusReport" Text="Production Delivery Status Report Against Order"
                                    runat="server" CssClass="labelbold" GroupName="OrderType" AutoPostBack="true"
                                    OnCheckedChanged="RadioButton_CheckedChanged" />
                                <%--<br />--%>
                                <br />
                                <asp:RadioButton ID="RDMaterialBalReport" Text="Material Bal Report At Contractor Against All Orders"
                                    runat="server" CssClass="labelbold" GroupName="OrderType" AutoPostBack="true"
                                    OnCheckedChanged="RadioButton_CheckedChanged" />
                                <%--<br />--%>
                                <br />
                                <asp:RadioButton ID="RDProcessIssRecWthConsumption" Text="Process Iss Rec Wth Consumption"
                                    runat="server" CssClass="labelbold" GroupName="OrderType" AutoPostBack="true"
                                    OnCheckedChanged="RadioButton_CheckedChanged" />
                                <%--<br />--%>
                                <br />
                                <asp:RadioButton ID="RDProdDeliveryStatus" Text="Production Delivery Status-Filter Against Order Or Contractor"
                                    runat="server" CssClass="labelbold" GroupName="OrderType" AutoPostBack="true"
                                    OnCheckedChanged="RadioButton_CheckedChanged" />
                                <%--<br />--%>
                                <br />
                                <asp:RadioButton ID="RDProcessHissabAgainstOrderNo" Text="Process Hissab Against Order No"
                                    runat="server" CssClass="labelbold" GroupName="OrderType" AutoPostBack="true"
                                    OnCheckedChanged="RadioButton_CheckedChanged" />
                                <%--<br />--%>
                                <br />
                                <asp:RadioButton ID="RDProductionDeliveryStatus" Text="Production Delivery Status"
                                    runat="server" CssClass="labelbold" GroupName="OrderType" AutoPostBack="true"
                                    OnCheckedChanged="RadioButton_CheckedChanged" />
                                <%--<br />--%>
                                <br />
                                <asp:RadioButton ID="RDBlanceQtyToPay" Text="Blance Qty To Pay" runat="server" CssClass="labelbold"
                                    GroupName="OrderType" AutoPostBack="true" OnCheckedChanged="RadioButton_CheckedChanged" />
                                <%--<br />--%>
                                <br />
                                <asp:RadioButton ID="RDProcessHissabFromDateToDate" Text="Process Hissab From Date To Date"
                                    runat="server" CssClass="labelbold" GroupName="OrderType" AutoPostBack="true"
                                    OnCheckedChanged="RadioButton_CheckedChanged" />
                                <%--<br />--%>
                                <br />
                                <asp:RadioButton ID="RDFinishingConsumptionReport" Text="Finishing Consumption Report"
                                    runat="server" CssClass="labelbold" GroupName="OrderType" AutoPostBack="true"
                                    OnCheckedChanged="RadioButton_CheckedChanged" />
                                <br />
                                <asp:RadioButton ID="RDProductionBalToIssueReport" Text="Production Bal To Issue Report"
                                    runat="server" CssClass="labelbold" GroupName="OrderType" Visible="false" AutoPostBack="true"
                                    OnCheckedChanged="RadioButton_CheckedChanged" />
                                <br />
                                <asp:RadioButton ID="RDProcessDetailWithConsumption" Text="Process Detail With Consumption"
                                    runat="server" CssClass="labelbold" GroupName="OrderType" AutoPostBack="true"
                                    OnCheckedChanged="RadioButton_CheckedChanged" />
                            </div>
                            <div style="float: left; width: 350px; height: 250px;">
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
                                    <tr id="Trorderstatus" runat="server">
                                        <td>
                                            <asp:Label ID="lblorderstatus" Text="Order Status" CssClass="labelbold" runat="server" />
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="DDorderstatus" CssClass="dropdown" runat="server" AutoPostBack="true"
                                                OnSelectedIndexChanged="DDorderstatus_SelectedIndexChanged">
                                                <asp:ListItem Text="ALL" Value="-1" />
                                                <asp:ListItem Text="Running" Value="0" />
                                                <asp:ListItem Text="Complete" Value="1" />
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="Label3" runat="server" Text="OrderNo" CssClass="labelbold"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="DDOrderNo" runat="server" Width="250px" CssClass="dropdown"
                                                AutoPostBack="True">
                                            </asp:DropDownList>
                                            <cc1:ListSearchExtender ID="ListSearchExtender2" runat="server" TargetControlID="DDOrderNo"
                                                ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                                            </cc1:ListSearchExtender>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="Label4" runat="server" Text="Process" CssClass="labelbold"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="DDProcess" runat="server" Width="140px" CssClass="dropdown"
                                                AutoPostBack="True" OnSelectedIndexChanged="DDProcess_SelectedIndexChanged">
                                            </asp:DropDownList>
                                            <cc1:ListSearchExtender ID="ListSearchExtender3" runat="server" TargetControlID="DDProcess"
                                                ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                                            </cc1:ListSearchExtender>
                                            &nbsp;
                                            <asp:DropDownList ID="DDItemName" runat="server" Width="100px" CssClass="dropdown"
                                                AutoPostBack="True">
                                            </asp:DropDownList>
                                            <cc1:ListSearchExtender ID="ListSearchExtender4" runat="server" TargetControlID="DDItemName"
                                                ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                                            </cc1:ListSearchExtender>
                                        </td>
                                    </tr>
                                    <tr id="TRToDate" runat="server" visible="false">
                                        <td>
                                            <asp:Label ID="Label5" runat="server" Text="To Date" CssClass="labelbold"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox CssClass="textb" ID="TxtToDate" runat="server" TabIndex="7"></asp:TextBox>
                                            <asp:CalendarExtender ID="CalendarExtender1" runat="server" Format="dd-MMM-yyyy"
                                                TargetControlID="TxtToDate">
                                            </asp:CalendarExtender>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">
                                            <asp:CheckBox ID="ChkForFt" runat="server" Text="For Ft Format" CssClass="checkboxbold" />
                                        </td>
                                        
                                    </tr>
                                    <tr>
                                         <td id="TDWithOrderWiseSummary" runat="server" visible="false" colspan="2">
                                            <asp:CheckBox ID="ChkWithOrderWiseSummary" Text="Order Wise Summary" runat="server" CssClass="checkboxbold" />
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
        </asp:UpdatePanel>
    </div>
    <div style="background-color: #edf3fe; text-align: center; width: 100%; position: relative;
        z-index: 1; margin-top: 100%">
        <span class="labelcopyr">&copy; Enablesoft Erp Consultancy Pvt. Ltd.All rights reserved.</span>
    </div>
    </form>
</body>
</html>
