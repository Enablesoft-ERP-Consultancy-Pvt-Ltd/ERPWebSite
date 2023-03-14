<%@ Page Title="Bin Master" Language="C#" MasterPageFile="~/ERPmaster.master" AutoEventWireup="true"
    CodeFile="frmBinmaster.aspx.cs" Inherits="Masters_Carpet_frmBinmaster" %>

<asp:Content ID="Content2" ContentPlaceHolderID="CPH_Form" runat="Server">
    <script language="javascript" type="text/javascript">
        function NewForm() {
            window.location.href = "frmBinMaster.aspx";
        }
        function CloseForm() {
            window.location.href = "../../main.aspx";
        }
       
    </script>
    <asp:UpdatePanel ID="upd1" runat="server">
        <ContentTemplate>
            <div style="margin: 0% 20% 0% 10%">
                <table border="1">
                    <tr>
                        <td>
                            <asp:Label ID="lblgodownname" Text="Godown Name" runat="server" CssClass="labelbold" />
                        </td>
                        <td>
                            <asp:DropDownList ID="DDgodown" CssClass="dropdown" Width="200px" AutoPostBack="true"
                                runat="server" OnSelectedIndexChanged="DDgodown_SelectedIndexChanged">
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ValidationGroup="a"
                                ControlToValidate="DDgodown" InitialValue="0" ErrorMessage="*" ForeColor="Red"></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="Label1" Text="Bin No." runat="server" CssClass="labelbold" />
                        </td>
                        <td>
                            <asp:TextBox ID="txtbinno" CssClass="textb" Width="150px" runat="server" />
                            <asp:RequiredFieldValidator ID="req1" runat="server" ValidationGroup="a" ControlToValidate="txtbinno"
                                ErrorMessage="*" ForeColor="Red" SetFocusOnError="true"></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="Label2" Text="Capacity(Kg)" runat="server" CssClass="labelbold" />
                        </td>
                        <td>
                            <asp:TextBox ID="txtcapacity" CssClass="textb" Width="150px" runat="server" />
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ValidationGroup="a"
                                ControlToValidate="txtcapacity" ErrorMessage="*" ForeColor="Red" SetFocusOnError="true"></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2" align="right">
                            <asp:Button ID="btnsave" Text="Save" CssClass="buttonnorm" runat="server" OnClick="btnsave_Click"
                                ValidationGroup="a" />
                            <asp:Button ID="btnclose" Text="Close" CssClass="buttonnorm" runat="server" OnClientClick="return CloseForm();" />
                            <asp:Button ID="btnnew" Text="New" CssClass="buttonnorm" runat="server" OnClientClick="return NewForm();" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <asp:Label ID="lblmsg" ForeColor="Red" CssClass="labelbold" Text="" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <div style="max-height: 300px; overflow: auto">
                                <asp:GridView ID="DGDetail" AutoGenerateColumns="false" CssClass="grid-views" runat="server"
                                    AutoGenerateSelectButton="true" OnRowDataBound="DGDetail_RowDataBound" OnSelectedIndexChanged="DGDetail_SelectedIndexChanged">
                                    <HeaderStyle CssClass="gvheaders" />
                                    <AlternatingRowStyle CssClass="gvalts" />
                                    <RowStyle CssClass="gvrow" />
                                    <Columns>
                                        <asp:TemplateField HeaderText="Sr No.">
                                            <ItemTemplate>
                                                <%#Container.DataItemIndex+1 %>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Bin No.">
                                            <ItemTemplate>
                                                <asp:Label ID="lblbinno" Text='<%#Bind("BinNo") %>' runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Capacity(Kg)">
                                            <ItemTemplate>
                                                <asp:Label ID="lblcapacity" Text='<%#Bind("capacity") %>' runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblbinid" Text='<%#Bind("binid") %>' runat="server" />
                                                <asp:Label ID="lblgodownid" Text='<%#Bind("godownid") %>' runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </div>
                        </td>
                    </tr>
                </table>
            </div>
            <asp:HiddenField ID="hnbinid" runat="server" Value="0" />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
