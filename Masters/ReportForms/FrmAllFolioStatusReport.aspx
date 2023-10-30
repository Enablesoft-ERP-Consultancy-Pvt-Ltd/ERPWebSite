<%@ Page Language="C#" AutoEventWireup="true" CodeFile="FrmAllFolioStatusReport.aspx.cs" Inherits="Masters_ReportForms_FrmAllFolioStatusReport"
    MasterPageFile="~/ERPmaster.master" Title="ALL Folio Status Report" EnableEventValidation="false" %>

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
    <asp:UpdatePanel ID="updatepenl1" runat="server">
        <ContentTemplate>
            <div>
                <div style="width: 366px; margin-left: 250px">
                    <asp:Panel runat="server" ID="panel1" Style="border: 1px groove Teal; border: 1px solid black;"
                        Width="350px">
                        <div style="padding: 0px 0px 0px 20px">
                            <table style="height: 150px">
                                <tr>
                                    <td>
                                        <asp:Label ID="lblCompany" Text="CompanyName" runat="server" CssClass="labelbold"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="DDCompany" runat="server" Width="200px" CssClass="dropdown">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr id="TRFromDate" runat="server" visible="true">
                                    <td>
                                        <asp:Label ID="lblMonthYaer" runat="server" Text="From Date" CssClass="labelbold"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtfromDate" CssClass="textb" runat="server" Width="100px" />
                                                    <asp:CalendarExtender ID="calfrom" runat="server" TargetControlID="txtfromDate" Format="dd-MMM-yyyy">
                                                    </asp:CalendarExtender>
                                    </td>
                                </tr>
                                <tr id="TRToDate" runat="server" visible="true">
                                    <td>
                                        <asp:Label ID="lblToMonthyear" runat="server" Text="To Date" CssClass="labelbold"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txttodate" CssClass="textb" runat="server" Width="100px" />
                                                    <asp:CalendarExtender ID="Caltodate" runat="server" TargetControlID="txttodate" Format="dd-MMM-yyyy">
                                                    </asp:CalendarExtender>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2" align="right">  
                                    <asp:CheckBox ID="ChkForPendingFolio" Text="For PendingFolio" runat="server" CssClass="checkboxbold" Visible="true" />
                                    <asp:CheckBox ID="ChkselectDate" Text="ByDate Wise" runat="server" CssClass="checkboxbold" Visible="true" />
                                    &nbsp;                                     
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
        <Triggers>
            <asp:PostBackTrigger ControlID="BtnPreview" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
