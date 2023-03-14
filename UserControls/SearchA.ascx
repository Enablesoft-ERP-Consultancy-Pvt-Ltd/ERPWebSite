<%@ Control Language="C#" AutoEventWireup="true" CodeFile="SearchA.ascx.cs" Inherits="UserControls_SearchA" %>

<style type="text/css">
    .style1
    {
        width: 100px;
    }
    .style2
    {
        width: 85px;
    }
</style>

<asp:UpdatePanel ID="up1" ChildrenAsTriggers="False" runat="server" UpdateMode="Conditional" >
<ContentTemplate>
<table >
    <tr>
        <td >Item</td>
        <td >
            <asp:DropDownList ID="ddlItem" runat="server" AutoPostBack="True" 
            OnSelectedIndexChanged="ddlItem_SelectedIndexChanged" CssClass="dropdownnormalsearch"></asp:DropDownList>
        </td>
    </tr>
    <tr>
        <td>Quality</td>
        <td >
            <asp:DropDownList ID="ddlQuality" runat="server" AutoPostBack="True" 
            CssClass="dropdownnormalsearch"></asp:DropDownList>
        </td>
    </tr>
     <tr>
        <td >Design</td>
        <td>
            <asp:DropDownList ID="ddlDesign" runat="server" AutoPostBack="True" 
            OnSelectedIndexChanged="ddlDesign_SelectedIndexChanged" CssClass="dropdownnormalsearch"></asp:DropDownList>
        </td>
    </tr>
    <tr>
        <td>Color</td>
        <td >
            <asp:DropDownList ID="ddlColor" runat="server" AutoPostBack="True" 
            CssClass="dropdownnormalsearch"></asp:DropDownList>
        </td>
    </tr>    
     <tr>
        <td >Shape</td>
        <td >
            <asp:DropDownList ID="ddlShape" runat="server" AutoPostBack="True" 
            OnSelectedIndexChanged="ddlShape_SelectedIndexChanged" CssClass="dropdownnormalsearch"></asp:DropDownList>
        </td>
    </tr>
     <tr>
        <td>Size</td>
        <td >
            <asp:DropDownList ID="ddlSize" runat="server" AutoPostBack="True" 
            CssClass="dropdownnormalsearch"></asp:DropDownList>
        </td>
    </tr>
     
     <tr>
        <td colspan="2">
             <asp:Button ID="btnSearch" runat="server" onclick="btnSearch_Click" 
                Text="Search" CausesValidation="False" CssClass="buttonnorm"/>
        </td>
    </tr>
</table>
</ContentTemplate>
</asp:UpdatePanel>