<%@ Page Title="" Language="C#" AutoEventWireup="true" CodeFile="FrmFourNewParameter.aspx.cs"
    MasterPageFile="~/ERPmaster.master" Inherits="Masters_Carpet_FrmFourNewParameter"
    EnableEventValidation="false" %>

<%@ Register Src="~/UserControls/MasterFourNewParameter.ascx" TagPrefix="uc" TagName="FourNewParameter" %>
<asp:Content ID="Content1" ContentPlaceHolderID="CPH_Form" runat="server">
    <uc:FourNewParameter ID="FourNewParameterUserControl" runat="server" />
</asp:Content>
