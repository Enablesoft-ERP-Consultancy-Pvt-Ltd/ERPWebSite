<%@ Page Title="Doc type master" Language="C#" MasterPageFile="~/ERPmaster.master"
    AutoEventWireup="true" CodeFile="frmdoctypemaster.aspx.cs" Inherits="Masters_Payroll_frmdoctypemaster" %>

<%@ Register Src="~/HRUserControls/DocType.ascx" TagPrefix="uc" TagName="DocType" %>
<asp:Content ID="Content2" ContentPlaceHolderID="CPH_Form" runat="Server">
    <div style="margin: 0% 20% 0% 20%">
        <div style="width: 100%">
            <uc:DocType ID="DocTypeid" runat="server" />
        </div>
    </div>
</asp:Content>
