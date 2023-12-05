<%@ Page Language="C#" AutoEventWireup="true" CodeFile="FrmProcessDetailIssueReceive.aspx.cs"
    Inherits="Masters_ReportForms_FrmProcessDetailIssueReceive" EnableEventValidation="false"
    Title="Process Issue_Rec Detail" MasterPageFile="~/ERPmaster.master" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Page" ContentPlaceHolderID="CPH_Form" runat="server">
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

            if (document.getElementById('CPH_Form_RDProcessIssRecDetailWithConsumpton').checked == true) {
                if (document.getElementById('CPH_Form_DDProcessName').options[document.getElementById('CPH_Form_DDProcessName').selectedIndex].value == 0) {
                    alert("Please select process name....!");
                    document.getElementById("CPH_Form_DDProcessName").focus();
                    return false;
                }
                if (document.getElementById('CPH_Form_DDEmpName').options[document.getElementById('CPH_Form_DDEmpName').selectedIndex].value == 0) {
                    alert("Please select employee name....!");
                    document.getElementById("CPH_Form_DDEmpName").focus();
                    return false;
                }

            }

            if (document.getElementById('CPH_Form_RDCommDetail').checked == true || document.getElementById('CPH_Form_RDStockNoTobeIssued').checked == true) {
                if (document.getElementById('CPH_Form_DDProcessName').options[document.getElementById('CPH_Form_DDProcessName').selectedIndex].value == 0) {
                    alert("Please select process name....!");
                    document.getElementById("CPH_Form_DDProcessName").focus();
                    return false;
                }

            }


            if (document.getElementById('CPH_Form_RDProcessOrderFolio')) {
                if (document.getElementById('RDProcessOrderFolio').checked == true) {

                    if (document.getElementById('CPH_Form_DDProcessName').options[document.getElementById('CPH_Form_DDProcessName').selectedIndex].value == 0) {
                        alert("Please select process name....!");
                        document.getElementById("CPH_Form_DDProcessName").focus();
                        return false;
                    }
                    if (document.getElementById('CPH_Form_DDEmpName').options[document.getElementById('CPH_Form_DDEmpName').selectedIndex].value == 0) {
                        alert("Please select Employee name....!");
                        document.getElementById("CPH_Form_DDEmpName").focus();
                        return false;
                    }
                    if (document.getElementById('CPH_Form_DDChallanNo').options[document.getElementById('CPH_Form_DDChallanNo').selectedIndex].value == 0) {
                        alert("Please select Challan No....!");
                        document.getElementById("CPH_Form_DDChallanNo").focus();
                        return false;
                    }
                }
            }

            if (document.getElementById('CPH_Form_RDPendingQty').checked == true) {
                if (document.getElementById('CPH_Form_DDProcessName').options[document.getElementById('CPH_Form_DDProcessName').selectedIndex].value == 0) {
                    alert("Please select process name....!");
                    document.getElementById("CPH_Form_DDProcessName").focus();
                    return false;
                }


            }

            if (document.getElementById('CPH_Form_RDStockRecQithwt').checked == true) {

                var e = $("#CPH_Form_DDProcessName").val();
                if (e == 0) {
                    alert('Please select process Name!!!');
                    return false;
                }
                if (e != 1) {
                    alert('This is only for Weaving...');
                    return false;
                }


            }

        }
        
    </script>
    <div>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <table width="85%">
                    <tr>
                        <td style="width: 300px" valign="top">
                            <div style="width: 287px; padding-top: 5px; height: 450px; float: left; border-style: solid;
                                border-width: thin">
                                &nbsp;&nbsp;
                                <br />
                                &nbsp;&nbsp;
                                <asp:RadioButton ID="RDProcessOrderFolio" runat="server" Text="Order Folio" GroupName="OrderType"
                                    CssClass="labelbold" OnCheckedChanged="RDProcessOrderFolio_CheckedChanged" Visible="false" />
                                <br />
                                &nbsp;&nbsp;
                                <asp:RadioButton ID="RDProcessIssRecDetail" Text="Process Receive Detail" runat="server"
                                    GroupName="OrderType" CssClass="labelbold" AutoPostBack="True" OnCheckedChanged="RDProcessIssRecDetail_CheckedChanged" />
                                <br />
                                 &nbsp;&nbsp;
                                <asp:RadioButton ID="RDFinishingpending" runat="server" Text="Process Pending Pcs"
                                    GroupName="OrderType" CssClass="labelbold" AutoPostBack="true" OnCheckedChanged="RDFinishingpending_CheckedChanged" />
                                <br />
                                 &nbsp;&nbsp;
                                <asp:RadioButton ID="RDFinishingIssueDetail" runat="server" Text="Process Issue Detail"
                                    GroupName="OrderType" CssClass="labelbold" AutoPostBack="True" OnCheckedChanged="RDFinishingIssueDetail_CheckedChanged" />
                                <br />
                                 &nbsp;&nbsp;
                                <asp:RadioButton ID="RDProcessIssueReceiveSummary" runat="server" Text="Process Issue Receive Summary"
                                    GroupName="OrderType" CssClass="labelbold" AutoPostBack="True" OnCheckedChanged="RDProcessIssueReceiveSummary_CheckedChanged" />
                                <br />
                                 &nbsp;&nbsp;
                                <asp:RadioButton ID="RDProcessWiseAdvancePayment" Text="Process Wise Advance Payment"
                                    runat="server" GroupName="OrderType" CssClass="labelbold" AutoPostBack="True"
                                    OnCheckedChanged="RDProcessWiseAdvancePayment_CheckedChanged" />
                                    <br>
                                    </br>
                                &nbsp;&nbsp;
                                <asp:RadioButton ID="RDGatePass" Text="Gate In OR Gate Pass Detail" runat="server"
                                    GroupName="OrderType" CssClass="labelbold" OnCheckedChanged="RDGatePass_CheckedChanged"
                                    AutoPostBack="True" /><br />
                                &nbsp;&nbsp;
                                <asp:RadioButton ID="RDProcessIssRecDetailWithConsumpton" Text="Issue Receive With Consp Detail"
                                    runat="server" GroupName="OrderType" CssClass="labelbold" AutoPostBack="True"
                                    OnCheckedChanged="RDProcessIssRecDetailWithConsumpton_CheckedChanged" />
                                &nbsp;&nbsp;
                                <br />
                                &nbsp;&nbsp;
                                <asp:RadioButton ID="RDCommDetail" Text="Commission Detail" runat="server" GroupName="OrderType"
                                    CssClass="labelbold" OnCheckedChanged="RDCommDetail_CheckedChanged" AutoPostBack="true" />
                                <br />
                                &nbsp;&nbsp;
                                <asp:RadioButton ID="RDStockNoTobeIssued" runat="server" Text="StockNo To Be Issued"
                                    GroupName="OrderType" CssClass="labelbold" AutoPostBack="true" OnCheckedChanged="RDStockNoTobeIssued_CheckedChanged" />
                                <br />
                                &nbsp;&nbsp;
                                <asp:RadioButton ID="RDPendingQty" runat="server" Text="Late Pending Qty" GroupName="OrderType"
                                    CssClass="labelbold" AutoPostBack="true" OnCheckedChanged="RDPendingQty_CheckedChanged" />
                                <br />
                                &nbsp;&nbsp;
                                <asp:RadioButton ID="RDStockRecQithwt" runat="server" Text="Stock Receive Detail with Weight"
                                    GroupName="OrderType" CssClass="labelbold" AutoPostBack="true" OnCheckedChanged="RDStockRecQithwt_CheckedChanged" />
                                <br />
                                &nbsp;&nbsp;
                                <asp:RadioButton ID="RDPerday" runat="server" Text="Per Day Production Status" GroupName="OrderType"
                                    CssClass="labelbold" AutoPostBack="true" OnCheckedChanged="RDPerday_CheckedChanged" />
                                <br />
                               
                               
                                &nbsp;&nbsp;
                                <asp:RadioButton ID="RDDailyfinreport" runat="server" Text="Daily Finishing Receive Detail"
                                    GroupName="OrderType" CssClass="labelbold" AutoPostBack="True" OnCheckedChanged="RDDailyfinreport_CheckedChanged" />
                                <br />
                                
                                &nbsp;&nbsp;
                                <asp:RadioButton ID="RDWeaverRawMaterialIssueDetail" runat="server" Text="Weaving & Finishing RawMaterial Issue Detail"
                                    Visible="false" GroupName="OrderType" CssClass="labelbold" AutoPostBack="True"
                                    OnCheckedChanged="RDWeaverRawMaterialIssueDetail_CheckedChanged" />
                                <br />
                                &nbsp;&nbsp;
                                <asp:RadioButton ID="RDWeaverRawMaterialReceiveDetail" runat="server" Text="Weaver RawMaterial Receive Detail"
                                    Visible="false" GroupName="OrderType" CssClass="labelbold" AutoPostBack="True"
                                    OnCheckedChanged="RDWeaverRawMaterialReceiveDetail_CheckedChanged" />
                                <br />
                                 &nbsp;&nbsp;
                                <asp:RadioButton ID="RDTasselIssueReceiveSummary" runat="server" Text="Tassal Issue Receive Summary"
                                    GroupName="OrderType" CssClass="labelbold" Visible="false" AutoPostBack="True" OnCheckedChanged="RDTasselIssueReceiveSummary_CheckedChanged" />
                                <br />
                              
                                 &nbsp;&nbsp;
                                <asp:RadioButton ID="RDTasselPartnerIssueSummary" runat="server" Text="Tassal Issue Report"
                                    GroupName="OrderType" CssClass="labelbold" Visible="false" AutoPostBack="True" OnCheckedChanged="RDTasselPartnerIssueSummary_CheckedChanged" />
                                <br />
                              
                                 &nbsp;&nbsp;
                                <asp:RadioButton ID="RDTasselPartnerReceiveSummary" runat="server" Text="Tassal Receive Report"
                                    GroupName="OrderType" CssClass="labelbold" Visible="false" AutoPostBack="True" OnCheckedChanged="RDTasselPartnerReceiveSummary_CheckedChanged" />
                                <br />                                
                                 &nbsp;&nbsp;
                                <asp:RadioButton ID="RDTasselMakingRawIssueDetail" runat="server" Text="Tassal RawMaterial Issue Detail"
                                    GroupName="OrderType" CssClass="labelbold" Visible="false" AutoPostBack="True" OnCheckedChanged="RDTasselMakingRawIssueDetail_CheckedChanged" />
                                <br />
                                  &nbsp;&nbsp;
                                <asp:RadioButton ID="RDFinishingBalance" runat="server" Text="Finishing Balance"
                                    GroupName="OrderType" CssClass="labelbold" AutoPostBack="True" OnCheckedChanged="RDFinishingBalance_CheckedChanged" />
                                <br />
                                &nbsp;&nbsp;
                                  <asp:RadioButton ID="RDFinishingRecSummaryWithTDS" runat="server" Text="Finishing Rec Summary With TDS" Visible="false"
                                    GroupName="OrderType" CssClass="labelbold" AutoPostBack="True" OnCheckedChanged="RDFinishingRecSummaryWithTDS_CheckedChanged" />
                                <br />
                                &nbsp;&nbsp;
                            </div>
                        </td>
                        <td valign="top">
                            <div style="float: left; width: 450px; max-height: 500px;  vertical-align:top">
                                <table>
                                    <tr>
                                        <td>
                                            <asp:Label ID="Label1" runat="server" CssClass="labelbold" Text="CompanyName"></asp:Label>
                                        </td>
                                        <td colspan="3">
                                            <asp:DropDownList ID="DDCompany" runat="server" AutoPostBack="True" CssClass="dropdown"
                                                Width="300px">
                                            </asp:DropDownList>
                                            <cc1:ListSearchExtender ID="ListSearchExtender3" runat="server" TargetControlID="DDCompany"
                                                ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                                            </cc1:ListSearchExtender>
                                        </td>
                                    </tr>
                                    <tr id="TRProcessName" runat="server">
                                        <td>
                                            <asp:Label ID="Label2" runat="server" CssClass="labelbold" Text="Process Name"></asp:Label>
                                        </td>
                                        <td colspan="3">
                                            <asp:DropDownList ID="DDProcessName" runat="server" AutoPostBack="True" CssClass="dropdown"
                                                OnSelectedIndexChanged="DDProcessName_SelectedIndexChanged" Width="300px">
                                            </asp:DropDownList>
                                            <cc1:ListSearchExtender ID="ListSearchExtender1" runat="server" TargetControlID="DDProcessName"
                                                ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                                            </cc1:ListSearchExtender>
                                        </td>
                                    </tr>
                                    <tr id="TRcustcode" runat="server" visible="false">
                                        <td>
                                            <asp:Label ID="Label10" runat="server" CssClass="labelbold" Text="Customer code"></asp:Label>
                                        </td>
                                        <td colspan="3">
                                            <asp:DropDownList ID="DDcustcode" runat="server" AutoPostBack="True" CssClass="dropdown"
                                                Width="300px" OnSelectedIndexChanged="DDcustcode_SelectedIndexChanged">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr id="TRorderno" runat="server" visible="false">
                                        <td>
                                            <asp:Label ID="Label11" runat="server" CssClass="labelbold" Text="Order No."></asp:Label>
                                        </td>
                                        <td colspan="3">
                                            <asp:DropDownList ID="DDorderno" runat="server" CssClass="dropdown" Width="300px">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr id="TREmpName" runat="server" visible="true">
                                        <td>
                                            <asp:Label ID="Label3" runat="server" CssClass="labelbold" Text="Emp Name"></asp:Label>
                                        </td>
                                        <td colspan="3">
                                            <asp:DropDownList ID="DDEmpName" runat="server" AutoPostBack="True" CssClass="dropdown"
                                                OnSelectedIndexChanged="DDEmpName_SelectedIndexChanged" Width="250px">
                                            </asp:DropDownList>
                                            <cc1:ListSearchExtender ID="ListSearchExtender2" runat="server" TargetControlID="DDEmpName"
                                                ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                                            </cc1:ListSearchExtender>
                                            <asp:CheckBox ID="ChkForComplete" Text="Complete" runat="server" CssClass="checkboxbold"  />
                                        </td>
                                    </tr>
                                    <tr id="TRRecChallan" runat="server">
                                        <td>
                                            <asp:Label ID="Label4" runat="server" CssClass="labelbold" Text="Rec Challan No"></asp:Label>
                                        </td>
                                        <td colspan="3">
                                            <asp:DropDownList ID="DDChallanNo" runat="server" AutoPostBack="True" CssClass="dropdown"
                                                Width="300px" OnSelectedIndexChanged="DDChallanNo_SelectedIndexChanged">
                                            </asp:DropDownList>
                                            <cc1:ListSearchExtender ID="ListSearchExtender4" runat="server" TargetControlID="DDChallanNo"
                                                ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                                            </cc1:ListSearchExtender>
                                        </td>
                                    </tr>
                                    <tr id="TRCategoryName" runat="server">
                                        <td>
                                            <asp:Label ID="lblcategoryname" runat="server" CssClass="labelbold" Text="Category Name"></asp:Label>
                                        </td>
                                        <td colspan="3">
                                            <asp:DropDownList ID="DDCategory" runat="server" AutoPostBack="True" CssClass="dropdown"
                                                Width="300px" OnSelectedIndexChanged="DDCategory_SelectedIndexChanged">
                                            </asp:DropDownList>
                                            <cc1:ListSearchExtender ID="ListSearchExtender5" runat="server" TargetControlID="DDCategory"
                                                ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                                            </cc1:ListSearchExtender>
                                        </td>
                                    </tr>
                                    <tr id="TRddItemName" runat="server">
                                        <td>
                                            <asp:Label ID="lblitemname" runat="server" CssClass="labelbold" Text="Item Name"></asp:Label>
                                        </td>
                                        <td colspan="3">
                                            <asp:DropDownList ID="ddItemName" runat="server" AutoPostBack="True" CssClass="dropdown"
                                                Width="300px" OnSelectedIndexChanged="ddItemName_SelectedIndexChanged">
                                            </asp:DropDownList>
                                            <cc1:ListSearchExtender ID="ListSearchExtender6" runat="server" TargetControlID="ddItemName"
                                                ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                                            </cc1:ListSearchExtender>
                                        </td>
                                    </tr>
                                    <tr id="TRDDQuality" runat="server" visible="false">
                                        <td>
                                            <asp:Label ID="lblqualityname" runat="server" CssClass="labelbold" Text="Quality"></asp:Label>
                                        </td>
                                        <td colspan="3">
                                            <asp:DropDownList ID="DDQuality" runat="server" AutoPostBack="True" CssClass="dropdown"
                                                Width="300px" OnSelectedIndexChanged="DDQuality_SelectedIndexChanged">
                                            </asp:DropDownList>
                                            <cc1:ListSearchExtender ID="ListSearchExtender7" runat="server" TargetControlID="DDQuality"
                                                ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                                            </cc1:ListSearchExtender>
                                        </td>
                                    </tr>
                                    <tr id="TRDDDesign" runat="server" visible="false">
                                        <td>
                                            <asp:Label ID="lbldesignname" runat="server" CssClass="labelbold" Text="Design"></asp:Label>
                                        </td>
                                        <td colspan="3">
                                            <asp:DropDownList ID="DDDesign" runat="server" AutoPostBack="True" CssClass="dropdown"
                                                Width="300px">
                                            </asp:DropDownList>
                                            <cc1:ListSearchExtender ID="ListSearchExtender8" runat="server" TargetControlID="DDDesign"
                                                ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                                            </cc1:ListSearchExtender>
                                        </td>
                                    </tr>
                                    <tr id="TRDDColor" runat="server" visible="false">
                                        <td>
                                            <asp:Label ID="lblcolorname" runat="server" CssClass="labelbold" Text="Color"></asp:Label>
                                        </td>
                                        <td colspan="3">
                                            <asp:DropDownList ID="DDColor" runat="server" AutoPostBack="True" CssClass="dropdown"
                                                Width="300px">
                                            </asp:DropDownList>
                                            <cc1:ListSearchExtender ID="ListSearchExtender9" runat="server" TargetControlID="DDColor"
                                                ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                                            </cc1:ListSearchExtender>
                                        </td>
                                    </tr>
                                    <tr id="TRDDShape" runat="server" visible="false">
                                        <td>
                                            <asp:Label ID="lblshapename" runat="server" CssClass="labelbold" Text="Shape"></asp:Label>
                                        </td>
                                        <td colspan="3">
                                            <asp:DropDownList ID="DDShape" runat="server" AutoPostBack="True" CssClass="dropdown"
                                                Width="300px" OnSelectedIndexChanged="DDShape_SelectedIndexChanged">
                                            </asp:DropDownList>
                                            <cc1:ListSearchExtender ID="ListSearchExtender10" runat="server" TargetControlID="DDShape"
                                                ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                                            </cc1:ListSearchExtender>
                                        </td>
                                    </tr>
                                    <tr id="TRDDSize" runat="server" visible="false">
                                        <td>
                                            <asp:Label ID="lblsizename" runat="server" CssClass="labelbold" Text="Size"></asp:Label>
                                        </td>
                                        <td colspan="3">
                                            <asp:DropDownList ID="DDSize" runat="server" AutoPostBack="True" CssClass="dropdown"
                                                Width="250px">
                                            </asp:DropDownList>
                                            <cc1:ListSearchExtender ID="ListSearchExtender11" runat="server" TargetControlID="DDSize"
                                                ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                                            </cc1:ListSearchExtender>
                                            <asp:CheckBox ID="chkmtr" runat="server" Text="For Mtr." Font-Bold="true" OnCheckedChanged="chkmtr_CheckedChanged"
                                                AutoPostBack="true" />
                                        </td>
                                    </tr>
                                    <tr id="TRDDShadeColor" runat="server" visible="false">
                                        <td>
                                            <asp:Label ID="lblshadename" runat="server" CssClass="labelbold" Text="Shade Color"></asp:Label>
                                        </td>
                                        <td colspan="3">
                                            <asp:DropDownList ID="DDShadeColor" runat="server" AutoPostBack="True" CssClass="dropdown"
                                                Width="300px">
                                            </asp:DropDownList>
                                            <cc1:ListSearchExtender ID="ListSearchExtender12" runat="server" TargetControlID="DDShadeColor"
                                                ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                                            </cc1:ListSearchExtender>
                                        </td>
                                    </tr>
                                    <tr id="TRlotNo" runat="server" visible="false">
                                        <td>
                                            <asp:Label ID="LblLotNo" runat="server" CssClass="labelbold" Text="Lot No."></asp:Label>
                                        </td>
                                        <td colspan="3">
                                            <asp:DropDownList ID="DDLotNo" runat="server" AutoPostBack="True" CssClass="dropdown"
                                                Width="300px">
                                            </asp:DropDownList>
                                            <cc1:ListSearchExtender ID="ListSearchExtender13" runat="server" TargetControlID="DDLotNo"
                                                ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                                            </cc1:ListSearchExtender>
                                        </td>
                                    </tr>
                                    <%-- <tr id="TRGatePass" runat="server" visible="false">
                                        <td>
                                            <asp:Label ID="Label7" runat="server" CssClass="labelbold" Text="Gate In/Pass No."></asp:Label>
                                        </td>
                                        <td colspan="3">
                                            <asp:DropDownList ID="DDGatePassNo" runat="server" AutoPostBack="True"  CssClass="dropdown"
                                                Width="300px">
                                            </asp:DropDownList>
                                            <cc1:ListSearchExtender ID="ListSearchExtender14" runat="server" TargetControlID="DDGatePassNo"
                                ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                            </cc1:ListSearchExtender>
                                        </td>
                                    </tr>--%>
                                    <tr id="TR1" runat="server">
                                        <td>
                                            <asp:Label ID="Label7" runat="server" CssClass="labelbold" Text="Local order No."></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtlocalorderno" runat="server" CssClass="textb" Width="100px"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr id="TRGatePass" runat="server" visible="false">
                                        <td colspan="2">
                                            <table>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="LblGateINPassNo" runat="server" CssClass="labelbold" Text="Gate IN/Pass No."></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="TxtGateINPassNo" runat="server" CssClass="textb" Width="100px"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label Text="Gate IN/Pass Type" CssClass="labelbold" runat="server" />
                                                    </td>
                                                    <td>
                                                        <asp:DropDownList ID="DDgateinpasstype" CssClass="dropdown" runat="server">
                                                            <asp:ListItem Text="All" Value="-1" />
                                                            <asp:ListItem Text="IN" Value="1" />
                                                            <asp:ListItem Text="OUT" Value="0" />
                                                        </asp:DropDownList>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr id="TRIssueNo" runat="server" visible="false">
                                        <td>
                                            <asp:CheckBox ID="Chkissueno" Text="For Issue No." runat="server" AutoPostBack="true"
                                                CssClass="checkboxbold" OnCheckedChanged="Chkissueno_CheckedChanged" />
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtissueno" CssClass="textb" runat="server" Width="100px" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2" align="left">
                                            <asp:CheckBox ID="ChkForDate" runat="server" Text="Check For Date" CssClass="checkboxbold" />
                                        </td>
                                    </tr>
                                     <tr>
                                        <td colspan="2" align="left">
                                            <asp:CheckBox ID="chksumm" runat="server" Text="Summary" Visible="false" CssClass="checkboxbold" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2" align="left">
                                            <asp:CheckBox ID="chkall" runat="server" Text="ALL" Visible="false" CssClass="checkboxbold" />
                                        </td>
                                    </tr>
                                    <tr id="trDates" runat="server">
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
                                        <td colspan="4">
                                            <asp:Label ID="lblMessage" runat="server" CssClass="labelbold" ForeColor="Red"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="4">
                                            <asp:CheckBox ID="ChkForPendingStockNo" runat="server" Text="Process Issue Rec Summary With Stock No"
                                                Visible="false" CssClass="checkboxbold" AutoPostBack="True" OnCheckedChanged="ChkForProcessIssRecSummary_CheckedChanged" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="3">
                                            <asp:CheckBox ID="ChkForProcessIssRecSummary" runat="server" Text="Process Issue Rec Summary"
                                                CssClass="checkboxbold" AutoPostBack="True" OnCheckedChanged="ChkForProcessIssRecSummary_CheckedChanged" />
                                        </td>
                                    </tr>
                                    <tr id="TRChkBoxIssueDate" runat="server" visible="false">
                                        <td colspan="2" align="left">
                                            <asp:CheckBox ID="ChkForIssueDate" runat="server" Text="Check As Per IssueDate" CssClass="checkboxbold"
                                                AutoPostBack="True" OnCheckedChanged="ChkForIssueDate_CheckedChanged" />
                                        </td>
                                    </tr>
                                    <tr id="trIssueDate" runat="server" visible="false">
                                        <td>
                                            <asp:Label ID="Label8" runat="server" CssClass="labelbold" Text="From Date"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtIssueFromDate" runat="server" CssClass="textb" Width="100px"></asp:TextBox>
                                            <asp:CalendarExtender ID="CalendarExtender3" runat="server" Format="dd-MMM-yyyy"
                                                TargetControlID="txtIssueFromDate">
                                            </asp:CalendarExtender>
                                        </td>
                                        <td align="right">
                                            <asp:Label ID="Label9" runat="server" CssClass="labelbold" Text="To Date" Width="80px"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtIssueToDate" runat="server" CssClass="textb" Width="100px"></asp:TextBox>
                                            <asp:CalendarExtender ID="CalendarExtender4" runat="server" Format="dd-MMM-yyyy"
                                                TargetControlID="txtIssueToDate">
                                            </asp:CalendarExtender>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="3" id="TDexcelExport" runat="server" visible="false">
                                            <asp:CheckBox ID="chkexcelexport" runat="server" CssClass="checkboxbold" Text="Excel Export" />
                                        </td>
                                        <tr>
                                            <td colspan="3" id="TDsizesummary" runat="server" visible="false">
                                                <asp:CheckBox ID="chksizesummary" runat="server" CssClass="checkboxbold" Text="Size Wise Summary" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="3" id="TDJobWiseSummary" runat="server" visible="false">
                                                <asp:CheckBox ID="chkjobwisesummary" runat="server" CssClass="checkboxbold" Text="Job Wise Summary" />
                                            </td>
                                        </tr>
                                        <tr id="TRCheckWithTime" runat="server" visible="false">
                                            <td colspan="3">
                                                <asp:CheckBox ID="ChkWithRecTime" runat="server" CssClass="checkboxbold" Text="WITH TIME" />
                                            </td>
                                        </tr>
                                         <tr id="TRQualityWiseSummary" runat="server" visible="false">
                                            <td colspan="3">
                                                <asp:CheckBox ID="ChkQualityWiseSummary" runat="server" CssClass="checkboxbold" Text="QualityWise Summary" />
                                            </td>
                                        </tr>
                                        <tr id="TRBuyerItemSizeWiseSummary" runat="server" visible="false">
                                            <td colspan="3">
                                                <asp:CheckBox ID="ChkBuyerItemSizeWiseSummary" runat="server" CssClass="checkboxbold" Text="Buyer Item Size Wise Summary" />
                                            </td>
                                        </tr>

                                         <tr id="TRAsOnDate" runat="server" visible="false">
                                        <td>
                                            <asp:Label ID="Label12" runat="server" CssClass="labelbold" Text="As ON Date"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtAsOnDate" runat="server" CssClass="textb" Width="100px"></asp:TextBox>
                                            <asp:CalendarExtender ID="CalendarExtender5" runat="server" Format="dd-MMM-yyyy"
                                                TargetControlID="txtAsOnDate">
                                            </asp:CalendarExtender>
                                        </td>
                                        <td align="right">
                                            &nbsp;
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                    </tr>

                                        <tr>
                                            <td align="right" colspan="4">
                                                &nbsp;<asp:CheckBox ID="ChkSummary" runat="server" CssClass="checkboxnormal" Text="For Summary" />
                                                &nbsp;<asp:Button ID="BtnPreview" runat="server" CssClass="buttonnorm" OnClick="BtnPreview_Click"
                                                    OnClientClick="return Validate();" Text="Preview" />
                                                &nbsp;<asp:Button ID="BtnPreview1" runat="server" CssClass="buttonnorm" OnClick="BtnPreview_Click"
                                                    OnClientClick="return Validate();" Text="Issue No. Wise" Visible="false" />
                                                &nbsp;<asp:Button ID="BtnClose" runat="server" CssClass="buttonnorm" OnClick="BtnClose_Click"
                                                    Text="Close" />
                                            </td>
                                        </tr>
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
        <style type="text/css">
            #mask
            {
                position: fixed;
                left: 0px;
                top: 0px;
                z-index: 4;
                opacity: 0.4;
                -ms-filter: "progid:DXImageTransform.Microsoft.Alpha(Opacity=40)"; /* first!*/
                filter: alpha(opacity=40); /* second!*/
                background-color: Gray;
                display: none;
                width: 100%;
                height: 100%;
            }
        </style>
        <script type="text/javascript" language="javascript">
            function ShowPopup() {
                $('#mask').show();
                $('#<%=pnlpopup.ClientID %>').show();
            }
            function HidePopup() {
                $('#mask').hide();
                $('#<%=pnlpopup.ClientID %>').hide();
            }
            $(".btnPwd").live('click', function () {
                HidePopup();
            });
        </script>
        <div id="mask">
        </div>
        <asp:Panel ID="pnlpopup" runat="server" BackColor="White" Height="175px" Width="300px"
            Style="z-index: 111; background-color: White; position: absolute; left: 35%;
            top: 40%; border: outset 2px gray; padding: 5px; display: none">
            <table width="100%" style="width: 100%; height: 100%;" cellpadding="0" cellspacing="5">
                <tr style="background-color: #8B7B8B; height: 1px">
                    <td colspan="2" style="color: White; font-weight: bold; font-size: 1.2em; padding: 3px"
                        align="center">
                        ENTER PASSWORD <a id="closebtn" style="color: white; float: right; text-decoration: none"
                            class="btnPwd" href="#">X</a>
                    </td>
                </tr>
                <tr>
                    <td align="right">
                        Enter Password:
                    </td>
                    <td>
                        <asp:TextBox ID="txtpwd" runat="server" TextMode="Password" Height="30px" Width="174px" />
                    </td>
                </tr>
                <tr>
                    <td>
                    </td>
                    <td align="right">
                        <asp:Button ID="btnCheck" CommandName="Check" runat="server" Text="Check" CssClass="btnPwd"
                            ValidationGroup="m" OnClick="btnCheck_Click" />
                        <input type="button" value="Cancel" class="btnPwd" />
                    </td>
                </tr>
            </table>
        </asp:Panel>
    </div>
</asp:Content>
