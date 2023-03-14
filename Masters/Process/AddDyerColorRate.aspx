<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AddDyerColorRate.aspx.cs"
    Inherits="Masters_Process_AddDyerColorRate" EnableEventValidation="false" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register Src="~/UserControls/MasterDyerColorRate.ascx" TagPrefix="uc" TagName="MasterDyerColorRate" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <uc:MasterDyerColorRate ID="usercontrol" runat="server" />
    </form>
</body>
</html>
