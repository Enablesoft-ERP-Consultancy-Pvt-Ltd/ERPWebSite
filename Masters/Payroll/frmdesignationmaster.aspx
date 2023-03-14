<%@ Page Title="Designation master" Language="C#" MasterPageFile="~/ERPmaster.master"
    AutoEventWireup="true" CodeFile="frmdesignationmaster.aspx.cs" Inherits="Masters_Payroll_frmdesignationmaster" %>

<%@ Register Src="~/HRUserControls/Designationmaster.ascx" TagPrefix="uc" TagName="Designationmaster" %>
<asp:Content ID="Content2" ContentPlaceHolderID="CPH_Form" runat="Server">
    <div style="margin: 0% 20% 0% 20%">
        <div style="width: 100%">
            <uc:Designationmaster ID="Designationmasterid" runat="server" />
        </div>
    </div>
</asp:Content>
