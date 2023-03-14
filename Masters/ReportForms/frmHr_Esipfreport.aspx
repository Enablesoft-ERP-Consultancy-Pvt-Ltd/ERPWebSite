<%@ Page Title="Employee Reports" Language="C#" MasterPageFile="~/ERPmaster.master"
    AutoEventWireup="true" CodeFile="frmHr_Esipfreport.aspx.cs" Inherits="Masters_ReportForms_frmHr_Esipfreport" %>

<asp:Content ID="Content2" ContentPlaceHolderID="CPH_Form" runat="Server">
    <script type="text/javascript">
        function CloseForm() { window.location.href = "../../main.aspx"; }
    </script>
    <asp:UpdatePanel ID="upd1" runat="server">
        <ContentTemplate>
            <div style="margin: 0% 0% 0% 0%">
                <table width="100%">
                    <tr>
                        <td style="width: 60%" valign="top">
                            <table border="1" cellspacing="2" style="width: 100%">
                                <tr>
                                    <td style="width: 20%; border-style: dotted">
                                        <asp:Label ID="Label5" runat="server" CssClass="labelbold" Text="Company Name" />
                                    </td>
                                    <td style="width: 75%; border-style: dotted">
                                        <asp:DropDownList ID="DDCompanyName" runat="server" CssClass="dropdown" Width="95%">
                                        </asp:DropDownList>
                                    </td>
                                </tr>                    <tr>
                        <td style="width: 20%; border-style: dotted">
                            <asp:Label ID="Label11" CssClass="labelbold" Text="Branch" runat="server" />
                        </td>
                        <td style="width: 80%; border-style: dotted">
                            <asp:DropDownList ID="DDBranchName" CssClass="dropdown" Width="90%" runat="server">
                            </asp:DropDownList>
                        </td>
                    </tr>
                                <tr>
                                    <td style="width: 20%; border-style: dotted">
                                        <asp:Label ID="lbldept" runat="server" CssClass="labelbold" Text="Department" />
                                    </td>
                                    <td style="width: 75%; border-style: dotted">
                                        <asp:DropDownList ID="DDDept" runat="server" CssClass="dropdown" Width="95%" OnSelectedIndexChanged="DDDept_SelectedIndexChanged"
                                            AutoPostBack="true">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 20%; border-style: dotted">
                                        <asp:Label ID="Label1" runat="server" CssClass="labelbold" Text="Designation" />
                                    </td>
                                    <td style="width: 75%; border-style: dotted">
                                        <asp:DropDownList ID="DDDesignation" runat="server" CssClass="dropdown" Width="95%"
                                            AutoPostBack="true" OnSelectedIndexChanged="DDDesignation_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 20%; border-style: dotted">
                                        <asp:Label ID="Label2" runat="server" CssClass="labelbold" Text="Emp. Code" />
                                    </td>
                                    <td style="width: 75%; border-style: dotted">
                                        <asp:TextBox ID="txtempcode" runat="server" CssClass="textboxm" Width="95%" />
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
                                        <asp:Label ID="Label3" runat="server" CssClass="labelbold" Text="Month" />
                                    </td>
                                    <td style="width: 75%; border-style: dotted">
                                        <asp:DropDownList ID="DDMonth" runat="server" CssClass="dropdown">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 20%; border-style: dotted">
                                        <asp:Label ID="Label4" runat="server" CssClass="labelbold" Text="Year" />
                                    </td>
                                    <td style="width: 75%; border-style: dotted">
                                        <asp:DropDownList ID="DDyear" runat="server" CssClass="dropdown">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr runat="server" visible="false">
                                    <td style="width: 20%; border-style: dotted">
                                        <asp:Label ID="Label8" Text="Wages Calculation" CssClass="labelbold" runat="server" />
                                    </td>
                                    <td style="width: 80%; border-style: dotted">
                                        <asp:DropDownList ID="DDwagescalculation" CssClass="dropdown" runat="server">
                                            <asp:ListItem Text="Monthly" Value="1" />
                                            <asp:ListItem Text="Pcs Wise" Value="3" />
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 20%; border-style: dotted">
                                        <asp:Label ID="Label7" runat="server" CssClass="labelbold" Text="Report Type" />
                                    </td>
                                    <td style="width: 75%; border-style: dotted">
                                        <asp:DropDownList ID="DDreporttype" runat="server" Width="50%" CssClass="dropdown">
                                            <asp:ListItem Text="ESI Format" Value="1"></asp:ListItem>
                                            <asp:ListItem Text="PF Format" Value="2"></asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right" colspan="2" style="width: 100%; border-style: dotted">
                                        <asp:Button ID="Btnpreview" runat="server" CssClass="buttonnorm" OnClick="Btnpreview_Click"
                                            Text="Preview" />
                                        <asp:Button ID="Btnclose" runat="server" CssClass="buttonnorm" Text="Close" OnClientClick="return CloseForm();" />
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2" style="width: 100%; border-style: dotted">
                                        <asp:Label ID="lblmsg" runat="server" CssClass="labelbold" ForeColor="Red" Text="" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                        <td style="width: 40%" valign="top">
                            <table border="1" width="100%" cellspacing="2">
                                <tr>
                                    <td>
                                        <table width="100%">
                                            <tr>
                                                <td>
                                                    <asp:Label ID="Label10" Text="Department" CssClass="labelbold" runat="server" />
                                                    <br />
                                                    <div style="width: 100%; overflow: auto">
                                                        <asp:ListBox ID="lstdept" runat="server" Width="95%" Height="100px" SelectionMode="Multiple">
                                                        </asp:ListBox>
                                                    </div>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="right">
                                                    <asp:LinkButton ID="btnDeletedept" Text="Remove Department" runat="server" CssClass="linkbuttonnew"
                                                        OnClick="btnDeletedept_Click" />
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <table width="100%">
                                            <tr>
                                                <td>
                                                    <asp:Label ID="Label9" Text="Designation" CssClass="labelbold" runat="server" />
                                                    <br />
                                                    <div style="width: 100%; overflow: auto">
                                                        <asp:ListBox ID="lstdesignation" runat="server" Width="95%" Height="100px" SelectionMode="Multiple">
                                                        </asp:ListBox>
                                                    </div>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="right">
                                                    <asp:LinkButton ID="btndeletedesignation" Text="Remove Designation" runat="server"
                                                        CssClass="linkbuttonnew" OnClick="btndeletedesignation_Click" />
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </div>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="Btnpreview" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
