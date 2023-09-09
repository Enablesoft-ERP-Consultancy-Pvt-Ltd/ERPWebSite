<%@ Page Language="C#" AutoEventWireup="true" CodeFile="FrmFinisherStatusReport.aspx.cs"
    Inherits="Masters_ReportForms_FrmFinisherStatusReport" MasterPageFile="~/ERPmaster.master"
    Title="Finisher Report" EnableEventValidation="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="CPH_Form" runat="server">
    <script src="../../Scripts/JScript.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery-1.4.1.js" type="text/javascript"></script>
    <%--<script type="text/javascript" src="../../Scripts/Fixfocus.js"></script>--%>
    <script type="text/javascript">
        function CloseForm() {
            window.location.href = "../../main.aspx";
        }
    </script>
    <%--<script runat="server">    
        protected void RadioButton_CheckedChanged(object sender, System.EventArgs e)
        {
            //DDItemName.Visible = true;
            //TRToDate.Visible = false;
            //ChkForFt.Visible = true;
            TRReportType.Visible = false;
            TRItemName.Visible = false;
            TRChkForSummary.Visible = false;
            TRQualityType.Visible = false;
            TRQualityTypeDropDown.Visible = false;
            TRGodown.Visible = false;
            if (RDWeaverRawMaterial.Checked == true)
            {
                TRItemName.Visible = true;
                TRChkForSummary.Visible = true;
                TRReportType.Visible = false;
                TRQualityType.Visible = true;
                TRQualityTypeDropDown.Visible = false;
                TRGodown.Visible = false;
            }
            else if (RDWeaverRawLedger.Checked == true)
            {
                TRItemName.Visible = false;
                TRChkForSummary.Visible = false;
                TRReportType.Visible = false;
                TRQualityType.Visible = true;
                TRQualityTypeDropDown.Visible = false;
                TRGodown.Visible = false;
            }
            else if (RDWeaverRawMaterialIssueReceive.Checked == true)
            {
                TRItemName.Visible = true;
                TRChkForSummary.Visible = false;
                TRReportType.Visible = true;
                TRQualityType.Visible = false;
                TRQualityTypeDropDown.Visible = true;
                TRGodown.Visible = true;
            }
            else if (RDFinisherRawMaterialIssueReceive.Checked == true)
            {
                TRItemName.Visible = true;
                TRChkForSummary.Visible = false;
                TRReportType.Visible = true;
                TRQualityType.Visible = false;
                TRQualityTypeDropDown.Visible = true;
                TRGodown.Visible = true;
            }        
        }
    </script>--%>
    <asp:UpdatePanel ID="updatepenl1" runat="server">
        <ContentTemplate>
            <table width="85%">
                <tr>
                    <td style="width: 300px" valign="top" align="left">
                        <div style="width: 287px; padding-top: 5px; height: 300px; float: left; border-style: solid;
                            border-width: thin">
                            &nbsp;&nbsp;
                            <br />
                            <%-- &nbsp;&nbsp;
                              <asp:RadioButton ID="RDProcessIssRecDetailALL" Text="ALL" runat="server"
                                    GroupName="OrderType" CssClass="labelbold" AutoPostBack="True" OnCheckedChanged="RDProcessIssRecDetailALL_CheckedChanged" />
                                <br />--%>
                            &nbsp;&nbsp;
                            <asp:RadioButton ID="RDFinishingIssueDetail" runat="server" Text="Finishing Issue Detail"
                                GroupName="OrderType" CssClass="labelbold" AutoPostBack="True" OnCheckedChanged="RDFinishingIssueDetail_CheckedChanged" />
                            <br />
                            &nbsp;&nbsp;
                            <asp:RadioButton ID="RDFinishingReceiveDetail" runat="server" Text="Finishing Receive Detail"
                                GroupName="OrderType" CssClass="labelbold" AutoPostBack="True" OnCheckedChanged="RDFinishingReceiveDetail_CheckedChanged" />
                            <br />
                            &nbsp;&nbsp;
                            <asp:RadioButton ID="RDFinisherPendingPcs" runat="server" Text="Finisher Pending Pcs"
                                GroupName="OrderType" CssClass="labelbold" AutoPostBack="True" OnCheckedChanged="RDFinisherPendingPcs_CheckedChanged" />
                            <br />
                            &nbsp;&nbsp;
                        </div>
                    </td>
                    <td>
                        <div>
                            <div style="float: left; width: 450px;">
                                <%--<div style="float: left; width: 450px; height: 500px;">--%>
                                <%--<asp:Panel runat="server" ID="panel1" Style="border: 1px groove Teal; border: 1px solid black;"
                        Width="100%">--%>
                                <div style="padding: 0px 0px 0px 20px">
                                    <table>
                                        <tr>
                                            <td>
                                                <asp:Label ID="Label1" runat="server" CssClass="labelbold" Text="CompanyName"></asp:Label>
                                            </td>
                                            <td colspan="3">
                                                <asp:DropDownList ID="DDCompany" runat="server" AutoPostBack="True" CssClass="dropdown"
                                                    Width="300px" OnSelectedIndexChanged="DDCompany_SelectedIndexChanged">
                                                </asp:DropDownList>
                                                <cc1:ListSearchExtender ID="ListSearchExtender3" runat="server" TargetControlID="DDCompany"
                                                    ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                                                </cc1:ListSearchExtender>
                                            </td>
                                        </tr>
                                        <tr id="TRFinisherStatus" runat="server" visible="true">
                                            <td>
                                                <asp:Label ID="Label7" runat="server" CssClass="labelbold" Text="Finisher Status"></asp:Label>
                                            </td>
                                            <td colspan="3">
                                                <asp:DropDownList ID="DDFinisherStatus" runat="server" CssClass="dropdown" Width="300px">
                                                    <asp:ListItem Value="0" Text="All">ALL</asp:ListItem>
                                                    <asp:ListItem Value="1" Text="All">Pending</asp:ListItem>
                                                    <asp:ListItem Value="2" Text="All">Complete</asp:ListItem>
                                                </asp:DropDownList>
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
                                        <tr>
                                            <td>
                                                <asp:Label ID="Label3" runat="server" CssClass="labelbold" Text="Emp Name"></asp:Label>
                                            </td>
                                            <td colspan="3">
                                                <asp:DropDownList ID="DDEmpName" runat="server" AutoPostBack="True" CssClass="dropdown"
                                                    OnSelectedIndexChanged="DDEmpName_SelectedIndexChanged" Width="300px">
                                                </asp:DropDownList>
                                                <cc1:ListSearchExtender ID="ListSearchExtender2" runat="server" TargetControlID="DDEmpName"
                                                    ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                                                </cc1:ListSearchExtender>
                                            </td>
                                        </tr>
                                        <tr id="TRRecChallan" runat="server">
                                            <td>
                                                <asp:Label ID="Label4" runat="server" CssClass="labelbold" Text="Rec Challan No"></asp:Label>
                                            </td>
                                            <td colspan="3">
                                                <asp:DropDownList ID="DDChallanNo" runat="server" AutoPostBack="True" CssClass="dropdown"
                                                    Width="300px">
                                                </asp:DropDownList>
                                                <cc1:ListSearchExtender ID="ListSearchExtender4" runat="server" TargetControlID="DDChallanNo"
                                                    ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                                                </cc1:ListSearchExtender>
                                            </td>
                                        </tr>
                                        <tr id="TRCategory" runat="server" visible="true">
                                            <td>
                                                <asp:Label ID="lblcategoryname" runat="server" CssClass="labelbold" Text="Category Name"></asp:Label>
                                            </td>
                                            <td colspan="3">
                                                <asp:DropDownList ID="DDCategory" runat="server" AutoPostBack="True" CssClass="dropdown"
                                                    Width="300px">
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
                                                    Width="300px">
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
                                        <%--  <tr id="TRlotNo" runat="server" visible="false">
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
                                    </tr>--%>
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
                                        <%-- <tr id="TR1" runat="server">
                                        <td>
                                            <asp:Label ID="Label7" runat="server" CssClass="labelbold" Text="Local order No."></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtlocalorderno" runat="server" CssClass="textb" Width="100px"></asp:TextBox>                                           
                                                
                                            
                                        </td>
                                    </tr>--%>
                                        <%-- <tr id="TRGatePass" runat="server" visible="false">
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
                                                        <asp:Label ID="Label1" Text="Gate IN/Pass Type" CssClass="labelbold" runat="server" />
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
                                    </tr>--%>
                                        <%-- <tr id="TRIssueNo" runat="server" visible="false">
                                        <td>
                                            <asp:CheckBox ID="Chkissueno" Text="For Issue No." runat="server" AutoPostBack="true"
                                                CssClass="checkboxbold" OnCheckedChanged="Chkissueno_CheckedChanged" />
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtissueno" CssClass="textb" runat="server" Width="100px" />
                                        </td>
                                    </tr>--%>
                                        <tr>
                                            <td colspan="2" align="left">
                                                <asp:CheckBox ID="ChkForDate" runat="server" Text="Check For Date" CssClass="checkboxbold"
                                                    AutoPostBack="True" OnCheckedChanged="ChkForDate_CheckedChanged" />
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
                                            <td colspan="4">
                                                <asp:Label ID="lblMessage" runat="server" CssClass="labelbold" ForeColor="Red"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="4">
                                                <asp:CheckBox ID="ChkForPendingStockNo" runat="server" Text="Detail For Pending StockNo"
                                                    Visible="false" CssClass="checkboxbold" AutoPostBack="True" OnCheckedChanged="ChkForPendingStockNo_CheckedChanged" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="3">
                                                <asp:CheckBox ID="ChkForFinisherNillDetail" runat="server" Text="Finisher Nill Detail"
                                                    Visible="false" CssClass="checkboxbold" AutoPostBack="True" OnCheckedChanged="ChkForFinisherNillDetail_CheckedChanged" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="right" colspan="4">
                                                &nbsp;<asp:Button ID="BtnPreview" runat="server" CssClass="buttonnorm" OnClick="BtnPreview_Click"
                                                    OnClientClick="return Validate();" Text="Preview" />
                                                &nbsp;<asp:Button ID="BtnClose" runat="server" CssClass="buttonnorm" OnClick="BtnClose_Click"
                                                    Text="Close" />
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </div>
                        </div>
                    </td>
                </tr>
            </table>
            </div>
            <%--</asp:Panel>--%>
            </div> </div> </td> </tr> </table>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="BtnPreview" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
