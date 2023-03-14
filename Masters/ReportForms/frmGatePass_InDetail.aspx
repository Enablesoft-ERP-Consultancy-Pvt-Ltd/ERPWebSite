<%@ Page Language="C#" AutoEventWireup="true" CodeFile="frmGatePass_InDetail.aspx.cs"
    Inherits="Masters_ReportForms_frmGatePass_InDetail_" MasterPageFile="~/ERPmaster.master"
    Title="General Gate Pass/In Detail" %>

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
        //        function Validate() {
        //            if (document.getElementById('RDProcessIssRecDetailWithConsumpton').checked == true) {
        //                if (document.getElementById('DDProcessName').options[document.getElementById('DDProcessName').selectedIndex].value == 0) {
        //                    alert("Please select process name....!");
        //                    document.getElementById("DDProcessName").focus();
        //                    return false;
        //                }
        //                if (document.getElementById('DDEmpName').options[document.getElementById('DDEmpName').selectedIndex].value == 0) {
        //                    alert("Please select employee name....!");
        //                    document.getElementById("DDEmpName").focus();
        //                    return false;
        //                }
        //                //               if (document.getElementById('DDChallanNo').options[document.getElementById('DDChallanNo').selectedIndex].value == 0) {
        //                //                    alert("Please select challan/order number....!");
        //                //                    document.getElementById("DDChallanNo").focus();
        //                //                    return false;
        //                //                }
        //            }
        //        }
    </script>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <table width="75%">
                <tr>
                    <td style="width: 300px" valign="top">
                        <div style="width: 287px; padding-top: 5px; height: 148px; float: left; border-style: solid;
                            border-width: thin">
                            &nbsp;&nbsp;
                            <asp:RadioButton ID="RDGatePassDetail" Text="Gate Pass Detail" runat="server" CssClass="labelbold"
                                GroupName="OrderType" />
                            <br />
                            &nbsp;&nbsp;
                            <asp:RadioButton ID="RDGateInDetail" Text="Gate In Detail" runat="server" CssClass="labelbold"
                                GroupName="OrderType" /><br />
                            &nbsp;&nbsp;
                            <asp:RadioButton ID="RDGatePass_inDetail" Text="GatePass/In Detail" runat="server"
                                CssClass="labelbold" GroupName="OrderType" /><br />
                        </div>
                    </td>
                    <td>
                        <div style="float: left; width: 550px; height: 500px;">
                            <table style="width: auto;">
                                <tr>
                                    <td>
                                        <asp:Label ID="Label1" runat="server" CssClass="labelbold" Text="CompanyName"></asp:Label>
                                    </td>
                                    <td colspan="3">
                                        <asp:DropDownList ID="DDCompany" runat="server" AutoPostBack="True" CssClass="dropdown"
                                            Width="276px">
                                        </asp:DropDownList>
                                        <cc1:ListSearchExtender ID="ListSearchExtender3" runat="server" TargetControlID="DDCompany"
                                            ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                                        </cc1:ListSearchExtender>
                                        <br />
                                    </td>
                                </tr>
                                <tr>
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
                                <tr id="TRRecChallan" runat="server">
                                    <td>
                                        <asp:Label ID="Label4" runat="server" CssClass="labelbold" Text="GatePass/In No."></asp:Label>
                                    </td>
                                    <td colspan="3">
                                        <asp:DropDownList ID="DDGatePass_In" runat="server" AutoPostBack="True" CssClass="dropdown"
                                            Width="276px">
                                        </asp:DropDownList>
                                        <cc1:ListSearchExtender ID="ListSearchExtender4" runat="server" TargetControlID="DDGatePass_In"
                                            ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                                        </cc1:ListSearchExtender>
                                        &nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="Label2" runat="server" CssClass="labelbold" Text="Godown Name"></asp:Label>
                                    </td>
                                    <td colspan="3">
                                        <asp:DropDownList ID="DDGodownName" runat="server" CssClass="dropdown" Width="276px">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblcategoryname" runat="server" CssClass="labelbold" Text="Category Name"></asp:Label>
                                    </td>
                                    <td colspan="3">
                                        <asp:DropDownList ID="DDCategory" runat="server" AutoPostBack="True" CssClass="dropdown"
                                            Width="276px" OnSelectedIndexChanged="DDCategory_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr id="TRddItemName" runat="server">
                                    <td>
                                        <asp:Label ID="lblitemname" runat="server" CssClass="labelbold" Text="Item Name"></asp:Label>
                                    </td>
                                    <td colspan="3">
                                        <asp:DropDownList ID="ddItemName" runat="server" AutoPostBack="True" CssClass="dropdown"
                                            Width="276px" OnSelectedIndexChanged="ddItemName_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr id="TRDDQuality" runat="server" visible="false">
                                    <td>
                                        <asp:Label ID="lblqualityname" runat="server" CssClass="labelbold" Text="Quality"></asp:Label>
                                    </td>
                                    <td colspan="3">
                                        <asp:DropDownList ID="DDQuality" runat="server" AutoPostBack="True" CssClass="dropdown"
                                            Width="276px">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr id="TRDDDesign" runat="server" visible="false">
                                    <td>
                                        <asp:Label ID="lbldesignname" runat="server" CssClass="labelbold" Text="Design"></asp:Label>
                                    </td>
                                    <td colspan="3">
                                        <asp:DropDownList ID="DDDesign" runat="server" CssClass="dropdown" Width="276px">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr id="TRDDColor" runat="server" visible="false">
                                    <td>
                                        <asp:Label ID="lblcolorname" runat="server" CssClass="labelbold" Text="Color"></asp:Label>
                                    </td>
                                    <td colspan="3">
                                        <asp:DropDownList ID="DDColor" runat="server" CssClass="dropdown" Width="276px">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="LblGateInOutPassNo" runat="server" CssClass="labelbold" Text="GateIn/Out Pass No."></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="TxtGateInOutPassNo" runat="server" CssClass="textb" Width="100px"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
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
                                <tr>
                                    <td align="right" colspan="4">
                                        <asp:CheckBox ID="chkexcelexport" Text="Excel Export" CssClass="checkboxbold" runat="server" />
                                        &nbsp;<asp:Button ID="BtnPreview" runat="server" CssClass="buttonnorm" Text="Preview"
                                            OnClick="BtnPreview_Click" />
                                        &nbsp;<asp:Button ID="BtnClose" runat="server" CssClass="buttonnorm" Text="Close"
                                            OnClientClick="return CloseForm();" />
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
