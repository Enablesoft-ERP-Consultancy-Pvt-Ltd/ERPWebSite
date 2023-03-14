<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AddShadeColor.aspx.cs" Inherits="Masters_Carpet_AddShadeColor"
    EnableEventValidation="false" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="../../Styles/vijay.css" rel="stylesheet" type="text/css" />
    <script src="../../Scripts/JScript.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery-1.4.1.min.js" type="text/javascript"></script>
    <script type="text/javascript">
        function CloseForm() {

            if (window.opener.document.getElementById('CPH_Form_refreshshade')) {
                window.opener.document.getElementById('CPH_Form_refreshshade').click();

            }
            else if (window.opener.document.getElementById('refreshshade')) {
                window.opener.document.getElementById('refreshshade').click();
            }
            else if (window.opener.document.getElementById('btnrefreshshadecolorform')) {
                window.opener.document.getElementById('btnrefreshshadecolorform').click();

            }
            else if (window.opener.document.getElementById('CPH_Form_btnrefreshshadecolorform')) {
                window.opener.document.getElementById('CPH_Form_btnrefreshshadecolorform').click();
            }
            else if (window.opener.document.getElementById('CPH_Form_refreshshadecolor')) {
                window.opener.document.getElementById('CPH_Form_refreshshadecolor').click();

            }
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
                </td>
                <td>
                    <asp:TextBox CssClass="textb" ID="txtid" runat="server" Visible="false"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="tdstyle">
                    <asp:Label ID="Label1" runat="server" Text="Color Box" CssClass="labelbold"></asp:Label>
                </td>
                <td>
                    <asp:TextBox CssClass="textb" ID="txtColorBox" runat="server" onkeydown="return (event.keyCode!=13);"
                        Width="200px"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="tdstyle">
                    <asp:Label ID="lblshadecolorname" runat="server" Text="Shade Color Name" CssClass="labelbold"></asp:Label>
                </td>
                <td>
                    <asp:TextBox CssClass="textb" ID="txtcolor" runat="server" Width="200px"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="tdstyle">
                    <asp:Label ID="Label2" runat="server" Text="Color" CssClass="labelbold"></asp:Label>
                </td>
                <td>
                    <asp:TextBox CssClass="textb" ID="txtShadeColor" runat="server" onkeydown="return (event.keyCode!=13);"
                        Width="200px"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td colspan="2" style="text-align: right">
                    <asp:Button ID="btnsave" runat="server" Text="Save" OnClick="btnsave_Click" OnClientClick="return confirm('Do you want to save data?')"
                        CssClass="buttonnorm" />
                    <asp:Button ID="btnnew" runat="server" Text="New" OnClick="btnnew_Click" CssClass="buttonnorm" />
                    <asp:Button ID="btnclose" runat="server" Text="Close" OnClientClick="return CloseForm();"
                        CssClass="buttonnorm" />
                    <asp:Button ID="btndelete" runat="server" Text="Delete" OnClick="btndelete_Click"
                        CssClass="buttonnorm" />
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <asp:Label ID="lblMessage" runat="server" ForeColor="Red"></asp:Label>
                    <div style="width: 90%; height: 400px; overflow: scroll;">
                        <asp:GridView ID="gdshadecolor" runat="server" OnRowDataBound="gdshadecolor_RowDataBound"
                            DataKeyNames="Sr_No" OnSelectedIndexChanged="gdshadecolor_SelectedIndexChanged"
                            CssClass="grid-views" OnRowCreated="gdshadecolor_RowCreated">
                        </asp:GridView>
                    </div>
                </td>
            </tr>
            <tr>
                <td colspan="2" style="font-family: Times New Roman; font-size: 18px">
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
