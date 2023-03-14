<%@ Page Title="Sub Dept. master" Language="C#" MasterPageFile="~/ERPmaster.master"
    AutoEventWireup="true" CodeFile="frmsubdeptmaster.aspx.cs" Inherits="Masters_Payroll_frmsubdeptmaster" %>

<%@ Register Src="~/HRUserControls/Subdept.ascx" TagPrefix="uc" TagName="Subdept" %>
<asp:Content ID="Content2" ContentPlaceHolderID="CPH_Form" runat="Server">
    <div style="margin: 0% 20% 0% 20%">
        <div style="width: 100%">
            <uc:Subdept ID="Subdeptid" runat="server" />
        </div>
    </div>
</asp:Content>
