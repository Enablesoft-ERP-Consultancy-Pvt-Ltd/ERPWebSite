<%@ Page Language="C#" AutoEventWireup="true" CodeFile="FrmIndentRecDetailWithMultipleOrder.aspx.cs"
    Inherits="Masters_ReportForms_FrmIndentRecDetailWithMultipleOrder" Title="INDENT_REC_DETAIL_WithOrder"
    EnableEventValidation="false" MasterPageFile="~/ERPmaster.master" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="page" ContentPlaceHolderID="CPH_Form" runat="server">
    <br />
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
        function Validate() {
            if (document.getElementById('RDProcessIssRecDetailWithConsumpton').checked == true) {
                if (document.getElementById('DDProcessName').options[document.getElementById('DDProcessName').selectedIndex].value == 0) {
                    alert("Please select process name....!");
                    document.getElementById("DDProcessName").focus();
                    return false;
                }
                if (document.getElementById('DDEmpName').options[document.getElementById('DDEmpName').selectedIndex].value == 0) {
                    alert("Please select employee name....!");
                    document.getElementById("DDEmpName").focus();
                    return false;
                }
                //               if (document.getElementById('DDChallanNo').options[document.getElementById('DDChallanNo').selectedIndex].value == 0) {
                //                    alert("Please select challan/order number....!");
                //                    document.getElementById("DDChallanNo").focus();
                //                    return false;
                //                }
            }
        }
    </script>
    <script runat="server">
        protected void RadioButton_CheckedChanged(object sender, System.EventArgs e)
        {
            //Trorderstatus.Visible = false;
            TRItemName.Visible = false;
            TRQuality.Visible = false;
            TRShadeColor.Visible = false;
            //TRIndentStatus.Visible = false;
            TRemployee.Visible = true;
            TRRecChallan.Visible = true;
            TRcheckdate.Visible = true;
            // TRPartyChallanNo.Visible = false;
            // TRExportExcel.Visible = false;
            //TRProcessProgram.Visible = false;
            if (RDProcessRecDetail.Checked == true)
            {
                //TRPartyChallanNo.Visible = true;
                TRItemName.Visible = true;
                TRQuality.Visible = true;
                TRShadeColor.Visible = true;
            }

        }
    </script>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <table width="75%">
                <tr>
                    <td style="width: 300px" valign="top">
                        <div style="width: 287px; padding-top: 5px; max-height: 250px; float: left; border-style: solid;
                            border-width: thin">
                            &nbsp;&nbsp;
                            <asp:RadioButton ID="RDProcessRecDetail" Text="INDENT REC. DETAIL" runat="server"
                                CssClass="labelbold" Checked="true" GroupName="OrderType" AutoPostBack="true"
                                OnCheckedChanged="RadioButton_CheckedChanged" /><br />
                        </div>
                    </td>
                    <td>
                        <div style="float: left; width: 450px; height: 500px;">
                            <table>
                                <tr>
                                    <td>
                                        <asp:Label ID="Label1" runat="server" CssClass="labelbold" Text="CompanyName"></asp:Label>
                                    </td>
                                    <td colspan="3">
                                        <asp:DropDownList ID="DDCompany" runat="server" CssClass="dropdown" Width="276px"
                                            AutoPostBack="true" OnSelectedIndexChanged="DDCompany_SelectedIndexChanged">
                                        </asp:DropDownList>
                                        <cc1:ListSearchExtender ID="ListSearchExtender3" runat="server" TargetControlID="DDCompany"
                                            ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                                        </cc1:ListSearchExtender>
                                    </td>
                                </tr>
                                <%--<tr runat="server" id="TRCustomerCode">
                                    <td>
                                        <asp:Label ID="Label7" runat="server" Text="CustomerCode" CssClass="labelbold"></asp:Label>
                                    </td>
                                    <td colspan="3">
                                        <asp:DropDownList ID="DDCustCode" runat="server" Width="276px" CssClass="dropdown"
                                            AutoPostBack="True" OnSelectedIndexChanged="DDCustCode_SelectedIndexChanged">
                                        </asp:DropDownList>
                                        <cc1:ListSearchExtender ID="ListSearchExtender5" runat="server" TargetControlID="DDCustCode"
                                            ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                                        </cc1:ListSearchExtender>
                                    </td>
                                </tr>--%>
                                <tr runat="server" id="TRorderNo">
                                    <td>
                                        <asp:Label ID="Label8" runat="server" Text="OrderNo" CssClass="labelbold"></asp:Label>
                                    </td>
                                    <td colspan="3">
                                        <asp:DropDownList ID="DDOrderNo" runat="server" Width="276px" CssClass="dropdown"
                                            AutoPostBack="True" OnSelectedIndexChanged="DDOrderNo_SelectedIndexChanged">
                                        </asp:DropDownList>
                                        <cc1:ListSearchExtender ID="ListSearchExtender6" runat="server" TargetControlID="DDOrderNo"
                                            ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                                        </cc1:ListSearchExtender>
                                    </td>
                                </tr>
                                <tr id="TRProcessName" runat="server">
                                    <td>
                                        <asp:Label ID="Label2" runat="server" CssClass="labelbold" Text="Process Name"></asp:Label>
                                    </td>
                                    <td colspan="3">
                                        <asp:DropDownList ID="DDProcessName" runat="server" CssClass="dropdown" Width="276px"
                                            OnSelectedIndexChanged="DDProcessName_SelectedIndexChanged" AutoPostBack="true">
                                        </asp:DropDownList>
                                        <cc1:ListSearchExtender ID="ListSearchExtender1" runat="server" TargetControlID="DDProcessName"
                                            ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                                        </cc1:ListSearchExtender>
                                    </td>
                                </tr>
                                <tr id="TRemployee" runat="server">
                                    <td>
                                        <asp:Label ID="Label3" runat="server" CssClass="labelbold" Text="Emp Name"></asp:Label>
                                    </td>
                                    <td colspan="3">
                                        <asp:DropDownList ID="DDEmpName" runat="server" AutoPostBack="True" CssClass="dropdown"
                                            Width="276px" OnSelectedIndexChanged="DDEmpName_SelectedIndexChanged">
                                        </asp:DropDownList>
                                        <cc1:ListSearchExtender ID="ListSearchExtender2" runat="server" TargetControlID="DDEmpName"
                                            ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                                        </cc1:ListSearchExtender>
                                    </td>
                                </tr>
                                <%-- <tr id="TRIndentStatus" runat="server" visible="false">
                                    <td>
                                        <asp:Label ID="Label9" runat="server" CssClass="labelbold" Text="Indent Status"></asp:Label>
                                    </td>
                                    <td colspan="3">
                                        <asp:DropDownList ID="DDindentStatus" CssClass="dropdown" runat="server">
                                            <asp:ListItem Text="All" Value="0" />
                                            <asp:ListItem Text="Pending" Value="1" />
                                            <asp:ListItem Text="Complete" Value="2" />
                                        </asp:DropDownList>
                                    </td>
                                </tr>--%>
                                <tr id="TRRecChallan" runat="server">
                                    <td>
                                        <asp:Label ID="Label4" runat="server" CssClass="labelbold" Text="Indent No"></asp:Label>
                                    </td>
                                    <td colspan="3">
                                        <asp:DropDownList ID="DDIndentNo" runat="server" AutoPostBack="True" CssClass="dropdown"
                                            Width="200px" OnSelectedIndexChanged="DDIndentNo_SelectedIndexChanged">
                                        </asp:DropDownList>
                                        <asp:CheckBox ID="chksample" Text="Sample" CssClass="checkboxbold" runat="server"
                                            OnCheckedChanged="chksample_CheckedChanged" AutoPostBack="true" Visible="false" />
                                        <cc1:ListSearchExtender ID="ListSearchExtender4" runat="server" TargetControlID="DDIndentNo"
                                            ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                                        </cc1:ListSearchExtender>
                                    </td>
                                </tr>
                                <tr id="TRItemName" runat="server" visible="false">
                                    <td>
                                        <asp:Label ID="lblitemname" runat="server" CssClass="labelbold" Text="Item Name"></asp:Label>
                                    </td>
                                    <td colspan="3">
                                        <asp:DropDownList ID="ddItemName" runat="server" AutoPostBack="True" CssClass="dropdown"
                                            Width="276px" OnSelectedIndexChanged="ddItemName_SelectedIndexChanged">
                                        </asp:DropDownList>
                                        <cc1:ListSearchExtender ID="ListSearchExtender7" runat="server" TargetControlID="ddItemName"
                                            ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                                        </cc1:ListSearchExtender>
                                    </td>
                                </tr>
                                <tr id="TRQuality" runat="server" visible="false">
                                    <td>
                                        <asp:Label ID="lblqualityname" runat="server" CssClass="labelbold" Text="Quality"></asp:Label>
                                    </td>
                                    <td colspan="3">
                                        <asp:DropDownList ID="DDQuality" runat="server" AutoPostBack="True" CssClass="dropdown"
                                            Width="276px" OnSelectedIndexChanged="DDQuality_SelectedIndexChanged">
                                        </asp:DropDownList>
                                        <cc1:ListSearchExtender ID="ListSearchExtender8" runat="server" TargetControlID="DDQuality"
                                            ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                                        </cc1:ListSearchExtender>
                                    </td>
                                </tr>
                                <tr id="TRShadeColor" runat="server" visible="false">
                                    <td>
                                        <asp:Label ID="lblshadename" runat="server" CssClass="labelbold" Text="Shade Color"></asp:Label>
                                    </td>
                                    <td colspan="3">
                                        <asp:DropDownList ID="DDShadeColor" runat="server" CssClass="dropdown" Width="276px">
                                        </asp:DropDownList>
                                        <cc1:ListSearchExtender ID="ListSearchExtender12" runat="server" TargetControlID="DDShadeColor"
                                            ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                                        </cc1:ListSearchExtender>
                                    </td>
                                </tr>
                                <%--<tr id="Trorderstatus" runat="server" visible="false">
                                    <td>
                                        <asp:Label ID="lblorderstatus" Text="Order Status" CssClass="labelbold" runat="server" />
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="DDorderstatus" CssClass="dropdown" runat="server">
                                            <asp:ListItem Text="ALL" Value="-1" />
                                            <asp:ListItem Text="Running" Value="0" />
                                            <asp:ListItem Text="Complete" Value="1" />
                                        </asp:DropDownList>
                                    </td>
                                </tr>--%>
                                <%--<tr id="TRPartyChallanNo" runat="server" visible="false">
                                    <td>
                                        <asp:Label ID="Label10" runat="server" CssClass="labelbold" Text="Party ChallanNo"></asp:Label>
                                    </td>
                                    <td colspan="3">
                                        <asp:TextBox ID="txtPartyChallanNo" runat="server" CssClass="textb" Width="100px"
                                            onkeydown="return (event.keyCode!=13);"></asp:TextBox>
                                    </td>
                                </tr>--%>
                                <tr id="TRcheckdate" runat="server">
                                    <td colspan="2" align="left">
                                        <asp:CheckBox ID="ChkForDate" runat="server" Text="Check For Date" CssClass="checkboxbold"
                                            OnCheckedChanged="ChkForDate_CheckedChanged" AutoPostBack="true" />
                                    </td>
                                </tr>
                                <tr id="trDates" runat="server" visible="false">
                                    <td>
                                        <asp:Label ID="Label5" runat="server" CssClass="labelbold" Text="From Date"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="TxtFromDate" runat="server" CssClass="textb" Width="100px"></asp:TextBox>
                                        <asp:CalendarExtender ID="CalendarExtender1" runat="server" Format="dd-MMM-yyyy"
                                            TargetControlID="TxtFromDate">
                                        </asp:CalendarExtender>
                                    </td>
                                    <td align="right">
                                        <asp:Label ID="Label6" runat="server" CssClass="labelbold" Text="To Date" Width="80px"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="TxtToDate" runat="server" CssClass="textb" Width="100px"></asp:TextBox>
                                        <asp:CalendarExtender ID="CalendarExtender2" runat="server" Format="dd-MMM-yyyy"
                                            TargetControlID="TxtToDate">
                                        </asp:CalendarExtender>
                                    </td>
                                </tr>
                                <%--  <tr id="TRExportExcel" runat="server" visible="false">
                                    <td colspan="2" align="left">
                                        <asp:CheckBox ID="ChkForExportExcel" runat="server" Text="Check For Export Excel"
                                            CssClass="checkboxbold" />
                                    </td>
                                </tr>--%>
                                <tr>
                                    <td align="right" colspan="4">
                                        &nbsp;<asp:Button ID="BtnPreview" runat="server" CssClass="buttonnorm" Text="Preview"
                                            OnClick="BtnPreview_Click" />
                                        &nbsp;<asp:Button ID="BtnClose" runat="server" CssClass="buttonnorm" Text="Close"
                                            OnClientClick="return CloseForm();" Width="50px" />
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </td>
                </tr>
            </table>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="BtnPreview" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
