<%@ Page Language="C#" AutoEventWireup="true" CodeFile="frmWeaver.aspx.cs" ViewStateMode="Enabled"
    Inherits="Masters_Company_frmWeaver" MasterPageFile="~/ERPmaster.master" EnableEventValidation="false" %>

<%@ Register Src="~/UserControls/MasterEmployee.ascx" TagPrefix="uc" TagName="EmployeeUserControl" %>
<asp:Content ContentPlaceHolderID="CPH_Form" runat="server">
    <script src="../../Scripts/JScript.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery-1.4.1.js" type="text/javascript"></script>
    <div id="1" style="height: auto" align="left">
        <uc:EmployeeUserControl ID="EmployeeUserControl" runat="server" />
    </div>
</asp:Content>
