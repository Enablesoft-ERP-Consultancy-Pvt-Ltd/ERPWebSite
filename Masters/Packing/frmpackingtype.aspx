<%@ Page Title="PACKING TYPE" Language="C#" MasterPageFile="~/ERPmaster.master" AutoEventWireup="true"
    CodeFile="frmpackingtype.aspx.cs" Inherits="Masters_Packing_frmpackingtype" EnableEventValidation="false" %>

<asp:Content ID="Content2" ContentPlaceHolderID="CPH_Form" runat="Server">
    <script type="text/javascript">
        function NewForm() {
            window.location.href = "frmpackingtype.aspx";
        }
        function CloseForm() {
            window.location.href = "../../main.aspx";
        }
    </script>
    <asp:UpdatePanel ID="updt1" runat="server">
        <ContentTemplate>
            <div style="margin: 0% 20% 0% 20%">
                <table>
                    <tr>
                        <td>
                            <asp:Label ID="lblpacktype" Text="Packing Type" CssClass="labelbold" runat="server" />
                        </td>
                        <td>
                            <asp:TextBox ID="txtpacktype" CssClass="textb" runat="server" Width="300px" />
                        </td>
                    </tr>
                    <tr>
                        <td valign="top">
                            <asp:Label ID="Label1" Text="Remarks" CssClass="labelbold" runat="server" />
                        </td>
                        <td>
                            <asp:TextBox ID="txtremarks" CssClass="textb" runat="server" Width="300px" TextMode="MultiLine"
                                Height="52px" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                        </td>
                        <td>
                            <div style="max-height: 200px; overflow: auto">
                                <asp:GridView ID="DGPacktype" runat="server" CssClass="grid-view" AutoGenerateColumns="false"
                                    OnRowDataBound="DGPacktype_RowDataBound" OnSelectedIndexChanged="DGPacktype_SelectedIndexChanged"
                                    AutoGenerateSelectButton="true">
                                    <HeaderStyle CssClass="gvheaders" />
                                    <AlternatingRowStyle CssClass="gvalts" />
                                    <RowStyle CssClass="gvrow" />
                                    <Columns>
                                        <asp:TemplateField HeaderText="Sr No.">
                                            <ItemTemplate>
                                                <%#Container.DataItemIndex+1 %>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Packing Type">
                                            <ItemTemplate>
                                                <asp:Label ID="lblpacktype" Text='<%#Bind("packingtype") %>' runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Remarks">
                                            <ItemTemplate>
                                                <asp:Label ID="lblremarks" Text='<%#Bind("Remarks") %>' runat="server" />
                                            </ItemTemplate>
                                            <ItemStyle Width="170px" />
                                        </asp:TemplateField>
                                        <asp:TemplateField Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblid" Text='<%#Bind("ID") %>' runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2" align="right">
                            <asp:Button ID="btnnew" Text="New" runat="server" CssClass="buttonnorm" OnClientClick="return NewForm();" />
                            <asp:Button ID="btnsave" Text="Save" runat="server" CssClass="buttonnorm" OnClick="btnsave_Click" />
                            <asp:Button ID="btnclose" Text="Close" runat="server" CssClass="buttonnorm" OnClientClick="return CloseForm();" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                        </td>
                        <td>
                            <asp:Label ID="lblmsg" Text="" CssClass="labelbold" ForeColor="Red" runat="server" />
                        </td>
                    </tr>
                </table>
            </div>
            <asp:HiddenField ID="hnid" runat="server" Value="0" />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
