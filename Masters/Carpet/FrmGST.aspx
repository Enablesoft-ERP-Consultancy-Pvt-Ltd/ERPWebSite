<%@ Page Title="GST Master Rate" Language="C#" AutoEventWireup="true" CodeFile="FrmGST.aspx.cs"
    MasterPageFile="~/ERPmaster.master" Inherits="Masters_Carpet_FrmGST" EnableEventValidation="false" %>

<%@ Register Src="~/UserControls/MasterGST.ascx" TagPrefix="uc" TagName="MasterGST" %>
<asp:Content ID="Content1" ContentPlaceHolderID="CPH_Form" runat="server">
    <uc:MasterGST ID="GSTUserControl" runat="server" />
</asp:Content>
