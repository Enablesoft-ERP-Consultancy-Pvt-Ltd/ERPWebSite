<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AddRecipeName.aspx.cs" Inherits="Masters_Carpet_AddRecipeName"
    EnableEventValidation="false" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script src="../../Scripts/JScript.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery-1.4.1.js" type="text/javascript"></script>
    <link href="../../Styles/vijay.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript">
        function CloseForm() {

            if (window.opener.document.getElementById('CPH_Form_RefreshRecipeName')) {
                window.opener.document.getElementById('CPH_Form_RefreshRecipeName').click();
                self.close();
            }
            else if (window.opener.document.getElementById('RefreshRecipeName')) {
                window.opener.document.getElementById('RefreshRecipeName').click();
                self.close();
            }
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <table>
                    <tr>
                        <td class="tdstyle">
                            <asp:Label ID="lblRecipeName" runat="server" Text="Recipe Name" CssClass="labelbold"></asp:Label>
                            &nbsp;<asp:TextBox CssClass="textb" ID="TxtRecipeName" runat="server" Width="200px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="Lblerrer" runat="server" ForeColor="Red" Text="" CssClass="labelbold"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: right">
                            <asp:Button ID="btnNew" runat="server" CssClass="buttonnorm" OnClick="btnNew_Click"
                                Text="New" Width="56px" />
                            <asp:Button ID="btnSave" runat="server" CssClass="buttonnorm" OnClick="btnSave_Click"
                                OnClientClick="return confirm('Do you want to save data?')" Text="Save" Width="56px" />
                            <asp:Button ID="btnclose" runat="server" CssClass="buttonnorm" OnClientClick="return  CloseForm();"
                                Text="Close" Width="48px" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <div style="width: 100%; height: 200px; overflow: scroll">
                                <asp:GridView ID="DGRecipeName" runat="server" DataKeyNames="ID" OnRowDataBound="DGRecipeName_RowDataBound"
                                    OnSelectedIndexChanged="DGRecipeName_SelectedIndexChanged" CssClass="grid-views"
                                    AutoGenerateColumns="False">
                                    <HeaderStyle CssClass="gvheaders" />
                                    <AlternatingRowStyle CssClass="gvalts" />
                                    <Columns>
                                        <asp:BoundField DataField="Name" HeaderText="Recipe Name">
                                            <ItemStyle Width="200px" HorizontalAlign="Center" VerticalAlign="Middle" />
                                        </asp:BoundField>
                                    </Columns>
                                    <EmptyDataRowStyle CssClass="gvemptytext" />
                                </asp:GridView>
                            </div>
                        </td>
                    </tr>
                </table>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    </form>
</body>
</html>
