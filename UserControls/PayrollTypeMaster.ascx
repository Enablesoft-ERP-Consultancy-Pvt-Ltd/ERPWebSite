<%@ Control Language="C#" AutoEventWireup="true" CodeFile="PayrollTypeMaster.ascx.cs"
    Inherits="UserControls_PayrollTypeMaster" %>
<script src="../../Scripts/JScript.js" type="text/javascript"></script>
<script src="../../Scripts/jquery-1.4.1.js" type="text/javascript"></script>
<link href="../../Styles/vijay.css" rel="stylesheet" type="text/css" />
<script type="text/javascript">
    function closeForm() {

        var objParent = window.opener;
        if (objParent != null) {
            if (window.opener.document.getElementById('EmployeeUserControl_refreshgrouponemployee')) {
                window.opener.document.getElementById('EmployeeUserControl_refreshgrouponemployee').click();
                self.close();
            }
            else if (window.opener.document.getElementById('CPH_Form_refreshpayrolltypemaster')) {
                window.opener.document.getElementById('CPH_Form_refreshpayrolltypemaster').click();
                self.close();
            }

        }
        else {
            window.location.href = "../../main.aspx";
        }



    }
</script>
<asp:UpdatePanel ID="upd1" runat="server">
    <ContentTemplate>
        <div>
            <table border="1">
                <tr>
                    <td>
                        <asp:Label ID="lblPtype" Text="Payroll Type" CssClass="labelbold" runat="server" />
                    </td>
                    <td>
                        <asp:TextBox ID="txtpayrolltype" Width="300px" runat="server" CssClass="textb" />
                        <asp:RequiredFieldValidator ID="Rf" ControlToValidate="txtpayrolltype" runat="server"
                            ErrorMessage="*" ForeColor="Red" ValidationGroup="a"></asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td colspan="2" align="right">
                        <asp:Button ID="btnsave" Text="Save" runat="server" CssClass="buttonnorm" OnClick="btnsave_Click"
                            ValidationGroup="a" />
                        <asp:Button ID="btnclose" Text="Close" runat="server" CssClass="buttonnorm" OnClientClick="closeForm();" />
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <asp:Label ID="lblmsg" Text="" runat="server" CssClass="labelbold" ForeColor="Red" />
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <div style="max-height: 300px; overflow: auto">
                            <asp:GridView ID="GDDetail" runat="server" AutoGenerateColumns="false" EmptyDataText="No Records found.."
                                CssClass="grid-views">
                                <HeaderStyle CssClass="gvheaders" />
                                <AlternatingRowStyle CssClass="gvalts" />
                                <RowStyle CssClass="gvrow" />
                                <EmptyDataRowStyle CssClass="gvemptytext" />
                                <Columns>
                                    <asp:TemplateField HeaderText="Sr No.">
                                        <ItemTemplate>
                                            <%#Container.DataItemIndex+1 %>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Payroll Type">
                                        <ItemTemplate>
                                            <asp:Label ID="lblpayrolltype" Text='<%#Bind("payrolltypename") %>' runat="server"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lnkupdate" runat="server" Text="Update" CausesValidation="false"
                                                OnClick="lnkupdate"></asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblpayrolltypeid" Text='<%#Bind("payrolltypeid") %>' runat="server"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </div>
                    </td>
                </tr>
            </table>
        </div>
        <asp:HiddenField ID="hngroupid" runat="server" Value="0" />
    </ContentTemplate>
</asp:UpdatePanel>
