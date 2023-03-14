<%@ Page Title="" Language="C#" MasterPageFile="~/ERPmaster.master" AutoEventWireup="true"
    CodeFile="FrmConvertImage.aspx.cs" Inherits="Masters_Carpet_FrmConvertImage" %>

<asp:Content ID="Content2" ContentPlaceHolderID="CPH_Form" runat="Server">
    <table>
        <tr>
            <td>
                <asp:DropDownList ID="DDlType" runat="server">
                    <asp:ListItem Text="DraftOrder" Value="0"></asp:ListItem>
                    <asp:ListItem Text="OrderMaster" Value="1"></asp:ListItem>
                    <asp:ListItem Text="Item" Value="2"></asp:ListItem>
                    <asp:ListItem Text="Draft Refresh" Value="3"></asp:ListItem>
                    <asp:ListItem Text="Order Refresh" Value="4"></asp:ListItem>
                    <asp:ListItem Text="Item" Value="2"></asp:ListItem>
                </asp:DropDownList>
            </td>
            <td>
                <asp:Button ID="Button1" runat="server" Text="Button" OnClick="Button1_Click" />
            </td>
        </tr>
    </table>
</asp:Content>
