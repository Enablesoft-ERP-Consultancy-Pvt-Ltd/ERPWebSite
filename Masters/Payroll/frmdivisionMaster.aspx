<%@ Page Title="Division master" Language="C#" MasterPageFile="~/ERPmaster.master"
    AutoEventWireup="true" CodeFile="frmdivisionMaster.aspx.cs" Inherits="Masters_Payroll_frmdivisionMaster" %>

<%@ Register Src="~/HRUserControls/Division.ascx" TagPrefix="uc" TagName="Division" %>
<asp:Content ID="Content2" ContentPlaceHolderID="CPH_Form" runat="Server">
    <div style="margin: 0% 20% 0% 20%">
        <div style="width: 100%">
            <uc:Division ID="Divisionid" runat="server" />
        </div>
    </div>
</asp:Content>
