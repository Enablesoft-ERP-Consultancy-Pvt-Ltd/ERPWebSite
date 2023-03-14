<%@ Page Language="C#" Title="GoodReceipt" AutoEventWireup="true" CodeFile="GoodReceipt.aspx.cs"
    MasterPageFile="~/ERPmaster.master" Inherits="Masters_Campany_GoodReceipt" EnableEventValidation="false" %>

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
    <div style="margin-left: 15%; margin-right: 15%; height: 450px">
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
                            <asp:Label ID="lbl" Text="Station Name" runat="server" Font-Bold="true" />
                        </td>
                        <br />
                        <td>
                            <asp:TextBox ID="txtStationname" runat="server" CssClass="textb">
                            </asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="Please Enter StationName"
                                ControlToValidate="txtStationName" ValidationGroup="m" ForeColor="Red">*</asp:RequiredFieldValidator>
                        </td>
                    </tr>
                </table>
                &nbsp;<table>
                    <tr>
                        <td class="style2" colspan="2">
                            <asp:Label ID="Label1" runat="server" ForeColor="Red"></asp:Label>
                            <asp:GridView ID="gdGoods" runat="server" Width="280px" AllowPaging="True" CellPadding="4"
                                PageSize="6" ForeColor="#333333" OnPageIndexChanging="gdGoods_PageIndexChanging"
                                OnRowDataBound="gdGoods_RowDataBound" OnSelectedIndexChanged="gdGoods_SelectedIndexChanged"
                                DataKeyNames="GoodsReceiptId" AutoGenerateColumns="False" CssClass="grid-views"
                                OnRowCreated="gdGoods_RowCreated">
                                <HeaderStyle CssClass="gvheaders" />
                                <AlternatingRowStyle CssClass="gvalts" />
                                <RowStyle CssClass="gvrow" />
                                <PagerStyle CssClass="PagerStyle" />
                                <EmptyDataRowStyle CssClass="gvemptytext" />
                                <Columns>
                                    <asp:BoundField DataField="GoodsreceiptId" HeaderText="Sr.No." />
                                    <asp:BoundField DataField="StationName" HeaderText="Station Name" />
                                </Columns>
                            </asp:GridView>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <asp:Button ID="btnsave" runat="server" CssClass="buttonnorm" Text="Save" OnClientClick="return confirm('Do you want to save data?')"
                                OnClick="btnsave_Click" ValidationGroup="m" Style="margin-left: 180px" />
                            <asp:Button ID="btnclose" runat="server" CssClass="buttonnorm" Text="Close" OnClick="btnclose_Click"
                                OnClientClick="return CloseForm();" />
                            <asp:Button ID="btndelete" runat="server" Text="Delete" CssClass="buttonnorm" OnClientClick="return confirm('Do you want to Delete data?')"
                                OnClick="btndelete_Click" Visible="False" />
                        </td>
                    </tr>
                </table>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</asp:Content>
