<%@ Page Title="Weaver Raw SubItem Rate" Language="C#" AutoEventWireup="true" CodeFile="WeaverRawSubItemRate.aspx.cs"
    MasterPageFile="~/ERPmaster.master" Inherits="Masters_RawMaterial_WeaverRawSubItemRate"
    EnableEventValidation="false" %>

<%@ Register Src="~/UserControls/MasterWeaverRawSubItemRate.ascx" TagPrefix="uc"
    TagName="MasterWeaverRawSubItemRate" %>
<asp:Content ID="Content1" ContentPlaceHolderID="CPH_Form" runat="server">
    <uc:MasterWeaverRawSubItemRate ID="ColorUserControl" runat="server" />
</asp:Content>
