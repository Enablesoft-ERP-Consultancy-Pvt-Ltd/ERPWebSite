<%@ Page Title="Color" Language="C#" AutoEventWireup="true" CodeFile="Color.aspx.cs"
    MasterPageFile="~/ERPmaster.master" Inherits="Masters_Carpet_Color" EnableEventValidation="false" %>

<%@ Register Src="~/UserControls/MasterColor.ascx" TagPrefix="uc" TagName="MasterColor" %>
<asp:Content ID="Content1" ContentPlaceHolderID="CPH_Form" runat="server">
    <uc:MasterColor ID="ColorUserControl" runat="server" />
</asp:Content>
