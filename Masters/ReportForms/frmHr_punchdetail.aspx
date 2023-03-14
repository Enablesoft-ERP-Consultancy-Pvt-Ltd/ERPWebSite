<%@ Page Title="Punch Detail" Language="C#" MasterPageFile="~/ERPmaster.master" AutoEventWireup="true"
    CodeFile="frmHr_punchdetail.aspx.cs" Inherits="Masters_ReportForms_frmHr_punchdetail" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content2" ContentPlaceHolderID="CPH_Form" runat="Server">
    <script type="text/javascript">
        function CloseForm() {
            window.location.href = "../../main.aspx";
        }
    </script>
    <asp:UpdatePanel ID="upd1" runat="server">
        <ContentTemplate>
            <table style="width: 100%">
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
                            </tr>
                            <tr>
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
                                    <asp:DropDownList ID="DDDept" runat="server" CssClass="dropdown" Width="80%" OnSelectedIndexChanged="DDDept_SelectedIndexChanged"
                                        AutoPostBack="true">
                                    </asp:DropDownList>
                                    &nbsp;<asp:CheckBox ID="CHkAllDept" Text="All Dept" runat="server" CssClass="checkboxbold"
                                        AutoPostBack="true" OnCheckedChanged="CHkAllDept_CheckedChanged" />
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 20%; border-style: dotted">
                                    <asp:Label ID="Label3" runat="server" CssClass="labelbold" Text="Designation" />
                                </td>
                                <td style="width: 75%; border-style: dotted">
                                    <asp:DropDownList ID="DDDesignation" runat="server" CssClass="dropdown" Width="80%"
                                        AutoPostBack="true" OnSelectedIndexChanged="DDDesignation_SelectedIndexChanged">
                                    </asp:DropDownList>
                                    &nbsp;<asp:CheckBox ID="ChkAllDesignation" Text="All Desig" runat="server" CssClass="checkboxbold"
                                        AutoPostBack="true" OnCheckedChanged="ChkAllDesignation_CheckedChanged" />
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 20%; border-style: dotted">
                                    <asp:Label ID="Label8" runat="server" CssClass="labelbold" Text="Wages Calculation" />
                                </td>
                                <td style="width: 75%; border-style: dotted">
                                    <asp:DropDownList ID="DDwagescalculation" CssClass="dropdown" runat="server">
                                        <asp:ListItem Text="All" Value="0" />
                                        <asp:ListItem Text="Monthly" Value="1" />
                                        <asp:ListItem Text="Daily" Value="2" />
                                        <asp:ListItem Text="Pcs Wise" Value="3" />
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
                                    <asp:Label ID="Label1" CssClass="labelbold" Text="From Date" runat="server" />
                                </td>
                                <td style="width: 80%; border-style: dotted">
                                    <table width="100%">
                                        <tr>
                                            <td style="width: 30%">
                                                <asp:TextBox ID="txtfromdate" CssClass="textboxm" Width="95%" runat="server" AutoComplete="off" />
                                                <asp:CalendarExtender ID="calfromdate" TargetControlID="txtfromdate" Format="dd-MMM-yyyy"
                                                    runat="server">
                                                </asp:CalendarExtender>
                                            </td>
                                            <td style="width: 70%" runat="server">
                                                <asp:ImageButton ID="imgfromdate" CausesValidation="false" AlternateText="Click here to display calender"
                                                    ImageUrl="~/Images/calendar.png" runat="server" Visible="false" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr runat="server" id="trtodate" visible="false">
                                <td style="width: 20%; border-style: dotted">
                                    <asp:Label ID="Label4" CssClass="labelbold" Text="To Date" runat="server" />
                                </td>
                                <td style="width: 80%; border-style: dotted">
                                    <table width="100%">
                                        <tr>
                                            <td style="width: 30%">
                                                <asp:TextBox ID="txttodate" CssClass="textboxm" Width="95%" runat="server" AutoComplete="off" />
                                                <asp:CalendarExtender ID="CalendarExtender1" TargetControlID="txttodate" Format="dd-MMM-yyyy"
                                                    runat="server">
                                                </asp:CalendarExtender>
                                            </td>
                                            <td id="Td1" style="width: 70%" runat="server">
                                                <asp:ImageButton ID="ImageButton1" CausesValidation="false" AlternateText="Click here to display calender"
                                                    ImageUrl="~/Images/calendar.png" runat="server" Visible="false" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 20%; border-style: dotted">
                                    <asp:Label ID="Label7" runat="server" CssClass="labelbold" Text="Report Type" />
                                </td>
                                <td style="width: 75%; border-style: dotted">
                                    <asp:DropDownList ID="DDreporttype" runat="server" Width="95%" CssClass="dropdown"
                                        OnSelectedIndexChanged="DDreporttype_SelectedIndexChanged" AutoPostBack="true">
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
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="Btnpreview" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
