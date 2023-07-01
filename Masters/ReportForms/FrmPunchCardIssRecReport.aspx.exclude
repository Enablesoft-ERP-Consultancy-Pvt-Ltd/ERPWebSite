<%@ Page Title="Punch Card Iss Rec Report" Language="C#" MasterPageFile="~/ERPmaster.master"
    AutoEventWireup="true" CodeFile="FrmPunchCardIssRecReport.aspx.cs" Inherits="Masters_ReportForms_FrmPunchCardIssRecReport" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content2" ContentPlaceHolderID="CPH_Form" runat="Server">
    <script src="../../Scripts/JScript.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery-1.4.1.js" type="text/javascript"></script>
    <script type="text/javascript">
        function CloseForm() {
            window.location.href = "../../main.aspx";
        }   
    </script>
    <asp:UpdatePanel ID="UPD1" runat="server">
        <ContentTemplate>
            <div style="margin: 2% 20% 0% 5%;">
                <div style="width: 100%">
                    <div style="float: left; width: 30%">
                        <asp:Panel ID="pnl1" runat="server" Style="border: 1px Solid">
                            <table>
                                <%--<tr>
                                    <td>
                                        <asp:RadioButton ID="RDAll" Text="All" runat="server" CssClass="radiobuttonnormal"
                                            GroupName="A" AutoPostBack="true" OnCheckedChanged="RDAll_CheckedChanged" />
                                    </td>
                                </tr>--%>
                                <tr>
                                    <td>
                                        <asp:RadioButton ID="RDPunchCardIssueDetail" Text="PunchCard Issue Detail" runat="server" CssClass="radiobuttonnormal"
                                            GroupName="A" AutoPostBack="true" OnCheckedChanged="RDPunchCardIssueDetail_CheckedChanged" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:RadioButton ID="RDPunchCardReceiveDetail" Text="PunchCard Receive Detail" runat="server" CssClass="radiobuttonnormal"
                                            GroupName="A" AutoPostBack="true" OnCheckedChanged="RDPunchCardReceiveDetail_CheckedChanged" />
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                    </div>
                    <div style="float: right; width: 70%">
                        <asp:Panel ID="pnl2" runat="server" Style="border: 1px Solid">
                            <table>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblcomp" Text="Company Name" runat="server" CssClass="labelbold" />
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="DDCompany" runat="server" CssClass="dropdown" Width="300px"
                                            AutoPostBack="True" OnSelectedIndexChanged="DDCompany_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="Label11" Text="Process Name" runat="server" CssClass="labelbold" />
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="DDProcessName" runat="server" CssClass="dropdown" Width="300px"
                                        AutoPostBack="True" OnSelectedIndexChanged="DDProcessName_SelectedIndexChanged">
                                        </asp:DropDownList >
                                    </td>
                                </tr>
                                <tr id="TRCustomerCode" runat="server">
                                    <td>
                                        <asp:Label ID="Label12" runat="server" Text="CustomerCode" CssClass="labelbold"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="DDCustCode" runat="server" Width="300px" CssClass="dropdown"
                                            AutoPostBack="True" OnSelectedIndexChanged="DDCustCode_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr id="TROrderNo" runat="server">
                                    <td>
                                        <asp:Label ID="Label13" runat="server" Text="OrderNo" CssClass="labelbold"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="DDOrderNo" runat="server" Width="300px" CssClass="dropdown">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr id="trWeaverName" runat="server">
                                    <td>
                                        <asp:Label ID="Label2" Text="EmpName" runat="server" CssClass="labelbold" />
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="DDWeaverName" runat="server" CssClass="dropdown" Width="300px"
                                            AutoPostBack="true" OnSelectedIndexChanged="DDWeaverName_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr id="TRFolioNo" runat="server">
                                    <td>
                                        <asp:Label ID="Label14" Text="FolioNo" runat="server" CssClass="labelbold" />
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="DDPOrderNo" runat="server" CssClass="dropdown" Width="300px"
                                            AutoPostBack="true" OnSelectedIndexChanged="DDPOrderNo_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </td>
                                </tr>

                                <tr id="trChallanNo" runat="server">
                                    <td>
                                        <asp:Label ID="Label3" Text="Challan No." runat="server" CssClass="labelbold" />
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="DDChallanNo" runat="server" CssClass="dropdown" Width="300px"
                                            AutoPostBack="true" OnSelectedIndexChanged="DDChallanNo_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                               <%-- <tr id="TRMapType" runat="server" visible="true">
                                    <td>
                                        <asp:Label ID="Label11" Text="Map Type" runat="server" CssClass="labelbold" />
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="DDMapStencilType" runat="server" CssClass="dropdown" Width="100px"
                                            Enabled="true">
                                            <asp:ListItem Value="1" Text="Map" Selected="True">Map</asp:ListItem>
                                            <asp:ListItem Value="2" Text="Trace">Trace</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>--%>
                                <tr id="trCategoryName" runat="server" visible="false">
                                    <td>
                                        <asp:Label ID="Label8" Text="Category Name" runat="server" CssClass="labelbold" />
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="DDCategory" runat="server" CssClass="dropdown" Width="300px"
                                            AutoPostBack="true" OnSelectedIndexChanged="DDCategory_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr id="trItemName" runat="server" visible="false">
                                    <td>
                                        <asp:Label ID="Label4" Text="Item Name" runat="server" CssClass="labelbold" />
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="DDItemName" runat="server" CssClass="dropdown" Width="300px"
                                            AutoPostBack="true" OnSelectedIndexChanged="DDItemName_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr id="Trquality" runat="server" visible="false">
                                    <td>
                                        <asp:Label ID="Label9" Text="Quality" runat="server" CssClass="labelbold" />
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="DDQuality" runat="server" CssClass="dropdown" Width="300px"
                                            AutoPostBack="true" OnSelectedIndexChanged="DDQuality_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr id="Trdesign" runat="server" visible="false">
                                    <td>
                                        <asp:Label ID="Label5" Text="Design" runat="server" CssClass="labelbold" />
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="DDDesign" runat="server" CssClass="dropdown" Width="300px"
                                            AutoPostBack="true" OnSelectedIndexChanged="DDDesign_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr id="Trcolor" runat="server" visible="false">
                                    <td>
                                        <asp:Label ID="Label6" Text="Color" runat="server" CssClass="labelbold" />
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="DDColor" runat="server" CssClass="dropdown" Width="300px" AutoPostBack="true"
                                            OnSelectedIndexChanged="DDColor_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr id="TrShape" runat="server" visible="false">
                                    <td>
                                        <asp:Label ID="Label1" Text="Shape" runat="server" CssClass="labelbold" />
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="DDShape" runat="server" CssClass="dropdown" Width="300px" AutoPostBack="true"
                                            OnSelectedIndexChanged="DDShape_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr id="Trsize" runat="server" visible="false">
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
                                <tr id="Trshadecolor" runat="server" visible="false">
                                    <td>
                                        <asp:Label ID="Label10" Text="Shadecolor" runat="server" CssClass="labelbold" />
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="DDshade" runat="server" CssClass="dropdown" Width="300px">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblfromdt" Text="From Date" runat="server" CssClass="labelbold" />
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtfromDate" CssClass="textb" runat="server" Width="100px" />
                                        <asp:CalendarExtender ID="calfrom" runat="server" TargetControlID="txtfromDate" Format="dd-MMM-yyyy">
                                        </asp:CalendarExtender>
                                    </td>
                                </tr>
                                <tr>
                                    <td id="Tdtodatelabel" runat="server">
                                        <asp:Label ID="lbltodt" Text="To Date" runat="server" CssClass="labelbold" />
                                    </td>
                                    <td id="Tdtodate" runat="server">
                                        <asp:TextBox ID="txttodate" CssClass="textb" runat="server" Width="100px" />
                                        <asp:CalendarExtender ID="Caltodate" runat="server" TargetControlID="txttodate" Format="dd-MMM-yyyy">
                                        </asp:CalendarExtender>
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
                </div>
            </div>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="btnpreview" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
