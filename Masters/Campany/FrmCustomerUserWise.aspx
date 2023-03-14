<%@ Page Title="" Language="C#" MasterPageFile="~/ERPmaster.master" AutoEventWireup="true"
    CodeFile="FrmCustomerUserWise.aspx.cs" Inherits="Masters_Campany_FrmCustomerUserWise" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content2" ContentPlaceHolderID="CPH_Form" runat="Server">
    <script src="../../Scripts/JScript.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery-1.4.1.min.js" type="text/javascript"></script>
    <%--<script type="text/javascript" src="../../Scripts/Fixfocus.js"></script>--%>
    <script type="text/javascript">
        function CloseForm() {
            window.location.href = "../../main.aspx"
        }
        function addpriview() {
            window.open("../../ReportViewer.aspx")
        }

        function CheckAllCheckBoxes() {
            if (document.getElementById('CPH_Form_ChkForAllCustomer').checked == true) {
                var gvcheck = document.getElementById('CPH_Form_DGcustomer');
                var i;
                for (i = 1; i < gvcheck.rows.length; i++) {
                    var inputs = gvcheck.rows[i].getElementsByTagName('input');
                    inputs[0].checked = true;
                }
            }
            else {
                var gvcheck = document.getElementById('CPH_Form_DGcustomer');
                var i;
                for (i = 1; i < gvcheck.rows.length; i++) {
                    var inputs = gvcheck.rows[i].getElementsByTagName('input');
                    inputs[0].checked = false;
                }
            }
        }
        function ValidationSave() {
            if (document.getElementById('CPH_Form_ddemp').options[document.getElementById('CPH_Form_ddemp').selectedIndex].value == 0) {
                alert("Please select employee name ....!");
                document.getElementById("CPH_Form_ddemp").focus();
                return false;
            }
            var k = 0;
            for (i = 1; i < document.getElementById('CPH_Form_DGcustomer').rows.length; i++) {
                var inputs = document.getElementById('CPH_Form_DGcustomer').rows[i].getElementsByTagName('input');
                if (inputs[0].checked == true) {
                    k = k + 1;
                    i = document.getElementById('CPH_Form_DGcustomer').rows.length;
                }
            }
            if (k == 0) {
                alert("Please select atleast one check box....!");
                return false;
            }
            return confirm('Do You Want To Save?')
        }
    </script>
    <table>
        <tr>
            <td>
                <asp:Label ID="lblempname" runat="server" Text="Employee Name"></asp:Label>
            </td>
            <td>
                &nbsp;&nbsp;&nbsp;&nbsp;
                <asp:DropDownList ID="ddemp" CssClass="dropdown" runat="server" Width="145px" OnSelectedIndexChanged="ddemp_SelectedIndexChanged"
                    AutoPostBack="true">
                </asp:DropDownList>
                <cc1:ListSearchExtender ID="ListSearchExtender2" runat="server" TargetControlID="ddemp"
                    ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                </cc1:ListSearchExtender>
            </td>
        </tr>
        <tr>
            <td>
                <asp:CheckBox ID="ChkForAllCustomer" runat="server" Text=" Chk For All Customer"
                    ForeColor="Red" onclick="return CheckAllCheckBoxes();" CssClass="checkboxbold" />
            </td>
        </tr>
        <tr align="center" id="trdrig" runat="server">
            <td align="center" colspan="2">
                <div style="width: 400px; height: 400px; overflow: scroll">
                    <asp:GridView ID="DGcustomer" runat="server" DataKeyNames="CustomerId" AutoGenerateColumns="False"
                        CssClass="grid-view" OnRowCreated="DGcustomer_RowCreated">
                        <AlternatingRowStyle BackColor="White" />
                        <Columns>
                            <asp:TemplateField HeaderText="">
                                <ItemTemplate>
                                    <asp:CheckBox ID="Chkbox" runat="server" />
                                    <%-- <asp:CheckBox ID="CheckBox1" runat="server" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>" />--%>
                                </ItemTemplate>
                                <HeaderStyle Width="80px" />
                            </asp:TemplateField>
                            <asp:BoundField DataField="CustomerCode" HeaderText="Customer Code" />
                            <asp:BoundField DataField="CustomerName" HeaderText="Customer Name" />
                        </Columns>
                        <EmptyDataRowStyle CssClass="gvemptytext" />
                    </asp:GridView>
                </div>
            </td>
        </tr>
        <tr align="center">
            <td>
                <asp:Button CssClass="buttonnorm" ID="BtnSave" Text="Submmit" runat="server" OnClick="BtnSave_Click"
                    OnClientClick="return ValidationSave();" />
            </td>
        </tr>
    </table>
</asp:Content>
