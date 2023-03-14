<%@ Page Language="C#" Title="Cone Type" AutoEventWireup="true" CodeFile="FrmConeTypeMaster.aspx.cs"
    MasterPageFile="~/ERPmaster.master" Inherits="FrmConeTypeMaster" EnableEventValidation="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Page" ContentPlaceHolderID="CPH_Form" runat="server">
    <script src="../../Scripts/JScript.js" type="text/javascript"></script>
    <link href="../../Styles/vijay.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript">
        function CloseForm() {
            window.location.href = "../../main.aspx"
        }
    </script>
    <div style="margin-left: 15%; margin-right: 15%; height: 480px">
        <asp:UpdatePanel ID="updatepanel1" runat="server">
            <ContentTemplate>
                <table>
                    <tr>
                        <td class="tdstyle">
                            <asp:Label Text="Cone Type" runat="server" ID="lblConeType" Font-Bold="true" />&nbsp;
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="please Enter TxtConeType"
                                ControlToValidate="TxtConeType" ForeColor="Red">*</asp:RequiredFieldValidator>
                            <br />
                            <asp:TextBox ID="TxtConeType" runat="server" CssClass="textb">
                            </asp:TextBox>
                        </td>
                        <td class="tdstyle">
                            <asp:Label Text="Cone Weight" runat="server" ID="Label1" Font-Bold="true" />
                            <br />
                            <asp:TextBox ID="TxtConeWeight" runat="server" CssClass="textb" onkeydown="return (event.keyCode!=13);">
                            </asp:TextBox>
                        </td>
                        <td class="tdstyle">
                            <asp:Label Text="Seq No" runat="server" ID="Label3" Font-Bold="true" />
                            <br />
                            <asp:TextBox ID="TxtSeqenceNo" runat="server" CssClass="textb" onkeydown="return (event.keyCode!=13);">
                            </asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="3" align="right">
                            <asp:Button ID="btnsave" runat="server" CssClass="buttonnorm" Text="Save" OnClientClick="return confirm('Do you want to save data?')"
                                OnClick="btnsave_Click" />
                            <asp:Button ID="btnclose" runat="server" CssClass="buttonnorm" Text="Close" OnClientClick="return CloseForm();"
                                OnClick="btnclose_Click" />
                        </td>
                    </tr>
                </table>
                <table>
                    <tr>
                        <td class="style2" colspan="2">
                            <asp:Label ID="Label2" runat="server" ForeColor="Red" Visible="False"></asp:Label>
                            <asp:GridView ID="DGConeType" runat="server" Width="280px" AllowPaging="True" CellPadding="4"
                                PageSize="25" ForeColor="#333333" GridLines="None" OnPageIndexChanging="DGConeType_PageIndexChanging"
                                OnRowDataBound="DGConeType_RowDataBound" OnSelectedIndexChanged="DGConeType_SelectedIndexChanged"
                                CssClass="grid-view" OnRowCreated="DGConeType_RowCreated">
                                <HeaderStyle CssClass="gvheaders" />
                                <AlternatingRowStyle CssClass="gvalts" />
                                <RowStyle CssClass="gvrow" />
                                <EmptyDataRowStyle CssClass="gvemptytext" />
                            </asp:GridView>
                        </td>
                    </tr>
                </table>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</asp:Content>
