<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ContinentMaster.aspx.cs"
    Inherits="Masters_Campany_ContinentMaster" EnableEventValidation="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="head1" runat="server">
    <meta content="blendTrans(Duration=0.5)" http-equiv="Page-Enter" />
    <meta content="blendTrans(Duration=0.5)" http-equiv="Page-Exit" />
    <title></title>
    <script src="../../Scripts/JScript.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery-1.4.1.js" type="text/javascript"></script>
    <link href="../../Styles/vijay.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript">
        function closecontinent() {
            window.opener.document.getElementById('CPH_Form_Continent').click();
            self.close();
        }
    </script>
    <script type="text/javascript">


        function getbacktostepone() {
            window.location = "ContinentMaster.aspx";
        }
        function onSuccess() {
            setTimeout(okay, 200);
        }
        function onError() {
            setTimeout(getbacktostepone, 200);
        }
        function okay() {
            window.parent.document.getElementById('btnOkay').click();
        }
        function cancel() {
            window.parent.document.getElementById('btnCancel').click();
        }
        function priview() {
            window.open('../../ReportViewer.aspx', '');

        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager ID="scriptmanager1" runat="server">
    </asp:ScriptManager>
    <div style="margin-left: 15%; margin-right: 15%">
        <table>
            <tr>
                <td>
                    <asp:TextBox ID="txtid" runat="server" CssClass="textb" Visible="False"></asp:TextBox>
                </td>
            </tr>
        </table>
        <asp:UpdatePanel ID="updatepanel1" runat="server">
            <ContentTemplate>
                <table>
                    <tr>
                        <td class="tdstyle">
                            <asp:Label ID="lbl11" Text="Continent Name" runat="server" Font-Bold="true" />
                        </td>
                        <td>
                            <asp:TextBox ID="txtContinent" runat="server" CssClass="textb">
                            </asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="Please Enter Continent Name"
                                ControlToValidate="txtContinent" ValidationGroup="f1" ForeColor="Red">*</asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdstyle">
                            <asp:Label ID="Label1" Text="Continent Code" runat="server" Font-Bold="true" />
                        </td>
                        <td>
                            <asp:TextBox ID="txtContinentCode" runat="server" CssClass="textb">
                            </asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="Please Enter Continent Code"
                                ControlToValidate="txtContinentCode" ValidationGroup="f1" ForeColor="Red">*</asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td class="style2" colspan="2">
                            <div style="width: 100%; height: 100px; overflow: auto">
                                <asp:Label ID="lblerr" runat="server" ForeColor="Red"></asp:Label>
                                <asp:GridView ID="gdContinent" runat="server" Width="250px" OnRowDataBound="gdContinent_RowDataBound"
                                    OnSelectedIndexChanged="gdContinent_SelectedIndexChanged" DataKeyNames="ContinentId"
                                    CssClass="grid-views" OnRowCreated="gdContinent_RowCreated">
                                    <HeaderStyle CssClass="gvheaders" />
                                    <AlternatingRowStyle CssClass="gvalts" />
                                    <RowStyle CssClass="gvrow" />
                                    <EmptyDataRowStyle CssClass="gvemptytext" />
                                </asp:GridView>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2" align="right">
                            <asp:Button ID="btnsave" runat="server" CssClass="buttonnorm" Text="Save" OnClick="btnsave_Click"
                                ValidationGroup="f1" />
                            <asp:Button ID="btnclose" runat="server" CssClass="buttonnorm" Text="Close" ValidationGroup="f1"
                                CausesValidation="false" OnClientClick="return closecontinent();" />
                            <asp:Button ID="btndelete" runat="server" Text="Delete" CssClass="buttonnorm" OnClientClick="return confirm('Do you want to Delete data?')"
                                OnClick="btndelete_Click" Visible="False" />
                        </td>
                    </tr>
                </table>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    </form>
</body>
</html>
