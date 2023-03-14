<%@ Page Title="Hold/Rejected Pcs Detail" Language="C#" MasterPageFile="~/ERPmaster.master"
    AutoEventWireup="true" CodeFile="frmhold_rejecteddetail.aspx.cs" Inherits="Masters_ReportForms_frmhold_rejecteddetail" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content2" ContentPlaceHolderID="CPH_Form" runat="Server">
    <script src="../../Scripts/JScript.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery-1.4.1.js" type="text/javascript"></script>
    <script type="text/javascript">
        function validate() {
            if (document.getElementById("<%=DDcompany.ClientID %>").selectedIndex < "0") {
                alert('Plz Select Company...')
                document.getElementById("<%=DDcompany.ClientID %>").focus()
                return false;
            }
            if (document.getElementById("<%=DDjobname.ClientID %>").selectedIndex <= "0") {
                alert('Plz Select Job Name...')
                document.getElementById("<%=DDjobname.ClientID %>").focus()
                return false;
            }
        }
        function CloseForm() {
            window.location.href = "../../main.aspx";
        }
    </script>
    <asp:UpdatePanel ID="upd1" runat="server">
        <ContentTemplate>
            <div style="margin: 1% 20% 0% 30%">
                <table border="0" cellpadding="0" cellspacing="5">
                    <tr>
                        <td>
                            <asp:Label ID="lblcompname" CssClass="labelbold" Text="Company Name" runat="server" />
                        </td>
                        <td>
                            <asp:DropDownList ID="DDcompany" CssClass="dropdown" Width="250px" runat="server">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="Label1" CssClass="labelbold" Text="Job Name" runat="server" />
                        </td>
                        <td>
                            <asp:DropDownList ID="DDjobname" CssClass="dropdown" Width="250px" runat="server">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="Label3" CssClass="labelbold" Text="From Date" runat="server" />
                        </td>
                        <td>
                            <asp:TextBox ID="txtfromdate" CssClass="textb" Width="100px" runat="server" />
                            <asp:CalendarExtender ID="calfrom" TargetControlID="txtfromdate" runat="server" Format="dd-MMM-yyyy">
                            </asp:CalendarExtender>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="Label4" CssClass="labelbold" Text="To Date" runat="server" />
                        </td>
                        <td>
                            <asp:TextBox ID="txttodate" CssClass="textb" Width="100px" runat="server" />
                            <asp:CalendarExtender ID="CalendarExtender1" TargetControlID="txttodate" runat="server"
                                Format="dd-MMM-yyyy">
                            </asp:CalendarExtender>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2" style="text-align: right">
                            <asp:Button ID="btnpreview" Text="Preview" CssClass="buttonnorm" runat="server" OnClientClick="return validate();"
                                OnClick="btnpreview_Click" />
                            <asp:Button ID="btnclose" Text="Close" CssClass="buttonnorm" OnClientClick="return CloseForm();"
                                runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <asp:Label ID="lblmsg" runat="server" CssClass="labelbold" ForeColor="Red"></asp:Label>
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
