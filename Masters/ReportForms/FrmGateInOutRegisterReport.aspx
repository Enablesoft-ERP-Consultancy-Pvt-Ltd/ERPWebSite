<%@ Page Title="GATE IN/OUT REGISTER REPORT" Language="C#" MasterPageFile="~/ERPmaster.master"
    AutoEventWireup="true" CodeFile="FrmGateInOutRegisterReport.aspx.cs" Inherits="Masters_ReportForms_FrmGateInOutRegisterReport" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
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
                            <asp:Label ID="Label1" runat="server" Text="Register Type" CssClass="labelbold"></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="DDRegisterType" runat="server" CssClass="dropdown" Width="200px"
                                AutoPostBack="true" OnSelectedIndexChanged="DDRegisterType_SelectedIndexChanged">
                                <asp:ListItem Value="1" Selected="True">Gate In Out Register</asp:ListItem>
                                <asp:ListItem Value="2">Material Gate Out Register</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="Label4" runat="server" Text="Gate Type" CssClass="labelbold"></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="DDGateType" runat="server" CssClass="dropdown" Width="200px">
                                <asp:ListItem Value="1" Selected="True">Gate In</asp:ListItem>
                                <asp:ListItem Value="2">Gate Out</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr id="TRMaterialType" runat="server" visible="false">
                        <td>
                            <asp:Label ID="Label5" runat="server" CssClass="labelbold" Text="Material Return Type"></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="DDMaterialReturnType" runat="server" CssClass="dropdown" Width="200px">
                                <asp:ListItem Value="0">ALL</asp:ListItem>
                                <asp:ListItem Value="1">Return able</asp:ListItem>
                                <asp:ListItem Value="2">Non Return able</asp:ListItem>
                            </asp:DropDownList>
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
                            <asp:Button ID="btnPreview" runat="server" Text="Export to Excel" CssClass="buttonnorm"
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
