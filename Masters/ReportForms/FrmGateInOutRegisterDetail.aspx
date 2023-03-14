<%@ Page Title="Gate-In/Out Register Report" Language="C#" MasterPageFile="~/ERPmaster.master"
    AutoEventWireup="true" CodeFile="FrmGateInOutRegisterDetail.aspx.cs" Inherits="Masters_ReportForms_FrmGateInOutRegisterDetail" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content2" ContentPlaceHolderID="CPH_Form" runat="Server">
    <script src="../../Scripts/JScript.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery-1.4.1.js" type="text/javascript"></script>
    <script type="text/javascript">
        function CloseForm() {
            window.location.href = "../../main.aspx";
        }
    </script>
    <asp:UpdatePanel ID="upd1" runat="server">
        <ContentTemplate>
            <div style="margin: 0% 20% 0% 20%">
                <table border="1" style="width: 50%">
                    <tr>
                        <td>
                            <asp:Label ID="Label2" Text="Company Name" runat="server" CssClass="labelbold" />
                        </td>
                        <td>
                            <asp:DropDownList ID="DDcompany" CssClass="dropdown" Width="200px" runat="server">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="Label3" Text="Gate Type" runat="server" CssClass="labelbold" />
                        </td>
                        <td>
                            <asp:DropDownList ID="DDGateTypeInOut" CssClass="dropdown" Width="200px" runat="server">
                                <asp:ListItem Value="1" Selected="True">Gate In</asp:ListItem>
                                <asp:ListItem Value="2">Gate Out</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lblfromdate" Text="From Date" runat="server" CssClass="labelbold" />
                        </td>
                        <td>
                            <asp:TextBox ID="txtfromdate" CssClass="textb" runat="server" />
                            <asp:CalendarExtender ID="cal1" runat="server" TargetControlID="txtfromdate" Format="dd-MMM-yyyy">
                            </asp:CalendarExtender>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="Label1" Text="To Date" runat="server" CssClass="labelbold" />
                        </td>
                        <td>
                            <asp:TextBox ID="txttodate" CssClass="textb" runat="server" />
                            <asp:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="txttodate"
                                Format="dd-MMM-yyyy">
                            </asp:CalendarExtender>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2" align="right">
                            <asp:Button ID="btnpreview" runat="server" Text="Preview" CssClass="buttonnorm" OnClick="btnpreview_Click" />
                            <asp:Button ID="btnclose" runat="server" Text="Close" CssClass="buttonnorm" OnClientClick="return CloseForm();" />
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
