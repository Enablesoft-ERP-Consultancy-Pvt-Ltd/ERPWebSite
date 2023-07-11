<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AddEmpType.aspx.cs" Inherits="Masters_Campany_AddEmpType" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="../../App_Themes/Default/Style.css" rel="stylesheet" type="text/css" />
    <script src="../../Scripts/JScript.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery-1.4.1.js" type="text/javascript"></script>
    <script type="text/javascript">
        function CloseForm() {
            window.opener.document.getElementById("addtype").click();
            self.close();
        }    
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <table>
            <tr>
                <td>
                    <asp:TextBox ID="TxtId" Visible="false" CssClass="textb" runat="server"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="tdstyle">
                    Employee Type
                </td>
                <td>
                    <asp:TextBox ID="TxtEmpType" CssClass="textb" runat="server"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <asp:Label ID="lblErr" runat="server" Font-Bold="true" ForeColor="Red"></asp:Label>
                </td>
            </tr>
            <tr>
                <td colspan="2" align="right">
                    <asp:Button ID="BtnSave" runat="server" Text="Save" OnClientClick="alert('Do you want to save');"
                        CssClass="buttonnorm" OnClick="BtnSave_Click" />
                    <asp:Button ID="BtnClose" runat="server" OnClientClick="CloseForm();" CssClass="buttonnorm"
                        Text="Close" />
                </td>
            </tr>
            <tr>
                <td colspan="2" align="center">
                    <asp:GridView ID="DGEmpType" AllowPaging="true" OnRowDataBound="DGEmpType_RowDataBound"
                        OnPageIndexChanging="DGEmpType_PageIndexChanging" runat="server" CssClass="grid-view"
                        OnRowCreated="DGEmpType_RowCreated">
                        <HeaderStyle CssClass="gvheader" />
                        <AlternatingRowStyle CssClass="gvalt" />
                        <RowStyle CssClass="gvrow" />
                        <EmptyDataRowStyle CssClass="gvemptytext" />
                    </asp:GridView>
                </td>
            </tr>
            <tr>
                <td>
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
