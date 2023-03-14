<%@ Page Title="OnLoom Inspection Consolidated Report" Language="C#" AutoEventWireup="true"
    CodeFile="FrmOnLoomInspectionConsolidatedReport.aspx.cs" MasterPageFile="~/ERPmaster.master"
    Inherits="Masters_Campany_FrmOnLoomInspectionConsolidatedReport" EnableEventValidation="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="CPH_Form" runat="server">
    <script src="../../Scripts/JScript.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery-1.4.1.js" type="text/javascript"></script>
    <%--<script type="text/javascript" src="../../Scripts/Fixfocus.js"></script>--%>
    <script type="text/javascript">
        function CloseForm() {
            window.location.href = "../../main.aspx";
        }      
       
    </script>
    <script runat="server">
        protected void RadioButton_CheckedChanged(object sender, System.EventArgs e)
        {
            TRConsolidated.Visible = true;
            TRFolioMaterialLotNoTracking.Visible = false;
            if (RDConsolidated.Checked == true)
            {
                TRFolioMaterialLotNoTracking.Visible = false;

            }
            if (RDFolioMaterialLotNoTrack.Checked == true)
            {
                TRFolioMaterialLotNoTracking.Visible = true;
                TRConsolidated.Visible = false;

            }

        }

    </script>
    <asp:UpdatePanel ID="updatepanel1" runat="server">
        <ContentTemplate>
            <div style="margin: 2% 20% 0% 5%;">
                <div style="width: 100%">
                    <div style="float: left; width: 30%">
                        <asp:Panel ID="pnl1" runat="server" Style="border: 1px Solid">
                            <table>
                                <tr>
                                    <td>
                                        <asp:RadioButton ID="RDConsolidated" Text="Consolidated Report" runat="server" CssClass="radiobuttonnormal"
                                            GroupName="A" Checked="true" AutoPostBack="true" OnCheckedChanged="RadioButton_CheckedChanged" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:RadioButton ID="RDFolioMaterialLotNoTrack" Text="Folio Material LotNo Track"
                                            runat="server" CssClass="radiobuttonnormal" GroupName="A" AutoPostBack="true"
                                            OnCheckedChanged="RadioButton_CheckedChanged" />
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                    </div>
                    <div style="float: right; width: 70%">
                        <asp:Panel ID="pnl2" runat="server" Style="border: 1px solid">
                            <table>
                                <tr id="TRConsolidated" runat="server" visible="true">
                                    <td>
                                        <asp:Label ID="lblDate" Text="Date" runat="server" CssClass="labelbold" />
                                        &nbsp;
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtDate" CssClass="textb" runat="server" Width="100px" />
                                        <asp:CalendarExtender ID="calfrom" runat="server" TargetControlID="txtDate" Format="dd-MMM-yyyy">
                                        </asp:CalendarExtender>
                                    </td>
                                </tr>
                                <tr id="TRFolioMaterialLotNoTracking" runat="server" visible="false">
                                    <td>
                                        <asp:Label ID="lblfromdt" Text="From Date" runat="server" CssClass="labelbold" />
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtfromDate" CssClass="textb" runat="server" Width="100px" />
                                        <asp:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="txtfromDate"
                                            Format="dd-MMM-yyyy">
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
                                <tr>
                                    <td colspan="2" align="right">
                                        <asp:Button ID="btnpreview" Text="Preview" runat="server" CssClass="buttonnorm" OnClick="btnpreview_Click" />
                                        <asp:Button ID="btnclose" Text="Close" runat="server" CssClass="buttonnorm" OnClientClick="CloseForm();" />
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <asp:Label ID="LblErrorMessage" runat="server" Text="" Font-Bold="false" ForeColor="Red"></asp:Label>
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                    </div>
                </div>
            </div>
        </ContentTemplate>
        <Triggers>
            <%-- <asp:PostBackTrigger ControlID="btnprintstockrawdetail" />--%>
            <asp:PostBackTrigger ControlID="btnpreview" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
