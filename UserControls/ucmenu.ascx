<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ucmenu.ascx.cs" Inherits="UserControls_ucmenu" %>
<%--<link href="Styles/vijay.css" rel="stylesheet" type="text/css" />--%>
<style type="text/css">
    .toolbar
    {
        font-size: 13px;
        font-family: Arial;
        padding: 2px;
        filter: progid:DXImageTransform.Microsoft.Gradient(gradientType=0,startColorStr=blue,endColorStr=lightblue);
    }
   
</style>
<asp:Menu ID="menu" runat="server" DataSourceID="xmlDataSource" DynamicHorizontalOffset="2"
    Font-Names="Arial" Font-Size="11px" Font-Bold="true" ForeColor="black  " Orientation="Horizontal"
    StaticSubMenuIndent="10px" CssClass="menuss">
    <DataBindings>
        <asp:MenuItemBinding DataMember="MenuItem" NavigateUrlField="NavigateUrl" TextField="Text"
            ToolTipField="ToolTip" SelectableField="Selectable" />
    </DataBindings>
    <DynamicHoverStyle BackColor="#8B7355" ForeColor="White" />
    <DynamicMenuItemStyle HorizontalPadding="5px" VerticalPadding="4px" />
    <DynamicMenuStyle BackColor="#D2B48C" />
    <DynamicSelectedStyle BackColor="#F5F5F5" />
    <StaticHoverStyle BackColor="#C1CDCD" ForeColor="White" />
    <StaticMenuItemStyle HorizontalPadding="5px" VerticalPadding="4px" />
    <StaticSelectedStyle BackColor="#F5F5DC" />
</asp:Menu>
<asp:XmlDataSource ID="xmlDataSource" TransformFile="~/TransformXSLT.xslt" XPath="MenuItems/MenuItem"
    runat="server" />
