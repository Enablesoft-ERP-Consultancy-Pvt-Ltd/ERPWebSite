<%@ Page Title="WARPING REPORTS" Language="C#" MasterPageFile="~/ERPmaster.master"
    AutoEventWireup="true" CodeFile="FrmWarpingReports.aspx.cs" Inherits="Masters_ReportForms_frmwarpingreports" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content2" ContentPlaceHolderID="CPH_Form" runat="Server">
    <script src="../../Scripts/JScript.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery-1.4.1.js" type="text/javascript"></script>
    <script type="text/javascript">
        function CloseForm() {
            window.location.href = "../../main.aspx";
        }
    </script>
    <script runat="server">
        protected void RadioButton_CheckedChanged(object sender, System.EventArgs e)
        {
            TRLotNoTagNo.Visible = false;
            trforfromandtodate.Visible = false;
            TRcustcode.Visible = false;
            TRorderNo.Visible = false;
            Trprocess.Visible = false;
            TRProductionUnit.Visible = false;
            TRLoomNo.Visible = false;
            TRFolioNo.Visible = false;
            TRWarpingIssueNo.Visible = false;    
            if (RDWarpingIssueDetail.Checked == true)
            {
                TRLotNoTagNo.Visible = true;
                trforfromandtodate.Visible = true;
                Trprocess.Visible = true;
            }
            if (RDWarpingsumm.Checked == true)
            {
                TRcustcode.Visible = true;
                TRorderNo.Visible = true;
                trforfromandtodate.Visible = true;
            }
            if (RDWarpingBeamReceiveDetail.Checked == true)
            {
                trforfromandtodate.Visible = true;
                Trprocess.Visible = true;
            }
            if (RDLoomBeamIssueDetail.Checked == true)
            {
                trforfromandtodate.Visible = true;
            }
            if (RDLoomBeamReceiveDetail.Checked == true)
            {
                trforfromandtodate.Visible = true;
                TRProductionUnit.Visible = true;
                TRLoomNo.Visible = true;
                TRFolioNo.Visible = true;
            }
            if (RDLoomBeamPendingDetail.Checked == true)
            {
                TRProductionUnit.Visible = true;
                TRLoomNo.Visible = true;
                TRFolioNo.Visible = true;
                trforfromandtodate.Visible = true;
            }
            if (RDWarpingBeamReceiveDetailWithRate.Checked == true)
            {
                trforfromandtodate.Visible = true;
                Trprocess.Visible = true;
            }
            if (RDWarpingMaterialIssueDetail.Checked == true)
            {
                TRLotNoTagNo.Visible = true;
                trforfromandtodate.Visible = true;
                Trprocess.Visible = true;
            }
            if (RDWarpingOrderWithBeamFolio.Checked == true)
            {
                TRWarpingIssueNo.Visible = true;               
            }
        }
    </script>
    <asp:UpdatePanel ID="upd1" runat="server">
        <ContentTemplate>
            <div style="margin-left: 20%">
                <table border="0" cellpadding="0" cellspacing="0">
                    <tr>
                        <td valign="top">
                            <table border="1">
                                <tr>
                                    <td>
                                        <asp:RadioButton ID="RDWarpingIssueDetail" Text="Warping Issue Detail" runat="server"
                                            CssClass="radiobuttonnormal" AutoPostBack="True" OnCheckedChanged="RadioButton_CheckedChanged"
                                            GroupName="a" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:RadioButton ID="RDBeamstock" Text="Beam Stock as on Date" runat="server" CssClass="radiobuttonnormal"
                                            AutoPostBack="True" OnCheckedChanged="RadioButton_CheckedChanged" GroupName="a" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:RadioButton ID="RDWarpingsumm" Text="Warping Summary" runat="server" CssClass="radiobuttonnormal"
                                            AutoPostBack="True" OnCheckedChanged="RadioButton_CheckedChanged" GroupName="a" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:RadioButton ID="RDWarpingBeamReceiveDetail" Text="Beam Receive Detail" runat="server"
                                            CssClass="radiobuttonnormal" AutoPostBack="True" OnCheckedChanged="RadioButton_CheckedChanged"
                                            GroupName="a" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:RadioButton ID="RDLoomBeamIssueDetail" Text="Loom Beam Issue Detail" runat="server"
                                            CssClass="radiobuttonnormal" AutoPostBack="True" OnCheckedChanged="RadioButton_CheckedChanged"
                                            GroupName="a" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:RadioButton ID="RDLoomBeamReceiveDetail" Text="Loom Beam Receive Detail" runat="server"
                                            CssClass="radiobuttonnormal" AutoPostBack="True" OnCheckedChanged="RadioButton_CheckedChanged"
                                            GroupName="a" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:RadioButton ID="RDLoomBeamPendingDetail" Text="Loom Beam Pending Detail" runat="server"
                                            CssClass="radiobuttonnormal" AutoPostBack="True" OnCheckedChanged="RadioButton_CheckedChanged"
                                            GroupName="a" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:RadioButton ID="RDWarpingBeamReceiveDetailWithRate" Text="Beam Receive Detail With Rate" runat="server"
                                            CssClass="radiobuttonnormal" AutoPostBack="True" OnCheckedChanged="RadioButton_CheckedChanged"
                                            GroupName="a" />
                                    </td>
                                </tr>
                                <tr id="TRWarpingMaterialIssueDetail" runat="server" visible="false">
                                    <td>
                                        <asp:RadioButton ID="RDWarpingMaterialIssueDetail" Text="Warping Material Issue Detail" runat="server"
                                            CssClass="radiobuttonnormal" AutoPostBack="True" OnCheckedChanged="RadioButton_CheckedChanged"
                                            GroupName="a" />
                                    </td>
                                </tr>

                                <tr id="TRWarpingOrderWithBeamFolio" runat="server" visible="false">
                                    <td>
                                        <asp:RadioButton ID="RDWarpingOrderWithBeamFolio" Text="Warping Order Detail With Beam Folio" runat="server"
                                            CssClass="radiobuttonnormal" AutoPostBack="True" OnCheckedChanged="RadioButton_CheckedChanged"
                                            GroupName="a" />
                                    </td>
                                </tr>

                            </table>
                        </td>
                        <td>
                            &nbsp
                        </td>
                        <td>
                            <table border="1">
                                <tr>
                                    <td>
                                        <asp:Label ID="Label1" runat="server" CssClass="labelbold" Text="CompanyName"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="DDCompany" runat="server" CssClass="dropdown" Width="250px">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr id="Trprocess" runat="server" visible="false">
                                    <td>
                                        <asp:Label ID="Label4" runat="server" CssClass="labelbold" Text="Process"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="DDProcess" runat="server" CssClass="dropdown" Width="250px">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr id="TRcustcode" runat="server" visible="false">
                                    <td>
                                        <asp:Label ID="Label2" Text="Customer Code" runat="server" CssClass="labelbold" />
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="DDcustcode" runat="server" CssClass="dropdown" Width="250px"
                                            OnSelectedIndexChanged="DDcustcode_SelectedIndexChanged" AutoPostBack="true">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr id="TRorderNo" runat="server" visible="false">
                                    <td>
                                        <asp:Label ID="Label3" Text="Order No." runat="server" CssClass="labelbold" />
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="DDorderno" runat="server" CssClass="dropdown" Width="250px">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr id="TRProductionUnit" runat="server" visible="false">
                                    <td>
                                        <asp:Label ID="Label6" Text="Production Unit" runat="server" CssClass="labelbold" />
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="DDProductionUnit" runat="server" CssClass="dropdown" Width="250px"
                                            AutoPostBack="true" OnSelectedIndexChanged="DDProductionUnit_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr id="TRLoomNo" runat="server" visible="false">
                                    <td>
                                        <asp:Label ID="Label7" Text="Loom No" runat="server" CssClass="labelbold" />
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="DDLoomNo" runat="server" CssClass="dropdown" Width="250px"
                                            AutoPostBack="true" OnSelectedIndexChanged="DDLoomNo_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr id="TRFolioNo" runat="server" visible="false">
                                    <td>
                                        <asp:Label ID="Label5" Text="Folio No." runat="server" CssClass="labelbold" />
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="DDFolioNo" runat="server" CssClass="dropdown" Width="250px">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                   <tr id="TRWarpingIssueNo" runat="server" visible="false">
                                    <td>
                                        <asp:Label ID="Label8" Text="Warping IssueNo." runat="server" CssClass="labelbold" />
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="DDWarpingIssueNo" runat="server" CssClass="dropdown" Width="250px">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblcategoryname" runat="server" CssClass="labelbold" Text="Category Name"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="DDCategory" runat="server" AutoPostBack="True" CssClass="dropdown"
                                            Width="250px" OnSelectedIndexChanged="DDCategory_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblitemname" runat="server" CssClass="labelbold" Text="Item Name"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddItemName" runat="server" AutoPostBack="True" CssClass="dropdown"
                                            Width="250px" OnSelectedIndexChanged="ddItemName_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr id="TRDDQuality" runat="server" visible="false">
                                    <td>
                                        <asp:Label ID="lblqualityname" runat="server" CssClass="labelbold" Text="Quality"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="DDQuality" runat="server" AutoPostBack="True" CssClass="dropdown"
                                            Width="250px" OnSelectedIndexChanged="DDQuality_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr id="TRDDDesign" runat="server" visible="false">
                                    <td>
                                        <asp:Label ID="lbldesignname" runat="server" CssClass="labelbold" Text="Design"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="DDDesign" runat="server" CssClass="dropdown" Width="250px">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr id="TRDDColor" runat="server" visible="false">
                                    <td>
                                        <asp:Label ID="lblcolorname" runat="server" CssClass="labelbold" Text="Color"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="DDColor" runat="server" CssClass="dropdown" Width="250px">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr id="TRDDShadeColor" runat="server" visible="false">
                                    <td>
                                        <asp:Label ID="lblshadename" runat="server" CssClass="labelbold" Text="Shade Color"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="DDShadeColor" runat="server" CssClass="dropdown" Width="250px">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr id="TRDDShape" runat="server" visible="false">
                                    <td>
                                        <asp:Label ID="lblshapename" runat="server" CssClass="labelbold" Text="Shape"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="DDShape" runat="server" AutoPostBack="True" CssClass="dropdown"
                                            Width="250px" OnSelectedIndexChanged="DDShape_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr id="TRDDSize" runat="server" visible="false">
                                    <td>
                                        <asp:Label ID="lblsizename" runat="server" CssClass="labelbold" Text="Size"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="DDSize" runat="server" CssClass="dropdown" Width="250px">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr id="TRLotNoTagNo" runat="server" visible="false">
                                    <td>
                                    </td>
                                    <td>
                                        <table>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lbllotno1" Text="Lot No" CssClass="labelbold" runat="server" /><br />
                                                    <asp:TextBox ID="txtLotno" Width="120px" runat="server" />
                                                    <asp:AutoCompleteExtender ID="txtLotno_AutoCompleteExtender" runat="server" BehaviorID="SrchAutoComplete"
                                                        CompletionInterval="20" Enabled="True" ServiceMethod="GetLotNo" EnableCaching="true"
                                                        CompletionSetCount="20" ServicePath="~/Autocomplete.asmx" TargetControlID="txtLotno"
                                                        UseContextKey="True" ContextKey="0#0#0" MinimumPrefixLength="2">
                                                    </asp:AutoCompleteExtender>
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblTagNo" Text="Tag No" CssClass="labelbold" runat="server" /><br />
                                                    <asp:TextBox ID="txtTagno" Width="120px" runat="server" />
                                                    <asp:AutoCompleteExtender ID="txtTagno_AutoCompleteExtender" runat="server" BehaviorID="SrchAutoComplete1"
                                                        CompletionInterval="20" Enabled="True" ServiceMethod="GetTagNo" EnableCaching="true"
                                                        CompletionSetCount="20" ServicePath="~/Autocomplete.asmx" TargetControlID="txtTagno"
                                                        UseContextKey="True" ContextKey="0#0#0" MinimumPrefixLength="2">
                                                    </asp:AutoCompleteExtender>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr id="trforfromandtodate" runat="server" visible="false">
                                    <td>
                                        <asp:Label ID="lblfromdate" Text="From Date" runat="server" CssClass="labelbold" />
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtfromdate" runat="server" Width="90px" CssClass="textb"></asp:TextBox>
                                        <asp:CalendarExtender ID="CalendarExtender2" runat="server" Format="dd-MMM-yyyy"
                                            TargetControlID="txtfromdate">
                                        </asp:CalendarExtender>
                                        <asp:Label ID="lblTodate" Text="To Date" runat="server" CssClass="labelbold" />
                                        <asp:TextBox ID="txttodate" runat="server" Width="90px" CssClass="textb"></asp:TextBox>
                                        <asp:CalendarExtender ID="CalendarExtender3" runat="server" Format="dd-MMM-yyyy"
                                            TargetControlID="txttodate">
                                        </asp:CalendarExtender>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2" align="right">
                                        <asp:Button ID="btnpreview" CssClass="buttonnorm" Text="Preview" runat="server" OnClick="btnpreview_Click" />
                                        <asp:Button ID="btnclose" CssClass="buttonnorm" Text="Close" runat="server" OnClientClick="return CloseForm();" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </div>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="btnpreview" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
