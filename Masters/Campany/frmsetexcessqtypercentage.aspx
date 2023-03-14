<%@ Page Title="set percentage" Language="C#" MasterPageFile="~/ERPmaster.master"
    AutoEventWireup="true" CodeFile="frmsetexcessqtypercentage.aspx.cs" Inherits="Masters_Campany_frmsetexcessqtypercentage" %>

<asp:Content ID="Content2" ContentPlaceHolderID="CPH_Form" runat="Server">
    <script type="text/javascript">
        function CloseForm() { window.location.href = "../../main.aspx"; }
        function isNumberKey(evt) {
            var charCode = (evt.which) ? evt.which : event.keyCode
            if (charCode != 46 && charCode > 31 && (charCode < 48 || charCode > 57)) {
                return false;
            }
            else {
                return true;
            }
        }
    </script>
    <asp:UpdatePanel ID="up1" runat="server">
        <ContentTemplate>
            <div style="margin: 0% 30% 0% 30%">
                <table border="1" cellpadding="10" cellspacing="5">
                    <tr>
                        <td>
                            <asp:Label Text="Excess qty Percentage For" runat="server" CssClass="labelbold" />
                        </td>
                        <td>
                            <asp:DropDownList ID="ddpercentageexcessqtyfor" CssClass="dropdown" runat="server"
                                Width="175px" AutoPostBack="true" OnSelectedIndexChanged="ddpercentageexcessqtyfor_SelectedIndexChanged">
                                <asp:ListItem Text="" Value="0" />
                                <asp:ListItem Text="Purchase Receive" Value="1" />
                                <asp:ListItem Text="Generate Indent" Value="2" />
                                <asp:ListItem Text="Indent Receive" Value="3" />
                                <asp:ListItem Text="Process Raw Issue" Value="4" />
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr id="TrPPNo" runat="server" visible="false" class="tdstyle">
                        <td>
                            <asp:Label ID="Label6" runat="server" Text="PPNo" CssClass="labelbold"></asp:Label>
                            &nbsp;&nbsp;
                            <asp:CheckBox ID="ChkForAllPPNo" runat="server" AutoPostBack="true" Text="All PPNo"
                                OnCheckedChanged="ChkForAllPPNo_CheckedChanged" />
                        </td>
                        <td>
                            <asp:DropDownList CssClass="dropdown" ID="DDProcessProgramNo" Width="175px" runat="server"
                                AutoPostBack="true" OnSelectedIndexChanged="DDProcessProgramNo_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label Text="value(%)" runat="server" CssClass="labelbold" />
                        </td>
                        <td>
                            <asp:TextBox ID="txtpercentage" CssClass="textb" Width="160px" runat="server" onkeypress="return isNumberKey(event);" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2" align="right">
                            <asp:Button Text="Update value" ID="btnupdate" runat="server" CssClass="buttonnorm"
                                OnClick="btnupdate_Click" />
                            <asp:Button Text="Close" ID="btnclose" CssClass="buttonnorm" runat="server" OnClientClick="return CloseForm();" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <asp:Label ID="lblmsg" CssClass="labelbold" ForeColor="Red" Text="" runat="server" />
                        </td>
                    </tr>
                </table>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
