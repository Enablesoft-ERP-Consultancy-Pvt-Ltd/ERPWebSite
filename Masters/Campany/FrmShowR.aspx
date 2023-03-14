<%@ Page Language="C#" AutoEventWireup="true" CodeFile="FrmShowR.aspx.cs" Inherits="Masters_Campany_FrmShowR" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<script type="text/javascript">
    function Preview() {
        window.open('../../ReportViewer.aspx', '');
    }
</script>
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:Button ID="preview" runat="server" OnClick="preview_Click" Text="Preview" UseSubmitBehavior="False" />
        <asp:TextBox ID="txtname" runat="server" placeholder="Email"></asp:TextBox>
    </div>
    </form>
</body>
</html>
