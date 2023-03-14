<%@ Page Title="UnitMaster" Language="C#" AutoEventWireup="true" CodeFile="UnitMaster.aspx.cs"
    MasterPageFile="~/ERPmaster.master" Inherits="Masters_Campany_UnitMaster" EnableEventValidation="false" %>

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
                <table>
                    <tr>
                        <td class="tdstyle">
                            <asp:Label Text="Unit" runat="server" ID="labelss" Font-Bold="true" />
                            &nbsp;
                        </td>
                        <td>
                            <asp:TextBox ID="txtUnit" runat="server" CssClass="textb"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="Please Enter  Unit"
                                ControlToValidate="txtUnit" ValidationGroup="a" ForeColor="Red">*</asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    &nbsp;&nbsp;&nbsp;&nbsp;</table>
                <br />
                <table>
                    <tr>
                        <td class="style2" colspan="2">
                            <asp:Label ID="Lblerr" runat="server" ForeColor="Red"></asp:Label>
                            <asp:GridView ID="gdUnit" runat="server" Width="280px" AllowPaging="True" PageSize="6"
                                OnPageIndexChanging="gdUnit_PageIndexChanging" OnRowDataBound="gdUnit_RowDataBound"
                                OnSelectedIndexChanged="gdUnit_SelectedIndexChanged" DataKeyNames="Sr_No">
                                <HeaderStyle CssClass="gvheaders" />
                                <AlternatingRowStyle CssClass="gvalts" />
                                <RowStyle CssClass="gvrow" />
                                <PagerStyle CssClass="PagerStyle" />
                                <EmptyDataRowStyle CssClass="gvemptytext" />
                            </asp:GridView>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2" align="right">
                            <asp:Button ID="btnNew0" runat="server" CssClass="buttonnorm" Text="New" OnClick="btnNew_Click" />
                            <asp:Button ID="btnsave" runat="server" CssClass="buttonnorm" Text="Save" OnClientClick="return confirm('Do you want to save data?')"
                                OnClick="btnsave_Click" ValidationGroup="a" />
                            <asp:Button ID="BtnClose" runat="server" Text="Close" OnClientClick="CloseForm();"
                                CssClass="buttonnorm" />
                        </td>
                    </tr>
                </table>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</asp:Content>
