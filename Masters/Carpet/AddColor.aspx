<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AddColor.aspx.cs" Inherits="Masters_Carpet_AddColor"
    EnableEventValidation="false" %>

<%@ Register Src="~/UserControls/MasterColor.ascx" TagPrefix="uc" TagName="MasterColor" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <uc:MasterColor ID="ColorUserControl" runat="server" />
    </div>
    </form>
</body>
</html>
