<%@ Page Language="C#" AutoEventWireup="true" CodeFile="FrmDyerDirectStockIssRecReport.aspx.cs"
    Inherits="Masters_ReportForms_FrmDyerDirectStockIssRecReport" MasterPageFile="~/ERPmaster.master"
    Title="Dyeing Direct Stock Issue Receive Report" EnableEventValidation="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="CPH_Form" runat="server">
    <script src="../../Scripts/JScript.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery-1.4.1.js" type="text/javascript"></script>
    <script type="text/javascript">
        function CloseForm() {

            window.location.href = "../../main.aspx";
        }
    </script>
    <asp:UpdatePanel ID="updatepenl1" runat="server">
        <ContentTemplate>
            <div>
                <div style="width: 100%; margin-left: 250px">
                    <asp:Panel runat="server" ID="panel1" Style="border: 1px groove Teal; border: 1px solid black;"
                        Width="70%">
                        <div style="padding: 0px 0px 0px 20px">
                            <table style="height: 150px">
                                <tr id="TRReportType" runat="server" visible="true">
                                    <td>
                                        <asp:Label ID="lblReportType" Text="Report Type" runat="server" CssClass="labelbold"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="DDReportType" runat="server" Width="350px" CssClass="dropdown"
                                            AutoPostBack="true" OnSelectedIndexChanged="DDReportType_SelectedIndexChanged">
                                            <asp:ListItem Value="0" Text="Issue">Issue</asp:ListItem>
                                            <asp:ListItem Value="1" Text="Receive">Receive</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblCompany" Text="CompanyName" runat="server" CssClass="labelbold"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="DDCompany" runat="server" Width="350px" CssClass="dropdown">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblDyeName" Text="DyerName" runat="server" CssClass="labelbold"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="DDDyerName" runat="server" Width="350px" CssClass="dropdown"
                                            AutoPostBack="true" OnSelectedIndexChanged="DDDyerName_SelectedIndexChanged">
                                        </asp:DropDownList>
                                        <asp:ListSearchExtender ID="ListSearchExtender1" runat="server" TargetControlID="DDDyerName"
                                            ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                                        </asp:ListSearchExtender>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="Label3" Text="Challan No" runat="server" CssClass="labelbold"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="DDChallanNo" runat="server" Width="350px" CssClass="dropdown">
                                        </asp:DropDownList>
                                        <asp:ListSearchExtender ID="ListSearchExtender5" runat="server" TargetControlID="DDChallanNo"
                                            ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                                        </asp:ListSearchExtender>
                                    </td>
                                </tr>
                                <tr id="TRItemName" runat="server" visible="true">
                                    <td>
                                        <asp:Label ID="lblItemName" Text="Item Name" runat="server" CssClass="labelbold"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="DDItemName" runat="server" Width="350px" CssClass="dropdown"
                                            AutoPostBack="true" OnSelectedIndexChanged="DDItemName_SelectedIndexChanged">
                                        </asp:DropDownList>
                                        <asp:ListSearchExtender ID="ListSearchExtender2" runat="server" TargetControlID="DDItemName"
                                            ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                                        </asp:ListSearchExtender>
                                    </td>
                                </tr>
                                <tr id="TRQuality" runat="server" visible="true">
                                    <td>
                                        <asp:Label ID="Label1" Text="Quality Name" runat="server" CssClass="labelbold"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList CssClass="dropdown" ID="DDQuality" Width="150px" runat="server">
                                        </asp:DropDownList>
                                        <asp:ListSearchExtender ID="ListSearchExtender3" runat="server" TargetControlID="DDQuality"
                                            ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                                        </asp:ListSearchExtender>
                                    </td>
                                </tr>
                                <tr id="TRShadeColor" runat="server" visible="true">
                                    <td>
                                        <asp:Label ID="Label2" Text="ShadeColor Name" runat="server" CssClass="labelbold"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList CssClass="dropdown" ID="DDShadeColorName" Width="150px" runat="server">
                                        </asp:DropDownList>
                                        <asp:ListSearchExtender ID="ListSearchExtender4" runat="server" TargetControlID="DDShadeColorName"
                                            ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                                        </asp:ListSearchExtender>
                                    </td>
                                </tr>
                                <tr>
                                    <td id="Tdselectdate" runat="server">
                                        <asp:CheckBox ID="ChkselectDate" Text="Select Date" runat="server" CssClass="checkboxbold"
                                            Checked="true" Enabled="false" />
                                    </td>
                                    <td>
                                        <table>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lblfromdt" Text="From Date" runat="server" CssClass="labelbold" />
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtfromDate" CssClass="textb" runat="server" Width="100px" />
                                                    <asp:CalendarExtender ID="calfrom" runat="server" TargetControlID="txtfromDate" Format="dd-MMM-yyyy">
                                                    </asp:CalendarExtender>
                                                </td>
                                                <td id="Tdtodatelabel" runat="server">
                                                    <asp:Label ID="lbltodt" Text="To Date" runat="server" CssClass="labelbold" />
                                                </td>
                                                <td id="Tdtodate" runat="server">
                                                    <asp:TextBox ID="txttodate" CssClass="textb" runat="server" Width="100px" />
                                                    <asp:CalendarExtender ID="Caltodate" runat="server" TargetControlID="txttodate" Format="dd-MMM-yyyy">
                                                    </asp:CalendarExtender>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr id="TRChkForSummary" runat="server" visible="true">
                                    <td id="Td1" runat="server">
                                        <asp:CheckBox ID="ChkForSummary" Text="For Summary" runat="server" CssClass="checkboxbold" />
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2" align="center">
                                        &nbsp;<asp:Button ID="BtnPreview" runat="server" CssClass="buttonnorm" Text="Preview"
                                            OnClick="BtnPreview_Click" />
                                        &nbsp;<asp:Button ID="BtnClose" runat="server" CssClass="buttonnorm" Text="Close"
                                            OnClientClick="return CloseForm();" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblErrmsg" runat="server" CssClass="labelbold" ForeColor="Red"></asp:Label>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </asp:Panel>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
