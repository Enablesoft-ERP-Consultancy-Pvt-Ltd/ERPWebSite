<%@ Page Title="Attendance Process" Language="C#" MasterPageFile="~/ERPmaster.master"
    AutoEventWireup="true" CodeFile="frmattendanceprocess.aspx.cs" Inherits="Masters_Payroll_frmattendanceprocess" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content2" ContentPlaceHolderID="CPH_Form" runat="Server">
    <script type="text/javascript">
        function CloseForm() {
            window.location.href = "../../main.aspx";
        }

        function validator() { return confirm('Do you want to Process Attendance ?'); } 
    </script>
    <asp:UpdatePanel ID="upd1" runat="server">
        <ContentTemplate>
            <div style="margin: 0% 20% 0% 20%">
                <table border="1" width="100%" cellpadding="" cellspacing="">
                    <tr>
                        <td style="width: 20%; border-style: dotted">
                            <asp:Label ID="Label9" CssClass="labelbold" Text="Company" runat="server" />
                        </td>
                        <td style="width: 80%; border-style: dotted">
                            <asp:DropDownList ID="DDCompanyName" CssClass="dropdown" Width="90%" runat="server">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 20%; border-style: dotted">
                            <asp:Label ID="Label10" CssClass="labelbold" Text="Branch" runat="server" />
                        </td>
                        <td style="width: 80%; border-style: dotted">
                            <asp:DropDownList ID="DDBranchName" CssClass="dropdown" Width="90%" runat="server">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 20%; border-style: dotted">
                            <asp:Label Text="Start Date" CssClass="labelbold" runat="server" />
                        </td>
                        <td style="width: 80%; border-style: dotted">
                            <asp:TextBox ID="txtfromdate" CssClass="textb" Width="90%" autocomplete="off" runat="server" />
                            <asp:CalendarExtender ID="calfromdate" TargetControlID="txtfromdate" Format="dd-MMM-yyyy"
                                runat="server">
                            </asp:CalendarExtender>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 20%; border-style: dotted">
                            <asp:Label ID="Label1" Text="End Date" CssClass="labelbold" runat="server" />
                        </td>
                        <td style="width: 80%; border-style: dotted">
                            <asp:TextBox ID="txttodate" CssClass="textb" Width="90%" runat="server" autocomplete="off" />
                            <asp:CalendarExtender ID="CalendarExtender1" TargetControlID="txttodate" Format="dd-MMM-yyyy"
                                runat="server">
                            </asp:CalendarExtender>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 20%; border-style: dotted">
                            <asp:Label ID="Label2" runat="server" CssClass="labelbold" Text="Emp. Code" />
                        </td>
                        <td style="width: 75%; border-style: dotted">
                            <asp:TextBox ID="txtempcode" runat="server" CssClass="textboxm" Width="90%" />
                        </td>
                    </tr>
                    <tr>
                        <td style="border-style: dotted">
                        </td>
                        <td style="width: 75%; border-style: dotted">
                            <asp:Label ID="Label6" runat="server" CssClass="labelbold" ForeColor="Red" Text="For multiple emp. Code use commas(,)eg:0001,0002" />
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 20%; border-style: dotted">
                        </td>
                        <td colspan="2" align="right" style="width: 80%; border-style: dotted">
                            <asp:Button ID="btnprocess" Text="Process" CssClass="buttonnorm" runat="server" UseSubmitBehavior="false"
                                OnClick="btnprocess_Click" OnClientClick="if (!validator())return; this.disabled=true;this.value = 'Attendance Processing wait ...';" />
                            <asp:Button ID="btnclose" Text="Close" CssClass="buttonnorm" OnClientClick="return CloseForm();"
                                runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <asp:Label ID="lblmsg" CssClass="labelbold" ForeColor="Red" Text="" runat="server" />
                        </td>
                    </tr>
                </table>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
