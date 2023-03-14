<%@ Page Language="C#" AutoEventWireup="true" CodeFile="FrmAttachConsumption.aspx.cs"
    Inherits="Masters_Carpet_FrmAttachConsumption" Title="Attach Consumption" %>

<%@ Register Src="~/UserControls/attachConsumption.ascx" TagName="attachConsumption"
    TagPrefix="uc2" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <link href="../../Styles/vijay.css" rel="Stylesheet" type="text/css" />
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <asp:Panel runat="server">
        <uc2:attachConsumption ID="uc1" runat="server" />
    </asp:Panel>
    </form>
</body>
</html>
