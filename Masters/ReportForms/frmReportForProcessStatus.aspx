<%@ Page Language="C#" AutoEventWireup="true" CodeFile="frmReportForProcessStatus.aspx.cs"
    Inherits="Masters_ReportForms_frmReportForProcessStatus" MasterPageFile="~/ERPmaster.master" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="page" ContentPlaceHolderID="CPH_Form" runat="server">
    <%--<script type="text/javascript" src="../../Scripts/Fixfocus.js"></script>--%>
    <script type="text/javascript">
        function CloseForm() {
            window.location.href = "../../main.aspx";
        }
        function ShowDates() {

            var Trdates = document.getElementById('<%=trDates.ClientID %>');
            if (document.getElementById('<%=ChkForDate.ClientID %>').checked = true) {
                Trdates.style.display = 'block';
            }
            else {
                Trdates.style.display = 'none';
            }

        }
    </script>
    <script runat="server">    
        protected void RadioButton_CheckedChanged(object sender, System.EventArgs e)
        {
            TRSummary.Visible = false;
            TrRawTransaction.Visible = false;

            TRSupervisorName.Visible = false;
            TRLoomNo.Visible = false;
            if (RDReceive.Checked == true)
            {
                TRSummary.Visible = true;
            }
            else if (RDRawMaterial.Checked == true)
            {
                TrStatus.Visible = true;
                TRProcessName.Visible = true;
                TREmpName.Visible = true;
                TRCategoryName.Visible = true;
                TRddItemName.Visible = true;
                TRLocalOrderNo.Visible = true;

                TrRawTransaction.Visible = true;
            }
            else if (RDRawMaterialSupervisorWise.Checked == true)
            {
                TrStatus.Visible = false;
                TRProcessName.Visible = false;
                TREmpName.Visible = false;
                TRCategoryName.Visible = false;
                TRddItemName.Visible = false;
                TRLocalOrderNo.Visible = false;

                TRSupervisorName.Visible = true;
                TRLoomNo.Visible = true;
                TrRawTransaction.Visible = true;

            }
        }
    </script>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <table width="85%">
                <tr>
                    <td style="width: 300px" valign="top">
                        <div style="width: 287px; padding-top: 5px; height: 300px; float: left; border-style: solid;
                            border-width: thin">
                            <table>
                                <tr>
                                    <td>
                                        <asp:RadioButton ID="RDAll" runat="server" Text="All" GroupName="OrderType" CssClass="labelbold"
                                            AutoPostBack="true" OnCheckedChanged="RadioButton_CheckedChanged" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:RadioButton ID="RDOrder" runat="server" Text="Order" GroupName="OrderType" CssClass="labelbold"
                                            AutoPostBack="true" OnCheckedChanged="RadioButton_CheckedChanged" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:RadioButton ID="RDReceive" runat="server" Text="Receive" GroupName="OrderType"
                                            CssClass="labelbold" AutoPostBack="true" OnCheckedChanged="RadioButton_CheckedChanged" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:RadioButton ID="RDRawMaterial" runat="server" Text="Raw Material" GroupName="OrderType"
                                            CssClass="labelbold" AutoPostBack="true" OnCheckedChanged="RadioButton_CheckedChanged" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:RadioButton ID="RDRawMaterialSupervisorWise" runat="server" Text="Raw Material Supervisor Wise"
                                            GroupName="OrderType" CssClass="labelbold" AutoPostBack="true" OnCheckedChanged="RadioButton_CheckedChanged" />
                                    </td>
                                </tr>
                            </table>
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
                                        <asp:DropDownList ID="DDCompany" runat="server" AutoPostBack="True" CssClass="dropdown"
                                            Width="300px">
                                        </asp:DropDownList>
                                        <cc1:ListSearchExtender ID="ListSearchExtender3" runat="server" TargetControlID="DDCompany"
                                            ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                                        </cc1:ListSearchExtender>
                                    </td>
                                </tr>
                                <tr id="TrStatus" runat="server">
                                    <td>
                                        <asp:Label ID="lblstatus" runat="server" CssClass="labelbold" Text="Status"></asp:Label>
                                    </td>
                                    <td colspan="3">
                                        <asp:DropDownList ID="DDStatus" runat="server" CssClass="dropdown" Width="300px">
                                            <asp:ListItem Value="0">All</asp:ListItem>
                                            <asp:ListItem Value="1">Pending</asp:ListItem>
                                            <asp:ListItem Value="2">Partially Processed</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr id="TRSupervisorName" runat="server" visible="false">
                                    <td>
                                        <asp:Label ID="Label4" runat="server" CssClass="labelbold" Text="Supervisor Name"></asp:Label>
                                    </td>
                                    <td colspan="3">
                                        <asp:DropDownList ID="DDSupervisorName" runat="server" AutoPostBack="True" CssClass="dropdown"
                                            Width="300px" OnSelectedIndexChanged="DDSupervisorName_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr id="TRLoomNo" runat="server" visible="false">
                                    <td>
                                        <asp:Label ID="Label7" runat="server" CssClass="labelbold" Text="Loom No"></asp:Label>
                                    </td>
                                    <td colspan="3">
                                        <asp:DropDownList ID="DDLoomNo" runat="server" CssClass="dropdown" Width="300px">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr id="TRProcessName" runat="server">
                                    <td>
                                        <asp:Label ID="Label2" runat="server" CssClass="labelbold" Text="Process Name"></asp:Label>
                                    </td>
                                    <td colspan="3">
                                        <asp:DropDownList ID="DDProcessName" runat="server" AutoPostBack="True" CssClass="dropdown"
                                            Width="300px" OnSelectedIndexChanged="DDProcessName_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr id="TREmpName" runat="server">
                                    <td>
                                        <asp:Label ID="Label3" runat="server" CssClass="labelbold" Text="Emp Name"></asp:Label>
                                    </td>
                                    <td colspan="3">
                                        <asp:DropDownList ID="DDEmpName" runat="server" AutoPostBack="True" CssClass="dropdown"
                                            Width="300px">
                                        </asp:DropDownList>
                                        <cc1:ListSearchExtender ID="ListSearchExtender2" runat="server" TargetControlID="DDEmpName"
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
                                            Width="300px" OnSelectedIndexChanged="DDDesign_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr id="TRDDColor" runat="server" visible="false">
                                    <td>
                                        <asp:Label ID="lblcolorname" runat="server" CssClass="labelbold" Text="Color"></asp:Label>
                                    </td>
                                    <td colspan="3">
                                        <asp:DropDownList ID="DDColor" runat="server" CssClass="dropdown" Width="300px">
                                        </asp:DropDownList>
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
                                    </td>
                                </tr>
                                <tr id="TRDDSize" runat="server" visible="false">
                                    <td>
                                        <asp:Label ID="lblsizename" runat="server" CssClass="labelbold" Text="Size"></asp:Label>
                                    </td>
                                    <td colspan="2">
                                        <asp:DropDownList ID="DDSize" runat="server" AutoPostBack="True" CssClass="dropdown"
                                            Width="200px">
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                        <asp:DropDownList CssClass="dropdown" Width="50" ID="DDsizetype" runat="server" AutoPostBack="True"
                                            OnSelectedIndexChanged="DDsizetype_SelectedIndexChanged" />
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
                                    </td>
                                </tr>
                                <tr id="TRLocalOrderNo" runat="server">
                                    <td>
                                        <asp:Label ID="lbllocalorderno" runat="server" CssClass="labelbold" Text="Local Order No."></asp:Label>
                                    </td>
                                    <td colspan="3">
                                        <asp:TextBox ID="txtlocalorderNo" CssClass="textb" runat="server" Width="150px" />
                                    </td>
                                </tr>
                                <tr id="TrRawTransaction" runat="server">
                                    <td>
                                        <asp:Label ID="lblRawMat" runat="server" CssClass="labelbold" Text="Raw Material Transaction"></asp:Label>
                                    </td>
                                    <td colspan="3">
                                        <asp:DropDownList ID="DDRawTransaction" runat="server" CssClass="dropdown" Width="300px">
                                            <asp:ListItem Value="0">All</asp:ListItem>
                                            <asp:ListItem Value="1">Issue</asp:ListItem>
                                            <asp:ListItem Value="2">Receive</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2" align="left">
                                        <asp:CheckBox ID="ChkForDate" runat="server" Text="Check For Date" AutoPostBack="true"
                                            CssClass="checkboxbold" OnCheckedChanged="ChkForDate_CheckedChanged" />
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
                                <tr id="TRSummary" runat="server" visible="false">
                                    <td>
                                        <asp:CheckBox ID="chksummary" Text="For Summary" runat="server" CssClass="checkboxbold"
                                            Font-Size="Small" />
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="4">
                                        <asp:Label ID="lblMessage" runat="server" CssClass="labelbold" ForeColor="Red"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right" colspan="4">
                                        &nbsp;<asp:Button ID="BtnPreview" runat="server" CssClass="buttonnorm" OnClientClick="return Validate();"
                                            Text="Preview" OnClick="BtnPreview_Click" />
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
    </asp:UpdatePanel>
</asp:Content>
