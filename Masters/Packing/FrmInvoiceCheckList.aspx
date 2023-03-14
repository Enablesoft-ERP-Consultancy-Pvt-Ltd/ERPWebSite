<%@ Page Language="C#" AutoEventWireup="true" CodeFile="FrmInvoiceCheckList.aspx.cs"
    Inherits="Masters_Packing_FrmInvoiceCheckList" MasterPageFile="~/ERPmaster.master"
    EnableEventValidation="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="CPH_Form" runat="server">
    <script src="../../Scripts/JScript.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery-1.4.1.js" type="text/javascript"></script>
    <%--<script type="text/javascript" src="../../Scripts/Fixfocus.js"></script>--%>
    <script type="text/javascript">
        function CloseForm() {

            window.location.href = "../../main.aspx";
        }
        function CheckAllCheckBoxes(header) {
            if (header.checked) {
                var gvcheck = document.getElementById('CPH_Form_gvforinvice');
                var i;
                for (i = 1; i < gvcheck.rows.length; i++) {
                    var inputs = gvcheck.rows[i].getElementsByTagName('input');
                    inputs[0].checked = true;
                }
            }
            else {
                var gvcheck = document.getElementById('CPH_Form_gvforinvice');
                var i;
                for (i = 1; i < gvcheck.rows.length; i++) {
                    var inputs = gvcheck.rows[i].getElementsByTagName('input');
                    inputs[0].checked = false;
                }
            }
        }
        function isnumber(evt) {
            var Charcode = (evt.which) ? evt.which : event.keycode
            if (Charcode != 46 && Charcode > 31 && (Charcode < 48 || Charcode > 57)) {
                alert('Plz Enter numeric value only...')
                return false;

            }
            else {
                return true;
            }

        }
        function validatesave() {
            if (document.getElementById("<%=DDCompany.ClientID %>").value <= "0") {
                alert("Plz Select Company Name");
                document.getElementById("<%=DDCompany.ClientID %>").focus();
                return false;
            }
            if (document.getElementById("<%=DDCustomerName.ClientID %>").value <= "0") {
                alert("Plz Select Customer Name");
                document.getElementById("<%=DDCustomerName.ClientID %>").focus();
                return false;

            }
            return confirm('Do You Want To Save?')
        }

    </script>
    <asp:UpdatePanel ID="updatepenl1" runat="server">
        <ContentTemplate>
            <div>
                <div style="width: 366px; margin-left: 250px">
                    <asp:Panel runat="server" ID="panel1" Style="border: 1px groove Teal; border: 5px solid #c8e5f6;"
                        Width="450px">
                        <div style="padding: 0px 0px 0px 20px">
                            <table>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblCompany" Text="CompanyName" runat="server" CssClass="labelnormalMM"></asp:Label>
                                    </td>
                                    <td>
                                        &nbsp;<asp:DropDownList ID="DDCompany" runat="server" Width="200px" CssClass="dropdown"
                                            AutoPostBack="true" OnSelectedIndexChanged="DDCompany_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                            </table>
                            <table>
                                <tr>
                                    <td>
                                        CustomerName
                                    </td>
                                    <td>
                                        &nbsp;<asp:DropDownList ID="DDCustomerName" runat="server" Width="200px" CssClass="dropdown"
                                            AutoPostBack="true" OnSelectedIndexChanged="DDCustomerName_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                            </table>
                            <div>
                                <asp:GridView ID="gvforinvice" runat="server" Width="300px" AutoGenerateColumns="false"
                                    OnRowCreated="gvforinvice_RowCreated" OnRowDataBound="gvforinvice_RowDataBound">
                                    <HeaderStyle ForeColor="white" BackColor="#0080C0" CssClass="gvheader" />
                                    <AlternatingRowStyle CssClass="gvalt" />
                                    <RowStyle CssClass="gvrow" />
                                    <Columns>
                                        <asp:TemplateField HeaderText="">
                                            <HeaderTemplate>
                                                <asp:CheckBox ID="Chkall" runat="server" onclick="return CheckAllCheckBoxes(this);" />
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:CheckBox ID="Chkbox" runat="server" />
                                            </ItemTemplate>
                                            <HeaderStyle Width="50px" />
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblid" runat="server" Text='<%#Bind("Id") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="Name" HeaderText="Type">
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:BoundField>
                                        <asp:TemplateField HeaderText="Number Of Copies">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtnoofcopies" runat="server" CssClass="textb" Width="75px" onkeypress="return isnumber(event);"></asp:TextBox>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </div>
                            <div style="margin-left: 200px;">
                                &nbsp;<asp:Button ID="Btnsave" runat="server" CssClass="buttonnorm" Text="Save" Width="50px"
                                    OnClick="Btnsave_Click" OnClientClick="return validatesave();" />
                                &nbsp;<asp:Button ID="BtnClose" runat="server" CssClass="buttonnorm" Text="Close"
                                    Width="50px" OnClientClick="return CloseForm();" />
                            </div>
                            <table>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblErrmsg" runat="server" CssClass="labelnormalMM" ForeColor="Red"></asp:Label>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </asp:Panel>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
