<%@ Page Language="C#" Title="Designation" AutoEventWireup="true" CodeFile="Designation.aspx.cs"
    MasterPageFile="~/ERPmaster.master" Inherits="Masters_Campany_Designation" EnableEventValidation="false" %>

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
        <table>
            <tr>
                <td>
                    <asp:TextBox ID="txtid" runat="server" Visible="False"></asp:TextBox>
                </td>
            </tr>
        </table>
        <asp:UpdatePanel ID="updatepanel1" runat="server">
            <ContentTemplate>
                <table>
                    <tr>
                        <td class="tdstyle">
                            <asp:Label Text="Designation Name" runat="server" ID="lbld" Font-Bold="true" />
                        </td>
                        <td>
                            <asp:TextBox ID="txtDesignationname" runat="server" CssClass="textb">
                            </asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="please Enter Designationname"
                                ControlToValidate="txtDesignationname" ForeColor="Red">*</asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <td class="tdstyle">
                        <asp:Label Text="Description" runat="server" ID="Label1" Font-Bold="true" />
                    </td>
                    <td>
                        <asp:TextBox ID="txtDescription" runat="server" CssClass="textb" onkeydown="return (event.keyCode!=13);">
                        </asp:TextBox>
                    </td>
                    </tr>
                </table>
                <table>
                    <tr>
                        <td class="style2" colspan="2">
                            <asp:Label ID="Label2" runat="server" ForeColor="Red" Visible="False"></asp:Label>
                            <asp:GridView ID="gdDesignation" runat="server" Width="280px" AllowPaging="True"
                                CellPadding="4" PageSize="6" ForeColor="#333333" GridLines="None" OnPageIndexChanging="gdDesignation_PageIndexChanging"
                                OnRowDataBound="gdDesignation_RowDataBound" OnSelectedIndexChanged="gdDesignation_SelectedIndexChanged"
                                CssClass="grid-view" OnRowCreated="gdDesignation_RowCreated">
                                <HeaderStyle CssClass="gvheaders" />
                                <AlternatingRowStyle CssClass="gvalts" />
                                <RowStyle CssClass="gvrow" />
                                <EmptyDataRowStyle CssClass="gvemptytext" />
                            </asp:GridView>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="3">
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <asp:Button ID="btnsave" runat="server" CssClass="buttonnorm" Text="Save" OnClientClick="return confirm('Do you want to save data?')"
                                OnClick="btnsave_Click" Style="margin-left: 170px" />
                            <asp:Button ID="btnclose" runat="server" CssClass="buttonnorm" Text="Close" OnClientClick="return CloseForm();"
                                OnClick="btnclose_Click" />
                            <asp:Button ID="btndelete" runat="server" Text="Delete" OnClientClick="return confirm('Do you want to Delete data?')"
                                CssClass="buttonnorm" OnClick="btndelete_Click" Visible="False" />
                        </td>
                    </tr>
                </table>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</asp:Content>
