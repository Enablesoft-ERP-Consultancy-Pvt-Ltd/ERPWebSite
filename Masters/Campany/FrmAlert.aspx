<%@ Page Title="" Language="C#" MasterPageFile="~/ERPmaster.master" AutoEventWireup="true"
    EnableEventValidation="false" CodeFile="FrmAlert.aspx.cs" Inherits="Masters_Campany_FrmAlert" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script src="../../Scripts/JScript.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery-1.4.1.js" type="text/javascript"></script>
    <script type="text/javascript">
        function closeForm() {
            window.location.href = "../../main.aspx";
        }
        function priview() {
            window.open("../../ReportViewer.aspx");
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPH_Form" runat="Server">
    <asp:UpdatePanel ID="updatepanel1" runat="server">
        <ContentTemplate>
            <div style="margin-left: 15%; margin-right: 15%; height: 430px">
                <table>
                    <tr>
                        <td class="tdstyle">
                            <asp:Label ID="lbldesignname" runat="server" Text="AlertName"></asp:Label>
                            &nbsp;
                        </td>
                        <td>
                            <asp:TextBox ID="txtDesign" runat="server" CssClass="textb" ValidationGroup="a"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="Please Enter Design Name"
                                ControlToValidate="txtDesign" ValidationGroup="a" ForeColor="Red">*</asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2" align="right">
                            <asp:Button ID="btnsave" runat="server" Text="Update" CssClass="buttonnorm" OnClientClick="return confirm('Do you want to save data?')"
                                OnClick="btnsave_Click" ValidationGroup="a" />
                            <asp:Button ID="btnclose" runat="server" CssClass="buttonnorm" Text="Close" OnClientClick="closeForm();" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <asp:Label ID="Lblerr" runat="server" ForeColor="Red"></asp:Label>
                            <asp:GridView ID="gdalert" runat="server" Width="400px" AllowPaging="True" AutoGenerateColumns="false"
                                PageSize="12" DataKeyNames="Sr_No" CssClass="grid-view" OnSelectedIndexChanged="gdalert_SelectedIndexChanged"
                                OnRowDataBound="gdalert_RowDataBound" OnRowCreated="gdalert_RowCreated">
                                <Columns>
                                    <asp:TemplateField HeaderText="Id">
                                        <ItemTemplate>
                                            <asp:Label ID="lblid" runat="server" Text='<%#Bind("Sr_No") %>'></asp:Label>
                                            <asp:Label ID="lblalertname" runat="server" Visible="false" Text='<%#Bind("AlertName") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="AlertName" HeaderText="Alert Name" />
                                </Columns>
                            </asp:GridView>
                        </td>
                    </tr>
                </table>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="CPH_SideBar" runat="Server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="CPH_Footer" runat="Server">
</asp:Content>
