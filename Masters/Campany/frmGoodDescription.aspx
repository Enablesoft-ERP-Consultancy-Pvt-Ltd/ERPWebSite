<%@ Page Title="GoodsDescription" Language="C#" AutoEventWireup="true" MasterPageFile="~/ERPmaster.master"
    CodeFile="~/Masters/Campany/frmGoodDescription.aspx.cs" Inherits="Masters_Campany_frmGoodDescription"
    EnableEventValidation="false" %>

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
    <div style="margin-left: 15%; margin-right: 15%; margin-top: 10px; height: 480px">
        <table>
            <tr>
                <td>
                    <asp:TextBox ID="txtid" runat="server" Visible="False"></asp:TextBox>
                </td>
            </tr>
        </table>
        <asp:UpdatePanel ID="updatepanel1" runat="server">
            <ContentTemplate>
                <table style="width: 50%">
                    <tr>
                        <td class="tdstyle">
                            <asp:Label Text=" Goods Name" runat="server" ID="lbl2" Font-Bold="true" />
                        </td>
                        <td>
                            <asp:TextBox ID="txtGoods" runat="server" CssClass="textb" Height="16px" Width="177px"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="Please Enter GoodsName"
                                ControlToValidate="txtGoods" ForeColor="Red">*</asp:RequiredFieldValidator>
                        </td>
                    </tr>
                </table>
                &nbsp;<table>
                    <tr>
                        <td class="style2" colspan="2">
                            <asp:Label ID="lbl" runat="server" ForeColor="Red"></asp:Label>
                            <asp:GridView ID="gdGoods" runat="server" Width="280px" ForeColor="#333333" AllowPaging="true"
                                PageSize="6" CellPadding="4" DataKeyNames="SrNo" OnPageIndexChanging="gdGoods_PageIndexChanging"
                                OnRowDataBound="gdGoods_RowDataBound" OnSelectedIndexChanged="gdGoods_SelectedIndexChanged"
                                OnRowCreated="gdGoods_RowCreated" CssClass="grid-views">
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
                            <asp:Button ID="btnsave" runat="server" CssClass="buttonnorm" Text="Save" OnClientClick="return confirm('Do you want to save data?')"
                                OnClick="btnsave_Click" Style="margin-left: 170px" />
                            <asp:Button ID="btnclose" runat="server" CssClass="buttonnorm" Text="Close" OnClick="btnclose_Click"
                                OnClientClick="return CloseForm();" />
                            <asp:Button ID="btndelete" runat="server" Text="Delete" OnClientClick="return confirm('Do you want to Delete data?')"
                                CssClass="buttonnorm" OnClick="btndelete_Click" Visible="False" />
                        </td>
                    </tr>
                </table>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</asp:Content>
