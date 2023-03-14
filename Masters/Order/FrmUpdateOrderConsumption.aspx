<%@ Page Language="C#" AutoEventWireup="true" CodeFile="FrmUpdateOrderConsumption.aspx.cs"
    Inherits="Masters_Order_FrmUpdateOrderConsumption" MasterPageFile="~/ERPmaster.master"
    Title="Update Order Consumption" %>

<asp:Content ID="Page" ContentPlaceHolderID="CPH_Form" runat="server">
    <script src="../../Scripts/JScript.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery-1.4.1.js" type="text/javascript"></script>
    <script type="text/javascript">
        function validate() {
            var message = "";
            if ($("#CPH_Form_ddcustomercode")) {
                var selectindex = $("#CPH_Form_ddcustomercode").attr('selectedIndex');
                if (selectindex <= 0) {
                    message = message + "Please,select customer code!!!\n";
                }
            }
            if ($("#CPH_Form_ddOrderNo")) {
                var selectindex = $("#CPH_Form_ddOrderNo").attr('selectedIndex');
                if (selectindex <= 0) {
                    message = message + "Please,select order no !!!\n";
                }
            }

            if (message == "") {
                return true;
            }
            else {
                alert(message);
                return false;
            }
        }
    </script>
    <div>
        <asp:UpdatePanel ID="updat1" runat="server">
            <ContentTemplate>
                <%--    <script type="text/javascript" language="javascript">
                    Sys.Application.add_load(validate);
                </script>--%>
                <div style="width: 50%; margin: auto">
                    <table style="width: 100%">
                        <tr>
                            <td>
                                <span class="labelbold">CompanyName</span>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddcompany" runat="server" CssClass="dropdown" Width="200px">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <span class="labelbold">Customer Code</span>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddcustomercode" runat="server" CssClass="dropdown" Width="200px"
                                    OnSelectedIndexChanged="ddcustomercode_SelectedIndexChanged" AutoPostBack="true">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <span class="labelbold">Order No.</span>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddOrderNo" runat="server" CssClass="dropdown" Width="200px">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td>
                            </td>
                            <td>
                                <asp:Button ID="btnupdateallconsmp" Text="Update All Consumption" runat="server"
                                    CssClass="buttonnorm" OnClick="btnupdateallconsmp_Click" OnClientClick="return validate();" />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <asp:Label Text="" ID="lblmsg" runat="server" CssClass="lblcss" ForeColor="Red" />
                            </td>
                        </tr>
                    </table>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</asp:Content>
