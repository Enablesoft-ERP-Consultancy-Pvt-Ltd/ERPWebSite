<%@ Page Title="Loom Beam/Raw Status" Language="C#" MasterPageFile="~/ERPmaster.master"
    AutoEventWireup="true" CodeFile="frmproductionloomstatus.aspx.cs" Inherits="Masters_ReportForms_frmproductionloomstatus" %>

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
            <div>
                <div style="margin: 0% 20% 0% 20%">
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
                                <asp:Label ID="Label1" runat="server" Text="Production Unit" CssClass="labelbold"></asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="DDProdunit" runat="server" CssClass="dropdown" AutoPostBack="true"
                                    Width="200px" OnSelectedIndexChanged="DDProdunit_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="Label15" runat="server" Text="Loom No." CssClass="labelbold"></asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="DDLoomNo" runat="server" CssClass="dropdown" AutoPostBack="true"
                                    Width="200px" OnSelectedIndexChanged="DDLoomNo_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="Label3" runat="server" Text="Status" CssClass="labelbold"></asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="DDstatus" CssClass="dropdown" runat="server" AutoPostBack="true"
                                    OnSelectedIndexChanged="DDstatus_SelectedIndexChanged">
                                    <asp:ListItem Text="All" Value="All" />
                                    <asp:ListItem Text="Complete" Value="Complete" />
                                    <asp:ListItem Text="Pending" Value="Pending" />
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="Label7" runat="server" Text="Folio No." CssClass="labelbold"></asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="DDFolioNo" runat="server" CssClass="dropdown" Width="150px">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:CheckBox ID="chkdate" Text="For Date" runat="server" CssClass="checkboxbold" />
                            </td>
                            <td>
                                <table>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblfrom" Text="From Date" runat="server" CssClass="labelbold" /><br />
                                            <asp:TextBox ID="txtfromdate" CssClass="textb" Width="100px" runat="server" />
                                            <asp:CalendarExtender ID="calfrom" TargetControlID="txtfromdate" runat="server" Format="dd-MMM-yyyy">
                                            </asp:CalendarExtender>
                                        </td>
                                        <td>
                                            <asp:Label ID="Label2" Text="To Date" runat="server" CssClass="labelbold" /><br />
                                            <asp:TextBox ID="txttodate" CssClass="textb" Width="100px" runat="server" />
                                            <asp:CalendarExtender ID="calto" TargetControlID="txttodate" runat="server" Format="dd-MMM-yyyy">
                                            </asp:CalendarExtender>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2" align="right">
                                <asp:Button ID="btnPreview" runat="server" Text="Preview" CssClass="buttonnorm" OnClick="btnPreview_Click" />
                                <asp:Button ID="btnclose" runat="server" Text="Close" CssClass="buttonnorm" OnClientClick="return CloseForm();" />
                            </td>
                        </tr>
                    </table>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
