﻿<%@ Page Title="PENDING BAZAAR PCS ISS TO FINISHING PROCESS REPORT" Language="C#" MasterPageFile="~/ERPmaster.master"
    AutoEventWireup="true" CodeFile="FrmPendingBazaarPcsIssToFinishingProcess.aspx.cs" Inherits="Masters_ReportForms_FrmPendingBazaarPcsIssToFinishingProcess" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content2" ContentPlaceHolderID="CPH_Form" runat="Server">
    <script type="text/javascript">
        function CloseForm() {
            window.location.href = "../../main.aspx";
        }
    </script>
    <asp:UpdatePanel ID="upd1" runat="server">
        <ContentTemplate>
            <div style="margin: 2% 20% 0% 20%">
                <table border="0" cellpadding="0" cellspacing="0">
                    <tr>
                        <td>
                            <asp:Label ID="Label9" runat="server" Text="Company Name" CssClass="labelbold"></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="DDcompany" runat="server" CssClass="dropdown" Width="200px">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="Label1" runat="server" Text="Process Name" CssClass="labelbold"></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="DDProcessName" runat="server" CssClass="dropdown" Width="200px">                               
                            </asp:DropDownList>
                             <cc1:ListSearchExtender ID="ListSearchExtender1" runat="server" TargetControlID="DDProcessName"
                                                ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                                            </cc1:ListSearchExtender>
                        </td>
                    </tr>                   
                   
                    <tr>
                        <td>
                            &nbsp
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <table border="0" cellpadding="0" cellspacing="0">
                                <tr>
                                    <td>
                                        <asp:Label ID="Label2" runat="server" CssClass="labelbold" Text="From Date"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtFromdate" runat="server" Width="95px" CssClass="textb"></asp:TextBox>
                                        <asp:CalendarExtender ID="CalenderExtendertxtdate" runat="server" TargetControlID="txtFromdate"
                                            Format="dd-MMM-yyyy">
                                        </asp:CalendarExtender>
                                    </td>
                                    <td>
                                        <asp:Label ID="Label3" runat="server" CssClass="labelbold" Text="To Date"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txttodate" runat="server" Width="95px" CssClass="textb"></asp:TextBox>
                                        <asp:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="txttodate"
                                            Format="dd-MMM-yyyy">
                                        </asp:CalendarExtender>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2" align="right">
                            <asp:Button ID="btnPreview" runat="server" Text="Preview" CssClass="buttonnorm"
                                OnClick="btnPreview_Click" />
                            <asp:Button ID="btnclose" runat="server" Text="Close" CssClass="buttonnorm" OnClientClick="return CloseForm();" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <asp:Label ID="lblmsg" Text="" CssClass="labelbold" ForeColor="Red" runat="server" />
                        </td>
                    </tr>
                </table>
            </div>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="btnPreview" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
