<%@ Page Title="RATE MASTER" Language="C#" MasterPageFile="~/ERPmaster.master" AutoEventWireup="true"
    CodeFile="frmMottelingratemaster.aspx.cs" Inherits="Masters_Motteling_frmMottelingratemaster" %>

<%@ Register Src="~/UserControls/Motellingratemaster.ascx" TagPrefix="uc" TagName="Motellingratemaster" %>
<asp:Content ID="Content2" ContentPlaceHolderID="CPH_Form" runat="Server">
    <uc:Motellingratemaster ID="MotellingRateid" runat="server" />
</asp:Content>
