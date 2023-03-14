<%@ Page Title="PRODUCTION UNIT MASTER" Language="C#" MasterPageFile="~/ERPmaster.master"
    AutoEventWireup="true" CodeFile="frmproductionunitmaster.aspx.cs" Inherits="Masters_Loom_frmproductionunitmaster" %>

<asp:Content ID="Content2" ContentPlaceHolderID="CPH_Form" runat="Server">
    <script src="../../Scripts/JScript.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery-1.4.1.js" type="text/javascript"></script>
    <%--<script type="text/javascript" src="../../Scripts/Fixfocus.js"></script>--%>
    <script type="text/javascript">
        function reloadPage() {
            window.location.href = "frmproductionunitmaster.aspx";
        }
        function CloseForm() {
            window.location.href = "../../main.aspx";
        }
    </script>
    <asp:UpdatePanel ID="Upd1" runat="server">
        <ContentTemplate>
            <div>
                <div style="margin: 2% 30% 0% 30%">
                    <table>
                        <tr>
                            <td>
                                <asp:Label ID="Label1" Text="Unit Name" CssClass="labelbold" runat="server" />
                            </td>
                            <td>
                                <asp:TextBox ID="txtunitname" CssClass="textb" Width="200px" runat="server" />
                                <asp:RequiredFieldValidator ErrorMessage="*" ControlToValidate="txtunitname" CssClass="labelbold"
                                    SetFocusOnError="true" ForeColor="Red" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="Label2" Text="Address" CssClass="labelbold" runat="server" />
                            </td>
                            <td>
                                <asp:TextBox ID="txtaddress" CssClass="textb" Width="200px" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="Label3" Text="Unit Code" CssClass="labelbold" runat="server" />
                            </td>
                            <td>
                                <asp:TextBox ID="txtunitcode" CssClass="textb" Width="200px" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2" align="right">
                                <asp:Button ID="btnnew" Text="New" CssClass="buttonnorm" runat="server" OnClientClick="return reloadPage();" />
                                <asp:Button ID="btnsave" Text="Save" CssClass="buttonnorm" runat="server" OnClick="btnsave_Click" />
                                <asp:Button ID="btnclose" Text="Close" CssClass="buttonnorm" runat="server" OnClientClick="return CloseForm();" />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <asp:Label ID="lblmsg" CssClass="labelbold" ForeColor="Red" Text="" runat="server" />
                            </td>
                        </tr>
                    </table>
                </div>
                <div style="margin: 0% 30% 0% 30%">
                    <div style="height: auto; max-height: 200px; overflow: auto;">
                        <asp:GridView ID="DG" runat="server" AutoGenerateColumns="False" CssClass="grid-views"
                            EmptyDataText="No Records Found!!!">
                            <HeaderStyle CssClass="gvheaders" />
                            <AlternatingRowStyle CssClass="gvalts" />
                            <Columns>
                                <asp:TemplateField HeaderText="SrNo.">
                                    <ItemTemplate>
                                        <%# Container.DataItemIndex+1 %>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Unit Name">
                                    <ItemTemplate>
                                        <asp:Label ID="lblunitname" Text='<%#Bind("unitname") %>' runat="server" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Address">
                                    <ItemTemplate>
                                        <asp:Label ID="lbladdress" Text='<%#Bind("Address") %>' runat="server" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Unit Code">
                                    <ItemTemplate>
                                        <asp:Label ID="lblunitcode" Text='<%#Bind("Unitcode") %>' runat="server" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField Visible="false">
                                    <ItemTemplate>
                                        <asp:Label ID="lbluid" Text='<%#Bind("uid") %>' runat="server" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lbEdit" runat="server" OnClick="lbEdit_Click" ToolTip="Edit"
                                            CausesValidation="False">Edit</asp:LinkButton>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <SelectedRowStyle CssClass="SelectedRowStyle" />
                        </asp:GridView>
                    </div>
                    <asp:HiddenField ID="hnUid" runat="server" />
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
