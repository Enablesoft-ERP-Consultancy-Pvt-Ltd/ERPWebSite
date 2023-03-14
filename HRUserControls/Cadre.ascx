<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Cadre.ascx.cs" Inherits="HRUserControls_Cadre" %>
<script src="../../Scripts/JScript.js" type="text/javascript"></script>
<script src="../../Scripts/jquery-1.4.1.js" type="text/javascript"></script>
<script type="text/javascript">
    function closeForm() {

        var objParent = window.opener;
        if (objParent != null) {
            self.close();
        }
        else {
            window.location.href = "../../main.aspx";
        }
    } 
 
</script>
<asp:UpdatePanel ID="upd1" runat="server">
    <ContentTemplate>
        <table style="width: 50%;" border="1" cellspacing="2">
            <tr>
                <td style="width: 20%; border-style: dotted">
                    <asp:Label Text="Cadre Name" CssClass="labelbold" runat="server" />
                </td>
                <td style="width: 80%; border-style: dotted">
                    <asp:TextBox ID="txtcadre" CssClass="textb" runat="server" Width="90%" onFocus="this.select()" />
                    <asp:RequiredFieldValidator ID="req1" ControlToValidate="txtcadre" ErrorMessage="*"
                        runat="server" ForeColor="Red" Font-Bold="true"></asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr>
                <td style="width: 20%; border-style: dotted">
                    <asp:Label ID="Label1" Text="Display Seq. No." CssClass="labelbold" runat="server" />
                </td>
                <td style="width: 80%; border-style: dotted">
                    <asp:TextBox ID="txtseqno" CssClass="textb" runat="server" Width="90%" />
                </td>
            </tr>
            <tr>
                <td colspan="2" align="right">
                    <asp:Button ID="btnsave" Text="Save" CssClass="buttonnorm" runat="server" OnClick="btnsave_Click" />
                    <asp:Button ID="btnclose" Text="Close" CssClass="buttonnorm" runat="server" OnClientClick="closeForm();" />
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <asp:Label ID="lblmsg" CssClass="labelbold" ForeColor="Red" Text="" runat="server" />
                </td>
            </tr>
        </table>
        <table style="width: 50%">
            <tr>
                <td>
                    <div style="width: 100%; overflow: auto; max-height: 200px">
                        <asp:GridView ID="Dgdetail" runat="server" Width="100%" CssClass="grid-views" AutoGenerateColumns="False"
                            EmptyDataText="No data fetched..." OnRowEditing="Dgdetail_RowEditing" OnRowUpdating="Dgdetail_RowUpdating"
                            OnRowCancelingEdit="Dgdetail_RowCancelingEdit" OnRowDeleting="Dgdetail_RowDeleting">
                            <HeaderStyle CssClass="gvheaders" />
                            <AlternatingRowStyle CssClass="gvalts" />
                            <RowStyle CssClass="gvrow" />
                            <EmptyDataRowStyle CssClass="gvemptytext" />
                            <Columns>
                                <asp:TemplateField HeaderText="Sr No.">
                                    <ItemTemplate>
                                        <itemtemplate>
                                                <%#Container.DataItemIndex+1 %>
                                            </itemtemplate>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Cadre">
                                    <ItemTemplate>
                                        <asp:Label ID="lblcadre" Text='<%#Bind("Cadre") %>' runat="server" />
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:TextBox ID="txtcadregrid" Text='<%#Bind("Cadre") %>' runat="server" />
                                    </EditItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Display Seq No.">
                                    <ItemTemplate>
                                        <asp:Label ID="lbldisporderno" Text='<%#Bind("dispseqno") %>' runat="server" />
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:TextBox ID="txtdispseqnogrid" Text='<%#Bind("dispseqno") %>' runat="server" />
                                    </EditItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField Visible="false">
                                    <ItemTemplate>
                                        <asp:Label ID="lblcadreid" Text='<%#Bind("cadreid") %>' runat="server" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:CommandField EditText="Edit" ShowEditButton="True" CausesValidation="false"
                                    ShowDeleteButton="True">
                                    <HeaderStyle HorizontalAlign="Center" />
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:CommandField>
                            </Columns>
                        </asp:GridView>
                    </div>
                </td>
            </tr>
        </table>
    </ContentTemplate>
</asp:UpdatePanel>
