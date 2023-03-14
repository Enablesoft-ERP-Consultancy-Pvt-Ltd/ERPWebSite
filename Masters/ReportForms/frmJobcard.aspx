<%@ Page Language="C#" AutoEventWireup="true" CodeFile="frmJobcard.aspx.cs" Inherits="Masters_ReportForms_frmJobcard"
    MasterPageFile="~/ERPmaster.master" Title="JOB CARD" %>

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
            if (document.getElementById('CPH_Form_DDjob').selectedIndex <= "0") {
                alert('Plz Select Job...')
                document.getElementById('CPH_Form_DDjob').focus()
                return false;
            }
            if (document.getElementById('CPH_Form_txtIdNo').value == "") {
                alert('Plz Enter ID No...')
                document.getElementById('CPH_Form_txtIdNo').focus()
                return false;
            }
            //            if (document.getElementById('CPH_Form_DDMonth').selectedIndex < "0") {
            //                alert('Plz Select Month...')
            //                document.getElementById('CPH_Form_DDMonth').focus()
            //                return false;
            //            }
            //            if (document.getElementById('CPH_Form_DDYear').selectedIndex < "0") {
            //                alert('Plz Select Year...')
            //                document.getElementById('CPH_Form_DDYear').focus()
            //                return false;
            //            }
        }
    </script>
    <asp:UpdatePanel ID="updatpanel1" runat="server">
        <ContentTemplate>
            <div style="width: 100%; margin: 1% 20% 0% 20%">
                <table border="1" cellpadding="0" cellspacing="1" style="width: 50%">
                    <tr>
                        <td style="width: 30%">
                            <asp:Label ID="lbljob" runat="server" Text="Job" CssClass="labelbold"></asp:Label>
                        </td>
                        <td style="width: 70%">
                            <asp:DropDownList ID="DDjob" runat="server" Width="90%" CssClass="dropdown">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 30%">
                            <asp:Label ID="lblIdNo" runat="server" Text="Enter ID No." CssClass="labelbold"></asp:Label>
                        </td>
                        <td style="width: 70%">
                            <asp:TextBox ID="txtIdNo" runat="server" Width="90%" CssClass="textb" Height="23px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 30%">
                            <asp:Label ID="lblDate" runat="server" Text="From Date" CssClass="labelbold"></asp:Label>
                        </td>
                        <td style="width: 70%">
                            <asp:TextBox ID="txtfromDate" CssClass="textb" runat="server" Width="50%" autocomplete="off" />
                            <asp:CalendarExtender ID="calfrom" runat="server" TargetControlID="txtfromDate" Format="dd-MMM-yyyy">
                            </asp:CalendarExtender>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 30%">
                            <asp:Label ID="lblyear" runat="server" Text="To Date" CssClass="labelbold"></asp:Label>
                        </td>
                        <td style="width: 70%">
                            <asp:TextBox ID="txttodate" CssClass="textb" runat="server" Width="50%" OnTextChanged="txttodate_TextChanged"
                                AutoPostBack="true" autocomplete="off" />
                            <asp:CalendarExtender ID="Caltodate" runat="server" TargetControlID="txttodate" Format="dd-MMM-yyyy">
                            </asp:CalendarExtender>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <asp:CheckBox ID="chkpaymentslip" CssClass="checkboxbold" Text="For Payment Slip"
                                runat="server" AutoPostBack="true" OnCheckedChanged="chkpaymentslip_CheckedChanged" />
                        </td>
                    </tr>
                    <tr runat="server" id="TDslipno" visible="false">
                        <td style="width: 30%">
                            <asp:Label ID="lblslipno" CssClass="labelbold" Text="Slip No." runat="server" />
                        </td>
                        <td style="width: 70%">
                            <asp:DropDownList ID="DDslipNo" CssClass="dropdown" runat="server" Width="90%" AutoPostBack="true"
                                OnSelectedIndexChanged="DDslipNo_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:CheckBox ID="chkForSummary" runat="server" Text=" For Summary" Font-Bold="true" />
                        </td>
                        <td>
                            <asp:Button ID="btnprint" runat="server" CssClass="buttonnorm" Text="Print" Width="50px"
                                OnClientClick="return validate();" OnClick="btnprint_Click" />
                            <asp:Button ID="btnclose" runat="server" CssClass="buttonnorm" Text="Close" Width="50px"
                                OnClientClick="return CloseForm();" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <asp:Label ID="lblErrorMsg" runat="server" Text="" CssClass="labelbold" ForeColor="Red"
                                Font-Size="Small"></asp:Label>
                        </td>
                    </tr>
                </table>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
