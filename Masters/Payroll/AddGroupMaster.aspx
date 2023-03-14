<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AddGroupMaster.aspx.cs" Inherits="Masters_Payroll_AddGroupMaster"
    EnableEventValidation="false" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register Src="~/UserControls/PayrollGroupmaster.ascx" TagPrefix="uc" TagName="PayrollGroupmaster" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager ID="src1" runat="server">
    </asp:ScriptManager>
    <div>
        <uc:PayrollGroupmaster ID="ucpayroll" runat="server" />
    </div>
    </form>
</body>
</html>
