<%@ Page Title="ON Loom Inspection Employee Wise" Language="C#" MasterPageFile="~/ERPmaster.master"
    AutoEventWireup="true" CodeFile="FrmOnLoomInspectionEmpWiseReport.aspx.cs" Inherits="Masters_ReportForms_FrmOnLoomInspectionEmpWiseReport" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content2" ContentPlaceHolderID="CPH_Form" runat="Server">
    <script type="text/javascript">
        function CloseForm() {
            window.location.href = "../../main.aspx";
        }
    </script>
    <asp:UpdatePanel ID="upd1" runat="server">
        <ContentTemplate>
            <div>
                <div style="margin: 0% 30% 0% 30%">
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
                                <asp:DropDownList ID="DDProdunit" runat="server" CssClass="dropdown" Width="200px">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="Label3" runat="server" Text="Weaver Name" CssClass="labelbold"></asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="DDEmpName" runat="server" CssClass="dropdown" AutoPostBack="true"
                                    Width="200px" OnSelectedIndexChanged="DDEmpName_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="Label15" runat="server" Text="Loom No." CssClass="labelbold"></asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="DDLoomNo" runat="server" CssClass="dropdown" AutoPostBack="true"
                                    Width="200px">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lblcategoryname" runat="server" CssClass="labelbold" Text="Category Name"></asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="DDCategory" runat="server" AutoPostBack="True" CssClass="dropdown"
                                    Width="200px" OnSelectedIndexChanged="DDCategory_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr id="TRddItemName" runat="server">
                            <td>
                                <asp:Label ID="lblitemname" runat="server" CssClass="labelbold" Text="Item Name"></asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddItemName" runat="server" AutoPostBack="True" CssClass="dropdown"
                                    Width="200px" OnSelectedIndexChanged="ddItemName_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr id="TRDDQuality" runat="server" visible="false">
                            <td>
                                <asp:Label ID="lblqualityname" runat="server" CssClass="labelbold" Text="Quality"></asp:Label>
                            </td>
                            <td colspan="3">
                                <asp:DropDownList ID="DDQuality" runat="server" AutoPostBack="True" CssClass="dropdown"
                                    Width="200px">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr id="TRDDDesign" runat="server" visible="false">
                            <td>
                                <asp:Label ID="lbldesignname" runat="server" CssClass="labelbold" Text="Design"></asp:Label>
                            </td>
                            <td colspan="3">
                                <asp:DropDownList ID="DDDesign" runat="server" AutoPostBack="True" CssClass="dropdown"
                                    Width="200px">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr id="TRDDColor" runat="server" visible="false">
                            <td>
                                <asp:Label ID="lblcolorname" runat="server" CssClass="labelbold" Text="Color"></asp:Label>
                            </td>
                            <td colspan="3">
                                <asp:DropDownList ID="DDColor" runat="server" CssClass="dropdown" Width="200px">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr id="TRDDShape" runat="server" visible="false">
                            <td>
                                <asp:Label ID="lblshapename" runat="server" CssClass="labelbold" Text="Shape"></asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="DDShape" runat="server" AutoPostBack="True" CssClass="dropdown"
                                    Width="200px" OnSelectedIndexChanged="DDShape_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr id="TRDDSize" runat="server" visible="false">
                            <td>
                                <asp:Label ID="lblsizename" runat="server" CssClass="labelbold" Text="Size"></asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList CssClass="dropdown" Width="50" ID="DDsizetype" runat="server" AutoPostBack="True"
                                    OnSelectedIndexChanged="DDsizetype_SelectedIndexChanged" />
                                <br />
                                <asp:DropDownList ID="DDSize" runat="server" AutoPostBack="True" CssClass="dropdown"
                                    Width="200px">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td>
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
                            <td>
                                <asp:CheckBox ID="chksummary" Text="Summary" runat="server" CssClass="checkboxbold"
                                    Visible="false" />
                            </td>
                            <td align="right">
                                <asp:CheckBox ID="chkexport" Text="Export" runat="server" CssClass="checkboxbold"
                                    Visible="false" />
                                <asp:Button ID="btnPreview" runat="server" Text="Preview" CssClass="buttonnorm" OnClick="btnPreview_Click" />
                                <asp:Button ID="btnclose" runat="server" Text="Close" CssClass="buttonnorm" OnClientClick="return CloseForm();" />
                            </td>
                        </tr>
                    </table>
                </div>
            </div>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="btnPreview" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
