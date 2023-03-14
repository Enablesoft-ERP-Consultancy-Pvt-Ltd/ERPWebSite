<%@ Page Language="C#" AutoEventWireup="true" CodeFile="frmReportForm_ProductionCard.aspx.cs"
    Inherits="Masters_ReportForms_frmReportFormJob_ProductionCard" MasterPageFile="~/ERPmaster.master"
    Title="PRODUCTION CARD" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="page" ContentPlaceHolderID="CPH_Form" runat="server">
    <script src="../../Scripts/JScript.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery-1.4.1.js" type="text/javascript"></script>
    <%--<script type="text/javascript" src="../../Scripts/Fixfocus.js"></script>--%>
    <script type="text/javascript">
        function Preview() {
            window.open('../../reportViewer.aspx', '');
        }
        function CloseForm() {
            window.location.href = "../../main.aspx";
        }
        function validate() {
            //            if (document.getElementById('CPH_Form_DDjob').selectedIndex <= "0") {
            //                alert('Plz Select Job...')
            //                document.getElementById('CPH_Form_DDjob').focus()
            //                return false;
            //            }
            if (document.getElementById('CPH_Form_txtIdNo').value == "") {
                alert('Plz Enter ID No...')
                document.getElementById('CPH_Form_txtIdNo').focus()
                return false;
            }
            if (document.getElementById('CPH_Form_DDMonth').selectedIndex < "0") {
                alert('Plz Select Month...')
                document.getElementById('CPH_Form_DDMonth').focus()
                return false;
            }
            if (document.getElementById('CPH_Form_DDYear').selectedIndex < "0") {
                alert('Plz Select Year...')
                document.getElementById('CPH_Form_DDYear').focus()
                return false;
            }
        }
        function isnumeric(evt) {
            var charcode = (evt.which) ? evt.which : evt.keycode
            if (charcode > 31 && (charcode < 48 || charcode > 57)) {
                alert('Plz Enter Numeric Value Only')
                return false
            }
            else {
                return true
            }
        }
    </script>
    <asp:UpdatePanel ID="updatpanel1" runat="server">
        <ContentTemplate>
            <div>
                <div style="margin: 1% 20% 0% 20%">
                    <div>
                        <asp:Panel runat="server" ID="panel1" Style="border: 1px groove Teal; background-color: #DEB887;
                            max-height: 400px" Width="370px">
                            <div>
                                <table style="width: 100%">
                                    <tr runat="server" visible="false">
                                        <td>
                                            <asp:Label ID="lbljob" runat="server" Text="Job" CssClass="labelbold"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="DDjob" runat="server" Width="150px" CssClass="dropdown">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblIdNo" runat="server" Text="Enter ID No." CssClass="labelbold"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtIdNo" runat="server" Width="150px" CssClass="textb" Height="23px"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr id="TRfromtoDate" runat="server" visible="false">
                                        <td>
                                            <asp:Label ID="lblfrom" CssClass="labelbold" Text="From Date" runat="server" />
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtfromdate" CssClass="textb" Width="100px" runat="server" />
                                            <asp:CalendarExtender ID="calfrom" TargetControlID="txtfromdate" Format="dd-MMM-yyyy"
                                                runat="server">
                                            </asp:CalendarExtender>
                                        </td>
                                        <td>
                                            <asp:Label ID="Label2" CssClass="labelbold" Text="To Date" runat="server" />
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txttodate" CssClass="textb" Width="100px" runat="server" />
                                            <asp:CalendarExtender ID="CalendarExtender1" TargetControlID="txttodate" Format="dd-MMM-yyyy"
                                                runat="server">
                                            </asp:CalendarExtender>
                                        </td>
                                    </tr>
                                    <tr id="Trmonthyear" runat="server" visible="false">
                                        <td>
                                            <asp:Label ID="lblDate" runat="server" Text="Month" CssClass="labelbold"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="DDMonth" runat="server" CssClass="dropdown" Width="100px">
                                                <asp:ListItem Value="1">JAN</asp:ListItem>
                                                <asp:ListItem Value="2">FEB</asp:ListItem>
                                                <asp:ListItem Value="3">MAR</asp:ListItem>
                                                <asp:ListItem Value="4">APR</asp:ListItem>
                                                <asp:ListItem Value="5">MAY</asp:ListItem>
                                                <asp:ListItem Value="6">JUN</asp:ListItem>
                                                <asp:ListItem Value="7">JUL</asp:ListItem>
                                                <asp:ListItem Value="8">AUG</asp:ListItem>
                                                <asp:ListItem Value="9">SEP</asp:ListItem>
                                                <asp:ListItem Value="10">OCT</asp:ListItem>
                                                <asp:ListItem Value="11">NOV</asp:ListItem>
                                                <asp:ListItem Value="12">DEC</asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblyear" runat="server" Text="Year" CssClass="labelbold"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="DDyear" runat="server" CssClass="dropdown" widh="100px">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr id="Tradvamount" runat="server" visible="false">
                                        <td>
                                            <asp:Label ID="lblAdvanceAmt" runat="server" Text="Adv. Amount" CssClass="labelbold"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtAdvanceAmt" runat="server" CssClass="textb" Width="150px" Height="23px"
                                                onkeypress="return isnumeric(event);"></asp:TextBox>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblAdvDate" runat="server" Text="Date" CssClass="labelbold"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtAdvancedate" runat="server" Width="100px" CssClass="textb" Height="23px"
                                                BackColor="Beige"></asp:TextBox>
                                            <asp:CalendarExtender ID="calAdvance" runat="server" TargetControlID="txtAdvancedate"
                                                Format="dd-MMM-yyyy">
                                            </asp:CalendarExtender>
                                        </td>
                                    </tr>
                                    <tr id="tradvremark" runat="server" visible="false">
                                        <td>
                                            <asp:Label ID="Label1" runat="server" Text="Adv. Amount Remark" CssClass="labelbold"></asp:Label>
                                        </td>
                                        <td colspan="3">
                                            <asp:TextBox ID="txtadvremark" CssClass="textb" runat="server" Width="280px" TextMode="MultiLine"
                                                Height="44px" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2" align="right">
                                            <asp:CheckBox ID="chksummary" runat="server" Text=" For Summary" Font-Bold="true" />
                                        </td>
                                        <td align="right" colspan="3">
                                            <asp:Button ID="btnprint" runat="server" CssClass="buttonnorm" Text="Print" Width="50px"
                                                OnClientClick="return validate();" OnClick="btnprint_Click" />
                                            <asp:Button ID="btnclose" runat="server" CssClass="buttonnorm" Text="Close" Width="50px"
                                                OnClientClick="return CloseForm();" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="4">
                                            <asp:Label ID="lblErrorMsg" runat="server" Text="" CssClass="labelbold" ForeColor="Red"></asp:Label>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </asp:Panel>
                    </div>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
