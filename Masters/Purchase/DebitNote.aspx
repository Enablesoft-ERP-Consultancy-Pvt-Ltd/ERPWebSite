<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DebitNote.aspx.cs" MasterPageFile="~/ERPmaster.master"
    Inherits="Masters_Purchase_DebitNote" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="page" ContentPlaceHolderID="CPH_Form" runat="server">
    <script src="../../Scripts/JScript.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery-1.4.1.js" type="text/javascript"></script>
    <%--<script type="text/javascript" src="../../Scripts/Fixfocus.js"></script>--%>
    <script type="text/javascript">
        function CloseForm() {
            window.location.href = "../../main.aspx"
        }
    </script>
    <div id="main" runat="server" style="height: 400px">
        <table>
            <tr>
                <td class="tdstyle">
                    CompanyName<br />
                    <asp:DropDownList ID="DDCompany" runat="server" AutoPostBack="True" OnSelectedIndexChanged="DDCompany_SelectedIndexChanged"
                        CssClass="dropdown">
                    </asp:DropDownList>
                    <cc1:ListSearchExtender ID="ListSearchExtender4" runat="server" TargetControlID="DDCompany"
                        ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                    </cc1:ListSearchExtender>
                </td>
                <td class="tdstyle">
                    PartyName<br />
                    <asp:DropDownList ID="DDPartyName" runat="server" AutoPostBack="True" OnSelectedIndexChanged="DDPartyName_SelectedIndexChanged"
                        CssClass="dropdown">
                    </asp:DropDownList>
                    <cc1:ListSearchExtender ID="ListSearchExtender1" runat="server" TargetControlID="DDPartyName"
                        ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                    </cc1:ListSearchExtender>
                </td>
                <td class="tdstyle">
                    GateInNo<br />
                    <asp:DropDownList ID="DDReceiveNo" runat="server" AutoPostBack="True" OnSelectedIndexChanged="DDReceiveNo_SelectedIndexChanged"
                        CssClass="dropdown">
                    </asp:DropDownList>
                    <cc1:ListSearchExtender ID="ListSearchExtender2" runat="server" TargetControlID="DDReceiveNo"
                        ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                    </cc1:ListSearchExtender>
                </td>
                <td class="tdstyle">
                    DebitNoteNo<br />
                    <asp:TextBox ID="TxtDebtNoteNo" runat="server" CssClass="textb"></asp:TextBox>
                </td>
                <td class="tdstyle">
                    Date
                    <br />
                    <asp:TextBox ID="TxtDate" runat="server" CssClass="textb"></asp:TextBox>
                    <asp:CalendarExtender ID="extender1" runat="server" TargetControlID="TxtDate" Format="dd-MMM-yyyy">
                    </asp:CalendarExtender>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <asp:Label ID="LblError" ForeColor="Red" runat="server" Text=""></asp:Label>
                </td>
                <td colspan="2">
                    <asp:Button ID="BtnSave" runat="server" Text="Save" OnClick="BtnSave_Click" CssClass="buttonnorm" />
                    <asp:Button ID="BtnPreview" runat="server" Text="Preview" CssClass="buttonnorm" />
                    <asp:Button ID="BtnClose" runat="server" OnClientClick="CloseForm();" Text="Close"
                        OnClick="BtnClose_Click" CssClass="buttonnorm" />
                </td>
            </tr>
            <tr>
                <td colspan="5">
                    <asp:GridView ID="DGItemDetail" runat="server" OnRowDataBound="DGItemDetail_RowDataBound"
                        AutoGenerateColumns="False" DataKeyNames="PurchaseReceiveDetailId" CssClass="grid-view"
                        OnRowCreated="DGItemDetail_RowCreated">
                        <HeaderStyle CssClass="gvheader" />
                        <AlternatingRowStyle CssClass="gvalt" />
                        <RowStyle CssClass="gvrow" />
                        <EmptyDataRowStyle CssClass="gvemptytext" />
                        <Columns>
                            <asp:BoundField DataField="PurchaseReceiveDetailId" HeaderText="" ControlStyle-ForeColor="AntiqueWhite" />
                            <asp:BoundField DataField="Item_Name" HeaderText="Item" />
                            <asp:BoundField DataField="ItemDescription" HeaderText="Description" ControlStyle-Width="120%" />
                            <asp:BoundField DataField="GodownName" HeaderText="GodownName" ControlStyle-Width="120%" />
                            <asp:BoundField DataField="LotNo" HeaderText="LotNo" ControlStyle-Width="120%" />
                            <asp:BoundField DataField="Qty" HeaderText="Qty" ControlStyle-Width="120%" />
                            <asp:TemplateField HeaderText="ReturnQty">
                                <ItemTemplate>
                                    <asp:TextBox ID="TxtReturnQty" runat="server" Text='<%# Bind("ReturnQty") %>' Width="70px"></asp:TextBox>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
