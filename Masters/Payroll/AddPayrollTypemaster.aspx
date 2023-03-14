<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AddPayrollTypemaster.aspx.cs"
    Inherits="Masters_Payroll_AddPayrollTypemaster" EnableEventValidation="false" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register Src="~/UserControls/PayrollTypeMaster.ascx" TagPrefix="uc" TagName="PayrollTypeMaster" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager ID="src1" runat="server">
    </asp:ScriptManager>
    <div>
        <uc:PayrollTypeMaster ID="ucpayrolltype" runat="server" />
    </div>
    </form>
</body>
</html>
