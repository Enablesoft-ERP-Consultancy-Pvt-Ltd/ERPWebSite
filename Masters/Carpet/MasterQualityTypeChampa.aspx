<%@ Page Title="Master Quality Type" Language="C#" AutoEventWireup="true" CodeFile="MasterQualityTypeChampa.aspx.cs"
    MasterPageFile="~/ERPmaster.master" Inherits="Masters_Carpet_MasterQualityTypeChampa"
    EnableEventValidation="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="CPH_Form" runat="server">
    <script src="../../Scripts/JScript.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery-1.4.1.js" type="text/javascript"></script>
    <link href="../../Styles/vijay.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript">
        function CloseForm() {
            window.location.href = "../../main.aspx";
        }
    </script>
    <div style="margin-left: 15%; margin-right: 15%; height: 480px">
        <asp:TextBox ID="txtid" runat="server" Visible="False"></asp:TextBox>
        <asp:UpdatePanel ID="updatepanel1" runat="server">
            <ContentTemplate>
                <div style="margin: 1% 20% 0% 20%">
                    <table>
                        <tr>
                            <td class="tdstyle">
                                <asp:Label ID="lblMasterQualityType" runat="server" Text="Master QualityType" Font-Bold="true"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtMasterQualityType" runat="server" CssClass="textb" ValidationGroup="a"
                                    Width="200px"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="Please Enter Master Quality Name"
                                    ControlToValidate="txtMasterQualityType" ValidationGroup="a" ForeColor="Red">*</asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td>
                                &nbsp;
                            </td>
                            <td align="left">
                                <%--   <asp:Button ID="Button1" runat="server" CssClass="buttonnorm preview_width" OnClick="Button1_Click"
                            Text="Preview" UseSubmitBehavior="False" />--%>
                                <asp:Button ID="btnsave" runat="server" Text="Save" CssClass="buttonnorm" OnClientClick="return confirm('Do you want to save data?')"
                                    OnClick="btnsave_Click" ValidationGroup="a" />
                                <asp:Button ID="btnclose" runat="server" CssClass="buttonnorm" Text="Close" OnClientClick="closeForm();"
                                    UseSubmitBehavior="False" />
                                <asp:Button ID="btndelete" runat="server" Text="Delete" OnClientClick="return confirm('Do you want to Delete data?')"
                                    CssClass="buttonnorm" OnClick="btndelete_Click" Visible="False" />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td>
                            </td>
                            <td>
                                <asp:Label ID="Lblerr" runat="server" ForeColor="Red"></asp:Label>
                                <div style="max-height: 500px; overflow: auto">
                                    <asp:GridView ID="GVMasterQualityType" runat="server" Width="400px" AllowPaging="True"
                                        PageSize="30" AutoGenerateColumns="false" DataKeyNames="MasterQualityTypeId"
                                        OnPageIndexChanging="GVMasterQualityType_PageIndexChanging" OnRowDataBound="GVMasterQualityType_RowDataBound"
                                        OnSelectedIndexChanged="GVMasterQualityType_SelectedIndexChanged" OnInit="GVMasterQualityType_Init">
                                        <HeaderStyle CssClass="gvheaders" />
                                        <AlternatingRowStyle CssClass="gvalts" />
                                        <RowStyle CssClass="gvrow" />
                                        <PagerStyle CssClass="PagerStyle" />
                                        <EmptyDataRowStyle CssClass="gvemptytext" />
                                        <Columns>
                                            <asp:TemplateField HeaderText="Sr No">
                                                <ItemTemplate>
                                                    <%# Container.DataItemIndex+1 %>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Master QualityTypeName">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblMasterQualityTypeName" Text='<%#Bind("MasterQualityTypeName") %>'
                                                        runat="server" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="" Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblMasterQualityTypeId" Text='<%#Bind("MasterQualityTypeId") %>' runat="server" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </div>
                            </td>
                        </tr>
                    </table>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</asp:Content>
