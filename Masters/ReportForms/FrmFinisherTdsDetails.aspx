<%@ Page Title="Finisher TDS Details" Language="C#" MasterPageFile="~/ERPmaster.master"
    AutoEventWireup="true" CodeFile="FrmFinisherTdsDetails.aspx.cs" Inherits="Masters_ReportForms_FrmFinisherTdsDetails" %>

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
                        <asp:Panel ID="pnl1" runat="server" Style="border: 0px Solid">
                            <table>
                                <tr>
                                    <td>
                                        &nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        &nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        &nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td id="A" runat="server">
                                        &nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td id="Td1" runat="server">
                                        &nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td id="Td2" runat="server">
                                        &nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        &nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        &nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td id="TDInternalBucket" runat="server">
                                        &nbsp;
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
                                        <asp:DropDownList ID="DDCompany" runat="server" CssClass="dropdown" Width="300px">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr id="trWeaver" runat="server">
                                    <td>
                                        <asp:Label ID="Label2" Text="Finisher/Contractor Name" runat="server" CssClass="labelbold" />
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="DDFinisher" runat="server" CssClass="dropdown" Width="300px">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <%-- <tr id="TRCustomerCode" runat="server" visible="true">
                                    <td>
                                        <asp:Label ID="Label1" Text="Customer Name" runat="server" CssClass="labelbold" />
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="DDCustomerName" runat="server" CssClass="dropdown" Width="300px">
                                        </asp:DropDownList>
                                    </td>
                                </tr>--%>
                                <tr id="TRProcess" runat="server" visible="true">
                                    <td>
                                        <asp:Label ID="Label1" Text="Process Name" runat="server" CssClass="labelbold" />
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="DDProcessName" runat="server" CssClass="dropdown" Width="300px">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr id="TRDeductionType" runat="server" visible="true">
                                    <td>
                                        <asp:Label ID="Label3" Text="Deduction Type" runat="server" CssClass="labelbold" />
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="DDDeductionType" runat="server" CssClass="dropdown" Width="300px">
                                            <asp:ListItem Value="0" Text="Both">Both</asp:ListItem>
                                            <asp:ListItem Value="1" Text="TDS">TDS</asp:ListItem>
                                            <asp:ListItem Value="2" Text="NonTDS">NonTDS</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td id="Tdselectdate" runat="server">
                                        <asp:CheckBox ID="ChkselectDate" Text="Select Date" runat="server" CssClass="checkboxbold"
                                            Checked="true" Enabled="false" />
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
                                                <td id="Tdtodatelabel" runat="server">
                                                    <asp:Label ID="lbltodt" Text="To Date" runat="server" CssClass="labelbold" />
                                                </td>
                                                <td id="Tdtodate" runat="server">
                                                    <asp:TextBox ID="txttodate" CssClass="textb" runat="server" Width="100px" />
                                                    <asp:CalendarExtender ID="Caltodate" runat="server" TargetControlID="txttodate" Format="dd-MMM-yyyy">
                                                    </asp:CalendarExtender>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td id="TDChkForsummary" runat="server" visible="false">
                                        <asp:CheckBox ID="ChkForSummary" Text="For Summary" runat="server" CssClass="checkboxbold" />
                                        <%-- <asp:CheckBox ID="CheckBox1" Text="For Name Wise" runat="server" CssClass="checkboxbold" AutoPostBack="true" OnCheckedChanged="chkNameWise_CheckedChanged" />--%>
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
