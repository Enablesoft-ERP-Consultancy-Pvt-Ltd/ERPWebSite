<%@ Page Language="C#" AutoEventWireup="true" CodeFile="frmJobPaymentSummary.aspx.cs"
    Inherits="Masters_ReportForms_frmJobPaymentSummary" MasterPageFile="~/ERPmaster.master"
    Title="JOB PAYMENT SUMMARY" %>

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
    </script>
    <asp:UpdatePanel ID="updatpanel1" runat="server">
        <ContentTemplate>
            <div style="width: 1000px; height: 1000px;">
                <div style="width: 900px; height: 1000px">
                    <div style="width: 391px; margin-left: 300px; height: 146px; margin-top: 20px">
                        <asp:Panel runat="server" ID="panel1" Style="border: 1px groove Teal; background-color: #DEB887;"
                            Width="370px">
                            <div style="padding-left: 20px">
                                <table style="width: 335px; height: 158px;">
                                    <tr id="Tr1">
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
                                    <tr>
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
                                    <tr>
                                        <td colspan="2" align="right" runat="server" visible="false">
                                            <asp:CheckBox ID="chkForSummary" runat="server" Text=" For Summary" Font-Bold="true" />
                                        </td>
                                        <td id="TDForWeaverWiseDetail" colspan="2" align="right" runat="server" visible="false">
                                            <asp:CheckBox ID="ChkForWeaverWiseDetail" runat="server" Text=" For Weaver Wise Detail" Font-Bold="true" />
                                        </td>
                                        <td align="right" colspan="5">
                                            <asp:Button ID="btnprint" runat="server" CssClass="buttonnorm" Text="Print" Width="50px"
                                                OnClick="btnprint_Click" />
                                            <asp:Button ID="btnclose" runat="server" CssClass="buttonnorm" Text="Close" Width="50px"
                                                OnClientClick="return CloseForm();" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblErrorMsg" runat="server" Text="" CssClass="dropdown" ForeColor="Red"></asp:Label>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </asp:Panel>
                    </div>
                </div>
            </div>
        </ContentTemplate>
         <Triggers>
            <asp:PostBackTrigger ControlID="btnprint" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
