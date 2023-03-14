<%@ Page Title="Tool Iss Rec" Language="C#" MasterPageFile="~/ERPmaster.master" AutoEventWireup="true"
    CodeFile="frmtoolissrec.aspx.cs" Inherits="Masters_ReportForms_frmtoolissrec" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content2" ContentPlaceHolderID="CPH_Form" runat="Server">
    <script type="text/javascript">

        function CloseForm() {
            window.location.href = "../../main.aspx";
        }
    </script>
    <asp:UpdatePanel ID="upd1" runat="server">
        <ContentTemplate>
            <div style="margin: 0% 20% 0% 20%">
                <table border="1" width="100%" cellspacing="">
                    <tr>
                        <td style="width: 20%; border-style: dotted">
                            <asp:Label Text="Employee" CssClass="labelbold" runat="server" />
                        </td>
                        <td style="width: 80%; border-style: dotted">
                            <asp:DropDownList ID="DDemployee" CssClass="dropdown" Width="95%" runat="server">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 20%; border-style: dotted">
                            <asp:Label ID="Label1" Text="Unit Name" CssClass="labelbold" runat="server" />
                        </td>
                        <td style="width: 80%; border-style: dotted">
                            <asp:DropDownList ID="DDunitname" CssClass="dropdown" Width="95%" runat="server">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 20%; border-style: dotted">
                            <asp:Label ID="Label2" Text="Job Name" CssClass="labelbold" runat="server" />
                        </td>
                        <td style="width: 80%; border-style: dotted">
                            <asp:DropDownList ID="DDjobname" CssClass="dropdown" Width="95%" runat="server">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 20%; border-style: dotted">
                            <asp:Label ID="Label3" Text="From" CssClass="labelbold" runat="server" />
                        </td>
                        <td style="width: 80%; border-style: dotted">
                            <table width="100%">
                                <tr>
                                    <td style="width: 40%">
                                        <asp:TextBox ID="txtfrom" CssClass="textb" runat="server" Width="80%" />
                                        <asp:CalendarExtender ID="calfrom" TargetControlID="txtfrom" runat="server" Format="dd-MMM-yyyy">
                                        </asp:CalendarExtender>
                                    </td>
                                    <td style="width: 20%">
                                        <asp:Label Text="To" CssClass="labelbold" runat="server" />
                                    </td>
                                    <td style="width: 50%">
                                        <asp:TextBox ID="txtto" CssClass="textb" runat="server" Width="80%" />
                                        <asp:CalendarExtender ID="calto" TargetControlID="txtto" runat="server" Format="dd-MMM-yyyy">
                                        </asp:CalendarExtender>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 100%" colspan="2" align="right">
                            <asp:Button Text="Preview" ID="btnpreview" CssClass="buttonnorm" runat="server" OnClick="btnpreview_Click" />
                            <asp:Button Text="Close" ID="btnclose" runat="server" CssClass="buttonnorm" OnClientClick="return CloseForm();" />
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
