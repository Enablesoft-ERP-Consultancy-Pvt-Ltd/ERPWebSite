<%@ Page Title="Weaving Report" Language="C#" MasterPageFile="~/ERPmaster.master"
    AutoEventWireup="true" CodeFile="FrmDepartmentDetailReport.aspx.cs" Inherits="Masters_ReportForms_FrmDepartmentDetailReport" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content2" ContentPlaceHolderID="CPH_Form" runat="Server">
    <script src="../../Scripts/JScript.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery-1.4.1.js" type="text/javascript"></script>
    <script type="text/javascript">
        function CloseForm() {
            window.location.href = "../../main.aspx";
        }            
    </script>
    <script runat="server">
        protected void RadioButton_CheckedChanged(object sender, System.EventArgs e)
        {
            //TDChampoExternalWeaverConsumption.Visible = true;
            //lblfromdt.Text = "From Date";
            //Tdtodate.Visible = true;
            //Tdtodatelabel.Visible = true;
            //Tdselectdate.Visible = true;

            //trProductionStatus.Visible = true;
            //TRCustomerCode.Visible = true;
            //TROrderNo.Visible = true;
            if (RDDepartmentRawIssueDetail.Checked == true)
            {
                TRCompanyName.Visible = true;
                TRCustomerCode.Visible = true;
                TROrderNo.Visible = true;
                trWeaver.Visible = true;
                trFolioNo.Visible = true;
                Tdselectdate.Visible = true;
            }
        }
    </script>
    <asp:UpdatePanel ID="UPD1" runat="server">
        <ContentTemplate>
            <div style="margin: 2% 20% 0% 5%;">
                <div style="width: 100%">
                    <div style="float: left; width: 30%">
                        <asp:Panel ID="pnl1" runat="server" Style="border: 1px Solid">
                            <table>
                                <tr>
                                    <td id="TDDepartmentRawIssueDetail" runat="server">
                                        <asp:RadioButton ID="RDDepartmentRawIssueDetail" Text="Department Raw Issue Detail"
                                            runat="server" CssClass="radiobuttonnormal" GroupName="A" AutoPostBack="true"
                                            OnCheckedChanged="RadioButton_CheckedChanged" />
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                    </div>
                    <div style="float: right; width: 70%">
                        <asp:Panel ID="pnl2" runat="server" Style="border: 1px Solid">
                            <table>
                                <tr id="TRCompanyName" runat="server" visible="true">
                                    <td>
                                        <asp:Label ID="lblcomp" Text="Company Name" runat="server" CssClass="labelbold" />
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="DDCompany" runat="server" CssClass="dropdown" Width="300px">
                                        </asp:DropDownList>
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
                                <tr id="trWeaver" runat="server">
                                    <td>
                                        <asp:Label ID="Label2" Text="Department Name" runat="server" CssClass="labelbold" />
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="DDDepartmentName" runat="server" CssClass="dropdown" Width="300px"
                                            AutoPostBack="true" OnSelectedIndexChanged="DDDepartmentName_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr id="trFolioNo" runat="server">
                                    <td>
                                        <asp:Label ID="Label3" Text="Issue No." runat="server" CssClass="labelbold" />
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="DDIssueNo" runat="server" CssClass="dropdown" Width="300px">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr id="TrSelectDate" runat="server" visible="true">
                                    <td id="Tdselectdate" runat="server">
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
