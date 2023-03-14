<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Leavetype.ascx.cs" Inherits="HRUserControls_Leavetype" %>
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
                <td style="width: 10%; border-style: dotted">
                    <asp:Label ID="lblname" Text="Name" CssClass="labelbold" runat="server" />
                </td>
                <td style="width: 40%; border-style: dotted">
                    <asp:TextBox ID="txtname" CssClass="textboxm" runat="server" Width="95%" MaxLength="30" />
                </td>
            </tr>
            <tr>
                <td style="width: 10%; border-style: dotted">
                    <asp:Label ID="Label1" Text="Code" CssClass="labelbold" runat="server" />
                </td>
                <td style="width: 40%; border-style: dotted; margin-left: 40px;">
                    <asp:TextBox ID="txtcode" CssClass="textboxm" runat="server" Width="95%" MaxLength="5" />
                </td>
            </tr>
            <tr>
                <td style="width: 10%; border-style: dotted">
                    <asp:Label ID="Label2" Text="Type" CssClass="labelbold" runat="server" />
                </td>
                <td style="width: 40%; border-style: dotted">
                    <asp:DropDownList ID="DDtype" CssClass="dropdown" runat="server">
                        <asp:ListItem Text="PAID" Value="1" />
                        <asp:ListItem Text="UNPAID" Value="2" />
                        <asp:ListItem Text="ONDUTY" Value="3" />
                        <asp:ListItem Text="RESTRICTED HOLIDAY" Value="4" />
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td style="width: 10%; border-style: dotted">
                    <asp:Label ID="Label3" Text="Unit" CssClass="labelbold" runat="server" />
                </td>
                <td style="width: 40%; border-style: dotted">
                    <asp:RadioButton ID="RDdays" Text="Days" CssClass="radiobutton" runat="server" GroupName="q"
                        Checked="true" />
                    <asp:RadioButton ID="RDhours" Text="Hours" CssClass="radiobutton" runat="server"
                        GroupName="q" />
                </td>
            </tr>
            <tr>
                <td style="width: 10%; border-style: dotted">
                    <asp:Label ID="Label4" Text="Description" CssClass="labelbold" runat="server" />
                </td>
                <td style="width: 40%; border-style: dotted">
                    <asp:TextBox ID="txtdesc" CssClass="textboxm" runat="server" Width="95%" TextMode="MultiLine"
                        Height="45px" MaxLength="100" />
                </td>
            </tr>
            <tr>
                <td colspan="2" align="right" style="border-style: dotted">
                    <asp:Button ID="btnsave" CssClass="buttonnorm" Text="Save" runat="server" OnClick="btnsave_Click" />
                    <asp:Button ID="btnclose" CssClass="buttonnorm" Text="Close" runat="server" OnClientClick="closeForm()" />
                </td>
            </tr>
            <tr>
                <td colspan="2" style="border-style: none">
                    <asp:Label ID="lblmsg" CssClass="labelbold" Text="" runat="server" ForeColor="Red" />
                </td>
            </tr>
            <tr>
                <td colspan="2" style="border-style: none">
                    <div style="width: 100%; max-height: 200px; overflow: auto">
                        <asp:GridView ID="Dgdetail" CssClass="grid-views" AutoGenerateColumns="false" runat="server"
                            Width="100%">
                            <HeaderStyle CssClass="gvheaders" />
                            <AlternatingRowStyle CssClass="gvalts" />
                            <RowStyle CssClass="gvrow" />
                            <EmptyDataRowStyle CssClass="gvemptytext" />
                            <Columns>
                                <asp:TemplateField HeaderText="Sr No.">
                                    <ItemTemplate>
                                        <asp:Label ID="lblRowNumber" Text='<%# Container.DataItemIndex + 1 %>' runat="server" />
                                    </ItemTemplate>
                                    <HeaderStyle Width="40px" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Name">
                                    <ItemTemplate>
                                        <asp:Label ID="lblname" Text='<%#Bind("Name") %>' runat="server" />
                                    </ItemTemplate>
                                    <HeaderStyle Width="150px" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Code">
                                    <ItemTemplate>
                                        <asp:Label ID="lblcode" Text='<%#Bind("Code") %>' runat="server" />
                                    </ItemTemplate>
                                    <HeaderStyle Width="40px" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Type">
                                    <ItemTemplate>
                                        <asp:Label ID="lbltype" Text='<%#Bind("Type") %>' runat="server" />
                                    </ItemTemplate>
                                    <HeaderStyle Width="100px" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Unit">
                                    <ItemTemplate>
                                        <asp:Label ID="lblunit" Text='<%#Bind("Unit") %>' runat="server" />
                                    </ItemTemplate>
                                    <HeaderStyle Width="30px" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Description">
                                    <ItemTemplate>
                                        <asp:Label ID="lbldesc" Text='<%#Bind("Description") %>' runat="server" />
                                    </ItemTemplate>
                                    <HeaderStyle Width="150px" />
                                </asp:TemplateField>
                                <asp:TemplateField Visible="false">
                                    <ItemTemplate>
                                        <asp:Label ID="lblleaveid" Text='<%#Bind("leaveid") %>' runat="server" />
                                        <asp:Label ID="lbltypeid" Text='<%#Bind("typeid") %>' runat="server" />
                                        <asp:Label ID="lblunitid" Text='<%#Bind("unitid") %>' runat="server" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lbEdit" runat="server" OnClick="lbEdit_Click" ToolTip="Edit"
                                            CausesValidation="False">Edit</asp:LinkButton>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lbldel" runat="server" OnClick="lbldel_Click" ToolTip="Edit"
                                            CausesValidation="False" OnClientClick="return confirm('Do you want to delete this row ?')">Delete</asp:LinkButton>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </div>
                </td>
            </tr>
        </table>
        <asp:HiddenField ID="hnleaveid" runat="server" Value="0" />
    </ContentTemplate>
</asp:UpdatePanel>
