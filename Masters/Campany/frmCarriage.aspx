<%@ Page Language="C#" Title="Carriage" AutoEventWireup="true" MasterPageFile="~/ERPmaster.master"
    CodeFile="frmCarriage.aspx.cs" Inherits="Masters_Campany_frmCarriage" EnableEventValidation="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="page" ContentPlaceHolderID="CPH_Form" runat="server">
    <script src="../../Scripts/JScript.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery-1.4.1.js" type="text/javascript"></script>
    <link href="../../Styles/vijay.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript">
        function CloseForm() {
            window.location.href = "../../main.aspx"
        }
    </script>
    <asp:UpdatePanel ID="updatepanel1" runat="server">
        <ContentTemplate>
            <div id="main" runat="server" style="padding-left: 20%; height: 455px">
                <table style="width: 50%; align: auto">
                    <tr>
                        <td>
                            <asp:TextBox ID="txtid" runat="server" Visible="false"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdstyle">
                            <asp:Label ID="Label1" Text="Carriage Name" runat="server" Font-Bold="true" />
                        </td>
                        &nbsp;<td>
                            <asp:TextBox ID="txtCarriage" runat="server" CssClass="textb" Width="177px"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="Please Enter CarriageName"
                                ControlToValidate="txtCarriage" ValidationGroup="s" ForeColor="Red">*</asp:RequiredFieldValidator>
                        </td>
                    </tr>
                </table>
                &nbsp;
                <table>
                    <tr>
                        <td class="style2" colspan="2">
                            <asp:Label ID="lbl" runat="server" ForeColor="Red"></asp:Label>
                            <asp:GridView ID="gdCarriage" runat="server" Width="400px" AllowPaging="True" CellPadding="4"
                                PageSize="6" ForeColor="#333333" DataKeyNames="CarriageId" OnPageIndexChanging="gdCarriage_PageIndexChanging"
                                OnRowDataBound="gdCarriage_RowDataBound" OnSelectedIndexChanged="gdCarriage_SelectedIndexChanged"
                                AutoGenerateColumns="False" CssClass="grid-views">
                                <HeaderStyle CssClass="gvheaders" />
                                <AlternatingRowStyle CssClass="gvalts" />
                                <RowStyle CssClass="gvrow" />
                                <PagerStyle CssClass="PagerStyle" />
                                <EmptyDataRowStyle CssClass="gvemptytext" />
                                <Columns>
                                    <asp:BoundField DataField="CarriageId" HeaderText="Sr.No." />
                                    <asp:BoundField DataField="CarriageName" HeaderText="Carriage Name" />
                                </Columns>
                            </asp:GridView>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="3" align="right">
                            <asp:Button ID="btnsave" runat="server" CssClass="buttonnorm" Text="Save" OnClientClick="return confirm('Do you want to save data?')"
                                OnClick="btnsave_Click" ValidationGroup="s" Style="margin-left: 200px" />
                            <asp:Button ID="btnclose" runat="server" CssClass="buttonnorm" Text="Close" OnClientClick="return CloseForm();" />
                            <asp:Button ID="btndelete" runat="server" Text="Delete" CssClass="buttonnorm" OnClientClick="return confirm('Do you want to Delete data?')"
                                OnClick="btndelete_Click" Visible="False" />
                        </td>
                    </tr>
                </table>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
