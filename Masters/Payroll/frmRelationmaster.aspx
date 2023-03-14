<%@ Page Title="Relation master" Language="C#" MasterPageFile="~/ERPmaster.master"
    AutoEventWireup="true" CodeFile="frmRelationmaster.aspx.cs" Inherits="Masters_Payroll_frmRelationmaster" %>

<%@ Register Src="~/HRUserControls/Relation.ascx" TagPrefix="uc" TagName="Relation" %>
<asp:Content ID="Content2" ContentPlaceHolderID="CPH_Form" runat="Server">
    <div style="margin: 0% 20% 0% 20%">
        <div style="width: 100%">
            <uc:Relation ID="Relationid" runat="server" />
        </div>
    </div>
</asp:Content>
