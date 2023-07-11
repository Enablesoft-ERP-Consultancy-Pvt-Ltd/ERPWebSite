<%@ Page Language="C#" AutoEventWireup="true" CodeFile="frmSignature.aspx.cs" Inherits="Masters_Campany_frmSignature"
    EnableEventValidation="false" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register Src="~/UserControls/ucmenu.ascx" TagName="ucmenu" TagPrefix="uc2" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="../../Styles/vijay.css" rel="stylesheet" type="text/css" />
    <script src="../../Scripts/JScript.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery-1.4.1.js" type="text/javascript"></script>
    <script type="text/javascript">
        function closeform() {
            window.opener.document.getElementById('CPH_Form_Button2').click();
            self.close();

        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager ID="scriptmanager1" runat="server">
    </asp:ScriptManager>
    <div style="margin-left: 15%; margin-right: 15%; margin-top: 5%">
        <table>
            <tr>
            </tr>
        </table>
        <asp:UpdatePanel ID="updatepanel1" runat="server">
            <ContentTemplate>
                <table>
                    <tr>
                        <td>
                            <asp:Label ID="lbl1" Text="Signatory" runat="server" color="#1E1E1E" Font-Bold="true" />
                            &nbsp;
                            <asp:TextBox ID="TxtSignature" runat="server" CssClass="textb" OnTextChanged="TxtSignature_TextChanged">
                            </asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="TxtSignature"
                                ErrorMessage="RequiredFieldValidator" ForeColor="Red" ValidationGroup="m">*</asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="LblErrer" runat="server" ForeColor="Red"></asp:Label>
                        </td>
                        <tr>
                            <td class="style2">
                                <div id="divgride" runat="server" style="height: 150px; border-width: medium; border-color: Black;">
                                    <asp:GridView ID="DGSignature" runat="server" AutoGenerateColumns="False" DataKeyNames="ID"
                                        OnRowDataBound="DGSignature_RowDataBound" OnSelectedIndexChanged="DGSignature_SelectedIndexChanged"
                                        Width="280px" AllowSorting="True" OnRowCreated="DGSignature_RowCreated">
                                        <HeaderStyle CssClass="gvheaders" />
                                        <AlternatingRowStyle CssClass="gvalts" />
                                        <Columns>
                                            <asp:BoundField DataField="Id" HeaderText="Sr.No" />
                                            <asp:BoundField DataField="SignatoryName" HeaderText="Signature" />
                                        </Columns>
                                    </asp:GridView>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td align="right">
                                <asp:Button ID="btndelete" runat="server" CssClass="buttonnorm" Text="Delete" OnClientClick="return confirm('Do you want to Delete data?')"
                                    Visible="False" OnClick="btndelete_Click" />
                                <asp:Button ID="btnsave" runat="server" CssClass="buttonnorm" OnClientClick="return confirm('Do you want to save data?')"
                                    OnClick="btnsave_Click" Text="Save" ValidationGroup="m" />
                                <asp:Button ID="btnclose" runat="server" CssClass="buttonnorm" OnClientClick="return closeform();"
                                    Text="Close" />
                            </td>
                        </tr>
                    </tr>
                </table>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    </form>
</body>
</html>
