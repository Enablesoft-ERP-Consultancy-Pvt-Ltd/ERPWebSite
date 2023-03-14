<%@ Page Title="TCS Master Rate" Language="C#" AutoEventWireup="true" CodeFile="FrmTCS.aspx.cs"
    MasterPageFile="~/ERPmaster.master" Inherits="Masters_Carpet_FrmTCS" EnableEventValidation="false" %>

<%@ Register Src="~/UserControls/MasterTCS.ascx" TagPrefix="uc" TagName="MasterTCS" %>
<asp:Content ID="Content1" ContentPlaceHolderID="CPH_Form" runat="server">
    <uc:MasterTCS ID="TCSUserControl" runat="server" />
</asp:Content>
