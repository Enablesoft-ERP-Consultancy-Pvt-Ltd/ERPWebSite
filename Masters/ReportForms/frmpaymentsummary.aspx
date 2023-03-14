<%@ Page Title="PAYMENT SUMMARY" Language="C#" MasterPageFile="~/ERPmaster.master"
    AutoEventWireup="true" CodeFile="frmpaymentsummary.aspx.cs" Inherits="Masters_ReportForms_frmpaymentsummary" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content2" ContentPlaceHolderID="CPH_Form" runat="Server">
    <script src="../../Scripts/JScript.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery-1.4.1.js" type="text/javascript"></script>
    <script type="text/javascript">
        function CloseForm() {
            window.location.href = "../../main.aspx";
        }       
    </script>
    <asp:UpdatePanel ID="Upd1" runat="server">
        <ContentTemplate>
            <div style="margin: 2% 20% 0% 20%">
                <asp:Panel ID="pnl2" runat="server" Style="border: 1px Solid">
                    <table>
                        <tr>
                            <td>
                                <asp:Label ID="lblcomp" Text="Company Name" runat="server" CssClass="labelbold" />
                            </td>
                            <td>
                                <asp:DropDownList ID="DDCompany" runat="server" CssClass="dropdown" Width="300px">
                                </asp:DropDownList>
                            </td>
                            <td id="TDsrno" runat="server" visible="false">
                                <table>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblsrno" Text="Sr No." CssClass="labelbold" runat="server" />
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtsrno" CssClass="textb" Width="150px" runat="server" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="Label1" Text="Process Name" runat="server" CssClass="labelbold" />
                            </td>
                            <td>
                                <asp:DropDownList ID="DDprocess" runat="server" CssClass="dropdown" Width="300px"
                                    AutoPostBack="true" OnSelectedIndexChanged="DDprocess_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="Label2" Text="Weaver/Contractor Name" runat="server" CssClass="labelbold" />
                            </td>
                            <td>
                                <asp:DropDownList ID="DDWeaver" runat="server" CssClass="dropdown" Width="300px">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="Label4" Text="Quality Type" runat="server" CssClass="labelbold" />
                            </td>
                            <td>
                                <asp:DropDownList ID="DDQtype" runat="server" CssClass="dropdown" Width="300px" AutoPostBack="true"
                                    OnSelectedIndexChanged="DDQtype_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="Label9" Text="Quality" runat="server" CssClass="labelbold" />
                            </td>
                            <td>
                                <asp:DropDownList ID="DDQuality" runat="server" CssClass="dropdown" Width="300px"
                                    AutoPostBack="true" OnSelectedIndexChanged="DDQuality_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="Label5" Text="Design" runat="server" CssClass="labelbold" />
                            </td>
                            <td>
                                <asp:DropDownList ID="DDDesign" runat="server" CssClass="dropdown" Width="300px"
                                    AutoPostBack="true" OnSelectedIndexChanged="DDDesign_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="Label6" Text="Color" runat="server" CssClass="labelbold" />
                            </td>
                            <td>
                                <asp:DropDownList ID="DDColor" runat="server" CssClass="dropdown" Width="300px" AutoPostBack="true"
                                    OnSelectedIndexChanged="DDColor_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="Label7" Text="Size" runat="server" CssClass="labelbold" />
                            </td>
                            <td>
                                <asp:DropDownList ID="DDSize" runat="server" CssClass="dropdown" Width="200px">
                                </asp:DropDownList>
                                <asp:CheckBox ID="Chkmtrsize" Text="Mtr Size" runat="server" AutoPostBack="true"
                                    CssClass="checkboxbold" OnCheckedChanged="Chkmtrsize_CheckedChanged" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:CheckBox ID="ChkselectDate" Text="Select Date" runat="server" CssClass="checkboxbold" />
                            </td>
                            <td>
                                <table>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblfromdt" Text="From Date" runat="server" CssClass="labelbold" />
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtfromDate" CssClass="textb" runat="server" Width="100px" />
                                            <asp:CalendarExtender ID="calfrom" runat="server" TargetControlID="txtfromDate" Format="dd-MMM-yyyy">
                                            </asp:CalendarExtender>
                                        </td>
                                        <td>
                                            <asp:Label ID="lbltodt" Text="To Date" runat="server" CssClass="labelbold" />
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txttodate" CssClass="textb" runat="server" Width="100px" />
                                            <asp:CalendarExtender ID="Caltodate" runat="server" TargetControlID="txttodate" Format="dd-MMM-yyyy">
                                            </asp:CalendarExtender>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2" align="right">
                                <asp:Button ID="btnpreview" Text="Preview" runat="server" CssClass="buttonnorm" OnClick="btnpreview_Click" />
                                <asp:Button ID="btnclose" Text="Close" runat="server" CssClass="buttonnorm" OnClientClick="CloseForm();" />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <asp:Label ID="lblmsg" Text="" runat="server" CssClass="labelbold" ForeColor="Red" />
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
