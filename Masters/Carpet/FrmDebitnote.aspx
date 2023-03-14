<%@ Page Title="DEBIT NOTE" Language="C#" MasterPageFile="~/ERPmaster.master" AutoEventWireup="true"
    EnableViewState="true" EnableEventValidation="false" EnableSessionState="True"
    CodeFile="FrmDebitnote.aspx.cs" Inherits="Masters_Carpet_FrmDebitnote" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPH_Form" runat="Server">
    <asp:UpdatePanel ID="up1" runat="server">
        <ContentTemplate>
            <table>
                <tr>
                    <td class="tdstyle">
                        <asp:CheckBox ID="ChkPurchase" runat="server" Text="Purchase" TabIndex="0" AutoPostBack="True" />
                    </td>
                    <td class="tdstyle">
                        <asp:CheckBox ID="ChkIndent" runat="server" Text="Indent" TabIndex="1" AutoPostBack="True" />
                    </td>
                    <td class="tdstyle">
                        <asp:CheckBox ID="ChkEdit" runat="server" Text="Edit" TabIndex="2" AutoPostBack="True"
                            OnCheckedChanged="ChkEdit_CheckedChanged" />
                    </td>
                </tr>
                <tr>
                    <td id="Td1" class="tdstyle">
                        Company Name<br />
                        <asp:DropDownList CssClass="dropdown" ID="ddCompName" runat="server" Width="115px"
                            AutoPostBack="true" TabIndex="3">
                        </asp:DropDownList>
                    </td>
                    <td id="Td2" class="tdstyle">
                        Customer Name<br />
                        <asp:DropDownList CssClass="dropdown" ID="DDCustomer" runat="server" Width="115px"
                            AutoPostBack="true" TabIndex="4">
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:Label ID="LblItemCode" runat="server" Text="Item Code"></asp:Label>
                        <br />
                        <asp:DropDownList ID="DDItemCode" runat="server" Width="150px" TabIndex="5" CssClass="dropdown"
                            AutoPostBack="true" OnSelectedIndexChanged="DDItemCode_SelectedIndexChanged">
                        </asp:DropDownList>
                        <cc1:ListSearchExtender ID="Listsearchextender8" runat="server" TargetControlID="DDItemCode"
                            ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                        </cc1:ListSearchExtender>
                    </td>
                    <td>
                        <asp:Label ID="LblFinish" runat="server" Text="Color"></asp:Label>
                        <br />
                        <asp:DropDownList ID="DDFinish" runat="server" Width="150px" TabIndex="6" CssClass="dropdown"
                            AutoPostBack="true" OnSelectedIndexChanged="DDFinish_SelectedIndexChanged">
                        </asp:DropDownList>
                        <cc1:ListSearchExtender ID="Listsearchextender3" runat="server" TargetControlID="DDFinish"
                            ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                        </cc1:ListSearchExtender>
                    </td>
                    <td>
                        <asp:Label ID="Label1" runat="server" Text="Order No"></asp:Label>
                        <br />
                        <asp:DropDownList ID="ddOrder" runat="server" Width="150px" TabIndex="7" AutoPostBack="true"
                            CssClass="dropdown" OnSelectedIndexChanged="ddOrder_SelectedIndexChanged">
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:Label ID="LblChallanno" runat="server" Text="ChallanNo"></asp:Label>
                        <br />
                        <asp:DropDownList ID="DDChallanNo" runat="server" Width="120px" TabIndex="8" CssClass="dropdown"
                            AutoPostBack="true" OnSelectedIndexChanged="DDChallanNo_SelectedIndexChanged">
                        </asp:DropDownList>
                        <cc1:ListSearchExtender ID="Listsearchextender2" runat="server" TargetControlID="DDChallanNo"
                            ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                        </cc1:ListSearchExtender>
                    </td>
                    <td runat="server" id="TDDNno" visible="false">
                        <asp:Label ID="LblDNno" runat="server" Text="DebitNote No"></asp:Label>
                        <br />
                        <asp:DropDownList ID="DDDNno" runat="server" Width="120px" CssClass="dropdown" AutoPostBack="true"
                            OnSelectedIndexChanged="DDDNno_SelectedIndexChanged">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="LblParty" runat="server" Text="Vendor"></asp:Label>
                        <br />
                        <asp:DropDownList ID="DDParty" runat="server" Width="150px" TabIndex="9" AutoPostBack="true"
                            CssClass="dropdown" OnSelectedIndexChanged="DDParty_SelectedIndexChanged">
                        </asp:DropDownList>
                        <cc1:ListSearchExtender ID="Listsearchextender1" runat="server" TargetControlID="DDParty"
                            ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                        </cc1:ListSearchExtender>
                    </td>
                    <td>
                        <asp:Label ID="LblQty" CssClass="labelbold" Text="Qty" runat="server"></asp:Label>
                        <br />
                        <asp:TextBox ID="txtQty" runat="server" CssClass="textb" TabIndex="10" OnTextChanged="txtQty_TextChanged"
                            AutoPostBack="true"></asp:TextBox>
                    </td>
                    <td>
                        <asp:Label ID="LblRate" CssClass="labelbold" Text="Rate" runat="server"></asp:Label>
                        <br />
                        <asp:TextBox ID="TxtRate" runat="server" CssClass="textb" TabIndex="11" OnTextChanged="TxtRate_TextChanged"
                            AutoPostBack="true"></asp:TextBox>
                    </td>
                    <td>
                        <asp:Label ID="LblAmount" CssClass="labelbold" Text="Amount" runat="server"></asp:Label>
                        <br />
                        <asp:TextBox ID="TxtAmount" runat="server" CssClass="textb" TabIndex="10"></asp:TextBox>
                    </td>
                    <td id="Td5" align="left" class="tdstyle">
                        Date<br />
                        <asp:TextBox ID="txtdate" runat="server" TabIndex="12" Width="120px" CssClass="textb"></asp:TextBox>
                        <asp:CalendarExtender ID="CalendarExtender1" runat="server" Format="dd-MMM-yyyy"
                            TargetControlID="txtdate">
                        </asp:CalendarExtender>
                    </td>
                    <td id="Td3" align="left" class="tdstyle">
                        Remarks<br />
                        <asp:TextBox ID="Txtremarks" runat="server" TabIndex="13" Width="150px" TextMode="MultiLine"
                            CssClass="textb"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td id="tdsubmit" runat="server" colspan="1" align="Left">
                        <asp:Button ID="btnSave" runat="server" Text="Save" TabIndex="14" CssClass="buttonnorm"
                            OnClick="btnSave_Click" />
                    </td>
                </tr>
                <tr>
                    <td colspan="6">
                        <asp:GridView ID="DGDebitNote" runat="server" Width="100%" CssClass="grid-view" DataKeyNames="ID"
                            AutoGenerateColumns="false" OnRowCreated="DGDebitNote_RowCreated" OnRowDataBound="DGDebitNote_RowDataBound"
                            OnSelectedIndexChanged="DGDebitNote_SelectedIndexChanged" OnRowDeleting="DGDebitNote_RowDeleting"
                            OnRowCommand="DGDebitNote_RowCommand">
                            <HeaderStyle CssClass="gvheader" />
                            <AlternatingRowStyle CssClass="gvalt" />
                            <RowStyle CssClass="gvrow" />
                            <EmptyDataRowStyle CssClass="gvemptytext" />
                            <Columns>
                                <asp:BoundField DataField="ID" HeaderText="DEBITNOTE NO" />
                                <asp:BoundField DataField="PRODUCTCODE" HeaderText="PRODUCT CODE" />
                                <asp:BoundField DataField="ORDERNO" HeaderText="ORDERNO" />
                                <asp:BoundField DataField="CHALLANNO" HeaderText="CHALLANNO" />
                                <asp:BoundField DataField="EMPNAME" HeaderText="EMPNAME" />
                                <asp:BoundField DataField="QTY" HeaderText="QTY" />
                                <asp:BoundField DataField="RATE" HeaderText="RATE" />
                                <asp:BoundField DataField="AMOUNT" HeaderText="AMOUNT" />
                                <asp:BoundField DataField="DATE" HeaderText="DATE" />
                                <asp:BoundField DataField="REMARKS" HeaderText="REMARKS" />
                                <asp:BoundField DataField="TYPE" HeaderText="TYPE" />
                                <asp:TemplateField Visible="true">
                                    <ItemTemplate>
                                        <asp:Button ID="BtnDelete" CssClass="buttonnorm" runat="server" Text="DEL" OnClientClick="return confirm('DO YOU WANT TO DELETE?')"
                                            CausesValidation="False" CommandName="Delete" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="CPH_SideBar" runat="Server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="CPH_Footer" runat="Server">
</asp:Content>
