<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AddProcess.aspx.cs" Inherits="Masters_Campany_Design"
    EnableEventValidation="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register Src="~/UserControls/ucmenu.ascx" TagName="ucmenu" TagPrefix="uc2" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta content="blendTrans(Duration=0.5)" http-equiv="Page-Enter" />
    <meta content="blendTrans(Duration=0.5)" http-equiv="Page-Exit" />
    <title></title>
    <script src="../../Scripts/JScript.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery-1.4.1.js" type="text/javascript"></script>
    <script type="text/javascript">
        function CloseForm() {
            window.opener.document.getElementById('btnrefreshprocess').click();
            self.close();
        }
    </script>
    <link href="../../App_Themes/Default/Style.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager ID="scriptmanager1" runat="server">
    </asp:ScriptManager>
    <table width="100%" border="1" cellspacing="1" cellpadding="1" align="center">
        <asp:Panel runat="server" ID="hs">
            <tr>
                <td width="75%" height="45">
                    <h1 align="center" class="style2">
                        Header</h1>
                </td>
                <td width="25%" valign="bottom">
                    <span class="style4">Welcome User--------------Logout </span>
                </td>
            </tr>
            <tr bgcolor="#999999">
                <td width="75%">
                    <uc2:ucmenu ID="ucmenu1" runat="server" />
                </td>
                <td width="25%">
                    <span class="style5">Home</span>
                </td>
            </tr>
        </asp:Panel>
    </table>
    <div style="margin-left: 15%; margin-right: 15%">
        <table>
            <tr>
            </tr>
        </table>
        <asp:UpdatePanel ID="updatepanel1" runat="server">
            <ContentTemplate>
                <table>
                    <tr>
                        <td class="tdstyle">
                            Process Name
                        </td>
                        <td>
                            <asp:TextBox ID="TxtProcessName" runat="server" CssClass="textb">
                            </asp:TextBox>
                        </td>
                        <td class="tdstyle">
                            Short Name
                        </td>
                        <td>
                            <asp:TextBox ID="TxtShortName" runat="server" CssClass="textb">
                            </asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="style2" colspan="3">
                            <asp:Label ID="LblErrer" runat="server" ForeColor="Red" Text="" align="left" Visible="false"></asp:Label>
                            <div id="divgride" runat="server" style="height: 150px; overflow: scroll; border-width: medium;
                                border-color: Black;">
                                <asp:GridView ID="DGCreateProcess" runat="server" AutoGenerateColumns="False" DataKeyNames="ID"
                                    ForeColor="#333333" GridLines="None" OnRowDataBound="DGProcess_RowDataBound"
                                    OnSelectedIndexChanged="DGCreateProcess_SelectedIndexChanged" Width="280px" CssClass="grid-view"
                                    OnRowCreated="DGCreateProcess_RowCreated">
                                    <Columns>
                                        <asp:BoundField DataField="ID" HeaderText="Sr.No" />
                                        <asp:BoundField DataField="PROCESS_NAME" HeaderText="PROCESS_NAME" />
                                        <asp:BoundField DataField="SHORTNAME" HeaderText="SHORTNAME" />
                                    </Columns>
                                    <HeaderStyle CssClass="gvheader" />
                                    <AlternatingRowStyle CssClass="gvalt" />
                                    <RowStyle CssClass="gvrow" />
                                    <EmptyDataRowStyle CssClass="gvemptytext" />
                                </asp:GridView>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td align="right" colspan="3">
                            <asp:Button ID="btnsave" runat="server" CssClass="buttonnorm" OnClick="btnsave_Click"
                                OnClientClick="return confirm('Do you want to save data?')" Text="Save" />
                            <asp:Button ID="btnclose" runat="server" CssClass="buttonnorm" OnClientClick="return CloseForm();"
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
