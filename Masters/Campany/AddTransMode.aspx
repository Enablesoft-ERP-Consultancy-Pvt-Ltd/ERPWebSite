<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AddTransMode.aspx.cs" Inherits="Masters_Campany_AddTransMode"
    EnableEventValidation="false" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="../../Styles/vijay.css" rel="stylesheet" type="text/css" />
    <script src="../../Scripts/JScript.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery-1.4.1.js" type="text/javascript"></script>
    <script type="text/javascript">
        function CloseForm() {
            var Opener = window.opener;
            if (Opener != null) {
                window.opener.document.getElementById('CPH_Form_transmode').click();
                self.close();

            }
            else {
                window.opener.document.getElementById('transmode').click();
                self.close();
            }

        }
       
        
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div style="margin-right: 15%">
        <table>
            <tr>
                <td class="style2">
                </td>
                <td>
                    <asp:TextBox ID="txtid" runat="server" Visible="False"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="Label1" Text="Transmode Name" runat="server" Font-Bold="true" />
                </td>
                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                <td>
                    <asp:TextBox ID="txtname" CssClass="textb" runat="server" Width="250px"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtname"
                        ErrorMessage="RequiredFieldValidator" ValidationGroup="m" ForeColor="Red">*</asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr>
                <td class="style2">
                    &nbsp;
                </td>
                <td>
                    <asp:Label ID="lbl" runat="server" ForeColor="Red"></asp:Label>
                </td>
            </tr>
            <tr>
                <td class="style2">
                    &nbsp;
                </td>
                <td class="style2" colspan="2">
                    <div style="width: 100%; height: 150px; overflow: auto">
                        <asp:GridView ID="gvtransmode" runat="server" Width="285px" OnSelectedIndexChanged="gvtransmode_SelectedIndexChanged"
                            OnRowDataBound="gvtransmode_RowDataBound" DataKeyNames="Sr_No" CssClass="grid-views"
                            OnRowCreated="gvtransmode_RowCreated">
                            <HeaderStyle CssClass="gvheaders" />
                            <AlternatingRowStyle CssClass="gvalts" />
                            <RowStyle CssClass="gvrow" />
                            <EmptyDataRowStyle CssClass="gvemptytext" />
                        </asp:GridView>
                    </div>
                </td>
            </tr>
            <tr>
                <td class="style2">
                    &nbsp;
                </td>
                <td colspan="1" align="right">
                    <asp:Button ID="btnsave" runat="server" CssClass="buttonnorm" OnClick="btnsave_Click"
                        Text="Save" ValidationGroup="m" />
                    <asp:Button ID="btnclose" runat="server" Text="Close" CssClass="buttonnorm" OnClientClick="return CloseForm();" />
                    <asp:Button ID="btndelete" runat="server" OnClick="btndelete_Click" Text="Delete"
                        Visible="False" CssClass="buttonnorm" />
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
