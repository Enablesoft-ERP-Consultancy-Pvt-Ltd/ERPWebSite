<%@ Page Language="C#" AutoEventWireup="true" CodeFile="FinishType.aspx.cs" EnableEventValidation="false"
    Inherits="Masters_Carpet_FinishType" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="../../App_Themes/Default/Style.css" rel="stylesheet" type="text/css" />
</head>
<script src="../../Scripts/JScript.js" type="text/javascript"></script>
<script src="../../Scripts/jquery-1.4.1.js" type="text/javascript"></script>
<script type="text/javascript">
    function closeForm() {
        window.opener.document.getElementById('refreshfinishedin').click();
        self.close();
    }
      
</script>
<body>
    <form id="form1" runat="server">
    <div align="center">
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <table>
            <tr>
                <td>
                    <asp:TextBox ID="txtid" runat="server" Visible="false"></asp:TextBox>
                </td>
                <td class="tdstyle">
                    FINISH TYPE
                </td>
                <td>
                    <asp:TextBox ID="txtfinishtype" runat="server"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtfinishtype"
                        ErrorMessage="please Enter Name" ForeColor="Red" SetFocusOnError="true" ValidationGroup="f1">*</asp:RequiredFieldValidator>
                </td>
                <td class="tdstyle">
                    SHORT NAME
                </td>
                <td>
                    <asp:TextBox ID="txtshortnsme" runat="server"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtshortnsme"
                        ErrorMessage="please Enter Short Name" ForeColor="Red" SetFocusOnError="true"
                        ValidationGroup="f1">*</asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr>
                <td colspan="5" align="center">
                    <div style="width: 90%; height: 200px; overflow: scroll;">
                        <asp:GridView ID="gvfinishtype" runat="server" DataKeyNames="id" OnSelectedIndexChanged="gvfinishtype_SelectedIndexChanged"
                            OnRowDataBound="gvfinishtype_RowDataBound" AutoGenerateColumns="False" CssClass="grid-view"
                            OnRowCreated="gvfinishtype_RowCreated">
                            <HeaderStyle Font-Bold="true" CssClass="gvheader" />
                            <AlternatingRowStyle CssClass="gvalt" />
                            <RowStyle CssClass="gvrow" />
                            <Columns>
                                <asp:BoundField DataField="id" HeaderText="SR_NO" />
                                <asp:BoundField DataField="FINISHED_TYPE_NAME" HeaderText="FINISH NAME" />
                                <asp:BoundField DataField="SHORT_NAME" HeaderText="SHORT NAME" />
                            </Columns>
                            <EmptyDataRowStyle CssClass="gvemptytext" />
                        </asp:GridView>
                    </div>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lblerror" runat="server" Text="Plz fill the field" ForeColor="Red"
                        Visible="false"></asp:Label>
                </td>
                <td>
                    <asp:Label ID="lblMessage" runat="server" ForeColor="Red"></asp:Label>
                </td>
            </tr>
            <tr>
                <td colspan="5" align="right">
                    <asp:Button ID="btnsave" runat="server" Text="Save" CssClass="buttonnorm" OnClick="btnsave_Click"
                        OnClientClick="return confirm('Do you want to save data?')" />
                    <asp:Button ID="btnnew" runat="server" Text="New" CssClass="buttonnorm" OnClick="btnnew_Click" />
                    <asp:Button ID="btnclose" runat="server" Text="Close" CssClass="buttonnorm" OnClientClick="return closeForm();" />
                    <asp:Button ID="btndelete" runat="server" Text="Delete" OnClick="btndelete_Click"
                        CssClass="buttonnorm" />
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
